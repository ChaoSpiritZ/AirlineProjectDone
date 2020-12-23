import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class MyFlights extends Component{

    state = {
        inputCss : {
            width : 130
        },
        userInfo : {},
        flights : [],
        countryList : [],

        flightToCancel : {}
        
    }

    componentDidMount = () => {

        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) },
            () => {
                Axios.get("http://localhost:9002/api/anonymousfacade/getallcountries").then((result) => {
                    //console.log(result.data)
                    //preparing the countries for the select/drop-down field
                    this.setState({
                        countryList : result.data
                    })
                    //console.log(this.state.countryList)
                })
        
                const username = this.state.userInfo.userName
                const password = this.state.userInfo.password
                const basicAuth = 'basic ' + btoa(username + ':' + password);
        
                Axios.get("http://localhost:9002/api/airlinefacade/getallflights",{headers: { 'Authorization': basicAuth}}).then((result) => {
                    if(result.status == 200){
                        this.setState({
                            flights : result.data
                        }, () => {
                            console.log(this.state.flights)
                        })
                    }
                }).catch((error) => {
                    if(error.response == null){
                        Swal.fire("Something went wrong!", "", "error")
                    }
                    else if(error.response.status == 400){
                    console.log(error.response.data.Message)
                    //alert(error.response.data.Message)
                    Swal.fire("Something went wrong!", error.response.data.Message, "error")
                    }
                    else{
                        console.log(error)
                    }
                })
            }
        )

        
    }

    handleOnClickModify = (e, newFlight) => {

        console.log(newFlight)
        //console.log(this.state.updatedAirline)

        const username = this.state.userInfo.userName
        const password = this.state.userInfo.password
        const basicAuth = 'basic ' + btoa(username + ':' + password);

        Axios.put("http://localhost:9002/api/airlinefacade/updateflight", newFlight, {headers: { 'Authorization': basicAuth}}).then((result) => {
            if(result.status == 200){
                console.log("flight " + newFlight.ID + " was modified successfully!")
                //alert("flight " + newFlight.ID + " was modified successfully!")
                Swal.fire("Flight " + newFlight.ID + " was modified successfully!", "", "success")
            }
        }).catch((error) => {
            if(error.response.status == 400){
            console.log(error.response.data.Message)
            //alert(error.response.data.Message)
            Swal.fire("Something went wrong!", error.response.data.Message, "error")
            }
            else{
                console.log(error)
            }
        })
    }

    handleOnClickDelete = (e) => {
        console.log("canceling flight " + e.target.id)
        this.setState({
            flightToCancel : this.state.flights.filter(f => f.ID == e.target.id)[0]
        }, () => {
            //console.log("about to get deleted: " + JSON.stringify(this.state.ticketToDelete))
            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.delete("http://localhost:9002/api/airlinefacade/cancelflight", { data : {ftc : this.state.flightToCancel} ,headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("flight " + this.state.flightToCancel.ID + " was CANCELLED successfully!")

                    const newFlightList = this.state.flights.filter(f => f.ID != this.state.flightToCancel.ID)

                    this.setState({
                        flights : newFlightList
                    })

                    //alert(("Flight " + this.state.flightToCancel.ID + " was CANCELLED successfully!"))
                    Swal.fire("Flight " + this.state.flightToCancel.ID + " was CANCELLED successfully!", "", "success")
                }
            }).catch((error) => {
                if(error.response.status == 400){
                console.log(error.response.data.Message)
                //alert(error.response.data.Message)
                Swal.fire("Something went wrong!", error.response.data.Message, "error")
                }
                else{
                    console.log(error)
                }
            })
        })
    }

    render()
    {
        const countryListHtml = this.state.countryList.map(c => {
            return (
            <option key={c.ID} value={c.ID}>{c.CountryName}</option>
            )
        })

        const flightsHtml = this.state.flights.map(f => {
            const originCountry = this.state.countryList.filter(c => c.ID == f.OriginCountryCode)[0]
            const destinationCountry = this.state.countryList.filter(c => c.ID == f.DestinationCountryCode)[0]
            return (
                <div key={f.ID}>
                    <div className="row" style={{"width" : "90%"}}>
                        <div className="col s12 m12">
                        <div className="card teal" style={{"height" : 525}}>
                            <div className="card-content white-text">
                            <span className="card-title">ID: {f.ID}</span>
                            <br/> <br/>
                            Number of Tickets: <input type="number" id={f.ID + "RemainingTickets"} defaultValue={f.RemainingTickets} style={this.state.inputCss}/> <br/>
                            Departure Time: ({f.DepartureTime}) =={">"} <input type="date" id={f.ID + "DepartureDate"} style={this.state.inputCss}/> &nbsp; &nbsp;
                                                                        <input type="time" id={f.ID + "DepartureClock"} style={this.state.inputCss}/> <br/>
                            Landing Time: ({f.LandingTime}) =={">"} <input type="date" id={f.ID + "LandingDate"} style={this.state.inputCss}/> &nbsp; &nbsp;
                                                                    <input type="time" id={f.ID + "LandingClock"} style={this.state.inputCss}/> <br/> <br/>
                            <span style={{marginLeft: -275}}>Origin Country: ({originCountry ? originCountry.CountryName : "Loading..."}) =={">"} </span>
                            <div className="container">
                                <div className="row" >
                                    <div className="input-field col s5" >
                                        <select id={f.ID + "originCountryCode"} name="originCountryCode" className="browser-default" style={{marginLeft:375, marginTop:-40}}>
                                            <option defaultValue="" disabled>Choose your option</option>
                                            {/*inserting all countries in here */}
                                            {countryListHtml}
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <span style={{marginLeft: -275}}>Destination Country: ({destinationCountry ? destinationCountry.CountryName : "Loading..."}) =={">"}</span>
                            <div className="container">
                                <div className="row" >
                                    <div className="input-field col s5" >
                                        <select id={f.ID + "destinationCountryCode"} name="destinationCountryCode" className="browser-default" style={{marginLeft:375, marginTop:-40}}>
                                            <option defaultValue="" disabled>Choose your option</option>
                                            {/*inserting all countries in here */}
                                            {countryListHtml}
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <button className="orange darken-3 btn" id={f.ID} onClick={(e) => {
                                const departureTime = document.getElementById(f.ID + "DepartureDate").value + "T" + document.getElementById(f.ID + "DepartureClock").value
                                const landingTime = document.getElementById(f.ID + "LandingDate").value + "T" + document.getElementById(f.ID + "LandingClock").value
                                    const newFlight = {
                                        ID : f.ID,
                                        AirlineCompanyId : this.state.userInfo.id,
                                        OriginCountryCode : document.getElementById(f.ID + "originCountryCode").value,
                                        DestinationCountryCode : document.getElementById(f.ID + "destinationCountryCode").value,
                                        DepartureTime : departureTime,
                                        LandingTime : landingTime,
                                        RemainingTickets : document.getElementById(f.ID + "RemainingTickets").value
                                    }
                                    this.handleOnClickModify(e, newFlight)
                                }}>Modify</button> &nbsp;&nbsp;
                            <button className="red btn" id={f.ID} name={f.ID} onClick={this.handleOnClickDelete}>Delete</button>
                            </div>
                            
                        </div>
                        </div>
                    </div>
            
                </div>
            )
            
        })

        return(
            <div>
                <h1>My Flights</h1>
                <div style={{"flex" : "0 0 30%", "margin" : "0 25%"}}>
                    {flightsHtml}
                </div>
            </div>
        )
    }

}

export default MyFlights