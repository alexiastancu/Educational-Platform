using EducationalPlatform.Helpers;
using EducationalPlatform.Views.Administrator;
using EducationalPlatform.Views.Elev;
using EducationalPlatform.ViewsModels.Elev;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Administrator
{
    public class MenuAdministratorVM
    {    

        private ICommand _profesorCommnad;

        public ICommand ProfesorCommand
        {
            get
            {
                if (_profesorCommnad == null)
                {
                    _profesorCommnad = new RelayCommand(Profesor);
                }
                return _profesorCommnad;
            }
        }

        private void Profesor()
        {
            ProfesorEdit w = new ProfesorEdit();
            ProfesorEditVM vm = new ProfesorEditVM();
            w.DataContext = vm;            
            App.Current.MainWindow = w;
            w.Show();
        }


        private ICommand _elevCommnad;

        public ICommand ElevCommand
        {
            get
            {
                if (_elevCommnad == null)
                {
                    _elevCommnad = new RelayCommand(Elev);
                }
                return _elevCommnad;
            }
        }

        private void Elev()
        {
            ElevEdit w = new ElevEdit();
            ElevEditVM vm = new ElevEditVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _materiicommand;

        public ICommand MateriiCommand
        {
            get
            {
                if (_materiicommand == null)
                {
                    _materiicommand = new RelayCommand(Materie);
                }
                return _materiicommand;
            }
        }

        private void Materie()
        {
            MateriiEdit w = new MateriiEdit();
            MateriiEditVM vm = new MateriiEditVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _specializariCommand;

        public ICommand SpecializariCommand
        {
            get
            {
                if (_specializariCommand == null)
                {
                    _specializariCommand = new RelayCommand(Specializare);
                }
                return _specializariCommand;
            }
        }

        private void Specializare()
        {
            SpecializariEdit w = new SpecializariEdit();
            SpecializariEditVM vm = new SpecializariEditVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _claseCommand;
        public ICommand ClaseCommand
        {
            get
            {
                if (_claseCommand == null)
                {
                    _claseCommand = new RelayCommand(Clase);
                }
                return _claseCommand;
            }
        }
        private void Clase()
        {
            ClasaEdit w = new ClasaEdit();
            ClasaEditVM vm = new ClasaEditVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        private ICommand _utilizatoriCommand;

        public ICommand UtilizatoriCommand
        {
            get
            {
                if (_utilizatoriCommand == null)
                    _utilizatoriCommand = new RelayCommand(Utilizatori);
                return _utilizatoriCommand;
            }
        }

        public void Utilizatori()
        {            
            UtilizatoriEdit w = new UtilizatoriEdit();
            UtilizatorEditVM vm = new UtilizatorEditVM();
            w.DataContext = vm;   
            w.Show();
            App.Current.MainWindow = w;
        }
        

        private ICommand _anSpecMateriiCommand;
        public ICommand AnSpecMateriiCommand
        {
            get
            {
                if (_anSpecMateriiCommand == null)
                {
                    _anSpecMateriiCommand = new RelayCommand(OpenAnSpecMateriiWindow);
                }
                return _anSpecMateriiCommand;
            }
        }

        private void OpenAnSpecMateriiWindow()
        {
            AnSpecMateriiWindow w = new AnSpecMateriiWindow();
            AnSpecMateriiWindow vm = new AnSpecMateriiWindow();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }


        private ICommand _anSpeProfCommand;
        public ICommand AnSpecProfCommand
        {
            get
            {
                if (_anSpeProfCommand == null)
                {
                    _anSpeProfCommand = new RelayCommand(OpenAnSpecProfWindow);
                }
                return _anSpeProfCommand;
            }
        }
        private void OpenAnSpecProfWindow()
        {
            ProfMaterieClasaWindow w = new ProfMaterieClasaWindow();
            ProfMaterieClasaVM vm = new ProfMaterieClasaVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }
        private ICommand _elevAnSpecCommand;
        public ICommand ElevAnSpecCommand
        {
            get
            {
                if (_elevAnSpecCommand == null)
                {
                    _elevAnSpecCommand = new RelayCommand(OpenAnSpecEleviWindow);
                }
                return _elevAnSpecCommand;
            }
        }
        private void OpenAnSpecEleviWindow()
        {
            EleviAnStudSpecializWindow w = new EleviAnStudSpecializWindow();
            EleviAnStudSpecializVM vm = new EleviAnStudSpecializVM();
            w.DataContext = vm;
            App.Current.MainWindow = w;
            w.Show();
        }

        
    }
}
