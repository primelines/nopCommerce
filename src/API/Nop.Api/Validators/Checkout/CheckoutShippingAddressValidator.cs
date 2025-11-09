using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Api.DTOs.Checkout;
using Nop.Api.Validators.Common;

namespace Nop.Api.Validators.Checkout;

public partial class CheckoutShippingAddressValidator : AbstractValidator<CheckoutShippingAddressDto>
{
    public CheckoutShippingAddressValidator(ILocalizationService localizationService,
        IStateProvinceService stateProvinceService,
        AddressSettings addressSettings,
        CustomerSettings customerSettings)
    {
        RuleFor(shippingAdress => shippingAdress.ShippingNewAddress).SetValidator(
            new AddressValidator(localizationService, stateProvinceService, addressSettings, customerSettings));
    }
}