using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class Player : Combatant
    {
        public Player(string name)
        {
            this.name = name;
            activeEffects = new Dictionary<int, StatusEffect>();
        }
    }
}
