using Android.App;
using Android.Content;
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
        string serializedAutomaton1;

        List<InputSymbol> inputSymbolsList;
		List<State> statesList;
		List<Transition> transitionsList;

		TextView lblInputSymbols;
		TextView lblStates;
		TextView lblTransitions;
        TextView txtAutomatonType;
        Button btnOperations;

        StringOperations stringOperations;
        AutomatonOperations automatonOperations;

        FiniteAutomaton finiteAutomaton;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.summary_activity);

			serializedInputSymbolsList = Intent.GetStringExtra("serializedInputSymbolsList");
			serializedStatesList = Intent.GetStringExtra("serializedStatesList");
			serializedTransitionsList = Intent.GetStringExtra("serializedTransitionsList");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");

            //No se pueden organizar las listas porque se perdería de vista el estado inicial
            //inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList).OrderBy(x => x.Name).ToList();
            //statesList = JsonConvert.DeserializeObject<List<State>>(serializedStatesList).OrderBy(x => x.Name).ToList();
            //transitionsList = JsonConvert.DeserializeObject<List<Transition>>(serializedTransitionsList).OrderBy(x => x.ActualState.Name).ThenBy(x => x.InputSymbol.Name).ToList();

            inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList);
            statesList = JsonConvert.DeserializeObject<List<State>>(serializedStatesList);
            transitionsList = JsonConvert.DeserializeObject<List<Transition>>(serializedTransitionsList);

            lblInputSymbols = FindViewById<TextView>(Resource.Id.lblInputSymbols);
			lblStates = FindViewById<TextView>(Resource.Id.lblStates);
			lblTransitions = FindViewById<TextView>(Resource.Id.lblTransitions);
            txtAutomatonType = FindViewById<TextView>(Resource.Id.txtAutomatonType);
            btnOperations = FindViewById<Button>(Resource.Id.btnOperations);
            btnOperations.Click += BtnOperations_Click;

            stringOperations = new StringOperations();

            lblInputSymbols.Text = stringOperations.ShowInputSymbols(inputSymbolsList);
			lblStates.Text = stringOperations.ShowStates(statesList);
			lblTransitions.Text = stringOperations.ShowTransitions(transitionsList);

            automatonOperations = new AutomatonOperations();

            bool isDeterministic = automatonOperations.IsDeterministic(inputSymbolsList, statesList, transitionsList);
            if (isDeterministic)
            {
                txtAutomatonType.Text = "Determinístico";
            }
            else
            {
                txtAutomatonType.Text = "No determinístico";
            }

            finiteAutomaton = new FiniteAutomaton();
            finiteAutomaton.InputSymbols = inputSymbolsList;
            finiteAutomaton.States = statesList;
            finiteAutomaton.Transitions = transitionsList;
            finiteAutomaton.IsDeterministic = isDeterministic;
        }

        private void BtnOperations_Click(object sender, System.EventArgs e)
        {
            var serializedAutomaton = JsonConvert.SerializeObject(finiteAutomaton);
            var intent = new Intent(this, typeof(TestFiniteAutomatonActivity));
            intent.PutExtra("serializedAutomaton", serializedAutomaton);
            intent.PutExtra("serializedAutomaton1", serializedAutomaton1);
            StartActivity(intent);
        }
    }
}
