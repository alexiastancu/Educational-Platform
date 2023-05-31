using EducationalPlatform.Helpers;
using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EducationalPlatform.ViewsModels.Profesor
{
    public class AbsenteVM : BaseVM
    {
        int profId;
        SchoolEntities dbContext;

        public AbsenteVM(int id)
        {
            this.profId = id;
            Load();
            dbContext = new SchoolEntities();
        }

        private ObservableCollection<Elevi> _elevi;
        private ObservableCollection<Materii> _materii;
        private Elevi _selectedElev;
        private Materii _selectedMaterie;

        public ObservableCollection<Elevi> Elevi
        {
            get { return _elevi; }
            set
            {
                _elevi = value;
                OnPropertyChanged(nameof(Elevi));
            }
        }

        public ObservableCollection<Materii> Materii
        {
            get { return _materii; }
            set
            {
                _materii = value;
                OnPropertyChanged(nameof(Materii));
            }
        }

        public Elevi SelectedElev
        {
            get { return _selectedElev; }
            set
            {
                _selectedElev = value;
                OnPropertyChanged(nameof(SelectedElev));
            }
        }

        public Materii SelectedMaterie
        {
            get { return _selectedMaterie; }
            set
            {
                _selectedMaterie = value;
                OnPropertyChanged(nameof(SelectedMaterie));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private Absente selectedAbsenta;

        public Absente SelectedAbsenta
        {
            get { return selectedAbsenta; }
            set
            {
                selectedAbsenta = value;
                OnPropertyChanged(nameof(SelectedAbsenta));
            }
        }

        private ObservableCollection<Absente> _absente;
        public ObservableCollection<Absente> Absente
        {
            get { return _absente; }
            set
            {
                _absente = value;
                OnPropertyChanged(nameof(Absente));
            }
        }      


        private void Load()
        {
            using (var context = new SchoolEntities())
            {
                var query = context.Database.SqlQuery<Materii>("EXEC GetMaterii");

                Materii = new ObservableCollection<Materii>(query);
                var query2 = context.Database.SqlQuery<Elevi>("SELECT * FROM Elevi");
                Elevi = new ObservableCollection<Elevi>(query2);
                var query3 = context.Absente
                                    .Where(a => a.Materii.AsocieriProfMaterieClasa.Any(apmc => apmc.ProfesorID == profId))
                                    .ToList();
                Absente = new ObservableCollection<Absente>(query3);
            }
        }

        private ICommand _motivareCommand;
        public ICommand MotivareCommand
        {
            get
            {
                if (_motivareCommand == null)
                {
                    _motivareCommand = new RelayCommand(Motivare);
                }
                return _motivareCommand;
            }
        }

        private ICommand _adaugaCommand;
        public ICommand AdaugaCommand
        {
            get
            {
                if (_adaugaCommand == null)
                {
                    _adaugaCommand = new RelayCommand(Adauga);
                }
                return _adaugaCommand;
            }
        }

        private void Motivare()
        {
            if (SelectedAbsenta != null)
            {
                SelectedAbsenta.Motivare = true;

                dbContext.Entry(SelectedAbsenta).State = EntityState.Modified;
                dbContext.SaveChanges();
                Load();
                OnPropertyChanged(nameof(Absente));
            }
            else
            {
                MessageBox.Show("Selectati o absenta de motivat!");
            }
        }
        private void Adauga()
        {
            if (SelectedElev == null || SelectedMaterie == null || SelectedDate == null)
            {
                MessageBox.Show("Completati toate campurile!");
                return;
            }

            Absente newAbsenta = new Absente
            {
                AbsentaID = GetNextID(),
                ElevID = SelectedElev.ElevID,
                MaterieID = SelectedMaterie.MaterieID,
                data_absenta = SelectedDate,
                Motivare = false
            };
            Absente.Add(newAbsenta);
            dbContext.Absente.Add(newAbsenta);
            dbContext.SaveChanges();
            Load();
            OnPropertyChanged(nameof(Absente));
        }

        private int GetNextID()
        {
            var maxID = dbContext.Absente.Max(u => u.AbsentaID);
            return maxID + 1;
        }

    }
}
