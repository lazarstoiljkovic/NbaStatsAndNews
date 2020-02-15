using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbaStatsAndNews.POCO
{
    [Serializable]
    public class Team
    {
        public ObjectId _id { get; set; }

        public int _teamId { get; set; }

        public string name { get; set; }

        public string logo { get; set; }

        public ThreeParts playedGames { get; set; }
        public ThreeParts wins { get; set; }
        public ThreeParts draws { get; set; }
        public ThreeParts loses { get; set; }
        public ThreeParts forPoints { get; set; }
        public ThreeParts againstPoints { get; set; }

    }

    public class ThreeParts
    {
        public int home { get; set; }
        public int away { get; set; }
        public int all { get; set; }
    }
}