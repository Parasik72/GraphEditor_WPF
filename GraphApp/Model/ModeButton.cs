using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphApp.Model {
    [Serializable]
    class ModeButton : INotifyPropertyChanged {
        private string toolTip;
        private string imgSource;
        public string ToolTip {
            get => toolTip;
            set {
                toolTip = value;
                OnPropertyChanged();
            }
        }
        public string ImgSource {
            get => imgSource;
            set {
                imgSource = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChangeModeListItem { get; private set; }
        public ICommand ChangeMode { get; private set; }
        public ICommand[] Funcs { get;  set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public static ModeButton[] getDefaultModes(params Action<object>[] funcArr) {
            return new ModeButton[] {
                new ModeButton{ToolTip="Node add", ImgSource="/nodeAdd.png", ChangeMode = new DelegateCommand(funcArr[0]), ChangeModeListItem = new DelegateCommand(funcArr[10]), Funcs = new ICommand[] { new DelegateCommand(funcArr[1]), null, new DelegateCommand(funcArr[4]), null, null } },
                new ModeButton{ToolTip="Node remove", ImgSource="/nodeRemove.png", ChangeMode = new DelegateCommand(funcArr[0]), ChangeModeListItem = new DelegateCommand(funcArr[10]), Funcs = new ICommand[] { null, new DelegateCommand(funcArr[2]), new DelegateCommand(funcArr[4]), null, null }},
                new ModeButton{ToolTip="Edge add", ImgSource="/edgeAdd.png", ChangeMode = new DelegateCommand(funcArr[0]), ChangeModeListItem = new DelegateCommand(funcArr[10]), Funcs = new ICommand[] {null, new DelegateCommand(funcArr[7]), new DelegateCommand(funcArr[4]), null, null }},
                new ModeButton{ToolTip="Edge remove", ImgSource="/edgeRemove.png", ChangeMode = new DelegateCommand(funcArr[0]), ChangeModeListItem = new DelegateCommand(funcArr[10]), Funcs = new ICommand[] {null, null, new DelegateCommand(funcArr[4]), new DelegateCommand(funcArr[8]), null }},
                new ModeButton{ToolTip="Move/Select", ImgSource="/move.png", ChangeMode = new DelegateCommand(funcArr[0]), ChangeModeListItem = new DelegateCommand(funcArr[10]), Funcs = new ICommand[] { new DelegateCommand(funcArr[6]), new DelegateCommand(funcArr[5]), new DelegateCommand(funcArr[3]), new DelegateCommand(funcArr[9]), new DelegateCommand(funcArr[11])}}
            };
        }
    }
}
