import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class CustomerEdit extends Component{

    state = {
        inputCss : {
            width : 200
        },
        userInfo : {},
        customers : [],
        //updatedCustomer : {}
    }

    componentDidMount = () => {

        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) }
        )

        Axios.get("http://localhost:9002/api/anonymousfacade/getallcustomers").then((result) => {
            console.log("customers recieved!")
            console.log(result.data)
            this.setState({
                customers : result.data
            }, () => {
                //testing if it's working:
                //console.log(this.state.customers)
            })
        })
        .catch((error) => {
            console.error("something's wrong with the ajax!")
            console.error(error)
        })
    }

    handleOnClickModify = (e, newCustomer) => {

            console.log(newCustomer)
            //console.log(this.state.updatedCustomer)

            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.put("http://localhost:9002/api/adminfacade/updatecustomerdetails", newCustomer, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("customer " + newCustomer.ID + " was modified successfully!")
                    //alert("Customer " + newCustomer.ID + " was modified successfully!")
                    Swal.fire("Customer " + newCustomer.ID + " was modified successfully!", "", "success")
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
        console.log(e.target.name)
            const customerId = e.target.name

            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.delete("http://localhost:9002/api/adminfacade/removecustomer/" + customerId, { headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("customer " + customerId + " was DELETED successfully!")
                    //alert("Customer " + customerId + " was DELETED successfully!")
                    Swal.fire("Customer " + customerId + " was DELETED successfully!", "", "success")
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

            const newcustomersList = this.state.customers.filter(c => c.ID != e.target.name)
        this.setState({
            customers : newcustomersList
        })
    }

    render()
    {

        const customersHtml = this.state.customers.map(c => {
            return (
                <div key={c.ID}>
                    <div className="row" style={{"width" : "100%"}}>
                        <div className="col s12 m12">
                        <div className="card pink darken-2" style={{"height" : 400}}>
                            <div className="card-content white-text">
                            <span className="card-title">ID: {c.ID}</span>
                            <br/> <br/>
                            Username: <input type="text" id={c.ID + "UserName"} defaultValue={c.UserName} style={this.state.inputCss}/> &nbsp;&nbsp;
                            Password: <input type="text" id={c.ID + "Password"} defaultValue={c.Password} style={this.state.inputCss}/> <br/>
                            First Name: <input type="text" id={c.ID + "FirstName"} defaultValue={c.FirstName} style={this.state.inputCss}/> &nbsp;&nbsp;
                            Last Name: <input type="text" id={c.ID + "LastName"} defaultValue={c.LastName} style={this.state.inputCss}/> <br/>
                            Email: <input type="text" id={c.ID + "Address"} defaultValue={c.Address} style={this.state.inputCss}/> &nbsp;&nbsp;
                            Phone Number: <input type="text" id={c.ID + "PhoneNo"} defaultValue={c.PhoneNo} style={this.state.inputCss}/> <br/>
                            Credit Card Number: <input type="text" id={c.ID + "CreditCardNumber"} defaultValue={c.CreditCardNumber} style={{"width" : 300}}/> <br/> <br/> 

                            <button className="orange darken-3 btn" id={c.ID} onClick={(e) => {
                                    const newCustomer = {
                                        ID : c.ID,
                                        UserName : document.getElementById(c.ID + "UserName").value,
                                        Password : document.getElementById(c.ID + "Password").value,
                                        FirstName : document.getElementById(c.ID + "FirstName").value,
                                        LastName : document.getElementById(c.ID + "LastName").value,
                                        Address : document.getElementById(c.ID + "Address").value,
                                        PhoneNo : document.getElementById(c.ID + "PhoneNo").value,
                                        CreditCardNumber : document.getElementById(c.ID + "CreditCardNumber").value
                                    }
                                    this.handleOnClickModify(e, newCustomer)
                                }}>Modify</button> &nbsp;&nbsp;
                            <button className="red btn" name={c.ID} onClick={this.handleOnClickDelete}>Delete</button>
                            </div>
                            
                        </div>
                        </div>
                    </div>
            
                </div>
            )
        })

        return(
            <div>
                <h1>Customer Edit Page</h1>
                <div style={{"flex" : "0 0 30%", "margin" : "0 30%"}}>
                    {customersHtml}
                </div>
            </div>
        )
    }

}

export default CustomerEdit