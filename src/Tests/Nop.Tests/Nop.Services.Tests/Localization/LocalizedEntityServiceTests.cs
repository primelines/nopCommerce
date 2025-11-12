using System.Globalization;
using FluentAssertions;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using NUnit.Framework;

namespace Nop.Tests.Nop.Services.Tests.Localization;

[TestFixture]
public class LocalizedEntityServiceTests : BaseNopTest
{
    private ILocalizedEntityService _localizedEntityService;

    [OneTimeSetUp]
    public void SetUp()
    {
        _localizedEntityService = GetService<ILocalizedEntityService>();
    }

    [Test]
    public async Task CanSaveLocalizedValueAsync()
    {
        var product = await GetService<IProductService>().GetProductByIdAsync(1);

        await _localizedEntityService.SaveLocalizedValueAsync(product, p => p.Name, "test lang 1", 1);

        var name = await _localizedEntityService.GetLocalizedValueAsync(1, 1, nameof(Product),
            nameof(Product.Name));

        name.Should().Be("test lang 1");

    }
}