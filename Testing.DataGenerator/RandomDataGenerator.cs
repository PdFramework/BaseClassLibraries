namespace PeinearyDevelopment.Framework.BaseClassLibraries.Testing.DataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class RandomDataGenerator
    {
        //http://stackoverflow.com/questions/1122483/1874522/random-string-generator-returning-same-string#answer-1122519
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
        private static readonly Dictionary<string, LanguageStringSpecifier> LanguageCharacters = new Dictionary<string, LanguageStringSpecifier>
        {
            { "en-us", new LanguageStringSpecifier { TokenBeginners = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", TokenFillers = "abcdefghijklmnopqrstuvwxyz" } },
            { "he", new LanguageStringSpecifier { TokenFillers ="אבגדהוזחטיכלמנסעפקצרשת", TokenEnders = "ךםןףץ" } }
        };
        private const string Digits = "1234567890";
        private const string WhiteSpaceCharacters = " \t\n\r";
        private const string SpecialCharacters = "`-=[]\\;',./~!@#$%^&*()_+|}{\":?><";

        public static string GenerateRandomString(int size)
        {
            return GenerateRandomString(size, "en-us");
        }

        public static string GenerateRandomString(int size, string language)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                builder.Append(GenerateRandomItemFromEnumerable<string, char>(string.Concat(LanguageCharacters[language].TokenBeginners, LanguageCharacters[language].TokenFillers, Digits, WhiteSpaceCharacters, SpecialCharacters)));
            }

            return builder.ToString();
        }

        public static int GenerateRandomInt32()
        {
            return Random.Next();
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.</param>
        /// <returns>A 32-bit signed integer less than or equal to maxValue.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static int GenerateRandomInt32(int maxValue)
        {
            return Random.Next(maxValue == int.MaxValue ? maxValue : maxValue + 1);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than or equal to maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public static int GenerateRandomInt32(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue == int.MaxValue ? maxValue : maxValue + 1);
        }

        public static DateTimeOffset GenerateRandomDateTimeOffset()
        {
            return GenerateRandomDateTimeOffset(null);
        }
        //TODO: update to make random date with lower bound a little better(month only has to use previous month for lower bound if year is the same, etc)
        //TODO: update to make random date more accurate(update days to reflect accurately for month -- shouldn't be 31 for February, etc)
        public static DateTimeOffset GenerateRandomDateTimeOffset(DateTimeOffset? lowerBoundDateTimeOffset)
        {
            var needsRelativeLowerBound = lowerBoundDateTimeOffset != null;
            var lowerYearBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Year : DateTimeOffset.MinValue.Year;
            var year = Random.Next(lowerYearBound, DateTimeOffset.MaxValue.Year + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && year == lowerBoundDateTimeOffset.Value.Year;
            var lowerMonthBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Month : DateTimeOffset.MinValue.Month;
            var month = Random.Next(lowerMonthBound, DateTimeOffset.MaxValue.Month + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && month == lowerBoundDateTimeOffset.Value.Month;
            var lowerDayBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Day : DateTimeOffset.MinValue.Day;
            var upperDayBound = new[] { 1, 3, 5, 7, 8, 10, 12 }.Contains(month) ? 31 : new[] { 4, 6, 9, 11 }.Contains(month) ? 30 : year % 4 != 0 ? 28 : 29;
            var day = Random.Next(lowerDayBound, upperDayBound + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && day == lowerBoundDateTimeOffset.Value.Day;
            var lowerHourBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Hour : DateTimeOffset.MinValue.Hour;
            var hour = Random.Next(lowerHourBound, DateTimeOffset.MaxValue.Hour + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && hour == lowerBoundDateTimeOffset.Value.Hour;
            var lowerMinuteBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Minute : DateTimeOffset.MinValue.Minute;
            var minute = Random.Next(lowerMinuteBound, DateTimeOffset.MaxValue.Minute + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && minute == lowerBoundDateTimeOffset.Value.Minute;
            var lowerSecondBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Second : DateTimeOffset.MinValue.Second;
            var second = Random.Next(lowerSecondBound, DateTimeOffset.MaxValue.Second + 1);

            needsRelativeLowerBound = needsRelativeLowerBound && second == lowerBoundDateTimeOffset.Value.Second;
            var lowerMillisecondBound = needsRelativeLowerBound ? lowerBoundDateTimeOffset.Value.Millisecond : DateTimeOffset.MinValue.Millisecond;
            var millisecond = Random.Next(lowerMillisecondBound, DateTimeOffset.MaxValue.Millisecond + 1);

            var lowerBoundTimeSpan = lowerBoundDateTimeOffset == null ? (TimeSpan?)null : new TimeSpan(lowerBoundDateTimeOffset.Value.Ticks);
            needsRelativeLowerBound = needsRelativeLowerBound && millisecond == lowerBoundDateTimeOffset.Value.Millisecond;
            var lowerTimeSpanHourBound = needsRelativeLowerBound ? lowerBoundTimeSpan.Value.Hours : DateTimeOffset.MinValue.Hour;
            const int upperTimeSpanHourBound = 14; // must be within 14 hours
            var timeSpanHour = Random.Next(lowerTimeSpanHourBound < upperTimeSpanHourBound ? lowerTimeSpanHourBound : upperTimeSpanHourBound, upperTimeSpanHourBound);

            needsRelativeLowerBound = needsRelativeLowerBound && timeSpanHour == lowerBoundTimeSpan.Value.Hours;
            var lowerTimeSpanMinuteBound = needsRelativeLowerBound ? lowerBoundTimeSpan.Value.Minutes : DateTimeOffset.MinValue.Minute;
            var timeSpanMinute = Random.Next(lowerTimeSpanMinuteBound, DateTimeOffset.MaxValue.Minute + 1);

            return new DateTimeOffset(year, month, day, hour, minute, second, millisecond, new TimeSpan(0, timeSpanHour, timeSpanMinute, 0, 0)); // must be in whole minutes
        }

        public static T GenerateRandomItemFromEnumerable<TEnumerable, T>(TEnumerable enumerable) where TEnumerable : IEnumerable<T>
        {
            return enumerable.ElementAt(Random.Next(0, enumerable.Count() - 1));
        }
    }
}
