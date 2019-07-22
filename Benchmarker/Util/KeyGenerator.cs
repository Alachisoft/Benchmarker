using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BechmarkingFramework.Util
{
    internal class KeyGenerator
    {
        private const long KEY_BASE = 10000000;

        internal string generateKey(long keyIndex)
        {
            var key = KEY_BASE + keyIndex;

            return key.ToString();
        }
    }
}
