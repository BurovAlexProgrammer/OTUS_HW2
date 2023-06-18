using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        public static void Add(object service)
        {
            if (service == null)
            {
                Debug.LogError("Service cannot be null.");
            }
            var type = service.GetType();
            _services.Add(type, service);
        }

        public static T Get<T>()
        {
            return (T)_services[typeof(T)];
        }

        public static object Get(Type type)
        {
            return _services[type];
        }
    }
}