﻿using FiniteAutomatonPractice.Core.Models;
using System.Collections.Generic;
using System.Linq;

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

		public FiniteAutomaton RemoveEqualStates(FiniteAutomaton finiteAutomatonToSimplify)
		{
			var finiteAutomatonResult = new FiniteAutomaton();
			finiteAutomatonResult.InputSymbols = finiteAutomatonToSimplify.InputSymbols;
			finiteAutomatonResult.IsDeterministic = finiteAutomatonToSimplify.IsDeterministic;

			List<Transition> repeatedTransitions = new List<Transition>();

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

		public bool EqualInputSymbols(FiniteAutomaton finiteAutomaton1, FiniteAutomaton finiteAutomaton2)
		{
			bool result = true;

			List<InputSymbol> inputSymbols1 = finiteAutomaton1.InputSymbols.OrderBy(x => x.Name).ToList();
			List<InputSymbol> inputSymbols2 = finiteAutomaton2.InputSymbols.OrderBy(x => x.Name).ToList();

			if (inputSymbols1.Count == inputSymbols2.Count)
			{
				for (int i = 0; i < inputSymbols1.Count; i++)
				{
					if (inputSymbols1[i].Name != inputSymbols2[i].Name)
					{
						result = false;
						i = inputSymbols1.Count;
					}
				}
			}
			else
			{
				result = false;
			}

			return result;
		}

		public FiniteAutomaton JoinFiniteAutomatons(FiniteAutomaton finiteAutomaton1, FiniteAutomaton finiteAutomaton2)
		{
			var finiteAutomatonResult = new FiniteAutomaton();
			finiteAutomatonResult.InputSymbols = finiteAutomaton1.InputSymbols;
			finiteAutomatonResult.IsDeterministic = finiteAutomaton1.IsDeterministic;
			finiteAutomatonResult.States = new List<State>();
			finiteAutomatonResult.Transitions = new List<Transition>();
			bool isAceptance;

			for (int i = 0; i < finiteAutomaton1.States.Count(); i++)
			{
				for (int j = 0; j < finiteAutomaton2.States.Count(); j++)
				{
					if (finiteAutomaton1.States[i].Acceptance || finiteAutomaton2.States[j].Acceptance)
					{
						isAceptance = true;
					}
					else
					{
						isAceptance = false;
					}
					finiteAutomatonResult.States.Add(
						new State
						{
							Name = string.Format("{0}{1}", finiteAutomaton1.States[i].Name, finiteAutomaton2.States[j].Name),
							Acceptance = isAceptance
						});
				}
			}

			for (int i = 0; i < finiteAutomaton1.Transitions.Count(); i++)
			{
				for (int j = 0; j < finiteAutomaton2.Transitions.Count(); j++)
				{
					if (finiteAutomaton1.Transitions[i].InputSymbol.Name == finiteAutomaton2.Transitions[j].InputSymbol.Name)
					{
						var actualState = finiteAutomatonResult.States.Where(x => x.Name == string.Format("{0}{1}", finiteAutomaton1.Transitions[i].ActualState.Name, finiteAutomaton2.Transitions[j].ActualState.Name)).FirstOrDefault();
						var destinationState = finiteAutomatonResult.States.Where(x => x.Name == string.Format("{0}{1}", finiteAutomaton1.Transitions[i].DestinationState.Name, finiteAutomaton2.Transitions[j].DestinationState.Name)).FirstOrDefault();
						finiteAutomatonResult.Transitions.Add(
							new Transition()
							{
								ActualState = actualState,
								InputSymbol = finiteAutomaton1.Transitions[i].InputSymbol,
								DestinationState = destinationState,
							}
						);
					}
				}
			}

			return finiteAutomatonResult;
		}

		public FiniteAutomaton IntersectFiniteAutomatons(FiniteAutomaton finiteAutomaton1, FiniteAutomaton finiteAutomaton2)
		{
			var finiteAutomatonResult = new FiniteAutomaton();
			finiteAutomatonResult.InputSymbols = finiteAutomaton1.InputSymbols;
			finiteAutomatonResult.IsDeterministic = finiteAutomaton1.IsDeterministic;
			finiteAutomatonResult.States = new List<State>();
			finiteAutomatonResult.Transitions = new List<Transition>();
			bool isAceptance;

			for (int i = 0; i < finiteAutomaton1.States.Count(); i++)
			{
				for (int j = 0; j < finiteAutomaton2.States.Count(); j++)
				{
					if (finiteAutomaton1.States[i].Acceptance && finiteAutomaton2.States[j].Acceptance)
					{
						isAceptance = true;
					}
					else
					{
						isAceptance = false;
					}
					finiteAutomatonResult.States.Add(
						new State
						{
							Name = string.Format("{0}{1}", finiteAutomaton1.States[i].Name, finiteAutomaton2.States[j].Name),
							Acceptance = isAceptance
						});
				}
			}

			for (int i = 0; i < finiteAutomaton1.Transitions.Count(); i++)
			{
				for (int j = 0; j < finiteAutomaton2.Transitions.Count(); j++)
				{
					if (finiteAutomaton1.Transitions[i].InputSymbol.Name == finiteAutomaton2.Transitions[j].InputSymbol.Name)
					{
						var actualState = finiteAutomatonResult.States.Where(x => x.Name == string.Format("{0}{1}", finiteAutomaton1.Transitions[i].ActualState.Name, finiteAutomaton2.Transitions[j].ActualState.Name)).FirstOrDefault();
						var destinationState = finiteAutomatonResult.States.Where(x => x.Name == string.Format("{0}{1}", finiteAutomaton1.Transitions[i].DestinationState.Name, finiteAutomaton2.Transitions[j].DestinationState.Name)).FirstOrDefault();
						finiteAutomatonResult.Transitions.Add(
							new Transition()
							{
								ActualState = actualState,
								InputSymbol = finiteAutomaton1.Transitions[i].InputSymbol,
								DestinationState = destinationState,
							}
						);
					}
				}
			}

			return finiteAutomatonResult;
		}

		public State TestRow(FiniteAutomaton finiteAutomaton, string row)
		{
			var actualState = finiteAutomaton.States[0];
			var errorState = new State
			{
				Name = "Estado de Error",
				Acceptance = false,
			};
			Transition actualTransition;

			for (int i = 0; i < row.Length; i++)
			{
				actualTransition = finiteAutomaton.Transitions.Where(x => x.ActualState.Name == actualState.Name && x.InputSymbol.Name == row.Substring(i, 1)).FirstOrDefault();
				if (actualTransition != null)
				{
					actualState = actualTransition.DestinationState;
				}
				else
				{
					actualState = errorState;
				}
			}

			return actualState;
		}
	}
}
