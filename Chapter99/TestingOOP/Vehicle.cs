using System.Threading.Channels;

namespace TestingOOP;

public class Vehicle
{
    protected bool TurnOn { get; set; }
    
    public virtual void TurnOnVehicle()
    {
        TurnOn = true;
        Console.WriteLine("Turning ON default vehicle");
    }
}