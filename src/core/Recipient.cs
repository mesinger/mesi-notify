namespace Mesi.Notify.Core
{
    public interface IRecipient
    {
        
    }

    public record EmailRecipient(string Email) : IRecipient
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"Email Recipient: {Email}";
        }
    }
}