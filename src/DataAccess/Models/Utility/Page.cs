namespace DanCart.DataAccess.Models.Utility;

public struct Page(int Number, int Size)
{
    public int Number = Number;
    public int Size = Size;
    public void ApplySizeRule(int min, int max) => Size = Math.Clamp(Size, min, max);
}
