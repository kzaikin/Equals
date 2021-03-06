﻿using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Equals.Fody.Extensions
{
    public static class CollectionInstructionExtensions
    {
        public static void If(this Collection<Instruction> ins,
            Action<Collection<Instruction>> condition,
            Action<Collection<Instruction>> thenStatment,
            Action<Collection<Instruction>> elseStetment)
        {
            var ifEnd = Instruction.Create(OpCodes.Nop);
            var ifElse = Instruction.Create(OpCodes.Nop);

            condition(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Bne_Un, ifElse);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brfalse, ifElse));
            }

            thenStatment(ins);

            ins.Add(Instruction.Create(OpCodes.Br, ifEnd));
            ins.Add(ifElse);

            elseStetment(ins);

            ins.Add(ifEnd);
        }

        public static void IfAnd(this Collection<Instruction> ins,
            Action<Collection<Instruction>> condition1,
            Action<Collection<Instruction>> condition2,
            Action<Collection<Instruction>> thenStatment,
            Action<Collection<Instruction>> elseStetment)
        {
            var ifEnd = Instruction.Create(OpCodes.Nop);
            var ifElse = Instruction.Create(OpCodes.Nop);

            condition1(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Bne_Un, ifElse);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brfalse, ifElse));
            }

            condition2(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Bne_Un, ifElse);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brfalse, ifElse));
            }

            thenStatment(ins);

            ins.Add(Instruction.Create(OpCodes.Br, ifEnd));
            ins.Add(ifElse);

            elseStetment(ins);

            ins.Add(ifEnd);
        }

        public static void If(this Collection<Instruction> ins,
            Action<Collection<Instruction>> condition,
            Action<Collection<Instruction>> thenStatment)
        {
            var ifEnd = Instruction.Create(OpCodes.Nop);

            condition(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Bne_Un, ifEnd);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brfalse, ifEnd));
            }

            thenStatment(ins);

            ins.Add(ifEnd);
        }

        public static void IfNot(this Collection<Instruction> ins,
            Action<Collection<Instruction>> condition,
            Action<Collection<Instruction>> thenStatment)
        {
            var ifEnd = Instruction.Create(OpCodes.Nop);

            condition(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Beq, ifEnd);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brtrue, ifEnd));
            }

            thenStatment(ins);

            ins.Add(ifEnd);
        }

        public static void While(this Collection<Instruction> ins,
            Action<Collection<Instruction>> condition,
            Action<Collection<Instruction>> body)
        {
            var loopBegin = Instruction.Create(OpCodes.Nop);
            var loopEnd = Instruction.Create(OpCodes.Nop);

            ins.Add(loopBegin);

            condition(ins);

            if (ins[ins.Count - 1].OpCode == OpCodes.Ceq)
            {
                ins[ins.Count - 1] = Instruction.Create(OpCodes.Bne_Un, loopEnd);
            }
            else
            {
                ins.Add(Instruction.Create(OpCodes.Brfalse, loopEnd));
            }

            body(ins);

            ins.Add(Instruction.Create(OpCodes.Br, loopBegin));
            ins.Add(loopEnd);
        }
    }
}