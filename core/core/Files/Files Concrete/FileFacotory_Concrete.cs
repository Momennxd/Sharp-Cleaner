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
    public class FileFactory : IFileFacotry
    {
        public IFile CreateFile(string path, long size, string FriendlyTypeName, string name, bool isFolder)
        {
            return new DFile(path, size, FriendlyTypeName, name, isFolder);
        }
    }
}
