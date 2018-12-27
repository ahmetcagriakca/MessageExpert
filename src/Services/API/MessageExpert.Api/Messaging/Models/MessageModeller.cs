using MessageExpert.Data.Models.Accounts;

namespace MessageExpert.Api.Security.Accounts.Models
{
    public class SendMessageRequest
	{
        public string Content { get; set; }
        public string To { get; set; }
    }
}
