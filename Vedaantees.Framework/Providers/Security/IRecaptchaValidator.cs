using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Security
{
    public interface IRecaptchaValidator
    {
        MethodResult Verify(IRecaptcha objectToValidate);
    }
}