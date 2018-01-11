using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ruvseq
{
    public class ModeViewModel : ViewModelBase
    {
        private string _name;
        private bool _isSelected;
        private string _outputDir = Directory.GetCurrentDirectory();
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
    }
}
