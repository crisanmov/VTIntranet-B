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


namespace VTIntranetD.Controllers
{
    public class UsersController : Controller
    {
        private UserDataModel db = new UserDataModel();

        // GET: Users
        public ActionResult Index()
        {
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();
            return View(db.User.ToList());
        }

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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(User user, String nameProfile, List<Depto> deptosD, String rolName)
        {
            if (ModelState.IsValid)
            {
                //Hash PASSWORD
                user.password = Hash(user.password);
                db.User.Add(user);
                db.SaveChanges();

                UserHelper uh = new UserHelper();
                int idUser = uh.GetUser();

                Profile p = new Profile {
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
                    ProfileTagDepto ptd = new ProfileTagDepto {
                        idProfile = idProfile,
                        idTag = int.Parse(deptosD[i].idTag),
                        idParent = deptosD[i].idParent,
                        idDepto = deptosD[i].idDepto,
                        active = true,
                    };

                    db.ProfileTagDepto.Add(ptd);
                    db.SaveChanges();
                }

                return Json("successfully");
            }
            else
            {
                return Json("Error");
            }
        }

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

        [HttpGet]
        public JsonResult GetAreas(String idDepto)
        {
            DeptoHelper dh = new DeptoHelper();
            var Areas = dh.GetArea(int.Parse(idDepto), Convert.ToInt32(Session["ProfileID"]));

            return Json(Areas, JsonRequestBehavior.AllowGet);
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

        private String SerializerNavBar()
        {
            var idProfile = Session["ProfileID"].ToString();
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var sR = serializer.Serialize(th.getTagsDeptos(int.Parse(idProfile)));

            return sR;
        }
    }
}
