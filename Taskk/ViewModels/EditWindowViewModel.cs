using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System;
using Taskk.Models;
using TaskManagerApp.Commands;
using TaskManagerApp.Services;
using System.IO;

namespace Taskk.ViewModels;

public class EditWindowViewModel : NotificationService
{
    public ICommand savebtn { get; set; }


    private Comment comment;

    public Comment Comment
    {
        get { return comment; }
        set { comment = value; OnPropertyChanged(); }
    }

    private ObservableCollection<Comment> comments;

    public ObservableCollection<Comment> Comments
    {
        get { return comments; }
        set { comments = value; OnPropertyChanged(); }
    }

    public EditWindowViewModel(Comment comment)
    {
        Comments = new ObservableCollection<Comment>();
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
        Comment = comment;

        savebtn = new RelayCommand(savec!);

    }

    public void savec(object pa)
    {
        foreach (var item in Comments)
        {
            if (item.id == Comment.id)
            {
                item.name = Comment.name;
                item.email = Comment.email;
                item.body = comment.body;
            }
        }

        var data = JsonConvert.SerializeObject(Comments);
        File.WriteAllText(@"../../../StaticFiles/comments.json", data);

        Window? window = pa as Window;

        window!.Close();

    }




}
