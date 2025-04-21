namespace e_commerce.Web.Models
{
    public class EmailSettings
    {
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}

