using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;

namespace ruvseq.Volcano
{
    public class VolcanoPlotViewModel : ModeViewModel
    {
        private string _inputFile, _result;
        private string _outputPath = "";
        private string _outputPrefix = "prefix";
        private bool _displayNames;
        private VolcanoPlotModel vpModel;

        private ICommand _openInput;
        private ICommand _run_r;
        public VolcanoPlotViewModel()
        {
            Name = "Volcano Plots";
            IsSelected = false;
            _displayNames = false;
        }

        public string OutputPlot
        {
            get { return _outputPath; }
            set
            {
                if(value != _outputPath)
                {
                    _outputPath = value;
                    RaisePropertyChanged("OutputPlot");
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
        public string InputFile
        {
            get { return _inputFile; }
            set
            {
                if(value != _inputFile)
                {
                    _inputFile = value;
                    RaisePropertyChanged("InputFile");
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

        public bool DisplayNames
        {
            get { return _displayNames; }
            set
            {
                if(value != _displayNames)
                {
                    _displayNames = value;
                    RaisePropertyChanged("DisplayNames");
                }
            }
        }


        public ICommand OpenInputFile
        {
            get
            {
                if (_openInput == null)
                    _openInput = new RelayCommand(openInputFile);
                return _openInput;
            }
        }

        public ICommand MakePlots
        {
            get
            {
                if (_run_r == null)
                    _run_r = new RelayCommand(run_r);
                return _run_r;
            }
        }

        private void run_r()
        {
            Result = "Loading";
            vpModel = new VolcanoPlotModel(InputFile, OutputDirectory, DisplayNames, OutputPrefix);
            Result = vpModel.make_plot();
            OutputPlot = string.Format("{0}\\{1}_volcanoplot.png", OutputDirectory, OutputPrefix);
        }

        private void openInputFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != null)
            {
                InputFile = ofd.FileName;
            }
        }
    }
}
