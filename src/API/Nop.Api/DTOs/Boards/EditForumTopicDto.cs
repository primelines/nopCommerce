using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Forums;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Boards;

[JsonObject(Title = "EditForumTopic")]
public partial record EditForumTopicDto : BaseNopDto
{
    #region Ctor

    public EditForumTopicDto()
    {
        TopicPriorities = new List<SelectListItemDto>();
    }

    #endregion

    #region Properties


    [JsonProperty("is_edit")]
    public bool IsEdit { get; set; }


    [JsonProperty("id")]
    public int Id { get; set; }


    [JsonProperty("forum_id")]
    public int ForumId { get; set; }


    [JsonProperty("forum_name")]
    public string ForumName { get; set; }


    [JsonProperty("forum_se_name")]
    public string ForumSeName { get; set; }


    [JsonProperty("topic_type_id")]
    public int TopicTypeId { get; set; }


    [JsonProperty("forum_editor")]
    public EditorType ForumEditor { get; set; }


    [JsonProperty("subject")]
    public string Subject { get; set; }


    [JsonProperty("text")]
    public string Text { get; set; }


    [JsonProperty("is_customer_allowed_to_set_topic_priority")]
    public bool IsCustomerAllowedToSetTopicPriority { get; set; }


    [JsonProperty("topic_priorities")]
    public IEnumerable<SelectListItemDto> TopicPriorities { get; set; }


    [JsonProperty("is_customer_allowed_to_subscribe")]
    public bool IsCustomerAllowedToSubscribe { get; set; }


    [JsonProperty("subscribed")]
    public bool Subscribed { get; set; }


    [JsonProperty("display_captcha")]
    public bool DisplayCaptcha { get; set; }

    #endregion
}
