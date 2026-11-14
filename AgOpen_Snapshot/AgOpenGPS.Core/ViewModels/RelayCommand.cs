using System;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        readonly Func<bool> _canExecute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
#if !NETFRAMEWORK
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
#endif
        }

#if NETFRAMEWORK
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
#else
        public event EventHandler? CanExecuteChanged;
#endif
    }

    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;


        public RelayCommand(Action<T> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

#if NETFRAMEWORK
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
#else
        public event EventHandler? CanExecuteChanged;
#endif

        public void Execute(object parameter)
        {
            _execute((T)parameter);
#if !NETFRAMEWORK
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
#endif
        }
    }
}
