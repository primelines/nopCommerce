using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.Errors
{
    public class ErrorsRootObject 
    {

    [JsonProperty("errors")]
        public Dictionary<string, List<string>> Errors { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "errors";
        }

        public Type GetPrimaryPropertyType()
        {
            return Errors.GetType();
        }
    }
}
