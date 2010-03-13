using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestGlobalClass
{
}

public struct TestGlobalStruct
{
}

public delegate void TestGlobalDelegate();

public class TestGenericGlobalClass<T>
{
}

public struct TestGenericGlobalStruct<T>
{
}

public delegate void TestGenericGlobalDelegate<T>(T arg);

namespace Test
{
    public delegate void TestNamespaceDelegate();

    public class TestNamespaceClass
    {
        public delegate void TestNestedDelegate();

        public event Action TestEvent;

        public int TestProperty { get; set; }

        public int TestField;

        public void TestMethod() { }

        public static void TestStaticMethod() { }

        public class TestNestedClass
        {
        }

        public struct TestNestedStruct
        {
        }
    }

    namespace TestNestedNamespace
    {
        public delegate void TestNamespaceDelegate();

        public class TestNamespaceClass
        {
            public delegate void TestNestedDelegate();

            public event Action TestEvent;

            public int TestProperty { get; set; }

            public int TestField;

            public void TestMethod() { }

            public static void TestStaticMethod() { }

            public class TestNestedClass
            {
            }

            public struct TestNestedStruct
            {
            }
        }
    }

    public delegate void TestGenericNamespaceDelegate<T>();

    public class TestGenericNamespaceClass<T>
    {
        public delegate void TestGenericNestedDelegate<R>();

        public event Action TestEvent;

        public int TestProperty { get; set; }

        public int TestField;

        public void TestGenericMethod<R>() { }

        public static void TestGenericStaticMethod<R>() { }

        public class TestGenericNestedClass<R>
        {
        }

        public struct TestGenericNestedStruct<R>
        {
        }
    }

    namespace TestNestedNamespace
    {
        public delegate void TestGenericNamespaceDelegate<T>();

        public class TestGenericNamespaceClass<T>
        {
            public delegate void TestGenericNestedDelegate<R>();

            public event Action TestEvent;

            public int TestProperty { get; set; }

            public int TestField;

            public void TestGenericMethod<R>() { }

            public static void TestGenericStaticMethod<R>() { }

            public class TestGenericNestedClass<R>
            {
            }

            public struct TestGenericNestedStruct<R>
            {
            }
        }
    }
}
