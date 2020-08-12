using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class StatusFire : StatusEffect
    {
        private int ID = 1;
        private int turnsLeft;
        private Combatant owner;


        public StatusFire(Combatant victim, int turns)
        {
            owner = victim;
            turnsLeft = turns;
            BattleEventProvider.battleEventProvider.Subscribe(this);
        }
        public StatusEffect Combine(StatusEffect effect)
        {
            if(effect is StatusFire)
            {
                Unsubscribe();
                effect.Unsubscribe();
                return new StatusFire(owner,turnsLeft+effect.RemainingTurns());
            } else
            {
                throw new Exception("STATUS_EFFECT_ERROR: You cannot combine different status effects.");
            }
        }

        public int GetID()
        {
            return ID;
        }

        public void OnEventNotified(BattleEvent value)
        {
            if ((value.type == BattleEvent.EVENT_TYPE.PLAYER_TURN_START || value.type == BattleEvent.EVENT_TYPE.ENEMY_TURN_START) && (value.data.target == owner) && (turnsLeft > 0))
            {
                owner.TakeDamage(2, this);
                turnsLeft--;
                if (Program.VERY_DEBUG) Console.WriteLine($"Fire ticked. Turns Left: {turnsLeft}");
            }
        }

        public int RemainingTurns()
        {
            return turnsLeft;
        }

        public void SetOwner(Combatant victim)
        {
            owner = victim;
        }

        public void Tick()
        {
        }

        public void Unsubscribe()
        {
            BattleEventProvider.battleEventProvider.Unsubscribe(this);
        }

        public override string ToString()
        {
            return "Fire";
        }

        public void OnEnd()
        {
            throw new NotImplementedException();
        }
    }
}
