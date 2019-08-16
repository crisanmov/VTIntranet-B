using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Helpers;
using VTIntranetD.Models.Dto;

namespace VTIntranetD.Controllers
{
    public class GalleryController : Controller
    {
        private SessionModel model;

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

            ViewBag.rolName = model.RolName;
            ViewBag.images = serializedResult;
            ViewBag.events = eh.GetAllEvent();
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult getPortrait(String idAlbum)
        {
            MultimediaHelper mh = new MultimediaHelper();
            var Portrait = mh.GetAlbumPortriat(Int32.Parse(idAlbum));

            return Json(Portrait, JsonRequestBehavior.AllowGet);
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