namespace LambdaCalculus
{
    // Bool(true, false) := λt.λf. t|f
    // True -> False -> True|False
    public delegate Func<T, T> Bool<T>(T @true);

    public static partial class ChurchBool
    {
        #region True False

        // True := λt.λf. t
        // True := True -> False -> True
        public static Func<T, T> True<T>(T @true) => @false => @true;
        //public static Bool<T> TrueV => True;

        // False := λt.λf. f
        // False := True -> False -> False
        public static Func<T, T> False<T>(T @true) => @false => @false;
        //public static Bool<T> FalseV => False;

        #endregion

        #region If

        // If := λp.λt.λe. p t e := λp. p
        // If := Bool -> Bool
        public static Bool<T> If<T>(this Bool<T> predicate) => predicate;
        public static T If<T>(this Bool<T> predicate, T @then, T @else) => predicate(@then)(@else);
        public static T LazyIf<T>(this Bool<Func<T>> predicate, Func<T> @then, Func<T> @else) => predicate(@then)(@else)();

        #endregion

        #region Not

        // Not := λb. b False True
        // Not := Bool -> Bool

        public static Bool<T> Not<T>(this Bool<Bool<T>> @bool) => @bool(new Bool<T>(False))(new Bool<T>(True));

        #endregion

        #region Or

        // Or := λa.λb. a a b := λa.λb. a True b
        // Or := Bool -> Bool -> Bool
        public static Func<Bool<T>, Bool<T>> Or<T>(this Bool<Bool<T>> a) => b => a(new Bool<T>(True))(b);
        public static Bool<T> Or<T>(this Bool<Bool<T>> a, Bool<T> b) => a(new Bool<T>(True))(b);

        #endregion

        #region And

        // And := λa.λb. a b a := λa.λb. a b False
        // And := Bool -> Bool -> Bool
        public static Func<Bool<T>, Bool<T>> And<T>(this Bool<Bool<T>> a) => b => a(b)(new Bool<T>(False));
        public static Bool<T> And<T>(this Bool<Bool<T>> a, Bool<T> b) => a(b)(new Bool<T>(False));

        #endregion

        #region Xor

        // Xor := λa.λb. a (Not b) b
        // Xor := Bool -> Bool -> Bool
        public static Func<Bool<Bool<T>>, Bool<T>> Xor<T>(this Bool<Bool<T>> a) => b => a(Not(b))(b(new Bool<T>(True))(new Bool<T>(False)));
        public static Bool<T> Xor<T>(this Bool<Bool<T>> a, Bool<Bool<T>> b) => a(Not(b))(b(new Bool<T>(True))(new Bool<T>(False)));

        #endregion

        #region Nor

        // Nor := λa.λb. a False (Not b)
        // Nor := Bool -> Bool -> Bool
        public static Func<Bool<Bool<T>>, Bool<T>> Nor<T>(this Bool<Bool<T>> a) => b => a(new Bool<T>(False))(Not(b));
        public static Bool<T> Nor<T>(this Bool<Bool<T>> a, Bool<Bool<T>> b) => a(new Bool<T>(False))(Not(b));

        #endregion

        #region Nand

        // Nand := λa.λb. a (Not b) True
        // Nand := Bool -> Bool -> Bool
        public static Func<Bool<Bool<T>>, Bool<T>> Nand<T>(this Bool<Bool<T>> a) => b => a(Not(b))(new Bool<T>(True));
        public static Bool<T> Nand<T>(this Bool<Bool<T>> a, Bool<Bool<T>> b) => a(Not(b))(new Bool<T>(True));

        #endregion

        #region Eq

        // Eq := λa.λb. a b Not(b)
        // Eq := Bool -> Bool -> Bool
        //public static Func<Bool<T>, Bool<T>> Eq<T>(this Bool<T> a) => b => a(b)(Not(b));
        public static Bool<T> Eq<T>(this Bool<Bool<T>> a, Bool<Bool<T>> b) => a(b(True)(False))(Not(b));

        #endregion

        #region Conversation

        // System bool to church Bool
        public static Bool<T> Church<T>(this bool value) => value ? True : False;

        // Church Bool to system bool
        // Unchurch := λp. If(p) true false := p true false
        public static T Unchurch<T>(this Bool<T> p, T @true, T @false) => p(@true)(@false); // predicate works them self
        public static bool Unchurch(this Bool<bool> p) => p(true)(false); // predicate works them self
        //public static bool Unchurch(this Bool<bool> p) => p.If()(true)(false); // If should work also
        //public static bool Unchurch(this Bool<bool> p) => p.If(true, false); // If should work also

        #endregion
    }
}
