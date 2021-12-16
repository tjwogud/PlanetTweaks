using HarmonyLib;
using System;

namespace PlanetTweaks.Utils
{
    public static class Reflection
    {
        public static object Get(this object obj, string name)
        {
            return obj.GetType().Get(name, obj);
        }

        public static object Get(this Type type, string name)
        {
            return type.Get(name, null);
        }

        public static object Get(this Type type, string name, object instance)
        {
            return AccessTools.Field(type, name).GetValue(instance);
        }

        public static T Get<T>(this object obj, string name)
        {
            return obj.GetType().Get<T>(name, obj);
        }

        public static T Get<T>(this Type type, string name)
        {
            return type.Get<T>(name, null);
        }

        public static T Get<T>(this Type type, string name, object instance)
        {
            object rtn = type.Get(name, instance);
            if (rtn != null && rtn.GetType().IsAssignableFrom(typeof(T)))
                return (T)rtn;
            else
                return default;
        }

        public static void Set(this object obj, string name, object value)
        {
            obj.GetType().Set(name, value, obj);
        }

        public static void Set(this Type type, string name, object value)
        {
            type.Set(name, value, null);
        }

        public static void Set(this Type type, string name, object value, object instance)
        {
            AccessTools.Field(type, name).SetValue(instance, value);
        }
    }
}
