using Android.Views;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using FiniteAutomatonPractice.Core.Models;

namespace FiniteAutomatonPractice1.Adapters
{
    public class InputSymbolAdapter : BaseAdapter, ISpinnerAdapter
    {
        Context context;
        List<InputSymbol> inputSymbolsList;

        public InputSymbolAdapter(Context context, List<InputSymbol> inputSymbolsList)
        {
            this.context = context;
            this.inputSymbolsList = inputSymbolsList;
        }

        public override int Count
        {
            get
            {
                return inputSymbolsList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var inflater = LayoutInflater.From(context);
            var view = inflater.Inflate(Resource.Layout.state_spinner_item, parent, false);

            TextView lblItemTitle = view.FindViewById<TextView>(Resource.Id.lblItemTitle);
            lblItemTitle.Text = inputSymbolsList[position].Name;

            return view;
        }
    }
}