using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Api.DTOs.Customer;
using Nop.Api.Validators.Common;

namespace Nop.Api.Validators.Customer;

public partial class CustomerAddressEditValidator : AbstractValidator<CustomerAddressEditDto>
{
    public CustomerAddressEditValidator(ILocalizationService localizationService,
        IStateProvinceService stateProvinceService,
        AddressSettings addressSettings,
        CustomerSettings customerSettings)
    {
        RuleFor(model => model.Address).SetValidator(new AddressValidator(localizationService, stateProvinceService, addressSettings, customerSettings));
    }
}