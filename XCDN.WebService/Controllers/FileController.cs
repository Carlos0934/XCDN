using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace XCDN.WebService 
{
    [Route("files")]
    public class FileController : ControllerBase {
        private IFileIO _fileIO;
        private CheckSumManager _checkSumManager;
        public FileController(IFileIO fileIO , CheckSumManager checkSumManager)
        {
            _fileIO = fileIO;
            _checkSumManager = checkSumManager;
        }

        
        [HttpPost("upload")]
        public async Task<IActionResult>  Upload(IFormFile file)
        {   
           
            using(var ms = new MemoryStream())
            {      

                try
                {
                    await file.CopyToAsync(ms);
                    ms.Position = 0;
                    var exists = await _checkSumManager.FileExists(ms);
                    if(exists)
                        return BadRequest("File already exists");
                    ms.Position = 0;
                    await _checkSumManager.AddFile(ms);
                    ms.Position= 0;
                    var filename = await _fileIO.Write(ms , file.FileName);
                    var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/files/{filename}";
                return Ok(url);
                }
                catch (System.Exception)
                {
                    
                    return BadRequest();
                }
              
            }
           
        }
        [ResponseCache(Duration = 100 , Location = ResponseCacheLocation.Client)]
        [HttpGet("{filename}")]
        public Task<FileStreamResult> Donwload(string filename) => Task.Run(() =>  {
            var stream = _fileIO.Read(filename);

            return File(stream , "application/octet-stream" , filename );
        });
       
    }
}