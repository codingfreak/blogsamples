namespace codingfreaks.PoshBicepLint.Helpers
{
    using System.Management.Automation;

    using Models;

    /// <summary>
    /// Contains the logic for linting Bicep files.
    /// </summary>
    internal class BicepLinter
    {
        #region member vars

        private readonly ProgressRecord _progress = new(1, "Linting", "Starting...");

        private readonly CancellationTokenSource _tokenSource = new();

        private int _filesCompleted;

        private int _filesTotal;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="progressCallback">
        /// A callback which takes a progress object and is expected to write it to the output
        /// stream.
        /// </param>
        public BicepLinter(Action<ProgressRecord> progressCallback)
        {
            ProgressCallback = progressCallback;
        }

        #endregion

        #region methods

        /// <summary>
        /// Starts the linting of all Bicep files in a given <paramref name="path" />.
        /// </summary>
        /// <param name="path">The path of a directory.</param>
        /// <returns>The collection of linting results (1 for each Bicep file found).</returns>
        /// <exception cref="DirectoryNotFoundException">Is thrown if the <paramref name="path" /> was not found.</exception>
        public LinterResult[]? Start(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory {path} not found.");
            }
            var token = _tokenSource.Token;
            var files = DirectoryHelper.GetFilesRecursive(path, "*.bicep");
            _filesTotal = files.Length;
            _filesCompleted = 0;
            _progress.PercentComplete = 0;
            var tasks = new List<Task<LinterResult>>();
            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => LintBicep(file), token));
            }
            StartWatcher(token);
            try
            {
                var results = Task.WhenAll(tasks)
                    .GetAwaiter()
                    .GetResult();
                return results;
            }
            catch (TaskCanceledException cancelEx)
            {
                return null;
            }
        }

        /// <summary>
        /// Is called internally to increase the overall progress by 1 and cancel the work loop if the progress is at 100%.
        /// </summary>
        private void IncreaseProgress()
        {
            Interlocked.Increment(ref _filesCompleted);
            _progress.PercentComplete = _filesCompleted * 100 / _filesTotal;
            _progress.StatusDescription = $"{_filesCompleted}/{_filesTotal}";
            if (_progress.PercentComplete >= 100)
            {
                _tokenSource.Cancel();
            }
        }

        /// <summary>
        /// Is called to lint a single Bicep file at the given <paramref name="path" />.
        /// </summary>
        /// <param name="path">The URI of the Bicep file.</param>
        /// <returns>The linter result for this file.</returns>
        private LinterResult LintBicep(string path)
        {
            try
            {
                var output = ProcessHelper.GetProcessOutput("bicep", ["lint", path]);
                var result = output.Split(Environment.NewLine);
                // remove empty lines
                result = result.Where(r => !string.IsNullOrEmpty(r))
                    .ToArray();
                return new LinterResult(path, result);
            }
            finally
            {
                IncreaseProgress();
            }
        }

        /// <summary>
        /// Starts a loop which writes executes the <see cref="ProgressCallback" /> continuesly.
        /// </summary>
        /// <param name="token">The token to cancel this loop.</param>
        private void StartWatcher(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    ProgressCallback(_progress);
                }
                catch (PipelineStoppedException stoppedEx)
                {
                    // The process was cancelled
                    _tokenSource.Cancel();
                }
                Thread.Sleep(10);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// A callback which takes a progress object and is expected to write it to the output
        /// stream.
        /// </summary>
        public Action<ProgressRecord> ProgressCallback { get; }

        #endregion
    }
}