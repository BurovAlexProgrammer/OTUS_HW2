using System;
using System.Collections.Generic;

namespace Service
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
        
        public static void Add(IService service)
        {
            var type = service.GetType();
            _services.Add(type, service);
        }

        public static T Get<T>() where T : IService
        {
            return (T)_services[typeof(T)];
        }

        public static object Get(Type type)
        {
            return _services[type];
        }
    }
}