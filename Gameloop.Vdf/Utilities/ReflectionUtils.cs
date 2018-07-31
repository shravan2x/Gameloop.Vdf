#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq;
using System.Reflection;

namespace Gameloop.Vdf.Utilities
{
#if PORTABLE
    [Flags]
    internal enum BindingFlags
    {
        Default = 0,
        IgnoreCase = 1,
        DeclaredOnly = 2,
        Instance = 4,
        Static = 8,
        Public = 16,
        NonPublic = 32,
        FlattenHierarchy = 64,
        InvokeMethod = 256,
        CreateInstance = 512,
        GetField = 1024,
        SetField = 2048,
        GetProperty = 4096,
        SetProperty = 8192,
        PutDispProperty = 16384,
        ExactBinding = 65536,
        PutRefDispProperty = 32768,
        SuppressChangeType = 131072,
        OptionalParamBinding = 262144,
        IgnoreReturn = 16777216
    }
#endif

    internal static class ReflectionUtils
    {
        public static bool IsNullable(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, nameof(t));

            if (t.IsValueType())
                return IsNullableType(t);

            return true;
        }

        public static bool IsNullableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, nameof(t));

            return (t.IsGenericType() && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
        {
            bool isMethodOverriden = currentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(info =>
                    info.Name == method &&
                    // check that the method overrides the original on DynamicObjectProxy
                    info.DeclaringType != methodDeclaringType
                    && info.GetBaseDefinition().DeclaringType == methodDeclaringType
                );

            return isMethodOverriden;
        }
    }
}
