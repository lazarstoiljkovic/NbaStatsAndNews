import React from 'react';
import "./Home.scss"
import {BrowserRouter as Router,Link} from 'react-router-dom';
import Route from 'react-router-dom/Route'; 
import Game from './Game.js';
import Button from 'react-bootstrap/Button';






class Home extends React.Component{
    constructor(props){
        super(props);
        this.state={
            isLoaded: false,
            games: []
        }
    }

    getDataToday(){
        fetch('https://localhost:44304/Api/game/getGames')
        .then(res => res.json())
        .then(
            (result) =>{
                this.setState({
                    isLoaded: true,
                    games: result

                });
            }
        )

    }

    getDataYesterday(){
        fetch('https://localhost:44304/Api/game/getGamesYesterday')
        .then(res => res.json())
        .then(
            (result) =>{
                this.setState({
                    isLoaded: true,
                    games: result

                });
            }
        )
    }


    componentDidMount(){
        this.getDataToday();
    }

    onTodayClick(){
        this.getDataToday();
    }

    onYesterdayClick(){
        this.getDataYesterday();
    }

    render(){
        
        const { isLoaded,games } = this.state;
        if(!isLoaded){
            return <div>Loading...</div>
        }
        else
        {
            return(
                <div id="all">
                    <div id="dugmici">
                        <div id="yesterday">
                            <Button type="button" onClick={this.onYesterdayClick.bind(this)}>
                                Yesterday
                            </Button>

                        </div>

                        <div id="today">
                            <Button type="button" onClick={this.onTodayClick.bind(this)}>
                                Today
                            </Button>
                        </div>



                    </div>
                    <div id="scoreTable">
                    {
                        games.map(item => (
                            <div id="matchScore" className={item.gameId}>
                                <div id="dateTime">
                                    <div id="date">

                                    </div>

                                    <div>

                                    </div>
                                </div>
                                <div id="matchStatus" >
                                    {item.matchStatus}
                                </div>
                                <div id="homeTeam">
                                    <div id="logo">
                                        <img src={item.homeTeam.logo}></img>
                                    </div>
                                    <div id="name">
                                        {item.homeTeam.name}
                                    </div>
                                </div>
                                <div id="homeTeamScore">
                                    {item.homeTeamScores}
                                </div>
                                <div>
                                    -
                                </div>
                                <div id="awayTeamScore">
                                    {item.awayTeamScores}
                                </div>
                                <div id="awayTeam">
                                    <div id="logo">
                                        <img src={item.awayTeam.logo}/>   
                                    </div>
                                    <div id="name">
                                        {item.awayTeam.name}
                                    </div>
                                </div> 
                                <Link to={"/game/"+item.gameId}>Open match</Link>
                            </div>
                        ))
                    }
                    </div>

                </div>
                
            )
        }
    }
}

export default Home;