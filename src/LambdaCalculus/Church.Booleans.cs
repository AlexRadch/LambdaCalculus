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
    public static Func<F, T> TrueF<T, F>(T t) => f => t!;
    public static readonly Boolean True = TrueF<dynamic, dynamic>;
    public static readonly Func<Boolean, Boolean> LazyTrue = TrueF<Boolean, Boolean>(True);

    // False := λt.λf. f
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<F, F> FalseF<T, F>(T t) => f => f!;
    public static readonly Boolean False = FalseF<dynamic, dynamic>;
    public static readonly Func<Boolean, Boolean> LazyFalse = TrueF<Boolean, Boolean>(False);

    #endregion

    #region Operations

    // Not := λp.λt.λf. p f t := λp. p False True
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Boolean Not_ab(Boolean p) => a => b => p(b)(a);
    public static Boolean Not(Boolean p) => p(False)(True);
    public static Func<Boolean, Boolean> LazyNot(Func<Boolean, Boolean> p) => _ => Not(p(True));


    // Or := λa.λb. a a b := λa.λb. a True b := λa. a True
    // OrR := λa.λb. b b a := λa.λb. b True a
    //public static Func<dynamic, Boolean> Or(Boolean a) => a(True);
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Or(Boolean a) => b => a(True)(b);
    public static Func<Func<Boolean, Boolean>, Func<Boolean, Boolean>> LazyOr(Boolean a) => b => a(LazyTrue)(b);

    // And := λa.λb. a b a := λa.λb. a b False
    // AndR := λa.λb. b a b := λa.λb. b a False
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> And(Boolean a) => b => a(b)(False);
    public static Func<Func<Boolean, Boolean>, Func<Boolean, Boolean>> LazyAnd(Boolean a) => b => a(b)(LazyFalse);

    // Xor := λa.λb. a (Not b) b
    public static Func<Boolean, Boolean> Xor(Boolean a) => b => a(Not(b))(b);

    // Nor := λa.λb. a False (Not b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nor(Boolean a) => b => a(False)(Not(b));
    public static Func<Func<Boolean, Boolean>, Func<Boolean, Boolean>> LazyNor(Boolean a) => b => a(LazyFalse)(LazyNot(b));

    //Nor := λa.λb. Not (Or a b) := λa.λb. And (Not a) (Not b) := λa.λb. (Not a) (Not b) False := λa.λb. a False (Not b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nor_not(Boolean a) => b => Not(Or(a)(b));

    // Nand := λa.λb. a (Not b) True := λa.λb. Not (And a b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<Boolean, Boolean> Nand(Boolean a) => b => a(Not(b))(True);
    public static Func<Func<Boolean, Boolean>, Func<Boolean, Boolean>> LazyNand(Boolean a) => b => a(LazyNot(b))(LazyTrue);

    // Xnor := λa.λb. a b Not(b)
    public static Func<Boolean, Boolean> Xnor(Boolean a) => b => a(b)(Not(b));

    #endregion

    #region Operators

    // If := λp.λt.λe. p t e := λp. p := Id
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Boolean If(Boolean p) => p;
    public static Func<Func<Boolean, dynamic>, Func<Func<Boolean, dynamic>, dynamic>> LazyIf(Boolean p) => @then => @else => p(@then)(@else)(True);
    //public static Func<Boolean<dynamic, Boolean>, Func<Boolean<dynamic, Boolean>, dynamic>> LazyIf(Boolean p) => @then => @else => p(@then)(@else)(True);
    public static Func<Func<Boolean, T>, Func<Func<Boolean, T>, T>> LazyIf<T>(Boolean p) => @then => @else => p(@then)(@else)(True);
    //public static Func<Boolean<T, Boolean>, Func<Boolean<T, Boolean>, T>> LazyIf<T>(Boolean p) => @then => @else => p(@then)(@else)(True);

    #endregion

    #region Extensions

    // AsLazy := Id
    public static Func<Boolean, T> AsLazy<T>(Func<Boolean, T> value) => value;

    // System bool to Boolean
    // ToChurch := b => b ? True : False
    public static Boolean ToChurch(this bool b) => b ? True : False;

    // Church Boolean to system bool
    // UnChurch := λb. If b true false := b true false
    public static bool UnChurch(this Boolean b) => b(true)(false); // Boolean works them self

    // Church lazy Boolean to system bool
    // UnChurch := λb. UnChurch b()
    public static bool UnChurch(this Func<Boolean, Boolean> b) => b(True).UnChurch();

    #endregion
}
