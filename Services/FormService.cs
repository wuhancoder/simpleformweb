using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using simpleformweb.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage;

using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace simpleformweb.Services
{

    public class FormService : IFormService
    {
        private readonly ILogger<FormService> _logger;
        private readonly FormServiceOption _option;

        private CloudStorageAccount _storage;
        private CloudTableClient _tableclient;
        private CloudTable _datatable;
        public FormService(ILogger<FormService> logger, IOptions<FormServiceOption> options)
        {
            _logger = logger;
            _option = options.Value;
            try
            {
                _storage = CloudStorageAccount.Parse(_option.ConnectionString);
                _tableclient = _storage.CreateCloudTableClient(new TableClientConfiguration());
                _datatable = _tableclient.GetTableReference(_option.DataTableName);

            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, "Failed to get storage account: {0} ", e.Message);
            }

        }
        public async Task<FormModel> getConfig(string id)
        {
            try
            {



                _logger.Log(LogLevel.Debug, "{0}: Debugging.... ", DateTime.Now.ToLongTimeString());
                _logger.Log(LogLevel.Information, "{0} : {1}", DateTime.Now.ToLongTimeString(), _option.ConnectionString);
                return new FormModel { formId = id, action = "This is from service" };
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, "Cannot get table: {0}, Error Message: {1}", _option.DataTableName, e.Message);
            }

            return new FormModel();

        }

        public async Task<string> submit(FormDataModel data)

        {
            if (await _datatable.CreateIfNotExistsAsync())
            {
                _logger.Log(LogLevel.Debug, "Created Table named: {0}", _option.DataTableName);
            }
            else
            {
                _logger.Log(LogLevel.Debug, "Table already exists: {0}", _option.DataTableName);
            }

            //FormDataModel data = new FormDataModel(id, "" + DateTime.Now.Ticks, values);
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(data);
            TableResult result = await _datatable.ExecuteAsync(insertOrMergeOperation);
            _logger.LogInformation("{0} inserted... Request charge", result.RequestCharge);


            return "Saved in service";
        }

        public async Task<FormDataModel> GetFormData(string id, string key)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<FormDataModel>(id, key);
            TableResult result = await _datatable.ExecuteAsync(retrieveOperation);
            FormDataModel formData = result.Result as FormDataModel;
            return formData;
        }
    }



}