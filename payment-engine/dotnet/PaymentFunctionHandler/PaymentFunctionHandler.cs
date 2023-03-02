using System;
using Amazon.Lambda.Core;

namespace PGPCrypt.API {
    public class FunctionHandler {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public Object RegisterBulkPayment(PaymentBO paymentBO) {
            paymentBO.RegisterBulkPayment();
        }
    }   
}