using Observer_Test.Battle_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test
{
    class Battle
    {
        int currentTurn = 1;
        Dictionary<string, Combatant> combatants = new Dictionary<string, Combatant>();
        Random rng = new Random();
        List<string> players = new List<string>();
        public Battle(int chars, int ens)
        {
            Console.WriteLine("Please enter your characters' names.");
            for (int i = chars; i > 0; i--)
            {
                Console.Write($"What is Character{i}'s name? ");
                var nm = Console.ReadLine();
                combatants.Add(nm, new Player(nm));
                players.Add(nm);
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Enemies:");
            for (int i = ens; i > 0; i--)
            {
                combatants.Add($"Enemy{i}", new Enemy($"Enemy{i}"));
                Console.WriteLine($"Enemy{i}");
            }
        }
        public void AddCombatant(Combatant c)
        {
            combatants.Add(c.name, c);
        }
        public void Tick()
        {
            //TODO: If you care, go ahead and make this the way it operates. For now there's no reason to waste the time.
        }

        public void Round()
        {
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ROUND_START, new BattleEvent.EventData() { turn = currentTurn }));
            //Player Round:
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.PLAYER_ROUND_START, new BattleEvent.EventData() { turn = currentTurn }));
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            // Player Turn:
            foreach (Combatant player in from c in combatants where c.Value is Player select c.Value)
            {
                BattleEvent.EventData playerData = new BattleEvent.EventData
                {
                    target = player,
                    turn = currentTurn
                };
                BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.PLAYER_TURN_START, playerData));
                if (player.isRooted) Console.WriteLine($"{player.name} is rooted and can't be rotated from their spot!\n...\nThis would matter if that system was implemented here, I promise!");
                for (int i = player.turnsPerRound; i > 0; i--)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"What do you want {player.name} to do? (attack, charge, haste, inflict poison, inflict fire)");
                    string input = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    string[] cmd_details = input.Split(' ');
                    switch (cmd_details[0].ToLower())
                    {
                        case "attack":
                            Console.WriteLine($"{player.name} attacked {combatants[cmd_details[1]].name}!");
                            combatants[cmd_details[1]].TakeDamage(player.attack, player);
                            BattleEvent.EventData attackData = new BattleEvent.EventData
                            {
                                target = combatants[cmd_details[1]],
                                turn = currentTurn,
                                source = player,
                                sourceType = BattleEvent.EventData.EVENT_SOURCE_TYPE.COMBATANT
                            };
                            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.COMBATANT_ATTACKS, attackData));
                            break;
                        case "inflict":
                            switch (cmd_details[1])
                            {
                                case "poison":
                                    Console.WriteLine($"You poisoned {combatants[cmd_details[2]].name}!");
                                    combatants[cmd_details[2]].GainStatus(new StatusPoison(null, 2, 1));
                                    break;
                                case "fire":
                                    Console.WriteLine($"You set {combatants[cmd_details[2]].name} on fire!");
                                    combatants[cmd_details[2]].GainStatus(new StatusFire(null, 2));
                                    break;
                                default:
                                    Console.WriteLine($"Error: \"{cmd_details[1]}\" is not a valid status.");
                                    break;
                            }
                            break;
                        case "charge":
                            int clevel = 1;
                            player.GainStatus(new StatusCharge(null, clevel));
                            Console.WriteLine($"{player.name} charged by {clevel}! They now have {((StatusCharge)player.activeEffects[2]).charge} levels of charge!");
                            break;
                        case "haste":
                            int numTurns = 1;
                            int hlevel = 1;
                            player.GainStatus(new StatusHaste(null,numTurns, hlevel));
                            Console.WriteLine($"{player.name} has hasted themselves. What a cheater!!!");
                            break;
                        default:
                            Console.WriteLine($"Error: \"{cmd_details[0]}\" is not a valid command.");
                            break;
                    }
                    BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.PLAYER_TURN_END, playerData));
                    player.Tick();
                }
            }
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.PLAYER_ROUND_END, new BattleEvent.EventData() { turn = currentTurn }));
            //Enemy Round:
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ENEMY_ROUND_START, new BattleEvent.EventData() { turn = currentTurn }));
            Console.BackgroundColor = ConsoleColor.DarkRed;
            foreach (KeyValuePair<string,Combatant> enemy in from c in combatants where c.Value is Enemy select c)
            {
                BattleEvent.EventData data = new BattleEvent.EventData
                {
                    target = enemy.Value,
                    turn = currentTurn
                };
                BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ENEMY_TURN_START, data));
                Player targetPlayer = (Player)combatants[players[rng.Next(players.Count)]];
                Console.WriteLine($"{enemy.Value.name} attacked {targetPlayer.name}!");
                targetPlayer.TakeDamage(1, enemy.Value);
                BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ENEMY_TURN_END, data));
                enemy.Value.Tick();
            }
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ENEMY_ROUND_END, new BattleEvent.EventData() { turn = currentTurn }));
            BattleEventProvider.battleEventProvider.PostEvent(new BattleEvent(BattleEvent.EVENT_TYPE.ROUND_END, new BattleEvent.EventData() { turn = currentTurn }));
            currentTurn++;
        }
    }
}
