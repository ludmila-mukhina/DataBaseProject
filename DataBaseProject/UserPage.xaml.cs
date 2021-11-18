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
        private UsersPhoto UP;  // пустой объект для добавления изображения в таблицу UsersPhoto
        public UserPage(Users User)
        {
            InitializeComponent();
            _user = User;
            TBUserName.Text = _user.Name;
            TBUserSurname.Text = _user.Surname;
            // проверяем, если ли у пользователя уже сохраненное поле в базе:
            if (User.UsersPhoto!=null && User.UsersPhoto.PhotoBinary!=null)
            {
                byte[] BArr = User.UsersPhoto.PhotoBinary;  // считываем изображение из базы (считываем байтовый массив двоичных данных)
                BitmapImage BI = new BitmapImage();  // создаем объект для загрузки изображения
                using (MemoryStream MS = new MemoryStream(BArr))  // для считывания байтового потока
                {
                    BI.BeginInit();  // начинаем считывание
                    BI.StreamSource = MS;  // задаем источник потока
                    BI.CacheOption = BitmapCacheOption.OnLoad;  // переводим изображение
                    BI.EndInit();  // заканчиваем считывание
                } 
                UserPhotoImage.Source = BI;  // показываем картинку на экране
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow upWin = new UpdateWindow(_user);  // создаем окно для редактирования данных
            upWin.ShowDialog();  // открываем окно
            // далее пишется код, который будет выполнятся после закрытия окна
            FrameClass.FrameMain.Navigate(new UserPage(_user));  // перезагружаем страницу
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // находим пользователя, для которого будем изменять или добавлять картинку (если у пользователя нет фото, объект будет пустым):
            UsersPhoto U = BaseClass.Base.UsersPhoto.FirstOrDefault(x => x.IDUser == _user.IDUser);
            if (U == null)  // если у пользователя не было изображения (то есть если объект U - пустой)
            {
                UP = new UsersPhoto();  // создаем объект для записи в базу
                UP.IDUser = _user.IDUser;  // заполняем поле с id
                OpenFileDialog OFD = new OpenFileDialog();   // создаем диалоговое окно
                OFD.ShowDialog();  // открываем диалоговое окно
                _path = OFD.FileName;   // считываем путь выбранного изображения
                System.Drawing.Image SDI = System.Drawing.Image.FromFile(_path);  // создаем объект для загрузки изображения в базу
                ImageConverter IC = new ImageConverter();  // создаем конвертер для перевода картинки в двоичный формат
                byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));  // создаем байтовый массив для хранения картинки
                UP.PhotoBinary = Barray;  // загружаем массив в базу
                BaseClass.Base.UsersPhoto.Add(UP);  // добавляем созданный объект в базу
                BaseClass.Base.SaveChanges();
                MessageBox.Show("Картинка добавлена");
            }
            else  // если у пользователя уже было изображение 
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
             
            FrameClass.FrameMain.Navigate(new UserPage(_user));  // перезагружаем страницу


        }
    }
}
