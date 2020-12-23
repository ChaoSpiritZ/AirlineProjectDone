function getCustomers(){
    //facade in anonymous for the time being

    $.ajax({
        url: `http://localhost:9002/api/anonymousfacade/getallcustomers`
    }).then(function (data) {
        console.log(data);
        //$('#result').html(JSON.stringify(data))
        var $customers_table = $("#customerstable")
                $customers_table.append(`
						<tr>
						<th>ID</th>
						<th>FULL NAME</th>
						<th>USERNAME</th>
                        <th>ADDRESS</th>
						<th>PHONE NUMBER</th></tr>`);

				$.each(data, function (i, customer) {
                    $customers_table.append(`
                    <tr id="row${customer.ID}"> 
                    <td> ${customer.ID}</td>
                    <td> ${customer.FirstName} ${customer.LastName}</td>
                    <td> ${customer.UserName}</td>
                    <td> ${customer.Address}</td>
                    <td> ${customer.PhoneNo}</td>
                    <td><button style="background-color: gold;" onclick='editCustomer(${customer.ID}, ${JSON.stringify(customer)})'>Edit</button>
                        <button style="background-color: red;" onclick='deleteCustomer(${customer.ID}, ${JSON.stringify(customer)})'>Delete</button></td></tr>`)
                    
				})
    }).fail(
        // what to do on error
        function (err) {
            console.error(err)
        }
        )
}

function editCustomer(custID, customer){

    //editing a second time still shows the first value, doesn't cause a problem except visually

    var FirstName = customer.FirstName
    var LastName = customer.LastName
    var UserName = customer.UserName
    var Address = customer.Address
    var PhoneNo = customer.PhoneNo

    var current_row = $(`#row${custID}`)
    $(`#row${custID} td`).remove();

    //the 'required' attribute here is for visuals, doesn't prevent "submitting" because it's technically not a submit
    current_row.append(`
                    <td> ${custID} </td> 
                    <td> <input type="text" id="row${custID}fname" value="${FirstName}" required> 
                         <input type="text" id="row${custID}lname" value="${LastName}" required></td>
                    <td> <input type="text" id="row${custID}uname" value="${UserName}" required></td>
                    <td> <input type="email" id="row${custID}address" size="35" value="${Address}" required></td>
                    <td> <input type="text" id="row${custID}phone" value="${PhoneNo}" required></td>

                    <td><button style="background-color: green;" onclick='doneCustomer(${custID}, ${JSON.stringify(customer)})'>Done</button>
                        <button style="background-color: orange;" onclick='cancelCustomer(${custID}, ${JSON.stringify(customer)})'>Cancel</button></td></tr>`);
                    console.log(`${$(`#row${custID}fname`).val()}`);

console.log("editing entry " + custID);

}

function doneCustomer(customerID, customer){
    
    var url_web_api = 'http://localhost:9002/api/anonymousfacade/updatecustomer'

    var ID = customerID
    var FirstName = $(`#row${customerID}fname`).val()
    var LastName = $(`#row${customerID}lname`).val()
    var UserName = $(`#row${customerID}uname`).val()
    var Password = customer.Password
    var Address = $(`#row${customerID}address`).val()
    var PhoneNo = $(`#row${customerID}phone`).val()
    var CreditCardNumber = customer.CreditCardNumber

    var item = {
        ID,
        FirstName,
        LastName,
        UserName,
        Password,
        Address,
        PhoneNo,
        CreditCardNumber
    }

    var ajaxPutDataConfig = {
        type: "PUT", // what is the method? post, get, put , delete
        url: url_web_api,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(item) // request http body
    }

    $.ajax(ajaxPutDataConfig).then(
        // what to do after success?
        function (data) {
            console.log(data)
        }
    ).fail(
        // what to do on error
        function (err) {
            console.error(err)
        }
    )

    var current_row = $(`#row${customerID}`)
    $(`#row${customerID} td`).remove();

    current_row.append(`
                    <td> ${customerID}</td>
                    <td> ${FirstName} ${LastName}</td>
                    <td> ${UserName}</td>
                    <td> ${Address}</td>
                    <td> ${PhoneNo}</td>
                    <td><button style="background-color: gold;" onclick='editCustomer(${customerID}, ${JSON.stringify(customer)})'>Edit</button>
                        <button style="background-color: red;" onclick='deleteCustomer(${customerID}, ${JSON.stringify(customer)})'>Delete</button></td>`)

    console.log("finished editing entry " + customerID);
    console.log("paramaters: " + ID + " " + JSON.stringify(customer) + " " + FirstName + " " + LastName + " " + UserName + " " + Address + " " + PhoneNo)


}

function cancelCustomer(custID, customer){

    var current_row = $(`#row${custID}`)
    $(`#row${custID} td`).remove();

    current_row.append(`
                    <td> ${customer.ID}</td>
                    <td> ${customer.FirstName} ${customer.LastName}</td>
                    <td> ${customer.UserName}</td>
                    <td> ${customer.Address}</td>
                    <td> ${customer.PhoneNo}</td>
                    <td><button style="background-color: gold;" onclick='editCustomer(${customer.ID}, ${JSON.stringify(customer)})'>Edit</button>
                        <button style="background-color: red;" onclick='deleteCustomer(${customer.ID}, ${JSON.stringify(customer)})'>Delete</button></td>`)

    console.log("cancelled editing entry " + custID);
}

function deleteCustomer(custID){

    //need to delete from the system
    //api/anonymousfacade/deletecustomer/{customerId}

    $.ajax({
        type: "DELETE",
        url: `http://localhost:9002/api/anonymousfacade/deletecustomer/${custID}`
    }).then(function () {
        console.log("deleted entry " + custID + " from the database");
        
    }).fail(
        // what to do on error
        function (err) {
            console.error(err)
        }
        )

    //deleting from the page
    var current_row = $(`#row${custID}`)
    current_row.remove();

    console.log("deleted entry " + custID + " from the page")
}