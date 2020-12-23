import React, {Component} from 'react';
import './App.css';
import { BrowserRouter, Route } from 'react-router-dom';
import AirlineUser from './Components/AirlineUser';
import Navbar from './Components/Navbar';
import MyFlights from './Components/MyFlights';
import AirlineEdit from './Components/AirlineEdit';
import AddFlight from './Components/AddFlight';
import ChangePassword from './Components/ChangePassword';


class App extends Component{
  render(){

  
  return (
    <div className="App">

                <BrowserRouter>
                  <Navbar/>
                  <Route exact path='/' component={AirlineUser}/>
                  <Route exact path='/MyFlights' component={MyFlights}/>
                  <Route exact path='/AirlineEdit' component={AirlineEdit}/>
                  <Route exact path='/AddFlight' component={AddFlight}/>
                  <Route exact path='/ChangePassword' component={ChangePassword}/>
                </BrowserRouter>
    </div>
  );
  }
}

export default App;
