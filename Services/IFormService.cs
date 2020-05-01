using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using simpleformweb.Models;

namespace simpleformweb.Services
{
    public interface IFormService
    {

        public Task<FormModel> getConfig(string id);

        public Task<string> submit(FormDataModel data);

        public Task<FormDataModel> GetFormData(string id, string key);
    }

}