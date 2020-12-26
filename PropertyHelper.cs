using System.Security.Principal;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System;
namespace ReflectionDelegateDemo
{
    public class PropertyHelper<TClass>
    {         private static readonly ConcurrentDictionary<string, Delegate> cache = new ConcurrentDictionary<string, Delegate>();
        public static Func<TClass, TResult> MakeFastPropertyGetter<TResult>(PropertyInfo prop)
        {
            if (cache.ContainsKey(prop.Name))
            {
                return (Func<TClass, TResult>)cache[prop.Name];
            }

            var getMethod = prop.GetMethod;
            var result = (Func<TClass, TResult>)getMethod.CreateDelegate(typeof(Func<TClass, TResult>));
            cache.TryAdd(prop.Name, result);
            return result;
        }
    }

}