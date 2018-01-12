using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft;

namespace ruvseq
{
    public class ApplicationViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private ICommand _changePageCommand;

        private ModeViewModel _currentPageViewModel;
        private List<ModeViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            PageViewModels.Add(new RUVSeqViewModel());
            PageViewModels.Add(new DESeq2.DESeq2ViewModel());
            PageViewModels.Add(new Volcano.VolcanoPlotViewModel());

            CurrentPageViewModel = PageViewModels[0];
        }

        public ModeViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if(_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    this.RaisePropertyChanged("CurrentPageViewModel");
                }
            }
        }

        public List<ModeViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<ModeViewModel>();
                return _pageViewModels;
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if(_changePageCommand == null)
                {
                    _changePageCommand = new GalaSoft.MvvmLight.Command.RelayCommand<ModeViewModel>((p) => ChangeViewModel(p));
                }
                return _changePageCommand;
            }
        }

        private void ChangeViewModel(ModeViewModel viewModel)
        {
            viewModel.OutputDirectory = CurrentPageViewModel.OutputDirectory;
            CurrentPageViewModel = viewModel;
            foreach(var vm in PageViewModels)
            {
                if(vm != viewModel)
                {
                    vm.IsSelected = false;
                }
            }
        }
    }

}
