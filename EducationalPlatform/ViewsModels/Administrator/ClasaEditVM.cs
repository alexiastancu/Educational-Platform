using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalPlatform.Helpers;
using System.Security.Claims;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity.Infrastructure;

namespace EducationalPlatform.ViewsModels.Administrator
{
    internal class ClasaEditVM : BaseVM
    {
        private ObservableCollection<Clase> clase;
        private Clase selectedClasa;
        private string nume;
        private int an;

        private readonly SchoolEntities dbContext;

        public ObservableCollection<Clase> Clase
        {
            get { return clase; }
            set
            {
                clase = value;
                OnPropertyChanged(nameof(Clase));
            }
        }

        public Clase SelectedClasa
        {
            get { return selectedClasa; }
            set
            {
                selectedClasa = value;
                if (selectedClasa != null)
                {
                    Nume = SelectedClasa.NumeClasa;
                    An = (int)SelectedClasa.AnStudiu;
                }
                OnPropertyChanged(nameof(SelectedClasa));
            }
        }

        public string Nume
        {
            get { return nume; }
            set
            {
                nume = value;
                OnPropertyChanged(nameof(Nume));
            }
        }
        public int An
        {
            get { return an; }
            set
            {
                an = value;
                OnPropertyChanged(nameof(An));
            }
        }

        private ObservableCollection<Specializari> specializari;
        public ObservableCollection<Specializari> Specializari
        {
            get { return specializari; }
            set
            {
                if (specializari != value)
                {
                    specializari = value;
                    OnPropertyChanged(nameof(Specializari));
                }
            }
        }

        private ObservableCollection<Diriginti> diriginti;
        public ObservableCollection<Diriginti> Diriginti
        {
            get { return diriginti; }
            set
            {
                if (diriginti != value)
                {
                    diriginti = value;
                    OnPropertyChanged(nameof(Diriginti));
                }
            }
        }

        public ClasaEditVM()
        {
            Clase = new ObservableCollection<Clase>();
            Specializari = new ObservableCollection<Specializari>();
            Diriginti = new ObservableCollection<Diriginti>();
            dbContext = new SchoolEntities();
            LoadClase();
            Load();
        }

        private void Load()
        {
            var query = dbContext.Database.SqlQuery<Specializari>("EXEC GetAllSpecializari");
            Specializari = new ObservableCollection<Specializari>(query);
            var query2 = dbContext.Database.SqlQuery<Diriginti>("SELECT * FROM Diriginti");
            Diriginti = new ObservableCollection<Diriginti>(query2);
        }

        private void LoadClase()
        {
            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Clase>("EXEC GetClase");
                Clase = new ObservableCollection<Clase>(query);
                OnPropertyChanged(nameof(Clase));
            }
        }

        private int? _selectedYear;
        public int? SelectedYear
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

        private Diriginti _selectedDiriginte;
        public Diriginti SelectedDiriginte
        {
            get { return _selectedDiriginte; }
            set
            {
                if (_selectedDiriginte != value)
                {
                    _selectedDiriginte = value;
                    OnPropertyChanged(nameof(SelectedDiriginte));
                }
            }
        }

        private Specializari _selectedSpecializare;
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

        private int GetNextID()
        {
            var maxID = dbContext.Clase.Max(u => u.ClasaID);
            return maxID + 1;
        }

        private ICommand _addCommand;

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddClasa)); }
        }

        private void AddClasa()
        {
            if (string.IsNullOrEmpty(Nume) || SelectedDiriginte == null || SelectedSpecializare == null || SelectedYear == null)
            {
                MessageBox.Show("Completati toate campurile!");
                return;
            }
            bool exists = dbContext.Clase
                          .Any(c => c.AnStudiu == SelectedYear && c.DiriginteID == SelectedDiriginte.DiriginteID && c.SpecializareID == SelectedSpecializare.SpecializareID);
            if (exists)
            {
                MessageBox.Show("Clasa inserata exista deja in tabel!", "Eroare!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Create a new Clase instance using the selected properties
            Clase newClasa = new Clase
            {
                ClasaID = GetNextID(),
                NumeClasa = Nume,
                AnStudiu = SelectedYear,
                DiriginteID = SelectedDiriginte.DiriginteID,
                SpecializareID = SelectedSpecializare.SpecializareID
            };

            // Add the new Clase to your data source or perform any additional logic
            Clase.Add(newClasa);
            dbContext.Clase.Add(newClasa);
            dbContext.SaveChanges();
            LoadClase();

            // Clear the input properties for the next entry
            Nume = string.Empty;
            SelectedYear = null;
            SelectedDiriginte = null;
            SelectedSpecializare = null;
        }

        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(DeleteClasa)); }
        }
        private void DeleteClasa()
        {            
            try
            {
                Clase existingEntity = dbContext.Clase.Find(SelectedClasa.ClasaID);
                if (existingEntity != null)
                {
                    dbContext.Clase.Remove(existingEntity);
                    dbContext.SaveChanges();
                    Clase.Remove(existingEntity);
                    LoadClase();
                }

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is InvalidOperationException innerException &&
                    innerException.Message.Contains("another entity of the same type already has the same primary key value"))
                {
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
        }


    }
}
