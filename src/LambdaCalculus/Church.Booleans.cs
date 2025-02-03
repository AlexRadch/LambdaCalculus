using System.ComponentModel;
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Boolean := λt.λf. t|f
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate Func<dynamic, dynamic> Boolean(dynamic @true);

    #endregion

    #region Constants

    // True := λt.λf. t
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<F, T> TrueL<T, F>(T t) => f => t;
    public static readonly Boolean True = TrueL<dynamic, dynamic>;

    // False := λt.λf. f
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<F, F> FalseL<T, F>(T t) => f => f;
    public static readonly Boolean False = FalseL<dynamic, dynamic>;

    #endregion

    #region Operations

    // Not := λp.λt.λf. p f t := λp. p False True
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Boolean Not_ab(Boolean p) => a => b => p(b)(a);
    public static Boolean Not(Boolean p) => p(False)(True);


    // Or := λa.λb. a a b := λa.λb. a True b := λa. a True
    // OrR := λa.λb. b b a := λa.λb. b True a
    //public static Func<dynamic, Boolean> Or(Boolean a) => a(True);
    public static Func<dynamic, Boolean> Or(Boolean a) => b => a(True)(b);

    // And := λa.λb. a b a := λa.λb. a b False
    // AndR := λa.λb. b a b := λa.λb. b a False
    public static Func<dynamic, Boolean> And(Boolean a) => b => a(b)(False);


    // Xor := λa.λb. a (Not b) b
    public static Func<dynamic, Boolean> Xor(Boolean a) => b => a(Not(b))(b);

    // Nor := λa.λb. a False (Not b)
    public static Func<dynamic, Boolean> Nor(Boolean a) => b => a(False)(Not(b));

    //Nor := λa.λb. Not (Or a b) := λa.λb. And (Not a) (Not b) := λa.λb. (Not a) (Not b) False := λa.λb. a False (Not b)
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<dynamic, Boolean> Nor_not(Boolean a) => b => Not(Or(a)(b));

    // Nand := λa.λb. a (Not b) True := λa.λb. Not (And a b)
    public static Func<dynamic, Boolean> Nand(Boolean a) => b => a(Not(b))(True);

    // Xnor := λa.λb. a b Not(b)
    public static Func<dynamic, Boolean> Xnor(Boolean a) => b => a(b)(Not(b));

    #endregion

    #region Operators

    // If := λp.λt.λe. p t e := λp. p := Id
    public static Boolean If(Boolean p) => p;

    #endregion

    #region Conversations

    // System bool to Church Bool
    // AsChurch := b => b ? True : False
    public static Boolean AsChurch(this bool b) => b ? True : False;

    // Church Bool to system bool
    // Unchurch := λb. If b true false := b true false
    public static bool UnChurch(this Boolean b) => b(true)(false); // Bool works them self

    #endregion

    #region Lazy evaluation

    public static Func<T> AsLazy<T>(Func<T> value) => value;

    #endregion
}
