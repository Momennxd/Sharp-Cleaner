using core.interfaces;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.Core_Services
{
    public interface IAnalyzer
    {
        IList<IFile> Analyze(AnalyzerFilterFlagsBase AZfilter);

    }
}
