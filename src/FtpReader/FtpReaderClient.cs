using System;
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
            public long Download(string ftpFile, string localFile)
        {

            long retCount = 0;

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
                int bufferSize = 51200;
                request.Credentials = new NetworkCredential(m_userName,
                                                            m_password);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        var contextLentgth = response.ContentLength;
                        ftpStream.ReadTimeout = 900000;
                        long countStream = 0;
                        try
                        {
                            countStream = response.ContentLength;
                        }
                        catch (Exception) { }
                                             
                        int readCount;
                        byte[] byteBuffer = new byte[bufferSize];

                        using (FileStream localFileStream = new FileStream(localFile, FileMode.Create))
                        {
                            if (countStream <= 0)
                            {
                                readCount = ftpStream.ReadAsync(byteBuffer, 0, bufferSize).Result;
                                retCount += readCount;
                                while (readCount > 0)
                                {
                                    localFileStream.Write(byteBuffer, 0, readCount);
                                    readCount = ftpStream.ReadAsync(byteBuffer, 0, bufferSize).Result;
                                    retCount += readCount;
                                }
                            } else
                            {
                                readCount = readDataFromStream(bufferSize, ftpStream, ref countStream, byteBuffer);
                                retCount += readCount;
                                while (readCount > 0)
                                {
                                    localFileStream.Write(byteBuffer, 0, readCount);
                                    readCount = readDataFromStream(bufferSize, ftpStream, ref countStream, byteBuffer);
                                    retCount += readCount;
                                    if (countStream == 0)
                                    {
                                        break;
                                    }
                                }
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
            return retCount;
        }

        private static int readDataFromStream(int bufferSize, Stream ftpStream, ref long countStream, byte[] byteBuffer)
        {
            int readCount;
            int realReadCount = bufferSize;
            if (countStream < (long)realReadCount)
            {
                realReadCount = (int)countStream;
            }
            readCount = ftpStream.ReadAsync(byteBuffer, 0, realReadCount).Result;
            countStream =  countStream - realReadCount;
            return readCount;
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
