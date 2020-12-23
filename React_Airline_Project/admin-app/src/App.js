import React, {Component} from 'react';
import './App.css';
import { BrowserRouter, Route } from 'react-router-dom';
import AdminUser from './Components/AdminUser';
import Navbar from './Components/Navbar';
import RequestInbox from './Components/RequestInbox';
import CustomerEdit from './Components/CustomerEdit';
import AirlineEdit from './Components/AirlineEdit';


class App extends Component{
  render(){

  
  return (
    <div className="App">

                <BrowserRouter>
                  <Navbar/>
                  <Route exact path='/' component={AdminUser}/>
                  <Route exact path='/RequestInbox' component={RequestInbox}/>
                  <Route exact path='/CustomerEdit' component={CustomerEdit}/>
                  <Route exact path='/AirlineEdit' component={AirlineEdit}/>
                </BrowserRouter>
    </div>
  );
  }
}

export default App;
