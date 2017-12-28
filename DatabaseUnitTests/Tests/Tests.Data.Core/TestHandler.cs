namespace codingfreaks.DatabaseUnitTests.Tests.Data.Core
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using DatabaseUnitTests.Data.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestHandler
    {
        #region methods

        /// <summary>
        /// Is called when every test in this assembly is finished.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Trace.TraceInformation("Assembly cleanup is starting.");
            using (var ctx = new UnitTestSampleEntities())
            {
                try
                {
                    ctx.Database.Delete();
                    Trace.TraceInformation("Database deleted.");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Is called by the test runtime when the test environment loads.
        /// </summary>
        /// <param name="context">The test context.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Trace.TraceInformation("Assembly initializing is starting.");
            Context = context;
            var file = GetScriptFileName();
            Trace.TraceInformation($"Starting script {file}...");
            if (file == null)
            {
                throw new FileNotFoundException("Could not locate database deploy script.");
            }
            var procInfo = new ProcessStartInfo(file.FullName)
            {
                WorkingDirectory = file.Directory.FullName,
                WindowStyle = ProcessWindowStyle.Normal
            };
            var proc = Process.Start(procInfo);
            proc?.WaitForExit();
        }

        /// <summary>
        /// Searches for the database-project and retrieves its full project-filename if found.
        /// </summary>
        /// <returns>The filename or <c>null</c> if the file wasn't found.</returns>
        private static FileInfo GetScriptFileName()
        {
            try
            {
                var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                if (dir == null)
                {
                    return null;
                }
                while (dir.Parent != null && !dir.GetDirectories(".shared").Any())
                {
                    dir = dir.Parent;
                    if (dir == null)
                    {
                        return null;
                    }
                }
                dir = dir.GetDirectories(".shared").First();
                var fileInfo = dir.GetFiles(ConfigurationManager.AppSettings["DatabaseDeployScript"]).First();
                return fileInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The context for the current test run.
        /// </summary>
        protected static TestContext Context { get; set; }

        /// <summary>
        /// Retrieves the database name used for tests on the current machine.
        /// </summary>
        

        #endregion
    }
}