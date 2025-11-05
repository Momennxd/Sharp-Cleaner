using core.core.Core_Services;
using core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.systems.recycle_bin
{
    public interface IRecyclebinService : IAnalyzer, ICleaner
    {

    }
}
