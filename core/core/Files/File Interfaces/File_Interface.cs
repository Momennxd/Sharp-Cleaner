
using System;
using System.Dynamic;

namespace core.interfaces
{
    /// <summary>
    /// represents the abstract form of data used in the system either a file or folder with its metadata.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// The full path or URI that uniquely identifies this file or folder.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The display name (file or folder name, without path).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates whether this entity represents a folder/directory.
        /// </summary>
        bool IsFolder { get; }

        /// <summary>
        /// The file size in bytes (0 for folders or unknown).
        /// </summary>
        long Size { get; }

        /// <summary>
        /// A user-friendly description of the file type (e.g. "JPEG Image", "Text Document").
        /// </summary>
        string FriendlyTypeName { get; }

        /// <summary>
        /// When the file or directory was created (if known).
        /// </summary>
        DateTime? CreationTime { get; }

        /// <summary>
        /// When the file or directory was last modified (if known).
        /// </summary>
        DateTime? LastWriteTime { get; }

        /// <summary>
        /// When the file or directory was last accessed (if known).
        /// </summary>
        DateTime? LastAccessTime { get; }

     
    }


}