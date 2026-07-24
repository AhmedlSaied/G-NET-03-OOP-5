using System;

namespace OOPAssignment05
{
    #region PART 01: THEORETICAL QUESTIONS ANSWERS

    /*
     * Q1: What is an interface in C#? Benefits & direct dependence vs interfaces.
     * - An interface is a full abstract contract (`interface`) that defines signatures for methods, 
     *   properties, events, or indexers without providing default storage/state.
     * - We use interfaces to decouple consumers from concrete implementations (Dependency Inversion),
     *   allowing code to interact with abstract capabilities rather than fixed class types.
     * - Benefits:
     *   1. Multiple Inheritance of Types: Enables a class to implement multiple contracts.
     *   2. Decoupling & Testability: Code depends on contracts, making mock objects easy to insert.
     *   3. Polymorphism across unrelated hierarchies: Allows completely different class hierarchies 
     *      to be processed uniformly if they implement the same interface (e.g., Ticket and Receipt implementing IPrintable).
     * 
     * Q2: Interface Member Name Collision & Explicit Interface Implementation.
     * a) Problem: Both `IEnglishSpeaker` and `IArabicSpeaker` contain `void Greet()`. Currently, `Translator` 
     *    implements a single implicit `Greet()` method that handles both interfaces simultaneously with a merged output.
     * b) Solution: Use Explicit Interface Implementation:
     *    void IEnglishSpeaker.Greet() => Console.WriteLine("Hello");
     *    void IArabicSpeaker.Greet() => Console.WriteLine("Ahlan");
     *    Technique Name: Explicit Interface Implementation.
     * c) No, you cannot call `translator.Greet()` directly on the object instance because explicit methods 
     *    are not public members of the class instance. You must cast the instance to the specific interface:
     *    ((IEnglishSpeaker)translator).Greet();
     *    ((IArabicSpeaker)translator).Greet();
     * 
     * Q3: Shallow Copy vs Deep Copy.
     * - Shallow Copy: Duplicates the top-level object structure. Primitive types are copied by value, 
     *   but reference-type fields copy only the memory address (reference).
     * - Deep Copy: Duplicates the top-level object AND recursively creates distinct copies of all 
     *   referenced objects, ensuring zero shared state.
     * - Usage: Shallow copy is suitable for simple flat value-based objects. Deep copy is mandatory 
     *   when objects contain nested reference types that must mutate independently.
     * - Risk of Shallow Copy: Mutating a reference field in the copied object silently modifies the original 
     *   object's data as both variables point to the exact same memory location on the heap.
     * 
     * Q4: Code Output & Explanation.
     * - Output:
     *   Dev - Testing
     *   QA - Testing
     * - Explanation: `MemberwiseClone()` created a shallow copy (`e2`). Modifying primitive `Title` on `e2` 
     *   ("QA") did not alter `e1.Title` ("Dev"). However, `Dept` is a reference-type field; both `e1.Dept` 
     *   and `e2.Dept` point to the same `Department` instance in memory. Setting `e2.Dept.Name = "Testing"` 
     *   mutated the underlying shared object, reflecting "Testing" for both employees.
     */

    #endregion
    #region SUPPORTING CLASSES

    public class Projector
    {
        public void Start() => Console.WriteLine("Projector started.");
        public void Stop() => Console.WriteLine("Projector stopped.");
    }

    #endregion
    #region CUSTOM INTERFACES

    // Requirement 1: Unified Printing Contract
    public interface IPrintable
    {
        void Print();
    }

    // Requirement 2: Booking & Cancellation Contract
    public interface IBookable
    {
        bool IsBooked { get; }
        bool Book();
        bool Cancel();
    }

    #endregion
    #region PART 02 - QUESTION 1: ABSTRACT BASE TICKET CLASS

    public abstract class Ticket : IPrintable, IBookable, ICloneable
    {
        private static int totalTickets = 0;

        private string movieName = "Unknown";
        private decimal price = 1m;

        public int TicketId { get; }

        public string MovieName
        {
            get => movieName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    movieName = value;
                }
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                if (value > 0)
                {
                    price = value;
                }
            }
        }

        public decimal PriceAfterTax => Price * 1.14m;

        // IBookable Implementation
        public bool IsBooked { get; private set; }

        public Ticket(string movieName, decimal price)
        {
            TicketId = ++totalTickets;
            MovieName = movieName;
            Price = price;
            IsBooked = false;
        }

        public Ticket(Ticket other)
        {
            TicketId = ++totalTickets;
            MovieName = other.MovieName;
            Price = other.Price;
            IsBooked = false; // Copy starts unbooked
        }

        public bool Book()
        {
            if (IsBooked) return false;
            IsBooked = true;
            return true;
        }

        public bool Cancel()
        {
            if (!IsBooked) return false;
            IsBooked = false;
            return true;
        }

        public abstract void Print();

        public abstract object Clone();
    }

    #endregion
    #region PART 02 - QUESTION 2: CHILD TICKET CLASSES

    public class StandardTicket : Ticket
    {
        public string SeatNumber { get; set; }

        public StandardTicket(string movieName, decimal price, string seatNumber)
            : base(movieName, price)
        {
            SeatNumber = seatNumber;
        }

        // Copy Constructor for Deep Copying
        public StandardTicket(StandardTicket other) : base(other)
        {
            SeatNumber = other.SeatNumber;
        }

        public override void Print()
        {
            string bookedStr = IsBooked ? "Yes" : "No";
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | Standard | Seat: {SeatNumber} | Price: {Price:F0} | After Tax: {PriceAfterTax:F1} | Booked: {bookedStr}");
        }

        public override object Clone()
        {
            return new StandardTicket(this);
        }
    }

    public class VIPTicket : Ticket
    {
        public bool LoungeAccess { get; set; }
        public decimal ServiceFee { get; } = 50m;

        public VIPTicket(string movieName, decimal price, bool loungeAccess)
            : base(movieName, price)
        {
            LoungeAccess = loungeAccess;
        }

        // Copy Constructor for Deep Copying
        public VIPTicket(VIPTicket other) : base(other)
        {
            LoungeAccess = other.LoungeAccess;
            ServiceFee = other.ServiceFee;
        }

        public override void Print()
        {
            string loungeStr = LoungeAccess ? "Yes" : "No";
            string bookedStr = IsBooked ? "Yes" : "No";
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | VIP | Lounge: {loungeStr} | Fee: {ServiceFee:F0} | Price: {Price:F0} | After Tax: {PriceAfterTax:F0} | Booked: {bookedStr}");
        }

        public override object Clone()
        {
            return new VIPTicket(this);
        }
    }

    public class IMAXTicket : Ticket
    {
        public bool Is3D { get; set; }

        public IMAXTicket(string movieName, decimal price, bool is3D)
            : base(movieName, price + (is3D ? 30m : 0m))
        {
            Is3D = is3D;
        }

        // Copy Constructor for Deep Copying
        public IMAXTicket(IMAXTicket other) : base(other)
        {
            Is3D = other.Is3D;
        }

        public override void Print()
        {
            string is3DStr = Is3D ? "Yes" : "No";
            string bookedStr = IsBooked ? "Yes" : "No";
            Console.WriteLine($"[Ticket #{TicketId}] {MovieName} | IMAX | 3D: {is3DStr} | Price: {Price:F0} | After Tax: {PriceAfterTax:F1} | Booked: {bookedStr}");
        }

        public override object Clone()
        {
            return new IMAXTicket(this);
        }
    }

    #endregion
    #region PART 02 - QUESTION 3: CINEMA CLASS & BOOKING HELPER

    public class Cinema
    {
        public string CinemaName { get; set; }
        private readonly Projector projector;
        private readonly Ticket[] tickets = new Ticket[20];

        public Cinema(string cinemaName)
        {
            CinemaName = cinemaName;
            projector = new Projector();
        }

        public bool AddTicket(Ticket t)
        {
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] == null)
                {
                    tickets[i] = t;
                    return true;
                }
            }
            return false;
        }

        public void PrintAllTickets()
        {
            Console.WriteLine("--- All Tickets ---");
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] != null)
                {
                    tickets[i].Print();
                }
            }
        }

        public void OpenCinema()
        {
            Console.WriteLine("=== Cinema Opened ===");
            projector.Start();
        }

        public void CloseCinema()
        {
            Console.WriteLine("\n=== Cinema Closed ===");
            projector.Stop();
        }
    }

    public static class BookingHelper
    {
        // Interface Polymorphism: Accepts any array of IPrintable objects
        public static void PrintAll(IPrintable[] printables)
        {
            Console.WriteLine("--- BookingHelper.PrintAll ---");
            foreach (var item in printables)
            {
                item?.Print();
            }
        }
    }

    #endregion
}