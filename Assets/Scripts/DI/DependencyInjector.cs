using System;
using System.Reflection;
using Service;
using UnityEngine;

namespace DI
{
    public static class DependencyInjector
    {
        private const BindingFlags InjectFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        
        public static void Inject(object target)
        {
            var type = target.GetType();
            var methods = type.GetMethods(InjectFlags);
            
            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(InjectAttribute)))
                {
                    InvokeMethod(method, target);
                }
            }
        }

        private static void InvokeMethod(MethodInfo method, object target)
        {
            var parameterInfos = method.GetParameters();
            
            object[] args = new object[parameterInfos.Length];
            
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var type = parameterInfos[i].GetType();
                args[i] = ServiceLocator.Get(type);
            }

            method.Invoke(target, args);
        }
    }
}