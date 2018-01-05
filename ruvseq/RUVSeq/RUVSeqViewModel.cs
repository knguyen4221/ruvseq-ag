using RProcesses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

namespace ruvseq
{
    public class RUVSeqViewModel : ModeViewModel
    {
        private RUVSeqModel _ruvseq_model;
        private ObservableCollection<string> vm_g1, vm_g2;
        private string vm_outputPrefix, vm_inputFile, vm_result;
        private string _selectedItem1, _selectedItem2;

        private ICommand _runRUVSeqCommand;
        private ICommand _g1doubleClickCommand;
        private ICommand _g2doubleClickCommand;
        private ICommand _openFileCommand { get; set; }
        

        public RUVSeqModel ruvseq_model
        {
            get { return _ruvseq_model; }
            set
            {
                if (value != this._ruvseq_model)
                {
                    this._ruvseq_model = value;
                    this.RaisePropertyChanged("ruvseq_model");
                }
            }
        }

        public ObservableCollection<string> g1
        {
            get { return vm_g1; }
            set
            {
                if (value != this.vm_g1)
                {
                    this.vm_g1 = value;
                    this.RaisePropertyChanged("g1");
                }
            }
        }

        public ObservableCollection<string> g2
        {
            get { return vm_g2; }
            set
            {
                if (value != this.vm_g2)
                {
                    this.vm_g2 = value;
                    this.RaisePropertyChanged("g2");
                }
            }
        }

        public string Group1SelectedItem
        {
            get{ return _selectedItem1; }
            set
            {
                if(value != _selectedItem1)
                {
                    _selectedItem1 = value;
                    RaisePropertyChanged("Group1SelectedItem");
                }
            }
        }

        public string Group2SelectedItem
        {
            get { return _selectedItem2; }
            set
            {
                if(value != _selectedItem2)
                {
                    _selectedItem2 = value;
                    RaisePropertyChanged("Group2SelectedItem");
                }
            }
        }
        public string outputPrefix
        {
            get { return this.vm_outputPrefix; }
            set
            {
                if (value != this.vm_outputPrefix)
                {
                    this.vm_outputPrefix = value;
                    this.RaisePropertyChanged("outputPrefix");
                }
            }
        }

        public string inputFile
        {
            get { return this.vm_inputFile; }
            set
            {
                if (value != this.vm_inputFile)
                {
                    this.vm_inputFile = value;
                    this.RaisePropertyChanged("inputFile");
                }
            }
        }

        public string Result
        {
            get { return this.vm_result; }
            set
            {
                if (value != this.vm_result)
                {
                    this.vm_result = value;
                    RaisePropertyChanged("Result");
                }
            }
        }

        public ICommand OpenMatrixCommand
        {
            get
            {
                if(this._openFileCommand == null)
                {
                    this._openFileCommand = new RelayCommand(Open_Executed);
                }
                return this._openFileCommand;
            }
        }

        public ICommand RunRUVSeqCommand
        {
            get
            {
                if(this._runRUVSeqCommand == null)
                {
                    this._runRUVSeqCommand = new RelayCommand(Start_Ruvseq);
                }
                return this._runRUVSeqCommand;
            }
        }

        //TODO: RUVSeq Double Click
        public ICommand Group1DoubleClickCommand
        {
            get
            {
                if (_g1doubleClickCommand == null)
                {
                    _g1doubleClickCommand = new RelayCommand(changeGroup1to2);
                }
                return _g1doubleClickCommand;
            }
        }

        public ICommand Group2DoubleClickCommand
        {
            get
            {
                if (_g2doubleClickCommand == null)
                    _g2doubleClickCommand = new RelayCommand(changeGroup2to1);
                return _g2doubleClickCommand;
            }
        }

        private void changeGroup1to2()
        {
            g2.Add(Group1SelectedItem);
            g1.Remove(Group1SelectedItem);
        }

        private void changeGroup2to1()
        {
            g1.Add(Group2SelectedItem);
            g2.Remove(Group2SelectedItem);
        }

        private void Start_Ruvseq()
        {
            Result = "Loading";
            string ret = ruvseq_run(inputFile, g1.ToList(), g2.ToList(), outputPrefix, Directory.GetCurrentDirectory());
            Result = ret;
        }

        private void Open_Executed()
        {
            OpenFileDialog ofd = new OpenFileDialog();


            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vm_inputFile = ofd.FileName;
                string[] cell_types = ReadCSV(vm_inputFile);
                if (vm_g1.Count != 0)
                    vm_g1.Clear();
                if (vm_g2.Count != 0)
                    vm_g2.Clear();
                for (int i = 1; i < cell_types.Length; ++i)
                {
                    g1.Add(cell_types[i]);
                }
            }
        }

        private string[] ReadCSV(string fileName)
        {
            IEnumerable<string> result = File.ReadLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
            string toSplit = result.First();
            return toSplit.Split(',');
        }

        private string ruvseq_run(string fullPath, List<string> g1, List<string> g2, string outFile, string currentDirectory)
        {
            RUVseq process = new RUVseq(fullPath, outFile, currentDirectory + "\\ruvseq_output");
            return process.run_ruvseq(g1, g2);
        }

        public RUVSeqViewModel()
        {
            vm_g1 = new ObservableCollection<string>();
            vm_g2 = new ObservableCollection<string>();
            outputPrefix = "Output Prefix";
            Name = "RUVSeq";
            IsSelected = true;
        }
    }
}

