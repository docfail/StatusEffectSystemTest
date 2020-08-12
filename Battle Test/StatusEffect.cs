using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    interface StatusEffect : BattleEventObserver
    {
        int RemainingTurns();
        int GetID();
        StatusEffect Combine(StatusEffect effect);
        void Tick();
        void SetOwner(Combatant victim);
        
        void OnEnd();
    };
}
