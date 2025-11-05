using Core.Core.ServicesFilters;
using System;

public class CleanerFilterFlagsBase
{
    public bool ShouldForceCloseProcesses { get; }
    public bool ShouldMarkForDeletion { get; }
    public TimeSpan? MaxFileAge { get; }
    public TimeSpan? MinFileAge { get; }
    public long MaxFileSizeBytes { get; }
    public long MinFileSizeBytes { get; }

    public CleanerFilterFlagsBase(
        bool shouldForceCloseProcesses = false,
        bool shouldMarkForDeletion = false,
        TimeSpan? maxFileAge = null,
        TimeSpan? minFileAge = null,
        long maxFileSizeBytes = long.MaxValue,
        long minFileSizeBytes = 0)
    {
        if (maxFileAge < TimeSpan.Zero)
            throw new ArgumentException("MaxFileAge cannot be negative.", nameof(maxFileAge));

        if (minFileAge < TimeSpan.Zero)
            throw new ArgumentException("MinFileAge cannot be negative.", nameof(minFileAge));

        if (maxFileSizeBytes < 0)
            throw new ArgumentException("MaxFileSizeBytes cannot be negative.", nameof(maxFileSizeBytes));

        if (minFileSizeBytes < 0)
            throw new ArgumentException("MinFileSizeBytes cannot be negative.", nameof(minFileSizeBytes));

        if (maxFileAge.HasValue && minFileAge.HasValue && minFileAge > maxFileAge)
            throw new ArgumentException("MinFileAge cannot be greater than MaxFileAge.");

        if (minFileSizeBytes > maxFileSizeBytes)
            throw new ArgumentException("MinFileSizeBytes cannot be greater than MaxFileSizeBytes.");

        ShouldForceCloseProcesses = shouldForceCloseProcesses;
        ShouldMarkForDeletion = shouldMarkForDeletion;
        MaxFileAge = maxFileAge;
        MinFileAge = minFileAge;
        MaxFileSizeBytes = maxFileSizeBytes;
        MinFileSizeBytes = minFileSizeBytes;
    }
}
