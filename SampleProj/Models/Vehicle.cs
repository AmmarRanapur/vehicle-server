namespace SampleProj.Models
{
    public class Vehicle
    {   
        public Vehicle(Guid id,string vin,string year,string make,string model,string description,bool isActive)
        {
            Id= id;
            VIN=vin;   
            Year= year;
            Make= make;
            Model= model;
            Description= description;
            IsActive=isActive;
        }
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
