namespace LambdaCalculus
{
    // Unit(x) := λx. some logic
    public delegate TResult Unit<in T, out TResult>(T x); // the same as Func<T, TResult>
    public delegate TResult HidenValue<out TResult>(); // the same as Func<TResult>

    public static partial class ChurchUnit
    {
        // Id := λx. x
        // Id(x) -> x
        public static T Id<T>(T x) => x;

        public static HidenValue<T> HideValue<T>(T value) => () => value;
    }
}
