using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace EducationalPlatform.ViewsModels.Administrator
{
    public class EleviAnStudSpecializVM : BaseVM
    {
        private string _selectedYear;
        private string _selectedSpecializare;
        private string _selectedElev;

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

        public string SelectedSpecializare
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

        public string SelectedElev
        {
            get { return _selectedElev; }
            set
            {
                if (_selectedElev != value)
                {
                    _selectedElev = value;
                    OnPropertyChanged(nameof(SelectedElev));
                }
            }
        }

        private ObservableCollection<GetAllElevi_Result> _elevi;
        private ObservableCollection<GetAllSpecializari_Result> _specializari;

        public ObservableCollection<GetAllElevi_Result> Elevi
        {
            get { return _elevi; }
            set
            {
                if (_elevi != value)
                {
                    _elevi = value;
                    OnPropertyChanged(nameof(Elevi));
                }
            }
        }

        public ObservableCollection<GetAllSpecializari_Result> Specializari
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

        private ObservableCollection<AsocieriElevAnSpec> asocieriElevAnSpec;
        public ObservableCollection<AsocieriElevAnSpec> AsocieriElevAnSpec
        {
            get { return asocieriElevAnSpec; }
            set
            {
                asocieriElevAnSpec = value;
                OnPropertyChanged(nameof(AsocieriElevAnSpec));
            }
        }

        private AsocieriElevAnSpec selectedAsociere;
        public AsocieriElevAnSpec SelectedAsociere
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
            int elevID = GetElevOrSpecializareIdByName(SelectedElev, "Elevi");
            int specializareID = GetElevOrSpecializareIdByName(SelectedSpecializare, "Specializari");

            bool exists = dbContext.AsocieriElevAnSpec
                 .Any(a => a.AnStudiu == anStudiu && a.ElevID == elevID && a.SpecializareID == specializareID);


            if (exists)
            {
                MessageBox.Show("Asocierea inserata exista deja in tabel!", "Eroare!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AsocieriElevAnSpec newAsociere = new AsocieriElevAnSpec
            {
                AsocieriElevAnSpecID = nextID,
                AnStudiu = anStudiu,
                ElevID = elevID,
                SpecializareID = specializareID
            };

            AsocieriElevAnSpec.Add(newAsociere);
            dbContext.AsocieriElevAnSpec.Add(newAsociere);
            dbContext.SaveChanges();
            LoadAsocieri();
        }


        private int GetNextID()
        {
            var maxID = dbContext.AsocieriElevAnSpec.Max(u => u.AsocieriElevAnSpecID);
            return maxID + 1;
        }

        private void LoadAsocieri()
        {
            //dbContext.AsocieriElevAnSpec.Load();
            var asocieri = dbContext.AsocieriElevAnSpec.ToList();
            AsocieriElevAnSpec = new ObservableCollection<AsocieriElevAnSpec>(asocieri);
            OnPropertyChanged(nameof(AsocieriElevAnSpec));
        }

        public int GetElevOrSpecializareIdByName(string nameToFind, string table)
        {
            if (table == "Elevi")
            {
                var elev = dbContext.Elevi.FirstOrDefault(m => m.Nume == nameToFind);
                if (elev != null)
                {
                    return elev.ElevID;
                }
            }
            if (table == "Specializari")
            {
                var specializare = dbContext.Specializari.FirstOrDefault(m => m.Nume == nameToFind);
                if (specializare != null)
                {
                    return specializare.SpecializareID;
                }
            }
            return -1;
        }
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

            dbContext.AsocieriElevAnSpec.Remove(SelectedAsociere);
            dbContext.SaveChanges();
             AsocieriElevAnSpec.Remove(SelectedAsociere);
            SelectedAsociere = null;
        }

        private SchoolEntities dbContext;

        public EleviAnStudSpecializVM()
        {
            dbContext = new SchoolEntities();
            Elevi = new ObservableCollection<GetAllElevi_Result>(dbContext.GetAllElevi().ToList());
            Specializari = new ObservableCollection<GetAllSpecializari_Result>(dbContext.GetAllSpecializari());
            LoadAsocieri();
        }
    }
}