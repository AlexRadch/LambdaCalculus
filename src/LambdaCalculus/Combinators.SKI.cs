using System.Diagnostics;

namespace LambdaCalculus;

public static partial class SKI
{
    #region S K I

    // I := λx. x
    public static Func<dynamic, dynamic> I = x => x;
    public static T Id<T>(T x) => x;

    // K := λx.λy. x
    public static readonly Func<dynamic, Func<dynamic, dynamic>> K = x => y => x;

    // S := λx.λy.λz. x z (y z)
    public static readonly Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>> S = x => y => z => x(z)(y(z));

    #endregion

    #region Combinators

    // SkI := SKK
    public static readonly Func<dynamic, dynamic> SkI = S(K)(K);
    public static T SkId<T>(T x) => S(K)(K)(x!);

    // Skι := λx. xSK
    public static readonly Func<dynamic, dynamic> Skι = x => x(S)(K);

    #endregion

    #region Boolean

    // Boolean := λt.λf. t|f
    [DebuggerDisplay("{LambdaCalculus.SKI.UnSki(this)}")]
    public delegate Func<dynamic, dynamic> Boolean(dynamic @true);

    // T := K
    public static readonly Func<dynamic, Func<dynamic, dynamic>> T = K;
    public static readonly Boolean TB = @true => K(@true);

    // F := SK
    public static readonly Func<dynamic, Func<dynamic, dynamic>> F = S(K);
    public static readonly Boolean FB = @true => S(K)(@true);

    // Not := S(SI(KF))(KT) := λb. λb (SK) K
    public static readonly Func<dynamic, dynamic> Not = S(S(I)(K(F)))(K(T));
    public static readonly Func<Boolean, Boolean> NotB = a => S(S(I)(K(FB)))(K(TB))(a);

    // Or := SI(KT) := λx.λy. xTy
    public static readonly Func<dynamic, dynamic> Or = S(I)(K(T));
    public static readonly Func<Boolean, Func<Boolean, Boolean>> OrB = a => b => S(I)(K(TB))(a)(b);

    // And := SS(K(KF)) := λx.λy. xyF
    public static readonly Func<dynamic, dynamic> And = S(S)(K(K(F)));
    public static readonly Func<Boolean, Func<Boolean, Boolean>> AndB = a => b => S(S)(K(K(FB)))(a)(b);

    #endregion

    #region Convertions

    public static Boolean ToSki(this bool b) => b ? TB : FB;

    public static bool UnSki(this Boolean b) => b(K(true))(K(false))(K);

    public static bool UnSkiBoolean(dynamic b) => b(K(true))(K(false))(K);

    #endregion
}