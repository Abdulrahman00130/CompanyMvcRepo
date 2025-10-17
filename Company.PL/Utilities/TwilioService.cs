using Company.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Twilio.Types;

namespace Company.PL.Utilities
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {
        public void SendSMS(SMS sms)
        {
            // Initialize connection
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

            // Build Message
            var message = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(_options.Value.PhoneNumber),
                to: sms.To,
                body: sms.Body
                );

            //return message
            //return message;
        }
    }
}
