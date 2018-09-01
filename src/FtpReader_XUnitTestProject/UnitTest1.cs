using FtpReader.Client;
using System;
using Xunit;
using Xunit.Abstractions;

namespace FtpReader_XUnitTestProject
{
    public class UnitTest1
    {

        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }


        String testUrl = "ftp://ftp.yyyyyyyyyy";
        String testUserName = "xxxxxxxxxx";
        String testPassword = "yyyyyyyyy";

        String testDownloadFileUrl = "ftp://ftp.zzzzzzzzzzzzzzzzzzz";
        String testLocalFile = "e:/tst/t1.zip";

        [Fact]
        public void ReadFilesList()
        {
            FtpReaderClient ftpClient = new FtpReaderClient(testUrl, testUserName, testPassword);
            var listFile = ftpClient.ListFiles();
            foreach ( var item in listFile)
            {
                output.WriteLine(item);
            }
        }

        [Fact]
        public void DownloadFile()
        {
            try
            {
                FtpReaderClient ftpClient = new FtpReaderClient(testUrl, testUserName, testPassword);
                ftpClient.Download(testDownloadFileUrl, testLocalFile);
            } catch ( Exception ex)
            {
                output.WriteLine(ex.Message);
            }

        }
    }
}
