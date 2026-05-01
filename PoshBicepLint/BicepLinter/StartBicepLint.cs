namespace codingfreaks.PoshBicepLint
{
    using System.Management.Automation;

    using Helpers;

    using Models;

    [Cmdlet(VerbsLifecycle.Start, "BicepLint")]
    [OutputType(typeof(LinterResult[]))]
    public class StartBicepLint : PSCmdlet
    {
        #region methods

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var worker = new BicepLinter(WriteProgress);
            var results = worker.Start(Path);
            if (results == null)
            {
                // the process was cancelled probably
                base.ProcessRecord();
                return;
            }
            // we went through normally
            WriteObject(results);
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