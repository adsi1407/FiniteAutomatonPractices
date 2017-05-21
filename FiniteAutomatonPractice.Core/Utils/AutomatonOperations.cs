using FiniteAutomatonPractice.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomatonPractice.Core.Utils
{
    public class AutomatonOperations
    {
        List<Transition> repeatedTransitions;
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

        public FiniteAutomaton RemoveEqualStates(FiniteAutomaton finiteAutomatonToSimplify)
        {
            var finiteAutomatonResult = new FiniteAutomaton();
            finiteAutomatonResult.InputSymbols = finiteAutomatonToSimplify.InputSymbols;
            finiteAutomatonResult.IsDeterministic = finiteAutomatonToSimplify.IsDeterministic;

            repeatedTransitions = new List<Transition>();

            List<Transition> transitionsByActualStateI;
            List<Transition> transitionsByActualStateJ;

            for (int i = 0; i < finiteAutomatonToSimplify.Transitions.Count; i++)
            {
                transitionsByActualStateI = finiteAutomatonToSimplify.Transitions.Where(x => x.ActualState.Name.Equals(finiteAutomatonToSimplify.Transitions[i].ActualState.Name)).OrderBy(x => x.InputSymbol.Name).ToList();
                for (int j = 0; j < finiteAutomatonToSimplify.Transitions.Count; j++)
                {
                    transitionsByActualStateJ = finiteAutomatonToSimplify.Transitions.Where(x => x.ActualState.Name.Equals(finiteAutomatonToSimplify.Transitions[j].ActualState.Name)).OrderBy(x => x.InputSymbol.Name).ToList();

                    if (finiteAutomatonToSimplify.Transitions[i].ActualState.Name != finiteAutomatonToSimplify.Transitions[j].ActualState.Name &&
                        finiteAutomatonToSimplify.Transitions[i].ActualState.Acceptance == finiteAutomatonToSimplify.Transitions[j].ActualState.Acceptance)
                    {
                        if (transitionsByActualStateI.Count == transitionsByActualStateJ.Count)
                        {
                            bool isEqualFlag = true;
                            for (int k = 0; k < transitionsByActualStateI.Count; k++)
                            {
                                if (transitionsByActualStateI[k].InputSymbol.Name != transitionsByActualStateJ[k].InputSymbol.Name ||
                                    transitionsByActualStateI[k].DestinationState.Name != transitionsByActualStateJ[k].DestinationState.Name)
                                {
                                    isEqualFlag = false;
                                    k = transitionsByActualStateI.Count;
                                }
                            }

                            if (isEqualFlag)
                            {
                                for (int k = 0; k < transitionsByActualStateJ.Count; k++)
                                {
                                    if (repeatedTransitions.Where(x => x.InputSymbol.Name == transitionsByActualStateJ[k].InputSymbol.Name &&
                                        x.DestinationState.Name == transitionsByActualStateJ[k].DestinationState.Name &&
                                        x.ActualState.Acceptance == transitionsByActualStateJ[k].ActualState.Acceptance).Count() == 0)
                                    {
                                        repeatedTransitions.Add(transitionsByActualStateJ[k]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            finiteAutomatonResult.States = new List<State>();
            for (int i = 0; i < finiteAutomatonToSimplify.States.Count; i++)
            {
                if (repeatedTransitions.Where(x => x.ActualState.Name == finiteAutomatonToSimplify.States[i].Name).ToList().Count == 0)
                {
                    finiteAutomatonResult.States.Add(finiteAutomatonToSimplify.States[i]);
                }
            }

            finiteAutomatonResult.Transitions = new List<Transition>();
            for (int i = 0; i < finiteAutomatonToSimplify.Transitions.Count; i++)
            {
                if (repeatedTransitions.Where(x => x.InputSymbol.Name == finiteAutomatonToSimplify.Transitions[i].InputSymbol.Name &&
                                        x.ActualState.Name == finiteAutomatonToSimplify.Transitions[i].ActualState.Name &&
                                        x.DestinationState.Name == finiteAutomatonToSimplify.Transitions[i].DestinationState.Name &&
                                        x.ActualState.Acceptance == finiteAutomatonToSimplify.Transitions[i].ActualState.Acceptance).Count() == 0)
                {
                    finiteAutomatonResult.Transitions.Add(finiteAutomatonToSimplify.Transitions[i]);
                }
            }

            return finiteAutomatonResult;
        }

        public FiniteAutomaton RemoveStrangeStates(FiniteAutomaton finiteAutomatonToSimplify)
        {
            var finiteAutomatonResult = new FiniteAutomaton();
            finiteAutomatonResult.InputSymbols = finiteAutomatonToSimplify.InputSymbols;
            finiteAutomatonResult.IsDeterministic = finiteAutomatonToSimplify.IsDeterministic;

            var initState = finiteAutomatonToSimplify.States.FirstOrDefault();

            var normalStates = new List<State>();
            normalStates.Add(initState);

            var normalTransitions = new List<Transition>();
            normalTransitions.AddRange(finiteAutomatonToSimplify.Transitions.Where(x => x.ActualState.Name == initState.Name));

            for (int i = 0; i < finiteAutomatonToSimplify.Transitions.Count; i++)
            {
                if (normalStates.Any(x => x.Name == finiteAutomatonToSimplify.Transitions[i].ActualState.Name))
                {
                    normalStates.Add(finiteAutomatonToSimplify.Transitions[i].DestinationState);
                }
            }

            finiteAutomatonResult.States = normalStates.Distinct().ToList();

            for (int i = 0; i < finiteAutomatonToSimplify.Transitions.Count; i++)
            {
                if (finiteAutomatonToSimplify.Transitions[i].ActualState.Name != initState.Name &&
                    finiteAutomatonResult.States.Where(x => x.Name == finiteAutomatonToSimplify.Transitions[i].ActualState.Name).ToList().Count >= 0)
                {
                    normalTransitions.Add(finiteAutomatonToSimplify.Transitions[i]);
                }
            }

            finiteAutomatonResult.Transitions = normalTransitions;

            return finiteAutomatonResult;
        }
    }
}
