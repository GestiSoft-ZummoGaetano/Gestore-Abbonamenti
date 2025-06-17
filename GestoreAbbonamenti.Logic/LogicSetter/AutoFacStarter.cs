using Autofac;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Logic.LogicSetter;
using IContainer = Autofac.IContainer;

namespace GestoreAbbonamenti.Logic.LogicSetter;

public static class AutoFacStarter
    {
        static IContainer Container { get; set; } = null!;
        public static void AutofacInit()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LogicCreator>();

            builder.RegisterType<LogicFactory>()
                .AsSelf()
                .As<ILogicFactory>()
                .PropertiesAutowired()
                .SingleInstance()
                .OnActivating(ag => ag.Instance.Initialize());

            builder.RegisterAssemblyTypes(typeof(LogicFactory).Assembly)
                .Where(t => t.Name.EndsWith("Logic"))
                .AsImplementedInterfaces();

            Container = builder.Build();

            Container.Resolve<LogicCreator>();
        }
    }
