using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Obrazky.Model
{
    class Image : //InotifyPropertychange
    {
        private string source;

        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
            
        }




    }
}
