using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace FileUploadAndDownload.Provider
{
    public class CustomMultipartFormDataStreamProvider: MultipartFormDataStreamProvider
    {
        public string guid { get; set; }
        public CustomMultipartFormDataStreamProvider(string rootPath,string guidStr="")
           : base(rootPath)
        {
            guid = guidStr;
        }
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            //guid = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(guid))
            {
                var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
                return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            }
            string extension = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? Path.GetExtension(GetValidFileName(headers.ContentDisposition.FileName)) : "";
            return guid + extension;
            
        }
        public string GetValidFileName(string filePath)
        {
            char[] invalids = System.IO.Path.GetInvalidFileNameChars();
            return String.Join("_", filePath.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}