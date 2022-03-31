using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.StudyArea
{
    internal class TableToScoreContextDTO
    {
        public int ScoresID { get; set; }
        public string StackName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int Score { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
