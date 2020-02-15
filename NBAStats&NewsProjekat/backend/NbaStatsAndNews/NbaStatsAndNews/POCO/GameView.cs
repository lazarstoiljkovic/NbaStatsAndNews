using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbaStatsAndNews.POCO
{
    public class GameView
    {
        public int gameId { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string matchStatus { get; set; }
        public Team homeTeam { get; set; }
        public Team awayTeam { get; set;}

        public int homeTeamScores { get; set; }
        public int awayTeamScores { get; set; }

        public List<string> comments { get; set; }

        public List<H2H> listH2H { get; set; }

        public GameView()
        {
            this.comments = new List<string>();
            this.listH2H = new List<H2H>();
        }
    }

    public class TeamView
    {
        public int teamId { get; set; }
        public string name { get; set; }
        public string logo { get; set; }

    }
}