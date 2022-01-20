using DiscountCalculator.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscountCalculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly IPageDialogService _pageDialogService;

        private int _goldPrice;
        /// <summary>
        /// Gets or sets the GoldPrice which is bound to entry
        /// </summary>
        public int GoldPrice
        {
            get { return _goldPrice; }
            set { SetProperty(ref _goldPrice, value); }
        }

        private int _weight;
        /// <summary>
        /// Gets or sets the Weight which is bound to entry
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set { SetProperty(ref _weight, value); }
        }

        private int _discount;
        /// <summary>
        /// Gets or sets the Discount which is bound to entry
        /// </summary>
        public int Discount
        {
            get { return _discount; }
            set { SetProperty(ref _discount, value); }
        }

        private ulong _result = 0;
        /// <summary>
        /// Gets or sets the Result which is bound to Label
        /// </summary>
        public ulong Result
        {
            get { return _result; }
            private set { SetProperty(ref _result, value); }
        }

        private DelegateCommand _calculateCommand;
        /// <summary>
        /// Gets the CalculateCommand which is bound to Calculate button
        /// </summary>
        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(ExecuteCalculateCommand));

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Discount Calculator";
            _pageDialogService = pageDialogService;
        }

        async void ExecuteCalculateCommand()
        {
            if (AreFieldsValid(out string errorMessage))
            {
                ulong netValue = (ulong)(GoldPrice * Weight);
                Result = netValue - ((ulong)Discount * netValue / 100);
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Constants.ERROR_TEXT, errorMessage, Constants.OK_TEXT);
                return;
            }
        }

        private bool AreFieldsValid(out string errorMessage)
        {
            if (GoldPrice <= 0)
            {
                errorMessage = Errors.REQUIRED_GOLD_PRICE;
                return false;
            }

            if (Weight <= 0)
            {
                errorMessage = Errors.REQUIRED_WEIGHT;
                return false;
            }

            if (GoldPrice >= int.MaxValue || Weight >= int.MaxValue)
            {
                errorMessage = Errors.CONVERSION_ERROR;
                return false;
            }

            if (Discount < 0 || Discount > 100)
            {
                errorMessage = Errors.INVALID_DISCOUNT;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
