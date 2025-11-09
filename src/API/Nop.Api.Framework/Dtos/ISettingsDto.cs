
namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents a settings model
/// </summary>
public partial interface ISettingsDto
{
    /// <summary>
    /// Gets or sets an active store scope configuration (store identifier)
    /// </summary>
    int ActiveStoreScopeConfiguration { get; set; }
}