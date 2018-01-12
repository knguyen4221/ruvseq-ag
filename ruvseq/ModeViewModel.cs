using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ruvseq
{
    public class ModeViewModel : ViewModelBase
    {
        private string _name;
        private bool _isSelected;
        private string _outputDir = Directory.GetCurrentDirectory();
        private ICommand _specifyOutputCommand;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }
        public string OutputDirectory
        {
            get { return this._outputDir; }
            set
            {
                if (value != this._outputDir)
                {
                    this._outputDir = value;
                    RaisePropertyChanged("OutputDirectory");
                }
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

        private void specifyOutput()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                OutputDirectory = fbd.SelectedPath;
            }
        }

    }
}
