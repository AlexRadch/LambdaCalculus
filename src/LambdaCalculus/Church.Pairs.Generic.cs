
using System.Diagnostics;

namespace LambdaCalculus;

public static partial class Church
{
    #region Delegates

    // Pair := λb. First|Second
    [DebuggerDisplay("{LambdaCalculus.Church.UnChurch(this)}")]
    public delegate dynamic Pair<F, S>(Func<F, Func<S, dynamic>> b);

    #endregion

    #region Operations

    // CreatePair := λf.λs.λb. b f s
    public static Func<dynamic, Pair<F, S>> CreatePair<F,S>(dynamic f) => s => b => b(f)(s);

    // First := λp. p (λx.λy. x) := λp. p True
    public static F First<F, S>(Pair<F, S> p) => p(f => s => f!);

    // Second := λp. p (λx.λy. y) := λp. p False
    public static S Second<F, S>(Pair<F, S> p) => p(f => s => s!);

    #endregion

    #region Extensions

    // Church Pair to (F, S) value tupple
    // UnChurch := λb. If b true false := b true false
    public static (F First, S Second) UnChurch<F, S>(this Pair<F, S> p) => p(f => s => (f, s));

    #endregion
}
