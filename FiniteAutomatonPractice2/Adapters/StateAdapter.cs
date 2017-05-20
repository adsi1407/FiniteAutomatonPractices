using Android.Views;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using FiniteAutomatonPractice.Core.Models;

namespace FiniteAutomatonPractice2.Adapters
{
    public class StateAdapter : BaseAdapter, ISpinnerAdapter
    {
        Context context;
        List<State> statesList;

        public StateAdapter(Context context, List<State> statesList)
        {
            this.context = context;
            this.statesList = statesList;
        }

        public override int Count
        {
            get
            {
                return statesList.Count;
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
            lblItemTitle.Text = statesList[position].Name;

            return view;
        }
    }
}