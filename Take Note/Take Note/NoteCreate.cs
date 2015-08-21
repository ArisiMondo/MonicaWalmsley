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
    public class NoteCreate : Activity
    {
        EditText title;
        TextView date;
        EditText body;
        Button save;
        DateTime dt;
        Button delete;
        String saveTitle;
        String saveBody;
        String saveDate;
        DatabaseManager dbManager = new DatabaseManager();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Note);
            title = FindViewById<EditText>(Resource.Id._editTextTitle);
            date = FindViewById<TextView>(Resource.Id._textViewDate1);
            body = FindViewById<EditText>(Resource.Id._editTextBody);
            
            dt = DateTime.Now;
            date.Text = dt.ToString("dd/MM/yyyy");

            save = FindViewById<Button>(Resource.Id._buttonSave);
            save.Click += Save_Click;

            delete = FindViewById<Button>(Resource.Id._buttonDelete);
            delete.Click += Delete_Click;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Are you sure you want to delete?");
            alert.SetPositiveButton("Yes", (s, ea) =>
            {
                Toast.MakeText(this, "Item deleted", ToastLength.Long).Show();
                this.Finish();
            });
            alert.SetNegativeButton("No", (s, ea) =>
            {
                return;
            });
            alert.Show();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (title.Text == "")
            {
                if (body.Text == "")
                {
                    this.Finish();
                }
                else
                {
                    saveTitle = "Note Taken";
                    saveBody = body.Text;
                    saveDate = date.Text;
                    dbManager.writeNote(saveTitle, saveDate, saveBody);
                    this.Finish();
                }
            }
            else
            {
                saveTitle = title.Text;
                saveBody = body.Text;
                saveDate = date.Text;
                dbManager.writeNote(saveTitle, saveDate, saveBody);
                this.Finish();
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
} //We're good