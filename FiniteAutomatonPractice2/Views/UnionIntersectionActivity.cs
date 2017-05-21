using Android.App;
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

        AutomatonOperations automatonOperations;
        StringOperations stringOperations;

        AlertDialog.Builder builder;
        AlertDialog alertDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.union_intersection_activity);

            serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");

            finiteAutomaton1 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton1);
            finiteAutomaton2 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton);

            btnUnion = FindViewById<Button>(Resource.Id.btnUnion);
            btnUnion.Click += BtnUnion_Click;
            btnIntersection = FindViewById<Button>(Resource.Id.btnIntersection);
            btnIntersection.Click += BtnIntersection_Click;

            automatonOperations = new AutomatonOperations();
            stringOperations = new StringOperations();
        }

        private void BtnIntersection_Click(object sender, System.EventArgs e)
        {
            if (automatonOperations.EqualInputSymbols(finiteAutomaton1, finiteAutomaton2))
            {
                finiteAutomatonResult = automatonOperations.JoinFiniteAutomatons(finiteAutomaton1, finiteAutomaton2);
                ShowAutomatonResultDialog();
            }
            else
            {
                Toast.MakeText(this, "Para unir dos autómatas finitos, éstos deben tener los mismos símbolos de entrada.", ToastLength.Short).Show();
            }
        }

        private void BtnUnion_Click(object sender, System.EventArgs e)
        {
            if (automatonOperations.EqualInputSymbols(finiteAutomaton1, finiteAutomaton2))
            {
                finiteAutomatonResult = automatonOperations.IntersectFiniteAutomatons(finiteAutomaton1, finiteAutomaton2);
                ShowAutomatonResultDialog();
            }
            else
            {
                Toast.MakeText(this, "Para intersectar dos autómatas finitos, éstos deben tener los mismos símbolos de entrada.", ToastLength.Short).Show();
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
    }
}