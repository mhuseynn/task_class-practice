using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Taskk.Models;
using Taskk.ViewModels;
using Taskk.Views.Windows;
using TaskManagerApp.Commands;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

public class MainWindowViewModel : NotificationService
{

    private ObservableCollection<Comment> comments;

    public ObservableCollection<Comment> Comments
    {
        get { return comments; }
        set { comments = value; OnPropertyChanged(); }
    }

    public ICommand addbtn { get; set; }
    public ICommand okbtn { get; set; }
    public ICommand removebtn { get; set; }
    public ICommand editbtn { get; set; }
    public ICommand refbtn { get; set; }


    private string search;

    public string Search
    {
        get { return search; }
        set { search = value; OnPropertyChanged(); }
    }


    public MainWindowViewModel()
    {
        Comments = new ObservableCollection<Comment>();
        Thread thread = new Thread(() =>
        {
            try
            {
                string filePath = "../../../StaticFiles/comments.json";

                if (File.Exists(filePath))
                {
                    string jsonText = File.ReadAllText(filePath);

                    Comments = JsonConvert.DeserializeObject<ObservableCollection<Comment>>(jsonText)!;
                }
                else
                {
                    MessageBox.Show("The file does not exist: " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        });
        thread.IsBackground = true;
        thread.Start();

        addbtn = new RelayCommand(addwindow!);
        okbtn = new RelayCommand(p => Task.Factory.StartNew(show));
        removebtn = new RelayCommand(remove_from!);
        editbtn = new RelayCommand(editwindow!);
        refbtn = new RelayCommand(refresh!);

    }
    public void refresh(object pa)
    {
        Thread thread = new Thread(() =>
        {
            try
            {
                string filePath = "../../../StaticFiles/comments.json";

                if (File.Exists(filePath))
                {
                    string jsonText = File.ReadAllText(filePath);

                    Comments = JsonConvert.DeserializeObject<ObservableCollection<Comment>>(jsonText)!;                 
                }
                else
                {
                    Console.WriteLine("The file does not exist: " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        });
        thread.IsBackground = true;
        thread.Start();
    }

    public void editwindow(object pa)
    {
        ListView? listView = pa as ListView;

        Comment? comment = listView!.SelectedItem as Comment;
        Window window = new EditWindowView();
        window.DataContext = new EditWindowViewModel(comment!);
        window.ShowDialog();
    }


    public void remove_from(object pa)
    {
        ListView? listView = pa as ListView;

        Comment? comment = listView!.SelectedItem as Comment;

        Comments.Remove(comment!);

        var data = JsonConvert.SerializeObject(Comments);
        File.WriteAllText(@"../../../StaticFiles/comments.json", data);


    }
    public void addwindow(object pa)
    {
        Window window = new AddWindowView();
        window.ShowDialog();

    }
    public async Task<string> GetAllDataFromJsonApi()
    {
        HttpClient client = new HttpClient();


        var data = await client.GetStringAsync(Search);

        return data;
    }

    public async Task show()
    {
        var Comments1 = await GetAllDataFromJsonApi();

        var data = Comments1.ToList();

        Comments = JsonConvert.DeserializeObject<ObservableCollection<Comment>>(Comments1)!;

        File.WriteAllText(@"../../../StaticFiles/comments.json", Comments1);
    }


}
