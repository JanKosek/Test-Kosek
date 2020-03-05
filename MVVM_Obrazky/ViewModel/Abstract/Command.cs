using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_Obrazky.ViewModel.Abstract
{
    
        class Command : ICommand
        {
        
        Action<object> executeMethod;
        Func<object, bool> canexecuteMethod;
        public event EventHandler CanExecuteChanged;
        public Command(Action<object> executeMethod)
        {
            this.executeMethod = executeMethod;
         
        }

        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        
        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }
  

    }
    
}
