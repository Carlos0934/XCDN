
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;

namespace XCDN.WebService 
{
    public class FilesMap
    {
        public FilesMap ()
        {
            LastEntry = DateTime.Now;
            CheckSums = new HashSet<string>();
        }
        public HashSet<string>  CheckSums {get; set;}
        public DateTime LastEntry  {get; set;}
    } 
    public class CheckSumManager 
    {
        private FilesMap _filesMap;
        private string _path;
 
        public CheckSumManager(string path)
        {
            _path = path;
            _filesMap = File.Exists(path) ? deserialize() : new FilesMap();

        }
        private async Task serialize() 
        {
            using(var jsonStream = File.Create(_path))
            {
                await JsonSerializer.SerializeAsync( jsonStream, _filesMap);
            }
           
        }
        private FilesMap deserialize()
        {
            var jsonString = File.ReadAllText(_path);
            var filesMap = JsonSerializer.Deserialize<FilesMap>(jsonString);
            return filesMap;
        }
        public async Task AddFile(MemoryStream fs)
        {
            var hash = await createHash(fs);
            _filesMap.CheckSums.Add(hash);
            _filesMap.LastEntry = DateTime.Now;
            await serialize();
        }
        
        
        public async Task<bool> FileExists(MemoryStream ms)
        {
            var hash =  await createHash(ms);
            return  _filesMap.CheckSums.Contains(hash);
            
        }
        private async Task<string> createHash(MemoryStream ms)
        {
            using(var sha256 = SHA256.Create())
            {
                var hash = await sha256.ComputeHashAsync(ms);
                return Convert.ToBase64String(hash);
            }
        }

    }
}