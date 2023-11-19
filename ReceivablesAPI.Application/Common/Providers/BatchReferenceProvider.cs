using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceivablesAPI.Application.Common.Providers
{
    public class BatchReferenceProvider : IBatchReferenceProvider
    {
        public string GenerateNextBatchReference<T>(CancellationToken cancellationToken)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string time = DateTime.Now.ToString("HHmmss");
            string uniqueId = Guid.NewGuid().ToString().Substring(0, 4);
    
            string batchReference = $"{typeof(T).Name}_{date}_{time}_{uniqueId}";

            return batchReference;
        }
    }
}
