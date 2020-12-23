function getCountries(){
    $.ajax({
        url: `http://localhost:9002/api/anonymousfacade/getallcountries`
    }).then(function (data) {
        console.log(data);
        //$('#result').html(JSON.stringify(data))
        var $origin_countries_dropdown = $("#origincountries")
        var $destination_countries_dropdown = $("#destinationcountries")

        $.each(data, function (i, country) {
            $origin_countries_dropdown.append(`<option value="${country.CountryName}">${country.CountryName}</option>`)
            $destination_countries_dropdown.append(`<option value="${country.CountryName}">${country.CountryName}</option>`)
        })
    }).fail(
        // what to do on error
        function (err) {
            console.error(err)
        }
        )
}

function searchFlights(){

    //need to also add all the info to a table

    var OriginCountries = $('#origincountries').val()
	var DestinationCountries = $('#destinationcountries').val()
	var Sorting = $('#sorting').val()

    $.ajax({
        url: `http://localhost:9002/api/anonymousfacade/searchflights?origin=${OriginCountries}&destination=${DestinationCountries}&sorting=${Sorting}`
    }).then(function (data) {
        console.log(data);
        
        var $flights_table = $("#flightstable")
                $("#flightstable th").remove(); //i wonder if we'll learn this
                $("#flightstable tr").remove();
                $flights_table.append(`
						< tr >
						<th>ID</th>
						<th>AIRLINE NAME</th>
						<th>ORIGIN COUNTRY</th>
                        <th>DESTINATION COUNTRY</th>
                        <th>DEPARTURE TIME</th>
                        <th>LANDING TIME</th>
						<th>REMAINING TICKETS</th></tr >`);

				$.each(data, function (i, flight) {
					$flights_table.append(`<tr> <td> ${flight.ID} </td><td>${flight.AirlineCompanyName}</td><td>${flight.OriginCountryName}</td><td>${flight.DestinationCountryName}</td><td> ${flight.DepartureTime} </td><td> ${flight.LandingTime} </td><td>${flight.RemainingTickets}</td></tr>`)
				})
    }).fail(
        // what to do on error
        function (err) {
            console.error(err)
        }
        )
}

