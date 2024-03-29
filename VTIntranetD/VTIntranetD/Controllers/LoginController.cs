﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using VTIntranetD.Models.Entities;
using VTIntranetD.Models.Dto;
using NLog;


using Microsoft.Owin.Security.Cookies;

namespace VTIntranetD.Controllers
{
    public class LoginController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                     
                    
                    var obj = db.User.Where(a => a.username.Equals(user.username)).FirstOrDefault();
                    var pass1 = obj.password;
                    var pass2 = user.password;

                    var validatePass = Crypto.VerifyHashedPassword(pass1, pass2);
                    if (obj != null && validatePass)
                    {
                        var profile = db.Profile.Where(p => p.idUser.Equals(obj.idUser)).FirstOrDefault();
                        /*Session["UserID"] = obj.idUser.ToString();
                        Session["UserName"] = obj.username.ToString();
                        Session["ProfileID"] = profile.idProfile.ToString();
                        Session["rolName"] = profile.rolName.ToString();*/

                        SessionModel sm = new SessionModel()
                        {
                            UserID = obj.idUser.ToString(),
                            UserName = obj.username.ToString(),
                            ProfileID = profile.idProfile.ToString(),
                            RolName = profile.rolName.ToString(),
                            UserActive = true
                        };

                        Session["SessionData"] = sm;

                        //Write in Log
                        LogManager.Configuration.Variables["userid"] = sm.UserID;
                        LogManager.Configuration.Variables["username"] = sm.UserName;
                        logger.Info("Initialize Session VTIntranet with username: " + sm.UserName + Environment.NewLine + DateTime.Now);


                        return RedirectToAction("Index", "Home");
                    }
                    
                }
            }
            
            logger.Error("Attempted to login but credentials are incorrect " + Environment.NewLine + DateTime.Now);
            return View(user);
            
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}