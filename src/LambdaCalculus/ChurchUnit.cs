namespace LambdaCalculus
{
    #region Utility

    // Unit := λx. some logic
    public delegate T Unit<T>(T value);

    // Unit := λx. some logic
    public delegate dynamic Unit(dynamic value);

    #endregion

    public static partial class ChurchUnit
    {
        // Id := λx.x
        public static T Id<T>(T x) => x;

        //// Id := λx.x
        //public static dynamic Id(dynamic x) => x;

        // IdV is sugar for new Unit(Id). Workaround for compiler error CS0173.
        public static readonly Unit IdV = Id;
    }
}
