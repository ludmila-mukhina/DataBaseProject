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
        List<Cats> CatsStart = BaseClass.Base.Cats.ToList();
        PageChange pc = new PageChange();  // создаем объект класса для отображения страниц
        public ListCatsPage()
        {
            InitializeComponent();
            LVCats.ItemsSource = CatsStart;
            TblCount.Text = CatsStart.Count + "";  // отоборажение исходного количества записей в списке
            List<GenderTable> GB = BaseClass.Base.GenderTable.ToList();
            CBFilter.Items.Add("Все");  // добавление в Combobox элемента "Все записи"
            for (int i=0; i<GB.Count(); i++)  // добавление в элеметов вз списка GB (в котором хранятся сведения о поле)
            {
                CBFilter.Items.Add(GB[i].Genger);
            }
            CBFilter.SelectedIndex = 0;  // по умолчанию будет выбран в Combobox элемент "Все"
            DataContext = pc;  // добавляем объект для отображения страниц в ресурсы страницы
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
        List<Cats> CatsFilter;  // список для хранения отфильтрованных данных
        private void Filter()  // метод для фильтрации по всем введенным параметрам
        {
            int index = CBFilter.SelectedIndex;  
            if (index != 0) // определения выбранного элемента из Combobox
            {
                CatsFilter = CatsStart.Where(x => x.IDGender == index).ToList();
            }
            else
            {
                CatsFilter = CatsStart;  // если ни один из эл-тов Combobox не выбран (по умолчанию в этом случае будут все записи)
            }
            if (!string.IsNullOrWhiteSpace(TBFilter.Text))  // будет происходить фильтрация только если текстовое поле не пустое или не заполнено пробелами
            {
                CatsFilter = CatsFilter.Where(x => x.Breed.ToLower().Contains(TBFilter.Text)).ToList();
            }
            if (ChBFilter.IsChecked==true)  //  фильтрация будет осуществляться в зависимости от того, активен флажок, или нет
            {
                CatsFilter = CatsFilter.Where(x => x.CatPhoto != null).ToList();
            }
            LVCats.ItemsSource = CatsFilter;  // отображение нового отфильтрованного списка в шаблоне
            TblCount.Text = CatsFilter.Count() + "";  // отображения количества записей, найденных в результате фильтрации
        }
        private void CBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
        
        private void TBFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void ChBFilter_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void ChBFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void BtnSortUp_Click(object sender, RoutedEventArgs e)  // сортировка по возрастанию
        {
            CatsFilter.Sort((x, y) => x.СatName.CompareTo(y.СatName));
            LVCats.Items.Refresh();
        }

        private void BtnSortDown_Click(object sender, RoutedEventArgs e)  // сортировка по убыванию
        {
            CatsFilter.Sort((x, y) => x.СatName.CompareTo(y.СatName));
            CatsFilter.Reverse();
            LVCats.Items.Refresh();
        }

        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(txtPageCount.Text); // если в текстовом поле есnь значение, присваиваем его свойству объекта, которое хранит количество записей на странице
            }
            catch
            {
                pc.CountPage = CatsFilter.Count; // если в текстовом поле значения нет, присваиваем свойству объекта, которое хранит количество записей на странице количество элементов в списке
            }
            pc.Countlist = CatsFilter.Count;  // присваиваем новое значение свойству, которое в объекте отвечает за общее количество записей
            LVCats.ItemsSource = CatsFilter.Skip(0).Take(pc.CountPage).ToList();  // отображаем первые записи в том количестве, которое равно CountPage
        }

        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)  // обработка нажатия на один из Textblock в меню с номерами страниц
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)  // определяем, куда конкретно было сделано нажатие
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            LVCats.ItemsSource = CatsFilter.Skip(pc.CurrentPage*pc.CountPage-pc.CountPage).Take(pc.CountPage).ToList();  // оображение записей постранично с определенным количеством на каждой странице
            // Skip(pc.CurrentPage* pc.CountPage - pc.CountPage) - сколько пропускаем записей
            // Take(pc.CountPage) - сколько записей отображаем на странице
        }
    }
}
