using FiniteAutomatonPractice.Core.Models;
using System.Collections.Generic;

namespace FiniteAutomatonPractice.Core.Utils
{
    public class AutomatonOperations
    {
        public bool IsDeterministic(List<InputSymbol> inputSymbolsList, List<State> statesList, List<Transition> transitionsList)
        {
            bool isDeterministic = true;

            for (int i = 0; i < transitionsList.Count; i++)
            {
                for (int j = 0; j < transitionsList.Count; j++)
                {
                    if (transitionsList[i].ActualState.Name == transitionsList[j].ActualState.Name &&
                        transitionsList[i].InputSymbol.Name == transitionsList[j].InputSymbol.Name &&
                        transitionsList[i].DestinationState.Name != transitionsList[j].DestinationState.Name)
                    {
                        isDeterministic = false;
                        j = transitionsList.Count;
                        i = transitionsList.Count;
                    }
                }
            }

            return isDeterministic;
        }
    }
}
