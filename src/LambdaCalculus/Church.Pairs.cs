
namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Pair := λb. First|Second
    public delegate dynamic Pair(Boolean b);

    #endregion

    #region Operations

    // CreatePair := λf.λs.λb. b f s
    public static Func<dynamic, Pair> CreatePair(dynamic f) => s => b => b(f)(s);

    // First := λp. p (λx.λy. x) := λp. p True
    public static dynamic First(Pair p) => p(True);

    // Second := λp. p (λx.λy. y) := λp. p False
    public static dynamic Second(Pair p) => p(False);

    #endregion

    #region Extensions

    // Church Pair to (dynamic, dynamic) value tupple
    // UnChurch := λb. If b true false := b true false
    public static (dynamic First, dynamic Second) UnChurch(this Pair p) => p(f => s => (f, s));

    // Church Pair to (T1, T2) value tupple
    // UnChurch := λb. If b true false := b true false
    public static (T1 First, T2 Second) UnChurch<T1, T2>(this Pair p) => p(f => s => ((T1)f, (T2)s));

    #endregion
}
