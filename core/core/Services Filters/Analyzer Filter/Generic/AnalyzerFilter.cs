using System;

namespace Core.Core.ServicesFilters.AnalyzerFilter.Generic
{
    public class AnalyzerFilterFlagsBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AnalyzerFilterFlags"/>.
        /// </summary>
        /// <param name="includeMarkedForDeletion">Whether to include files marked for deletion on next reboot.</param>
        /// <param name="maxFileAge">Maximum file age to include in analysis.</param>
        /// <param name="minFileAge">Minimum file age to include in analysis.</param>
        /// <param name="maxFileSizeBytes">Maximum file size (in bytes) to include in analysis.</param>
        /// <param name="minFileSizeBytes">Minimum file size (in bytes) to include in analysis.</param>
        public AnalyzerFilterFlagsBase(
            bool includeMarkedForDeletion = false,
            TimeSpan? maxFileAge = null,
            TimeSpan? minFileAge = null,
            long maxFileSizeBytes = long.MaxValue,
            long minFileSizeBytes = 0)
        {
            if (MaxFileSizeBytes != null && MaxFileSizeBytes < 0)
                throw new ArgumentException("MaxFileSizeBytes cannot be negative.", nameof(maxFileSizeBytes));

            if (MinFileSizeBytes != null && MinFileSizeBytes < 0)
                throw new ArgumentException("MinFileSizeBytes cannot be negative.", nameof(minFileSizeBytes));

            if (maxFileAge.HasValue && minFileAge.HasValue && minFileAge > maxFileAge)
                throw new ArgumentException("MinFileAge cannot be greater than MaxFileAge.");

            if (minFileSizeBytes > maxFileSizeBytes)
                throw new ArgumentException("MinFileSizeBytes cannot be greater than MaxFileSizeBytes.");

            IncludeMarkedForDeletion = includeMarkedForDeletion;
            MaxFileAge = maxFileAge;
            MinFileAge = minFileAge;
            MaxFileSizeBytes = maxFileSizeBytes;
            MinFileSizeBytes = minFileSizeBytes;
        }

        public bool IncludeMarkedForDeletion { get; }

        public TimeSpan? MaxFileAge { get; }

        public TimeSpan? MinFileAge { get; }

        public long MaxFileSizeBytes { get; }

        public long MinFileSizeBytes { get; }
    }
}
