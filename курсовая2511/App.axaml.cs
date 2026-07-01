using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;

namespace курсовая2511
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                try
                {
                    var context = new global::TechAccounting.Data.AppDbContext();

                    // Гарантированно создаёт базу и таблицы, если их ещё нет
                    // (если используешь миграции — замени на context.Database.Migrate())
                    context.Database.EnsureCreated();

                    var authService = new курсовая2511.TechAccounting.Application.Dtos.Services.AuthService(context);
                    var reportService = new курсовая2511.TechAccounting.Application.Dtos.Services.ReportService(context);
                    var equipmentService = new курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService(context);

                    var authViewModel = new AuthViewModel(authService, reportService, equipmentService);

                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = authViewModel
                    };
                }
                catch (Exception ex)
                {
                    // Раньше при ошибке здесь приложение либо не стартовало,
                    // либо оставалось на старой сборке. Теперь ошибка видна сразу.
                    desktop.MainWindow = new Window
                    {
                        Width = 600,
                        Height = 300,
                        Content = new TextBlock
                        {
                            Text = "Ошибка запуска приложения:\n\n" + ex,
                            Margin = new Avalonia.Thickness(20),
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap
                        }
                    };
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
