namespace LambdaCalculus;

public static partial class Combinators
{
    // ι := λf. f S K := λf. f (λa.λb.λc. a c (b c)) (λd.λe. d)
    public static readonly Func<dynamic, dynamic> ι = f => 
        f((Func<dynamic, Func<dynamic, Func<dynamic, dynamic>>>)(a => b => c => a(c)(b(c))))
        ((Func<dynamic, Func<dynamic, dynamic>>)(d => e => d));

    #region SKI

    // IotaI = (ιι)
    public static readonly Func<dynamic, dynamic> IotaI = ι(ι);

    // IotaK = (ι(ι(ιι)))
    public static readonly Func<dynamic, dynamic> IotaK = ι(ι(ι(ι)));

    // IotaS = (ι(ι(ι(ιι))))
    public static readonly Func<dynamic, dynamic> IotaS = ι(ι(ι(ι(ι))));

    #endregion
}
