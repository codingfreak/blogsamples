namespace codingfreaks.PoshBicepLint.Helpers
{
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Provides logic for easy handling of processes.
    /// </summary>
    internal class ProcessHelper
    {
        #region methods

        /// <summary>
        /// Creates a new process with the given <paramref name="command" /> and <paramref name="args" /> and returns the raw but
        /// cleared output the process created.
        /// </summary>
        /// <param name="command">The command to execute in the process (usually the path to an executable).</param>
        /// <param name="args">Optional command-line-arguments for the process.</param>
        /// <returns></returns>
        public static string GetProcessOutput(string command, string[] args)
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

        #endregion
    }
}