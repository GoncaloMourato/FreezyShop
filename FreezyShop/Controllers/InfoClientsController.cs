using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;

namespace FreezyShop.Controllers
{
    public class InfoClientsController : Controller
    {
        private readonly DataContext _context;
        private readonly IInfoClientRepository _infoClientRepository;
        private readonly IShippingInfoRepository _shippingInfoRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IUserHelper _userHelper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClientPreferencesRepository _clientPreferencesRepository;
        private readonly IBlobHelper _blobHelper;

        public InfoClientsController(DataContext context, IInfoClientRepository infoClientRepository,IShippingInfoRepository shippingInfoRepository,IPaymentMethodRepository paymentMethodRepository,ISizeRepository sizeRepository, IUserHelper userHelper,ICategoryRepository categoryRepository,IClientPreferencesRepository clientPreferencesRepository,IBlobHelper blobHelper)
        {
            _context = context;
            _infoClientRepository = infoClientRepository;
            _shippingInfoRepository = shippingInfoRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _sizeRepository = sizeRepository;
            _userHelper = userHelper;
            _categoryRepository = categoryRepository;
            _clientPreferencesRepository = clientPreferencesRepository;
            _blobHelper = blobHelper;
        }

        // GET: InfoClients
        public async Task<IActionResult> Profile()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var infoClient =  await _infoClientRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefaultAsync() ;
            var infoshipping = await _shippingInfoRepository.GetAll().Where(P => P.UserId == user.Id).FirstOrDefaultAsync();
            var infoPaymentMethod =await _paymentMethodRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefaultAsync();
            var infopreferences =  await _clientPreferencesRepository.GetAll().Where(p => p.UserId == user.Id).Select(p => p.Preferences).ToListAsync();
            var model = new ProfileViewModel();
            if (infoClient == null )
            {

             
                return RedirectToAction("CreateProfile");


            }
            if(infoshipping != null)
            {
                 model = new ProfileViewModel
                {
                    Id = infoClient.Id,
                    FullName = infoClient.FullName,
                    GenderClient = infoClient.GenderClient,
                    ImageUrl = infoClient.ImageUrl,
                    Imageurl1 = infoClient.ImageFullPath1,
                    User = user,
                    UserId = user.Id,
                    PhoneNumber = infoClient.PhoneNumber,
                    Address = infoshipping.Address,
                    District = infoshipping.District,
                    RecepientName = infoshipping.RecepientName,
                    PostalCode = infoshipping.PostalCode,
                    CardNumber = infoPaymentMethod.CardNumber,
                    ExperyDate = infoPaymentMethod.ExperyDate,
                    NameOnCard = infoPaymentMethod.NameOnCard,
                    CVV = infoPaymentMethod.CVV,
                    ClientPreferences = infopreferences.ToArray(),
                    Preferences = await _categoryRepository.GetAll().Select(P => P.Name).ToArrayAsync()
                };
            }else{
                 model = new ProfileViewModel
                {
                    Id = infoClient.Id,
                    FullName = infoClient.FullName,
                    GenderClient = infoClient.GenderClient,
                    ImageUrl = infoClient.ImageUrl,
                     Imageurl1 = infoClient.ImageFullPath1,
                     User = user,
                    UserId = user.Id,
                    PhoneNumber = infoClient.PhoneNumber,
                    Address = infoshipping.Address,
                    District = infoshipping.District,
                    RecepientName = infoshipping.RecepientName,
                    PostalCode = infoshipping.PostalCode,
                    CardNumber = infoPaymentMethod.CardNumber,
                    ExperyDate = infoPaymentMethod.ExperyDate,
                    NameOnCard = infoPaymentMethod.NameOnCard,
                    CVV = infoPaymentMethod.CVV,
                    Preferences = await _categoryRepository.GetAll().Select(P => P.Name).ToArrayAsync()
                };

            } 
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var InfoClient =await _infoClientRepository.GetByIdAsync(model.Id);
                    var infoshipping = _shippingInfoRepository.GetAll().Where(P => P.UserId == model.UserId).FirstOrDefault();
                    var infoPaymentMethod = _paymentMethodRepository.GetAll().Where(p => p.UserId == model.UserId).FirstOrDefault();
                    Guid imageId = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }
                    else
                    {
                        imageId = model.ImageUrl;
                    }


                    InfoClient.FullName = model.FullName;
                    InfoClient.GenderClient = model.GenderClient;
                    InfoClient.ImageUrl = imageId;
                    InfoClient.PhoneNumber = model.PhoneNumber;
                    InfoClient.GenderClient = model.GenderClient;

                    infoshipping.District = model.District;
                    infoshipping.PostalCode = model.PostalCode;
                    infoshipping.Address = model.Address;
                    infoshipping.PhoneNumber = model.PhoneNumber;
                    infoshipping.RecepientName = model.RecepientName;
                    
                    infoPaymentMethod.NameOnCard = model.NameOnCard;
                    infoPaymentMethod.CardNumber = model.CardNumber;
                    infoPaymentMethod.CVV = model.CVV;
                    infoPaymentMethod.ExperyDate = model.ExperyDate;
                   
                    

                    await _infoClientRepository.UpdateAsync(InfoClient);
                    await _shippingInfoRepository.UpdateAsync(infoshipping);
                    await _paymentMethodRepository.UpdateAsync(infoPaymentMethod);

                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();

                }
                return RedirectToAction("Profile");
            }
            return View(model);

        }
   
        public async Task<IActionResult> CreateProfile()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ProfileViewModel();
            var userpreferences = await  _clientPreferencesRepository.GetAll().Where(p=>p.UserId == user.Id).Select(p=>p.Preferences).ToListAsync();
            if (userpreferences != null)
            {
                model.ClientPreferences = userpreferences.ToArray();
            }
            model.Preferences = await _categoryRepository.GetAll().Select(P => P.Name).ToArrayAsync();
            model.UserId = user.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(ProfileViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                var infoshipping = _shippingInfoRepository.GetAll().Where(P => P.UserId == user.Id).FirstOrDefault();
                var infoPaymentMethod = _paymentMethodRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefault();

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }
                var InfoClient = new InfoClient
                {
                    User = user ,
                    FullName = model.FullName,
                    GenderClient = model.GenderClient,
                    ImageUrl = imageId,
                    UserId = user.Id,
                    PhoneNumber = model.PhoneNumber
                     
                };
               await _infoClientRepository.CreateAsync(InfoClient);

              
                if (model.RecepientName != null && infoshipping == null)
                {
                    var shipping = new ShippingInfo
                    {
                        Address = model.Address,
                        District = model.District,
                        PhoneNumber= model.PhoneNumber,
                        PostalCode = model.PostalCode,
                        RecepientName = model.RecepientName,
                        UserId = user.Id,
                        User = user
                    };

                    await _shippingInfoRepository.CreateAsync(shipping);
                }

                if(model.NameOnCard != null && infoPaymentMethod == null)
                {
                    var payment = new PaymentMethod
                    {
                        CardNumber = model.CardNumber,
                        CVV = model.CVV,
                        ExperyDate = model.ExperyDate,
                        NameOnCard = model.NameOnCard,
                        UserId = user.Id,
                        User = user
                    };

                    await _paymentMethodRepository.CreateAsync(payment);
                }

                return RedirectToAction("Profile");
            }
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> AddPreferences(ProfileViewModel model)
        {
            var user =await _userHelper.GetUserByIdAsync(model.UserId);
            var userpreferences = _context.ClientPreferences.Where(p => p.UserId == user.Id).ToList();
            if(user == null)
            {
                //errorview 
            }
             if(model.ClientPreferences == null)
            {
                //errorview
            }
             if(userpreferences != null)
            {
                foreach(var item in userpreferences)
                {
                    await _clientPreferencesRepository.DeleteAsync(item);
                }
            }
             foreach(var item in model.ClientPreferences)
            {
                var clientepreferences = new ClientPreference
                {
                    Preferences = item,
                    UserId = user.Id

                };

                try
                {
                    await _clientPreferencesRepository.CreateAsync(clientepreferences);
                    //flashmessage
                }
                catch
                {
                    //flashmessage
                }
            }
             
             return RedirectToAction("Profile");
        
        }
        public async Task<IActionResult> SizeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var genderuser = _infoClientRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefault().GenderClient;
            var sizeinfo = _sizeRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefault();
            SizeViewModel model = new SizeViewModel();
            if(sizeinfo == null)
            {
                model = new SizeViewModel
                {
                    Genderuser = genderuser
                };
            }
            else
            {
                if(genderuser == "Male"){
                  model = new SizeViewModel
                    {
                        WaistMan = sizeinfo.WaistMan,
                        BreastMan = sizeinfo.BreastMan,
                        Genderuser = genderuser,
                        UserId = sizeinfo.UserId,
                        ResultSize = sizeinfo.ResultSize
                    };
                }
                else
                {
                     model = new SizeViewModel
                    {
                       WaistWoman = sizeinfo.WaistWoman,
                       BreastWoman = sizeinfo.BreastWoman,
                       Hip = sizeinfo.Hip,
                        Genderuser = genderuser,
                        UserId = sizeinfo.UserId,
                         ResultSize = sizeinfo.ResultSize
                     };
                }

                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SizeUser(SizeViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var genderuser = _infoClientRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefault().GenderClient;
            var sizeinfo = _sizeRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefault();
            if(genderuser == null)
            {
                return RedirectToAction("Profile");
            }
            if(sizeinfo == null)
            {
                if (genderuser == "Female")
                {
                    var size = CalculateSizeWoman(model.Hip, model.WaistWoman, model.BreastWoman);
                    var newsize = new Size
                    {
                        Hip = model.Hip,
                        WaistWoman = model.WaistWoman,
                        BreastWoman = model.BreastWoman,
                        ResultSize = model.ResultSize,
                        UserId = user.Id
                    };
                    await _sizeRepository.CreateAsync(newsize);
                }
                else
                {
                    var size = CalculateSizeMan(model.BreastMan, model.WaistMan);

                    var newsize = new Size
                    {
                        BreastMan = model.BreastMan,
                        WaistMan = model.WaistMan,
                        ResultSize = size,
                        UserId = user.Id
                    };
                    await _sizeRepository.CreateAsync(newsize);
                }

            }
            else
            {
                if (genderuser == "Female")
                {
                    var size = CalculateSizeWoman(model.Hip, model.WaistWoman, model.BreastWoman);

                    sizeinfo.Hip = model.Hip;
                    sizeinfo.WaistWoman = model.WaistWoman;
                    sizeinfo.BreastWoman = model.BreastWoman;
                    sizeinfo.ResultSize = size;
                  
                    await _sizeRepository.UpdateAsync(sizeinfo);
                }
                else
                {
                    var size = CalculateSizeMan(model.BreastMan, model.WaistMan);


                    sizeinfo.BreastMan = model.BreastMan;
                        sizeinfo.WaistMan = model.WaistMan;
                    sizeinfo.ResultSize = size;
                   
                    await _sizeRepository.UpdateAsync(sizeinfo);
                }
            }

            return RedirectToAction("SizeUser");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoClient = await _context.InfoClients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (infoClient == null)
            {
                return NotFound();
            }

            return View(infoClient);
        }

        // POST: InfoClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var infoClient = await _context.InfoClients.FindAsync(id);
            _context.InfoClients.Remove(infoClient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private static string CalculateSizeWoman(int hip, int waist, int breast)
        {

            string tamanho = null;
            //simple cases
            if(hip >= 86 && hip <= 89 || waist >= 58 && waist <=61 || breast >= 80 && breast <=81 )
            {
                tamanho = "XS";
            }
            if (hip >= 90 && hip <= 93||waist >= 63 && waist <= 65 || breast >= 82 && breast <= 85)
            {
                tamanho = "S";
            }
             if (hip >= 94 && hip <= 97 || waist >= 66 && waist <= 69 ||  breast >= 86 && breast <= 89)
            {
                tamanho = "M";
            }
            if (hip >= 98 && hip <= 103 || waist >= 70 &&  waist <= 75 || breast >= 90 && breast <= 95)
            {
                tamanho = "L";
            }
            if (hip >= 104 && hip <= 109 ||  waist >= 76 && waist <= 82 || breast >= 96 && breast <= 101)
            {
                tamanho = "XL";
            }
            if (hip >= 110 && hip <= 116 || waist >= 82 && waist <= 90 || breast >= 102 && breast <= 108)
            {
                tamanho = "XXL";
            }

            return tamanho;
        }
        private static string CalculateSizeMan(int breast, int waist)
        {
            string tamanho = null;
            if(breast >= 86 && breast <=91 || waist >= 72 && waist <= 74)
            {
                tamanho = "XS";
            }
            if (breast >= 91 && breast <= 96 || waist >= 75 && waist <= 80)
            {
                tamanho = "S";
            }
            if (breast >= 97 && breast <= 101 || waist >= 81 && waist <= 88)
            {
                tamanho = "M";
            }
            if (breast >= 102 && breast <= 106 || waist >= 89 && waist <= 96)
            {
                tamanho = "L";
            }
            if (breast >= 106 && breast <= 111 || waist >= 97 && waist <= 104)
            {
                tamanho = "XL";
            }
            if (breast >= 112 && breast <= 116 || waist >= 105 && waist <= 112)
            {
                tamanho = "XXL";
            }
            return tamanho;
        }

    }
}
