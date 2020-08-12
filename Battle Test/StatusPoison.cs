using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class StatusPoison : StatusEffect
    {
        private int ID = 0;
        private int turnsLeft;
        public readonly int intensity;
        private Combatant owner;

        public StatusPoison(Combatant victim, int turns, int intnsty)
        {
            owner = victim;
            turnsLeft = turns;
            intensity = intnsty;
            BattleEventProvider.battleEventProvider.Subscribe(this);
        }

        public StatusEffect Combine(StatusEffect effect)
        {
            if(!(effect is StatusPoison))
            {
                throw new Exception("STATUS_EFFECT_ERROR: You cannot combine different status effects.");
            } else
            {
                Unsubscribe();
                effect.Unsubscribe();
                if (((StatusPoison)effect).intensity > intensity)
                {
                    return new StatusPoison(owner, ((StatusPoison)effect).turnsLeft, ((StatusPoison)effect).intensity);
                }
                else
                {
                    return new StatusPoison(owner, (turnsLeft > ((StatusPoison)effect).turnsLeft) ? turnsLeft : ((StatusPoison)effect).turnsLeft, intensity);
                }
            }
        }

        public int GetID()
        {
            return ID;
        }

        public void OnEventNotified(BattleEvent value)
        {
            if((value.type==BattleEvent.EVENT_TYPE.PLAYER_TURN_END || value.type==BattleEvent.EVENT_TYPE.ENEMY_TURN_END) && (value.data.target == owner) && (turnsLeft > 0))
            {
                owner.TakeDamage(intensity, this);
                turnsLeft--;
                if(Program.VERY_DEBUG) Console.WriteLine($"Poison ticked. Turns Left: {turnsLeft}");
            }
        }

        public int RemainingTurns()
        {
            return turnsLeft;
        }

        public void Tick() // Is tick really needed?
        {
        }

        public void SetOwner(Combatant victim)
        {
            owner = victim;
        }
        public override string ToString()
        {
            return "Poison";
        }

        public void Unsubscribe()
        {
            BattleEventProvider.battleEventProvider.Unsubscribe(this);
        }

        public void OnEnd()
        {
            throw new NotImplementedException();
        }
    }
}
