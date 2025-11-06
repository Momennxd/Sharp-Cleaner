using core.concrete;
using core.core.File_Interfaces;
using core.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
    public static class uSHELL_LAYER
    {
        /// <summary>
        /// creates a Shell.Application COM object to interact with the Windows Shell functions like (file explorer) to manipulate files and folders
        /// that system.io cannot like recyclbin access , special folders access ..ect
        /// </summary>
        public static dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));

        public static class DIR {
            private static IList<Tuple<string, bool>> _SDirectoryItems(int _namespace = -1, bool include_folders = true, string path = "")
            {
                var list = new List<Tuple<string, bool>>();
                dynamic folder = _namespace == -1 ? shell.Namespace(path) : shell.NameSpace(_namespace);
                dynamic items = folder.Items();
                long size = items.Count;

                for (int i = 0; i < size; i++)
                {
                    dynamic item = items.Item(i);

                    Tuple<string, bool> t = new Tuple<string, bool>(item.Path, item.IsFolder);
                    if (include_folders || !item.IsFolder) list.Add(t);
                }

                return list;
            }
            public static IList<Tuple<string, bool>> SHLGetNSPaths(int _namespace = -1, bool include_folders = true)
            {

                if (_namespace < 0)
                    throw new ArgumentException("namespace must be provided");

                return _SDirectoryItems(_namespace, include_folders, "");
            }
            public static IList<Tuple<string, bool>> SHLGetNSPaths(string Path = "", bool include_folders = true)
            {

                if (string.IsNullOrEmpty(Path))
                    throw new ArgumentException("Path must be provided");

                return _SDirectoryItems(-1, include_folders, Path);
            }
        }



        public static class API
        {

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

}
