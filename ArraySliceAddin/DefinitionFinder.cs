using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ArraySliceAddin.Fody
{
    public static class DefinitionFinder
    {
        public static PropertyDefinition FindProperty<T>(Expression<Func<T>> expression)
        {
            var memberExpression = ((MemberExpression)expression.Body);
            var declaringType = memberExpression.Member.DeclaringType;
            return FindProperty(declaringType, memberExpression.Member.Name);
        }

        public static PropertyDefinition FindProperty<T>(string name)
        {
            var typeDefinition = FindType(typeof(T));
            return typeDefinition.Properties.First(x => x.Name == name);
        }

        public static FieldDefinition FindField<T>(Expression<Func<T>> expression)
        {
            var memberExpression = ((MemberExpression)expression.Body);
            var declaringType = memberExpression.Member.DeclaringType;
            return FindField(declaringType, memberExpression.Member.Name);
        }

        public static FieldDefinition FindField<T>(string name)
        {
            var typeDefinition = FindType(typeof(T));
            return typeDefinition.Fields.First(x => x.Name == name);
        }

        static PropertyDefinition FindProperty(Type declaringType, string name)
        {
            var typeDefinition = FindType(declaringType);
            return typeDefinition.Properties.First(x => x.Name == name);
        }

        static FieldDefinition FindField ( Type declaringType, string name)
        {
            var typeDefinition = FindType(declaringType);
            return typeDefinition.Fields.First(x => x.Name == name);
        }

        public static MethodDefinition FindMethod<T>(Expression<Action> expression)
        {
            var callExpression = ((MethodCallExpression)expression.Body);
            var declaringType = callExpression.Method.DeclaringType;

            var typeDefinition = FindType(declaringType);

            return typeDefinition.Methods.First(x => x.Name == callExpression.Method.Name);
        }

        public static TypeDefinition FindType<T>()
        {
            var declaringType = typeof(T);

            return FindType(declaringType);
        }

        public static TypeDefinition FindType(Type typeToFind)
        {
            var moduleDefinition = ModuleDefinition.ReadModule(typeToFind.Assembly.Location);
            foreach (var type in moduleDefinition.Types)
            {
                if (type.Name == typeToFind.Name)
                {
                    return type;
                }
                foreach (var nestedType in type.NestedTypes)
                {
                    if (nestedType.Name == typeToFind.Name && nestedType.DeclaringType.Name == typeToFind.DeclaringType.Name)
                    {
                        return nestedType;
                    }
                }
            }
            throw new Exception();
        }
    }
}
