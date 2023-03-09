// See https://aka.ms/new-console-template for more information
// Thread.CurrentThread.CurrentCulture = 
//     System.Globalization.CultureInfo.GetCultureInfo("en-EN");

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
