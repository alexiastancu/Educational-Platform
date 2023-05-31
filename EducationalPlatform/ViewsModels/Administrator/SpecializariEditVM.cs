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
      public class SpecializariEditVM : BaseVM
    {
        private ObservableCollection<Specializari> specializari;
        private Specializari selectedSpecializare;
        private int id;
        private string nume;


        private readonly SchoolEntities dbContext;

        public ObservableCollection<Specializari> Specializari
        {
            get { return specializari; }
            set
            {
                specializari = value;
                OnPropertyChanged(nameof(Profesori));
            }
        }

        public Specializari SelectedSpecializare
        {
            get { return selectedSpecializare; }
            set
            {
                selectedSpecializare = value;
                if (selectedSpecializare != null)
                {
                    Id = selectedSpecializare.SpecializareID;
                    Nume = selectedSpecializare.Nume;
            
                }
                OnPropertyChanged(nameof(SelectedSpecializare));
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

       
        public SpecializariEditVM()
        {
            Specializari = new ObservableCollection<Specializari>();
            dbContext = new SchoolEntities();
            LoadSpecializari();
        }


        private void LoadSpecializari()
        {
            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Specializari>("EXEC GetAllSpecializari");
                Specializari = new ObservableCollection<Specializari>(query);
            }
        }


        private ICommand addSpecializareCommand;
        private ICommand updateSpecializareCommand;
        private ICommand deleteSpecializareCommand;

        public ICommand AddSpeciaizareCommand
        {
            get
            {
                if (addSpecializareCommand == null)
                {
                    addSpecializareCommand = new RelayCommand(AddSpecializare);
                }
                return addSpecializareCommand;
            }
        }

        
        private int GetNextID()
        {

            int maxID = dbContext.Specializari.Max(u => u.SpecializareID);
            return maxID + 1;

        }
        private void AddSpecializare()
        {
          
            Specializari specializare = new Specializari
            {
                SpecializareID = GetNextID(),
                Nume = Nume
            };
            dbContext.Specializari.Add(specializare);
            dbContext.SaveChanges();
            LoadSpecializari();
            OnPropertyChanged(nameof(Specializari));
        }

        public ICommand UpdateSpecializareCommand
        {
            get
            {
                if (updateSpecializareCommand == null)
                {
                    updateSpecializareCommand = new RelayCommand(UpdateSpecializare);
                }
                return updateSpecializareCommand;
            }
        }

        private void UpdateSpecializare()
        {
            var idParam = new SqlParameter("@SpecializareID", Id);
            var numeParam = new SqlParameter("@Nume", Nume);
           
            dbContext.Database.ExecuteSqlCommand("EXEC UpdateSpecializare @SpecializareID, @Nume",
                idParam, numeParam);

            dbContext.SaveChanges();
            LoadSpecializari();
            OnPropertyChanged(nameof(Specializari));
        }

        public ICommand DeleteSpecializareCommand
        {
            get
            {
                if (deleteSpecializareCommand == null)
                {
                    deleteSpecializareCommand = new RelayCommand(DeleteSpecializare);
                }
                return deleteSpecializareCommand;
            }
        }

        private void DeleteSpecializare()
        {

            var idParam = new SqlParameter("@Id", id);
            var numeParam = new SqlParameter("@Nume", nume);

            dbContext.Database.ExecuteSqlCommand("EXEC DeleteSpecializare @Id, @Nume",
                idParam, numeParam);
            dbContext.SaveChanges();
            LoadSpecializari();
            OnPropertyChanged(nameof(Specializari));

            Nume = string.Empty;
            MessageBox.Show("The selected item was deleted with success!");
        }

    }
}
