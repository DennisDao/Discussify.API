﻿

using System.Runtime.CompilerServices;

namespace CommonDataContract.Extension
{
    public static class DateExtension
    {
        public static string ToElaspedTime(this DateTime value)
        {
            DateTime now = DateTime.Now;

            TimeSpan elapsedTime = now - value;

            if (elapsedTime.Days >= 1) 
                return  $"{elapsedTime.Days} {(elapsedTime.Days > 1 ? "days" : "day")} ago"; 
            if (elapsedTime.TotalHours >= 1) 
                return $"{elapsedTime.Hours} {(elapsedTime.Hours > 1 ? "hours" : "hour")} ago";
            if (elapsedTime.TotalMinutes >= 1) 
                return $"{elapsedTime.Minutes} {(elapsedTime.Minutes > 1 ? "minutes" : "minute")} ago";

            return "";
        }
    }
}
