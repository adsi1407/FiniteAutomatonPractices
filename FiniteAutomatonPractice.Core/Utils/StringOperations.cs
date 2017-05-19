using FiniteAutomatonPractice.Core.Models;
using System.Collections.Generic;
using System.Text;

namespace FiniteAutomatonPractice.Core.Utils
{
    public class StringOperations
    {
        public string ShowInputSymbols(List<InputSymbol> inputSymbolsList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Símbolos de Entrada: ");
            for (int i = 0; i < inputSymbolsList.Count; i++)
            {
                builder.Append(inputSymbolsList[i].Name);

                if (i != inputSymbolsList.Count - 1)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }

        public string ShowStates(List<State> statesList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Estados:");
            builder.Append("\n");
            for (int i = 0; i < statesList.Count; i++)
            {
                builder.Append(statesList[i].Name);
                if (statesList[i].Acceptance)
                {
                    builder.Append(": Aceptación");
                }

                if (i != statesList.Count - 1)
                {
                    builder.Append("\n");
                }
            }
            return builder.ToString();
        }

        public string ShowTransitions(List<Transition> transitionsList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Transiciones:");
            builder.Append("\n");
            for (int i = 0; i < transitionsList.Count; i++)
            {
                builder.Append("Desde: ");
                builder.Append(transitionsList[i].ActualState.Name);
                builder.Append(" - Si Entra: ");
                builder.Append(transitionsList[i].InputSymbol.Name);
                builder.Append(" - Va Hacia: ");
                builder.Append(transitionsList[i].DestinationState.Name);

                if (i != transitionsList.Count - 1)
                {
                    builder.Append("\n");
                }
            }
            return builder.ToString();
        }

        public string WriteFiniteAutomaton(string serializedInputSymbolsList, string serializedStatesList, string serializedTransitionsList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(serializedInputSymbolsList);
            builder.Append("\n");
            builder.Append(serializedStatesList);
            builder.Append("\n");
            builder.Append(serializedTransitionsList);
            return builder.ToString();
        }
    }
}
