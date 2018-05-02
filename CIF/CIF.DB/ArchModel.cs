using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.DB
{
    public enum ModelType
    {
        MAX3D,
        SKETCHUP
    }

    public class ArchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DownloadLink { get; set; }
        public ModelType Type { get; set; }
    }
}