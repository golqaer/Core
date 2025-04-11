using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DbTransactions.Abstracts
{
    public interface ITransactionScopeFactory
    {
        ITransactionScope Create();
    }
}
