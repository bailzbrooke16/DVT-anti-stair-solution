using data.Models;

namespace services
{
    public class TripProcessorService : ITripProcessorService
    {
        private Queue<Trip> tripQueue = new Queue<Trip>();
        public Trip? getNextTrip()
        {
            try
            {
               return tripQueue.Dequeue();
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public void addTripToQueue(Trip trip)
        {
            try
            {
                tripQueue.Enqueue(trip);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding trip to queue: {ex.Message}");
            }
        }
    }
}