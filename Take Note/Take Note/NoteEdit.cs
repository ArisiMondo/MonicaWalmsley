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
            base.OnCreate(bundle); //----------------------------------------Declares as actions on opening of main screen
            SetContentView(Resource.Layout.Note); //-------------------------Declares view type

            title = FindViewById<EditText>(Resource.Id._editTextTitle); //---Gets edit text
            date = FindViewById<TextView>(Resource.Id._textViewDate1); //----Gets text view
            body = FindViewById<EditText>(Resource.Id._editTextBody); //-----Gets edit text

            saveTitle = Intent.GetStringExtra("title") ?? "No Data"; //------Receives values from intent
            saveDate = Intent.GetStringExtra("date") ?? "No Data";
            saveBody = Intent.GetStringExtra("body") ?? "No Data";
            saveID = Intent.GetStringExtra("id") ?? "0";

            title.Text = saveTitle;
            body.Text = saveBody;
            date.Text = saveDate;

            save = FindViewById<Button>(Resource.Id._buttonSave); //---------Gets save button
            save.Click += SaveEdit_Click; //---------------------------------Action on save clicked

            delete = FindViewById<Button>(Resource.Id._buttonDelete); //-----Gets delete button
            delete.Click += DeleteNote_Click; //-----------------------------Action on delete clicked
        } //-------------Creates activity

        private void DeleteNote_Click(object sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(this); //---------------------------Stops app from leaving
            alert.SetTitle("Are you sure you want to delete?");
            alert.SetPositiveButton("Yes", (s, ea) =>
            {
                dbManager.deleteNote(saveID); //------------------------------------if the user confirms delete note
                Toast.MakeText(this, "Item deleted", ToastLength.Long).Show(); //---tell them successful
                this.Finish(); //---------------------------------------------------and close screen
            });
            alert.SetNegativeButton("No", (s, ea) =>
            {
                return; //----------------------------------------------------------if the user cancels then everything continues
            });
            alert.Show(); //--------------------------------------------------------Shows the dialog
        } //---Action to delete note

        private void SaveEdit_Click(object sender, EventArgs e)
        {
            if (title.Text == saveTitle && body.Text == saveBody) //----Checks if the note has been changed
            {
                this.Finish(); //---------------------------------------if not Close screen
            }
            else
            {
                dbManager.editNote(title.Text, body.Text, saveID); //---if yes Pass values to database
                this.Finish(); //---------------------------------------Close screen
            }
        } //-----Action to save note

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
        } //------------------------Action when user trys to leave
    }
}