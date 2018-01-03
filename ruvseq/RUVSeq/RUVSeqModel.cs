using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft;
using RProcesses;

namespace ruvseq
{
    public class RUVSeqModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<string> g1, g2;
        private string inputFilePath, outputFilePrefix, currentDirectory;

        public List<string> firstGroup
        {
            get { return g1; }
            set
            {
                if (value != g1)
                {
                    g1 = value;
                    this.RaisePropertyChanged("firstGroup");
                }
            }
        }

        public List<string> secondGroup
        {
            get { return g2; }
            set
            {
                if (value != g2)
                {
                    g2 = value;
                    this.RaisePropertyChanged("secondGroup");
                }
            }
        }

        public string inputFile
        {
            get { return this.inputFilePath; }
            set
            {
                if (value != this.inputFilePath)
                {
                    this.inputFilePath = value;
                    this.RaisePropertyChanged("inputFile");
                }
            }
        }

        public string outputPrefix
        {
            get { return this.outputFilePrefix; }
            set
            {
                if (value != this.outputFilePrefix)
                {
                    this.outputFilePrefix = value;
                    this.RaisePropertyChanged("outputPrefix");
                }
            }
        }

        public string currDirectory
        {
            get { return this.currentDirectory; }
            set
            {
                if (value != this.currentDirectory)
                {
                    this.currentDirectory = value;
                    this.RaisePropertyChanged("currDirectory");
                }
            }
        }
    }
}
