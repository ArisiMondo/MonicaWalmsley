using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Take_Note
{
    [Activity(Label = "Take Note", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class ListSearch : Activity
    {
        ListView noteList;
        List<ListInfo> notes;
        Button search;
        EditText searchBar;
        DatabaseManager dbManager = new DatabaseManager();
        string query;
        string id;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.viewSearch);

            noteList = FindViewById<ListView>(Resource.Id._listViewNoteList);
            noteList.ItemClick += NoteList_ItemClick;
            noteList.ItemLongClick += NoteList_ItemLongClick;

            notes = dbManager.ViewSearch(query);
            noteList.Adapter = new ListInfoAdapter(this, notes);

            searchBar = FindViewById<EditText>(Resource.Id._editTextSearch);
            search = FindViewById<Button>(Resource.Id._buttonSearch);
            search.Click += Search_Click;
            // Create your application here
        }


        protected override void OnResume()
        {
            base.OnResume();
            query = searchBar.Text;
            notes = dbManager.ViewSearch(query);
            noteList.Adapter = new ListInfoAdapter(this, notes);
        }


        void NoteList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            OpenOptionsMenu();
            id = notes[e.Position].NoteID;
        }


        void NoteList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(NoteEdit));
            intent.PutExtra("title", notes[e.Position].Title);
            intent.PutExtra("date", notes[e.Position].Dt);
            intent.PutExtra("body", notes[e.Position].Body);
            intent.PutExtra("id", notes[e.Position].NoteID);
            StartActivity(intent);
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("Delete");
            return base.OnPrepareOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var itemTitle = item.TitleFormatted.ToString();
            switch (itemTitle.ToString())
            {
                case "Delete":
                    dbManager.deleteNote(id);
                    notes = dbManager.ViewSearch(query);
                    noteList.Adapter = new ListInfoAdapter(this, notes);
                    Toast.MakeText(this, "Item deleted", ToastLength.Long).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }


        private void Search_Click(object sender, EventArgs e)
        {
            query = searchBar.Text;
            notes = dbManager.ViewSearch(query);
            noteList.Adapter = new ListInfoAdapter(this, notes);
        }


        public override void OnBackPressed()
        {

            bool stopbackkey = true;

            if (stopbackkey)
            {
                var alert = new AlertDialog.Builder(this);
                alert.SetTitle("Are you sure you want to leave?");
                alert.SetPositiveButton("Yes", (s, ea) =>
                {
                    this.Finish();
                });
                alert.SetNegativeButton("No", (s, ea) =>
                {
                    return;
                });
                alert.Show();
            }
        }
    }
}