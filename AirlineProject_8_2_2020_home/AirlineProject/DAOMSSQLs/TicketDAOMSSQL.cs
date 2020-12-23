﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace AirlineProject
{

    public class TicketDAOMSSQL : ITicketDAO //supposed to be internal
    {
        private bool testMode = false;
        private string connection_string = "";

        public TicketDAOMSSQL(bool testMode = false)
        {
            this.testMode = testMode;
            connection_string = !testMode ? AirlineProjectConfig.CONNECTION_STRING : AirlineProjectConfig.TEST_CONNECTION_STRING;
        }

        public void Add(Ticket t)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ADD_TICKET", con))
                {
                    cmd.Parameters.AddWithValue("@flightId", t.FlightId);
                    cmd.Parameters.AddWithValue("@customerId", t.CustomerId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    t.ID = (long)(decimal)cmd.ExecuteScalar(); //adds the id to the returned ticket which is nice!
                }
            }
        }

        public void ClearTickets()
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("CLEAR_TICKETS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }

        public void ClearTicketsHistory()
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("CLEAR_TICKETS_HISTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }

        public Ticket Get(long id)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GET_TICKET", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket ticket = new Ticket()
                            {
                                ID = (long)reader["ID"],
                                FlightId = (long)reader["FLIGHT_ID"],
                                CustomerId = (long)reader["CUSTOMER_ID"]
                            };
                            return ticket;
                        }
                        return null;
                    }
                }
            }
        }

        public IList<Ticket> GetAll()
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GET_ALL_TICKETS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket ticket = new Ticket()
                            {
                                ID = (long)reader["ID"],
                                FlightId = (long)reader["FLIGHT_ID"],
                                CustomerId = (long)reader["CUSTOMER_ID"]
                            };
                            tickets.Add(ticket);
                        }
                    }
                }
            }
            return tickets;
        }

        public IList<Ticket> GetTicketsByAirlineCompany(AirlineCompany airline)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GET_TICKETS_BY_AIRLINE_COMPANY", con))
                {
                    cmd.Parameters.AddWithValue("@airlineCompanyId", airline.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket ticket = new Ticket()
                            {
                                ID = (long)reader["ID"],
                                FlightId = (long)reader["FLIGHT_ID"],
                                CustomerId = (long)reader["CUSTOMER_ID"]
                            };
                            tickets.Add(ticket);
                        }
                    }
                }
            }
            return tickets;
        }

        public IList<Ticket> GetTicketsByCustomerId(Customer customer)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GET_TICKETS_BY_CUSTOMER_ID", con))
                {
                    cmd.Parameters.AddWithValue("@customerId", customer.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket ticket = new Ticket()
                            {
                                ID = (long)reader["ID"],
                                FlightId = (long)reader["FLIGHT_ID"],
                                CustomerId = (long)reader["CUSTOMER_ID"]
                            };
                            tickets.Add(ticket);
                        }
                    }
                }
            }
            return tickets;
        }

        public void Remove(Ticket t)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("REMOVE_TICKET", con))
                {
                    cmd.Parameters.AddWithValue("@id", t.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }

        public void RemoveTicketsByCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("REMOVE_TICKETS_BY_CUSTOMER", con))
                {
                    cmd.Parameters.AddWithValue("@customerId", customer.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }

        public void RemoveTicketsByFlight(Flight flight)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("REMOVE_TICKETS_BY_FLIGHT", con))
                {
                    cmd.Parameters.AddWithValue("@flightId", flight.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }

        public void Update(Ticket t)
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE_TICKET", con))
                {
                    cmd.Parameters.AddWithValue("@flightId", t.FlightId);
                    cmd.Parameters.AddWithValue("@customerId", t.CustomerId);
                    cmd.Parameters.AddWithValue("@id", t.ID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{

                    //}
                }
            }
        }
    }
}
