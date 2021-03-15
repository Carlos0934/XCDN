
using System.IO;
using System.Threading.Tasks;

namespace XCDN.WebService
{
    public interface IFileIO 
    {
        Task<string> Write(MemoryStream fs , string filename);
        FileStream Read( string filename);
    }
}