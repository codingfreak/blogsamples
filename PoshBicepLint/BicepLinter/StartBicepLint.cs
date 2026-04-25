namespace codingfreaks.PoshBicepLint
{
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Start, "BicepLint")]
    [OutputType(typeof(string))]
    public class StartBicepLint : PSCmdlet
    {
        #region methods

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public string Message { get; set; } = null!;

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            WriteObject($"Hello: {Message}");
            base.ProcessRecord();
        }

        #endregion
    }
}