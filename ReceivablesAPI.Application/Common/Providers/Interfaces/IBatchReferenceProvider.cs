using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceivablesAPI.Application.Common.Providers
{
    public interface IBatchReferenceProvider
    {
        public string GenerateNextBatchReference<T>(CancellationToken cancellationToken);
    }
}
