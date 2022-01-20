using System;
using System.Collections.Generic;
using System.Text;

namespace DiscountCalculator.Helpers
{
    public class Errors
    {
        public const string INVALID_USERNAME = "Invalid username";
        public const string INVALID_PASSWORD = "Invalid password";
        public const string INVALID_DISCOUNT = "The discount value ranges from 0 to 100.";
        public const string REQUIRED_GOLD_PRICE = "Gold price should be greater than 0";
        public const string REQUIRED_WEIGHT = "Weight should be greater than 0";
        public const string CONVERSION_ERROR = "Conversion is not possible";

    }
}
