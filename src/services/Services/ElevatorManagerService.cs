using data.Models;

namespace services
{
    public class ElevatorManagerService : IElevatorManagerService
    {
        private List<Elevator> initialElevators;
        private ITripProcessorService tripProcessor;
        public ElevatorManagerService()
        {
            this.initialElevators = new List<Elevator>();
            this.seedInitialElevators();

            this.tripProcessor = new TripProcessorService();

            Thread elevatorCheckThread = new Thread(ElevatorStatusChecker);
            elevatorCheckThread.Start();

            Thread inputAllowerThread = new Thread(ListenForInput);
            inputAllowerThread.Start();

            Thread processQueueThread = new Thread(processQueue);
            processQueueThread.Start();

        }

        private void seedInitialElevators()
        {
            for (int i = 0; i < 5; i++)
            {
                this.initialElevators.Add(new Elevator());
            }
        }

        private void ElevatorStatusChecker()
        {
            while (true)
            {
                int index = 0;
                Console.WriteLine("--------");
                Console.WriteLine($"Elevator|Current Floor");
                foreach (var elevator in initialElevators)
                {
                    Console.Write($"{index}|{elevator.currentFloor}\t");
                    index++;
                }
                Console.WriteLine("\n--------");
                Thread.Sleep(15000); // Wait for 5 seconds
            }
        }

        private void ListenForInput()
        {
            try
            {

                int originFloor = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine($"How many passengers are at: {originFloor}");
                try
                {
                    int passengers = Convert.ToInt16(Console.ReadLine());
                    Console.WriteLine($"What floor do you need to go to?");
                    try
                    {
                        int destinationFloor = Convert.ToInt16(Console.ReadLine());
                        if(originFloor == destinationFloor){
                            Console.WriteLine($"You cannot have the same origin and destination floor");
                        }
                        else{
                            createTrip(originFloor, passengers, destinationFloor);
                        } 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred with your destination floor: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred with the number of passengers you have selected: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred with the origin floor: {ex.Message}");
            }
        }

        private void createTrip(int originFloor, int passengers, int destinationFloor)
        {
            try
            {
                Trip newTrip = new Trip()
                {
                    originFloor = originFloor,
                    destinationFloor = destinationFloor,
                    numberOfPassengers = passengers,
                    direction = originFloor < destinationFloor ? "Upwards" : "Downwards"
                };
                this.tripProcessor.addTripToQueue(newTrip);
                Console.WriteLine($"Your trip has been added to the queue");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred adding your trip to the queue : {ex.Message}");
            }
        }

        private void processQueue()
        {
            while (true)
            {
                Trip? nextTrip = this.tripProcessor.getNextTrip();
                if (nextTrip != null)
                {
                    Elevator bestElevator = getMostViableElevator(nextTrip);

                }
                Thread.Sleep(5000);
            }
        }

        private List<Elevator> getElevatorsInASpecificDirection(List<Elevator> elevatorSet , string direction)
        {
            return elevatorSet.Where(elevator => elevator.direction == direction).ToList();
        }

        private Elevator getNearestElevatorToFloor(List<Elevator> viableElevators, int desiredFloor)
        {
            Elevator nearestElevator = viableElevators[0];
            int minDistance = int.MaxValue;

            foreach (var elevator in viableElevators)
            {
                int distance = Math.Abs(elevator.currentFloor - desiredFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestElevator = elevator;
                }
            }

            return nearestElevator;
        }

        private Elevator getMostViableElevator(Trip trip){
            List<Elevator> inactiveViableElevators = getElevatorsInASpecificDirection(this.initialElevators , "None");
            List<Elevator> viableElevators = getElevatorsInASpecificDirection(this.initialElevators, trip.direction); 
            viableElevators.AddRange(inactiveViableElevators);

            if(viableElevators.Count == 0){
                Elevator nearestElevator = getNearestElevatorToFloor(this.initialElevators, trip.originFloor);
                return nearestElevator;
            }
            else{
                Elevator nearestElevator = getNearestElevatorToFloor(viableElevators,trip.originFloor);
                return nearestElevator;
            }

            
        }
    }
}
