using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
namespace simpleformweb.Models
{
    public class FormDataModel : TableEntity
    {
        String[] SystemProperties = { "PartitionKey", "RowKey", "Timestamp" };

        public FormDataModel(string formName, string dataKey, Dictionary<string, string> data)
        {
            PartitionKey = formName;
            RowKey = dataKey;
            DataItems = data;
        }

        public Dictionary<string, string> DataItems { get; set; }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            foreach (var item in DataItems)
            {
                //results.Add("D_" + item.Key, new EntityProperty(item.Value));
                results.Add(item.Key, new EntityProperty(item.Value));
            }
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            DataItems = new Dictionary<string, string>();

            foreach (var item in properties)
            {
                //if (item.Key.StartsWith("D_"))
                if (!Array.Exists(SystemProperties, element => element == item.Key))
                {
                    //string realKey = item.Key.Substring(2);
                    DataItems[item.Key] = item.Value.StringValue;
                }
            }
        }
    }
}
