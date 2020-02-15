import React from 'react';
import "./Home.scss"
import {BrowserRouter as Router, useParams} from 'react-router-dom';
import Route from 'react-router-dom/Route'; 
import './Game.scss';
import {NavLink} from 'react-router-dom'; 
import {Navbar,Nav, NavDropdown, NavItem, Button} from 'react-bootstrap';
import ReactDOM from 'react-dom';
import {Bar,Line,Pie} from 'react-chartjs-2';


class Match extends React.Component{
    constructor(props){
        super(props);
        this.state={
            isLoaded: false,
            match: null
        }
    }

    intervalID;

    componentDidMount(){

        this.getData();

        
        

    }



    getData(){
        fetch('https://localhost:44304/Api/game/getGames/'+this.props.match.params.id)
        .then(res => res.json())
        .then(
            (result) =>{
                this.setState({
                    isLoaded: true,
                    match: result

                });
            }
        )

    }

    livePredictionsOnClick(){
        ReactDOM.render(<Comments dataFromParent={this.state.match} />,document.getElementById('divKontejner'));
        console.log("live prediction");
    }

    liveChatOnClick(){
        ReactDOM.render(<H2H dataFromParent={this.state.match} />,document.getElementById('divKontejner'));
        console.log("liveChat");
    }

    details(){
        ReactDOM.render(<Statistics dataFromParent={this.state.match} />,document.getElementById('divKontejner'));
    }


    render(){
        const {isLoaded, match}=this.state;
        if(!isLoaded){
            return <div>Loading...</div>
        }
        else
        {
            return(

                <div id="glavniDiv">
                    <div id="divGornji">
                        <div id="divDomaciTim">
                            <div id="divLogoDomaciTim">
                                <img src={match.homeTeam.logo}></img>
                            </div>
                            <div id="divNazivDomaciTim">
                                {match.homeTeam.name}
                            </div>

                        </div>
                        <div id="divPodaci">
                            <div id="divDatum">
                                <div id="date">
                                    {match.date}
                                </div>
                                <div id="time">
                                    {match.time}
                                </div>
                            </div>
                            <div id="divRezultat">
                                <div id="goloviDomaciTim">
                                    <a>
                                        {match.homeTeamScores}
                                    </a>
                                   
                                </div>
                                <div id="crtica">
                                    -
                                </div>
                                <div id="goloviGostujuciTim">
                                    <a>
                                        {match.awayTeamScores}

                                    </a>
                                    
                                </div>
                            </div>
                            <div id="divStatus">
                                <div id="matchStatus">
                                    {match.matchStatus}
                                </div>
                            </div>

                        </div>
                        <div id="divGostujuciTim">
                            <div id="divLogoGostujuciTim">
                                <img src={match.awayTeam.logo}></img>
                            </div>
                            <div id="divNazivGostujuciTim">
                                {match.awayTeam.name}
                            </div>

                        </div>

                    </div>

                    <div id="divSrednji">
                        <div id="divNavigacioniBar">
                            <ul className="menu">
                                <li id="li-livePredictions">
                                    <span>
                                        <a id="a-livePredictions" href="#" onClick={this.livePredictionsOnClick.bind(this)}>CommentsAndNews</a>
                                    </span>

                                </li>

                                <li id="li-liveChat">
                                    <span>
                                        <a id="a-liveChat" href="#" onClick={this.liveChatOnClick.bind(this)}>H2H</a>
                                    </span>
                                </li>

                                <li id="li-details">
                                    <span>
                                        <a id="a-details" href="#" onClick={this.details.bind(this)}>Statistics</a>
                                    </span>
                                </li>
                            </ul>
                            
                        </div>

                    </div>

                    <div id="divDonji">

                        <div id="divKontejner">

                        </div>

                    </div>
                    
                </div>
            )

        }


    }
    

}

class Comments extends React.Component{
    constructor(props)
    {
        super(props);

        this.state={
            isLoaded:true,
            comment: "",
            comments:[]
        }
    }

    componentDidMount(){
        this.setState({comments:this.props.dataFromParent.comments});
    }

    handleComment(ev){
        this.setState({comment: ev.target.value});
    }

    clickSubmitComment(){

        fetch('https://localhost:44304/Api/game/addComment',{
            method: 'post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body : JSON.stringify({
                comment:this.state.comment,
                gameId:this.props.dataFromParent.gameId
            })
        }).then((Response) => Response.json())
            .then((result) =>{
                console.log(result);
                this.setState({comments:result});

            })

    }


    render(){
        const  match = this.props.dataFromParent;
        const comments=this.state.comments;

        return(
            <div id="componentComments">
                <div id="divComments">
                    {
                        this.state.comments.map((string)=>(
                            <div id="jedanKomentar">
                                {string}
                            </div>
                            
                        ))
                    }
                </div>
                <br></br>
                <div id="addComment">
                        <textarea className="form-control" rows="8" onChange={this.handleComment.bind(this)}>
                        </textarea>
                        <br></br>
                        <Button type="submit" className="btn btn-primary" onClick={this.clickSubmitComment.bind(this)}>Submit</Button>
                </div>

            </div>

        )

    }
}

class H2H extends React.Component{
    constructor(props)
    {
        super(props);
        this.state={
            listH2H:[],
            homeTeam:"",
            awayTeam:""
        }
    }

    componentDidMount(){
        this.setState({listH2H:this.props.dataFromParent.listH2H});
        this.setState({homeTeam:this.props.dataFromParent.homeTeam.name});
        this.setState({awayTeam:this.props.dataFromParent.awayTeam.name});
    }

    render(){
        return(
            <div id="H2H">
                {
                    this.state.listH2H.map((item)=>(
                        <div id="oneMatch">
                            <div id="homeTeam">
                                {this.state.homeTeam}
                            </div>
                            <div id="result">
                                {item.homeTeamPoints} - {item.awayTeamPoints}
                            </div>
                            <div id="awayTeam">
                                {this.state.awayTeam}
                            </div>
                        </div>
                    ))
                }

            </div>
        )
    }

}

class Statistics extends React.Component{
    constructor(props)
    {
        super(props);
        this.state={
            gameInfo:null
        }
    }

    componentDidMount(){
        this.setState({gameInfo:this.props.dataFromParent})
    }

    render(){
        return(
            <div id="divStatistika">
                <div id="red">
                    <Chart1 homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.wins.home,this.props.dataFromParent.awayTeam.wins.home]}
                    naziv={"Home Wins"}>

                    </Chart1>

                    <Chart1 homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.loses.home,this.props.dataFromParent.awayTeam.loses.home]}
                    naziv={"Home Loses"}>

                    </Chart1>

                </div>

                <div id="red">
                    <Chart1 homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.wins.away,this.props.dataFromParent.awayTeam.wins.away]}
                    naziv={"Away Wins"}>

                    </Chart1>

                    <Chart1 homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.loses.away,this.props.dataFromParent.awayTeam.loses.away]}
                    naziv={"Away Loses"}>

                    </Chart1>

                </div>

                <div id="red">
                    <Chart homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.forPoints.home,this.props.dataFromParent.awayTeam.forPoints.home]}
                    naziv={"For Points Home"}>

                    </Chart>

                    <Chart homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.forPoints.away,this.props.dataFromParent.awayTeam.forPoints.away]}
                    naziv={"For Points Away"}>

                    </Chart>

                </div>

                <div id="red">
                    <Chart homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.againstPoints.home,this.props.dataFromParent.awayTeam.againstPoints.home]}
                    naziv={"Against Points Home"}>

                    </Chart>

                    <Chart homeTeam={this.props.dataFromParent.homeTeam.name} 
                    awayTeam={this.props.dataFromParent.awayTeam.name}
                    podaci={[this.props.dataFromParent.homeTeam.againstPoints.away,this.props.dataFromParent.awayTeam.againstPoints.away]}
                    naziv={"Against Points Away"}>

                    </Chart>

                </div>
                
            </div>
        )
    }

}

class Chart1 extends React.Component{
    constructor(props){
        super(props);
        this.state={
            chartData:{
                labels: [this.props.homeTeam,this.props.awayTeam],
                datasets:[
                    {
                        label: this.props.naziv,
                        data:[
                            this.props.podaci[0],
                            this.props.podaci[1]
                        ],
                        backgroundColor:[
                            'rgba(222,113,44,0.6)',
                            'rgba(78,234,99,0.6)'
                        ],
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 2,
                        borderColor: 'red'
                    }
                ]
            },

        }
    }

    render(){
        return (
            <div className="chart">
                <Pie data={this.state.chartData} width={400} height={350} options={{

                    title:{
                        display: true,
                        text:this.props.naziv,
                        fontSize: 15
                    },
                    legend:{
                        display:true,
                        position: 'top'
                    }
                }}>

                </Pie>

            </div>
        )
    }
}

class Chart extends React.Component{
    constructor(props){
        super(props);
        this.state={
            chartData:{
                labels: [this.props.homeTeam,this.props.awayTeam],
                datasets:[
                    {
                        label: this.props.naziv,
                        data:[
                            this.props.podaci[0],
                            this.props.podaci[1]
                        ],
                        backgroundColor:[
                            'rgba(222,113,44,0.6)',
                            'rgba(78,234,99,0.6)'
                        ],
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 2,
                        borderColor: 'green'
                    }
                ]
            },

        }
    }

    render(){
        return (
            <div className="chart">
                <Bar data={this.state.chartData} width={400} height={350} options={{

                    title:{
                        display: true,
                        fontSize: 15
                    },
                    legend:{
                        display:true,
                        position: 'top'
                    }
                }}>

                </Bar>

            </div>
        )
    }

}





export default Match;