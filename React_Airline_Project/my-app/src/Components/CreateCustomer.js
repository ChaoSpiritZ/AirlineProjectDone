import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';

class CreateCustomer extends Component{

    state = {
        inputCSS : {
            width: 200
        }
    }

    render()
    {

        return(
            <div>
                <div className="left-align">
                <NavLink to="/">&larr; Back to login</NavLink>
                </div>
                <h1>Create a Customer</h1>
                <form method="post" action="http://localhost:9002/page/RequestAddingCustomer">
                Username: <input name="inputUsername" type="text" style={this.state.inputCSS}/> <br/>
                Password: <input name="inputPassword" type="password" style={this.state.inputCSS}/> <br/>
                First Name: <input name="inputFirstName" type="text" style={this.state.inputCSS}/> <br/>
                Last Name: <input  name="inputLastName"type="text" style={this.state.inputCSS}/> <br/>
                Email Address: <input name="inputEmail" type="text" style={this.state.inputCSS}/><br/>
                Phone Number: <input name="inputPhoneNo" type="text" style={this.state.inputCSS}/><br/>
                Credit Card Number: <input name="inputCreditCard" type="text" style={this.state.inputCSS}/><br/>

                {// for some reason select won't work with materialize
                }

                <br/><button className="btn">Submit</button>
                </form>
                <br/>
                <button className="btn" onClick={() => {window.history.back()}}>Back</button>
            </div>
        )
    }

}

export default CreateCustomer