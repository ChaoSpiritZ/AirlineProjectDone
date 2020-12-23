import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import Axios from 'axios';
import Swal from 'sweetalert2';

class Login extends Component{

    state = {
        username: "",
        password: "",
        flightId : -1
    }

    componentDidMount = () => {
        console.log(this.props.location.search)
        console.log("clearing local storage...")
        localStorage.clear()
        console.log("local storage cleared!")
        console.log("!_!_!_!_!_!_!")
        console.log(this.props.location.search.split('=')[1])
        if(this.props.location.search.split('=')[1] != null){
        this.setState({
            flightId : this.props.location.search.split('=')[1]
        })
    }
    }

    handleOnChange = (e) => {
        this.setState({
            [e.target.id]: e.target.value
        })
        
    }

    handleOnClick = (e) => {
        //would need switch case or ifs in case i have more than 1 button

        //need the result to have more info about the user
        Axios.post("http://localhost:9002/api/login/login" + this.props.location.search, {username: this.state.username, password: this.state.password}).then((result) => {
            // console.log(1)    
            // console.log(result)
            // console.log(2)    
            // console.log(result.data)
            // console.log(3)    
            // console.log(JSON.parse(result.data))
            // console.log(4)
            const userInfo = JSON.parse(result.data);
            console.log(userInfo)
            //console.log(result.data.type) //nothing......
            
            if(result.status == 200){
                localStorage.setItem("userInfo", result.data); //put this line before getting the info on the next pages because it doesn't work in different ports
                //console.log("user info: " + localStorage.getItem("userInfo")) 
                console.log(localStorage.getItem("userInfo"))
                //entered as one of the customers
                if(userInfo.type == "Customer"){
                    if(this.state.flightId != -1){
                        window.location.href = "http://localhost:3000/buyticket?flightId=" + this.state.flightId //LINK TO THE TICKET BUY PAGE
                    }
                    else{
                        window.location.href = 'http://localhost:3000/';
                    }
                         
                    

                    //alert("you logged in as one of the customers!")
                }
                //entered as one of the airline companies
                if(userInfo.type == "Airline"){
                    
                    window.location.href = 'http://localhost:3003/';

                    //alert("you logged in as one of the airline companies!")
                }
                //entered as one of the admins
                if(userInfo.type == "Admin"){
                    //window.history.back() <--back button - pretty neat
                    
                    window.location.href = 'http://localhost:3002/';

                    //alert("you logged in as one of the admins!")
                }
            }
        }).catch((error) => {
            if(error.response.status == 401){
            //alert("Wrong Username or Password!")
            Swal.fire("Wrong Username or Password!", "", "error")
            }
            else{
                console.log(error)
            }
        })

    }

    render()
    {
        const ticketPurchaseMessage = () => {
            if(this.props.location.search != ""){
                return(
                    <div>
                        <b className="red"><u>To purchase a ticket, please login as a customer.</u></b>
                    </div>
                )
            }
            else{
                return(
                    <div>

                    </div>
                )
            }
        }

        return(
            
            <div className="login">

                {ticketPurchaseMessage()} <br/>
                {/* for debug purposes: <br/>
                 <b>
                 customer: crazyswan425 madcat (IS DEAD)<br/>
                 airline: beautifulgorilla981 logan2 <br/>
                 admin: admin 9999
                 </b> */}

                <div className="row">
                    <div className="col m4 offset-m4">
                    <div className="card blue darken-4 round">
                        <div className="card-content white-text">
                        <span className="card-title">Login</span>
                        {/* Username: <br/> */}
                        <input className="col m8 offset-m2 white-text" type="text" id="username" placeholder="Username" onChange={this.handleOnChange}></input> <br/>
                        {/* <br/> <br/> Password:  */}
                        <br/><br/>
                        <input className="col m8 offset-m2 white-text" type="password" id="password" placeholder="Password" onChange={this.handleOnChange}></input> <br/> 
                        <br/><br/> <button className="btn pink darken-3" onClick={this.handleOnClick}>Login</button> <br/> <br/>

                        Don't have a user? <br/>
                        <NavLink to='/AnonymousUser'>Enter as an anonymous user</NavLink> <br/>
                        or sign up as: <br/>
                        <NavLink to='/CreateCustomer'> Customer</NavLink>
                        / 
                        <NavLink to='/CreateAirlineCompany'>Airline Company</NavLink>
                        </div>
                    </div>
                    </div>
                </div>

            </div>
            
        )
    }

}

export default Login