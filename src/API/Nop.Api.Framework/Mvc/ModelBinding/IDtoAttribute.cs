
namespace Nop.Api.Framework.Mvc.ModelBinding;

/// <summary>
/// Represents custom model attribute
/// </summary>
public partial interface IDtoAttribute
{
    /// <summary>
    /// Gets name of the attribute
    /// </summary>
    string Name { get; }
}