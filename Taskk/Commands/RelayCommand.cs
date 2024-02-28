using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskManagerApp.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }

    private readonly Predicate<object?>? _CanExecute;

    private readonly Action<object?>? _Execute;
    
    private readonly Func<object?>? _Execute2;



    public RelayCommand(Action<object?>? execute, Predicate<object?>? canExecute = null)
    {
        _CanExecute = canExecute;
        _Execute = execute;
    }

    public RelayCommand(Func<Task> ex)
    {
        _Execute2 = ex;
    }

    public bool CanExecute(object? parameter)
    {
        if (_CanExecute == null) return true;
        return _CanExecute!.Invoke(parameter);
    }

    public void Execute(object? parameter)
    {
        _Execute?.Invoke(parameter);
    }

    public void Execute2(object? parameter)
    {
        _Execute2?.DynamicInvoke(parameter);
    }
}
