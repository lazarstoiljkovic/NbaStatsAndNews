using MongoDB.Bson;
using MongoDB.Driver;
using NbaStatsAndNews.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbaStatsAndNews
{
    public class DataProvider
    {
        private static string connectionString="mongodb://localhost:27017";


        //metoda za dodavanje utakmica u mongoDb bazu
        public void addGames(List<Game> games)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var database = client.GetDatabase("NbaStats");

            var collection = database.GetCollection<Game>("Games");
            collection.InsertMany(games);

        }

        //metoda koja za specificirani datu brise sve utakmice
        public void deleteGamesFromDate(string date)
        {

            var client = new MongoClient(DataProvider.connectionString);
            var database = client.GetDatabase("NbaStats");
            var collection = database.GetCollection<Game>("Games");
            int count = collection.Find(x => x.date.Equals(date)).ToList().Count();


            for (int i = 0; i < count; i++)
            {
                collection.FindOneAndDelete(x => x.date.Equals(date));
            }

        }

        //funkcija koja vraca listu utakmica za specificirani datum
        public List<GameView> GetGamesCollection(string date)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var database = client.GetDatabase("NbaStats");
            var collection = database.GetCollection<Game>("Games");
            var collection1 = database.GetCollection<Team>("Teams");
            var result = collection.Find(x => x.date.Equals(date)).ToList();
            

            if(result.Count==0)
            {
                return null;
            }

            List<Game> gamesCollection = result;
            List<GameView> gameViews = new List<GameView>();

            foreach(Game g in gamesCollection)
            {
                GameView gameView = new GameView();
                gameView.gameId = g.gameId;
                gameView.date = g.date;
                gameView.time = g.time;
                gameView.matchStatus = g.matchStatus;
                gameView.homeTeamScores = g.homeTeamScores;
                gameView.awayTeamScores = g.awayTeamScores;

                var home = collection1.Find(t => t._teamId.Equals(g.homeTeamId)).First();
                gameView.homeTeam = new Team();
                gameView.homeTeam._teamId = home._teamId;
                gameView.homeTeam.name = home.name;
                gameView.homeTeam.logo = home.logo;

                var away = collection1.Find(t => t._teamId.Equals(g.awayTeamId)).First();
                gameView.awayTeam = new Team();
                gameView.awayTeam._teamId = away._teamId;
                gameView.awayTeam.name = away.name;
                gameView.awayTeam.logo = away.logo;

                gameViews.Add(gameView);

            }

            return gameViews;
        }

        //funkcija koja vraca utakmicu sa zadatim id-jem
        public GameView getGame(int id)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var database = client.GetDatabase("NbaStats");
            var collection = database.GetCollection<Game>("Games");
            var collection1 = database.GetCollection<Team>("Teams");
            var game = collection.Find(g => g.gameId.Equals(id)).First();

            GameView gameView = new GameView();

            gameView.gameId = game.gameId;
            gameView.date = game.date;
            gameView.time = game.time;
            gameView.matchStatus = game.matchStatus;
            gameView.homeTeamScores = game.homeTeamScores;
            gameView.awayTeamScores = game.awayTeamScores;

            gameView.comments = new List<string>();
            foreach (string comment in game.comments)
            {
                gameView.comments.Add(comment);
            }

            gameView.listH2H = new List<H2H>();
            foreach(H2H h2h in game.listH2H)
            {
                H2H h2H = new H2H();
                h2H.homeTeamPoints = h2h.homeTeamPoints;
                h2H.awayTeamPoints = h2h.awayTeamPoints;

                gameView.listH2H.Add(h2H);
            }

                var home = collection1.Find(t => t._teamId.Equals(game.homeTeamId)).First();
            gameView.homeTeam = new Team();
            gameView.homeTeam._teamId = home._teamId;
            gameView.homeTeam.name = home.name;
            gameView.homeTeam.logo = home.logo;
            gameView.homeTeam.forPoints = home.forPoints;
            gameView.homeTeam.againstPoints = home.againstPoints;

            gameView.homeTeam.playedGames = new ThreeParts();
            gameView.homeTeam.wins = new ThreeParts();
            gameView.homeTeam.draws = new ThreeParts();
            gameView.homeTeam.loses = new ThreeParts();

            gameView.homeTeam.playedGames.home = home.playedGames.home;
            gameView.homeTeam.playedGames.away = home.playedGames.away;
            gameView.homeTeam.playedGames.all = home.playedGames.all;

            gameView.homeTeam.wins.home = home.wins.home;
            gameView.homeTeam.wins.away = home.wins.away;
            gameView.homeTeam.wins.all = home.wins.all;

            gameView.homeTeam.draws.home = home.draws.home;
            gameView.homeTeam.draws.away = home.draws.away;
            gameView.homeTeam.draws.all = home.draws.all;

            gameView.homeTeam.loses.home = home.loses.home;
            gameView.homeTeam.loses.away = home.loses.away;
            gameView.homeTeam.loses.all = home.loses.all;

            var away = collection1.Find(t => t._teamId.Equals(game.awayTeamId)).First();
            gameView.awayTeam = new Team();

            gameView.awayTeam._teamId = away._teamId;
            gameView.awayTeam.name = away.name;
            gameView.awayTeam.logo = away.logo;
            gameView.awayTeam.forPoints = away.forPoints;
            gameView.awayTeam.againstPoints = away.againstPoints;

            gameView.awayTeam.playedGames = new ThreeParts();
            gameView.awayTeam.wins = new ThreeParts();
            gameView.awayTeam.draws = new ThreeParts();
            gameView.awayTeam.loses = new ThreeParts();

            gameView.awayTeam.playedGames.home = away.playedGames.home;
            gameView.awayTeam.playedGames.away = away.playedGames.away;
            gameView.awayTeam.playedGames.all = away.playedGames.all;

            gameView.awayTeam.wins.home = away.wins.home;
            gameView.awayTeam.wins.away = away.wins.away;
            gameView.awayTeam.wins.all = away.wins.all;

            gameView.awayTeam.draws.home = away.draws.home;
            gameView.awayTeam.draws.away = away.draws.away;
            gameView.awayTeam.draws.all = away.draws.all;

            gameView.awayTeam.loses.home = away.loses.home;
            gameView.awayTeam.loses.away = away.loses.away;
            gameView.awayTeam.loses.all = away.loses.all;



          

            return gameView;

        }

        //funkcija koja u okviru kolekcije Games u jsonArray comments dodaje komentar za specificiranu utakmicu
        public void addComment(int gameId, string comment)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var database = client.GetDatabase("NbaStats");
            var collection = database.GetCollection<Game>("Games");
            var gameToUpdate = collection.Find(g => g.gameId == gameId).First();
            gameToUpdate.comments.Add(comment);

            collection.ReplaceOne(c => c._id == gameToUpdate._id, gameToUpdate);


        }

        //funkcija koja za prosledjenu listu utakmica azurira te utakmice u bazi
        public void updateGames(IEnumerable<Game> games)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var db = client.GetDatabase("NbaStats");
            var collection = db.GetCollection<Game>("Games");

            foreach (Game g in games)
            {
                var gameToUpdate = collection.Find(x => x.gameId == g.gameId).First();
                gameToUpdate.matchStatus = g.matchStatus;
                gameToUpdate.homeTeamScores = g.homeTeamScores;
                gameToUpdate.awayTeamScores = g.awayTeamScores;
                gameToUpdate.listH2H = g.listH2H;

                collection.ReplaceOne(c => c._id == gameToUpdate._id, gameToUpdate);

            }

        }

        //funkcija koja za prosledjenu listu timova azurira te timove u bazi
        public void updateTeams(IEnumerable<Team> teams)
        {
            var client = new MongoClient(DataProvider.connectionString);
            var db = client.GetDatabase("NbaStats");
            var collection = db.GetCollection<Team>("Teams");

            foreach (Team t in teams)
            {
                var teamToUpdate = collection.Find(x => x._teamId == t._teamId).First();
                teamToUpdate.playedGames.home = t.playedGames.home;
                teamToUpdate.playedGames.away = t.playedGames.away;
                teamToUpdate.playedGames.all = t.playedGames.all;

                teamToUpdate.wins.home = t.wins.home;
                teamToUpdate.wins.away = t.wins.away;
                teamToUpdate.wins.all = t.wins.all;

                teamToUpdate.draws.home = t.draws.home;
                teamToUpdate.draws.away = t.draws.away;
                teamToUpdate.draws.all = t.draws.all;

                teamToUpdate.loses.home = t.loses.home;
                teamToUpdate.loses.away = t.loses.away;
                teamToUpdate.loses.all = t.loses.all;

                teamToUpdate.forPoints.home = t.forPoints.home;
                teamToUpdate.forPoints.all = t.forPoints.all;
                teamToUpdate.forPoints.away = t.forPoints.away;

                teamToUpdate.againstPoints.home = t.againstPoints.home;
                teamToUpdate.againstPoints.away = t.againstPoints.away;
                teamToUpdate.againstPoints.all = t.againstPoints.all;

                collection.ReplaceOne(c => c._id == teamToUpdate._id, teamToUpdate);

            }
        }
    }
}