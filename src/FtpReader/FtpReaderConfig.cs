using Newtonsoft.Json;
using System;
using System.IO;
////////////////////////////////////////////////////////////////////////////
//	Copyright 2009-2018 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
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
