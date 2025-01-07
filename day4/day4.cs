/* Ethan's Advent of code day 4 challenge

https://adventofcode.com/2024/day/4
*/

// base library
using System;
// for use with files and directories
using System.IO;
// for use with regex
using System.Text.RegularExpressions;

// the class we're working with for this project
class day4 {

    // my new overloaded reverse funct
    // this reverses a given string using char[] array reversal
    public static string Reverse(string s) {
        // Console.WriteLine($"given {s}");
        char[] arr = s.ToCharArray();
        Array.Reverse(arr);
        string temp = new string(arr);
        // Console.WriteLine($"giving {temp}");
        return temp;
    }

    // this counts the number of matches in a given line
    static int counter(string ln, string word) {
        Regex myregex = new Regex(word);
        MatchCollection matches = myregex.Matches(ln);
        return matches.Count;
    }

    static string[] generateDiagonals(string[] grid) {
        int n = grid.Length;    // get height
        int m = grid[0].Length; // get width
        List<string> dlist = new List<string>(); // list to do work with, easier to manipulate

        // get west to east diagonals
        for(int i = 0; i < n + m; i++) {
            // our diagonal string obj
            string diagonal = "";
            // j is a row
            for(int row = 0; row < n; row++) {
                // k is a column location
                int col = i - row;
                // if k not negative and k < grid width, append
                if(col >= 0 && col < m) {
                    diagonal += grid[row][col];
                }
            }
            // if our string isn't empty, append to list
            if (diagonal.Length > 0) {
                // Console.WriteLine($"diagonal is {diagonal}");
                dlist.Add(diagonal);
            }
        }

        // get east to west diagonals, do basically same thing as above
        for(int i = 0; i < n + m; i++) {
            string diagonal = "";
            for(int row = 0; row < n; row++) {
                int col = i - (n - 1 - row);
                if(col >= 0 && col < m) {
                    diagonal += grid[row][col];
                }
            }
            if (diagonal.Length > 0) {
                // Console.WriteLine($"diagonal is {diagonal}");
                dlist.Add(diagonal);
            }
        }

        // turn list back into an array
        string[] retlist = dlist.ToArray();

        return retlist;
    }

    static int findWord(string[] grid, string word) {
        int n = grid.Length;
        int m = grid[0].Length;
        int ctr = 0;

        // count rows front & back
        foreach(string ln in grid) {
            ctr += counter(ln, word);
            ctr += counter(Reverse(ln), word);
        }

        // count cols front & back
        for(int i = 0; i < m; i++) {
            string colstr = "";
            for(int j = 0; j < n; j++) {
                colstr += grid[j][i];
            }
            ctr += counter(colstr, word);
            ctr += counter(Reverse(colstr), word);
        }

        // count diagonals
        string[] temp = generateDiagonals(grid);
        foreach(string di in temp) {
            ctr += counter(di, word);
            ctr += counter(Reverse(di), word);
        }

        return ctr;
    }
    
    // make a file reader, takes in a text file of arguments to parse
    // This file reader returns a string array
    static string[] fileReader(string f) {
        // f is our filename, and here's how we can check if we can get it
        string fil = Path.Combine(Directory.GetCurrentDirectory(), f);
        
        // a list to actually work with in a helper function
        List<string> grid = new List<string>();
        

        // To read a text file line by line 
        if (File.Exists(fil)) {
            Console.WriteLine("Found the file");
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(fil);
            // Console.WriteLine($"size of lines is {lines.Length}");

            foreach(string ln in lines) {
                // Console.WriteLine($"ln is {ln}");
                // Console.WriteLine();
                grid.Add(ln);
               
            }
        } else {
            Console.WriteLine("The filename " + fil + " doesn't exist");
            Console.WriteLine("Please recheck spelling and file location");
        }

        string[] gridarr = grid.ToArray();
        /*
        for (int i = 0; i < gridarr.Length; i++) {
            Console.WriteLine($"gridarr[{i}] is {gridarr[i]}");
        }
        */

        return gridarr;
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
        var mygrid = fileReader(args[0]);
        
        // when there's nothing in the list
        if(!mygrid.Any()) {
            Console.WriteLine("The parsed list has issues with it, exit failure");
            Console.WriteLine("The list is empty");
            Environment.Exit(1);
        }

        // the word I'm searching for
        string myword = "XMAS";

        /*
        foreach(string elem in mygrid) {
            string temp = Reverse(elem);
        }
        string[] thingy = generateDiagonals(mygrid);
        */

        
        int answer = findWord(mygrid, myword);
        Console.WriteLine("Our answer is " + answer);

        // pt2 section
        part2 mypt2 = new part2();
        string myword2 = "MAS";
        int pt2ans = mypt2.findCross(mygrid, myword2);
        Console.WriteLine($"pt2 answer is {pt2ans}");
        
        
        Console.WriteLine("Hello World!"); 
        Environment.Exit(0);
    }
}

class part2 : day4 {
    /* the crossword search problem actually simplifies things a lot
    all I have to do is scan the grid generated by the parent's filereader
    and check if the cross exists
    */

    // detCross determines if a cross exists
    static bool detCross(string[] grid, int row, int col, string word) {
        // get the diag starting with northwest position to southeast position
        string nwse = $"{grid[row - 1][col - 1]}{grid[row][col]}{grid[row + 1][col + 1]}";

        // get the diag starting on northeast to southwest
        string nesw = $"{grid[row - 1][col + 1]}{grid[row][col]}{grid[row + 1][col - 1]}";

        // check if the word is on the cross in either direction, then return
        bool diag1 = nwse == word || nwse == Reverse(word);
        bool diag2 = nesw == word || nesw == Reverse(word);

        return diag1 && diag2;
    }

    // ctrCross counts the number of crosses determined
    // it's actually not that fancy... but I still got stumped for a while
    public int findCross(string[] grid, string word) {
        // rip from findWord
        int n = grid.Length;
        int m = grid[0].Length;
        int ctr = 0;

        // the simple loop that finds the crosses...
        for(int row = 1; row < n -1; row++) {
            for(int col = 1; col < m - 1; col++) {
                if(detCross(grid, row, col, word)) {
                    ctr++;
                }
            }
        }

        return ctr;
    }
}
