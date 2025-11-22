using Microsoft.Extensions.Options;
using MimeKit;
using Lab2.Models;
using MailKit.Net.Smtp;

namespace Lab2.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string ToEmail, string Subject, string Message);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSetting;

    }
}
