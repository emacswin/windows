using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMultiThreadedWebDownloader
{
    public class DownloadCompletedEventArgs : EventArgs
    {
        public Int64 DownloadedSize { get; private set; }
        public Int64 TotalSize { get; private set; }
        public Exception Error { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public FileInfo DownloadedFile { get; private set; }

        public DownloadCompletedEventArgs(
            FileInfo downloadedFile, Int64 downloadedSize,
            Int64 totalSize, TimeSpan totalTime, Exception ex)
        {
            this.DownloadedFile = downloadedFile;
            this.DownloadedSize = downloadedSize;
            this.TotalSize = totalSize;
            this.TotalTime = totalTime;
            this.Error = ex;
        }
    }
}
