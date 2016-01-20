using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrangle
{
    public static class Dictionary
    {
        public static IDictionary<string, string> From(string[] args)
        {
            return From(args, null);
        }

        public static IDictionary<string, string> From(string[] args, string keyPrefix)
        {
            if (args == null)
                return new Dictionary<string, string>();

            if (args.Length % 2 != 0)
                throw new ArgumentException($"The provided arguments must be in pairs");

            var d = new Dictionary<string, string>();

            for (var i = 0; i < args.Length; i += 2)
            {
                var k = args[i];

                if (!string.IsNullOrEmpty(keyPrefix))
                {
                    if (!k.StartsWith(keyPrefix))
                        throw new ArgumentException($"Expected a parameter name at position {i} but received \"{k}\"");

                    k = k.Substring(keyPrefix.Length);
                }

                d.Add(k, args[i + 1]);
            }
            
            return d;
        }

        public static IDictionary<string, string> FromPairs(string separator, string[] args)
        {
            if(string.IsNullOrEmpty(separator))
                throw new ArgumentNullException(nameof(separator));

            var d = new Dictionary<string, string>();

            if (args == null || args.Length == 0) return d;
            
            for(var i = 0; i < args.Length; i++)
            {
                var pair = args[i].Split(new[] { separator }, StringSplitOptions.None);

                if (pair.Length != 2)
                    throw new ArgumentException($"The provided argument \"{args[i]}\" is invalid; arguments must be pairs separated by \"{separator}\"");

                d.Add(pair[0], pair[1]);
            }

            return d;
        }
    }
}
