using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private Users _user;
        private string _path;
        private UsersPhoto UP;
        public UserPage(Users User)
        {
            InitializeComponent();
            _user = User;
            TBUserName.Text = _user.Name;
            TBUserSurname.Text = _user.Surname;
            if (User.UsersPhoto!=null && User.UsersPhoto.PhotoBinary!=null)
            {
                byte[] BArr = User.UsersPhoto.PhotoBinary;
                BitmapImage BI = new BitmapImage();
                using (MemoryStream MS = new MemoryStream(BArr))
                {
                    BI.BeginInit();
                    BI.StreamSource = MS;
                    BI.CacheOption = BitmapCacheOption.OnLoad;
                    BI.EndInit();
                }
                UserPhotoImage.Source = BI;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow upWin = new UpdateWindow(_user);
            upWin.ShowDialog();
            FrameClass.FrameMain.Navigate(new UserPage(_user));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UsersPhoto U = BaseClass.Base.UsersPhoto.FirstOrDefault(x => x.IDUser == _user.IDUser);
            if (U == null)
            {
                UP = new UsersPhoto();
                UP.IDUser = _user.IDUser;
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.ShowDialog();
                _path = OFD.FileName;
                System.Drawing.Image SDI = System.Drawing.Image.FromFile(_path);
                ImageConverter IC = new ImageConverter();
                byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));
                UP.PhotoBinary = Barray;
                BaseClass.Base.UsersPhoto.Add(UP);
                BaseClass.Base.SaveChanges();
                MessageBox.Show("Картинка добавлена");
            }
            else
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.ShowDialog();
                _path = OFD.FileName;
                System.Drawing.Image SDI = System.Drawing.Image.FromFile(_path);
                ImageConverter IC = new ImageConverter();
                byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));
                U.PhotoBinary = Barray;
                BaseClass.Base.SaveChanges();
                MessageBox.Show("Картинка изменена");
            }
             
            FrameClass.FrameMain.Navigate(new UserPage(_user));


        }
    }
}
