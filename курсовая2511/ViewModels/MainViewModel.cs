using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using EquipmentDto = курсовая2511.TechAccounting.Application.Dtos.DTOs.EquipmentDto;
using EmployeeDto = курсовая2511.TechAccounting.Application.Dtos.DTOs.EmployeeDto;
using AssignmentDto = курсовая2511.TechAccounting.Application.DTOs.AssignmentDto;

namespace курсовая2511
{
    public class ActiveAssignmentItem
    {
        public Guid AssignmentId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string DisplayText => $"{EquipmentName} — у {EmployeeName} (с {AssignedDate:dd.MM.yyyy})";
    }

    public class HistoryDisplayItem
    {
        public string EquipmentName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string DisplayText => $"[{ActionDate:dd.MM.yyyy HH:mm}] {ActionType} — {EquipmentName}" +
            (string.IsNullOrEmpty(EmployeeName) ? "" : $" (сотрудник: {EmployeeName})") +
            $". {Details}";
    }

    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.AuthService _authService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.ReportService _reportService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService _equipmentService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.EmployeeService _employeeService;
        private readonly курсовая2511.TechAccounting.Application.Dtos.Services.AssignmentService _assignmentService;

        private readonly AppDbContext _context;

  
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

        private IEnumerable<EmployeeDto> _employeeItems = new List<EmployeeDto>();
        private IEnumerable<EquipmentDto> _availableEquipmentItems = new List<EquipmentDto>();
        private EmployeeDto? _selectedEmployee;
        private EquipmentDto? _selectedEquipmentForIssue;
        private string _issueCondition = string.Empty;
        private string _assignmentStatusMessage = string.Empty;


        private IEnumerable<ActiveAssignmentItem> _activeAssignments = new List<ActiveAssignmentItem>();
        private ActiveAssignmentItem? _selectedActiveAssignment;
        private string _returnCondition = string.Empty;
        private string _returnStatusMessage = string.Empty;


        private string _newEmployeeFirstName = string.Empty;
        private string _newEmployeeLastName = string.Empty;
        private string _newEmployeeDepartment = string.Empty;
        private string _newEmployeePosition = string.Empty;
        private string _newEmployeeEmail = string.Empty;
        private string _newEmployeePhone = string.Empty;
        private string _addEmployeeStatusMessage = string.Empty;


        private IEnumerable<HistoryDisplayItem> _historyItems = new List<HistoryDisplayItem>();


        private IEnumerable<EquipmentDto> _repairableEquipmentItems = new List<EquipmentDto>();
        private IEnumerable<EquipmentDto> _underRepairEquipmentItems = new List<EquipmentDto>();
        private EquipmentDto? _selectedEquipmentForRepair;
        private EquipmentDto? _selectedEquipmentForRepairReturn;
        private string _repairStatusMessage = string.Empty;
        private IEnumerable<курсовая2511.Models.EquipmentType> _equipmentTypes = new List<курсовая2511.Models.EquipmentType>();
        private курсовая2511.Models.EquipmentType? _selectedEquipmentType;
        private string _newEquipmentName = string.Empty;
        private string _newEquipmentSerialNumber = string.Empty;
        private string _newEquipmentInventoryNumber = string.Empty;
        private string _newEquipmentManufacturer = string.Empty;
        private string _newEquipmentModel = string.Empty;
        private string _newEquipmentLocation = string.Empty;
        private string _newEquipmentPrice = string.Empty;
        private string _addEquipmentStatusMessage = string.Empty;

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

        public string NewEmployeeFirstName { get => _newEmployeeFirstName; set { _newEmployeeFirstName = value; OnPropertyChanged(); } }
        public string NewEmployeeLastName { get => _newEmployeeLastName; set { _newEmployeeLastName = value; OnPropertyChanged(); } }
        public string NewEmployeeDepartment { get => _newEmployeeDepartment; set { _newEmployeeDepartment = value; OnPropertyChanged(); } }
        public string NewEmployeePosition { get => _newEmployeePosition; set { _newEmployeePosition = value; OnPropertyChanged(); } }
        public string NewEmployeeEmail { get => _newEmployeeEmail; set { _newEmployeeEmail = value; OnPropertyChanged(); } }
        public string NewEmployeePhone { get => _newEmployeePhone; set { _newEmployeePhone = value; OnPropertyChanged(); } }
        public string AddEmployeeStatusMessage { get => _addEmployeeStatusMessage; set { _addEmployeeStatusMessage = value; OnPropertyChanged(); } }

        public IEnumerable<HistoryDisplayItem> HistoryItems
        {
            get => _historyItems;
            set { _historyItems = value; OnPropertyChanged(); }
        }

        public IEnumerable<EquipmentDto> RepairableEquipmentItems
        {
            get => _repairableEquipmentItems;
            set { _repairableEquipmentItems = value; OnPropertyChanged(); }
        }

        public IEnumerable<EquipmentDto> UnderRepairEquipmentItems
        {
            get => _underRepairEquipmentItems;
            set { _underRepairEquipmentItems = value; OnPropertyChanged(); }
        }

        public EquipmentDto? SelectedEquipmentForRepair
        {
            get => _selectedEquipmentForRepair;
            set { _selectedEquipmentForRepair = value; OnPropertyChanged(); }
        }

        public EquipmentDto? SelectedEquipmentForRepairReturn
        {
            get => _selectedEquipmentForRepairReturn;
            set { _selectedEquipmentForRepairReturn = value; OnPropertyChanged(); }
        }

        public string RepairStatusMessage
        {
            get => _repairStatusMessage;
            set { _repairStatusMessage = value; OnPropertyChanged(); }
        }

        public IEnumerable<курсовая2511.Models.EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set { _equipmentTypes = value; OnPropertyChanged(); }
        }

        public курсовая2511.Models.EquipmentType? SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set { _selectedEquipmentType = value; OnPropertyChanged(); }
        }

        public string NewEquipmentName { get => _newEquipmentName; set { _newEquipmentName = value; OnPropertyChanged(); } }
        public string NewEquipmentSerialNumber { get => _newEquipmentSerialNumber; set { _newEquipmentSerialNumber = value; OnPropertyChanged(); } }
        public string NewEquipmentInventoryNumber { get => _newEquipmentInventoryNumber; set { _newEquipmentInventoryNumber = value; OnPropertyChanged(); } }
        public string NewEquipmentManufacturer { get => _newEquipmentManufacturer; set { _newEquipmentManufacturer = value; OnPropertyChanged(); } }
        public string NewEquipmentModel { get => _newEquipmentModel; set { _newEquipmentModel = value; OnPropertyChanged(); } }
        public string NewEquipmentLocation { get => _newEquipmentLocation; set { _newEquipmentLocation = value; OnPropertyChanged(); } }
        public string NewEquipmentPrice { get => _newEquipmentPrice; set { _newEquipmentPrice = value; OnPropertyChanged(); } }
        public string AddEquipmentStatusMessage { get => _addEquipmentStatusMessage; set { _addEquipmentStatusMessage = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand SelectEquipmentTabCommand { get; }
        public ICommand SelectAssignmentsTabCommand { get; }
        public ICommand SelectReportsTabCommand { get; }
        public ICommand SelectEmployeesTabCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand AssignEquipmentCommand { get; }
        public ICommand ReturnEquipmentCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand AddEquipmentCommand { get; }
        public ICommand SendToRepairCommand { get; }
        public ICommand ReturnFromRepairCommand { get; }

        public AuthViewModel(
            курсовая2511.TechAccounting.Application.Dtos.Services.AuthService authService,
            курсовая2511.TechAccounting.Application.Dtos.Services.ReportService reportService,
            курсовая2511.TechAccounting.Application.Dtos.Services.EquipmentService equipmentService,
            курсовая2511.TechAccounting.Application.Dtos.Services.EmployeeService employeeService,
            курсовая2511.TechAccounting.Application.Dtos.Services.AssignmentService assignmentService,
            AppDbContext context)
        {
            _authService = authService;
            _reportService = reportService;
            _equipmentService = equipmentService;
            _employeeService = employeeService;
            _assignmentService = assignmentService;
            _context = context;

            LoginCommand = new TabRelayCommand(async () => await ExecuteLoginAsync());
            RegisterCommand = new TabRelayCommand(async () => await ExecuteRegisterAsync());
            ToggleModeCommand = new TabRelayCommand(() => { IsRegisterMode = !IsRegisterMode; StatusMessage = ""; });

            SelectEquipmentTabCommand = new TabRelayCommand(async () => { CurrentTab = 0; await LoadEquipmentTypesAsync(); });
            SelectAssignmentsTabCommand = new TabRelayCommand(async () => { CurrentTab = 1; await RefreshAssignmentFormDataAsync(); });
            SelectReportsTabCommand = new TabRelayCommand(async () => { CurrentTab = 2; await RefreshReportsDataAsync(); });
            SelectEmployeesTabCommand = new TabRelayCommand(async () => { CurrentTab = 3; await RefreshAssignmentFormDataAsync(); });

            AssignEquipmentCommand = new TabRelayCommand(async () => await ExecuteAssignEquipmentAsync());
            ReturnEquipmentCommand = new TabRelayCommand(async () => await ExecuteReturnEquipmentAsync());
            AddEmployeeCommand = new TabRelayCommand(async () => await ExecuteAddEmployeeAsync());
            AddEquipmentCommand = new TabRelayCommand(async () => await ExecuteAddEquipmentAsync());
            SendToRepairCommand = new TabRelayCommand(async () => await ExecuteSendToRepairAsync());
            ReturnFromRepairCommand = new TabRelayCommand(async () => await ExecuteReturnFromRepairAsync());

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
                    await LoadEquipmentTypesAsync();
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


        private async Task LoadEquipmentTypesAsync()
        {
            try
            {
                EquipmentTypes = await _context.EquipmentTypes.OrderBy(t => t.Description).ToListAsync();
            }
            catch (Exception ex)
            {
                AddEquipmentStatusMessage = "Не удалось загрузить типы техники: " + ex.Message;
            }
        }

        private async Task RefreshAssignmentFormDataAsync()
        {
            try
            {
                var allEquipment = await _equipmentService.GetAllAsync();
                var allEmployees = await _employeeService.GetAllAsync();
                var allAssignments = await _assignmentService.GetAllAsync();

                EquipmentItems = allEquipment;
                EmployeeItems = allEmployees;

                AvailableEquipmentItems = allEquipment.Where(e => e.Status == "InStock").ToList();

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

                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                AssignmentStatusMessage = "Ошибка: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

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

        private async Task ExecuteAddEmployeeAsync()
        {
            AddEmployeeStatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewEmployeeFirstName) || string.IsNullOrWhiteSpace(NewEmployeeLastName))
            {
                AddEmployeeStatusMessage = "Укажите хотя бы имя и фамилию.";
                return;
            }

            try
            {
                var dto = new EmployeeDto
                {
                    FirstName = NewEmployeeFirstName,
                    LastName = NewEmployeeLastName,
                    Department = NewEmployeeDepartment,
                    Position = NewEmployeePosition,
                    Email = NewEmployeeEmail,
                    Phone = NewEmployeePhone,
                    HireDate = DateTime.Now,
                    IsActive = true,
                    UserId = Guid.Empty
                };

                await _employeeService.CreateAsync(dto);

                AddEmployeeStatusMessage = $"Сотрудник {dto.LastName} {dto.FirstName} добавлен.";

                NewEmployeeFirstName = string.Empty;
                NewEmployeeLastName = string.Empty;
                NewEmployeeDepartment = string.Empty;
                NewEmployeePosition = string.Empty;
                NewEmployeeEmail = string.Empty;
                NewEmployeePhone = string.Empty;

                await RefreshAssignmentFormDataAsync();
            }
            catch (Exception ex)
            {
                var detail = ex.InnerException?.Message ?? ex.Message;
                AddEmployeeStatusMessage = "Ошибка: " + detail;
            }
        }

  
        private async Task ExecuteAddEquipmentAsync()
        {
            AddEquipmentStatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewEquipmentName))
            {
                AddEquipmentStatusMessage = "Укажите название техники.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewEquipmentSerialNumber))
            {
                AddEquipmentStatusMessage = "Укажите серийный номер (должен быть уникальным).";
                return;
            }

            if (SelectedEquipmentType == null)
            {
                AddEquipmentStatusMessage = "Выберите тип техники.";
                return;
            }

            decimal.TryParse(NewEquipmentPrice, out var price);

            try
            {
                var dto = new EquipmentDto
                {
                    Name = NewEquipmentName,
                    SerialNumber = NewEquipmentSerialNumber,
                    InventoryNumber = string.IsNullOrWhiteSpace(NewEquipmentInventoryNumber) ? null : NewEquipmentInventoryNumber,
                    Manufacturer = NewEquipmentManufacturer,
                    Model = NewEquipmentModel,
                    Location = NewEquipmentLocation,
                    Price = price,
                    PurchaseDate = DateTime.Now,
                    EquipmentTypeId = SelectedEquipmentType.Id,
                    SupplierId = Guid.Empty 
                };

                await _equipmentService.CreateAsync(dto);

                AddEquipmentStatusMessage = $"Техника \"{dto.Name}\" добавлена на склад.";

                NewEquipmentName = string.Empty;
                NewEquipmentSerialNumber = string.Empty;
                NewEquipmentInventoryNumber = string.Empty;
                NewEquipmentManufacturer = string.Empty;
                NewEquipmentModel = string.Empty;
                NewEquipmentLocation = string.Empty;
                NewEquipmentPrice = string.Empty;
                SelectedEquipmentType = null;

     
                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                var detail = ex.InnerException?.Message ?? ex.Message;
                AddEquipmentStatusMessage = "Ошибка: " + detail;
            }
        }

       
        private async Task RefreshReportsDataAsync()
        {
            try
            {
                var allEquipment = await _equipmentService.GetAllAsync();
                var allEmployees = await _employeeService.GetAllAsync();

                var equipmentById = allEquipment.ToDictionary(e => e.Id);
                var employeesById = allEmployees.ToDictionary(e => e.Id);

                var histories = await _context.AssignmentHistories
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                HistoryItems = histories.Select(h => new HistoryDisplayItem
                {
                    EquipmentName = equipmentById.TryGetValue(h.EquipmentId, out var eq) ? eq.Name : "Неизвестная техника",
                    EmployeeName = (h.EmployeeId.HasValue && employeesById.TryGetValue(h.EmployeeId.Value, out var emp))
                        ? $"{emp.LastName} {emp.FirstName}"
                        : string.Empty,
                    ActionType = h.ActionType,
                    ActionDate = h.ActionDate,
                    PerformedBy = h.PerformedBy,
                    Details = h.Details
                }).ToList();

                RepairableEquipmentItems = allEquipment.Where(e => e.Status == "InStock" || e.Status == "Issued").ToList();
                UnderRepairEquipmentItems = allEquipment.Where(e => e.Status == "UnderRepair").ToList();
            }
            catch (Exception ex)
            {
                RepairStatusMessage = "Не удалось загрузить отчёты: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }


        private async Task ExecuteSendToRepairAsync()
        {
            RepairStatusMessage = string.Empty;

            if (SelectedEquipmentForRepair == null)
            {
                RepairStatusMessage = "Выберите технику для отправки в ремонт.";
                return;
            }

            try
            {
                var dto = SelectedEquipmentForRepair;
                dto.Status = "UnderRepair";
                await _equipmentService.UpdateAsync(dto);

                _context.AssignmentHistories.Add(new курсовая2511.Models.AssignmentHistory
                {
                    Id = Guid.NewGuid(),
                    EquipmentId = dto.Id,
                    EmployeeId = null,
                    ActionType = "Отправлено в ремонт",
                    ActionDate = DateTime.Now,
                    PerformedBy = Username,
                    Details = $"Техника \"{dto.Name}\" отправлена в ремонт."
                });
                await _context.SaveChangesAsync();

                RepairStatusMessage = $"Техника \"{dto.Name}\" отправлена в ремонт.";
                SelectedEquipmentForRepair = null;

                await RefreshReportsDataAsync();
                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                RepairStatusMessage = "Ошибка: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        private async Task ExecuteReturnFromRepairAsync()
        {
            RepairStatusMessage = string.Empty;

            if (SelectedEquipmentForRepairReturn == null)
            {
                RepairStatusMessage = "Выберите технику, возвращаемую из ремонта.";
                return;
            }

            try
            {
                var dto = SelectedEquipmentForRepairReturn;
                dto.Status = "InStock";
                await _equipmentService.UpdateAsync(dto);

                _context.AssignmentHistories.Add(new курсовая2511.Models.AssignmentHistory
                {
                    Id = Guid.NewGuid(),
                    EquipmentId = dto.Id,
                    EmployeeId = null,
                    ActionType = "Возвращено из ремонта",
                    ActionDate = DateTime.Now,
                    PerformedBy = Username,
                    Details = $"Техника \"{dto.Name}\" отремонтирована и возвращена на склад."
                });
                await _context.SaveChangesAsync();

                RepairStatusMessage = $"Техника \"{dto.Name}\" возвращена на склад.";
                SelectedEquipmentForRepairReturn = null;

                await RefreshReportsDataAsync();
                await RefreshAssignmentFormDataAsync();
                await LoadDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                RepairStatusMessage = "Ошибка: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
