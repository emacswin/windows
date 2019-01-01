using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSMultiThreadedWebDownloader
{
    public class MultiThreadedWebDownloader
    {
        // Used while calculating download speed.
        static object locker = new object();

        /// <summary>
        /// The Url of the file to be downloaded. 
        /// </summary>
        public Uri Url { get; private set; }

        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Specify whether the remote server supports "Accept-Ranges" header.
        /// Use the CheckUrl method to initialize this property internally.
        /// </summary>
        public bool IsRangeSupported { get; set; }

        /// <summary>
        /// The total size of the file. Use the CheckUrl method to initialize this 
        /// property internally.
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// This property is a member of IDownloader interface.
        /// </summary>
        public long StartPoint { get; set; }

        /// <summary>
        /// This property is a member of IDownloader interface.
        /// </summary>
        public long EndPoint { get; set; }

        /// <summary>
        /// The local path to store the file.
        /// If there is no file with the same name, a new file will be created.
        /// </summary>
        public string DownloadPath { get; set; }

        /// <summary>
        /// The Proxy of all the download client.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        // The time and size in last DownloadProgressChanged event. These two fields
        // are used to calculate the download speed.
        private DateTime lastNotificationTime;

        private long lastNotificationDownloadedSize;

        /// <summary>
        /// If get a number of buffers, then fire DownloadProgressChanged event.
        /// </summary>
        public int BufferCountPerNotification { get; set; }

        /// <summary>
        /// Set the BufferSize when read data in Response Stream.
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// The cache size in memory.
        /// </summary>
        public int MaxCacheSize { get; set; }

        DownloadStatus status;

        public DownloadStatus Status
        {
            get
            { return status; }

            private set
            {
                if (status != value)
                {
                    status = value;
                    this.OnStatusChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The max threads count. The real threads count number is the min value of this
        /// value and TotalSize / MaxCacheSize.
        /// </summary>
        public int MaxThreadCount { get; set; }

        public bool HasChecked { get; set; }

        // The HttpDownloadClients to download the file. Each client uses one thread to
        // download part of the file.
        List<HttpDownloadClient> downloadClients = null;

        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        public event EventHandler StatusChanged;

        /// <summary>
        /// The downloaded size of the file.
        /// </summary>
        public long DownloadedSize
        {
            get
            {
                return this.downloadClients.Sum(client => client.DownloadedSize);
            }
        }

        public int CachedSize
        {
            get
            {
                return this.downloadClients.Sum(client => client.CachedSize);
            }
        }

        // Store the used time spent in downloading data. The value does not include
        // the paused time and it will only be updated when the download is paused, 
        // canceled or completed.
        private TimeSpan usedTime = new TimeSpan();

        private DateTime lastStartTime;

        /// <summary>
        /// If the status is Downloading, then the total time is usedTime. Else the 
        /// total should include the time used in current download thread.
        /// </summary>
        public TimeSpan TotalUsedTime
        {
            get
            {
                if (this.Status != DownloadStatus.Downloading)
                {
                    return usedTime;
                }
                else
                {
                    return usedTime.Add(DateTime.Now - lastStartTime);
                }
            }
        }

        public MultiThreadedWebDownloader(string url)
            : this(url, 1024, 1048576, 64, Environment.ProcessorCount * 2)
        {
        }

        public MultiThreadedWebDownloader(string url, int bufferSize, int cacheSize,
            int bufferCountPerNotification, int maxThreadCount)
        {

            this.Url = new Uri(url);
            this.StartPoint = 0;
            this.EndPoint = long.MaxValue;
            this.BufferSize = bufferSize;
            this.MaxCacheSize = cacheSize;
            this.BufferCountPerNotification = bufferCountPerNotification;

            this.MaxThreadCount = maxThreadCount;

            // Set the maximum number of concurrent connections allowed by 
            // a ServicePoint object
            ServicePointManager.DefaultConnectionLimit = maxThreadCount;

            // Initialize the HttpDownloadClient list.
            downloadClients = new List<HttpDownloadClient>();

            // Set the Initialized status.
            this.Status = DownloadStatus.Initialized;
        }

        protected virtual void OnStatusChanged(EventArgs e)
        {

            switch (this.Status)
            {
                case DownloadStatus.Waiting:
                case DownloadStatus.Downloading:
                case DownloadStatus.Paused:
                case DownloadStatus.Canceled:
                case DownloadStatus.Completed:
                    if (this.StatusChanged != null)
                    {
                        this.StatusChanged(this, e);
                    }
                    break;
                default:
                    break;
            }

            if (this.Status == DownloadStatus.Paused
                || this.Status == DownloadStatus.Canceled
                || this.Status == DownloadStatus.Completed)
            {
                this.usedTime += DateTime.Now - lastStartTime;
            }

            if (this.Status == DownloadStatus.Canceled)
            {
                Exception ex = new Exception("Downloading is canceled by user's request. ");
                this.OnDownloadCompleted(
                    new DownloadCompletedEventArgs(
                        null,
                        this.DownloadedSize,
                        this.TotalSize,
                        this.TotalUsedTime,
                        ex));
            }

            if (this.Status == DownloadStatus.Completed)
            {
                this.OnDownloadCompleted(
                    new DownloadCompletedEventArgs(
                        new FileInfo(this.DownloadPath),
                        this.DownloadedSize,
                        this.TotalSize,
                        this.TotalUsedTime,
                        null));
            }
        }

        protected virtual void OnDownloadCompleted(
            DownloadCompletedEventArgs e)
        {
            if (DownloadCompleted != null)
            {
                DownloadCompleted(this, e);
            }
        }
    }
}
