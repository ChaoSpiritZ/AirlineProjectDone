import React, {Component} from 'react';
import './App.css';
import { BrowserRouter, Route } from 'react-router-dom';
import CustomerUser from './Components/CustomerUser';
import Navbar from './Components/Navbar';
import MyTickets from './Components/MyTickets';
import CustomerEdit from './Components/CustomerEdit';
import BuyTicket from './Components/BuyTicket';


class App extends Component{
  render(){

  
  return (
    <div className="App">

                <BrowserRouter>
                  <Navbar/>
                  <Route exact path='/' component={CustomerUser}/>
                  <Route exact path='/MyTickets' component={MyTickets}/>
                  <Route exact path='/CustomerEdit' component={CustomerEdit}/>
                  <Route exact path='/BuyTicket' component={BuyTicket}/>
                </BrowserRouter>
    </div>
  );
  }
}

export default App;
