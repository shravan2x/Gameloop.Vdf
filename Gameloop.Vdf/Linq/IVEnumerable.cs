using System.Collections.Generic;

namespace Gameloop.Vdf.Linq
{
    public interface IVEnumerable<
#if HAVE_VARIANT_TYPE_PARAMETERS
        out
#endif
        T> : IEnumerable<T> where T : VToken
    {
        IVEnumerable<VToken> this[object key] { get; }
    }
}
