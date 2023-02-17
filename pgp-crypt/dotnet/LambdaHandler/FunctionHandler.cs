using System;
using Amazon.Lambda.Core;

namespace PGPCrypt.API {
    public class FunctionHandler {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public Object Invoke() {
            var sc=new ServiceCollection();
            //sc.AddSingleton<IInternalPaymentService, ProPayment>();
            sc.AddSingleton<PGPCryptFacade, PGPCryptFacade>();
            sc.AddSingleton<S3Facade, S3Facade>();
            var serviceProvider=sc.BuildServiceProvider();

            var pgpCryptFacade=serviceProvider.GetService<PGPCryptFacade>();
            if(pgpCryptFacade==null) throw new Exception("Please make sure that PGPCryptFacade is initialized!");
            var objectStore=serviceProvider.GetService<S3Facade>();
            if(objectStore==null) throw new Exception("Please make sure that S3Facade is initialized!");
            var encryptedFile=pgpCryptFacade.EncryptFile();
            await objectStore.UploadFile("pgp-demo-output-archive", encryptedFile);
        }
    }   
}