using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Obrazky.Model
{
    class Buttons
    {

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            
            set
            {
                id = value;
            }
        }

        private string odkaz;
        public string Odkaz
        {
            get
            {
                return odkaz;
              
            }
            set
            {
                odkaz = value;
            }
        }
    }
}
