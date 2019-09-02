using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddDelSRP
{
    class RegValue {
        public string Path { get; set; }
        public string Description { get; set; }
        public string ItemData { get; set; }
        public string LastModified { get;set; }
        public string SaferFlags { get; set; }

       public RegValue(string path, string description, string itemdata, string lastmodified, string saferflags) {
            Path = path;
            Description = description;
            ItemData = itemdata;
            LastModified = lastmodified;
            SaferFlags = saferflags;
        }
    }
}
