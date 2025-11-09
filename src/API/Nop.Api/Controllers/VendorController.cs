using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Api.Factories;
using Nop.Api.Framework.Controllers;
using Nop.Api.Framework.Mvc.Filters;
using Nop.Api.DTOs.Vendors;

namespace Nop.Api.Controllers;

public partial class VendorController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly ICustomerService _customerService;
    protected readonly IDownloadService _downloadService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPictureService _pictureService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVendorDtoFactory _vendorModelFactory;
    protected readonly IVendorService _vendorService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly VendorSettings _vendorSettings;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public VendorController(CaptchaSettings captchaSettings,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        ICustomerService customerService,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IHtmlFormatter htmlFormatter,
        ILocalizationService localizationService,
        IPictureService pictureService,
        IUrlRecordService urlRecordService,
        IVendorDtoFactory vendorModelFactory,
        IVendorService vendorService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        VendorSettings vendorSettings)
    {
        _captchaSettings = captchaSettings;
        _vendorAttributeParser = vendorAttributeParser;
        _vendorAttributeService = vendorAttributeService;
        _customerService = customerService;
        _downloadService = downloadService;
        _genericAttributeService = genericAttributeService;
        _htmlFormatter = htmlFormatter;
        _localizationService = localizationService;
        _pictureService = pictureService;
        _urlRecordService = urlRecordService;
        _vendorModelFactory = vendorModelFactory;
        _vendorService = vendorService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _vendorSettings = vendorSettings;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdatePictureSeoNamesAsync(Vendor vendor)
    {
        var picture = await _pictureService.GetPictureByIdAsync(vendor.PictureId);
        if (picture != null)
            await _pictureService.SetSeoFilenameAsync(picture.Id, await _pictureService.GetPictureSeNameAsync(vendor.Name));
    }

    protected virtual async Task<string> ParseVendorAttributesAsync(IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var attributesXml = "";
        var attributes = await _vendorAttributeService.GetAllAttributesAsync();
        foreach (var attribute in attributes)
        {
            var controlId = $"{NopVendorDefaults.VendorAttributePrefix}{attribute.Id}";
            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                {
                    var ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var selectedAttributeId = int.Parse(ctrlAttributes);
                        if (selectedAttributeId > 0)
                            attributesXml = _vendorAttributeParser.AddAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                    }
                }
                    break;
                case AttributeControlType.Checkboxes:
                {
                    var cblAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(cblAttributes))
                    {
                        foreach (var item in cblAttributes.ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries)
                                )
                        {
                            var selectedAttributeId = int.Parse(item);
                            if (selectedAttributeId > 0)
                                attributesXml = _vendorAttributeParser.AddAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }
                }
                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                {
                    //load read-only (already server-side selected) values
                    var attributeValues = await _vendorAttributeService.GetAttributeValuesAsync(attribute.Id);
                    foreach (var selectedAttributeId in attributeValues
                                 .Where(v => v.IsPreSelected)
                                 .Select(v => v.Id)
                                 .ToList())
                    {
                        attributesXml = _vendorAttributeParser.AddAttribute(attributesXml,
                            attribute, selectedAttributeId.ToString());
                    }
                }
                    break;
                case AttributeControlType.TextBox:
                case AttributeControlType.MultilineTextbox:
                {
                    var ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var enteredText = ctrlAttributes.ToString().Trim();
                        attributesXml = _vendorAttributeParser.AddAttribute(attributesXml,
                            attribute, enteredText);
                    }
                }
                    break;
                case AttributeControlType.Datepicker:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                case AttributeControlType.FileUpload:
                //not supported vendor attributes
                default:
                    break;
            }
        }

        return attributesXml;
    }

    #endregion

    #region Methods

    [HttpGet]
    [Route("ApplyVendor", Name = "ApplyVendor")]
    [ProducesResponseType(typeof(ApplyVendorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ApplyVendor()
    {
        if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
            return Error(errorMessage:"Disabled form setting");

        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var model = new ApplyVendorDto();
        model = await _vendorModelFactory.PrepareApplyVendorDtoAsync(model, true, false, null);
        return Ok(model);
    }

    //[ValidateCaptcha]
    //TODO: deferent parameters ([FromQuery]  contentType)
    [HttpPost]
    [Route("ApplyVendorSubmit", Name = "ApplyVendorSubmit")]
    [ProducesResponseType(typeof(ApplyVendorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> ApplyVendorSubmit(ApplyVendorDto model, bool captchaValid, IFormFile uploadedFile, IFormCollection form)
    {
        if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
            return Error(errorMessage: "Disabled form setting");

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        if (await _customerService.IsAdminAsync(customer))
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Vendors.ApplyAccount.IsAdmin"));

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnApplyVendorPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }

        var pictureId = 0;

        if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        {
            try
            {
                var contentType = uploadedFile.ContentType.ToLowerInvariant();

                if (!contentType.StartsWith("image/") || contentType.StartsWith("image/svg"))
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Picture.ErrorMessage"));
                else
                {
                    var vendorPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);
                    var picture = await _pictureService.InsertPictureAsync(vendorPictureBinary, contentType, null);

                    if (picture != null)
                        pictureId = picture.Id;
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Picture.ErrorMessage"));
            }
        }

        //vendor attributes
        var vendorAttributesXml = await ParseVendorAttributesAsync(form);
        var warnings = (await _vendorAttributeParser.GetAttributeWarningsAsync(vendorAttributesXml)).ToList();
        foreach (var warning in warnings)
        {
            ModelState.AddModelError(string.Empty, warning);
        }

        if (ModelState.IsValid)
        {
            var description = _htmlFormatter.FormatText(model.Description, false, false, true, false, false, false);
            //disabled by default
            var vendor = new Vendor
            {
                Name = model.Name,
                Email = model.Email,
                PictureId = pictureId,
                Description = WebUtility.HtmlEncode(description)
            };
            await _vendorService.InsertVendorAsync(vendor);
            //search engine name (the same as vendor name)
            var seName = await _urlRecordService.ValidateSeNameAsync(vendor, vendor.Name, vendor.Name, true);
            await _urlRecordService.SaveSlugAsync(vendor, seName, 0);

            //associate to the current customer
            //but a store owner will have to manually add this customer role to "Vendors" role
            //if he wants to grant access to admin area
            customer.VendorId = vendor.Id;
            await _customerService.UpdateCustomerAsync(customer);

            //update picture seo file name
            await UpdatePictureSeoNamesAsync(vendor);

            //save vendor attributes
            await _genericAttributeService.SaveAttributeAsync(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

            //notify store owner here (email)
            await _workflowMessageService.SendNewVendorAccountApplyStoreOwnerNotificationAsync(customer,
                vendor, _localizationSettings.DefaultAdminLanguageId);

            model.DisableFormInput = true;
            model.Result = await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Submitted");
            return Ok(model);
        }

        //If we got this far, something failed, redisplay form
        model = await _vendorModelFactory.PrepareApplyVendorDtoAsync(model, false, true, vendorAttributesXml);
        return Ok(model);
    }

    [HttpGet]
    [Route("GetVendorInfo", Name = "VendorInfo")]
    [ProducesResponseType(typeof(VendorInfoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> Info()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        if (await _workContext.GetCurrentVendorAsync() == null || !_vendorSettings.AllowVendorsToEditInfo)
            return Error(); // RedirectToRoute("CustomerInfo");

        var model = new VendorInfoDto();
        model = await _vendorModelFactory.PrepareVendorInfoDtoAsync(model, false);
        return Ok(model);
    }

    //TODO: no routes
    //[HttpPost, ActionName("Info")]
    //[Route("Info")]
    [FormValueRequired("save-info-button")]

    public virtual async Task<IActionResult> Info(VendorInfoDto model, IFormFile uploadedFile, IFormCollection form)
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == null || !_vendorSettings.AllowVendorsToEditInfo)
            return Error(); // RedirectToRoute("CustomerInfo");

        Picture picture = null;

        if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        {
            try
            {
                var contentType = uploadedFile.ContentType.ToLowerInvariant();

                if (!contentType.StartsWith("image/") || contentType.StartsWith("image/svg"))
                    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.VendorInfo.Picture.ErrorMessage"));
                else
                {
                    var vendorPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);
                    picture = await _pictureService.InsertPictureAsync(vendorPictureBinary, contentType, null);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.VendorInfo.Picture.ErrorMessage"));
            }
        }

        var prevPicture = await _pictureService.GetPictureByIdAsync(vendor.PictureId);

        //vendor attributes
        var vendorAttributesXml = await ParseVendorAttributesAsync(form);
        var warnings = (await _vendorAttributeParser.GetAttributeWarningsAsync(vendorAttributesXml)).ToList();
        foreach (var warning in warnings)
        {
            ModelState.AddModelError(string.Empty, warning);
        }

        if (ModelState.IsValid)
        {
            var description = _htmlFormatter.FormatText(model.Description, false, false, true, false, false, false);

            vendor.Name = model.Name;
            vendor.Email = model.Email;
            vendor.Description = WebUtility.HtmlEncode(description);

            if (picture != null)
            {
                vendor.PictureId = picture.Id;

                if (prevPicture != null)
                    await _pictureService.DeletePictureAsync(prevPicture);
            }

            //update picture seo file name
            await UpdatePictureSeoNamesAsync(vendor);

            await _vendorService.UpdateVendorAsync(vendor);

            //save vendor attributes
            await _genericAttributeService.SaveAttributeAsync(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

            //notifications
            if (_vendorSettings.NotifyStoreOwnerAboutVendorInformationChange)
                await _workflowMessageService.SendVendorInformationChangeStoreOwnerNotificationAsync(vendor, _localizationSettings.DefaultAdminLanguageId);

            return RedirectToAction("Info");
        }

        //If we got this far, something failed, redisplay form
        model = await _vendorModelFactory.PrepareVendorInfoDtoAsync(model, true, vendorAttributesXml);
        return Ok(model);
    }


    [FormValueRequired("remove-picture")]
    [HttpDelete]
    [Route("RemovePicture", Name = "RemovePicture")]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public virtual async Task<IActionResult> RemovePicture()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var vendor = await _workContext.GetCurrentVendorAsync();
        if (vendor == null || !_vendorSettings.AllowVendorsToEditInfo)
            return Error(); // RedirectToRoute("CustomerInfo");

        var picture = await _pictureService.GetPictureByIdAsync(vendor.PictureId);

        if (picture != null)
            await _pictureService.DeletePictureAsync(picture);

        vendor.PictureId = 0;
        await _vendorService.UpdateVendorAsync(vendor);

        //notifications
        if (_vendorSettings.NotifyStoreOwnerAboutVendorInformationChange)
            await _workflowMessageService.SendVendorInformationChangeStoreOwnerNotificationAsync(vendor, _localizationSettings.DefaultAdminLanguageId);

        return Ok(); // RedirectToAction("Info");
    }

    #endregion

    #region Components



    #endregion
}
