using core.core.File_Interfaces;
using core.interfaces;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.Services_Filters.Analyzer_Filter.Generic
{
    public interface IAnalyzerFilterService
    {
        IEnumerable<IFile> FilterFiles(IFileFactory factory, IEnumerable<string> FilesPaths, AnalyzerFilterFlagsBase AZfilter);
        IEnumerable<IFile> FilterFiles(IFileFactory factory, string DirPath, AnalyzerFilterFlagsBase AZfilter);
        IFile FilterFile(IFileFactory factory, string FilePath, AnalyzerFilterFlagsBase AZfilter);
        IEnumerable<IFile> FilterDirectoryRecursiveInclusive(IFileFactory factory, string ParentDirPath, AnalyzerFilterFlagsBase AZfilter);

        IFile FilterDirectory(IFileFactory factory, string ParentDirPath, AnalyzerFilterFlagsBase AZfilter);


    }
}
