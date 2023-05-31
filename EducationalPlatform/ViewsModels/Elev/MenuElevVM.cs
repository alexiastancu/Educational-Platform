using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using EducationalPlatform.Views.Administrator;
using EducationalPlatform.ViewsModels.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Elev
{
    public class MenuElevVM
    {
        int id;
        public MenuElevVM(int id)
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
            Views.Elev.MaterialeDidactice w = new Views.Elev.MaterialeDidactice();
            ViewsModels.Elev.MaterialeDidacticeVM vm = new ViewsModels.Elev.MaterialeDidacticeVM();
            w.DataContext = vm;
            App.Current.MainWindow.Close();
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
            Views.Elev.Absente w = new Views.Elev.Absente();
            ViewsModels.Elev.AbsenteVM vm = new ViewsModels.Elev.AbsenteVM();
            w.DataContext = vm;
            App.Current.MainWindow.Close();
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
            Views.Elev.Note w = new Views.Elev.Note();
            ViewsModels.Elev.NoteVM vm = new ViewsModels.Elev.NoteVM();
            w.DataContext = vm;
            App.Current.MainWindow.Close();
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
            Views.Elev.Medii w = new Views.Elev.Medii();
            ViewsModels.Elev.MediiVM vm = new ViewsModels.Elev.MediiVM();
            w.DataContext = vm;
            App.Current.MainWindow.Close();
            App.Current.MainWindow = w;
            w.Show();
        }
    }
}
