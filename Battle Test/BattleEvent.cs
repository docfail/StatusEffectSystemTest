using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class BattleEvent
    {
        public enum EVENT_TYPE
        {
            ROUND_START,
            ROUND_END,
            PLAYER_ROUND_START,
            PLAYER_ROUND_END,
            ENEMY_ROUND_START,
            ENEMY_ROUND_END,
            PLAYER_TURN_START,
            PLAYER_TURN_END,
            ENEMY_TURN_START,
            ENEMY_TURN_END,
            COMBATANT_ATTACKS,
            COMBATANT_ATTACKED,
            COMBATANT_DAMAGED,
            COMBATANT_DEFEATED
        };
        public EVENT_TYPE type;
        public struct EventData
        {
            public enum EVENT_SOURCE_TYPE
            {
                SCENERY,
                COMBATANT,
                STATUS_EFFECT,
            }
            public Combatant target;
            public int turn;
            public EVENT_SOURCE_TYPE sourceType;
            public Combatant source;
            public StatusEffect sourceEffect;
        };
        public EventData data;

        public BattleEvent(EVENT_TYPE type, EventData data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
