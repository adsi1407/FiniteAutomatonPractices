using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using Newtonsoft.Json;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Probar Automata Finito")]
    public class UnionIntersectionActivity : Activity
    {
        string serializedAutomaton;
        string serializedAutomaton1;
        FiniteAutomaton finiteAutomaton1;
        FiniteAutomaton finiteAutomaton2;
        FiniteAutomaton finiteAutomatonResult;

        Button btnUnion;
        Button btnIntersection;
		Button btnEnterRow;

        AutomatonOperations automatonOperations;
        StringOperations stringOperations;

        AlertDialog.Builder builder;
        AlertDialog alertDialog;

		bool isJoinedOrIntersected;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.union_intersection_activity);

            serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");

            finiteAutomaton1 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton1);
            finiteAutomaton2 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton);

			//BuildFiniteAutomatonForTest();

            btnUnion = FindViewById<Button>(Resource.Id.btnUnion);
            btnUnion.Click += BtnUnion_Click;
            btnIntersection = FindViewById<Button>(Resource.Id.btnIntersection);
            btnIntersection.Click += BtnIntersection_Click;
			btnEnterRow = FindViewById<Button>(Resource.Id.btnEnterRow);
			btnEnterRow.Click += BtnEnterRow_Click;

            automatonOperations = new AutomatonOperations();
            stringOperations = new StringOperations();
        }

		private void BtnUnion_Click(object sender, System.EventArgs e)
		{
			if (automatonOperations.EqualInputSymbols(finiteAutomaton1, finiteAutomaton2))
			{
				finiteAutomatonResult = automatonOperations.JoinFiniteAutomatons(finiteAutomaton1, finiteAutomaton2);
				isJoinedOrIntersected = true;
				ShowAutomatonResultDialog();
			}
			else
			{
				Toast.MakeText(this, "Para intersectar dos autómatas finitos, éstos deben tener los mismos símbolos de entrada.", ToastLength.Short).Show();
			}
		}

        private void BtnIntersection_Click(object sender, System.EventArgs e)
        {
            if (automatonOperations.EqualInputSymbols(finiteAutomaton1, finiteAutomaton2))
            {
                finiteAutomatonResult = automatonOperations.IntersectFiniteAutomatons(finiteAutomaton1, finiteAutomaton2);
				isJoinedOrIntersected = true;
                ShowAutomatonResultDialog();
            }
            else
            {
                Toast.MakeText(this, "Para unir dos autómatas finitos, éstos deben tener los mismos símbolos de entrada.", ToastLength.Short).Show();
            }
        }

		private void BtnEnterRow_Click(object sender, System.EventArgs e)
		{
			if (isJoinedOrIntersected)
			{
				var intent = new Intent(this, typeof(EnterRowActivity));
				intent.PutExtra("serializedAutomaton", JsonConvert.SerializeObject(finiteAutomatonResult));
				StartActivity(intent);
			}
			else
			{
				Toast.MakeText(this, "Primero debes realizar la unión o intersección de los autómatas.", ToastLength.Short).Show();
			}
		}

        private void ShowAutomatonResultDialog()
        {
            builder = new AlertDialog.Builder(this);
            alertDialog = builder.Create();
            alertDialog.SetTitle("Resultado");
            alertDialog.SetMessage(stringOperations.ShowAllAutomaton(finiteAutomatonResult));
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

			finiteAutomaton1 = new FiniteAutomaton();
			finiteAutomaton1.InputSymbols = new List<InputSymbol>
			{
				inputSymbol0,
				inputSymbol1
			};
			finiteAutomaton1.States = new List<State>
			{
				stateA,
				stateB,
			};
			finiteAutomaton1.Transitions = new List<Transition>
			{
				new Transition { ActualState = stateA, InputSymbol = inputSymbol0, DestinationState = stateA },
				new Transition { ActualState = stateA, InputSymbol = inputSymbol1, DestinationState = stateB },
				new Transition { ActualState = stateB, InputSymbol = inputSymbol0, DestinationState = stateB },
            };
			finiteAutomaton1.IsDeterministic = true;

			finiteAutomaton2 = new FiniteAutomaton();
			finiteAutomaton2.InputSymbols = new List<InputSymbol>
			{
				inputSymbol0,
				inputSymbol1
			};
			finiteAutomaton2.States = new List<State>
			{
				stateC,
				stateD,
				stateE
			};
			finiteAutomaton2.Transitions = new List<Transition>
			{
				new Transition { ActualState = stateC, InputSymbol = inputSymbol1, DestinationState = stateD },
				new Transition { ActualState = stateC, InputSymbol = inputSymbol0, DestinationState = stateC },
				new Transition { ActualState = stateD, InputSymbol = inputSymbol0, DestinationState = stateC },
                //new Transition { ActualState = stateE, InputSymbol = inputSymbol0, DestinationState = stateE },
            };
			finiteAutomaton2.IsDeterministic = true;
		}
    }
}