﻿using System;

namespace EDC.SearchEngine.Model
{
    [Serializable()]
    public class Article
    {
        public long Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Msg
        {
            get;
            set;
        }
    }
}
