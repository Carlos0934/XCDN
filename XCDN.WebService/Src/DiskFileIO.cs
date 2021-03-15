
using System;
using System.IO;
using System.Threading.Tasks;
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
        public async Task<string> Write(MemoryStream ms, string filename)
        {
           
            filename = Path.ChangeExtension(Path.GetRandomFileName(),  Path.GetExtension(filename) );
            var filePath =  Path.Join(_path , filename );
    

            using(var fs = new FileStream 
                (
                    filePath , FileMode.Create ,
                    FileAccess.Write , FileShare.None,
                    _bufferSize ,  useAsync : true 
                )
            )
            {
                await ms.CopyToAsync(fs); 
            }
            return filename;
            
        }

        public FileStream Read(string filename) 
        {   
            var filePath =  Path.Join(_path, filename);
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