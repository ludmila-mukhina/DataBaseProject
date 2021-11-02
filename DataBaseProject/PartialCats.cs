using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataBaseProject
{
    public partial class Cats
    {
        public string CatGender
        {
            get
            {
                switch (IDGender)
                {
                    case 1:
                        return "Кот " + СatName;
                    case 2:
                        return "Кошка " + СatName;
                    default:
                        return "Пол не определен";                           
                }
            }
        }
        public SolidColorBrush GenderColor
        {
            get
            {
                switch (IDGender)
                {
                    case 1:
                        return Brushes.LightBlue;
                    case 2:
                        return Brushes.LightPink;
                    default:
                        return Brushes.White;
                }
            }
        }
    }
}
