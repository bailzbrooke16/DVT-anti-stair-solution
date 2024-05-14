using data.Models;

namespace services
{
    public interface ITripProcessorService
    {
        public Trip? getNextTrip();
        public void addTripToQueue(Trip trip);

    }
}