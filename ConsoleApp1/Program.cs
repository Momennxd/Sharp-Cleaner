using core.core.Concrete;
using core.core.Services_Filters.Analyzer_Filter.Generic;
using core.systems.recycle_bin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

class Program
{
   

    [STAThread]
    static void Main()
    {
        //foreach (var info in GetClearableUserTempFolders())
        //{
        //    Console.WriteLine($"User: {info.UserName}\n  SID: {info.Sid}\n  Profile: {info.ProfilePath}\n  Temp: {info.TempPath}\n");
        //}

        RecyclebinService recyclebinService = new RecyclebinService(new FileFactory());
        IAnalyzerFilterService analyzerFilterService = new core.core.Services_Filters.Analyzer_Filter.Generic.services.AnalyzerService();
        var files = recyclebinService.Analyze(new Core.Core.ServicesFilters.AnalyzerFilter.Generic.AnalyzerFilterFlagsBase(), analyzerFilterService);

        foreach (var file in files)
        {
            Console.WriteLine($"File: {file.Name}, Size: {file.Size} bytes, Path: {file.Path}, IsFolder: {file.IsFolder}");
        }


    }

    /// <summary>
    /// Returns enumerable of real, local user profiles that have temp folders you can inspect/clean.
    /// Each item contains: username, SID, profile path, and resolved temp path.
    /// Skips known system/service accounts and profiles without NTUSER.DAT (not a real user hive).
    /// </summary>
    public static IList<(string UserName, string Sid, string ProfilePath, string TempPath)> GetClearableUserTempFolders()
    {
        const string ProfileListKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
        IList<(string, string, string, string)> res = new List<(string, string, string, string)>();
        // SIDs to always skip (well-known)
        var skipSids = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "S-1-5-18", // Local System (systemprofile)
            "S-1-5-19", // LocalService
            "S-1-5-20"  // NetworkService
        };

        // usernames/folders to skip
        var skipNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Default", "DefaultUser0", "DefaultAccount", "Public", "All Users", "Default User", "WDAGUtilityAccount"
        };

        using (var profilesKey = Registry.LocalMachine.OpenSubKey(ProfileListKey))
        {
            if (profilesKey == null) return res;

            foreach (var sid in profilesKey.GetSubKeyNames())
            {
                try
                {
                    if (skipSids.Contains(sid)) continue;

                    using (var userKey = profilesKey.OpenSubKey(sid))
                    {
                        if (userKey == null) continue;

                        var profilePathObj = userKey.GetValue("ProfileImagePath");
                        if (profilePathObj == null) continue;

                        // Expand environment vars in registry value (some profile paths can contain %SystemDrive%, etc.)
                        string profilePath = Environment.ExpandEnvironmentVariables(profilePathObj.ToString());

                        // Must exist on disk to be a valid profile
                        if (string.IsNullOrWhiteSpace(profilePath) || !Directory.Exists(profilePath))
                            continue;

                        string userName = Path.GetFileName(profilePath);
                        if (string.IsNullOrWhiteSpace(userName)) continue;

                        // Skip well-known system/service accounts by name or if profile is under ServiceProfiles
                        if (skipNames.Contains(userName)) continue;
                        if (profilePath.IndexOf(@"\ServiceProfiles\", StringComparison.OrdinalIgnoreCase) >= 0) continue;

                        // Ensure this is a normal user profile by checking for NTUSER.DAT (user registry hive)
                        string ntUserFile = Path.Combine(profilePath, "NTUSER.DAT");
                        if (!File.Exists(ntUserFile)) continue;

                        // Try to get per-user TEMP from HKEY_USERS\<SID>\Volatile Environment\TEMP
                        string tempPath = null;
                        try
                        {
                            using (var volKey = Registry.Users.OpenSubKey($@"{sid}\Volatile Environment"))
                            {
                                if (volKey != null)
                                {
                                    var tempVal = volKey.GetValue("TEMP") ?? volKey.GetValue("TMP");
                                    if (tempVal != null)
                                        tempPath = Environment.ExpandEnvironmentVariables(tempVal.ToString());
                                }
                            }
                        }
                        catch
                        {
                            // access to HKEY_USERS\<sid> might fail for some SIDs — ignore and fallback
                            tempPath = null;
                        }

                        // Fallback to default per-profile location
                        if (string.IsNullOrWhiteSpace(tempPath))
                        {
                            tempPath = Path.Combine(profilePath, "AppData", "Local", "Temp");
                        }
                        else
                        {
                            // Expand variables (just in case)
                            tempPath = Environment.ExpandEnvironmentVariables(tempPath);
                        }

                        // Only return if the temp folder actually exists (so caller doesn't get dead paths)
                        if (!string.IsNullOrWhiteSpace(tempPath) && Directory.Exists(tempPath))
                        {
                            res.Add((userName, sid, profilePath, tempPath));
                        }
                    }
                }
                catch
                {
                    // Keep enumerating even if one profile causes an exception
                    continue;
                }

            }
        }

        return res;
    }
}
