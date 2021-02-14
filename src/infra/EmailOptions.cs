namespace Mesi.Notify.Infra
{
    public class EmailOptions
    {
        public string SmtpHost { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string Subject { get; set; } = null!;
    }
}