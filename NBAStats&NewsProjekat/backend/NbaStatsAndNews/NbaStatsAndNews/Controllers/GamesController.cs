using NbaStatsAndNews.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NbaStatsAndNews.Controllers
{
    [RoutePrefix("Api/game")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GamesController : ApiController
    {

        private RapidApi rapidApi = new RapidApi();
        private DataProvider dataProvider = new DataProvider();

        [Route("getGamesYesterday")]
        [HttpGet]
        public IEnumerable<GameView> getGameYesterday()
        {
            //ovde treba da skida podatke od juce ali zbog testiranja bice specificiran konretan datum
            //DateTime dt = DateTime.Now;
            //DateTime yesterday = new DateTime(dt.Year, dt.Month, dt.Day - 1);

            DateTime dt1 = new DateTime(2020, 02, 04);
            List<GameView> gamesCollections = dataProvider.GetGamesCollection(dt1.ToString("yyyy'-'MM'-'dd"));

            return gamesCollections;
        }


        [Route("getGames")]
        [HttpGet]
        public IEnumerable<GameView> getGames()
        {

            //i ovde treba da skida za danasnji i jucerasnji datum
            //DateTime today = DateTime.Now;
            //DateTime yesterday = new DateTime(today.Year, today.Month, today.Day - 10);

            DateTime dt1 = new DateTime(2020, 02, 05);
            DateTime dt2 = new DateTime(2020, 02, 04);
            List<GameView> gamesCollections = dataProvider.GetGamesCollection(dt1.ToString("yyyy'-'MM'-'dd")); 
            
            //ovo je slucaj da za danasnji dan nema utakmica u bazi 
            //potrebno je skinuti utakmice sa rapidApi-ja za danasnji dan,takodje potrebno je skinuti i za jucerasnji dan 
            //i azurirati bazu,na kraju je potrebno i obrisati utakmice od pre dva dana iz baze
            if(gamesCollections==null)
            {

                DateTime today = DateTime.Now;
                DateTime yesterday = new DateTime(dt1.Year, dt1.Month, dt1.Day - 1);
                IEnumerable<Game> games = rapidApi.getGamesFromOneDate(dt1.ToString("yyyy'-'MM'-'dd"));
                List<Game> listGames = new List<Game>();
                foreach (Game game in games)
                {
                    Game g = new Game();
                    g.gameId = game.gameId;
                    g.date = game.date;
                    g.time = game.time;
                    g.matchStatus = game.matchStatus;
                    g.homeTeamId = game.homeTeamId;
                    g.awayTeamId = game.awayTeamId;
                    g.homeTeamScores = game.homeTeamScores;
                    g.awayTeamScores = game.awayTeamScores;
                    g.comments = game.comments;
                    g.listH2H = game.listH2H;

                    listGames.Add(g);
                }
                dataProvider.addGames(listGames);

                IEnumerable<Game> gamesYesterday= rapidApi.getGamesFromOneDate(dt2.ToString("yyyy'-'MM'-'dd"));

                List<Game> listGames1 = new List<Game>();
                foreach (Game game in gamesYesterday)
                {
                    Game g = new Game();
                    g.gameId = game.gameId;
                    g.date = game.date;
                    g.time = game.time;
                    g.matchStatus = game.matchStatus;
                    g.homeTeamId = game.homeTeamId;
                    g.awayTeamId = game.awayTeamId;
                    g.homeTeamScores = game.homeTeamScores;
                    g.awayTeamScores = game.awayTeamScores;
                    g.comments = game.comments;
                    g.listH2H = game.listH2H;

                    listGames1.Add(g);
                }
                dataProvider.addGames(listGames1);


                //IEnumerable<Team> teams = rapidApi.getTeamsStats();
                //dataProvider.updateTeams(teams);
                //dataProvider.deleteGamesFromDate(dt2.ToString("yyyy'-'MM'-'dd"));

                List<GameView> gamesView = dataProvider.GetGamesCollection(dt1.ToString("yyyy'-'MM'-'dd"));
                return gamesView;
            }
            else
            {
                
                return gamesCollections; 
            }
            


            
        }

        [Route("getGames/{id}")]
        [HttpGet]

        public GameView getGame(int id)
        {
            return dataProvider.getGame(id);
        }

        [Route("addComment")]
        [HttpPost]
        public List<string> addComment(Message message)
        {
            dataProvider.addComment(message.gameId, message.comment);
            GameView gameView = dataProvider.getGame(message.gameId);

            List<string> lista = new List<string>();

            foreach(string com in gameView.comments)
            {
                lista.Add(com);
            }

            return lista;
        }

    }
}
