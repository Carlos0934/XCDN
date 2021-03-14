using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace XCDN.WebService 
{
    [Route("files")]
    public class FileController : ControllerBase {
        private IFileIO _fileIO;
        public FileController(IFileIO fileIO)
        {
            _fileIO = fileIO;
        }

        
        [HttpPost("upload")]
        public async Task<IActionResult>  Upload(IFormFile file)
        {
            
            var filename = await _fileIO.Write(file);
            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/files/{filename}";
            return Ok(url);
        }
        [HttpGet("{filename}")]
        public Task<FileStreamResult> Donwload(string filename) => Task.Run(() =>  {
            var stream = _fileIO.Read(filename);

            return File(stream , "application/octet-stream" , filename );
        });
       
    }
}