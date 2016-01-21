using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wrangle
{
    public static class Instance<T> where T : new()
    {
        public static T From(string[] args)
        {
            var value = new T();

            if (args == null || args.Length == 0) return value;

            var properties = typeof(T).GetProperties()
                .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null);

            var d = Wrangle.Dictionary.From(args);

            foreach(var pair in d)
            {
                var property = properties.Where(p => p.Name == pair.Key).Single();
                property.SetValue(value, pair.Value);
            }

            return value;
        }
    }
}
