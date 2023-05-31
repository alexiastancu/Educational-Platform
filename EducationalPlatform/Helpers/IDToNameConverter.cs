using System.Globalization;
using System.Windows.Data;
using System;
using EducationalPlatform.Models.BusinessLogicLayer;
using System.Reflection;
using System.Data.Entity;
using System.Linq;

namespace EducationalPlatform.Helpers
{
    public class IDToNameConverter : IValueConverter
    {
        private SchoolEntities dbContext;

        public IDToNameConverter()
        {
            dbContext = new SchoolEntities();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return string.Empty;

            int id = (int)value;
            string parameterName = parameter.ToString();
            string name = string.Empty;

            switch (parameterName)
            {
                case "Materie":
                    var materieQuery = from materie in dbContext.Materii
                                       where materie.MaterieID == id
                                       select materie.Nume;

                    name = materieQuery.FirstOrDefault() ?? string.Empty;
                    break;

                case "Specializare":
                    var specializareQuery = from specializare in dbContext.Specializari
                                            where specializare.SpecializareID == id
                                            select specializare.Nume;

                    name = specializareQuery.FirstOrDefault() ?? string.Empty;
                    break;

                case "Elev":
                    var elevQuery = from elevi in dbContext.Elevi
                                    where elevi.ElevID == id
                                    select new { elevi.Nume, elevi.Prenume };

                    var elevResult = elevQuery.FirstOrDefault();
                    if (elevResult != null)
                        name = $"{elevResult.Nume} {elevResult.Prenume}";
                    else
                        name = string.Empty;
                    break;

                case "Diriginte":
                    var diriginteQuery = from diriginte in dbContext.Diriginti
                                         join profesor in dbContext.Profesori on diriginte.ProfesorID equals profesor.ProfesorID
                                         where diriginte.DiriginteID == id
                                         select new { profesor.Nume, profesor.Prenume };

                    var diriginteResult = diriginteQuery.FirstOrDefault();
                    if (diriginteResult != null)
                        name = $"{diriginteResult.Nume} {diriginteResult.Prenume}";
                    else
                        name = string.Empty;
                    break;

                case "Clasa":
                    var clasaQuery = from clasa in dbContext.Clase
                                     where clasa.ClasaID == id
                                     select clasa.NumeClasa;

                    name = clasaQuery.FirstOrDefault() ?? string.Empty;
                    break;
                case "Profesor":
                    var profesorQuery = from profesor in dbContext.Profesori
                                        where profesor.ProfesorID == id
                                        select new { profesor.Nume, profesor.Prenume };

                    var profesorResult = profesorQuery.FirstOrDefault();
                    if (profesorResult != null)
                        name = $"{profesorResult.Nume} {profesorResult.Prenume}";
                    else
                        name = string.Empty;
                    break;
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}

