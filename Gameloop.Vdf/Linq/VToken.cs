using Gameloop.Vdf.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Gameloop.Vdf.Linq
{
    public abstract class VToken : IVEnumerable<VToken>, IDynamicMetaObjectProvider
    {
        // TODO: Implement these.
        public VToken? Parent { get; internal set; }
        public VToken? Previous { get; internal set; }
        public VToken? Next { get; internal set; }

        public abstract void WriteTo(VdfWriter writer);

        public abstract VTokenType Type { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<VToken>) this).GetEnumerator();
        }

        IEnumerator<VToken> IEnumerable<VToken>.GetEnumerator()
        {
            return Children().GetEnumerator();
        }

        IVEnumerable<VToken> IVEnumerable<VToken>.this[object key] => this[key]!;

        public static bool DeepEquals(VToken? t1, VToken? t2)
        {
            return (t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2)));
        }

        public abstract VToken DeepClone();

        public virtual VToken? this[object key]
        {
            get => throw new InvalidOperationException($"Cannot access child value on {GetType()}.");
            set => throw new InvalidOperationException($"Cannot set child value on {GetType()}.");
        }

        public virtual T Value<T>(object key)
        {
            VToken? token = this[key];
            return (token == null ? default : Extensions.Convert<VToken, T>(token));
        }

        public virtual IEnumerable<VToken> Children()
        {
            return Enumerable.Empty<VToken>();
        }

        public IEnumerable<T> Children<T>() where T : VToken
        {
            return Children().OfType<T>();
        }

        protected abstract bool DeepEquals(VToken node);

        protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicProxyMetaObject<VToken>(parameter, this, new DynamicProxy<VToken>());
        }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return GetMetaObject(parameter);
        }

        public override string ToString()
        {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                VdfTextWriter vdfTextWriter = new VdfTextWriter(stringWriter);
                WriteTo(vdfTextWriter);

                return stringWriter.ToString();
            }
        }
    }
}
