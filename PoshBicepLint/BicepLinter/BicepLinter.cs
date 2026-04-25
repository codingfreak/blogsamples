namespace codingfreaks.PoshBicepLint
{
    using System.Diagnostics;
    using System.Management.Automation;
    using System.Text;
    using System.Threading.Channels;

    internal class BicepLinter
    {
        #region member vars

        private readonly Channel<Action> _mainThreadQueue = Channel.CreateUnbounded<Action>();

        private int _filesCompleted;

        private int _filesTotal;

        private ProgressRecord? _progress;

        public BicepLinter(Action<ProgressRecord> handleProgress)
        {
            HandleProgress = handleProgress;
        }

        #endregion

        #region methods

        // Call this on the main thread to process all queued actions
        public async Task RunMainThreadLoop(CancellationToken ct)
        {
            await foreach (var action in _mainThreadQueue.Reader.ReadAllAsync(ct))
            {
                action();
            }
        }

        public LinterResult[] Start(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory {path} not found.");
            }
            var files = GetBicepFiles(path);
            _filesTotal = files.Length;
            _filesCompleted = 0;
            _progress = new ProgressRecord(1, "Linting", "Running");
            var tasks = new List<Task<LinterResult>>();
            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => LintBicep(file)));
            }
            var results = Task.WhenAll(tasks)
                .GetAwaiter()
                .GetResult();
            StopMainThreadLoop();
            return results;
        }

        // Call Complete() when all work is done so the loop exits
        public void StopMainThreadLoop()
        {
            _mainThreadQueue.Writer.Complete();
        }

        private static string[] GetBicepFiles(string path)
        {
            var files = Directory.GetFiles(path, "*.bicep")
                .ToList();
            foreach (var dir in Directory.GetDirectories(path))
            {
                files.AddRange(GetBicepFiles(dir));
            }
            return files.ToArray();
        }

        private static string GetProcessOutput(string command, string[] args)
        {
            var info = new ProcessStartInfo(command, args)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            var outputStringBuilder = new StringBuilder();
            var process = new Process();
            process.StartInfo = info;
            process.OutputDataReceived += (_, eventArgs) =>
            {
                if (string.IsNullOrEmpty(eventArgs.Data))
                {
                    return;
                }
                outputStringBuilder.AppendLine(eventArgs.Data);
            };
            process.ErrorDataReceived += (_, eventArgs) =>
            {
                if (string.IsNullOrEmpty(eventArgs.Data))
                {
                    return;
                }
                outputStringBuilder.AppendLine(eventArgs.Data);
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return outputStringBuilder.ToString()
                .TrimEnd('\r')
                .Trim('\n');
        }

        private void IncreaseProgress()
        {
            _mainThreadQueue.Writer.TryWrite(() =>
            {
                _filesCompleted++;
                _progress?.StatusDescription = $"{_filesCompleted}/{_filesTotal}";
                HandleProgress(_progress!);
            });
        }

        public Action<ProgressRecord> HandleProgress { get; }

        private LinterResult LintBicep(string path)
        {
            var output = GetProcessOutput("bicep", ["lint", path]);
            var result = output.Split(Environment.NewLine);
            // remove empty lines
            result = result.Where(r => !string.IsNullOrEmpty(r))
                .ToArray();
            //IncreaseProgress();
            return new LinterResult(path, result);
        }

        #endregion
    }
}