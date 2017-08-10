namespace BlueWarriors.Services
{
    public interface ISmsMessage
    {
        void Send(string message, string recipient);
    }
}