using Hangfire.Web.Services;

namespace Hangfire.Web.BackGroundJob
{
    public class FireAndForgetJob
    {
        public static void EmailSendJobToUser(string userId,string message)
        {
            Hangfire.BackgroundJob.Enqueue<IEmailSender>(x=>x.Sender(userId,message));
        }
    }
}
