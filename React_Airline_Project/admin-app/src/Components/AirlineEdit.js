import React, { Component } from 'react';
import Axios from 'axios';
import Swal from 'sweetalert2';

class AirlineEdit extends Component{

    state = {
        inputCss : {
            width : 200
        },
        userInfo : {},
        airlines : [],
        countryList : []
        //updatedAirline : {}
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

        Axios.get("http://localhost:9002/api/anonymousfacade/getallairlinecompanies").then((result) => {
            console.log("airline companies recieved!")
            console.log(result.data)
            this.setState({
                airlines : result.data
            }, () => {
                //testing if it's working:
                //console.log(this.state.airlines)
            })
        })
        .catch((error) => {
            console.error("something's wrong with the ajax!")
            console.error(error)
        })
    }

    handleOnClickModify = (e, newAirline) => {

            console.log(newAirline)
            //console.log(this.state.updatedAirline)

            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.put("http://localhost:9002/api/adminfacade/updateairlinedetails", newAirline, {headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("airline " + newAirline.ID + " was modified successfully!")
                    //alert("Airline " + newAirline.ID + " was modified successfully!")
                    Swal.fire("Airline " + newAirline.ID + " was modified successfully!", "", "success")
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
            const airlineId = e.target.name

            const username = this.state.userInfo.userName
            const password = this.state.userInfo.password
            const basicAuth = 'basic ' + btoa(username + ':' + password);

            Axios.delete("http://localhost:9002/api/adminfacade/removeairline/" + airlineId, { headers: { 'Authorization': basicAuth}}).then((result) => {
                if(result.status == 200){
                    console.log("airline " + airlineId + " was DELETED successfully!")
                    //alert("Airline " + airlineId + " was DELETED successfully!")
                    Swal.fire("Airline " + airlineId + " was DELETED successfully!", "", "success")
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

            const newairlinesList = this.state.airlines.filter(c => c.ID != e.target.name)
        this.setState({
            airlines : newairlinesList
        })
    }

    render()
    {
        const countryListHtml = this.state.countryList.map(c => {
            return (
            <option key={c.ID} value={c.ID}>{c.CountryName}</option>
            )
        })

        const airlinesHtml = this.state.airlines.map(a => {
            const country = this.state.countryList.filter(c => c.ID == a.CountryCode)[0]
            return (
                <div key={a.ID}>
                    <div className="row" style={{"width" : "100%"}}>
                        <div className="col s12 m12">
                        <div className="card pink darken-2" style={{"height" : 400}}>
                            <div className="card-content white-text">
                            <span className="card-title">ID: {a.ID}</span>
                            <br/> <br/>
                            Username: <input type="text" id={a.ID + "UserName"} defaultValue={a.UserName} style={this.state.inputCss}/> &nbsp;&nbsp;
                            Password: <input type="text" id={a.ID + "Password"} defaultValue={a.Password} style={this.state.inputCss}/> <br/>
                            Airline Name: <input type="text" id={a.ID + "AirlineName"} defaultValue={a.AirlineName} style={this.state.inputCss}/> &nbsp;&nbsp;
                            Country Created: ({country ? country.CountryName : "Loading..."}) <br/>
                             <span style={{marginLeft:420}}>V</span>
                            <div className="container">
                                <div className="row" >
                                    <div className="input-field col s5" >
                                        <select id={a.ID + "updatedCountryCode"} name="updatedCountryCode" className="browser-default" style={{marginLeft:400}}>
                                            <option defaultValue="" disabled>Choose your option</option>
                                            {/*inserting all countries in here */}
                                            {countryListHtml}
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <button className="orange darken-3 btn" id={a.ID} onClick={(e) => {
                                    const newAirline = {
                                        ID : a.ID,
                                        UserName : document.getElementById(a.ID + "UserName").value,
                                        Password : document.getElementById(a.ID + "Password").value,
                                        AirlineName : document.getElementById(a.ID + "AirlineName").value,
                                        CountryCode : document.getElementById(a.ID + "updatedCountryCode").value
                                    }
                                    this.handleOnClickModify(e, newAirline)
                                }}>Modify</button> &nbsp;&nbsp;
                            <button className="red btn" name={a.ID} onClick={this.handleOnClickDelete}>Delete</button>
                            </div>
                            
                        </div>
                        </div>
                    </div>
            
                </div>
            )
            
        })

        return(
            <div>
                <h1>Airline Edit Page</h1>
                <div style={{"flex" : "0 0 30%", "margin" : "0 25%"}}>
                    {airlinesHtml}
                </div>
            </div>
        )
    }

}

export default AirlineEdit