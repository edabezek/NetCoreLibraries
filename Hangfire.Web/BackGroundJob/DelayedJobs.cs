using System.Drawing;
using System.IO;

namespace Hangfire.Web.BackGroundJob
{
    public class DelayedJobs
    {
        public static string AddWaterMarkJob(string filename,string watermarkText)
        {
           return Hangfire.BackgroundJob.Schedule(() => AddWaterMarkJob(filename, watermarkText),System.TimeSpan.FromSeconds(20));
        }
        public static void ApplyWatermark(string filename,string watermarkText)
        {
            //üç tane yolu birleştiriyoruz, yüklenen fotoyu almak için
            string path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Pictures" ,filename);

            using (var bitmap=Bitmap.FromFile(path))
            {
                using (Bitmap tempBitmap=new Bitmap(bitmap.Width,bitmap.Height))
                {
                    using (Graphics grp= Graphics.FromImage(tempBitmap))
                    {
                        grp.DrawImage(bitmap, 0, 0);

                        var font=new Font(FontFamily.GenericSansSerif,25,FontStyle.Bold);   

                        var color= Color.FromArgb(255,255,255);    

                        var brush= new SolidBrush(color);

                        var point=new Point(20,bitmap.Height-50);

                        grp.DrawString(watermarkText,font,brush,point);

                        tempBitmap.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Pictures/watermarks", filename));
                    }
                }
            }
        }
    }
}
