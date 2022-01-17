using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.GameModes;
using WillsWackyManagers.Utils;

namespace InvertedRounds
{
    class AddCurseToWinnerPatch
    {
        static int numTeams = 0;
        static int[] teamScores;
        internal static IEnumerator patch()
        {
            if(numTeams == 0)
            {
                numTeams = PlayerManager.instance.players.Select(p => p.teamID).Distinct().Count();
                teamScores = new int[numTeams];
            }
            int winningteam = -1;

            for(int i = 0; i < numTeams; i++)
            {
                TeamScore newScore = GameModeManager.CurrentHandler.GetTeamScore(i);
                if (newScore.rounds != teamScores[i])
                {
                    winningteam = i;
                    teamScores[i] = newScore.rounds;
                    break;
                }
            }

            Player[] winningPlayers = PlayerManager.instance.GetPlayersInTeam(winningteam);

            foreach (Player person in winningPlayers)
            {
                for(int i = 0; i < InvertedRoundsPlugin.CursesPerWin; i++)
                {
                    CurseManager.instance.CursePlayer(person, (curse) => { ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(person, curse); });
                }
            }
            yield break;
        }

        internal static IEnumerator reset()
        {
            numTeams = 0;
            yield break;
        }
    }
}
