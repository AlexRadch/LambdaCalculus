namespace LambdaCalculus
{
    // Unit(x) := λx. some logic
    // Unit(x) := λx. some logic
    public delegate TResult Unit<in T, out TResult>(T x); // the same as Func<T, TResult>

    public static partial class ChurchUnit
    {
        // Id(x) := λx. x
        // T -> T
        public static T Id<T>(T x) => x;
    }
}
