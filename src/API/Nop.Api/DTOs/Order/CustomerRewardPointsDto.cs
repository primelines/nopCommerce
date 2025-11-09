using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.Framework.Mvc.ModelBinding;
using Nop.Api.DTOs.Common;

namespace Nop.Api.DTOs.Order;

[JsonObject(Title = "CustomerRewardPoints")]
public partial record CustomerRewardPointsDto : BaseNopDto
{
    public CustomerRewardPointsDto()
    {
        RewardPoints = new List<RewardPointsHistoryModel>();
    }


    [JsonProperty("reward_points")]
    public IList<RewardPointsHistoryModel> RewardPoints { get; set; }

    [JsonProperty("pager")]
    public PagerDto Pager { get; set; }

    [JsonProperty("reward_points_balance")]
    public int RewardPointsBalance { get; set; }

    [JsonProperty("reward_points_amount")]
    public string RewardPointsAmount { get; set; }

    [JsonProperty("minimum_reward_points_balance")]
    public int MinimumRewardPointsBalance { get; set; }

    [JsonProperty("minimum_reward_points_amount")]
    public string MinimumRewardPointsAmount { get; set; }

    #region Nested classes

    public partial record RewardPointsHistoryModel : BaseNopEntityDto
    {
        [NopResourceDisplayName("RewardPoints.Fields.Points")]

    [JsonProperty("points")]
        public int Points { get; set; }

        [NopResourceDisplayName("RewardPoints.Fields.PointsBalance")]

    [JsonProperty("points_balance")]
        public string PointsBalance { get; set; }

        [NopResourceDisplayName("RewardPoints.Fields.Message")]

    [JsonProperty("message")]
        public string Message { get; set; }

        [NopResourceDisplayName("RewardPoints.Fields.CreatedDate")]

    [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("RewardPoints.Fields.EndDate")]

    [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }
    }

    #endregion
}
