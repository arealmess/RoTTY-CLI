//using System;
//using System.Text;

//class Program
//{
//    static void Main(string[] args)
//    {
//        try
//        {
//            Console.ForegroundColor = ConsoleColor.Green;
//            string prompt = "Seed:";
//            string mapping = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";

//            while (true)
//            {
//                Console.Write(prompt);
//                string? input = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(input))
//                {
//                    // Toggle between "Target:" and "Seed:" prompts.
//                    prompt = (prompt == "Seed:") ? "Target:" : "Seed:";
//                    continue;
//                }

//                if (input.StartsWith("-m"))
//                {
//                    string[] commandParts = input.Split(' ');
//                    if (commandParts.Length == 2)
//                    {
//                        string customMapping = commandParts[1];
//                        mapping = customMapping; // Update the mapping with the new value.
//                    }
//                    else
//                    {
//                        Console.WriteLine("Invalid command format. Use -m [newMapping] to set the custom mapping.");
//                    }
//                }
//                else
//                {
//                    decimal seed;
//                    if (decimal.TryParse(input, out seed))
//                    {
//                        string customMapping = mapping; // Use the updated mapping.
//                        int wrapRange = customMapping.Length;

//                        var transitionStringBuilder = new StringBuilder();
//                        int interval = (int)(1 / seed);

//                        for (int i = 0; i < wrapRange; i++)
//                        {
//                            int transition = i * interval % wrapRange;
//                            char letter = customMapping[transition];
//                            transitionStringBuilder.Append(letter);
//                        }

//                        Console.WriteLine("Logged transitions: " + transitionStringBuilder);
//                    }
//                    else
//                    {
//                        Console.WriteLine("Invalid input. Please enter a valid seed.");
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("An error occurred: " + ex.Message);
//            Console.ReadLine(); // Wait for user input to close the window
//        }
//    }
//}