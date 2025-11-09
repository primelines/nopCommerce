using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents base nopCommerce model
/// </summary>
public partial record BaseNopDto
{
    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    public BaseNopDto()
    {
        //CustomProperties = new Dictionary<string, string>();
        PostInitialize();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Perform additional actions for the model initialization
    /// </summary>
    /// <remarks>Developers can override this method in custom partial classes in order to add some custom initialization code to constructors</remarks>
    protected virtual void PostInitialize()
    {
    }

    #endregion

    #region Properties

    ///// <summary>
    ///// Gets or sets property to store any custom values for models 
    ///// </summary>
    //[XmlIgnore]
    //[JsonProperty("custom_properties")]
    //public Dictionary<string, string> CustomProperties { get; set; }

    #endregion
}