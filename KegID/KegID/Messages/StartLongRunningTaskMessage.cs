﻿using KegID.Model;
using KegID.Services;
using System.Collections.Generic;

namespace KegID.Messages
{
    public class StartLongRunningTaskMessage
    {
        public IList<string> Barcode { get; set; }
        public string Page { get; set; }
    }
}
