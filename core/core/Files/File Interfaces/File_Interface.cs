
using System.Dynamic;

namespace core.interfaces
{
 
    public interface IFile
    {
        string FriendlyTypeName { get; }
        bool IsFolder { get; }
        string Path { get; }
        string Name { get; }
        long Size { get; }
    }


}