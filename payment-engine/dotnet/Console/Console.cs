using PGPDemo.BL;
using Microsoft.Extensions.DependencyInjection;
using PGPDemo.SL;
using ProPay;

Console.WriteLine("Started PGP Encrypt Console...");


var sc=new ServiceCollection();
sc.AddSingleton<IInternalPaymentService, ProPayment>();
sc.AddSingleton<IExternalPaymentService, EasyPay>();
sc.AddSingleton<PaymentBO, PaymentBO>();
var serviceProvider=sc.BuildServiceProvider();

var internalPaymentService=serviceProvider.GetService<IInternalPaymentService>();
var externalPaymentService=serviceProvider.GetService<IExternalPaymentService>();
var paymentBO=serviceProvider.GetService<IExternalPaymentService>();

paymentBO.RegisterBulkPayment();
//Console.WriteLine($"Decrypted contents are {decryptedContents}");
Console.WriteLine("Ended PGP Encrypt Console.");