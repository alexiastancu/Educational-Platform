using EducationalPlatform.Models.BusinessLogicLayer;
using EducationalPlatform.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace EducationalPlatform.ViewsModels.Elev
{
 
     public class ElevEditVM : BaseVM
    {
        private ObservableCollection<Elevi> elev;
        private Elevi selectedElev;
        private int id;
        private string nume;
        private string prenume;

        private readonly SchoolEntities dbContext;

        public ObservableCollection<Elevi> Elevi
        {
            get { return elev; }
            set
            {
                elev = value;
                OnPropertyChanged(nameof(Profesori));
            }
        }

        public Elevi SelectedElev
        {
            get { return selectedElev; }
            set
            {
                selectedElev = value;
                if (selectedElev != null)
                {
                    Id = selectedElev.ElevID;
                    Nume = selectedElev.Nume;
                    Prenume = selectedElev.Prenume;
                }
                OnPropertyChanged(nameof(SelectedElev));
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

        public ElevEditVM()
        {
            Elevi = new ObservableCollection<Elevi>();
            dbContext = new SchoolEntities();
            LoadElevi();
        }


        private void LoadElevi()
        {

            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Elevi>("EXEC GetAllElevi");
                Elevi = new ObservableCollection<Elevi>(query);
            }
        }


        private ICommand addElevCommand;
        private ICommand updateElevCommand;
        private ICommand deleteElevCommand;

        public ICommand AddElevCommand
        {
            get
            {
                if (addElevCommand == null)
                {
                    addElevCommand = new RelayCommand(AddElev);
                }
                return addElevCommand;
            }
        }

        private int GetNextUtilizatorID()
        {

            var maxUtilizatorID = dbContext.Utilizatori.Max(u => u.UtilizatorID);
            return maxUtilizatorID + 1;

        }

        private int GetNextID()
        {

            var maxID = dbContext.Elevi.Max(u => u.ElevID);
            return maxID + 1;

        }
        private void AddElev()
        {
            Utilizatori utilizator = new Utilizatori();
            if (!dbContext.Utilizatori.Any(u => u.Nume == Nume && u.Rol == "Elev"))
            {
                utilizator = new Utilizatori
                {
                    UtilizatorID = GetNextUtilizatorID(),
                    Nume = Nume,
                    Parola = Prenume,
                    Rol = "elev"
                };
            }
            dbContext.Utilizatori.Add(utilizator);
            Elevi elev = new Elevi
            {
                ElevID =GetNextID(),
                Nume = Nume,
                Prenume = Prenume,
                UtilizatorID=GetNextID()
            };



            dbContext.Elevi.Add(elev);
            dbContext.SaveChanges();
            LoadElevi();
            OnPropertyChanged(nameof(Elevi));
        }

        public ICommand UpdateElevCommand
        {
            get
            {
                if (updateElevCommand == null)
                {
                    updateElevCommand = new RelayCommand(UpdateElev);
                }
                return updateElevCommand;
            }
        }

        private void UpdateElev()
        {

            var idParam = new SqlParameter("@ElevID", Id);
            var numeParam = new SqlParameter("@Nume", Nume);
            var prenumeParam = new SqlParameter("@Prenume", Prenume);
            if(SelectedElev == null )
            {
                MessageBox.Show("Selectati un elev!");
                return;
            }
            var utilizatorIDParam = new SqlParameter("@UtilizatorID", SelectedElev.UtilizatorID);
            dbContext.Database.ExecuteSqlCommand("EXEC UpdateElev @ElevID, @Nume, @Prenume, @UtilizatorID",
                idParam, numeParam, prenumeParam, utilizatorIDParam);

            dbContext.SaveChanges();
            LoadElevi();
            OnPropertyChanged(nameof(Elevi));
        }

        public ICommand DeleteElevCommand
        {
            get
            {
                if (deleteElevCommand == null)
                {
                    deleteElevCommand = new RelayCommand(DeleteElev);
                }
                return deleteElevCommand;
            }
        }

        private void DeleteElev()
        {

            var idParam = new SqlParameter("@Id", id);
            var numeParam = new SqlParameter("@Nume", nume);
            var prenumeParam = new SqlParameter("@Prenume", prenume);

            dbContext.Database.ExecuteSqlCommand("EXEC DeleteElev @Id, @Nume, @Prenume",
                idParam, numeParam, prenumeParam);
            dbContext.SaveChanges();
            LoadElevi();
            OnPropertyChanged(nameof(Elevi));

            Nume = string.Empty;
            Prenume = string.Empty;
            MessageBox.Show("The selected item was deleted with success!");
        }

    }
}

