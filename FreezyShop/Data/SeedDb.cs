using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace FreezyShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private Random _random;
        public SeedDb(DataContext context, IUserHelper userHelper,IProductSizeRepository productSizeRepository, ICategoryRepository categoryRepository,IPromoCodeRepository promoCodeRepository, IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _productSizeRepository = productSizeRepository;
            _categoryRepository = categoryRepository;
            _promoCodeRepository = promoCodeRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Client");

            //Add User Admin when create db
            var userAdmin = await _userHelper.GetUserByEmailAsync("adminFreezy@gmail.com");
            if (userAdmin == null)
            {
                userAdmin = new User
                {
                    Email = "adminFreezy@gmail.com",
                    UserName = "adminFreezy@gmail.com",
                    Password = "admin01freezy",
                    FirstName = "Admin",
                    LastName = "01",
                    NickName = "Admin01"
                };
                var result1 = await _userHelper.AddUserAsync(userAdmin, "admin01freezy");
                if (result1 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user Admin in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userAdmin);

                await _userHelper.ConfirmEmailAsync(userAdmin, token);
            }
            var isInRole = await _userHelper.IsUserInRoleAsync(userAdmin, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");
            }
            //Add User Employee when create db
            var userEmployee = await _userHelper.GetUserByEmailAsync("EmployeeFreezy@gmail.com");
            if (userEmployee == null)
            {
                userEmployee = new User
                {
                    Email = "EmployeeFreezy@gmail.com",
                    UserName = "EmployeeFreezy@gmail.com",
                    Password = "E123456",
                    FirstName = "Employee",
                    LastName = "01",
                    NickName = "Employee01"
                };
                var result2 = await _userHelper.AddUserAsync(userEmployee, "E123456");
                if (result2 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user Employee in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userEmployee, "Employee");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userEmployee);

                await _userHelper.ConfirmEmailAsync(userEmployee, token);
            }
            var isInRole2 = await _userHelper.IsUserInRoleAsync(userEmployee, "Employee");
            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(userEmployee, "Employee");
            }
            //Add User Client when create db
            var userClient = await _userHelper.GetUserByEmailAsync("ClientFreezy@gmail.com");
            if (userClient == null)
            {
                userClient = new User
                {
                    Email = "ClientFreezy@gmail.com",
                    UserName = "ClientFreezy@gmail.com",
                    Password = "C123456",
                    FirstName = "Client",
                    LastName = "01",
                    NickName = "Client01"
                };
                var result3 = await _userHelper.AddUserAsync(userClient, "C123456");
                if (result3 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user Client in seeder");
                }
                await _userHelper.AddUserToRoleAsync(userClient, "Client");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userClient);

                await _userHelper.ConfirmEmailAsync(userClient, token);
            }
            var isInRole3 = await _userHelper.IsUserInRoleAsync(userClient, "Client");
            if (!isInRole3)
            {
                await _userHelper.AddUserToRoleAsync(userClient, "Client");
            }
            await AddCategories();
            await AddProducts();
            await AddProductCategories();
            await AddPromoCode();
            await AddSizesProducts();
        }

        private async Task AddSizesProducts()
        {
            var existProductsizes = await _productSizeRepository.GetByIdAsync(1);

            if (existProductsizes == null)
            {
                ProductSizes productSizes1 = new ProductSizes
                {
                    ProductId = 1,
                    Size ="M"
                };
                ProductSizes productSizes2 = new ProductSizes
                {
                    ProductId = 1,
                    Size = "S"
                };
                ProductSizes productSizes3 = new ProductSizes
                {
                    ProductId = 1,
                    Size = "XL"
                };
                ProductSizes productSizes4 = new ProductSizes
                {
                    ProductId = 2,
                    Size = "L"
                };
                ProductSizes productSizes5 = new ProductSizes
                {
                    ProductId = 2,
                    Size = "XS"
                };
                ProductSizes productSizes6 = new ProductSizes
                {
                    ProductId = 2,
                    Size = "XXL"
                };
                //
                ProductSizes productSizes7 = new ProductSizes
                {
                    ProductId = 3,
                    Size = "L"
                };
                ProductSizes productSizes8 = new ProductSizes
                {
                    ProductId = 3,
                    Size = "XS"
                };
                ProductSizes productSizes9 = new ProductSizes
                {
                    ProductId = 3,
                    Size = "XXL"
                };
                //
                ProductSizes productSizes10 = new ProductSizes
                {
                    ProductId = 4,
                    Size = "XL"
                };
                ProductSizes productSizes11 = new ProductSizes
                {
                    ProductId = 4,
                    Size = "XXL"
                };
                ProductSizes productSizes12 = new ProductSizes
                {
                    ProductId = 4,
                    Size = "L"
                };
                //
                ProductSizes productSizes13 = new ProductSizes
                {
                    ProductId = 5,
                    Size = "L"
                };
                ProductSizes productSizes14 = new ProductSizes
                {
                    ProductId = 5,
                    Size = "S"
                };
                ProductSizes productSizes15 = new ProductSizes
                {
                    ProductId = 5,
                    Size = "M"
                };
                //
                ProductSizes productSizes16 = new ProductSizes
                {
                    ProductId = 6,
                    Size = "XL"
                };
                ProductSizes productSizes17 = new ProductSizes
                {
                    ProductId = 6,
                    Size = "M"
                };
                ProductSizes productSizes18 = new ProductSizes
                {
                    ProductId = 6,
                    Size = "L"
                };
                //
                ProductSizes productSizes19 = new ProductSizes
                {
                    ProductId = 7,
                    Size = "L"
                };
                ProductSizes productSizes20 = new ProductSizes
                {
                    ProductId = 7,
                    Size = "S"
                };
                ProductSizes productSizes21 = new ProductSizes
                {
                    ProductId = 7,
                    Size = "M"
                };
                //
                ProductSizes productSizes22 = new ProductSizes
                {
                    ProductId = 8,
                    Size = "L"
                };
                ProductSizes productSizes23 = new ProductSizes
                {
                    ProductId = 8,
                    Size = "S"
                };
                ProductSizes productSizes24 = new ProductSizes
                {
                    ProductId = 8,
                    Size = "M"
                };
                //
                ProductSizes productSizes25 = new ProductSizes
                {
                    ProductId = 9,
                    Size = "L"
                };
                ProductSizes productSizes26 = new ProductSizes
                {
                    ProductId = 9,
                    Size = "S"
                };
                //
                
                ProductSizes productSizes29 = new ProductSizes
                {
                    ProductId = 10,
                    Size = "XL"
                };
                ProductSizes productSizes30 = new ProductSizes
                {
                    ProductId = 10,
                    Size = "XXL"
                };
                //
                ProductSizes productSizes31 = new ProductSizes
                {
                    ProductId = 11,
                    Size = "S"
                };
                ProductSizes productSizes32 = new ProductSizes
                {
                    ProductId = 11,
                    Size = "L"
                };
                //
                ProductSizes productSizes33 = new ProductSizes
                {
                    ProductId = 12,
                    Size = "XS"
                };
                ProductSizes productSizes34 = new ProductSizes
                {
                    ProductId = 12,
                    Size = "L"
                };
                //
                ProductSizes productSizes35 = new ProductSizes
                {
                    ProductId = 13,
                    Size = "S"
                };
                ProductSizes productSizes36 = new ProductSizes
                {
                    ProductId = 13,
                    Size = "XL"
                };
                //
                ProductSizes productSizes37 = new ProductSizes
                {
                    ProductId = 14,
                    Size = "XS"
                };
                ProductSizes productSizes38 = new ProductSizes
                {
                    ProductId = 14,
                    Size = "L"
                };
                //
                ProductSizes productSizes39 = new ProductSizes
                {
                    ProductId = 15,
                    Size = "XS"
                };
                ProductSizes productSizes40 = new ProductSizes
                {
                    ProductId = 15,
                    Size = "L"
                };
                //
                ProductSizes productSizes41 = new ProductSizes
                {
                    ProductId = 16,
                    Size = "XL"
                };
                ProductSizes productSizes42 = new ProductSizes
                {
                    ProductId = 16,
                    Size = "L"
                };
                //
                ProductSizes productSizes43 = new ProductSizes
                {
                    ProductId = 17,
                    Size = "XXL"
                };
                ProductSizes productSizes44 = new ProductSizes
                {
                    ProductId = 17,
                    Size = "S"
                };

                try
                {
                    await _productSizeRepository.CreateAsync(productSizes1);
                    await _productSizeRepository.CreateAsync(productSizes2);
                    await _productSizeRepository.CreateAsync(productSizes3);
                    await _productSizeRepository.CreateAsync(productSizes4);
                    await _productSizeRepository.CreateAsync(productSizes5);
                    await _productSizeRepository.CreateAsync(productSizes6);
                    await _productSizeRepository.CreateAsync(productSizes7);
                    await _productSizeRepository.CreateAsync(productSizes8);
                    await _productSizeRepository.CreateAsync(productSizes9);
                    await _productSizeRepository.CreateAsync(productSizes10);
                    await _productSizeRepository.CreateAsync(productSizes11);
                    await _productSizeRepository.CreateAsync(productSizes12);
                    await _productSizeRepository.CreateAsync(productSizes13);
                    await _productSizeRepository.CreateAsync(productSizes14);
                    await _productSizeRepository.CreateAsync(productSizes15);
                    await _productSizeRepository.CreateAsync(productSizes16);
                    await _productSizeRepository.CreateAsync(productSizes17);
                    await _productSizeRepository.CreateAsync(productSizes18);
                    await _productSizeRepository.CreateAsync(productSizes19);
                    await _productSizeRepository.CreateAsync(productSizes20);
                    await _productSizeRepository.CreateAsync(productSizes21);
                    await _productSizeRepository.CreateAsync(productSizes22);
                    await _productSizeRepository.CreateAsync(productSizes23);
                    await _productSizeRepository.CreateAsync(productSizes24);
                    await _productSizeRepository.CreateAsync(productSizes25);
                    await _productSizeRepository.CreateAsync(productSizes26);
                    await _productSizeRepository.CreateAsync(productSizes29);
                    await _productSizeRepository.CreateAsync(productSizes30);
                    await _productSizeRepository.CreateAsync(productSizes32);
                    await _productSizeRepository.CreateAsync(productSizes33);
                    await _productSizeRepository.CreateAsync(productSizes34);
                    await _productSizeRepository.CreateAsync(productSizes35);
                    await _productSizeRepository.CreateAsync(productSizes36);
                    await _productSizeRepository.CreateAsync(productSizes37);
                    await _productSizeRepository.CreateAsync(productSizes38);
                    await _productSizeRepository.CreateAsync(productSizes39);
                    await _productSizeRepository.CreateAsync(productSizes40);
                    await _productSizeRepository.CreateAsync(productSizes41);
                    await _productSizeRepository.CreateAsync(productSizes42);
                    await _productSizeRepository.CreateAsync(productSizes43);
                    await _productSizeRepository.CreateAsync(productSizes44);
                        
                        


                }
                catch
                {

                }
            }
        }
        private async Task AddPromoCode()
        {
            var existPromocode = await _promoCodeRepository.GetByIdAsync(1);

            if (existPromocode == null)
            {
                PromoCode promoCode = new PromoCode
                {
                    Code = "Freezy10",
                    Percentagem = 10
                };
                try
                {
                    await _promoCodeRepository.UpdateAsync(promoCode);
                }
                catch
                {

                }
            }
        }

                private async Task AddProductCategories()
        {
            var existProductcategory = await _productCategoryRepository.GetByIdAsync(1);

            if(existProductcategory == null)
            {

                //CATEGORY PRODUCT - Jeans
                ProductCategory productCategory1 = new ProductCategory
                {
                    CategoryId = 1,
                    ProductId = 4
                };
                //CATEGORY PRODUCT - Vest
                ProductCategory productCategory2 = new ProductCategory
                {
                    CategoryId = 10,
                    ProductId =17
                };
                //CATEGORY PRODUCT - Hoodie
                ProductCategory productCategory3 = new ProductCategory
                {
                    CategoryId = 11,
                    ProductId = 6
                };
                //CATEGORY PRODUCT - Sweatshirt
                ProductCategory productCategory4 = new ProductCategory
                {
                    CategoryId = 2,
                    ProductId = 1
                };
                ProductCategory productCategory5 = new ProductCategory
                {
                    CategoryId = 2,
                    ProductId = 2
                };
                ProductCategory productCategory6= new ProductCategory
                {
                    CategoryId = 2,
                    ProductId = 5
                };
                ProductCategory productCategory7 = new ProductCategory
                {
                    CategoryId = 2,
                    ProductId = 16
                };
                //CATEGORY PRODUCT - T-shirts

            
                ProductCategory productCategory9 = new ProductCategory
                {
                    CategoryId = 3,
                    ProductId = 8
                };
                ProductCategory productCategory10 = new ProductCategory
                {
                    CategoryId = 3,
                    ProductId = 9
                };
                ProductCategory productCategory11 = new ProductCategory
                {
                    CategoryId = 3,
                    ProductId = 13
                };
                ProductCategory productCategory12 = new ProductCategory
                {
                    CategoryId = 3,
                    ProductId = 14
                };
                //Category product - top
                ProductCategory productCategory13 = new ProductCategory
                {
                    CategoryId = 4,
                    ProductId = 11
                };
                //Category product - jumpsuits
                ProductCategory productCategory14 = new ProductCategory
                {
                    CategoryId = 5,
                    ProductId = 10
                };
                //Category product - Winter
                ProductCategory productCategory15 = new ProductCategory
                {
                    CategoryId = 6,
                    ProductId = 1
                };
                ProductCategory productCategory16 = new ProductCategory
                {
                    CategoryId = 6,
                    ProductId = 5
                };
                ProductCategory productCategory17 = new ProductCategory
                {
                    CategoryId = 6,
                    ProductId = 16
                };
                ProductCategory productCategory18 = new ProductCategory
                {
                    CategoryId = 6,
                    ProductId = 6
                };
                ProductCategory productCategory25 = new ProductCategory
                {
                    CategoryId = 6,
                    ProductId = 7
                };
                //Category product - Summer
                ProductCategory productCategory19 = new ProductCategory
                {
                    CategoryId = 7,
                    ProductId = 8
                };
                ProductCategory productCategory20 = new ProductCategory
                {
                    CategoryId = 7,
                    ProductId = 9
                };
                ProductCategory productCategory21 = new ProductCategory
                {
                    CategoryId = 7,
                    ProductId = 13
                };
                ProductCategory productCategory22 = new ProductCategory
                {
                    CategoryId = 7,
                    ProductId = 14
                };
                ProductCategory productCategory23 = new ProductCategory
                {
                    CategoryId = 7,
                    ProductId = 11
                };
                //Category product - jacket
                ProductCategory productCategory24 = new ProductCategory
                {
                    CategoryId = 8,
                    ProductId = 7
                };

                //Category product - joggers
                ProductCategory productCategory26 = new ProductCategory
                {
                    CategoryId = 9,
                    ProductId = 3
                }; 
                ProductCategory productCategory27 = new ProductCategory
                {
                    CategoryId = 9,
                    ProductId = 12
                }; 
                ProductCategory productCategory28 = new ProductCategory
                {
                    CategoryId = 9,
                    ProductId = 15
                };
                
                try
                {
                    await _productCategoryRepository.CreateAsync(productCategory1);
                    await _productCategoryRepository.CreateAsync(productCategory2);
                    await _productCategoryRepository.CreateAsync(productCategory3);
                    await _productCategoryRepository.CreateAsync(productCategory4);



                    await _productCategoryRepository.CreateAsync(productCategory27);
                    await _productCategoryRepository.CreateAsync(productCategory12);
                    await _productCategoryRepository.CreateAsync(productCategory13);
                    await _productCategoryRepository.CreateAsync(productCategory14);
                    await _productCategoryRepository.CreateAsync(productCategory15);
                    await _productCategoryRepository.CreateAsync(productCategory16);
                    await _productCategoryRepository.CreateAsync(productCategory17);
                    await _productCategoryRepository.CreateAsync(productCategory18);
                    await _productCategoryRepository.CreateAsync(productCategory19);
                    await _productCategoryRepository.CreateAsync(productCategory20);
                    await _productCategoryRepository.CreateAsync(productCategory21);
                    await _productCategoryRepository.CreateAsync(productCategory22);
                    await _productCategoryRepository.CreateAsync(productCategory23);
                    await _productCategoryRepository.CreateAsync(productCategory24);
                    await _productCategoryRepository.CreateAsync(productCategory25);
                    await _productCategoryRepository.CreateAsync(productCategory26);
                    
                    await _productCategoryRepository.CreateAsync(productCategory28);
                    await _productCategoryRepository.CreateAsync(productCategory5);
                    await _productCategoryRepository.CreateAsync(productCategory6);
                    await _productCategoryRepository.CreateAsync(productCategory7);
                    await _productCategoryRepository.CreateAsync(productCategory10);
                    await _productCategoryRepository.CreateAsync(productCategory11);
                    
                }
                catch
                {

                }
            }

        }

        private async Task AddCategories()
        {
            var existcategory = await _categoryRepository.GetByIdAsync(1);
            if (existcategory == null)
            {
                Category category1 = new Category
                {
                    Name = "Jeans",
                    CreatedOn = DateTime.Now
                };
                Category category11 = new Category
                {
                    Name = "Hoodie",
                    CreatedOn = DateTime.Now
                };
                Category category2 = new Category
                {
                    Name = "SweatShirt",
                    CreatedOn = DateTime.Now
                };
                Category category3 = new Category
                {
                    Name = "T-shirt",
                    CreatedOn = DateTime.Now
                };
                Category category4 = new Category
                {
                    Name = "Top",
                    CreatedOn = DateTime.Now
                };
                Category category5 = new Category
                {
                    Name = "Jumpsuits",
                    CreatedOn = DateTime.Now
                };
                Category category6 = new Category
                {
                    Name = "Winter",
                    CreatedOn = DateTime.Now
                };
                Category category7 = new Category
                {
                    Name = "Summer",
                    CreatedOn = DateTime.Now
                };
                Category category8 = new Category
                {
                    Name = "Jacket",
                    CreatedOn = DateTime.Now
                };
                Category category9 = new Category
                {
                    Name = "Jogers",
                    CreatedOn = DateTime.Now
                };
                Category category10 = new Category
                {
                    Name = "Vest",
                    CreatedOn = DateTime.Now
                };
              
                try
                {
                    await _categoryRepository.CreateAsync(category1);
                    await _categoryRepository.CreateAsync(category2);
                    await _categoryRepository.CreateAsync(category3);
                    await _categoryRepository.CreateAsync(category4);
                    await _categoryRepository.CreateAsync(category5);
                    await _categoryRepository.CreateAsync(category6);
                    await _categoryRepository.CreateAsync(category7);
                    await _categoryRepository.CreateAsync(category8);
                    await _categoryRepository.CreateAsync(category9);
                    await _categoryRepository.CreateAsync(category10);
                    await _categoryRepository.CreateAsync(category11);
                }
                catch
                {

                }


            }
        }
        private async Task AddProducts()
        {
            var existproducts = await _productRepository.GetByIdAsync(1);
            if (existproducts == null)
            {
                Product product1 = new Product
                {
                    Color = "Grey",
                    ImageUrl1 = new Guid("882D6F40-ECB4-4005-8378-106E94A465E0"),
                    ImageUrl2 = new Guid("5A28654E-F896-4182-AFA0-1BE565F9EB03"),
                    Description = "Sweatshirt in Particle Grey. Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",
                    Gender = "Both",
                    Name = "Grey SweatShirt",
                    Price = 30.99M,
                    Quantity = 10,
                    Accessed = 6,
                    Ordered = 0

                };
                Product product2 = new Product
                {
                    Color = "Blue",
                    ImageUrl1 = new Guid("1CBE1433-C00C-4C54-94BC-715349BFEEB0"),
                    ImageUrl2 = new Guid("B8C575BB-075B-4AF3-BDF1-0A099A599885"),
                    Description = "Sweatshirt in Blue. Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",
                    Gender = "Both",
                    Name = "Blue SweatShirt",
                    Price = 30.99M,
                    Quantity = 10,
                    Accessed = 3,
                    Ordered = 0
                    

                };
                Product product3 = new Product
                {
                    Color = "Blue",
                    ImageUrl1 = new Guid("D150D8F8-4085-429E-9792-846FA8EFE47F"),
                    ImageUrl2 = new Guid("C9367AE7-6D7C-434B-9348-08D04A50C881"),
                    Description = "The great outdoors has never looked so good, or at least the people in it haven’t. Get kitted out with this latest piece from Monterrain, both lightweight and durable and crafted from comfortable stretchy polyester fabric that won’t let you down whilst out on the trails.",
                    Gender = "Both",
                    Name = "Blue Joggers",
                    Price = 30.99M,
                    Quantity = 10,
                    Accessed = 3,
                    Ordered = 0


                };
                Product product4 = new Product
                {
                    Color = "Green",
                    ImageUrl1 = new Guid("A4488BE3-720F-4A50-88E9-251B044D7B8F"),
                    ImageUrl2 = new Guid("B7BB8097-2AA9-46D5-99E4-6F10C5D54AB9"),
                    Description = "Pants in Gorge Green. Providing cosy warmth without the added weight, these soft fleece fabric joggers feature a slim adjustable drawstring waistband, relaxed standard fitting. ",
                    Gender = "Female",
                    Name = "Green Jeans",
                    Price = 26.99M,
                    Quantity = 13,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product5 = new Product
                {
                    Color = "Beige",
                    ImageUrl1 = new Guid("E2617869-937A-4B5A-99F9-9AA1FA881324"),
                    ImageUrl2 = new Guid("CCDCFE95-B85A-46C2-A31C-EBCBB0B728E1"),
                    Description = "Sweatshirt in Beige. Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",
               
                    Gender = "Both",
                    Name = "Beige SweatShirt",
                    Price = 30.99M,
                    Quantity = 10,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product6 = new Product
                {
                    Color = "Beige",
                    ImageUrl1 = new Guid("CCDCFE95-B85A-46C2-A31C-EBCBB0B728E1"),
                    ImageUrl2 = new Guid("2A2F4E8F-6303-4F6B-AAA6-1828CE267EDC"),
                    Description = "Hoodie in Beige. Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",

                    Gender = "Both",
                    Name = "Beige Hoodie",
                    Price = 39.99M,
                    Quantity = 10,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product7 = new Product
                {
                    Color = "Red",
                    ImageUrl1 = new Guid("7004802C-2FCD-40FB-BE94-67EFB205AF37"),
                    ImageUrl2 = new Guid("0C0D6B05-21E8-4824-BAFC-C18208506798"),
                    Description = "Red Jacket . Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",
                    Gender = "Both",
                    Name = "Red Jacket",
                    Price = 39.99M,
                    Quantity = 14,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product8 = new Product
                {
                    Color = "Blue",
                    ImageUrl1 = new Guid("87B604B2-CEB7-42F8-9442-B8751D73658D"),
                    ImageUrl2 = new Guid("E6C3D1FE-5DEA-403D-AE4A-0042D8662937"),
                    Description = "Blue T.shirt .Join the Club as Freeze add a selection of their classic regular  t-shirts to bolster your sport inspired casual wear. With a soft cotton fabrication and swoosh embroidered chest.",

                    Gender = "Male",
                    Name = "Blue T-shirt",
                    Price = 29.99M,
                    Quantity = 11,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product9 = new Product
                {
                    Color = "Pink",
                    ImageUrl1 = new Guid("E5A0A1EF-E541-4028-BBA2-39E3C3F79084"),
                    ImageUrl2 = new Guid("77D167C8-FD9B-4D4F-AAEA-058623B48402"),
                    Description = "Pink T.shirt .Join the Club as Freeze add a selection of their classic regular  t-shirts to bolster your sport inspired casual wear. With a soft cotton fabrication and swoosh embroidered chest.",

                    Gender = "Female",
                    Name = "Pink T-shirt",
                    Price = 25.99M,
                    Quantity = 16,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product10 = new Product
                {
                    Color = "Blue",
                    ImageUrl1 = new Guid("59850A67-3B8A-4D6C-8E2F-B7DD8A619881"),
                    ImageUrl2 = new Guid("7FA2C306-37E7-47B7-AD6D-F361A0090C82"),
                    Description = "Jumpsuits Jeans. Providing cosy warmth without the added weight, these soft fleece fabric jumpsuits feature a slim adjustable drawstring waistband, relaxed standard fitting. ",

                    Gender = "Male",
                    Name = "Blue Jumpsuits",
                    Price = 25.99M,
                    Quantity = 16,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product11 = new Product
                {
                    Color = "Beige",
                    ImageUrl1 = new Guid("F777A746-7A4A-41AE-9A7C-948DC3C11659"),
                    ImageUrl2 = new Guid("3DC5AB00-263A-4024-86B6-EB7FABA8ABA5"),
                    Description = "Beige Top .Join the Club as Freeze add a selection of their classic regular tops to bolster your sport inspired casual wear. With a soft cotton fabrication and swoosh embroidered chest.",

                    Gender = "Female",
                    Name = "Beige Top",
                    Price = 20.99M,
                    Quantity = 11,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product12 = new Product
                {
                    Color = "Black",
                    ImageUrl1 = new Guid("CA42E7A3-9C06-4333-86E2-ED981165DBFE"),
                    ImageUrl2 = new Guid("0B815247-E68C-4DEC-9FEE-5E2CFBD0BC75"),
                    Description = "Joggers in Black. Providing cosy warmth without the added weight, these soft fleece fabric joggers feature a slim adjustable drawstring waistband, relaxed standard fitting. ",

                    Gender = "Male",
                    Name = "Black Joggers",
                    Price = 27.99M,
                    Quantity = 6,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product13 = new Product
                {
                    Color = "Yellow",
                    ImageUrl1 = new Guid("E2FB8531-5587-4FC9-A76D-4EBB196D785A"),
                    ImageUrl2 = new Guid("A6AF3BC8-9400-4100-8005-B635EB0B421A"),
                    Description = "Yellow T-shirt .Join the Club as Freeze add a selection of their classic regular  t-shirts to bolster your sport inspired casual wear. With a soft cotton fabrication and swoosh embroidered chest.",

                    Gender = "Both",
                    Name = "Yellow T-shirt",
                    Price = 22.99M,
                    Quantity = 9,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product14 = new Product
                {
                    Color = "White",
                    ImageUrl1 = new Guid("8480F1BF-2F9E-4009-91E0-3CF1E12F8159"),
                    ImageUrl2 = new Guid("83A4F728-992F-4AD7-AAB2-FE69556CBC9F"),
                    Description = "White T-shirt .Join the Club as Freeze add a selection of their classic regular  t-shirts to bolster your sport inspired casual wear. With a soft cotton fabrication and swoosh embroidered chest.",

                    Gender = "Both",
                    Name = "White T-shirt",
                    Price = 22.99M,
                    Quantity = 9,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product15 = new Product
                {
                    Color = "Black",
                    ImageUrl1 = new Guid("DC55D519-63CA-4DA3-B020-ED1D384AF3F5"),
                    ImageUrl2 = new Guid("A0B848E4-40A5-437C-AFCA-0C8DD40B408B"),
                    Description = "Joggers in Black. Providing cosy warmth without the added weight, these soft fleece fabric joggers feature a slim adjustable drawstring waistband, relaxed standard fitting. ",
                    Gender = "Female",
                    Name = "Black Joggers",
                    Price = 22.99M,
                    Quantity = 11,
                    Accessed = 1,
                    Ordered = 0


                };
                Product product16 = new Product
                {
                    Color = "Yellow",
                    ImageUrl1 = new Guid("8C7CE1E5-AB32-44C5-A2B8-A22FD23F81AA"),
                    ImageUrl2 = new Guid("F8ED8292-50BF-4CA5-AA1E-23332A8E8F92"),
                    Description = "Sweatshirt in Yellow. Back to basics and sometimes simplicity is best, this sweatshirt from Freezy is perfect for the colder weather with its simple style and quality build.",
                    Gender = "Both",
                    Name = "Yellow SweatShirt",
                    Price = 30.99M,
                    Quantity = 8,
                    Accessed = 4,
                    Ordered = 0

                };
                Product product17 = new Product
                {
                    Color = "Black",
                    ImageUrl1 = new Guid("0EB3BB63-BEA9-47F5-B4D8-FB51319607C3"),
                    ImageUrl2 = new Guid("7B7BDA8C-F213-4A43-B2C9-C7C100884DF0"),
                    Description = "Vest in Black. Back to basics and sometimes simplicity is best, this Vest from Freezy is perfect for the colder weather with its simple style and quality build.",
                    Gender = "Both",
                    Name = "Black Vest",
                    Price = 23.99M,
                    Quantity = 19,
                    Accessed = 3,
                    Ordered = 0

                };
                try
                {
                    await _productRepository.CreateAsync(product1);
                    await _productRepository.CreateAsync(product2);
                    await _productRepository.CreateAsync(product3);
                    await _productRepository.CreateAsync(product4);
                    await _productRepository.CreateAsync(product5);
                    await _productRepository.CreateAsync(product6);
                    await _productRepository.CreateAsync(product7);
                    await _productRepository.CreateAsync(product8);
                    await _productRepository.CreateAsync(product9);
                    await _productRepository.CreateAsync(product10);
                    await _productRepository.CreateAsync(product11);
                    await _productRepository.CreateAsync(product12);
                    await _productRepository.CreateAsync(product13);
                    await _productRepository.CreateAsync(product14);
                    await _productRepository.CreateAsync(product15);
                    await _productRepository.CreateAsync(product16);
                    await _productRepository.CreateAsync(product17);
                }
                catch
                {

                }
            }
        }
    }
}

