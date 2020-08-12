using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    internal abstract class Combatant : BattleEventObserver
    {
        public string name;

        public int turnsPerRound = 1;

        public int attack = 1;

        public int defense = 0;

        public bool isRooted = false;

        public float hitChance = 1.0f;

        public Dictionary<int, StatusEffect> activeEffects;

        // TODO: Collapse these into more condensed overloads. They all post the same event, its just a question of the data.
        virtual public void TakeDamage(int damage, StatusEffect src)
        {
            Console.WriteLine($"{name} took {damage} damage from {src.ToString()}!");
            BattleEvent.EventData data = new BattleEvent.EventData()
            {
                target = this,
                sourceType = BattleEvent.EventData.EVENT_SOURCE_TYPE.STATUS_EFFECT,
                sourceEffect = src
            };
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.COMBATANT_DAMAGED, data));
        }
        virtual public void TakeDamage(int damage, string src)
        {
            Console.WriteLine($"{name} took {damage} damage from {src}!");
            BattleEvent.EventData data = new BattleEvent.EventData()
            {
                target = this,
                sourceType = BattleEvent.EventData.EVENT_SOURCE_TYPE.SCENERY
            };
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.COMBATANT_DAMAGED, data));
        }
        virtual public void TakeDamage(int damage, Combatant src)
        {
            Console.WriteLine($"{name} took {damage} damage from {src.name}!");
            BattleEvent.EventData data = new BattleEvent.EventData()
            {
                target = this,
                sourceType = BattleEvent.EventData.EVENT_SOURCE_TYPE.COMBATANT,
                source = src
            };
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.COMBATANT_DAMAGED, data));
        }

        virtual public void GainStatus(StatusEffect effect)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{name} has been inflicted with the \"{effect}\" status for {effect.RemainingTurns()} turns!");
            Console.ForegroundColor = ConsoleColor.White;
            effect.SetOwner(this);
            int ID = effect.GetID();
            if (activeEffects.ContainsKey(ID)) {
                activeEffects[ID] = effect.Combine(activeEffects[ID]);
            } else {
                activeEffects.Add(effect.GetID(), effect);
            }
        }

        public void PurgeEndedStatuses()
        {
            List<int> removeList = new List<int>();
            foreach(KeyValuePair<int, StatusEffect> status in activeEffects)
            {
                if(status.Value.RemainingTurns() < 1)
                {
                    removeList.Add(status.Key);
                    status.Value.Unsubscribe();
                }
            }
            foreach(var status in removeList)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{name} got over the \"{activeEffects[status]}\" status!");
                Console.ForegroundColor = ConsoleColor.White;
                activeEffects.Remove(status);
            }
        }

        virtual public void Tick()
        {
            PurgeEndedStatuses();
        }

        virtual public void OnEventNotified(BattleEvent value)
        {
            return;
        }

        public void Unsubscribe()
        {
            BattleEventProvider.battleEventProvider.Unsubscribe(this);
        }
    }
}
