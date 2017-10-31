using System;
using System.Reflection;

namespace Provider.Infrastructure
{
    public static class Extensions
    {
		public static object GetPropertyValue<T>(this T obj, string name) where T : class
		{
			Type t = typeof(T);
			return t.GetRuntimeProperty(name).GetValue(obj, null);
		}
    }
}
