namespace ProjectX.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics.Eventing.Reader;
    using System.DirectoryServices.Protocols;
    using System.Net;

    public class AccountController : Controller
    {
        private readonly string _ldapServer = "ldap://example.com";
        private readonly string _ldapBaseDn = "OU=Users,DC=example,DC=com";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (IsValidUser(username, password))
            {
                // Успешная аутентификация
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Неудачная аутентификация
                ModelState.AddModelError("", "Неправильные учетные данные");
                return View();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            try
            {
                using (var ldapConnection = new LdapConnection(_ldapServer))
                {
                    ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    ldapConnection.Bind(new NetworkCredential(username, password));
                    return true;
                }
            }
            catch (LdapException)
            {
                return false;
            }
        }
    }
}
