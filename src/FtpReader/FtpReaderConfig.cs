using Newtonsoft.Json;
using System;
using System.IO;

namespace FtpReader.Client
{
    public class FtpReaderConfig
    {
        private static FtpReaderConfigData mFtpConfig = null;

        public static FtpReaderConfigData GetConfigData
        {
            get
            {
                if (mFtpConfig == null)
                {
                    GetConfiguration();
                }
                return mFtpConfig;
            }
        }

        private static void GetConfiguration()
        {
            String configFile = "ftp_config.json";

            String pathToTheFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + configFile;

            using (StreamReader file = File.OpenText(pathToTheFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                mFtpConfig = (FtpReaderConfigData)serializer.Deserialize(file, typeof(FtpReaderConfigData));
            }

        }

    }
}
