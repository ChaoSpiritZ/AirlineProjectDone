import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class ChangePassword extends Component{

    state = {
        inputCss : {
            width : 200
        },
        userInfo : {},
        oldPassword : {},
        newPassword : {},

        passwordPair : {}
    }

    componentDidMount = () => {
        this.setState({
            userInfo: JSON.parse(localStorage.getItem("userinfo")) }
        )

        console.log("mounted")
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
            //passwordPair : this.state.oldPassword + ":" + this.state.newPassword
            passwordPair : [
                this.state.oldPassword,
                this.state.newPassword
            ]
        }, () => {

            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);
            Axios.put("http://localhost:9002/api/airlinefacade/changemypassword", this.state.passwordPair, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("Changed password successfully!")
                    Swal.fire("Changed password successfully!")
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

        //const passwordPair = this.state.oldPassword + ":" + this.state.newPassword

        
    }

    render()
    {

        return(
            <div>
                <h1>Change Password Page</h1>
                <b>Old Password:</b> <input id="oldPassword" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                <b>New Password:</b> <input id="newPassword" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/> <br/>

                <button className="btn orange darken-3" onClick={this.handleOnClick}>Modify</button>
            </div>
        )
    }

}

export default ChangePassword