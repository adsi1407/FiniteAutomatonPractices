namespace FiniteAutomatonPractice1.Models
{
    public class Transition
    {
        public State ActualState { get; set; }

        public InputSymbol InputSymbol { get; set; }

        public State DestinationState { get; set; }
    }
}