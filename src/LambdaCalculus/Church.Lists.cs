using System.ComponentModel;
using System.Diagnostics;
using static LambdaCalculus.Church;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // List := CPair<dynamic, List> := λf. (λh.λt. )
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic List(Boolean<dynamic, List> b);

    #endregion

    #region Constants

    // Nil := False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<dynamic, dynamic> NilF(dynamic h) => FalseF<dynamic, dynamic>(h);
    public static readonly List Nil = NilF;
    public static readonly Func<List> LazyNil = () => NilF;

    #endregion

    #region Constructors

    // Cons := Pair
    public static Func<dynamic, List> Cons(dynamic h) => t => b => b(h)(t);

    #endregion

    #region Properties

    // Head := First
    public static dynamic Head(List l) => l(h => t => h);

    // HeadOrNil := λl. l (λh.λt.λ_. h⁡) Nil
    public static dynamic HeadOrNil(List l) => l(h => t => new Func<List, dynamic>(_ => h))(Nil);

    // Tail := Second
    public static List Tail(List l) => l(h => t => t);

    // TailOrNil := λl. l (λh.λt.λ_. t⁡) Nil
    public static List TailOrNil(List l) => l(h => t => new Func<List, List>(_ => t))(Nil);

    // IsNil := λl. l (λh.λt.λ_. False⁡) True
    public static Boolean IsNil(List p) => p(h => t => new Func<dynamic, Boolean>(_ => False))(True);

    // Length := Fold (λa.λh. Succ a) Zero
    public static Numeral Length(List l) => Fold(a => h => Succ(a))(Zero)(l);

    #endregion

    #region Operations

    // Fold := λf. Z (λr.λa.λl. l (λh.λt.λ_. r (f a h) t) a)
    public static Func<dynamic, Func<List, dynamic>> Fold(Func<dynamic, Func<dynamic, dynamic>> f) =>
        Combinators.Z<dynamic, Func<List, dynamic>>(r => a => l => l
            (h => t => new Func<dynamic, dynamic>(_ => r(f(a)(h))(t)))(a));

    // RFold := λf.λa. Z (λr.λl. l (λh.λt.λ_. (f (r t) h)) a)
    public static Func<dynamic, Func<List, dynamic>> RFold(Func<dynamic, Func<dynamic, dynamic>> f) => a =>
        Combinators.Z<List, dynamic>(r => l => l
            (h => t => new Func<dynamic, dynamic>(_ => f(r(t))(h)))(a));

    //// FoldList := λf. Z (λr.λa.λl. l (λh.λt.λ_. r (f a l h t) t) a)
    //public static Func<dynamic, Func<List, dynamic>> FoldList(Func<dynamic, Func<List, Func<dynamic, Func<List, dynamic>>>> f) =>
    //    Combinators.Z<dynamic, Func<List, dynamic>>(r => a => l => l
    //        (h => t => new Func<dynamic, dynamic>(_ => r(f(a)(l)(h)(t))(t)))(a));

    //// RFoldList := λf.λa. Z (λr.λl. l (λh.λt.λ_. (f (r t) l h t)) a)
    //public static Func<dynamic, Func<List, dynamic>> RFoldList(Func<dynamic, Func<List, Func<dynamic, Func<List, dynamic>>>> f) => a =>
    //    Combinators.Z<List, dynamic>(r => l => l
    //        (h => t => new Func<dynamic, dynamic>(_ => f(r(t))(l)(h)(t)))(a));

    // Map := λf.λl. RFold (λa.λh. Cons (f h) a) Nil l := λf. RFold (λa.λh. Cons (f h) a) Nil // RFold(a => h => Cons(f(h))(a))(Nil)(l);
    // Map := λf. Z (λr.λlr. lr (λh.λt.λ_. Cons (f h) (r t)) Nil)
    public static Func<List, List> Map(Func<dynamic, dynamic> f) => Combinators.Z<List, List>(r => lr => 
        lr(h => t => new Func<List, List>(_ => 
            Cons(f(h))(r(t))
        ))
        (Nil));

    // Filter := λf.λl. RFold (λa.λh. f h (Cons h a) a) Nil l := λf. RFold (λa.λh. f h (Cons h a) a) Nil
    // RFold(a => h => LazyIf(f(h))(new Func<dynamic>(() => Cons(h)(a)))(new Func<dynamic>(() => a)))(Nil)(l);
    // Filter := λf. Z (λr.λlr. lr (λh.λt.λ_. f h (Cons h (r t)) (r t)) Nil)
    public static Func<List, List> Filter(Func<dynamic, dynamic> f) => Combinators.Z<List, List>(r => lr =>
        lr(h => t => new Func<List, List>(_ => LazyIf<List>(f(h))
            (new Func<List>(() => Cons(h)(r(t))))
            (new Func<List>(() => r(t)))
        ))
        (Nil));

    // Reverse := λl. Fold (λa.λh. Cons h a) Nil l := Fold (λa.λh. Cons h a) Nil
    public static List Reverse(List l) => Fold(a => h => Cons(h)(a))(Nil)(l);

    // Concat := λl1.λl2. RFold (λa.λh. Cons h a) l2 l1
    public static Func<List, List> Concat(List l1) => l2 => RFold(a => h => Cons(h)(a))(l2)(l1);

    // Append := λl.λv. Concat l (Cons v Nil)
    public static Func<dynamic, List> Append(List l) => t => Concat(l)(Cons(t)(Nil));

    // Skip := λn.λl. n TailOrNil l := λn. n TailOrNil
    // Skip := Z (λr.λn.λl. l (λh.λt.λ_. IsZero n l (r (Pred n) t)) Nil)
    public static Func<Numeral, Func<List, List>> Skip = Combinators.Z<Numeral, Func<List, List>>(r => n => l => l(h => t =>
        new Func<List, List>(_ =>
            LazyIf<List>(IsZero(n))
                (() => l)
                (() => r(Pred(n))(t))
        ))
        (Nil));

    // SkipLast := λn.λl. IsZero(n) l Second (Z         (λr.λlr. lr (λhr.λtr.λ_. r tr (λna.λla. IsZero(na) (Pair Zero (Cons h la)) (Pair (Pred na) Nil))) (Pair n Nil)) l)
    public static Func<List, List> SkipLast(Numeral n) => l => LazyIf(IsZero(n))(() => l)(() =>
        Second(Combinators.Z<List, CPair<Numeral, List>>(r => lr => lr(hr => tr =>
            new Func<CPair<Numeral, List>, CPair<Numeral, List>>(_ =>
                r(tr)(na => la => LazyIf<CPair<Numeral, List>>(IsZero(na))
                    (() => Pair<Numeral, List>(Zero)(Cons(hr)(la)))
                    (() => Pair<Numeral, List>(Pred(na))(Nil))
                )
            ))
            (Pair<Numeral, List>(n)(Nil))
        )
        (l)));

    // SkipWhile := λf. FoldList (λa.λlr.     λhr.λtr.    IsNil a (f hr Nil    lr) a) Nil
    // SkipWhile := λf. Z        (λr.λlr. lr (λhr.λtr.λ_.          f hr (r tr) lr)    Nil)
    public static Func<List, List> SkipWhile(Func<dynamic, Boolean> f) =>
        Combinators.Z<List, List>(r => lr => lr
            (hr => tr => new Func<List, List>(_ =>
                LazyIf<List>(f(hr))
                    (new Func<List>(() => r(tr)))
                    (new Func<List>(() => lr))
            ))
            (Nil));

    // Take := λn. FoldList (λa.λlr.λhr.λtr. a (λna.λla. )) (Pair n Nil)
    // Take := Z (λr.λn.λl. l (λh.λt.λ_. IsZero n Nil (Cons h (r (Pred n) t))) Nil)
    public static Func<Numeral, Func<List, List>> Take =
        Combinators.Z<Numeral, Func<List, List>>(r => n => l => l
            (h => t => new Func<List, List>(_ =>
                LazyIf(IsZero(n))
                    (LazyNil)
                    (() => Cons(h)(r(Pred(n))(t))
                )
            ))
            (Nil));

    // TakeLast := λn.λl1. Second(
    //     Z (λr.λl. l
    //         (λh.λt.λ_. r(t)
    //             (λn2.λt2. IsZero(n2) (Pair Zero t2) (Pair (Pred n2) (Cons h t2)))
    //         (Pair n Nil)
    //     )
    //     (l1)
    // )
    public static Func<List, List> TakeLast(Numeral n) => l1 =>
        Second(Combinators.Z<List, CPair<Numeral, List>>(r => l => l(h => t =>
            new Func<CPair<Numeral, List>, CPair<Numeral, List>>(_ =>
                r(t)(n2 => t2 =>
                    LazyIf(IsZero(n2))
                        (() => Pair<Numeral, List>(Zero)(t2))
                        (() => Pair<Numeral, List>(Pred(n2))(Cons(h)(t2)))
                )
            ))
            (Pair<Numeral, List>(n)(Nil))
        )
        (l1));

    // TakeWhile := λf. Z (λr.λl. l (λh.λt.λ_. f d (Cons h (r t)) Nil) Nil)
    public static Func<List, List> TakeWhile(Func<dynamic, Boolean> f) =>
        Combinators.Z<List, List>(r => l => l
            (h => t => new Func<List, List>(_ =>
                LazyIf(f(h))
                    (new Func<List>(() => Cons(h)(r(t))))
                    (LazyNil)
            ))
            (Nil));

    // All := Z (λr.λf.λl. l (λh.λt.λ_. f h (r f t) (False) True)
    public static Func<Func<dynamic, Boolean>, Func<List, Boolean>> All =
        Combinators.Z<Func<dynamic, Boolean>, Func<List, Boolean>>(r => f => l => l
            (h => t => new Func<Boolean, Boolean>(_ =>
                LazyIf(f(h))
                    (new Func<Boolean>(() => r(f)(t)))
                    (LazyFalse)
            ))
            (True));

    // Any := Z (λr.λf.λl. l (λh.λt.λ_. f h (True) (r f t) False)
    public static Func<Func<dynamic, Boolean>, Func<List, Boolean>> Any =
        Combinators.Z<Func<dynamic, Boolean>, Func<List, Boolean>>(r => f => l => l
            (h => t => new Func<Boolean, Boolean>(_ =>
                LazyIf(f(h))
                    (LazyTrue)
                    (new Func<Boolean>(() => r(f)(t)))
            ))
            (False));

    // ElementAt := λn.λl. Head (Skip n l)
    public static Func<List, dynamic> ElementAt(Numeral n) => l => Head(Skip(n)(l));

    // IndexOf := λf. (Z (λr.λn.λl. l (λh.λt.λ_. f h (Cons n Nil) (r (Succ n) t)) Nil)) Zero
    public static Func<List, List> IndexOf(Func<dynamic, Boolean> f) =>
        Combinators.Z<Numeral, Func<List, List>>(r => n => l => l
            (h => t => new Func<List, List>(_ =>
                LazyIf<List>(f(h))
                    (new Func<List>(() => Cons(n)(Nil)))
                    (new Func<List>(() => r(Succ(n))(t)))
            ))
            (Nil)
        )
        (Zero);

    //// LastIndexOf := RFold (λa.λh. a (λn.λt.λ_. Cons (Succ n) Nil) (f h (Cons Zero Nil) Nil)) Nil
    //public static Func<List, List> LastIndexOf(Func<dynamic, dynamic> f) => l =>
    //    RFold(a => h => ((List)a)
    //        (n => t => new Func<List, List>(_ =>
    //            Cons(Succ(n))(Nil)
    //        ))
    //        (LazyIf<List>(f(h))
    //            (new Func<List>(() => Cons(Zero)(Nil)))
    //            (LazyNil)
    //    )(Nil)(l);

    // LastIndexOf := λf. (Z (λr.λn.λl. l (λh.λt.λ_. (λi IsNil i (f(h) (Cons n Nil) Nil) i) (r (Succ n) t)) Nil)) Zero
    public static Func<List, List> LastIndexOf(Func<dynamic, dynamic> f) =>
        Combinators.Z<Numeral, Func<List, List>>(r => n => l => l
            (h => t => new Func<List, List>(_ =>
                new Func<List, List>(i =>
                    LazyIf(IsNil(i))
                        (() => f(h)(Cons(n)(Nil))(Nil))
                        (() => i)
                )
                (r(Succ(n))(t))
            ))
            (Nil)
        )
        (Zero);

    // Range := λf.λz. (Z (λr.λs.λn. IsZero n Nil (Cons (s f z) (r (Succ s) (Pred n))))) Zero
    public static Func<dynamic, Func<Numeral, List>> Range(NextValue f) => z =>
        Combinators.Z<Numeral, Func<Numeral, List>>(r => s => n =>
            LazyIf(IsZero(n))
                (LazyNil)
                (() => Cons(s(f)(z))(r(Succ(s))(Pred(n))))
        )
        (Zero);

    // Repeat := λv. (Z (λr.λn. IsZero n Nil (Cons v (r (Pred n))))
    public static Func<Numeral, List> Repeat(dynamic v) =>
        Combinators.Z<Numeral, List>(r => n =>
            LazyIf(IsZero(n))
                (LazyNil)
                (() => Cons(v)(r(Pred(n))))
        );

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
    public static IEnumerable<dynamic> UnChurch(this List p)
    {
        while (!IsNil(p).UnChurch())
        {
            yield return Head(p);
            p = Tail(p);
        }
    }

    #endregion
}
