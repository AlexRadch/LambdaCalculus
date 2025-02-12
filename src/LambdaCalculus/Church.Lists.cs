using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // ListNode := CPair<dynamic, ListNode> := λf. (λh.λt. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic ListNode(Boolean<dynamic, ListNode> b);

    #endregion

    #region Constants

    // Nil := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<dynamic, dynamic> NilF(dynamic h) => FalseF<dynamic, dynamic>(h);
    public static readonly ListNode Nil = NilF;

    #endregion

    #region Constructors

    // Cons := Pair
    public static Func<dynamic, ListNode> Cons(dynamic h) => t => b => b(h)(t);

    #endregion

    #region Properties

    // Head := First
    public static dynamic Head(ListNode l) => l(h => t => h);

    // Tail := Second
    public static ListNode Tail(ListNode l) => l(h => t => t);

    // IsNil := λl. l (λh.λt.λ_. False⁡) True
    public static Boolean IsNil(ListNode p) => p(h => t => new Func<dynamic, Boolean>(_ => False))(True);

    // Length := Fold (λa.λh. Succ a) Zero
    public static Numeral Length(ListNode l) => Fold(a => h => Succ(a))(Zero)(l);

    #endregion

    #region Operations

    // Map := Z (λr.λf.λl. l (λh.λt.λ_. Cons (f h) (r f t)) Nil)
    public static Func<Func<dynamic, dynamic>, Func<ListNode, ListNode>> Map =
        Combinators.Z<Func<dynamic, dynamic>, Func<ListNode, ListNode>>(r => f => l => l
            (h => t => new Func<ListNode, ListNode>(_ => 
                Cons(f(h))(r(f)(t))
            ))
            (Nil));

    // Fold := Z (λr.λf.λa.λl. l (λh.λt.λ_. r f (f a h) t) a)
    public static Func<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>> Fold =
        Combinators.Z<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>>(r => f => a => l => l
            (h => t => new Func<dynamic, dynamic>(_ =>
                r(f)(f(a)(h))(t)
            ))
            (a));

    // RFold := Z (λr.λf.λa.λl. l (λh.λt.λ_. (f (r f a t) h)) a)
    public static Func<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>> RFold =
        Combinators.Z<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>>(r => f => a => l => l
            (h => t => new Func<dynamic, dynamic>(_ =>
                f(r(f)(a)(t))(h)
            ))
            (a));


    // Filter := Z (λr.λf.λl. l (λh.λt.λ_. f h (Cons h (r f t)) (r f t)) Nil)
    public static Func<Func<dynamic, Boolean>, Func<ListNode, ListNode>> Filter =
        Combinators.Z<Func<dynamic, Boolean>, Func<ListNode, ListNode>>(r => f => l => l
            (h => t => new Func<ListNode, ListNode>(_ =>
                f(h)(Cons(h)(r(f)(t)))(r(f)(t))
            ))
            (Nil));

    // Reverse := Fold (λa.λh. Cons h a) Nil
    public static ListNode Reverse(ListNode l) => Fold(a => h => Cons(h)(a))(Nil)(l);

    // Concat := λl1.λl2. Fold Cons l2 l1
    public static Func<ListNode, ListNode> Concat(ListNode l1) => l2 => RFold(a => h => Cons(h)(a))(l2)(l1);

    // Append := λl.λt. Concat l (Cons t Nil)
    public static Func<dynamic, ListNode> Append(ListNode l) => t => Concat(l)(Cons(t)(Nil));

    //// Skip := λn.λl. n (λt IsNil t Nil (Tail t)) l
    //public static Func<ListNode, ListNode> Skip(Numeral n) => l => n(t => 
    //    LazyIf(IsNil(t))
    //        (new Func<dynamic>(() => t))
    //        (new Func<dynamic>(() => Tail(t)))
    //    )(l);

    // Skip := Z (λr.λn.λl. l (λh.λt.λ_. IsZero n l (r (Pred n) t)) Nil)
    public static Func<Numeral, Func<ListNode, ListNode>> Skip =
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(_ =>
                LazyIf(IsZero(n))
                    (() => l)
                    (() => r(Pred(n))(t))
            ))
            (Nil));

    // Take := Z (λr.λn.λl. l (λh.λt.λ_. IsZero n Nil (Cons h (r (Pred n) t))) Nil)
    public static Func<Numeral, Func<ListNode, ListNode>> Take =
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(_ =>
                LazyIf(IsZero(n))
                    (() => Nil)
                    (() => Cons(h)(r(Pred(n))(t))
                )
            ))
            (Nil));

    // SkipLast := λn.λl1. Second(
    //     Z (λr.λl. l
    //         (λh.λt.λ_. r(t)
    //             (λn2.λt2. IsZero(n2) (Pair Zero (Cons h t2)) (Pair (Pred n2) Nil))
    //         (Pair n Nil)
    //     )
    //     (l1)
    // )
    public static Func<ListNode, ListNode> SkipLast(Numeral n) => l1 =>
        Second(Combinators.Z<ListNode, CPair<Numeral, ListNode>>(r => l => l(h => t => 
            new Func<CPair<Numeral, ListNode>, CPair<Numeral, ListNode>>(_ =>
                r(t)(n2 => t2 =>
                    LazyIf(IsZero(n2))
                        (() => Pair<Numeral, ListNode>(Zero)(Cons(h)(t2)))
                        (() => Pair<Numeral, ListNode>(Pred(n2))(Nil))
                )
            ))
            (Pair<Numeral, ListNode>(n)(Nil))
        )
        (l1));

    // TakeLast := λn.λl1. Second(
    //     Z (λr.λl. l
    //         (λh.λt.λ_. r(t)
    //             (λn2.λt2. IsZero(n2) (Pair Zero t2) (Pair (Pred n2) (Cons h t2)))
    //         (Pair n Nil)
    //     )
    //     (l1)
    // )
    public static Func<ListNode, ListNode> TakeLast(Numeral n) => l1 =>
        Second(Combinators.Z<ListNode, CPair<Numeral, ListNode>>(r => l => l(h => t =>
            new Func<CPair<Numeral, ListNode>, CPair<Numeral, ListNode>>(_ =>
                r(t)(n2 => t2 =>
                    LazyIf(IsZero(n2))
                        (() => Pair<Numeral, ListNode>(Zero)(t2))
                        (() => Pair<Numeral, ListNode>(Pred(n2))(Cons(h)(t2)))
                )
            ))
            (Pair<Numeral, ListNode>(n)(Nil))
        )
        (l1));

    // All := Z (λr.λf.λl. l (λh.λt.λ_. f h (r f t) (False) True)
    public static Func<Func<dynamic, Boolean>, Func<ListNode, Boolean>> All =
        Combinators.Z<Func<dynamic, Boolean>, Func<ListNode, Boolean>>(r => f => l => l
            (h => t => new Func<Boolean, Boolean>(_ =>
                LazyIf(f(h))
                    (new Func<Boolean>(() => r(f)(t)))
                    (LazyFalse)
            ))
            (True));

    // Any := Z (λr.λf.λl. l (λh.λt.λ_. f h (True) (r f t) False)
    public static Func<Func<dynamic, Boolean>, Func<ListNode, Boolean>> Any =
        Combinators.Z<Func<dynamic, Boolean>, Func<ListNode, Boolean>>(r => f => l => l
            (h => t => new Func<Boolean, Boolean>(_ =>
                LazyIf(f(h))
                    (LazyTrue)
                    (new Func<Boolean>(() => r(f)(t)))
            ))
            (False));

    // AtIndex := λn.λl. Head (Skip n l)
    public static Func<ListNode, dynamic> AtIndex(Numeral n) => l => Head(Skip(n)(l));

    // IndexOf := λf. (Z (λr.λn.λl. l (λh.λt.λ_. f h (Cons n Nil) (r (Succ n) t)) Nil)) Zero
    public static Func<ListNode, ListNode> IndexOf(Func<dynamic, Boolean> f) =>
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(_ =>
                LazyIf<ListNode>(f(h))
                    (new Func<ListNode>(() => Cons(n)(Nil)))
                    (new Func<ListNode>(() => r(Succ(n))(t)))
            ))
            (Nil)
        )
        (Zero);

    //// LastIndexOf := RFold (λa.λh. a (λn.λt.λ_. Cons (Succ n) Nil) (f h (Cons Zero Nil) Nil)) Nil
    //public static Func<ListNode, ListNode> LastIndexOf(Func<dynamic, dynamic> f) => l =>
    //    RFold(a => h => ((ListNode)a)
    //        (n => t => new Func<ListNode, ListNode>(_ =>
    //            Cons(Succ(n))(Nil)
    //        ))
    //        (LazyIf<ListNode>(f(h))
    //            (new Func<ListNode>(() => Cons(Zero)(Nil)))
    //            (new Func<ListNode>(() => Nil)))
    //    )(Nil)(l);

    // LastIndexOf := λf. (Z (λr.λn.λl. l (λh.λt.λ_. (λi IsNil i (f(h) (Cons n Nil) Nil) i) (r (Succ n) t)) Nil)) Zero
    public static Func<ListNode, ListNode> LastIndexOf(Func<dynamic, dynamic> f) =>
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(_ =>
                new Func<ListNode, ListNode>(i =>
                    LazyIf(IsNil(i))
                        (() => f(h)(Cons(n)(Nil))(Nil))
                        (() => i)
                )
                (r(Succ(n))(t))
            ))
            (Nil)
        )
        (Zero);

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
