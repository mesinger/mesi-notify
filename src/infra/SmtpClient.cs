using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mesi.Notify.Infra
{
    public interface ISmtpClient
    {
        Task SendMail(string from, string fromName, string to, string subject, string message);
    }

    public class DefaultSmtpClient : ISmtpClient
    {
        private readonly EmailOptions _options;
        private readonly ILogger<DefaultSmtpClient> _logger;

        public DefaultSmtpClient(IOptions<EmailOptions> options, ILogger<DefaultSmtpClient> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task SendMail(string fromEmail, string fromName, string toEmail, string subject, string messageContent)
        {
            SmtpClient? smtpClient = null;
            MailMessage? message = null;
            
            try
            {
                smtpClient = new SmtpClient(_options.SmtpHost, _options.SmtpPort);

                var from = new MailAddress(fromEmail, fromName);
                var to = new MailAddress(toEmail);

                message = new MailMessage(from, to)
                {
                    Subject = _options.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = messageContent,
                    BodyEncoding = Encoding.UTF8,
                };

                smtpClient.SendCompleted += EmailSentCallback;

                var clientIdentifier = Guid.NewGuid().ToString();
                _notDisposedClientsAndMessages.Add(clientIdentifier, (smtpClient, message));
                smtpClient.SendAsync(message, clientIdentifier);

                _logger.LogInformation("Sending email to {}", toEmail);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                smtpClient?.Dispose();
                message?.Dispose();
                
                _logger.LogWarning("Unable to send email because of an exception '{}'", ex.Message);
                return Task.CompletedTask;
            }
        }
        
        private static readonly Dictionary<string, (SmtpClient, MailMessage)> _notDisposedClientsAndMessages = new();

        private static void EmailSentCallback(object sender, AsyncCompletedEventArgs e)
        {
            var clientIdentifier = e.UserState as string ?? string.Empty;
            
            if (_notDisposedClientsAndMessages.ContainsKey(clientIdentifier))
            {
                _notDisposedClientsAndMessages[clientIdentifier].Item1.Dispose();
                _notDisposedClientsAndMessages[clientIdentifier].Item2.Dispose();
            }
        }
    }
}