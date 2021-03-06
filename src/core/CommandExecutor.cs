using System;
using System.Threading.Tasks;

namespace Mesi.Notify.Core
{
    public interface ICommandResponseSender
    {
        Task SendCommandResponse(CommandResponse commandResponse, IRecipient recipient);
    }

    public class DefaultCommandResponseSender : ICommandResponseSender
    {
        private readonly IEmailCommandResponseSender _emailCommandResponseSender;

        public DefaultCommandResponseSender(IEmailCommandResponseSender emailCommandResponseSender)
        {
            _emailCommandResponseSender = emailCommandResponseSender;
        }
        
        /// <inheritdoc />
        public async Task SendCommandResponse(CommandResponse commandResponse, IRecipient recipient)
        {
            switch (recipient)
            {
                case EmailRecipient emailRecipient:
                    await _emailCommandResponseSender.SendCommandResponse(commandResponse, emailRecipient);
                    break;
                default:
                    throw new ArgumentException($"Unable to send command response to recipient of type '{recipient.GetType().Name}'");
            };
        }
    }

    public interface IEmailCommandResponseSender : ICommandResponseSender
    {
        Task SendCommandResponse(CommandResponse commandResponse, EmailRecipient recipient);
    }
}