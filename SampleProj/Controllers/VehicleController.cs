using Microsoft.AspNetCore.Mvc;
using SampleProj.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Microsoft.AspNetCore.Cors;

namespace SampleProj.Controllers
{   

    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {   

        private string connectionString = @"Server=(localdb)\MSSQLLOCALDB;Database=Vehicle;Trusted_Connection=True;";
        
        [HttpGet]
        public string Get()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM VehicleDetails", connection);
                SqlDataReader reader = command.ExecuteReader();
              
                while (reader.Read())
                {
                    Guid id = reader.GetGuid(0);
                    string registration = reader.GetString(1);
                    string year = reader.GetString(2);
                    string make = reader.GetString(3);
                    string model = reader.GetString(4);
                    string info = reader.GetString(5);
                    bool isActive = reader.GetBoolean(6);

                    vehicles.Add(new Vehicle(id, registration, year, make, model, info, isActive));
                }
            }

            return JsonConvert.SerializeObject(vehicles);
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            List<Vehicle> vehicles1 = new List<Vehicle>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM VehicleDetails where Id = '" + id + "'", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Guid Id = reader.GetGuid(0);
                    string registration = reader.GetString(1);
                    string year = reader.GetString(2);
                    string make = reader.GetString(3);
                    string model = reader.GetString(4);
                    string info = reader.GetString(5);
                    bool isActive = reader.GetBoolean(6);

                    vehicles1.Add(new Vehicle(Id, registration, year, make, model, info, isActive));
                }
            }

            return JsonConvert.SerializeObject(vehicles1);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE VehicleDetails SET IsActive=0 WHERE Id=@id", connection);
                command.Parameters.AddWithValue("@id", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok($"Vehicle with ID {id} deleted successfully.");
                }
                else
                {
                    return NotFound($"Vehicle with ID {id} not found.");
                }
            }
        }
        [HttpPost]
        public IActionResult Post(Vehicle vehicle)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("INSERT INTO VehicleDetails VALUES (@id, @vin, @year, @make, @model, @description, @isActive)", connection);
                command.Parameters.AddWithValue("@id", Guid.NewGuid());
                command.Parameters.AddWithValue("@vin", vehicle.VIN);
                command.Parameters.AddWithValue("@year", vehicle.Year);
                command.Parameters.AddWithValue("@make", vehicle.Make);
                command.Parameters.AddWithValue("@model", vehicle.Model);
                command.Parameters.AddWithValue("@description", vehicle.Description);
                command.Parameters.AddWithValue("@isActive", vehicle.IsActive);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok($"Vehicle with registration {vehicle.VIN} added successfully.");
                }
                else
                {
                    return BadRequest("Unable to add vehicle.");
                }
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(Guid id,Vehicle vehicle)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE VehicleDetails SET VIN=@vin,year=@year,make=@make,model=@model,description=@description,isActive=@isActive WHERE id=@id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@vin", vehicle.VIN);
                command.Parameters.AddWithValue("@year", vehicle.Year);
                command.Parameters.AddWithValue("@make", vehicle.Make);
                command.Parameters.AddWithValue("@model", vehicle.Model);
                command.Parameters.AddWithValue("@description", vehicle.Description);
                command.Parameters.AddWithValue("@isActive", vehicle.IsActive);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok($"Vehicle with Id {id} updated successfully.");
                }
                else
                {
                    return BadRequest("Unable to update vehicle.");
                }
            }
        }
    }
}