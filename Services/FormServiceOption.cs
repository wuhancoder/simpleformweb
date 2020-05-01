using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using simpleformweb.Models;


namespace simpleformweb.Services
{

    public class FormServiceOption
    {

        public FormServiceOption()
        {

        }

        public string ConnectionString { get; set; }
        public string DataTableName { get; set; }
        public string ConfigBlobContainer { get; set; }

    }
}