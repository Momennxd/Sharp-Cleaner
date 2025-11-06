using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace core.libs.APIs
{
    public static class APIRecyclebin
    {
        public static class Clear {
            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            public static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

            // Flags for SHEmptyRecycleBin
            public const uint SHERB_NOCONFIRMATION = 0x00000001;
            public const uint SHERB_NOPROGRESSUI = 0x00000002;
            public const uint SHERB_NOSOUND = 0x00000004;
        }





        public static class  Query
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct SHQUERYRBINFO
            {
                public UInt32 cbSize;
                public UInt64 i64Size;
                public UInt64 i64NumItems;
            }

            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            public static extern int SHQueryRecycleBin(string pszRootPath, ref SHQUERYRBINFO pSHQueryRBInfo);
        }

        
    }
}
