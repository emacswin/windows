using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSMultiThreadedWebDownloader
{
    public class HttpDownloadClient
    {
        // Used when creates or writes a file.
        static object fileLocker = new object();

        object statusLocker = new object();


        // The Url of the file to be downloaded.
        public Uri Url { get; private set; }

        // The local path to store the file.
        // If there is no file with the same name, a new file will be created.
        public string DownloadPath { get; set; }

        // Ask the server for the file size and store it.
        // Use the CheckUrl method to initialize this property internally.
        public long TotalSize { get; set; }

        public ICredentials Credentials { get; set; }

        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Specify whether the remote server supports "Accept-Ranges" header.
        /// Use the CheckUrl method to initialize this property internally.
        /// </summary>
        public bool IsRangeSupported { get; set; }

        // The properties StartPoint and EndPoint can be used in the multi-thread download scenario, and
        // every thread starts to download a specific block of the whole file. 
        public long StartPoint { get; set; }

        public long EndPoint { get; set; }


        // The size of downloaded data that has been writen to local file.
        public long DownloadedSize { get; private set; }

        public int CachedSize { get; private set; }

        public bool HasChecked { get; set; }


        DownloadStatus status;
    }
}
