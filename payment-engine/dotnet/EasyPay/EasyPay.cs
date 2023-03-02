using System;
using System.IO;
using PGPCrypto;
using PGPDemo.SL;

namespace EasyPay
{
    public class EasyPay : IExternalPaymentService
    {
        public void RegisterBulkPayment()
        {
            var crypt=new PGPCryptFacade();
            crypt.GenerateKeyPair();
            crypt.EncryptFile();
            var decryptedContents=crypt.DecryptFile();
            //crypt.Simple();
        }
    }
}