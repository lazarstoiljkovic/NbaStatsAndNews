using NbaStatsAndNews.POCO;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbaStatsAndNews
{
    public class RapidApi
    {
        private const string rapidApiHost = "api-basketball.p.rapidapi.com";
        private const string rapidApiKey = "2c5232c9b1msh2bbbb76d7d41940p154704jsn65e2d7bd06b8";
        private const string rapidApiURL = "https://api-basketball.p.rapidapi.com/";


        //preuzimanje svih utakmica za zadati datum sa rapidApi-ja
        public IEnumerable<Game> getGamesFromOneDate(string date)
        {

            IEnumerable<Game> games = Enumerable.Empty<Game>();
            var client = new RestClient(RapidApi.rapidApiURL + "games?season=2019-2020&league=12&date="
                    + date);


            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", RapidApi.rapidApiHost);
            request.AddHeader("x-rapidapi-key", RapidApi.rapidApiKey);
            IRestResponse response = client.Execute(request);



            IList<Game> lista = new List<Game>();

            JObject responseJObject = JObject.Parse(response.Content);
            JArray gamesArray = (JArray)responseJObject["response"];

            foreach(JObject json in gamesArray)
            {
                Game game = new Game();
                game.gameId = int.Parse(json["id"].ToString());
                game.date = date;
                game.time = json["time"].ToString();
                JObject status = (JObject)json["status"];
                game.matchStatus = status["long"].ToString();
                JObject homeTeam = (JObject)((JObject)json["teams"])["home"];
                JObject awayTeam = (JObject)((JObject)json["teams"])["away"];
                game.homeTeamId = int.Parse(homeTeam["id"].ToString());
                game.awayTeamId = int.Parse(awayTeam["id"].ToString());

                JObject homeTeamScores = (JObject)((JObject)json["scores"])["home"];
                JObject awayTeamScores = (JObject)((JObject)json["scores"])["away"];

                if (homeTeamScores["total"].Type == Newtonsoft.Json.Linq.JTokenType.Null)
                {
                    game.homeTeamScores = 0;
                }
                else
                {
                    game.homeTeamScores = int.Parse(homeTeamScores["total"].ToString());
                }

                if (awayTeamScores["total"].Type == Newtonsoft.Json.Linq.JTokenType.Null)
                {
                    game.awayTeamScores = 0;
                }
                else
                {
                    game.awayTeamScores = int.Parse(awayTeamScores["total"].ToString());
                }

                

                game.comments = new List<string>();

  

                game.listH2H = this.getH2H(int.Parse(homeTeam["id"].ToString()), int.Parse(awayTeam["id"].ToString()));

                lista.Add(game);


            }

            games = lista;
            return games;
        }

        //preuzimanje timova i azurirane statistike sa rapidApi-ja
        public IEnumerable<Team> getTeamsStats()
        {
            IEnumerable<Team> teams = Enumerable.Empty<Team>();

            List<Team> lista = new List<Team>();

            for(int i = 132; i < 162; i++)
            {
                var client = new RestClient(RapidApi.rapidApiURL + "statistics?league=12&season=2019-2020&team="+i);
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-host", RapidApi.rapidApiHost);
                request.AddHeader("x-rapidapi-key", RapidApi.rapidApiKey);
                IRestResponse response = client.Execute(request);

                JObject responseJObject = JObject.Parse(response.Content);
                JObject json = (JObject)responseJObject["response"];

                JObject jsonObjectTeam = (JObject)json["team"];
                JObject jsonObjectGames = (JObject)json["games"];
                JObject jsonObjectPoints = (JObject)json["points"];

                Team team = new Team();
                team._teamId = int.Parse(jsonObjectTeam["id"].ToString());
                team.name = jsonObjectTeam["name"].ToString();

                team.playedGames = new ThreeParts();
                team.wins= new ThreeParts();
                team.draws = new ThreeParts();
                team.loses = new ThreeParts();
                team.forPoints = new ThreeParts();
                team.againstPoints = new ThreeParts();

                JObject jsonObjectGamesPlayed = (JObject)jsonObjectGames["played"];
                JObject jsonObjectGamesWins = (JObject)jsonObjectGames["wins"];
                JObject jsonObjectGamesDraws = (JObject)jsonObjectGames["draws"];
                JObject jsonObjectGamesLoses = (JObject)jsonObjectGames["loses"];

                team.playedGames.home = int.Parse(jsonObjectGamesPlayed["home"].ToString());
                team.playedGames.away = int.Parse(jsonObjectGamesPlayed["away"].ToString());
                team.playedGames.all = int.Parse(jsonObjectGamesPlayed["all"].ToString());

                team.wins.home = int.Parse(((JObject)jsonObjectGamesWins["home"])["total"].ToString());
                team.wins.away = int.Parse(((JObject)jsonObjectGamesWins["away"])["total"].ToString());
                team.wins.all = int.Parse(((JObject)jsonObjectGamesWins["all"])["total"].ToString());

                team.loses.home = int.Parse(((JObject)jsonObjectGamesLoses["home"])["total"].ToString());
                team.loses.away = int.Parse(((JObject)jsonObjectGamesLoses["away"])["total"].ToString());
                team.loses.all = int.Parse(((JObject)jsonObjectGamesLoses["all"])["total"].ToString());

                team.draws.home = int.Parse(((JObject)jsonObjectGamesDraws["home"])["total"].ToString());
                team.draws.away = int.Parse(((JObject)jsonObjectGamesDraws["away"])["total"].ToString());
                team.draws.all = int.Parse(((JObject)jsonObjectGamesDraws["all"])["total"].ToString());

                JObject jsonObjectPointsFor = (JObject)jsonObjectPoints["for"];
                JObject jsonObjectPointsAgainst = (JObject)jsonObjectPoints["against"];

                JObject jsonObjectPointsForTotal = (JObject)jsonObjectPointsFor["total"];
                JObject jsonObjectPointsAgainstTotal = (JObject)jsonObjectPointsAgainst["total"];

                team.forPoints.home = int.Parse(jsonObjectPointsForTotal["home"].ToString()); 
                team.forPoints.away = int.Parse(jsonObjectPointsForTotal["away"].ToString());
                team.forPoints.all = int.Parse(jsonObjectPointsForTotal["all"].ToString());

                team.againstPoints.home = int.Parse(jsonObjectPointsAgainstTotal["home"].ToString());
                team.againstPoints.away = int.Parse(jsonObjectPointsAgainstTotal["away"].ToString());
                team.againstPoints.all = int.Parse(jsonObjectPointsAgainstTotal["all"].ToString());


                lista.Add(team);
            }

            teams = lista;

            return teams;


        }

        public List<H2H> getH2H(int homeTeamId,int awayTeamId)
        {
            var client1 = new RestClient(RapidApi.rapidApiURL + "games?h2h=" + homeTeamId + "-" + awayTeamId);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("x-rapidapi-host", RapidApi.rapidApiHost);
            request1.AddHeader("x-rapidapi-key", RapidApi.rapidApiKey);
            IRestResponse response1 = client1.Execute(request1);

            JObject responseJObject1 = JObject.Parse(response1.Content);
            JArray h2hArray = (JArray)responseJObject1["response"];

            List<H2H> h2Hs = new List<H2H>();

            foreach (JObject jObject in h2hArray)
            {
                H2H h2H = new H2H();
                JObject homeTeam1 = (JObject)((JObject)jObject["scores"])["home"];
                JObject awayTeam1 = (JObject)((JObject)jObject["scores"])["away"];
                if (homeTeam1["total"].Type != Newtonsoft.Json.Linq.JTokenType.Null && 
                    awayTeam1["total"].Type != Newtonsoft.Json.Linq.JTokenType.Null)
                {
                    h2H.homeTeamPoints = int.Parse(homeTeam1["total"].ToString());
                    h2H.awayTeamPoints = int.Parse(awayTeam1["total"].ToString());
                    h2Hs.Add(h2H);
                }


                
            }

            return h2Hs;

        }




    }
}