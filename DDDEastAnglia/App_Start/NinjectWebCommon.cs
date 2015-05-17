using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.HtmlControls;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.VotingData;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Stop")]

namespace DDDEastAnglia.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<IDateTimeOffsetProvider>().To<LocalDateTimeOffsetProvider>();
            kernel.Bind<IDataProvider>().To<DataProvider>();
            kernel.Bind<IRenderer>()
                .ToMethod(x =>
                {
                    var htmlEmailTemplateFileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DDDEastAnglia.EmailTemplate.html");
                    using (StreamReader reader = new StreamReader(htmlEmailTemplateFileStream))
                    {
                        var htmlEmailTemplate = reader.ReadToEnd();
                        return new HtmlRenderer(htmlEmailTemplate);
                    }
                })
                .InSingletonScope();

            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(typeof(NinjectWebCommon).Assembly);
        }
    }
}
