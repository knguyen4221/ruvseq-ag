using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ruvseq.DESeq2
{
    public class DESeq2ViewModel : ModeViewModel
    {

        private string _colDataInput;
        private string _countsInput;
        private string _outputPrefix = "Output Prefix";
        private string _result = "";

        private ICommand _specifyOutputCommand;
        private ICommand _openColData;
        private ICommand _openCountsData;
        private ICommand _run_deseq;

        public DESeq2ViewModel()
        {
            Name = "DESeq2";
            IsSelected = false;
        }

        public string ColumnDataFile
        {
            get { return _colDataInput; }
            set
            {
                if(value != _colDataInput)
                {
                    _colDataInput = value;
                    RaisePropertyChanged("ColumnDataFile");
                }
            }
        }

        public string CountsDataFile
        {
            get { return _countsInput; }
            set
            {
                if(value != _countsInput)
                {
                    _countsInput = value;
                    RaisePropertyChanged("CountsDataFile");
                }
            }
        }

        public string OutputPrefix
        {
            get { return _outputPrefix; }
            set
            {
                if(value != _outputPrefix)
                {
                    _outputPrefix = value;
                    RaisePropertyChanged("OutputPrefix");
                }
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    RaisePropertyChanged("Result");
                }
            }
        }

        public ICommand OpenColData
        {
            get
            {
                if(_openColData == null)
                {
                    _openColData = new RelayCommand(openColumnData);
                }
                return _openColData;
            }
        }

        public ICommand OpenCountsData
        {
            get
            {
                if(_openCountsData == null)
                {
                    _openCountsData = new RelayCommand(openCountsData);
                }
                return _openCountsData;
            }
        }

        public ICommand SpecifyOutput
        {
            get
            {
                if (this._specifyOutputCommand == null)
                {
                    this._specifyOutputCommand = new RelayCommand(specifyOutput);
                }
                return this._specifyOutputCommand;
            }
        }

        public ICommand RunDESeq
        {
            get
            {
                if(this._run_deseq == null)
                {
                    this._run_deseq = new RelayCommand(run_deseq);
                }
                return this._run_deseq;
            }
        }

        private void openCountsData()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.RestoreDirectory = true;
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CountsDataFile = ofd.FileName;
            }
        }

        private void openColumnData()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.RestoreDirectory = true;
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                ColumnDataFile = ofd.FileName;
            }
        }

        private void specifyOutput()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                OutputDirectory = fbd.SelectedPath;
            }
        }

        private void run_deseq()
        {
            Result = "Loading";
            DESeq2 deseq = new DESeq2(ColumnDataFile, CountsDataFile, OutputPrefix, OutputDirectory);
            Result = deseq.run_deseq2();
        }
    }
}
