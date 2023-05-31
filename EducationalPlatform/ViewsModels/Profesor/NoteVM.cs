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

namespace EducationalPlatform.ViewsModels.Profesor
{
    public class NoteVM : BaseVM
    {
        int profesorID;
        SchoolEntities dbContext;

        public NoteVM(int id)
        {
            this.profesorID = id;
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

        private Note selectedNota;

        public Note SelectedNota
        {
            get { return selectedNota; }
            set
            {
                selectedNota = value;
                OnPropertyChanged(nameof(SelectedNota));
            }
        }


        private ObservableCollection<Note> _note;
        public ObservableCollection<Note> Note
        {
            get { return _note; }
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
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

                var query3 = context.Note
                                    .Where(n => n.Materii.AsocieriProfMaterieClasa.Any(apmc => apmc.ProfesorID == profesorID))
                                    .ToList();
                Note = new ObservableCollection<Note>(query3);

            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(Delete);
                }
                return _deleteCommand;
            }
        }


        private int _nota;
        public int Nota
        {
            get { return _nota; }
            set
            {
                _nota = value;
                OnPropertyChanged(nameof(Nota));
            }
        }
        private void Delete()
        {
            if (SelectedNota != null)
            {
                var foundNota = dbContext.Set<Note>().Find(SelectedNota.NotaID);
                if (foundNota != null)
                {
                    dbContext.Note.Remove(foundNota);
                    dbContext.SaveChanges();
                    Note.Remove(foundNota);
                    SelectedNota = null;
                    Load();
                    OnPropertyChanged(nameof(Note));
                }                
            }
            else
            {
                MessageBox.Show("Selectați o notă!");
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

     
        private void Adauga()
        {
            if (SelectedElev == null || SelectedMaterie == null )
            {
                MessageBox.Show("Completati toate campurile!");
                return;
            }

            Note newNota = new Note
            {
                NotaID = GetNextID(),
                ElevID = SelectedElev.ElevID,
                MaterieID = SelectedMaterie.MaterieID,
                Valoare= Nota
            };
            Note.Add(newNota);
            dbContext.Note.Add(newNota);
            dbContext.SaveChanges();
            Load();
            OnPropertyChanged(nameof(Note));
        }

        private int GetNextID()
        {
            var maxID = dbContext.Note.Max(u => u.NotaID);
            return maxID + 1;
        }

    }
}




