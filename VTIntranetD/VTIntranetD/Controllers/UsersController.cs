using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Entities;
using VTIntranetD.Models.Helpers;
using VTIntranetD.Models.Dto;
using System.Data.Entity.Infrastructure;
using NLog;

namespace VTIntranetD.Controllers
{
    public class UsersController : Controller
    {
        private SessionModel model;
        private UserDataModel db = new UserDataModel();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [SessionTimeOut]
        // GET: Users
        public ActionResult Index()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.rolName = model.RolName;
            ViewBag.UserName = model.UserName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);
            return View(db.User.ToList());
        }

        [SessionTimeOut]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [SessionTimeOut]
        // GET: Users/Create
        public ActionResult Create()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.rolName = model.RolName;
            ViewBag.UserName = model.UserName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionTimeOut]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(User user, String nameProfile, List<Depto> deptosD, String rolName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Hash PASSWORD
                    user.password = Hash(user.password);
                    db.User.Add(user);
                    db.SaveChanges();

                    UserHelper uh = new UserHelper();
                    int idUser = uh.GetUser();

                    Profile p = new Profile
                    {
                        name = nameProfile,
                        rolName = rolName,
                        profileActive = user.userActive.Equals("1"),
                        idUser = idUser,
                    };
                    db.Profile.Add(p);
                    db.SaveChanges();

                    var profile = db.Profile.Where(a => a.idUser.Equals(p.idUser)).FirstOrDefault();
                    int idProfile = Convert.ToInt32(profile.idProfile);

                    for (int i = 0; i < deptosD.Count; i++)
                    {
                        ProfileTagDepto ptd = new ProfileTagDepto
                        {
                            idProfile = idProfile,
                            idTag = int.Parse(deptosD[i].idTag),
                            idParent = deptosD[i].idParent,
                            idDepto = deptosD[i].idDepto,
                            active = true,
                        };

                        db.ProfileTagDepto.Add(ptd);
                        db.SaveChanges();
                    }
                 
                }
                catch (DbUpdateException e)
                {
                    string errMsg = e.Message;
                    logger.Error(errMsg + Environment.NewLine + DateTime.Now);
                }

                model = (SessionModel)this.Session["SessionData"];
                logger.Info("Nuevo usuario creado en VTIntranet for username: " + model.UserName + Environment.NewLine + DateTime.Now);

                return Json(new { success = true, msg = "El usuario " + nameProfile + " se creo correctamente." });
            }
            else
            {
                logger.Error("Intento fallido al crear nuevo usuario. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msgError = "El usuario no se pudo crear correctamente." });
            }
        }

        [SessionTimeOut]
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionTimeOut]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idUser,username,password,name,lastNameP,lastNameM,userActive,skype")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [SessionTimeOut]
        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [SessionTimeOut]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            Profile pr = db.Profile.Where(p => p.idUser.Equals(id)).FirstOrDefault();
            DeptoHelper dh = new DeptoHelper();

            int res = dh.DeleteProfileDepto(pr.idProfile);
            if (res.Equals(1))
            {
                db.Profile.Remove(pr);
                db.User.Remove(user);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetAreas(String idDepto)
        {
            model = (SessionModel)this.Session["SessionData"];

            DeptoHelper dh = new DeptoHelper();
            var Areas = dh.GetArea(int.Parse(idDepto), Convert.ToInt32(model.ProfileID));

            if(Areas == null)
            {
                logger.Info("Areas no disponibles para crear nuevo usuario." + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, data = Areas, msgError = "Areas No Disponibles para este Depto." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data = Areas }, JsonRequestBehavior.AllowGet);
            } 
        }

        public static string Hash(string input)
        {
            var base64EncodedHash = Crypto.HashPassword(input);
            return base64EncodedHash.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private String SerializerNavBar(string idProfile)
        {
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var sR = serializer.Serialize(th.getTagsDeptos(int.Parse(idProfile)));

            return sR;
        }
    }
}
