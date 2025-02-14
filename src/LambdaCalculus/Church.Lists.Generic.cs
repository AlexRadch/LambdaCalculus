using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // List := CPair<dynamic, List> := λf. (λh.λt. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic List<out T>(Boolean<T, List<T>> b);

    #endregion

    #region Constants

    // Nil := False
    public static Func<List<T>, dynamic> NilTF<T>(T h) => FalseF<T, List<T>>(h);
    public static List<T> NilT<T>() => NilTF;
    public static Func<List<T>> LazyNilT<T>() => () => NilTF;

    #endregion

    #region Operations

    // Cons := Pair
    public static Func<dynamic, List<T>> ConsT<T>(T h) => t => b => b(h)(t);

    // Head := First
    public static dynamic Head<T>(List<T> p) => p(h => t => h!);

    // Tail := Second
    public static List<T> Tail<T>(List<T> p) => p(h => t => t);

    #endregion

    #region Logical

    // IsNil := λl. l (λh.λt.λd. False⁡) True
    public static Boolean IsNil<T>(List<T> p) => p(h => t => new Func<dynamic, Boolean>(d => False))(True);

    #endregion

    #region Extensions

    public static List<T> ToChurchListT<T>(this IEnumerable<T> source)
    {
        List<T> list = NilF;
        foreach (T item in source.Reverse())
            list = ConsT(item)(list);

        return list;
    }

    // Church ListPair to LinkedList<dynamic>
    public static IEnumerable<dynamic> UnChurch<T>(this List<T> p)
    {
        while (!IsNil(p).UnChurch())
        {
            yield return Head(p);
            p = Tail(p);
        }
    }

    #endregion
}
