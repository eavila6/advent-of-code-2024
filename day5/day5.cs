/* Ethan's Advent of code day 5 challenge

https://adventofcode.com/2024/day/5
*/

// base library
using System;
// for use with files and directories
using System.IO;
// for use with regex
using System.Text.RegularExpressions;
// the special linq list library
using System.Linq;

// the class we're working with for this project
class day5 {

    // this function will return a dictionary that correctly finds the ordering of the rules
    static Dictionary<int, List<int>> ordering(string[] rules) {
        // create a return dictionary
        var rdict = new Dictionary<int, List<int>>();
        // foreach rule in the rules list we parsed earlier
        foreach(string rule in rules) {
            // split the rule into key and the page it has to be before
            // very ugly splitting I found off the net, but it works
            var part = rule.Split("|").Select(int.Parse).ToArray();
            // if the key isn't in the dictionary yet, add it
            if(!rdict.ContainsKey(part[0])) {
                rdict[part[0]] = new List<int>();
            }
            // add the page the key must be before to its list
            rdict[part[0]].Add(part[1]);
        }
        // debugging terminal writes
        /*
        foreach(KeyValuePair<int, List<int>> key in rdict) {
            Console.WriteLine($"key {key.Key}'s list is {key.Value}");
            foreach(var elem in key.Value) {
                Console.WriteLine($"elem is {elem}");
            }
            Console.WriteLine();
        }
        */

        return rdict;
    }

    // this only triggers if our page order is correct, so this is fine implementation
    static int getMiddle(List<int> uelems) {
        int middle = uelems.Count/2;
        return uelems[middle];
    } 

    static bool validateUpdate(List<int> elems, Dictionary<int, List<int>> rules) {
        // generate a mini dictionary to validate the rules of each of the elems inside
        var pagePos = elems.Select((page, index) => new {page, index}).ToDictionary(x=>x.page, x=>x.index);

        // foreach rule we know of
        foreach(var law in rules) {
            // get a key
            int ante = law.Key;
            // foreach element in that key's list
            foreach(int post in law.Value) {
                // if the update has both keys in it
                if (pagePos.ContainsKey(ante) && pagePos.ContainsKey(post)) {
                    // but if the ordering's wrong
                    if(pagePos[ante] >= pagePos[post]) {
                        // fail
                        return false;
                    }
                }
            }
        }
        // succeed
        return true;
    }

    static int sumUpdates(string[] update, Dictionary<int, List<int>> rules) {
        // the return value with our solution
        int retval = 0;

        // starting at 1 bc our input separates the two lists with a blank line
        for(int i = 1; i < update.Length; i++) {
            var uelems = update[i].Split(",").Select(int.Parse).ToList();
            bool valid = validateUpdate(uelems, rules);

            if(valid) {
                int temp = getMiddle(uelems);
                retval += temp;
            }
        }


        return retval;
    }
    
    // make a file reader, takes in a text file of arguments to parse
    // This file reader returns a tuple of (string array, string array)
    // I feel like this weird tuple type will help me do dictionary work more easily
    static (string[], string[]) fileReader(string f) {
        // f is our filename, and here's how we can check if we can get it
        string fil = Path.Combine(Directory.GetCurrentDirectory(), f);
        
        // a list to actually work with in a helper function
        List<string> rules = new List<string>();
        List<string> updates = new List<string>();
        
        // the regex pattern I'm using to find rules
        string pattern = @"(\d+)\|(\d+)";

        // To read a text file line by line 
        if (File.Exists(fil)) {
            Console.WriteLine("Found the file");
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(fil);
            // Console.WriteLine($"size of lines is {lines.Length}");

            foreach(string ln in lines) {
                // Console.WriteLine($"ln is {ln}");
                // Console.WriteLine();
                Match mymatch = Regex.Match(ln, pattern);
                // if regex matching was successful, append line to rules
                if(mymatch.Success) {
                    rules.Add(ln);
                }
                // else append line to updates
                else {
                    updates.Add(ln);
                }
            }
        } else {
            Console.WriteLine("The filename " + fil + " doesn't exist");
            Console.WriteLine("Please recheck spelling and file location");
        }

        /*
        foreach(string elem in rules) {
            Console.WriteLine($"elem in rules is {elem}");
        }
        foreach(string elem in updates) {
            Console.WriteLine($"elem in updates is {elem}");
        }
        */
        // Console.WriteLine($"sizeof rules is {rules.Count}");
        // Console.WriteLine($"sizeof updates is {updates.Count}");
        
        string[] rularr = rules.ToArray();
        string[] updarr = updates.ToArray();
        
        return (rularr, updarr);
        
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
        var(rules, updates) = fileReader(args[0]);
        
        // when there's nothing in the list
        
        if(!rules.Any() || !updates.Any()) {
            Console.WriteLine("The parsed lists have issues with them, exit failure");
            Console.WriteLine("One or both of the lists are empty");
            
            Environment.Exit(1);
        }
        
        var rulesDict = ordering(rules);
        
        int answer = sumUpdates(updates, rulesDict);
        Console.WriteLine("Our answer is " + answer);
        
        
        Console.WriteLine("Hello World!"); 
        Environment.Exit(0);
    }
}
