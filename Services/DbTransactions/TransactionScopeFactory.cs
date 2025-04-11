using Services.DbTransactions.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Services.DbTransactions
{
    public class TransactionScopeFactory: ITransactionScopeFactory
    {
        private readonly Context _context;

        public TransactionScopeFactory(Context context)
        {
            _context = context;
        }

        public ITransactionScope Create()
        {
            return new TransactionScope(_context);
        }
    }
}
