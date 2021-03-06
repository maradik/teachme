﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using TeachMe.Services.Notifications;
using TeachMe.Services.Jobs;

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
            kernel.Bind<ApplicationUserManager>().ToMethod(context => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            kernel.Bind(x => x.FromThisAssembly()
                              .SelectAllClasses()
                              .BindDefaultInterface()
                              .Configure(y => y.InRequestScope()));
            kernel.Bind(x => x.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom(typeof(ICustomSmsService))
                              .BindAllInterfaces()
                              .Configure(y => y.InSingletonScope()));
            kernel.Bind(x => x.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom(typeof(IJobActionCustomHandler))
                              .BindAllInterfaces()
                              .Configure(y => y.InSingletonScope()));
        }
    }
}