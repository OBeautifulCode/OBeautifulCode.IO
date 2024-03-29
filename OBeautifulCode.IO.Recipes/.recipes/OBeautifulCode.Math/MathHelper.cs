﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Math.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Math.Recipes
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using static global::System.FormattableString;

    /// <summary>
    /// Supports various mathematical and numerical methods.
    /// </summary>
#if !OBeautifulCodeMathSolution
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [global::System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Math.Recipes", "See package version number")]
    internal
#else
    public
#endif
    static class MathHelper
    {
        /// <summary>
        /// Determines if two values are almost equal (given some level of tolerance).
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the two values are almost equal, false if not.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="value1"/> or <paramref name="value2"/> is <see cref="double.NaN"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsAlmostEqualTo(
            this double value1,
            double value2,
            double tolerance = 1e-8)
        {
            if (double.IsNaN(value1))
            {
                throw new ArgumentException("value1 is NaN", nameof(value1));
            }

            if (double.IsNaN(value2))
            {
                throw new ArgumentException("value2 is NaN", nameof(value2));
            }

            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var diff = Math.Abs(value1 - value2);
            var almostEqual = diff <= tolerance;
            return almostEqual;
        }

        /// <summary>
        /// Determines if two values are almost equal (given some level of tolerance).
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the two values are almost equal, false if not.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsAlmostEqualTo(
            this decimal value1,
            decimal value2,
            decimal tolerance = 1e-8m)
        {
            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var diff = Math.Abs(value1 - value2);
            var almostEqual = diff <= tolerance;
            return almostEqual;
        }

        /// <summary>
        /// Determines if one value is greater than or almost equal (given some level of tolerance) to a second value.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the first value is greater than or almost equal to the second value, false if not.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="value1"/> or <paramref name="value2"/> is <see cref="double.NaN"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsGreaterThanOrAlmostEqualTo(
            this double value1,
            double value2,
            double tolerance = 1e-8)
        {
            if (double.IsNaN(value1))
            {
                throw new ArgumentException("value1 is NaN", nameof(value1));
            }

            if (double.IsNaN(value2))
            {
                throw new ArgumentException("value2 is NaN", nameof(value2));
            }

            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var result = (value1 > value2) || value1.IsAlmostEqualTo(value2, tolerance);
            return result;
        }

        /// <summary>
        /// Determines if one value is greater than or almost equal to a second value (given some level of tolerance).
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the first value is greater than or almost equal to the second value, false if not.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsGreaterThanOrAlmostEqualTo(
            this decimal value1,
            decimal value2,
            decimal tolerance = 1e-8m)
        {
            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var result = (value1 > value2) || value1.IsAlmostEqualTo(value2, tolerance);
            return result;
        }

        /// <summary>
        /// Determines if one value is less than or almost equal (given some level of tolerance) to a second value.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the first value is less than or almost equal to the second value, false if not.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="value1"/> or <paramref name="value2"/> is <see cref="double.NaN"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsLessThanOrAlmostEqualTo(
            this double value1,
            double value2,
            double tolerance = 1e-8)
        {
            if (double.IsNaN(value1))
            {
                throw new ArgumentException("value1 is NaN", nameof(value1));
            }

            if (double.IsNaN(value2))
            {
                throw new ArgumentException("value2 is NaN", nameof(value2));
            }

            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var result = (value1 < value2) || value1.IsAlmostEqualTo(value2, tolerance);
            return result;
        }

        /// <summary>
        /// Determines if one value is less than or almost equal to a second value (given some level of tolerance).
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The the second value.</param>
        /// <param name="tolerance">
        /// The tolerance for differences between the specified values.  If the
        /// absolute value of the difference between the two values is less than or equal to
        /// this tolerance, then the two values are considered to be almost equal.
        /// </param>
        /// <returns>
        /// true if the first value is less than or almost equal to the second value, false if not.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tolerance"/> is less than 0.</exception>
        public static bool IsLessThanOrAlmostEqualTo(
            this decimal value1,
            decimal value2,
            decimal tolerance = 1e-8m)
        {
            if (tolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance), "tolerance is less than 0");
            }

            var result = (value1 < value2) || value1.IsAlmostEqualTo(value2, tolerance);
            return result;
        }

        /// <summary>
        /// Calculates the covariance of two sets of doubles.
        /// </summary>
        /// <param name="values1">The first set of doubles.</param>
        /// <param name="values2">The second set of doubles.</param>
        /// <returns>
        /// Returns the covariance of two sets of doubles.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values1"/> or <paramref name="values2"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values1"/> or <paramref name="values2"/> is empty.</exception>
        /// <exception cref="ArgumentException">The sets have different lengths.</exception>
        public static double Covariance(
            IList<double> values1,
            IList<double> values2)
        {
            if (values1 == null)
            {
                throw new ArgumentNullException(nameof(values1));
            }

            if (values1.Count == 0)
            {
                throw new ArgumentException("values1 is empty", nameof(values1));
            }

            if (values2 == null)
            {
                throw new ArgumentNullException(nameof(values2));
            }

            if (values2.Count == 0)
            {
                throw new ArgumentException("values2 is empty", nameof(values2));
            }

            int valuesCount = values1.Count;
            if (valuesCount != values2.Count)
            {
                throw new ArgumentException("The sets have different lengths");
            }

            // covariance of one item is always 0
            if (valuesCount == 1)
            {
                return 0;
            }

            // do the math
            double avg1 = values1.Average();
            double avg2 = values2.Average();
            double cov = 0;
            for (int i = 0; i < valuesCount; i++)
            {
                cov += (values1[i] - avg1) * (values2[i] - avg2);
            }

            cov /= valuesCount;
            return cov;
        }

        /// <summary>
        /// Calculates the covariance of two sets of decimal.
        /// </summary>
        /// <param name="values1">The first set of decimals.</param>
        /// <param name="values2">The second set of decimals.</param>
        /// <returns>
        /// Returns the covariance of two sets of decimals.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values1"/> or <paramref name="values2"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values1"/> or <paramref name="values2"/> is empty.</exception>
        /// <exception cref="ArgumentException">The sets have different lengths.</exception>
        public static decimal Covariance(
            IList<decimal> values1,
            IList<decimal> values2)
        {
            if (values1 == null)
            {
                throw new ArgumentNullException(nameof(values1));
            }

            if (values1.Count == 0)
            {
                throw new ArgumentException("values1 is empty", nameof(values1));
            }

            if (values2 == null)
            {
                throw new ArgumentNullException(nameof(values2));
            }

            if (values2.Count == 0)
            {
                throw new ArgumentException("values2 is empty", nameof(values2));
            }

            int valuesCount = values1.Count;
            if (valuesCount != values2.Count)
            {
                throw new ArgumentException("The sets have different lengths");
            }

            // covariance of one item is always 0
            if (valuesCount == 1)
            {
                return 0;
            }

            // do the math
            decimal avg1 = values1.Average();
            decimal avg2 = values2.Average();
            decimal cov = 0;
            for (int i = 0; i < valuesCount; i++)
            {
                cov += (values1[i] - avg1) * (values2[i] - avg2);
            }

            cov /= valuesCount;
            return cov;
        }

        /// <summary>
        /// Determines the factors of a number.
        /// </summary>
        /// <param name="toFactor">The number whose factors are to be returned.</param>
        /// <returns>
        /// Returns the factors of a number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="toFactor"/> is less than or equal to 0.</exception>
        public static IEnumerable<int> Factors(
            int toFactor)
        {
            if (toFactor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toFactor), "The number to factor is less than or equal to 0");
            }

            int max = toFactor / 2;
            for (int i = 1; i <= max; i++)
            {
                if ((toFactor % i) == 0)
                {
                    yield return i;
                }
            }

            yield return toFactor;
        }

        /// <summary>
        /// Generates a truth table of booleans.
        /// </summary>
        /// <param name="numberOfInputs">The number of inputs.</param>
        /// <param name="resultInBigEndian">
        /// A value that determines whether the result is ordered with most significant bit first, in a numeric progression represented in binary using booleans.
        /// DEFAULT is true; the most significant bit will be first.
        /// If true (most significant bit first), then results will be as follows: {false, false}, {false, true}, {true, false}, {true, true}.
        /// If false (least significant bit first), then results will be as follows: {false, false}, {true, false}, {false, true}, {true, true}.
        /// </param>
        /// <returns>
        /// The truth table represented as a list of entries, where each entry is a list of <see cref="bool"/>s
        /// in a binary representation of the number within the numeric progression.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="numberOfInputs"/> is less than 0.</exception>
        public static IReadOnlyList<IReadOnlyList<bool>> GenerateTruthTable(
            int numberOfInputs,
            bool resultInBigEndian = true)
        {
            if (numberOfInputs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfInputs), Invariant($"{nameof(numberOfInputs)} must be >= 0."));
            }

            var result = new List<IReadOnlyList<bool>>();

            if (numberOfInputs == 0)
            {
                return result;
            }

            var maxOptions = Math.Pow(2, numberOfInputs);

            for (var option = 0; option < maxOptions; option++)
            {
                var row = new bool[numberOfInputs];

                for (var position = 0; position < numberOfInputs; position++)
                {
                    row[position] = (option & (1 << position)) != 0;
                }

                if (resultInBigEndian)
                {
                    // could also right shift and tweak check above; six of one half-dozen of the other...
                    result.Add(row.Reverse().ToList());
                }
                else
                {
                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the standard deviation of a set of doubles.
        /// </summary>
        /// <param name="values">The double values to use in the calculation.</param>
        /// <returns>
        /// Returns the standard deviation of the set.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty or has only one element.</exception>
        /// <exception cref="ArgumentException">There is only one unique value in <paramref name="values"/>.  Two or more required.</exception>
        public static double StandardDeviation(
            IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesList = values as IList<double> ?? values.ToArray();
            if (valuesList.Count <= 1)
            {
                throw new ArgumentException("values is empty or has only one element", nameof(values));
            }

            double avg = valuesList.Average();
            double sumOfSquares = valuesList.Sum(value => Math.Pow(value - avg, 2));
            return Math.Sqrt(sumOfSquares / Convert.ToDouble(valuesList.Count - 1));
        }

        /// <summary>
        /// Returns the standard deviation of a set of decimals.
        /// </summary>
        /// <param name="values">The decimal values to use in the calculation.</param>
        /// <returns>
        /// Returns the standard deviation of the set.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty or has only one element.</exception>
        /// <exception cref="ArgumentException">There is only one unique value in <paramref name="values"/>.  Two or more required.</exception>
        public static decimal StandardDeviation(
            IEnumerable<decimal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesList = values as IList<decimal> ?? values.ToArray();
            if (valuesList.Count <= 1)
            {
                throw new ArgumentException("values is empty or has only one element", nameof(values));
            }

            var result = Convert.ToDecimal(StandardDeviation(valuesList.Select(Convert.ToDouble)));
            return result;
        }

        /// <summary>
        /// Rounds a decimal to a given number of digits.
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <param name="digits">The number of digits to round to.</param>
        /// <param name="strategy">OPTIONAL strategy to use to round the number.  DEFAULT is to round to the nearest number, and when a number is halfway between two others, it's rounded toward the nearest number that's away from zero.</param>
        /// <returns>
        /// Returns the rounded decimal.
        /// </returns>
        public static decimal Round(
            this decimal value,
            int digits,
            MidpointRounding strategy = MidpointRounding.AwayFromZero)
        {
            var result = Math.Round(value, digits, strategy);

            return result;
        }

        /// <summary>
        /// Rounds a decimal to a given number of digits if not null.
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <param name="digits">The number of digits to round to.</param>
        /// <param name="strategy">OPTIONAL strategy to use to round the number.  DEFAULT is to round to the nearest number, and when a number is halfway between two others, it's rounded toward the nearest number that's away from zero.</param>
        /// <returns>
        /// Returns the rounded decimal if not null; otherwise null.
        /// </returns>
        public static decimal? Round(
            this decimal? value,
            int digits,
            MidpointRounding strategy = MidpointRounding.AwayFromZero)
        {
            var result = value?.Round(digits, strategy);

            return result;
        }

        /// <summary>
        /// Truncates everything after the decimal point of a decimal and returns the resulting integer number.
        /// </summary>
        /// <param name="value">The decimal to truncate into an integer.</param>
        /// <returns>Integer with the truncated double.</returns>
        /// <remarks>1.49 will return 1, 1.51 will return 1, 1.99 will return 1.</remarks>
        /// <exception cref="OverflowException"><paramref name="value"/> overflows the bounds of an <see cref="int"/>.</exception>
        public static int Truncate(
            decimal value)
        {
            if ((value > int.MaxValue) || (value < int.MinValue))
            {
                throw new OverflowException("decimal value overflows the bounds of an Int32");
            }

            // if we get here then we don't need to check whether value-.5 will overflow a double's max/min values
            return Convert.ToInt32(Math.Truncate(value));
        }

        /// <summary>
        /// Truncates everything after the decimal point of a double and returns the resulting integer number.
        /// </summary>
        /// <param name="value">The double to truncate.</param>
        /// <returns>Integer with the truncated double.</returns>
        /// <remarks>1.49 will return 1, 1.51 will return 1, 1.99 will return 1.</remarks>
        /// <exception cref="OverflowException"><paramref name="value"/> overflows the bounds of an <see cref="int"/>.</exception>
        public static int Truncate(
            double value)
        {
            if ((value > int.MaxValue) || (value < int.MinValue))
            {
                throw new OverflowException("double value overflows the bounds of an Int32");
            }

            // if we get here then we don't need to check whether value-.5 will overflow a double's max/min values
            return Convert.ToInt32(Math.Truncate(value));
        }

        /// <summary>
        /// Truncates a decimal to a given number of digits.
        /// </summary>
        /// <param name="value">The value to truncate.</param>
        /// <param name="digits">The number of digits to keep.</param>
        /// <returns>
        /// Returns the truncated decimal.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="digits"/> must be less than 0.</exception>
        /// <exception cref="OverflowException"><paramref name="digits"/> is too high.</exception>
        public static decimal TruncateSignificantDigits(
            decimal value,
            int digits)
        {
            if (digits < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), "digits is less than 0");
            }

            if (digits == 0)
            {
                return decimal.Truncate(value);
            }

            return decimal.Truncate(value * (decimal)Math.Pow(10, digits)) / (decimal)Math.Pow(10, digits);
        }

        /// <summary>
        /// Calculates the variance of a set of doubles.
        /// </summary>
        /// <param name="values">The values used in the calculation.</param>
        /// <returns>
        /// Returns the variance of the set of doubles.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty or has only one element.</exception>
        public static double Variance(
            IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesList = values as IList<double> ?? values.ToArray();
            if (valuesList.Count <= 1)
            {
                throw new ArgumentException("values is empty or has only one element", nameof(values));
            }

            double avg = valuesList.Average();
            double sum = valuesList.Sum(value => Math.Pow(value - avg, 2));
            return sum / valuesList.Count;
        }

        /// <summary>
        /// Returns the variance of a set of decimals.
        /// </summary>
        /// <param name="values">The values used in the calculation.</param>
        /// <returns>
        /// Returns the variance of the set of decimals.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty or has only one element.</exception>
        public static decimal Variance(
            IEnumerable<decimal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesList = values as IList<decimal> ?? values.ToArray();
            if (valuesList.Count <= 1)
            {
                throw new ArgumentException("values is empty or has only one element", nameof(values));
            }

            var result = Convert.ToDecimal(Variance(valuesList.Select(Convert.ToDouble)));
            return result;
        }

        /// <summary>
        /// Converts an int to a GUID.
        /// </summary>
        /// <remarks>
        /// Adapted from <a href="https://stackoverflow.com/a/4826200/356790" />.
        /// </remarks>
        /// <param name="value">The int to convert.</param>
        /// <returns>
        /// A GUID converted from an int.
        /// </returns>
        public static Guid ToGuid(
            this int value)
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            var result = new Guid(bytes);
            return result;
        }

        /// <summary>
        /// Determines if an integer value is even.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// true if the value is even; otherwise false.
        /// </returns>
        public static bool IsEven(
            this int value)
        {
            var result = value % 2 == 0;
            return result;
        }

        /// <summary>
        /// Determines if an integer value is odd.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// true if the value is odd; otherwise false.
        /// </returns>
        public static bool IsOdd(
            this int value) => !IsEven(value);
    }
}
