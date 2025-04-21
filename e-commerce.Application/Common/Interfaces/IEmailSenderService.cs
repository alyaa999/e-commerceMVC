using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
