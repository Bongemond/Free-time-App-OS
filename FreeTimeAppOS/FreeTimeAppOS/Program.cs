using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace FreeTimeAppOS
{
    internal class Program
    {
        //
        //RANDOM
        //
        Random random = new Random();
        StringBuilder sb = new StringBuilder();

        //
        //VALUES
        //
        int log = 1;
        bool logged = false;

        //
        //TABLES
        //
        List<List<string>> Accounts = new List<List<string>>();
        List<string> Logs = new List<string>();
        static string[] Colors = new string[] { "Red", "Orange", "Yellow", "Green", "Blue", "Pink", "Purple", "Brown", "Gray1", "Gray2", "White" };
        static Dictionary<string, string> SystemColors = new Dictionary<string, string>()
            {
                {"Red", "38;2;255;60;60" },
                {"Orange", "38;2;255;140;0" },
                {"Yellow", "38;2;255;220;50" },
                {"Green", "38;2;0;200;70" },
                {"Blue", "38;2;50;130;255" },
                {"Pink", "38;2;255;120;180" },
                {"Purple", "38;2;150;80;255" },
                {"Brown", "38;2;160;80;0" },
                {"Gray1", "38;2;100;100;100" },
                {"Gray2", "38;2;180;180;180" },
                {"White", "38;2;255;255;255" },
            };

        //
        //MINESWEEPERSETTINGS
        //
        static int ColorCount = Colors.Length-1;
        static string[] GMode = ["Controled", "Uncontroled"];
        static string[] CMode = ["Off", "On"];
        static string[] MSKeys = ["GenMode", "CustomColors", "FlagColor", "SelectorColor", "UnRevColor", "ExplodedColor", "DefusedColor", "Color0", "Color1", "Color2", "Color3", "Color4", "Color5", "Color6", "Color7", "Color8"];
        static Dictionary<string, int[]> MineSweeperSettings = new Dictionary<string, int[]>()
            {
                {"GenMode", [0,0,1] },
                {"CustomColors", [0,0,1] },
                {"FlagColor", [0,10, ColorCount] },
                {"SelectorColor", [0,10, ColorCount] },
                {"UnRevColor", [0,10, ColorCount] },
                {"ExplodedColor", [0,10, ColorCount] },
                {"DefusedColor", [0,10, ColorCount] },
                {"Color0", [0,10, ColorCount] },
                {"Color1", [0,10, ColorCount] },
                {"Color2", [0,10, ColorCount] },
                {"Color3", [0,10, ColorCount] },
                {"Color4", [0,10, ColorCount] },
                {"Color5", [0,10, ColorCount] },
                {"Color6", [0,10, ColorCount] },
                {"Color7", [0,10, ColorCount] },
                {"Color8", [0,10, ColorCount] }
            };

        //
        //MAIN
        //
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Program program = new Program();
            program.Log("SYSTEM - Start. Turning on...");
            //program.Base();
            Task.Run(() => program.Base());
            while (true)
            {
                program.Sleep(60);
            }
        }

        //
        //BASE
        //
        private void Base()
        {
            while (true)
            {
                if (!logged)
                {
                    LogIn();
                }
                else
                {
                    Console.WriteLine("Inputs: 'Turn off'; 'Log out'; 'Calculator'; 'Minesweeper'; 'Logs'\n");
                    Console.Write("Input: ");
                    string input = Convert.ToString(Console.ReadLine()).Replace(" ", "").ToLower();
                    if (input == "end" || input == "turnoff")
                    {
                        Console.Clear();
                        Log("SYSTEM - End. Turning off...");
                        Space(1);
                        Environment.Exit(0);
                    }
                    else if (input == "logout")
                    {
                        logged = false;
                        Console.Clear();
                        LogIn();
                    }
                    else if (input == "calculator")
                    {
                        Console.Clear();
                        Calculator();
                    }
                    else if (input == "minesweeper")
                    {
                        Console.Clear();
                        Minesweeper();
                    }
                    else if (input == "logs" || input == "log")
                    {
                        Console.Clear();
                        Console.WriteLine("[ESC] - Exit\n");
                        for (int i = 0; i < Logs.Count; i++)
                        {
                            Console.WriteLine(Logs[i]);
                        }
                        while (true)
                        {
                            ConsoleKeyInfo key = Console.ReadKey();
                            if (key.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                        }
                    }
                    Console.Clear();
                }
            }
        }

        //
        //LOGIN
        //
        private void GetIn(string type, string username)
        {
            while (!logged)
            {
                if (type == "delete")
                {
                    Console.Write("Delete account: ");
                    string ACinput = Convert.ToString(Console.ReadLine());
                    if (ACinput == "exit" || ACinput == "Exit" || ACinput == "Back" || ACinput == "back")
                    {
                        Console.Clear();
                        return;
                    }
                    else if (Accounts.Any(x => x.Contains(ACinput)))
                    {
                        Console.Write("Confirm? Yes / No: ");
                        string answer = Convert.ToString(Console.ReadLine());
                        if (answer == "Yes" || answer == "yes")
                        {
                            Accounts.RemoveAll(x => x[0] == ACinput);
                            Console.WriteLine("Account " + ACinput + " deleted.\n");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account not found.\n");
                    }
                }
                else if (type == "login")
                {
                    Console.WriteLine("Logging in to account");
                    Console.Write("Enter password: ");
                    string input2 = Convert.ToString(Console.ReadLine());
                    if (input2 == "Exit" || input2 == "exit" || input2 == "Back" || input2 == "back")
                    {
                        Console.Clear();
                        return;
                    }
                    else if (Accounts.Any(x => x[1].Contains(input2)))
                    {
                        logged = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong password.");
                        Space(1);
                    }
                }
                else if (type == "create")
                {
                    Console.WriteLine("Creating account");
                    Console.Write("Create password: ");
                    string passwordCreate = Convert.ToString(Console.ReadLine());
                    if (passwordCreate == "Exit" || passwordCreate == "exit" || passwordCreate == "Back" || passwordCreate == "back")
                    {
                        Console.Clear();
                        return;
                    }
                    Console.Write("Confirm? Yes / No: ");
                    string answer = Convert.ToString(Console.ReadLine());
                    if (answer == "Yes" || answer == "yes")
                    {
                        Accounts.Add(new List<string> { username, passwordCreate });
                        foreach (var x in Accounts)
                        {
                            if (x[0] == username && x[1] == passwordCreate)
                            {
                                logged = true;
                                Space(1);
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        return;
                    }
                }
            }
        }
        private void LogIn()
        {
            while (true)
            {
                Console.WriteLine("Other inputs: 'Delete'\n");
                Console.Write("Username: ");
                string input = Convert.ToString(Console.ReadLine()).Replace(" ", "").ToLower();
                if (input == "end" || input == "End")
                {
                    Console.Clear();
                    Log("End. Turning off...");
                    Space(1);
                    Environment.Exit(0);
                }
                else if (input == "exit" || input == "back")
                {
                    Console.Clear();
                    return;
                }
                else if (input == "accounts")
                {
                    Console.Clear();
                    Console.WriteLine("[ENTER] - Exit\n");
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        if (i > 0)
                        {
                            Console.Write("; ");
                        }
                        Console.Write(Accounts[i][0]);
                    }
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                    }
                }
                else if (input == "delete" || input == "remove")
                {
                    Space(1);
                    GetIn("delete", input);
                }
                else if (input == "help" || input == "functions" || input == "actions" || input == "commands")
                {
                    Console.Clear();
                    Console.WriteLine("[ENTER] - Exit");
                    Console.WriteLine("'Accounts'");
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                    }
                }
                else if (!Accounts.Any(x => x.Contains(input)))
                {
                    Space(1);
                    GetIn("create", input);
                }
                else
                {
                    Space(1);
                    GetIn("login", input);
                }
                Console.Clear();
                if (logged)
                {
                    return;
                }
            }
        }

        //
        //LOG
        //
        private void Log(string LogText)
        {
            string text = "Log." + Convert.ToString(log) + ": " + LogText;
            Console.WriteLine(text);
            Space(1);
            Logs.Add(text);
            log++;
        }

        //
        //SPACING
        //
        private void Space(int SpaceAmount)
        {
            for (int i = 0; i < SpaceAmount; i++)
            {
                Console.WriteLine();
            }
        }

        //
        //SLEEP
        //
        private void Sleep(int Time)
        {
            Thread.Sleep(Time*1000);
        }

        //
        //CALCULATOR
        //

        private void Calculator()
        {
            List<string> PMMD = new List<string>();
            List<string> Numbers = new List<string>();
            Console.WriteLine("Other inputs: 'Exit'; 'Clear'; 'Result'\n");
            while (true)
            {
                Console.Write("Calculator: ");
                string input = Regex.Replace(Convert.ToString(Console.ReadLine()).Replace(" ", "").Replace(",", "."), @"(?<!\d)\.", "0.");
                if (input == "exit" || input == "Exit" || input == "Back" || input == "back")
                {
                    Console.Clear();
                    return;
                }
                else if (input == "Vipe" || input == "vipe" || input == "Reset" || input == "reset" || input == "Clear" || input == "clear")
                {
                    PMMD.Clear();
                    Numbers.Clear();
                    Console.Clear();
                    Console.WriteLine("Other inputs: 'Exit'; 'Clear'; 'Result'\n");
                }
                else if (input == "Equals" || input == "equals" || input == "Result" || input == "result" || input == "=")
                {
                    if (Numbers.Count > PMMD.Count)
                    {
                        string FinalExperssion = Numbers[0];
                        for (int i = 1; i < Numbers.Count; i++)
                        {
                            FinalExperssion = FinalExperssion + PMMD[i - 1] + Numbers[i];
                        }
                        var table = new DataTable();
                        object result = table.Compute(FinalExperssion, "");
                        Console.WriteLine($"Result of '{FinalExperssion}' is: {result}\n");
                        PMMD.Clear();
                        Numbers.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Please input another number first.\n");
                        PMMD.Clear();
                        Numbers.Clear();
                    }
                }
                else if (input.All(x => char.IsDigit(x) || x == '/' || x == '(' || x == ')' || x == '*' || x == '+' || x == '-' || x == '.') && (input.Contains("/") || input.Contains("*") || input.Contains("+") || input.Contains("-")) && input.Length > 1)
                {
                    if (!Regex.IsMatch(input, @"[\/\*\-\+\(\)]{2,}") && !Regex.IsMatch(input, @"^[/*+]") && !Regex.IsMatch(input, @"[/*+\-]$") && input.Count(i => i == '(') == input.Count(i => i == ')'))
                    {
                        var table = new DataTable();
                        object result = table.Compute(input, "");
                        Console.WriteLine($"Result is: {result}\n");
                        PMMD.Clear();
                        Numbers.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Wrong input.\n");
                    }
                }
                else if (input == "/" || input == "*" || input == "+" || input == "-")
                {
                    if (Numbers.Count == 0 || Numbers.Count > PMMD.Count)
                    {
                        PMMD.Add(input);
                        if (Numbers.Count == 0)
                        {
                            Numbers.Add("0");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please input number.");
                    }
                }
                else if (input.All(c => char.IsDigit(c) || c == '.'))
                {
                    if (Numbers.Count == PMMD.Count)
                    {
                        Numbers.Add(input);
                    }
                    else
                    {
                        Console.WriteLine("Please input operator.");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input.");
                }
            }
        }

        //
        // MINESWEEPER
        //
        private void Minesweeper()
        {
            Dictionary<(int row, int col), int> grid = new Dictionary<(int, int), int>();
            while (true)
            {
                int[] last = [1, 1];
                int left = 0;
                bool won = false;
                bool lost = false;
                string SSy = "";
                string SSx = "";
                string minesinput = "";
                string GetInput(ref string value, string choose)
                {
                    while (true)
                    {
                        Console.WriteLine("Other inputs: 'Exit'; 'Settings'; 'Clear'\n");
                        if (choose == "x" || choose == "y")
                        {
                            Console.Write($"Choose size {choose}: ");
                        }
                        else
                        {
                            Console.Write("Choose amount of mines: ");
                        }
                        value = Convert.ToString(Console.ReadLine()).Replace(" ", "").ToLower();
                        if (value == "exit" || value == "back")
                        {
                            Console.Clear();
                            return "exit";
                        }
                        else if (value == "settings" || value == "setting")
                        {
                            Console.Clear();
                            MineSweeperSetting();
                            return "false";
                        }
                        else if (value == "clear")
                        {
                            return "false";
                        }
                        else if (int.TryParse(value, out int Nil))
                        {
                            return "true";
                        }
                        Console.Clear();
                    }
                }
                bool MustReturn = false;
                bool CanPass = false;
                while (CanPass == false)
                {
                    string x = GetInput(ref SSy, "x");
                    if (x == "exit")
                    {
                        return;
                    }
                    else if (x == "true")
                    {
                        CanPass = true;
                    }
                    if (CanPass == true)
                    {
                        Console.Clear();
                        string y = GetInput(ref SSx, "y");
                        if (y == "exit")
                        {
                            return;
                        }
                        else if (y == "true")
                        {
                            CanPass = true;
                        }
                        else
                        {
                            CanPass = false;
                        }
                    }
                    if (CanPass == true)
                    {
                        Console.Clear();
                        string z = GetInput(ref minesinput, "z");
                        if (z == "exit")
                        {
                            return;
                        }
                        else if (z == "true")
                        {
                            CanPass = true;
                        }
                        else
                        {
                            CanPass = false;
                        }
                    }
                    Console.Clear();
                }
                if (MustReturn == true)
                {
                    return;
                }
                int Sy = Convert.ToInt16(SSy);
                int Sx = Convert.ToInt16(SSx);
                int mines = Convert.ToInt16(minesinput);
                if (mines >= 0 && Sx * Sy > mines)
                {
                    for (int x = 0; x <= Sx+1; x++)
                    {
                        for (int y = 0; y <= Sy+1; y++)
                        {
                            grid[(x, y)] = 0;
                        }
                    }
                    left = mines;
                    int tries = 0;
                    for (int i = 0; i < mines;)
                    {
                        int x = random.Next(1, Sx+1);
                        int y = random.Next(1, Sy+1);
                        if (grid[(x, y)] != 9)
                        {
                            if (MineSweeperSettings["GenMode"][1] == 0)
                            {
                                int count = 0;
                                for (int a = -1; a <= 1; a++)
                                {
                                    for (int b = -1; b <= 1; b++)
                                    {
                                        if (grid[(x - a, y - b)] == 9)
                                        {
                                            grid[(x, y)] = count;
                                        }
                                    }
                                }
                                if (count < 5)
                                {
                                    grid[(x, y)] = 9;
                                    i++;
                                }
                            }
                            else
                            {
                                grid[(x, y)] = 9;
                                i++;
                            }
                        }
                        tries++;
                        if (tries >= Sx * Sy * 10)
                        {
                            Log("MINESWEEPER - Generation have failed. Program will now exit.");
                            Sleep(3);
                            return;
                        }
                    }
                    for (int x = 1; x <= Sx; x++)
                    {
                        for (int y = 1; y <= Sy; y++)
                        {
                            if (grid[(x, y)] != 9)
                            {
                                int count = 0;
                                for (int a = -1; a <= 1; a++)
                                {
                                    for (int b = -1; b <= 1; b++)
                                    {
                                        if (grid[(x - a, y - b)] == 9)
                                        {
                                            count++;
                                        }
                                    }
                                }
                                grid[(x, y)] = count;
                            }
                        }
                    }
                    while (true)
                    {
                        if (MineSweeperSettings["CustomColors"][1] == 0)
                        {
                            for (int x = 1; x <= Sx; x++)
                            {
                                for (int y = 1; y <= Sy; y++)
                                {
                                    if ((x, y) == (last[0], last[1]))
                                    {
                                        sb.Append("[");
                                    }
                                    else if ((x, y - 1) == (last[0], last[1]))
                                    {
                                        sb.Append("]");
                                    }
                                    else
                                    {
                                        sb.Append(" ");
                                    }
                                    if (grid[(x, y)] >= 0 && grid[(x, y)] <= 9)
                                    {
                                        sb.Append("■");
                                    }
                                    else if (grid[(x, y)] >= 10 && grid[(x, y)] <= 18)
                                    {
                                        sb.Append(grid[(x, y)] - 10);
                                    }
                                    else if (grid[(x, y)] == 19)
                                    {
                                        sb.Append("✹");
                                    }
                                    else if (grid[(x, y)] >= 20 && grid[(x, y)] <= 29)
                                    {
                                        sb.Append("⚑");
                                    }
                                    else if (grid[(x, y)] == 30)
                                    {
                                        sb.Append("●");
                                    }
                                }
                                if (x == last[0] && last[1] == Sy)
                                {
                                    sb.Append("]");
                                }
                                sb.AppendLine();
                            }
                        }
                        else
                        {
                            for (int x = 1; x <= Sx; x++)
                            {
                                for (int y = 1; y <= Sy; y++)
                                {
                                    if ((x, y) == (last[0], last[1]))
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["SelectorColor"][1]]]}m[");
                                    }
                                    else if ((x, y - 1) == (last[0], last[1]))
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["SelectorColor"][1]]]}m]");
                                    }
                                    else
                                    {
                                        sb.Append(" ");
                                    }
                                    if (grid[(x, y)] >= 0 && grid[(x, y)] <= 9)
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["UnRevColor"][1]]]}m■");
                                    }
                                    else if (grid[(x, y)] >= 10 && grid[(x, y)] <= 18)
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["Color"+Convert.ToString(grid[(x, y)]-10)][1]]]}m{grid[(x, y)]-10}");
                                    }
                                    else if (grid[(x, y)] == 19)
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["ExplodedColor"][1]]]}m✹");
                                    }
                                    else if (grid[(x, y)] >= 20 && grid[(x, y)] <= 29)
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["FlagColor"][1]]]}m⚑");
                                    }
                                    else if (grid[(x, y)] == 30)
                                    {
                                        sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["DefusedColor"][1]]]}m●");
                                    }
                                }
                                if (x == last[0] && last[1] == Sy)
                                {
                                    sb.Append($"\u001b[{SystemColors[Colors[MineSweeperSettings["SelectorColor"][1]]]}m]");
                                }
                                sb.AppendLine();
                            }
                            sb.Append("\u001b[0m");
                        }
                        if (won == true)
                        {
                            sb.AppendLine();
                            sb.Append("You have won!");
                        }
                        else if (lost == true)
                        {
                            sb.AppendLine();
                            sb.Append("You have lost");
                        }
                        Console.Clear();
                        Console.WriteLine("[ESC] - Exit; [ARROWS] - Selection; [SPACEBAR] - Mine; [ENTER] - Flag\n");
                        Console.WriteLine($"Mines left: {left}\n");
                        Console.Write(sb.ToString());
                        sb.Clear();
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.LeftArrow)
                        {
                            if (last[1] > 1)
                            {
                                last[1] -= 1;
                            }
                        }
                        else if (key.Key == ConsoleKey.RightArrow)
                        {
                            if (last[1] < Sy)
                            {
                                last[1] += 1;
                            }
                        }
                        else if (key.Key == ConsoleKey.UpArrow)
                        {
                            if (last[0] > 1)
                            {
                                last[0] -= 1;
                            }
                        }
                        else if (key.Key == ConsoleKey.DownArrow)
                        {
                            if (last[0] < Sx)
                            {
                                last[0] += 1;
                            }
                        }
                        else if (key.Key == ConsoleKey.Spacebar)
                        {
                            if (grid[(last[0], last[1])] < 10)
                            {
                                if (grid[(last[0], last[1])] == 0)
                                {
                                    grid[(last[0], last[1])] = grid[(last[0], last[1])] + 10;
                                    bool changed = true;
                                    while (changed == true)
                                    {
                                        changed = false;
                                        for (int x = 1; x <= Sx; x++)
                                        {
                                            for (int y = 1; y <= Sy; y++)
                                            {
                                                if (grid[(x, y)] < 9 && (grid[(x + 1, y)] == 10 || grid[(x - 1, y)] == 10 || grid[(x, y + 1)] == 10 || grid[(x, y - 1)] == 10 || grid[(x + 1, y + 1)] == 10 || grid[(x - 1, y + 1)] == 10 || grid[(x + 1, y - 1)] == 10 || grid[(x - 1, y - 1)] == 10))
                                                {
                                                    grid[(x, y)] += 10;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (grid[(last[0], last[1])] < 9)
                                {
                                    grid[(last[0], last[1])] = grid[(last[0], last[1])] + 10;
                                }
                                else if (grid[(last[0], last[1])] == 9)
                                {
                                    lost = true;
                                    left = 0;
                                    for (int x = 1; x <= Sx; x++)
                                    {
                                        for (int y = 1; y <= Sy; y++)
                                        {
                                            if (grid[(x, y)] < 10)
                                            {
                                                grid[(x, y)] += 10;
                                            }
                                            else if (grid[(x, y)] >= 20)
                                            {
                                                grid[(x, y)] -= 10;
                                            }
                                        }
                                    }
                                }
                                int count = 0;
                                for (int x = 1; x <= Sx; x++)
                                {
                                    for (int y = 1; y <= Sy; y++)
                                    {
                                        if (grid[(x, y)] > 9 && grid[(x, y)] < 20)
                                        {
                                            count++;
                                        }
                                    }
                                }
                                if (Sx*Sy-count == mines)
                                {
                                    won = true;
                                    left = 0;
                                    for (int x = 1; x <= Sx; x++)
                                    {
                                        for (int y = 1; y <= Sy; y++)
                                        {
                                            if (grid[(x, y)] == 9 || grid[(x, y)] == 29)
                                            {
                                                grid[(x, y)] = 30;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            if (grid[(last[0], last[1])] < 10)
                            {
                                grid[(last[0], last[1])] += 20;
                                left -= 1;
                            }
                            else if (grid[(last[0], last[1])] >= 20 && grid[(last[0], last[1])] <= 29)
                            {
                                grid[(last[0], last[1])] -= 20;
                                left++;
                            }
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            won = false;
                            lost = false;
                            Console.Clear();
                            sb.Clear();
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount of mines");
                }
            }
        }
        private void MineSweeperSetting()
        {
            string LastS = "GenMode";
            int LastI = 0;
            while (true)
            {
                Console.WriteLine("[ESC] - Exit; [SPACE] - Cycle; [ARROWS] - Up and down, next and last\n");
                Console.WriteLine("SETTINGS\n");
                Console.WriteLine("SYSTEM");
                if (LastS == "GenMode") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Generation mode: {GMode[MineSweeperSettings["GenMode"][1]]}");
                if (LastS == "CustomColors") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Custom colors: {CMode[MineSweeperSettings["CustomColors"][1]]}\n");
                Console.WriteLine("COLORS");
                if (LastS == "FlagColor") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Flag: \u001b[{SystemColors[Colors[MineSweeperSettings["FlagColor"][1]]]}m{Colors[MineSweeperSettings["FlagColor"][1]]}\u001b[0m");
                if (LastS == "SelectorColor") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Selector: \u001b[{SystemColors[Colors[MineSweeperSettings["SelectorColor"][1]]]}m{Colors[MineSweeperSettings["SelectorColor"][1]]}\u001b[0m");
                if (LastS == "UnRevColor") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"UnRevealed: \u001b[{SystemColors[Colors[MineSweeperSettings["UnRevColor"][1]]]}m{Colors[MineSweeperSettings["UnRevColor"][1]]}\u001b[0m");
                if (LastS == "ExplodedColor") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Exploded: \u001b[{SystemColors[Colors[MineSweeperSettings["ExplodedColor"][1]]]}m{Colors[MineSweeperSettings["ExplodedColor"][1]]}\u001b[0m");
                if (LastS == "DefusedColor") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Defused: \u001b[{SystemColors[Colors[MineSweeperSettings["DefusedColor"][1]]]}m{Colors[MineSweeperSettings["DefusedColor"][1]]}\u001b[0m");
                if (LastS == "Color0") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number0: \u001b[{SystemColors[Colors[MineSweeperSettings["Color0"][1]]]}m{Colors[MineSweeperSettings["Color0"][1]]}\u001b[0m");
                if (LastS == "Color1") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number1: \u001b[{SystemColors[Colors[MineSweeperSettings["Color1"][1]]]}m{Colors[MineSweeperSettings["Color1"][1]]}\u001b[0m");
                if (LastS == "Color2") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number2: \u001b[{SystemColors[Colors[MineSweeperSettings["Color2"][1]]]}m{Colors[MineSweeperSettings["Color2"][1]]}\u001b[0m");
                if (LastS == "Color3") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number3: \u001b[{SystemColors[Colors[MineSweeperSettings["Color3"][1]]]}m{Colors[MineSweeperSettings["Color3"][1]]}\u001b[0m");
                if (LastS == "Color4") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number4: \u001b[{SystemColors[Colors[MineSweeperSettings["Color4"][1]]]}m{Colors[MineSweeperSettings["Color4"][1]]}\u001b[0m");
                if (LastS == "Color5") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number5: \u001b[{SystemColors[Colors[MineSweeperSettings["Color5"][1]]]}m{Colors[MineSweeperSettings["Color5"][1]]}\u001b[0m");
                if (LastS == "Color6") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number6: \u001b[{SystemColors[Colors[MineSweeperSettings["Color6"][1]]]}m{Colors[MineSweeperSettings["Color6"][1]]}\u001b[0m");
                if (LastS == "Color7") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number7: \u001b[{SystemColors[Colors[MineSweeperSettings["Color7"][1]]]}m{Colors[MineSweeperSettings["Color7"][1]]}\u001b[0m");
                if (LastS == "Color8") { Console.Write("● "); } else { Console.Write("  "); }
                Console.WriteLine($"Number8: \u001b[{SystemColors[Colors[MineSweeperSettings["Color8"][1]]]}m{Colors[MineSweeperSettings["Color8"][1]]}\u001b[0m");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (MineSweeperSettings[LastS][1] > MineSweeperSettings[LastS][0])
                    {
                        MineSweeperSettings[LastS][1] -= 1;
                    }
                    else
                    {
                        MineSweeperSettings[LastS][1] = MineSweeperSettings[LastS][2];
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.Spacebar)
                {
                    if (MineSweeperSettings[LastS][1] < MineSweeperSettings[LastS][2])
                    {
                        MineSweeperSettings[LastS][1] += 1;
                    }
                    else
                    {
                        MineSweeperSettings[LastS][1] = MineSweeperSettings[LastS][0];
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if (LastI > 0)
                    {
                        LastI -= 1;
                    }
                    else
                    {
                        LastI = MSKeys.Count()-1;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (LastI < MSKeys.Count()-1)
                    {
                        LastI++;
                    }
                    else
                    {
                        LastI = 0;
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    break;
                }
                LastS = MSKeys[LastI];
                sb.Clear();
                Console.Clear();
            }
        }
    }
}