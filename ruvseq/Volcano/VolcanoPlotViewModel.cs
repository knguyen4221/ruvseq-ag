﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;

namespace ruvseq.Volcano
{
    public class VolcanoPlotViewModel : ModeViewModel
    {
        private string _inputFile, _result;
        private bool _displayNames;
        private VolcanoPlotModel vpModel;

        private ICommand _openInput;
        private ICommand _run_r;
        public VolcanoPlotViewModel()
        {
            Name = "Volcano Plots";
            IsSelected = false;
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
            vpModel = new VolcanoPlotModel(InputFile, OutputDirectory, DisplayNames);
            Result = vpModel.make_plot();
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
