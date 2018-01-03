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

        private GalaSoft.MvvmLight.ViewModelBase _currentPageViewModel;
        private List<GalaSoft.MvvmLight.ViewModelBase> _pageViewModels;

        public ApplicationViewModel()
        {
            PageViewModels.Add(new RUVSeqViewModel());

            CurrentPageViewModel = PageViewModels[0];
        }

        public GalaSoft.MvvmLight.ViewModelBase CurrentPageViewModel
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

        public List<GalaSoft.MvvmLight.ViewModelBase> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<GalaSoft.MvvmLight.ViewModelBase>();
                return _pageViewModels;
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if(_changePageCommand == null)
                {
                    _changePageCommand = new GalaSoft.MvvmLight.Command.RelayCommand<GalaSoft.MvvmLight.ViewModelBase>((p) => ChangeViewModel(p));
                }
                return _changePageCommand;
            }
        }

        private void ChangeViewModel(GalaSoft.MvvmLight.ViewModelBase viewModel)
        {
            _currentPageViewModel = viewModel;
        }
    }

}
