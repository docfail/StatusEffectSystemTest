using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    interface BattleEventObserver
    {
        void OnEventNotified(BattleEvent data);
        void Unsubscribe();
    }
}
