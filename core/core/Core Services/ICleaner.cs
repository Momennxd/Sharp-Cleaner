using core.core.Services_Filters.Cleaner_Filter.Generic.services;
using Core.Core.ServicesFilters;
using System;
using System.Collections.Generic;

namespace core.interfaces
{
   /// <summary>
   /// Represents the main cleaner Interface for any cleaner category
   /// </summary>
   /// <typeparam name="TFilter">represents the main filter interface for cleaning</typeparam>
    public interface ICleaner
    {
        /// <summary>
        /// represents the cleaning method
        /// </summary>
        /// <param name="CFilter">an implementation of ICleanerFilterFlags interface</param>
        /// <returns>size of deleted data in bytes</returns>
        UInt64 Clean(CleanerFilterFlagsBase CFilter, ICleanerFilterService CLFilterService);
    }

}
