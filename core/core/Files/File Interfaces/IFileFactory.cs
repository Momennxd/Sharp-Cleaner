using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.File_Interfaces
{
    /// <summary>
    /// Factory responsible for creating <see cref="IFile"/> instances.
    /// Keeps object construction consistent and centralized.
    /// </summary>
    public interface IFileFactory
    {
        /// <summary>
        /// Creates a new <see cref="IFile"/> instance with the provided metadata.
        /// </summary>
        /// <param name="path">The full path or URI of the file or folder.</param>
        /// <param name="size">The file size in bytes (0 for folders or unknown).</param>
        /// <param name="friendlyTypeName">A human-readable description of the file type.</param>
        /// <param name="name">The file or folder name (no path).</param>
        /// <param name="isFolder">Whether the entity is a folder.</param>
        /// <param name="creationTime">When it was created (if known).</param>
        /// <param name="lastWriteTime">When it was last modified (if known).</param>
        /// <param name="lastAccessTime">When it was last accessed (if known).</param>
        IFile CreateFile(
            string path,
            long size,
            string friendlyTypeName,
            string name,
            bool isFolder,
            DateTime? creationTime = null,
            DateTime? lastWriteTime = null,
            DateTime? lastAccessTime = null);
    }
}
