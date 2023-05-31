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

namespace EducationalPlatform.ViewsModels.Administrator
{
 
    public class UtilizatorEditVM : BaseVM
    {
        private ObservableCollection<Utilizatori> utilizator;
        private Utilizatori selectedUtilizator;
        private int id;
        private string nume;
        private string parola;
        private string rol;

        private readonly SchoolEntities dbContext;

        public ObservableCollection<Utilizatori> Utilizatori
        {
            get { return utilizator; }
            set
            {
                utilizator = value;
                OnPropertyChanged(nameof(Utilizatori));
            }
        }

        public Utilizatori SelectedUtilizator
        {
            get { return selectedUtilizator; }
            set
            {
                selectedUtilizator = value;
                if (selectedUtilizator != null)
                {
                    Id = selectedUtilizator.UtilizatorID;
                    Nume = selectedUtilizator.Nume;
                    Parola =selectedUtilizator.Parola;
                    Rol = selectedUtilizator.Rol;
                }
                OnPropertyChanged(nameof(SelectedUtilizator));
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

        public string Parola
        {
            get { return parola; }
            set
            {
                parola = "";
                OnPropertyChanged(nameof(parola));
            }
        }

        public string Rol
        {
            get { return rol; }
            set
            {
                rol = "";
                OnPropertyChanged(nameof(rol));
            }
        }

        private string _selectedRol;
        public string SelectedRol
        {
            get { return _selectedRol; }
            set
            {
                _selectedRol = value;
                OnPropertyChanged(nameof(SelectedRol));
            }
        }

        public UtilizatorEditVM()
        {
            Utilizatori = new ObservableCollection<Utilizatori>();
            dbContext = new SchoolEntities();
            LoadUtilizatori();
        }


        private void LoadUtilizatori()
        {

            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Utilizatori>("EXEC GetAllUtilizatori");
                Utilizatori = new ObservableCollection<Utilizatori>(query);
            }
        }


        private ICommand addUtilizatorCommand;
        private ICommand updateUtilizatorCommand;
        private ICommand deleteUtilizatorCommand;

        public ICommand AddUtilizatorCommand
        {
            get
            {
                if (addUtilizatorCommand == null)
                {
                    addUtilizatorCommand = new RelayCommand(AddUtilizator);
                }
                return addUtilizatorCommand;
            }
        }

        private int GetNextUtilizatorID()
        {

            var maxUtilizatorID = dbContext.Utilizatori.Max(u => u.UtilizatorID);
            return maxUtilizatorID + 1;

        }

        private void AddUtilizator()
        {
            Utilizatori utilizator = new Utilizatori
            {
                UtilizatorID = GetNextUtilizatorID(),
                Nume = Nume,
                Parola ="parola",
                Rol=SelectedRol.ToString()
            };
            dbContext.Utilizatori.Add(utilizator);
            dbContext.SaveChanges();
            LoadUtilizatori();
            OnPropertyChanged(nameof(Utilizatori));
        }

        public ICommand UpdateUtilizatorCommand
        {
            get
            {
                if (updateUtilizatorCommand == null)
                {
                    updateUtilizatorCommand = new RelayCommand(UpdateUtilizator);
                }
                return updateUtilizatorCommand;
            }
        }

        private void UpdateUtilizator()
        {

            var numeParam = new SqlParameter("@Nume", Nume);
            var parolaParam = new SqlParameter("@Parola", Parola);
            var rolParam = new SqlParameter("@Rol", SelectedRol);
            if (SelectedUtilizator== null)
            {
                MessageBox.Show("Selectati un utilizator!");
                return;
            }
            var utilizatorIDParam = new SqlParameter("@UtilizatorID", SelectedUtilizator.UtilizatorID);
            dbContext.Database.ExecuteSqlCommand("EXEC UpdateUtilizator @UtilizatorID, @Nume, @Parola, @Rol",
                utilizatorIDParam, numeParam, parolaParam, rolParam);

            dbContext.SaveChanges();
            LoadUtilizatori();
            OnPropertyChanged(nameof(Utilizatori));
        }

        public ICommand DeleteUtilizatorCommand
        {
            get
            {
                if (deleteUtilizatorCommand == null)
                {
                    deleteUtilizatorCommand = new RelayCommand(DeleteUtilizator);
                }
                return deleteUtilizatorCommand;
            }
        }

        private void DeleteUtilizator()
        {

            var idParam = new SqlParameter("@UtilizatorID", SelectedUtilizator.UtilizatorID);
            var rolParam = new SqlParameter("@Rol", SelectedUtilizator.Rol);

            switch (rolParam.Value.ToString().ToLower() ){
                case "elev":
                dbContext.Database.ExecuteSqlCommand("EXEC DeleteElev_Utilizator @UtilizatorID", idParam);
                break;

                case "profesor":
                dbContext.Database.ExecuteSqlCommand("EXEC DeleteProfesor_Utilizator @UtilizatorID", idParam);
                break;

            case "diriginte":
                dbContext.Database.ExecuteSqlCommand("EXEC DeleteDiriginte_Utilizator @UtilizatorID", idParam);
                break;
            }

            dbContext.Database.ExecuteSqlCommand("EXEC DeleteUtilizator @UtilizatorID", idParam);
            dbContext.SaveChanges();
            LoadUtilizatori();
            OnPropertyChanged(nameof(Utilizatori));

            Nume = string.Empty;
            parola = string.Empty;

            MessageBox.Show("The selected item was deleted with success!");
        }

    }
}
