using System.Diagnostics;

namespace Hangfire.Web.BackGroundJob
{
    public class RecurringJob
    {
        public static void ReportingJob()
        {
            Hangfire.RecurringJob.AddOrUpdate("reportjob1", () => EmailReport(), Cron.Minutely);
        }
        public static void EmailReport()
        {
            Debug.WriteLine("Rapor email olarak gönderildi");
        }
    }
}
