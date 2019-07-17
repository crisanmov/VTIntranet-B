using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using VTIntranetD.Models.Entities;

namespace VTIntranetD.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                using (UserDataModel db = new UserDataModel())
                {
                     
                    try
                    {
                        var obj = db.User.Where(a => a.username.Equals(user.username)).FirstOrDefault();
                        var pass1 = obj.password;
                        var pass2 = user.password;

                        var validatePass = Crypto.VerifyHashedPassword(pass1, pass2);
                        if (obj != null && validatePass)
                        {
                            var profile = db.Profile.Where(p => p.idUser.Equals(obj.idUser)).FirstOrDefault();
                            Session["UserID"] = obj.idUser.ToString();
                            Session["UserName"] = obj.username.ToString();
                            Session["ProfileID"] = profile.idProfile.ToString();
                            Session["rolName"] = profile.rolName.ToString();
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch
                    {
                        return View(user);
                    }
                    
                    
                }
            }
            return View(user);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}