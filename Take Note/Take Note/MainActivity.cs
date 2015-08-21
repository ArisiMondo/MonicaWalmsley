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
        string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);
        ListView noteList;
        List<ListInfo> notes;
        Button createNote;
        Button search;
        ImageView appName;
        DatabaseManager dbManager = new DatabaseManager();
        string id;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            noteList = FindViewById<ListView>(Resource.Id._listViewNoteList);
            CopyDatabase();
            notes = dbManager.ViewAll();

            noteList.Adapter = new ListInfoAdapter(this, notes);
            noteList.ItemClick += NoteList_ItemClick;
            noteList.ItemLongClick += NoteList_ItemLongClick;

            createNote = FindViewById<Button>(Resource.Id._buttonCreate);
            createNote.Click += CreateNote_Click;

            search = FindViewById<Button>(Resource.Id._buttonSearch);
            search.Click += Search_Click;

            appName = FindViewById<ImageView>(Resource.Id._imageAppName);
        }


        private void Search_Click(object sender, EventArgs e)
        {
           StartActivity(typeof(ListSearch));
        }


        protected override void OnResume()
        {
            base.OnResume();
            notes = dbManager.ViewAll();
            noteList.Adapter = new ListInfoAdapter(this, notes);
        }


        private void CreateNote_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(NoteCreate));
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
                    notes = dbManager.ViewAll();
                    noteList.Adapter = new ListInfoAdapter(this, notes);
                    Toast.MakeText(this, "Item deleted", ToastLength.Long).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }


        public void CopyDatabase()
        {
            if (!File.Exists(dbPath))
            {
                using (BinaryReader br = new BinaryReader(Assets.Open(dbName)))
                {
                    try
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
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
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                }
            }
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

