using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.systems.glob
{
    [Flags]
    public enum enCleaneOptions : UInt32
    {
        None = 0,

        SystemTemp = 1 << 0,
        UsersTemp = 1 << 1,
        //Prefetch = 1 << 2,             
        //RecycleBin = 1 << 3,            
        //WindowsUpdateCache = 1 << 4,   
        //Clipboard = 1 << 5,            

        //BrowserCache = 1 << 5,         
        //BrowserCookies = 1 << 6,       
        //BrowserHistory = 1 << 7,        
        //AppTempFiles = 1 << 8,         
        //InstallerTemp = 1 << 9,         

        //LogFiles = 1 << 10,            
        //CrashDumps = 1 << 11,          
        //MiniDumps = 1 << 12,            
    }



}
