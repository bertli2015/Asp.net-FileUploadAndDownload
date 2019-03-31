using FileUploadAndDownload.Provider;
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
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="guid">GUID由前端生成</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Upload")]
        public async Task<string> Post(string guid)
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
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

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
            return string.Join(",", files);
        }
    }
}
