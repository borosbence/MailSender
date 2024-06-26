﻿using System.Net.Mail;
using System.Net;

namespace MailSender.Services
{
    public class EmailService
    {
        private readonly SmtpClient _client;

        public EmailService(string userName, string password, string host, int port)
        {
            _client = new SmtpClient
            {
                Host = host,
                Port = port,
                // _client.Timeout = 10000;
                // _client.UseDefaultCredentials = false;
                // _client.DeliveryMethod = SmtpDeliveryMethod.Network;
                EnableSsl = true,
                Credentials = new NetworkCredential(userName, password)
            };
        }

        public async Task SendEmailAsync(string from, string to, string? subject, string? body,
            string? fromName = null, string? toName = null)
        {
            MailAddress _from = new(from, fromName);
            MailAddress _to = new(to, toName);
            MailMessage mail = new()
            {
                From = _from,
                To = { _to },
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            await _client.SendMailAsync(mail);
        }
    }
}
