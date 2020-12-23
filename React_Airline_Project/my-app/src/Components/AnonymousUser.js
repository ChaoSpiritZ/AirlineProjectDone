import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

class AnonymousUser extends Component{

    render()
    {

        return(
            <div>
                
                <nav>
                    <div className="nav-wrapper">
                    <h6  className="brand-logo">Flight Center</h6>
                    <ul id="nav-mobile" className="right hide-on-med-and-down">
                        {/* <li><NavLink to="/Page/SearchFlights">Search Flights</NavLink></li> */} {/* is this how i connect to the razors from here?*/}
                        <li><a href="http://localhost:9002/page/searchflights">Search Flights</a></li>
                        <li><a href="http://localhost:9002/page/departingflights">Departing Flights</a></li>
                        <li><a href="http://localhost:9002/page/landingflights">Landing Flights</a></li>
                    </ul>
                    <ul id="nav-mobile" className="left hide-on-med-and-down">
                        <li className="blue"><NavLink to="/">Login</NavLink></li>
                    </ul>
                    </div>
                </nav>
        


                <div className="left-align">
                
                </div>

                <h1>Anonymous User Page</h1>

                To purchase tickets, please login.
            </div>
        )
    }

}

export default AnonymousUser