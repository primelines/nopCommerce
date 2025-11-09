using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Topics;

[JsonObject(Title = "Topic")]
public partial record TopicDto : BaseNopEntityDto
{

    [JsonProperty("system_name")]
    public string SystemName { get; set; }


    [JsonProperty("include_in_sitemap")]
    public bool IncludeInSitemap { get; set; }


    [JsonProperty("is_password_protected")]
    public bool IsPasswordProtected { get; set; }


    [JsonProperty("title")]
    public string Title { get; set; }


    [JsonProperty("body")]
    public string Body { get; set; }


    [JsonProperty("meta_keywords")]
    public string MetaKeywords { get; set; }


    [JsonProperty("meta_description")]
    public string MetaDescription { get; set; }


    [JsonProperty("meta_title")]
    public string MetaTitle { get; set; }


    [JsonProperty("se_name")]
    public string SeName { get; set; }


    [JsonProperty("topic_template_id")]
    public int TopicTemplateId { get; set; }
}
