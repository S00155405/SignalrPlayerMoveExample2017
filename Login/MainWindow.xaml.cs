using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         ObservableCollection<PersonalDetails> LoginDetails = new ObservableCollection<PersonalDetails>();
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LoginDetails)
            {
                if (txtEmail.Text != item.Email )
                {
                    
                    PersonalDetails a5 = new PersonalDetails(txtEmail.Text, txtPassword.Text);
                    LoginDetails.Add(a5);

                    //Sent to Server
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LoginDetails)
            {
                if(txtEmail.Text == item.Email && txtPassword.Text == item.Password)
                {
                    //Sent to Server
                } 
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PersonalDetails a1 = new PersonalDetails("Pat@Gmail.com", "123456");
            PersonalDetails a2 = new PersonalDetails("Joe@Gmail.com", "123456");
            PersonalDetails a3 = new PersonalDetails("Daniel@Gmail.com", "123456");
            PersonalDetails a4 = new PersonalDetails("Clodagh@Gmail.com", "123456");

            LoginDetails.Add(a1);
            LoginDetails.Add(a2);
            LoginDetails.Add(a3);
            LoginDetails.Add(a4);

            
        }
    }
}
