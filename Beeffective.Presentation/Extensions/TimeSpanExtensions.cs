using System;

namespace Beeffective.Presentation.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToFormattedString(this TimeSpan timeSpan)
        {
            string result = string.Empty;

            if (timeSpan.Days > 0)
            {
                result += $"{timeSpan.Days}d ";
            }

            if (timeSpan.Hours > 0)
            {
                result += $"{timeSpan.Hours}h ";
            }

            if (timeSpan.Minutes > 0)
            {
                result += $"{timeSpan.Minutes}m ";
            }

            if (timeSpan.Seconds > 0)
            {
                result += $"{timeSpan.Seconds}s";
            }

            if (timeSpan <= TimeSpan.Zero)
            {
                result += "- ";
                if (timeSpan.Days < 0)
                {
                    result += $"{-1 * timeSpan.Days}d ";
                }

                if (timeSpan.Hours < 0)
                {
                    result += $"{-1 * timeSpan.Hours}h ";
                }

                if (timeSpan.Minutes < 0)
                {
                    result += $"{-1 * timeSpan.Minutes}m ";
                }

                if (timeSpan.Seconds < 0)
                {
                    result += $"{-1 * timeSpan.Seconds}s";
                }
            }

            return result;
        }
    }
}