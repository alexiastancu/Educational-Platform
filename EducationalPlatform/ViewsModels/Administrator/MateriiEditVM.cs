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
using System.Windows;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Administrator
{
    public class MateriiEditVM: BaseVM
    {
        private ObservableCollection<Materii> materii;
        private Materii selectedMaterie;
        private int id;
        private string nume;

        private readonly SchoolEntities dbContext;

        public ObservableCollection<Materii> Materii
        {
            get { return materii; }
            set
            {
                materii = value;
                OnPropertyChanged(nameof(Materii));
            }
        }

        public Materii SelectedMaterie
        {
            get { return selectedMaterie; }
            set
            {
                selectedMaterie = value;
                if (selectedMaterie != null)
                {
                    // Update the details properties
                    Id = selectedMaterie.MaterieID;
                    Nume = selectedMaterie.Nume;
                    
                }
                OnPropertyChanged(nameof(SelectedMaterie));
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

        public MateriiEditVM()
        {
            Materii = new ObservableCollection<Materii>();
            dbContext = new SchoolEntities();
            LoadMaterii();
        }

        private void LoadMaterii()
        {
            // Call the stored procedure to retrieve all professors
            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Materii>("EXEC GetMaterii");

                Materii = new ObservableCollection<Materii>();
                Materii = new ObservableCollection<Materii>(query);
            }
        }

        private int GetNextID()
        {

            var maxID = dbContext.Materii.Max(u => u.MaterieID);
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
                    addCommand = new RelayCommand(AddMaterie);
                }
                return addCommand;
            }
        }

        private void AddMaterie()
        {
            Materii materie = new Materii
            {
                MaterieID = GetNextID(),
                Nume = Nume,
            };



            dbContext.Materii.Add(materie);
            dbContext.SaveChanges();
            LoadMaterii();
            OnPropertyChanged(nameof(Materii));
        }


        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand(UpdateMaterie);
                }
                return updateCommand;
            }
        }

        private void UpdateMaterie()
        {
            var materieIDParam = new SqlParameter("@MaterieID", Id);
            var numeMaterieParam = new SqlParameter("@NumeMaterie", Nume);

            dbContext.Database.ExecuteSqlCommand("EXEC UpdateMaterie @MaterieID, @NumeMaterie",
                materieIDParam, numeMaterieParam);

            dbContext.SaveChanges();
            LoadMaterii();
            OnPropertyChanged(nameof(Materii));

        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteMaterie);
                }
                return deleteCommand;
            }
        }

        private void DeleteMaterie()
        {
            // Create parameters for the stored procedure
            var materieIDParam = new SqlParameter("@MaterieID", Id);
            var numeMaterieParam = new SqlParameter("@Nume", Nume);
            // Execute the stored procedure
            dbContext.Database.ExecuteSqlCommand("EXEC DeleteMaterie @MaterieID, @Nume",
                materieIDParam, numeMaterieParam);
            dbContext.SaveChanges();
            LoadMaterii();
            OnPropertyChanged(nameof(Materii));

            Id = 0;
            Nume = string.Empty;
            MessageBox.Show("The selected item was deleted with success!");

        }
    }
}
