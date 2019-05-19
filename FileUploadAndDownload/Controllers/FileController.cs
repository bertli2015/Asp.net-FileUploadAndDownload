using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploadAndDownload.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public JsonResult UploadFile(string fileType, HttpPostedFileBase file)
        {
            var res = string.Empty;
            if (string.IsNullOrEmpty(fileType))
            {
                fileType = "UnknowType";
            }
            if (file == null || file.ContentLength == 0)
            {
                res = "Error:文件大小为0";
                return Json(res);
            }
            
            try
            {
                var dir = Server.MapPath("~/Uploads/" + fileType);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var name = Path.GetFileName(file.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + name;
                var filePath = Path.Combine(dir, fileName);
                file.SaveAs(filePath);
                res = filePath;
                return Json(res);
            }
            catch (Exception ex)
            {
                var ex0 = ex.InnerException ?? ex;
                res = ex0.Message;
                return Json(res);
            }
        }
    }
    //[HttpGet]
    //public ActionResult DownloadFile(string name1)
    //{
    //    var webFile = _fileService.GetFileById(id);
    //    if (webFile == null || string.IsNullOrEmpty(webFile.FilePath))
    //    {
    //        return Json("未找到文件");
    //    }
    //    return File("", "application/octet-stream", name1);
    //}
}