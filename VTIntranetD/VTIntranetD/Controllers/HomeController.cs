using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Helpers;
using VTIntranetD.Models.Dto;
using System.Text;

namespace VTIntranetD.Controllers
{
    public class HomeController : Controller
    {
        /*SECTIONS*/
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                NoticeHelper nh = new NoticeHelper();
                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Serialize(nh.getAllNotice());

                ViewBag.UserName = this.Session["userName"];
                ViewBag.rolName = this.Session["rolName"];
                ViewBag.news2 = serializedResult;
                ViewBag.news = nh.getAllNotice();
                ViewBag.Navbar = SerializerNavBar();

                return View();
            }
            else
            {
                return RedirectToAction("Login/Index");
            }
        }

        public ActionResult Attachment()
        {
            var tag = Request["tag"];
            var idProfile = Session["ProfileID"].ToString();

            TagHelper th = new TagHelper();
            var serializerDepto = new JavaScriptSerializer();
            var serializedResultD = serializerDepto.Serialize(th.GetDepto(tag, int.Parse(idProfile)));

            ViewBag.D = serializedResultD;
            ViewBag.UserName = this.Session["userName"];
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.Tag = tag;
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult Directory()
        {
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult Events()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.rolName = this.Session["rolName"];
            ViewBag.Navbar = SerializerNavBar();

            EventHelper eh = new EventHelper();
            ViewBag.events = eh.GetAllEvent();

            return View();
        }

        public ActionResult Post()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult Talend()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();

            return View();
        }

        public ActionResult Volunteer()
        {
            ViewBag.UserName = this.Session["userName"];
            ViewBag.Navbar = SerializerNavBar();
            return View();
        }

        /*FUNCTIONS GET AND POST*/

        [HttpPost]
        public JsonResult DeleteAttach(String idAttach, String idDepto, String fileName)
        {
            
            AttachmentHelper ah = new AttachmentHelper();
            
            //delete references tblAttachmentstags
            int delR = ah.DeleteAttachRelation(int.Parse(idAttach), int.Parse(idDepto));
            //delete from tblAttachments
            int delA = ah.DeleteAttach(int.Parse(idAttach));
            //delete file from Path
            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/attachments/"), fileName);
            System.IO.File.Delete(_path);

            if (delR == 1 && delA == 1)
            {
                return Json("successfully");
            }
            else
            {
                return Json("Error");
            }

        }

        [HttpGet]
        public JsonResult GetAreas(String idDepto)
        {
            var idProfile = int.Parse(Session["ProfileID"].ToString());
            DeptoHelper dh = new DeptoHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(dh.GetAreaDepto(int.Parse(idDepto), idProfile));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeptos(String brand)
        {
            var idProfile = int.Parse(Session["ProfileID"].ToString());
            DeptoHelper dh = new DeptoHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(dh.GetDepto(brand, idProfile));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNotice(String idNotice)
        {
            int id = Int32.Parse(idNotice);
            NoticeHelper nh = new NoticeHelper();
            var Notice = nh.getNotice(id);

            return Json(Notice, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAttachArea(int idParent)
        {
            AttachmentHelper at = new AttachmentHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(at.GetAttachArea(idParent));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAttachDepto(String tagClabe)
        {
            AttachmentHelper at = new AttachmentHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(at.GetAttachDepto(tagClabe));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTags()
        {
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(th.GetTagsAll());

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAlbum(IEnumerable<HttpPostedFileBase> filesPost)
        {
            try
            {
                foreach (var file in filesPost)
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(_FileName);
                        string idAlb = Convert.ToString(Request["idAlbum"]);
                        int idMultimedia = 0;

                        StringBuilder builder = new StringBuilder();
                        Random random = new Random();
                        char ch;
                        for (int i = 0; i < _FileName.Length; i++)
                        {
                            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                            builder.Append(ch);
                        }
                        string fileName = builder.ToString().ToLower();
                        fileName = fileName + extension;
                        System.Diagnostics.Debug.WriteLine(fileName);

                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles/events"), fileName);
                        file.SaveAs(_path);

                        Multimedia mult = new Multimedia
                        {
                            FileName = fileName,
                            Path = "~/UploadedFiles/events/" + fileName
                        };

                        MultimediaHelper mh = new MultimediaHelper();
                        idMultimedia = mh.CreateMultimedia(mult);
                        var idAlbum = int.Parse(idAlb);
                        mh.SaveEventMult(idAlbum, idMultimedia);

                    }
                }

                return Json(new { success = true, responseText = "succesfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("File upload failed!!");
                
            }

        }

        [HttpPost]
        public JsonResult SaveEvent(HttpPostedFileBase filePost)
        {
            try
            {
                if (filePost.ContentLength > 0)
                {
                    string title = Convert.ToString(Request["title"]);
                    string url = Convert.ToString(Request["url"]);
                    string description = Convert.ToString(Request["description"]);

                    System.Diagnostics.Debug.WriteLine(title);
                    System.Diagnostics.Debug.WriteLine(url);
                    System.Diagnostics.Debug.WriteLine(description);

                    string _FileName = Path.GetFileName(filePost.FileName);

                    StringBuilder builder = new StringBuilder();
                    Random random = new Random();
                    char ch;
                    for (int i = 0; i < _FileName.Length; i++)
                    {
                        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                        builder.Append(ch);
                    }
                    string fileName = builder.ToString().ToLower();
                    fileName = fileName + ".jpg";
                    System.Diagnostics.Debug.WriteLine(fileName);

                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles/events"), fileName);
                    filePost.SaveAs(_path);

                    EventFull evt = new EventFull
                    {
                        Title = title,
                        Description = description,
                        FileName = fileName,
                        Path = "~/UploadedFiles/events/" + fileName,
                        Url = url
                    };

                    EventHelper eh = new EventHelper();
                    eh.CreateActivity(evt);

                }

                return Json(new { success = true, responseText = "succesfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return Json("Error");
            }

        }

        [HttpPost]
        public JsonResult SaveFilePdf(IEnumerable<HttpPostedFileBase> attachmentPost)
        {
            int res = 0;
            try
            {
                foreach (var file in attachmentPost)
                {
                    if (file.ContentLength > 0)
                    {
                        String FileExt = Path.GetExtension(file.FileName).ToUpper();

                        if (FileExt == ".PDF")
                        {
                            string title_temp = Convert.ToString(Request["title"]);
                            string fileClabe = Convert.ToString(Request["fileClabe"]);
                            string title =  fileClabe + Path.GetExtension(file.FileName);
                            int idAttachment = 0;

                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/attachments/"), title);
                            file.SaveAs(_path);

                            Attachment attachment = new Attachment()
                            {
                                AttachmentName = title,
                                AttachmentDirectory = "~/UploadedFiles/attachments/" + title,
                                AttachmentActive = "1",
                            };

                            AttachmentHelper at = new AttachmentHelper();
                            //save references attachment
                            idAttachment = at.CreateAttachment(attachment);

                            int idTag = Convert.ToInt32(Request["idTag"]);
                            int idParent = Convert.ToInt32(Request["idParent"]);
                            int idDepto = Convert.ToInt32(Request["idDepto"]);

                            //create relationship
                            res = at.CreateRelationAttachment(idTag, idParent, idDepto, idAttachment);

                        }

                    }
                }

                return Json("successfully");
               
            }
            catch
            {
                return Json("File upload failed!!");
            }
        }

        [HttpPost]
        public JsonResult SaveNotice(Notice notice)
        {
            Activity a = new Activity
            {
                Title = notice.Title,
                Description = notice.Description,
                Date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            };

            Notice n = new Notice()
            {
                StartDateNotice = notice.StartDateNotice,
                EndDateNotice = notice.EndDateNotice,
            };

            NoticeHelper nh = new NoticeHelper();
            nh.CreateActivity(a, n);

            return Json("successfully");

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