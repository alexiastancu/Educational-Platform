using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPlatform.ViewsModels.Profesor
{
    public class MediiVM : BaseVM
    {
        private int profesorID;
        private SchoolEntities dbContext;

        public MediiVM(int id)
        {
            this.profesorID = id;
            dbContext = new SchoolEntities();
            CalculareMedii();
        }

        private ObservableCollection<Medie> medii;
        public ObservableCollection<Medie> Medii
        {
            get { return medii; }
            set
            {
                medii = value;
                OnPropertyChanged(nameof(Medii));
            }
        }

        private void CalculareMedii()
        {
            //var groupedNotes = dbContext.Note.Include("Elevi").Include("Materii")
            //                         .GroupBy(n => new { n.ElevID, n.MaterieID })
            //                         .Select(g => new Medie
            //                         {
            //                             elevID = (int)g.Key.ElevID,
            //                             materieID = (int)g.Key.MaterieID,
            //                             medie = (double)g.Average(n => n.Valoare)
            //                         })
            //                         .ToList();
            //Medii = new ObservableCollection<Medie>(groupedNotes);

            Medii = new ObservableCollection<Medie>(
            dbContext.Note
                .Where(n => n.Materii.AsocieriProfMaterieClasa.Any(apmc => apmc.ProfesorID == profesorID))
                .GroupBy(n => new { n.ElevID, n.MaterieID })
                .Select(g => new Medie
                {
                    elevID = (int)g.Key.ElevID,
                    materieID = (int)g.Key.MaterieID,
                    medie = (double)g.Average(n => n.Valoare)
                })
                .ToList());
        }
    }

    public class Medie
    {
        public int elevID { get; set; }
        public int materieID { get; set; }
        public double medie { get; set; }
    }
}
