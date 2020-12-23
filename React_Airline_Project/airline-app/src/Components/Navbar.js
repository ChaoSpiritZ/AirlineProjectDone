import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

class Navbar extends Component{

    state = {
        userInfo : {}
    }

    componentDidMount = () => {
           //need to add setItem here for it to work until all pages are on the same port
        localStorage.setItem("userinfo", '{"type":"Airline","id":"329","airlineName":"shorter Airline Name","userName":"myEditedAirline123","password":"logan2","countryCode":"323","countryName":"Angola"}')
        //localStorage.setItem("firstName", "Haim")
        console.log("airline info:")
        console.log(localStorage.getItem("userinfo"))
        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo"))
        })
        // const userInfo = JSON.parse(localStorage.getItem("userinfo"))
         //console.log("user info: " + JSON.stringify(this.state))


         //BUT CAN I EDIT IN THE EDIT PAGE TWICE WITHOUT REFRESHING???
    }

    render()
    {
        
        

        return(
            <div>

                <nav>
                    <div className="nav-wrapper blue">
                    <h6  className="brand-logo"><NavLink to="/">Flight Center</NavLink></h6>
                    <ul id="nav-mobile" className="right hide-on-med-and-down">
                        <li className="teal"><NavLink to="/AddFlight">Add Flight</NavLink></li>
                        <li className="teal"><NavLink to="/MyFlights">My Flights</NavLink></li>
                        {/* <li><NavLink to="/Page/DepartingFlights">Departing Flights</NavLink></li> */} {/* is this how i connect to the razors from here?*/}
                        <li><a href="http://localhost:9002/page/searchflights">Search Flights</a></li>
                        <li><a href="http://localhost:9002/page/departingflights">Departing Flights</a></li>
                        <li><a href="http://localhost:9002/page/landingflights">Landing Flights</a></li>
                        <li className="red"><a href="http://localhost:3001/">Logout</a></li>
                    </ul>
                    <ul id="nav-mobile" className="left hide-on-med-and-down ">
                        
                        <li> <NavLink to="/">Welcome, {this.state.userInfo.airlineName} Airline!</NavLink></li>
                        <li className="orange darken-3"><NavLink to="/AirlineEdit">Edit</NavLink></li>
                        <li className="orange darken-3"><NavLink to="/ChangePassword">Change Password</NavLink></li>
                    </ul>
                    </div>
                </nav>

            </div>
        )
    }

}

export default Navbar