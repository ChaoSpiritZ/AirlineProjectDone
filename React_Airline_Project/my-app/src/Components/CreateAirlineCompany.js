import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import Axios from 'axios';

class CreateAirlineCompany extends Component{
    state = {
        countryList : [], 
        inputCSS : {  //or import this page's css - CreateAirlineCompany.css - gotta create it
            width: 200
        }
    }

    componentDidMount = () => {
        Axios.get("http://localhost:9002/api/anonymousfacade/getallcountries").then((result) => {
            //console.log(result.data)
            //preparing the countries for the select/drop-down field
            this.setState({
                countryList : result.data
            })
            //console.log(this.state.countryList)
        })
    }
    

    render()
    {
        const countryListHtml = this.state.countryList.map(c => {return (
                <option key={c.ID} value={c.ID}>{c.CountryName}</option>
        )})

        return(
            <div>
                <div className="left-align">
                <NavLink to="/">&larr; Back to login</NavLink>
                </div>
                <h1>Create an Airline Company</h1>
                <form method="post" action="http://localhost:9002/page/RequestAddingAirline">
                Username: <input name="inputUsername" type="text" style={this.state.inputCSS}/> <br/>
                Password: <input name="inputPassword" type="password" style={this.state.inputCSS}/> <br/>
                Airline Name: <input name="inputAirlineName" type="text" style={this.state.inputCSS}/> <br/>


                <div className="container">
                        <div className="row" >
                            <div className="col s3" style={{marginTop:25,marginRight:-100,marginLeft:370}}>
                                <span >Country Created:</span> 
                            </div>

                            <div className="input-field col s2" >
                                <select name="inputCountryId" className="browser-default">
                                    <option defaultValue="" disabled>Choose your option</option>
                                    {/*inserting all countries in here */}
                                    {countryListHtml}
                                </select>
                            </div>
                    </div>
                </div>
                <br/>

                <br/><button className="btn">Submit</button>
                
                
                </form>
                <br/>
                <button className="btn" onClick={() => {window.history.back()}}>Back</button>

                

            </div>
        )
    }

}

export default CreateAirlineCompany