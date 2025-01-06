/* Ethan's Advent of Code day 2 project

today's challenge

https://adventofcode.com/2024/day/2
*/


// base library
using System;
// for use with files and directories
using System.IO;
// for use with specialized lists
using System.Linq;

// the class we're working with for this project
class day2 {

    public static bool incdec(int[] levels) {
        /*
        This is the helper function for checking constant
        incrementing /decrementing

        this was a pain to check
        */
        // checking this tells us which direction we're going
        int id = levels[0] - levels[1];
        // init directions as false
        bool incr = false; bool decr = false;

        switch (id) {
            case > 0:
                incr = true;
                // Console.WriteLine("we are incrementing");
                break;
            case < 0:
                decr = true;
                // Console.WriteLine("we are decrementing");
                break;
            default:
            // jsut return false
                // Console.WriteLine("there's no initial increment");
                return false;    
        }
    

        for (int j = 1; j < levels.Length - 1; j++) {
            // return false if any other element breaks the initial trend
            if(incr && (levels[j] - levels[j+1] <= 0)) {
                return false;
            }
            if (decr && (levels[j] - levels[j+1] >= 0)) {
                return false;
            }
        }

        return true;
    }

    public static bool differ(int[] levels) {
        // this was leagues easier than checking incrementing/decrementing
        int temp;   // temp val for checking
        for (int i = 0; i < levels.Length - 1; i++) {
            temp = Math.Abs(levels[i] - levels[i+1]);
            // if this abs val fails this check, return false
            if((temp < 1) || (temp > 3)) {
                return false;
            }
        }
        return true;
    }
    
    // this is where we'll complete the parsing of the list and return our answer
    // for each list in our list, parse and check each value against the rules
    static int reportChecker(List<string> l) {
        char[] delimiter = {' '};  // declare a delimiter
        int safe = 0;

        foreach(string ln in l) {
            int ctr = 0;
            // Console.WriteLine($"line is {ln}");
            
            string[] splitter = ln.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            // these are what the problem calls levels
            int[] levels = new int[splitter.Length];
            // Console.WriteLine($"length is {splitter.Length}");

            // store the parsed elements into a temporary array of ints
            foreach(string elem in splitter) {
                // Console.WriteLine($"elem is {elem}");
                try {
                    levels[ctr] = Int32.Parse(elem);
                    
                } catch (FormatException) {
                    Console.WriteLine($"{elem} has bad format");
                }
                // Console.WriteLine($"current elem is {levels[ctr]}");
                ctr++;
            }

            // do work here with helper functions
            bool id = incdec(levels);   // check if all incrementing or decrementing
            bool di = differ(levels);   // check if difference is being maintained

            if(id && di) {
                safe++;
            }
            
            // Console.WriteLine($"{ctr}");
            // Console.WriteLine();
        }

        return safe;
    }

    // make a file reader, takes in a text file of arguments to parse
    // The rationale for this reader is to return a list of strings that'll  further parsed
    // In a different helper function
    // I feel like this best suits the structure for this problem since this function's job
    // is just to return the input.txt into a more useable format
    // This file reader returns a list of strings
    static List<string> fileReader(string f) {
        // f is our filename, and here's how we can check if we can get it
        string fil = Path.Combine(Directory.GetCurrentDirectory(), f);
        
        List<string> reports = new List<string>();

        // To read a text file line by line 
        if (File.Exists(fil)) { 
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(fil);

            foreach(string ln in lines) {
                // the ith report in our input
                reports.Add(ln);
                // Console.WriteLine(ln);
            }
        } else {
            Console.WriteLine("The filename " + fil + " doesn't exist");
            Console.WriteLine("Please recheck spelling and file location");
        }

        /*
        foreach(string elem in reports) {
            Console.WriteLine($"element is {elem}");
        }
        Console.WriteLine($"Size of reports is " + reports.Count);
        */

        return reports;
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
        var reports = fileReader(args[0]);

        
        // when there's nothing in the list
        if(!reports.Any()) {
            Console.WriteLine("The parsed list has issues with it, exit failure");
            Console.WriteLine("The list is empty");
            Environment.Exit(1);
        }

        int answer = reportChecker(reports);
        Console.WriteLine("Our answer is " + answer);

        // make a new instance of part 2 child
        // I know this isn't the best way to organize this file
        // there should honestly be a class Program that houses main and the file reader
        // so that the parent + child makes more sense to read
        // but I am too lazy to implement better practice lol :/
        part2 mypt2 = new part2();
        int pt2ans = mypt2.reportChecker(reports);
        Console.WriteLine($"pt2 answer is {pt2ans}");
        
        
        Console.WriteLine("Hello World!"); 
        Environment.Exit(0);
    }
}

class part2 : day2 {

    /* tolerate is a special method for solving the day2 subproblem
    this method will only be relevant if both incdec & differ fail (this is the main problem)
    */
    static bool tolerate(int[] levels) {
        // Console.WriteLine("got in pt2 tolerate");
        var levellist = levels.ToList();

        for (int i = 0; i < levellist.Count; i++) {
            // create a temp copy of the level list to check if it's still safe
            // if each ith elem is removed
            List<int> temp = new List<int>(levellist);
            temp.RemoveAt(i);

            // if we can verify at least one instance of the level being
            // true if we remove the i-th element, we win
            if(incdec(temp.ToArray()) && differ(temp.ToArray())) {
                return true;
            }
        }
        // assume we fail if we can't find a solution to this subproblem
        return false;
    }

    public int reportChecker(List<string> l) {
        char[] delimiter = {' '};  // declare a delimiter
        int safe = 0;

        foreach(string ln in l) {
            int ctr = 0;
            // Console.WriteLine($"line is {ln}");
            
            string[] splitter = ln.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            // these are what the problem calls levels
            int[] levels = new int[splitter.Length];
            // Console.WriteLine($"length is {splitter.Length}");

            // store the parsed elements into a temporary array of ints
            foreach(string elem in splitter) {
                // Console.WriteLine($"elem is {elem}");
                try {
                    levels[ctr] = Int32.Parse(elem);
                    
                } catch (FormatException) {
                    Console.WriteLine($"{elem} has bad format");
                }
                // Console.WriteLine($"current elem is {levels[ctr]}");
                ctr++;
            }

            // do work here with helper functions
            bool id = incdec(levels);   // check if all incrementing or decrementing
            bool di = differ(levels);   // check if difference is being maintained
            bool to = tolerate(levels);   // check if tolerance is being maintained

            // our new bool check that goes into the subproblem if the main problem returns false
            if((id && di) || to) {
                safe++;
            }
            
            // Console.WriteLine($"{ctr}");
            // Console.WriteLine();
        }

        return safe;
    }
}
