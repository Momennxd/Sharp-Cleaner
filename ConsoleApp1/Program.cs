using Microsoft.Win32;

class Program
{
   

    [STAThread]
    static void Main()
    {
        foreach (var info in GetClearableUserTempFolders())
        {
            Console.WriteLine($"User: {info.UserName}\n  SID: {info.Sid}\n  Profile: {info.ProfilePath}\n  Temp: {info.TempPath}\n");
        }
    }

   

    public static IList<(string UserName, string Sid, string ProfilePath, string TempPath)> GetClearableUserTempFolders()
    {
        const string ProfileListKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
        IList<(string, string, string, string)> res = new List<(string, string, string, string)>();
        var skipSids = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
           
        };

        // usernames/folders to skip
        var skipNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            
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
                       // string ntUserFile = Path.Combine(profilePath, "NTUSER.DAT");
                        //if (!File.Exists(ntUserFile)) continue;

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
