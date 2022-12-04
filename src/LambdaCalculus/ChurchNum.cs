namespace LambdaCalculus
{
    // Num := λf.λz. n times f x (0 := x, 1 := f x, 2 := f(f x), n := f(f...(f x)..)
    // Num := (Prev -> Next) -> Start -> Result : n times f x
    public delegate Func<dynamic, dynamic> Num(Func<dynamic, dynamic> Succ);

    public static partial class ChurchNum
    {
        #region Zero One Two

        // Zerro := λf.λz. z // the same as False := λt.λf. f
        // Zerro := (Succ -> Zerro) -> Zerro -> Zerro
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => zero => zero;
        //public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => ChurchBool.False; // should work also!!!
        //public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => ChurchBool.FalseV(succ); // should work also!!!

        public static Num ZeroV => Zero;

        //// One(f, z) := λf.λz. f z
        //// One := (Succ -> Zerro) -> Zerro -> Succ(Zerro)
        //public static Func<dynamic, dynamic> One(Func<dynamic, dynamic> succ) => zerro => succ(zerro);

        //public static Num OneV => One;

        //// Two := λf.λz. f (f z)
        //// Two := (Succ -> Zerro) -> Zerro -> Succ(Succ(Zerro)) := 
        //public static Func<dynamic, dynamic> Two(Func<dynamic, dynamic> succ) => zero => succ(succ(zero));

        //public static Num TwoV => Two;

        #endregion

        // Add := λm.λn.λf.λz. m f (n f z)
        public static Func<Num, Num> Add(this Num m) => n => f => z => m(f)(n(f)(z));
        public static Num Add(this Num m, Num n) => f => z => m(f)(n(f)(z));

        // Succ := λn.λf.λz. f (n f z)
        public static Num Succ(this Num n) => f => z => f(n(f)(z));

        // Mult = m ∗ n \operatorname{mult}(m, n) = m*n
    }
}
