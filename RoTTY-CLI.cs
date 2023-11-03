using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Colorify;
using Colorify.UI;
using ToolBox;

class Program
{
    public static Format? colorify { get; set; }


    static decimal NewYear = 2 * (decimal)Math.PI;

    static Dictionary<int, List<decimal>> Transits(decimal moon, int startYear = 1, int endYear = 4)
    {
        decimal planet = 1.0m;
        int lunarEclipses = 0;
        int thisYear = startYear;
        decimal time = 0.0m;
        decimal duration = NewYear / planet;
        Dictionary<int, List<decimal>> events = new Dictionary<int, List<decimal>>();

        while (thisYear <= endYear)
        {
            decimal t = NewYear * lunarEclipses / (moon - planet);
            while (t - time > duration)
            {
                thisYear += 1;
                time += duration;
                if (thisYear > endYear)
                {
                    return events;
                }
            }
            decimal days = (planet * t) % NewYear;
            if (!events.ContainsKey(thisYear))
            {
                events[thisYear] = new List<decimal>();
            }
            events[thisYear].Add(days * 180m / (decimal)Math.PI);
            lunarEclipses += 1;
        }
        return events;
    }

    static decimal MarkCalendar(List<decimal> angles)
    {
        if (angles.Count < 2)
        {
            return 0.0m;
        }

        angles = angles.Select(angle => angle == 0m ? 360m : angle).ToList();

        decimal averageTransits = angles
            .Skip(1)
            .Select((angle, i) => angle - angles[i])
            .Average();
        return (360.0m / averageTransits) + 1.0m;
    }


    static void Main(string[] args)
    {
        colorify = new Format(Theme.Dark);
        colorify.WriteLine("     _/\\/\\/\\/\\/\\___     ____________     _/\\/\\/\\/\\/\\/\\_     _/\\/\\/\\/\\/\\/\\_     _/\\/\\____/\\/\\_", Colors.txtInfo);
        colorify.WriteLine("    _/\\/\\____/\\/\\_     ___/\\/\\/\\___     _____/\\/\\_____     _____/\\/\\_____     _/\\/\\____/\\/\\_", Colors.txtInfo);
        colorify.WriteLine("   _/\\/\\/\\/\\/\\___     _/\\/\\__/\\/\\_     _____/\\/\\_____     _____/\\/\\_____     ___/\\/\\/\\/\\___", Colors.txtInfo);
        colorify.WriteLine("  _/\\/\\__/\\/\\___     _/\\/\\__/\\/\\_     _____/\\/\\_____     _____/\\/\\_____     _____/\\/\\_____", Colors.txtInfo);
        colorify.WriteLine(" _/\\/\\____/\\/\\_     ___/\\/\\/\\___     _____/\\/\\_____     _____/\\/\\_____     _____/\\/\\_____", Colors.txtInfo);
        colorify.WriteLine("______________     ____________     ______________     ______________     ______________", Colors.txtInfo);
        colorify.WriteLine("                                                                             C L I", Colors.txtInfo);



        colorify.WriteLine("+------------------------------------+", Colors.txtInfo);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("¦ -d (#) to set decimal places       ¦");
        Console.WriteLine("¦ -r (#-#) to set year range         ¦");
        Console.WriteLine("¦ ratio for interval is 1/seed       ¦");
        Console.WriteLine("¦ pi notation, ie: 13pi              ¦");
        colorify.WriteLine("+------------------------------------+\n", Colors.txtInfo);

        int decimalLimit = 15;
        int[] yearRange = { 1, 4 };
        bool translateToText = false;
        bool isInputSeed = true;
        Console.ForegroundColor = ConsoleColor.Green;

        while (true)
        {

            if (isInputSeed)
            {
                Console.Write("Seed: ");
                string? input = ReadInput();
                if (string.IsNullOrEmpty(input))
                {
                    isInputSeed = !isInputSeed;
                    continue;
                }
                else
                {
                    try
                    {

                        if (input.StartsWith("-"))
                        {
                            string[] parts = input.Split(' ');
                            string command = parts[0];

                            if (command == "-d" && parts.Length == 2)
                            {
                                if (int.TryParse(parts[1], out int limit))
                                {
                                    decimalLimit = limit;
                                    Console.WriteLine($"¦ Decimal limit set to {decimalLimit}");
                                }
                                else
                                {
                                    Console.WriteLine("¦ Error: Invalid decimal limit.");
                                }
                            }
                            else if (command == "-r" && parts.Length == 2)
                            {
                                string[] rangeParts = parts[1].Split('-');
                                if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                                {
                                    yearRange[0] = start;
                                    yearRange[1] = end;
                                    Console.WriteLine($"¦ Year range set to {start}-{end}");
                                }
                                else
                                {
                                    Console.WriteLine("¦ Error: Invalid year range");
                                }
                            }
                            else if (command == "-t")
                            {
                                translateToText = !translateToText;

                                if (translateToText)
                                {
                                    colorify.WriteLine("Translation enabled", Colors.txtWarning);
                                }
                                else
                                {
                                    colorify.WriteLine("Translation disabled", Colors.txtWarning);
                                }

                            }
                            else
                            {
                                Console.WriteLine("¦ Error: Invalid command");
                            }
                        }
                        else
                        {
                            decimal moon;
                            if (decimal.TryParse(input, out moon))
                            {

                                Dictionary<int, List<decimal>> transitAngles = Transits(moon, yearRange[0], yearRange[1]);
                                colorify.WriteLine("+------------------------+", Colors.txtInfo);
                                Console.WriteLine($"¦ Results for Seed: {input}");
                                colorify.WriteLine("+------------------------+", Colors.txtInfo);
                                Console.ForegroundColor = ConsoleColor.Green;
                                if (translateToText)
                                {
                                    string textOutput = string.Join("", transitAngles.Values.SelectMany(angles => angles).Skip(1).Select(angle => AngleToChar(angle)));
                                    Console.WriteLine(textOutput);
                                }
                                foreach (var kvp in transitAngles)
                                {
                                    int year = kvp.Key;
                                    List<decimal> angles = kvp.Value;
                                    string anglesStr = string.Join(", ", angles.Select(angle => CleanFloats(angle)));
                                    Console.WriteLine($"¦ Year: {year}");
                                    foreach (string line in WrapByComma(anglesStr, 200))
                                    {
                                        Console.WriteLine(line);
                                    }
                                    colorify.WriteLine("+------------------------+", Colors.txtInfo);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                            }

                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("¦ Error: Invalid input");
                    }
                }
            }
            else
            {
                Console.Write("Target: ");
                string? targetAngles = ReadInput();
                if (string.IsNullOrEmpty(targetAngles))
                {
                    isInputSeed = !isInputSeed;
                    continue;
                }
                else
                {
                    try
                    {
                        if (targetAngles.StartsWith("-"))
                        {
                            string[] parts = targetAngles.Split(' ');
                            string command = parts[0];

                            if (command == "-d" && parts.Length == 2)
                            {
                                if (int.TryParse(parts[1], out int limit))
                                {
                                    decimalLimit = limit;
                                    Console.WriteLine($"¦ Decimal limit set to {decimalLimit}");
                                }
                                else
                                {
                                    Console.WriteLine("¦ Error: Invalid decimal limit.");
                                }
                            }
                            else if (command == "-r" && parts.Length == 2)
                            {
                                string[] rangeParts = parts[1].Split('-');
                                if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                                {
                                    yearRange[0] = start;
                                    yearRange[1] = end;
                                    Console.WriteLine($"¦ Year range set to {start}-{end}");
                                }
                                else
                                {
                                    Console.WriteLine("¦ Error: Invalid year range");
                                }
                            }
                            else if (command == "-t")
                            {
                                translateToText = !translateToText;

                                if (translateToText)
                                {
                                    colorify.WriteLine("Translation enabled", Colors.txtWarning);
                                }
                                else
                                {
                                    colorify.WriteLine("Translation disabled", Colors.txtWarning);
                                }

                            }
                            else
                            {
                                Console.WriteLine("¦ Error: Invalid command");
                            }
                        }
                        else
                        {
                            try
                            {
                                List<decimal> findSeed = targetAngles.Split(',').Select(angle => decimal.Parse(angle)).ToList();
                                if (findSeed.Count > 0)
                                {
                                    colorify.WriteLine("+--------------------------------------------", Colors.txtInfo);
                                    Console.WriteLine(string.Format("¦ Freq/seed match: {0:F" + decimalLimit + "}", MarkCalendar(findSeed)));
                                    colorify.WriteLine("+--------------------------------------------", Colors.txtInfo);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("¦ Error: Invalid target angles.");
                            }
                        }
                    }
                    catch (FormatException)
                    { Console.WriteLine("Error"); }
                }
            }
        }


        static char AngleToChar(decimal angle)
        {
            int charRange = 360 / 26;
            int index = (int)(angle / charRange);
            return (char)(65 + (index % 26));
        }

        static string CleanFloats(decimal f)
        {
            string s = $"{f:F15}";
            s = s.TrimEnd('0').TrimEnd('.');
            return s.Contains('.') ? s : s;
        }

        static List<string> WrapByComma(string text, int maxLength)
        {
            List<string> lines = new List<string>();
            List<string> line = new List<string>();
            int currentLength = 0;

            foreach (string item in text.Split(", "))
            {
                if (currentLength + item.Length + 2 > maxLength)
                {
                    lines.Add(string.Join(", ", line));
                    line.Clear();
                    currentLength = 0;
                }
                line.Add(item);
                currentLength += item.Length + 2;
            }

            if (line.Any())
            {
                lines.Add(string.Join(", ", line));
            }

            return lines;
        }
        static string? ReadInput()

        {
            return Task.Run(() => Console.ReadLine()).Result;
        }
    }
}

