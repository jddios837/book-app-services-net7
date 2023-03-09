// See https://aka.ms/new-console-template for more information
// Thread.CurrentThread.CurrentCulture = 
//     System.Globalization.CultureInfo.GetCultureInfo("en-EN");
using System.Globalization; // CultureInfo

SectionTitle("Specifying date and time values");

WriteLine($"DateTime.MinValue:    {DateTime.MinValue}");
WriteLine($"DateTime.MaxValue:    {DateTime.MaxValue}");
WriteLine($"DateTime.UnixEpoch:   {DateTime.UnixEpoch}");
WriteLine($"DateTime.Now:         {DateTime.Now}");
WriteLine($"DateTime.Today:       {DateTime.Today}");


DateTime xmas = new(year: 2024, month:12, day: 25);
WriteLine($"Christmas (default format): {xmas}");
WriteLine($"Christmas (custom format): {xmas:dddd, dd MMMM yyyy}");
WriteLine($"Christmas is in month: {xmas.Month} of the year");
WriteLine($"Christmas is day: {xmas.DayOfYear} of the year 2024");
WriteLine($"Christmas {xmas.Year} is on a {xmas.DayOfWeek}.");


SectionTitle("Date and time calculations");
DateTime beforeXmas = xmas.Subtract(TimeSpan.FromDays(12));
DateTime afterXmas = xmas.AddDays(12);

// :d means format as short date only without time
WriteLine($"12 days before of Christmas {beforeXmas:d}");
WriteLine($"12 days after of Christmas {afterXmas:d}");

TimeSpan untilXmas = xmas - DateTime.Now;

WriteLine($"Now: {DateTime.Now}");
WriteLine("There are {0} days and {1} hours until Christmas 2024.", 
    arg0: untilXmas.Days, arg1: untilXmas.Hours);

WriteLine("There are {0:N0} hours until Christmas 2024.", 
    arg0: untilXmas.TotalHours);

DateTime kidsWakeUp = new(year: 2024, month: 12, day: 25,
    hour: 6, minute: 30, second: 0);

WriteLine($"Kids wake up {kidsWakeUp}");
WriteLine($"Kids woke me up at {kidsWakeUp.ToShortTimeString()}");


SectionTitle("Milli-, micro-, and nanoseconds");

DateTime preciseTime = new(year: 2022, month: 11, day: 8,
    hour: 12, minute: 0, second: 0,
    millisecond: 6, microsecond: 999);

WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
    preciseTime.Millisecond, preciseTime.Microsecond, preciseTime.Nanosecond);

preciseTime = DateTime.UtcNow;

// Nanosecond value will be 0 to 900 in 100 nanosecond increments.
WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
    preciseTime.Millisecond, preciseTime.Microsecond, preciseTime.Nanosecond);

SectionTitle("Globalization with Dates and Times");

// same as Thread.CurrentThread.CurrentCulture
WriteLine($"Current culture is: {CultureInfo.CurrentCulture.Name}");

string textDate = "4 July 2024";
DateTime independenceDay = DateTime.Parse(textDate);

WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");  

textDate = "7/4/2024";
independenceDay = DateTime.Parse(textDate);

WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");

independenceDay = DateTime.Parse(textDate, provider: CultureInfo.GetCultureInfo("en-US"));

WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");

WriteLine($"Culture Info: {CultureInfo.GetCultureInfo("en-US")}");

for (int year = 2022; year <= 2028; year++)
{
    Write($"{year} is a Leap Year: {DateTime.IsLeapYear(year)} ");
    WriteLine("There are {0} days in February {1}.",DateTime.DaysInMonth(year: year, month: 2), year);
}

WriteLine("Is Christmas daylight saving time? {0}", xmas.IsDaylightSavingTime());
WriteLine("Is July 4 daylight saving time? {0}", independenceDay.IsDaylightSavingTime());