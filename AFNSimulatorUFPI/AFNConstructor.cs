using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AFNSimulatorUFPI.Types;

namespace AFNSimulatorUFPI
{
    public class AFNConstructor
    {
        public string path = "../../../Data.txt";

        
        public void ReadAFNFromFile(out State[] states, out Transition[] transitions)
        {
            List<State> statesList = new List<State>();
            List<Transition> transitionsList = new List<Transition>();

            string[] stateSeparator = new[]
            {
                "S:",
                "Final:",
            };
            
            string[] transitionSeparator = new[]
            {
                "Entry:",
                "From:",
                "To:",
            };

            foreach (var line in File.ReadAllLines(path))
            {
                string[] result;
                if (line.StartsWith("S:"))
                {
                    result = line.Split(stateSeparator, StringSplitOptions.RemoveEmptyEntries);
                    statesList.Add(new State()
                    {
                        Name = result[0].Remove(result[0].Length-1, 1),
                        IsFinal = result[1].Remove(result[1].Length-1, 1).Equals("true")
                    });
                }
                else if (line.StartsWith("Entry:"))
                {
                    result = line.Split(transitionSeparator, StringSplitOptions.RemoveEmptyEntries);
                    transitionsList.Add(new Transition()
                    {
                        Entry = Convert.ToChar(result[0].Remove(result[0].Length-1, 1)),
                        From = result[1].Remove(result[1].Length-1, 1),
                        To = result[2].Remove(result[2].Length-1, 1),
                    });
                }
                
            }
            
            foreach (var s in statesList)
            {
                Console.WriteLine($"State: {s.Name} Final: {s.IsFinal}");
            }

            foreach (var transition in transitionsList)
            {
                Console.WriteLine($"Transition entry: {transition.Entry} From {transition.From} To {transition.To}");
            }

            states = statesList.ToArray();
            transitions = transitionsList.ToArray();
        }
    }
}