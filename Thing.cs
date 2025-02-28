using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    public abstract class Thing
    {
        protected string Name { get; set; }
        protected int Quantity { get; set; }
        protected string Date { get; set; }

        public abstract void Read(string line);
        public abstract void Write();
    }
}
