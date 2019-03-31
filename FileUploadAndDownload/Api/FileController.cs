using FileUploadAndDownload.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace FileUploadAndDownload.Api
{
    public class FileController : ApiController
    {
        private JsonSerializerSettings _camelCaseSetting = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            //, NullValueHandling = NullValueHandling.Ignore
        };
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="guid">GUID由前端生成</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Upload")]
        public async Task<IHttpActionResult> Post(string guid)
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string uploadFolderPath = HostingEnvironment.MapPath("~/Upload");
            if (!Directory.Exists(uploadFolderPath))
                Directory.CreateDirectory(uploadFolderPath);

            List<string> files = new List<string>();
            var provider = new CustomMultipartFormDataStreamProvider(uploadFolderPath, guid);
            var res = string.Empty;
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);


                string param1 = string.Empty;
                string param2 = string.Empty;
                //// This illustrates how to get the form data.
                foreach (var key in provider.FormData.AllKeys)
                {
                    if (key == "param1") param1 = provider.FormData.GetValues(key)[0];
                    if (key == "param2") param2 = provider.FormData.GetValues(key)[0];
                }
                if (string.IsNullOrEmpty(param1))
                {
                    res=HttpStatusCode.BadRequest + ",no param1";
                    return Json(res, _camelCaseSetting);
                }
                if (string.IsNullOrEmpty(param2))
                {
                    res = HttpStatusCode.BadRequest + ",no param2";
                    return Json(res, _camelCaseSetting);
                }
                if (provider.FileData == null || provider.FileData.Count == 0)
                {
                    return Json(HttpStatusCode.BadRequest + ", no file", _camelCaseSetting);
                }
                // This illustrates how to get the file names.
                foreach (var file in provider.FileData)
                {//接收文件
                    files.Add(Path.GetFileName(file.LocalFileName));
                }
            }
            catch
            {
                throw;
            }
            return Json(string.Join(",", files), _camelCaseSetting);
        }
        //方法二https://www.cnblogs.com/GarsonZhang/p/5511427.html
    }
}
