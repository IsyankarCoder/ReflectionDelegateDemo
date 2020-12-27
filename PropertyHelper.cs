using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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

    public class PropertyHelper
    {
        //private readonly Type;
        private static readonly MethodInfo CallInnerDelegateMethod
           = typeof(PropertyHelper).GetMethod(nameof(CallInnerDelegate), BindingFlags.NonPublic | BindingFlags.Default);
        public static Func<object, TResult> MakeFastPropertyGetter<TResult>(PropertyInfo prop)
        {
            var getMethod = prop.GetMethod;
            var declaringClass = prop.DeclaringType;

            //Func<ControllerType,TResult>
            var getMethodDelegateType = typeof(Func<,>).MakeGenericType(declaringClass, typeof(TResult));

            //c=> c.Data
            var getMethodDelegate = getMethod.CreateDelegate(getMethodDelegateType);

            //CallInnerDelegate<ControllerType,TResult>
            var callInnerGenericMethodWithTypes = CallInnerDelegateMethod.MakeGenericMethod(declaringClass, typeof(TResult));

            //Func<object,TResult>
            var result = callInnerGenericMethodWithTypes.Invoke(null, new[] { getMethodDelegate });

            return (Func<object, TResult>)result;

        }


        private static Func<object, TResult> CallInnerDelegate<TClass, TResult>(Func<TClass, TResult> deleg)
        => instance => deleg((TClass)instance);

    }
}