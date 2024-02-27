using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class UserAuthen
    {
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty("pwd", Required = Required.Always)]
        public string pwd { get; set; }

        [JsonProperty("token", Required = Required.Always)]
        public string token { get; set; }
    }

}