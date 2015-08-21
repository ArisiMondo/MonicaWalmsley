using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Take_Note
{
    [Activity(Label = "Take Note", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class MainActivity : Activity
        {
        static string dbName = "Notes.sqlite";
        string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName); //---Connection to db
        ListView noteList;
        List<ListInfo> notes;
        Button createNote;
        Button search;
        ImageView appName;
        DatabaseManager dbManager = new DatabaseManager();
        string id;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle); //----------------------------------------------Declares as actions on opening of main screen
            SetContentView(Resource.Layout.Main); //-------------------------------Declares view type
            CopyDatabase(); //-----------------------------------------------------Copies database if not already on device

            noteList = FindViewById<ListView>(Resource.Id._listViewNoteList); //---Gets list view
            notes = dbManager.ViewAll(); //----------------------------------------Gets note list
            noteList.Adapter = new ListInfoAdapter(this, notes); //----------------Shows notes
            noteList.ItemClick += NoteList_ItemClick; //---------------------------Action on list item clicked
            noteList.ItemLongClick += NoteList_ItemLongClick; //-------------------Action on list item held down

            createNote = FindViewById<Button>(Resource.Id._buttonCreate); //-------Gets create note button
            createNote.Click += CreateNote_Click; //-------------------------------Action on create clicked

            search = FindViewById<Button>(Resource.Id._buttonSearch); //-----------Gets search button
            search.Click += Search_Click; //---------------------------------------Action on search clicked

            appName = FindViewById<ImageView>(Resource.Id._imageAppName);
        } //------------------------------------Creates activity

        private void Search_Click(object sender, EventArgs e)
        {
           StartActivity(typeof(ListSearch)); //---Open search screen
        } //------------------------------Action to search notes

        protected override void OnResume()
        {
            base.OnResume(); //---------------------------------------Declares as actions on resuming main screen
            notes = dbManager.ViewAll(); //---------------------------Refreshes notes
            noteList.Adapter = new ListInfoAdapter(this, notes); //---Shows search results
        } //-------------------------------------------------Resumes activity

        private void CreateNote_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(NoteCreate)); //---Opens new note screen
        } //--------------------------Action to create note

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
                    notes = dbManager.ViewAll(); //-----------------------------Refreshes note list
                    noteList.Adapter = new ListInfoAdapter(this, notes); //-------------Shows note list
                    Toast.MakeText(this, "Item deleted", ToastLength.Long).Show(); //---Tells user of success
                    break;
            }
            return base.OnOptionsItemSelected(item);
        } //-------------------------Action to delete note

        public void CopyDatabase()
        {
            if (!File.Exists(dbPath)) //-------------------------------------------------------------------------Check if file exists
            {
                using (BinaryReader br = new BinaryReader(Assets.Open(dbName))) //-------------------------------if not opens database
                {
                    try
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create))) //---writes file to phone
                        {
                            byte[] buffer = new byte[2048];
                            int len = 0;
                            while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, len);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e); //----------------------------------------write error to output
                    }
                }
            }
        } //---------------------------------------------------------Copies database if not already on device

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

