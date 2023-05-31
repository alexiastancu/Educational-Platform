using EducationalPlatform.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Profesor
{
    internal class MenuProfesorVM
    {
        int id;
        public MenuProfesorVM(int id)
        {
            this.id = id;
        }
        private ICommand _materialeDidacticeCommand;

        public ICommand MaterialeDidacticeCommand
        {
            get
            {
                if (_materialeDidacticeCommand == null)
                    _materialeDidacticeCommand = new RelayCommand(MaterialeDidactice);
                return _materialeDidacticeCommand;
            }
        }

        public void MaterialeDidactice()
        {
            Views.Profesor.MaterialeDidactice w = new Views.Profesor.MaterialeDidactice();
            ViewsModels.Profesor.MaterialeDidacticeVM vm = new ViewsModels.Profesor.MaterialeDidacticeVM(id);
            w.DataContext = vm;            
            App.Current.MainWindow = w;
            w.Show();
        }
        private ICommand _absenteCommand;

        public ICommand AbsenteCommand
        {
            get
            {
                if (_absenteCommand == null)
                    _absenteCommand = new RelayCommand(Absente);
                return _absenteCommand;
            }
        }

        public void Absente()
        {
            Views.Profesor.Absente w = new Views.Profesor.Absente();
            ViewsModels.Profesor.AbsenteVM vm = new ViewsModels.Profesor.AbsenteVM(id);
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _noteCommand;

        public ICommand NoteCommand
        {
            get
            {
                if (_noteCommand == null)
                    _noteCommand = new RelayCommand(Note);
                return _noteCommand;
            }
        }

        public void Note()
        {
            Views.Profesor.Note w = new Views.Profesor.Note();
            ViewsModels.Profesor.NoteVM vm = new ViewsModels.Profesor.NoteVM(id);
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _mediiCommand;

        public ICommand MediiCommand
        {
            get
            {
                if (_mediiCommand == null)
                    _mediiCommand = new RelayCommand(Medii);
                return _mediiCommand;
            }
        }

        public void Medii()
        {
            Views.Profesor.Medii w = new Views.Profesor.Medii();
            ViewsModels.Profesor.MediiVM vm = new ViewsModels.Profesor.MediiVM(id);
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }
    }
}
