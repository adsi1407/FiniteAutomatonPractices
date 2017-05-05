using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice1.Adapters;
using FiniteAutomatonPractice1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FiniteAutomatonPractice1.Views
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
            inputSymbolsList = JsonConvert.DeserializeObject<List<InputSymbol>>(serializedInputSymbolsList);
            statesList = JsonConvert.DeserializeObject<List<State>>(serializedStatesList);

            spinnerActualState.Adapter = new StateAdapter(this, statesList);
            spinnerInputSymbol.Adapter = new InputSymbolAdapter(this, inputSymbolsList);
            spinnerDestinationState.Adapter = new StateAdapter(this, statesList);

            transitionsList = new List<Transition>();
        }

        private void BtnSaveTransition_Click(object sender, System.EventArgs e)
        {
            transitionsList.Add(new Transition
            {
                ActualState = statesList[spinnerActualState.SelectedItemPosition],
                InputSymbol = inputSymbolsList[spinnerInputSymbol.SelectedItemPosition],
                DestinationState = statesList[spinnerDestinationState.SelectedItemPosition],
            });

            lblTransitions.Text = ShowTransitions();
            lblTransitions.Visibility = Android.Views.ViewStates.Visible;
        }

        private void BtnSaveFiniteAutomaton_Click(object sender, System.EventArgs e)
        {
            string directoryDocuments = Environment.ExternalStorageDirectory.Path;
            string fileName = Path.Combine(directoryDocuments, "AutomataFinito.txt");
            serializedTransitionsList = JsonConvert.SerializeObject(transitionsList);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, WriteFiniteAutomaton());

            Toast.MakeText(this, string.Format("El autómata finito se ha guardado correctamente en {0}", fileName), ToastLength.Long).Show();

			var intent = new Intent(this, typeof(SummaryActivity));
            intent.PutExtra("serializedInputSymbolsList", serializedInputSymbolsList);
            intent.PutExtra("serializedStatesList", serializedStatesList);
            intent.PutExtra("serializedTransitionsList", serializedTransitionsList);
            StartActivity(intent);
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

        private string WriteFiniteAutomaton()
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