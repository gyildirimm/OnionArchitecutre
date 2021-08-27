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
        private Type _responseType;

        public CacheAspect(int duration = 600)
        {
            _duration = duration;
            //_responseType = responseType;
            _cacheService = (ICacheService)ServiceTool.ServiceProvider.GetService(typeof(ICacheService));
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";

            #region closeTry

            Type x = ((TypeInfo)invocation.Method.ReturnType).DeclaredFields.ElementAt(0).FieldType;

            //invocation.ReturnValue = Newtonsoft.Json.JsonConvert.DeserializeObject(_cacheService.Get(key), x);
            #endregion
            var isAwaitable = invocation.Method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

            if (_cacheService.IsAdd(key))
            {
                if (!isAwaitable)
                {
                    invocation.ReturnValue = _cacheService.GetObject(key);
                }
                else
                {
                    //invocation.ReturnValue = _cacheService.GetObject(key);
                    //var data = CastTool.DynamicCast(_cacheService.GetObject(key), _responseType);
                    var data = CastTool.dynamicCast.MakeGenericMethod(new[] { x }).Invoke(null, new[] { _cacheService.GetObject(key) });
                    invocation.ReturnValue = data;
                    //invocation.ReturnValue = Task.FromResult((Application.Core.Wrappers.IResponse<List<OrderDto>>)Convert.ChangeType(_cacheService.GetObject(key), _responseType));
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
