using System;

namespace AssemblyToProcess
{
    public class CoreFunctionality : ArraySliceContainerBase
    {
        private ArraySlice<float> FieldSegment;
        private ArraySlice<float> AnotherFieldSegment;

        public CoreFunctionality()
        {
            int offset = 10;
            var data = InitializeData();

            int offset2 = 10;
            var data2 = InitializeData();

            FieldSegment = new ArraySlice<float>(data, offset, 5);
            AnotherFieldSegment = new ArraySlice<float>(data2, offset2, 5);
        }

        public float SingleAccessGetter()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            float result = segment[4];

            return result;
        }

        public float[] SingleAccessSetter()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            segment[4] = 999;

            return segment.Array;
        }

        public T[] GenericGetter<T>()
        {
            int offset = 10;
            var data = InitializeData<T>();

            var segment = new ArraySlice<T>(data, offset, 5);
            T result = segment[4];

            return segment.Array;
        }

        public T[] GenericSetter<T>()
        {
            int offset = 10;
            var data = InitializeData<T>();

            var segment = new ArraySlice<T>(data, offset, 5);
            segment[4] = default(T);

            return segment.Array;
        }

        public float[] InnerLoop()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            

            float t;
            for (int i = 0; i < 4; i++)
            {
                segment[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segment[j];
            }

            return segment.Array;
        }

        public void AccessAsParameter(ArraySlice<float> segment)
        {
            float t;
            for (int i = 0; i < 4; i++)
            {
                segment[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segment[j];
            }
        }

        public void PassthroughParameter(ArraySlice<float> segment)
        {
            AccessAsParameter(segment);
        }

        public void PassthroughParameterWithInnerUse(ArraySlice<float> segment)
        {
            AccessAsParameter(segment);

            int offset = 10;
            var data = InitializeData<float>();
            var x = new ArraySlice<float>(data, offset, 5);

            float t;
            for (int i = 0; i < 4; i++)
            {
                x[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = x[j];
            }
        }

        public void CreationButNotUsing()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            AccessAsParameter(segment);
        }

        public float ManyVariablesDefinitionForShortAccess()
        {
            int a, b, c, d, e, f, g, h, i;
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            float result = segment[4];

            return result;
        }

        public void Aliasing()
        {
            var segment = new ArraySlice<float>(InitializeData(), 10, 5);
            var segmentAlias = segment;

            float t;
            for (int i = 0; i < 4; i++)
            {
                segmentAlias[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segmentAlias[j];
            }

            for (int i = 0; i < 4; i++)
            {
                segment[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segment[j];
            }
        }

        public void MultipleParameters(ArraySlice<float> getParameter, ArraySlice<float> setParameter)
        {
            for (int i = 0; i < 4; i++)
                setParameter[i] = getParameter[i];
        }

        public void TryImplicitCast ()
        {
            var getData = InitializeData<float>();
            var setData = InitializeData<float>();
            MultipleParameters(getData, setData);
        }

        public void UseFields()
        {
            var segmentAlias = FieldSegment;

            float t;
            for (int i = 0; i < 4; i++)
            {
                segmentAlias[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segmentAlias[j];
            }
        }

        public void FieldAliasing ()
        {
            var segmentAlias = FieldSegment;

            float t;
            for (int i = 0; i < 4; i++)
            {
                segmentAlias[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segmentAlias[j];
            }
        }

        public void PassthroughFieldWithInnerUse()
        {
            var segmentAlias = FieldSegment;

            AccessAsParameter(FieldSegment);

            float t;
            for (int i = 0; i < 4; i++)
            {
                segmentAlias[i] = 2;
                for (int j = 0; j < 4; j++)
                    t = segmentAlias[j];
            }
        }

        public void MultipleFields()
        {
            var segmentAlias = FieldSegment;
            var anotherSegmentAlias = AnotherFieldSegment;

            for (int i = 0; i < 4; i++)
                anotherSegmentAlias[i] = segmentAlias[i];
        }
    }
}
