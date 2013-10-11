using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SignalrWeb.Models
{
    public class Line
    {
        [Range(1,50)]
        [JsonProperty("b")]
        public int WidthBrush { get; set; }

        [Range(0, 800)]
        [JsonProperty("x")]
        public int OldX { get; set; }

        [Range(0, 500)]
        [JsonProperty("y")]
        public int OldY { get; set; }

        [Range(0, 800)]
        [JsonProperty("nx")]
        public int NewX { get; set; }

        [Range(0, 500)]
        [JsonProperty("ny")]
        public int NewY { get; set; }

        [JsonProperty("c")]
        public string Color { get; set; }

        [JsonProperty("g")]
        public string GroupName { get; set; }

    }
}