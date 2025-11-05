using core.concrete;
using core.core.File_Interfaces;
using core.interfaces;
using core.libs;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.systems.recycle_bin
{
    public class RecyclebinService : IRecyclebinService
    {
        IFileFacotry FF;
        
        public RecyclebinService(IFileFacotry _ff)
        {
            FF = _ff;
        }
       
        private RecyclebinService()
        {

        }

        public IList<IFile> Analyze(AnalyzerFilterFlagsBase AZFilter)
        {
            return SHELL_LAYER.SDirectoryItems(FF, include_folders: true, NAMESPACES.RECYCLE_BIN_SHELL_NAMESPACE);
        }
        public ulong Clean(CleanerFilterFlagsBase CFilter)
        {
            return (ulong)SHELL_LAYER.API_SHEmptyRecycleBin();
        }
    }
}
