using core.concrete;
using core.core.File_Interfaces;
using core.interfaces;
using Core.Core.ServicesFilters.AnalyzerFilter.Generic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.core.Services_Filters.Analyzer_Filter.Generic.services
{
    public class AnalyzerFilterService : IAnalyzerFilterService
    {
        public IEnumerable<IFile> FilterFiles(IFileFactory factory, IEnumerable<string> FilesPaths, AnalyzerFilterFlagsBase AZfilter)
        {
            IEnumerable<IFile> filters = new List<IFile>();
            foreach (string path in FilesPaths)
            {
                var f = FilterFile(factory, path, AZfilter);
                if (f != null)
                {
                    (filters as List<IFile>).Add(f);
                }
            }
            return filters;
        }

        public IEnumerable<IFile> FilterFiles(IFileFactory factory, string DirPath, AnalyzerFilterFlagsBase AZfilter)
        {
            string[] allFiles = Directory.GetFiles(DirPath, "*", SearchOption.AllDirectories);
            return FilterFiles(factory, allFiles, AZfilter);

        }

        public IFile FilterFile(IFileFactory factory, string FilePath, AnalyzerFilterFlagsBase AZfilter)
        {
            var list = new List<IFile>();
            FileInfo fi = null;
            if (File.Exists(FilePath)) fi = new FileInfo(FilePath);
            else throw new Exception("File Path does not exist");

            if (fi != null && fi.Length < AZfilter.MinFileSizeBytes || fi.Length > AZfilter.MaxFileSizeBytes)
                return null;

            if (fi != null)
            {
                DateTime created = fi.CreationTimeUtc;
                DateTime modified = fi.LastWriteTimeUtc;
                DateTime accessed = fi.LastAccessTimeUtc;

                TimeSpan fileAge = DateTime.UtcNow - created;
                if (AZfilter.MaxFileAge.HasValue && fileAge > AZfilter.MaxFileAge.Value)
                    return null;
                if (AZfilter.MinFileAge.HasValue && fileAge < AZfilter.MinFileAge.Value)
                    return null;

                return factory.CreateFile(
                     fi.FullName,
                     fi.Length,
                     fi.Extension,
                     fi.Name,
                     isFolder: false,
                     created,
                     modified,
                     accessed

                );
            }

            return null;
        }

        /// <summary>
        /// Filters a directory and all its subdirectories based on the provided AnalyzerFilterFlags + it includes the parent directory.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="ParentDirPath"></param>
        /// <param name="AZfilter"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<IFile> FilterDirectoryRecursiveInclusive(IFileFactory factory, string ParentDirPath, AnalyzerFilterFlagsBase AZfilter)
        {
            var list = new List<IFile>();
            DirectoryInfo di = null;
            if (Directory.Exists(ParentDirPath)) di = new DirectoryInfo(ParentDirPath);
            else throw new Exception("Directory Path does not exist");

            string[] allFiles = Directory.GetDirectories(ParentDirPath, "*", SearchOption.AllDirectories);
            list.Add(FilterDirectory(factory, ParentDirPath, AZfilter));

            foreach (var file in allFiles)
            {
                var f = FilterDirectory(factory, file, AZfilter);
                if (f != null)
                {
                    list.Add(f);
                }
            }

            return list;
        }

        /// <summary>
        /// Filters just the parent directory based on the provided AnalyzerFilterFlags.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="ParentDirPath"></param>
        /// <param name="AZfilter"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IFile FilterDirectory(IFileFactory factory, string ParentDirPath, AnalyzerFilterFlagsBase AZfilter)
        {
            DirectoryInfo di = null;
            if (Directory.Exists(ParentDirPath)) di = new DirectoryInfo(ParentDirPath);
            else throw new Exception("Directory Path does not exist");
            if (di != null)
            {
                DateTime created = di.CreationTimeUtc;
                DateTime modified = di.LastWriteTimeUtc;
                DateTime accessed = di.LastAccessTimeUtc;

                TimeSpan fileAge = DateTime.UtcNow - created;
                if (AZfilter.MaxFileAge.HasValue && fileAge > AZfilter.MaxFileAge.Value)
                    return null;
                if (AZfilter.MinFileAge.HasValue && fileAge < AZfilter.MinFileAge.Value)
                    return null;

                return factory.CreateFile(
                     di.FullName,
                     0,
                     di.Extension,
                     di.Name,
                     isFolder: true,
                     created,
                     modified,
                     accessed

                );
            }
            return null;
        }
    }
}
