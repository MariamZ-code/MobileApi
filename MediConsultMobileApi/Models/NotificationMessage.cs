namespace MediConsultMobileApi.Models
{
    public class NotificationMessage
    {
        public IReadOnlyList<string> membersIds { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }


    }
    public class NotificationMessageToAll
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }
}
