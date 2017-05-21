using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using FiniteAutomatonPractice2.Adapters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Ingresar Transiciones")]
    public class TransitionsActivity : Activity
    {
        Spinner spinnerActualState;
        Spinner spinnerInputSymbol;
        Spinner spinnerDestinationState;
        Button btnSaveTransition;
        Button btnSaveFiniteAutomaton;
        TextView lblTransitions;

        string serializedInputSymbolsList;
        string serializedStatesList;
        List<InputSymbol> inputSymbolsList;
        List<State> statesList;
        List<Transition> transitionsList;
        string serializedTransitionsList;
        string serializedAutomaton1;

        StringOperations stringOperations;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.transitions_activity);

            spinnerActualState = FindViewById<Spinner>(Resource.Id.spinnerActualState);
            spinnerInputSymbol = FindViewById<Spinner>(Resource.Id.spinnerInputSymbol);
            spinnerDestinationState = FindViewById<Spinner>(Resource.Id.spinnerDestinationState);

            btnSaveTransition = FindViewById<Button>(Resource.Id.btnSaveTransition);
            btnSaveTransition.Click += BtnSaveTransition_Click;
            btnSaveFiniteAutomaton = FindViewById<Button>(Resource.Id.btnSaveFiniteAutomaton);
            btnSaveFiniteAutomaton.Click += BtnSaveFiniteAutomaton_Click;

            lblTransitions = FindViewById<TextView>(Resource.Id.lblTransitions);

            serializedInputSymbolsList = Intent.GetStringExtra("serializedInputSymbolsList");
            serializedStatesList = Intent.GetStringExtra("serializedStatesList");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");
            inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList);
            statesList = JsonConvert.DeserializeObject<List<State>>(serializedStatesList);

            spinnerActualState.Adapter = new StateAdapter(this, statesList);
            spinnerInputSymbol.Adapter = new InputSymbolAdapter(this, inputSymbolsList);
            spinnerDestinationState.Adapter = new StateAdapter(this, statesList);

            transitionsList = new List<Transition>();
            stringOperations = new StringOperations();
        }

        private void BtnSaveTransition_Click(object sender, System.EventArgs e)
        {
            transitionsList.Add(new Transition
            {
                ActualState = statesList[spinnerActualState.SelectedItemPosition],
                InputSymbol = inputSymbolsList[spinnerInputSymbol.SelectedItemPosition],
                DestinationState = statesList[spinnerDestinationState.SelectedItemPosition],
            });

            lblTransitions.Text = stringOperations.ShowTransitions(transitionsList);
            lblTransitions.Visibility = Android.Views.ViewStates.Visible;
        }

        private void BtnSaveFiniteAutomaton_Click(object sender, System.EventArgs e)
        {
            if (transitionsList.Count > 0)
            {
                string directoryDocuments = Environment.ExternalStorageDirectory.Path;
                string fileName = Path.Combine(directoryDocuments, "AutomataFinito.txt");
                serializedTransitionsList = JsonConvert.SerializeObject(transitionsList);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                if (string.IsNullOrEmpty(serializedAutomaton1))
                {
                    FiniteAutomaton finiteAutomatonAux1 = new FiniteAutomaton();
                    finiteAutomatonAux1.InputSymbols = new List<InputSymbol>();
                    finiteAutomatonAux1.States = new List<State>();
                    finiteAutomatonAux1.Transitions = new List<Transition>();
                    finiteAutomatonAux1.IsDeterministic = true;

                    string serializedFiniteAutomatonAux1 = JsonConvert.SerializeObject(finiteAutomatonAux1);

                    FiniteAutomaton finiteAutomatonAux2 = new FiniteAutomaton();
                    finiteAutomatonAux2.InputSymbols = inputSymbolsList;
                    finiteAutomatonAux2.States = statesList;
                    finiteAutomatonAux2.Transitions = transitionsList;
                    finiteAutomatonAux2.IsDeterministic = true;

                    string serializedFiniteAutomatonAux2 = JsonConvert.SerializeObject(finiteAutomatonAux2);

                    File.WriteAllText(fileName, stringOperations.WriteTwoFiniteAutomatons(serializedFiniteAutomatonAux1, serializedFiniteAutomatonAux2));
                }
                else
                {
                    FiniteAutomaton finiteAutomatonAux2 = new FiniteAutomaton();
                    finiteAutomatonAux2.InputSymbols = inputSymbolsList;
                    finiteAutomatonAux2.States = statesList;
                    finiteAutomatonAux2.Transitions = transitionsList;
                    finiteAutomatonAux2.IsDeterministic = true;

                    string serializedFiniteAutomatonAux2 = JsonConvert.SerializeObject(finiteAutomatonAux2);

                    File.WriteAllText(fileName, stringOperations.WriteTwoFiniteAutomatons(serializedAutomaton1, serializedFiniteAutomatonAux2));
                }

                Toast.MakeText(this, string.Format("El autómata finito se ha guardado correctamente en {0}", fileName), ToastLength.Long).Show();

                var intent = new Intent(this, typeof(SummaryActivity));
                intent.PutExtra("serializedInputSymbolsList", serializedInputSymbolsList);
                intent.PutExtra("serializedStatesList", serializedStatesList);
                intent.PutExtra("serializedTransitionsList", serializedTransitionsList);
                intent.PutExtra("serializedAutomaton1", serializedAutomaton1);
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Debes ingresar al menos una transición.", ToastLength.Short).Show();
            }
        }
    }
}