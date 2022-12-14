namespace Template.Common.Models.Notifications
{
    public class SendSmsResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Recipient> recipients { get; set; }
    }

    public class Recipient
    {
        public string id { get; set; }
        public double cost { get; set; }
        public string number { get; set; }
        public string status { get; set; }
    }
}
