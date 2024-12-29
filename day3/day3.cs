/* Ethan's Advent of Code day 3 project

today's challenge: Regex

https://adventofcode.com/2024/day/3
*/


// base library
using System;
// for use with files and directories
using System.IO;
// for use with regex
using System.Text.RegularExpressions;

// the class we're working with for this project
class day3 {

    // the thing we actually have to do
    static int mul(int x, int y) {
        return x * y;
    }

    // this is the function that'll do the work I need it to do
    static int domath(List<(int, int)> operations) {
        int retval = 0;
        foreach((int, int) elem in operations) {
            // Console.WriteLine($"elem 0 {elem.Item1} and elem 1 {elem.Item2}");
            retval += mul(elem.Item1, elem.Item2);
        }
        return retval;
    }
    
    // make a file reader, takes in a text file of arguments to parse
    // This file reader returns a tuple (int, int) list
    static List<(int, int)> fileReader(string f) {
        // f is our filename, and here's how we can check if we can get it
        string fil = Path.Combine(Directory.GetCurrentDirectory(), f);
        
        // a list to actually work with in a helper function
        List<(int, int)> regexmatch = new List<(int, int)>();
        // my regex pattern
        string pattern = @"mul\((\d+),(\d+)\)";

        // To read a text file line by line 
        if (File.Exists(fil)) {
            // Console.WriteLine("Found the file");
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(fil);
            // Console.WriteLine($"size of lines is {lines.Length}");

            foreach(string ln in lines) {
                // Console.WriteLine($"ln is {ln}");
                // generate matches foreach line
                MatchCollection matches = Regex.Matches(ln, pattern);
                foreach(Match m in matches) {
                    // append elements to the return list
                    /*
                    Console.WriteLine($"matched content is {m.Value}");
                    Console.WriteLine($"first number is {m.Groups[1].Value}");
                    Console.WriteLine($"second number is {m.Groups[2].Value}");
                    */;
                    // this is the first (\d+) in the pattern -- parens denote a grouping
                    int a = Int32.Parse(m.Groups[1].Value);
                    // this is the second (\d+) 
                    int b = Int32.Parse(m.Groups[2].Value);
                    // Console.WriteLine($"biggo tuple {(a, b)}");
                    regexmatch.Add((a, b));
                }
            }
        } else {
            Console.WriteLine("The filename " + fil + " doesn't exist");
            Console.WriteLine("Please recheck spelling and file location");
        }

        return regexmatch;
    }


    // VERY IMPORTANT NOTE: MAIN MUST BE CAPITALIZED LIKE THIS
    static void Main(string[] args) {

        int arglen = args.Length;

        if (arglen == 0) {
            Console.WriteLine("Please enter a file name to open");
            Console.WriteLine("Files should be located in the same directory as the executable");
            Environment.Exit(1);
        } else if (arglen > 1) {
            Console.WriteLine("More than one arg entered, ignoring additional arguments");
        }

        /*
        goto day1.cs to see why I use var instead of type List<string>
        */
        var ops = fileReader(args[0]);
        
        // when there's nothing in the list
        if(!ops.Any()) {
            Console.WriteLine("The parsed list has issues with it, exit failure");
            Console.WriteLine("The list is empty");
            Environment.Exit(1);
        }

        int answer = domath(ops);
        Console.WriteLine("Our answer is " + answer);
        
        Console.WriteLine("Hello World!"); 
        Environment.Exit(0);
    }
}
