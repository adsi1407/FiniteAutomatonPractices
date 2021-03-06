﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Automatas Finitos", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button btnCreate;
        Button btnLoad;
        Button btnUnionIntersection;
        string serializedAutomaton;
        string serializedAutomaton1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            btnCreate.Click += BtnCreate_Click;
            btnLoad = FindViewById<Button>(Resource.Id.btnLoad);
            btnLoad.Click += BtnLoad_Click;
            btnUnionIntersection = FindViewById<Button>(Resource.Id.btnUnionIntersection);
            btnUnionIntersection.Click += BtnUnionIntersection_Click;

            serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");
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

        private void BtnUnionIntersection_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(serializedAutomaton1) && !string.IsNullOrEmpty(serializedAutomaton))
            {
                var intent = new Intent(this, typeof(UnionIntersectionActivity));
                intent.PutExtra("serializedAutomaton", serializedAutomaton);
                intent.PutExtra("serializedAutomaton1", serializedAutomaton1);
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Primero debes crear o cargar los autómatas finitos.", ToastLength.Long).Show();
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
                        
                        serializedAutomaton1 = autiomatonTextSplitted[0];
                        serializedAutomaton = autiomatonTextSplitted[1];

                        var intent = new Intent(this, typeof(LoadSummaryActivity));
                        intent.PutExtra("serializedAutomaton1", serializedAutomaton1);
                        intent.PutExtra("serializedAutomaton2", serializedAutomaton);
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