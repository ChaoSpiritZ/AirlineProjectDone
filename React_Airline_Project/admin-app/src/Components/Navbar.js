import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

class Navbar extends Component{

    state = {
        userInfo : {}
    }

    componentDidMount = () => {
        //need to add setItem here for it to work until all pages are on the same port
        localStorage.setItem("firstName", "Isaac")
        console.log("first name: " + localStorage.getItem("firstName"))

        console.log("admin info:")
        localStorage.setItem("userinfo", '{"type":"Admin","userName":"admin","password":"9999"}')
        console.log(localStorage.getItem("userinfo"))
        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo"))
        })
    }

    render()
    {
        
        

        return(
            <div>

                <nav>
                    <div className="nav-wrapper blue">
                    <h6  className="brand-logo"><NavLink to="/">Flight Center</NavLink></h6>
                    <ul id="nav-mobile" className="right hide-on-med-and-down">
                        <li className="pink darken-2"><NavLink to="/CustomerEdit">Customer Edit</NavLink></li>
                        <li className="pink darken-2"><NavLink to="/AirlineEdit">Airline Edit</NavLink></li>
                        {/*<li><NavLink to="/Page/SearchFlights">Search Flights</NavLink></li>*/} {/* is this how i connect to the razors from here?*/}
                        <li><a href="http://localhost:9002/page/searchflights">Search Flights</a></li>
                        <li><a href="http://localhost:9002/page/departingflights">Departing Flights</a></li>
                        <li><a href="http://localhost:9002/page/landingflights">Landing Flights</a></li>
                        <li className="red"><a href="http://localhost:3001/">Logout</a></li>
                    </ul>
                    <ul id="nav-mobile" className="left hide-on-med-and-down ">
                        
                        <li> <NavLink to="/">Welcome, Admin {localStorage.getItem("firstName")}!</NavLink></li>
                        <li className="pink darken-2"><NavLink to="/RequestInbox">Request Inbox</NavLink></li>
                        
                    </ul>
                    </div>
                </nav>

            </div>
        )
    }

}

export default Navbar