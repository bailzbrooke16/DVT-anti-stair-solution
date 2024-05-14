using data.Models;

namespace services
{
    public interface ITripProcessorService
    {
        public void actionNextTrip();
        public void addTripToQueue(Trip trip);

    }
}