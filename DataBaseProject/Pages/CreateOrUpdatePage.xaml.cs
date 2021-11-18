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
        string path;  // путь к картинке
        Cats CAT;  // объект, в котором будет хранится данные о новом или отредактированном коте
        bool flag;  // для определения, создаем мы новый объект или редактируем старый
        List<TraitsCats> TraitsCatsList = BaseClass.Base.TraitsCats.ToList();  // список с таблицей качеств всех котов
        List<Diets> DietsList = BaseClass.Base.Diets.ToList();  // список с таблицей кормов для всех котов
        public CreateOrUpdatePage()  // конструктор для добавления кота
        {
            InitializeComponent();
            flag = true;  // флаг равен true, так как это конструктор для добавления кота
            LBTraits.ItemsSource = BaseClass.Base.Traits.ToList();  // ассоциируем коллекцию списка с чертами характера с соответствующей таблицей БД (Traits)
            LBTraits.SelectedValuePath = "idTrait";
            LBTraits.DisplayMemberPath = "Trait";
            LBDiets.ItemsSource = BaseClass.Base.FeedCat.ToList();  // ассоциируем коллекцию списка с кормами для кота с соответсвующей таблицей БД (FeedCat)
        }
        public CreateOrUpdatePage(Cats CatUpdate) // конструктор для редактирования данных о коте ( с непустым конструктором)
        {
            InitializeComponent();
            CAT = CatUpdate;  // ассоциируем выше созданный глобавльный объект с объектом в кострукторе для дальнейшего редактирования этих данных
            TBName.Text = CatUpdate.СatName; // вывод имени кота
            TBBreed.Text = CatUpdate.Breed;  // вывод породы кота
            DPDate.SelectedDate = CatUpdate.CatDateBirtр;  // вывод даты рождения кота
            switch (CatUpdate.idCat)  // вывод пола
            {
                case 1:
                    RBGenderM.IsChecked = true;
                    break;
                case 2:
                    RBGenderW.IsChecked = true;
                    break;
            }
            if (path!=null)  // вывод картинки
            {
                BitmapImage BI = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                ImageCat.Source = BI;
            }
            LBTraits.ItemsSource = BaseClass.Base.Traits.ToList();  // ассоциируем коллекцию списка с чертами характера с соответствующей таблицей БД (Traits)
            LBTraits.SelectedValuePath = "idTrait";
            LBTraits.DisplayMemberPath = "Trait";
            List<TraitsCats> TC = TraitsCatsList.Where(x => x.idCat == CatUpdate.idCat).ToList();  // находим черты характера того кота, которого мы редактируем
            // цикл для выделения черт характера кота в общем списке:
            foreach (Traits t in LBTraits.Items)
            {
                if (TC.FirstOrDefault(x => x.idTrait == t.idTrait) != null)
                {
                    LBTraits.SelectedItems.Add(t);
                }
            }
            LBDiets.ItemsSource = BaseClass.Base.FeedCat.ToList();  // ассоциируем коллекцию списка с кормами кота с соответствующей таблицей БД (Diets)
            List<Diets> D = DietsList.Where(x => x.idCat == CatUpdate.idCat).ToList();  // находим корма для того кота, которого мы редактируем
            // цикл для отображения кормов и их количества для кота:
            foreach (FeedCat f in LBDiets.Items)
            {
                Diets obj = D.FirstOrDefault(x => x.idFeed == f.idFeed);
                if (obj!=null)
                {
                    f.QM = obj.QuantityMonth;
                }
            }
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
                CAT.СatName = TBName.Text;
                CAT.Breed = Name = TBBreed.Text;
                CAT.CatDateBirtр = DPDate.DisplayDate.Date;
                CAT.IDGender = gender; 
                CAT.CatPhoto = path;
                if (flag==true) {
                    BaseClass.Base.Cats.Add(CAT);  // добавляем запись в модель БД
                }
                BaseClass.Base.SaveChanges();  // сохраняем изменения в БД
                // Для заполнения таблицы TraitsCats нужно организовать цикл, так как черт характера у кота может быть несколько
                // Цикл будет организовывать по чертам характера, которые выделены в списке
                List<TraitsCats> LTC = TraitsCatsList.Where(x => x.idCat == CAT.idCat).ToList();  // находим список черт характера кота
                if (LTC.Count != 0)  // если список не пустой, удаляем из него все черты характера  этого кота
                {
                    foreach (TraitsCats tc in LTC)
                    {
                        BaseClass.Base.TraitsCats.Remove(tc);
                    }
                }
                foreach (Traits t in LBTraits.SelectedItems)  // перезаписываем черты кота (или добавляем черты для нового кота)
                {
                    TraitsCats TC = new TraitsCats();  // объект для записи в таблицу TraitsCat
                    TC.idCat = CAT.idCat;  // idCat берем из объекта Cat, созданного выше
                    TC.idTrait = t.idTrait;  // idTrait берем из объекта TC, по которому организовываем цикл
                    BaseClass.Base.TraitsCats.Add(TC); // добавляем запись в модель БД
                }
                BaseClass.Base.SaveChanges();  // когда все черты добалены, сохраняем изменения в БД
                                               // Для заполнения таблицы Diets нужно организовать цикл, так как кормов у кота может быть несколько
                                               // Цикл будет организовывать по всем кормам, которые есть в списке

                List<Diets> LD = DietsList.Where(x => x.idCat == CAT.idCat).ToList();  // находим список с кормами для кота
                if (LD.Count != 0)  // если список не пустой, удаляем из него все корма для  этого кота
                {
                    foreach (Diets d in LD)
                    {
                        BaseClass.Base.Diets.Remove(d);
                    }
                }
                foreach (FeedCat FC in LBDiets.Items)  // перезаписываем корма для кота (или добавляем корма для нового кота)
                {
                    Diets D = new Diets(); // объект для записи в таблицу Diets
                    D.idFeed = FC.idFeed;   // idFeed берем из объекта D, по которому организовываем цикл
                    D.idCat = CAT.idCat;  // idCat берем из объекта Cat, созданного выше
                    D.QuantityMonth = FC.QM;  // значения будет брать из свойства в созданном частичном классе FeedCapPartial
                    if (D.QuantityMonth != 0)
                    {
                        BaseClass.Base.Diets.Add(D); // добавляем запись в модель БД
                    }
                }
                BaseClass.Base.SaveChanges();  // когда все корма добалены, сохраняем изменения в БД
                MessageBox.Show("Данные записаны");
                FrameClass.FrameMain.Navigate(new ListCatsPage());
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
