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
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "Select NoteID, Title, Body, Dt from NOTES_TBL";
                    var NoteList = cmd.ExecuteQuery<ListInfo>();
                    return NoteList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }


        public void writeNote(string title, string dt, string body)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    string sql = "Insert into NOTES_TBL (Title, Body, Dt) values ('" + title + "','" + body + "', '" + dt + "')"; 
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    ViewAll();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        public void editNote(string title, string body, string id)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    string sql = "UPDATE NOTES_TBL SET Title = '" + title + "', Body = '" + body + "' WHERE  NoteID = " + id;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    ViewAll();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        public void deleteNote(string id)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    string sql = "DELETE from main.NOTES_TBL WHERE  NoteID = " + id;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    //conn.Delete(cmd.CommandText);
                    ViewAll();              
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        public List<ListInfo> ViewSearch(string query)
        {
            try
            {
                using (var conn = new SQLite.SQLiteConnection(dbPath))
                {
                    var cmd = new SQLite.SQLiteCommand(conn);
                    cmd.CommandText = "Select NoteID, Title, Body, Dt from NOTES_TBL where Title like '%" + query + "%' or Body like '%" + query + "%' or Dt like '%" + query + "%'";
                    var NoteList = cmd.ExecuteQuery<ListInfo>();
                    return NoteList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }

    }
}


