using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public interface ITransaction
    {
        void MakeTransaction();
        void AddTranLog();
        void PrintTran();
    }
}
