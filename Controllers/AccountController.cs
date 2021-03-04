using System;
using iAttend.Models;
using iAttend.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text.RegularExpressions;

namespace iAttend.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        /*
        must include one of each: 0-9, a-z, A-Z, !@#$%&?
        must not include: ;"
        must be 8-32 characters
        */
        private const string passwordRegex = "((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%&?])(?!.*[;\"]).{8,32})";

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password) {
            /*
                Account account = get account with email

                if (doesPasswordMatchSaved(password, account.passwordHash, account.salt))
                {
                    sign in user
                }
                else
                {
                    return Json(can't sign in);
                }
            */
            return Json("test");
        }

        [HttpGet]
        public IActionResult Register(string email = null)
        {
            return View(new RegisterViewModel { email = email, passwordRegex = passwordRegex });
        }

        [HttpPost]
        public IActionResult Register(string email, string firstName, string nickname, string lastName, int expGradMonth, int expGradYear, string password, string confirmPassword, Guid trackId)
        {
            if (String.IsNullOrEmpty(email))
            {
                return Json( new { success = false, message = "Your email is required." } );
            }
            else if (!email.ToLower().EndsWith("@mountunion.edu"))
            {
                return Json( new { success = false, message = "You must use an email ending with @mountunion.edu." } );
            }
            else if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName))
            {
                return Json( new { success = false, message = "Your first and last name are required." } );
            }
            else if (firstName.Length > 30 || nickname.Length > 30 || lastName.Length > 30)
            {
                return Json( new { success = false, message = "Your first name, nickname, and last name must be less than 30 characters." } );
            }
            else if (!doesPasswordMeetRequirements(password))
            {
                return Json( new { success = false, message = "Your password does not meet the complexity requirements." } );
            }
            else if (!password.Equals(confirmPassword))
            {
                return Json( new { success = false, message = "Your passwords do not match." } );
            }

            try
            {
                long id = Int64.Parse(DateTime.UtcNow.ToString("yyMMddhhmmffff"));
                
                Tuple<byte[], string> passwordTuple = getHashedPassword(password);
                byte[] salt = passwordTuple.Item1;
                string passwordHash = passwordTuple.Item2;

                DateTime expGradDate = new DateTime(expGradYear, expGradMonth, DateTime.DaysInMonth(expGradYear, expGradMonth));

                Account account = new Account(id, firstName, String.IsNullOrEmpty(nickname) ? null : nickname, lastName, email.ToLower(), salt, passwordHash, expGradDate, trackId);

                // submit to database

                // send email

                // change to page saying email was sent
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                return Json( new { success = false, message = "An error occurred processing your request." } );
            }
        }

        private Tuple<byte[], string> getHashedPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return new Tuple<byte[], string>(salt, hashed);
        }

        private bool doesPasswordMatchSaved(string inputPassword, string savedPassword, byte[] salt)
        {
            string hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputPassword.ToLower(),
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedInput == savedPassword;
        }

        private bool doesPasswordMeetRequirements(string password)
        {
            return Regex.Match(password, passwordRegex, RegexOptions.IgnoreCase).Success;
        }
    }
}