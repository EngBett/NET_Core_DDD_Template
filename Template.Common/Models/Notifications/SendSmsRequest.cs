namespace Template.Common.Models.Notifications
{
    public class SendSmsRequest
    {
        public string source { get; set; }
        public bool multi { get; set; }
        public string message { get; set; }
        public List<Destination> destination { get; set; }
        public string status_url { get; set; }
        public string status_secret { get; set; }
    }

    public class Destination
    {
        public string number { get; set; }
        public string message { get; set; }
    }
}
