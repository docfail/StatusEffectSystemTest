using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class BattleEventProvider
    {
        public static BattleEventProvider battleEventProvider = new BattleEventProvider();
        private List<BattleEventObserver> observers = new List<BattleEventObserver>();
        // So a funny thing happens when you try to unsubscribe while in the middle of PostEvent - 
        // the program crashes because the list was modified in the middle of a foreach loop.
        // This was an issue with the MSDN version too, not just this newer implementation.
        // The solution? Don't let things modify that list in the middle of posting. 
        // A queue for things that need to be unsubscribed and a boolean semaphore to determine 
        // when it's an appropriate time. This solves the issue.
        private Queue<BattleEventObserver> unsubscribeQueue = new Queue<BattleEventObserver>();
        private bool observerListLocked = false;
        public void Subscribe(BattleEventObserver observer)
        {
            observers.Add(observer);
        }

        public void PostEvent(BattleEvent evnt)
        {
            if (Program.VERY_DEBUG) Console.WriteLine($"Event Posted of Type:{evnt.type}");
            observerListLocked = true;
            foreach (var observer in observers)
            {
                observer.OnEventNotified(evnt);
            }
            observerListLocked = false;
            while (unsubscribeQueue.Count > 0)
            {
                BattleEventObserver observer = unsubscribeQueue.Dequeue();
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }

        public void Unsubscribe(BattleEventObserver observer)
        {
            if (observers.Contains(observer))
            {
                if (observerListLocked)
                {
                    unsubscribeQueue.Enqueue(observer);
                } 
                else
                {
                    observers.Remove(observer);
                }
            }
        }
    }
}
