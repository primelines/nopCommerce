using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Profile;

[JsonObject(Title = "ProfileInfo")]
public partial record ProfileInfoDto : BaseNopDto
{

    [JsonProperty("customer_profile_id")]
    public int CustomerProfileId { get; set; }


    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }


    [JsonProperty("location_enabled")]
    public bool LocationEnabled { get; set; }

    [JsonProperty("location")]
    public string Location { get; set; }


    [JsonProperty("p_m_enabled")]
    public bool PMEnabled { get; set; }


    [JsonProperty("total_posts_enabled")]
    public bool TotalPostsEnabled { get; set; }

    [JsonProperty("total_posts")]
    public string TotalPosts { get; set; }


    [JsonProperty("join_date_enabled")]
    public bool JoinDateEnabled { get; set; }

    [JsonProperty("join_date")]
    public string JoinDate { get; set; }


    [JsonProperty("date_of_birth_enabled")]
    public bool DateOfBirthEnabled { get; set; }

    [JsonProperty("date_of_birth")]
    public string DateOfBirth { get; set; }
}
