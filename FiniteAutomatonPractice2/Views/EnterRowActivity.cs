using Android.App;
using Android.OS;
using Android.Widget;
using FiniteAutomatonPractice.Core.Models;
using FiniteAutomatonPractice.Core.Utils;
using Newtonsoft.Json;

namespace FiniteAutomatonPractice2
{
	[Activity(Label = "Ingresar Hilera")]
	public class EnterRowActivity: Activity
	{
		EditText txtRow;
		Button btnTestRow;
		string serializedAutomaton;
		FiniteAutomaton finiteAutomaton;
		AlertDialog.Builder builder;
		AlertDialog alertDialog;
		AutomatonOperations automatonOperations;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.enter_row_activity);

			serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
			finiteAutomaton = JsonConvert.DeserializeObject<FiniteAutomaton>(serializedAutomaton);

			txtRow = FindViewById<EditText>(Resource.Id.txtRow);
			btnTestRow = FindViewById<Button>(Resource.Id.btnTestRow);
			btnTestRow.Click += BtnTestRow_Click;

			automatonOperations = new AutomatonOperations();
		}

		private void BtnTestRow_Click(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtRow.Text))
			{
				var result = automatonOperations.TestRow(finiteAutomaton, txtRow.Text.Trim());
				ShowAutomatonResultDialog(string.Format("Estado Resultante: {0}, Aceptación: {1}", result.Name, result.Acceptance.ToString()));
			}
			else
			{
				Toast.MakeText(this, "Por favor ingresa una hilera para probar el autómata finito", ToastLength.Short).Show();
			}
		}

		private void ShowAutomatonResultDialog(string message)
		{
			builder = new AlertDialog.Builder(this);
			alertDialog = builder.Create();
			alertDialog.SetTitle("Resultado");
			alertDialog.SetMessage(message);
			alertDialog.Show();
		}
	}
}
