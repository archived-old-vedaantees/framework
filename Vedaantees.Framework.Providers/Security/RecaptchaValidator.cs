using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Security
{
    public class RecaptchaValidator : IRecaptchaValidator
    {
        private readonly ILogger _logger;

        public RecaptchaValidator(ILogger logger)
        {
            _logger = logger;
        }

        public MethodResult Verify(IRecaptcha objectToValidate)
        {

            try
            {
                string response;
                dynamic captchaResponse;

                using (var client = new WebClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection
                    {
                        {"secret", "6LcKjeESAAAAAKMMi9kxVMThRkiGlU31rjPDlEM4"},
                        {"response", objectToValidate.CaptchaResponse}
                    };
                    var responsebytes = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", "POST", reqparm);
                    response = Encoding.UTF8.GetString(responsebytes);
                    captchaResponse = JsonConvert.DeserializeObject(response);
                }

                if (captchaResponse.success == "true")
                {
                    return new MethodResult(MethodResultStates.Successful);
                }
                else
                {
                    _logger.Warning("Recaptcha response was not as expected: " + response);
                    return new MethodResult(MethodResultStates.UnSuccessful, "Invalid captcha response, try again.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error occured in Recaptcha", exception);
                return new MethodResult(MethodResultStates.UnSuccessful, "Exception occured while validating recaptcha. Cannot continue.");
            }
        }

        public string CaptchaResponse { get; set; }
    }
}
