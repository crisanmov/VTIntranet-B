using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Helpers;

namespace VTIntranetD.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            EventHelper eh = new EventHelper();
            var serializerEvent = new JavaScriptSerializer();
            var serializedResultE = serializerEvent.Serialize(eh.GetAllEvent());

            ViewBag.events = serializedResultE;
            ViewBag.UserName = this.Session["userName"];
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        // GET: Gallery for ID}
        [HttpGet]
        public ActionResult Album(int idEvent)
        {
            EventHelper eh = new EventHelper();
            MultimediaHelper mh = new MultimediaHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(mh.GetImages(idEvent));

            ViewBag.images = serializedResult;
            ViewBag.events = eh.GetAllEvent();
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        [HttpGet]
        public JsonResult getPortrait(String idAlbum)
        {
            MultimediaHelper mh = new MultimediaHelper();
            var Portrait = mh.GetAlbumPortriat(Int32.Parse(idAlbum));

            return Json(Portrait, JsonRequestBehavior.AllowGet);
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