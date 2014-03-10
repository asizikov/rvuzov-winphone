using System;
using System.Diagnostics;
using System.Globalization;
using JetBrains.Annotations;

namespace TimeTable.ViewModel.Utils
{
    internal static class DateTimeUtils
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

         [PublicAPI]
        public static long GetCurrentUnixTimestampMillis()
        {
            return (long) (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
        }

         [PublicAPI]
        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

         [PublicAPI]
        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long) (DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        [PublicAPI]
        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }

        [PublicAPI]
        public static int GetWeekNumber(DateTime date)
        {
            var currentCulture = new CultureInfo("Ru-ru");
            return currentCulture.Calendar.GetWeekOfYear(
                date,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                DayOfWeek.Monday);
        }

        [PublicAPI]
        public static int GetRelativeWeekNumber(long parityCountdown)
        {
            var parityCountDown = DateTimeFromUnixTimestampSeconds(parityCountdown);
            Debug.WriteLine("count down" + parityCountDown);
            var currentWeekNumber = GetWeekNumber(DateTime.UtcNow);
            Debug.WriteLine("current week" + currentWeekNumber);
            var firstWeekNumber = GetWeekNumber(parityCountDown);
            Debug.WriteLine("firstWeek" + firstWeekNumber);
            if (currentWeekNumber >= firstWeekNumber)
            {
                return currentWeekNumber - firstWeekNumber + 1;
            }
            return currentWeekNumber + (52 - firstWeekNumber) + 1; //todo: fixme
        }
    }
}