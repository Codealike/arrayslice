using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArraySliceAddin.Fody.Gendarme
{
    /// <summary>
    /// Provide an easy way to get TypeReference to primitive types without having
    /// direct access to the mscorlib.dll assembly (any ModuleDefinition will do).
    /// </summary>
    public static class PrimitiveReferences
    {

        // To avoid memory allocations this is done in three stages
        // 1. the references are cached at the first call and reused afterward (very cheap)
        // 2. we look if the module already have the TypeReference (cheap)
        // 3. at last we go thru the full Import (costly)

        // TODO - extend to all primivites
        static TypeReference single_ref;
        static TypeReference double_ref;

        static TypeReference GetReference(Type type, IMetadataTokenProvider metadata)
        {
            ModuleDefinition module = metadata.GetAssembly().MainModule;
            TypeReference tr;
            if (!module.TryGetTypeReference(type.FullName, out tr))
                tr = module.Import(type);
            return tr;
        }

        static public TypeReference GetDouble(IMetadataTokenProvider metadata)
        {
            if (double_ref == null)
                double_ref = GetReference(typeof(double), metadata);
            return double_ref;
        }

        static public TypeReference GetSingle(IMetadataTokenProvider metadata)
        {
            if (single_ref == null)
                single_ref = GetReference(typeof(float), metadata);
            return single_ref;
        }
    }
}
