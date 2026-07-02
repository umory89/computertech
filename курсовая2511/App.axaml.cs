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
                    context.Database.EnsureCreated();

                    var authService = new курсовая2511.TechAccounting.Application.Dtos.Services.AuthService(context);
                    var reportService = new курсовая2511.TechAccounting.Application.Dtos.Services.ReportService(context);
                    var equipmentService = new курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService(context);
                    var employeeService = new курсовая2511.TechAccounting.Application.Dtos.Services.EmployeeService(context);
                    var assignmentService = new курсовая2511.TechAccounting.Application.Dtos.Services.AssignmentService(context);

                    var authViewModel = new AuthViewModel(authService, reportService, equipmentService, employeeService, assignmentService);

                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = authViewModel
                    };
                }
                catch (Exception ex)
                {
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