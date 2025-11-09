using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.DTOs.Checkout;

[JsonObject(Title = "CheckoutPaymentMethod")]
public partial record CheckoutPaymentMethodDto : BaseNopDto
{
    public CheckoutPaymentMethodDto()
    {
        PaymentMethods = new List<PaymentMethodDto>();
    }


    [JsonProperty("payment_methods")]
    public IList<PaymentMethodDto> PaymentMethods { get; set; }


    [JsonProperty("display_reward_points")]
    public bool DisplayRewardPoints { get; set; }

    [JsonProperty("reward_points_balance")]
    public int RewardPointsBalance { get; set; }

    [JsonProperty("reward_points_to_use")]
    public int RewardPointsToUse { get; set; }

    [JsonProperty("reward_points_to_use_amount")]
    public string RewardPointsToUseAmount { get; set; }

    [JsonProperty("reward_points_enough_to_pay_for_order")]
    public bool RewardPointsEnoughToPayForOrder { get; set; }

    [JsonProperty("use_reward_points")]
    public bool UseRewardPoints { get; set; }

    #region Nested classes

    public partial record PaymentMethodDto : BaseNopDto
    {

    [JsonProperty("payment_method_system_name")]
        public string PaymentMethodSystemName { get; set; }

    [JsonProperty("name")]
        public string Name { get; set; }

    [JsonProperty("description")]
        public string Description { get; set; }

    [JsonProperty("fee")]
        public string Fee { get; set; }

    [JsonProperty("selected")]
        public bool Selected { get; set; }

    [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
    }

    #endregion
}
