public enum Navigation
{
    [StringValue("forward")]
    Forward,

    [StringValue("backward")]
    Backward,

    [StringValue("left")]
    Left,

    [StringValue("right")]
    Right,

    [StringValue("turnCW")]
    TurnCW,

    [StringValue("turnCCW")]
    TurnCCW,

    None
}


public static class NavigationExtensions
{
    public static bool Translates(this Navigation navigation) => navigation == Navigation.Forward || navigation == Navigation.Backward || navigation == Navigation.Left || navigation == Navigation.Right;

    public static bool Rotates(this Navigation navigation) => navigation == Navigation.TurnCW || navigation == Navigation.TurnCCW;
}