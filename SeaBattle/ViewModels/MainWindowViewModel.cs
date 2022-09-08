using System.Threading.Tasks;
using SeaBattle.Domain.Models;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SeaBattle.Domain.Exceptions;

namespace SeaBattle.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private bool _isMapGenerationError;
        private bool _isBlockedInput;

        public MainWindowViewModel(PlayerViewModel humanPlayerViewModel, PlayerViewModel computerPlayerViewModel)
        {
            HumanPlayerViewModel = humanPlayerViewModel;
            ComputerPlayerViewModel = computerPlayerViewModel;
        }

        public PlayerViewModel HumanPlayerViewModel { get; }

        public PlayerViewModel ComputerPlayerViewModel { get; }

        public bool IsMapGenerationError
        {
            get => _isMapGenerationError;
            set => SetProperty(ref _isMapGenerationError, value);
        }

        public bool IsBlockedInput
        {
            get => _isBlockedInput;
            set => SetProperty(ref _isBlockedInput, value);
        }

        public ICommand StartCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        IsMapGenerationError = false;
                        ComputerPlayerViewModel.Start();
                        HumanPlayerViewModel.Start();
                    }
                    catch (GameException)
                    {
                        IsMapGenerationError = true;
                    }
                });
            }
        }

        public ICommand SelectCommand
        {
            get
            {
                return new DelegateCommand<BattleCellViewModel>(cell =>
                    {
                        IsBlockedInput = true;
                        ComputerPlayerViewModel.Fire(cell.Point);
                        if (ComputerPlayerViewModel.isWinner)
                        {
                            IsBlockedInput = false;
                            return;
                        }
                        Task.Delay(2000).ContinueWith(_ =>
                            {
                                HumanPlayerViewModel.Fire(null);
                                IsBlockedInput = false;
                            }
                        );
                    },
                    model =>
                        model.Type is FieldType.Boat or FieldType.See
                        && !(HumanPlayerViewModel.isWinner || ComputerPlayerViewModel.isWinner));
            }
        }
    }
}
