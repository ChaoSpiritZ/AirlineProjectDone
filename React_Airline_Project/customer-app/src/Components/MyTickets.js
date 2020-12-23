import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class MyTickets extends Component{

    state = {
        userInfo : {},
        countries : [],
        airlines : [],
        tickets : [],
        flights : [],

        ticketToDelete : {}
    }

    componentDidMount = () => {

        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) }, () => {

                const username = this.state.userInfo.userName
                const password = this.state.userInfo.password
                const basicAuth = 'basic ' + btoa(username + ':' + password);

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
                    Swal.fire("Something went wrong!", error.response.data.Message, "error")
                    }
                    else{
                        console.log(error)
                    }
                })

                Axios.get("http://localhost:9002/api/anonymousfacade/getallairlinecompanies").then((result) => {
                    if(result.status == 200){
                        console.log("airlines recieved!")
                        console.log(result.data)
                        this.setState({
                            airlines : result.data
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

                Axios.get("http://localhost:9002/api/customerfacade/getallmytickets",{headers: { 'Authorization': basicAuth}}).then((result) => {
                    if(result.status == 200){
                        console.log("tickets recieved!")
                        console.log(result.data)
                        this.setState({
                            tickets : result.data
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

                Axios.get("http://localhost:9002/api/customerfacade/getallmyflights",{headers: { 'Authorization': basicAuth}}).then((result) => {
                    if(result.status == 200){
                        console.log("flights recieved!")
                        console.log(result.data)
                        this.setState({
                            flights : result.data
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

    handleOnClickCancelTicket = (e) => {
        console.log("canceling ticket " + e.target.id + "...")
        this.setState({
            ticketToDelete : this.state.tickets.filter(t => t.ID == e.target.id)[0]
        }, () => {
            //console.log("about to get deleted: " + JSON.stringify(this.state.ticketToDelete))
            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.delete("http://localhost:9002/api/customerfacade/cancelticket", { data : {ttd : this.state.ticketToDelete} ,headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("ticket " + this.state.ticketToDelete.ID + " was DELETED successfully!")

                    const newTicketList = this.state.tickets.filter(t => t.ID != this.state.ticketToDelete.ID)

                    this.setState({
                        tickets : newTicketList
                    })

                    //alert("ticket " + this.state.ticketToDelete.ID + " was DELETED successfully!")
                    Swal.fire("ticket " + this.state.ticketToDelete.ID + " was DELETED successfully!", "", "success")
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

        // const flightsHtml = this.state.flights.map(f => {

        //     const ticket = this.state.tickets.filter(t => t.FlightId == f.ID)
        //     const airline = this.state.airlines.filter(a => a.ID == f.AirlineCompanyId)
        //     const originCountry = this.state.countries.filter(oC => oC.ID == f.OriginCountryCode)
        //     const destinationCountry = this.state.countries.filter(oC => oC.ID == f.DestinationCountryCode)
        //     if(ticket != undefined)
        //     {
        //     return (
        //         <div key={f.ID}>
        //             ticket ID: {ticket[0].ID} <br/>
        //             flight ID: {f.ID} <br/>
        //             Airline Company: {airline[0].AirlineName}<br/>
        //             Origin Country Code: {originCountry[0].CountryName} <br/>
        //             Destination Country Code: {destinationCountry[0].CountryName} <br/>
        //             Departure Time: {f.DepartureTime} <br/>
        //             Landing Time: {f.LandingTime} <br/>
        //             Remaining Tickets: {f.RemainingTickets} <br/> <br/>
        //         </div>
        //     )
        //     }
        //     return (
        //         <div key={f.ID}>

        //         </div>
        //     )
        // })

        const ticketsHtml = this.state.tickets.map(t => {

            const flight = this.state.flights.filter(f => t.FlightId == f.ID)[0]
            if(typeof flight != 'undefined'){
            const airline = this.state.airlines.filter(a => a.ID == flight.AirlineCompanyId)[0]
            const originCountry = this.state.countries.filter(oC => oC.ID == flight.OriginCountryCode)[0]
            const destinationCountry = this.state.countries.filter(oC => oC.ID == flight.DestinationCountryCode)[0]
            if(typeof airline != 'undefined' && typeof originCountry != 'undefined' && typeof destinationCountry != 'undefined'){
            return (
                <div key={t.ID}>
                    <div className="row" style={{"width" : "30%"}}>
                        <div className="col s12 m12">
                            <div className="card purple" style={{"height" : 370}}>
                                <div className="card-content white-text">
                                    <span className="card-title">Ticket ID: {t.ID}</span>
                                    <br/> <br/>
                                    flight ID: {flight.ID} <br/>
                                    Airline Company: {airline.AirlineName}<br/>
                                    Origin Country: {originCountry.CountryName} <br/>
                                    Destination Country: {destinationCountry.CountryName} <br/>
                                    Departure Time: {flight.DepartureTime} <br/>
                                    Landing Time: {flight.LandingTime} <br/>
                                    Remaining Tickets: {flight.RemainingTickets} <br/> <br/>
                                    <button className="btn red" id={t.ID} onClick={this.handleOnClickCancelTicket}>Cancel Ticket</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )
            }
            }
        })

        return(
            <div>
                <h1>My Tickets</h1>
                {ticketsHtml}
            </div>
        )
    }

}

export default MyTickets