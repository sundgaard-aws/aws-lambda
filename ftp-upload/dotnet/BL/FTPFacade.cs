using System;
using System.IO;
using System.Net;
using System.Text;
using DTL;

namespace BL {
    public class FTPFacade {
        public static void createLocalDummFile(string localFilePath)
        {
            System.Console.WriteLine($"currentDir=[{Directory.GetCurrentDirectory()}]");
            File.WriteAllText(localFilePath, "id;name;company\n100;Michael;AWS\n200;Jane;AWS");
        }

        public static void uploadFile(string localFilePath, string targetFileName, FTPSecret ftpSecret) {
            FileStream fileStream = null;
            try { 
                fileStream = new FileStream(localFilePath, FileMode.Open);
                uploadData(fileStream, targetFileName, ftpSecret); 
            }
            finally { if(fileStream != null) fileStream.Dispose(); }
        }

        public static void uploadData(string contents, string targetFileName, FTPSecret ftpSecret) {
            MemoryStream memStream = null;
            try { 
                memStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
                uploadData(memStream, targetFileName, ftpSecret); 
            }
            finally { if(memStream != null) memStream.Dispose(); }
        }

        public static void uploadData(Stream dataStream, string targetFileName, FTPSecret ftpSecret)
        {
            var usePassiveFTP = System.Environment.GetEnvironmentVariable("usePassiveFTP");
            if (string.IsNullOrEmpty(usePassiveFTP)) throw new System.Exception("Please define an environment variable for [usePassiveFTP]");
            var targetFilePath = ftpSecret.Host + "/" + targetFileName;
            System.Console.WriteLine($"targetFTPPath={targetFilePath}");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(targetFilePath);
            request.Credentials = new NetworkCredential(ftpSecret.Login, ftpSecret.Password);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UsePassive=Boolean.Parse(usePassiveFTP);
            request.KeepAlive = false;
            using (Stream ftpStream = request.GetRequestStream())
            {                
                dataStream.CopyTo(ftpStream);
            }    
            System.Console.WriteLine($"Data uploaded to FTP server.");
        }
    }
}