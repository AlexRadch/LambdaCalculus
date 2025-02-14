namespace LambdaCalculus;

public delegate TResult SelfApplicable<TResult>(SelfApplicable<TResult> sa);

public static partial class Combinators
{
    // SelfApplied := λf. f (f)
    public static TResult SelfApplied<TResult>(SelfApplicable<TResult> f) => f(f);


    // Omega := λ. ItSelf(ItSelf);
    public static TResult Omega<TResult>() => SelfApplied<TResult>(SelfApplied);
}
