import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

class Navbar extends Component{

    state = {
        userInfo : {}
    }

    componentDidMount = () => {
        //need to add setItem here for it to work until all pages are on the same port
        
        localStorage.setItem("userinfo", '{"type":"Customer","id":"367","firstName":"Belinda","lastName":"Dupont","userName":"ticklishdog163","password":"wives","address":"belinda.dupont@example.com","phoneNo":"077 962 83 18","creditCardNumber":"23b557d0-c96d-4697-b0fd-21770c43959e"}')
        //localStorage.setItem("firstName", "Haim")
        console.log("customer info")
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
                    <li className="purple"><NavLink to="/MyTickets">My Tickets</NavLink></li>
                        <li><a href="http://localhost:9002/page/searchflights">Search Flights</a></li> 
                        <li><a href="http://localhost:9002/page/departingflights">Departing Flights</a></li> 
                        {/* <NavLink to="/Page/DepartingFlights">Departing Flights</NavLink> */} {/* is this how i connect to the razors from here?*/}
                        <li><a href="http://localhost:9002/page/landingflights">Landing Flights</a></li>
                        {/* <li className="red"><a href="http://localhost:3001/">Logout</a></li>   */}
                        <li className="red"><a href="http://localhost:9002/">Logout</a></li>  {/*  clears local storage on logout */}
                    </ul>
                    <ul id="nav-mobile" className="left hide-on-med-and-down ">
                        
                        <li> <NavLink to="/">Welcome, {this.state.userInfo.firstName}!</NavLink></li>
                        <li className="orange darken-3"><NavLink to="/CustomerEdit">Edit</NavLink></li>
                    </ul>
                    </div>
                </nav>

            </div>
        )
    }

}

export default Navbar