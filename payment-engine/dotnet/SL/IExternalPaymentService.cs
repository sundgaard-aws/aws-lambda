using System;
using System.IO;
using PgpCore;

namespace PGPDemo.SL
{
    public interface IExternalPaymentService
    {        
        public void RegisterBulkPayment();
    }
}