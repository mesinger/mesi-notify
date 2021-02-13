using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mesi.Notify.Infra
{
    public interface IFileSystem
    {
        string GetExecutingBaseDirectory();
        
        IEnumerable<string> Ls(string path);
    }

    public class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public string GetExecutingBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <inheritdoc />
        public IEnumerable<string> Ls(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}