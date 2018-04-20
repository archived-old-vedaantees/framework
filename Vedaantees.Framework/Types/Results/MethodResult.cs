#region  usings 

using System;
using System.Runtime.InteropServices;

#endregion

namespace Vedaantees.Framework.Types.Results
{
    /// <summary>
    ///     Defines the return types with void result.
    /// </summary>
    public class MethodResult
    {
        public MethodResultStates MethodResultState { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public void UpdateMessageIfSuccessful(string message)
        {
            if (MethodResultState == MethodResultStates.Successful) Message = message;
        }

        #region [Constructors]

        public MethodResult()
        {
        }

        public MethodResult(MethodResultStates methodResultState, [Optional] string message)
        {
            MethodResultState = methodResultState;
            Message = message;
        }

        public MethodResult(Exception exception, [Optional] string message)
        {
            MethodResultState = MethodResultStates.Exception;
            Exception = exception;
            Message = message;
        }

        #endregion
    }
}