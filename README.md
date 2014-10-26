Array Slices is a NuGet Package (https://www.nuget.org/packages/ArraySlice.Fody/) that uses Fody (https://github.com/Fody/Fody) to optimize the use of the ArraySlice to be on par performance wise with the standard managed arrays when dealing with high-performance numerical code.

With array slices you can build multiple views from the same array and you dont have to modify your algorithms to account for the moving offset inside the backing array. In essence, it is an enhanced version of System.ArraySegment<T> (http://referencesource.microsoft.com/#mscorlib/system/arraysegment.cs) but your algorithms wont need to know they are dealing with a data segment's offset explicitely. 

There are other similar alternatives, like using indexers (http://weblogs.asp.net/wim/ArraySegment-Structure---what-were-they-thinking_3F00_), but in the end the indexers performance hit will make it worthwhile to avoid those implementations for high-performance code. 

With System.ArraySlice<T> you dont pay the performance cost imposed by the indexers while keeping the simplicity they provide to handle multiple views inside a shared backing array. 

### How does Array Slices works?

Wherever you use an T[] you can switch it to an ArraySlice<T>. Why? Because the constructor would even take your array and pack it inside the ArraySlice structure. 

Lets say you have this code:

        public void Assign(float[] getParameter, float[] setParameter)
        {
            for (int i = 0; i < getParameter.Count; i++)
                setParameter[i] = getParameter[i];
        }

Now you can just go and change it to:

        public void Assign(ArraySlice<float> getParameter, ArraySlice<float> setParameter)
        {
            for (int i = 0; i < getParameter.Count; i++)
                setParameter[i] = getParameter[i];
        }

All the call sites will convert their array types into ArraySlice<T> and work as expected.

#### Performance


Lets say that we do micro-benchmark (even though microbenchmarks have lots of issues). 

We will use an standard backing array of 1000000 floats and do 1000 tries of 100000 elements each starting at offset 49421 (to avoid optimizations at 0) and run those in release mode from the command propmt.

An standard array use would look like this:

                const int offset = 49421;
                const int endPlace = offset + 100000;

                float t = 0;
                for (int tries = 0; tries < 1000; tries++)
                {
                    for (int i = offset; i < endPlace; i++)
                    {
                        t = data[i];
                    }
                }

Access it though an ArraySegment<float> would look like this:

                for (int tries = 0; tries < 1000; tries++)
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        t = segment.Array[segment.Offset + i];
                    }
                }
                
You can still optimize this creating local variables for the array and offset and get performance comparable with the standard array use at the cost of code readability.

We can do the same with the DelimitedArray implementation shown in the article.

                var delimited = new DelimitedArray<float>(data, offset, 100000);

                for (int tries = 0; tries < 1000; tries++)
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        t = delimited[i];
                    }
                }
                
And we can go even further and optimize the hell out of the indexer in the following way (look maaa no checks):

        public T this[int index]
        {            
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get
            {
                return this._array[this._offset + index];
            }
        }

####Results

- Access via delimited array: 258ms.
- Access via array segment: 68ms.
- Access via inline no checks delimited array: 45ms.
- Access without offset: 38ms.
- Access via array slice: 38ms.

The performance of the microbenchmark tells the history. 

Delimited arrays are the least performant method. Mainly because of the indexer and the checks. Closing the gap is the use of the optimized inline no checks delimited array, however that implementation is still slower and for high performance (typically numerical code) the 30% difference is still too much.

Historically the preferred method for high performance code is accessing the naked array working with offsets which is the fastest way available. But, working with the offsets can become a problem pretty fast. 

With Array Slices we achieve comparable performance, without having to deal with offsets. We achieve that rewriting the IL to achieve the fastest implementation of indexers around. Truth be told, we actually get rid of the indexers altoghether but that is another story.

#### Known issues

Array Slices will not optimize slices that are unsafe. For example, slices that are stored in fields. The reason is that they can change in multithreaded environments and therefore the inlining process will create very difficult to diagnose problems. In the future we will let you override the inlining process with an Unsafe version you can use to achieve this.

For now, as a workaround, to allow optimization you just need to copy the reference to a method variable and use it from there.

Array Slices current optimization heuristic is also a bit too aggresize to optimize, and select single accesses (non loops) methods, causing some simple code to execute a couple extra instructions. Those are the type of methods you wouldnt even consider use an ArraySegment anyways.  

#### Contributions

This project accepts contributions. We will always look for better inlining analysis and smarter code to achieve the fastest implementation available. Also let us know if your project use Array Slices
