using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Mesi.Notify.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mesi.Notify.Infra
{
    public class EmailCommandResponseSender : IEmailCommandResponseSender
    {
        private readonly ISmtpClient _smtpClient;
        private readonly EmailOptions _options;
        private readonly ILogger<EmailCommandResponseSender> _logger;

        public EmailCommandResponseSender(ISmtpClient smtpClient, IOptions<EmailOptions> options, ILogger<EmailCommandResponseSender> logger)
        {
            _smtpClient = smtpClient;
            _options = options.Value;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public Task SendCommandResponse(CommandResponse commandResponse, IRecipient recipient)
        {
            if (recipient is EmailRecipient emailRecipient)
            {
                return SendCommandResponse(commandResponse, emailRecipient);
            }
            
            _logger.LogError("Trying to send a command response for a recipient of type '{}' with the email command response sender", nameof(recipient));
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task SendCommandResponse(CommandResponse commandResponse, EmailRecipient recipient)
        {
            await _smtpClient.SendMail(_options.FromEmail, _options.FromName, recipient.Email, _options.Subject,
                commandResponse.Data);
        }
    }
}