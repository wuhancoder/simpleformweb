using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using simpleformweb.Models;
using simpleformweb.Services;

namespace simpleformweb.Controllers
{


    public class FormController : Controller
    {
        private readonly IFormService _formService;
        private readonly IFileService _fileService;
        private readonly ILogger<FormController> _logger;

        public FormController(ILogger<FormController> logger, IFormService formService, IFileService fileService)
        {
            _logger = logger;
            _formService = formService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Config(string id)
        {
            FormModel config = await _formService.getConfig(id);
            return Json(config);
        }

        public async Task<IActionResult> GetData(string id, string key)
        {
            FormDataModel data = await _formService.GetFormData(id, key);
            return Json(data);

        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromRoute]string id, [FromForm]Dictionary<string, string> values)
        {
            FormDataModel data = new FormDataModel(id, "" + DateTime.Now.Ticks, values);
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var formFile in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        var blobname = String.Format("{0}/{1}/{2}/{3}", data.PartitionKey, data.RowKey, formFile.Name, formFile.FileName);
                        var uri = await _fileService.submit(blobname, formFile.ContentType, memoryStream);
                        //var uri = await _fileService.GetContainerClient().GetBlobClient(formFile.FileName).UploadAsync(memoryStream, true);
                    }
                    _logger.LogInformation("{0} is attached to {1}, content type is {2}", formFile.FileName, formFile.Name, formFile.ContentType);

                }

                data.DataItems.Add("uploadedfiles", String.Concat(_fileService.GetContainerUrl(), "/", data.PartitionKey, "/", data.RowKey));
            }
            string result = await _formService.submit(data);


            return Ok(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
