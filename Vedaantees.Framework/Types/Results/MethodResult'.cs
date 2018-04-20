#region  usings 

using System;
using System.Runtime.InteropServices;

#endregion

namespace Vedaantees.Framework.Types.Results
{
    /// <summary>
    ///     Defines the return types with some result.
    /// </summary>
    public class MethodResult<T> : MethodResult
    {
        /// <summary>
        ///     Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public T Result { get; set; }

        #region [Constructors]

        public MethodResult()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="methodResult">The action result.</param>
        /// <param name="result">The result.</param>
        /// <param name="message">The message.</param>
        public MethodResult(MethodResult methodResult, T result, string message) : base(methodResult.MethodResultState,
            message)
        {
            if (result != null)
            {
                Result = result;
                MethodResultState = MethodResultStates.Successful;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="methodResultState">State of the action.</param>
        /// <param name="message">The message.</param>
        public MethodResult(MethodResultStates methodResultState, [Optional] string message)
            : base(methodResultState, message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public MethodResult(Exception exception, [Optional] string message) : base(exception, message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="result">The result.</param>
        public MethodResult(T result)
        {
            Result = result;
            MethodResultState = MethodResultStates.Successful;
        }

        public MethodResult(T result, MethodResult methodResult)
        {
            if (methodResult.MethodResultState == MethodResultStates.Successful)
                Result = result;

            MethodResultState = methodResult.MethodResultState;
            Exception = methodResult.Exception;
            Message = methodResult.Message;
        }

        /// <summary>
        ///     Gets the action result.
        /// </summary>
        public MethodResult ToMethodResult => MethodResultState == MethodResultStates.Exception
            ? new MethodResult(Exception, Message)
            : new MethodResult(MethodResultState, Message);

        #endregion
    }
}