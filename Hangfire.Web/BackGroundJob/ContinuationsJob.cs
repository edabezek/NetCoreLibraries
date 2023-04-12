using System.Diagnostics;

namespace Hangfire.Web.BackGroundJob
{
    public class ContinuationsJob
    {
        public static void WriteWaterMarkStatusJob(string Id,string fileNAme)
        {
            Hangfire.BackgroundJob.ContinueJobWith(Id, () => Debug.WriteLine($"{fileNAme} : resime watermark eklenmiştir."));
        }
    }
}
