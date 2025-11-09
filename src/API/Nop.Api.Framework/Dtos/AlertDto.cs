namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Alert model
/// </summary>
public partial record ActionAlertDto : BaseNopEntityDto
{
    /// <summary>
    /// Window ID
    /// </summary>
    public string WindowId { get; set; }
    /// <summary>
    /// Alert ID
    /// </summary>
    public string AlertId { get; set; }
    /// <summary>
    /// Alert message
    /// </summary>
    public string AlertMessage { get; set; }
}