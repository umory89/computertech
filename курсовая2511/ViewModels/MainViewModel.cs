using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EquipmentDto = курсовая2511.TechAccounting.Application.Dtos.DTOs.EquipmentDto;
using EmployeeDto = курсовая2511.TechAccounting.Application.Dtos.DTOs.EmployeeDto;
using AssignmentDto = курсовая2511.TechAccounting.Application.DTOs.AssignmentDto;

namespace курсовая2511
{
    // Вспомогательная модель для отображения активной выдачи в списке "Возврат"
    // (в самой AssignmentDto нет имён - только Id сотрудника/техники, поэтому
    // собираем читаемую строку здесь, во ViewModel, не трогая твои DTO).
    public class ActiveAssignmentItem
    {
        public Guid AssignmentId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string DisplayText => $"{EquipmentName} — у {EmployeeName} (с {AssignedDate:dd.MM.yyyy})";
    }

    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.AuthService _authService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.ReportService _reportService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService _equipmentService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.EmployeeService _employeeService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.AssignmentService _assignmentService;

        // ---------- Вход / регистрация ----------
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _regEmail = string.Empty;
        private string _regUsername = string.Empty;
        private string _regPassword = string.Empty;
        private string _statusMessage = string.Empty;

        private bool _isRegisterMode;
        private bool _isLoggedIn;
        private int _currentTab;

        private int _inStockCount;
        private int _issuedCount;
        private int _underRepairCount;

        private IEnumerable<EquipmentDto> _equipmentItems = new List<EquipmentDto>();

        // ---------- Выдача техники ----------
        private IEnumerable<EmployeeDto> _employeeItems = new List<EmployeeDto>();
        private IEnumerable<EquipmentDto> _availableEquipmentItems = new List<EquipmentDto>();
        private EmployeeDto? _selectedEmployee;
        private EquipmentDto? _selectedEquipmentForIssue;
        private string _issueCondition = string.Empty;
        private string _assignmentStatusMessage = string.Empty;

        // ---------- Возврат техники ----------
        private IEnumerable<ActiveAssignmentItem> _activeAssignments = new List<ActiveAssignmentItem>();
        private ActiveAssignmentItem? _selectedActiveAssignment;
        private string _returnCondition = string.Empty;
        private string _returnStatusMessage = string.Empty;

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

        public IEnumerable<EquipmentDto> EquipmentItems
        {
            get => _equipmentItems;
            set { _equipmentItems = value; OnPropertyChanged(); }
        }

        public IEnumerable<EmployeeDto> EmployeeItems
        {
            get => _employeeItems;
            set { _employeeItems = value; OnPropertyChanged(); }
        }

        public IEnumerable<EquipmentDto> AvailableEquipmentItems
        {
            get => _availableEquipmentItems;
            set { _availableEquipmentItems = value; OnPropertyChanged(); }
        }

        public EmployeeDto? SelectedEmployee
        {
            get => _selectedEmployee;
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        public EquipmentDto? SelectedEquipmentForIssue
        {
            get => _selectedEquipmentForIssue;
            set { _selectedEquipmentForIssue = value; OnPropertyChanged(); }
        }

        public string IssueCondition
        {
            get => _issueCondition;
            set { _issueCondition = value; OnPropertyChanged(); }
        }

        public string AssignmentStatusMessage
        {
            get => _assignmentStatusMessage;
            set { _assignmentStatusMessage = value; OnPropertyChanged(); }
        }

        public IEnumerable<ActiveAssignmentItem> ActiveAssignments
        {
            get => _activeAssignments;
            set { _activeAssignments = value; OnPropertyChanged(); }
        }

        public ActiveAssignmentItem? SelectedActiveAssignment
        {
            get => _selectedActiveAssignment;
            set { _selectedActiveAssignment = value; OnPropertyChanged(); }
        }

        public string ReturnCondition
        {
            get => _returnCondition;
            set { _returnCondition = value; OnPropertyChanged(); }
        }

        public string ReturnStatusMessage
        {
            get => _returnStatusMessage;
            set { _returnStatusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand SelectEquipmentTabCommand { get; }
        public ICommand SelectAssignmentsTabCommand { get; }
        public ICommand SelectReportsTabCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand AssignEquipmentCommand { get; }
        public ICommand ReturnEquipmentCommand { get; }

        public AuthViewModel(
            курсовая2511.TechAccounting.Application.Dtos.Services.AuthService authService,
            курсовая2511.TechAccounting.Application.Dtos.Services.ReportService reportService,
            курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService equipmentService,
            курсовая2511.TechAccounting.Application.Dtos.Services.EmployeeService employeeService,
            курсовая2511.TechAccounting.Application.Dtos.Services.AssignmentService assignmentService)
        {
            _authService = authService;
            _reportService = reportService;
            _equipmentService = equipmentService;
            _employeeService = employeeService;
            _assignmentService = assignmentService;

            LoginCommand = new TabRelayCommand(async () => await ExecuteLoginAsync());
            RegisterCommand = new TabRelayCommand(async () => await ExecuteRegisterAsync());
            ToggleModeCommand = new TabRelayCommand(() => { IsRegisterMode = !IsRegisterMode; StatusMessage = ""; });

            SelectEquipmentTabCommand = new TabRelayCommand(() => CurrentTab = 0);
            SelectAssignmentsTabCommand = new TabRelayCommand(async () => { CurrentTab = 1; await RefreshAssignmentFormDataAsync(); });
            SelectReportsTabCommand = new TabRelayCommand(() => CurrentTab = 2);

            AssignEquipmentCommand = new TabRelayCommand(async () => await ExecuteAssignEquipmentAsync());
            ReturnEquipmentCommand = new TabRelayCommand(async () => await ExecuteReturnEquipmentAsync());

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
                    await RefreshAssignmentFormDataAsync();
                    IsLoggedIn = true;
                }
                else
                {
                    StatusMessage = "Неверный логин или пароль.";
                }
            }
            catch (Exception ex)
            {
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

        // Подгружает всё, что нужно для формы "Выдача / Возврат"
        private async Task RefreshAssignmentFormDataAsync()
        {
            try
            {
                var allEquipment = await _equipmentService.GetAllAsync();
                var allEmployees = await _employeeService.GetAllAsync();
                var allAssignments = await _assignmentService.GetAllAsync();

                EquipmentItems = allEquipment;
                EmployeeItems = allEmployees;

                // Свободная техника - фильтруем на клиенте по статусу InStock
                AvailableEquipmentItems = allEquipment.Where(e => e.Status == "InStock").ToList();

                // Активные выдачи (ReturnDate == null) с читаемыми именами
                var employeesById = allEmployees.ToDictionary(e => e.Id);
                var equipmentById = allEquipment.ToDictionary(e => e.Id);

                ActiveAssignments = allAssignments
                    .Where(a => a.ReturnDate == null)
                    .Select(a => new ActiveAssignmentItem
                    {
                        AssignmentId = a.Id,
                        EmployeeName = employeesById.TryGetValue(a.EmployeeId, out var emp)
                            ? $"{emp.LastName} {emp.FirstName}"
                            : "Неизвестный сотрудник",
                        EquipmentName = equipmentById.TryGetValue(a.EquipmentId, out var eq)
                            ? eq.Name
                            : "Неизвестная техника",
                        AssignedDate = a.AssignedDate
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                AssignmentStatusMessage = "Не удалось загрузить данные: " + ex.Message;
            }
        }

        // Выдача техники сотруднику + цепная реакция обновлений
        private async Task ExecuteAssignEquipmentAsync()
        {
            AssignmentStatusMessage = string.Empty;

            if (SelectedEmployee == null)
            {
                AssignmentStatusMessage = "Выберите сотрудника.";
                return;
            }

            if (SelectedEquipmentForIssue == null)
            {
                AssignmentStatusMessage = "Выберите технику.";
                return;
            }

            try
            {
                var dto = new AssignmentDto
                {
                    EmployeeId = SelectedEmployee.Id,
                    EquipmentId = SelectedEquipmentForIssue.Id,
                    ConditionAtIssue = string.IsNullOrWhiteSpace(IssueCondition) ? "Норма" : IssueCondition
                };

                await _assignmentService.CreateAsync(dto);

                AssignmentStatusMessage = $"Техника \"{SelectedEquipmentForIssue.Name}\" выдана сотруднику {SelectedEmployee.LastName} {SelectedEmployee.FirstName}.";

                SelectedEmployee = null;
                SelectedEquipmentForIssue = null;
                IssueCondition = string.Empty;

                // Цепная реакция: перечитываем всё, что зависит от статуса техники
                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                AssignmentStatusMessage = "Ошибка: " + ex.Message;
            }
        }

        // Приём техники обратно на склад + цепная реакция обновлений
        private async Task ExecuteReturnEquipmentAsync()
        {
            ReturnStatusMessage = string.Empty;

            if (SelectedActiveAssignment == null)
            {
                ReturnStatusMessage = "Выберите активную выдачу для возврата.";
                return;
            }

            try
            {
                var condition = string.IsNullOrWhiteSpace(ReturnCondition) ? "Норма" : ReturnCondition;
                await _assignmentService.ReturnEquipmentAsync(SelectedActiveAssignment.AssignmentId, condition);

                ReturnStatusMessage = $"Техника \"{SelectedActiveAssignment.EquipmentName}\" принята на склад.";

                SelectedActiveAssignment = null;
                ReturnCondition = string.Empty;

                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                ReturnStatusMessage = "Ошибка: " + ex.Message;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
