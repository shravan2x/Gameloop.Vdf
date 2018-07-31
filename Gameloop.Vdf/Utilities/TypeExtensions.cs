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
using System.Collections.Generic;
using System.Reflection;

namespace Gameloop.Vdf.Utilities
{
    internal static class TypeExtensions
    {
#if DOTNET || PORTABLE
#if !DOTNET
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
#endif
#endif

    public static bool IsGenericType(this Type type)
    {
#if HAVE_FULL_REFLECTION
        return type.IsGenericType;
#else
        return type.GetTypeInfo().IsGenericType;
#endif
    }

#if (DOTNET || PORTABLE)
        public static MethodInfo GetBaseDefinition(this MethodInfo method)
        {
            return method.GetRuntimeBaseDefinition();
        }
#endif

#if (DOTNET || PORTABLE)
#if !DOTNET
        public static MethodInfo GetMethod(this Type type, string name)
        {
            return type.GetMethod(name, DefaultFlags);
        }

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetTypeInfo().GetDeclaredMethod(name);
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags bindingFlags)
        {
            return type.GetTypeInfo().DeclaredMethods;
        }
#endif
#endif

        public static Type BaseType(this Type type)
        {
#if HAVE_FULL_REFLECTION
            return type.BaseType;
#else
            return type.GetTypeInfo().BaseType;
#endif
        }

        public static bool IsVisible(this Type type)
        {
#if HAVE_FULL_REFLECTION
            return type.IsVisible;
#else
            return type.GetTypeInfo().IsVisible;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if HAVE_FULL_REFLECTION
            return type.IsValueType;
#else
            return type.GetTypeInfo().IsValueType;
#endif
        }
    }
}