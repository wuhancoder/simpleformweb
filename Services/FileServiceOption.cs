using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using simpleformweb.Models;


namespace simpleformweb.Services
{

    public class FileServiceOption
    {

        public FileServiceOption()
        {

        }

        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }

    }
}