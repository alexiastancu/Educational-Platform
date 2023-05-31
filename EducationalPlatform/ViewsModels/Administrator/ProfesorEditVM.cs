using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Administrator
{
    public class ProfesorEditVM : BaseVM
    {
        private ObservableCollection<Profesori> profesori;
        private Profesori selectedProfesor;
        private int id;
        private string nume;
        private string prenume;


        private readonly SchoolEntities dbContext;

        public ObservableCollection<Profesori> Profesori
        {
            get { return profesori; }
            set
            {
                profesori = value;
                OnPropertyChanged(nameof(Profesori));
            }
        }

        public Profesori SelectedProfesor
        {
            get { return selectedProfesor; }
            set
            {
                selectedProfesor = value;
                if (selectedProfesor != null)
                {

                    Id = selectedProfesor.ProfesorID;
                    Nume = selectedProfesor.Nume;
                    Prenume = selectedProfesor.Prenume;
                }
                OnPropertyChanged(nameof(SelectedProfesor));
            }
        }

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
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

        public string Prenume
        {
            get { return prenume; }
            set
            {
                prenume = value;
                OnPropertyChanged(nameof(Prenume));
            }
        }

        public ProfesorEditVM()
        {
            Profesori = new ObservableCollection<Profesori>();
            dbContext = new SchoolEntities();
            LoadProfesori();
        }


        private void LoadProfesori()
        {

            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Profesori>("EXEC GetAllProfessors");
                Profesori = new ObservableCollection<Profesori>(query);
            }
        }

        private bool _isDiriginteChecked;
        public bool IsDiriginteChecked
        {
            get { return _isDiriginteChecked; }
            set
            {
                if (_isDiriginteChecked != value)
                {
                    _isDiriginteChecked = value;

                    if (SelectedProfesor != null)
                    {
                        SelectedProfesor.esteDiriginte = value;
                    }

                    OnPropertyChanged(nameof(IsDiriginteChecked));
                }
            }
        }

        public int GetNextID(string tabel)
        {
            tabel = tabel.ToLower();
            int maxID = -1;

            switch (tabel)
            {
                case "profesori":
                    maxID = dbContext.Profesori.Max(u => u.ProfesorID);
                    break;
                case "diriginti":
                    maxID = dbContext.Diriginti.Max(u => u.DiriginteID);
                    break;
                case "utilizatori":
                    maxID = dbContext.Utilizatori.Max(u => u.UtilizatorID);
                    break;
                default:
                    // Handle the case where the specified table is not recognized
                    Console.WriteLine("Invalid table name.");
                    break;
            }

            return maxID + 1;
        }

        private ICommand addCommand;
        private ICommand updateCommand;
        private ICommand deleteCommand;

        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(AddProfesor);
                }
                return addCommand;
            }
        }

        
        private void AddProfesor()
        {
            Utilizatori utilizator = dbContext.Utilizatori.FirstOrDefault(u => u.Nume == Nume && u.Rol == "Profesor");
            int utilizatorID = GetNextID("utilizatori");
            if (utilizator == null)
            {                
                utilizator = new Utilizatori
                {
                    UtilizatorID = utilizatorID,
                    Nume = Nume,
                    Parola = Prenume,
                    Rol = "profesor"
                };
                dbContext.Utilizatori.Add(utilizator);
                dbContext.SaveChanges();
            }
            else
            {
                utilizatorID = utilizator.UtilizatorID;
            }
            
            int profID = GetNextID("profesori");
            Profesori profesor = new Profesori
            {
                ProfesorID = profID,
                Nume = Nume,
                Prenume = Prenume,
                UtilizatorID = utilizator.UtilizatorID,
                esteDiriginte = IsDiriginteChecked
            };
            dbContext.Profesori.Add(profesor);
            dbContext.SaveChanges();
            if (IsDiriginteChecked)
            {
                utilizator = dbContext.Utilizatori.FirstOrDefault(u => u.UtilizatorID == utilizatorID);
                if(utilizator != null)
                {
                    Diriginti diriginte = new Diriginti
                    {
                        DiriginteID = GetNextID("diriginti"),
                        ProfesorID = profID,
                        UtilizatorID = utilizatorID
                    };
                    dbContext.Diriginti.Add(diriginte);
                    dbContext.SaveChanges();
                }
                
            }

            
            dbContext.SaveChanges();
            LoadProfesori();
            OnPropertyChanged(nameof(Profesori));
            Nume = string.Empty;
            Prenume = string.Empty;
            IsDiriginteChecked = false;
        }


        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand(UpdateProfesor);
                }
                return updateCommand;
            }
        }

        private void UpdateProfesor()
        {
            if (SelectedProfesor == null)
            {
                MessageBox.Show("Selectati un profesor de actualizat!");
                return;
            }
            var idParam = new SqlParameter("@ProfessorID", SelectedProfesor.ProfesorID);
            var numeParam = new SqlParameter("@Nume", Nume);
            var prenumeParam = new SqlParameter("@Prenume", Prenume);
            var utilizatorIDParam = new SqlParameter("@UtilizatorID", SelectedProfesor.UtilizatorID);
            var esteDiriginteParam = new SqlParameter("@esteDiriginte", IsDiriginteChecked);


            dbContext.Database.ExecuteSqlCommand("EXEC UpdateProfessor @ProfessorID, @Nume, @Prenume, @esteDiriginte, @UtilizatorID",
                idParam, numeParam, prenumeParam, esteDiriginteParam, utilizatorIDParam);

            dbContext.SaveChanges();
            LoadProfesori();
            OnPropertyChanged(nameof(Profesori));
            Nume = string.Empty;
            Prenume = string.Empty;
            IsDiriginteChecked = false;
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteProfesor);
                }
                return deleteCommand;
            }
        }

        private void DeleteProfesor()
        {
            if (SelectedProfesor == null)
                return;


            var profesor = dbContext.Profesori
                .Include(p => p.Diriginti)
                .Include(p => p.AsocieriProfMaterieClasa)
                .FirstOrDefault(p => p.ProfesorID == SelectedProfesor.ProfesorID);

            if (profesor != null)
            {
                // Remove associated records from Clasa
                var AsocieriProfMaterieClasa = profesor.AsocieriProfMaterieClasa
                                                               .Select(amcd => amcd.ProfesorID)
                                                               .ToList();

                var clasaIds = dbContext.Clase
                    .Where(c => c.AsocieriProfMaterieClasa
                    .Any(amcd => AsocieriProfMaterieClasa.Contains(amcd.AsocieriProfMaterieClasaID)))
                    .Select(c => c.ClasaID)
                    .ToList();

                foreach (var clasaId in clasaIds)
                {
                    var clasa = dbContext.Clase.Find(clasaId);
                    if (clasa != null)
                    {
                        dbContext.Clase.Remove(clasa);
                    }
                }

                // Remove associated records from AsocieriMaterieClasaDiriginte
                dbContext.AsocieriProfMaterieClasa.RemoveRange(profesor.AsocieriProfMaterieClasa);

                // Delete the professor
                dbContext.Profesori.Remove(profesor);

                // Delete the associated user
                var user = dbContext.Utilizatori.FirstOrDefault(u => u.UtilizatorID == profesor.UtilizatorID);
                if (user != null)
                {
                    dbContext.Utilizatori.Remove(user);
                }

                // Save changes
                try
                {
                    dbContext.SaveChanges();
                    LoadProfesori();
                    OnPropertyChanged(nameof(Profesori));
                    Nume = string.Empty;
                    Prenume = string.Empty;
                    IsDiriginteChecked = false;
                    Console.WriteLine("Professor deleted successfully.");
                }
                catch (DbUpdateException ex)
                {
                    // Handle any exceptions that occurred during the save process
                    Console.WriteLine("An error occurred while deleting the professor: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Professor not found.");
            }
        }
    }
}



