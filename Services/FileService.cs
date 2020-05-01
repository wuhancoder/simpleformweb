using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

using simpleformweb.Models;

namespace simpleformweb.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly FileServiceOption _option;

        private readonly CloudStorageAccount _storageAccount;

        private readonly CloudBlobClient _blobClient;

        private readonly CloudBlobContainer _container;

        public FileService(ILogger<FileService> logger, IOptions<FileServiceOption> options)
        {
            _logger = logger;
            _option = options.Value;
            _storageAccount = CloudStorageAccount.Parse(_option.ConnectionString);
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference(_option.ContainerName);
            _container.CreateIfNotExists();
        }

        public string GetContainerUrl()
        {
            return _container.Uri.ToString();

        }
        public async Task<string> submit(string filename, string contentType, Stream stream)
        {
            try
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference(filename);
                blob.Properties.ContentType = contentType;
                blob.UploadFromStream(stream);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            //lines modified

            return "";
        }
    }
}