using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // List := CPair<dynamic, List> := λf. (λh.λt. )
    [DebuggerDisplay("{System.Linq.Enumerable.ToList(LambdaCalculus.Church.UnChurch(this))}")]
    public delegate dynamic List(Boolean<dynamic, List> b);

    #endregion

    #region Constants

    // Nil := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<dynamic, dynamic> NilF(dynamic h) => FalseF<dynamic, dynamic>(h);
    public static readonly List Nil = NilF;
    public static readonly Func<Boolean, List> LazyNil = _ => NilF;

    #endregion

    #region Constructors

    // Cons := Pair
    public static Func<List, List> Cons(dynamic h) => t => b => b(h)(t);

    #endregion

    #region Properties

    // Head := First
    public static dynamic Head(List l) => l(h => t => h);

    // Tail := Second
    public static List Tail(List l) => l(h => t => t);

    // TailOrNil := λl. l (λh.λt. True t⁡) Nil
    public static List TailOrNil(List l) => l(h => t => TrueF<List, List>(t))(Nil);

    // IsNil := λl. l (λh.λt. True False⁡) True
    public static Boolean IsNil(List p) => p(h => t => TrueF<Boolean, Boolean>(False))(True);

    // Length := Fold (λa.λh. Succ a) Zero
    public static Numeral Length(List l) => Fold(a => h => Succ(a))(Zero)(l);

    #endregion

    #region Operations

    // Fold := λf. Z (λr.λa.λl. l (λh.λt. True (r (f a h) t)) a)
    public static Func<dynamic, Func<List, dynamic>> Fold(Func<dynamic, Func<dynamic, dynamic>> f) =>
        Combinators.Z<dynamic, Func<List, dynamic>>(r => a => l => l
            (h => t => True(r(f(a)(h))(t)))(a));

    // RFold := λf.λa. Z (λr.λl. l (λh.λt. True (f (r t) h)) a)
    public static Func<dynamic, Func<List, dynamic>> RFold(Func<dynamic, Func<dynamic, dynamic>> f) => a =>
        Combinators.Z<List, dynamic>(r => l => l(h => t => True(f(r(t))(h)))(a));

    //// Map := λf. Z (λr.λl. l (λh.λt. True (Cons (f h) (r t))) Nil)
    //public static Func<List, List> Map(Func<dynamic, dynamic> f) => Combinators.Z<List, List>(r => l => 
    //    l(h => t => True(Cons(f(h))(r(t))))(Nil));

    // Map := λf.λl. RFold (λa.λh. Cons (f h) a) Nil l := λf. RFold (λa.λh. Cons (f h) a) Nil 
    public static Func<List, List> Map(Func<dynamic, dynamic> f) => l => RFold(a => h => Cons(f(h))(a))(Nil)(l);

    //// Filter := λf. Z (λr.λlr. lr (λh.λt. True (f h (Cons h (r t)) (r t)) Nil))
    //public static Func<List, List> Filter(Func<dynamic, dynamic> f) => Combinators.Z<List, List>(r => lr =>
    //    lr(h => t => True(LazyIf<List>(f(h))(new Func<List>(() => Cons(h)(r(t))))(new Func<List>(() => r(t)))))(Nil));

    // Filter := λf.λl. RFold (λa.λh. f h (Cons h a) a) Nil l := λf. RFold (λa.λh. f h (Cons h a) a) Nil
    public static Func<List, List> Filter(Func<dynamic, dynamic> f) => l => 
        RFold(a => h => LazyIf(f(h))(AsLazy(_ => Cons(h)(a)))(AsLazy(_ => a)))(Nil)(l);

    // Reverse := λl. Fold (λa.λh. Cons h a) Nil l := Fold (λa.λh. Cons h a) Nil
    public static List Reverse(List l) => Fold(a => h => Cons(h)(a))(Nil)(l);

    // Concat := λl1.λl2. RFold (λa.λh. Cons h a) l2 l1
    public static Func<List, List> Concat(List l1) => l2 => RFold(a => h => Cons(h)(a))(l2)(l1);

    // Append := λl.λv. Concat l (Cons v Nil)
    public static Func<dynamic, List> Append(List l) => t => Concat(l)(Cons(t)(Nil));

    // Skip := λn.λl. n TailOrNil l := λn. n TailOrNil
    // Skip := Z (λr.λn.λl. l (λh.λt. True (IsZero n l (r (Pred n) t))) Nil)
    public static Func<Numeral, Func<List, List>> Skip = Combinators.Z<Numeral, Func<List, List>>(r => n => l => 
        l(h => t => TrueF<List, List>(
            LazyIf<List>(IsZero(n))
                (_ => l)
                (_ => r(Pred(n))(t))
        ))
        (Nil));

    // SkipLast := λn.λl. IsZero(n) l (Second (
    //     Z (λr.λlr. lr (λhr.λtr. True
    //         (r tr (λna.λla. IsZero(na)
    //             (Pair Zero (Cons h la))
    //             (Pair (Pred na) Nil)
    //         )))
    //         (Pair n Nil)
    //     ) l))
    public static Func<List, List> SkipLast(Numeral n) => l => LazyIf<List>(IsZero(n))(_ => l)(_ => Second(
        Combinators.Z<List, CPair<Numeral, List>>(r => lr => lr(h => t => True
            (r(t)(na => la => LazyIf<CPair<Numeral, List>>(IsZero(na))
                (_ => Pair<Numeral, List>(Zero)(Cons(h)(la)))
                (_ => Pair<Numeral, List>(Pred(na))(Nil))
            )))
            (Pair<Numeral, List>(n)(Nil))
        )(l)));

    // SkipWhile := λf. Z (λr.λl. l (λh.λt. True (f hr (r tr) lr)) Nil)
    public static Func<List, List> SkipWhile(Func<dynamic, Boolean> f) =>
        Combinators.Z<List, List>(r => lr => lr (hr => tr => TrueF<List, List>(
            LazyIf<List>(f(hr))
                (AsLazy(_ => r(tr)))
                (AsLazy(_ => lr))
        ))
        (Nil));

    // Take := Z (λr.λn.λl. l (λh.λt. True (IsZero n Nil (Cons h (r (Pred n) t)))) Nil)
    public static Func<List, List> Take(Numeral n) =>
        Combinators.Z<Numeral, Func<List, List>>(r => n => l => l(h => t => TrueF<List, List>(
            LazyIf<List>(IsZero(n))
                (LazyNil)
                (_ => Cons(h)(r(Pred(n))(t)))
        ))
        (Nil)
        )(n);

    // SkipLast := λn.λl. IsZero(n) l (Second (
    //     Z (λr.λlr. lr (λhr.λtr. True
    //         (r tr (λna.λla. IsZero(na)
    //             (Pair Zero (Cons h la))
    //             (Pair (Pred na) Nil)
    //         )))
    //         (Pair n Nil)
    //     ) l))

    // TakeLast := λn.λl. IsZero(n) Nil (Second (
    //     Z (λr.λlr. lr (λh.λt. True
    //         ((r t) (λna.λla. IsZero(na)
    //             (Pair Zero la)
    //             (Pair (Pred na) lr)
    //         )))
    //         (Pair n Nil)
    //     ) l))
    public static Func<List, List> TakeLast(Numeral n) => l => LazyIf<List>(IsZero(n))(_ => Nil)(_ => Second(
        Combinators.Z<List, CPair<Numeral, List>>(r => lr => lr(h => t => True
            (r(t)(n => la => LazyIf<CPair<Numeral, List>>(IsZero(n))
                (_ => Pair<Numeral, List>(Zero)(la))
                (_ => Pair<Numeral, List>(Pred(n))(lr))
            )))
            (Pair<Numeral, List>(n)(Nil))
        )(l)));

    // TakeWhile := λf. Z (λr.λl. l (λh.λt. True (f d (Cons h (r t)) Nil) Nil))
    public static Func<List, List> TakeWhile(Func<dynamic, Boolean> f) =>
        Combinators.Z<List, List>(r => l => l(h => t => TrueF<List, List>(
            LazyIf(f(h))
                (AsLazy(_ => Cons(h)(r(t))))
                (LazyNil)
            ))
            (Nil));

    // All := Z (λr.λf.λl. l (λh.λt. True (f h (r f t) False)) True)
    public static Func<Func<dynamic, Boolean>, Func<List, Boolean>> All =
        Combinators.Z<Func<dynamic, Boolean>, Func<List, Boolean>>(r => f => l => l
            (h => t => TrueF<Boolean, Boolean>(
                LazyIf<Boolean>(f(h))
                    (AsLazy(_ => r(f)(t)))
                    (LazyFalse)
            ))
            (True));

    // Any := Z (λr.λf.λl. l (λh.λt. True (f h (True) (r f t)) False)
    public static Func<Func<dynamic, Boolean>, Func<List, Boolean>> Any =
        Combinators.Z<Func<dynamic, Boolean>, Func<List, Boolean>>(r => f => l => l
            (h => t => TrueF<Boolean, Boolean>(
                LazyIf<Boolean>(f(h))
                    (LazyTrue)
                    (AsLazy(_ => r(f)(t)))
            ))
            (False));

    // ElementAt := λn.λl. Head (Skip n l)
    public static Func<List, dynamic> ElementAt(Numeral n) => l => Head(Skip(n)(l));

    // IndexOf := λf. (Z (λr.λn.λl. l (λh.λt. True (f h n (r (Succ n) t))) Zero)) One
    public static Func<List, Numeral> IndexOf(Func<dynamic, Boolean> f) =>
        Combinators.Z<Numeral, Func<List, Numeral>>(r => n => l => l
            (h => t => TrueF<Numeral, Numeral>(
                LazyIf<Numeral>(f(h))
                    (AsLazy(_ => n))
                    (AsLazy(_ => r(Succ(n))(t)))
            ))
            (Zero)
        )
        (One);

    //// LastIndexOf := RFold (λa.λh. a (λn.λt. True (Cons (Succ n) Nil)) (f h (Cons Zero Nil) Nil)) Nil
    //public static Func<List, List> LastIndexOf(Func<dynamic, dynamic> f) => l =>
    //    RFold(a => h => ((List)a)
    //        (n => t => True(
    //            Cons(Succ(n))(Nil)
    //        ))
    //        (LazyIf<List>(f(h))
    //            (new Func<List>(() => Cons(Zero)(Nil)))
    //            (LazyNil)
    //    ))(Nil)(l);

    // LastIndexOf := λf. (Z (λr.λn.λl. l (λh.λt. True ((λi IsZero i (f(h) n Zero) i) (r (Succ n) t))) Zero)) One
    public static Func<List, Numeral> LastIndexOf(Func<dynamic, dynamic> f) =>
        Combinators.Z<Numeral, Func<List, Numeral>>(r => n => l => l
            (h => t => TrueF<Numeral, Numeral>(
                new Func<Numeral, Numeral>(i =>
                    LazyIf<Numeral>(IsZero(i))
                        (_ => f(h)(n)(Zero))
                        (_ => i)
                )
                (r(Succ(n))(t))
            ))
            (Zero)
        )
        (One);

    // Range := λf.λz. (Z (λr.λs.λn. IsZero n Nil (Cons (s f z) (r (Succ s) (Pred n))))) Zero
    public static Func<dynamic, Func<Numeral, List>> Range(NextValue f) => z =>
        Combinators.Z<Numeral, Func<Numeral, List>>(r => s => n =>
            LazyIf<List>(IsZero(n))
                (LazyNil)
                (_ => Cons(s(f)(z))(r(Succ(s))(Pred(n))))
        )
        (Zero);

    // Repeat := λv. (Z (λr.λn. IsZero n Nil (Cons v (r (Pred n))))
    public static Func<Numeral, List> Repeat(dynamic v) =>
        Combinators.Z<Numeral, List>(r => n =>
            LazyIf<List>(IsZero(n))
                (LazyNil)
                (_ => Cons(v)(r(Pred(n))))
        );

    // Zip := (Z (λr.λl1.λl2. l1 (λh1.λt1. True ( l2 (λh2.λt2. True (Cons (Pair h1, h2) r(t1, t2)) Nil ) Nil)
    public static Func<List, Func<List, List>> Zip = Combinators.Z<List, Func<List, List>>(r => l1 => l2 =>
        l1(h1 => t1 => TrueF<List, List>(
            l2(h2 => t2 => TrueF<List, List>(
                Cons(Pair<dynamic, dynamic>(h1)(h2))(r(t1)(t2))
            ))
            (Nil)
        ))
        (Nil));

    #endregion

    #region Extensions

    public static List ToChurchList<T>(this IEnumerable<T> source)
    {
        List list = Nil;
        foreach (T item in source.Reverse())
            list = Cons(item!)(list);

        return list;
    }

    // Church ListPair to LinkedList<dynamic>
    public static IEnumerable<dynamic> UnChurch(this List l)
    {
        while (!IsNil(l).UnChurch())
        {
            yield return Head(l);
            l = Tail(l);
        }
    }

    #endregion
}
