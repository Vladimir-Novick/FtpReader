using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FtpReader.Client
{
    [DataContract(Namespace = "")]
    public class FtpReaderConfigData
    {
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]

        public string Password { get; set; }


    
    }
}
