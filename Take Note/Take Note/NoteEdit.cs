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

namespace Take_Note //We're good
{
    [Activity(Label = "Take Note", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class NoteEdit : Activity
    {
        EditText title;
        TextView date;
        EditText body;
        string saveTitle = "";
        string saveBody = "";
        string saveDate = "";
        string saveID = "";
        Button save;
        Button delete;
        DatabaseManager dbManager = new DatabaseManager();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Note);
            title = FindViewById<EditText>(Resource.Id._editTextTitle);
            date = FindViewById<TextView>(Resource.Id._textViewDate1);
            body = FindViewById<EditText>(Resource.Id._editTextBody);
            saveTitle = Intent.GetStringExtra("title") ?? "No Data";
            saveDate = Intent.GetStringExtra("date") ?? "No Data";
            saveBody = Intent.GetStringExtra("body") ?? "No Data";
            saveID = Intent.GetStringExtra("id") ?? "0";
            title.Text = saveTitle;
            body.Text = saveBody;
            date.Text = saveDate;

            save = FindViewById<Button>(Resource.Id._buttonSave);
            save.Click += SaveEdit_Click;

            delete = FindViewById<Button>(Resource.Id._buttonDelete);
            delete.Click += DeleteNote_Click;
        }


        private void DeleteNote_Click(object sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Are you sure you want to delete?");
            alert.SetPositiveButton("Yes", (s, ea) =>
            {
                dbManager.deleteNote(saveID);
                Toast.MakeText(this, "Item deleted", ToastLength.Long).Show();
                this.Finish();
            });
            alert.SetNegativeButton("No", (s, ea) =>
            {
                return;
            });
            alert.Show();
        }


        private void SaveEdit_Click(object sender, EventArgs e)
        {
            if (title.Text == saveTitle && body.Text == saveBody)
            {
                this.Finish();
            }
            else
            {
                dbManager.editNote(title.Text, body.Text, saveID);
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
}