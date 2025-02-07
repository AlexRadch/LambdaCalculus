
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Pair := λb. First|Second
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic Pair(Func<dynamic, Func<dynamic, dynamic>> b);

    #endregion

    #region Operations

    // CreatePair := λf.λs.λb. b f s
    public static Func<dynamic, Pair> CreatePair(dynamic f) => s => b => b(f)(s);

    // First := λp. p (λf.λs. f) := λp. p True
    public static dynamic First(Pair p) => p(f => s => f);

    // Second := λp. p (λf.λs. s) := λp. p False
    public static dynamic Second(Pair p) => p(f => s => s);

    #endregion

    #region Extensions

    // Church Pair to (dynamic, dynamic) value tupple
    // UnChurch := λp. (λf.λs. (f, s))
    public static (dynamic First, dynamic Second) UnChurch(this Pair p) => p(f => s => (f, s));

    #endregion
}
