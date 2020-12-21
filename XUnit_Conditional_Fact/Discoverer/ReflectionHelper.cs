using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Xunit.Sdk;

namespace XUnit_Conditional_Fact
{
    internal class ReflectionHelper : IReflectionHelper
    {
        public T CreateInstance<T>(Type type, object[] args)
        {
            return (T)Activator.CreateInstance(type,args);
        }

        public ICustomAttributeDataWrapper CustomAttributeDataRepresentingFactAttribute(Type discovererFactAttributeType, object[] discovererFactAttributeCtorArgs)
        {
            return new CustomAttributeDataWrapper( 
                DynamicallyCreateMethodWithDiscoveryAttribute(
                    discovererFactAttributeType,
                    discovererFactAttributeCtorArgs
                ).GetCustomAttributesData()[0]
            );
        }
        private MethodInfo DynamicallyCreateMethodWithDiscoveryAttribute(Type wrappedDiscovererFactAttributeType, object[] wrappedDiscovererFactAttributeCtorArgs)
        {
            AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
#if NET472

            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            TypeBuilder tb = mb.DefineType(
                "MyDynamicType",
                 TypeAttributes.Public);


#else
            var ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder mb =
                ab.DefineDynamicModule(aName.Name + ".dll");
            TypeBuilder tb = mb.DefineType(
                "MyDynamicType",
                 TypeAttributes.Public);
#endif


            var methodName = "Method";
            var methodBuilder = tb.DefineMethod(methodName, MethodAttributes.Public, typeof(void), new Type[] { });
            //todo consider optional arguments
            var customAttributeBuilder = new CustomAttributeBuilder(wrappedDiscovererFactAttributeType.GetConstructor(wrappedDiscovererFactAttributeCtorArgs.Select(a => a.GetType()).ToArray()), wrappedDiscovererFactAttributeCtorArgs);
            methodBuilder.SetCustomAttribute(customAttributeBuilder);
            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            var dt = tb.CreateType();
            var method = tb.GetMethod(methodName);
            return method;
        }

        public Type GetType(string typeName, string assemblyName)
        {
            return Assembly.Load(assemblyName).GetType(typeName);
        }

        public (string typeName, string assemblyName) GetDiscovererTypeNameAndAssemblyName(Type factAttributeType)
        {
            var customAttributeDatas = CustomAttributeData.GetCustomAttributes(factAttributeType);
            var discovererAttributeData = customAttributeDatas.First(customAttributeData => customAttributeData.AttributeType == typeof(XunitTestCaseDiscovererAttribute));
            var ctorArguments = discovererAttributeData.ConstructorArguments;
            var typeName = ctorArguments[0].Value as string;
            var assemblyName = ctorArguments[1].Value as string;
            return (typeName, assemblyName);

        }
    }
    
}
