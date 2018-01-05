using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ruvseq.DESeq2
{
    public class DESeq2ViewModel : ModeViewModel
    {
        public DESeq2ViewModel()
        {
            Name = "DESeq2";
            IsSelected = false;
        }
    }
}
