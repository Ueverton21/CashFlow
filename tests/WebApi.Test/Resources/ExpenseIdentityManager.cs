using CashFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.Resources
{
    public class ExpenseIdentityManager
    {
        private readonly Expense _expense;
        
        public ExpenseIdentityManager(Expense expense)
        {
            _expense = expense;
        }

        public long GetExpenseId() => _expense.Id;
        public DateTime GetDate() => _expense.Date;
    }
}
