using System.Collections.Generic;

namespace FiniteAutomatonPractice.Core.Models
{
    public class FiniteAutomaton
    {
        public List<InputSymbol> InputSymbols { get; set; }

        public List<State> States { get; set; }

        public List<Transition> Transitions { get; set; }

        public bool IsDeterministic { get; set; }
    }
}
