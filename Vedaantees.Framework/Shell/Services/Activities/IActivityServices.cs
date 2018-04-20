#region  usings 
using Vedaantees.Framework.Types.Results;
#endregion



namespace Vedaantees.Framework.Shell.Services.Activities
{
    /// <summary>
    ///     Track all the activities done by user using user-friendly messages.
    /// </summary>
    public interface IActivityServices
    {
        MethodResult LogActivity(Activity activity);
        MethodResult LogActivity(int parentId, Activity activity);
    }
}