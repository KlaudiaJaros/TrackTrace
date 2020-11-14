using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    public class Location
    {
        private int id;
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
        public int GetId()
        {
            return this.id;
        }
        public void SetId(int i)
        {
            this.id = i;
        }
        public override string ToString()
        {
            return "Location: " + this.id + ", name: " + this.Name + ", address: " + this.Address + ", " + this.PostCode + ", " + this.Town;
        }
        // TODO: address can't have comas or it will cause problems
        public string ToCSV()
        {
            return this.id + "," + Name + "," + Address + "," + PostCode + "," + Town;
        }
    }
}
