/* Ethan's Advent of Code day 1 project

Today's challenge

https://adventofcode.com/2024/day/1
*/

/*
useful commands for terminal
dotnet build    # compiles the project
dotnet run  # runs the project
dotnet clean    # like clean in makefiles
*/

// base library
using System;
// for use with files and directories
using System.IO;

// the class we're working with for this project
class day1 {
    static int listSum(List<int> l, List<int> r) {
        int a, b;   // declare our two ints for math
        int retVal = 0;
        int temp = 0;

        for(int i = 0; i < l.Count; i++) {
            a = l[i];   // the first thing to subt
            b = r[i]; // the second thing to subt
            // Console.WriteLine($"a is {a} and b is {b}");
            if (a < b) {
                temp = b - a;
            } else {
                temp = a - b;
            }

            // Console.WriteLine($"temp is {temp}");
            retVal += temp;
        }

        return retVal;
    }

    // make a file reader, takes in a text file of arguments to parse
    // this returns two lists to parse from: the left list and the right list
    static (List<int>, List<int>) fileReader(string f) {
        // f is our filename, and here's how we can check if we can get it

        string fil = Path.Combine(Directory.GetCurrentDirectory(), f);
        char[] delimiter = {' ', ','};  // declare a delimiter
        List<int> returnA = new List<int>();
        List<int> returnB = new List<int>();

        // Console.WriteLine("We're searching for file " + fil);

        // To read a text file line by line 
        if (File.Exists(fil)) { 
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(fil);

            
            // go through each line and parse
            foreach(string ln in lines) {
                // Console.WriteLine(ln);
                // split the list by entries
                string[] splitter = ln.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                // Console.WriteLine($"left is {splitter[0]} and right is {splitter[1]}");

                // parse text line from string to int
                // erroneous states
                int toA = -1;
                int toB = -1;
                try {
                    toA = Int32.Parse(splitter[0]);
                    
                } catch (FormatException) {
                    Console.WriteLine("toA has bad format");
                }
                try {
                    toB = Int32.Parse(splitter[1]);
                } catch (FormatException) {
                    Console.WriteLine($"toB has bad format");
                }
                returnA.Add(toA);
                returnB.Add(toB);

            }
            // Console.WriteLine();
            // Console.WriteLine("Read the file line by line");
        } else {
            Console.WriteLine("The filename " + fil + " doesn't exist");
            Console.WriteLine("Please recheck spelling and file location");
        }

        // using a builtin sorting method
        returnA.Sort();
        returnB.Sort();
        // Console.WriteLine("size of returnA is " + returnA.Count);
        // Console.WriteLine("Size of returnB is " + returnB.Count);

        return (returnA, returnB);
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
        even though I know what searchLeft and searchRight are supposed to be of type List<int>,
        sometimes it's helpful to return tuples as type var

        the compiler will infer the types of all my holder vars so I can do work with them
        more easily
        */
        var (searchLeft, searchRight) = fileReader(args[0]);

        // when there's anything in the list and lists are equal in size
        if(!searchLeft.Any() || !searchRight.Any() && (searchLeft.Count == searchRight.Count)) {
            Console.WriteLine("A parsed list has issues with it, exit failure");
            Console.WriteLine("either a list is empty or the lists aren't of equal size");
            Environment.Exit(1);
        }

        int answer = listSum(searchLeft, searchRight);
        Console.WriteLine("Our answer is " + answer);
        
        // Console.WriteLine("Hello World!"); 
        Environment.Exit(0);
    }
}
