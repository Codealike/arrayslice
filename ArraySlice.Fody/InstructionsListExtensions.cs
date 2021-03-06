﻿using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corvalius.ArraySlice.Fody
{
    public static class InstructionListExtensions
    {
        public static void Prepend(this Collection<Instruction> collection, params Instruction[] instructions)
        {
            for (var index = 0; index < instructions.Length; index++)
            {
                var instruction = instructions[index];
                collection.Insert(index, instruction);
            }
        }

        public static void Prepend(this ILProcessor processor, params Instruction[] instructions)
        {
            processor.Body.Instructions.Prepend(instructions);            
        }

        public static void Append(this Collection<Instruction> collection, params Instruction[] instructions)
        {          
            for (var index = 0; index < instructions.Length; index++)
            {
                collection.Insert(index, instructions[index]);
            }
        }

        public static void BeforeLast(this Collection<Instruction> collection, params Instruction[] instructions)
        {
            var index = collection.Count - 1;
            foreach (var instruction in instructions)
            {
                collection.Insert(index, instruction);
                index++;
            }
        }
        public static void BeforeLast(this ILProcessor processor, params Instruction[] instructions)
        {
            processor.Body.Instructions.BeforeLast(instructions);   
        }

        public static int Insert(this Collection<Instruction> collection, int index, params Instruction[] instructions)
        {
            foreach (var instruction in instructions.Reverse())
            {
                collection.Insert(index, instruction);
            }
            return index + instructions.Length;
        }

        public static void InsertAfter(this ILProcessor processor, Instruction src, params Instruction[] instructions)
        {
            foreach (var instruction in instructions.Reverse())
                processor.InsertAfter(src, instruction);
        }

        public static void InsertBefore(this ILProcessor processor, Instruction src, params Instruction[] instructions)
        {
            foreach (var instruction in instructions.Reverse())
                processor.InsertBefore(src, instruction);
        }
    }
}
