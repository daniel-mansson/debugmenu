#nullable enable
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public static class DebugMenuUtils {
        public static string GetPropertyType(Type type) {
            if(type == typeof(int) || type == typeof(float) || type == typeof(double)) {
                return "number";
            }

            if(type == typeof(bool)) {
                return "boolean";
            }

            if(type == typeof(string)) {
                return "string";
            }

            return "unknown";
        }

        public static object? ToValue(JObject payload, ParameterInfo p) {
            var property = payload[p.Name];
            if(property == null) {
                return null;
            }

            if(p.ParameterType == typeof(bool)) {
                return property.Value<bool>();
            }

            if(p.ParameterType == typeof(int)) {
                return property.Value<int>();
            }

            if(p.ParameterType == typeof(long)) {
                return property.Value<long>();
            }

            if(p.ParameterType == typeof(float)) {
                return property.Value<float>();
            }

            if(p.ParameterType == typeof(double)) {
                return property.Value<double>();
            }

            if(p.ParameterType == typeof(string)) {
                return property.Value<string>();
            }

            return property.Value<object>();
        }
    }
}
