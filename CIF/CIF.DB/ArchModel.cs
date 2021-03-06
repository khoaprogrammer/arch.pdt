﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIF.DB
{
    public enum ModelType
    {
        [Display(Name="Mô hình 3D")]
        MAX3D,
        [Display(Name = "Mô hình Sketchup")]
        SKETCHUP
    }

    public class ArchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DownloadLink { get; set; }
        public DateTime AddDate { get; set; }
        public string DESC { get; set; }
        public ModelType Type { get; set; }

       
    }
}