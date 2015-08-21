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
            base.OnCreate(bundle); //----------------------------------------------Declares as actions on opening of search
            SetContentView(Resource.Layout.viewSearch); //-------------------------Declares view type

            noteList = FindViewById<ListView>(Resource.Id._listViewNoteList); //---Gets list view
            noteList.ItemClick += NoteList_ItemClick; //---------------------------Action on list item clicked
            noteList.ItemLongClick += NoteList_ItemLongClick; //-------------------Action on list item held down
            notes = dbManager.ViewSearch(query); //--------------------------------Gets note list
            noteList.Adapter = new ListInfoAdapter(this, notes); //----------------Shows notes

            searchBar = FindViewById<EditText>(Resource.Id._editTextSearch); //----Gets search bar
            search = FindViewById<Button>(Resource.Id._buttonSearch); //-----------Gets search button
            search.Click += Search_Click; //---------------------------------------Action on search clicked
        } //------------------------------------Creates activity

        protected override void OnResume()
        {
            base.OnResume(); //---------------------------------------Declares as actions on resuming  search
            query = searchBar.Text; 
            notes = dbManager.ViewSearch(query); //-------------------Searches notes
            noteList.Adapter = new ListInfoAdapter(this, notes); //---Shows search results
        } //-------------------------------------------------Resumes activity

        void NoteList_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            OpenOptionsMenu();
            id = notes[e.Position].NoteID;
        } //---Action to open options menu

        void NoteList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(NoteEdit)); //---Declaring variables to pass to editing of notes
            intent.PutExtra("title", notes[e.Position].Title);
            intent.PutExtra("date", notes[e.Position].Dt);
            intent.PutExtra("body", notes[e.Position].Body);
            intent.PutExtra("id", notes[e.Position].NoteID);
            StartActivity(intent); //-----------------------------Opens note edit
        } //-----------Action to view note

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("Delete"); //-----------------------Adds item to options menu
            return base.OnPrepareOptionsMenu(menu);
        } //-------------------------------Creates options menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var itemTitle = item.TitleFormatted.ToString();
            switch (itemTitle.ToString())
            {
                case "Delete":
                    dbManager.deleteNote(id); //----------------------------------------Deletes the note
                    notes = dbManager.ViewSearch(query); //-----------------------------Refreshes note list
                    noteList.Adapter = new ListInfoAdapter(this, notes); //-------------Shows note list
                    Toast.MakeText(this, "Item deleted", ToastLength.Long).Show(); //---Tells user of success
                    break;
            }
            return base.OnOptionsItemSelected(item);
        } //-------------------------Action to delete note

        private void Search_Click(object sender, EventArgs e)
        {
            query = searchBar.Text;
            notes = dbManager.ViewSearch(query); //-------------------Searches notes
            noteList.Adapter = new ListInfoAdapter(this, notes); //---Shows notes
        } //------------------------------Action to search notes

        public override void OnBackPressed()
        {
            bool stopbackkey = true; //---------------------------------Stops app from leaving
            if (stopbackkey)
            {
                var alert = new AlertDialog.Builder(this); //-----------Creates a dialog asking user to confirm they are leaving
                alert.SetTitle("Are you sure you want to leave?");
                alert.SetPositiveButton("Yes", (s, ea) =>
                {
                    this.Finish(); //-----------------------------------if the user confirms then screen is closed
                });
                alert.SetNegativeButton("No", (s, ea) =>
                {
                    return; //------------------------------------------if the user cancels then everything continues
                });
                alert.Show(); //----------------------------------------Shows the dialog
            }
        } //-----------------------------------------------Action when user trys to leave
    }
}