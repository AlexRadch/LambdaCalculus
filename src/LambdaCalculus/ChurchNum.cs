namespace LambdaCalculus
{
    // Num := λf.λz. n times f x (0 := x, 1 := f x, 2 := f(f x), n := f(f...(f x)..)
    // Num := (Succ -> Zerro) -> Zerro -> Result
    public delegate Func<dynamic, dynamic> Num(Func<dynamic, dynamic> Succ);

    public static partial class ChurchNum
    {
        // Zerro := λf.λz. z // the same as False := λt.λf. f
        // Zerro := (Succ -> Zerro) -> Zerro -> Zerro
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => zero => zero;

        //public static Func<dynamic, dynamic> Zero(Func<dynamic, dynamic> succ) => False(succ); // should work also!!!

        // One := λf.λz. f z
        // One := (Succ -> Zerro) -> Zerro -> Succ(Zerro)
        public static Func<dynamic, dynamic> One(Func<dynamic, dynamic> succ) => zero => succ(zero);

        // Two := λf.λz. f(f z) := f(One f z)) := One(One(f z))
        // Two := (Succ -> Zerro) -> Zerro -> Succ(Succ(Zerro)) := 
        public static Func<dynamic, dynamic> Two(Func<dynamic, dynamic> succ) => zero => One(One(succ)(zero));

        // Succ := λf.λn. f n // the same as One
        // Succ := (Succ -> Num) -> Num -> Succ(Num)
        public static Func<Num, Num> Succ(Func<Num, Num> succ) => num => succ(num);
    }
}
