using AFNSimulatorUFPI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFNSimulatorUFPI
{
  class Program
  {
    private static State[] _states;
    private static Transition[] _transitions;

    private static List<State> _currentState;


    static void Main(string[] args)
    {
      var contructor = new AFNConstructor();

      contructor.ReadAFNFromFile(out _states, out _transitions);

      string entry = "100000000000000000100";
      int entryPointer = 0;

      _currentState = new List<State>();
      _currentState.Add(_states[0]);
      var statesToAdd = new List<State>();

      while (_currentState.Count > 0)
      {
        for (var index = _currentState.Count-1; index >= 0; index--)
        {
          var state = _currentState[index];
          var found = false;
          for (int i = 0; i < _transitions.Length; i++)
          {
            var transition = _transitions[i];
            if (transition.From.Equals(state.Name) && transition.Entry == entry[entryPointer])
            {
              var next = _states.FirstOrDefault(x => x.Name == transition.To);
              if (!string.IsNullOrEmpty(next.Name))
              {
                found = true;
                statesToAdd.Add(next);
              }
            }
          }

          if (!found && entryPointer < entry.Length-1)
          {
              Console.WriteLine($"State {state.Name} died.");
          }
          
          Console.WriteLine($"Current: {state.Name}, current entry: {entry[entryPointer]}");
          _currentState.Remove(state);
        }


        foreach (var state1 in statesToAdd)
        {
          Console.WriteLine($"States to add: {state1.Name}");
        }
        _currentState.AddRange(statesToAdd);
        statesToAdd.Clear();
        if (++entryPointer >= entry.Length)
        {
          foreach (var state in _currentState)
          {
            if (state.IsFinal)
            {
              Console.WriteLine("Accepted.");
            }
          }
          return;
        }
      }
    }

    private static void CreateTest()
    {
      _states = new State[4]
      {
        new State{Name = "Q1", IsFinal = false },
        new State{Name = "Q2", IsFinal = false },
        new State{Name = "Q3", IsFinal = false },
        new State{Name = "Q4", IsFinal = true }
      };

      _transitions = new Transition[7]
      {
        new Transition{Entry = '0', From = "Q1", To = "Q1"},
        new Transition{Entry = '1', From = "Q1", To = "Q1"},
        new Transition{Entry = '1', From = "Q1", To = "Q2"},
        new Transition{Entry = '0', From = "Q2", To = "Q3"},
        new Transition{Entry = '1', From = "Q2", To = "Q3"},
        new Transition{Entry = '0', From = "Q3", To = "Q4"},
        new Transition{Entry = '1', From = "Q3", To = "Q4"},
      };
    }
  }
}
