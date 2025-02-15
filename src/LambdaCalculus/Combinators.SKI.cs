namespace LambdaCalculus;

public static partial class Combinators
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

    #region Iota

    // Skι := λx. xSK
    public static readonly Func<dynamic, dynamic> Skι = x => x(S)(K);

    #endregion

    #region Sk

    // SkI := SKK
    public static T SkId<T>(T x) => S(K)(K)(x!);
    public static readonly Func<dynamic, dynamic> SkI = S(K)(K);

    // SkTrue := K
    public static readonly Func<dynamic, dynamic> SkTrue = K;

    // SkFalse := SK
    public static readonly Func<dynamic, dynamic> SkFalse = S(K);

    // SkNot := S(SI(KF))(KT) := λb. λb (SK) K
    public static readonly Func<dynamic, dynamic> SkNot = S(S(I)(K(SkFalse)))(K(SkTrue));

    // SkOr := SI(KT) := λx.λy. xTy
    public static readonly Func<dynamic, dynamic> SkOr = S(I)(K(SkTrue));

    // SkAnd := SS(K(KF)) := λx.λy. xyF
    public static readonly Func<dynamic, dynamic> SkAnd = S(S)(K(K(SkFalse)));

    #endregion

    #region Convertions

    public static Func<dynamic, dynamic> ToSki(this bool b) => b ? SkTrue : SkFalse;

    public static bool UnSkiBoolean(Func<dynamic, dynamic> b) => b(K(true))(K(false))(K);

    #endregion
}
