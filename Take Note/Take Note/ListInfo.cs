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
    public class ListInfo
    {
        public string NoteID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Dt { get; set; }

        public ListInfo()
        {
        }
    }
}