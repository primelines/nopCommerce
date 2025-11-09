
namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents a paging request model
/// </summary>
public partial interface IPagingRequestDto
{
    /// <summary>
    /// Gets a page number
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Gets a page size
    /// </summary>
    int PageSize { get; }
}