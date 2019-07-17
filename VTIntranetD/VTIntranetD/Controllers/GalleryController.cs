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
            var idProfile = Session["ProfileID"].ToString();
            ViewBag.UserName = this.Session["userName"];
            ViewBag.rolName = this.Session["rolName"];
            //helpers models
            TagHelper th = new TagHelper();

            //serializers
            var serializer = new JavaScriptSerializer();
            var sNav = serializer.Serialize(th.getTagsDeptos(int.Parse(idProfile)));
            ViewBag.Navbar = sNav;

            //get all events
            EventHelper eh = new EventHelper();
            var serializerEvent = new JavaScriptSerializer();
            var serializedResultE = serializerEvent.Serialize(eh.GetAllEvent());

            ViewBag.events = serializedResultE;

            return View();
        }

        // GET: Gallery for ID}
        [HttpGet]
        public ActionResult Album(int idEvent)
        {
            var idProfile = Session["ProfileID"].ToString();
            //helpers models
            TagHelper th = new TagHelper();

            //serializers
            var serializer = new JavaScriptSerializer();
            var sNav = serializer.Serialize(th.getTagsDeptos(int.Parse(idProfile)));
            ViewBag.Navbar = sNav;

            //get all events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.GetAllEvent();

            //get images for idEvent
            MultimediaHelper mh = new MultimediaHelper();
            //ViewBag.images = mh.getImages(idEvent);


            var serializedResult = serializer.Serialize(mh.GetImages(idEvent));
            ViewBag.images = serializedResult;

            return View();
        }

        [HttpGet]
        public JsonResult getPortrait(String idAlbum)
        {
            //System.Diagnostics.Debug.WriteLine("#");
            int id = Int32.Parse(idAlbum);

            MultimediaHelper mh = new MultimediaHelper();
            var Portrait = mh.GetAlbumPortriat(id);

            return Json(Portrait, JsonRequestBehavior.AllowGet);
      
        }
    }
}