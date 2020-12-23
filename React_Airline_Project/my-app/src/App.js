import React, {Component} from 'react';
import './App.css';
import { BrowserRouter, Route } from 'react-router-dom';
import Login from './Components/Login';
import AnonymousUser from './Components/AnonymousUser';
import CreateCustomer from './Components/CreateCustomer';
import CreateAirlineCompany from './Components/CreateAirlineCompany';


class App extends Component{
  render(){

  
  return (
    <div className="App">

                <BrowserRouter>
                <Route exact path='/' component={Login}/>
                <Route path='/AnonymousUser' component={AnonymousUser}/>
                <Route path='/CreateCustomer' component={CreateCustomer}/>
                <Route path='/CreateAirlineCompany' component={CreateAirlineCompany}/>
                
                </BrowserRouter>
    </div>
  );
  }
}

export default App;
