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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace EducationalPlatform.ViewsModels.Administrator
{
  public class ProfMaterieClasaVM : BaseVM
    {
        private Profesori _selectedProfesor;
        private Clase _selectedClasa;
        private Materii _selectedMaterie;

        public Profesori SelectedProfesor
        {
            get { return _selectedProfesor; }
            set
            {
                if (_selectedProfesor != value)
                {
                    _selectedProfesor = value;
                    OnPropertyChanged(nameof(SelectedProfesor));
                }
            }
        }

        public Clase SelectedClasa
        {
            get { return _selectedClasa; }
            set
            {
                if (_selectedClasa != value)
                {
                    _selectedClasa = value;
                    OnPropertyChanged(nameof(SelectedClasa));
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
        private ObservableCollection<Clase> _clase;
        private ObservableCollection<Profesori> _profesori;

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

        public ObservableCollection<Clase> Clase
        {
            get { return _clase; }
            set
            {
                if (_clase != value)
                {
                    _clase = value;
                    OnPropertyChanged(nameof(Clase));
                }
            }
        }

        public ObservableCollection<Profesori> Profesori
        {
            get { return _profesori; }
            set
            {
                if (_profesori != value)
                {
                    _profesori = value;
                    OnPropertyChanged(nameof(Profesori));
                }
            }
        }

        private ObservableCollection<AsocieriProfMaterieClasa> asocieriProfMaterieClasa;
        public ObservableCollection<AsocieriProfMaterieClasa> AsocieriProfMaterieClasa
        {
            get { return asocieriProfMaterieClasa; }
            set
            {
                asocieriProfMaterieClasa = value;
                OnPropertyChanged(nameof(AsocieriProfMaterieClasa));
            }
        }

        private AsocieriProfMaterieClasa selectedAsociere;
        public AsocieriProfMaterieClasa SelectedAsociere
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
            bool exists = dbContext.AsocieriProfMaterieClasa.Any(a => a.ProfesorID == SelectedProfesor.ProfesorID && a.MaterieID == SelectedMaterie.MaterieID && a.ClasaID == SelectedClasa.ClasaID);
            if (exists)
            {
                MessageBox.Show("Asocierea inserata exista deja in tabel!", "Eroare!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AsocieriProfMaterieClasa newAsociere = new AsocieriProfMaterieClasa
            {
                AsocieriProfMaterieClasaID = GetNextID(),
                ProfesorID  = SelectedProfesor.ProfesorID,
                MaterieID = SelectedMaterie.MaterieID,
                ClasaID = SelectedClasa.ClasaID
            };

            AsocieriProfMaterieClasa.Add(newAsociere);
            dbContext.AsocieriProfMaterieClasa.Add(newAsociere);
            dbContext.SaveChanges();
            LoadAsocieri();
        }


        private int GetNextID()
        {
            var maxID = dbContext.AsocieriProfMaterieClasa.Max(u => u.AsocieriProfMaterieClasaID);
            return maxID + 1;
        }

        private void LoadAsocieri()
        {
            var query2 = dbContext.Database.SqlQuery<AsocieriProfMaterieClasa>("SELECT * FROM AsocieriProfMaterieClasa");
            AsocieriProfMaterieClasa = new ObservableCollection<AsocieriProfMaterieClasa>(query2);
            OnPropertyChanged(nameof(AsocieriProfMaterieClasa));
        }

        public int GetClasaOrMaterieIdByName(string nameToFind, string table)
        {
            if (table == "Clase")
            {
                var clasa = dbContext.Clase.FirstOrDefault(m => m.NumeClasa == nameToFind);
                if (clasa != null)
                {
                    return clasa.ClasaID;
                }
            }
            if (table == "Materii")
            {
                var specializare = dbContext.Materii.FirstOrDefault(m => m.Nume == nameToFind);
                if (specializare != null)
                {
                    return specializare.MaterieID;
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

            dbContext.AsocieriProfMaterieClasa.Attach(SelectedAsociere);
            dbContext.Entry(SelectedAsociere).State = EntityState.Deleted;

            //dbContext.AsocieriProfMaterieClasa.Remove(SelectedAsociere);
            dbContext.SaveChanges();
            AsocieriProfMaterieClasa.Remove(SelectedAsociere);
            SelectedAsociere = null;
        }

        private SchoolEntities dbContext;

        public ProfMaterieClasaVM()
        {
            dbContext = new SchoolEntities();
            Materii = new ObservableCollection<Materii>(dbContext.Materii);
            Clase = new ObservableCollection<Clase>(dbContext.Clase);
            Profesori = new ObservableCollection<Profesori>(dbContext.Profesori);
            LoadAsocieri();
        }
    }
}
