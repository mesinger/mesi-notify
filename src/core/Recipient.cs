namespace Mesi.Notify.Core
{
    public interface IRecipient
    {
        
    }

    public record EmailRecipient(string Email) : IRecipient;
}