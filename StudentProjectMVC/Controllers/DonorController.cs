using Business_Logic_Layer.JwtToken;
using Business_Logic_Layer.Models;
using Data_Access_Layer.Entities;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;
using Business_Logic_Layer.Roles;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StudentProjectMVC.Controllers
{
    [Authorize]
    public class DonorController : Controller
    {
        private readonly Business_Logic_Layer.DonorBLL _business_Logic_Layer;
        public DonorController(Business_Logic_Layer.DonorBLL business_Logic_Layer)
        {
            _business_Logic_Layer = business_Logic_Layer;
        }
        public IActionResult Edit(int id)
        {
            var donor = _business_Logic_Layer.GetDonorById(id);
            if (donor != null)
            {
                return View("UpdateDonor", donor);
            }
            else
            {
                ViewData["IdError"] = "Donor Not Found!";
                return View("UpdateDonor");
            }
        }
        public IActionResult EditRecord(int id, string age, string name, string bloodType, string phoneNumber, string emailAddress, string address)
        {
            var donorModel = _business_Logic_Layer.GetDonorById(id);
            if (donorModel != null)
            {
                string message = _business_Logic_Layer.EditRecord(id, name, age, bloodType, phoneNumber, emailAddress, address, donorModel);
                if (message == "Donor Updated Successfully!")
                {
                    ViewData["SuccessMessage"] = message;
                }
                else
                {
                    ViewData["EmailExist"] = message;
                }
            }
            else
            {
                ViewData["Error"] = "Something went wrong. Go back to list and try again!";
            }
            var donor = new DonorModel
            {
                DonorId = id,
                DonorName = name,
                DonorAge = Convert.ToInt32(age),
                BloodType = bloodType,
                ContactNumber = phoneNumber,
                EmailAddress = emailAddress,
                Address = address
            };
            return View("UpdateDonor", donor);
        }
        public IActionResult UpdateRecord(int donorId, DonorModel donorModel)
        {
            if (ModelState.IsValid)
            {
                return EditRecord(donorId, donorModel.DonorAge.ToString(), donorModel.DonorName, donorModel.BloodType, donorModel.ContactNumber, donorModel.EmailAddress, donorModel.Address);
            }
            else
            {
                if (HttpContext.Request.Method == "POST")
                {
                    if (ModelState.ContainsKey("DonorAge") && ModelState["DonorAge"].Errors.Count > 0)
                    {
                        ViewData["AgeError"] = ModelState["DonorAge"].Errors[0].ErrorMessage;
                    }
                    if (ModelState.ContainsKey("EmailAddress") && ModelState["EmailAddress"].Errors.Count > 0)
                    {
                        ViewData["EmailError"] = ModelState["EmailAddress"].Errors[0].ErrorMessage;
                    }
                }
                return View("UpdateDonor", donorModel);
            }
        }
        public IActionResult DeleteRecord(int id)
        {
            var donor = _business_Logic_Layer.GetDonorById(id);
            if (donor != null)
            {
                _business_Logic_Layer.DeleteRecord(donor);
                return RedirectToAction("DonorsRecord");
            }
            return View("DonorsRecord", "Some Error Occur Please Try Again");
        }
        public List<DonorModel> GetDonors(string search)
        {
            ViewBag.Search = search;
            List<DonorModel> donor = _business_Logic_Layer.GetDonors(search);
            if (donor != null)
            {
                return donor;
            }
            else
            {
                return new List<DonorModel>();
            }
        }
        public IActionResult AddRecord(string age, string name, string bloodtype, string phoneNumber, string emailAddress, string address)
        {
            string message = _business_Logic_Layer.AddRecord(age, name, bloodtype, phoneNumber, emailAddress, address);
            if (message == "Donor Added Successfully!")
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                ViewData["EmailExistMessage"] = message;
                var donor = new DonorModel
                {
                    DonorName = name,
                    DonorAge = Convert.ToInt32(age),
                    BloodType = bloodtype,
                    ContactNumber = phoneNumber,
                    EmailAddress = emailAddress,
                    Address = address
                };
                return View("AddDonor", donor);
            }
            return View("AddDonor");
        }
        public IActionResult Delete(int id)
        {
            return DeleteRecord(id);
        }
        [HttpGet]
        public IActionResult AddDonor()
        {
            return View("AddDonor");
        }
        [HttpPost]
        public IActionResult AddDonor(DonorModel donorModel)
        {
            if (ModelState.IsValid)
            {
                return AddRecord(donorModel.DonorAge.ToString(), donorModel.DonorName, donorModel.BloodType, donorModel.ContactNumber, donorModel.EmailAddress, donorModel.Address);
            }
            else
            {
                if (HttpContext.Request.Method == "POST")
                {
                    if (ModelState.ContainsKey("DonorAge") && ModelState["DonorAge"].Errors.Count > 0)
                    {
                        ViewData["AgeError"] = ModelState["DonorAge"].Errors[0].ErrorMessage;
                    }
                    if (ModelState.ContainsKey("EmailAddress") && ModelState["EmailAddress"].Errors.Count > 0)
                    {
                        ViewData["EmailError"] = ModelState["EmailAddress"].Errors[0].ErrorMessage;
                    }
                }
                return View(donorModel);
            }
        }
        public IActionResult DonorsRecord(int? page, string search)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var donorsRecord = GetDonors(search);
            if (donorsRecord.Count > 0)
            {
                return View(donorsRecord.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View("DonorsRecord", new List<DonorModel>());
            }
        }
        public void Dispose()
        {
            _business_Logic_Layer.Dispose();
        }
        //////////////////////////////////////////////////
        ///////////////////////
        //  -------- SIGN IN and SIGN UP Function

        public IActionResult LogOut()
        {
            //return View("SignIn");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // return RedirectToAction("LogOut", "Donor");
            return View("LogOut");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LogOut()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    // Clear cookies and session data
        //    HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
        //    HttpContext.Session.Clear();

        //    // Redirect to the sign-in page
        //    return RedirectToAction("SignIn", "Donor");
        //}




       // [Authorize]
        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult UserSignIn(Credentials credential)
        {
            string message = _business_Logic_Layer.UserSignIn(credential);
            if (message == "Sign In Successfully!")
            {
                ViewData["SuccessMessage"] = message;

                // generate token
                //if (credential.UserName != null)
                {
                   
                    Dashboard(credential);
                    //return Json(new { Token = jwtToken });
                }


                return View("Dashboard");
            }
            else if (message == "Please Enter Valid User Name and Password")
            {
                ViewData["ErrorMessage"] = message;
            }
            else
            {
                if (message == "Some Issue Please Try Again")
                ViewData["Error"] = message;
            }
            return View("SignIn");
        }






        public IActionResult UserSignUp(string userName, string password)
        {
           string message = _business_Logic_Layer.UserSignUp(userName,password);
            if (message == "Account Created Successfully! Please Sign In")
            {
                ViewData["SuccessMessage"] = message;
            }
            else if (message == "User Name Already Exist")
            {
                ViewData["ErrorMessage"] = message;
            }
            else
            {
                ViewData["Error"] = message;
            }
           return View("SignUp");
        }
        [HttpGet]
        // [Authorize]
       [AllowAnonymous]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult SignIn()
        {
            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1
            //Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0
            //Response.Headers.Add("Expires", "0"); // Proxies
            return View(/*"SignIn"*/);
        }
        [HttpPost]
       // [Authorize]
        // [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult SignIn(Credentials credential)
        {

            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1
            //Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0
            //Response.Headers.Add("Expires", "0"); // Proxies
            return UserSignIn(credential);
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }
        [HttpPost]
        public IActionResult SignUp(Credentials credential)
        {   
            return UserSignUp(credential.UserName, credential.Password);
        }

       

        /// <summary>
        /// ///////////////////
        /// </summary>
        /// <param name="inventoryStore"></param>
        /// <returns></returns>
        public IActionResult AddInventory(BloodInventoryStore inventoryStore) 
        {

            string message = _business_Logic_Layer.AddInventory(inventoryStore.BloodTypeName, inventoryStore.Quantity);
            if (message == "Record Added Successfully!")
            {
                ViewData["SuccessMessage"] = message;
            }
            else
            {
                ViewData["Error"] = message;
            }
            return View("AddInventory");
        }
        [HttpGet]
        public IActionResult Inventory()
        {
            return View("AddInventory");
        }

        [HttpPost]
        public IActionResult Inventory(BloodInventoryStore inventoryStore) 
        {
            return AddInventory(inventoryStore);
        }

        ////
        ///
        //[HttpGet]
        //public IActionResult InventoryRecords()
        //{
        //    return View();
        //}
        //[HttpPost]
        public IActionResult InventoryRecords(int? page, string search)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var inventoryRecord = GetInventory(search);
            if (inventoryRecord.Count > 0)
            {
                return View(inventoryRecord.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View("InventoryRecords", new List<BloodInventoryStore>());
            }
        }

        ///////////////
        ///
        public List<BloodInventoryStore> GetInventory(string search)
        {
            ViewBag.Search = search;
            List<BloodInventoryStore> inventory = _business_Logic_Layer.GetInventory(search);
            if (inventory != null)
            {
                return inventory;
            }
            else
            {
                return new List<BloodInventoryStore>();
            }
        }

        ////////////////
        ////////////
        ///

       // [Authorize(Policy = "Admin")]
        public IActionResult DeleteInventory(int id)
        {
            var inventory = _business_Logic_Layer.GetInventoryById(id);
            if (inventory != null)
            {
                _business_Logic_Layer.DeleteInventory(inventory);
                return RedirectToAction("InventoryRecords");
            }
            return View("InventoryRecords", "Some Error Occur Please Try Again");
        }




        //////////////
        public IActionResult EditInventory(int id)
        {
            var inventory = _business_Logic_Layer.GetInventoryById(id);
            if (inventory != null)
            {
                return View("UpdateInventory", inventory);
            }
            else
            {
                ViewData["IdError"] = "Inventory Record Not Found!";
                return View("UpdateInventory");
            }
        }

        public IActionResult UpdateInventory(int id, BloodInventoryStore inventory)
        {
           
                return UpdateInventoryRecord(id, inventory.BloodTypeName, inventory.Quantity);
 
        }

        public IActionResult UpdateInventoryRecord(int id, string bloodTypeName, int quantity)
        {
            var inventoryRecord = _business_Logic_Layer.GetInventoryById(id);
            if (inventoryRecord != null)
            {
                string message = _business_Logic_Layer.UpdateInventoryRecord(bloodTypeName, quantity, inventoryRecord);
                if (message == "Record Updated Successfully!")
                {
                    ViewData["SuccessMessage"] = message;
                    return View("UpdateInventory");
                }
                else
                {
                    ViewData["ErrorMessage"] = message;
                }
            }
            else
            {
                ViewData["Error"] = "Something went wrong. Go back and try again!";
            }
            var inventory = new BloodInventoryStore
            {
                Id = id,
                BloodTypeName = bloodTypeName,
                Quantity = quantity
            };
            return View("UpdateInventory", inventory);
        }

        //////////////////////
        ///
        //  Show graph data

        //public IActionResult GetGraphData() 
        //{
        //   Dictionary<string, int> records = _business_Logic_Layer.GetGraphData();
        //    return View("Dashboard", records);
        //}

        public IActionResult Dashboard(Credentials credential)
        {
            var (donorCount, inventoryCount) = _business_Logic_Layer.GetGraphData();
            if (donorCount != null && inventoryCount != null)
            {
                ViewData["DonorCount"] = donorCount;
                ViewData["InventoryCount"] = inventoryCount;
            }
            var tokenService = new TokenService();
            var jwtToken = tokenService.GenerateJwtToken(credential.UserName);

            ViewData["Token"] = jwtToken;
            return View("Dashboard");
            //return GetGraphData();
            //return View();
        }

        /// <summary>
        /// /////////
        /// 
        /// check for jwt token validation 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Del()
        {
            // Retrieve the token from the request
            string token = HttpContext.Request.Form["token"];

            // Your code here for token validation
            if (string.IsNullOrWhiteSpace(token))
            {
                // Token is missing or empty
                ViewData["InValid"] = "This is invalid user";
                return View("ABC");
            }

                // Validate the token using the same validation parameters as on the client side
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Set to true if you want to validate the issuer
                    ValidateAudience = true, // Set to true if you want to validate the audience
                    ValidateIssuerSigningKey = true, // Set to true if you want to validate the signing key
                    ValidIssuer = "https://localhost:7124/", // Replace with your issuer
                    ValidAudience = "https://localhost:7124/", // Replace with your audience
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdhfhdhfkfhdsh798987bhkjahkhsfkjhtext")) // Replace with your secret key
                };

                // Validate the token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                // Check if the token is valid and not expired
                if (validatedToken != null && validatedToken.ValidTo > DateTime.UtcNow)
                {
                    Console.WriteLine("This is admin user");
                ViewData["Success"] = "This is admin";
                return View("ABC");
                }
                else
                {
                // Token is invalid or expired
                Console.WriteLine("access denied !!!!");
                ViewData["Error"] = "This is Not admin use";
                return View("ABC");
            }
            
        }


    }
}