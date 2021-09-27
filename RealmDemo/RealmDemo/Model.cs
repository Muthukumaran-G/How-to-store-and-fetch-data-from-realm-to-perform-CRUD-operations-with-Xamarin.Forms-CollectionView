using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealmDemo
{
    public class Model : RealmObject
    {
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public string BookAuthor { get; set; }
    }
}
