using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace курсовая2511
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        // ВАЖНО: namespace здесь должен ТОЧНО совпадать с тем,
        // что используется в App.axaml.cs при создании сервисов.
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.AuthService _authService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.ReportService _reportService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService _equipmentService;

        // Поля авторизации и регистрации
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _regEmail = string.Empty;
        private string _regUsername = string.Empty;
        private string _regPassword = string.Empty;
        private string _statusMessage = string.Empty;

        // Флаги переключения экранов и вкладок
        private bool _isRegisterMode;
        private bool _isLoggedIn;
        private int _currentTab;

        // Свойства дашборда администратора (Метрики из ТЗ)
        private int _inStockCount;
        private int _issuedCount;
        private int _underRepairCount;

        // Коллекция для вывода всей техники предприятия
        private IEnumerable<курсовая2511.TechAccounting.Application.Dtos.DTOs.EquipmentDto> _equipmentItems
            = new List<курсовая2511.TechAccounting.Application.Dtos.DTOs.EquipmentDto>();

        // Свойства Binding
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string RegEmail { get => _regEmail; set { _regEmail = value; OnPropertyChanged(); } }
        public string RegUsername { get => _regUsername; set { _regUsername = value; OnPropertyChanged(); } }
        public string RegPassword { get => _regPassword; set { _regPassword = value; OnPropertyChanged(); } }
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

        public bool IsRegisterMode { get => _isRegisterMode; set { _isRegisterMode = value; OnPropertyChanged(); } }
        public bool IsLoggedIn { get => _isLoggedIn; set { _isLoggedIn = value; OnPropertyChanged(); } }
        public int CurrentTab { get => _currentTab; set { _currentTab = value; OnPropertyChanged(); } }

        public int InStockCount { get => _inStockCount; set { _inStockCount = value; OnPropertyChanged(); } }
        public int IssuedCount { get => _issuedCount; set { _issuedCount = value; OnPropertyChanged(); } }
        public int UnderRepairCount { get => _underRepairCount; set { _underRepairCount = value; OnPropertyChanged(); } }

        public IEnumerable<курсовая2511.TechAccounting.Application.Dtos.DTOs.EquipmentDto> EquipmentItems
        {
            get => _equipmentItems;
            set { _equipmentItems = value; OnPropertyChanged(); }
        }

        // Команды интерфейса
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand SelectEquipmentTabCommand { get; }
        public ICommand SelectAssignmentsTabCommand { get; }
        public ICommand SelectReportsTabCommand { get; }
        public ICommand LogoutCommand { get; }

        public AuthViewModel(
            курсовая2511.TechAccounting.Application.Dtos.Services.AuthService authService,
            курсовая2511.TechAccounting.Application.Dtos.Services.ReportService reportService,
            курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService equipmentService)
        {
            _authService = authService;
            _reportService = reportService;
            _equipmentService = equipmentService;

            LoginCommand = new TabRelayCommand(async () => await ExecuteLoginAsync());
            RegisterCommand = new TabRelayCommand(async () => await ExecuteRegisterAsync());
            ToggleModeCommand = new TabRelayCommand(() => { IsRegisterMode = !IsRegisterMode; StatusMessage = ""; });

            SelectEquipmentTabCommand = new TabRelayCommand(() => CurrentTab = 0);
            SelectAssignmentsTabCommand = new TabRelayCommand(() => CurrentTab = 1);
            SelectReportsTabCommand = new TabRelayCommand(() => CurrentTab = 2);

            LogoutCommand = new TabRelayCommand(() =>
            {
                IsLoggedIn = false;
                Username = string.Empty;
                Password = string.Empty;
                StatusMessage = string.Empty;
            });
        }

        private async Task ExecuteLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Заполните все поля!";
                return;
            }

            StatusMessage = "Проверка...";

            try
            {
                var user = await _authService.LoginAsync(Username, Password);

                if (user != null)
                {
                    StatusMessage = string.Empty;
                    await LoadDashboardStatsAsync();
                    EquipmentItems = await _equipmentService.GetAllAsync();
                    IsLoggedIn = true;
                }
                else
                {
                    StatusMessage = "Неверный логин или пароль.";
                }
            }
            catch (Exception ex)
            {
                // Если раньше падало молча — теперь увидишь причину прямо в интерфейсе
                StatusMessage = "Ошибка входа: " + ex.Message;
            }
        }

        private async Task ExecuteRegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(RegUsername) || string.IsNullOrWhiteSpace(RegEmail) || string.IsNullOrWhiteSpace(RegPassword))
            {
                StatusMessage = "Заполните все поля!";
                return;
            }

            StatusMessage = "Регистрация...";

            try
            {
                var result = await _authService.RegisterAsync(RegUsername, RegEmail, RegPassword);
                StatusMessage = result.Message;

                if (result.IsSuccess)
                {
                    Username = RegUsername;
                    IsRegisterMode = false;
                    RegUsername = string.Empty;
                    RegEmail = string.Empty;
                    RegPassword = string.Empty;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка регистрации: " + ex.Message;
            }
        }

        public async Task LoadDashboardStatsAsync()
        {
            try
            {
                var summary = await _reportService.GetEquipmentSummaryAsync();
                InStockCount = summary.InStockCount;
                IssuedCount = summary.IssuedCount;
                UnderRepairCount = summary.UnderRepairCount;
            }
            catch
            {
                InStockCount = 0;
                IssuedCount = 0;
                UnderRepairCount = 0;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
