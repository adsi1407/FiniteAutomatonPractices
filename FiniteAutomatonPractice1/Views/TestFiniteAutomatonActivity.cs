using Android.App;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FiniteAutomatonPractice1.Views
{
    [Activity(Label = "Probar Automata Finito")]
    public class TestFiniteAutomatonActivity : Activity
    {
        Button btnRemoveEqualStates;
        Button btnRemoveStrangeStates;

        string serializedInputSymbolsList;
        string serializedStatesList;
        string serializedTransitionsList;

        List<InputSymbol> inputSymbolsList;
        List<State> statesList;
        List<Transition> transitionsList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.test_automaton_activity);

            btnRemoveEqualStates = FindViewById<Button>(Resource.Id.btnRemoveEqualStates);
            btnRemoveEqualStates.Click += BtnRemoveEqualStates_Click;
            btnRemoveStrangeStates = FindViewById<Button>(Resource.Id.btnRemoveStrangeStates);
            btnRemoveStrangeStates.Click += BtnRemoveStrangeStates_Click;

            serializedInputSymbolsList = Intent.GetStringExtra("serializedInputSymbolsList");
            serializedStatesList = Intent.GetStringExtra("serializedStatesList");
            serializedTransitionsList = Intent.GetStringExtra("serializedTransitionsList");

            inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList);
            statesList = JsonConvert.DeserializeObject<List<State>>(serializedInputSymbolsList);
            transitionsList = JsonConvert.DeserializeObject<List<Transition>>(serializedInputSymbolsList);
        }

        private void BtnRemoveEqualStates_Click(object sender, System.EventArgs e)
        {
        }

        private void BtnRemoveStrangeStates_Click(object sender, System.EventArgs e)
        {
        }
    }
}