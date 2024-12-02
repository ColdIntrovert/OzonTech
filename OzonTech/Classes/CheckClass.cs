using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OzonTech.Classes
{
    public class CheckClass
    {
        public void CheckString( TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            if (char.IsDigit(e.Text[0]))
            {
                // Если это число, отменяем ввод
                e.Handled = true;
            }
        }
    }
}
