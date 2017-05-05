using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice1.Models;
using Newtonsoft.Json;

namespace FiniteAutomatonPractice1.Views
{
	[Activity(Label = "Resumen")]
	public class SummaryActivity: Activity
	{
		string serializedInputSymbolsList;
		string serializedStatesList;
		string serializedTransitionsList;

		List<InputSymbol> inputSymbolsList;
		List<State> statesList;
		List<Transition> transitionsList;

		TextView lblInputSymbols;
		TextView lblStates;
		TextView lblTransitions;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.summary_activity);

			serializedInputSymbolsList = Intent.GetStringExtra("serializedInputSymbolsList");
			serializedStatesList = Intent.GetStringExtra("serializedStatesList");
			serializedTransitionsList = Intent.GetStringExtra("serializedTransitionsList");

			inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList);
			statesList = JsonConvert.DeserializeObject<List<State>>(serializedStatesList);
			transitionsList = JsonConvert.DeserializeObject<List<Transition>>(serializedTransitionsList);

			lblInputSymbols = FindViewById<TextView>(Resource.Id.lblInputSymbols);
			lblStates = FindViewById<TextView>(Resource.Id.lblStates);
			lblTransitions = FindViewById<TextView>(Resource.Id.lblTransitions);

			lblInputSymbols.Text = ShowInputSymbols();
			lblStates.Text = ShowStates();
			lblTransitions.Text = ShowTransitions();
		}

		private string ShowInputSymbols()
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

		private string ShowStates()
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

		private string ShowTransitions()
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
	}
}
