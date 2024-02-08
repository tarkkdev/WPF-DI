# WPF-DI
WPF DI Implementation

### WPF application with built-in .NET Core Dependency Injection

#### Progress

1. Install *Microsoft.Extensions.DependencyInjection* Nuget package
2. Install *Microsoft.Extensions.Hosting* Nuget package
3. At App.xaml.cs
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

### WPF Multi-Window with DI

#### Progress

1. Follow the same steps as above
2. Create new DataWindow class
3. Create a directory called Helpers
4. Create generic IAbstractFactory with Create method
    ```c#
        ﻿namespace WpfMultiWindow.Helpers
        {
            public interface IAbstractFactory<T>
            {
                T Create();
            }
        }
    ```
5. Create AbstractFactory class that implements IAbstractFactory
    ```c#
        ﻿using System;

        namespace WpfMultiWindow.Helpers;

        public class AbstractFactory<T> : IAbstractFactory<T>
        {
            private readonly Func<T> _factory;

            public AbstractFactory(Func<T> factory)
            {
                _factory = factory;
            }

            public T Create()
            {
                return _factory();
            }
        }
    ```
6. Create ServiceExtensions class for DI extension method AddWindowFactory
    ```c#
        ﻿using Microsoft.Extensions.DependencyInjection;
        using System;

        namespace WpfMultiWindow.Helpers;

        public static class ServiceExtensions
        {
            public static void AddWindowFactory<TWindow>(this IServiceCollection services) 
                where TWindow : class
            {
                services.AddTransient<TWindow>();
                services.AddSingleton<Func<TWindow>>(x => () => x.GetService<TWindow>()!);
                services.AddSingleton<IAbstractFactory<TWindow>, AbstractFactory<TWindow>>();
            }
        }  
    ```
    1. services.AddTransient\<TWindow>() // added a Window
    2. services.AddSingleton<Func\<TWindow>>(...) // added a delegate that is going to create that window, when we run the delegate
        - Delegate gets added to DI not just the Window
        - This is a delegate of the Window, this will get run, whenever we run the delegate
    3. services.AddSingleton\<IAbstractFactory\<TWindow>, AbstractFactory\<TWindow>>() // added a abstract factory
        - Run the delegate when call to Create method
    4. Basically when App.xaml.cs services.AddWindowFactory run
        - It is going to add a child window as a Transient
        - It is going to add a Func of child Window, that will get the child Window
        - It is going to add an IAbstractFactory of type child Window to give us a child Window

7. At App.xaml.cs add AddWindowFactory to DI
    ==services.AddWindowFactory\<DataWindow>();==
    ```c#
         public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<IDataAccess, DataAccess>();
                    services.AddWindowFactory<DataWindow>();
                })
                .Build();
        }
    ```
8. At openWindow_Click handler, call AbstractFactory Create method to create new child Window
    ```c#
        private void openWindow_Click(object sender, RoutedEventArgs e)
        {
            _factory.Create().Show();
        }
    ```


