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
            LBTraits.ItemsSource = BaseClass.Base.Traits.ToList();
            LBTraits.SelectedValuePath = "idTrait";
            LBTraits.DisplayMemberPath = "Trait";
            LBDiets.ItemsSource = BaseClass.Base.FeedCat.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   try {
                int gender = 0;
                if (RBGenderM.IsChecked == true)
                {
                    gender = 1;
                }
                if (RBGenderW.IsChecked == true)
                {
                    gender = 2;
                }
                Cats Cat = new Cats() { СatName = TBName.Text, Breed = Name = TBBreed.Text, CatDateBirtр = DPDate.DisplayDate.Date, IDGender = gender, CatPhoto = path };
                BaseClass.Base.Cats.Add(Cat);
                BaseClass.Base.SaveChanges();
                foreach (Traits t in LBTraits.SelectedItems)
                {
                    TraitsCats TC = new TraitsCats();
                    TC.idCat = Cat.idCat;
                    TC.idTrait = t.idTrait;
                    BaseClass.Base.TraitsCats.Add(TC);
                }
         //   BaseClass.Base.SaveChanges();
            foreach (FeedCat FC in LBDiets.Items)
                {
                    Diets D = new Diets();
                    D.idFeed = FC.idFeed;
                    D.idCat = Cat.idCat;
                    D.QuantityMonth = FC.QM;
                    BaseClass.Base.Diets.Add(D);
                }
                BaseClass.Base.SaveChanges();
                MessageBox.Show("Данные записаны");
         //   }
         //   catch
         //   {
               // MessageBox.Show("Данные не записаны");
         //   }
        }           
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            path = OFD.FileName;
            int n = path.IndexOf("PhotoCat");
            path = path.Substring(n);
        }
    }
}
