namespace TempleOfDoom.Logic.Helpers;

public static class RoomMathHelper
{
    // Relatieve positie in nieuwe kamer bepalen
    public static int TranslateX(int currentX, int currentW, int nextW)
    {
        return (currentX - (currentW / 2)) + (nextW / 2);
    }

    public static int TranslateY(int currentY, int currentH, int nextH)
    {
        return (currentY - (currentH / 2)) + (nextH / 2);
    }
    
    public static int Clamp(int value, int max)
    {
        // Zorgen dat speler in kamer laadt bij verandering van kamergrootte
        return Math.Max(0, Math.Min(value, max - 1));
    }
}