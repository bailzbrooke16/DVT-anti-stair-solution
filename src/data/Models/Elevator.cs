namespace data.Models
{
    public class Elevator
    {
        public int currentFloor {get;set;} =0;
        public string direction {get; set;} = "None";
        public List<int> floorsToStopAt {get; set;} = new List<int>();
        public List <int> inaccessableFloors {get; set;} = new List<int>();
        public Dictionary<int,Trip> activeTrips {get; set;} = new Dictionary<int,Trip>();
    }
}