namespace LambdaCalculus;

public delegate TResult SelfApplicable<TResult>(SelfApplicable<TResult> self);

public static partial class Combinators

{
    // ItSelf := λf. f (f)
    public static TResult ItSelf<TResult>(SelfApplicable<TResult> f) => f(f);


    // Omega := ItSelf(ItSelf);
    public static TResult Omega<TResult>() => ItSelf<TResult>(ItSelf);
}
