using core.concrete;
using core.core.Core_Services;
using core.core.File_Interfaces;
using core.core.Services_Filters.Analyzer_Filter.Generic;
using core.core.Services_Filters.Cleaner_Filter.Generic.services;
using core.interfaces;
using core.libs;
using core.libs.APIs;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static core.libs.APIs.APIRecyclebin.Query;
using static core.libs.APIs.APIRecyclebin.Clear;

namespace core.systems.recycle_bin
{
    public class RecyclebinService : IAnalyzer, ICleaner
    {
        IFileFactory FF;
        
        public RecyclebinService(IFileFactory _ff)
        {
            FF = _ff;
        }
       
        private RecyclebinService()
        {

        }

        public  IEnumerable<IFile> Analyze(AnalyzerFilterFlagsBase AZfilter, IAnalyzerFilterService AZFileService)
        {
            IList<Tuple<string, bool>> items = uSHELL_LAYER.DIR.SHLGetNSPaths(NAMESPACES.RECYCLE_BIN_SHELL_NAMESPACE, true);


            var dirs = new List<IFile>();
            var files = new List<IFile>();

            foreach (var item in items)
            {
                var path = item.Item1;
                var isDir = item.Item2;

                if (isDir)
                {
                    var dir = AZFileService.FilterDirectoryRecursiveInclusive(FF, path, AZfilter);                   
                    dirs.AddRange(dir);
                    
                }
                else
                {
                    var file = AZFileService.FilterFile(FF, path, AZfilter);
                    if (file != null)
                    {
                        files.Add(file);
                    }
                }
            }
         
            return files.Concat(dirs).ToList();
        }

        public ulong Clean(CleanerFilterFlagsBase CFilter, ICleanerFilterService CLFilterService)
        {
            ulong size = _GetRecycleBinSize();
            _EmptyRecycleBin();
            return size;
        }



        private static int _EmptyRecycleBin()
        {
            int hr = SHEmptyRecycleBin(IntPtr.Zero, null,
             SHERB_NOCONFIRMATION |SHERB_NOPROGRESSUI | SHERB_NOSOUND);
            return hr;
        }

        private static ulong _GetRecycleBinSize(string drive = null)
        {
            SHQUERYRBINFO info = new SHQUERYRBINFO();
            info.cbSize = (UInt32)Marshal.SizeOf(typeof(SHQUERYRBINFO));
            int hr = SHQueryRecycleBin(drive, ref info);
            if (hr != 0) throw Marshal.GetExceptionForHR(hr);
            return info.i64Size;
        }
    }
}
