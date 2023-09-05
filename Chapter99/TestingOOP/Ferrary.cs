namespace TestingOOP;

public class Ferrary : Vehicle
{
    public override void TurnOnVehicle()
    {
        TurnOn = true;
        Console.WriteLine("Turning ON Ferrary");
    }
}