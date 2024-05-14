namespace data.Models
{
    public class Trip
    {
        public int numberOfPassengers {get; set;}
        public int originFloor {get; set;}
        public int destinationFloor {get; set;}
        public string direction {get; set;} = "None";
    }
}