using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace MakingAMillionBy30
{
    class IO
    {
        public List<string> GetDates()
        {
            List<string> dateList = new List<string>();
            string line;
            using (StreamReader file = new StreamReader("AchDates.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    dateList.Add(line);
                }
            }
            return dateList;
        }
    }
}
