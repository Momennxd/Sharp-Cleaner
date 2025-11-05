using core.concrete;
using core.core.File_Interfaces;
using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace core.libs
{

    /// <summary>
    /// A wrapper class for the core functions of the Windows Shell COM object to deal with directories and files 
    /// </summary>
    /// <remarks>NOTE : dealing with Shell is slow and has alot of overhead but it has more privileges than normal system.io
    /// liking dealing with special folders like recycle bin</remarks>
    public static class SHELL_LAYER
    {
        /// <summary>
        /// creates a Shell.Application COM object to interact with the Windows Shell functions like (file explorer) to manipulate files and folders
        /// that system.io cannot like recyclbin access , special folders access ..ect
        /// </summary>
        public static dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));

        /// <summary>
        /// Retrieves a list of files and optionally folders from a specified directory.
        /// </summary>
        /// <remarks>This method uses the shell namespace to access directory items. The <paramref
        /// name="path"/> is only used if <paramref name="_namespace"/> is set to -1, otherwise the namespace identifier
        /// is used to locate the directory.</remarks>
        /// <param name="FF">The factory used to create file objects.</param>
        /// <param name="_namespace">The namespace identifier for the directory. If set to -1, the directory is specified by the <paramref
        /// name="path"/> parameter.</param>
        /// <param name="include_folders">A boolean value indicating whether to include folders in the returned list. <see langword="true"/> to
        /// include folders; otherwise, <see langword="false"/>.</param>
        /// <param name="path">The file system path to the directory. This parameter is used when <paramref name="_namespace"/> is set to
        /// -1.</param>
        /// <returns>A list of <see cref="IFile"/> objects representing the files and optionally folders in the specified
        /// directory.</returns>
        private static IList<IFile> _SDirectoryItems(IFileFacotry FF, int _namespace = -1, bool include_folders = true, string path = "")
        {
            var list = new List<IFile>();
            dynamic folder;
            folder = _namespace == -1 ? shell.Namespace(path) : shell.NameSpace(_namespace);
            dynamic items = folder.Items();
            long size = items.Count;
            for (int i = 0; i < size; i++)
            {
                dynamic item = items.Item(i);
                if (!item.IsFolder || include_folders)
                    list.Add(FF.CreateFile(item.Path, item.Size, item.Type, item.Name, item.IsFolder));

            }
            return list;
        }

        /// <summary>
        /// Retrieves a list of directory items from the specified file factory.
        /// </summary>
        /// <param name="FF">The file factory used to access the directory items.</param>
        /// <param name="include_folders">A boolean value indicating whether to include folders in the returned list. <see langword="true"/> to
        /// include folders; otherwise, <see langword="false"/>.</param>
        /// <param name="_namespace">The namespace identifier used to filter the directory items. Must be a non-negative integer.</param>
        /// <returns>A list of <see cref="IFile"/> objects representing the directory items.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="_namespace"/> is less than zero.</exception>
        public static IList<IFile> SDirectoryItems(IFileFacotry FF, bool include_folders = true, int _namespace = -1)
        {
            if (_namespace < 0)
                throw new ArgumentException("namespace must be provided");

            return _SDirectoryItems(FF, _namespace, include_folders, string.Empty);
        }

        /// <summary>
        /// Retrieves a list of file items from the specified directory path.
        /// </summary>
        /// <param name="FF">The factory used to create file instances.</param>
        /// <param name="include_folders">A boolean value indicating whether to include folders in the returned list. <see langword="true"/> to
        /// include folders; otherwise, <see langword="false"/>.</param>
        /// <param name="path">The directory path from which to retrieve file items. Must not be null or empty.</param>
        /// <returns>A list of file items from the specified directory. The list may include folders if <paramref
        /// name="include_folders"/> is <see langword="true"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is null or empty.</exception>
        public static IList<IFile> SDirectoryItems(IFileFacotry FF, bool include_folders = true, string path = "")
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path must be provided");

            return _SDirectoryItems(FF, -1, include_folders, path);
        }


        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

        // Flags for SHEmptyRecycleBin
        private const uint SHERB_NOCONFIRMATION = 0x00000001;
        private const uint SHERB_NOPROGRESSUI = 0x00000002;
        private const uint SHERB_NOSOUND = 0x00000004;

        /// <summary>
        /// Empties the Recycle Bin for all drives without displaying confirmation or progress UI.
        /// </summary>
        /// <remarks>This method performs the operation silently, without any user interface prompts or
        /// sounds.</remarks>
        /// <returns>An integer value indicating the result of the operation. A return value of zero indicates success.
        /// <br> S_OK = 0x00000000 Success // E_FAIL = 0x80004005 // E_ACCESSDENIED = 0x80070005</br></returns>
        public static int API_SHEmptyRecycleBin()
        {
           return SHEmptyRecycleBin(IntPtr.Zero, null,
           SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND);


        }

    }

}
