﻿using System.ComponentModel;
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

    // IsNil := λl. l (λh.λt.λd. False⁡) True
    public static Boolean IsNil(ListNode p) => p(h => t => new Func<dynamic, Boolean>(z => False))(True);

    // Length := Fold (λa.λh. Succ a) Zero
    public static Numeral Length(ListNode l) => Fold(a => h => Succ(a))(Zero)(l);

    #endregion

    #region Operations

    // Map := Z (λr.λf.λl. l (λh.λt.λd. Cons (f h) (r f t)) Nil)
    public static Func<Func<dynamic, dynamic>, Func<ListNode, ListNode>> Map =
        Combinators.Z<Func<dynamic, dynamic>, Func<ListNode, ListNode>>(r => f => l => l
            (h => t => new Func<ListNode, ListNode>(z => 
                Cons(f(h))(r(f)(t))
            ))
            (Nil));

    // Fold := Z (λr.λf.λa.λl. l (λh.λt.λd. r f (f a h) t) a)
    public static Func<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>> Fold =
        Combinators.Z<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>>(r => f => a => l => l
            (h => t => new Func<dynamic, dynamic>(z =>
                r(f)(f(a)(h))(t)
            ))
            (a));

    // RFold := Z (λr.λf.λa.λl. l (λh.λt.λd. (f (r f a t) h)) a)
    public static Func<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>> RFold =
        Combinators.Z<Func<dynamic, Func<dynamic, dynamic>>, Func<dynamic, Func<ListNode, dynamic>>>(r => f => a => l => l
            (h => t => new Func<dynamic, dynamic>(z =>
                f(r(f)(a)(t))(h)
            ))
            (a));


    // Filter := Z (λr.λf.λl. l (λh.λt.λd. f h (Cons h (r f t)) (r f t)) Nil)
    public static Func<Func<dynamic, Boolean>, Func<ListNode, ListNode>> Filter =
        Combinators.Z<Func<dynamic, Boolean>, Func<ListNode, ListNode>>(r => f => l => l
            (h => t => new Func<ListNode, ListNode>(z =>
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

    // Skip := Z (λr.λn.λl. l (λh.λt.λz. IsZero n l (r (Pred n) t)) Nil)
    public static Func<Numeral, Func<ListNode, ListNode>> Skip =
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(z =>
                LazyIf(IsZero(n))
                    (() => l)
                    (() => r(Pred(n))(t))
            ))
            (Nil));

    // Take := Z (λr.λn.λl. l (λh.λt.λz. IsZero n Nil (Cons h (r (Pred n) t))) Nil)
    public static Func<Numeral, Func<ListNode, ListNode>> Take =
        Combinators.Z<Numeral, Func<ListNode, ListNode>>(r => n => l => l
            (h => t => new Func<ListNode, ListNode>(z =>
                LazyIf(IsZero(n))
                    (() => Nil)
                    (() => Cons(h)(r(Pred(n))(t))
                )
            ))
            (Nil));

    // AtIndex := λn.λl. Head (Skip n l)
    public static Func<ListNode, dynamic> AtIndex(Numeral n) => l => Head(Skip(n)(l));

    // IndexOf := Z (λr.λn.λf.λl. l (λh.λt.λz. f(h) n (r (Succ n) t)) Zero) Zero f l

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
