using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;


namespace Take_Note
{
    public class ListInfoAdapter : BaseAdapter<ListInfo>
    {
        List<ListInfo> items;
        Activity context;

        public ListInfoAdapter(Activity context, List<ListInfo> items)
            : base()
        {
            this.context = context;
            this.items = items;
        } //-------------------Gets values

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ListInfo this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
           get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);

            view.FindViewById<TextView>(Resource.Id._textViewID).Text = item.NoteID;
            view.FindViewById<TextView>(Resource.Id._textViewTitle).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id._textViewBody).Text = item.Body;
            view.FindViewById<TextView>(Resource.Id._textViewDate2).Text = item.Dt;

            return view;
        } //---Passes note list to list view

    }
}
