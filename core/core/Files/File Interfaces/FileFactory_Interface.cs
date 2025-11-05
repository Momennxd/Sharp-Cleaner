using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.File_Interfaces
{
    public interface IFileFacotry
    {
        IFile CreateFile(string path, long size, string FriendlyTypeName, string name, bool IsFolder);
    }
}
