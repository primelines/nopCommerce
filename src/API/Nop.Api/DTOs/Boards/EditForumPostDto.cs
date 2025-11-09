using Newtonsoft.Json;
using Nop.Core.Domain.Forums;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "EditForumPost")]
public partial record EditForumPostDto : BaseNopDto
{
    #region Properties


    [JsonProperty("id")]
    public int Id { get; set; }


    [JsonProperty("forum_topic_id")]
    public int ForumTopicId { get; set; }


    [JsonProperty("is_edit")]
    public bool IsEdit { get; set; }


    [JsonProperty("text")]
    public string Text { get; set; }


    [JsonProperty("forum_editor")]
    public EditorType ForumEditor { get; set; }


    [JsonProperty("forum_name")]
    public string ForumName { get; set; }


    [JsonProperty("forum_topic_subject")]
    public string ForumTopicSubject { get; set; }


    [JsonProperty("forum_topic_se_name")]
    public string ForumTopicSeName { get; set; }


    [JsonProperty("is_customer_allowed_to_subscribe")]
    public bool IsCustomerAllowedToSubscribe { get; set; }


    [JsonProperty("subscribed")]
    public bool Subscribed { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }

    #endregion
}
