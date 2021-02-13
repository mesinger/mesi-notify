using System.Threading.Tasks;
using Mesi.Notify.Core;

namespace Mesi.Notify.Infra
{
    public class EmailCommandResponseSender : IEmailCommandResponseSender
    {
        /// <inheritdoc />
        public Task SendCommandResponse(CommandResponse commandResponse, IRecipient recipient)
        {
            if (recipient is EmailRecipient emailRecipient)
            {
                return SendCommandResponse(commandResponse, emailRecipient);
            }
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task SendCommandResponse(CommandResponse commandResponse, EmailRecipient recipient)
        {
            throw new System.NotImplementedException();
        }
    }
}