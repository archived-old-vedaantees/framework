using System;
using Vedaantees.Framework.Providers.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class Logging
    {
        private ILogger _logger;

        [TestInitialize]
        public void Initialize()
        {
            _logger = new Logger(new LoggerConfiguration
                                 {
                                    EnableSerilogDebugger = true,
                                    EnableFile = true,
                                    EnableEmail = true
                                 }, 
                                 "-+> Test-Framework <+-");
        }

        [TestMethod]
        public void TestIfLogsAreCreated()
        {
            _logger.Debug("Debug Logger works.");
            _logger.Information("Info Logger works.");
            _logger.Verbose("Verbose Logger works.");
            _logger.Warning("Warning Logger works.");
            _logger.Error("Error Logger works.");
            _logger.Fatal("Fatal Logger works.");

            _logger.Debug(new Exception ("Test Exception"),"Debug Logger works");
            _logger.Information(new Exception("Test Exception"), "Info Logger works.");
            _logger.Verbose(new Exception ("Test Exception"), "Verbose Logger works.");
            _logger.Warning(new Exception ("Test Exception"), "Warning Logger works.");
            _logger.Error(new Exception ("Test Exception"), "Error Logger works.");
            _logger.Fatal(new Exception ("Test Exception"), "Fatal Logger works.");
        }
    }
}