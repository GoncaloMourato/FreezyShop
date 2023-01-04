using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FreezyShop.Controllers
{
    public class ReturnController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IReturnRepository _returnRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;

        public ReturnController(IUserHelper userHelper, IReturnRepository returnRepository,IProductRepository productRepository, IBlobHelper blobHelper, IMailHelper mailHelper,DataContext dataContext)
        {
            _userHelper = userHelper;
            _returnRepository = returnRepository;
            _productRepository = productRepository;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
            _context = dataContext;
        }
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            
            return View(_returnRepository.GetAll().Include(p=>p.User).Where(p=>p.UserId == user.Id));
        }
        [Authorize(Roles = "Client")]
        public IActionResult Create(int id)
        {
            ReturnViewModel model = new ReturnViewModel
            {
                OrderDetailId = id
                
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReturnViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
               
                if (model.ImageFile != null && model.ImageFile.Length > 0 )
                {

                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "returns");
                    
                }
                Return returns = new Return
                {
                    Reason=  model.Reason,
                    Description= model.Description,
                    ImageUrl = imageId, 
                    Status = "Waiting for Confirmation",
                    UserId = user.Id,
                    OrderDetailId = model.OrderDetailId
                };
                try
                {
                    await _returnRepository.CreateAsync(returns);
                    var order = _context.OrderDetails.Include(p=>p.Product).Where(p => p.Id == returns.OrderDetailId).FirstOrDefault();
                    string tokenLink = Url.Action("ConfirmReturn", "Return", new
                    {
                        userid = user.Id,
                        returnid = returns.Id

                    }, protocol: HttpContext.Request.Scheme);

                    string emailbody = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' style='width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'><head><meta charset='UTF-8'><meta content='width=device-width, initial-scale=1' name='viewport'><meta name='x-apple-disable-message-reformatting'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta content='telephone=no' name='format-detection'><title>New message 2</title><link href='https://fonts.googleapis.com/css?family=Lato:400,400i,700,700i' rel='stylesheet'><style type='text/css'>#outlook a {	padding:0;}.ExternalClass {	width:100%;}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div {	line-height:100%;}.es-button {	mso-style-priority:100!important;	text-decoration:none!important;}a[x-apple-data-detectors] {	color:inherit!important;	text-decoration:none!important;	font-size:inherit!important;	font-family:inherit!important;	font-weight:inherit!important;	line-height:inherit!important;}.es-desk-hidden {	display:none;	float:left;	overflow:hidden;	width:0;	max-height:0;	line-height:0;	mso-hide:all;}[data-ogsb] .es-button {	border-width:0!important;	padding:15px 25px 15px 25px!important;}[data-ogsb] .es-button.es-button-1 {	padding:15px 30px!important;}@media only screen and (max-width:600px) {p, ul li, ol li, a { line-height:150%!important } h1, h2, h3, h1 a, h2 a, h3 a { line-height:120%!important } h1 { font-size:30px!important; text-align:center } h2 { font-size:26px!important; text-align:center } h3 { font-size:20px!important; text-align:center } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:30px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-menu td a { font-size:16px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:16px!important } .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:16px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class='gmail-fix'] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:block!important } a.es-button, button.es-button { font-size:20px!important; display:block!important; border-width:15px 25px 15px 25px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } .es-desk-hidden { display:table-row!important; width:auto!important; overflow:visible!important; max-height:inherit!important } }</style></head>"+
                        "<body style='width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;font-family:lato, helvetica neue, helvetica, arial, sans-serif;padding:0;Margin:0'><div class='es-wrapper-color' style='background-color:#F4F4F4'><table class='es-wrapper' width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#F4F4F4'><tr class='gmail-fix' height='0' style='border-collapse:collapse'><td style='padding:0;Margin:0'><table cellspacing='0' cellpadding='0' border='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:600px'><tr style='border-collapse:collapse'><td cellpadding='0' cellspacing='0' border='0' style='padding:0;Margin:0;line-height:1px;min-width:600px' height='0'><img src='https://zpqqin.stripocdn.email/content/guids/CABINET_837dc1d79e3a5eca5eb1609bfe9fd374/images/41521605538834349.png' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;max-height:0px;min-height:0px;min-width:600px;width:600px' alt width='600' height='1'></td>"+
                        "</tr></table></td></tr><tr style='border-collapse:collapse'><td valign='top' style='padding:0;Margin:0'><table cellpadding='0' cellspacing='0' class='es-content' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:15px;padding-bottom:15px'><table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;width:282px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td class='es-infoblock es-m-txt-c' align='left' style='padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, helvetica neue, helvetica, sans-serif;line-height:14px;color:#CCCCCC;font-size:12px'>FreezyShop<br></p>"+
                        "</td></tr></table></td></tr></table><table class='es-right' cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;width:278px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='right' class='es-infoblock es-m-txt-c' style='padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:14px;color:#CCCCCC;font-size:12px'><a href='https://viewstripo.email' class='view' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px;font-family:arial, helvetica neue, helvetica, sans-serif'></a></p>"+
                        "</td></tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-header' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:#FFA73B;background-repeat:repeat;background-position:center top'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-header-body' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:20px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:580px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:25px;padding-bottom:25px;font-size:0px'><img src='https://zpqqin.stripocdn.email/content/guids/CABINET_f706dbdd32fe88ce1f6295378d5c3eb1/images/ficon.png' alt style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic' width='70'></td>"+
                        "</tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;background-color:#bf85fa' bgcolor='#bf85fa' align='center'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;background-color:#ffffff;border-radius:4px' width='100%' cellspacing='0' cellpadding='0' bgcolor='#ffffff'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-bottom:5px;padding-left:30px;padding-right:30px;padding-top:35px'><h1 style='Margin:0;line-height:58px;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;font-size:48px;font-style:normal;font-weight:normal;color:#111111'>Return Requested!</h1>"+
                        "</td></tr><tr style='border-collapse:collapse'><td bgcolor='#ffffff' align='center' style='Margin:0;padding-top:5px;padding-bottom:5px;padding-left:20px;padding-right:20px;font-size:0'><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;border-bottom:1px solid #ffffff;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px'></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td>"+
                        "</tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;border-radius:4px;background-color:#ffffff' width='100%' cellspacing='0' cellpadding='0' bgcolor='#ffffff'><tr style='border-collapse:collapse'><td class='es-m-txt-l' bgcolor='#ffffff' align='left' style='Margin:0;padding-top:20px;padding-bottom:20px;padding-left:30px;padding-right:30px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>We received an request from you but first, you need to confirm it so we can have sure that it was you. Just press the button below.</p>"+
                        $"</td></tr><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:35px;padding-bottom:35px'><span class='msohide es-button-border' style='border-style:solid;border-color:#bf85fa;background:#bf85fa;border-width:1px;display:inline-block;border-radius:2px;width:auto;mso-hide:all'><a href='{tokenLink}' class='es-button es-button-1' target='_blank' style='mso-style-priority:100 !important;text-decoration:none;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;color:#FFFFFF;font-size:20px;border-style:solid;border-color:#bf85fa;border-width:15px 30px;display:inline-block;background:#bf85fa;border-radius:2px;font-family:helvetica, helvetica neue, arial, verdana, sans-serif;font-weight:normal;font-style:normal;line-height:24px;width:auto;text-align:center'> Confirm Return Request</a></span></td>" +
                        $"</tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='padding:0;Margin:0;padding-top:20px;padding-left:30px;padding-right:30px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>If that doesn't work, copy and paste the following link in your browser:</p></td></tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='padding:0;Margin:0;padding-top:20px;padding-left:30px;padding-right:30px'><a target='_blank' href='{tokenLink}' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#bf85fa;font-size:18px'>XXXXXXXXXXXXXXXXXXXXXXXXX</a></td>" +
                         "</tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='padding:0;Margin:0;padding-top:20px;padding-left:30px;padding-right:30px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>If you have any questions, just reply to this email—we're always happy to help out.</p></td></tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='Margin:0;padding-top:20px;padding-left:30px;padding-right:30px;padding-bottom:40px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>Cheers,</p>" +
                         "<p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>The Freezy Team</p></td></tr></table></td></tr></table></td></tr></table></td>" +
                         "</tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-top:10px;padding-bottom:20px;padding-left:20px;padding-right:20px;font-size:0'><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;border-bottom:1px solid #f4f4f4;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px'></td>" +
                         "</tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table><table cellpadding='0' cellspacing='0' class='es-footer' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-footer-body' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:540px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;padding-top:25px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:21px;color:#ffffff;font-size:14px'>You received this email because we received an return request from you. If it looks weird, <strong><a class='view' target='_blank' href='https://viewstripo.email' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#111111;font-size:14px'>view it in your browser</a></strong>.</p>" +
                         "</td></tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-left:20px;padding-right:20px;padding-top:30px;padding-bottom:30px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:560px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0;display:none'></td>" +
                         "</tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table></div></body></html>";
                    Response response = _mailHelper.SendEmail(user.Email, "Returned Requested",emailbody);
                    //\"{tokenLink}\"
                }
                catch
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound(); 
            }

            var returnedit = await _returnRepository.GetByIdAsync(id);
            if(returnedit == null)
            {
                return NotFound();
            }
            ReturnViewModel model = new ReturnViewModel
            {
              Id = returnedit.Id,
                Description = returnedit.Description,
              ImageUrl = returnedit.ImageUrl,
              Reason = returnedit.Reason,
              OrderDetailId = returnedit.OrderDetailId,
              Status = returnedit.Status,
              UserId = returnedit.UserId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReturnViewModel model)
        {
            if (ModelState.IsValid)
            {
                Return returnedit = await _returnRepository.GetByIdAsync(model.Id);
                try
                {
                    Guid imageId = Guid.Empty;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "returns");
                    }

                    returnedit.Description = model.Description;
                    returnedit.Reason= model.Reason;
                    returnedit.ImageUrl = imageId;
                    await _returnRepository.UpdateAsync(returnedit);
                    
                }
                catch
                {
                    //flassmessage
                }
             
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Return = await _returnRepository.GetByIdAsync(id);
            if (Return == null)
            {
                return NotFound();
            }

            await _returnRepository.DeleteAsync(Return);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmReturn(string userid,int returnid)
        {
            var returnconfirmation = await _returnRepository.GetByIdAsync(returnid);

            try
            {
                returnconfirmation.Status = "Confirmed";
                await _returnRepository.UpdateAsync(returnconfirmation);
            }
            catch
            {
                //flashnotification
            }
            return View();

        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ReturnsStaff()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var returns = _returnRepository.GetAll().Include(p => p.User).Include(p => p.OrderDetail).ThenInclude(p => p.Product).Where(p => p.Status != "Waiting for Confirmation");
            return View(returns);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ManageReturn(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var returnedit = await _returnRepository.GetByIdAsync(id);
            var orderdetail = await _context.OrderDetails.Where(p => p.Id == returnedit.OrderDetailId).FirstOrDefaultAsync();
            if (returnedit == null)
            {
                return NotFound();
            }
            ReturnViewModel model = new ReturnViewModel
            {
                Id = returnedit.Id,
                Description = returnedit.Description,
                ImageUrl = returnedit.ImageUrl,
                Reason = returnedit.Reason,
                OrderDetailId = returnedit.OrderDetailId,
                Status = returnedit.Status,
                UserId = returnedit.UserId,
                User = await _userHelper.GetUserByIdAsync(returnedit.UserId),
                Product =await _productRepository.GetByIdAsync(orderdetail.ProductId)
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageReturn(ReturnViewModel model)
        {
            var user =await  _userHelper.GetUserByIdAsync(model.UserId);
            var returninfo = await _returnRepository.GetAll().Include(p => p.User).Include(p => p.OrderDetail).ThenInclude(p => p.Product).Where(p => p.Id == model.Id).FirstOrDefaultAsync();

                Return returnedit = await _returnRepository.GetByIdAsync(model.Id);
                try
                {

                string emailmessage = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' style='width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'><head><meta charset='UTF-8'><meta content='width=device-width, initial-scale=1' name='viewport'><meta name='x-apple-disable-message-reformatting'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta content='telephone=no' name='format-detection'><title>New message 2</title><link href='https://fonts.googleapis.com/css?family=Lato:400,400i,700,700i' rel='stylesheet'><style type='text/css'>#outlook a {	padding:0;}.ExternalClass {	width:100%;}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div {	line-height:100%;}.es-button {	mso-style-priority:100!important;	text-decoration:none!important;}a[x-apple-data-detectors] {	color:inherit!important;	text-decoration:none!important;	font-size:inherit!important;	font-family:inherit!important;	font-weight:inherit!important;	line-height:inherit!important;}.es-desk-hidden {	display:none;	float:left;	overflow:hidden;	width:0;	max-height:0;	line-height:0;	mso-hide:all;}[data-ogsb] .es-button {	border-width:0!important;	padding:15px 25px 15px 25px!important;}[data-ogsb] .es-button.es-button-1 {	padding:15px 30px!important;}@media only screen and (max-width:600px) {p, ul li, ol li, a { line-height:150%!important } h1, h2, h3, h1 a, h2 a, h3 a { line-height:120%!important } h1 { font-size:30px!important; text-align:center } h2 { font-size:26px!important; text-align:center } h3 { font-size:20px!important; text-align:center } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:30px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-menu td a { font-size:16px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:16px!important } .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:16px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class='gmail-fix'] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:block!important } a.es-button, button.es-button { font-size:20px!important; display:block!important; border-width:15px 25px 15px 25px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } .es-desk-hidden { display:table-row!important; width:auto!important; overflow:visible!important; max-height:inherit!important } }</style></head>" +
                    "<body style='width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;font-family:lato, helvetica neue, helvetica, arial, sans-serif;padding:0;Margin:0'><div class='es-wrapper-color' style='background-color:#F4F4F4'><table class='es-wrapper' width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#F4F4F4'><tr class='gmail-fix' height='0' style='border-collapse:collapse'><td style='padding:0;Margin:0'><table cellspacing='0' cellpadding='0' border='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:600px'><tr style='border-collapse:collapse'><td cellpadding='0' cellspacing='0' border='0' style='padding:0;Margin:0;line-height:1px;min-width:600px' height='0'><img src='https://zpqqin.stripocdn.email/content/guids/CABINET_837dc1d79e3a5eca5eb1609bfe9fd374/images/41521605538834349.png' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;max-height:0px;min-height:0px;min-width:600px;width:600px' alt width='600' height='1'></td>" +
                    "</tr></table></td></tr><tr style='border-collapse:collapse'><td valign='top' style='padding:0;Margin:0'><table cellpadding='0' cellspacing='0' class='es-content' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:15px;padding-bottom:15px'><table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;width:282px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td class='es-infoblock es-m-txt-c' align='left' style='padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, helvetica neue, helvetica, sans-serif;line-height:14px;color:#CCCCCC;font-size:12px'>FreezyShop<br></p>" +
                    "</td></tr></table></td></tr></table><table class='es-right' cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;width:278px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='right' class='es-infoblock es-m-txt-c' style='padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:14px;color:#CCCCCC;font-size:12px'><a href='https://viewstripo.email' class='view' target='_blank' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px;font-family:arial, helvetica neue, helvetica, sans-serif'></a></p>" +
                    "</td></tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-header' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:#FFA73B;background-repeat:repeat;background-position:center top'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-header-body' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-bottom:10px;padding-left:10px;padding-right:10px;padding-top:20px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:580px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:25px;padding-bottom:25px;font-size:0px'><img src='https://zpqqin.stripocdn.email/content/guids/CABINET_f706dbdd32fe88ce1f6295378d5c3eb1/images/ficon.png' alt style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic' width='70'></td>" +
                    "</tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;background-color:#bf85fa' bgcolor='#bf85fa' align='center'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;background-color:#ffffff;border-radius:4px' width='100%' cellspacing='0' cellpadding='0' bgcolor='#ffffff'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-bottom:5px;padding-left:30px;padding-right:30px;padding-top:35px'><h1 style='Margin:0;line-height:58px;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;font-size:48px;font-style:normal;font-weight:normal;color:#111111'>Return Requested Status!</h1>" +
                    "</td></tr><tr style='border-collapse:collapse'><td bgcolor='#ffffff' align='center' style='Margin:0;padding-top:5px;padding-bottom:5px;padding-left:20px;padding-right:20px;font-size:0'><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;border-bottom:1px solid #ffffff;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px'></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td>" +
                    "</tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;border-radius:4px;background-color:#ffffff' width='100%' cellspacing='0' cellpadding='0' bgcolor='#ffffff'><tr style='border-collapse:collapse'><td class='es-m-txt-l' bgcolor='#ffffff' align='left' style='Margin:0;padding-top:20px;padding-bottom:20px;padding-left:30px;padding-right:30px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>Your request has been analized! Check out the informations:</p>" +
                    $"</td></tr><td align='left' class='esd-block-text es-p20t'><h2>The new  status about your request is:</h2></td><tr><td align='left' class='esd-block-text es-m-txt-l es-p10t'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>{model.Status}.</p></td></tr><td align='left' class='esd-block-text es-p20t'><h2>Adicional Information:</h2></td><tr><td align='left' class='esd-block-text es-m-txt-l es-p10t'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>{model.AdicionalInfEmail}.</p></td></tr>" +
                    "</tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='padding:0;Margin:0;padding-top:20px;padding-left:30px;padding-right:30px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'> </p></td></tr><tr style='border-collapse:collapse'><td class='es-m-txt-l' align='left' style='Margin:0;padding-top:20px;padding-left:30px;padding-right:30px;padding-bottom:40px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>Cheers,</p>"+
                    "<p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:27px;color:#666666;font-size:18px'>The Freezy Team</p></td></tr></table></td></tr></table></td></tr></table></td>"+
                    "</tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:600px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='Margin:0;padding-top:10px;padding-bottom:20px;padding-left:20px;padding-right:20px;font-size:0'><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td style='padding:0;Margin:0;border-bottom:1px solid #f4f4f4;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px'></td>"+
                    "</tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table><table cellpadding='0' cellspacing='0' class='es-footer' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-footer-body' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-top:30px;padding-bottom:30px;padding-left:30px;padding-right:30px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:540px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='left' style='padding:0;Margin:0;padding-top:25px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:lato, helvetica neue, helvetica, arial, sans-serif;line-height:21px;color:#ffffff;font-size:14px'>"+
                    "</td></tr></table></td></tr></table></td></tr></table></td></tr></table><table class='es-content' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'><tr style='border-collapse:collapse'><td align='center' bgcolor='#bf85fa' style='padding:0;Margin:0;background-color:#bf85fa'><table class='es-content-body' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px' cellspacing='0' cellpadding='0' align='center'><tr style='border-collapse:collapse'><td align='left' style='Margin:0;padding-left:20px;padding-right:20px;padding-top:30px;padding-bottom:30px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td valign='top' align='center' style='padding:0;Margin:0;width:560px'><table width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'><tr style='border-collapse:collapse'><td align='center' style='padding:0;Margin:0;display:none'></td>" +
                    "</tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table></div></body></html>";
                    returnedit.Status = model.Status;
                    await _returnRepository.UpdateAsync(returnedit);
                Response response = _mailHelper.SendEmail(user.Email, "Return Status",emailmessage);
            }
            catch
                {
                    //flassmessage
                }

            
            
            return RedirectToAction("ReturnsStaff");
        }
    }

}
