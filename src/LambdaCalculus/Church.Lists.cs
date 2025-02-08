
using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // ListNode := CPair<dynamic, ListNode> := λf. (λh.λt. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic ListNode(Boolean b);

    #endregion

    #region Constants

    // Nil := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<dynamic, dynamic> NilF(dynamic h) => FalseF<dynamic, dynamic>(h);
    public static readonly ListNode Nil = NilF;

    #endregion

    #region Operations

    // Cons := Pair
    public static Func<dynamic, ListNode> Cons(dynamic h) => t => b => b(h)(t);

    // Head := First
    public static dynamic Head(ListNode p) => p(h => t => h);

    // Tail := Second
    public static ListNode Tail(ListNode p) => p(h => t => t);

    #endregion

    #region Logical

    // IsNil := λl. l (λh.λt.λd. False⁡) True
    public static Boolean IsNil(ListNode p) => p(h => t => new Func<dynamic, Boolean>(d => False))(True);

    #endregion

    #region Extensions

    public static ListNode ToChurchList<T>(this IEnumerable<T> source)
    {
        ListNode list = Nil;
        foreach (T item in source.Reverse())
            list = Cons(item!)(list);

        return list;
    }

    // Church ListPair to LinkedList<dynamic>
    public static IEnumerable<dynamic> UnChurch(this ListNode p)
    {
        while (!IsNil(p).UnChurch())
        {
            yield return Head(p);
            p = Tail(p);
        }
    }

    #endregion
}
