using System;
using System.IO;
using PgpCore;

namespace PGPDemo.BL
{
    public class S3Facade
    {        
        private string TempPath { get { return Path.Join(Path.GetTempPath(),"pgp"); } }
        
    }
}