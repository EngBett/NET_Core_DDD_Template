namespace Template.Common.Models.Notifications
{
    public class SendEmailRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
    }
}
