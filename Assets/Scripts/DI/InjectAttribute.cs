using System;
using JetBrains.Annotations;

namespace DI
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
         
    }
}