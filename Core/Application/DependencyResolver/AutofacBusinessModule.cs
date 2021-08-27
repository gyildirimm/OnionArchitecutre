using Application.Core.Utilities.Interceptors;
using Application.Core.Wrappers;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DependencyResolver
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterType(typeof(Response<List<OrderDto>>)).As(typeof(IResponse<List<OrderDto>>)).InstancePerDependency();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
