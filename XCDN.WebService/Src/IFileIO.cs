
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace XCDN.WebService
{
    public interface IFileIO 
    {
        Task<string> Write(IFormFile file);
        FileStream Read( string filename);
    }
}