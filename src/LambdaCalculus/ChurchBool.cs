namespace LambdaCalculus
{
    // Bool := λt.λf. t|f
    // Bool := True -> False -> True|False
    public delegate Func<dynamic, dynamic> Bool(dynamic @true);

    public static partial class ChurchBool
    {
        #region True False

        // True := λt.λf. t
        // True := True -> False -> True
        public static Func<dynamic, dynamic> True(dynamic @true) => @false => @true;
        public static Bool TrueV => True;
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        //public static dynamic True(dynamic @true, dynamic @false) => @true;

        // False := λt.λf. f
        // False := True -> False -> False
        public static Func<dynamic, dynamic> False(dynamic @true) => @false => @false;
        public static Bool FalseV => False;
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        //public static dynamic False(dynamic @true, dynamic @false) => @false;

        #endregion

        #region If

        // If := λp.λt.λe. p t e := λp. p
        // If := Bool -> Bool
        public static Bool If(this Bool predicate) => predicate;
        public static dynamic If(this Bool predicate, dynamic @then, dynamic @else) => predicate(@then)(@else);
        public static dynamic LazyIf(this Bool predicate, Func<dynamic> @then, Func<dynamic> @else) => predicate(@then)(@else)();

        #endregion

        #region Not

        // Not := λb. b False True
        // Not := Bool -> Bool

        public static Bool Not(this Bool @bool) => @bool(FalseV)(TrueV);
        // TrueV FalseV can be replaced to new Bool(False) new Bool(True)
        //public static Bool Not(this Bool @bool) => @bool(new Bool(False))(new Bool(True));

        #endregion

        #region Or

        // Or := λa.λb. a a b := λa.λb. a True b
        // Or := Bool -> Bool -> Bool
        public static Func<Bool, Bool> Or(this Bool a) => b => a(TrueV)(b);
        public static Bool Or(this Bool a, Bool b) => a(TrueV)(b);

        #endregion

        #region And

        // And := λa.λb. a b a := λa.λb. a b False
        // And := Bool -> Bool -> Bool
        public static Func<Bool, Bool> And(this Bool a) => b => a(b)(FalseV);
        public static Bool And(this Bool a, Bool b) => a(b)(FalseV);

        #endregion

        #region Xor

        // Xor := λa.λb. a (Not b) b
        // Xor := Bool -> Bool -> Bool
        public static Func<Bool, Bool> Xor(this Bool a) => b => a(Not(b))(b);
        public static Bool Xor(this Bool a, Bool b) => a(Not(b))(b);

        #endregion

        #region Nor

        // Nor := λa.λb. a False (Not b)
        // Nor := Bool -> Bool -> Bool
        public static Func<Bool, Bool> Nor(this Bool a) => b => a(FalseV)(Not(b));
        public static Bool Nor(this Bool a, Bool b) => a(FalseV)(Not(b));

        #endregion

        #region Nand

        // Nand := λa.λb. a (Not b) True
        // Nand := Bool -> Bool -> Bool
        public static Func<Bool, Bool> Nand(this Bool a) => b => a(Not(b))(TrueV);
        public static Bool Nand(this Bool a, Bool b) => a(Not(b))(TrueV);

        #endregion

        #region Eq

        // Eq := λa.λb. a b Not(b)
        // Eq := Bool -> Bool -> Bool
        public static Func<Bool, Bool> Eq(this Bool a) => b => a(b)(Not(b));
        public static Bool Eq(this Bool a, Bool b) => a(b)(Not(b));

        #endregion

        #region Conversation

        // System bool to church Bool
        public static Bool Church(this bool value) => value ? TrueV : FalseV;

        // Church Bool to system bool
        // Unchurch := λp. If(p) true false := p true false
        public static bool Unchurch(this Bool p) => p(true)(false); // predicate works them self
        //public static bool Unchurch(this Bool p) => p.If()(true)(false); // If should work also
        //public static bool Unchurch(this Bool p) => p.If(true, false); // If should work also

        //public static dynamic ToLazy<T>(Func<T> f) => f;

        #endregion
    }
}
