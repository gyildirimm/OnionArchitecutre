using Application.Core.CrossCuttingConcerns;
using Application.Core.Utilities.Interceptors;
using Application.Core.Utilities.IoC;
using Application.Core.Wrappers;
using Castle.DynamicProxy;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Aspects.Autofac
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheService _cacheService;

        public CacheAspect(int duration = 600)
        {
            _duration = duration;
            _cacheService = (ICacheService)ServiceTool.ServiceProvider.GetService(typeof(ICacheService));
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";

            #region closeTry

            
            #endregion
            var isAwaitable = invocation.Method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

            if (_cacheService.IsAdd(key))
            {
                if (!isAwaitable)
                {
                    invocation.ReturnValue = _cacheService.Get(key);
                }
                else
                {
                    Type x = ((TypeInfo)invocation.Method.ReturnType).DeclaredFields.ElementAt(0).FieldType;
                    var data = CastTool.dynamicCast.MakeGenericMethod(new[] { x }).Invoke(null, new[] { _cacheService.Get(key) });

                    invocation.ReturnValue = data;
                }
                return;
            }
            invocation.Proceed();

            if (isAwaitable)
            {
                var returnValue = invocation.ReturnValue;
                var task = (Task)returnValue;

                task.ContinueWith((antecedent) =>
                {
                    var result =
                        antecedent.GetType()
                                  .GetProperty("Result")
                                  .GetValue(antecedent, null);
                    _cacheService.Add(key, result, _duration);
                });
                return;
            }

            _cacheService.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
