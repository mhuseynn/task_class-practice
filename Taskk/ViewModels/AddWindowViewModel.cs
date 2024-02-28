using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Taskk.Models;
using TaskManagerApp.Commands;
using TaskManagerApp.Services;

namespace Taskk.ViewModels
{
    public class AddWindowViewModel : NotificationService
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

        public AddWindowViewModel()
        {
            Comments = new ObservableCollection<Comment>();
            Comment = new Comment();
            savebtn = new RelayCommand(savec!);
        }

        public void savec(object pa)
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

            Comments.Add(Comment!);

            var data = JsonConvert.SerializeObject(Comments);
            File.WriteAllText(@"../../../StaticFiles/comments.json", data);

            Window? window = pa as Window;

            window!.Close();

        }
    }
}
