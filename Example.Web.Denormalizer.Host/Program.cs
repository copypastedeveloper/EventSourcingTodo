using System;
using System.ServiceProcess;
using System.Threading;
using log4net;
using StructureMap;

namespace Example.Web.Denormalizer.Host
{
    class Program : ServiceBase
    {
        public const string GlobalWebDenormalizerHostStarted = @"Global\Web.Denormalizer.Host.Started";
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            new StartupTask().Execute(ObjectFactory.Container);

            var host = new WebDenormalizerHost();

            if (Environment.UserInteractive)
            {
                var fullName = host.GetType().FullName;
                Console.WriteLine("{0}::starting...", fullName);

                EventWaitHandle startedEvent;
                if (!EventWaitHandle.TryOpenExisting(GlobalWebDenormalizerHostStarted, out startedEvent))
                {
                    startedEvent = new EventWaitHandle(false, EventResetMode.ManualReset, GlobalWebDenormalizerHostStarted);
                }
                    
                host.StartUp();
 
                // Signal the event so that all the waiting clients can proceed
                startedEvent.Set();

                Console.WriteLine("{0}::ready (ENTER to shutdown)", fullName);
                Console.ReadLine();

                host.ShutDown();

                Console.WriteLine("{0}::stopped (ENTER to exit)", fullName);
                Console.ReadLine();
                Environment.Exit(1);
            }
            else
            {
                var servicesToRun = new ServiceBase[] { host };
                Run(servicesToRun);
            }
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ILog logger;
            try
            {
                logger = ObjectFactory.GetInstance<ILog>();
            }
            catch (StructureMapException)
            {
                logger = LogManager.GetLogger(typeof (StartupTask));
            }

            logger.Error("Unhandled exception.", e.ExceptionObject as Exception);
            Environment.Exit(1);
        }
    }
}
