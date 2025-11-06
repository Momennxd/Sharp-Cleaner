using core.core.File_Interfaces;
using core.core.Services_Filters.Analyzer_Filter.Generic;
using core.interfaces;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.Core_Services
{
    public interface IAnalyzer
    {
        IEnumerable<IFile> Analyze(AnalyzerFilterFlagsBase AZfilter, IAnalyzerFilterService AZFilterService);        
    }
}
