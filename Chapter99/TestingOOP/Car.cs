namespace TestingOOP;

public class Car
{
    private int _velocity { get; set; }
    
    public int Velocity()
    {
        return 0;
    }
    
    // se puede acceder siempre al mismo assemble 
    internal int Volumne()
    {
        return 1;
    }
    
    // private para cualquier clases fuera de la gerarquia (no visible cuando se instancia)
    protected int Velocity1()
    {
        return 0;
    }
    
}