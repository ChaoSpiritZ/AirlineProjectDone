import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class RequestInbox extends Component{

    state = {
        userInfo : {},
        airlineRequests : [],
        customerRequests : []
    }

    componentDidMount = () => {

        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) }
        )

        Axios.get("http://localhost:9002/api/Redis/GetLists").then((result) => {
            console.log("there you go:")
            console.log(result.data)
            this.setState({
                airlineRequests : result.data.AirlineList,
                customerRequests : result.data.CustomerList
            })
        })
        .catch((error) => {
            console.error("something's wrong with the ajax!")
            console.error(error)
        })
    }

    handleOnClickAirlineAccept = (e, airlineRequest) => {
        //console.log(e.target.id)
        console.log("airlineRequest")
        console.log(airlineRequest)
        const airline = {
            ID : 0,
            AirlineName : airlineRequest.AirlineName,
            UserName : airlineRequest.Username,
            Password : airlineRequest.Password,
            CountryCode : airlineRequest.CountryId
        }
        const body = [airline, airlineRequest]
            
        console.log("airline:")
        console.log(airline)

        console.log("body")
        console.log(body)

        //REMOVE FROM REDIS + ADD AIRLINE TO DATABASE (or at least try to because facade errors and stuff)
        const username = this.state.userInfo.userName
        const password = this.state.userInfo.password
        const basicAuth = 'basic ' + btoa(username + ':' + password);
        Axios.post("http://localhost:9002/api/adminfacade/createnewairline", body, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("airline accepted successfully!")
                    //alert("Airline accepted successfully!")
                    Swal.fire("Airline accepted successfully!", "", "success")
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

        const newairlineRequestsList = this.state.airlineRequests.filter(a => a.Username != e.target.id)
        this.setState({
            airlineRequests : newairlineRequestsList
        })
        console.log("NEAT")
    }

    handleOnClickAirlineReject = (e, airlineRequest) => {
        // console.log("e:" + e.target.id)
        // console.log("airline:" + airline.CountryId)
        // const airline = {
        //     ID : 0,
        //     AirlineName : airlineRequest.AirlineName,
        //     UserName : airlineRequest.userName,
        //     Password : airlineRequest.password,
        //     CountryCode : airlineRequest.CountryCode
        // }

        //REMOVE FROM REDIS
        const username = this.state.userInfo.userName
        const password = this.state.userInfo.password
        const basicAuth = 'basic ' + btoa(username + ':' + password);
        Axios.put("http://localhost:9002/api/AdminFacade/RejectAirlineRequest", airlineRequest, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("airline rejected successfully!")
                    //alert("Airline rejected successfully!")
                    Swal.fire("Airline rejected successfully!", "", "success")
                }
            })

        //Axios.put("http://localhost:9002/api/AdminFacade/RejectAirlineRequest", airline) //ADD HEADER WITH AUTHORIZATION BECAUSE IT'S GOING TO BE IN THE ADMIN CONTROLLER

        const newairlineRequestsList = this.state.airlineRequests.filter(a => a.Username != e.target.id)
        this.setState({
            airlineRequests : newairlineRequestsList
        })
        console.log("NOPE!")
    }

    handleOnClickCustomerAccept = (e, customerRequest) => {
        //console.log(e.target.id)
        console.log("customerRequest")
        console.log(customerRequest)
        const customer = {
            ID : 0,
            UserName : customerRequest.Username,
            Password : customerRequest.Password,
            FirstName : customerRequest.FirstName,
            LastName : customerRequest.LastName,
            Address : customerRequest.Email,
            PhoneNo : customerRequest.PhoneNo,
            CreditCardNumber : customerRequest.CreditCard,
        }
        const body = [customer, customerRequest]
            
        console.log("customer:")
        console.log(customer)

        console.log("body")
        console.log(body)

        //REMOVE FROM REDIS + ADD CUSTOMER TO DATABASE (or at least try to because facade errors and stuff)
        const username = this.state.userInfo.userName
        const password = this.state.userInfo.password
        const basicAuth = 'basic ' + btoa(username + ':' + password);
        Axios.post("http://localhost:9002/api/adminfacade/createnewcustomer", body, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("customer accepted successfully!")
                    //alert("Customer accepted successfully!")
                    Swal.fire("Customer accepted successfully!", "", "success")
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

        const newcustomerRequestsList = this.state.customerRequests.filter(c => c.Username != e.target.id)
        this.setState({
            customerRequests : newcustomerRequestsList
        })
        console.log("NEAT")
    }

    handleOnClickCustomerReject = (e, customerRequest) => {
        //REMOVE FROM REDIS
        const username = this.state.userInfo.userName
        const password = this.state.userInfo.password
        const basicAuth = 'basic ' + btoa(username + ':' + password);

        Axios.put("http://localhost:9002/api/AdminFacade/RejectCustomerRequest", customerRequest, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("customer rejected successfully!")
                    //alert("Customer rejected successfully!")
                    Swal.fire("Customer rejected successfully!", "", "success")
                }
            })

        //Axios.put("http://localhost:9002/api/AdminFacade/RejectAirlineRequest", airline) //ADD HEADER WITH AUTHORIZATION BECAUSE IT'S GOING TO BE IN THE ADMIN CONTROLLER

        const newcustomerRequestsList = this.state.customerRequests.filter(c => c.Username != e.target.id)
        this.setState({
            customerRequests : newcustomerRequestsList
        })
        console.log("NOPE")
    }

    render()
    {

        const airlineRequestsHtml = this.state.airlineRequests.map(a => {
            return (
                <div key={a.Username}>
                    <div className="row" style={{"width" : "100%"}}>
                        <div className="col s12 m12">
                        <div className="card blue-grey darken-1" style={{"height" : 300}}>
                            <div className="card-content white-text">
                            <span className="card-title">Airline</span>
                            <br/> <br/>
                            Username: {a.Username} <br/>
                            Password: {a.Password} <br/>
                            Airline Name: {a.AirlineName} <br/>
                            Country Originated: {a.Country} <br/> <br/> 
                            {/* Country code: {a.CountryId} <br/> <br/>  */}
                            </div>
                            <div className="card-action">
                            <button className="btn orange darken-3" id={a.Username} onClick={(e) => {this.handleOnClickAirlineAccept(e, a)}}>Accept</button> {/*  does like below, also sends ajax with authorization header that also removes this from redis */}
                            <button className="btn orange darken-3" id={a.Username} onClick={(e) => {this.handleOnClickAirlineReject(e, a)}}>Reject</button> {/*  remove a from airlineRequests, it should hopefully remove this card */}
                            </div>
                        </div>
                        </div>
                    </div>
            
                </div>
            )
        })

        const customerRequestsHtml = this.state.customerRequests.map(c => {
            return (
                <div key={c.Username}>
                    <div className="row" style={{"width" : "100%"}}>
                        <div className="col s12 m12">
                        <div className="card red darken-4" style={{"height" : 300}}>
                            <div className="card-content white-text">
                            <span className="card-title">Customer</span>
                            Username: {c.Username} <br/>
                            Password: {c.Password} <br/>
                            First Name: {c.FirstName} <br/>
                            Last Name: {c.LastName} <br/>
                            Email Address: {c.Email} <br/>
                            Phone Number: {c.PhoneNo} <br/>
                            Credit Card Number: {c.CreditCard} <br/>
                            </div>
                            <div className="card-action">
                            <button className="btn orange darken-3" id={c.Username} onClick={(e) => {this.handleOnClickCustomerAccept(e, c)}}>Accept</button> {/*  does like below, also sends ajax with authorization header that also removes this from redis */}
                            <button className="btn orange darken-3" id={c.Username} onClick={(e) => {this.handleOnClickCustomerReject(e, c)}}>Reject</button> {/*  remove a from customerRequests, it should hopefully remove this card */}
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            )
        })

        return(
            <div>
                <h1>Request Inbox</h1>
                <div style={{"display" : "flex"}}>
                <div style={{"flex" : "0 0 30%", "marginLeft" : "20%"}}>
                    {airlineRequestsHtml}
                </div>
                <div style={{"flex" : "0 0 30%"}}>
                    {customerRequestsHtml}
                </div>
                </div>
            </div>
        )
    }

}

export default RequestInbox