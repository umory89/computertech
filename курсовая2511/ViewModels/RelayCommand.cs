
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace курсовая2511
{
    public class TabRelayCommand : ICommand
    {
        private readonly Action? _execute;
        private readonly Func<Task>? _executeAsync;

        public TabRelayCommand(Action execute) => _execute = execute;
        public TabRelayCommand(Func<Task> executeAsync) => _executeAsync = executeAsync;

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            if (_executeAsync != null)
                await _executeAsync();
            else
                _execute?.Invoke();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
