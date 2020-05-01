using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using simpleformweb.Models;
using Azure.Storage.Blobs;

namespace simpleformweb.Services
{
    public interface IFileService
    {

        public Task<string> submit(string filename, string contentType, Stream stream);

        public string GetContainerUrl();


    }

}