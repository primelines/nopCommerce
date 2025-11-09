using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "ForumPost")]
public partial record ForumPostDto : BaseNopDto
{

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }

    [JsonProperty("forum_topic_se_name")]
    public string ForumTopicSeName { get; set; }


    [JsonProperty("formatted_text")]
    public string FormattedText { get; set; }


    [JsonProperty("is_current_customer_allowed_to_edit_post")]
    public bool IsCurrentCustomerAllowedToEditPost { get; set; }

    [JsonProperty("is_current_customer_allowed_to_delete_post")]
    public bool IsCurrentCustomerAllowedToDeletePost { get; set; }


    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }

    [JsonProperty("allow_viewing_profiles")]
    public bool AllowViewingProfiles { get; set; }

    [JsonProperty("customer_avatar_url")]
    public string CustomerAvatarUrl { get; set; }

    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }

    [JsonProperty("is_customer_forum_moderator")]
    public bool IsCustomerForumModerator { get; set; }


    [JsonProperty("post_created_on_str")]
    public string PostCreatedOnStr { get; set; }


    [JsonProperty("show_customers_post_count")]
    public bool ShowCustomersPostCount { get; set; }

    [JsonProperty("forum_post_count")]
    public int ForumPostCount { get; set; }


    [JsonProperty("show_customers_join_date")]
    public bool ShowCustomersJoinDate { get; set; }

    [JsonProperty("customer_join_date")]
    public DateTime CustomerJoinDate { get; set; }


    [JsonProperty("show_customers_location")]
    public bool ShowCustomersLocation { get; set; }

    [JsonProperty("customer_location")]
    public string CustomerLocation { get; set; }


    [JsonProperty("allow_private_messages")]
    public bool AllowPrivateMessages { get; set; }


    [JsonProperty("signatures_enabled")]
    public bool SignaturesEnabled { get; set; }

    [JsonProperty("formatted_signature")]
    public string FormattedSignature { get; set; }


    [JsonProperty("current_topic_page")]
    public int CurrentTopicPage { get; set; }


    [JsonProperty("allow_post_voting")]
    public bool AllowPostVoting { get; set; }

    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    [JsonProperty("vote_is_up")]
    public bool? VoteIsUp { get; set; }
}
