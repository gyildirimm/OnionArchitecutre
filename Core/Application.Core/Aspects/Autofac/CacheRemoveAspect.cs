using Application.Core.CrossCuttingConcerns;
using Application.Core.Utilities.Interceptors;
using Application.Core.Utilities.IoC;
using Castle.DynamicProxy;

namespace Application.Core.Aspects.Autofac
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheService _cacheService;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheService = (ICacheService)ServiceTool.ServiceProvider.GetService(typeof(ICacheService));
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheService.RemoveByPattern(_pattern);
        }
    }
}
