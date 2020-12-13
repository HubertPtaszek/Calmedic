namespace Calmedic.Application
{
    public interface IAppMailMessageService : IService
    {
        bool AddCreateConfirmationMessage(int id, string url);
        void SendMessageJob();
        bool AddForgotPasswordMessage(string url, string userId);
    }
}
