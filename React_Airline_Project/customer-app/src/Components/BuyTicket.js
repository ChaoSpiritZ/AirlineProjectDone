import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class BuyTicket extends Component{

    state = {
        userInfo : {},
        countries : [],
        airline : {},
        flightId : -1,
        flight : {}
    }

    componentDidMount = () => {
        console.log("!!!!!!!!!!!!!!!!!!!!!")
        console.log(this.props.location.search.split('=')[1]) //do this where it's needed
        this.setState({
            userInfo : JSON.parse(localStorage.getItem("userinfo")),
            flightId : this.props.location.search.split('=')[1] }, () => {

                Axios.get("http://localhost:9002/api/anonymousfacade/getallcountries").then((result) => {
                    if(result.status == 200){
                        console.log("countries recieved!")
                        console.log(result.data)
                        this.setState({
                            countries : result.data
                        })
                    }
                }).catch((error) => {
                    if(error.response.status == 400){
                    console.log(error.response.data.Message)
                    //alert(error.response.data.Message)
                    }
                    else{
                        console.log(error)
                    }
                })

                Axios.get("http://localhost:9002/api/anonymousfacade/getflight?id=" + this.state.flightId).then((result) => {
                    if(result.status == 200){
                        console.log("flight recieved!")
                        console.log(result.data)
                        this.setState({
                            flight : result.data
                        }, () => {
                            Axios.get("http://localhost:9002/api/anonymousfacade/getairlinecompanybyid/" + this.state.flight.AirlineCompanyId).then((airlineResult) => {
                                if(airlineResult.status == 200){
                                    console.log("airline recieved!")
                                    console.log(airlineResult.data)
                                    this.setState({
                                        airline : airlineResult.data
                                    })
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
                    if(result.status == 204){
                        console.log("no flight was found!")
                        console.log(result.data)
                        this.setState({
                            flight : null
                        })
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
                )
    }

    handleOnClickPurchase = () => {
        console.log("purchasing ticket to flight " + this.state.flight.ID + "...")

        const username = this.state.userInfo.userName
                const password = this.state.userInfo.password
                const basicAuth = 'basic ' + btoa(username + ':' + password);

        Axios.post("http://localhost:9002/api/customerfacade/purchaseticket", this.state.flight, {headers: { 'Authorization': basicAuth}}).then((result) => {
            if(result.status == 200){
                console.log("ticket purchased! here it is:")
                console.log(result.data)
                //alert("Ticket purchased! \n Ticket number: " + result.data.ID)
                Swal.fire("Ticket purchased!", "Ticket number: " + result.data.ID, "success")
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

    handleOnClickCancel = () => {
        window.location.href = "http://localhost:9002/page/searchflights"
    }

    render()
    {
        const ticketConfirmationHtml = () => {
            if(this.state.flight == null){
                return(
                    <div>
                        <b className="red">No flight was found! please return to the flights page and try again!</b>
                    </div>
                )
            }
            else{

                const originCountry = this.state.countries.filter(oC => oC.ID == this.state.flight.OriginCountryCode)[0]
                const destinationCountry = this.state.countries.filter(oC => oC.ID == this.state.flight.DestinationCountryCode)[0]
                if(typeof this.state.airline != 'undefined' && typeof originCountry != 'undefined' && typeof destinationCountry != 'undefined'){
                return(
                    <div className="row" style={{"width" : "30%"}}>
                        <div className="col s12 m12">
                            <div className="card purple" style={{"height" : 370}}>
                                <div className="card-content white-text">
                                    <span className="card-title">Ticket to Flight: {this.state.flight.ID}</span>
                                    <br/> <br/>
                                    Airline: {this.state.airline.AirlineName} <br/>
                                    Origin Country: {originCountry.CountryName} <br/>
                                    Destination Country: {destinationCountry.CountryName} <br/>
                                    Departure Time: {this.state.flight.DepartureTime} <br/>
                                    Landing Time: {this.state.flight.LandingTime} <br/>
                                    Remaining Tickets: {this.state.flight.RemainingTickets} <br/> <br/> <br/>

                                    <button className="btn green" onClick={this.handleOnClickPurchase}>Purchase Ticket</button> &nbsp; &nbsp; &nbsp; &nbsp;
                                    <button className="btn red" onClick={this.handleOnClickCancel}>Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                )
                }
            }
        }

        return(
            <div>
                <h1>Flight Ticket Confirmation</h1>

                {ticketConfirmationHtml()}
            </div>
        )
    }

}

export default BuyTicket