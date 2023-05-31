using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalPlatform.Helpers {
    class RelayCommand : ICommand {
        private Action commandTask;
        private Predicate<object> canExecuteTask;
        private ICommand signIN;

        public RelayCommand(Action workToDo)
            : this(workToDo, DefaultCanExecute) {
            commandTask = workToDo;
        }

        public RelayCommand(Action workToDo, Predicate<object> canExecute) {
            commandTask = workToDo;
            canExecuteTask = canExecute;
        }

        public RelayCommand(ICommand signIN) {
            this.signIN=signIN;
        }

        private static bool DefaultCanExecute(object parameter) {
            return true;
        }

        public bool CanExecute(object parameter) {
            return canExecuteTask != null && canExecuteTask(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
            }

            remove {
                CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter) {
            commandTask();
        }
    }
}
