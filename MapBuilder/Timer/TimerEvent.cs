using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilder.Timer
{
    class TimerEvent
    {
        public bool HasHappened { get; set; } = false;
        public string EventName { get; set; }
        public double TimeTaken { get; set; }
        public int NumberOfDrops { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }

        public TimerEvent(string name ,int drops)
        {
            EventName = name;
            NumberOfDrops = drops;
        }

        public void StartEvent()
        {
            StartTime = TimingManager.GetTime();
        }

        public void FinishEvent(bool write)
        {
            EndTime = TimingManager.GetTime();
            
            if(write)
            {
                WriteToFile();
            }
            

        }

        public void WriteToFile()
        {
            Reporter.Reporter.StartSection(EventName);

            Reporter.Reporter.WriteContent("Event Started at: " + StartTime + " Ended: " + EndTime, 0);
            Reporter.Reporter.WriteContent("Time Taken " + (EndTime - StartTime), 0);

            Reporter.Reporter.WriteContent("Event Had " + NumberOfDrops + " Drops", 0);
            Reporter.Reporter.WriteContent("Event Rating: " + DropRating(), 0);
        }

        public double DropRating()
        {
            return NumberOfDrops / (double)(EndTime-StartTime);
        }



    }
}
