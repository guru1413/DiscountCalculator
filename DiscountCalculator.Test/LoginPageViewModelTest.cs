using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountCalculator.Helpers;
using DiscountCalculator.ViewModels;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;

namespace DiscountCalculator.Test
{
    [TestFixture]
    class LoginPageViewModelTest
    {
        Mock<INavigationService> _mockNavigationService;
        Mock<IPageDialogService> _mockPageDialogService;
        LoginPageViewModel _loginPageViewModel;

        [SetUp]
        public void Setup()
        {
            _mockNavigationService = new Mock<INavigationService>();
            _mockPageDialogService = new Mock<IPageDialogService>();
            _loginPageViewModel = new LoginPageViewModel(_mockNavigationService.Object, _mockPageDialogService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockNavigationService = null;
            _mockPageDialogService = null;
            _loginPageViewModel = null;
        }

        [Test]
        [TestCase(Constants.REGISTERED_USER, Constants.PASSWORD)]
        public void LogInWithValidCredentials(string userName, string password)
        {
            _loginPageViewModel.UserName = userName;
            _loginPageViewModel.Password = password;

            _loginPageViewModel.LoginCommand.Execute();

            _mockNavigationService.Verify(x => x.NavigateAsync("MainPage"));
        }

        [Test]
        [TestCase("", "pass", Errors.INVALID_USERNAME)]
        [TestCase("   ", "pass", Errors.INVALID_USERNAME)]
        [TestCase(null, "pass", Errors.INVALID_USERNAME)]
        [TestCase("testuser", "pass", Errors.INVALID_USERNAME)]
        public void LogInWithInvalidUserName_AlertMessageShown(string userName, string password, string errorMessage)
        {
            _loginPageViewModel.UserName = userName;
            _loginPageViewModel.Password = password;

            _loginPageViewModel.LoginCommand.Execute();


            _mockPageDialogService.Verify(x => x.DisplayAlertAsync(Constants.ERROR_TEXT, errorMessage, Constants.OK_TEXT));
        }

        [Test]
        [TestCase(Constants.REGISTERED_USER, "pass", Errors.INVALID_PASSWORD)]
        [TestCase(Constants.REGISTERED_USER, null, Errors.INVALID_PASSWORD)]
        [TestCase(Constants.REGISTERED_USER, "", Errors.INVALID_PASSWORD)]
        [TestCase(Constants.REGISTERED_USER, "pass@123", Errors.INVALID_PASSWORD)]
        public void LogInWithInvalidPassword_AlertMessageShown(string userName, string password, string errorMessage)
        {
            _loginPageViewModel.UserName = userName;
            _loginPageViewModel.Password = password;

            _loginPageViewModel.LoginCommand.Execute();


            _mockPageDialogService.Verify(x => x.DisplayAlertAsync(Constants.ERROR_TEXT, errorMessage, Constants.OK_TEXT));
        }
    }
}
