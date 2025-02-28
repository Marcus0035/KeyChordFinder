﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Data.Model
{
    public class Scale
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<IntervalScale> Intervals { get; set; } = new();
    }
}