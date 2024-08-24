using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Memo.Model;
using Memo.Commands;
using Microsoft.Win32;
using System.Windows;

namespace Memo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string SaveFilePath = "notes.txt";
        private Note? _selectedNote;

        public ObservableCollection<Note> Notes { get; set; }
        public Note? SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNoteCommand { get; private set; }
        public ICommand DeleteNoteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }

        public MainViewModel()
        {
            Notes = new ObservableCollection<Note>();
            AddNoteCommand = new RelayCommand(o => AddNote());
            DeleteNoteCommand = new RelayCommand(o => DeleteNote(), o => SelectedNote != null);
            SaveCommand = new RelayCommand(o => SaveNotes());
            LoadCommand = new RelayCommand(o => LoadNotes());
        }

        public void AddNote()
        {
            Notes.Add(new Note { Title = "New Note", Content = "Write your content here..." });
        }

        public void DeleteNote()
        {
            if (SelectedNote != null)
            {
                Notes.Remove(SelectedNote);
            }
        }

        public void SaveNotes()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.Title = "Save Notes";


            if (saveFileDialog.ShowDialog() == true)
            {

                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var note in Notes)
                        {
                            writer.WriteLine(note.Title);
                            writer.WriteLine(note.Content);
                            writer.WriteLine("---"); // 메모 구분자
                        }

                        foreach (var note in Notes)
                        {
                            note.FileName = Path.GetFileName(saveFileDialog.FileName);
                        }

                        MessageBox.Show("파일 저장이 완료되었습니다", "저장완료!", MessageBoxButton.OK, MessageBoxImage.Information);
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("파일 저장 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void LoadNotes()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            openFileDialog.Title = "Load Notes";
            if (openFileDialog.ShowDialog() == true)
            {
                Notes.Clear();
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string? title = reader.ReadLine();
                        string? content = reader.ReadLine();
                        reader.ReadLine(); // 구분자 건너뜀

                        Notes.Add(new Note
                        {
                            Title = title,
                            Content = content,
                            FileName = Path.GetFileName(openFileDialog.FileName) // 파일 제목을 저장
                        });
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
