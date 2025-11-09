using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Api.Factories;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc.Filters;
using System.Net;
using Nop.Api.DTOs.Order;
using Nop.Api.DTOs.ShoppingCart;
using Nop.Core.Domain.Orders;

namespace Nop.Api.Controllers;

public partial class OrderController : BasePublicController
{
    #region Fields

    protected readonly ICustomerService _customerService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IOrderDtoFactory _orderModelFactory;
    protected readonly IOrderProcessingService _orderProcessingService;
    protected readonly IOrderService _orderService;
    protected readonly IPaymentService _paymentService;
    protected readonly IPdfService _pdfService;
    protected readonly IShipmentService _shipmentService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly RewardPointsSettings _rewardPointsSettings;

    #endregion

    #region Ctor

    public OrderController(ICustomerService customerService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IOrderDtoFactory orderModelFactory,
        IOrderProcessingService orderProcessingService,
        IOrderService orderService,
        IPaymentService paymentService,
        IPdfService pdfService,
        IShipmentService shipmentService,
        IWebHelper webHelper,
        IWorkContext workContext,
        RewardPointsSettings rewardPointsSettings)
    {
        _customerService = customerService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _orderModelFactory = orderModelFactory;
        _orderProcessingService = orderProcessingService;
        _orderService = orderService;
        _paymentService = paymentService;
        _pdfService = pdfService;
        _shipmentService = shipmentService;
        _webHelper = webHelper;
        _workContext = workContext;
        _rewardPointsSettings = rewardPointsSettings;
    }

    #endregion

    #region Methods

    //My account / Orders
    [HttpGet]
    [Route("CustomerOrders", Name = "CustomerOrders")]
    [ProducesResponseType(typeof(CustomerOrderListDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> CustomerOrders()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var model = await _orderModelFactory.PrepareCustomerOrderListDtoAsync();
        return Ok(model);
    }

    //My account / Reward points
    [HttpGet]
    [Route("CustomerRewardPoints", Name = "CustomerRewardPoints")]
    [ProducesResponseType(typeof(CustomerRewardPointsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> CustomerRewardPoints([FromQuery] int? pageNumber)
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        if (!_rewardPointsSettings.Enabled)
            return Error(); // RedirectToRoute("CustomerInfo");

        var model = await _orderModelFactory.PrepareCustomerRewardPointsAsync(pageNumber);
        return Ok(model);
    }

    //My account / Order details page
    [HttpGet]
    [Route("Details/{orderId}", Name = "Details")]
    [ProducesResponseType(typeof(OrderDetailsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> Details([FromRoute] int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        var customer = await _workContext.GetCurrentCustomerAsync();

        if (order == null || order.Deleted || customer.Id != order.CustomerId)
            return Challenge();

        var model = await _orderModelFactory.PrepareOrderDetailsDtoAsync(order);
        return Ok(model);
    }

    //My account / Order details page / Print
    
    //TODO: No routes
    //public virtual async Task<IActionResult> PrintOrderDetails(int orderId)
    //{
    //    var order = await _orderService.GetOrderByIdAsync(orderId);
    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    if (order == null || order.Deleted || customer.Id != order.CustomerId)
    //        return Challenge();

    //    var model = await _orderModelFactory.PrepareOrderDetailsDtoAsync(order);
    //    model.PrintMode = true;

    //    return Ok(model);
    //}

    //My account / Order details page / PDF invoice
    [CheckLanguageSeoCode(ignore: true)]
    [HttpGet]
    [Route("GetPdfInvoice/{orderId}", Name = "GetPdfInvoice")]
    [ProducesResponseType(typeof(byte), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> GetPdfInvoice([FromRoute] int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (order == null || order.Deleted || customer.Id != order.CustomerId)
            return Challenge();

        byte[] bytes;
        await using (var stream = new MemoryStream())
        {
            await _pdfService.PrintOrderToPdfAsync(stream, order, await _workContext.GetWorkingLanguageAsync());
            bytes = stream.ToArray();
        }
        return File(bytes, MimeTypes.ApplicationPdf, string.Format(await _localizationService.GetResourceAsync("PDFInvoice.FileName"), order.CustomOrderNumber) + ".pdf");
    }

    //My account / Order details page / re-order
    [HttpGet]
    [Route("ReOrder/{orderId}", Name = "ReOrder")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ReOrder(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (order == null || order.Deleted || customer.Id != order.CustomerId)
            return Challenge();

        var warnings = await _orderProcessingService.ReOrderAsync(order);

        if (warnings.Any())
            _notificationService.WarningNotification(await _localizationService.GetResourceAsync("ShoppingCart.ReorderWarning"));

        return RedirectToRoute("ShoppingCart");
    }

    //My account / Order details page / Complete payment

    [FormValueRequired("repost-payment")]
    [HttpGet]
    [Route("RePostPayment/{orderId}", Name = "RePostPayment")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> RePostPayment([FromRoute] int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (order == null || order.Deleted || customer.Id != order.CustomerId)
            return Challenge();

        if (!await _paymentService.CanRePostProcessPaymentAsync(order))
            return RedirectToRoute("OrderDetails", new { orderId = orderId });

        var postProcessPaymentRequest = new PostProcessPaymentRequest
        {
            Order = order
        };
        await _paymentService.PostProcessPaymentAsync(postProcessPaymentRequest);

        if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
        {
            //redirection or POST has been done in PostProcessPayment
            return Content("Redirected");
        }

        //if no redirection has been done (to a third-party payment page)
        //theoretically it's not possible
        return RedirectToRoute("OrderDetails", new { orderId = orderId });
    }

    //My account / Order details page / Shipment details page
    [HttpGet]
    [Route("ShipmentDetails/{shipmentId}", Name = "ShipmentDetails")]
    [ProducesResponseType(typeof(ShipmentDetailsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public virtual async Task<IActionResult> ShipmentDetails([FromRoute] int shipmentId)
    {
        var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);
        if (shipment == null)
            return Challenge();

        var order = await _orderService.GetOrderByIdAsync(shipment.OrderId);
        var customer = await _workContext.GetCurrentCustomerAsync();

        if (order == null || order.Deleted || customer.Id != order.CustomerId)
            return Challenge();

        var model = await _orderModelFactory.PrepareShipmentDetailsDtoAsync(shipment);
        return Ok(model);
    }

    #endregion

}
