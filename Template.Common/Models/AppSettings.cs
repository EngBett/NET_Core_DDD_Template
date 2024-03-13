namespace Template.Common.Models
{
    public class AppSettings
    {
        public const string Name = "AppSettings";
        public string[] AllowedOrigins { get; set; }
        public string Authority { get; set; }
        public string[] DefaultApis { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string LogUrl { get; set; }
        public string ClientSecret { get; set; }
        public string MetadataAddress { get; set; }

        public string SensitiveDataKeys { get; set; }
        public string SensitiveDataDefaultValues { get; set; }
        public bool EnableAutoMigration { get; set; }
        public bool UseLoggerMiddleWare { get; set; }
        public string DefaultRedirectUrl { get; set; }
        public bool ShowSwagger { get; set; }
    }
}
