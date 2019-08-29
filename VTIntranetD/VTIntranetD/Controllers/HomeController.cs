using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranetD.Models.Dto;
using VTIntranetD.Models.Helpers;

namespace VTIntranetD.Controllers
{
    public class HomeController : Controller
    {
        private SessionModel model;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [SessionTimeOut]
        public ActionResult Index()
        {
            model = (SessionModel)this.Session["SessionData"];

            NoticeHelper nh = new NoticeHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(nh.getAllNotice());

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.news2 = serializedResult;
            //ViewBag.news = nh.getAllNotice();
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);
            
            return View();
        }

        [SessionTimeOut]
        public ActionResult Attachment()
        {
            model = (SessionModel)this.Session["SessionData"];
            var tag = Request["tag"];
            var idProfile = model.ProfileID;

            TagHelper th = new TagHelper();
            var serializerDepto = new JavaScriptSerializer();
            var serializedResultD = serializerDepto.Serialize(th.GetDepto(tag, int.Parse(idProfile)));

            ViewBag.D = serializedResultD;
            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Tag = tag;
            ViewBag.Navbar = SerializerNavBar(idProfile);

            return View();
        }

        [SessionTimeOut]
        public ActionResult About()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        public ActionResult Contact()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        public ActionResult Directory()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.rolName = model.RolName;
            ViewBag.UserName = model.UserName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        public ActionResult Events()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            EventHelper eh = new EventHelper();
            ViewBag.events = eh.GetAllEvent();

            return View();
        }

        [SessionTimeOut]
        public ActionResult Post()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        /*public ActionResult Prof()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);
            return View();
        }*/

        [SessionTimeOut]
        public ActionResult Talend()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);

            return View();
        }

        [SessionTimeOut]
        public ActionResult Volunteer()
        {
            model = (SessionModel)this.Session["SessionData"];

            ViewBag.UserName = model.UserName;
            ViewBag.rolName = model.RolName;
            ViewBag.Navbar = SerializerNavBar(model.ProfileID);
            return View();
        }

        /*FUNCTIONS GET AND POST*/
        [SessionTimeOut]
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

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetAreas(String idDepto)
        {
            model = (SessionModel)this.Session["SessionData"];
            var idProfile = int.Parse(model.ProfileID);

            DeptoHelper dh = new DeptoHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(dh.GetAreaDepto(int.Parse(idDepto), idProfile));

            if(serializedResult != "null")
            {
                return Json( new { success = true, data = serializedResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                logger.Error("Error al intentar obtener la información de las Areas en la db. " + Environment.NewLine + DateTime.Now);
                return Json( new { success = false, msgError = "Ocurrio un error, no se encontraron areas."}, JsonRequestBehavior.AllowGet);
            }
            
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetDeptos(String brand)
        {
            model = (SessionModel)this.Session["SessionData"];
            var idProfile = int.Parse(model.ProfileID);

            DeptoHelper dh = new DeptoHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(dh.GetDepto(brand, idProfile));

            if(serializedResult != "null")
            {
                return Json(new { success = true, data = serializedResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                logger.Error("Error al intentar obtener la información de los departamentos en la db. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msgError = "Ocurrio un problema, departamentos no disponibles."}, JsonRequestBehavior.AllowGet);
            }
            
        }

        [SessionTimeOut]
        [HttpPost]
        public JsonResult GetNotice(String idNotice)
        {
            int id = Int32.Parse(idNotice);
            NoticeHelper nh = new NoticeHelper();
            var Notice = nh.getNotice(id);

            if(Notice != null)
            {
                return Json(new { success = true, data = Notice });
            }
            else
            {
                logger.Error("Error al intentar obtener la información detalles de noticia en la db. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, data = Notice, error = "Detalles de la noticia no disponibles." });
            }
            
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetAttachArea(int idParent)
        {
            AttachmentHelper at = new AttachmentHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(at.GetAttachArea(idParent));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetAttachDepto(String tagClabe)
        {
            AttachmentHelper at = new AttachmentHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(at.GetAttachDepto(tagClabe));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeOut]
        [HttpGet]
        public JsonResult GetTags()
        {
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(th.GetTagsAll());

            if(serializedResult != "null")
            {
                return Json(new { success = true, data = serializedResult }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, msgError = "No se pueden agregar manuales. Intentelo más tarde."}, JsonRequestBehavior.AllowGet);
            }
            
        }

        [SessionTimeOut]
        [HttpPost]
        public JsonResult SaveAlbum(IEnumerable<HttpPostedFileBase> filesPost)
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

                    if(idMultimedia != 0 && idAlbum != 0)
                    {
                       mh.SaveEventMult(idAlbum, idMultimedia);
                    }

                }
                else
                {
                    logger.Error("Error al intentar crear el album en la db. " + Environment.NewLine + DateTime.Now);
                    return Json(new { success = false, msgError = "Error, no se pudo crear el album." });
                }
                
            }
           
            model = (SessionModel)this.Session["SessionData"];
            logger.Info("Album create for username: " + model.UserName + Environment.NewLine + DateTime.Now);

            return Json(new { success = true, msg = "Se genero el album para el evento correctamente." });
        }

        [SessionTimeOut]
        [HttpPost]
        public JsonResult SaveEvent(HttpPostedFileBase filePost)
        {
            int res = 0;
            
            if (filePost.ContentLength > 0)
            {
                string title = Convert.ToString(Request["title"]);
                string url = Convert.ToString(Request["url"]);
                string description = Convert.ToString(Request["description"]);
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
                //System.Diagnostics.Debug.WriteLine(fileName);

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
                res = eh.CreateActivity(evt);

                if(res == 1)
                {
                    model = (SessionModel)this.Session["SessionData"];
                    logger.Info("Evento create for username: " + model.UserName + Environment.NewLine + DateTime.Now);

                    return Json(new { success = true, msg = "El evento " + title + " se ha creado correctamente." });
                }
                else
                {
                    logger.Error("Error al intentar crear un evento en la db. " + Environment.NewLine + DateTime.Now);
                    return Json(new { success = false, msgError = "Error, no se pudo crear el evento." });
                }
            }
            else
            {
                logger.Error("Error al intentar crear un evento. filePost < 0" + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msgError = "Error, nose pudo crear el evento." }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [SessionTimeOut]
        [HttpPost]
        public JsonResult SaveFilePdf(IEnumerable<HttpPostedFileBase> attachmentPost)
        {
            int res = 0;
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

                        if(idAttachment == 0)
                        {
                            logger.Error("Error al intentar guardar el registro la información del documento en la db. " + Environment.NewLine + DateTime.Now);
                                              }
                        if(idAttachment != 0)
                        {
                            int idTag = Convert.ToInt32(Request["idTag"]);
                            int idParent = Convert.ToInt32(Request["idParent"]);
                            int idDepto = Convert.ToInt32(Request["idDepto"]);

                            //create relationship
                            res = at.CreateRelationAttachment(idTag, idParent, idDepto, idAttachment);
                        }
                    }
                }
            }

            if (res == 1)
            {
                model = (SessionModel)this.Session["SessionData"];

                logger.Info("Documento guardado for username: " + model.UserName + Environment.NewLine + DateTime.Now);
                return Json(new { success = true, msgError = "El Documento se guardo correctamente." });
            }
            else
            {
                logger.Error("Error al intentar guardar la información del documento en la db. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msgError = "La información del documento no se pudo guardar." });
            }
        }

        [SessionTimeOut]
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
            if ((nh.CreateActivity(a, n)) == 1)
            {
                model = (SessionModel)this.Session["SessionData"];

                LogManager.Configuration.Variables["userid"] = model.UserID;
                LogManager.Configuration.Variables["username"] = model.UserName;
                logger.Info("Notice created VTIntranet for username: " + model.UserName + Environment.NewLine + DateTime.Now);

                return Json(new { success = true, msg = "Noticia creada correctamente"});
            }
            else
            {
                logger.Error("Error al intentar al crear una noticia en la db. " + Environment.NewLine + DateTime.Now);
                return Json(new { success = false, msg = "Hubo un error al crear la noticia"});
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