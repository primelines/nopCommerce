using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Api.DTOs.Customer;

namespace Nop.Api.Factories;

/// <summary>
/// Represents the interface of the customer model factory
/// </summary>
public partial interface ICustomerDtoFactory
{
    /// <summary>
    /// Prepare the customer info model
    /// </summary>
    /// <param name="model">Customer info model</param>
    /// <param name="customer">Customer</param>
    /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
    /// <param name="overrideCustomCustomerAttributesXml">Overridden customer attributes in XML format; pass null to use CustomCustomerAttributes of customer</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer info model
    /// </returns>
    Task<CustomerInfoDto> PrepareCustomerInfoDtoAsync(CustomerInfoDto model, Customer customer,
        bool excludeProperties, string overrideCustomCustomerAttributesXml = "");

    /// <summary>
    /// Prepare the customer register model
    /// </summary>
    /// <param name="model">Customer register model</param>
    /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
    /// <param name="overrideCustomCustomerAttributesXml">Overridden customer attributes in XML format; pass null to use CustomCustomerAttributes of customer</param>
    /// <param name="setDefaultValues">Whether to populate model properties by default values</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer register model
    /// </returns>
    Task<RegisterDto> PrepareRegisterDtoAsync(RegisterDto model, bool excludeProperties,
        string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false);

    /// <summary>
    /// Prepare the login model
    /// </summary>
    /// <param name="checkoutAsGuest">Whether to checkout as guest is enabled</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the login model
    /// </returns>
    Task<LoginDto> PrepareLoginDtoAsync(bool? checkoutAsGuest);

    /// <summary>
    /// Prepare the password recovery model
    /// </summary>
    /// <param name="model">Password recovery model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the password recovery model
    /// </returns>
    Task<PasswordRecoveryDto> PreparePasswordRecoveryModelAsync(PasswordRecoveryDto model);

    /// <summary>
    /// Prepare the register result model
    /// </summary>
    /// <param name="resultId">Value of UserRegistrationType enum</param>
    /// <param name="returnUrl">URL to redirect</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the register result model
    /// </returns>
    Task<RegisterResultDto> PrepareRegisterResultDtoAsync(int resultId, string returnUrl);

    /// <summary>
    /// Prepare the customer navigation model
    /// </summary>
    /// <param name="selectedTabId">Identifier of the selected tab</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer navigation model
    /// </returns>
    Task<CustomerNavigationDto> PrepareCustomerNavigationDtoAsync(int selectedTabId = 0);

    /// <summary>
    /// Prepare the customer address list model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer address list model  
    /// </returns>
    Task<CustomerAddressListDto> PrepareCustomerAddressListDtoAsync();

    /// <summary>
    /// Prepare the change password model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the change password model
    /// </returns>
    Task<ChangePasswordDto> PrepareChangePasswordDtoAsync();

    /// <summary>
    /// Prepare the customer avatar model
    /// </summary>
    /// <param name="model">Customer avatar model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer avatar model
    /// </returns>
    Task<CustomerAvatarDto> PrepareCustomerAvatarDtoAsync(CustomerAvatarDto model);

    /// <summary>
    /// Prepare the GDPR tools model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the gDPR tools model
    /// </returns>
    Task<GdprToolsDto> PrepareGdprToolsDtoAsync();

    /// <summary>
    /// Prepare the check gift card balance model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the check gift card balance model
    /// </returns>
    Task<CheckGiftCardBalanceDto> PrepareCheckGiftCardBalanceDtoAsync();

    /// <summary>
    /// Prepare the multi-factor authentication model
    /// </summary>
    /// <param name="model">Multi-factor authentication model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the multi-factor authentication model
    /// </returns>
    Task<MultiFactorAuthenticationDto> PrepareMultiFactorAuthenticationDtoAsync(MultiFactorAuthenticationDto model);

    /// <summary>
    /// Prepare the multi-factor provider model
    /// </summary>
    /// <param name="providerModel">Multi-factor provider model</param>
    /// <param name="sysName">Multi-factor provider system name</param>
    /// <param name="isLogin">Is login page</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the multi-factor authentication model
    /// </returns>
    Task<MultiFactorAuthenticationProviderDto> PrepareMultiFactorAuthenticationProviderDtoAsync(MultiFactorAuthenticationProviderDto providerModel, string sysName, bool isLogin = false);

    /// <summary>
    /// Prepare the custom customer attribute models
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="overrideAttributesXml">Overridden customer attributes in XML format; pass null to use CustomCustomerAttributes of customer</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of the customer attribute model
    /// </returns>
    Task<IList<CustomerAttributeDto>> PrepareCustomCustomerAttributesAsync(Customer customer, string overrideAttributesXml = "");
}