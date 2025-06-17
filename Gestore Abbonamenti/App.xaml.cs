using Autofac;
using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View.ShowDialog;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Logic.Mapping;
using System.Data;
using System.Windows;
using Application = System.Windows.Application;
using IContainer = Autofac.IContainer;

namespace Gestore_Abbonamenti
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static IContainer Container { get; set; } = null!;

        public App()
        {
            this.Startup += Application_Startup;

        }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var dummyWindow = new MainWindow();
            dummyWindow.Resources = Application.Current.Resources;
            var splash = new SplashScreen("Avvio in corso...");
            splash.Show();

            try
            {
                await Task.Run(() =>
                {
                    AutoFacInit();
                    AutoMapperInit();
                    LogicFactory.Instance.StartUp.OnStarting();
                });
            }
            catch (Exception ex)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "ERRORE DI AVVIO: " + ex, ShowDialogImage.ERROR);
                Shutdown();
                return;
            }
            finally
            {
                splash.Close();
            }

            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore mostrando MainWindow: " + ex.Message);
                Shutdown();
            }
        }

        static void AutoFacInit()
        {
            try
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
            catch (Exception e)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "AUTOFAC: " + e, ShowDialogImage.ERROR);
            }
        }
        static void AutoMapperInit()
        {
            try
            {
                AutoMapperConfiguration.Configure();
            }
            catch (Exception e)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "AUTOMAPPER: " + e, ShowDialogImage.ERROR);
            }
        }
    }

}
