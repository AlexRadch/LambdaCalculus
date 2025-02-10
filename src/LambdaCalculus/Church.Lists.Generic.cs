using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // ListNode := CPair<dynamic, ListNode> := λf. (λh.λt. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic ListNode<out T>(Boolean<T, ListNode<T>> b);

    #endregion

    #region Constants

    // Nil := False
    public static Func<ListNode<T>, dynamic> NilTF<T>(T h) => FalseF<T, ListNode<T>>(h);
    //public static ListNode<T> NilT<T>() => NilTF;

    #endregion

    #region Operations

    // Cons := Pair
    public static Func<dynamic, ListNode<T>> ConsT<T>(T h) => t => b => b(h)(t);

    // Head := First
    public static dynamic Head<T>(ListNode<T> p) => p(h => t => h!);

    // Tail := Second
    public static ListNode<T> Tail<T>(ListNode<T> p) => p(h => t => t);

    #endregion

    #region Logical

    // IsNil := λl. l (λh.λt.λd. False⁡) True
    public static Boolean IsNil<T>(ListNode<T> p) => p(h => t => new Func<dynamic, Boolean>(d => False))(True);

    #endregion

    #region Extensions

    public static ListNode<T> ToChurchListT<T>(this IEnumerable<T> source)
    {
        ListNode<T> list = NilF;
        foreach (T item in source.Reverse())
            list = ConsT(item)(list);

        return list;
    }

    // Church ListPair to LinkedList<dynamic>
    public static IEnumerable<dynamic> UnChurch<T>(this ListNode<T> p)
    {
        while (!IsNil(p).UnChurch())
        {
            yield return Head(p);
            p = Tail(p);
        }
    }

    #endregion
}
