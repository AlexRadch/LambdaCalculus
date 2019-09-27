using System;

namespace LambdaCalculus
{
    // Bool := λt.λf. some logic
    public delegate Func<dynamic, dynamic> Bool(dynamic @true);

    public static partial class ChurchBool
    {
        #region False

        // False := λt.λf.f
        public static Func<dynamic, dynamic> False(dynamic @true) => (dynamic @false) => @false;

        // False := λt.λf.f
        public static Func<dynamic, dynamic> FalseLazy1(Func<dynamic> @true) => (@false) => @false;

        // FalseV is sugar for new Bool(False). Workaround for compiler error CS0173.
        public static readonly Bool FalseV = False;

        // FalseV is sugar for new Bool(FalseLazyT). Workaround for compiler error CS0173.
        public static readonly Func<Func<dynamic>, Func<dynamic, dynamic>> FalseLazy1V = FalseLazy1;

        //// False := λt.λf.f
        //public static Func<B, B> FalseT<B>(Func<B> @true) => (B @false) => @false;

        //// False := λt.λf.f
        //public static Func<F, F> FalseT<T, F>(Func<T> @true) => (F @false) => @false;

        #endregion

        #region True

        // True := λt.λf.t
        public static Func<dynamic, dynamic> True(dynamic @true) => (dynamic @false) => @true;

        // True := λt.λf.t
        public static Func<Func<dynamic, dynamic>, dynamic> TrueLazy2(dynamic @true) => (Func<dynamic, dynamic> @false) => @true;

        // TrueV is sugar for new Bool(TrueV). Workaround for compiler error CS0173.
        public static readonly Bool TrueV = True;

        // TrueV is sugar for new Bool(TrueLazyF). Workaround for compiler error CS0173.
        public static readonly Func<dynamic, Func<Func<dynamic, dynamic>, dynamic>> TrueLazy2V = TrueLazy2;

        //// True := λt.λf.t
        //public static Func<Func<B>, B> TrueT<B>(B @true) => (Func<B> @false) => @true;

        //// True := λt.λf.t
        //public static Func<Func<F>, T> TrueT<T, F>(T @true) => (Func<F> @false) => @true;

        #endregion

        #region If

        // If := λp.λt.λe.p t e := λp.p
        public static Bool If(this Bool p) => p;

        // If := λp.λt.λe.p t e := λp.p
        public static Func<Func<dynamic>, Func<Func<dynamic>, dynamic>>  IfLazy(this Bool p) => 
            (Func<dynamic> @then) => (Func<dynamic>  @else) => p (@then) (@else) ();

        // If := λp.λt.λe.p t e := λp.p
        //
        // Unfortunately, extension methods do not work for dynamic values when method parameters are dynamic already.
        // See compiler error CS1973
        // See https://github.com/dotnet/roslyn/issues/34716 issue.
        public static dynamic If(this Bool p, dynamic @then, dynamic @else) => p (@then) (@else);

        // If := λp.λt.λe.p t e := λp.p
        //
        // Unfortunately, extension methods do not work for dynamic values when method parameters are dynamic already.
        // See compiler error CS1973
        // See https://github.com/dotnet/roslyn/issues/34716 issue.
        public static dynamic IfLazy(this Bool p, Func<dynamic> @then, Func<dynamic> @else) => p (@then) (@else) ();

        #endregion

        #region Not

        // Not := λb.b False True
        public static Bool Not(this Bool b) => b (FalseV) (TrueV);

        #endregion

        #region Or

        // Or := λa.λb.a a b := λa.λb.a True b
        public static Func<Bool, Bool> Or(this Bool a) => (Bool b) => a (TrueV) (b);

        // Or := λa.λb.a a b := λa.λb.a True b
        public static Func<Func<Bool>, Bool> OrLazy2(this Bool a) => (Func<Bool> b) => a (new Func<dynamic>(() => TrueV)) (b) ();

        // Or := λa.λb.a a b := λa.λb.a True b
        public static Bool Or(this Bool a, Bool b) => a (TrueV) (b);

        // Or := λa.λb.a a b := λa.λb.a True b
        public static Bool OrLazy2(this Bool a, Func<Bool> b) => a (new Func<dynamic>(() => TrueV)) (b) ();

        #endregion

        #region And

        // And := λa.λb.a b a := λa.λb.a b False
        public static Func<Bool, Bool> And(this Bool a) => (Bool b) => a (b) (FalseV);

        // And := λa.λb.a b a := λa.λb.a b False
        public static Bool And(this Bool a, Bool b) => a (b) (FalseV);

        #endregion

        #region Xor

        // Xor := λa.λb.a (Not b) b
        public static Func<Bool, Bool> Xor(this Bool a) => (Bool b) => a (Not (b)) (b);

        // Xor := λa.λb.a (Not b) b
        public static Bool Xor(this Bool a, Bool b) => a (Not (b)) (b);

        #endregion

        #region Nor

        // Nor := λa.λb.a False (Not b)
        public static Func<Bool, Bool> Nor(this Bool a) => (Bool b) => a (FalseV) (Not (b));

        // Nor := λa.λb.a False (Not b)
        public static Func<Func<Bool>, Bool> NorLazy2(this Bool a) => (Func<Bool> b) => 
            a (new Func<dynamic>(() => FalseV)) (new Func<dynamic>(() => Not(b()))) ();

        // Nor := λa.λb.a False (Not b)
        public static Bool Nor(this Bool a, Bool b) => a (FalseV) (Not (b));

        // Nor := λa.λb.a False (Not b)
        public static Bool NorLazy2(this Bool a, Func<Bool> b) =>
            a (new Func<dynamic>(() => FalseV)) (new Func<dynamic>(() => Not(b()))) ();

        #endregion

        #region Nand

        // Nand := λa.λb.a (Not b) True
        public static Func<Bool, Bool> Nand(this Bool a) => (Bool b) => a (Not (b)) (TrueV);

        // Nand := λa.λb.a (Not b) True
        public static Bool Nand(this Bool a, Bool b) => a (Not (b)) (TrueV);

        #endregion

        #region Eq

        // Eq := λa.λb.a b Not(b)
        public static Func<Bool, Bool> Eq(this Bool a) => (Bool b) => a (b) (Not (b));

        // Eq := λa.λb.a b Not(b)
        public static Bool Eq(this Bool a, Bool b) => a (b) (Not (b));

        #endregion

        #region Conversation

        // System bool to church Bool
        public static Bool Church(this bool value) => value ? TrueV : FalseV;

        // Church Bool to system bool
        // Unchurch := λp.If(p) true false := p true false
        public static bool Unchurch(this Bool p) => p (true) (false);

        #endregion
    }
}
