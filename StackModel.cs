using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class StackModel
    {
        private int _stackId;
        private string _stackName;

        public int StackId
        {
            get { return _stackId; }
            set { _stackId = value; }
        }
        public string StackName
        {
            get { return _stackName; }
            set { _stackName = value; }
        }
    }
}
