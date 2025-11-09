using Newtonsoft.Json;
using Nop.Api.Framework.Dtos;
using Nop.Api.DTOs.Media;

namespace Nop.Api.DTOs.Catalog;
[JsonObject(Title = "ProductAttributeValue")]
public partial record ProductAttributeValueDto : BaseNopEntityDto
    {
        public ProductAttributeValueDto()
        {
            ImageSquaresPicture = new PictureDto();
        }


    [JsonProperty("name")]
        public string Name { get; set; }


    [JsonProperty("color_squares_rgb")]
        public string ColorSquaresRgb { get; set; }

        //picture model is used with "image square" attribute type

    [JsonProperty("image_squares_picture")]
        public PictureDto ImageSquaresPicture { get; set; }


    [JsonProperty("price_adjustment")]
        public string PriceAdjustment { get; set; }


    [JsonProperty("price_adjustment_use_percentage")]
        public bool PriceAdjustmentUsePercentage { get; set; }


    [JsonProperty("price_adjustment_value")]
        public decimal PriceAdjustmentValue { get; set; }


    [JsonProperty("is_pre_selected")]
        public bool IsPreSelected { get; set; }

        //product picture ID (associated to this value)

    [JsonProperty("picture_id")]
        public int PictureId { get; set; }


    [JsonProperty("customer_enters_qty")]
        public bool CustomerEntersQty { get; set; }


    [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

