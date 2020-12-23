import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class AddFlight extends Component{

    state = {
        inputCss : {
            width : 130
        },
        userInfo : {},
        countryList : [],

        originCountryCode : -1,
        destinationCountryCode : -1,
        departureDate : {},
        departureClock : {},
        landingDate : {},
        landingClock : {},
        numOfTickets : -1,

        flightToCreate : {}
    }

    componentDidMount = () => {
        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) }
        )

        Axios.get("http://localhost:9002/api/anonymousfacade/getallcountries").then((result) => {
            //console.log(result.data)
            //preparing the countries for the select/drop-down field
            this.setState({
                countryList : result.data
            })
            //console.log(this.state.countryList)
        })

        console.log("mounted")
        //console.log("from edit page: " + this.state.userInfo.userName)
        //console.log(this.state.updatedCustomer)
    }

    handleOnChange = (e) => {
        this.setState({
            [e.target.id]: e.target.value
        })
    }

    handleOnClick = () => {
        console.log(this.state)

        const departureTime = this.state.departureDate + "T" + this.state.departureClock
        const landingTime = this.state.landingDate + "T" + this.state.landingClock
        console.log(departureTime)
        console.log(landingTime)
        
        this.setState({
            flightToCreate : {
                ID : 0,
                AirlineCompanyId : this.state.userInfo.id,
                OriginCountryCode : this.state.originCountryCode,
                DestinationCountryCode : this.state.destinationCountryCode,
                DepartureTime : departureTime,
                LandingTime : landingTime,
                RemainingTickets : this.state.numOfTickets
            }
        }, () => {
            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);
            Axios.post("http://localhost:9002/api/airlinefacade/createflight", this.state.flightToCreate, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("flight added successfully!")
                    //alert("flight added successfully!")
                    Swal.fire("Flight added successfully!", "", "success")
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
        const countryListHtml = this.state.countryList.map(c => {return (
            <option key={c.ID} value={c.ID}>{c.CountryName}</option>
        )})

        //new Date()
        //var d = new Date(year, month, day, hours, minutes, seconds, milliseconds);
        //take value from inputs and put it in the line above
        return(
            <div>
                <h1>Add Flight Page</h1>
                <div className="container">
                        <div className="row" >
                            <div className="col s3" style={{marginTop:25,marginRight:-75,marginLeft:370}}>
                                <span ><b>Origin Country:</b> </span> 
                            </div>

                            <div className="input-field col s2" >
                                <select id="originCountryCode" name="originCountryCode" className="browser-default" onChange={this.handleOnChange}>
                                    <option defaultValue="0">Choose your option</option>
                                    {/*inserting all countries in here */}
                                    {countryListHtml}
                                </select>
                            </div>
                            
                            <div className="col s3" style={{marginTop:25,marginRight:-75,marginLeft:370}}>
                                <span ><b>Destination Country:</b> </span> 
                            </div>

                            <div className="input-field col s2" >
                                <select id="destinationCountryCode" name="destinationCountryCode" className="browser-default" onChange={this.handleOnChange}>
                                    <option defaultValue="0">Choose your option</option>
                                    {/*inserting all countries in here */}
                                    {countryListHtml}
                                </select>
                            </div>
                    </div>
                    </div>
                <b>Departure Time:</b> <input type="date" id="departureDate" style={this.state.inputCss } onChange={this.handleOnChange}/> &nbsp;
                                        <input type="time" id="departureClock" style={this.state.inputCss} onChange={this.handleOnChange}/> <br/>
                <b>Landing Time:</b> <input type="date" id="landingDate" style={this.state.inputCss} onChange={this.handleOnChange}/> &nbsp;
                                    <input type="time" id="landingClock" style={this.state.inputCss} onChange={this.handleOnChange}/> <br/>
                <b>Number of Tickets:</b> <input type="number" id="numOfTickets" style={this.state.inputCss} onChange={this.handleOnChange}/> <br/>
                <button className="btn" onClick={this.handleOnClick}>Create Flight</button>
            </div>
        )
    }

}

export default AddFlight