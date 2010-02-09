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
}
