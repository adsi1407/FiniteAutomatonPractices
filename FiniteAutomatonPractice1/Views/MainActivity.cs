using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace FiniteAutomatonPractice1.Views
{
    [Activity(Label = "Menu", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button btnCreate;
        Button btnLoad;
        Button btnUpdate;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            btnCreate.Click += BtnCreate_Click;
            btnLoad = FindViewById<Button>(Resource.Id.btnLoad);
            btnLoad.Click += BtnLoad_Click;
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);
            btnUpdate.Click += BtnUpdate_Click;
        }

        private void BtnCreate_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateAutomatonFiniteActivity));
            StartActivity(intent);
        }

        private void BtnLoad_Click(object sender, System.EventArgs e)
        {
        }

        private void BtnUpdate_Click(object sender, System.EventArgs e)
        {
        }
    }
}