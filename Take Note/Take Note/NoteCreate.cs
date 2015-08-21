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
            base.OnCreate(bundle); //----------------------------------------Declares as actions on opening of main screen
            SetContentView(Resource.Layout.Note); //-------------------------Declares view type

            title = FindViewById<EditText>(Resource.Id._editTextTitle); //---Gets edit text
            date = FindViewById<TextView>(Resource.Id._textViewDate1); //----Gets text view
            body = FindViewById<EditText>(Resource.Id._editTextBody); //-----Gets edit text
            
            dt = DateTime.Now; //--------------------------------------------Sets date to current
            date.Text = dt.ToString("dd/MM/yyyy"); //------------------------Formats and shows date

            save = FindViewById<Button>(Resource.Id._buttonSave); //---------Gets save button
            save.Click += Save_Click; //-------------------------------------Action on save clicked

            delete = FindViewById<Button>(Resource.Id._buttonDelete); //-----Gets delete button
            delete.Click += Delete_Click; //---------------------------------Action on delete clicked
        } //---------Creates activity

        private void Delete_Click(object sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(this); //---------------------------Stops app from leaving
            alert.SetTitle("Are you sure you want to delete?");
            alert.SetPositiveButton("Yes", (s, ea) =>
            {
                Toast.MakeText(this, "Item deleted", ToastLength.Long).Show(); //---if the user confirms then tell them successful
                this.Finish(); //---------------------------------------------------and close screen
            });
            alert.SetNegativeButton("No", (s, ea) =>
            {
                return; //----------------------------------------------------------if the user cancels then everything continues
            });
            alert.Show(); //--------------------------------------------------------Shows the dialog
        } //---Action to delete note

        private void Save_Click(object sender, EventArgs e)
        {
            if (title.Text == "") //-----------------------------------------Checks if note has a title
            {
                if (body.Text == "") //--------------------------------------if not Check if note has a body
                {
                    this.Finish(); //----------------------------------------   if not Close note screen
                }
                else
                {
                    saveTitle = "Note Taken"; //-----------------------------   if yes Set title to generic
                    saveBody = body.Text;
                    saveDate = date.Text;
                    dbManager.writeNote(saveTitle, saveDate, saveBody); //---   pass note values to database
                    this.Finish(); //----------------------------------------   close note screen
                }
            }
            else
            {
                saveTitle = title.Text; //-----------------------------------if yes set values
                saveBody = body.Text;
                saveDate = date.Text;
                dbManager.writeNote(saveTitle, saveDate, saveBody); //-------pass note values to database
                this.Finish(); //--------------------------------------------close note screen
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
        } //--------------------Action when user trys to leave
    }
} //We're good