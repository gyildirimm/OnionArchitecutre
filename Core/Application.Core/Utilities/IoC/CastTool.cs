using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Utilities.IoC
{
    public static class CastTool
    {
        public static T Cast<T>(object b ) where T : class
        {
            return b as T;
        }

        private static readonly MethodInfo CastInfo = typeof(CastTool).GetMethod("Cast");

        public static readonly MethodInfo dynamicCast = typeof(CastTool).GetMethod("DynamicCast");

        public static Task<T> DynamicCast<T>(object source) where T : class
        {
            return Task.FromResult((T)CastInfo.MakeGenericMethod(new[] { typeof(T) }).Invoke(null, new[] { source }));
        }
    }
}
