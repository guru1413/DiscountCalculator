using DiscountCalculator.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;

namespace DiscountCalculator.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        readonly IPageDialogService _pageDialogService;

        private string _userName;
        /// <summary>
        /// Gets or sets the UserName which is bound to entry
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        /// <summary>
        /// Gets or sets the Password which is bound to entry
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private DelegateCommand _loginCommand;
        /// <summary>
        /// Gets the LoginCommand which is bound to login button
        /// </summary>
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(ExecuteLoginCommand));

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
        }
        async void ExecuteLoginCommand()
        {
            if (AreFieldsValid(out string errorMessage))
            {
                await NavigationService.NavigateAsync("MainPage");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Constants.ERROR_TEXT, errorMessage, Constants.OK_TEXT);
            }
        }

        private bool AreFieldsValid(out string errorMessage)
        {
            if (!string.Equals(UserName, Constants.REGISTERED_USER, StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = Errors.INVALID_USERNAME;
                return false;
            }
            
            if (Password != Constants.PASSWORD)
            {
                errorMessage = Errors.INVALID_PASSWORD;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
