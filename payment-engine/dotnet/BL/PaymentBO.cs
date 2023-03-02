using System;
using System.IO;
using PgpCore;
using PGPDemo.SL;

namespace PGPDemo.BL
{
    public class PaymentBO
    {        
        public PaymentBO(IInternalPaymentService internalPaymentService, IExternalPaymentService externalPaymentService) {

        }

        public void SendBulkPayment(dynamic payment) {

        }

        public void ReceiveBulkPaymentReceipt(dynamic payment) {
            
        }
    }
}