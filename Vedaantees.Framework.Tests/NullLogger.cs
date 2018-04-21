using System;
using Vedaantees.Framework.Providers;
using Vedaantees.Framework.Providers.Logging;

namespace Vedaantees.Framework.Tests
{
    public class NullLogger : ILogger
    {
        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            
        }
    }
}