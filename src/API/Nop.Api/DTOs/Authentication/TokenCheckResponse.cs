using System;
using Newtonsoft.Json;

namespace Nop.Api.DTOs.Authentication
{
    public class TokenCheckResponse
    {
        [JsonProperty("username")]
        public string Username { get; init; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; init; }

        [JsonProperty("is_registered")]
        public bool IsRegistered { get; init; }

        [JsonProperty("is_vendor")]
        public bool IsVendor { get; init; }

        [JsonProperty("customer_guid")]
        public Guid CustomerGuid { get; init; }
    }
}
