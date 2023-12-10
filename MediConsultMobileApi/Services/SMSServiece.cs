using MediConsultMobileApi.Helper;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MediConsultMobileApi.Services
{
    public class SMSService : ISMSService
    {
        private readonly TwilioSettings twilio;

        public SMSService(IOptions<TwilioSettings> twilio)
        {
            this.twilio = twilio.Value;
        }
        public MessageResource SendMessage(string mobileNumber, string body)
        {
            TwilioClient.Init(twilio.AccountSID, twilio.AuthToken);

            var result = MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(twilio.TwilioPhoneNumber),
                    to: mobileNumber
                );

            return result;
        }
    }
}
