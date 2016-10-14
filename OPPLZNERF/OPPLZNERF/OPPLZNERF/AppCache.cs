using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPPLZNERF
{
    public class AppCache
    {
        private static Dictionary<string, object> _localCache;

        public object this[string element]
        {
            get
            {
                return _localCache.ContainsKey(element) ? _localCache[element] : null;
            }
            set
            {
                _localCache[element] = value;
            }
        }
    }
}
