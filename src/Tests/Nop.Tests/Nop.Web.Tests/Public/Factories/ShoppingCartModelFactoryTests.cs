using FluentAssertions;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Web.Factories;
using Nop.Web.Models.ShoppingCart;
using NUnit.Framework;

namespace Nop.Tests.Nop.Web.Tests.Public.Factories;

[TestFixture]
public class ShoppingCartModelFactoryTests : WebTest
{
    private IShoppingCartModelFactory _shoppingCartModelFactory;
    private IShoppingCartService _shoppingCartService;
    private IWorkContext _workContext;
    private IProductService _producService;
    private ILocalizationService _localizationService;
    private ShoppingCartItem _shoppingCartItem;
    private ICustomerService _customerService;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _shoppingCartModelFactory = GetService<IShoppingCartModelFactory>();
        _shoppingCartService = GetService<IShoppingCartService>();
        _workContext = GetService<IWorkContext>();
        _producService = GetService<IProductService>();
        _localizationService = GetService<ILocalizationService>();
        _customerService = GetService<ICustomerService>();

        var store = await GetService<IStoreContext>().GetCurrentStoreAsync();

        var customer = await _workContext.GetCurrentCustomerAsync();

        _shoppingCartItem = new ShoppingCartItem
        {
            ProductId = 1,
            Quantity = 1,
            CustomerId = customer.Id,
            StoreId = store.Id
        };


        var shoppingCartRepo = GetService<IRepository<ShoppingCartItem>>();

        await shoppingCartRepo.InsertAsync(new List<ShoppingCartItem> { _shoppingCartItem });

        customer.HasShoppingCartItems = true;
        await _customerService.UpdateCustomerAsync(customer);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _shoppingCartService.DeleteShoppingCartItemAsync(_shoppingCartItem);

        var customer = await _workContext.GetCurrentCustomerAsync();
        customer.HasShoppingCartItems = false;
        await _customerService.UpdateCustomerAsync(customer);
    }

    [Test]
    public async Task CanPrepareEstimateShippingModel()
    {
        var model = await _shoppingCartModelFactory.PrepareEstimateShippingModelAsync(await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync()));

        model.AvailableCountries.Any().Should().BeTrue();
        model.AvailableStates.Any().Should().BeTrue();
        model.Enabled.Should().BeTrue();
        model.ZipPostalCode.Should().Be("10021");
        model.CountryId.Should().BeNull();
        model.StateProvinceId.Should().BeNull();
    }

    [Test]
    public async Task CanPrepareShoppingCartModel()
    {
        var model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(new ShoppingCartModel(),
            new List<ShoppingCartItem> { _shoppingCartItem });

        model.IsEditable.Should().BeTrue();
        model.Items.Any().Should().BeTrue();
        model.Items.Count.Should().Be(1);
        model.Warnings.Count.Should().Be(0);

        model.OrderReviewData.Should().NotBeNull();
        model.OrderReviewData.Display.Should().BeFalse();
        model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(new ShoppingCartModel(),
            new List<ShoppingCartItem> { _shoppingCartItem }, true, true, true);
        model.OrderReviewData.Should().NotBeNull();
        model.OrderReviewData.Display.Should().BeTrue();
    }


    [Test]
    public async Task CanPrepareMiniShoppingCartModel()
    {
        var model = await _shoppingCartModelFactory.PrepareMiniShoppingCartModelAsync();

        model.CurrentCustomerIsGuest.Should().BeFalse();
        model.Items.Any().Should().BeTrue();
        model.Items.Count.Should().Be(1);
        model.TotalProducts.Should().Be(1);
        model.SubTotal.Should().Be("$1,200.00");
    }

    [Test]
    public async Task CanPrepareOrderTotalsModel()
    {
        var model = await _shoppingCartModelFactory.PrepareOrderTotalsModelAsync(new List<ShoppingCartItem> { _shoppingCartItem }, true);

        model.SubTotal.Should().Be("$1,200.00");
        model.OrderTotal.Should().Be("$1,200.00");

        model.GiftCards.Any().Should().BeFalse();
        model.Shipping.Should().Be("$0.00");
        model.Tax.Should().Be("$0.00");
        model.WillEarnRewardPoints.Should().Be(120);
    }

    [Test]
    public async Task CanPrepareEstimateShippingResultModel()
    {
        var model = await _shoppingCartModelFactory.PrepareEstimateShippingResultModelAsync(new List<ShoppingCartItem> { _shoppingCartItem }, new EstimateShippingModel(), true);
        model.Errors.Any().Should().BeFalse();
    }

    [Test]
    public async Task CanPrepareCartItemPictureModel()
    {
        var product = await _producService.GetProductByIdAsync(_shoppingCartItem.ProductId);

        var model = await _shoppingCartModelFactory.PrepareCartItemPictureModelAsync(_shoppingCartItem, 100, true, await _localizationService.GetLocalizedAsync(product, x => x.Name));

        model.AlternateText.Should().Be("Picture of Build your own computer");
        model.ImageUrl.Should()
            .Be($"http://{NopTestsDefaults.HostIpAddress}/images/thumbs/0000020_build-your-own-computer_100.jpeg");
        model.Title.Should().Be("Show details for Build your own computer");

        model.FullSizeImageUrl.Should().Be($"http://{NopTestsDefaults.HostIpAddress}/images/thumbs/0000020_build-your-own-computer.jpeg");
        model.ThumbImageUrl.Should().BeNull();
    }
}