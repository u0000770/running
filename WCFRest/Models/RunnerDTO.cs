using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFRest.Models
{
    [DataContract]
    public class RunnerDTO
    {
        [DataMember]
        public int RunnerId { get; set; }

        [DataMember]
        public string RunnerName { get; set; }

    }
}