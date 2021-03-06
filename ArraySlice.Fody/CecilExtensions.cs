﻿using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corvalius.ArraySlice.Fody    
{
    public static class CecilExtensions
    {
        public static bool IsCall(this OpCode opCode)
        {
            return opCode == OpCodes.Call ||
                   opCode == OpCodes.Callvirt ||
                   opCode == OpCodes.Calli ||
                   opCode == OpCodes.Ldftn;
        }

        public static bool IsConstructor( this OpCode opCode )
        {
            return opCode == OpCodes.Newobj;
        }

        public static MethodDefinition FindMethod(this TypeDefinition typeReference, string name, params string[] paramTypes)
        {
            foreach (var method in typeReference.Methods)
            {
                if (method.IsMatch(name, paramTypes))
                {
                    return method;
                }
            }
            throw new WeavingException(string.Format("Could not find '{0}' on '{1}'", name, typeReference.Name));
        }

        public static FieldDefinition FindField(this TypeDefinition typeReference, string name, params string[] paramTypes)
        {
            foreach (var field in typeReference.Fields)
            {
                if (field.IsMatch(name, paramTypes))
                {
                    return field;
                }
            }
            throw new WeavingException(string.Format("Could not find '{0}' on '{1}'", name, typeReference.Name));
        }

        public static bool IsMatch(this MethodReference methodReference, string name, params string[] paramTypes)
        {
            if (methodReference.Parameters.Count != paramTypes.Length)
            {
                return false;
            }
            if (methodReference.Name != name)
            {
                return false;
            }
            for (var index = 0; index < methodReference.Parameters.Count; index++)
            {
                var parameterDefinition = methodReference.Parameters[index];
                var paramType = paramTypes[index];
                if (parameterDefinition.ParameterType.Name != paramType)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsMatch(this FieldReference field, string name, params string[] paramTypes)
        {
            if (field.Name != name)
                return false;

            return true;
        }

        public static MethodReference MakeHostInstanceGeneric(this MethodReference self, params TypeReference[] args)
        {
            var reference = new MethodReference(
                self.Name,
                self.ReturnType,
                self.DeclaringType.MakeGenericInstanceType(args))
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention
            };

            foreach (var parameter in self.Parameters)
            {
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));
            }

            foreach (var genericParam in self.GenericParameters)
            {
                reference.GenericParameters.Add(new GenericParameter(genericParam.Name, reference));
            }

            return reference;
        }

        public static FieldReference MakeHostInstanceGeneric(this FieldReference self, params TypeReference[] args)
        {
            var reference = new FieldReference(
                self.Name,
                self.FieldType,
                self.DeclaringType.MakeGenericInstanceType(args))
            {};

            return reference;
        }

        public static bool ContainsBehaviorAttribute(this ICustomAttributeProvider definition, out CustomAttribute attribute)
        {
            attribute = null;

            var customAttributes = definition.CustomAttributes;

            var query = customAttributes.Where(x => x.AttributeType.Name.StartsWith("ArraySlice"));
            if (!query.Any())
                return false;

            attribute = query.First();
            return attribute != null;
        }

        public static void RemoveAllArraySliceAttributes(this ICustomAttributeProvider definition)
        {
            var customAttributes = definition.CustomAttributes;

            var attributes = customAttributes.Where(x => x.AttributeType.Name.StartsWith("ArraySlice")).ToArray();

            foreach (var attribute in attributes)
            {
                customAttributes.Remove(attribute);
            }
        }
    }
}
