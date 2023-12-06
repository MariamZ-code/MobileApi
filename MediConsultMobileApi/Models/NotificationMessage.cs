namespace MediConsultMobileApi.Models
{
    public class NotificationMessage
    {
        public List<string>? membersIds { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }


    }
    public class NotificationMessageToAll
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
    }
}
