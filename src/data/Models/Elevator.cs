namespace data.Models
{
    public class Elevator
    {
        public int currentFloor {get;set;}
        public string direction {get; set;} = "None";
        public List<int> floorsToStopAt {get; set;} = new List<int>();
    }

}