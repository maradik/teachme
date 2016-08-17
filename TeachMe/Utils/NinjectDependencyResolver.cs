using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;

namespace TeachMe.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind(x => x.FromThisAssembly()
                              .SelectAllClasses()
                              .BindDefaultInterface()
                              .Configure(y => y.InRequestScope()));
        }
    }
}