using Microsoft.Win32;
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
    /// Логика взаимодействия для CreateOrUpdatePage.xaml
    /// </summary>
    public partial class CreateOrUpdatePage : Page
    {
        string path;
        public CreateOrUpdatePage()
        {
            InitializeComponent();
            LBTraits.ItemsSource = BaseClass.Base.Traits.ToList();  // ассоциируем коллекцию списка с чертами характера с соответствующей таблицей БД (Traits)
            LBTraits.SelectedValuePath = "idTrait";
            LBTraits.DisplayMemberPath = "Trait";
            LBDiets.ItemsSource = BaseClass.Base.FeedCat.ToList();  // ассоциируем коллекцию списка с кормами для кота с соответсвующей таблицей БД (FeedCat)
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int gender = 0;  // переменная для записи индекса пола
                if (RBGenderM.IsChecked == true)
                {
                    gender = 1;
                }
                if (RBGenderW.IsChecked == true)
                {
                    gender = 2;
                }
                // Создаем объект типа таблицы Cats и заполняем все поля этой таблицы (кроме idCat, он заполняется автоматически)
                Cats Cat = new Cats() { СatName = TBName.Text, Breed = Name = TBBreed.Text, CatDateBirtр = DPDate.DisplayDate.Date, IDGender = gender, CatPhoto = path };
                BaseClass.Base.Cats.Add(Cat);  // добавляем запись в модель БД
                BaseClass.Base.SaveChanges();  // сохраняем изменения в БД
                // Для заполнения таблицы TraitsCats нужно организовать цикл, так как черт характера у кота может быть несколько
                // Цикл будет организовывать по чертам характера, которые выделены в списке
                foreach (Traits t in LBTraits.SelectedItems)
                {
                    TraitsCats TC = new TraitsCats();  // объект для записи в таблицу TraitsCat
                    TC.idCat = Cat.idCat;  // idCat берем из объекта Cat, созданного выше
                    TC.idTrait = t.idTrait;  // idTrait берем из объекта TC, по которому организовываем цикл
                    BaseClass.Base.TraitsCats.Add(TC); // добавляем запись в модель БД
                }
                BaseClass.Base.SaveChanges();  // когда все черты добалены, сохраняем изменения в БД
                // Для заполнения таблицы Diets нужно организовать цикл, так как кормов у кота может быть несколько
                // Цикл будет организовывать по всем кормам, которые есть в списке
                foreach (FeedCat FC in LBDiets.Items)
                {
                    Diets D = new Diets(); // объект для записи в таблицу Diets
                    D.idFeed = FC.idFeed;   // idFeed берем из объекта D, по которому организовываем цикл
                    D.idCat = Cat.idCat;  // idCat берем из объекта Cat, созданного выше
                    D.QuantityMonth = FC.QM;  // значения будет брать из свойства в созданном частичном классе FeedCapPartial
                    BaseClass.Base.Diets.Add(D); // добавляем запись в модель БД
                }
                BaseClass.Base.SaveChanges();  // когда все корма добалены, сохраняем изменения в БД
                MessageBox.Show("Данные записаны");
            }
            catch
            {
                MessageBox.Show("Данные не записаны");
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)  // добавление фото кота с помощью диалогового окна
        {
            OpenFileDialog OFD = new OpenFileDialog();  // создаем объект диалогового окна
            OFD.ShowDialog();  // открываем диалоговое окно
            path = OFD.FileName;  // извлекаем полный поть к картинке
            int n = path.IndexOf("PhotoCat");  // ищем индекс, с которого начинается имя папки в пути к картике
            path = path.Substring(n);  // обрезаем путь, для того чтобы в базу записать только путь, который начинается с папки картинки
        }
    }
}
