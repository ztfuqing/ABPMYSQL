using System;
using System.IO;

namespace Fq
{
    public class AppVersionHelper
    {
        public const string Version = "1.0.0.0";

        public static DateTime ReleaseDate
        {
            get { return new FileInfo(typeof(AppVersionHelper).Assembly.Location).LastWriteTime; }
        }
    }
}
