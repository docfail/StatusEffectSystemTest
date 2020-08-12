using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class StatusHaste : StatusEffect
    {
        private int ID = 3;
        private Combatant owner;
        private int turnsLeft;
        private int hasteLevel;
        public StatusHaste(Combatant own, int numTurns, int hLevel)
        {
            owner = own;
            if (own != null) own.turnsPerRound += hLevel;
            turnsLeft = numTurns;
            hasteLevel = hLevel;
            BattleEventProvider.battleEventProvider.Subscribe(this);
        }

        public StatusEffect Combine(StatusEffect effect)
        {
            if (!(effect is StatusHaste))
            {
                throw new Exception("STATUS_EFFECT_ERROR: You cannot combine different status effects.");
            }
            else
            {
                Unsubscribe();
                effect.Unsubscribe();
                if (((StatusHaste)effect).hasteLevel > hasteLevel)
                {
                    return new StatusHaste(owner, ((StatusHaste)effect).turnsLeft, ((StatusHaste)effect).hasteLevel);
                }
                else
                {
                    return new StatusHaste(owner, (turnsLeft > ((StatusHaste)effect).turnsLeft) ? turnsLeft : ((StatusHaste)effect).turnsLeft, hasteLevel);
                }
            }
        }

        public int GetID()
        {
            return ID;
        }

        public void OnEnd()
        {
        }

        public void OnEventNotified(BattleEvent data)
        {
            if ((data.type == BattleEvent.EVENT_TYPE.ENEMY_TURN_START || data.type == BattleEvent.EVENT_TYPE.PLAYER_TURN_START) && (data.data.target == owner) && turnsLeft > 0)
            {
                turnsLeft--;
                if (Program.VERY_DEBUG) Console.WriteLine($"Haste ticked. Turns Left: {turnsLeft}");
            }
        }

        public int RemainingTurns()
        {
            return turnsLeft;
        }

        public void SetOwner(Combatant own)
        {
            owner = own;
            owner.turnsPerRound += hasteLevel;
        }

        public void Tick()
        {
        }

        public void Unsubscribe()
        {
            owner.turnsPerRound -= 1;
            BattleEventProvider.battleEventProvider.Unsubscribe(this);
        }

        public override string ToString()
        {
            return "Haste";
        }
    }
}
