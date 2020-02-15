using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbaStatsAndNews.POCO
{
    [Serializable]
    public class Game
    {
        public ObjectId _id { get; set; }
        public int gameId { get; set; }

        public string date { get; set; }

        public string time { get; set; }
        public string matchStatus { get; set; }

        public int homeTeamId { get; set; }

        public int awayTeamId { get; set; }

        public int homeTeamScores { get; set; }
        public int awayTeamScores { get; set; }

        public List<string> comments { get; set; }

        public List<H2H> listH2H { get; set; }


        public Game()
        {
            this.comments = new List<string>();
            this.listH2H = new List<H2H>();
        }

    }

    public class H2H
    {
        public int homeTeamPoints { get; set; }
        public int awayTeamPoints { get; set; }
    } 


}