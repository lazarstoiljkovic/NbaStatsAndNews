import React from 'react';
import logo from './logo.svg';
import './App.css';



import createHistory from 'history/createBrowserHistory';
import {Switch,Route,Router} from 'react-router-dom';
import Home from './Home';
import Game from './Game';



import Button from 'react-bootstrap/Button';


const history = createHistory();   


class App extends React.Component {
  constructor(props){
    super(props);

  }

  render() {
    return (
    <Router history={history} forceRefresh={true}>
      <div id="continer">
      
      <Switch>
            
            <Route exact path='/' component={Home}  />
            <Route path="/game/:id" component={Game} />  

    
      </Switch>

      </div>

    </Router>

  );
  }
}

export default App;
