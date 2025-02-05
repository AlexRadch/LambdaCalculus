
using System.ComponentModel;

namespace LambdaCalculus;

public delegate TResult SelfApplicableFunc<TResult>(SelfApplicableFunc<TResult> self);

public static partial class Combinators

{

    // Y := λf. (λx.f (x x)) (λx.f (x x))
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Func<T, TResult> Y<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
        ((SelfApplicableFunc<Func<T, TResult>>)(x => f(x(x))))(x => f(x(x)));

    // Z1 := λf. (λx.λv. f (x x) v) (λx.λv. f (x x) v)
    public static Func<T, TResult> Z1<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
        ((SelfApplicableFunc<Func<T, TResult>>)(x => v => f(x(x))(v)))(x => v => f(x(x))(v));


    // Z2 := λf. (λx. f(λv. x x v)) (λx. f (λv. x x v))
    public static Func<T, TResult> Z2<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f) =>
        ((SelfApplicableFunc<Func<T, TResult>>)(x => f(v => x(x)(v))))(x => f(v => x(x)(v)));
}
