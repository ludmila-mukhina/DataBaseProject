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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.FrameMain.Navigate(new CreateOrUpdatePage());
        }
    }
}
