using System;
using System.Timers;

namespace IotSimulator
{
    public class TimerController
    {
        private readonly string timerName;
        public Timer timer;
        public void InitTimer()
        {
            System.Console.WriteLine($"{timerName} inited");
            timer = new Timer() { Interval = 10_000 };
            timer.Elapsed += ElaspedTimerHandler;
        }

        private void ElaspedTimerHandler(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine($"{timerName} elapsed");
            TimerElapsed();
        }

        public event Action TimerElapsed;
        public TimerController(string timerName)
        {
            this.timerName = timerName;
        }

        public void StartTimer()
        {
            System.Console.WriteLine($"{timerName} started");
            timer.Start();
        }

        public void StopTimer(bool force = false)
        {
            System.Console.WriteLine($"{timerName} was manually stopped");

            if (force)
            {
                timer.Elapsed -= ElaspedTimerHandler;
            }
            timer.Stop();
        }
    }
}