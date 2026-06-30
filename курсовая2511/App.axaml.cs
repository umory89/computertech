using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

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
                // Инициализируем ваш контекст базы данных LocalDB по точному глобальному пути
                var context = new global::TechAccounting.Data.AppDbContext();

                // Создаем прикладные сервисы по точным путям
                var authService = new global::курсовая2511.TechAccounting.Application.Dtos.Services.AuthService(context);
                var reportService = new global::курсовая2511.TechAccounting.Application.Dtos.Services.ReportService(context);
                var equipmentService = new global::курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService(context);

                // Передаем все сервисы в единую AuthViewModel
                var authViewModel = new global::курсовая2511.AuthViewModel(authService, reportService, equipmentService);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = authViewModel
                };
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
