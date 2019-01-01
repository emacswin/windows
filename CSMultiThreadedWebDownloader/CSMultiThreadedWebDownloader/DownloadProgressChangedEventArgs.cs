using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMultiThreadedWebDownloader
{
    public class DownloadProgressChangedEventArgs : EventArgs
    {
        public Int64 ReceivedSize { get; private set; }
        public Int64 TotalSize { get; private set; }
        public int DownloadSpeed { get; private set; }

        public DownloadProgressChangedEventArgs(Int64 receivedSize,
            Int64 totalSize, int downloadSpeed)
        {
            this.ReceivedSize = receivedSize;
            this.TotalSize = totalSize;
            this.DownloadSpeed = downloadSpeed;
        }
    }
}
