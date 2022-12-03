namespace LambdaCalculus
{
    #region Utility

    // Unit := λx. some logic
    public delegate dynamic Unit(dynamic value);


    #endregion

    public static partial class ChurchUnit
    {
        // Id := λx.x
        // Id := T -> T
        public static dynamic Id(dynamic x) => x;
    }
}
