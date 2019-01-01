using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSMultiThreadedWebDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime lastNotificationTime;

            WebProxy proxy = null;

            string path = Path.Combine(Path.GetTempPath(), "DotNetFx4.exe");
            string url = "http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (File.Exists(path + ".tmp"))
            {
                File.Delete(path + ".tmp");
            }

            MultiThreadedWebDownloader downloader = null;
            downloader = new MultiThreadedWebDownloader(url);

            downloader.Proxy = proxy;
        }
    }
}
