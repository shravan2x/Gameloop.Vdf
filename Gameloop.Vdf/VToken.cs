using Gameloop.Vdf.Utilities;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;

namespace Gameloop.Vdf
{
    public abstract class VToken : IDynamicMetaObjectProvider
    {
        public VToken Parent { get; set; }
        public VToken Previous { get; set; }
        public VToken Next { get; set; }

        public abstract void WriteTo(VdfWriter writer);

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
