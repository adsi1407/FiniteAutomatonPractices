using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomatonPractice1.Views
{
    [Activity(Label = "Automatas Finitos", MainLauncher = true, Icon = "@drawable/icon")]
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
        }

        private void BtnCreate_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateAutomatonFiniteActivity));
            StartActivity(intent);
        }

        private void BtnLoad_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("text/plain");
            if (intent.ResolveActivity(PackageManager) != null)
            {
                StartActivityForResult(intent, 0);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0 && resultCode == Result.Ok)
            {
                var fileName = data.Data.EncodedPath;
                if (File.Exists(data.Data.EncodedPath))
                {
                    var streamReader = new StreamReader(fileName);
                    var automatonText = streamReader.ReadToEnd();
                    try
                    {
                        List<string> autiomatonTextSplitted = automatonText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        string serializedInputSymbolsList = autiomatonTextSplitted[0];
                        string serializedStatesList = autiomatonTextSplitted[1];
                        string serializedTransitionsList = autiomatonTextSplitted[2];

                        var intent = new Intent(this, typeof(SummaryActivity));
                        intent.PutExtra("serializedInputSymbolsList", serializedInputSymbolsList);
                        intent.PutExtra("serializedStatesList", serializedStatesList);
                        intent.PutExtra("serializedTransitionsList", serializedTransitionsList);
                        StartActivity(intent);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(this, "El autómata no se encuentra en el formato requerido.", ToastLength.Long).Show();
                    }
                }
            }
        }
    }
}