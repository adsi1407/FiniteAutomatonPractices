﻿using Android.App;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomatonPractice2.Views
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

        StringOperations stringOperations;

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

            stringOperations = new StringOperations();

            lblInputSymbols.Text = stringOperations.ShowInputSymbols(inputSymbolsList.OrderBy(x => x.Name).ToList());
			lblStates.Text = stringOperations.ShowStates(statesList.OrderBy(x => x.Name).ToList());
			lblTransitions.Text = stringOperations.ShowTransitions(transitionsList.OrderBy(x => x.ActualState.Name).ThenBy(x => x.InputSymbol.Name).ToList());

		}
	}
}
