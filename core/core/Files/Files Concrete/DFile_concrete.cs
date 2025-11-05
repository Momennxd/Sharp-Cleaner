using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.concrete
{
    public class DFile : IFile
    {

        public string FriendlyTypeName { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public string Name { get; set; }

        public bool IsFolder { get; set; }

        public DFile(string path, long size, string FriendlyTypeName, string name, bool isFolder)
        {
            this.Path = path;
            this.FriendlyTypeName = FriendlyTypeName;
            this.Size = size;
            Name = name;
            IsFolder = isFolder;
        }
    }

   
}
