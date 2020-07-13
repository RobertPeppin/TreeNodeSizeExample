using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace TreeNodeSizeExample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        Random RNG = new Random(DateTime.Now.Millisecond);
        const int PROB_CHILD_NODES = 30;
        const int MAX_DEPTH = 4;
        ObservableCollection<ITreeItem> items = new ObservableCollection<ITreeItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ITreeItem> Groups { get => items; set => SetProperty(ref items, value); }
        public ICommand BuildFirstSet { get; private set; }
        public ICommand BuildSecondSet { get; private set; }
        public ICommand BuildThirdSet { get; private set; }
        public ICommand BuildFourthSet { get; private set; }

        public MainPageViewModel()
        {
            BuildFirstSet = new Command(BuildIntialData);
            BuildSecondSet = new Command(UpdateExistingEntries);
            BuildThirdSet = new Command(UpdateToMakeContentSmaller);
            BuildFourthSet = new Command(AddSomeExtraItems);
        }

        public void SetProperty<T>(ref T item, T newValue, [CallerMemberName] string caller = "")
        {
            item = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        private void BuildIntialData()
        {
            var groups = new ObservableCollection<ITreeItem>();

            int numberOfTopLevels = RNG.Next(1, 7);

            for (int index = 0; index < numberOfTopLevels; index++)
            {
                TreeGroup group = new TreeGroup()
                {
                    Id = Guid.NewGuid(),
                    Header = $"Top Group {index + 1}"
                };

                BuildEntryChildren(group, 1);
                groups.Add(group);
            }

            Groups = groups;
        }

        private void BuildEntryChildren(ITreeItem parent, int depth)
        {
            int numberOfChildren = RNG.Next(1, 7);

            try
            {

                for (int index = 0; index < numberOfChildren; index++)
                {
                    int childrenChance = RNG.Next(0, 100);

                    if (childrenChance < PROB_CHILD_NODES)
                    {
                        var group = new TreeGroup()
                        {
                            Id = Guid.NewGuid(),
                            Header = $"L{depth} Child Group {index + 1}"
                        };
                        if (depth < MAX_DEPTH)
                        {
                            BuildEntryChildren(group, depth + 1);
                        }
                        parent.Items.Add(group);

                    }
                    else
                    {
                        var item = new TreeItem()
                        {
                            Id = Guid.NewGuid(),
                            Header = $"L{depth} Child Item {index + 1}"
                        };

                        parent.Items.Add(item);
                    }
                }
            }
            catch { }
        }

        private void UpdateExistingEntries()
        {
            // make the text entries much bigger 
            foreach (var item in Groups)
            {
                MakeChildrenBigger(item);
            }
            Groups = Groups;
        }

        private void MakeChildrenBigger(ITreeItem item)
        {
            if (RNG.Next(0, 100) > 60)
            {
                item.Header += " - This will make the node much bigger than it used to be";
            }

            foreach (var child in item.Items)
            {
                MakeChildrenBigger(child);
            }
        }


        private void UpdateToMakeContentSmaller()
        {
            // make the text entries much bigger 
            foreach (var item in Groups)
            {
                MakeChildrenSmaller(item);
            }
            Groups = Groups;
        }

        private void MakeChildrenSmaller(ITreeItem item)
        {
            item.Header = item.Header.Split("-".ToCharArray())[0].Trim();

            foreach (var child in item.Items)
            {
                MakeChildrenSmaller(child);
            }
        }

        private void AddSomeExtraItems()
        {
            foreach (var item in Groups)
            {
                AddChildren(item);
            }

            // add an extra group

            Groups = Groups;
        }

        private void AddChildren(ITreeItem item)
        {
            if (RNG.Next(0, 100) > 30)
            {
                item.Items.Add(new TreeItem()
                {
                    Id = Guid.NewGuid(),
                    Header = "New Item"
                });
            }

            foreach (var child in item.Items)
            {
                if (child is TreeGroup)
                {
                    AddChildren(child);
                }
            }
        }
    }

    public interface ITreeItem
    {
        Guid Id { get; set; }
        string Header { get; set; }
        ObservableCollection<ITreeItem> Items { get; set; }
    }

    public class TreeGroup : ITreeItem, INotifyPropertyChanged
    {
        string header;

        public Guid Id { get; set; }
        public ObservableCollection<ITreeItem> Items { get; set; } = new ObservableCollection<ITreeItem>();
        public string Header
        {
            get => header;
            set
            {
                header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Header"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TreeItem : ITreeItem, INotifyPropertyChanged
    {
        string header;
        bool isChanged;
        bool isReadAndSign;
        public Guid Id { get; set; }
        public ObservableCollection<ITreeItem> Items { get; set; } = new ObservableCollection<ITreeItem>();
        public string Header
        {
            get => header;
            set
            {
                header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Header"));
            }
        }

        public bool IsChanged
        {
            get => isChanged;
            set
            {
                isChanged = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChanged"));
            }
        }

        public bool IsReadAndSign
        {
            get => isReadAndSign;
            set
            {
                isReadAndSign = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsReadAndSign"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
