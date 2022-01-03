using System;
using System.IO;
using System.Net;

public class FTPFacade {
    public static void createLocalDummFile(string localFilePath)
    {
        System.Console.WriteLine($"currentDir=[{Directory.GetCurrentDirectory()}]");
        File.WriteAllText(localFilePath, "id;name;company\n100;Michael;AWS\n200;Jane;AWS");
    }

    public static void uploadFile()
    {
        var localFilePath = System.Environment.GetEnvironmentVariable("localFilePath");
        var targetFTPPath = System.Environment.GetEnvironmentVariable("targetFTPPath");
        var ftpUsername = System.Environment.GetEnvironmentVariable("ftpUsername");
        var ftpPassword = System.Environment.GetEnvironmentVariable("ftpPassword");
        var usePassiveFTP = System.Environment.GetEnvironmentVariable("usePassiveFTP");
        if (string.IsNullOrEmpty(localFilePath)) throw new System.Exception("Please define an environment variable for [localFilePath]");
        if (string.IsNullOrEmpty(targetFTPPath)) throw new System.Exception("Please define an environment variable for [targetFTPPath]");
        if (string.IsNullOrEmpty(ftpUsername)) throw new System.Exception("Please define an environment variable for [ftpUsername]");
        if (string.IsNullOrEmpty(ftpPassword)) throw new System.Exception("Please define an environment variable for [ftpPassword]");
        if (string.IsNullOrEmpty(usePassiveFTP)) throw new System.Exception("Please define an environment variable for [usePassiveFTP]");
        System.Console.WriteLine($"localFilePath={localFilePath}");
        System.Console.WriteLine($"targetFTPPath={targetFTPPath}");
        createLocalDummFile(localFilePath);
        System.Console.WriteLine($"Local file created.");
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(targetFTPPath);
        request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.UsePassive=Boolean.Parse(usePassiveFTP);
        request.KeepAlive = false;
        using (Stream fileStream = File.OpenRead(localFilePath))
        using (Stream ftpStream = request.GetRequestStream())
        {                
            fileStream.CopyTo(ftpStream);
        }    
        System.Console.WriteLine($"File uploaded to FTP server.");
    }
}