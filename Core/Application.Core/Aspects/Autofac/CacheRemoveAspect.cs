using Application.Core.CrossCuttingConcerns;
using Application.Core.Utilities.Interceptors;
using Castle.DynamicProxy;

namespace Application.Core.Aspects.Autofac
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheService _cacheManager;

        public CacheRemoveAspect(ICacheService cacheService,string pattern)
        {
            _pattern = pattern;
            _cacheManager = cacheService;
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
