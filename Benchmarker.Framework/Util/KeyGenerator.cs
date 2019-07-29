using System;

namespace Benchmarker.Framework.Util
{
    public class KeyGenerator
    {
        private const long KEY_BASE = 10000000;

        public string Generate(long keyIndex)
        {
            return (KEY_BASE + keyIndex).ToString();
        }
    }
}
