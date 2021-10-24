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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            CbGenderReg.ItemsSource = BaseClass.Base.GenderTable.ToList();
            CbGenderReg.SelectedValuePath = "IDGender";
            CbGenderReg.DisplayMemberPath = "Genger";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int pasGegCode = TbPasReg.Password.GetHashCode();
            Users UserRez = new Users() { Name = TbNameReg.Text, Surname = TbSurnameReg.Text, Login = TbLoginReg.Text, Password = pasGegCode, IDGender = CbGenderReg.SelectedIndex + 1, IDRole = 2 };
            BaseClass.Base.Users.Add(UserRez);
            BaseClass.Base.SaveChanges();
            MessageBox.Show("Вы зарегистрированы");
        }
    }
}
