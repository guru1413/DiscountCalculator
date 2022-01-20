using DiscountCalculator.Helpers;
using DiscountCalculator.ViewModels;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCalculator.Test
{
    [TestFixture]
    class MainPageViewModelTest
    {
        Mock<INavigationService> _mockNavigationService;
        Mock<IPageDialogService> _mockPageDialogService;
        MainPageViewModel _mainPageViewModel;

        [SetUp]
        public void Setup()
        {
            _mockNavigationService = new Mock<INavigationService>();
            _mockPageDialogService = new Mock<IPageDialogService>();
            _mainPageViewModel = new MainPageViewModel(_mockNavigationService.Object, _mockPageDialogService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockNavigationService = null;
            _mainPageViewModel = null;
            _mockPageDialogService = null;
        }

        [Test]
        [TestCase(1000, 10, 0, 10000)]
        [TestCase(1000, 10, 5, 9500)]
        [TestCase(0, 10, 5, 0)]
        [TestCase(1000, 0, 5, 0)]
        public void CalculateDiscount(int goldPrice, int weight, int discount, long result)
        {
            _mainPageViewModel.GoldPrice = goldPrice;
            _mainPageViewModel.Weight = weight;
            _mainPageViewModel.Discount = discount;

            _mainPageViewModel.CalculateCommand.Execute();

            Assert.AreEqual(_mainPageViewModel.Result, result);
        }

        [Test]
        [TestCase(0, 1, 0, Errors.REQUIRED_GOLD_PRICE)]
        [TestCase(-10, 1, 0, Errors.REQUIRED_GOLD_PRICE)]
        [TestCase(100, 0, 0, Errors.REQUIRED_WEIGHT)]
        [TestCase(100, -1, 0, Errors.REQUIRED_WEIGHT)]
        [TestCase(int.MaxValue, 10, 5, Errors.CONVERSION_ERROR)]
        [TestCase(1000, int.MaxValue, 5, Errors.CONVERSION_ERROR)]
        [TestCase(100, 10, -1, Errors.INVALID_DISCOUNT)]
        [TestCase(100, 10, 101, Errors.INVALID_DISCOUNT)]
        public void CalculateDiscountWithInvalidValues_AlertMessageShown(int goldPrice, int weight, int discount, string errorMessage)
        {
            _mainPageViewModel.GoldPrice = goldPrice;
            _mainPageViewModel.Weight = weight;
            _mainPageViewModel.Discount = discount;

            _mainPageViewModel.CalculateCommand.Execute();

            _mockPageDialogService.Verify(x => x.DisplayAlertAsync(Constants.ERROR_TEXT, errorMessage, Constants.OK_TEXT));
        }
    }
}
