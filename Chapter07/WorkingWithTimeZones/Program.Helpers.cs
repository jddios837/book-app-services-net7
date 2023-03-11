﻿using System.Collections.ObjectModel;

partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine();
        WriteLine($"*");
        WriteLine($"* {title}");
        WriteLine($"*");
        ForegroundColor = previousColor;
    }

    static void OutputTimeZones()
    {
        // get the timezone registered with the OS
        ReadOnlyCollection<TimeZoneInfo> zones =
            TimeZoneInfo.GetSystemTimeZones();
        
        WriteLine($"*");
        WriteLine($"* {zones.Count} time zones:");
        WriteLine($"*");
        
        // order the time zones by Id instead of DisplayName
        foreach (TimeZoneInfo zone in zones.OrderBy(z => z.Id))
        {
            WriteLine($" {zone.Id}");
        }
    }

    static void OutputDateTime(DateTime dateTime, string title)
    {
        SectionTitle(title);
        
        WriteLine($"Value: {dateTime}");
        WriteLine($"Kind: {dateTime.Kind}");
        WriteLine($"IsDaylightSavingTime: {dateTime.IsDaylightSavingTime()}");
        WriteLine($"ToLocalTime: {dateTime.ToLocalTime()}");
        WriteLine($"ToUniversalTime: {dateTime.ToUniversalTime()}");
    }

    static void OutputTimeZone(TimeZoneInfo zone, string title)
    {
        SectionTitle(title);
        
        WriteLine($"Id: {zone.Id}");
        WriteLine("IsDaylightSavingTime(DateTime.Now): {0}",
            zone.IsDaylightSavingTime(DateTime.Now));
        WriteLine($"StandardName: {zone.StandardName}");
        WriteLine($"DaylightName: {zone.DaylightName}");
        WriteLine($"BaseUTCOffset: {zone.BaseUtcOffset}");
    }

    static string GetCurrentZoneName(TimeZoneInfo zone, DateTime when)
    {
        // time zone names change if DayLight Saving time is active
        // e.g GMT Standard Time becomes GMT Summer Time
        return zone.IsDaylightSavingTime(when) ? zone.DaylightName : zone.StandardName;
    }
}