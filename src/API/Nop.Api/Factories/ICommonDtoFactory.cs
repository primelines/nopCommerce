using Nop.Core.Domain.Vendors;
using Nop.Api.DTOs.Common;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the interface of the common models factory
/// </summary>
public partial interface ICommonDtoFactory
{
    /// <summary>
    /// Prepare the language selector model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the language selector model
    /// </returns>
    Task<LanguageSelectorDto> PrepareLanguageSelectorDtoAsync();

    /// <summary>
    /// Prepare the currency selector model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the currency selector model
    /// </returns>
    Task<CurrencySelectorDto> PrepareCurrencySelectorDtoAsync();

    /// <summary>
    /// Prepare the tax type selector model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the ax type selector model
    /// </returns>
    Task<TaxTypeSelectorDto> PrepareTaxTypeSelectorDtoAsync();

    /// <summary>
    /// Prepare the header links model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the header links model
    /// </returns>
    Task<HeaderLinksDto> PrepareHeaderLinksDtoAsync();

    /// <summary>
    /// Prepare the admin header links model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the admin header links model
    /// </returns>
    Task<AdminHeaderLinksDto> PrepareAdminHeaderLinksDtoAsync();

    /// <summary>
    /// Prepare the social model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the social model
    /// </returns>
    Task<SocialDto> PrepareSocialDtoAsync();

    ///// <summary>
    ///// Prepare the footer model
    ///// </summary>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the footer model
    ///// </returns>
    //Task<FooterDto> PrepareFooterDtoAsync();

    /// <summary>
    /// Prepare the contact us model
    /// </summary>
    /// <param name="model">Contact us model</param>
    /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the contact us model
    /// </returns>
    Task<ContactUsDto> PrepareContactUsDtoAsync(ContactUsDto model, bool excludeProperties);

    /// <summary>
    /// Prepare the contact vendor model
    /// </summary>
    /// <param name="model">Contact vendor model</param>
    /// <param name="vendor">Vendor</param>
    /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the contact vendor model
    /// </returns>
    Task<ContactVendorDto> PrepareContactVendorDtoAsync(ContactVendorDto model, Vendor vendor,
        bool excludeProperties);

    /// <summary>
    /// Prepare the favicon model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the favicon model
    /// </returns>
    Task<FaviconAndAppIconsDto> PrepareFaviconAndAppIconsDtoAsync();

    /// <summary>
    /// Get robots.txt file
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the robots.txt file as string
    /// </returns>
    Task<string> PrepareRobotsTextFileAsync();
}