using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class StatusCharge : StatusEffect
    {
        private int ID = 2;
        private int turnsLeft;
        public readonly int charge;
        private Combatant owner;

        public StatusCharge(Combatant own, int charge)
        {
            owner = own;
            if(own!=null) own.attack += charge;
            this.charge = charge;
            turnsLeft = 1;
            BattleEventProvider.battleEventProvider.Subscribe(this);
        }

        public StatusEffect Combine(StatusEffect effect)
        {
            if(effect is StatusCharge)
            {
                Unsubscribe();
                effect.Unsubscribe();
                return new StatusCharge(owner, charge + ((StatusCharge)effect).charge);
            } 
            else
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
            if(value.type == BattleEvent.EVENT_TYPE.COMBATANT_ATTACKS && value.data.source == owner)
            {
                turnsLeft = 0;
                Console.WriteLine($"{owner.name} used up their {charge} levels of charge!");
            }
        }

        public int RemainingTurns()
        {
            return turnsLeft;
        }

        public void SetOwner(Combatant victim)
        {
            owner = victim;
            owner.attack += charge;
        }

        public void Tick()
        {

        }

        public void Unsubscribe()
        {
            owner.attack -= charge;
            BattleEventProvider.battleEventProvider.Unsubscribe(this);
        }

        public void OnEnd()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Charge";
        }
    }
}
