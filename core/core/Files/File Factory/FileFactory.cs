using core.concrete;
using core.core.File_Interfaces;
using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.Concrete
{
    /// <summary>
    /// Factory responsible for creating IFile instances.
    /// Keeps creation logic centralized and consistent.
    /// </summary>
    public class FileFactory : IFileFactory
    {
        
        public IFile CreateFile(string path, long size, string friendlyTypeName, string name, bool isFolder,
            DateTime? creationTime = null, DateTime? lastWriteTime = null, DateTime? lastAccessTime = null)
        {
            return new DFile(path, size, friendlyTypeName, name, isFolder, creationTime, lastWriteTime, lastAccessTime);
        }
    }
}
