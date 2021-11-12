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
    /// Логика взаимодействия для ListCatsPage.xaml
    /// </summary>
    public partial class ListCatsPage : Page
    {
        
        public ListCatsPage()
        {
            InitializeComponent();
            LVCats.ItemsSource = BaseClass.Base.Cats.ToList();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            List<TraitsCats> TC = BaseClass.Base.TraitsCats.Where(x => x.idCat == index).ToList();
            string str = "";
            foreach (TraitsCats item in TC)
            {
                str += item.Traits.Trait + ", ";
            }
            tb.Text = str.Substring(0, str.Length - 2);
        }

        private void TextBlock_Loaded_1(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            List<Diets> TC = BaseClass.Base.Diets.Where(x => x.idCat == index).ToList();
            int sum = 0;
            foreach(Diets item in TC)
            {
                sum += item.QuantityMonth * item.FeedCat.Pricefeed;
            }
            tb.Text = sum + " рублей";
        }

        private void Button_Click(object sender, RoutedEventArgs e) // переход на форму для добавления кота
        {
            FrameClass.FrameMain.Navigate(new CreateOrUpdatePage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // кнопка удаления кота из списка
        {
            Button B = (Button)sender; // задаем кнопке имя
            int ind = Convert.ToInt32(B.Uid); // считываем индекс кнопки, который соответсвует id кота
            Cats CatDelete = BaseClass.Base.Cats.FirstOrDefault(y => y.idCat == ind); // находим кота с соответствующим индексом
            BaseClass.Base.Cats.Remove(CatDelete);  // удаляем кота
            BaseClass.Base.SaveChanges();
            FrameClass.FrameMain.Navigate(new ListCatsPage());  // перезагружаем страницу, переходя на нее же саму
            MessageBox.Show("Запись удалена");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)  // кнопка для редактирования, ведет на ту же страницу, где находится форма добавления кота
        {
            Button B = (Button)sender;  // задаем кнопке имя
            int ind = Convert.ToInt32(B.Uid);  // считываем индекс кнопки, который соответсвует id кота
            Cats CatUpdate = BaseClass.Base.Cats.FirstOrDefault(y => y.idCat == ind);  // находим кота с соответствующим индексом
            FrameClass.FrameMain.Navigate(new CreateOrUpdatePage(CatUpdate));  // переходим на страницу с формой добавления, которую будем использовать и для редактирования
            // Обратите внимание, что конструктор в этом случае не пустой. Он содержит того кота, который соотвествует нужному индексу
        }
    }
}
