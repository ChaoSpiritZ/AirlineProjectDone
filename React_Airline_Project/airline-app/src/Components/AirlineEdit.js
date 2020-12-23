import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class AirlineEdit extends Component{

    state = {
        inputCss : {
            width : 200
        },
        countryList : [],
        userInfo : {},
        updatedUserName : {},
        updatedAirlineName : {},
        updatedCountryCode : {},

        updatedAirline : {}
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

    handleOnClick = (e) => {
        this.setState({
            updatedAirline : {
                ID: this.state.userInfo.id,
                UserName : this.state.updatedUserName,
                Password : this.state.userInfo.password, //unchanged
                AirlineName : this.state.updatedAirlineName,
                CountryCode : this.state.updatedCountryCode,
            }
        }, () => {
            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);
            Axios.put("http://localhost:9002/api/airlinefacade/modifyairlinedetails", this.state.updatedAirline, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("Modified airline successfully!")
                    //alert("Modified airline successfully!")
                    Swal.fire("Modified airline successfully!", "", "success")
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

        return(
            <div>
                <h1>Airline Edit Page</h1>
                <b>To change your password, go to the "Change Password" page</b><br/><br/>
                <b>ID: </b> ({this.state.userInfo.id}) (should probably be hidden) <input name="id" type="hidden"/> <br/>
                    <b>Username:</b> ({this.state.userInfo.userName}) =={">"} <input id="updatedUserName" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <b>Airline Name:</b> ({this.state.userInfo.airlineName}) =={">"} <input id="updatedAirlineName" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    {/* <b>Country:</b> ({this.state.userInfo.countryName}) =={">"} <input id="updatedCountryCode" type="text" style={this.state.inputCss} onChange={this.handleOnChange}/><br/>
                    <br/> */}

                    <div className="container">
                        <div className="row" >
                            <div className="col s3" style={{marginTop:25,marginRight:-50,marginLeft:370}}>
                                <span ><b>Country Created:</b> ({this.state.userInfo.countryName}) =={">"}</span> 
                            </div>

                            <div className="input-field col s2" >
                                <select id="updatedCountryCode" name="updatedCountryCode" className="browser-default" onChange={this.handleOnChange}>
                                    <option defaultValue="" disabled>Choose your option</option>
                                    {/*inserting all countries in here */}
                                    {countryListHtml}
                                </select>
                            </div>
                    </div>
                    </div>

                    <button className="btn orange darken-3" onClick={this.handleOnClick}>Modify</button>
            </div>
        )
    }

}

export default AirlineEdit