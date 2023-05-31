//using EducationalPlatform.Models;
//using EducationalPlatform.Models.BusinessLogicLayer;
//using EducationalPlatform.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data.Entity;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Runtime.Remoting.Channels;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;

//namespace EducationalPlatform.ViewsModels.Administrator
//{
//    public class AnSpecMateriiVM : BaseVM
//    {
//        private string _selectedYear;
//        private string _selectedSpecializare;
//        private string _selectedMaterie;

//        public string SelectedYear
//        {
//            get { return _selectedYear; }
//            set
//            {
//                if (_selectedYear != value)
//                {
//                    _selectedYear = value;
//                    OnPropertyChanged(nameof(SelectedYear));
//                }
//            }
//        }

//        public string SelectedSpecializare
//        {
//            get { return _selectedSpecializare; }
//            set
//            {
//                if (_selectedSpecializare != value)
//                {
//                    _selectedSpecializare = value;
//                    OnPropertyChanged(nameof(SelectedSpecializare));
//                }
//            }
//        }

//        public string SelectedMaterie
//        {
//            get { return _selectedMaterie; }
//            set
//            {
//                if (_selectedMaterie != value)
//                {
//                    _selectedMaterie = value;
//                    OnPropertyChanged(nameof(SelectedMaterie));
//                }
//            }
//        }

//        private ObservableCollection<GetMaterii_Result> _materii;
//        private ObservableCollection<GetAllSpecializari_Result> _specializari;

//        public ObservableCollection<GetMaterii_Result> Materii
//        {
//            get { return _materii; }
//            set
//            {
//                if (_materii != value)
//                {
//                    _materii = value;
//                    OnPropertyChanged(nameof(Materii));
//                }
//            }
//        }

//        public ObservableCollection<GetAllSpecializari_Result> Specializari
//        {
//            get { return _specializari; }
//            set
//            {
//                if (_specializari != value)
//                {
//                    _specializari = value;
//                    OnPropertyChanged(nameof(Specializari));
//                }
//            }
//        }

//        private ObservableCollection<AsocieriAnSpecMaterie> asocieriAnSpecializareMaterie;
//        public ObservableCollection<AsocieriAnSpecMaterie> AsocieriAnSpecializareMaterie
//        {
//            get { return asocieriAnSpecializareMaterie; }
//            set
//            {
//                asocieriAnSpecializareMaterie = value;
//                OnPropertyChanged(nameof(AsocieriAnSpecializareMaterie));
//            }
//        }

//        private AsocieriAnSpecMaterie selectedAsociere;
//        public AsocieriAnSpecMaterie SelectedAsociere
//        {
//            get { return selectedAsociere; }
//            set
//            {
//                selectedAsociere = value;
//                OnPropertyChanged(nameof(SelectedAsociere));
//            }
//        }

//        private ICommand addCommand;
//        public ICommand AddCommand
//        {
//            get
//            {
//                if (addCommand == null)
//                {
//                    addCommand = new RelayCommand(Add);
//                }
//                return addCommand;
//            }
//        }

//        private void Add()
//        {
//            int nextID = GetNextID();
//            int anStudiu = int.Parse(SelectedYear);
//            int materieID = GetMaterieOrSpecializareIdByName(SelectedMaterie, "Materii");
//            int specializareID = GetMaterieOrSpecializareIdByName(SelectedSpecializare, "Specializari");

//            bool exists = dbContext.AsocieriAnSpecMaterie
//                 .Any(a => a.AnStudiu == anStudiu && a.MaterieID == materieID && a.SpecializareID == specializareID);


//            if (exists)
//            {
//                MessageBox.Show("Asocierea inserata exista deja in tabel!", "Eroare!", MessageBoxButton.OK, MessageBoxImage.Warning);
//                return;
//            }

//            AsocieriAnSpecMaterie newAsociere = new AsocieriAnSpecMaterie
//            {
//                AsocieriAnSpecMaterieID = nextID,
//                AnStudiu = anStudiu,
//                MaterieID = materieID,
//                SpecializareID = specializareID
//            };

//            AsocieriAnSpecializareMaterie.Add(newAsociere);
//            dbContext.AsocieriAnSpecMaterie.Add(newAsociere);
//            dbContext.SaveChanges();
//            LoadAsocieri();
//        }


//        private int GetNextID()
//        {
//            var maxID = dbContext.AsocieriAnSpecMaterie.Max(u => u.AsocieriAnSpecMaterieID);
//            return maxID + 1;
//        }

//        private void LoadAsocieri()
//        {
//            dbContext.AsocieriAnSpecMaterie.Load();
//            var asocieri = dbContext.AsocieriAnSpecMaterie.ToList();
//            AsocieriAnSpecializareMaterie = new ObservableCollection<AsocieriAnSpecMaterie>(asocieri);
//            OnPropertyChanged(nameof(AsocieriAnSpecializareMaterie));
//        }

//        public int GetMaterieOrSpecializareIdByName(string nameToFind, string table)
//        {
//            if (table == "Materii")
//            {
//                var materie = dbContext.Materii.FirstOrDefault(m => m.Nume == nameToFind);
//                if (materie != null)
//                {
//                    return materie.MaterieID;
//                }
//            }
//            if (table == "Specializari")
//            {
//                var specializare = dbContext.Specializari.FirstOrDefault(m => m.Nume == nameToFind);
//                if (specializare != null)
//                {
//                    return specializare.SpecializareID;
//                }
//            }
//            return -1;
//        }

//        private ICommand deleteCommand;
//        public ICommand DeleteCommand
//        {
//            get
//            {
//                if (deleteCommand == null)
//                {
//                    deleteCommand = new RelayCommand(Delete);
//                }
//                return deleteCommand;
//            }
//        }

//        private void Delete()
//        {
//            if (SelectedAsociere == null)
//                MessageBox.Show("Selectati o asociere de sters!");

//            dbContext.AsocieriAnSpecMaterie.Remove(SelectedAsociere);
//            dbContext.SaveChanges();
//            AsocieriAnSpecializareMaterie.Remove(SelectedAsociere);
//            SelectedAsociere = null;
//        }

//        private SchoolEntities dbContext;

//        public AnSpecMateriiVM()
//        {
//            dbContext = new SchoolEntities();
//            Materii = new ObservableCollection<GetMaterii_Result>(dbContext.GetMaterii());
//            Specializari = new ObservableCollection<GetAllSpecializari_Result>(dbContext.GetAllSpecializari());
//            LoadAsocieri();
//        }
//    }
//}


using EducationalPlatform.Models;
using EducationalPlatform.Models.BusinessLogicLayer;
using EducationalPlatform.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Administrator
{
    public class AnSpecMateriiVM : BaseVM
    {
        private string _selectedYear;
        private Specializari _selectedSpecializare;
        private Materii _selectedMaterie;

        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                }
            }
        }

        public Specializari SelectedSpecializare
        {
            get { return _selectedSpecializare; }
            set
            {
                if (_selectedSpecializare != value)
                {
                    _selectedSpecializare = value;
                    OnPropertyChanged(nameof(SelectedSpecializare));
                }
            }
        }

        public Materii SelectedMaterie
        {
            get { return _selectedMaterie; }
            set
            {
                if (_selectedMaterie != value)
                {
                    _selectedMaterie = value;
                    OnPropertyChanged(nameof(SelectedMaterie));
                }
            }
        }

        private ObservableCollection<Materii> _materii;
        private ObservableCollection<Specializari> _specializari;

        public ObservableCollection<Materii> Materii
        {
            get { return _materii; }
            set
            {
                if (_materii != value)
                {
                    _materii = value;
                    OnPropertyChanged(nameof(Materii));
                }
            }
        }

        public ObservableCollection<Specializari> Specializari
        {
            get { return _specializari; }
            set
            {
                if (_specializari != value)
                {
                    _specializari = value;
                    OnPropertyChanged(nameof(Specializari));
                }
            }
        }

        private ObservableCollection<AsocieriAnSpecMaterie> asocieriAnSpecializareMaterie;
        public ObservableCollection<AsocieriAnSpecMaterie> AsocieriAnSpecializareMaterie
        {
            get { return asocieriAnSpecializareMaterie; }
            set
            {
                asocieriAnSpecializareMaterie = value;
                OnPropertyChanged(nameof(AsocieriAnSpecializareMaterie));
            }
        }

        private AsocieriAnSpecMaterie selectedAsociere;
        public AsocieriAnSpecMaterie SelectedAsociere
        {
            get { return selectedAsociere; }
            set
            {
                selectedAsociere = value;
                OnPropertyChanged(nameof(SelectedAsociere));
            }
        }

        private ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(Add);
                }
                return addCommand;
            }
        }

        private void Add()
        {
            int nextID = GetNextID();
            int anStudiu = int.Parse(SelectedYear);
            int materieID = SelectedMaterie.MaterieID;
            int specializareID = SelectedSpecializare.SpecializareID;

            bool exists = dbContext.AsocieriAnSpecMaterie
                 .Any(a => a.AnStudiu == anStudiu && a.MaterieID == materieID && a.SpecializareID == specializareID);


            if (exists)
            {
                MessageBox.Show("Asocierea inserata exista deja in tabel!", "Eroare!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AsocieriAnSpecMaterie newAsociere = new AsocieriAnSpecMaterie
            {
                AsocieriAnSpecMaterieID = nextID,
                AnStudiu = anStudiu,
                MaterieID = materieID,
                SpecializareID = specializareID
            };

            AsocieriAnSpecializareMaterie.Add(newAsociere);
            dbContext.AsocieriAnSpecMaterie.Add(newAsociere);
            dbContext.SaveChanges();
            LoadAsocieri();
        }


        private int GetNextID()
        {
            var maxID = dbContext.AsocieriAnSpecMaterie.Max(u => u.AsocieriAnSpecMaterieID);
            return maxID + 1;
        }

        private void LoadAsocieri()
        {
            dbContext.AsocieriAnSpecMaterie.Load();
            var asocieri = dbContext.AsocieriAnSpecMaterie.ToList();
            AsocieriAnSpecializareMaterie = new ObservableCollection<AsocieriAnSpecMaterie>(asocieri);
            OnPropertyChanged(nameof(AsocieriAnSpecializareMaterie));
        }

        //public int GetMaterieOrSpecializareIdByName(string nameToFind, string table)
        //{
        //    if (table == "Materii")
        //    {
        //        MessageBox.Show(nameToFind);
        //        var materie = dbContext.Materii.FirstOrDefault(m => m.NumeMaterie == nameToFind);
        //        if (materie != null)
        //        {
        //            return materie.MaterieID;
        //        }
        //    }
        //    if (table == "Specializari")
        //    {
        //        var specializare = dbContext.Specializari.FirstOrDefault(m => m.Nume == nameToFind);
        //        if (specializare != null)
        //        {
        //            return specializare.SpecializareID;
        //        }
        //    }
        //    return -1;
        //}

        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(Delete);
                }
                return deleteCommand;
            }
        }

        private void Delete()
        {
            if (SelectedAsociere == null)
                MessageBox.Show("Selectati o asociere de sters!");

            dbContext.AsocieriAnSpecMaterie.Remove(SelectedAsociere);
            dbContext.SaveChanges();
            AsocieriAnSpecializareMaterie.Remove(SelectedAsociere);
            SelectedAsociere = null;
        }

        private SchoolEntities dbContext;

        public AnSpecMateriiVM()
        {
            dbContext = new SchoolEntities();
            Materii = new ObservableCollection<Materii>(dbContext.Materii.ToList());
            Specializari = new ObservableCollection<Specializari>(dbContext.Specializari.ToList());
            LoadAsocieri();
        }
    }
}