using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class SetTableContext
    {
        private static TableToStackContextDTO _tableContext;
        public static TableToStackContextDTO TableContext
        {
            get { return _tableContext; }
            set { _tableContext = value; }
        }
    }
}
