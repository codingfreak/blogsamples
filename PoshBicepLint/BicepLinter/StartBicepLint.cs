namespace codingfreaks.PoshBicepLint
{
    using System.Diagnostics;
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Start, "BicepLint")]
    [OutputType(typeof(LinterResult[]))]
    public class StartBicepLint : PSCmdlet
    {
        #region methods

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var watch = new Stopwatch();
            watch.Start();
            var worker = new BicepLinter(WriteProgress);
            var loopTask = Task.Run(() => worker.RunMainThreadLoop(CancellationToken.None));
            var resultsTask = Task.Run(() => worker.Start(Path));
            Task.WhenAll(resultsTask, loopTask)
                .GetAwaiter()
                .GetResult();
            var results = resultsTask.GetAwaiter()
                .GetResult();
            watch.Stop();
            WriteObject(results);
            //WriteInformation(new InformationRecord($"Handled {results.Length} files in {watch.Elapsed}.", ""));
            if (!DontFailOnError.IsPresent && results.Any(r => !r.Succeeded))
            {
                WriteError(
                    new ErrorRecord(
                        new Exception("Bicep linting found errors."),
                        "BicepLintFailed",
                        ErrorCategory.InvalidData,
                        null));
            }
            base.ProcessRecord();
        }

        #endregion

        #region properties

        /// <summary>
        /// Set this flag if you want this cmdlet to return success in any case.
        /// </summary>
        [Parameter]
        public SwitchParameter DontFailOnError { get; set; }

        /// <summary>
        /// The directory in which to search for Bicep files.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public string Path { get; set; } = null!;

        #endregion
    }
}