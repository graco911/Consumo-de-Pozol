using Newtonsoft.Json;
using System;

namespace PozolFrioApp
{
    public class PozolFrio
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string id { get; set; }

        public bool conAzucar { get; set; }

        public DateTime DateUtc { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

        [Newtonsoft.Json.JsonIgnore]
        public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t"); } }
    }
}