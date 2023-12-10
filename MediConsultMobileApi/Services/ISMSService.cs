using Twilio.Rest.Api.V2010.Account;

namespace MediConsultMobileApi.Services
{
    public interface ISMSService
    {
        MessageResource SendMessage(string mobileNumber, string body);
    }
}
