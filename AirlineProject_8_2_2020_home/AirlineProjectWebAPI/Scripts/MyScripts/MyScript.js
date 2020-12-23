function purchaseTicket(flightId) {
    //localStorage.setItem("userinfo", '{"type":"Customer","id":"367","firstName":"Belinda","lastName":"Dupont","userName":"ticklishdog163","password":"wives","address":"belinda.dupont@example.com","phoneNo":"077 962 83 18","creditCardNumber":"23b557d0-c96d-4697-b0fd-21770c43959e"}')
    const userInfo = JSON.parse(localStorage.getItem("userinfo"))
    if (userInfo != null) {
        if (userInfo.type == "Customer") {
            window.location.href = 'http://localhost:3000/buyticket?flightId=' + flightId;
        }
    }
    else { //YES! THE 'ELSE' IS IMPORTANT!
        window.location.href = 'http://localhost:9002?flightId=' + flightId;
    }
    //also check if type is customer to send directly (maybe later, or not)
}

function backToMain() {
        //localStorage.setItem("userinfo", '{"type":"Customer","id":"367","firstName":"Belinda","lastName":"Dupont","userName":"ticklishdog163","password":"wives","address":"belinda.dupont@example.com","phoneNo":"077 962 83 18","creditCardNumber":"23b557d0-c96d-4697-b0fd-21770c43959e"}')

    //go back to main page depending on user type:
    const userInfo = JSON.parse(localStorage.getItem("userinfo"))
    if (userInfo == null)
        window.location.href = 'http://localhost:9002/AnonymousUser';
    else {
        switch (userInfo.type) {
            case "Customer": window.location.href = 'http://localhost:3000/'; break;
            case "Airline": window.location.href = 'http://localhost:3002/'; break;
            case "Admin": window.location.href = 'http://localhost:3003/'; break;
            default: window.location.href = 'http://localhost:9002/AnonymousUser'; break;
        }
    }
}