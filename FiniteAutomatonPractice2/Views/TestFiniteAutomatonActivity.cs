using Android.App;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Probar Automata Finito", MainLauncher = true)]
    public class TestFiniteAutomatonActivity : Activity
    {
        Button btnRemoveEqualStates;
        Button btnRemoveStrangeStates;
        Button btnConvertToDeterministic;
        Button btnFinish;

        string serializedAutomaton;

        FiniteAutomaton finiteAutomaton;

        AutomatonOperations automatonOperations;
        StringOperations stringOperations;

        bool equalStatesRemoved;
        bool strangeStatesRemoved;

        AlertDialog.Builder builder;
        AlertDialog alertDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.test_automaton_activity);

            btnRemoveEqualStates = FindViewById<Button>(Resource.Id.btnRemoveEqualStates);
            btnRemoveEqualStates.Click += BtnRemoveEqualStates_Click;
            btnRemoveStrangeStates = FindViewById<Button>(Resource.Id.btnRemoveStrangeStates);
            btnRemoveStrangeStates.Click += BtnRemoveStrangeStates_Click;
            btnRemoveStrangeStates.Visibility = Android.Views.ViewStates.Gone;
            btnConvertToDeterministic = FindViewById<Button>(Resource.Id.btnConvertToDeterministic);
            btnConvertToDeterministic.Click += BtnConvertToDeterministic_Click;
            btnFinish = FindViewById<Button>(Resource.Id.btnFinish);
            btnFinish.Click += BtnFinish_Click;

            //serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
            //finiteAutomaton = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton);

            BuildFiniteAutomatonForTest();

            automatonOperations = new AutomatonOperations();
            stringOperations = new StringOperations();
        }

        private void BtnRemoveEqualStates_Click(object sender, System.EventArgs e)
        {
            finiteAutomaton = automatonOperations.RemoveEqualStates(finiteAutomaton);
            equalStatesRemoved = true;
            ShowAutomatonResultDialog();
        }

        private void BtnRemoveStrangeStates_Click(object sender, System.EventArgs e)
        {
            if (equalStatesRemoved)
            {
                finiteAutomaton = automatonOperations.RemoveStrangeStates(finiteAutomaton);
                strangeStatesRemoved = true;
                ShowAutomatonResultDialog();
            }
            else
            {
                Toast.MakeText(this, "Primero debes quitar los estados equivalentes.", ToastLength.Short).Show();
            }
        }

        private void BtnConvertToDeterministic_Click(object sender, System.EventArgs e)
        {
            if (!finiteAutomaton.IsDeterministic)
            {
                if (equalStatesRemoved && strangeStatesRemoved)
                {
                    ShowAutomatonResultDialog();
                }
                else
                {
                    Toast.MakeText(this, "Primero debes quitar los estados equivalentes y extraños.", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "El autómata ya es determinístico.", ToastLength.Short).Show();
            }
        }

        private void BtnFinish_Click(object sender, System.EventArgs e)
        {
        }

        private void ShowAutomatonResultDialog()
        {
            builder = new AlertDialog.Builder(this);
            alertDialog = builder.Create();
            alertDialog.SetTitle("Resultado");
            alertDialog.SetMessage(stringOperations.ShowAllAutomaton(finiteAutomaton));
            alertDialog.Show();
        }

        private void BuildFiniteAutomatonForTest()
        {
            var inputSymbol0 = new InputSymbol { Name = "0" };
            var inputSymbol1 = new InputSymbol { Name = "1" };
            var stateA = new State { Name = "a", Acceptance = true };
            var stateB = new State { Name = "b", Acceptance = false };
            var stateC = new State { Name = "c", Acceptance = true };
            var stateD = new State { Name = "d", Acceptance = false };
            var stateE = new State { Name = "e", Acceptance = false };

            finiteAutomaton = new FiniteAutomaton();
            finiteAutomaton.InputSymbols = new List<InputSymbol>
            {
                inputSymbol0,
                inputSymbol1
            };
            finiteAutomaton.States = new List<State>
            {
                stateA,
                stateB,
                stateC,
                stateD,
                stateE
            };
            finiteAutomaton.Transitions = new List<Transition>
            {
                new Transition { ActualState = stateA, InputSymbol = inputSymbol0, DestinationState = stateA },
                new Transition { ActualState = stateA, InputSymbol = inputSymbol1, DestinationState = stateB },
                new Transition { ActualState = stateB, InputSymbol = inputSymbol0, DestinationState = stateC },
                new Transition { ActualState = stateC, InputSymbol = inputSymbol1, DestinationState = stateB },
                new Transition { ActualState = stateC, InputSymbol = inputSymbol0, DestinationState = stateA },
                new Transition { ActualState = stateD, InputSymbol = inputSymbol0, DestinationState = stateC },
                new Transition { ActualState = stateE, InputSymbol = inputSymbol0, DestinationState = stateE },
            };
            finiteAutomaton.IsDeterministic = true;
        }
    }
}