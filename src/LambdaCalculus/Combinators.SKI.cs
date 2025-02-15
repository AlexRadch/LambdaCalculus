namespace LambdaCalculus;

public static partial class Combinators
{
    // I := λx. x
    public static T I<T>(T x) => x;

    // K := λx.λy. x
    public static readonly Func<dynamic, Func<dynamic, dynamic>> K = x => y => x;

    // S := λx.λy.λz. x z (y z)
    public static readonly Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>> S = x => y => z => x(z)(y(z));

    #region Sk

    // SkI := SKK
    public static T SkI<T>(T x) => S(K)(K)(x!);

    // Skι := λx. xSK
    public static readonly Func<dynamic, dynamic> Skι = x => x(S)(K);

    #endregion
}
