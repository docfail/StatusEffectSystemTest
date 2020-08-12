using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test.Battle_Test
{
    class Enemy : Combatant
    {
        public Enemy(string name)
        {
            this.name = name;
            activeEffects = new Dictionary<int, StatusEffect>();
        }
    }
}
