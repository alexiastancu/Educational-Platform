using EducationalPlatform.Helpers;
using EducationalPlatform.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EducationalPlatform.Models;
using EducationalPlatform.Views.Administrator;
using EducationalPlatform.Models.BusinessLogicLayer;
using EducationalPlatform.ViewsModels.Administrator;
using EducationalPlatform.Views.Elev;
using EducationalPlatform.ViewsModels.Elev;
using EducationalPlatform.Views.Profesor;
using EducationalPlatform.ViewsModels.Profesor;
using System.Data.Entity.Core.Objects;

namespace EducationalPlatform.ViewsModels
{
    public class SignInVM : BaseVM
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _selectedRole;
        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        private ICommand signIN;
        public ICommand SignIN
        {
            get
            {
                if (signIN == null)
                {
                    signIN = new RelayCommand(SignIn);
                }
                return signIN;
            }
        }
        public void SignIn()
        {
            //SignInWindow w = new SignInWindow();
            //SignInVM vm = new SignInVM();
            //w.DataContext = vm;
            //App.Current.MainWindow.Close();
            //App.Current.MainWindow = w;
            //w.Show();
            int id = 0;
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(SelectedRole))
            {
                using (var dbContext = new SchoolEntities())
                //using (SqlConnection connection = new SqlConnection(Costants.connectionString))
                {
                    //ObjectParameter existsParameter = new ObjectParameter("Exists", typeof(bool));
                    //int isAuthenticated = dbContext.AuthenticateUser(Username, SelectedRole, existsParameter);
                    //bool exists = (bool)existsParameter.Value;
                    using (SqlConnection connection = new SqlConnection(Costants.connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("AuthenticateUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Add the input parameters
                            command.Parameters.AddWithValue("@Username", Username);
                            command.Parameters.AddWithValue("@Role", SelectedRole.ToLower());

                            // Add the output parameter
                            SqlParameter outputParameter = new SqlParameter("@UserId", SqlDbType.Int);
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            // Execute the stored procedure
                            command.ExecuteNonQuery();

                            // Get the value of the output parameter
                            id = Convert.ToInt32(outputParameter.Value);

                        }
                    }
                }

                if (id != -1)
                {
                    switch (SelectedRole.ToLower())
                    {
                        case "elev":
                            MenuElev e = new MenuElev();
                            MenuElevVM evm = new MenuElevVM(id);
                            e.DataContext = evm;
                            App.Current.MainWindow.Close();
                            App.Current.MainWindow = e;
                            e.Show();
                            break;
                        case "administrator":
                            MenuAdministrator a = new MenuAdministrator();
                            MenuAdministratorVM avm = new MenuAdministratorVM();
                            a.DataContext = avm;
                            App.Current.MainWindow.Close();
                            App.Current.MainWindow = a;
                            a.Show();
                            break;
                        case "profesor":
                            MenuProfesor p = new MenuProfesor();
                            MenuProfesorVM pvm = new MenuProfesorVM(id);
                            p.DataContext = pvm;
                            App.Current.MainWindow.Close();
                            App.Current.MainWindow = p;
                            p.Show();
                            break;
                        case "diriginte":
                            //DiriginteWindow diriginteWindow = new DiriginteWindow();
                            //diriginteWindow.Show();
                            break;
                        default:
                            MessageBox.Show("Invalid role.");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username");
                }
            }

            else
            {
                MessageBox.Show("Completati toate campurile!");
            }

        }
    }
}

