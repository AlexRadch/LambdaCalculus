using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Boolean := λt.λf. t|f
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate Func<dynamic, dynamic> Boolean(dynamic @true);
    public delegate Func<F, dynamic> Boolean<in T, in F>(T @true);

    #endregion

    #region Constants

    // True := λt.λf. t
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<F, dynamic> TrueF<T, F>(T t) => f => t!;
    public static readonly Boolean True = TrueF<dynamic, dynamic>;
    public static readonly Func<Boolean> LazyTrue = () => True;

    // False := λt.λf. f
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<F, dynamic> FalseF<T, F>(T t) => f => f!;
    public static readonly Boolean False = FalseF<dynamic, dynamic>;
    public static readonly Func<Boolean> LazyFalse = () => False;

    #endregion

    #region Operations

    // Not := λp.λt.λf. p f t := λp. p False True
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Boolean Not_ab(Boolean p) => a => b => p(b)(a);
    public static Boolean Not(Boolean p) => p(False)(True);
    public static Func<Boolean> Not(Func<Boolean> p) => () => Not(p());


    // Or := λa.λb. a a b := λa.λb. a True b := λa. a True
    // OrR := λa.λb. b b a := λa.λb. b True a
    //public static Func<dynamic, Boolean> Or(Boolean a) => a(True);
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Or(Boolean a) => b => a(True)(b);
    public static Func<Func<Boolean>, Func<Boolean>> LazyOr(Boolean a) => b => a(LazyTrue)(b);
    //public static Func<Func<Boolean>, Func<Boolean>> Or(Func<Boolean> a) => b => () => LazyOr(a())(b)();

    // And := λa.λb. a b a := λa.λb. a b False
    // AndR := λa.λb. b a b := λa.λb. b a False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> And(Boolean a) => b => a(b)(False);
    public static Func<Func<Boolean>, Func<Boolean>> LazyAnd(Boolean a) => b => a(b)(LazyFalse);
    //public static Func<Func<Boolean>, Func<Boolean>> And(Func<Boolean> a) => LazyAnd(a());

    // Xor := λa.λb. a (Not b) b
    public static Func<Boolean, Boolean> Xor(Boolean a) => b => a(Not(b))(b);

    // Nor := λa.λb. a False (Not b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nor(Boolean a) => b => a(False)(Not(b));
    public static Func<Func<Boolean>, Func<Boolean>> LazyNor(Boolean a) => b => a(LazyFalse)(Not(b));
    //public static Func<Func<Boolean>, Func<Boolean>> Nor(Func<Boolean> a) => b => () => LazyNor(a())(b)();

    //Nor := λa.λb. Not (Or a b) := λa.λb. And (Not a) (Not b) := λa.λb. (Not a) (Not b) False := λa.λb. a False (Not b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nor_not(Boolean a) => b => Not(Or(a)(b));

    // Nand := λa.λb. a (Not b) True := λa.λb. Not (And a b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nand(Boolean a) => b => a(Not(b))(True);
    public static Func<Func<Boolean>, Func<Boolean>> LazyNand(Boolean a) => b => a(Not(b))(LazyTrue);
    //public static Func<Func<Boolean>, Func<Boolean>> Nand(Func<Boolean> a) => LazyNand(a());

    // Xnor := λa.λb. a b Not(b)
    public static Func<Boolean, Boolean> Xnor(Boolean a) => b => a(b)(Not(b));

    #endregion

    #region Operators

    // If := λp.λt.λe. p t e := λp. p := Id
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Boolean If(Boolean p) => p;
    public static Func<Func<dynamic>, Func<Func<dynamic>, dynamic>> LazyIf(Boolean p) => @then => @else => p(@then)(@else)();
    //public static Func<Boolean<dynamic, Boolean>, Func<Boolean<dynamic, Boolean>, dynamic>> LazyIf(Boolean p) => @then => @else => p(@then)(@else)(True);
    public static Func<Func<T>, Func<Func<T>, T>> LazyIf<T>(Boolean p) => @then => @else => p(@then)(@else)();
    //public static Func<Boolean<T, Boolean>, Func<Boolean<T, Boolean>, T>> LazyIf<T>(Boolean p) => @then => @else => p(@then)(@else)(True);

    #endregion

    #region Extensions

    // AsLazy := Id
    public static Func<T> AsLazy<T>(Func<T> value) => value;

    // System bool to Boolean
    // ToChurch := b => b ? True : False
    public static Boolean ToChurch(this bool b) => b ? True : False;

    // Church Boolean to system bool
    // UnChurch := λb. If b true false := b true false
    public static bool UnChurch(this Boolean b) => b(true)(false); // Boolean works them self

    // Church lazy Boolean to system bool
    // UnChurch := λb. UnChurch b()
    public static bool UnChurch(this Func<Boolean> b) => b().UnChurch();

    #endregion
}
