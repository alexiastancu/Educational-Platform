using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace EducationalPlatform.ViewsModels.Profesor
{
    public class MaterialeDidacticeVM : BaseVM
    {
        private SchoolEntities dbContext;

        public MaterialeDidacticeVM(int id)
        {
            dbContext = new SchoolEntities();
            Materii = new ObservableCollection<Materii>();
            Materiale = new ObservableCollection<MaterialeDidactice>();
            LoadData();
            ProfesorID = id;
        }
        private int profesorID;
        public int ProfesorID
        {
            get { return profesorID; }
            set
            {
                profesorID = value;
                LoadData();
            }
        }


        private ObservableCollection<Materii> materii;
        private ObservableCollection<MaterialeDidactice> materiale;
        private Materii selectedMaterie;
        private MaterialeDidactice selectedMaterial;
        private string numeMaterial;

        public ObservableCollection<Materii> Materii
        {
            get { return materii; }
            set
            {
                materii = value;
                OnPropertyChanged(nameof(Materii));
            }
        }

        public ObservableCollection<MaterialeDidactice> Materiale
        {
            get { return materiale; }
            set
            {
                materiale = value;
                OnPropertyChanged(nameof(Materiale));
            }
        }

        public Materii SelectedMaterie
        {
            get { return selectedMaterie; }
            set
            {
                selectedMaterie = value;
                OnPropertyChanged(nameof(SelectedMaterie));
            }
        }

        public MaterialeDidactice SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                selectedMaterial = value;
                OnPropertyChanged(nameof(SelectedMaterial));
            }
        }

        public string NumeMaterial
        {
            get { return numeMaterial; }
            set
            {
                numeMaterial = value;
                OnPropertyChanged(nameof(NumeMaterial));
            }
        }


        private void LoadData()
        {
            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Materii>("EXEC GetMaterii");

                Materii = new ObservableCollection<Materii>(query);
                var materials = context.MaterialeDidactice
                                       .Where(m => context.AsocieriProfMaterieClasa
                                       .Any(apmc => apmc.ProfesorID == ProfesorID && apmc.MaterieID == m.MaterieID))
                                       .ToList();

                Materiale = new ObservableCollection<MaterialeDidactice>(materials);
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
            var newMaterial = new MaterialeDidactice
            {
                MaterialID = dbContext.MaterialeDidactice.Max(u => u.MaterialID)+1,
                MaterieID = SelectedMaterie.MaterieID,
                NumeMateriale = NumeMaterial,
                TipFisier = "pdf",
            };

            Materiale.Add(newMaterial);
            dbContext.MaterialeDidactice.Add(newMaterial);
            dbContext.SaveChanges();
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
            if (SelectedMaterial != null)
            {
                var material = dbContext.Set<MaterialeDidactice>().Find(SelectedMaterial.MaterialID);

                if (material != null)
                {
                    dbContext.Set<MaterialeDidactice>().Remove(material);
                    dbContext.SaveChanges();
                    Materiale.Remove(SelectedMaterial);
                    LoadData();
                    OnPropertyChanged(nameof(Materiale));
                    SelectedMaterial = null;
                }
                
            }
            else
            {
                MessageBox.Show("Selectati un material didactic de sters!");
            }
        }

    }
}
