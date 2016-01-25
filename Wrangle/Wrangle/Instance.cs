using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrangle
{
    public static class Instance<T> where T : new()
    {
        private static readonly Dictionary<Type, Func<string, object>> KnownConversions = new Dictionary<Type, Func<string, object>>
        {
            { typeof(TimeSpan), s => TimeSpan.Parse(s) },
            { typeof(DateTimeOffset), s => DateTimeOffset.Parse(s) },
            { typeof(Guid), s => Guid.Parse(s) }
        };

        public static T From(string[] args)
        {
            var arguments = new T();

            if (args == null || args.Length == 0) return arguments;

            var properties = typeof(T).GetProperties()
                .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null);

            var d = Wrangle.Dictionary.From(args);

            foreach(var pair in d)
            {
                var property = properties.Where(p => p.Name == pair.Key).Single();
                
                if(KnownConversions.ContainsKey(property.PropertyType))
                {
                    property.SetValue(arguments, KnownConversions[property.PropertyType](pair.Value));
                }
                else if(property.PropertyType.IsEnum)
                {
                    var value = Enum.Parse(property.PropertyType, pair.Value, ignoreCase: true);
                    property.SetValue(arguments, value);   
                }
                else
                {
                    var value = Convert.ChangeType(pair.Value, property.PropertyType);
                    property.SetValue(arguments, value);
                }
            }

            return arguments;
        }
    }
}
