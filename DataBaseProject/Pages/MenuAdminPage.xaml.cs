using System;
using System.Collections.Generic;
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

namespace DataBaseProject
{
    /// <summary>
    /// Логика взаимодействия для MenuAdminPage.xaml
    /// </summary>
    public partial class MenuAdminPage : Page
    {
        private Users _user;  // переменная для храния объекта
        public MenuAdminPage(Users User)
        {
            InitializeComponent();
            _user = User;  // записываем объект из контруктора в выше созданный объект для того, чтобы эти данные были даступны на всей странице
            DgUsers.ItemsSource = BaseClass.Base.Users.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SPGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SPGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FrameClass.FrameMain.Navigate(new ListCatsPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FrameClass.FrameMain.Navigate(new UserPage(_user)); // переход администратора в личный кабинет
        }
    }
}
