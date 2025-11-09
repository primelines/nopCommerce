namespace Nop.Api.Framework.Dtos;

/// <summary>
/// Represents localized model
/// </summary>
public partial interface ILocalizedDto
{
}

/// <summary>
/// Represents generic localized model
/// </summary>
/// <typeparam name="TLocalizedModel">Localized model type</typeparam>
public partial interface ILocalizedDto<TLocalizedModel> : ILocalizedDto
{
    /// <summary>
    /// Gets or sets localized locale models
    /// </summary>
    IList<TLocalizedModel> Locales { get; set; }
}