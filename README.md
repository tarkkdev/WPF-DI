# WPF-DI
WPF DI Implementation

### WPF application with built-in .NET Core Dependency Injection

#### Progress

1. Install *Microsoft.Extensions.DependencyInjection* Nuget package
2. Install *Microsoft.Extensions.Hosting* Nuget package
3. At App.xaml
	1. Add property for IHost
	2. At Costructor initialize IHost property by CreateDefaultBuilder, which has pre-configured defaults, such as IConfiguration and ILoggerFactory
	3. Add *MainWindow* as singleton to DI services
	4. Override OnStartup method and call IHost StartAsync 
	5. OnStartup method using DI to call and initialize **MainWindow**
	6. Override OnExit method and call IHost StopAsync
	```c#
		public partial class App : Application
            {
                public static IHost? AppHost { get; private set; }

                public App()
                {
                    AppHost = Host.CreateDefaultBuilder()
                        .ConfigureServices((hostContext,  services) => 
                        {
                            services.AddSingleton<MainWindow>();
                        })
                        .Build();
                }

                protected override async void OnStartup(StartupEventArgs e)
                {
                    await AppHost!.StartAsync();

                    var startupWindow = AppHost.Services.GetRequiredService<MainWindow>();
                    startupWindow.Show();
                    base.OnStartup(e);
                }

                protected override async void OnExit(ExitEventArgs e)
                {
                    await AppHost!.StopAsync();
                    base.OnExit(e);
                }
            }
	```


