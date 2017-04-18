using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace FiniteAutomatonPractice1.Views
{
    [Activity(Label = "Crear AF")]
    public class CreateAutomatonFiniteActivity: Activity
    {
        Button btnSaveInputSymbols;
        Button btnSaveStates;
        Button btnTransitions;
        EditText txtInputSymbols;
        EditText txtStates;
        TextView lblInputSymbols;
        TextView lblStates;
        CheckBox checkIsAceptance;
        List<InputSymbol> inputSymbolsList;
        List<State> statesList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.create_automaton_activity);

            btnSaveInputSymbols = FindViewById<Button>(Resource.Id.btnSaveInputSymbols);
            btnSaveInputSymbols.Click += BtnSaveInputSymbols_Click;
            btnSaveStates = FindViewById<Button>(Resource.Id.btnSaveStates);
            btnSaveStates.Click += BtnSaveStates_Click;
            btnTransitions = FindViewById<Button>(Resource.Id.btnTransitions);
            btnTransitions.Click += BtnTransitions_Click;

            txtInputSymbols = FindViewById<EditText>(Resource.Id.txtInputSymbols);
            txtStates = FindViewById<EditText>(Resource.Id.txtStates);
            lblInputSymbols = FindViewById<TextView>(Resource.Id.lblInputSymbols);
            lblStates = FindViewById<TextView>(Resource.Id.lblStates);
            checkIsAceptance = FindViewById<CheckBox>(Resource.Id.checkIsAceptance);

            inputSymbolsList = new List<InputSymbol>();
            statesList = new List<State>();
        }

        private void BtnSaveInputSymbols_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInputSymbols.Text))
            {
                var inputSymbol = inputSymbolsList.Find(x => x.Name == txtInputSymbols.Text);
                if (inputSymbol == null)
                {
                    inputSymbolsList.Add(new InputSymbol { Name = txtInputSymbols.Text.Trim() });

                    lblInputSymbols.Text = ShowInputSymbols();
                    lblInputSymbols.Visibility = Android.Views.ViewStates.Visible;

                    txtInputSymbols.Text = string.Empty;
                }
                else
                {
                    Toast.MakeText(this, "este símbolo de entrada ya existe", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Debes ingresar el símbolo de entrada", ToastLength.Short).Show();
            }
        }

        private void BtnSaveStates_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtStates.Text))
            {
                var state = statesList.Find(x => x.Name == txtStates.Text);
                if (state == null)
                {
                    statesList.Add(new State { Name = txtStates.Text, Acceptance = checkIsAceptance.Checked });

                    lblStates.Text = ShowStates();
                    lblStates.Visibility = Android.Views.ViewStates.Visible;

                    txtStates.Text = string.Empty;
                    checkIsAceptance.Checked = false;
                }
                else
                {
                    Toast.MakeText(this, "este estado ya existe", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Debes ingresar el estado", ToastLength.Short).Show();
            }
        }

        private void BtnTransitions_Click(object sender, System.EventArgs e)
        {
            string serializedInputSymbolsList = JsonConvert.SerializeObject(inputSymbolsList);
            string serializedStatesList = JsonConvert.SerializeObject(statesList);

            var intent = new Intent(this, typeof(TransitionsActivity));
            intent.PutExtra("serializedInputSymbolsList", serializedInputSymbolsList);
            intent.PutExtra("serializedStatesList", serializedStatesList);
            StartActivity(intent);
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
    }
}