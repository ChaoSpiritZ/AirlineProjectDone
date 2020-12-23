import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class CustomerEdit extends Component{

    state = {
        inputCss : {
            width : 200
        },
        userInfo : {},
        updatedUserName : {},
        updatedPassword : {},
        updatedFirstName : {},
        updatedLastName : {},
        updatedAddress : {},
        updatedPhoneNo : {},
        updatedCreditCardNumber : {},

        updatedCustomer : {}
    }

    componentDidMount = () => {
        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) } //,
            // () => {this.setState({
            //     updatedCustomer : this.state.userInfo
            // })}
        )
        console.log("yo")
        //console.log("from edit page: " + this.state.userInfo.userName)
        //console.log(this.state.updatedCustomer)
    }

    handleOnChange = (e) => {
        this.setState({
            [e.target.id]: e.target.value
        })
    }

    handleOnClick = (e) => {
        this.setState({
            updatedCustomer : {
                ID: this.state.userInfo.id,
                UserName : this.state.updatedUserName,
                Password : this.state.updatedPassword,
                FirstName : this.state.updatedFirstName,
                LastName : this.state.updatedLastName,
                Address : this.state.updatedAddress,
                PhoneNo : this.state.updatedPhoneNo,
                CreditCardNumber : this.state.updatedCreditCardNumber,
            }
        }, () => {
            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);
            Axios.put("http://localhost:9002/api/customerfacade/modifycustomerdetails", this.state.updatedCustomer, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("Modified customer successfully!")
                    //alert("Modified customer successfully!")
                    Swal.fire("Modified customer successfully!", "", "success")
                }
            }).catch((error) => {
                if(error.response.status == 401){
                    console.log(error.response.data)
                    //alert(error.response.data.Message)
                    Swal.fire("Something went wrong!", error.response.data, "error")
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
        })
    }

    render()
    {

        return(
            <div>
                <h1>Customer Edit Page</h1>
                    <b>there's no good password change system here (because it wasn't stated in the instructions of part 1), check the airline user for the better one</b> <br/>
                    <b>ID: </b> ({this.state.userInfo.id}) (should probably be hidden) <input name="id" type="hidden"/> <br/>
                    <b>Username:</b> ({this.state.userInfo.userName}) =={">"} <input id="updatedUserName" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Password:</b> ({"<"}hidden{">"}) =={">"} <input id="updatedPassword" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>First Name:</b> ({this.state.userInfo.firstName}) =={">"} <input id="updatedFirstName" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Last Name:</b> ({this.state.userInfo.lastName}) =={">"} <input id="updatedLastName" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Email Address:</b> ({this.state.userInfo.address}) =={">"} <input id="updatedAddress" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Phone Number:</b> ({this.state.userInfo.phoneNo}) =={">"} <input id="updatedPhoneNo" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Credit Card Number:</b> ({this.state.userInfo.creditCardNumber}) =={">"} <input id="updatedCreditCardNumber" type="text" style={{"width": 300}} onChange={this.handleOnChange}/><br/><br/>
                    <button className="darken-3 orange btn" onClick={this.handleOnClick}>Modify</button>
            </div>
        )
    }

}

export default CustomerEdit