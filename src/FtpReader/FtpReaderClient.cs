﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;

namespace FtpReader.Client
{
    public class FtpReaderClient
    {
        private String m_ftpUrl;
        private String m_userName;
        private String m_password;

        public FtpReaderClient(String ftpUrl, String userName, String password)
        {
            m_ftpUrl = ftpUrl;
            m_userName = userName;
            m_password = password;
        }
        public FtpReaderClient()
        {
            m_ftpUrl = FtpReaderConfig.GetConfigData.Url;
            m_userName = FtpReaderConfig.GetConfigData.UserName;
            m_password = FtpReaderConfig.GetConfigData.Password;
        }

        /// <summary>
        ///   Download file to folder with original name
        /// </summary>
        /// <param name="ftpFile"></param>
        /// <param name="locaFolder"></param>
        public String DownloadToFolder(string ftpFile, string locaFolder)
        {
            String[] s = ftpFile.Split('/');
            String localFileName = locaFolder + "/" +  s[s.Count() - 1];
            Download(ftpFile, localFileName);
            return localFileName;
        }

            /// <summary>
            ///    Dounload file from FTP to local Storage
            /// </summary>
            /// <param name="ftpFile">full url with ftp:// </param>
            /// <param name="localFile"></param>
            public void Download(string ftpFile, string localFile)
        {

            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFile);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.ServicePoint.ConnectionLeaseTimeout = 900000;
                request.ServicePoint.MaxIdleTime = 900000;
                request.Timeout = 900000;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(m_userName,
                                                            m_password);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        long cl = response.ContentLength;
                        int bufferSize = 51200;
                        int readCount;
                        byte[] byteBuffer = new byte[bufferSize];

                        using (FileStream localFileStream = new FileStream(localFile, FileMode.Create))
                        {

                            readCount = ftpStream.Read(byteBuffer, 0, bufferSize);

                            while (readCount > 0)
                            {
                                localFileStream.Write(byteBuffer, 0, readCount);
                                readCount = ftpStream.Read(byteBuffer, 0, bufferSize);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public List<String> ListFiles()
        {
            string names = "";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(m_ftpUrl);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(m_userName, m_password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            names = reader.ReadToEnd();
                        }
                    }
                }

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
