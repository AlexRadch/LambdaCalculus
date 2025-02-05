
namespace LambdaCalculus;

public static partial class Extensions

{
    public static Func<T> AsLazy<T>(Func<T> value) => value;

}
