using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskManagerApp.Services;

public  abstract class NotificationService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string property_name = null) 
        => PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(property_name));
}
