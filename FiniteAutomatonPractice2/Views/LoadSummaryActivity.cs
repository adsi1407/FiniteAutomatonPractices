using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using Newtonsoft.Json;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Resumen")]
    public class LoadSummaryActivity : Activity
    {
        string serializedAutomaton1;
        string serializedAutomaton2;

        FiniteAutomaton finiteAutomaton1;
        FiniteAutomaton finiteAutomaton2;

        TextView lblInputSymbols1;
        TextView lblStates1;
        TextView lblTransitions1;
        TextView txtAutomatonType1;
        TextView lblInputSymbols2;
        TextView lblStates2;
        TextView lblTransitions2;
        TextView txtAutomatonType2;

        StringOperations stringOperations;
        AutomatonOperations automatonOperations;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.load_summary_activity);

            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");
            serializedAutomaton2 = Intent.GetStringExtra("serializedAutomaton2");

            finiteAutomaton1 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton1);
            finiteAutomaton2 = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton2);

            lblInputSymbols1 = FindViewById<TextView>(Resource.Id.lblInputSymbols1);
            lblStates1 = FindViewById<TextView>(Resource.Id.lblStates1);
            lblTransitions1 = FindViewById<TextView>(Resource.Id.lblTransitions1);
            txtAutomatonType1 = FindViewById<TextView>(Resource.Id.txtAutomatonType1);
            lblInputSymbols2 = FindViewById<TextView>(Resource.Id.lblInputSymbols2);
            lblStates2 = FindViewById<TextView>(Resource.Id.lblStates2);
            lblTransitions2 = FindViewById<TextView>(Resource.Id.lblTransitions2);
            txtAutomatonType2 = FindViewById<TextView>(Resource.Id.txtAutomatonType2);

            stringOperations = new StringOperations();
            automatonOperations = new AutomatonOperations();

            lblInputSymbols1.Text = stringOperations.ShowInputSymbols(finiteAutomaton1.InputSymbols);
            lblStates1.Text = stringOperations.ShowStates(finiteAutomaton1.States);
            lblTransitions1.Text = stringOperations.ShowTransitions(finiteAutomaton1.Transitions);
            bool isDeterministic = automatonOperations.IsDeterministic(finiteAutomaton1.InputSymbols, finiteAutomaton1.States, finiteAutomaton1.Transitions);
            if (isDeterministic)
            {
                txtAutomatonType1.Text = "Determinístico";
            }
            else
            {
                txtAutomatonType1.Text = "No determinístico";
            }

            lblInputSymbols2.Text = stringOperations.ShowInputSymbols(finiteAutomaton2.InputSymbols);
            lblStates2.Text = stringOperations.ShowStates(finiteAutomaton2.States);
            lblTransitions2.Text = stringOperations.ShowTransitions(finiteAutomaton2.Transitions);
            isDeterministic = automatonOperations.IsDeterministic(finiteAutomaton2.InputSymbols, finiteAutomaton2.States, finiteAutomaton2.Transitions);
            if (isDeterministic)
            {
                txtAutomatonType2.Text = "Determinístico";
            }
            else
            {
                txtAutomatonType2.Text = "No determinístico";
            }
        }
    }
}