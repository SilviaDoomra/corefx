// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace System.Reflection.Emit.Tests
{
    public class EvBMyAttribute1 : Attribute
    {
    }

    public class EventBuilderSetCustomAttribute1
    {
        public delegate void TestEventHandler(object sender, object arg);

        private TypeBuilder TypeBuilder
        {
            get
            {
                if (null == _typeBuilder)
                {
                    AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName("EventBuilderAddOtherMethod_Assembly"), AssemblyBuilderAccess.Run);
                    ModuleBuilder module = TestLibrary.Utilities.GetModuleBuilder(assembly, "EventBuilderAddOtherMethod_Module");
                    _typeBuilder = module.DefineType("EventBuilderAddOtherMethod_Type", TypeAttributes.Abstract);
                }

                return _typeBuilder;
            }
        }

        private TypeBuilder _typeBuilder;

        private const int ArraySize = 256;

        [Fact]
        public void PosTest1()
        {
            EventBuilder ev = TypeBuilder.DefineEvent("Event_PosTest1", EventAttributes.None, typeof(TestEventHandler));
            Type type = typeof(EvBMyAttribute1);
            ConstructorInfo con = type.GetConstructor(new Type[] { });
            byte[] bytes = new byte[ArraySize];
            TestLibrary.Generator.GetBytes(bytes);

            ev.SetCustomAttribute(con, bytes);
        }

        [Fact]
        public void NegTest1()
        {
            EventBuilder ev = TypeBuilder.DefineEvent("Event_NegTest1", EventAttributes.None, typeof(TestEventHandler));
            byte[] bytes = new byte[ArraySize];
            Assert.Throws<ArgumentNullException>(() => { ev.SetCustomAttribute(null, bytes); });
        }

        [Fact]
        public void NegTest2()
        {
            EventBuilder ev = TypeBuilder.DefineEvent("Event_NegTest1", EventAttributes.None, typeof(TestEventHandler));
            Type type = typeof(EvBMyAttribute1);
            ConstructorInfo con = type.GetConstructor(new Type[] { });
            Assert.Throws<ArgumentNullException>(() => { ev.SetCustomAttribute(con, null); });
        }

        [Fact]
        public void NegTest3()
        {
            try
            {
                EventBuilder ev = TypeBuilder.DefineEvent("Event_NegTest3", EventAttributes.None, typeof(TestEventHandler));
                Type type = typeof(EvBMyAttribute1);
                ConstructorInfo con = type.GetConstructor(new Type[] { });
                byte[] bytes = new byte[ArraySize];
                TestLibrary.Generator.GetBytes(bytes);
                TypeBuilder.CreateTypeInfo().AsType();

                Assert.Throws<InvalidOperationException>(() => { ev.SetCustomAttribute(con, bytes); });
            }
            finally
            {
                _typeBuilder = null;
            }
        }
    }
}
