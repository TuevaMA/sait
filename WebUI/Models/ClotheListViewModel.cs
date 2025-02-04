﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sait.domein.Entities;


namespace WebUI.Models
{
    public class ClotheListViewModel
    {
        public IEnumerable<Clothe> Clothes { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}