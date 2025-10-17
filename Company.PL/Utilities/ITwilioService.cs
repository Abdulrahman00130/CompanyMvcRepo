using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Utilities
{
    public interface ITwilioService
    {
        public void SendSMS(SMS sms);
    }
}
