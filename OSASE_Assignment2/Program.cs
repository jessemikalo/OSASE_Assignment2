using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var shell = new Shell();
        shell.Run();
    }
}

class Shell
{
    private Dictionary<string, string> Aliases = new Dictionary<string, string>
    {
    };

    public void Run()
    {
        string input = null;

        do
        {
            Console.Write("$ ");
            input = Console.ReadLine();
            Execute(input);
        } while (input != "exit");
    }

    public int Execute(string input)
    {
        if (Aliases.Keys.Contains(input))
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(Aliases[input])
            {
                UseShellExecute = false
            };

            process.Start();
            process.WaitForExit();

            return 0;
        }

        if (input == "pwd")
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            return 0;
        }

        if (input.StartsWith("cd "))
        {
            string path = input.Substring(3);
            try
            {
                Directory.SetCurrentDirectory(path);
                Console.Write(Directory.GetCurrentDirectory());
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"cd: Cannot find the path '{path}' because it doesn't exist");
                return 1;
            }
            return 0;
        }

        if (input.StartsWith("wc "))
        {
            string[] inputParts = input.Split(' ');
            string filename = inputParts[1];

            try
            {
                string[] lines = File.ReadAllLines(filename);
                int wordCount = 0;
                foreach (string line in lines)
                {
                    wordCount += line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                }
                Console.WriteLine(wordCount);
                return 0;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"wc: Cannot find the file '{filename}'");
                return 1;
            }
        }

        if (input == "ls")
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
            return 0;
        }

        if (input == "clear")
        {
            Console.Clear();
            return 0;
        }

        Console.WriteLine($"{input}: not found");
        return 1;
    }
}
