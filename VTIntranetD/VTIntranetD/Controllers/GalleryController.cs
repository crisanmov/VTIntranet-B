using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Helpers;
using VTIntranetD.Models.Dto;
using NLog;

namespace VTIntranetD.Controllers
{
    public class GalleryController : Controller
    {
        private SessionModel model;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [SessionTimeOut]
        // GET: Gallery
        public ActionResult Index()
        {
            model = (SessionModel)this.Session["SessionData"];

            EventHelper eh = new EventHelper();
            var serializerEvent = new JavaScriptSerializer();
            var serializedResultE = serializerEvent.Serialize(eh.GetAllEvent());

            ViewBag.events = serializedResultE;
            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        // GET: Gallery for ID}
        [SessionTimeOut]
        [HttpGet]
        public ActionResult Album(int idEvent)
        {
            model = (SessionModel)this.Session["SessionData"];

            EventHelper eh = new EventHelper();
            MultimediaHelper mh = new MultimediaHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(mh.GetImages(idEvent));

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.images = serializedResult;

            model = (SessionModel)this.Session["SessionData"];
            ViewBag.events = eh.GetAllEvent();
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetPortrait(String idAlbum)
        {
            MultimediaHelper mh = new MultimediaHelper();
            var Portrait = mh.GetAlbumPortriat(Int32.Parse(idAlbum));

            if(Portrait == null)
            {
                logger.Error("Error al intentar crear el album en la db. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msgError ="No se encontro portada del album."}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data = Portrait, msg = "Portada con idAlbum " + idAlbum }, JsonRequestBehavior.AllowGet);
            }
            
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