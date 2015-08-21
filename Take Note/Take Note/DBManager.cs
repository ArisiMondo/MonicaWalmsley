using System;
using System.IO;
using System.Collections.Generic;

namespace Take_Note
{
    public class DatabaseManager
    {
        static string dbName = "Notes.sqlite";
        string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

        public DatabaseManager()
        {
        }

        public List<ListInfo> ViewAll()
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath)) //-------------------Prepares connection to database
                {
                    var cmd = new SQLite.SQLiteCommand(conn); //----------------------------Prepares communication with database
                    cmd.CommandText = "Select NoteID, Title, Body, Dt from NOTES_TBL"; //---SQLite statement to pass back
                    var NoteList = cmd.ExecuteQuery<ListInfo>(); //-------------------------Reads information from db and prepares it in a list
                    return NoteList; //-----------------------------------------------------Passes list to data adapter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message); //--------------------------------Write error in english to output
                return null;
            }
        } //-------------------------------Gets and passes in all the notes

        public void writeNote(string title, string dt, string body)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath)) //--------------------------------------------------------------Prepares connection to database
                {
                    var cmd = new SQLite.SQLiteCommand(conn); //-----------------------------------------------------------------------Prepares communication with database
                    string sql = "Insert into NOTES_TBL (Title, Body, Dt) values ('" + title + "','" + body + "', '" + dt + "')"; //---SQLite query to pass back
                    cmd.CommandText = sql; //------------------------------------------------------------------------------------------Passes query to db
                    cmd.ExecuteNonQuery(); //------------------------------------------------------------------------------------------Writes information to db
                    ViewAll(); //------------------------------------------------------------------------------------------------------Gets a list of the notes and passes it in
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message); //---------------------------------------------------------------------------Write error in english to output
            }
        } //---Creates a new note

        public void editNote(string title, string body, string id)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath)) //----------------------------------------------------------Prepares connection to database
                {
                    var cmd = new SQLite.SQLiteCommand(conn); //-------------------------------------------------------------------Prepares communication with database
                    string sql = "UPDATE NOTES_TBL SET Title = '" + title + "', Body = '" + body + "' WHERE  NoteID = " + id; //---SQLite query to pass back
                    cmd.CommandText = sql; //--------------------------------------------------------------------------------------Passes query to db
                    cmd.ExecuteNonQuery(); //--------------------------------------------------------------------------------------Writes information to db
                    ViewAll(); //--------------------------------------------------------------------------------------------------Gets a list of the notes and passes it in
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message); //-----------------------------------------------------------------------Write error in english to output
            }
        } //----Makes changes to notes already stored

        public void deleteNote(string id)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath)) //-----------------Prepares connection to database
                {
                    var cmd = new SQLite.SQLiteCommand(conn); //--------------------------Prepares communication with database
                    string sql = "DELETE from main.NOTES_TBL WHERE  NoteID = " + id; //---SQLite query to pass back
                    cmd.CommandText = sql; //---------------------------------------------Passes query to db
                    cmd.ExecuteNonQuery(); //---------------------------------------------Writes information to db
                    ViewAll(); //---------------------------------------------------------Gets a list of the notes and passes it in
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message); //------------------------------Write error in english to output
            }
        } //-----------------------------Removes notes

        public List<ListInfo> ViewSearch(string query)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath)) //---Prepares connection to database
                {
                    var cmd = new SQLite.SQLiteCommand(conn); //------------Prepares communication with database
                    cmd.CommandText = "Select NoteID, Title, Body, Dt from NOTES_TBL where Title like '%" + query + "%' or Body like '%" + query + "%' or Dt like '%" + query + "%'"; //---------------------------SQLite query to pass back
                    var NoteList = cmd.ExecuteQuery<ListInfo>(); //---------Reads information from db and prepares it in a list
                    return NoteList; //-------------------------------------Passes list to data adapter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message); //----------------Write error in english to output
                return null;
            }
        } //----------------Gets and passes in any notes with title/bodies/dates referenced in the user's search
    }
}


