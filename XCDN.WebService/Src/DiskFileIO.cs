
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace XCDN.WebService 
{
    public class DiskFileIO : IFileIO {
        private string _path;
        private int _bufferSize;
        public DiskFileIO(string path , int bufferSize) 
        {
            _path = path;
            _bufferSize = bufferSize;
            Directory.CreateDirectory(path);
            
        }
        public async Task<string> Write(IFormFile file)
        {   var filename = Path.GetRandomFileName();
            var filePath =  Path.Join(_path , filename );
            if(file.Length == 0)
                throw new FileLoadException();

            using(var stream = new FileStream 
                (
                    filePath , FileMode.Create ,
                    FileAccess.Write , FileShare.None,
                    _bufferSize ,  useAsync : true 
                )
            )
            {
                await file.CopyToAsync(stream);
               
            }
            return filename;
            
        }

        public FileStream Read(string filename) 
        {   var filePath =  Path.Join(_path, filename);
            var stream = new FileStream 
                (
                    filePath , FileMode.Open ,
                    FileAccess.Read , FileShare.Read,
                    _bufferSize ,  useAsync : true 
                );

            return stream;  
        }
    }
}