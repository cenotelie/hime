using System.Collections.Generic;

namespace Hime.Kernel.Resources.Parser
{
    class FileCentralDogma_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] staticSymbolsSID = { 0xB, 0xA, 0x11, 0x12, 0xDA, 0x2D, 0x40, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4D, 0x4E, 0x51, 0x55, 0x56, 0xDB, 0x7, 0x42, 0x2E, 0x2F, 0x4B, 0x4C, 0x53, 0xD9, 0x30, 0x31, 0x39, 0x3F, 0x34, 0x3A, 0x54, 0xC, 0x32, 0x33, 0x35, 0x3B, 0xD, 0x4F, 0x52, 0xF, 0xE, 0x10, 0x50, 0x36, 0x3C, 0x37, 0x3D, 0x38, 0x3E };
        private static string[] staticSymbolsName = { "_T[.]", "NAME", "_T[{]", "_T[}]", "_T[[]", "INTEGER", "_T[=]", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[<]", "_T[>]", "_T[:]", "_T[^]", "_T[!]", "_T[]]", "SEPARATOR", "_T[..]", "QUOTED_DATA", "ESCAPEES", "_T[=>]", "_T[->]", "_T[cf]", "_T[cs]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_BINARY", "SYMBOL_VALUE_UINT8", "SYMBOL_JOKER_UINT8", "_T[rules]", "_T[public]", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT16", "SYMBOL_JOKER_UINT16", "_T[private]", "_T[options]", "_T[grammar]", "_T[internal]", "_T[protected]", "_T[namespace]", "_T[terminals]", "SYMBOL_VALUE_UINT32", "SYMBOL_JOKER_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_JOKER_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_JOKER_UINT128" };
        private static ushort[][] staticTransitions0 = { new ushort[3] { 0x2F, 0x2F, 0x1 }, new ushort[3] { 0x2E, 0x2E, 0x54 }, new ushort[3] { 0x70, 0x70, 0x55 }, new ushort[3] { 0x69, 0x69, 0x56 }, new ushort[3] { 0x6E, 0x6E, 0x57 }, new ushort[3] { 0x7B, 0x7B, 0x5E }, new ushort[3] { 0x7D, 0x7D, 0x5F }, new ushort[3] { 0x22, 0x22, 0x3 }, new ushort[3] { 0x27, 0x27, 0x4 }, new ushort[3] { 0x5B, 0x5B, 0x60 }, new ushort[3] { 0x5C, 0x5C, 0x5 }, new ushort[3] { 0x30, 0x30, 0x61 }, new ushort[3] { 0x3D, 0x3D, 0x63 }, new ushort[3] { 0x3B, 0x3B, 0x64 }, new ushort[3] { 0x28, 0x28, 0x65 }, new ushort[3] { 0x29, 0x29, 0x66 }, new ushort[3] { 0x2A, 0x2A, 0x67 }, new ushort[3] { 0x2B, 0x2B, 0x68 }, new ushort[3] { 0x3F, 0x3F, 0x69 }, new ushort[3] { 0x2C, 0x2C, 0x6A }, new ushort[3] { 0x2D, 0x2D, 0x6B }, new ushort[3] { 0x7C, 0x7C, 0x6C }, new ushort[3] { 0x3C, 0x3C, 0x6D }, new ushort[3] { 0x3E, 0x3E, 0x6E }, new ushort[3] { 0x6F, 0x6F, 0x58 }, new ushort[3] { 0x74, 0x74, 0x59 }, new ushort[3] { 0x3A, 0x3A, 0x6F }, new ushort[3] { 0x67, 0x67, 0x5A }, new ushort[3] { 0x63, 0x63, 0x5B }, new ushort[3] { 0x72, 0x72, 0x5C }, new ushort[3] { 0x5E, 0x5E, 0x70 }, new ushort[3] { 0x21, 0x21, 0x71 }, new ushort[3] { 0x5D, 0x5D, 0x72 }, new ushort[3] { 0xA, 0xA, 0x73 }, new ushort[3] { 0x2028, 0x2029, 0x73 }, new ushort[3] { 0x9, 0x9, 0x75 }, new ushort[3] { 0xB, 0xC, 0x75 }, new ushort[3] { 0x20, 0x20, 0x75 }, new ushort[3] { 0x41, 0x5A, 0x5D }, new ushort[3] { 0x5F, 0x5F, 0x5D }, new ushort[3] { 0x61, 0x62, 0x5D }, new ushort[3] { 0x64, 0x66, 0x5D }, new ushort[3] { 0x68, 0x68, 0x5D }, new ushort[3] { 0x6A, 0x6D, 0x5D }, new ushort[3] { 0x71, 0x71, 0x5D }, new ushort[3] { 0x73, 0x73, 0x5D }, new ushort[3] { 0x75, 0x7A, 0x5D }, new ushort[3] { 0x370, 0x3FF, 0x5D }, new ushort[3] { 0x31, 0x39, 0x62 }, new ushort[3] { 0x40, 0x40, 0x6 }, new ushort[3] { 0xD, 0xD, 0x74 } };
        private static ushort[][] staticTransitions1 = { new ushort[3] { 0x2F, 0x2F, 0x7 }, new ushort[3] { 0x2A, 0x2A, 0x8 } };
        private static ushort[][] staticTransitions2 = { new ushort[3] { 0x2A, 0x2A, 0x9 }, new ushort[3] { 0x2F, 0x2F, 0x18 } };
        private static ushort[][] staticTransitions3 = { new ushort[3] { 0x0, 0x21, 0x3 }, new ushort[3] { 0x23, 0xFFFF, 0x3 }, new ushort[3] { 0x22, 0x22, 0xA9 } };
        private static ushort[][] staticTransitions4 = { new ushort[3] { 0x5C, 0x5C, 0xA }, new ushort[3] { 0x0, 0x26, 0xB }, new ushort[3] { 0x28, 0x5B, 0xB }, new ushort[3] { 0x5D, 0xFFFF, 0xB } };
        private static ushort[][] staticTransitions5 = { new ushort[3] { 0x75, 0x75, 0xE }, new ushort[3] { 0x30, 0x30, 0xAA }, new ushort[3] { 0x5C, 0x5C, 0xAA }, new ushort[3] { 0x61, 0x62, 0xAA }, new ushort[3] { 0x66, 0x66, 0xAA }, new ushort[3] { 0x6E, 0x6E, 0xAA }, new ushort[3] { 0x72, 0x72, 0xAA }, new ushort[3] { 0x74, 0x74, 0xAA }, new ushort[3] { 0x76, 0x76, 0xAA } };
        private static ushort[][] staticTransitions6 = { new ushort[3] { 0x41, 0x5A, 0x81 }, new ushort[3] { 0x5F, 0x5F, 0x81 }, new ushort[3] { 0x61, 0x7A, 0x81 }, new ushort[3] { 0x370, 0x3FF, 0x81 } };
        private static ushort[][] staticTransitions7 = { new ushort[3] { 0x0, 0x9, 0x7 }, new ushort[3] { 0xB, 0xC, 0x7 }, new ushort[3] { 0xE, 0x2027, 0x7 }, new ushort[3] { 0x202A, 0xFFFF, 0x7 }, new ushort[3] { 0xA, 0xA, 0xB1 }, new ushort[3] { 0x2028, 0x2029, 0xB1 }, new ushort[3] { 0xD, 0xD, 0xB2 } };
        private static ushort[][] staticTransitions8 = { new ushort[3] { 0x2A, 0x2A, 0x11 }, new ushort[3] { 0x0, 0x29, 0x12 }, new ushort[3] { 0x2B, 0xFFFF, 0x12 } };
        private static ushort[][] staticTransitions9 = { new ushort[3] { 0x2A, 0x2A, 0x1B }, new ushort[3] { 0x0, 0x29, 0x13 }, new ushort[3] { 0x2B, 0xFFFF, 0x13 } };
        private static ushort[][] staticTransitionsA = { new ushort[3] { 0x27, 0x27, 0xB }, new ushort[3] { 0x30, 0x30, 0xB }, new ushort[3] { 0x5C, 0x5C, 0xB }, new ushort[3] { 0x61, 0x62, 0xB }, new ushort[3] { 0x66, 0x66, 0xB }, new ushort[3] { 0x6E, 0x6E, 0xB }, new ushort[3] { 0x72, 0x72, 0xB }, new ushort[3] { 0x74, 0x74, 0xB }, new ushort[3] { 0x76, 0x76, 0xB } };
        private static ushort[][] staticTransitionsB = { new ushort[3] { 0x27, 0x27, 0xB3 }, new ushort[3] { 0x5C, 0x5C, 0xA }, new ushort[3] { 0x0, 0x26, 0xB }, new ushort[3] { 0x28, 0x5B, 0xB }, new ushort[3] { 0x5D, 0xFFFF, 0xB } };
        private static ushort[][] staticTransitionsC = { new ushort[3] { 0x30, 0x30, 0xD }, new ushort[3] { 0x5B, 0x5D, 0xD }, new ushort[3] { 0x61, 0x62, 0xD }, new ushort[3] { 0x66, 0x66, 0xD }, new ushort[3] { 0x6E, 0x6E, 0xD }, new ushort[3] { 0x72, 0x72, 0xD }, new ushort[3] { 0x74, 0x74, 0xD }, new ushort[3] { 0x76, 0x76, 0xD } };
        private static ushort[][] staticTransitionsD = { new ushort[3] { 0x5D, 0x5D, 0xB4 }, new ushort[3] { 0x5C, 0x5C, 0xC }, new ushort[3] { 0x0, 0x5A, 0xD }, new ushort[3] { 0x5E, 0xFFFF, 0xD } };
        private static ushort[][] staticTransitionsE = { new ushort[3] { 0x62, 0x62, 0x14 }, new ushort[3] { 0x63, 0x63, 0x15 } };
        private static ushort[][] staticTransitionsF = { new ushort[3] { 0x30, 0x39, 0x16 }, new ushort[3] { 0x41, 0x46, 0x16 }, new ushort[3] { 0x61, 0x66, 0x16 }, new ushort[3] { 0x58, 0x58, 0x17 } };
        private static ushort[][] staticTransitions10 = { new ushort[3] { 0x30, 0x31, 0xB5 }, new ushort[3] { 0x42, 0x42, 0xB6 } };
        private static ushort[][] staticTransitions11 = { new ushort[3] { 0x2A, 0x2A, 0x11 }, new ushort[3] { 0x0, 0x29, 0x12 }, new ushort[3] { 0x2B, 0x2E, 0x12 }, new ushort[3] { 0x30, 0xFFFF, 0x12 }, new ushort[3] { 0x2F, 0x2F, 0xB7 } };
        private static ushort[][] staticTransitions12 = { new ushort[3] { 0x2A, 0x2A, 0x11 }, new ushort[3] { 0x0, 0x29, 0x12 }, new ushort[3] { 0x2B, 0xFFFF, 0x12 } };
        private static ushort[][] staticTransitions13 = { new ushort[3] { 0x2A, 0x2A, 0x1B }, new ushort[3] { 0x0, 0x29, 0x13 }, new ushort[3] { 0x2B, 0xFFFF, 0x13 } };
        private static ushort[][] staticTransitions14 = { new ushort[3] { 0x7B, 0x7B, 0x19 } };
        private static ushort[][] staticTransitions15 = { new ushort[3] { 0x7B, 0x7B, 0x1A } };
        private static ushort[][] staticTransitions16 = { new ushort[3] { 0x30, 0x39, 0xB8 }, new ushort[3] { 0x41, 0x46, 0xB8 }, new ushort[3] { 0x61, 0x66, 0xB8 } };
        private static ushort[][] staticTransitions17 = { new ushort[3] { 0x58, 0x58, 0xB9 } };
        private static ushort[][] staticTransitions18 = { new ushort[3] { 0x0, 0x9, 0x18 }, new ushort[3] { 0xB, 0xC, 0x18 }, new ushort[3] { 0xE, 0x2027, 0x18 }, new ushort[3] { 0x202A, 0xFFFF, 0x18 }, new ushort[3] { 0xA, 0xA, 0xAF }, new ushort[3] { 0xD, 0xD, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitions19 = { new ushort[3] { 0x40, 0x40, 0x1C }, new ushort[3] { 0x41, 0x5A, 0x1E }, new ushort[3] { 0x5F, 0x5F, 0x1E }, new ushort[3] { 0x61, 0x7A, 0x1E }, new ushort[3] { 0x370, 0x3FF, 0x1E } };
        private static ushort[][] staticTransitions1A = { new ushort[3] { 0x40, 0x40, 0x1D }, new ushort[3] { 0x41, 0x5A, 0x1F }, new ushort[3] { 0x5F, 0x5F, 0x1F }, new ushort[3] { 0x61, 0x7A, 0x1F }, new ushort[3] { 0x370, 0x3FF, 0x1F } };
        private static ushort[][] staticTransitions1B = { new ushort[3] { 0x2A, 0x2A, 0x1B }, new ushort[3] { 0x0, 0x29, 0x13 }, new ushort[3] { 0x2B, 0x2E, 0x13 }, new ushort[3] { 0x30, 0xFFFF, 0x13 }, new ushort[3] { 0x2F, 0x2F, 0xAF } };
        private static ushort[][] staticTransitions1C = { new ushort[3] { 0x41, 0x5A, 0x1E }, new ushort[3] { 0x5F, 0x5F, 0x1E }, new ushort[3] { 0x61, 0x7A, 0x1E }, new ushort[3] { 0x370, 0x3FF, 0x1E } };
        private static ushort[][] staticTransitions1D = { new ushort[3] { 0x41, 0x5A, 0x1F }, new ushort[3] { 0x5F, 0x5F, 0x1F }, new ushort[3] { 0x61, 0x7A, 0x1F }, new ushort[3] { 0x370, 0x3FF, 0x1F } };
        private static ushort[][] staticTransitions1E = { new ushort[3] { 0x30, 0x39, 0x1E }, new ushort[3] { 0x41, 0x5A, 0x1E }, new ushort[3] { 0x5F, 0x5F, 0x1E }, new ushort[3] { 0x61, 0x7A, 0x1E }, new ushort[3] { 0x370, 0x3FF, 0x1E }, new ushort[3] { 0x7D, 0x7D, 0xBC } };
        private static ushort[][] staticTransitions1F = { new ushort[3] { 0x30, 0x39, 0x1F }, new ushort[3] { 0x41, 0x5A, 0x1F }, new ushort[3] { 0x5F, 0x5F, 0x1F }, new ushort[3] { 0x61, 0x7A, 0x1F }, new ushort[3] { 0x370, 0x3FF, 0x1F }, new ushort[3] { 0x7D, 0x7D, 0xBD } };
        private static ushort[][] staticTransitions20 = { new ushort[3] { 0x30, 0x39, 0xBE }, new ushort[3] { 0x41, 0x46, 0xBE }, new ushort[3] { 0x61, 0x66, 0xBE } };
        private static ushort[][] staticTransitions21 = { new ushort[3] { 0x58, 0x58, 0xBF } };
        private static ushort[][] staticTransitions22 = { new ushort[3] { 0x30, 0x39, 0x35 }, new ushort[3] { 0x41, 0x46, 0x35 }, new ushort[3] { 0x61, 0x66, 0x35 } };
        private static ushort[][] staticTransitions23 = { new ushort[3] { 0x30, 0x39, 0x2D }, new ushort[3] { 0x41, 0x46, 0x2D }, new ushort[3] { 0x61, 0x66, 0x2D } };
        private static ushort[][] staticTransitions24 = { new ushort[3] { 0x30, 0x39, 0x25 }, new ushort[3] { 0x41, 0x46, 0x25 }, new ushort[3] { 0x61, 0x66, 0x25 } };
        private static ushort[][] staticTransitions25 = { new ushort[3] { 0x30, 0x39, 0x26 }, new ushort[3] { 0x41, 0x46, 0x26 }, new ushort[3] { 0x61, 0x66, 0x26 } };
        private static ushort[][] staticTransitions26 = { new ushort[3] { 0x30, 0x39, 0x27 }, new ushort[3] { 0x41, 0x46, 0x27 }, new ushort[3] { 0x61, 0x66, 0x27 } };
        private static ushort[][] staticTransitions27 = { new ushort[3] { 0x30, 0x39, 0x28 }, new ushort[3] { 0x41, 0x46, 0x28 }, new ushort[3] { 0x61, 0x66, 0x28 } };
        private static ushort[][] staticTransitions28 = { new ushort[3] { 0x30, 0x39, 0x29 }, new ushort[3] { 0x41, 0x46, 0x29 }, new ushort[3] { 0x61, 0x66, 0x29 } };
        private static ushort[][] staticTransitions29 = { new ushort[3] { 0x30, 0x39, 0x2A }, new ushort[3] { 0x41, 0x46, 0x2A }, new ushort[3] { 0x61, 0x66, 0x2A } };
        private static ushort[][] staticTransitions2A = { new ushort[3] { 0x30, 0x39, 0x2B }, new ushort[3] { 0x41, 0x46, 0x2B }, new ushort[3] { 0x61, 0x66, 0x2B } };
        private static ushort[][] staticTransitions2B = { new ushort[3] { 0x30, 0x39, 0x2C }, new ushort[3] { 0x41, 0x46, 0x2C }, new ushort[3] { 0x61, 0x66, 0x2C } };
        private static ushort[][] staticTransitions2C = { new ushort[3] { 0x30, 0x39, 0x2E }, new ushort[3] { 0x41, 0x46, 0x2E }, new ushort[3] { 0x61, 0x66, 0x2E } };
        private static ushort[][] staticTransitions2D = { new ushort[3] { 0x30, 0x39, 0x2F }, new ushort[3] { 0x41, 0x46, 0x2F }, new ushort[3] { 0x61, 0x66, 0x2F } };
        private static ushort[][] staticTransitions2E = { new ushort[3] { 0x30, 0x39, 0x30 }, new ushort[3] { 0x41, 0x46, 0x30 }, new ushort[3] { 0x61, 0x66, 0x30 } };
        private static ushort[][] staticTransitions2F = { new ushort[3] { 0x30, 0x39, 0x31 }, new ushort[3] { 0x41, 0x46, 0x31 }, new ushort[3] { 0x61, 0x66, 0x31 } };
        private static ushort[][] staticTransitions30 = { new ushort[3] { 0x30, 0x39, 0x32 }, new ushort[3] { 0x41, 0x46, 0x32 }, new ushort[3] { 0x61, 0x66, 0x32 } };
        private static ushort[][] staticTransitions31 = { new ushort[3] { 0x30, 0x39, 0x33 }, new ushort[3] { 0x41, 0x46, 0x33 }, new ushort[3] { 0x61, 0x66, 0x33 } };
        private static ushort[][] staticTransitions32 = { new ushort[3] { 0x30, 0x39, 0x34 }, new ushort[3] { 0x41, 0x46, 0x34 }, new ushort[3] { 0x61, 0x66, 0x34 } };
        private static ushort[][] staticTransitions33 = { new ushort[3] { 0x30, 0x39, 0x36 }, new ushort[3] { 0x41, 0x46, 0x36 }, new ushort[3] { 0x61, 0x66, 0x36 } };
        private static ushort[][] staticTransitions34 = { new ushort[3] { 0x30, 0x39, 0x37 }, new ushort[3] { 0x41, 0x46, 0x37 }, new ushort[3] { 0x61, 0x66, 0x37 } };
        private static ushort[][] staticTransitions35 = { new ushort[3] { 0x30, 0x39, 0x4E }, new ushort[3] { 0x41, 0x46, 0x4E }, new ushort[3] { 0x61, 0x66, 0x4E } };
        private static ushort[][] staticTransitions36 = { new ushort[3] { 0x30, 0x39, 0x50 }, new ushort[3] { 0x41, 0x46, 0x50 }, new ushort[3] { 0x61, 0x66, 0x50 } };
        private static ushort[][] staticTransitions37 = { new ushort[3] { 0x30, 0x39, 0x52 }, new ushort[3] { 0x41, 0x46, 0x52 }, new ushort[3] { 0x61, 0x66, 0x52 } };
        private static ushort[][] staticTransitions38 = { new ushort[3] { 0x58, 0x58, 0x4B } };
        private static ushort[][] staticTransitions39 = { new ushort[3] { 0x58, 0x58, 0x43 } };
        private static ushort[][] staticTransitions3A = { new ushort[3] { 0x58, 0x58, 0x3B } };
        private static ushort[][] staticTransitions3B = { new ushort[3] { 0x58, 0x58, 0x3C } };
        private static ushort[][] staticTransitions3C = { new ushort[3] { 0x58, 0x58, 0x3D } };
        private static ushort[][] staticTransitions3D = { new ushort[3] { 0x58, 0x58, 0x3E } };
        private static ushort[][] staticTransitions3E = { new ushort[3] { 0x58, 0x58, 0x3F } };
        private static ushort[][] staticTransitions3F = { new ushort[3] { 0x58, 0x58, 0x40 } };
        private static ushort[][] staticTransitions40 = { new ushort[3] { 0x58, 0x58, 0x41 } };
        private static ushort[][] staticTransitions41 = { new ushort[3] { 0x58, 0x58, 0x42 } };
        private static ushort[][] staticTransitions42 = { new ushort[3] { 0x58, 0x58, 0x44 } };
        private static ushort[][] staticTransitions43 = { new ushort[3] { 0x58, 0x58, 0x45 } };
        private static ushort[][] staticTransitions44 = { new ushort[3] { 0x58, 0x58, 0x46 } };
        private static ushort[][] staticTransitions45 = { new ushort[3] { 0x58, 0x58, 0x47 } };
        private static ushort[][] staticTransitions46 = { new ushort[3] { 0x58, 0x58, 0x48 } };
        private static ushort[][] staticTransitions47 = { new ushort[3] { 0x58, 0x58, 0x49 } };
        private static ushort[][] staticTransitions48 = { new ushort[3] { 0x58, 0x58, 0x4A } };
        private static ushort[][] staticTransitions49 = { new ushort[3] { 0x58, 0x58, 0x4C } };
        private static ushort[][] staticTransitions4A = { new ushort[3] { 0x58, 0x58, 0x4D } };
        private static ushort[][] staticTransitions4B = { new ushort[3] { 0x58, 0x58, 0x4F } };
        private static ushort[][] staticTransitions4C = { new ushort[3] { 0x58, 0x58, 0x51 } };
        private static ushort[][] staticTransitions4D = { new ushort[3] { 0x58, 0x58, 0x53 } };
        private static ushort[][] staticTransitions4E = { new ushort[3] { 0x30, 0x39, 0xC7 }, new ushort[3] { 0x41, 0x46, 0xC7 }, new ushort[3] { 0x61, 0x66, 0xC7 } };
        private static ushort[][] staticTransitions4F = { new ushort[3] { 0x58, 0x58, 0xC8 } };
        private static ushort[][] staticTransitions50 = { new ushort[3] { 0x30, 0x39, 0xC9 }, new ushort[3] { 0x41, 0x46, 0xC9 }, new ushort[3] { 0x61, 0x66, 0xC9 } };
        private static ushort[][] staticTransitions51 = { new ushort[3] { 0x58, 0x58, 0xCA } };
        private static ushort[][] staticTransitions52 = { new ushort[3] { 0x30, 0x39, 0xCB }, new ushort[3] { 0x41, 0x46, 0xCB }, new ushort[3] { 0x61, 0x66, 0xCB } };
        private static ushort[][] staticTransitions53 = { new ushort[3] { 0x58, 0x58, 0xCC } };
        private static ushort[][] staticTransitions54 = { new ushort[3] { 0x2E, 0x2E, 0x76 } };
        private static ushort[][] staticTransitions55 = { new ushort[3] { 0x75, 0x75, 0x77 }, new ushort[3] { 0x72, 0x72, 0x78 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x71, 0x79 }, new ushort[3] { 0x73, 0x74, 0x79 }, new ushort[3] { 0x76, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions56 = { new ushort[3] { 0x6E, 0x6E, 0x7A }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6D, 0x79 }, new ushort[3] { 0x6F, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions57 = { new ushort[3] { 0x61, 0x61, 0x7B }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions58 = { new ushort[3] { 0x70, 0x70, 0x7C }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6F, 0x79 }, new ushort[3] { 0x71, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions59 = { new ushort[3] { 0x65, 0x65, 0x7D }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions5A = { new ushort[3] { 0x72, 0x72, 0x7E }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x71, 0x79 }, new ushort[3] { 0x73, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions5B = { new ushort[3] { 0x66, 0x66, 0xAD }, new ushort[3] { 0x73, 0x73, 0xAE }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x65, 0x79 }, new ushort[3] { 0x67, 0x72, 0x79 }, new ushort[3] { 0x74, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions5C = { new ushort[3] { 0x75, 0x75, 0x7F }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x74, 0x79 }, new ushort[3] { 0x76, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions5D = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions5E = {  };
        private static ushort[][] staticTransitions5F = {  };
        private static ushort[][] staticTransitions60 = { new ushort[3] { 0x5C, 0x5C, 0xC }, new ushort[3] { 0x0, 0x5A, 0xD }, new ushort[3] { 0x5E, 0xFFFF, 0xD } };
        private static ushort[][] staticTransitions61 = { new ushort[3] { 0x78, 0x78, 0xF }, new ushort[3] { 0x62, 0x62, 0x10 } };
        private static ushort[][] staticTransitions62 = { new ushort[3] { 0x30, 0x39, 0xB0 } };
        private static ushort[][] staticTransitions63 = { new ushort[3] { 0x3E, 0x3E, 0xAB } };
        private static ushort[][] staticTransitions64 = {  };
        private static ushort[][] staticTransitions65 = {  };
        private static ushort[][] staticTransitions66 = {  };
        private static ushort[][] staticTransitions67 = {  };
        private static ushort[][] staticTransitions68 = {  };
        private static ushort[][] staticTransitions69 = {  };
        private static ushort[][] staticTransitions6A = {  };
        private static ushort[][] staticTransitions6B = { new ushort[3] { 0x3E, 0x3E, 0xAC } };
        private static ushort[][] staticTransitions6C = {  };
        private static ushort[][] staticTransitions6D = {  };
        private static ushort[][] staticTransitions6E = {  };
        private static ushort[][] staticTransitions6F = {  };
        private static ushort[][] staticTransitions70 = {  };
        private static ushort[][] staticTransitions71 = {  };
        private static ushort[][] staticTransitions72 = {  };
        private static ushort[][] staticTransitions73 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitions74 = { new ushort[3] { 0xA, 0xA, 0x73 }, new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0x9, 0xAF }, new ushort[3] { 0xB, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitions75 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitions76 = {  };
        private static ushort[][] staticTransitions77 = { new ushort[3] { 0x62, 0x62, 0x80 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x61, 0x79 }, new ushort[3] { 0x63, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions78 = { new ushort[3] { 0x69, 0x69, 0x82 }, new ushort[3] { 0x6F, 0x6F, 0x83 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x68, 0x79 }, new ushort[3] { 0x6A, 0x6E, 0x79 }, new ushort[3] { 0x70, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions79 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7A = { new ushort[3] { 0x74, 0x74, 0x84 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x73, 0x79 }, new ushort[3] { 0x75, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7B = { new ushort[3] { 0x6D, 0x6D, 0x85 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6C, 0x79 }, new ushort[3] { 0x6E, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7C = { new ushort[3] { 0x74, 0x74, 0x86 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x73, 0x79 }, new ushort[3] { 0x75, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7D = { new ushort[3] { 0x72, 0x72, 0x87 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x71, 0x79 }, new ushort[3] { 0x73, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7E = { new ushort[3] { 0x61, 0x61, 0x88 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions7F = { new ushort[3] { 0x6C, 0x6C, 0x89 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6B, 0x79 }, new ushort[3] { 0x6D, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions80 = { new ushort[3] { 0x6C, 0x6C, 0x8A }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6B, 0x79 }, new ushort[3] { 0x6D, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions81 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions82 = { new ushort[3] { 0x76, 0x76, 0x8B }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x75, 0x79 }, new ushort[3] { 0x77, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions83 = { new ushort[3] { 0x74, 0x74, 0x8C }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x73, 0x79 }, new ushort[3] { 0x75, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions84 = { new ushort[3] { 0x65, 0x65, 0x8D }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions85 = { new ushort[3] { 0x65, 0x65, 0x8E }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions86 = { new ushort[3] { 0x69, 0x69, 0x8F }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x68, 0x79 }, new ushort[3] { 0x6A, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions87 = { new ushort[3] { 0x6D, 0x6D, 0x90 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6C, 0x79 }, new ushort[3] { 0x6E, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions88 = { new ushort[3] { 0x6D, 0x6D, 0x91 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6C, 0x79 }, new ushort[3] { 0x6E, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions89 = { new ushort[3] { 0x65, 0x65, 0x92 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8A = { new ushort[3] { 0x69, 0x69, 0x93 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x68, 0x79 }, new ushort[3] { 0x6A, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8B = { new ushort[3] { 0x61, 0x61, 0x94 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8C = { new ushort[3] { 0x65, 0x65, 0x95 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8D = { new ushort[3] { 0x72, 0x72, 0x96 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x71, 0x79 }, new ushort[3] { 0x73, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8E = { new ushort[3] { 0x73, 0x73, 0x98 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x72, 0x79 }, new ushort[3] { 0x74, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions8F = { new ushort[3] { 0x6F, 0x6F, 0x99 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6E, 0x79 }, new ushort[3] { 0x70, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions90 = { new ushort[3] { 0x69, 0x69, 0x97 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x68, 0x79 }, new ushort[3] { 0x6A, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions91 = { new ushort[3] { 0x6D, 0x6D, 0x9A }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6C, 0x79 }, new ushort[3] { 0x6E, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions92 = { new ushort[3] { 0x73, 0x73, 0xBA }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x72, 0x79 }, new ushort[3] { 0x74, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions93 = { new ushort[3] { 0x63, 0x63, 0xBB }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x62, 0x79 }, new ushort[3] { 0x64, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions94 = { new ushort[3] { 0x74, 0x74, 0xA2 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x73, 0x79 }, new ushort[3] { 0x75, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions95 = { new ushort[3] { 0x63, 0x63, 0x9B }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x62, 0x79 }, new ushort[3] { 0x64, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions96 = { new ushort[3] { 0x6E, 0x6E, 0x9C }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6D, 0x79 }, new ushort[3] { 0x6F, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions97 = { new ushort[3] { 0x6E, 0x6E, 0x9E }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6D, 0x79 }, new ushort[3] { 0x6F, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions98 = { new ushort[3] { 0x70, 0x70, 0x9D }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6F, 0x79 }, new ushort[3] { 0x71, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions99 = { new ushort[3] { 0x6E, 0x6E, 0xA3 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6D, 0x79 }, new ushort[3] { 0x6F, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9A = { new ushort[3] { 0x61, 0x61, 0xA4 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9B = { new ushort[3] { 0x74, 0x74, 0x9F }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x73, 0x79 }, new ushort[3] { 0x75, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9C = { new ushort[3] { 0x61, 0x61, 0xA5 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9D = { new ushort[3] { 0x61, 0x61, 0xA0 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9E = { new ushort[3] { 0x61, 0x61, 0xA1 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x62, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitions9F = { new ushort[3] { 0x65, 0x65, 0xA6 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA0 = { new ushort[3] { 0x63, 0x63, 0xA7 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x62, 0x79 }, new ushort[3] { 0x64, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA1 = { new ushort[3] { 0x6C, 0x6C, 0xA8 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6B, 0x79 }, new ushort[3] { 0x6D, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA2 = { new ushort[3] { 0x65, 0x65, 0xC0 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA3 = { new ushort[3] { 0x73, 0x73, 0xC1 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x72, 0x79 }, new ushort[3] { 0x74, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA4 = { new ushort[3] { 0x72, 0x72, 0xC2 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x71, 0x79 }, new ushort[3] { 0x73, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA5 = { new ushort[3] { 0x6C, 0x6C, 0xC3 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x6B, 0x79 }, new ushort[3] { 0x6D, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA6 = { new ushort[3] { 0x64, 0x64, 0xC4 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x63, 0x79 }, new ushort[3] { 0x65, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA7 = { new ushort[3] { 0x65, 0x65, 0xC5 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x64, 0x79 }, new ushort[3] { 0x66, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA8 = { new ushort[3] { 0x73, 0x73, 0xC6 }, new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x72, 0x79 }, new ushort[3] { 0x74, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsA9 = {  };
        private static ushort[][] staticTransitionsAA = {  };
        private static ushort[][] staticTransitionsAB = {  };
        private static ushort[][] staticTransitionsAC = {  };
        private static ushort[][] staticTransitionsAD = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsAE = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsAF = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitionsB0 = { new ushort[3] { 0x30, 0x39, 0xB0 } };
        private static ushort[][] staticTransitionsB1 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitionsB2 = { new ushort[3] { 0xA, 0xA, 0xB1 }, new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0x9, 0xAF }, new ushort[3] { 0xB, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitionsB3 = {  };
        private static ushort[][] staticTransitionsB4 = {  };
        private static ushort[][] staticTransitionsB5 = { new ushort[3] { 0x30, 0x31, 0xB5 } };
        private static ushort[][] staticTransitionsB6 = { new ushort[3] { 0x42, 0x42, 0xB6 } };
        private static ushort[][] staticTransitionsB7 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF } };
        private static ushort[][] staticTransitionsB8 = { new ushort[3] { 0x30, 0x39, 0x20 }, new ushort[3] { 0x41, 0x46, 0x20 }, new ushort[3] { 0x61, 0x66, 0x20 } };
        private static ushort[][] staticTransitionsB9 = { new ushort[3] { 0x58, 0x58, 0x21 } };
        private static ushort[][] staticTransitionsBA = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsBB = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsBC = {  };
        private static ushort[][] staticTransitionsBD = {  };
        private static ushort[][] staticTransitionsBE = { new ushort[3] { 0x30, 0x39, 0x22 }, new ushort[3] { 0x41, 0x46, 0x22 }, new ushort[3] { 0x61, 0x66, 0x22 } };
        private static ushort[][] staticTransitionsBF = { new ushort[3] { 0x58, 0x58, 0x38 } };
        private static ushort[][] staticTransitionsC0 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC1 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC2 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC3 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC4 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC5 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC6 = { new ushort[3] { 0x30, 0x39, 0x79 }, new ushort[3] { 0x41, 0x5A, 0x79 }, new ushort[3] { 0x5F, 0x5F, 0x79 }, new ushort[3] { 0x61, 0x7A, 0x79 }, new ushort[3] { 0x370, 0x3FF, 0x79 } };
        private static ushort[][] staticTransitionsC7 = { new ushort[3] { 0x30, 0x39, 0x23 }, new ushort[3] { 0x41, 0x46, 0x23 }, new ushort[3] { 0x61, 0x66, 0x23 } };
        private static ushort[][] staticTransitionsC8 = { new ushort[3] { 0x58, 0x58, 0x39 } };
        private static ushort[][] staticTransitionsC9 = { new ushort[3] { 0x30, 0x39, 0x24 }, new ushort[3] { 0x41, 0x46, 0x24 }, new ushort[3] { 0x61, 0x66, 0x24 } };
        private static ushort[][] staticTransitionsCA = { new ushort[3] { 0x58, 0x58, 0x3A } };
        private static ushort[][] staticTransitionsCB = {  };
        private static ushort[][] staticTransitionsCC = {  };
        private static ushort[][][] staticTransitions = { staticTransitions0, staticTransitions1, staticTransitions2, staticTransitions3, staticTransitions4, staticTransitions5, staticTransitions6, staticTransitions7, staticTransitions8, staticTransitions9, staticTransitionsA, staticTransitionsB, staticTransitionsC, staticTransitionsD, staticTransitionsE, staticTransitionsF, staticTransitions10, staticTransitions11, staticTransitions12, staticTransitions13, staticTransitions14, staticTransitions15, staticTransitions16, staticTransitions17, staticTransitions18, staticTransitions19, staticTransitions1A, staticTransitions1B, staticTransitions1C, staticTransitions1D, staticTransitions1E, staticTransitions1F, staticTransitions20, staticTransitions21, staticTransitions22, staticTransitions23, staticTransitions24, staticTransitions25, staticTransitions26, staticTransitions27, staticTransitions28, staticTransitions29, staticTransitions2A, staticTransitions2B, staticTransitions2C, staticTransitions2D, staticTransitions2E, staticTransitions2F, staticTransitions30, staticTransitions31, staticTransitions32, staticTransitions33, staticTransitions34, staticTransitions35, staticTransitions36, staticTransitions37, staticTransitions38, staticTransitions39, staticTransitions3A, staticTransitions3B, staticTransitions3C, staticTransitions3D, staticTransitions3E, staticTransitions3F, staticTransitions40, staticTransitions41, staticTransitions42, staticTransitions43, staticTransitions44, staticTransitions45, staticTransitions46, staticTransitions47, staticTransitions48, staticTransitions49, staticTransitions4A, staticTransitions4B, staticTransitions4C, staticTransitions4D, staticTransitions4E, staticTransitions4F, staticTransitions50, staticTransitions51, staticTransitions52, staticTransitions53, staticTransitions54, staticTransitions55, staticTransitions56, staticTransitions57, staticTransitions58, staticTransitions59, staticTransitions5A, staticTransitions5B, staticTransitions5C, staticTransitions5D, staticTransitions5E, staticTransitions5F, staticTransitions60, staticTransitions61, staticTransitions62, staticTransitions63, staticTransitions64, staticTransitions65, staticTransitions66, staticTransitions67, staticTransitions68, staticTransitions69, staticTransitions6A, staticTransitions6B, staticTransitions6C, staticTransitions6D, staticTransitions6E, staticTransitions6F, staticTransitions70, staticTransitions71, staticTransitions72, staticTransitions73, staticTransitions74, staticTransitions75, staticTransitions76, staticTransitions77, staticTransitions78, staticTransitions79, staticTransitions7A, staticTransitions7B, staticTransitions7C, staticTransitions7D, staticTransitions7E, staticTransitions7F, staticTransitions80, staticTransitions81, staticTransitions82, staticTransitions83, staticTransitions84, staticTransitions85, staticTransitions86, staticTransitions87, staticTransitions88, staticTransitions89, staticTransitions8A, staticTransitions8B, staticTransitions8C, staticTransitions8D, staticTransitions8E, staticTransitions8F, staticTransitions90, staticTransitions91, staticTransitions92, staticTransitions93, staticTransitions94, staticTransitions95, staticTransitions96, staticTransitions97, staticTransitions98, staticTransitions99, staticTransitions9A, staticTransitions9B, staticTransitions9C, staticTransitions9D, staticTransitions9E, staticTransitions9F, staticTransitionsA0, staticTransitionsA1, staticTransitionsA2, staticTransitionsA3, staticTransitionsA4, staticTransitionsA5, staticTransitionsA6, staticTransitionsA7, staticTransitionsA8, staticTransitionsA9, staticTransitionsAA, staticTransitionsAB, staticTransitionsAC, staticTransitionsAD, staticTransitionsAE, staticTransitionsAF, staticTransitionsB0, staticTransitionsB1, staticTransitionsB2, staticTransitionsB3, staticTransitionsB4, staticTransitionsB5, staticTransitionsB6, staticTransitionsB7, staticTransitionsB8, staticTransitionsB9, staticTransitionsBA, staticTransitionsBB, staticTransitionsBC, staticTransitionsBD, staticTransitionsBE, staticTransitionsBF, staticTransitionsC0, staticTransitionsC1, staticTransitionsC2, staticTransitionsC3, staticTransitionsC4, staticTransitionsC5, staticTransitionsC6, staticTransitionsC7, staticTransitionsC8, staticTransitionsC9, staticTransitionsCA, staticTransitionsCB, staticTransitionsCC };
        private static int[] staticFinals = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 22, 22, 23, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 24, 25, 26, 27, 28, 29, 22, 5, 22, 22, 30, 31, 32, 33, 22, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54 };
        protected override void setup() {
            symbolsSID = staticSymbolsSID;
            symbolsName = staticSymbolsName;
            symbolsSubGrammars = new Dictionary<ushort, MatchSubGrammar>();
            transitions = staticTransitions;
            finals = staticFinals;
            separatorID = 0x7;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new FileCentralDogma_Lexer(this);
        }
        public FileCentralDogma_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public FileCentralDogma_Lexer(System.IO.TextReader input) : base(input) {}
        public FileCentralDogma_Lexer(FileCentralDogma_Lexer original) : base(original) {}
    }
    class FileCentralDogma_Parser : Hime.Redist.Parsers.LR1TextParser
    {
        private static void Production_13_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x13, "qualified_name"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_14_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x14, "symbol_access_public"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("access_public"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_15_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x15, "symbol_access_private"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("access_private"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_16_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x16, "symbol_access_protected"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("access_protected"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_17_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x17, "symbol_access_internal"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("access_internal"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_18_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_18_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_18_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_18_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_18_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_19_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x19, "Namespace_content"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_1A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x1A, "Namespace"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_1B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x1B, "_m20"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_1B_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x1B, "_m20"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_1C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x1C, "_m25"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_1C_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x1C, "_m25"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_57_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x57, "option"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_58_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x58, "terminal_def_atom_any"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_59_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x59, "terminal_def_atom_unicode"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_59_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x59, "terminal_def_atom_unicode"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5A, "terminal_def_atom_text"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5B, "terminal_def_atom_set"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5C, "terminal_def_atom_ublock"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5D_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5D, "terminal_def_atom_ucat"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5E_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5E, "terminal_def_atom_span"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_5 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_6 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_5F_7 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_60_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x60, "terminal_def_element"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_60_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x60, "terminal_def_element"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_61_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_61_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_61_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_61_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("range"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_61_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("range"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_62_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x62, "terminal_def_repetition"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_62_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x62, "terminal_def_repetition"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_63_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x63, "terminal_def_fragment"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_64_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x64, "terminal_def_restrict"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_65_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x65, "terminal_definition"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_66_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x66, "terminal_subgrammar"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_66_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x66, "terminal_subgrammar"));
            nodes.Add(SubRoot);
        }
        private static void Production_67_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x67, "terminal"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_68_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x68, "rule_sym_action"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_69_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x69, "rule_sym_virtual"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6A, "rule_sym_ref_simple"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6B, "rule_template_params"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_5 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_6 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_7 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_8 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_9 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_A (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6C_B (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6D_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6D, "grammar_text_terminal"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_6E_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6E, "grammar_options"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_6F_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x6F, "grammar_terminals"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_70_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x70, "grammar_parency"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_70_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x70, "grammar_parency"));
            nodes.Add(SubRoot);
        }
        private static void Production_71_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_71_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_71_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_71_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_72_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 10, 10);
            nodes.RemoveRange(nodes.Count - 10, 10);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x72, "cf_grammar_text"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4]);
            SubRoot.AppendChild(Definition[5], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[6]);
            SubRoot.AppendChild(Definition[7]);
            SubRoot.AppendChild(Definition[8]);
            SubRoot.AppendChild(Definition[9], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_73_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 9, 9);
            nodes.RemoveRange(nodes.Count - 9, 9);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x73, "cf_grammar_bin"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4]);
            SubRoot.AppendChild(Definition[5], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[6]);
            SubRoot.AppendChild(Definition[7]);
            SubRoot.AppendChild(Definition[8], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_74_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x74, "_m89"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("concat"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_74_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x74, "_m89"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_75_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x75, "_m91"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_75_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x75, "_m91"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_76_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x76, "_m93"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_76_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x76, "_m93"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_77_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x77, "_m101"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_77_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x77, "_m101"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_78_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x78, "_m105"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_78_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x78, "_m105"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_79_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x79, "_m109"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_79_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x79, "_m109"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_7A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7A, "_m113"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_7A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7A, "_m113"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_7B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7B, "grammar_cf_rules<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_7C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7C, "cf_rule_simple<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_7D_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7D, "rule_definition<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_7E_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7E, "rule_def_choice<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_7E_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7E, "rule_def_choice<grammar_text_terminal>"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("emptypart"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            nodes.Add(SubRoot);
        }
        private static void Production_7F_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x7F, "rule_def_restrict<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_80_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x80, "rule_def_fragment<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_81_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_81_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_81_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_81_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_82_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_82_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_82_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_83_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x83, "rule_def_element<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_83_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x83, "rule_def_element<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_84_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_84_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_84_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_84_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_84_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_85_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x85, "rule_sym_ref_template<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_86_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x86, "rule_sym_ref_params<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_87_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x87, "_m134"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_87_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x87, "_m134"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_88_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x88, "_m143"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("concat"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_88_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x88, "_m143"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_89_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x89, "_m145"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_89_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x89, "_m145"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_8A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8A, "_m147"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_8A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8A, "_m147"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_8B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8B, "cf_rule_template<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_8C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_8C_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_8C_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_8D_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8D, "grammar_cf_rules<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_8E_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8E, "cf_rule_simple<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_8F_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8F, "rule_definition<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_90_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x90, "rule_def_choice<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_90_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x90, "rule_def_choice<grammar_bin_terminal>"));
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("emptypart"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            nodes.Add(SubRoot);
        }
        private static void Production_91_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x91, "rule_def_restrict<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_92_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x92, "rule_def_fragment<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_93_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_93_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_93_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_93_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_94_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_94_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_94_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_95_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x95, "rule_def_element<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_95_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x95, "rule_def_element<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_96_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_96_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_96_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_96_3 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_96_4 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_97_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x97, "rule_sym_ref_template<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_98_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x98, "rule_sym_ref_params<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_99_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x99, "_m175"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_99_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x99, "_m175"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_9A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9A, "_m184"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVirtual("concat"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_9A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9A, "_m184"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_9B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9B, "_m186"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_9B_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9B, "_m186"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_9C_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9C, "_m188"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_9C_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9C, "_m188"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_9D_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9D, "cf_rule_template<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_9E_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_9E_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_9E_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_DC_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 9, 9);
            nodes.RemoveRange(nodes.Count - 9, 9);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xDC, "cs_grammar_text"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[5]);
            SubRoot.AppendChild(Definition[6]);
            SubRoot.AppendChild(Definition[7]);
            SubRoot.AppendChild(Definition[8], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_DD_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 8, 8);
            nodes.RemoveRange(nodes.Count - 8, 8);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xDD, "cs_grammar_bin"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[5]);
            SubRoot.AppendChild(Definition[6]);
            SubRoot.AppendChild(Definition[7], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_DE_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xDE, "grammar_cs_rules<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_DF_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 6, 6);
            nodes.RemoveRange(nodes.Count - 6, 6);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xDF, "cs_rule_simple<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[4]);
            SubRoot.AppendChild(Definition[5], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E0_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE0, "cs_rule_context<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E0_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE0, "cs_rule_context<grammar_text_terminal>"));
            nodes.Add(SubRoot);
        }
        private static void Production_E1_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 7, 7);
            nodes.RemoveRange(nodes.Count - 7, 7);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE1, "cs_rule_template<grammar_text_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[5]);
            SubRoot.AppendChild(Definition[6], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E2_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_E2_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_E2_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_E3_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE3, "grammar_cs_rules<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E4_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 6, 6);
            nodes.RemoveRange(nodes.Count - 6, 6);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE4, "cs_rule_simple<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[4]);
            SubRoot.AppendChild(Definition[5], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E5_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE5, "cs_rule_context<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E5_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE5, "cs_rule_context<grammar_bin_terminal>"));
            nodes.Add(SubRoot);
        }
        private static void Production_E6_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 7, 7);
            nodes.RemoveRange(nodes.Count - 7, 7);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE6, "cs_rule_template<grammar_bin_terminal>"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            SubRoot.AppendChild(Definition[4], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            SubRoot.AppendChild(Definition[5]);
            SubRoot.AppendChild(Definition[6], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static void Production_E7_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_E7_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_E7_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_E8_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE8, "file_item"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            nodes.Add(SubRoot);
        }
        private static void Production_E9_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xE9, "file"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_EA_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xEA, "_m234"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            nodes.Add(SubRoot);
        }
        private static void Production_EA_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xEA, "_m234"), Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace);
            nodes.Add(SubRoot);
        }
        private static void Production_EB_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xEB, "_Axiom_"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Production[] staticRules = { Production_13_0, Production_14_0, Production_15_0, Production_16_0, Production_17_0, Production_18_0, Production_18_1, Production_18_2, Production_18_3, Production_18_4, Production_19_0, Production_1A_0, Production_1B_0, Production_1B_1, Production_1C_0, Production_1C_1, Production_57_0, Production_58_0, Production_59_0, Production_59_1, Production_5A_0, Production_5B_0, Production_5C_0, Production_5D_0, Production_5E_0, Production_5F_0, Production_5F_1, Production_5F_2, Production_5F_3, Production_5F_4, Production_5F_5, Production_5F_6, Production_5F_7, Production_60_0, Production_60_1, Production_61_0, Production_61_1, Production_61_2, Production_61_3, Production_61_4, Production_62_0, Production_62_1, Production_63_0, Production_64_0, Production_65_0, Production_66_0, Production_66_1, Production_67_0, Production_68_0, Production_69_0, Production_6A_0, Production_6B_0, Production_6C_0, Production_6C_1, Production_6C_2, Production_6C_3, Production_6C_4, Production_6C_5, Production_6C_6, Production_6C_7, Production_6C_8, Production_6C_9, Production_6C_A, Production_6C_B, Production_6D_0, Production_6E_0, Production_6F_0, Production_70_0, Production_70_1, Production_71_0, Production_71_1, Production_71_2, Production_71_3, Production_72_0, Production_73_0, Production_74_0, Production_74_1, Production_75_0, Production_75_1, Production_76_0, Production_76_1, Production_77_0, Production_77_1, Production_78_0, Production_78_1, Production_79_0, Production_79_1, Production_7A_0, Production_7A_1, Production_7B_0, Production_7C_0, Production_7D_0, Production_7E_0, Production_7E_1, Production_7F_0, Production_80_0, Production_81_0, Production_81_1, Production_81_2, Production_81_3, Production_82_0, Production_82_1, Production_82_2, Production_83_0, Production_83_1, Production_84_0, Production_84_1, Production_84_2, Production_84_3, Production_84_4, Production_85_0, Production_86_0, Production_87_0, Production_87_1, Production_88_0, Production_88_1, Production_89_0, Production_89_1, Production_8A_0, Production_8A_1, Production_8B_0, Production_8C_0, Production_8C_1, Production_8C_2, Production_8D_0, Production_8E_0, Production_8F_0, Production_90_0, Production_90_1, Production_91_0, Production_92_0, Production_93_0, Production_93_1, Production_93_2, Production_93_3, Production_94_0, Production_94_1, Production_94_2, Production_95_0, Production_95_1, Production_96_0, Production_96_1, Production_96_2, Production_96_3, Production_96_4, Production_97_0, Production_98_0, Production_99_0, Production_99_1, Production_9A_0, Production_9A_1, Production_9B_0, Production_9B_1, Production_9C_0, Production_9C_1, Production_9D_0, Production_9E_0, Production_9E_1, Production_9E_2, Production_DC_0, Production_DD_0, Production_DE_0, Production_DF_0, Production_E0_0, Production_E0_1, Production_E1_0, Production_E2_0, Production_E2_1, Production_E2_2, Production_E3_0, Production_E4_0, Production_E5_0, Production_E5_1, Production_E6_0, Production_E7_0, Production_E7_1, Production_E7_2, Production_E8_0, Production_E9_0, Production_EA_0, Production_EA_1, Production_EB_0 };
        private static ushort[] staticRulesHeadID = { 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x18, 0x18, 0x18, 0x18, 0x19, 0x1A, 0x1B, 0x1B, 0x1C, 0x1C, 0x57, 0x58, 0x59, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x5F, 0x5F, 0x5F, 0x5F, 0x5F, 0x5F, 0x5F, 0x60, 0x60, 0x61, 0x61, 0x61, 0x61, 0x61, 0x62, 0x62, 0x63, 0x64, 0x65, 0x66, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x70, 0x71, 0x71, 0x71, 0x71, 0x72, 0x73, 0x74, 0x74, 0x75, 0x75, 0x76, 0x76, 0x77, 0x77, 0x78, 0x78, 0x79, 0x79, 0x7A, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7E, 0x7F, 0x80, 0x81, 0x81, 0x81, 0x81, 0x82, 0x82, 0x82, 0x83, 0x83, 0x84, 0x84, 0x84, 0x84, 0x84, 0x85, 0x86, 0x87, 0x87, 0x88, 0x88, 0x89, 0x89, 0x8A, 0x8A, 0x8B, 0x8C, 0x8C, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x90, 0x91, 0x92, 0x93, 0x93, 0x93, 0x93, 0x94, 0x94, 0x94, 0x95, 0x95, 0x96, 0x96, 0x96, 0x96, 0x96, 0x97, 0x98, 0x99, 0x99, 0x9A, 0x9A, 0x9B, 0x9B, 0x9C, 0x9C, 0x9D, 0x9E, 0x9E, 0x9E, 0xDC, 0xDD, 0xDE, 0xDF, 0xE0, 0xE0, 0xE1, 0xE2, 0xE2, 0xE2, 0xE3, 0xE4, 0xE5, 0xE5, 0xE6, 0xE7, 0xE7, 0xE7, 0xE8, 0xE9, 0xEA, 0xEA, 0xEB };
        private static string[] staticRulesHeadName = { "qualified_name", "symbol_access_public", "symbol_access_private", "symbol_access_protected", "symbol_access_internal", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_content", "Namespace", "_m20", "_m20", "_m25", "_m25", "option", "terminal_def_atom_any", "terminal_def_atom_unicode", "terminal_def_atom_unicode", "terminal_def_atom_text", "terminal_def_atom_set", "terminal_def_atom_ublock", "terminal_def_atom_ucat", "terminal_def_atom_span", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_element", "terminal_def_element", "terminal_def_cardinalilty", "terminal_def_cardinalilty", "terminal_def_cardinalilty", "terminal_def_cardinalilty", "terminal_def_cardinalilty", "terminal_def_repetition", "terminal_def_repetition", "terminal_def_fragment", "terminal_def_restrict", "terminal_definition", "terminal_subgrammar", "terminal_subgrammar", "terminal", "rule_sym_action", "rule_sym_virtual", "rule_sym_ref_simple", "rule_template_params", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_text_terminal", "grammar_options", "grammar_terminals", "grammar_parency", "grammar_parency", "grammar_access", "grammar_access", "grammar_access", "grammar_access", "cf_grammar_text", "cf_grammar_bin", "_m89", "_m89", "_m91", "_m91", "_m93", "_m93", "_m101", "_m101", "_m105", "_m105", "_m109", "_m109", "_m113", "_m113", "grammar_cf_rules<grammar_text_terminal>", "cf_rule_simple<grammar_text_terminal>", "rule_definition<grammar_text_terminal>", "rule_def_choice<grammar_text_terminal>", "rule_def_choice<grammar_text_terminal>", "rule_def_restrict<grammar_text_terminal>", "rule_def_fragment<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_element<grammar_text_terminal>", "rule_def_element<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_sym_ref_template<grammar_text_terminal>", "rule_sym_ref_params<grammar_text_terminal>", "_m134", "_m134", "_m143", "_m143", "_m145", "_m145", "_m147", "_m147", "cf_rule_template<grammar_text_terminal>", "_m152", "_m152", "_m152", "grammar_cf_rules<grammar_bin_terminal>", "cf_rule_simple<grammar_bin_terminal>", "rule_definition<grammar_bin_terminal>", "rule_def_choice<grammar_bin_terminal>", "rule_def_choice<grammar_bin_terminal>", "rule_def_restrict<grammar_bin_terminal>", "rule_def_fragment<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_element<grammar_bin_terminal>", "rule_def_element<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_sym_ref_template<grammar_bin_terminal>", "rule_sym_ref_params<grammar_bin_terminal>", "_m175", "_m175", "_m184", "_m184", "_m186", "_m186", "_m188", "_m188", "cf_rule_template<grammar_bin_terminal>", "_m193", "_m193", "_m193", "cs_grammar_text", "cs_grammar_bin", "grammar_cs_rules<grammar_text_terminal>", "cs_rule_simple<grammar_text_terminal>", "cs_rule_context<grammar_text_terminal>", "cs_rule_context<grammar_text_terminal>", "cs_rule_template<grammar_text_terminal>", "_m160", "_m160", "_m160", "grammar_cs_rules<grammar_bin_terminal>", "cs_rule_simple<grammar_bin_terminal>", "cs_rule_context<grammar_bin_terminal>", "cs_rule_context<grammar_bin_terminal>", "cs_rule_template<grammar_bin_terminal>", "_m178", "_m178", "_m178", "file_item", "file", "_m234", "_m234", "_Axiom_" };
        private static ushort[] staticRulesParserLength = { 0x2, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x5, 0x3, 0x0, 0x2, 0x0, 0x4, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x3, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x3, 0x1, 0x1, 0x1, 0x5, 0x3, 0x2, 0x1, 0x2, 0x2, 0x2, 0x2, 0x0, 0x5, 0x3, 0x1, 0x1, 0x4, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x4, 0x4, 0x3, 0x0, 0x1, 0x1, 0x1, 0x1, 0xA, 0x9, 0x2, 0x0, 0x3, 0x0, 0x3, 0x0, 0x3, 0x0, 0x2, 0x0, 0x2, 0x0, 0x3, 0x0, 0x4, 0x4, 0x2, 0x1, 0x0, 0x2, 0x2, 0x2, 0x2, 0x2, 0x1, 0x2, 0x2, 0x1, 0x1, 0x3, 0x1, 0x1, 0x1, 0x1, 0x1, 0x2, 0x4, 0x3, 0x0, 0x2, 0x0, 0x3, 0x0, 0x3, 0x0, 0x5, 0x2, 0x2, 0x0, 0x4, 0x4, 0x2, 0x1, 0x0, 0x2, 0x2, 0x2, 0x2, 0x2, 0x1, 0x2, 0x2, 0x1, 0x1, 0x3, 0x1, 0x1, 0x1, 0x1, 0x1, 0x2, 0x4, 0x3, 0x0, 0x2, 0x0, 0x3, 0x0, 0x3, 0x0, 0x5, 0x2, 0x2, 0x0, 0x9, 0x8, 0x4, 0x6, 0x3, 0x0, 0x7, 0x2, 0x2, 0x0, 0x4, 0x6, 0x3, 0x0, 0x7, 0x2, 0x2, 0x0, 0x1, 0x2, 0x2, 0x0, 0x2 };
        private static ushort[] stateExpectedIDs_0 = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x52 };
        private static string[] stateExpectedNames_0 = { "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[grammar]" };
        private static string[] stateItems_0 = { "[_Axiom_ → • file $]", "[file → • file_item _m234]", "[file_item → • Namespace_child_symbol]", "[Namespace_child_symbol → • Namespace]", "[Namespace_child_symbol → • cf_grammar_text]", "[Namespace_child_symbol → • cf_grammar_bin]", "[Namespace_child_symbol → • cs_grammar_text]", "[Namespace_child_symbol → • cs_grammar_bin]", "[Namespace → • namespace qualified_name { Namespace_content }]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access → • symbol_access_public]", "[grammar_access → • symbol_access_private]", "[grammar_access → • symbol_access_protected]", "[grammar_access → • symbol_access_internal]", "[symbol_access_public → • public]", "[symbol_access_private → • private]", "[symbol_access_protected → • protected]", "[symbol_access_internal → • internal]" };
        private static ushort[][] stateShiftsOnTerminal_0 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x52, 0xB }, new ushort[2] { 0xc, 0x10 }, new ushort[2] { 0xd, 0x11 }, new ushort[2] { 0xe, 0x12 }, new ushort[2] { 0xf, 0x13 } };
        private static ushort[][] stateShiftsOnVariable_0 = { new ushort[2] { 0xe9, 0x1 }, new ushort[2] { 0xe8, 0x2 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1a, 0x4 }, new ushort[2] { 0x72, 0x5 }, new ushort[2] { 0x73, 0x6 }, new ushort[2] { 0xdc, 0x7 }, new ushort[2] { 0xdd, 0x8 }, new ushort[2] { 0x71, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] stateReducsOnTerminal_0 = {  };
        private static ushort[] stateExpectedIDs_1 = { 0x2 };
        private static string[] stateExpectedNames_1 = { "$" };
        private static string[] stateItems_1 = { "[_Axiom_ → file • $]" };
        private static ushort[][] stateShiftsOnTerminal_1 = { new ushort[2] { 0x2, 0x14 } };
        private static ushort[][] stateShiftsOnVariable_1 = {  };
        private static ushort[][] stateReducsOnTerminal_1 = {  };
        private static ushort[] stateExpectedIDs_2 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x52 };
        private static string[] stateExpectedNames_2 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[grammar]" };
        private static string[] stateItems_2 = { "[file → file_item • _m234]", "[_m234 → • file_item _m234]", "[_m234 → •]", "[file_item → • Namespace_child_symbol]", "[Namespace_child_symbol → • Namespace]", "[Namespace_child_symbol → • cf_grammar_text]", "[Namespace_child_symbol → • cf_grammar_bin]", "[Namespace_child_symbol → • cs_grammar_text]", "[Namespace_child_symbol → • cs_grammar_bin]", "[Namespace → • namespace qualified_name { Namespace_content }]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access → • symbol_access_public]", "[grammar_access → • symbol_access_private]", "[grammar_access → • symbol_access_protected]", "[grammar_access → • symbol_access_internal]", "[symbol_access_public → • public]", "[symbol_access_private → • private]", "[symbol_access_protected → • protected]", "[symbol_access_internal → • internal]" };
        private static ushort[][] stateShiftsOnTerminal_2 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x52, 0xB }, new ushort[2] { 0xc, 0x10 }, new ushort[2] { 0xd, 0x11 }, new ushort[2] { 0xe, 0x12 }, new ushort[2] { 0xf, 0x13 } };
        private static ushort[][] stateShiftsOnVariable_2 = { new ushort[2] { 0xea, 0x15 }, new ushort[2] { 0xe8, 0x16 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1a, 0x4 }, new ushort[2] { 0x72, 0x5 }, new ushort[2] { 0x73, 0x6 }, new ushort[2] { 0xdc, 0x7 }, new ushort[2] { 0xdd, 0x8 }, new ushort[2] { 0x71, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] stateReducsOnTerminal_2 = { new ushort[2] { 0x2, 0xB4 } };
        private static ushort[] stateExpectedIDs_3 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x52 };
        private static string[] stateExpectedNames_3 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[grammar]" };
        private static string[] stateItems_3 = { "[file_item → Namespace_child_symbol •]" };
        private static ushort[][] stateShiftsOnTerminal_3 = {  };
        private static ushort[][] stateShiftsOnVariable_3 = {  };
        private static ushort[][] stateReducsOnTerminal_3 = { new ushort[2] { 0x2, 0xB1 }, new ushort[2] { 0xc, 0xB1 }, new ushort[2] { 0xd, 0xB1 }, new ushort[2] { 0xe, 0xB1 }, new ushort[2] { 0xf, 0xB1 }, new ushort[2] { 0x10, 0xB1 }, new ushort[2] { 0x52, 0xB1 } };
        private static ushort[] stateExpectedIDs_4 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_4 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_4 = { "[Namespace_child_symbol → Namespace •]" };
        private static ushort[][] stateShiftsOnTerminal_4 = {  };
        private static ushort[][] stateShiftsOnVariable_4 = {  };
        private static ushort[][] stateReducsOnTerminal_4 = { new ushort[2] { 0x2, 0x5 }, new ushort[2] { 0xc, 0x5 }, new ushort[2] { 0xd, 0x5 }, new ushort[2] { 0xe, 0x5 }, new ushort[2] { 0xf, 0x5 }, new ushort[2] { 0x10, 0x5 }, new ushort[2] { 0x12, 0x5 }, new ushort[2] { 0x52, 0x5 } };
        private static ushort[] stateExpectedIDs_5 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_5 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_5 = { "[Namespace_child_symbol → cf_grammar_text •]" };
        private static ushort[][] stateShiftsOnTerminal_5 = {  };
        private static ushort[][] stateShiftsOnVariable_5 = {  };
        private static ushort[][] stateReducsOnTerminal_5 = { new ushort[2] { 0x2, 0x6 }, new ushort[2] { 0xc, 0x6 }, new ushort[2] { 0xd, 0x6 }, new ushort[2] { 0xe, 0x6 }, new ushort[2] { 0xf, 0x6 }, new ushort[2] { 0x10, 0x6 }, new ushort[2] { 0x12, 0x6 }, new ushort[2] { 0x52, 0x6 } };
        private static ushort[] stateExpectedIDs_6 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_6 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_6 = { "[Namespace_child_symbol → cf_grammar_bin •]" };
        private static ushort[][] stateShiftsOnTerminal_6 = {  };
        private static ushort[][] stateShiftsOnVariable_6 = {  };
        private static ushort[][] stateReducsOnTerminal_6 = { new ushort[2] { 0x2, 0x7 }, new ushort[2] { 0xc, 0x7 }, new ushort[2] { 0xd, 0x7 }, new ushort[2] { 0xe, 0x7 }, new ushort[2] { 0xf, 0x7 }, new ushort[2] { 0x10, 0x7 }, new ushort[2] { 0x12, 0x7 }, new ushort[2] { 0x52, 0x7 } };
        private static ushort[] stateExpectedIDs_7 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_7 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_7 = { "[Namespace_child_symbol → cs_grammar_text •]" };
        private static ushort[][] stateShiftsOnTerminal_7 = {  };
        private static ushort[][] stateShiftsOnVariable_7 = {  };
        private static ushort[][] stateReducsOnTerminal_7 = { new ushort[2] { 0x2, 0x8 }, new ushort[2] { 0xc, 0x8 }, new ushort[2] { 0xd, 0x8 }, new ushort[2] { 0xe, 0x8 }, new ushort[2] { 0xf, 0x8 }, new ushort[2] { 0x10, 0x8 }, new ushort[2] { 0x12, 0x8 }, new ushort[2] { 0x52, 0x8 } };
        private static ushort[] stateExpectedIDs_8 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_8 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_8 = { "[Namespace_child_symbol → cs_grammar_bin •]" };
        private static ushort[][] stateShiftsOnTerminal_8 = {  };
        private static ushort[][] stateShiftsOnVariable_8 = {  };
        private static ushort[][] stateReducsOnTerminal_8 = { new ushort[2] { 0x2, 0x9 }, new ushort[2] { 0xc, 0x9 }, new ushort[2] { 0xd, 0x9 }, new ushort[2] { 0xe, 0x9 }, new ushort[2] { 0xf, 0x9 }, new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x12, 0x9 }, new ushort[2] { 0x52, 0x9 } };
        private static ushort[] stateExpectedIDs_9 = { 0xA };
        private static string[] stateExpectedNames_9 = { "NAME" };
        private static string[] stateItems_9 = { "[Namespace → namespace • qualified_name { Namespace_content }]", "[qualified_name → • NAME _m20]" };
        private static ushort[][] stateShiftsOnTerminal_9 = { new ushort[2] { 0xa, 0x18 } };
        private static ushort[][] stateShiftsOnVariable_9 = { new ushort[2] { 0x13, 0x17 } };
        private static ushort[][] stateReducsOnTerminal_9 = {  };
        private static ushort[] stateExpectedIDs_A = { 0x52 };
        private static string[] stateExpectedNames_A = { "_T[grammar]" };
        private static string[] stateItems_A = { "[cf_grammar_text → grammar_access • grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access • grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_A = { new ushort[2] { 0x52, 0x19 } };
        private static ushort[][] stateShiftsOnVariable_A = {  };
        private static ushort[][] stateReducsOnTerminal_A = {  };
        private static ushort[] stateExpectedIDs_B = { 0xD9 };
        private static string[] stateExpectedNames_B = { "_T[cs]" };
        private static string[] stateItems_B = { "[cs_grammar_text → grammar • cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar • cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_B = { new ushort[2] { 0xd9, 0x1A } };
        private static ushort[][] stateShiftsOnVariable_B = {  };
        private static ushort[][] stateReducsOnTerminal_B = {  };
        private static ushort[] stateExpectedIDs_C = { 0x52 };
        private static string[] stateExpectedNames_C = { "_T[grammar]" };
        private static string[] stateItems_C = { "[grammar_access → symbol_access_public •]" };
        private static ushort[][] stateShiftsOnTerminal_C = {  };
        private static ushort[][] stateShiftsOnVariable_C = {  };
        private static ushort[][] stateReducsOnTerminal_C = { new ushort[2] { 0x52, 0x45 } };
        private static ushort[] stateExpectedIDs_D = { 0x52 };
        private static string[] stateExpectedNames_D = { "_T[grammar]" };
        private static string[] stateItems_D = { "[grammar_access → symbol_access_private •]" };
        private static ushort[][] stateShiftsOnTerminal_D = {  };
        private static ushort[][] stateShiftsOnVariable_D = {  };
        private static ushort[][] stateReducsOnTerminal_D = { new ushort[2] { 0x52, 0x46 } };
        private static ushort[] stateExpectedIDs_E = { 0x52 };
        private static string[] stateExpectedNames_E = { "_T[grammar]" };
        private static string[] stateItems_E = { "[grammar_access → symbol_access_protected •]" };
        private static ushort[][] stateShiftsOnTerminal_E = {  };
        private static ushort[][] stateShiftsOnVariable_E = {  };
        private static ushort[][] stateReducsOnTerminal_E = { new ushort[2] { 0x52, 0x47 } };
        private static ushort[] stateExpectedIDs_F = { 0x52 };
        private static string[] stateExpectedNames_F = { "_T[grammar]" };
        private static string[] stateItems_F = { "[grammar_access → symbol_access_internal •]" };
        private static ushort[][] stateShiftsOnTerminal_F = {  };
        private static ushort[][] stateShiftsOnVariable_F = {  };
        private static ushort[][] stateReducsOnTerminal_F = { new ushort[2] { 0x52, 0x48 } };
        private static ushort[] stateExpectedIDs_10 = { 0x52 };
        private static string[] stateExpectedNames_10 = { "_T[grammar]" };
        private static string[] stateItems_10 = { "[symbol_access_public → public •]" };
        private static ushort[][] stateShiftsOnTerminal_10 = {  };
        private static ushort[][] stateShiftsOnVariable_10 = {  };
        private static ushort[][] stateReducsOnTerminal_10 = { new ushort[2] { 0x52, 0x1 } };
        private static ushort[] stateExpectedIDs_11 = { 0x52 };
        private static string[] stateExpectedNames_11 = { "_T[grammar]" };
        private static string[] stateItems_11 = { "[symbol_access_private → private •]" };
        private static ushort[][] stateShiftsOnTerminal_11 = {  };
        private static ushort[][] stateShiftsOnVariable_11 = {  };
        private static ushort[][] stateReducsOnTerminal_11 = { new ushort[2] { 0x52, 0x2 } };
        private static ushort[] stateExpectedIDs_12 = { 0x52 };
        private static string[] stateExpectedNames_12 = { "_T[grammar]" };
        private static string[] stateItems_12 = { "[symbol_access_protected → protected •]" };
        private static ushort[][] stateShiftsOnTerminal_12 = {  };
        private static ushort[][] stateShiftsOnVariable_12 = {  };
        private static ushort[][] stateReducsOnTerminal_12 = { new ushort[2] { 0x52, 0x3 } };
        private static ushort[] stateExpectedIDs_13 = { 0x52 };
        private static string[] stateExpectedNames_13 = { "_T[grammar]" };
        private static string[] stateItems_13 = { "[symbol_access_internal → internal •]" };
        private static ushort[][] stateShiftsOnTerminal_13 = {  };
        private static ushort[][] stateShiftsOnVariable_13 = {  };
        private static ushort[][] stateReducsOnTerminal_13 = { new ushort[2] { 0x52, 0x4 } };
        private static ushort[] stateExpectedIDs_14 = { 0x1 };
        private static string[] stateExpectedNames_14 = { "ε" };
        private static string[] stateItems_14 = { "[_Axiom_ → file $ •]" };
        private static ushort[][] stateShiftsOnTerminal_14 = {  };
        private static ushort[][] stateShiftsOnVariable_14 = {  };
        private static ushort[][] stateReducsOnTerminal_14 = { new ushort[2] { 0x1, 0xB5 } };
        private static ushort[] stateExpectedIDs_15 = { 0x2 };
        private static string[] stateExpectedNames_15 = { "$" };
        private static string[] stateItems_15 = { "[file → file_item _m234 •]" };
        private static ushort[][] stateShiftsOnTerminal_15 = {  };
        private static ushort[][] stateShiftsOnVariable_15 = {  };
        private static ushort[][] stateReducsOnTerminal_15 = { new ushort[2] { 0x2, 0xB2 } };
        private static ushort[] stateExpectedIDs_16 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x52 };
        private static string[] stateExpectedNames_16 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[grammar]" };
        private static string[] stateItems_16 = { "[_m234 → file_item • _m234]", "[_m234 → • file_item _m234]", "[_m234 → •]", "[file_item → • Namespace_child_symbol]", "[Namespace_child_symbol → • Namespace]", "[Namespace_child_symbol → • cf_grammar_text]", "[Namespace_child_symbol → • cf_grammar_bin]", "[Namespace_child_symbol → • cs_grammar_text]", "[Namespace_child_symbol → • cs_grammar_bin]", "[Namespace → • namespace qualified_name { Namespace_content }]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access → • symbol_access_public]", "[grammar_access → • symbol_access_private]", "[grammar_access → • symbol_access_protected]", "[grammar_access → • symbol_access_internal]", "[symbol_access_public → • public]", "[symbol_access_private → • private]", "[symbol_access_protected → • protected]", "[symbol_access_internal → • internal]" };
        private static ushort[][] stateShiftsOnTerminal_16 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x52, 0xB }, new ushort[2] { 0xc, 0x10 }, new ushort[2] { 0xd, 0x11 }, new ushort[2] { 0xe, 0x12 }, new ushort[2] { 0xf, 0x13 } };
        private static ushort[][] stateShiftsOnVariable_16 = { new ushort[2] { 0xea, 0x1B }, new ushort[2] { 0xe8, 0x16 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1a, 0x4 }, new ushort[2] { 0x72, 0x5 }, new ushort[2] { 0x73, 0x6 }, new ushort[2] { 0xdc, 0x7 }, new ushort[2] { 0xdd, 0x8 }, new ushort[2] { 0x71, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] stateReducsOnTerminal_16 = { new ushort[2] { 0x2, 0xB4 } };
        private static ushort[] stateExpectedIDs_17 = { 0x11 };
        private static string[] stateExpectedNames_17 = { "_T[{]" };
        private static string[] stateItems_17 = { "[Namespace → namespace qualified_name • { Namespace_content }]" };
        private static ushort[][] stateShiftsOnTerminal_17 = { new ushort[2] { 0x11, 0x1C } };
        private static ushort[][] stateShiftsOnVariable_17 = {  };
        private static ushort[][] stateReducsOnTerminal_17 = {  };
        private static ushort[] stateExpectedIDs_18 = { 0xB, 0x11, 0x41, 0x48 };
        private static string[] stateExpectedNames_18 = { "_T[.]", "_T[{]", "_T[;]", "_T[,]" };
        private static string[] stateItems_18 = { "[qualified_name → NAME • _m20]", "[_m20 → • . NAME _m20]", "[_m20 → •]" };
        private static ushort[][] stateShiftsOnTerminal_18 = { new ushort[2] { 0xb, 0x1E } };
        private static ushort[][] stateShiftsOnVariable_18 = { new ushort[2] { 0x1b, 0x1D } };
        private static ushort[][] stateReducsOnTerminal_18 = { new ushort[2] { 0x11, 0xD }, new ushort[2] { 0x41, 0xD }, new ushort[2] { 0x48, 0xD } };
        private static ushort[] stateExpectedIDs_19 = { 0x53 };
        private static string[] stateExpectedNames_19 = { "_T[cf]" };
        private static string[] stateItems_19 = { "[cf_grammar_text → grammar_access grammar • cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar • cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_19 = { new ushort[2] { 0x53, 0x1F } };
        private static ushort[][] stateShiftsOnVariable_19 = {  };
        private static ushort[][] stateReducsOnTerminal_19 = {  };
        private static ushort[] stateExpectedIDs_1A = { 0xA };
        private static string[] stateExpectedNames_1A = { "NAME" };
        private static string[] stateItems_1A = { "[cs_grammar_text → grammar cs • NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar cs • NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_1A = { new ushort[2] { 0xa, 0x20 } };
        private static ushort[][] stateShiftsOnVariable_1A = {  };
        private static ushort[][] stateReducsOnTerminal_1A = {  };
        private static ushort[] stateExpectedIDs_1B = { 0x2 };
        private static string[] stateExpectedNames_1B = { "$" };
        private static string[] stateItems_1B = { "[_m234 → file_item _m234 •]" };
        private static ushort[][] stateShiftsOnTerminal_1B = {  };
        private static ushort[][] stateShiftsOnVariable_1B = {  };
        private static ushort[][] stateReducsOnTerminal_1B = { new ushort[2] { 0x2, 0xB3 } };
        private static ushort[] stateExpectedIDs_1C = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_1C = { "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_1C = { "[Namespace → namespace qualified_name { • Namespace_content }]", "[Namespace_content → • _m25]", "[_m25 → • Namespace_child_symbol _m25]", "[_m25 → •]", "[Namespace_child_symbol → • Namespace]", "[Namespace_child_symbol → • cf_grammar_text]", "[Namespace_child_symbol → • cf_grammar_bin]", "[Namespace_child_symbol → • cs_grammar_text]", "[Namespace_child_symbol → • cs_grammar_bin]", "[Namespace → • namespace qualified_name { Namespace_content }]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access → • symbol_access_public]", "[grammar_access → • symbol_access_private]", "[grammar_access → • symbol_access_protected]", "[grammar_access → • symbol_access_internal]", "[symbol_access_public → • public]", "[symbol_access_private → • private]", "[symbol_access_protected → • protected]", "[symbol_access_internal → • internal]" };
        private static ushort[][] stateShiftsOnTerminal_1C = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x52, 0xB }, new ushort[2] { 0xc, 0x10 }, new ushort[2] { 0xd, 0x11 }, new ushort[2] { 0xe, 0x12 }, new ushort[2] { 0xf, 0x13 } };
        private static ushort[][] stateShiftsOnVariable_1C = { new ushort[2] { 0x19, 0x21 }, new ushort[2] { 0x1c, 0x22 }, new ushort[2] { 0x18, 0x23 }, new ushort[2] { 0x1a, 0x4 }, new ushort[2] { 0x72, 0x5 }, new ushort[2] { 0x73, 0x6 }, new ushort[2] { 0xdc, 0x7 }, new ushort[2] { 0xdd, 0x8 }, new ushort[2] { 0x71, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] stateReducsOnTerminal_1C = { new ushort[2] { 0x12, 0xF } };
        private static ushort[] stateExpectedIDs_1D = { 0x11, 0x41, 0x48 };
        private static string[] stateExpectedNames_1D = { "_T[{]", "_T[;]", "_T[,]" };
        private static string[] stateItems_1D = { "[qualified_name → NAME _m20 •]" };
        private static ushort[][] stateShiftsOnTerminal_1D = {  };
        private static ushort[][] stateShiftsOnVariable_1D = {  };
        private static ushort[][] stateReducsOnTerminal_1D = { new ushort[2] { 0x11, 0x0 }, new ushort[2] { 0x41, 0x0 }, new ushort[2] { 0x48, 0x0 } };
        private static ushort[] stateExpectedIDs_1E = { 0xA };
        private static string[] stateExpectedNames_1E = { "NAME" };
        private static string[] stateItems_1E = { "[_m20 → . • NAME _m20]" };
        private static ushort[][] stateShiftsOnTerminal_1E = { new ushort[2] { 0xa, 0x24 } };
        private static ushort[][] stateShiftsOnVariable_1E = {  };
        private static ushort[][] stateReducsOnTerminal_1E = {  };
        private static ushort[] stateExpectedIDs_1F = { 0xA };
        private static string[] stateExpectedNames_1F = { "NAME" };
        private static string[] stateItems_1F = { "[cf_grammar_text → grammar_access grammar cf • NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar cf • NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_1F = { new ushort[2] { 0xa, 0x25 } };
        private static ushort[][] stateShiftsOnVariable_1F = {  };
        private static ushort[][] stateReducsOnTerminal_1F = {  };
        private static ushort[] stateExpectedIDs_20 = { 0x11, 0x51 };
        private static string[] stateExpectedNames_20 = { "_T[{]", "_T[:]" };
        private static string[] stateItems_20 = { "[cs_grammar_text → grammar cs NAME • grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar cs NAME • grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_parency → • : qualified_name _m113]", "[grammar_parency → •]" };
        private static ushort[][] stateShiftsOnTerminal_20 = { new ushort[2] { 0x51, 0x27 } };
        private static ushort[][] stateShiftsOnVariable_20 = { new ushort[2] { 0x70, 0x26 } };
        private static ushort[][] stateReducsOnTerminal_20 = { new ushort[2] { 0x11, 0x44 } };
        private static ushort[] stateExpectedIDs_21 = { 0x12 };
        private static string[] stateExpectedNames_21 = { "_T[}]" };
        private static string[] stateItems_21 = { "[Namespace → namespace qualified_name { Namespace_content • }]" };
        private static ushort[][] stateShiftsOnTerminal_21 = { new ushort[2] { 0x12, 0x28 } };
        private static ushort[][] stateShiftsOnVariable_21 = {  };
        private static ushort[][] stateReducsOnTerminal_21 = {  };
        private static ushort[] stateExpectedIDs_22 = { 0x12 };
        private static string[] stateExpectedNames_22 = { "_T[}]" };
        private static string[] stateItems_22 = { "[Namespace_content → _m25 •]" };
        private static ushort[][] stateShiftsOnTerminal_22 = {  };
        private static ushort[][] stateShiftsOnVariable_22 = {  };
        private static ushort[][] stateReducsOnTerminal_22 = { new ushort[2] { 0x12, 0xA } };
        private static ushort[] stateExpectedIDs_23 = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_23 = { "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_23 = { "[_m25 → Namespace_child_symbol • _m25]", "[_m25 → • Namespace_child_symbol _m25]", "[_m25 → •]", "[Namespace_child_symbol → • Namespace]", "[Namespace_child_symbol → • cf_grammar_text]", "[Namespace_child_symbol → • cf_grammar_bin]", "[Namespace_child_symbol → • cs_grammar_text]", "[Namespace_child_symbol → • cs_grammar_bin]", "[Namespace → • namespace qualified_name { Namespace_content }]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access → • symbol_access_public]", "[grammar_access → • symbol_access_private]", "[grammar_access → • symbol_access_protected]", "[grammar_access → • symbol_access_internal]", "[symbol_access_public → • public]", "[symbol_access_private → • private]", "[symbol_access_protected → • protected]", "[symbol_access_internal → • internal]" };
        private static ushort[][] stateShiftsOnTerminal_23 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x52, 0xB }, new ushort[2] { 0xc, 0x10 }, new ushort[2] { 0xd, 0x11 }, new ushort[2] { 0xe, 0x12 }, new ushort[2] { 0xf, 0x13 } };
        private static ushort[][] stateShiftsOnVariable_23 = { new ushort[2] { 0x1c, 0x29 }, new ushort[2] { 0x18, 0x23 }, new ushort[2] { 0x1a, 0x4 }, new ushort[2] { 0x72, 0x5 }, new ushort[2] { 0x73, 0x6 }, new ushort[2] { 0xdc, 0x7 }, new ushort[2] { 0xdd, 0x8 }, new ushort[2] { 0x71, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] stateReducsOnTerminal_23 = { new ushort[2] { 0x12, 0xF } };
        private static ushort[] stateExpectedIDs_24 = { 0xB, 0x11, 0x41, 0x48 };
        private static string[] stateExpectedNames_24 = { "_T[.]", "_T[{]", "_T[;]", "_T[,]" };
        private static string[] stateItems_24 = { "[_m20 → . NAME • _m20]", "[_m20 → • . NAME _m20]", "[_m20 → •]" };
        private static ushort[][] stateShiftsOnTerminal_24 = { new ushort[2] { 0xb, 0x1E } };
        private static ushort[][] stateShiftsOnVariable_24 = { new ushort[2] { 0x1b, 0x2A } };
        private static ushort[][] stateReducsOnTerminal_24 = { new ushort[2] { 0x11, 0xD }, new ushort[2] { 0x41, 0xD }, new ushort[2] { 0x48, 0xD } };
        private static ushort[] stateExpectedIDs_25 = { 0x11, 0x51 };
        private static string[] stateExpectedNames_25 = { "_T[{]", "_T[:]" };
        private static string[] stateItems_25 = { "[cf_grammar_text → grammar_access grammar cf NAME • grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar cf NAME • grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_parency → • : qualified_name _m113]", "[grammar_parency → •]" };
        private static ushort[][] stateShiftsOnTerminal_25 = { new ushort[2] { 0x51, 0x27 } };
        private static ushort[][] stateShiftsOnVariable_25 = { new ushort[2] { 0x70, 0x2B } };
        private static ushort[][] stateReducsOnTerminal_25 = { new ushort[2] { 0x11, 0x44 } };
        private static ushort[] stateExpectedIDs_26 = { 0x11 };
        private static string[] stateExpectedNames_26 = { "_T[{]" };
        private static string[] stateItems_26 = { "[cs_grammar_text → grammar cs NAME grammar_parency • { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar cs NAME grammar_parency • { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_26 = { new ushort[2] { 0x11, 0x2C } };
        private static ushort[][] stateShiftsOnVariable_26 = {  };
        private static ushort[][] stateReducsOnTerminal_26 = {  };
        private static ushort[] stateExpectedIDs_27 = { 0xA };
        private static string[] stateExpectedNames_27 = { "NAME" };
        private static string[] stateItems_27 = { "[grammar_parency → : • qualified_name _m113]", "[qualified_name → • NAME _m20]" };
        private static ushort[][] stateShiftsOnTerminal_27 = { new ushort[2] { 0xa, 0x18 } };
        private static ushort[][] stateShiftsOnVariable_27 = { new ushort[2] { 0x13, 0x2D } };
        private static ushort[][] stateReducsOnTerminal_27 = {  };
        private static ushort[] stateExpectedIDs_28 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_28 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_28 = { "[Namespace → namespace qualified_name { Namespace_content } •]" };
        private static ushort[][] stateShiftsOnTerminal_28 = {  };
        private static ushort[][] stateShiftsOnVariable_28 = {  };
        private static ushort[][] stateReducsOnTerminal_28 = { new ushort[2] { 0x2, 0xB }, new ushort[2] { 0xc, 0xB }, new ushort[2] { 0xd, 0xB }, new ushort[2] { 0xe, 0xB }, new ushort[2] { 0xf, 0xB }, new ushort[2] { 0x10, 0xB }, new ushort[2] { 0x12, 0xB }, new ushort[2] { 0x52, 0xB } };
        private static ushort[] stateExpectedIDs_29 = { 0x12 };
        private static string[] stateExpectedNames_29 = { "_T[}]" };
        private static string[] stateItems_29 = { "[_m25 → Namespace_child_symbol _m25 •]" };
        private static ushort[][] stateShiftsOnTerminal_29 = {  };
        private static ushort[][] stateShiftsOnVariable_29 = {  };
        private static ushort[][] stateReducsOnTerminal_29 = { new ushort[2] { 0x12, 0xE } };
        private static ushort[] stateExpectedIDs_2A = { 0x11, 0x41, 0x48 };
        private static string[] stateExpectedNames_2A = { "_T[{]", "_T[;]", "_T[,]" };
        private static string[] stateItems_2A = { "[_m20 → . NAME _m20 •]" };
        private static ushort[][] stateShiftsOnTerminal_2A = {  };
        private static ushort[][] stateShiftsOnVariable_2A = {  };
        private static ushort[][] stateReducsOnTerminal_2A = { new ushort[2] { 0x11, 0xC }, new ushort[2] { 0x41, 0xC }, new ushort[2] { 0x48, 0xC } };
        private static ushort[] stateExpectedIDs_2B = { 0x11 };
        private static string[] stateExpectedNames_2B = { "_T[{]" };
        private static string[] stateItems_2B = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency • { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency • { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] stateShiftsOnTerminal_2B = { new ushort[2] { 0x11, 0x2E } };
        private static ushort[][] stateShiftsOnVariable_2B = {  };
        private static ushort[][] stateReducsOnTerminal_2B = {  };
        private static ushort[] stateExpectedIDs_2C = { 0x4F };
        private static string[] stateExpectedNames_2C = { "_T[options]" };
        private static string[] stateItems_2C = { "[cs_grammar_text → grammar cs NAME grammar_parency { • grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar cs NAME grammar_parency { • grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_options → • options { _m105 }]" };
        private static ushort[][] stateShiftsOnTerminal_2C = { new ushort[2] { 0x4f, 0x30 } };
        private static ushort[][] stateShiftsOnVariable_2C = { new ushort[2] { 0x6e, 0x2F } };
        private static ushort[][] stateReducsOnTerminal_2C = {  };
        private static ushort[] stateExpectedIDs_2D = { 0x11, 0x48 };
        private static string[] stateExpectedNames_2D = { "_T[{]", "_T[,]" };
        private static string[] stateItems_2D = { "[grammar_parency → : qualified_name • _m113]", "[_m113 → • , qualified_name _m113]", "[_m113 → •]" };
        private static ushort[][] stateShiftsOnTerminal_2D = { new ushort[2] { 0x48, 0x32 } };
        private static ushort[][] stateShiftsOnVariable_2D = { new ushort[2] { 0x7a, 0x31 } };
        private static ushort[][] stateReducsOnTerminal_2D = { new ushort[2] { 0x11, 0x58 } };
        private static ushort[] stateExpectedIDs_2E = { 0x4F };
        private static string[] stateExpectedNames_2E = { "_T[options]" };
        private static string[] stateItems_2E = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { • grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { • grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_options → • options { _m105 }]" };
        private static ushort[][] stateShiftsOnTerminal_2E = { new ushort[2] { 0x4f, 0x30 } };
        private static ushort[][] stateShiftsOnVariable_2E = { new ushort[2] { 0x6e, 0x33 } };
        private static ushort[][] stateReducsOnTerminal_2E = {  };
        private static ushort[] stateExpectedIDs_2F = { 0x50, 0x54 };
        private static string[] stateExpectedNames_2F = { "_T[terminals]", "_T[rules]" };
        private static string[] stateItems_2F = { "[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options • grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options • grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_terminals → • terminals { _m109 }]", "[grammar_cs_rules<grammar_bin_terminal> → • rules { _m178 }]" };
        private static ushort[][] stateShiftsOnTerminal_2F = { new ushort[2] { 0x50, 0x36 }, new ushort[2] { 0x54, 0x37 } };
        private static ushort[][] stateShiftsOnVariable_2F = { new ushort[2] { 0x6f, 0x34 }, new ushort[2] { 0xe3, 0x35 } };
        private static ushort[][] stateReducsOnTerminal_2F = {  };
        private static ushort[] stateExpectedIDs_30 = { 0x11 };
        private static string[] stateExpectedNames_30 = { "_T[{]" };
        private static string[] stateItems_30 = { "[grammar_options → options • { _m105 }]" };
        private static ushort[][] stateShiftsOnTerminal_30 = { new ushort[2] { 0x11, 0x38 } };
        private static ushort[][] stateShiftsOnVariable_30 = {  };
        private static ushort[][] stateReducsOnTerminal_30 = {  };
        private static ushort[] stateExpectedIDs_31 = { 0x11 };
        private static string[] stateExpectedNames_31 = { "_T[{]" };
        private static string[] stateItems_31 = { "[grammar_parency → : qualified_name _m113 •]" };
        private static ushort[][] stateShiftsOnTerminal_31 = {  };
        private static ushort[][] stateShiftsOnVariable_31 = {  };
        private static ushort[][] stateReducsOnTerminal_31 = { new ushort[2] { 0x11, 0x43 } };
        private static ushort[] stateExpectedIDs_32 = { 0xA };
        private static string[] stateExpectedNames_32 = { "NAME" };
        private static string[] stateItems_32 = { "[_m113 → , • qualified_name _m113]", "[qualified_name → • NAME _m20]" };
        private static ushort[][] stateShiftsOnTerminal_32 = { new ushort[2] { 0xa, 0x18 } };
        private static ushort[][] stateShiftsOnVariable_32 = { new ushort[2] { 0x13, 0x39 } };
        private static ushort[][] stateReducsOnTerminal_32 = {  };
        private static ushort[] stateExpectedIDs_33 = { 0x50, 0x54 };
        private static string[] stateExpectedNames_33 = { "_T[terminals]", "_T[rules]" };
        private static string[] stateItems_33 = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options • grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options • grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_terminals → • terminals { _m109 }]", "[grammar_cf_rules<grammar_bin_terminal> → • rules { _m193 }]" };
        private static ushort[][] stateShiftsOnTerminal_33 = { new ushort[2] { 0x50, 0x36 }, new ushort[2] { 0x54, 0x3C } };
        private static ushort[][] stateShiftsOnVariable_33 = { new ushort[2] { 0x6f, 0x3A }, new ushort[2] { 0x8d, 0x3B } };
        private static ushort[][] stateReducsOnTerminal_33 = {  };
        private static ushort[] stateExpectedIDs_34 = { 0x54 };
        private static string[] stateExpectedNames_34 = { "_T[rules]" };
        private static string[] stateItems_34 = { "[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals • grammar_cs_rules<grammar_text_terminal> }]", "[grammar_cs_rules<grammar_text_terminal> → • rules { _m160 }]" };
        private static ushort[][] stateShiftsOnTerminal_34 = { new ushort[2] { 0x54, 0x3E } };
        private static ushort[][] stateShiftsOnVariable_34 = { new ushort[2] { 0xde, 0x3D } };
        private static ushort[][] stateReducsOnTerminal_34 = {  };
        private static ushort[] stateExpectedIDs_35 = { 0x12 };
        private static string[] stateExpectedNames_35 = { "_T[}]" };
        private static string[] stateItems_35 = { "[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> • }]" };
        private static ushort[][] stateShiftsOnTerminal_35 = { new ushort[2] { 0x12, 0x3F } };
        private static ushort[][] stateShiftsOnVariable_35 = {  };
        private static ushort[][] stateReducsOnTerminal_35 = {  };
        private static ushort[] stateExpectedIDs_36 = { 0x11 };
        private static string[] stateExpectedNames_36 = { "_T[{]" };
        private static string[] stateItems_36 = { "[grammar_terminals → terminals • { _m109 }]" };
        private static ushort[][] stateShiftsOnTerminal_36 = { new ushort[2] { 0x11, 0x40 } };
        private static ushort[][] stateShiftsOnVariable_36 = {  };
        private static ushort[][] stateReducsOnTerminal_36 = {  };
        private static ushort[] stateExpectedIDs_37 = { 0x11 };
        private static string[] stateExpectedNames_37 = { "_T[{]" };
        private static string[] stateItems_37 = { "[grammar_cs_rules<grammar_bin_terminal> → rules • { _m178 }]" };
        private static ushort[][] stateShiftsOnTerminal_37 = { new ushort[2] { 0x11, 0x41 } };
        private static ushort[][] stateShiftsOnVariable_37 = {  };
        private static ushort[][] stateReducsOnTerminal_37 = {  };
        private static ushort[] stateExpectedIDs_38 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_38 = { "NAME", "_T[}]" };
        private static string[] stateItems_38 = { "[grammar_options → options { • _m105 }]", "[_m105 → • option _m105]", "[_m105 → •]", "[option → • NAME = QUOTED_DATA ;]" };
        private static ushort[][] stateShiftsOnTerminal_38 = { new ushort[2] { 0xa, 0x44 } };
        private static ushort[][] stateShiftsOnVariable_38 = { new ushort[2] { 0x78, 0x42 }, new ushort[2] { 0x57, 0x43 } };
        private static ushort[][] stateReducsOnTerminal_38 = { new ushort[2] { 0x12, 0x54 } };
        private static ushort[] stateExpectedIDs_39 = { 0x11, 0x48 };
        private static string[] stateExpectedNames_39 = { "_T[{]", "_T[,]" };
        private static string[] stateItems_39 = { "[_m113 → , qualified_name • _m113]", "[_m113 → • , qualified_name _m113]", "[_m113 → •]" };
        private static ushort[][] stateShiftsOnTerminal_39 = { new ushort[2] { 0x48, 0x32 } };
        private static ushort[][] stateShiftsOnVariable_39 = { new ushort[2] { 0x7a, 0x45 } };
        private static ushort[][] stateReducsOnTerminal_39 = { new ushort[2] { 0x11, 0x58 } };
        private static ushort[] stateExpectedIDs_3A = { 0x54 };
        private static string[] stateExpectedNames_3A = { "_T[rules]" };
        private static string[] stateItems_3A = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals • grammar_cf_rules<grammar_text_terminal> }]", "[grammar_cf_rules<grammar_text_terminal> → • rules { _m152 }]" };
        private static ushort[][] stateShiftsOnTerminal_3A = { new ushort[2] { 0x54, 0x47 } };
        private static ushort[][] stateShiftsOnVariable_3A = { new ushort[2] { 0x7b, 0x46 } };
        private static ushort[][] stateReducsOnTerminal_3A = {  };
        private static ushort[] stateExpectedIDs_3B = { 0x12 };
        private static string[] stateExpectedNames_3B = { "_T[}]" };
        private static string[] stateItems_3B = { "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> • }]" };
        private static ushort[][] stateShiftsOnTerminal_3B = { new ushort[2] { 0x12, 0x48 } };
        private static ushort[][] stateShiftsOnVariable_3B = {  };
        private static ushort[][] stateReducsOnTerminal_3B = {  };
        private static ushort[] stateExpectedIDs_3C = { 0x11 };
        private static string[] stateExpectedNames_3C = { "_T[{]" };
        private static string[] stateItems_3C = { "[grammar_cf_rules<grammar_bin_terminal> → rules • { _m193 }]" };
        private static ushort[][] stateShiftsOnTerminal_3C = { new ushort[2] { 0x11, 0x49 } };
        private static ushort[][] stateShiftsOnVariable_3C = {  };
        private static ushort[][] stateReducsOnTerminal_3C = {  };
        private static ushort[] stateExpectedIDs_3D = { 0x12 };
        private static string[] stateExpectedNames_3D = { "_T[}]" };
        private static string[] stateItems_3D = { "[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> • }]" };
        private static ushort[][] stateShiftsOnTerminal_3D = { new ushort[2] { 0x12, 0x4A } };
        private static ushort[][] stateShiftsOnVariable_3D = {  };
        private static ushort[][] stateReducsOnTerminal_3D = {  };
        private static ushort[] stateExpectedIDs_3E = { 0x11 };
        private static string[] stateExpectedNames_3E = { "_T[{]" };
        private static string[] stateItems_3E = { "[grammar_cs_rules<grammar_text_terminal> → rules • { _m160 }]" };
        private static ushort[][] stateShiftsOnTerminal_3E = { new ushort[2] { 0x11, 0x4B } };
        private static ushort[][] stateShiftsOnVariable_3E = {  };
        private static ushort[][] stateReducsOnTerminal_3E = {  };
        private static ushort[] stateExpectedIDs_3F = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_3F = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_3F = { "[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> } •]" };
        private static ushort[][] stateShiftsOnTerminal_3F = {  };
        private static ushort[][] stateShiftsOnVariable_3F = {  };
        private static ushort[][] stateReducsOnTerminal_3F = { new ushort[2] { 0x2, 0xA0 }, new ushort[2] { 0xc, 0xA0 }, new ushort[2] { 0xd, 0xA0 }, new ushort[2] { 0xe, 0xA0 }, new ushort[2] { 0xf, 0xA0 }, new ushort[2] { 0x10, 0xA0 }, new ushort[2] { 0x12, 0xA0 }, new ushort[2] { 0x52, 0xA0 } };
        private static ushort[] stateExpectedIDs_40 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_40 = { "NAME", "_T[}]" };
        private static string[] stateItems_40 = { "[grammar_terminals → terminals { • _m109 }]", "[_m109 → • terminal _m109]", "[_m109 → •]", "[terminal → • NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] stateShiftsOnTerminal_40 = { new ushort[2] { 0xa, 0x4E } };
        private static ushort[][] stateShiftsOnVariable_40 = { new ushort[2] { 0x79, 0x4C }, new ushort[2] { 0x67, 0x4D } };
        private static ushort[][] stateReducsOnTerminal_40 = { new ushort[2] { 0x12, 0x56 } };
        private static ushort[] stateExpectedIDs_41 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_41 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_41 = { "[grammar_cs_rules<grammar_bin_terminal> → rules { • _m178 }]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178]", "[_m178 → •]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_41 = { new ushort[2] { 0xda, 0x53 } };
        private static ushort[][] stateShiftsOnVariable_41 = { new ushort[2] { 0xe7, 0x4F }, new ushort[2] { 0xe4, 0x50 }, new ushort[2] { 0xe6, 0x51 }, new ushort[2] { 0xe5, 0x52 } };
        private static ushort[][] stateReducsOnTerminal_41 = { new ushort[2] { 0x12, 0xB0 }, new ushort[2] { 0xa, 0xAC } };
        private static ushort[] stateExpectedIDs_42 = { 0x12 };
        private static string[] stateExpectedNames_42 = { "_T[}]" };
        private static string[] stateItems_42 = { "[grammar_options → options { _m105 • }]" };
        private static ushort[][] stateShiftsOnTerminal_42 = { new ushort[2] { 0x12, 0x54 } };
        private static ushort[][] stateShiftsOnVariable_42 = {  };
        private static ushort[][] stateReducsOnTerminal_42 = {  };
        private static ushort[] stateExpectedIDs_43 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_43 = { "NAME", "_T[}]" };
        private static string[] stateItems_43 = { "[_m105 → option • _m105]", "[_m105 → • option _m105]", "[_m105 → •]", "[option → • NAME = QUOTED_DATA ;]" };
        private static ushort[][] stateShiftsOnTerminal_43 = { new ushort[2] { 0xa, 0x44 } };
        private static ushort[][] stateShiftsOnVariable_43 = { new ushort[2] { 0x78, 0x55 }, new ushort[2] { 0x57, 0x43 } };
        private static ushort[][] stateReducsOnTerminal_43 = { new ushort[2] { 0x12, 0x54 } };
        private static ushort[] stateExpectedIDs_44 = { 0x40 };
        private static string[] stateExpectedNames_44 = { "_T[=]" };
        private static string[] stateItems_44 = { "[option → NAME • = QUOTED_DATA ;]" };
        private static ushort[][] stateShiftsOnTerminal_44 = { new ushort[2] { 0x40, 0x56 } };
        private static ushort[][] stateShiftsOnVariable_44 = {  };
        private static ushort[][] stateReducsOnTerminal_44 = {  };
        private static ushort[] stateExpectedIDs_45 = { 0x11 };
        private static string[] stateExpectedNames_45 = { "_T[{]" };
        private static string[] stateItems_45 = { "[_m113 → , qualified_name _m113 •]" };
        private static ushort[][] stateShiftsOnTerminal_45 = {  };
        private static ushort[][] stateShiftsOnVariable_45 = {  };
        private static ushort[][] stateReducsOnTerminal_45 = { new ushort[2] { 0x11, 0x57 } };
        private static ushort[] stateExpectedIDs_46 = { 0x12 };
        private static string[] stateExpectedNames_46 = { "_T[}]" };
        private static string[] stateItems_46 = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> • }]" };
        private static ushort[][] stateShiftsOnTerminal_46 = { new ushort[2] { 0x12, 0x57 } };
        private static ushort[][] stateShiftsOnVariable_46 = {  };
        private static ushort[][] stateReducsOnTerminal_46 = {  };
        private static ushort[] stateExpectedIDs_47 = { 0x11 };
        private static string[] stateExpectedNames_47 = { "_T[{]" };
        private static string[] stateItems_47 = { "[grammar_cf_rules<grammar_text_terminal> → rules • { _m152 }]" };
        private static ushort[][] stateShiftsOnTerminal_47 = { new ushort[2] { 0x11, 0x58 } };
        private static ushort[][] stateShiftsOnVariable_47 = {  };
        private static ushort[][] stateReducsOnTerminal_47 = {  };
        private static ushort[] stateExpectedIDs_48 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_48 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_48 = { "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> } •]" };
        private static ushort[][] stateShiftsOnTerminal_48 = {  };
        private static ushort[][] stateShiftsOnVariable_48 = {  };
        private static ushort[][] stateReducsOnTerminal_48 = { new ushort[2] { 0x2, 0x4A }, new ushort[2] { 0xc, 0x4A }, new ushort[2] { 0xd, 0x4A }, new ushort[2] { 0xe, 0x4A }, new ushort[2] { 0xf, 0x4A }, new ushort[2] { 0x10, 0x4A }, new ushort[2] { 0x12, 0x4A }, new ushort[2] { 0x52, 0x4A } };
        private static ushort[] stateExpectedIDs_49 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_49 = { "NAME", "_T[}]" };
        private static string[] stateItems_49 = { "[grammar_cf_rules<grammar_bin_terminal> → rules { • _m193 }]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193]", "[_m193 → •]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_49 = { new ushort[2] { 0xa, 0x5C } };
        private static ushort[][] stateShiftsOnVariable_49 = { new ushort[2] { 0x9e, 0x59 }, new ushort[2] { 0x8e, 0x5A }, new ushort[2] { 0x9d, 0x5B } };
        private static ushort[][] stateReducsOnTerminal_49 = { new ushort[2] { 0x12, 0x9E } };
        private static ushort[] stateExpectedIDs_4A = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_4A = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_4A = { "[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> } •]" };
        private static ushort[][] stateShiftsOnTerminal_4A = {  };
        private static ushort[][] stateShiftsOnVariable_4A = {  };
        private static ushort[][] stateReducsOnTerminal_4A = { new ushort[2] { 0x2, 0x9F }, new ushort[2] { 0xc, 0x9F }, new ushort[2] { 0xd, 0x9F }, new ushort[2] { 0xe, 0x9F }, new ushort[2] { 0xf, 0x9F }, new ushort[2] { 0x10, 0x9F }, new ushort[2] { 0x12, 0x9F }, new ushort[2] { 0x52, 0x9F } };
        private static ushort[] stateExpectedIDs_4B = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_4B = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_4B = { "[grammar_cs_rules<grammar_text_terminal> → rules { • _m160 }]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160]", "[_m160 → •]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_4B = { new ushort[2] { 0xda, 0x61 } };
        private static ushort[][] stateShiftsOnVariable_4B = { new ushort[2] { 0xe2, 0x5D }, new ushort[2] { 0xdf, 0x5E }, new ushort[2] { 0xe1, 0x5F }, new ushort[2] { 0xe0, 0x60 } };
        private static ushort[][] stateReducsOnTerminal_4B = { new ushort[2] { 0x12, 0xA8 }, new ushort[2] { 0xa, 0xA4 } };
        private static ushort[] stateExpectedIDs_4C = { 0x12 };
        private static string[] stateExpectedNames_4C = { "_T[}]" };
        private static string[] stateItems_4C = { "[grammar_terminals → terminals { _m109 • }]" };
        private static ushort[][] stateShiftsOnTerminal_4C = { new ushort[2] { 0x12, 0x62 } };
        private static ushort[][] stateShiftsOnVariable_4C = {  };
        private static ushort[][] stateReducsOnTerminal_4C = {  };
        private static ushort[] stateExpectedIDs_4D = { 0xA, 0x12 };
        private static string[] stateExpectedNames_4D = { "NAME", "_T[}]" };
        private static string[] stateItems_4D = { "[_m109 → terminal • _m109]", "[_m109 → • terminal _m109]", "[_m109 → •]", "[terminal → • NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] stateShiftsOnTerminal_4D = { new ushort[2] { 0xa, 0x4E } };
        private static ushort[][] stateShiftsOnVariable_4D = { new ushort[2] { 0x79, 0x63 }, new ushort[2] { 0x67, 0x4D } };
        private static ushort[][] stateReducsOnTerminal_4D = { new ushort[2] { 0x12, 0x56 } };
        private static ushort[] stateExpectedIDs_4E = { 0x4C };
        private static string[] stateExpectedNames_4E = { "_T[->]" };
        private static string[] stateItems_4E = { "[terminal → NAME • -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] stateShiftsOnTerminal_4E = { new ushort[2] { 0x4c, 0x64 } };
        private static ushort[][] stateShiftsOnVariable_4E = {  };
        private static ushort[][] stateReducsOnTerminal_4E = {  };
        private static ushort[] stateExpectedIDs_4F = { 0x12 };
        private static string[] stateExpectedNames_4F = { "_T[}]" };
        private static string[] stateItems_4F = { "[grammar_cs_rules<grammar_bin_terminal> → rules { _m178 • }]" };
        private static ushort[][] stateShiftsOnTerminal_4F = { new ushort[2] { 0x12, 0x65 } };
        private static ushort[][] stateShiftsOnVariable_4F = {  };
        private static ushort[][] stateReducsOnTerminal_4F = {  };
        private static ushort[] stateExpectedIDs_50 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_50 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_50 = { "[_m178 → cs_rule_simple<grammar_bin_terminal> • _m178]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178]", "[_m178 → •]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_50 = { new ushort[2] { 0xda, 0x53 } };
        private static ushort[][] stateShiftsOnVariable_50 = { new ushort[2] { 0xe7, 0x66 }, new ushort[2] { 0xe4, 0x50 }, new ushort[2] { 0xe6, 0x51 }, new ushort[2] { 0xe5, 0x52 } };
        private static ushort[][] stateReducsOnTerminal_50 = { new ushort[2] { 0x12, 0xB0 }, new ushort[2] { 0xa, 0xAC } };
        private static ushort[] stateExpectedIDs_51 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_51 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_51 = { "[_m178 → cs_rule_template<grammar_bin_terminal> • _m178]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178]", "[_m178 → •]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_51 = { new ushort[2] { 0xda, 0x53 } };
        private static ushort[][] stateShiftsOnVariable_51 = { new ushort[2] { 0xe7, 0x67 }, new ushort[2] { 0xe4, 0x50 }, new ushort[2] { 0xe6, 0x51 }, new ushort[2] { 0xe5, 0x52 } };
        private static ushort[][] stateReducsOnTerminal_51 = { new ushort[2] { 0x12, 0xB0 }, new ushort[2] { 0xa, 0xAC } };
        private static ushort[] stateExpectedIDs_52 = { 0xA };
        private static string[] stateExpectedNames_52 = { "NAME" };
        private static string[] stateItems_52 = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> • NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> • NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_52 = { new ushort[2] { 0xa, 0x68 } };
        private static ushort[][] stateShiftsOnVariable_52 = {  };
        private static ushort[][] stateReducsOnTerminal_52 = {  };
        private static ushort[] stateExpectedIDs_53 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x43, 0x4A, 0xDB };
        private static string[] stateExpectedNames_53 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[(]", "_T[|]", "_T[]]" };
        private static string[] stateItems_53 = { "[cs_rule_context<grammar_bin_terminal> → [ • rule_definition<grammar_bin_terminal> ]]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_53 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_53 = { new ushort[2] { 0x8f, 0x69 }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_53 = { new ushort[2] { 0x4a, 0x80 }, new ushort[2] { 0xdb, 0x80 } };
        private static ushort[] stateExpectedIDs_54 = { 0x50, 0x54 };
        private static string[] stateExpectedNames_54 = { "_T[terminals]", "_T[rules]" };
        private static string[] stateItems_54 = { "[grammar_options → options { _m105 } •]" };
        private static ushort[][] stateShiftsOnTerminal_54 = {  };
        private static ushort[][] stateShiftsOnVariable_54 = {  };
        private static ushort[][] stateReducsOnTerminal_54 = { new ushort[2] { 0x50, 0x41 }, new ushort[2] { 0x54, 0x41 } };
        private static ushort[] stateExpectedIDs_55 = { 0x12 };
        private static string[] stateExpectedNames_55 = { "_T[}]" };
        private static string[] stateItems_55 = { "[_m105 → option _m105 •]" };
        private static ushort[][] stateShiftsOnTerminal_55 = {  };
        private static ushort[][] stateShiftsOnVariable_55 = {  };
        private static ushort[][] stateReducsOnTerminal_55 = { new ushort[2] { 0x12, 0x53 } };
        private static ushort[] stateExpectedIDs_56 = { 0x2E };
        private static string[] stateExpectedNames_56 = { "QUOTED_DATA" };
        private static string[] stateItems_56 = { "[option → NAME = • QUOTED_DATA ;]" };
        private static ushort[][] stateShiftsOnTerminal_56 = { new ushort[2] { 0x2e, 0x86 } };
        private static ushort[][] stateShiftsOnVariable_56 = {  };
        private static ushort[][] stateReducsOnTerminal_56 = {  };
        private static ushort[] stateExpectedIDs_57 = { 0x2, 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x52 };
        private static string[] stateExpectedNames_57 = { "$", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "_T[namespace]", "_T[}]", "_T[grammar]" };
        private static string[] stateItems_57 = { "[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> } •]" };
        private static ushort[][] stateShiftsOnTerminal_57 = {  };
        private static ushort[][] stateShiftsOnVariable_57 = {  };
        private static ushort[][] stateReducsOnTerminal_57 = { new ushort[2] { 0x2, 0x49 }, new ushort[2] { 0xc, 0x49 }, new ushort[2] { 0xd, 0x49 }, new ushort[2] { 0xe, 0x49 }, new ushort[2] { 0xf, 0x49 }, new ushort[2] { 0x10, 0x49 }, new ushort[2] { 0x12, 0x49 }, new ushort[2] { 0x52, 0x49 } };
        private static ushort[] stateExpectedIDs_58 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_58 = { "NAME", "_T[}]" };
        private static string[] stateItems_58 = { "[grammar_cf_rules<grammar_text_terminal> → rules { • _m152 }]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152]", "[_m152 → •]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_58 = { new ushort[2] { 0xa, 0x8A } };
        private static ushort[][] stateShiftsOnVariable_58 = { new ushort[2] { 0x8c, 0x87 }, new ushort[2] { 0x7c, 0x88 }, new ushort[2] { 0x8b, 0x89 } };
        private static ushort[][] stateReducsOnTerminal_58 = { new ushort[2] { 0x12, 0x7B } };
        private static ushort[] stateExpectedIDs_59 = { 0x12 };
        private static string[] stateExpectedNames_59 = { "_T[}]" };
        private static string[] stateItems_59 = { "[grammar_cf_rules<grammar_bin_terminal> → rules { _m193 • }]" };
        private static ushort[][] stateShiftsOnTerminal_59 = { new ushort[2] { 0x12, 0x8B } };
        private static ushort[][] stateShiftsOnVariable_59 = {  };
        private static ushort[][] stateReducsOnTerminal_59 = {  };
        private static ushort[] stateExpectedIDs_5A = { 0xA, 0x12 };
        private static string[] stateExpectedNames_5A = { "NAME", "_T[}]" };
        private static string[] stateItems_5A = { "[_m193 → cf_rule_simple<grammar_bin_terminal> • _m193]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193]", "[_m193 → •]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_5A = { new ushort[2] { 0xa, 0x5C } };
        private static ushort[][] stateShiftsOnVariable_5A = { new ushort[2] { 0x9e, 0x8C }, new ushort[2] { 0x8e, 0x5A }, new ushort[2] { 0x9d, 0x5B } };
        private static ushort[][] stateReducsOnTerminal_5A = { new ushort[2] { 0x12, 0x9E } };
        private static ushort[] stateExpectedIDs_5B = { 0xA, 0x12 };
        private static string[] stateExpectedNames_5B = { "NAME", "_T[}]" };
        private static string[] stateItems_5B = { "[_m193 → cf_rule_template<grammar_bin_terminal> • _m193]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193]", "[_m193 → •]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_5B = { new ushort[2] { 0xa, 0x5C } };
        private static ushort[][] stateShiftsOnVariable_5B = { new ushort[2] { 0x9e, 0x8D }, new ushort[2] { 0x8e, 0x5A }, new ushort[2] { 0x9d, 0x5B } };
        private static ushort[][] stateReducsOnTerminal_5B = { new ushort[2] { 0x12, 0x9E } };
        private static ushort[] stateExpectedIDs_5C = { 0x4C, 0x4D };
        private static string[] stateExpectedNames_5C = { "_T[->]", "_T[<]" };
        private static string[] stateItems_5C = { "[cf_rule_simple<grammar_bin_terminal> → NAME • -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> → NAME • rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params → • < NAME _m101 >]" };
        private static ushort[][] stateShiftsOnTerminal_5C = { new ushort[2] { 0x4c, 0x8E }, new ushort[2] { 0x4d, 0x90 } };
        private static ushort[][] stateShiftsOnVariable_5C = { new ushort[2] { 0x6b, 0x8F } };
        private static ushort[][] stateReducsOnTerminal_5C = {  };
        private static ushort[] stateExpectedIDs_5D = { 0x12 };
        private static string[] stateExpectedNames_5D = { "_T[}]" };
        private static string[] stateItems_5D = { "[grammar_cs_rules<grammar_text_terminal> → rules { _m160 • }]" };
        private static ushort[][] stateShiftsOnTerminal_5D = { new ushort[2] { 0x12, 0x91 } };
        private static ushort[][] stateShiftsOnVariable_5D = {  };
        private static ushort[][] stateReducsOnTerminal_5D = {  };
        private static ushort[] stateExpectedIDs_5E = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_5E = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_5E = { "[_m160 → cs_rule_simple<grammar_text_terminal> • _m160]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160]", "[_m160 → •]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_5E = { new ushort[2] { 0xda, 0x61 } };
        private static ushort[][] stateShiftsOnVariable_5E = { new ushort[2] { 0xe2, 0x92 }, new ushort[2] { 0xdf, 0x5E }, new ushort[2] { 0xe1, 0x5F }, new ushort[2] { 0xe0, 0x60 } };
        private static ushort[][] stateReducsOnTerminal_5E = { new ushort[2] { 0x12, 0xA8 }, new ushort[2] { 0xa, 0xA4 } };
        private static ushort[] stateExpectedIDs_5F = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_5F = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_5F = { "[_m160 → cs_rule_template<grammar_text_terminal> • _m160]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160]", "[_m160 → •]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_5F = { new ushort[2] { 0xda, 0x61 } };
        private static ushort[][] stateShiftsOnVariable_5F = { new ushort[2] { 0xe2, 0x93 }, new ushort[2] { 0xdf, 0x5E }, new ushort[2] { 0xe1, 0x5F }, new ushort[2] { 0xe0, 0x60 } };
        private static ushort[][] stateReducsOnTerminal_5F = { new ushort[2] { 0x12, 0xA8 }, new ushort[2] { 0xa, 0xA4 } };
        private static ushort[] stateExpectedIDs_60 = { 0xA };
        private static string[] stateExpectedNames_60 = { "NAME" };
        private static string[] stateItems_60 = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> • NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> • NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_60 = { new ushort[2] { 0xa, 0x94 } };
        private static ushort[][] stateShiftsOnVariable_60 = {  };
        private static ushort[][] stateReducsOnTerminal_60 = {  };
        private static ushort[] stateExpectedIDs_61 = { 0xA, 0x11, 0x2E, 0x30, 0x43, 0x4A, 0xDB };
        private static string[] stateExpectedNames_61 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[(]", "_T[|]", "_T[]]" };
        private static string[] stateItems_61 = { "[cs_rule_context<grammar_text_terminal> → [ • rule_definition<grammar_text_terminal> ]]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_61 = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_61 = { new ushort[2] { 0x7d, 0x95 }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_61 = { new ushort[2] { 0x4a, 0x5D }, new ushort[2] { 0xdb, 0x5D } };
        private static ushort[] stateExpectedIDs_62 = { 0x54 };
        private static string[] stateExpectedNames_62 = { "_T[rules]" };
        private static string[] stateItems_62 = { "[grammar_terminals → terminals { _m109 } •]" };
        private static ushort[][] stateShiftsOnTerminal_62 = {  };
        private static ushort[][] stateShiftsOnVariable_62 = {  };
        private static ushort[][] stateReducsOnTerminal_62 = { new ushort[2] { 0x54, 0x42 } };
        private static ushort[] stateExpectedIDs_63 = { 0x12 };
        private static string[] stateExpectedNames_63 = { "_T[}]" };
        private static string[] stateItems_63 = { "[_m109 → terminal _m109 •]" };
        private static ushort[][] stateShiftsOnTerminal_63 = {  };
        private static ushort[][] stateShiftsOnVariable_63 = {  };
        private static ushort[][] stateReducsOnTerminal_63 = { new ushort[2] { 0x12, 0x55 } };
        private static ushort[] stateExpectedIDs_64 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x43 };
        private static string[] stateExpectedNames_64 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[(]" };
        private static string[] stateItems_64 = { "[terminal → NAME -> • terminal_definition terminal_subgrammar ;]", "[terminal_definition → • terminal_def_restrict _m93]", "[terminal_def_restrict → • terminal_def_fragment _m91]", "[terminal_def_fragment → • terminal_def_repetition _m89]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_64 = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_64 = { new ushort[2] { 0x65, 0xA6 }, new ushort[2] { 0x64, 0xA7 }, new ushort[2] { 0x63, 0xA8 }, new ushort[2] { 0x62, 0xA9 }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_64 = {  };
        private static ushort[] stateExpectedIDs_65 = { 0x12 };
        private static string[] stateExpectedNames_65 = { "_T[}]" };
        private static string[] stateItems_65 = { "[grammar_cs_rules<grammar_bin_terminal> → rules { _m178 } •]" };
        private static ushort[][] stateShiftsOnTerminal_65 = {  };
        private static ushort[][] stateShiftsOnVariable_65 = {  };
        private static ushort[][] stateReducsOnTerminal_65 = { new ushort[2] { 0x12, 0xA9 } };
        private static ushort[] stateExpectedIDs_66 = { 0x12 };
        private static string[] stateExpectedNames_66 = { "_T[}]" };
        private static string[] stateItems_66 = { "[_m178 → cs_rule_simple<grammar_bin_terminal> _m178 •]" };
        private static ushort[][] stateShiftsOnTerminal_66 = {  };
        private static ushort[][] stateShiftsOnVariable_66 = {  };
        private static ushort[][] stateReducsOnTerminal_66 = { new ushort[2] { 0x12, 0xAE } };
        private static ushort[] stateExpectedIDs_67 = { 0x12 };
        private static string[] stateExpectedNames_67 = { "_T[}]" };
        private static string[] stateItems_67 = { "[_m178 → cs_rule_template<grammar_bin_terminal> _m178 •]" };
        private static ushort[][] stateShiftsOnTerminal_67 = {  };
        private static ushort[][] stateShiftsOnVariable_67 = {  };
        private static ushort[][] stateReducsOnTerminal_67 = { new ushort[2] { 0x12, 0xAF } };
        private static ushort[] stateExpectedIDs_68 = { 0x4C, 0x4D, 0xDA };
        private static string[] stateExpectedNames_68 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] stateItems_68 = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME • cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME • cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_68 = { new ushort[2] { 0xda, 0x53 } };
        private static ushort[][] stateShiftsOnVariable_68 = { new ushort[2] { 0xe5, 0xBB } };
        private static ushort[][] stateReducsOnTerminal_68 = { new ushort[2] { 0x4c, 0xAC }, new ushort[2] { 0x4d, 0xAC } };
        private static ushort[] stateExpectedIDs_69 = { 0xDB };
        private static string[] stateExpectedNames_69 = { "_T[]]" };
        private static string[] stateItems_69 = { "[cs_rule_context<grammar_bin_terminal> → [ rule_definition<grammar_bin_terminal> • ]]" };
        private static ushort[][] stateShiftsOnTerminal_69 = { new ushort[2] { 0xdb, 0xBC } };
        private static ushort[][] stateShiftsOnVariable_69 = {  };
        private static ushort[][] stateReducsOnTerminal_69 = {  };
        private static ushort[] stateExpectedIDs_6A = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_6A = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_6A = { "[rule_definition<grammar_bin_terminal> → rule_def_choice<grammar_bin_terminal> • _m188]", "[_m188 → • | rule_def_choice<grammar_bin_terminal> _m188]", "[_m188 → •]" };
        private static ushort[][] stateShiftsOnTerminal_6A = { new ushort[2] { 0x4a, 0xBE } };
        private static ushort[][] stateShiftsOnVariable_6A = { new ushort[2] { 0x9c, 0xBD } };
        private static ushort[][] stateReducsOnTerminal_6A = { new ushort[2] { 0x41, 0x9A }, new ushort[2] { 0x44, 0x9A }, new ushort[2] { 0xdb, 0x9A } };
        private static ushort[] stateExpectedIDs_6B = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_6B = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_6B = { "[rule_def_choice<grammar_bin_terminal> → rule_def_restrict<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_6B = {  };
        private static ushort[][] stateShiftsOnVariable_6B = {  };
        private static ushort[][] stateReducsOnTerminal_6B = { new ushort[2] { 0x41, 0x7F }, new ushort[2] { 0x44, 0x7F }, new ushort[2] { 0x4a, 0x7F }, new ushort[2] { 0xdb, 0x7F } };
        private static ushort[] stateExpectedIDs_6C = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_6C = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_6C = { "[rule_def_restrict<grammar_bin_terminal> → rule_def_fragment<grammar_bin_terminal> • _m186]", "[_m186 → • - rule_def_fragment<grammar_bin_terminal> _m186]", "[_m186 → •]" };
        private static ushort[][] stateShiftsOnTerminal_6C = { new ushort[2] { 0x49, 0xC0 } };
        private static ushort[][] stateShiftsOnVariable_6C = { new ushort[2] { 0x9b, 0xBF } };
        private static ushort[][] stateReducsOnTerminal_6C = { new ushort[2] { 0x41, 0x98 }, new ushort[2] { 0x44, 0x98 }, new ushort[2] { 0x4a, 0x98 }, new ushort[2] { 0xdb, 0x98 } };
        private static ushort[] stateExpectedIDs_6D = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_6D = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_6D = { "[rule_def_fragment<grammar_bin_terminal> → rule_def_repetition<grammar_bin_terminal> • _m184]", "[_m184 → • rule_def_repetition<grammar_bin_terminal> _m184]", "[_m184 → •]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_6D = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_6D = { new ushort[2] { 0x9a, 0xC1 }, new ushort[2] { 0x93, 0xC2 }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_6D = { new ushort[2] { 0x41, 0x96 }, new ushort[2] { 0x44, 0x96 }, new ushort[2] { 0x49, 0x96 }, new ushort[2] { 0x4a, 0x96 }, new ushort[2] { 0xdb, 0x96 } };
        private static ushort[] stateExpectedIDs_6E = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_6E = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_6E = { "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • *]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • +]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • ?]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_6E = { new ushort[2] { 0x45, 0xC3 }, new ushort[2] { 0x46, 0xC4 }, new ushort[2] { 0x47, 0xC5 } };
        private static ushort[][] stateShiftsOnVariable_6E = {  };
        private static ushort[][] stateReducsOnTerminal_6E = { new ushort[2] { 0xa, 0x86 }, new ushort[2] { 0x11, 0x86 }, new ushort[2] { 0x2e, 0x86 }, new ushort[2] { 0x34, 0x86 }, new ushort[2] { 0x35, 0x86 }, new ushort[2] { 0x36, 0x86 }, new ushort[2] { 0x37, 0x86 }, new ushort[2] { 0x38, 0x86 }, new ushort[2] { 0x39, 0x86 }, new ushort[2] { 0x3a, 0x86 }, new ushort[2] { 0x3b, 0x86 }, new ushort[2] { 0x3c, 0x86 }, new ushort[2] { 0x3d, 0x86 }, new ushort[2] { 0x3e, 0x86 }, new ushort[2] { 0x3f, 0x86 }, new ushort[2] { 0x41, 0x86 }, new ushort[2] { 0x43, 0x86 }, new ushort[2] { 0x44, 0x86 }, new ushort[2] { 0x49, 0x86 }, new ushort[2] { 0x4a, 0x86 }, new ushort[2] { 0xdb, 0x86 } };
        private static ushort[] stateExpectedIDs_6F = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_6F = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_6F = { "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> • ^]", "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> • !]", "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_6F = { new ushort[2] { 0x55, 0xC6 }, new ushort[2] { 0x56, 0xC7 } };
        private static ushort[][] stateShiftsOnVariable_6F = {  };
        private static ushort[][] stateReducsOnTerminal_6F = { new ushort[2] { 0xa, 0x89 }, new ushort[2] { 0x11, 0x89 }, new ushort[2] { 0x2e, 0x89 }, new ushort[2] { 0x34, 0x89 }, new ushort[2] { 0x35, 0x89 }, new ushort[2] { 0x36, 0x89 }, new ushort[2] { 0x37, 0x89 }, new ushort[2] { 0x38, 0x89 }, new ushort[2] { 0x39, 0x89 }, new ushort[2] { 0x3a, 0x89 }, new ushort[2] { 0x3b, 0x89 }, new ushort[2] { 0x3c, 0x89 }, new ushort[2] { 0x3d, 0x89 }, new ushort[2] { 0x3e, 0x89 }, new ushort[2] { 0x3f, 0x89 }, new ushort[2] { 0x41, 0x89 }, new ushort[2] { 0x43, 0x89 }, new ushort[2] { 0x44, 0x89 }, new ushort[2] { 0x45, 0x89 }, new ushort[2] { 0x46, 0x89 }, new ushort[2] { 0x47, 0x89 }, new ushort[2] { 0x49, 0x89 }, new ushort[2] { 0x4a, 0x89 }, new ushort[2] { 0xdb, 0x89 } };
        private static ushort[] stateExpectedIDs_70 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_70 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_70 = { "[rule_def_element<grammar_bin_terminal> → rule_def_atom<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_70 = {  };
        private static ushort[][] stateShiftsOnVariable_70 = {  };
        private static ushort[][] stateReducsOnTerminal_70 = { new ushort[2] { 0xa, 0x8A }, new ushort[2] { 0x11, 0x8A }, new ushort[2] { 0x2e, 0x8A }, new ushort[2] { 0x34, 0x8A }, new ushort[2] { 0x35, 0x8A }, new ushort[2] { 0x36, 0x8A }, new ushort[2] { 0x37, 0x8A }, new ushort[2] { 0x38, 0x8A }, new ushort[2] { 0x39, 0x8A }, new ushort[2] { 0x3a, 0x8A }, new ushort[2] { 0x3b, 0x8A }, new ushort[2] { 0x3c, 0x8A }, new ushort[2] { 0x3d, 0x8A }, new ushort[2] { 0x3e, 0x8A }, new ushort[2] { 0x3f, 0x8A }, new ushort[2] { 0x41, 0x8A }, new ushort[2] { 0x43, 0x8A }, new ushort[2] { 0x44, 0x8A }, new ushort[2] { 0x45, 0x8A }, new ushort[2] { 0x46, 0x8A }, new ushort[2] { 0x47, 0x8A }, new ushort[2] { 0x49, 0x8A }, new ushort[2] { 0x4a, 0x8A }, new ushort[2] { 0x55, 0x8A }, new ushort[2] { 0x56, 0x8A }, new ushort[2] { 0xdb, 0x8A } };
        private static ushort[] stateExpectedIDs_71 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x43, 0x44, 0x4A };
        private static string[] stateExpectedNames_71 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[(]", "_T[)]", "_T[|]" };
        private static string[] stateItems_71 = { "[rule_def_element<grammar_bin_terminal> → ( • rule_definition<grammar_bin_terminal> )]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_71 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_71 = { new ushort[2] { 0x8f, 0xC8 }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_71 = { new ushort[2] { 0x44, 0x80 }, new ushort[2] { 0x4a, 0x80 } };
        private static ushort[] stateExpectedIDs_72 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_72 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_72 = { "[rule_def_atom<grammar_bin_terminal> → rule_sym_action •]" };
        private static ushort[][] stateShiftsOnTerminal_72 = {  };
        private static ushort[][] stateShiftsOnVariable_72 = {  };
        private static ushort[][] stateReducsOnTerminal_72 = { new ushort[2] { 0xa, 0x8C }, new ushort[2] { 0x11, 0x8C }, new ushort[2] { 0x2e, 0x8C }, new ushort[2] { 0x34, 0x8C }, new ushort[2] { 0x35, 0x8C }, new ushort[2] { 0x36, 0x8C }, new ushort[2] { 0x37, 0x8C }, new ushort[2] { 0x38, 0x8C }, new ushort[2] { 0x39, 0x8C }, new ushort[2] { 0x3a, 0x8C }, new ushort[2] { 0x3b, 0x8C }, new ushort[2] { 0x3c, 0x8C }, new ushort[2] { 0x3d, 0x8C }, new ushort[2] { 0x3e, 0x8C }, new ushort[2] { 0x3f, 0x8C }, new ushort[2] { 0x41, 0x8C }, new ushort[2] { 0x43, 0x8C }, new ushort[2] { 0x44, 0x8C }, new ushort[2] { 0x45, 0x8C }, new ushort[2] { 0x46, 0x8C }, new ushort[2] { 0x47, 0x8C }, new ushort[2] { 0x48, 0x8C }, new ushort[2] { 0x49, 0x8C }, new ushort[2] { 0x4a, 0x8C }, new ushort[2] { 0x4e, 0x8C }, new ushort[2] { 0x55, 0x8C }, new ushort[2] { 0x56, 0x8C }, new ushort[2] { 0xdb, 0x8C } };
        private static ushort[] stateExpectedIDs_73 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_73 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_73 = { "[rule_def_atom<grammar_bin_terminal> → rule_sym_virtual •]" };
        private static ushort[][] stateShiftsOnTerminal_73 = {  };
        private static ushort[][] stateShiftsOnVariable_73 = {  };
        private static ushort[][] stateReducsOnTerminal_73 = { new ushort[2] { 0xa, 0x8D }, new ushort[2] { 0x11, 0x8D }, new ushort[2] { 0x2e, 0x8D }, new ushort[2] { 0x34, 0x8D }, new ushort[2] { 0x35, 0x8D }, new ushort[2] { 0x36, 0x8D }, new ushort[2] { 0x37, 0x8D }, new ushort[2] { 0x38, 0x8D }, new ushort[2] { 0x39, 0x8D }, new ushort[2] { 0x3a, 0x8D }, new ushort[2] { 0x3b, 0x8D }, new ushort[2] { 0x3c, 0x8D }, new ushort[2] { 0x3d, 0x8D }, new ushort[2] { 0x3e, 0x8D }, new ushort[2] { 0x3f, 0x8D }, new ushort[2] { 0x41, 0x8D }, new ushort[2] { 0x43, 0x8D }, new ushort[2] { 0x44, 0x8D }, new ushort[2] { 0x45, 0x8D }, new ushort[2] { 0x46, 0x8D }, new ushort[2] { 0x47, 0x8D }, new ushort[2] { 0x48, 0x8D }, new ushort[2] { 0x49, 0x8D }, new ushort[2] { 0x4a, 0x8D }, new ushort[2] { 0x4e, 0x8D }, new ushort[2] { 0x55, 0x8D }, new ushort[2] { 0x56, 0x8D }, new ushort[2] { 0xdb, 0x8D } };
        private static ushort[] stateExpectedIDs_74 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_74 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_74 = { "[rule_def_atom<grammar_bin_terminal> → rule_sym_ref_simple •]" };
        private static ushort[][] stateShiftsOnTerminal_74 = {  };
        private static ushort[][] stateShiftsOnVariable_74 = {  };
        private static ushort[][] stateReducsOnTerminal_74 = { new ushort[2] { 0xa, 0x8E }, new ushort[2] { 0x11, 0x8E }, new ushort[2] { 0x2e, 0x8E }, new ushort[2] { 0x34, 0x8E }, new ushort[2] { 0x35, 0x8E }, new ushort[2] { 0x36, 0x8E }, new ushort[2] { 0x37, 0x8E }, new ushort[2] { 0x38, 0x8E }, new ushort[2] { 0x39, 0x8E }, new ushort[2] { 0x3a, 0x8E }, new ushort[2] { 0x3b, 0x8E }, new ushort[2] { 0x3c, 0x8E }, new ushort[2] { 0x3d, 0x8E }, new ushort[2] { 0x3e, 0x8E }, new ushort[2] { 0x3f, 0x8E }, new ushort[2] { 0x41, 0x8E }, new ushort[2] { 0x43, 0x8E }, new ushort[2] { 0x44, 0x8E }, new ushort[2] { 0x45, 0x8E }, new ushort[2] { 0x46, 0x8E }, new ushort[2] { 0x47, 0x8E }, new ushort[2] { 0x48, 0x8E }, new ushort[2] { 0x49, 0x8E }, new ushort[2] { 0x4a, 0x8E }, new ushort[2] { 0x4e, 0x8E }, new ushort[2] { 0x55, 0x8E }, new ushort[2] { 0x56, 0x8E }, new ushort[2] { 0xdb, 0x8E } };
        private static ushort[] stateExpectedIDs_75 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_75 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_75 = { "[rule_def_atom<grammar_bin_terminal> → rule_sym_ref_template<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_75 = {  };
        private static ushort[][] stateShiftsOnVariable_75 = {  };
        private static ushort[][] stateReducsOnTerminal_75 = { new ushort[2] { 0xa, 0x8F }, new ushort[2] { 0x11, 0x8F }, new ushort[2] { 0x2e, 0x8F }, new ushort[2] { 0x34, 0x8F }, new ushort[2] { 0x35, 0x8F }, new ushort[2] { 0x36, 0x8F }, new ushort[2] { 0x37, 0x8F }, new ushort[2] { 0x38, 0x8F }, new ushort[2] { 0x39, 0x8F }, new ushort[2] { 0x3a, 0x8F }, new ushort[2] { 0x3b, 0x8F }, new ushort[2] { 0x3c, 0x8F }, new ushort[2] { 0x3d, 0x8F }, new ushort[2] { 0x3e, 0x8F }, new ushort[2] { 0x3f, 0x8F }, new ushort[2] { 0x41, 0x8F }, new ushort[2] { 0x43, 0x8F }, new ushort[2] { 0x44, 0x8F }, new ushort[2] { 0x45, 0x8F }, new ushort[2] { 0x46, 0x8F }, new ushort[2] { 0x47, 0x8F }, new ushort[2] { 0x48, 0x8F }, new ushort[2] { 0x49, 0x8F }, new ushort[2] { 0x4a, 0x8F }, new ushort[2] { 0x4e, 0x8F }, new ushort[2] { 0x55, 0x8F }, new ushort[2] { 0x56, 0x8F }, new ushort[2] { 0xdb, 0x8F } };
        private static ushort[] stateExpectedIDs_76 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_76 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_76 = { "[rule_def_atom<grammar_bin_terminal> → grammar_bin_terminal •]" };
        private static ushort[][] stateShiftsOnTerminal_76 = {  };
        private static ushort[][] stateShiftsOnVariable_76 = {  };
        private static ushort[][] stateReducsOnTerminal_76 = { new ushort[2] { 0xa, 0x90 }, new ushort[2] { 0x11, 0x90 }, new ushort[2] { 0x2e, 0x90 }, new ushort[2] { 0x34, 0x90 }, new ushort[2] { 0x35, 0x90 }, new ushort[2] { 0x36, 0x90 }, new ushort[2] { 0x37, 0x90 }, new ushort[2] { 0x38, 0x90 }, new ushort[2] { 0x39, 0x90 }, new ushort[2] { 0x3a, 0x90 }, new ushort[2] { 0x3b, 0x90 }, new ushort[2] { 0x3c, 0x90 }, new ushort[2] { 0x3d, 0x90 }, new ushort[2] { 0x3e, 0x90 }, new ushort[2] { 0x3f, 0x90 }, new ushort[2] { 0x41, 0x90 }, new ushort[2] { 0x43, 0x90 }, new ushort[2] { 0x44, 0x90 }, new ushort[2] { 0x45, 0x90 }, new ushort[2] { 0x46, 0x90 }, new ushort[2] { 0x47, 0x90 }, new ushort[2] { 0x48, 0x90 }, new ushort[2] { 0x49, 0x90 }, new ushort[2] { 0x4a, 0x90 }, new ushort[2] { 0x4e, 0x90 }, new ushort[2] { 0x55, 0x90 }, new ushort[2] { 0x56, 0x90 }, new ushort[2] { 0xdb, 0x90 } };
        private static ushort[] stateExpectedIDs_77 = { 0xA };
        private static string[] stateExpectedNames_77 = { "NAME" };
        private static string[] stateItems_77 = { "[rule_sym_action → { • NAME }]" };
        private static ushort[][] stateShiftsOnTerminal_77 = { new ushort[2] { 0xa, 0xC9 } };
        private static ushort[][] stateShiftsOnVariable_77 = {  };
        private static ushort[][] stateReducsOnTerminal_77 = {  };
        private static ushort[] stateExpectedIDs_78 = { 0xA, 0x11, 0x2E, 0x30, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_78 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_78 = { "[rule_sym_virtual → QUOTED_DATA •]" };
        private static ushort[][] stateShiftsOnTerminal_78 = {  };
        private static ushort[][] stateShiftsOnVariable_78 = {  };
        private static ushort[][] stateReducsOnTerminal_78 = { new ushort[2] { 0xa, 0x31 }, new ushort[2] { 0x11, 0x31 }, new ushort[2] { 0x2e, 0x31 }, new ushort[2] { 0x30, 0x31 }, new ushort[2] { 0x34, 0x31 }, new ushort[2] { 0x35, 0x31 }, new ushort[2] { 0x36, 0x31 }, new ushort[2] { 0x37, 0x31 }, new ushort[2] { 0x38, 0x31 }, new ushort[2] { 0x39, 0x31 }, new ushort[2] { 0x3a, 0x31 }, new ushort[2] { 0x3b, 0x31 }, new ushort[2] { 0x3c, 0x31 }, new ushort[2] { 0x3d, 0x31 }, new ushort[2] { 0x3e, 0x31 }, new ushort[2] { 0x3f, 0x31 }, new ushort[2] { 0x41, 0x31 }, new ushort[2] { 0x43, 0x31 }, new ushort[2] { 0x44, 0x31 }, new ushort[2] { 0x45, 0x31 }, new ushort[2] { 0x46, 0x31 }, new ushort[2] { 0x47, 0x31 }, new ushort[2] { 0x48, 0x31 }, new ushort[2] { 0x49, 0x31 }, new ushort[2] { 0x4a, 0x31 }, new ushort[2] { 0x4e, 0x31 }, new ushort[2] { 0x55, 0x31 }, new ushort[2] { 0x56, 0x31 }, new ushort[2] { 0xdb, 0x31 } };
        private static ushort[] stateExpectedIDs_79 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4D, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_79 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[<]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_79 = { "[rule_sym_ref_simple → NAME •]", "[rule_sym_ref_template<grammar_bin_terminal> → NAME • rule_sym_ref_params<grammar_bin_terminal>]", "[rule_sym_ref_params<grammar_bin_terminal> → • < rule_def_atom<grammar_bin_terminal> _m175 >]" };
        private static ushort[][] stateShiftsOnTerminal_79 = { new ushort[2] { 0x4d, 0xCB } };
        private static ushort[][] stateShiftsOnVariable_79 = { new ushort[2] { 0x98, 0xCA } };
        private static ushort[][] stateReducsOnTerminal_79 = { new ushort[2] { 0xa, 0x32 }, new ushort[2] { 0x11, 0x32 }, new ushort[2] { 0x2e, 0x32 }, new ushort[2] { 0x34, 0x32 }, new ushort[2] { 0x35, 0x32 }, new ushort[2] { 0x36, 0x32 }, new ushort[2] { 0x37, 0x32 }, new ushort[2] { 0x38, 0x32 }, new ushort[2] { 0x39, 0x32 }, new ushort[2] { 0x3a, 0x32 }, new ushort[2] { 0x3b, 0x32 }, new ushort[2] { 0x3c, 0x32 }, new ushort[2] { 0x3d, 0x32 }, new ushort[2] { 0x3e, 0x32 }, new ushort[2] { 0x3f, 0x32 }, new ushort[2] { 0x41, 0x32 }, new ushort[2] { 0x43, 0x32 }, new ushort[2] { 0x44, 0x32 }, new ushort[2] { 0x45, 0x32 }, new ushort[2] { 0x46, 0x32 }, new ushort[2] { 0x47, 0x32 }, new ushort[2] { 0x48, 0x32 }, new ushort[2] { 0x49, 0x32 }, new ushort[2] { 0x4a, 0x32 }, new ushort[2] { 0x4e, 0x32 }, new ushort[2] { 0x55, 0x32 }, new ushort[2] { 0x56, 0x32 }, new ushort[2] { 0xdb, 0x32 } };
        private static ushort[] stateExpectedIDs_7A = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7A = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7A = { "[grammar_bin_terminal → SYMBOL_VALUE_UINT8 •]" };
        private static ushort[][] stateShiftsOnTerminal_7A = {  };
        private static ushort[][] stateShiftsOnVariable_7A = {  };
        private static ushort[][] stateReducsOnTerminal_7A = { new ushort[2] { 0xa, 0x34 }, new ushort[2] { 0x11, 0x34 }, new ushort[2] { 0x2e, 0x34 }, new ushort[2] { 0x34, 0x34 }, new ushort[2] { 0x35, 0x34 }, new ushort[2] { 0x36, 0x34 }, new ushort[2] { 0x37, 0x34 }, new ushort[2] { 0x38, 0x34 }, new ushort[2] { 0x39, 0x34 }, new ushort[2] { 0x3a, 0x34 }, new ushort[2] { 0x3b, 0x34 }, new ushort[2] { 0x3c, 0x34 }, new ushort[2] { 0x3d, 0x34 }, new ushort[2] { 0x3e, 0x34 }, new ushort[2] { 0x3f, 0x34 }, new ushort[2] { 0x41, 0x34 }, new ushort[2] { 0x43, 0x34 }, new ushort[2] { 0x44, 0x34 }, new ushort[2] { 0x45, 0x34 }, new ushort[2] { 0x46, 0x34 }, new ushort[2] { 0x47, 0x34 }, new ushort[2] { 0x48, 0x34 }, new ushort[2] { 0x49, 0x34 }, new ushort[2] { 0x4a, 0x34 }, new ushort[2] { 0x4e, 0x34 }, new ushort[2] { 0x55, 0x34 }, new ushort[2] { 0x56, 0x34 }, new ushort[2] { 0xdb, 0x34 } };
        private static ushort[] stateExpectedIDs_7B = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7B = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7B = { "[grammar_bin_terminal → SYMBOL_VALUE_UINT16 •]" };
        private static ushort[][] stateShiftsOnTerminal_7B = {  };
        private static ushort[][] stateShiftsOnVariable_7B = {  };
        private static ushort[][] stateReducsOnTerminal_7B = { new ushort[2] { 0xa, 0x35 }, new ushort[2] { 0x11, 0x35 }, new ushort[2] { 0x2e, 0x35 }, new ushort[2] { 0x34, 0x35 }, new ushort[2] { 0x35, 0x35 }, new ushort[2] { 0x36, 0x35 }, new ushort[2] { 0x37, 0x35 }, new ushort[2] { 0x38, 0x35 }, new ushort[2] { 0x39, 0x35 }, new ushort[2] { 0x3a, 0x35 }, new ushort[2] { 0x3b, 0x35 }, new ushort[2] { 0x3c, 0x35 }, new ushort[2] { 0x3d, 0x35 }, new ushort[2] { 0x3e, 0x35 }, new ushort[2] { 0x3f, 0x35 }, new ushort[2] { 0x41, 0x35 }, new ushort[2] { 0x43, 0x35 }, new ushort[2] { 0x44, 0x35 }, new ushort[2] { 0x45, 0x35 }, new ushort[2] { 0x46, 0x35 }, new ushort[2] { 0x47, 0x35 }, new ushort[2] { 0x48, 0x35 }, new ushort[2] { 0x49, 0x35 }, new ushort[2] { 0x4a, 0x35 }, new ushort[2] { 0x4e, 0x35 }, new ushort[2] { 0x55, 0x35 }, new ushort[2] { 0x56, 0x35 }, new ushort[2] { 0xdb, 0x35 } };
        private static ushort[] stateExpectedIDs_7C = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7C = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7C = { "[grammar_bin_terminal → SYMBOL_VALUE_UINT32 •]" };
        private static ushort[][] stateShiftsOnTerminal_7C = {  };
        private static ushort[][] stateShiftsOnVariable_7C = {  };
        private static ushort[][] stateReducsOnTerminal_7C = { new ushort[2] { 0xa, 0x36 }, new ushort[2] { 0x11, 0x36 }, new ushort[2] { 0x2e, 0x36 }, new ushort[2] { 0x34, 0x36 }, new ushort[2] { 0x35, 0x36 }, new ushort[2] { 0x36, 0x36 }, new ushort[2] { 0x37, 0x36 }, new ushort[2] { 0x38, 0x36 }, new ushort[2] { 0x39, 0x36 }, new ushort[2] { 0x3a, 0x36 }, new ushort[2] { 0x3b, 0x36 }, new ushort[2] { 0x3c, 0x36 }, new ushort[2] { 0x3d, 0x36 }, new ushort[2] { 0x3e, 0x36 }, new ushort[2] { 0x3f, 0x36 }, new ushort[2] { 0x41, 0x36 }, new ushort[2] { 0x43, 0x36 }, new ushort[2] { 0x44, 0x36 }, new ushort[2] { 0x45, 0x36 }, new ushort[2] { 0x46, 0x36 }, new ushort[2] { 0x47, 0x36 }, new ushort[2] { 0x48, 0x36 }, new ushort[2] { 0x49, 0x36 }, new ushort[2] { 0x4a, 0x36 }, new ushort[2] { 0x4e, 0x36 }, new ushort[2] { 0x55, 0x36 }, new ushort[2] { 0x56, 0x36 }, new ushort[2] { 0xdb, 0x36 } };
        private static ushort[] stateExpectedIDs_7D = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7D = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7D = { "[grammar_bin_terminal → SYMBOL_VALUE_UINT64 •]" };
        private static ushort[][] stateShiftsOnTerminal_7D = {  };
        private static ushort[][] stateShiftsOnVariable_7D = {  };
        private static ushort[][] stateReducsOnTerminal_7D = { new ushort[2] { 0xa, 0x37 }, new ushort[2] { 0x11, 0x37 }, new ushort[2] { 0x2e, 0x37 }, new ushort[2] { 0x34, 0x37 }, new ushort[2] { 0x35, 0x37 }, new ushort[2] { 0x36, 0x37 }, new ushort[2] { 0x37, 0x37 }, new ushort[2] { 0x38, 0x37 }, new ushort[2] { 0x39, 0x37 }, new ushort[2] { 0x3a, 0x37 }, new ushort[2] { 0x3b, 0x37 }, new ushort[2] { 0x3c, 0x37 }, new ushort[2] { 0x3d, 0x37 }, new ushort[2] { 0x3e, 0x37 }, new ushort[2] { 0x3f, 0x37 }, new ushort[2] { 0x41, 0x37 }, new ushort[2] { 0x43, 0x37 }, new ushort[2] { 0x44, 0x37 }, new ushort[2] { 0x45, 0x37 }, new ushort[2] { 0x46, 0x37 }, new ushort[2] { 0x47, 0x37 }, new ushort[2] { 0x48, 0x37 }, new ushort[2] { 0x49, 0x37 }, new ushort[2] { 0x4a, 0x37 }, new ushort[2] { 0x4e, 0x37 }, new ushort[2] { 0x55, 0x37 }, new ushort[2] { 0x56, 0x37 }, new ushort[2] { 0xdb, 0x37 } };
        private static ushort[] stateExpectedIDs_7E = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7E = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7E = { "[grammar_bin_terminal → SYMBOL_VALUE_UINT128 •]" };
        private static ushort[][] stateShiftsOnTerminal_7E = {  };
        private static ushort[][] stateShiftsOnVariable_7E = {  };
        private static ushort[][] stateReducsOnTerminal_7E = { new ushort[2] { 0xa, 0x38 }, new ushort[2] { 0x11, 0x38 }, new ushort[2] { 0x2e, 0x38 }, new ushort[2] { 0x34, 0x38 }, new ushort[2] { 0x35, 0x38 }, new ushort[2] { 0x36, 0x38 }, new ushort[2] { 0x37, 0x38 }, new ushort[2] { 0x38, 0x38 }, new ushort[2] { 0x39, 0x38 }, new ushort[2] { 0x3a, 0x38 }, new ushort[2] { 0x3b, 0x38 }, new ushort[2] { 0x3c, 0x38 }, new ushort[2] { 0x3d, 0x38 }, new ushort[2] { 0x3e, 0x38 }, new ushort[2] { 0x3f, 0x38 }, new ushort[2] { 0x41, 0x38 }, new ushort[2] { 0x43, 0x38 }, new ushort[2] { 0x44, 0x38 }, new ushort[2] { 0x45, 0x38 }, new ushort[2] { 0x46, 0x38 }, new ushort[2] { 0x47, 0x38 }, new ushort[2] { 0x48, 0x38 }, new ushort[2] { 0x49, 0x38 }, new ushort[2] { 0x4a, 0x38 }, new ushort[2] { 0x4e, 0x38 }, new ushort[2] { 0x55, 0x38 }, new ushort[2] { 0x56, 0x38 }, new ushort[2] { 0xdb, 0x38 } };
        private static ushort[] stateExpectedIDs_7F = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_7F = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_7F = { "[grammar_bin_terminal → SYMBOL_VALUE_BINARY •]" };
        private static ushort[][] stateShiftsOnTerminal_7F = {  };
        private static ushort[][] stateShiftsOnVariable_7F = {  };
        private static ushort[][] stateReducsOnTerminal_7F = { new ushort[2] { 0xa, 0x39 }, new ushort[2] { 0x11, 0x39 }, new ushort[2] { 0x2e, 0x39 }, new ushort[2] { 0x34, 0x39 }, new ushort[2] { 0x35, 0x39 }, new ushort[2] { 0x36, 0x39 }, new ushort[2] { 0x37, 0x39 }, new ushort[2] { 0x38, 0x39 }, new ushort[2] { 0x39, 0x39 }, new ushort[2] { 0x3a, 0x39 }, new ushort[2] { 0x3b, 0x39 }, new ushort[2] { 0x3c, 0x39 }, new ushort[2] { 0x3d, 0x39 }, new ushort[2] { 0x3e, 0x39 }, new ushort[2] { 0x3f, 0x39 }, new ushort[2] { 0x41, 0x39 }, new ushort[2] { 0x43, 0x39 }, new ushort[2] { 0x44, 0x39 }, new ushort[2] { 0x45, 0x39 }, new ushort[2] { 0x46, 0x39 }, new ushort[2] { 0x47, 0x39 }, new ushort[2] { 0x48, 0x39 }, new ushort[2] { 0x49, 0x39 }, new ushort[2] { 0x4a, 0x39 }, new ushort[2] { 0x4e, 0x39 }, new ushort[2] { 0x55, 0x39 }, new ushort[2] { 0x56, 0x39 }, new ushort[2] { 0xdb, 0x39 } };
        private static ushort[] stateExpectedIDs_80 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_80 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_80 = { "[grammar_bin_terminal → SYMBOL_JOKER_UINT8 •]" };
        private static ushort[][] stateShiftsOnTerminal_80 = {  };
        private static ushort[][] stateShiftsOnVariable_80 = {  };
        private static ushort[][] stateReducsOnTerminal_80 = { new ushort[2] { 0xa, 0x3A }, new ushort[2] { 0x11, 0x3A }, new ushort[2] { 0x2e, 0x3A }, new ushort[2] { 0x34, 0x3A }, new ushort[2] { 0x35, 0x3A }, new ushort[2] { 0x36, 0x3A }, new ushort[2] { 0x37, 0x3A }, new ushort[2] { 0x38, 0x3A }, new ushort[2] { 0x39, 0x3A }, new ushort[2] { 0x3a, 0x3A }, new ushort[2] { 0x3b, 0x3A }, new ushort[2] { 0x3c, 0x3A }, new ushort[2] { 0x3d, 0x3A }, new ushort[2] { 0x3e, 0x3A }, new ushort[2] { 0x3f, 0x3A }, new ushort[2] { 0x41, 0x3A }, new ushort[2] { 0x43, 0x3A }, new ushort[2] { 0x44, 0x3A }, new ushort[2] { 0x45, 0x3A }, new ushort[2] { 0x46, 0x3A }, new ushort[2] { 0x47, 0x3A }, new ushort[2] { 0x48, 0x3A }, new ushort[2] { 0x49, 0x3A }, new ushort[2] { 0x4a, 0x3A }, new ushort[2] { 0x4e, 0x3A }, new ushort[2] { 0x55, 0x3A }, new ushort[2] { 0x56, 0x3A }, new ushort[2] { 0xdb, 0x3A } };
        private static ushort[] stateExpectedIDs_81 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_81 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_81 = { "[grammar_bin_terminal → SYMBOL_JOKER_UINT16 •]" };
        private static ushort[][] stateShiftsOnTerminal_81 = {  };
        private static ushort[][] stateShiftsOnVariable_81 = {  };
        private static ushort[][] stateReducsOnTerminal_81 = { new ushort[2] { 0xa, 0x3B }, new ushort[2] { 0x11, 0x3B }, new ushort[2] { 0x2e, 0x3B }, new ushort[2] { 0x34, 0x3B }, new ushort[2] { 0x35, 0x3B }, new ushort[2] { 0x36, 0x3B }, new ushort[2] { 0x37, 0x3B }, new ushort[2] { 0x38, 0x3B }, new ushort[2] { 0x39, 0x3B }, new ushort[2] { 0x3a, 0x3B }, new ushort[2] { 0x3b, 0x3B }, new ushort[2] { 0x3c, 0x3B }, new ushort[2] { 0x3d, 0x3B }, new ushort[2] { 0x3e, 0x3B }, new ushort[2] { 0x3f, 0x3B }, new ushort[2] { 0x41, 0x3B }, new ushort[2] { 0x43, 0x3B }, new ushort[2] { 0x44, 0x3B }, new ushort[2] { 0x45, 0x3B }, new ushort[2] { 0x46, 0x3B }, new ushort[2] { 0x47, 0x3B }, new ushort[2] { 0x48, 0x3B }, new ushort[2] { 0x49, 0x3B }, new ushort[2] { 0x4a, 0x3B }, new ushort[2] { 0x4e, 0x3B }, new ushort[2] { 0x55, 0x3B }, new ushort[2] { 0x56, 0x3B }, new ushort[2] { 0xdb, 0x3B } };
        private static ushort[] stateExpectedIDs_82 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_82 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_82 = { "[grammar_bin_terminal → SYMBOL_JOKER_UINT32 •]" };
        private static ushort[][] stateShiftsOnTerminal_82 = {  };
        private static ushort[][] stateShiftsOnVariable_82 = {  };
        private static ushort[][] stateReducsOnTerminal_82 = { new ushort[2] { 0xa, 0x3C }, new ushort[2] { 0x11, 0x3C }, new ushort[2] { 0x2e, 0x3C }, new ushort[2] { 0x34, 0x3C }, new ushort[2] { 0x35, 0x3C }, new ushort[2] { 0x36, 0x3C }, new ushort[2] { 0x37, 0x3C }, new ushort[2] { 0x38, 0x3C }, new ushort[2] { 0x39, 0x3C }, new ushort[2] { 0x3a, 0x3C }, new ushort[2] { 0x3b, 0x3C }, new ushort[2] { 0x3c, 0x3C }, new ushort[2] { 0x3d, 0x3C }, new ushort[2] { 0x3e, 0x3C }, new ushort[2] { 0x3f, 0x3C }, new ushort[2] { 0x41, 0x3C }, new ushort[2] { 0x43, 0x3C }, new ushort[2] { 0x44, 0x3C }, new ushort[2] { 0x45, 0x3C }, new ushort[2] { 0x46, 0x3C }, new ushort[2] { 0x47, 0x3C }, new ushort[2] { 0x48, 0x3C }, new ushort[2] { 0x49, 0x3C }, new ushort[2] { 0x4a, 0x3C }, new ushort[2] { 0x4e, 0x3C }, new ushort[2] { 0x55, 0x3C }, new ushort[2] { 0x56, 0x3C }, new ushort[2] { 0xdb, 0x3C } };
        private static ushort[] stateExpectedIDs_83 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_83 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_83 = { "[grammar_bin_terminal → SYMBOL_JOKER_UINT64 •]" };
        private static ushort[][] stateShiftsOnTerminal_83 = {  };
        private static ushort[][] stateShiftsOnVariable_83 = {  };
        private static ushort[][] stateReducsOnTerminal_83 = { new ushort[2] { 0xa, 0x3D }, new ushort[2] { 0x11, 0x3D }, new ushort[2] { 0x2e, 0x3D }, new ushort[2] { 0x34, 0x3D }, new ushort[2] { 0x35, 0x3D }, new ushort[2] { 0x36, 0x3D }, new ushort[2] { 0x37, 0x3D }, new ushort[2] { 0x38, 0x3D }, new ushort[2] { 0x39, 0x3D }, new ushort[2] { 0x3a, 0x3D }, new ushort[2] { 0x3b, 0x3D }, new ushort[2] { 0x3c, 0x3D }, new ushort[2] { 0x3d, 0x3D }, new ushort[2] { 0x3e, 0x3D }, new ushort[2] { 0x3f, 0x3D }, new ushort[2] { 0x41, 0x3D }, new ushort[2] { 0x43, 0x3D }, new ushort[2] { 0x44, 0x3D }, new ushort[2] { 0x45, 0x3D }, new ushort[2] { 0x46, 0x3D }, new ushort[2] { 0x47, 0x3D }, new ushort[2] { 0x48, 0x3D }, new ushort[2] { 0x49, 0x3D }, new ushort[2] { 0x4a, 0x3D }, new ushort[2] { 0x4e, 0x3D }, new ushort[2] { 0x55, 0x3D }, new ushort[2] { 0x56, 0x3D }, new ushort[2] { 0xdb, 0x3D } };
        private static ushort[] stateExpectedIDs_84 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_84 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_84 = { "[grammar_bin_terminal → SYMBOL_JOKER_UINT128 •]" };
        private static ushort[][] stateShiftsOnTerminal_84 = {  };
        private static ushort[][] stateShiftsOnVariable_84 = {  };
        private static ushort[][] stateReducsOnTerminal_84 = { new ushort[2] { 0xa, 0x3E }, new ushort[2] { 0x11, 0x3E }, new ushort[2] { 0x2e, 0x3E }, new ushort[2] { 0x34, 0x3E }, new ushort[2] { 0x35, 0x3E }, new ushort[2] { 0x36, 0x3E }, new ushort[2] { 0x37, 0x3E }, new ushort[2] { 0x38, 0x3E }, new ushort[2] { 0x39, 0x3E }, new ushort[2] { 0x3a, 0x3E }, new ushort[2] { 0x3b, 0x3E }, new ushort[2] { 0x3c, 0x3E }, new ushort[2] { 0x3d, 0x3E }, new ushort[2] { 0x3e, 0x3E }, new ushort[2] { 0x3f, 0x3E }, new ushort[2] { 0x41, 0x3E }, new ushort[2] { 0x43, 0x3E }, new ushort[2] { 0x44, 0x3E }, new ushort[2] { 0x45, 0x3E }, new ushort[2] { 0x46, 0x3E }, new ushort[2] { 0x47, 0x3E }, new ushort[2] { 0x48, 0x3E }, new ushort[2] { 0x49, 0x3E }, new ushort[2] { 0x4a, 0x3E }, new ushort[2] { 0x4e, 0x3E }, new ushort[2] { 0x55, 0x3E }, new ushort[2] { 0x56, 0x3E }, new ushort[2] { 0xdb, 0x3E } };
        private static ushort[] stateExpectedIDs_85 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_85 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_85 = { "[grammar_bin_terminal → SYMBOL_JOKER_BINARY •]" };
        private static ushort[][] stateShiftsOnTerminal_85 = {  };
        private static ushort[][] stateShiftsOnVariable_85 = {  };
        private static ushort[][] stateReducsOnTerminal_85 = { new ushort[2] { 0xa, 0x3F }, new ushort[2] { 0x11, 0x3F }, new ushort[2] { 0x2e, 0x3F }, new ushort[2] { 0x34, 0x3F }, new ushort[2] { 0x35, 0x3F }, new ushort[2] { 0x36, 0x3F }, new ushort[2] { 0x37, 0x3F }, new ushort[2] { 0x38, 0x3F }, new ushort[2] { 0x39, 0x3F }, new ushort[2] { 0x3a, 0x3F }, new ushort[2] { 0x3b, 0x3F }, new ushort[2] { 0x3c, 0x3F }, new ushort[2] { 0x3d, 0x3F }, new ushort[2] { 0x3e, 0x3F }, new ushort[2] { 0x3f, 0x3F }, new ushort[2] { 0x41, 0x3F }, new ushort[2] { 0x43, 0x3F }, new ushort[2] { 0x44, 0x3F }, new ushort[2] { 0x45, 0x3F }, new ushort[2] { 0x46, 0x3F }, new ushort[2] { 0x47, 0x3F }, new ushort[2] { 0x48, 0x3F }, new ushort[2] { 0x49, 0x3F }, new ushort[2] { 0x4a, 0x3F }, new ushort[2] { 0x4e, 0x3F }, new ushort[2] { 0x55, 0x3F }, new ushort[2] { 0x56, 0x3F }, new ushort[2] { 0xdb, 0x3F } };
        private static ushort[] stateExpectedIDs_86 = { 0x41 };
        private static string[] stateExpectedNames_86 = { "_T[;]" };
        private static string[] stateItems_86 = { "[option → NAME = QUOTED_DATA • ;]" };
        private static ushort[][] stateShiftsOnTerminal_86 = { new ushort[2] { 0x41, 0xCC } };
        private static ushort[][] stateShiftsOnVariable_86 = {  };
        private static ushort[][] stateReducsOnTerminal_86 = {  };
        private static ushort[] stateExpectedIDs_87 = { 0x12 };
        private static string[] stateExpectedNames_87 = { "_T[}]" };
        private static string[] stateItems_87 = { "[grammar_cf_rules<grammar_text_terminal> → rules { _m152 • }]" };
        private static ushort[][] stateShiftsOnTerminal_87 = { new ushort[2] { 0x12, 0xCD } };
        private static ushort[][] stateShiftsOnVariable_87 = {  };
        private static ushort[][] stateReducsOnTerminal_87 = {  };
        private static ushort[] stateExpectedIDs_88 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_88 = { "NAME", "_T[}]" };
        private static string[] stateItems_88 = { "[_m152 → cf_rule_simple<grammar_text_terminal> • _m152]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152]", "[_m152 → •]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_88 = { new ushort[2] { 0xa, 0x8A } };
        private static ushort[][] stateShiftsOnVariable_88 = { new ushort[2] { 0x8c, 0xCE }, new ushort[2] { 0x7c, 0x88 }, new ushort[2] { 0x8b, 0x89 } };
        private static ushort[][] stateReducsOnTerminal_88 = { new ushort[2] { 0x12, 0x7B } };
        private static ushort[] stateExpectedIDs_89 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_89 = { "NAME", "_T[}]" };
        private static string[] stateItems_89 = { "[_m152 → cf_rule_template<grammar_text_terminal> • _m152]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152]", "[_m152 → •]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_89 = { new ushort[2] { 0xa, 0x8A } };
        private static ushort[][] stateShiftsOnVariable_89 = { new ushort[2] { 0x8c, 0xCF }, new ushort[2] { 0x7c, 0x88 }, new ushort[2] { 0x8b, 0x89 } };
        private static ushort[][] stateReducsOnTerminal_89 = { new ushort[2] { 0x12, 0x7B } };
        private static ushort[] stateExpectedIDs_8A = { 0x4C, 0x4D };
        private static string[] stateExpectedNames_8A = { "_T[->]", "_T[<]" };
        private static string[] stateItems_8A = { "[cf_rule_simple<grammar_text_terminal> → NAME • -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> → NAME • rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params → • < NAME _m101 >]" };
        private static ushort[][] stateShiftsOnTerminal_8A = { new ushort[2] { 0x4c, 0xD0 }, new ushort[2] { 0x4d, 0x90 } };
        private static ushort[][] stateShiftsOnVariable_8A = { new ushort[2] { 0x6b, 0xD1 } };
        private static ushort[][] stateReducsOnTerminal_8A = {  };
        private static ushort[] stateExpectedIDs_8B = { 0x12 };
        private static string[] stateExpectedNames_8B = { "_T[}]" };
        private static string[] stateItems_8B = { "[grammar_cf_rules<grammar_bin_terminal> → rules { _m193 } •]" };
        private static ushort[][] stateShiftsOnTerminal_8B = {  };
        private static ushort[][] stateShiftsOnVariable_8B = {  };
        private static ushort[][] stateReducsOnTerminal_8B = { new ushort[2] { 0x12, 0x7C } };
        private static ushort[] stateExpectedIDs_8C = { 0x12 };
        private static string[] stateExpectedNames_8C = { "_T[}]" };
        private static string[] stateItems_8C = { "[_m193 → cf_rule_simple<grammar_bin_terminal> _m193 •]" };
        private static ushort[][] stateShiftsOnTerminal_8C = {  };
        private static ushort[][] stateShiftsOnVariable_8C = {  };
        private static ushort[][] stateReducsOnTerminal_8C = { new ushort[2] { 0x12, 0x9C } };
        private static ushort[] stateExpectedIDs_8D = { 0x12 };
        private static string[] stateExpectedNames_8D = { "_T[}]" };
        private static string[] stateItems_8D = { "[_m193 → cf_rule_template<grammar_bin_terminal> _m193 •]" };
        private static ushort[][] stateShiftsOnTerminal_8D = {  };
        private static ushort[][] stateShiftsOnVariable_8D = {  };
        private static ushort[][] stateReducsOnTerminal_8D = { new ushort[2] { 0x12, 0x9D } };
        private static ushort[] stateExpectedIDs_8E = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_8E = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_8E = { "[cf_rule_simple<grammar_bin_terminal> → NAME -> • rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_8E = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_8E = { new ushort[2] { 0x8f, 0xD2 }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_8E = { new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x4a, 0x80 } };
        private static ushort[] stateExpectedIDs_8F = { 0x4C };
        private static string[] stateExpectedNames_8F = { "_T[->]" };
        private static string[] stateItems_8F = { "[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params • -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_8F = { new ushort[2] { 0x4c, 0xD3 } };
        private static ushort[][] stateShiftsOnVariable_8F = {  };
        private static ushort[][] stateReducsOnTerminal_8F = {  };
        private static ushort[] stateExpectedIDs_90 = { 0xA };
        private static string[] stateExpectedNames_90 = { "NAME" };
        private static string[] stateItems_90 = { "[rule_template_params → < • NAME _m101 >]" };
        private static ushort[][] stateShiftsOnTerminal_90 = { new ushort[2] { 0xa, 0xD4 } };
        private static ushort[][] stateShiftsOnVariable_90 = {  };
        private static ushort[][] stateReducsOnTerminal_90 = {  };
        private static ushort[] stateExpectedIDs_91 = { 0x12 };
        private static string[] stateExpectedNames_91 = { "_T[}]" };
        private static string[] stateItems_91 = { "[grammar_cs_rules<grammar_text_terminal> → rules { _m160 } •]" };
        private static ushort[][] stateShiftsOnTerminal_91 = {  };
        private static ushort[][] stateShiftsOnVariable_91 = {  };
        private static ushort[][] stateReducsOnTerminal_91 = { new ushort[2] { 0x12, 0xA1 } };
        private static ushort[] stateExpectedIDs_92 = { 0x12 };
        private static string[] stateExpectedNames_92 = { "_T[}]" };
        private static string[] stateItems_92 = { "[_m160 → cs_rule_simple<grammar_text_terminal> _m160 •]" };
        private static ushort[][] stateShiftsOnTerminal_92 = {  };
        private static ushort[][] stateShiftsOnVariable_92 = {  };
        private static ushort[][] stateReducsOnTerminal_92 = { new ushort[2] { 0x12, 0xA6 } };
        private static ushort[] stateExpectedIDs_93 = { 0x12 };
        private static string[] stateExpectedNames_93 = { "_T[}]" };
        private static string[] stateItems_93 = { "[_m160 → cs_rule_template<grammar_text_terminal> _m160 •]" };
        private static ushort[][] stateShiftsOnTerminal_93 = {  };
        private static ushort[][] stateShiftsOnVariable_93 = {  };
        private static ushort[][] stateReducsOnTerminal_93 = { new ushort[2] { 0x12, 0xA7 } };
        private static ushort[] stateExpectedIDs_94 = { 0x4C, 0x4D, 0xDA };
        private static string[] stateExpectedNames_94 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] stateItems_94 = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME • cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME • cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> → •]" };
        private static ushort[][] stateShiftsOnTerminal_94 = { new ushort[2] { 0xda, 0x61 } };
        private static ushort[][] stateShiftsOnVariable_94 = { new ushort[2] { 0xe0, 0xD5 } };
        private static ushort[][] stateReducsOnTerminal_94 = { new ushort[2] { 0x4c, 0xA4 }, new ushort[2] { 0x4d, 0xA4 } };
        private static ushort[] stateExpectedIDs_95 = { 0xDB };
        private static string[] stateExpectedNames_95 = { "_T[]]" };
        private static string[] stateItems_95 = { "[cs_rule_context<grammar_text_terminal> → [ rule_definition<grammar_text_terminal> • ]]" };
        private static ushort[][] stateShiftsOnTerminal_95 = { new ushort[2] { 0xdb, 0xD6 } };
        private static ushort[][] stateShiftsOnVariable_95 = {  };
        private static ushort[][] stateReducsOnTerminal_95 = {  };
        private static ushort[] stateExpectedIDs_96 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_96 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_96 = { "[rule_definition<grammar_text_terminal> → rule_def_choice<grammar_text_terminal> • _m147]", "[_m147 → • | rule_def_choice<grammar_text_terminal> _m147]", "[_m147 → •]" };
        private static ushort[][] stateShiftsOnTerminal_96 = { new ushort[2] { 0x4a, 0xD8 } };
        private static ushort[][] stateShiftsOnVariable_96 = { new ushort[2] { 0x8a, 0xD7 } };
        private static ushort[][] stateReducsOnTerminal_96 = { new ushort[2] { 0x41, 0x77 }, new ushort[2] { 0x44, 0x77 }, new ushort[2] { 0xdb, 0x77 } };
        private static ushort[] stateExpectedIDs_97 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_97 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_97 = { "[rule_def_choice<grammar_text_terminal> → rule_def_restrict<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_97 = {  };
        private static ushort[][] stateShiftsOnVariable_97 = {  };
        private static ushort[][] stateReducsOnTerminal_97 = { new ushort[2] { 0x41, 0x5C }, new ushort[2] { 0x44, 0x5C }, new ushort[2] { 0x4a, 0x5C }, new ushort[2] { 0xdb, 0x5C } };
        private static ushort[] stateExpectedIDs_98 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_98 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_98 = { "[rule_def_restrict<grammar_text_terminal> → rule_def_fragment<grammar_text_terminal> • _m145]", "[_m145 → • - rule_def_fragment<grammar_text_terminal> _m145]", "[_m145 → •]" };
        private static ushort[][] stateShiftsOnTerminal_98 = { new ushort[2] { 0x49, 0xDA } };
        private static ushort[][] stateShiftsOnVariable_98 = { new ushort[2] { 0x89, 0xD9 } };
        private static ushort[][] stateReducsOnTerminal_98 = { new ushort[2] { 0x41, 0x75 }, new ushort[2] { 0x44, 0x75 }, new ushort[2] { 0x4a, 0x75 }, new ushort[2] { 0xdb, 0x75 } };
        private static ushort[] stateExpectedIDs_99 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_99 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_99 = { "[rule_def_fragment<grammar_text_terminal> → rule_def_repetition<grammar_text_terminal> • _m143]", "[_m143 → • rule_def_repetition<grammar_text_terminal> _m143]", "[_m143 → •]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_99 = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_99 = { new ushort[2] { 0x88, 0xDB }, new ushort[2] { 0x81, 0xDC }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_99 = { new ushort[2] { 0x41, 0x73 }, new ushort[2] { 0x44, 0x73 }, new ushort[2] { 0x49, 0x73 }, new ushort[2] { 0x4a, 0x73 }, new ushort[2] { 0xdb, 0x73 } };
        private static ushort[] stateExpectedIDs_9A = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_9A = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_9A = { "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • *]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • +]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • ?]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_9A = { new ushort[2] { 0x45, 0xDD }, new ushort[2] { 0x46, 0xDE }, new ushort[2] { 0x47, 0xDF } };
        private static ushort[][] stateShiftsOnVariable_9A = {  };
        private static ushort[][] stateReducsOnTerminal_9A = { new ushort[2] { 0xa, 0x63 }, new ushort[2] { 0x11, 0x63 }, new ushort[2] { 0x2e, 0x63 }, new ushort[2] { 0x30, 0x63 }, new ushort[2] { 0x41, 0x63 }, new ushort[2] { 0x43, 0x63 }, new ushort[2] { 0x44, 0x63 }, new ushort[2] { 0x49, 0x63 }, new ushort[2] { 0x4a, 0x63 }, new ushort[2] { 0xdb, 0x63 } };
        private static ushort[] stateExpectedIDs_9B = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_9B = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_9B = { "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> • ^]", "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> • !]", "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_9B = { new ushort[2] { 0x55, 0xE0 }, new ushort[2] { 0x56, 0xE1 } };
        private static ushort[][] stateShiftsOnVariable_9B = {  };
        private static ushort[][] stateReducsOnTerminal_9B = { new ushort[2] { 0xa, 0x66 }, new ushort[2] { 0x11, 0x66 }, new ushort[2] { 0x2e, 0x66 }, new ushort[2] { 0x30, 0x66 }, new ushort[2] { 0x41, 0x66 }, new ushort[2] { 0x43, 0x66 }, new ushort[2] { 0x44, 0x66 }, new ushort[2] { 0x45, 0x66 }, new ushort[2] { 0x46, 0x66 }, new ushort[2] { 0x47, 0x66 }, new ushort[2] { 0x49, 0x66 }, new ushort[2] { 0x4a, 0x66 }, new ushort[2] { 0xdb, 0x66 } };
        private static ushort[] stateExpectedIDs_9C = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_9C = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_9C = { "[rule_def_element<grammar_text_terminal> → rule_def_atom<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_9C = {  };
        private static ushort[][] stateShiftsOnVariable_9C = {  };
        private static ushort[][] stateReducsOnTerminal_9C = { new ushort[2] { 0xa, 0x67 }, new ushort[2] { 0x11, 0x67 }, new ushort[2] { 0x2e, 0x67 }, new ushort[2] { 0x30, 0x67 }, new ushort[2] { 0x41, 0x67 }, new ushort[2] { 0x43, 0x67 }, new ushort[2] { 0x44, 0x67 }, new ushort[2] { 0x45, 0x67 }, new ushort[2] { 0x46, 0x67 }, new ushort[2] { 0x47, 0x67 }, new ushort[2] { 0x49, 0x67 }, new ushort[2] { 0x4a, 0x67 }, new ushort[2] { 0x55, 0x67 }, new ushort[2] { 0x56, 0x67 }, new ushort[2] { 0xdb, 0x67 } };
        private static ushort[] stateExpectedIDs_9D = { 0xA, 0x11, 0x2E, 0x30, 0x43, 0x44, 0x4A };
        private static string[] stateExpectedNames_9D = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[(]", "_T[)]", "_T[|]" };
        private static string[] stateItems_9D = { "[rule_def_element<grammar_text_terminal> → ( • rule_definition<grammar_text_terminal> )]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_9D = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_9D = { new ushort[2] { 0x7d, 0xE2 }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_9D = { new ushort[2] { 0x44, 0x5D }, new ushort[2] { 0x4a, 0x5D } };
        private static ushort[] stateExpectedIDs_9E = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_9E = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_9E = { "[rule_def_atom<grammar_text_terminal> → rule_sym_action •]" };
        private static ushort[][] stateShiftsOnTerminal_9E = {  };
        private static ushort[][] stateShiftsOnVariable_9E = {  };
        private static ushort[][] stateReducsOnTerminal_9E = { new ushort[2] { 0xa, 0x69 }, new ushort[2] { 0x11, 0x69 }, new ushort[2] { 0x2e, 0x69 }, new ushort[2] { 0x30, 0x69 }, new ushort[2] { 0x41, 0x69 }, new ushort[2] { 0x43, 0x69 }, new ushort[2] { 0x44, 0x69 }, new ushort[2] { 0x45, 0x69 }, new ushort[2] { 0x46, 0x69 }, new ushort[2] { 0x47, 0x69 }, new ushort[2] { 0x48, 0x69 }, new ushort[2] { 0x49, 0x69 }, new ushort[2] { 0x4a, 0x69 }, new ushort[2] { 0x4e, 0x69 }, new ushort[2] { 0x55, 0x69 }, new ushort[2] { 0x56, 0x69 }, new ushort[2] { 0xdb, 0x69 } };
        private static ushort[] stateExpectedIDs_9F = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_9F = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_9F = { "[rule_def_atom<grammar_text_terminal> → rule_sym_virtual •]" };
        private static ushort[][] stateShiftsOnTerminal_9F = {  };
        private static ushort[][] stateShiftsOnVariable_9F = {  };
        private static ushort[][] stateReducsOnTerminal_9F = { new ushort[2] { 0xa, 0x6A }, new ushort[2] { 0x11, 0x6A }, new ushort[2] { 0x2e, 0x6A }, new ushort[2] { 0x30, 0x6A }, new ushort[2] { 0x41, 0x6A }, new ushort[2] { 0x43, 0x6A }, new ushort[2] { 0x44, 0x6A }, new ushort[2] { 0x45, 0x6A }, new ushort[2] { 0x46, 0x6A }, new ushort[2] { 0x47, 0x6A }, new ushort[2] { 0x48, 0x6A }, new ushort[2] { 0x49, 0x6A }, new ushort[2] { 0x4a, 0x6A }, new ushort[2] { 0x4e, 0x6A }, new ushort[2] { 0x55, 0x6A }, new ushort[2] { 0x56, 0x6A }, new ushort[2] { 0xdb, 0x6A } };
        private static ushort[] stateExpectedIDs_A0 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A0 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A0 = { "[rule_def_atom<grammar_text_terminal> → rule_sym_ref_simple •]" };
        private static ushort[][] stateShiftsOnTerminal_A0 = {  };
        private static ushort[][] stateShiftsOnVariable_A0 = {  };
        private static ushort[][] stateReducsOnTerminal_A0 = { new ushort[2] { 0xa, 0x6B }, new ushort[2] { 0x11, 0x6B }, new ushort[2] { 0x2e, 0x6B }, new ushort[2] { 0x30, 0x6B }, new ushort[2] { 0x41, 0x6B }, new ushort[2] { 0x43, 0x6B }, new ushort[2] { 0x44, 0x6B }, new ushort[2] { 0x45, 0x6B }, new ushort[2] { 0x46, 0x6B }, new ushort[2] { 0x47, 0x6B }, new ushort[2] { 0x48, 0x6B }, new ushort[2] { 0x49, 0x6B }, new ushort[2] { 0x4a, 0x6B }, new ushort[2] { 0x4e, 0x6B }, new ushort[2] { 0x55, 0x6B }, new ushort[2] { 0x56, 0x6B }, new ushort[2] { 0xdb, 0x6B } };
        private static ushort[] stateExpectedIDs_A1 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A1 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A1 = { "[rule_def_atom<grammar_text_terminal> → rule_sym_ref_template<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_A1 = {  };
        private static ushort[][] stateShiftsOnVariable_A1 = {  };
        private static ushort[][] stateReducsOnTerminal_A1 = { new ushort[2] { 0xa, 0x6C }, new ushort[2] { 0x11, 0x6C }, new ushort[2] { 0x2e, 0x6C }, new ushort[2] { 0x30, 0x6C }, new ushort[2] { 0x41, 0x6C }, new ushort[2] { 0x43, 0x6C }, new ushort[2] { 0x44, 0x6C }, new ushort[2] { 0x45, 0x6C }, new ushort[2] { 0x46, 0x6C }, new ushort[2] { 0x47, 0x6C }, new ushort[2] { 0x48, 0x6C }, new ushort[2] { 0x49, 0x6C }, new ushort[2] { 0x4a, 0x6C }, new ushort[2] { 0x4e, 0x6C }, new ushort[2] { 0x55, 0x6C }, new ushort[2] { 0x56, 0x6C }, new ushort[2] { 0xdb, 0x6C } };
        private static ushort[] stateExpectedIDs_A2 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A2 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A2 = { "[rule_def_atom<grammar_text_terminal> → grammar_text_terminal •]" };
        private static ushort[][] stateShiftsOnTerminal_A2 = {  };
        private static ushort[][] stateShiftsOnVariable_A2 = {  };
        private static ushort[][] stateReducsOnTerminal_A2 = { new ushort[2] { 0xa, 0x6D }, new ushort[2] { 0x11, 0x6D }, new ushort[2] { 0x2e, 0x6D }, new ushort[2] { 0x30, 0x6D }, new ushort[2] { 0x41, 0x6D }, new ushort[2] { 0x43, 0x6D }, new ushort[2] { 0x44, 0x6D }, new ushort[2] { 0x45, 0x6D }, new ushort[2] { 0x46, 0x6D }, new ushort[2] { 0x47, 0x6D }, new ushort[2] { 0x48, 0x6D }, new ushort[2] { 0x49, 0x6D }, new ushort[2] { 0x4a, 0x6D }, new ushort[2] { 0x4e, 0x6D }, new ushort[2] { 0x55, 0x6D }, new ushort[2] { 0x56, 0x6D }, new ushort[2] { 0xdb, 0x6D } };
        private static ushort[] stateExpectedIDs_A3 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4D, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A3 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[<]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A3 = { "[rule_sym_ref_simple → NAME •]", "[rule_sym_ref_template<grammar_text_terminal> → NAME • rule_sym_ref_params<grammar_text_terminal>]", "[rule_sym_ref_params<grammar_text_terminal> → • < rule_def_atom<grammar_text_terminal> _m134 >]" };
        private static ushort[][] stateShiftsOnTerminal_A3 = { new ushort[2] { 0x4d, 0xE4 } };
        private static ushort[][] stateShiftsOnVariable_A3 = { new ushort[2] { 0x86, 0xE3 } };
        private static ushort[][] stateReducsOnTerminal_A3 = { new ushort[2] { 0xa, 0x32 }, new ushort[2] { 0x11, 0x32 }, new ushort[2] { 0x2e, 0x32 }, new ushort[2] { 0x30, 0x32 }, new ushort[2] { 0x41, 0x32 }, new ushort[2] { 0x43, 0x32 }, new ushort[2] { 0x44, 0x32 }, new ushort[2] { 0x45, 0x32 }, new ushort[2] { 0x46, 0x32 }, new ushort[2] { 0x47, 0x32 }, new ushort[2] { 0x48, 0x32 }, new ushort[2] { 0x49, 0x32 }, new ushort[2] { 0x4a, 0x32 }, new ushort[2] { 0x4e, 0x32 }, new ushort[2] { 0x55, 0x32 }, new ushort[2] { 0x56, 0x32 }, new ushort[2] { 0xdb, 0x32 } };
        private static ushort[] stateExpectedIDs_A4 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A4 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A4 = { "[grammar_text_terminal → terminal_def_atom_text •]" };
        private static ushort[][] stateShiftsOnTerminal_A4 = {  };
        private static ushort[][] stateShiftsOnVariable_A4 = {  };
        private static ushort[][] stateReducsOnTerminal_A4 = { new ushort[2] { 0xa, 0x40 }, new ushort[2] { 0x11, 0x40 }, new ushort[2] { 0x2e, 0x40 }, new ushort[2] { 0x30, 0x40 }, new ushort[2] { 0x41, 0x40 }, new ushort[2] { 0x43, 0x40 }, new ushort[2] { 0x44, 0x40 }, new ushort[2] { 0x45, 0x40 }, new ushort[2] { 0x46, 0x40 }, new ushort[2] { 0x47, 0x40 }, new ushort[2] { 0x48, 0x40 }, new ushort[2] { 0x49, 0x40 }, new ushort[2] { 0x4a, 0x40 }, new ushort[2] { 0x4e, 0x40 }, new ushort[2] { 0x55, 0x40 }, new ushort[2] { 0x56, 0x40 }, new ushort[2] { 0xdb, 0x40 } };
        private static ushort[] stateExpectedIDs_A5 = { 0xA, 0xB, 0x11, 0x2E, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_A5 = { "NAME", "_T[.]", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[=>]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_A5 = { "[terminal_def_atom_text → SYMBOL_TERMINAL_TEXT •]" };
        private static ushort[][] stateShiftsOnTerminal_A5 = {  };
        private static ushort[][] stateShiftsOnVariable_A5 = {  };
        private static ushort[][] stateReducsOnTerminal_A5 = { new ushort[2] { 0xa, 0x14 }, new ushort[2] { 0xb, 0x14 }, new ushort[2] { 0x11, 0x14 }, new ushort[2] { 0x2e, 0x14 }, new ushort[2] { 0x30, 0x14 }, new ushort[2] { 0x31, 0x14 }, new ushort[2] { 0x32, 0x14 }, new ushort[2] { 0x33, 0x14 }, new ushort[2] { 0x34, 0x14 }, new ushort[2] { 0x35, 0x14 }, new ushort[2] { 0x41, 0x14 }, new ushort[2] { 0x43, 0x14 }, new ushort[2] { 0x44, 0x14 }, new ushort[2] { 0x45, 0x14 }, new ushort[2] { 0x46, 0x14 }, new ushort[2] { 0x47, 0x14 }, new ushort[2] { 0x48, 0x14 }, new ushort[2] { 0x49, 0x14 }, new ushort[2] { 0x4a, 0x14 }, new ushort[2] { 0x4b, 0x14 }, new ushort[2] { 0x4e, 0x14 }, new ushort[2] { 0x55, 0x14 }, new ushort[2] { 0x56, 0x14 }, new ushort[2] { 0xdb, 0x14 } };
        private static ushort[] stateExpectedIDs_A6 = { 0x41, 0x4B };
        private static string[] stateExpectedNames_A6 = { "_T[;]", "_T[=>]" };
        private static string[] stateItems_A6 = { "[terminal → NAME -> terminal_definition • terminal_subgrammar ;]", "[terminal_subgrammar → • => qualified_name]", "[terminal_subgrammar → •]" };
        private static ushort[][] stateShiftsOnTerminal_A6 = { new ushort[2] { 0x4b, 0xE6 } };
        private static ushort[][] stateShiftsOnVariable_A6 = { new ushort[2] { 0x66, 0xE5 } };
        private static ushort[][] stateReducsOnTerminal_A6 = { new ushort[2] { 0x41, 0x2E } };
        private static ushort[] stateExpectedIDs_A7 = { 0x41, 0x44, 0x4A, 0x4B };
        private static string[] stateExpectedNames_A7 = { "_T[;]", "_T[)]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_A7 = { "[terminal_definition → terminal_def_restrict • _m93]", "[_m93 → • | terminal_def_restrict _m93]", "[_m93 → •]" };
        private static ushort[][] stateShiftsOnTerminal_A7 = { new ushort[2] { 0x4a, 0xE8 } };
        private static ushort[][] stateShiftsOnVariable_A7 = { new ushort[2] { 0x76, 0xE7 } };
        private static ushort[][] stateReducsOnTerminal_A7 = { new ushort[2] { 0x41, 0x50 }, new ushort[2] { 0x44, 0x50 }, new ushort[2] { 0x4b, 0x50 } };
        private static ushort[] stateExpectedIDs_A8 = { 0x41, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_A8 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_A8 = { "[terminal_def_restrict → terminal_def_fragment • _m91]", "[_m91 → • - terminal_def_fragment _m91]", "[_m91 → •]" };
        private static ushort[][] stateShiftsOnTerminal_A8 = { new ushort[2] { 0x49, 0xEA } };
        private static ushort[][] stateShiftsOnVariable_A8 = { new ushort[2] { 0x75, 0xE9 } };
        private static ushort[][] stateReducsOnTerminal_A8 = { new ushort[2] { 0x41, 0x4E }, new ushort[2] { 0x44, 0x4E }, new ushort[2] { 0x4a, 0x4E }, new ushort[2] { 0x4b, 0x4E } };
        private static ushort[] stateExpectedIDs_A9 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_A9 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_A9 = { "[terminal_def_fragment → terminal_def_repetition • _m89]", "[_m89 → • terminal_def_repetition _m89]", "[_m89 → •]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_A9 = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_A9 = { new ushort[2] { 0x74, 0xEB }, new ushort[2] { 0x62, 0xEC }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_A9 = { new ushort[2] { 0x41, 0x4C }, new ushort[2] { 0x44, 0x4C }, new ushort[2] { 0x49, 0x4C }, new ushort[2] { 0x4a, 0x4C }, new ushort[2] { 0x4b, 0x4C } };
        private static ushort[] stateExpectedIDs_AA = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_AA = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_AA = { "[terminal_def_repetition → terminal_def_element • terminal_def_cardinalilty]", "[terminal_def_repetition → terminal_def_element •]", "[terminal_def_cardinalilty → • *]", "[terminal_def_cardinalilty → • +]", "[terminal_def_cardinalilty → • ?]", "[terminal_def_cardinalilty → • { INTEGER , INTEGER }]", "[terminal_def_cardinalilty → • { INTEGER }]" };
        private static ushort[][] stateShiftsOnTerminal_AA = { new ushort[2] { 0x45, 0xEE }, new ushort[2] { 0x46, 0xEF }, new ushort[2] { 0x47, 0xF0 }, new ushort[2] { 0x11, 0xF1 } };
        private static ushort[][] stateShiftsOnVariable_AA = { new ushort[2] { 0x61, 0xED } };
        private static ushort[][] stateReducsOnTerminal_AA = { new ushort[2] { 0xa, 0x29 }, new ushort[2] { 0xb, 0x29 }, new ushort[2] { 0x30, 0x29 }, new ushort[2] { 0x31, 0x29 }, new ushort[2] { 0x32, 0x29 }, new ushort[2] { 0x33, 0x29 }, new ushort[2] { 0x34, 0x29 }, new ushort[2] { 0x35, 0x29 }, new ushort[2] { 0x41, 0x29 }, new ushort[2] { 0x43, 0x29 }, new ushort[2] { 0x44, 0x29 }, new ushort[2] { 0x49, 0x29 }, new ushort[2] { 0x4a, 0x29 }, new ushort[2] { 0x4b, 0x29 } };
        private static ushort[] stateExpectedIDs_AB = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_AB = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_AB = { "[terminal_def_element → terminal_def_atom •]" };
        private static ushort[][] stateShiftsOnTerminal_AB = {  };
        private static ushort[][] stateShiftsOnVariable_AB = {  };
        private static ushort[][] stateReducsOnTerminal_AB = { new ushort[2] { 0xa, 0x21 }, new ushort[2] { 0xb, 0x21 }, new ushort[2] { 0x11, 0x21 }, new ushort[2] { 0x30, 0x21 }, new ushort[2] { 0x31, 0x21 }, new ushort[2] { 0x32, 0x21 }, new ushort[2] { 0x33, 0x21 }, new ushort[2] { 0x34, 0x21 }, new ushort[2] { 0x35, 0x21 }, new ushort[2] { 0x41, 0x21 }, new ushort[2] { 0x43, 0x21 }, new ushort[2] { 0x44, 0x21 }, new ushort[2] { 0x45, 0x21 }, new ushort[2] { 0x46, 0x21 }, new ushort[2] { 0x47, 0x21 }, new ushort[2] { 0x49, 0x21 }, new ushort[2] { 0x4a, 0x21 }, new ushort[2] { 0x4b, 0x21 } };
        private static ushort[] stateExpectedIDs_AC = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x43 };
        private static string[] stateExpectedNames_AC = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[(]" };
        private static string[] stateItems_AC = { "[terminal_def_element → ( • terminal_definition )]", "[terminal_definition → • terminal_def_restrict _m93]", "[terminal_def_restrict → • terminal_def_fragment _m91]", "[terminal_def_fragment → • terminal_def_repetition _m89]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_AC = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_AC = { new ushort[2] { 0x65, 0xF2 }, new ushort[2] { 0x64, 0xA7 }, new ushort[2] { 0x63, 0xA8 }, new ushort[2] { 0x62, 0xA9 }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_AC = {  };
        private static ushort[] stateExpectedIDs_AD = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_AD = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_AD = { "[terminal_def_atom → terminal_def_atom_any •]" };
        private static ushort[][] stateShiftsOnTerminal_AD = {  };
        private static ushort[][] stateShiftsOnVariable_AD = {  };
        private static ushort[][] stateReducsOnTerminal_AD = { new ushort[2] { 0xa, 0x19 }, new ushort[2] { 0xb, 0x19 }, new ushort[2] { 0x11, 0x19 }, new ushort[2] { 0x30, 0x19 }, new ushort[2] { 0x31, 0x19 }, new ushort[2] { 0x32, 0x19 }, new ushort[2] { 0x33, 0x19 }, new ushort[2] { 0x34, 0x19 }, new ushort[2] { 0x35, 0x19 }, new ushort[2] { 0x41, 0x19 }, new ushort[2] { 0x43, 0x19 }, new ushort[2] { 0x44, 0x19 }, new ushort[2] { 0x45, 0x19 }, new ushort[2] { 0x46, 0x19 }, new ushort[2] { 0x47, 0x19 }, new ushort[2] { 0x49, 0x19 }, new ushort[2] { 0x4a, 0x19 }, new ushort[2] { 0x4b, 0x19 } };
        private static ushort[] stateExpectedIDs_AE = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_AE = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[..]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_AE = { "[terminal_def_atom → terminal_def_atom_unicode •]", "[terminal_def_atom_span → terminal_def_atom_unicode • .. terminal_def_atom_unicode]" };
        private static ushort[][] stateShiftsOnTerminal_AE = { new ushort[2] { 0x42, 0xF3 } };
        private static ushort[][] stateShiftsOnVariable_AE = {  };
        private static ushort[][] stateReducsOnTerminal_AE = { new ushort[2] { 0xa, 0x1A }, new ushort[2] { 0xb, 0x1A }, new ushort[2] { 0x11, 0x1A }, new ushort[2] { 0x30, 0x1A }, new ushort[2] { 0x31, 0x1A }, new ushort[2] { 0x32, 0x1A }, new ushort[2] { 0x33, 0x1A }, new ushort[2] { 0x34, 0x1A }, new ushort[2] { 0x35, 0x1A }, new ushort[2] { 0x41, 0x1A }, new ushort[2] { 0x43, 0x1A }, new ushort[2] { 0x44, 0x1A }, new ushort[2] { 0x45, 0x1A }, new ushort[2] { 0x46, 0x1A }, new ushort[2] { 0x47, 0x1A }, new ushort[2] { 0x49, 0x1A }, new ushort[2] { 0x4a, 0x1A }, new ushort[2] { 0x4b, 0x1A } };
        private static ushort[] stateExpectedIDs_AF = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_AF = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_AF = { "[terminal_def_atom → terminal_def_atom_text •]" };
        private static ushort[][] stateShiftsOnTerminal_AF = {  };
        private static ushort[][] stateShiftsOnVariable_AF = {  };
        private static ushort[][] stateReducsOnTerminal_AF = { new ushort[2] { 0xa, 0x1B }, new ushort[2] { 0xb, 0x1B }, new ushort[2] { 0x11, 0x1B }, new ushort[2] { 0x30, 0x1B }, new ushort[2] { 0x31, 0x1B }, new ushort[2] { 0x32, 0x1B }, new ushort[2] { 0x33, 0x1B }, new ushort[2] { 0x34, 0x1B }, new ushort[2] { 0x35, 0x1B }, new ushort[2] { 0x41, 0x1B }, new ushort[2] { 0x43, 0x1B }, new ushort[2] { 0x44, 0x1B }, new ushort[2] { 0x45, 0x1B }, new ushort[2] { 0x46, 0x1B }, new ushort[2] { 0x47, 0x1B }, new ushort[2] { 0x49, 0x1B }, new ushort[2] { 0x4a, 0x1B }, new ushort[2] { 0x4b, 0x1B } };
        private static ushort[] stateExpectedIDs_B0 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B0 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B0 = { "[terminal_def_atom → terminal_def_atom_set •]" };
        private static ushort[][] stateShiftsOnTerminal_B0 = {  };
        private static ushort[][] stateShiftsOnVariable_B0 = {  };
        private static ushort[][] stateReducsOnTerminal_B0 = { new ushort[2] { 0xa, 0x1C }, new ushort[2] { 0xb, 0x1C }, new ushort[2] { 0x11, 0x1C }, new ushort[2] { 0x30, 0x1C }, new ushort[2] { 0x31, 0x1C }, new ushort[2] { 0x32, 0x1C }, new ushort[2] { 0x33, 0x1C }, new ushort[2] { 0x34, 0x1C }, new ushort[2] { 0x35, 0x1C }, new ushort[2] { 0x41, 0x1C }, new ushort[2] { 0x43, 0x1C }, new ushort[2] { 0x44, 0x1C }, new ushort[2] { 0x45, 0x1C }, new ushort[2] { 0x46, 0x1C }, new ushort[2] { 0x47, 0x1C }, new ushort[2] { 0x49, 0x1C }, new ushort[2] { 0x4a, 0x1C }, new ushort[2] { 0x4b, 0x1C } };
        private static ushort[] stateExpectedIDs_B1 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B1 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B1 = { "[terminal_def_atom → terminal_def_atom_span •]" };
        private static ushort[][] stateShiftsOnTerminal_B1 = {  };
        private static ushort[][] stateShiftsOnVariable_B1 = {  };
        private static ushort[][] stateReducsOnTerminal_B1 = { new ushort[2] { 0xa, 0x1D }, new ushort[2] { 0xb, 0x1D }, new ushort[2] { 0x11, 0x1D }, new ushort[2] { 0x30, 0x1D }, new ushort[2] { 0x31, 0x1D }, new ushort[2] { 0x32, 0x1D }, new ushort[2] { 0x33, 0x1D }, new ushort[2] { 0x34, 0x1D }, new ushort[2] { 0x35, 0x1D }, new ushort[2] { 0x41, 0x1D }, new ushort[2] { 0x43, 0x1D }, new ushort[2] { 0x44, 0x1D }, new ushort[2] { 0x45, 0x1D }, new ushort[2] { 0x46, 0x1D }, new ushort[2] { 0x47, 0x1D }, new ushort[2] { 0x49, 0x1D }, new ushort[2] { 0x4a, 0x1D }, new ushort[2] { 0x4b, 0x1D } };
        private static ushort[] stateExpectedIDs_B2 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B2 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B2 = { "[terminal_def_atom → terminal_def_atom_ucat •]" };
        private static ushort[][] stateShiftsOnTerminal_B2 = {  };
        private static ushort[][] stateShiftsOnVariable_B2 = {  };
        private static ushort[][] stateReducsOnTerminal_B2 = { new ushort[2] { 0xa, 0x1E }, new ushort[2] { 0xb, 0x1E }, new ushort[2] { 0x11, 0x1E }, new ushort[2] { 0x30, 0x1E }, new ushort[2] { 0x31, 0x1E }, new ushort[2] { 0x32, 0x1E }, new ushort[2] { 0x33, 0x1E }, new ushort[2] { 0x34, 0x1E }, new ushort[2] { 0x35, 0x1E }, new ushort[2] { 0x41, 0x1E }, new ushort[2] { 0x43, 0x1E }, new ushort[2] { 0x44, 0x1E }, new ushort[2] { 0x45, 0x1E }, new ushort[2] { 0x46, 0x1E }, new ushort[2] { 0x47, 0x1E }, new ushort[2] { 0x49, 0x1E }, new ushort[2] { 0x4a, 0x1E }, new ushort[2] { 0x4b, 0x1E } };
        private static ushort[] stateExpectedIDs_B3 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B3 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B3 = { "[terminal_def_atom → terminal_def_atom_ublock •]" };
        private static ushort[][] stateShiftsOnTerminal_B3 = {  };
        private static ushort[][] stateShiftsOnVariable_B3 = {  };
        private static ushort[][] stateReducsOnTerminal_B3 = { new ushort[2] { 0xa, 0x1F }, new ushort[2] { 0xb, 0x1F }, new ushort[2] { 0x11, 0x1F }, new ushort[2] { 0x30, 0x1F }, new ushort[2] { 0x31, 0x1F }, new ushort[2] { 0x32, 0x1F }, new ushort[2] { 0x33, 0x1F }, new ushort[2] { 0x34, 0x1F }, new ushort[2] { 0x35, 0x1F }, new ushort[2] { 0x41, 0x1F }, new ushort[2] { 0x43, 0x1F }, new ushort[2] { 0x44, 0x1F }, new ushort[2] { 0x45, 0x1F }, new ushort[2] { 0x46, 0x1F }, new ushort[2] { 0x47, 0x1F }, new ushort[2] { 0x49, 0x1F }, new ushort[2] { 0x4a, 0x1F }, new ushort[2] { 0x4b, 0x1F } };
        private static ushort[] stateExpectedIDs_B4 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B4 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B4 = { "[terminal_def_atom → NAME •]" };
        private static ushort[][] stateShiftsOnTerminal_B4 = {  };
        private static ushort[][] stateShiftsOnVariable_B4 = {  };
        private static ushort[][] stateReducsOnTerminal_B4 = { new ushort[2] { 0xa, 0x20 }, new ushort[2] { 0xb, 0x20 }, new ushort[2] { 0x11, 0x20 }, new ushort[2] { 0x30, 0x20 }, new ushort[2] { 0x31, 0x20 }, new ushort[2] { 0x32, 0x20 }, new ushort[2] { 0x33, 0x20 }, new ushort[2] { 0x34, 0x20 }, new ushort[2] { 0x35, 0x20 }, new ushort[2] { 0x41, 0x20 }, new ushort[2] { 0x43, 0x20 }, new ushort[2] { 0x44, 0x20 }, new ushort[2] { 0x45, 0x20 }, new ushort[2] { 0x46, 0x20 }, new ushort[2] { 0x47, 0x20 }, new ushort[2] { 0x49, 0x20 }, new ushort[2] { 0x4a, 0x20 }, new ushort[2] { 0x4b, 0x20 } };
        private static ushort[] stateExpectedIDs_B5 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B5 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B5 = { "[terminal_def_atom_any → . •]" };
        private static ushort[][] stateShiftsOnTerminal_B5 = {  };
        private static ushort[][] stateShiftsOnVariable_B5 = {  };
        private static ushort[][] stateReducsOnTerminal_B5 = { new ushort[2] { 0xa, 0x11 }, new ushort[2] { 0xb, 0x11 }, new ushort[2] { 0x11, 0x11 }, new ushort[2] { 0x30, 0x11 }, new ushort[2] { 0x31, 0x11 }, new ushort[2] { 0x32, 0x11 }, new ushort[2] { 0x33, 0x11 }, new ushort[2] { 0x34, 0x11 }, new ushort[2] { 0x35, 0x11 }, new ushort[2] { 0x41, 0x11 }, new ushort[2] { 0x43, 0x11 }, new ushort[2] { 0x44, 0x11 }, new ushort[2] { 0x45, 0x11 }, new ushort[2] { 0x46, 0x11 }, new ushort[2] { 0x47, 0x11 }, new ushort[2] { 0x49, 0x11 }, new ushort[2] { 0x4a, 0x11 }, new ushort[2] { 0x4b, 0x11 } };
        private static ushort[] stateExpectedIDs_B6 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B6 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[..]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B6 = { "[terminal_def_atom_unicode → SYMBOL_VALUE_UINT8 •]" };
        private static ushort[][] stateShiftsOnTerminal_B6 = {  };
        private static ushort[][] stateShiftsOnVariable_B6 = {  };
        private static ushort[][] stateReducsOnTerminal_B6 = { new ushort[2] { 0xa, 0x12 }, new ushort[2] { 0xb, 0x12 }, new ushort[2] { 0x11, 0x12 }, new ushort[2] { 0x30, 0x12 }, new ushort[2] { 0x31, 0x12 }, new ushort[2] { 0x32, 0x12 }, new ushort[2] { 0x33, 0x12 }, new ushort[2] { 0x34, 0x12 }, new ushort[2] { 0x35, 0x12 }, new ushort[2] { 0x41, 0x12 }, new ushort[2] { 0x42, 0x12 }, new ushort[2] { 0x43, 0x12 }, new ushort[2] { 0x44, 0x12 }, new ushort[2] { 0x45, 0x12 }, new ushort[2] { 0x46, 0x12 }, new ushort[2] { 0x47, 0x12 }, new ushort[2] { 0x49, 0x12 }, new ushort[2] { 0x4a, 0x12 }, new ushort[2] { 0x4b, 0x12 } };
        private static ushort[] stateExpectedIDs_B7 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B7 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[..]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B7 = { "[terminal_def_atom_unicode → SYMBOL_VALUE_UINT16 •]" };
        private static ushort[][] stateShiftsOnTerminal_B7 = {  };
        private static ushort[][] stateShiftsOnVariable_B7 = {  };
        private static ushort[][] stateReducsOnTerminal_B7 = { new ushort[2] { 0xa, 0x13 }, new ushort[2] { 0xb, 0x13 }, new ushort[2] { 0x11, 0x13 }, new ushort[2] { 0x30, 0x13 }, new ushort[2] { 0x31, 0x13 }, new ushort[2] { 0x32, 0x13 }, new ushort[2] { 0x33, 0x13 }, new ushort[2] { 0x34, 0x13 }, new ushort[2] { 0x35, 0x13 }, new ushort[2] { 0x41, 0x13 }, new ushort[2] { 0x42, 0x13 }, new ushort[2] { 0x43, 0x13 }, new ushort[2] { 0x44, 0x13 }, new ushort[2] { 0x45, 0x13 }, new ushort[2] { 0x46, 0x13 }, new ushort[2] { 0x47, 0x13 }, new ushort[2] { 0x49, 0x13 }, new ushort[2] { 0x4a, 0x13 }, new ushort[2] { 0x4b, 0x13 } };
        private static ushort[] stateExpectedIDs_B8 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B8 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B8 = { "[terminal_def_atom_set → SYMBOL_TERMINAL_SET •]" };
        private static ushort[][] stateShiftsOnTerminal_B8 = {  };
        private static ushort[][] stateShiftsOnVariable_B8 = {  };
        private static ushort[][] stateReducsOnTerminal_B8 = { new ushort[2] { 0xa, 0x15 }, new ushort[2] { 0xb, 0x15 }, new ushort[2] { 0x11, 0x15 }, new ushort[2] { 0x30, 0x15 }, new ushort[2] { 0x31, 0x15 }, new ushort[2] { 0x32, 0x15 }, new ushort[2] { 0x33, 0x15 }, new ushort[2] { 0x34, 0x15 }, new ushort[2] { 0x35, 0x15 }, new ushort[2] { 0x41, 0x15 }, new ushort[2] { 0x43, 0x15 }, new ushort[2] { 0x44, 0x15 }, new ushort[2] { 0x45, 0x15 }, new ushort[2] { 0x46, 0x15 }, new ushort[2] { 0x47, 0x15 }, new ushort[2] { 0x49, 0x15 }, new ushort[2] { 0x4a, 0x15 }, new ushort[2] { 0x4b, 0x15 } };
        private static ushort[] stateExpectedIDs_B9 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_B9 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_B9 = { "[terminal_def_atom_ucat → SYMBOL_TERMINAL_UCAT •]" };
        private static ushort[][] stateShiftsOnTerminal_B9 = {  };
        private static ushort[][] stateShiftsOnVariable_B9 = {  };
        private static ushort[][] stateReducsOnTerminal_B9 = { new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x17 }, new ushort[2] { 0x11, 0x17 }, new ushort[2] { 0x30, 0x17 }, new ushort[2] { 0x31, 0x17 }, new ushort[2] { 0x32, 0x17 }, new ushort[2] { 0x33, 0x17 }, new ushort[2] { 0x34, 0x17 }, new ushort[2] { 0x35, 0x17 }, new ushort[2] { 0x41, 0x17 }, new ushort[2] { 0x43, 0x17 }, new ushort[2] { 0x44, 0x17 }, new ushort[2] { 0x45, 0x17 }, new ushort[2] { 0x46, 0x17 }, new ushort[2] { 0x47, 0x17 }, new ushort[2] { 0x49, 0x17 }, new ushort[2] { 0x4a, 0x17 }, new ushort[2] { 0x4b, 0x17 } };
        private static ushort[] stateExpectedIDs_BA = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_BA = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_BA = { "[terminal_def_atom_ublock → SYMBOL_TERMINAL_UBLOCK •]" };
        private static ushort[][] stateShiftsOnTerminal_BA = {  };
        private static ushort[][] stateShiftsOnVariable_BA = {  };
        private static ushort[][] stateReducsOnTerminal_BA = { new ushort[2] { 0xa, 0x16 }, new ushort[2] { 0xb, 0x16 }, new ushort[2] { 0x11, 0x16 }, new ushort[2] { 0x30, 0x16 }, new ushort[2] { 0x31, 0x16 }, new ushort[2] { 0x32, 0x16 }, new ushort[2] { 0x33, 0x16 }, new ushort[2] { 0x34, 0x16 }, new ushort[2] { 0x35, 0x16 }, new ushort[2] { 0x41, 0x16 }, new ushort[2] { 0x43, 0x16 }, new ushort[2] { 0x44, 0x16 }, new ushort[2] { 0x45, 0x16 }, new ushort[2] { 0x46, 0x16 }, new ushort[2] { 0x47, 0x16 }, new ushort[2] { 0x49, 0x16 }, new ushort[2] { 0x4a, 0x16 }, new ushort[2] { 0x4b, 0x16 } };
        private static ushort[] stateExpectedIDs_BB = { 0x4C, 0x4D };
        private static string[] stateExpectedNames_BB = { "_T[->]", "_T[<]" };
        private static string[] stateItems_BB = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> • -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> • rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params → • < NAME _m101 >]" };
        private static ushort[][] stateShiftsOnTerminal_BB = { new ushort[2] { 0x4c, 0xF4 }, new ushort[2] { 0x4d, 0x90 } };
        private static ushort[][] stateShiftsOnVariable_BB = { new ushort[2] { 0x6b, 0xF5 } };
        private static ushort[][] stateReducsOnTerminal_BB = {  };
        private static ushort[] stateExpectedIDs_BC = { 0xA, 0x4C, 0x4D };
        private static string[] stateExpectedNames_BC = { "NAME", "_T[->]", "_T[<]" };
        private static string[] stateItems_BC = { "[cs_rule_context<grammar_bin_terminal> → [ rule_definition<grammar_bin_terminal> ] •]" };
        private static ushort[][] stateShiftsOnTerminal_BC = {  };
        private static ushort[][] stateShiftsOnVariable_BC = {  };
        private static ushort[][] stateReducsOnTerminal_BC = { new ushort[2] { 0xa, 0xAB }, new ushort[2] { 0x4c, 0xAB }, new ushort[2] { 0x4d, 0xAB } };
        private static ushort[] stateExpectedIDs_BD = { 0x41, 0x44, 0xDB };
        private static string[] stateExpectedNames_BD = { "_T[;]", "_T[)]", "_T[]]" };
        private static string[] stateItems_BD = { "[rule_definition<grammar_bin_terminal> → rule_def_choice<grammar_bin_terminal> _m188 •]" };
        private static ushort[][] stateShiftsOnTerminal_BD = {  };
        private static ushort[][] stateShiftsOnVariable_BD = {  };
        private static ushort[][] stateReducsOnTerminal_BD = { new ushort[2] { 0x41, 0x7E }, new ushort[2] { 0x44, 0x7E }, new ushort[2] { 0xdb, 0x7E } };
        private static ushort[] stateExpectedIDs_BE = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_BE = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_BE = { "[_m188 → | • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_BE = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_BE = { new ushort[2] { 0x90, 0xF6 }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_BE = { new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x44, 0x80 }, new ushort[2] { 0x4a, 0x80 }, new ushort[2] { 0xdb, 0x80 } };
        private static ushort[] stateExpectedIDs_BF = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_BF = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_BF = { "[rule_def_restrict<grammar_bin_terminal> → rule_def_fragment<grammar_bin_terminal> _m186 •]" };
        private static ushort[][] stateShiftsOnTerminal_BF = {  };
        private static ushort[][] stateShiftsOnVariable_BF = {  };
        private static ushort[][] stateReducsOnTerminal_BF = { new ushort[2] { 0x41, 0x81 }, new ushort[2] { 0x44, 0x81 }, new ushort[2] { 0x4a, 0x81 }, new ushort[2] { 0xdb, 0x81 } };
        private static ushort[] stateExpectedIDs_C0 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x43 };
        private static string[] stateExpectedNames_C0 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[(]" };
        private static string[] stateItems_C0 = { "[_m186 → - • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_C0 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_C0 = { new ushort[2] { 0x92, 0xF7 }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_C0 = {  };
        private static ushort[] stateExpectedIDs_C1 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C1 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C1 = { "[rule_def_fragment<grammar_bin_terminal> → rule_def_repetition<grammar_bin_terminal> _m184 •]" };
        private static ushort[][] stateShiftsOnTerminal_C1 = {  };
        private static ushort[][] stateShiftsOnVariable_C1 = {  };
        private static ushort[][] stateReducsOnTerminal_C1 = { new ushort[2] { 0x41, 0x82 }, new ushort[2] { 0x44, 0x82 }, new ushort[2] { 0x49, 0x82 }, new ushort[2] { 0x4a, 0x82 }, new ushort[2] { 0xdb, 0x82 } };
        private static ushort[] stateExpectedIDs_C2 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C2 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C2 = { "[_m184 → rule_def_repetition<grammar_bin_terminal> • _m184]", "[_m184 → • rule_def_repetition<grammar_bin_terminal> _m184]", "[_m184 → •]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_C2 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_C2 = { new ushort[2] { 0x9a, 0xF8 }, new ushort[2] { 0x93, 0xC2 }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_C2 = { new ushort[2] { 0x41, 0x96 }, new ushort[2] { 0x44, 0x96 }, new ushort[2] { 0x49, 0x96 }, new ushort[2] { 0x4a, 0x96 }, new ushort[2] { 0xdb, 0x96 } };
        private static ushort[] stateExpectedIDs_C3 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C3 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C3 = { "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> * •]" };
        private static ushort[][] stateShiftsOnTerminal_C3 = {  };
        private static ushort[][] stateShiftsOnVariable_C3 = {  };
        private static ushort[][] stateReducsOnTerminal_C3 = { new ushort[2] { 0xa, 0x83 }, new ushort[2] { 0x11, 0x83 }, new ushort[2] { 0x2e, 0x83 }, new ushort[2] { 0x34, 0x83 }, new ushort[2] { 0x35, 0x83 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x83 }, new ushort[2] { 0x38, 0x83 }, new ushort[2] { 0x39, 0x83 }, new ushort[2] { 0x3a, 0x83 }, new ushort[2] { 0x3b, 0x83 }, new ushort[2] { 0x3c, 0x83 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x83 }, new ushort[2] { 0x3f, 0x83 }, new ushort[2] { 0x41, 0x83 }, new ushort[2] { 0x43, 0x83 }, new ushort[2] { 0x44, 0x83 }, new ushort[2] { 0x49, 0x83 }, new ushort[2] { 0x4a, 0x83 }, new ushort[2] { 0xdb, 0x83 } };
        private static ushort[] stateExpectedIDs_C4 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C4 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C4 = { "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> + •]" };
        private static ushort[][] stateShiftsOnTerminal_C4 = {  };
        private static ushort[][] stateShiftsOnVariable_C4 = {  };
        private static ushort[][] stateReducsOnTerminal_C4 = { new ushort[2] { 0xa, 0x84 }, new ushort[2] { 0x11, 0x84 }, new ushort[2] { 0x2e, 0x84 }, new ushort[2] { 0x34, 0x84 }, new ushort[2] { 0x35, 0x84 }, new ushort[2] { 0x36, 0x84 }, new ushort[2] { 0x37, 0x84 }, new ushort[2] { 0x38, 0x84 }, new ushort[2] { 0x39, 0x84 }, new ushort[2] { 0x3a, 0x84 }, new ushort[2] { 0x3b, 0x84 }, new ushort[2] { 0x3c, 0x84 }, new ushort[2] { 0x3d, 0x84 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x84 }, new ushort[2] { 0x41, 0x84 }, new ushort[2] { 0x43, 0x84 }, new ushort[2] { 0x44, 0x84 }, new ushort[2] { 0x49, 0x84 }, new ushort[2] { 0x4a, 0x84 }, new ushort[2] { 0xdb, 0x84 } };
        private static ushort[] stateExpectedIDs_C5 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C5 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C5 = { "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> ? •]" };
        private static ushort[][] stateShiftsOnTerminal_C5 = {  };
        private static ushort[][] stateShiftsOnVariable_C5 = {  };
        private static ushort[][] stateReducsOnTerminal_C5 = { new ushort[2] { 0xa, 0x85 }, new ushort[2] { 0x11, 0x85 }, new ushort[2] { 0x2e, 0x85 }, new ushort[2] { 0x34, 0x85 }, new ushort[2] { 0x35, 0x85 }, new ushort[2] { 0x36, 0x85 }, new ushort[2] { 0x37, 0x85 }, new ushort[2] { 0x38, 0x85 }, new ushort[2] { 0x39, 0x85 }, new ushort[2] { 0x3a, 0x85 }, new ushort[2] { 0x3b, 0x85 }, new ushort[2] { 0x3c, 0x85 }, new ushort[2] { 0x3d, 0x85 }, new ushort[2] { 0x3e, 0x85 }, new ushort[2] { 0x3f, 0x85 }, new ushort[2] { 0x41, 0x85 }, new ushort[2] { 0x43, 0x85 }, new ushort[2] { 0x44, 0x85 }, new ushort[2] { 0x49, 0x85 }, new ushort[2] { 0x4a, 0x85 }, new ushort[2] { 0xdb, 0x85 } };
        private static ushort[] stateExpectedIDs_C6 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C6 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C6 = { "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> ^ •]" };
        private static ushort[][] stateShiftsOnTerminal_C6 = {  };
        private static ushort[][] stateShiftsOnVariable_C6 = {  };
        private static ushort[][] stateReducsOnTerminal_C6 = { new ushort[2] { 0xa, 0x87 }, new ushort[2] { 0x11, 0x87 }, new ushort[2] { 0x2e, 0x87 }, new ushort[2] { 0x34, 0x87 }, new ushort[2] { 0x35, 0x87 }, new ushort[2] { 0x36, 0x87 }, new ushort[2] { 0x37, 0x87 }, new ushort[2] { 0x38, 0x87 }, new ushort[2] { 0x39, 0x87 }, new ushort[2] { 0x3a, 0x87 }, new ushort[2] { 0x3b, 0x87 }, new ushort[2] { 0x3c, 0x87 }, new ushort[2] { 0x3d, 0x87 }, new ushort[2] { 0x3e, 0x87 }, new ushort[2] { 0x3f, 0x87 }, new ushort[2] { 0x41, 0x87 }, new ushort[2] { 0x43, 0x87 }, new ushort[2] { 0x44, 0x87 }, new ushort[2] { 0x45, 0x87 }, new ushort[2] { 0x46, 0x87 }, new ushort[2] { 0x47, 0x87 }, new ushort[2] { 0x49, 0x87 }, new ushort[2] { 0x4a, 0x87 }, new ushort[2] { 0xdb, 0x87 } };
        private static ushort[] stateExpectedIDs_C7 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_C7 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_C7 = { "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> ! •]" };
        private static ushort[][] stateShiftsOnTerminal_C7 = {  };
        private static ushort[][] stateShiftsOnVariable_C7 = {  };
        private static ushort[][] stateReducsOnTerminal_C7 = { new ushort[2] { 0xa, 0x88 }, new ushort[2] { 0x11, 0x88 }, new ushort[2] { 0x2e, 0x88 }, new ushort[2] { 0x34, 0x88 }, new ushort[2] { 0x35, 0x88 }, new ushort[2] { 0x36, 0x88 }, new ushort[2] { 0x37, 0x88 }, new ushort[2] { 0x38, 0x88 }, new ushort[2] { 0x39, 0x88 }, new ushort[2] { 0x3a, 0x88 }, new ushort[2] { 0x3b, 0x88 }, new ushort[2] { 0x3c, 0x88 }, new ushort[2] { 0x3d, 0x88 }, new ushort[2] { 0x3e, 0x88 }, new ushort[2] { 0x3f, 0x88 }, new ushort[2] { 0x41, 0x88 }, new ushort[2] { 0x43, 0x88 }, new ushort[2] { 0x44, 0x88 }, new ushort[2] { 0x45, 0x88 }, new ushort[2] { 0x46, 0x88 }, new ushort[2] { 0x47, 0x88 }, new ushort[2] { 0x49, 0x88 }, new ushort[2] { 0x4a, 0x88 }, new ushort[2] { 0xdb, 0x88 } };
        private static ushort[] stateExpectedIDs_C8 = { 0x44 };
        private static string[] stateExpectedNames_C8 = { "_T[)]" };
        private static string[] stateItems_C8 = { "[rule_def_element<grammar_bin_terminal> → ( rule_definition<grammar_bin_terminal> • )]" };
        private static ushort[][] stateShiftsOnTerminal_C8 = { new ushort[2] { 0x44, 0xF9 } };
        private static ushort[][] stateShiftsOnVariable_C8 = {  };
        private static ushort[][] stateReducsOnTerminal_C8 = {  };
        private static ushort[] stateExpectedIDs_C9 = { 0x12 };
        private static string[] stateExpectedNames_C9 = { "_T[}]" };
        private static string[] stateItems_C9 = { "[rule_sym_action → { NAME • }]" };
        private static ushort[][] stateShiftsOnTerminal_C9 = { new ushort[2] { 0x12, 0xFA } };
        private static ushort[][] stateShiftsOnVariable_C9 = {  };
        private static ushort[][] stateReducsOnTerminal_C9 = {  };
        private static ushort[] stateExpectedIDs_CA = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_CA = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_CA = { "[rule_sym_ref_template<grammar_bin_terminal> → NAME rule_sym_ref_params<grammar_bin_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_CA = {  };
        private static ushort[][] stateShiftsOnVariable_CA = {  };
        private static ushort[][] stateReducsOnTerminal_CA = { new ushort[2] { 0xa, 0x91 }, new ushort[2] { 0x11, 0x91 }, new ushort[2] { 0x2e, 0x91 }, new ushort[2] { 0x34, 0x91 }, new ushort[2] { 0x35, 0x91 }, new ushort[2] { 0x36, 0x91 }, new ushort[2] { 0x37, 0x91 }, new ushort[2] { 0x38, 0x91 }, new ushort[2] { 0x39, 0x91 }, new ushort[2] { 0x3a, 0x91 }, new ushort[2] { 0x3b, 0x91 }, new ushort[2] { 0x3c, 0x91 }, new ushort[2] { 0x3d, 0x91 }, new ushort[2] { 0x3e, 0x91 }, new ushort[2] { 0x3f, 0x91 }, new ushort[2] { 0x41, 0x91 }, new ushort[2] { 0x43, 0x91 }, new ushort[2] { 0x44, 0x91 }, new ushort[2] { 0x45, 0x91 }, new ushort[2] { 0x46, 0x91 }, new ushort[2] { 0x47, 0x91 }, new ushort[2] { 0x48, 0x91 }, new ushort[2] { 0x49, 0x91 }, new ushort[2] { 0x4a, 0x91 }, new ushort[2] { 0x4e, 0x91 }, new ushort[2] { 0x55, 0x91 }, new ushort[2] { 0x56, 0x91 }, new ushort[2] { 0xdb, 0x91 } };
        private static ushort[] stateExpectedIDs_CB = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F };
        private static string[] stateExpectedNames_CB = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] stateItems_CB = { "[rule_sym_ref_params<grammar_bin_terminal> → < • rule_def_atom<grammar_bin_terminal> _m175 >]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_CB = { new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_CB = { new ushort[2] { 0x96, 0xFB }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_CB = {  };
        private static ushort[] stateExpectedIDs_CC = { 0xA, 0x12 };
        private static string[] stateExpectedNames_CC = { "NAME", "_T[}]" };
        private static string[] stateItems_CC = { "[option → NAME = QUOTED_DATA ; •]" };
        private static ushort[][] stateShiftsOnTerminal_CC = {  };
        private static ushort[][] stateShiftsOnVariable_CC = {  };
        private static ushort[][] stateReducsOnTerminal_CC = { new ushort[2] { 0xa, 0x10 }, new ushort[2] { 0x12, 0x10 } };
        private static ushort[] stateExpectedIDs_CD = { 0x12 };
        private static string[] stateExpectedNames_CD = { "_T[}]" };
        private static string[] stateItems_CD = { "[grammar_cf_rules<grammar_text_terminal> → rules { _m152 } •]" };
        private static ushort[][] stateShiftsOnTerminal_CD = {  };
        private static ushort[][] stateShiftsOnVariable_CD = {  };
        private static ushort[][] stateReducsOnTerminal_CD = { new ushort[2] { 0x12, 0x59 } };
        private static ushort[] stateExpectedIDs_CE = { 0x12 };
        private static string[] stateExpectedNames_CE = { "_T[}]" };
        private static string[] stateItems_CE = { "[_m152 → cf_rule_simple<grammar_text_terminal> _m152 •]" };
        private static ushort[][] stateShiftsOnTerminal_CE = {  };
        private static ushort[][] stateShiftsOnVariable_CE = {  };
        private static ushort[][] stateReducsOnTerminal_CE = { new ushort[2] { 0x12, 0x79 } };
        private static ushort[] stateExpectedIDs_CF = { 0x12 };
        private static string[] stateExpectedNames_CF = { "_T[}]" };
        private static string[] stateItems_CF = { "[_m152 → cf_rule_template<grammar_text_terminal> _m152 •]" };
        private static ushort[][] stateShiftsOnTerminal_CF = {  };
        private static ushort[][] stateShiftsOnVariable_CF = {  };
        private static ushort[][] stateReducsOnTerminal_CF = { new ushort[2] { 0x12, 0x7A } };
        private static ushort[] stateExpectedIDs_D0 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_D0 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_D0 = { "[cf_rule_simple<grammar_text_terminal> → NAME -> • rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_D0 = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_D0 = { new ushort[2] { 0x7d, 0xFC }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_D0 = { new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x4a, 0x5D } };
        private static ushort[] stateExpectedIDs_D1 = { 0x4C };
        private static string[] stateExpectedNames_D1 = { "_T[->]" };
        private static string[] stateItems_D1 = { "[cf_rule_template<grammar_text_terminal> → NAME rule_template_params • -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_D1 = { new ushort[2] { 0x4c, 0xFD } };
        private static ushort[][] stateShiftsOnVariable_D1 = {  };
        private static ushort[][] stateReducsOnTerminal_D1 = {  };
        private static ushort[] stateExpectedIDs_D2 = { 0x41 };
        private static string[] stateExpectedNames_D2 = { "_T[;]" };
        private static string[] stateItems_D2 = { "[cf_rule_simple<grammar_bin_terminal> → NAME -> rule_definition<grammar_bin_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_D2 = { new ushort[2] { 0x41, 0xFE } };
        private static ushort[][] stateShiftsOnVariable_D2 = {  };
        private static ushort[][] stateReducsOnTerminal_D2 = {  };
        private static ushort[] stateExpectedIDs_D3 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_D3 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_D3 = { "[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> • rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_D3 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_D3 = { new ushort[2] { 0x8f, 0xFF }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_D3 = { new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x4a, 0x80 } };
        private static ushort[] stateExpectedIDs_D4 = { 0x48, 0x4E };
        private static string[] stateExpectedNames_D4 = { "_T[,]", "_T[>]" };
        private static string[] stateItems_D4 = { "[rule_template_params → < NAME • _m101 >]", "[_m101 → • , NAME _m101]", "[_m101 → •]" };
        private static ushort[][] stateShiftsOnTerminal_D4 = { new ushort[2] { 0x48, 0x101 } };
        private static ushort[][] stateShiftsOnVariable_D4 = { new ushort[2] { 0x77, 0x100 } };
        private static ushort[][] stateReducsOnTerminal_D4 = { new ushort[2] { 0x4e, 0x52 } };
        private static ushort[] stateExpectedIDs_D5 = { 0x4C, 0x4D };
        private static string[] stateExpectedNames_D5 = { "_T[->]", "_T[<]" };
        private static string[] stateItems_D5 = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> • -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> • rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params → • < NAME _m101 >]" };
        private static ushort[][] stateShiftsOnTerminal_D5 = { new ushort[2] { 0x4c, 0x102 }, new ushort[2] { 0x4d, 0x90 } };
        private static ushort[][] stateShiftsOnVariable_D5 = { new ushort[2] { 0x6b, 0x103 } };
        private static ushort[][] stateReducsOnTerminal_D5 = {  };
        private static ushort[] stateExpectedIDs_D6 = { 0xA, 0x4C, 0x4D };
        private static string[] stateExpectedNames_D6 = { "NAME", "_T[->]", "_T[<]" };
        private static string[] stateItems_D6 = { "[cs_rule_context<grammar_text_terminal> → [ rule_definition<grammar_text_terminal> ] •]" };
        private static ushort[][] stateShiftsOnTerminal_D6 = {  };
        private static ushort[][] stateShiftsOnVariable_D6 = {  };
        private static ushort[][] stateReducsOnTerminal_D6 = { new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x4c, 0xA3 }, new ushort[2] { 0x4d, 0xA3 } };
        private static ushort[] stateExpectedIDs_D7 = { 0x41, 0x44, 0xDB };
        private static string[] stateExpectedNames_D7 = { "_T[;]", "_T[)]", "_T[]]" };
        private static string[] stateItems_D7 = { "[rule_definition<grammar_text_terminal> → rule_def_choice<grammar_text_terminal> _m147 •]" };
        private static ushort[][] stateShiftsOnTerminal_D7 = {  };
        private static ushort[][] stateShiftsOnVariable_D7 = {  };
        private static ushort[][] stateReducsOnTerminal_D7 = { new ushort[2] { 0x41, 0x5B }, new ushort[2] { 0x44, 0x5B }, new ushort[2] { 0xdb, 0x5B } };
        private static ushort[] stateExpectedIDs_D8 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_D8 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_D8 = { "[_m147 → | • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_D8 = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_D8 = { new ushort[2] { 0x7e, 0x104 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_D8 = { new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x44, 0x5D }, new ushort[2] { 0x4a, 0x5D }, new ushort[2] { 0xdb, 0x5D } };
        private static ushort[] stateExpectedIDs_D9 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_D9 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_D9 = { "[rule_def_restrict<grammar_text_terminal> → rule_def_fragment<grammar_text_terminal> _m145 •]" };
        private static ushort[][] stateShiftsOnTerminal_D9 = {  };
        private static ushort[][] stateShiftsOnVariable_D9 = {  };
        private static ushort[][] stateReducsOnTerminal_D9 = { new ushort[2] { 0x41, 0x5E }, new ushort[2] { 0x44, 0x5E }, new ushort[2] { 0x4a, 0x5E }, new ushort[2] { 0xdb, 0x5E } };
        private static ushort[] stateExpectedIDs_DA = { 0xA, 0x11, 0x2E, 0x30, 0x43 };
        private static string[] stateExpectedNames_DA = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[(]" };
        private static string[] stateItems_DA = { "[_m145 → - • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_DA = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_DA = { new ushort[2] { 0x80, 0x105 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_DA = {  };
        private static ushort[] stateExpectedIDs_DB = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_DB = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_DB = { "[rule_def_fragment<grammar_text_terminal> → rule_def_repetition<grammar_text_terminal> _m143 •]" };
        private static ushort[][] stateShiftsOnTerminal_DB = {  };
        private static ushort[][] stateShiftsOnVariable_DB = {  };
        private static ushort[][] stateReducsOnTerminal_DB = { new ushort[2] { 0x41, 0x5F }, new ushort[2] { 0x44, 0x5F }, new ushort[2] { 0x49, 0x5F }, new ushort[2] { 0x4a, 0x5F }, new ushort[2] { 0xdb, 0x5F } };
        private static ushort[] stateExpectedIDs_DC = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_DC = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_DC = { "[_m143 → rule_def_repetition<grammar_text_terminal> • _m143]", "[_m143 → • rule_def_repetition<grammar_text_terminal> _m143]", "[_m143 → •]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_DC = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_DC = { new ushort[2] { 0x88, 0x106 }, new ushort[2] { 0x81, 0xDC }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_DC = { new ushort[2] { 0x41, 0x73 }, new ushort[2] { 0x44, 0x73 }, new ushort[2] { 0x49, 0x73 }, new ushort[2] { 0x4a, 0x73 }, new ushort[2] { 0xdb, 0x73 } };
        private static ushort[] stateExpectedIDs_DD = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_DD = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_DD = { "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> * •]" };
        private static ushort[][] stateShiftsOnTerminal_DD = {  };
        private static ushort[][] stateShiftsOnVariable_DD = {  };
        private static ushort[][] stateReducsOnTerminal_DD = { new ushort[2] { 0xa, 0x60 }, new ushort[2] { 0x11, 0x60 }, new ushort[2] { 0x2e, 0x60 }, new ushort[2] { 0x30, 0x60 }, new ushort[2] { 0x41, 0x60 }, new ushort[2] { 0x43, 0x60 }, new ushort[2] { 0x44, 0x60 }, new ushort[2] { 0x49, 0x60 }, new ushort[2] { 0x4a, 0x60 }, new ushort[2] { 0xdb, 0x60 } };
        private static ushort[] stateExpectedIDs_DE = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_DE = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_DE = { "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> + •]" };
        private static ushort[][] stateShiftsOnTerminal_DE = {  };
        private static ushort[][] stateShiftsOnVariable_DE = {  };
        private static ushort[][] stateReducsOnTerminal_DE = { new ushort[2] { 0xa, 0x61 }, new ushort[2] { 0x11, 0x61 }, new ushort[2] { 0x2e, 0x61 }, new ushort[2] { 0x30, 0x61 }, new ushort[2] { 0x41, 0x61 }, new ushort[2] { 0x43, 0x61 }, new ushort[2] { 0x44, 0x61 }, new ushort[2] { 0x49, 0x61 }, new ushort[2] { 0x4a, 0x61 }, new ushort[2] { 0xdb, 0x61 } };
        private static ushort[] stateExpectedIDs_DF = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_DF = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_DF = { "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> ? •]" };
        private static ushort[][] stateShiftsOnTerminal_DF = {  };
        private static ushort[][] stateShiftsOnVariable_DF = {  };
        private static ushort[][] stateReducsOnTerminal_DF = { new ushort[2] { 0xa, 0x62 }, new ushort[2] { 0x11, 0x62 }, new ushort[2] { 0x2e, 0x62 }, new ushort[2] { 0x30, 0x62 }, new ushort[2] { 0x41, 0x62 }, new ushort[2] { 0x43, 0x62 }, new ushort[2] { 0x44, 0x62 }, new ushort[2] { 0x49, 0x62 }, new ushort[2] { 0x4a, 0x62 }, new ushort[2] { 0xdb, 0x62 } };
        private static ushort[] stateExpectedIDs_E0 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_E0 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_E0 = { "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> ^ •]" };
        private static ushort[][] stateShiftsOnTerminal_E0 = {  };
        private static ushort[][] stateShiftsOnVariable_E0 = {  };
        private static ushort[][] stateReducsOnTerminal_E0 = { new ushort[2] { 0xa, 0x64 }, new ushort[2] { 0x11, 0x64 }, new ushort[2] { 0x2e, 0x64 }, new ushort[2] { 0x30, 0x64 }, new ushort[2] { 0x41, 0x64 }, new ushort[2] { 0x43, 0x64 }, new ushort[2] { 0x44, 0x64 }, new ushort[2] { 0x45, 0x64 }, new ushort[2] { 0x46, 0x64 }, new ushort[2] { 0x47, 0x64 }, new ushort[2] { 0x49, 0x64 }, new ushort[2] { 0x4a, 0x64 }, new ushort[2] { 0xdb, 0x64 } };
        private static ushort[] stateExpectedIDs_E1 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_E1 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_E1 = { "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> ! •]" };
        private static ushort[][] stateShiftsOnTerminal_E1 = {  };
        private static ushort[][] stateShiftsOnVariable_E1 = {  };
        private static ushort[][] stateReducsOnTerminal_E1 = { new ushort[2] { 0xa, 0x65 }, new ushort[2] { 0x11, 0x65 }, new ushort[2] { 0x2e, 0x65 }, new ushort[2] { 0x30, 0x65 }, new ushort[2] { 0x41, 0x65 }, new ushort[2] { 0x43, 0x65 }, new ushort[2] { 0x44, 0x65 }, new ushort[2] { 0x45, 0x65 }, new ushort[2] { 0x46, 0x65 }, new ushort[2] { 0x47, 0x65 }, new ushort[2] { 0x49, 0x65 }, new ushort[2] { 0x4a, 0x65 }, new ushort[2] { 0xdb, 0x65 } };
        private static ushort[] stateExpectedIDs_E2 = { 0x44 };
        private static string[] stateExpectedNames_E2 = { "_T[)]" };
        private static string[] stateItems_E2 = { "[rule_def_element<grammar_text_terminal> → ( rule_definition<grammar_text_terminal> • )]" };
        private static ushort[][] stateShiftsOnTerminal_E2 = { new ushort[2] { 0x44, 0x107 } };
        private static ushort[][] stateShiftsOnVariable_E2 = {  };
        private static ushort[][] stateReducsOnTerminal_E2 = {  };
        private static ushort[] stateExpectedIDs_E3 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_E3 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_E3 = { "[rule_sym_ref_template<grammar_text_terminal> → NAME rule_sym_ref_params<grammar_text_terminal> •]" };
        private static ushort[][] stateShiftsOnTerminal_E3 = {  };
        private static ushort[][] stateShiftsOnVariable_E3 = {  };
        private static ushort[][] stateReducsOnTerminal_E3 = { new ushort[2] { 0xa, 0x6E }, new ushort[2] { 0x11, 0x6E }, new ushort[2] { 0x2e, 0x6E }, new ushort[2] { 0x30, 0x6E }, new ushort[2] { 0x41, 0x6E }, new ushort[2] { 0x43, 0x6E }, new ushort[2] { 0x44, 0x6E }, new ushort[2] { 0x45, 0x6E }, new ushort[2] { 0x46, 0x6E }, new ushort[2] { 0x47, 0x6E }, new ushort[2] { 0x48, 0x6E }, new ushort[2] { 0x49, 0x6E }, new ushort[2] { 0x4a, 0x6E }, new ushort[2] { 0x4e, 0x6E }, new ushort[2] { 0x55, 0x6E }, new ushort[2] { 0x56, 0x6E }, new ushort[2] { 0xdb, 0x6E } };
        private static ushort[] stateExpectedIDs_E4 = { 0xA, 0x11, 0x2E, 0x30 };
        private static string[] stateExpectedNames_E4 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT" };
        private static string[] stateItems_E4 = { "[rule_sym_ref_params<grammar_text_terminal> → < • rule_def_atom<grammar_text_terminal> _m134 >]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_E4 = { new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_E4 = { new ushort[2] { 0x84, 0x108 }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_E4 = {  };
        private static ushort[] stateExpectedIDs_E5 = { 0x41 };
        private static string[] stateExpectedNames_E5 = { "_T[;]" };
        private static string[] stateItems_E5 = { "[terminal → NAME -> terminal_definition terminal_subgrammar • ;]" };
        private static ushort[][] stateShiftsOnTerminal_E5 = { new ushort[2] { 0x41, 0x109 } };
        private static ushort[][] stateShiftsOnVariable_E5 = {  };
        private static ushort[][] stateReducsOnTerminal_E5 = {  };
        private static ushort[] stateExpectedIDs_E6 = { 0xA };
        private static string[] stateExpectedNames_E6 = { "NAME" };
        private static string[] stateItems_E6 = { "[terminal_subgrammar → => • qualified_name]", "[qualified_name → • NAME _m20]" };
        private static ushort[][] stateShiftsOnTerminal_E6 = { new ushort[2] { 0xa, 0x18 } };
        private static ushort[][] stateShiftsOnVariable_E6 = { new ushort[2] { 0x13, 0x10A } };
        private static ushort[][] stateReducsOnTerminal_E6 = {  };
        private static ushort[] stateExpectedIDs_E7 = { 0x41, 0x44, 0x4B };
        private static string[] stateExpectedNames_E7 = { "_T[;]", "_T[)]", "_T[=>]" };
        private static string[] stateItems_E7 = { "[terminal_definition → terminal_def_restrict _m93 •]" };
        private static ushort[][] stateShiftsOnTerminal_E7 = {  };
        private static ushort[][] stateShiftsOnVariable_E7 = {  };
        private static ushort[][] stateReducsOnTerminal_E7 = { new ushort[2] { 0x41, 0x2C }, new ushort[2] { 0x44, 0x2C }, new ushort[2] { 0x4b, 0x2C } };
        private static ushort[] stateExpectedIDs_E8 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x43 };
        private static string[] stateExpectedNames_E8 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[(]" };
        private static string[] stateItems_E8 = { "[_m93 → | • terminal_def_restrict _m93]", "[terminal_def_restrict → • terminal_def_fragment _m91]", "[terminal_def_fragment → • terminal_def_repetition _m89]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_E8 = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_E8 = { new ushort[2] { 0x64, 0x10B }, new ushort[2] { 0x63, 0xA8 }, new ushort[2] { 0x62, 0xA9 }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_E8 = {  };
        private static ushort[] stateExpectedIDs_E9 = { 0x41, 0x44, 0x4A, 0x4B };
        private static string[] stateExpectedNames_E9 = { "_T[;]", "_T[)]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_E9 = { "[terminal_def_restrict → terminal_def_fragment _m91 •]" };
        private static ushort[][] stateShiftsOnTerminal_E9 = {  };
        private static ushort[][] stateShiftsOnVariable_E9 = {  };
        private static ushort[][] stateReducsOnTerminal_E9 = { new ushort[2] { 0x41, 0x2B }, new ushort[2] { 0x44, 0x2B }, new ushort[2] { 0x4a, 0x2B }, new ushort[2] { 0x4b, 0x2B } };
        private static ushort[] stateExpectedIDs_EA = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x43 };
        private static string[] stateExpectedNames_EA = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[(]" };
        private static string[] stateItems_EA = { "[_m91 → - • terminal_def_fragment _m91]", "[terminal_def_fragment → • terminal_def_repetition _m89]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_EA = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_EA = { new ushort[2] { 0x63, 0x10C }, new ushort[2] { 0x62, 0xA9 }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_EA = {  };
        private static ushort[] stateExpectedIDs_EB = { 0x41, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_EB = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_EB = { "[terminal_def_fragment → terminal_def_repetition _m89 •]" };
        private static ushort[][] stateShiftsOnTerminal_EB = {  };
        private static ushort[][] stateShiftsOnVariable_EB = {  };
        private static ushort[][] stateReducsOnTerminal_EB = { new ushort[2] { 0x41, 0x2A }, new ushort[2] { 0x44, 0x2A }, new ushort[2] { 0x49, 0x2A }, new ushort[2] { 0x4a, 0x2A }, new ushort[2] { 0x4b, 0x2A } };
        private static ushort[] stateExpectedIDs_EC = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_EC = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_EC = { "[_m89 → terminal_def_repetition • _m89]", "[_m89 → • terminal_def_repetition _m89]", "[_m89 → •]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty]", "[terminal_def_repetition → • terminal_def_element]", "[terminal_def_element → • terminal_def_atom]", "[terminal_def_element → • ( terminal_definition )]", "[terminal_def_atom → • terminal_def_atom_any]", "[terminal_def_atom → • terminal_def_atom_unicode]", "[terminal_def_atom → • terminal_def_atom_text]", "[terminal_def_atom → • terminal_def_atom_set]", "[terminal_def_atom → • terminal_def_atom_span]", "[terminal_def_atom → • terminal_def_atom_ucat]", "[terminal_def_atom → • terminal_def_atom_ublock]", "[terminal_def_atom → • NAME]", "[terminal_def_atom_any → • .]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] stateShiftsOnTerminal_EC = { new ushort[2] { 0x43, 0xAC }, new ushort[2] { 0xa, 0xB4 }, new ushort[2] { 0xb, 0xB5 }, new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 }, new ushort[2] { 0x30, 0xA5 }, new ushort[2] { 0x31, 0xB8 }, new ushort[2] { 0x33, 0xB9 }, new ushort[2] { 0x32, 0xBA } };
        private static ushort[][] stateShiftsOnVariable_EC = { new ushort[2] { 0x74, 0x10D }, new ushort[2] { 0x62, 0xEC }, new ushort[2] { 0x60, 0xAA }, new ushort[2] { 0x5f, 0xAB }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x59, 0xAE }, new ushort[2] { 0x5a, 0xAF }, new ushort[2] { 0x5b, 0xB0 }, new ushort[2] { 0x5e, 0xB1 }, new ushort[2] { 0x5d, 0xB2 }, new ushort[2] { 0x5c, 0xB3 } };
        private static ushort[][] stateReducsOnTerminal_EC = { new ushort[2] { 0x41, 0x4C }, new ushort[2] { 0x44, 0x4C }, new ushort[2] { 0x49, 0x4C }, new ushort[2] { 0x4a, 0x4C }, new ushort[2] { 0x4b, 0x4C } };
        private static ushort[] stateExpectedIDs_ED = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_ED = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_ED = { "[terminal_def_repetition → terminal_def_element terminal_def_cardinalilty •]" };
        private static ushort[][] stateShiftsOnTerminal_ED = {  };
        private static ushort[][] stateShiftsOnVariable_ED = {  };
        private static ushort[][] stateReducsOnTerminal_ED = { new ushort[2] { 0xa, 0x28 }, new ushort[2] { 0xb, 0x28 }, new ushort[2] { 0x30, 0x28 }, new ushort[2] { 0x31, 0x28 }, new ushort[2] { 0x32, 0x28 }, new ushort[2] { 0x33, 0x28 }, new ushort[2] { 0x34, 0x28 }, new ushort[2] { 0x35, 0x28 }, new ushort[2] { 0x41, 0x28 }, new ushort[2] { 0x43, 0x28 }, new ushort[2] { 0x44, 0x28 }, new ushort[2] { 0x49, 0x28 }, new ushort[2] { 0x4a, 0x28 }, new ushort[2] { 0x4b, 0x28 } };
        private static ushort[] stateExpectedIDs_EE = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_EE = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_EE = { "[terminal_def_cardinalilty → * •]" };
        private static ushort[][] stateShiftsOnTerminal_EE = {  };
        private static ushort[][] stateShiftsOnVariable_EE = {  };
        private static ushort[][] stateReducsOnTerminal_EE = { new ushort[2] { 0xa, 0x23 }, new ushort[2] { 0xb, 0x23 }, new ushort[2] { 0x30, 0x23 }, new ushort[2] { 0x31, 0x23 }, new ushort[2] { 0x32, 0x23 }, new ushort[2] { 0x33, 0x23 }, new ushort[2] { 0x34, 0x23 }, new ushort[2] { 0x35, 0x23 }, new ushort[2] { 0x41, 0x23 }, new ushort[2] { 0x43, 0x23 }, new ushort[2] { 0x44, 0x23 }, new ushort[2] { 0x49, 0x23 }, new ushort[2] { 0x4a, 0x23 }, new ushort[2] { 0x4b, 0x23 } };
        private static ushort[] stateExpectedIDs_EF = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_EF = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_EF = { "[terminal_def_cardinalilty → + •]" };
        private static ushort[][] stateShiftsOnTerminal_EF = {  };
        private static ushort[][] stateShiftsOnVariable_EF = {  };
        private static ushort[][] stateReducsOnTerminal_EF = { new ushort[2] { 0xa, 0x24 }, new ushort[2] { 0xb, 0x24 }, new ushort[2] { 0x30, 0x24 }, new ushort[2] { 0x31, 0x24 }, new ushort[2] { 0x32, 0x24 }, new ushort[2] { 0x33, 0x24 }, new ushort[2] { 0x34, 0x24 }, new ushort[2] { 0x35, 0x24 }, new ushort[2] { 0x41, 0x24 }, new ushort[2] { 0x43, 0x24 }, new ushort[2] { 0x44, 0x24 }, new ushort[2] { 0x49, 0x24 }, new ushort[2] { 0x4a, 0x24 }, new ushort[2] { 0x4b, 0x24 } };
        private static ushort[] stateExpectedIDs_F0 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_F0 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_F0 = { "[terminal_def_cardinalilty → ? •]" };
        private static ushort[][] stateShiftsOnTerminal_F0 = {  };
        private static ushort[][] stateShiftsOnVariable_F0 = {  };
        private static ushort[][] stateReducsOnTerminal_F0 = { new ushort[2] { 0xa, 0x25 }, new ushort[2] { 0xb, 0x25 }, new ushort[2] { 0x30, 0x25 }, new ushort[2] { 0x31, 0x25 }, new ushort[2] { 0x32, 0x25 }, new ushort[2] { 0x33, 0x25 }, new ushort[2] { 0x34, 0x25 }, new ushort[2] { 0x35, 0x25 }, new ushort[2] { 0x41, 0x25 }, new ushort[2] { 0x43, 0x25 }, new ushort[2] { 0x44, 0x25 }, new ushort[2] { 0x49, 0x25 }, new ushort[2] { 0x4a, 0x25 }, new ushort[2] { 0x4b, 0x25 } };
        private static ushort[] stateExpectedIDs_F1 = { 0x2D };
        private static string[] stateExpectedNames_F1 = { "INTEGER" };
        private static string[] stateItems_F1 = { "[terminal_def_cardinalilty → { • INTEGER , INTEGER }]", "[terminal_def_cardinalilty → { • INTEGER }]" };
        private static ushort[][] stateShiftsOnTerminal_F1 = { new ushort[2] { 0x2d, 0x10E } };
        private static ushort[][] stateShiftsOnVariable_F1 = {  };
        private static ushort[][] stateReducsOnTerminal_F1 = {  };
        private static ushort[] stateExpectedIDs_F2 = { 0x44 };
        private static string[] stateExpectedNames_F2 = { "_T[)]" };
        private static string[] stateItems_F2 = { "[terminal_def_element → ( terminal_definition • )]" };
        private static ushort[][] stateShiftsOnTerminal_F2 = { new ushort[2] { 0x44, 0x10F } };
        private static ushort[][] stateShiftsOnVariable_F2 = {  };
        private static ushort[][] stateReducsOnTerminal_F2 = {  };
        private static ushort[] stateExpectedIDs_F3 = { 0x34, 0x35 };
        private static string[] stateExpectedNames_F3 = { "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16" };
        private static string[] stateItems_F3 = { "[terminal_def_atom_span → terminal_def_atom_unicode .. • terminal_def_atom_unicode]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16]" };
        private static ushort[][] stateShiftsOnTerminal_F3 = { new ushort[2] { 0x34, 0xB6 }, new ushort[2] { 0x35, 0xB7 } };
        private static ushort[][] stateShiftsOnVariable_F3 = { new ushort[2] { 0x59, 0x110 } };
        private static ushort[][] stateReducsOnTerminal_F3 = {  };
        private static ushort[] stateExpectedIDs_F4 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_F4 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_F4 = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> • rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_F4 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_F4 = { new ushort[2] { 0x8f, 0x111 }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_F4 = { new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x4a, 0x80 } };
        private static ushort[] stateExpectedIDs_F5 = { 0x4C };
        private static string[] stateExpectedNames_F5 = { "_T[->]" };
        private static string[] stateItems_F5 = { "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params • -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_F5 = { new ushort[2] { 0x4c, 0x112 } };
        private static ushort[][] stateShiftsOnVariable_F5 = {  };
        private static ushort[][] stateReducsOnTerminal_F5 = {  };
        private static ushort[] stateExpectedIDs_F6 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_F6 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_F6 = { "[_m188 → | rule_def_choice<grammar_bin_terminal> • _m188]", "[_m188 → • | rule_def_choice<grammar_bin_terminal> _m188]", "[_m188 → •]" };
        private static ushort[][] stateShiftsOnTerminal_F6 = { new ushort[2] { 0x4a, 0xBE } };
        private static ushort[][] stateShiftsOnVariable_F6 = { new ushort[2] { 0x9c, 0x113 } };
        private static ushort[][] stateReducsOnTerminal_F6 = { new ushort[2] { 0x41, 0x9A }, new ushort[2] { 0x44, 0x9A }, new ushort[2] { 0xdb, 0x9A } };
        private static ushort[] stateExpectedIDs_F7 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_F7 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_F7 = { "[_m186 → - rule_def_fragment<grammar_bin_terminal> • _m186]", "[_m186 → • - rule_def_fragment<grammar_bin_terminal> _m186]", "[_m186 → •]" };
        private static ushort[][] stateShiftsOnTerminal_F7 = { new ushort[2] { 0x49, 0xC0 } };
        private static ushort[][] stateShiftsOnVariable_F7 = { new ushort[2] { 0x9b, 0x114 } };
        private static ushort[][] stateReducsOnTerminal_F7 = { new ushort[2] { 0x41, 0x98 }, new ushort[2] { 0x44, 0x98 }, new ushort[2] { 0x4a, 0x98 }, new ushort[2] { 0xdb, 0x98 } };
        private static ushort[] stateExpectedIDs_F8 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_F8 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_F8 = { "[_m184 → rule_def_repetition<grammar_bin_terminal> _m184 •]" };
        private static ushort[][] stateShiftsOnTerminal_F8 = {  };
        private static ushort[][] stateShiftsOnVariable_F8 = {  };
        private static ushort[][] stateReducsOnTerminal_F8 = { new ushort[2] { 0x41, 0x95 }, new ushort[2] { 0x44, 0x95 }, new ushort[2] { 0x49, 0x95 }, new ushort[2] { 0x4a, 0x95 }, new ushort[2] { 0xdb, 0x95 } };
        private static ushort[] stateExpectedIDs_F9 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_F9 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_F9 = { "[rule_def_element<grammar_bin_terminal> → ( rule_definition<grammar_bin_terminal> ) •]" };
        private static ushort[][] stateShiftsOnTerminal_F9 = {  };
        private static ushort[][] stateShiftsOnVariable_F9 = {  };
        private static ushort[][] stateReducsOnTerminal_F9 = { new ushort[2] { 0xa, 0x8B }, new ushort[2] { 0x11, 0x8B }, new ushort[2] { 0x2e, 0x8B }, new ushort[2] { 0x34, 0x8B }, new ushort[2] { 0x35, 0x8B }, new ushort[2] { 0x36, 0x8B }, new ushort[2] { 0x37, 0x8B }, new ushort[2] { 0x38, 0x8B }, new ushort[2] { 0x39, 0x8B }, new ushort[2] { 0x3a, 0x8B }, new ushort[2] { 0x3b, 0x8B }, new ushort[2] { 0x3c, 0x8B }, new ushort[2] { 0x3d, 0x8B }, new ushort[2] { 0x3e, 0x8B }, new ushort[2] { 0x3f, 0x8B }, new ushort[2] { 0x41, 0x8B }, new ushort[2] { 0x43, 0x8B }, new ushort[2] { 0x44, 0x8B }, new ushort[2] { 0x45, 0x8B }, new ushort[2] { 0x46, 0x8B }, new ushort[2] { 0x47, 0x8B }, new ushort[2] { 0x49, 0x8B }, new ushort[2] { 0x4a, 0x8B }, new ushort[2] { 0x55, 0x8B }, new ushort[2] { 0x56, 0x8B }, new ushort[2] { 0xdb, 0x8B } };
        private static ushort[] stateExpectedIDs_FA = { 0xA, 0x11, 0x2E, 0x30, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_FA = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_FA = { "[rule_sym_action → { NAME } •]" };
        private static ushort[][] stateShiftsOnTerminal_FA = {  };
        private static ushort[][] stateShiftsOnVariable_FA = {  };
        private static ushort[][] stateReducsOnTerminal_FA = { new ushort[2] { 0xa, 0x30 }, new ushort[2] { 0x11, 0x30 }, new ushort[2] { 0x2e, 0x30 }, new ushort[2] { 0x30, 0x30 }, new ushort[2] { 0x34, 0x30 }, new ushort[2] { 0x35, 0x30 }, new ushort[2] { 0x36, 0x30 }, new ushort[2] { 0x37, 0x30 }, new ushort[2] { 0x38, 0x30 }, new ushort[2] { 0x39, 0x30 }, new ushort[2] { 0x3a, 0x30 }, new ushort[2] { 0x3b, 0x30 }, new ushort[2] { 0x3c, 0x30 }, new ushort[2] { 0x3d, 0x30 }, new ushort[2] { 0x3e, 0x30 }, new ushort[2] { 0x3f, 0x30 }, new ushort[2] { 0x41, 0x30 }, new ushort[2] { 0x43, 0x30 }, new ushort[2] { 0x44, 0x30 }, new ushort[2] { 0x45, 0x30 }, new ushort[2] { 0x46, 0x30 }, new ushort[2] { 0x47, 0x30 }, new ushort[2] { 0x48, 0x30 }, new ushort[2] { 0x49, 0x30 }, new ushort[2] { 0x4a, 0x30 }, new ushort[2] { 0x4e, 0x30 }, new ushort[2] { 0x55, 0x30 }, new ushort[2] { 0x56, 0x30 }, new ushort[2] { 0xdb, 0x30 } };
        private static ushort[] stateExpectedIDs_FB = { 0x48, 0x4E };
        private static string[] stateExpectedNames_FB = { "_T[,]", "_T[>]" };
        private static string[] stateItems_FB = { "[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> • _m175 >]", "[_m175 → • , rule_def_atom<grammar_bin_terminal> _m175]", "[_m175 → •]" };
        private static ushort[][] stateShiftsOnTerminal_FB = { new ushort[2] { 0x48, 0x116 } };
        private static ushort[][] stateShiftsOnVariable_FB = { new ushort[2] { 0x99, 0x115 } };
        private static ushort[][] stateReducsOnTerminal_FB = { new ushort[2] { 0x4e, 0x94 } };
        private static ushort[] stateExpectedIDs_FC = { 0x41 };
        private static string[] stateExpectedNames_FC = { "_T[;]" };
        private static string[] stateItems_FC = { "[cf_rule_simple<grammar_text_terminal> → NAME -> rule_definition<grammar_text_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_FC = { new ushort[2] { 0x41, 0x117 } };
        private static ushort[][] stateShiftsOnVariable_FC = {  };
        private static ushort[][] stateReducsOnTerminal_FC = {  };
        private static ushort[] stateExpectedIDs_FD = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_FD = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_FD = { "[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> • rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_FD = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_FD = { new ushort[2] { 0x7d, 0x118 }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_FD = { new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x4a, 0x5D } };
        private static ushort[] stateExpectedIDs_FE = { 0xA, 0x12 };
        private static string[] stateExpectedNames_FE = { "NAME", "_T[}]" };
        private static string[] stateItems_FE = { "[cf_rule_simple<grammar_bin_terminal> → NAME -> rule_definition<grammar_bin_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_FE = {  };
        private static ushort[][] stateShiftsOnVariable_FE = {  };
        private static ushort[][] stateReducsOnTerminal_FE = { new ushort[2] { 0xa, 0x7D }, new ushort[2] { 0x12, 0x7D } };
        private static ushort[] stateExpectedIDs_FF = { 0x41 };
        private static string[] stateExpectedNames_FF = { "_T[;]" };
        private static string[] stateItems_FF = { "[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> rule_definition<grammar_bin_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_FF = { new ushort[2] { 0x41, 0x119 } };
        private static ushort[][] stateShiftsOnVariable_FF = {  };
        private static ushort[][] stateReducsOnTerminal_FF = {  };
        private static ushort[] stateExpectedIDs_100 = { 0x4E };
        private static string[] stateExpectedNames_100 = { "_T[>]" };
        private static string[] stateItems_100 = { "[rule_template_params → < NAME _m101 • >]" };
        private static ushort[][] stateShiftsOnTerminal_100 = { new ushort[2] { 0x4e, 0x11A } };
        private static ushort[][] stateShiftsOnVariable_100 = {  };
        private static ushort[][] stateReducsOnTerminal_100 = {  };
        private static ushort[] stateExpectedIDs_101 = { 0xA };
        private static string[] stateExpectedNames_101 = { "NAME" };
        private static string[] stateItems_101 = { "[_m101 → , • NAME _m101]" };
        private static ushort[][] stateShiftsOnTerminal_101 = { new ushort[2] { 0xa, 0x11B } };
        private static ushort[][] stateShiftsOnVariable_101 = {  };
        private static ushort[][] stateReducsOnTerminal_101 = {  };
        private static ushort[] stateExpectedIDs_102 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_102 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_102 = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> • rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_102 = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_102 = { new ushort[2] { 0x7d, 0x11C }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_102 = { new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x4a, 0x5D } };
        private static ushort[] stateExpectedIDs_103 = { 0x4C };
        private static string[] stateExpectedNames_103 = { "_T[->]" };
        private static string[] stateItems_103 = { "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params • -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] stateShiftsOnTerminal_103 = { new ushort[2] { 0x4c, 0x11D } };
        private static ushort[][] stateShiftsOnVariable_103 = {  };
        private static ushort[][] stateReducsOnTerminal_103 = {  };
        private static ushort[] stateExpectedIDs_104 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_104 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_104 = { "[_m147 → | rule_def_choice<grammar_text_terminal> • _m147]", "[_m147 → • | rule_def_choice<grammar_text_terminal> _m147]", "[_m147 → •]" };
        private static ushort[][] stateShiftsOnTerminal_104 = { new ushort[2] { 0x4a, 0xD8 } };
        private static ushort[][] stateShiftsOnVariable_104 = { new ushort[2] { 0x8a, 0x11E } };
        private static ushort[][] stateReducsOnTerminal_104 = { new ushort[2] { 0x41, 0x77 }, new ushort[2] { 0x44, 0x77 }, new ushort[2] { 0xdb, 0x77 } };
        private static ushort[] stateExpectedIDs_105 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_105 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_105 = { "[_m145 → - rule_def_fragment<grammar_text_terminal> • _m145]", "[_m145 → • - rule_def_fragment<grammar_text_terminal> _m145]", "[_m145 → •]" };
        private static ushort[][] stateShiftsOnTerminal_105 = { new ushort[2] { 0x49, 0xDA } };
        private static ushort[][] stateShiftsOnVariable_105 = { new ushort[2] { 0x89, 0x11F } };
        private static ushort[][] stateReducsOnTerminal_105 = { new ushort[2] { 0x41, 0x75 }, new ushort[2] { 0x44, 0x75 }, new ushort[2] { 0x4a, 0x75 }, new ushort[2] { 0xdb, 0x75 } };
        private static ushort[] stateExpectedIDs_106 = { 0x41, 0x44, 0x49, 0x4A, 0xDB };
        private static string[] stateExpectedNames_106 = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[]]" };
        private static string[] stateItems_106 = { "[_m143 → rule_def_repetition<grammar_text_terminal> _m143 •]" };
        private static ushort[][] stateShiftsOnTerminal_106 = {  };
        private static ushort[][] stateShiftsOnVariable_106 = {  };
        private static ushort[][] stateReducsOnTerminal_106 = { new ushort[2] { 0x41, 0x72 }, new ushort[2] { 0x44, 0x72 }, new ushort[2] { 0x49, 0x72 }, new ushort[2] { 0x4a, 0x72 }, new ushort[2] { 0xdb, 0x72 } };
        private static ushort[] stateExpectedIDs_107 = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_107 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_107 = { "[rule_def_element<grammar_text_terminal> → ( rule_definition<grammar_text_terminal> ) •]" };
        private static ushort[][] stateShiftsOnTerminal_107 = {  };
        private static ushort[][] stateShiftsOnVariable_107 = {  };
        private static ushort[][] stateReducsOnTerminal_107 = { new ushort[2] { 0xa, 0x68 }, new ushort[2] { 0x11, 0x68 }, new ushort[2] { 0x2e, 0x68 }, new ushort[2] { 0x30, 0x68 }, new ushort[2] { 0x41, 0x68 }, new ushort[2] { 0x43, 0x68 }, new ushort[2] { 0x44, 0x68 }, new ushort[2] { 0x45, 0x68 }, new ushort[2] { 0x46, 0x68 }, new ushort[2] { 0x47, 0x68 }, new ushort[2] { 0x49, 0x68 }, new ushort[2] { 0x4a, 0x68 }, new ushort[2] { 0x55, 0x68 }, new ushort[2] { 0x56, 0x68 }, new ushort[2] { 0xdb, 0x68 } };
        private static ushort[] stateExpectedIDs_108 = { 0x48, 0x4E };
        private static string[] stateExpectedNames_108 = { "_T[,]", "_T[>]" };
        private static string[] stateItems_108 = { "[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> • _m134 >]", "[_m134 → • , rule_def_atom<grammar_text_terminal> _m134]", "[_m134 → •]" };
        private static ushort[][] stateShiftsOnTerminal_108 = { new ushort[2] { 0x48, 0x121 } };
        private static ushort[][] stateShiftsOnVariable_108 = { new ushort[2] { 0x87, 0x120 } };
        private static ushort[][] stateReducsOnTerminal_108 = { new ushort[2] { 0x4e, 0x71 } };
        private static ushort[] stateExpectedIDs_109 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_109 = { "NAME", "_T[}]" };
        private static string[] stateItems_109 = { "[terminal → NAME -> terminal_definition terminal_subgrammar ; •]" };
        private static ushort[][] stateShiftsOnTerminal_109 = {  };
        private static ushort[][] stateShiftsOnVariable_109 = {  };
        private static ushort[][] stateReducsOnTerminal_109 = { new ushort[2] { 0xa, 0x2F }, new ushort[2] { 0x12, 0x2F } };
        private static ushort[] stateExpectedIDs_10A = { 0x41 };
        private static string[] stateExpectedNames_10A = { "_T[;]" };
        private static string[] stateItems_10A = { "[terminal_subgrammar → => qualified_name •]" };
        private static ushort[][] stateShiftsOnTerminal_10A = {  };
        private static ushort[][] stateShiftsOnVariable_10A = {  };
        private static ushort[][] stateReducsOnTerminal_10A = { new ushort[2] { 0x41, 0x2D } };
        private static ushort[] stateExpectedIDs_10B = { 0x41, 0x44, 0x4A, 0x4B };
        private static string[] stateExpectedNames_10B = { "_T[;]", "_T[)]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_10B = { "[_m93 → | terminal_def_restrict • _m93]", "[_m93 → • | terminal_def_restrict _m93]", "[_m93 → •]" };
        private static ushort[][] stateShiftsOnTerminal_10B = { new ushort[2] { 0x4a, 0xE8 } };
        private static ushort[][] stateShiftsOnVariable_10B = { new ushort[2] { 0x76, 0x122 } };
        private static ushort[][] stateReducsOnTerminal_10B = { new ushort[2] { 0x41, 0x50 }, new ushort[2] { 0x44, 0x50 }, new ushort[2] { 0x4b, 0x50 } };
        private static ushort[] stateExpectedIDs_10C = { 0x41, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_10C = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_10C = { "[_m91 → - terminal_def_fragment • _m91]", "[_m91 → • - terminal_def_fragment _m91]", "[_m91 → •]" };
        private static ushort[][] stateShiftsOnTerminal_10C = { new ushort[2] { 0x49, 0xEA } };
        private static ushort[][] stateShiftsOnVariable_10C = { new ushort[2] { 0x75, 0x123 } };
        private static ushort[][] stateReducsOnTerminal_10C = { new ushort[2] { 0x41, 0x4E }, new ushort[2] { 0x44, 0x4E }, new ushort[2] { 0x4a, 0x4E }, new ushort[2] { 0x4b, 0x4E } };
        private static ushort[] stateExpectedIDs_10D = { 0x41, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_10D = { "_T[;]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_10D = { "[_m89 → terminal_def_repetition _m89 •]" };
        private static ushort[][] stateShiftsOnTerminal_10D = {  };
        private static ushort[][] stateShiftsOnVariable_10D = {  };
        private static ushort[][] stateReducsOnTerminal_10D = { new ushort[2] { 0x41, 0x4B }, new ushort[2] { 0x44, 0x4B }, new ushort[2] { 0x49, 0x4B }, new ushort[2] { 0x4a, 0x4B }, new ushort[2] { 0x4b, 0x4B } };
        private static ushort[] stateExpectedIDs_10E = { 0x12, 0x48 };
        private static string[] stateExpectedNames_10E = { "_T[}]", "_T[,]" };
        private static string[] stateItems_10E = { "[terminal_def_cardinalilty → { INTEGER • , INTEGER }]", "[terminal_def_cardinalilty → { INTEGER • }]" };
        private static ushort[][] stateShiftsOnTerminal_10E = { new ushort[2] { 0x48, 0x124 }, new ushort[2] { 0x12, 0x125 } };
        private static ushort[][] stateShiftsOnVariable_10E = {  };
        private static ushort[][] stateReducsOnTerminal_10E = {  };
        private static ushort[] stateExpectedIDs_10F = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_10F = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_10F = { "[terminal_def_element → ( terminal_definition ) •]" };
        private static ushort[][] stateShiftsOnTerminal_10F = {  };
        private static ushort[][] stateShiftsOnVariable_10F = {  };
        private static ushort[][] stateReducsOnTerminal_10F = { new ushort[2] { 0xa, 0x22 }, new ushort[2] { 0xb, 0x22 }, new ushort[2] { 0x11, 0x22 }, new ushort[2] { 0x30, 0x22 }, new ushort[2] { 0x31, 0x22 }, new ushort[2] { 0x32, 0x22 }, new ushort[2] { 0x33, 0x22 }, new ushort[2] { 0x34, 0x22 }, new ushort[2] { 0x35, 0x22 }, new ushort[2] { 0x41, 0x22 }, new ushort[2] { 0x43, 0x22 }, new ushort[2] { 0x44, 0x22 }, new ushort[2] { 0x45, 0x22 }, new ushort[2] { 0x46, 0x22 }, new ushort[2] { 0x47, 0x22 }, new ushort[2] { 0x49, 0x22 }, new ushort[2] { 0x4a, 0x22 }, new ushort[2] { 0x4b, 0x22 } };
        private static ushort[] stateExpectedIDs_110 = { 0xA, 0xB, 0x11, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_110 = { "NAME", "_T[.]", "_T[{]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_110 = { "[terminal_def_atom_span → terminal_def_atom_unicode .. terminal_def_atom_unicode •]" };
        private static ushort[][] stateShiftsOnTerminal_110 = {  };
        private static ushort[][] stateShiftsOnVariable_110 = {  };
        private static ushort[][] stateReducsOnTerminal_110 = { new ushort[2] { 0xa, 0x18 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x11, 0x18 }, new ushort[2] { 0x30, 0x18 }, new ushort[2] { 0x31, 0x18 }, new ushort[2] { 0x32, 0x18 }, new ushort[2] { 0x33, 0x18 }, new ushort[2] { 0x34, 0x18 }, new ushort[2] { 0x35, 0x18 }, new ushort[2] { 0x41, 0x18 }, new ushort[2] { 0x43, 0x18 }, new ushort[2] { 0x44, 0x18 }, new ushort[2] { 0x45, 0x18 }, new ushort[2] { 0x46, 0x18 }, new ushort[2] { 0x47, 0x18 }, new ushort[2] { 0x49, 0x18 }, new ushort[2] { 0x4a, 0x18 }, new ushort[2] { 0x4b, 0x18 } };
        private static ushort[] stateExpectedIDs_111 = { 0x41 };
        private static string[] stateExpectedNames_111 = { "_T[;]" };
        private static string[] stateItems_111 = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_111 = { new ushort[2] { 0x41, 0x126 } };
        private static ushort[][] stateShiftsOnVariable_111 = {  };
        private static ushort[][] stateReducsOnTerminal_111 = {  };
        private static ushort[] stateExpectedIDs_112 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_112 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_112 = { "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> • rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>]", "[rule_def_choice<grammar_bin_terminal> → •]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_112 = { new ushort[2] { 0x43, 0x71 }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_112 = { new ushort[2] { 0x8f, 0x127 }, new ushort[2] { 0x90, 0x6A }, new ushort[2] { 0x91, 0x6B }, new ushort[2] { 0x92, 0x6C }, new ushort[2] { 0x93, 0x6D }, new ushort[2] { 0x94, 0x6E }, new ushort[2] { 0x95, 0x6F }, new ushort[2] { 0x96, 0x70 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_112 = { new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x4a, 0x80 } };
        private static ushort[] stateExpectedIDs_113 = { 0x41, 0x44, 0xDB };
        private static string[] stateExpectedNames_113 = { "_T[;]", "_T[)]", "_T[]]" };
        private static string[] stateItems_113 = { "[_m188 → | rule_def_choice<grammar_bin_terminal> _m188 •]" };
        private static ushort[][] stateShiftsOnTerminal_113 = {  };
        private static ushort[][] stateShiftsOnVariable_113 = {  };
        private static ushort[][] stateReducsOnTerminal_113 = { new ushort[2] { 0x41, 0x99 }, new ushort[2] { 0x44, 0x99 }, new ushort[2] { 0xdb, 0x99 } };
        private static ushort[] stateExpectedIDs_114 = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_114 = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_114 = { "[_m186 → - rule_def_fragment<grammar_bin_terminal> _m186 •]" };
        private static ushort[][] stateShiftsOnTerminal_114 = {  };
        private static ushort[][] stateShiftsOnVariable_114 = {  };
        private static ushort[][] stateReducsOnTerminal_114 = { new ushort[2] { 0x41, 0x97 }, new ushort[2] { 0x44, 0x97 }, new ushort[2] { 0x4a, 0x97 }, new ushort[2] { 0xdb, 0x97 } };
        private static ushort[] stateExpectedIDs_115 = { 0x4E };
        private static string[] stateExpectedNames_115 = { "_T[>]" };
        private static string[] stateItems_115 = { "[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> _m175 • >]" };
        private static ushort[][] stateShiftsOnTerminal_115 = { new ushort[2] { 0x4e, 0x128 } };
        private static ushort[][] stateShiftsOnVariable_115 = {  };
        private static ushort[][] stateReducsOnTerminal_115 = {  };
        private static ushort[] stateExpectedIDs_116 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F };
        private static string[] stateExpectedNames_116 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] stateItems_116 = { "[_m175 → , • rule_def_atom<grammar_bin_terminal> _m175]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY]" };
        private static ushort[][] stateShiftsOnTerminal_116 = { new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0x79 }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3a, 0x80 }, new ushort[2] { 0x3b, 0x81 }, new ushort[2] { 0x3c, 0x82 }, new ushort[2] { 0x3d, 0x83 }, new ushort[2] { 0x3e, 0x84 }, new ushort[2] { 0x3f, 0x85 } };
        private static ushort[][] stateShiftsOnVariable_116 = { new ushort[2] { 0x96, 0x129 }, new ushort[2] { 0x68, 0x72 }, new ushort[2] { 0x69, 0x73 }, new ushort[2] { 0x6a, 0x74 }, new ushort[2] { 0x97, 0x75 }, new ushort[2] { 0x6c, 0x76 } };
        private static ushort[][] stateReducsOnTerminal_116 = {  };
        private static ushort[] stateExpectedIDs_117 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_117 = { "NAME", "_T[}]" };
        private static string[] stateItems_117 = { "[cf_rule_simple<grammar_text_terminal> → NAME -> rule_definition<grammar_text_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_117 = {  };
        private static ushort[][] stateShiftsOnVariable_117 = {  };
        private static ushort[][] stateReducsOnTerminal_117 = { new ushort[2] { 0xa, 0x5A }, new ushort[2] { 0x12, 0x5A } };
        private static ushort[] stateExpectedIDs_118 = { 0x41 };
        private static string[] stateExpectedNames_118 = { "_T[;]" };
        private static string[] stateItems_118 = { "[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> rule_definition<grammar_text_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_118 = { new ushort[2] { 0x41, 0x12A } };
        private static ushort[][] stateShiftsOnVariable_118 = {  };
        private static ushort[][] stateReducsOnTerminal_118 = {  };
        private static ushort[] stateExpectedIDs_119 = { 0xA, 0x12 };
        private static string[] stateExpectedNames_119 = { "NAME", "_T[}]" };
        private static string[] stateItems_119 = { "[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> rule_definition<grammar_bin_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_119 = {  };
        private static ushort[][] stateShiftsOnVariable_119 = {  };
        private static ushort[][] stateReducsOnTerminal_119 = { new ushort[2] { 0xa, 0x9B }, new ushort[2] { 0x12, 0x9B } };
        private static ushort[] stateExpectedIDs_11A = { 0x4C };
        private static string[] stateExpectedNames_11A = { "_T[->]" };
        private static string[] stateItems_11A = { "[rule_template_params → < NAME _m101 > •]" };
        private static ushort[][] stateShiftsOnTerminal_11A = {  };
        private static ushort[][] stateShiftsOnVariable_11A = {  };
        private static ushort[][] stateReducsOnTerminal_11A = { new ushort[2] { 0x4c, 0x33 } };
        private static ushort[] stateExpectedIDs_11B = { 0x48, 0x4E };
        private static string[] stateExpectedNames_11B = { "_T[,]", "_T[>]" };
        private static string[] stateItems_11B = { "[_m101 → , NAME • _m101]", "[_m101 → • , NAME _m101]", "[_m101 → •]" };
        private static ushort[][] stateShiftsOnTerminal_11B = { new ushort[2] { 0x48, 0x101 } };
        private static ushort[][] stateShiftsOnVariable_11B = { new ushort[2] { 0x77, 0x12B } };
        private static ushort[][] stateReducsOnTerminal_11B = { new ushort[2] { 0x4e, 0x52 } };
        private static ushort[] stateExpectedIDs_11C = { 0x41 };
        private static string[] stateExpectedNames_11C = { "_T[;]" };
        private static string[] stateItems_11C = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_11C = { new ushort[2] { 0x41, 0x12C } };
        private static ushort[][] stateShiftsOnVariable_11C = {  };
        private static ushort[][] stateReducsOnTerminal_11C = {  };
        private static ushort[] stateExpectedIDs_11D = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x4A };
        private static string[] stateExpectedNames_11D = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[|]" };
        private static string[] stateItems_11D = { "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> • rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>]", "[rule_def_choice<grammar_text_terminal> → •]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_11D = { new ushort[2] { 0x43, 0x9D }, new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_11D = { new ushort[2] { 0x7d, 0x12D }, new ushort[2] { 0x7e, 0x96 }, new ushort[2] { 0x7f, 0x97 }, new ushort[2] { 0x80, 0x98 }, new ushort[2] { 0x81, 0x99 }, new ushort[2] { 0x82, 0x9A }, new ushort[2] { 0x83, 0x9B }, new ushort[2] { 0x84, 0x9C }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_11D = { new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x4a, 0x5D } };
        private static ushort[] stateExpectedIDs_11E = { 0x41, 0x44, 0xDB };
        private static string[] stateExpectedNames_11E = { "_T[;]", "_T[)]", "_T[]]" };
        private static string[] stateItems_11E = { "[_m147 → | rule_def_choice<grammar_text_terminal> _m147 •]" };
        private static ushort[][] stateShiftsOnTerminal_11E = {  };
        private static ushort[][] stateShiftsOnVariable_11E = {  };
        private static ushort[][] stateReducsOnTerminal_11E = { new ushort[2] { 0x41, 0x76 }, new ushort[2] { 0x44, 0x76 }, new ushort[2] { 0xdb, 0x76 } };
        private static ushort[] stateExpectedIDs_11F = { 0x41, 0x44, 0x4A, 0xDB };
        private static string[] stateExpectedNames_11F = { "_T[;]", "_T[)]", "_T[|]", "_T[]]" };
        private static string[] stateItems_11F = { "[_m145 → - rule_def_fragment<grammar_text_terminal> _m145 •]" };
        private static ushort[][] stateShiftsOnTerminal_11F = {  };
        private static ushort[][] stateShiftsOnVariable_11F = {  };
        private static ushort[][] stateReducsOnTerminal_11F = { new ushort[2] { 0x41, 0x74 }, new ushort[2] { 0x44, 0x74 }, new ushort[2] { 0x4a, 0x74 }, new ushort[2] { 0xdb, 0x74 } };
        private static ushort[] stateExpectedIDs_120 = { 0x4E };
        private static string[] stateExpectedNames_120 = { "_T[>]" };
        private static string[] stateItems_120 = { "[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> _m134 • >]" };
        private static ushort[][] stateShiftsOnTerminal_120 = { new ushort[2] { 0x4e, 0x12E } };
        private static ushort[][] stateShiftsOnVariable_120 = {  };
        private static ushort[][] stateReducsOnTerminal_120 = {  };
        private static ushort[] stateExpectedIDs_121 = { 0xA, 0x11, 0x2E, 0x30 };
        private static string[] stateExpectedNames_121 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT" };
        private static string[] stateItems_121 = { "[_m134 → , • rule_def_atom<grammar_text_terminal> _m134]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal]", "[rule_sym_action → • { NAME }]", "[rule_sym_virtual → • QUOTED_DATA]", "[rule_sym_ref_simple → • NAME]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal → • terminal_def_atom_text]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] stateShiftsOnTerminal_121 = { new ushort[2] { 0x11, 0x77 }, new ushort[2] { 0x2e, 0x78 }, new ushort[2] { 0xa, 0xA3 }, new ushort[2] { 0x30, 0xA5 } };
        private static ushort[][] stateShiftsOnVariable_121 = { new ushort[2] { 0x84, 0x12F }, new ushort[2] { 0x68, 0x9E }, new ushort[2] { 0x69, 0x9F }, new ushort[2] { 0x6a, 0xA0 }, new ushort[2] { 0x85, 0xA1 }, new ushort[2] { 0x6d, 0xA2 }, new ushort[2] { 0x5a, 0xA4 } };
        private static ushort[][] stateReducsOnTerminal_121 = {  };
        private static ushort[] stateExpectedIDs_122 = { 0x41, 0x44, 0x4B };
        private static string[] stateExpectedNames_122 = { "_T[;]", "_T[)]", "_T[=>]" };
        private static string[] stateItems_122 = { "[_m93 → | terminal_def_restrict _m93 •]" };
        private static ushort[][] stateShiftsOnTerminal_122 = {  };
        private static ushort[][] stateShiftsOnVariable_122 = {  };
        private static ushort[][] stateReducsOnTerminal_122 = { new ushort[2] { 0x41, 0x4F }, new ushort[2] { 0x44, 0x4F }, new ushort[2] { 0x4b, 0x4F } };
        private static ushort[] stateExpectedIDs_123 = { 0x41, 0x44, 0x4A, 0x4B };
        private static string[] stateExpectedNames_123 = { "_T[;]", "_T[)]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_123 = { "[_m91 → - terminal_def_fragment _m91 •]" };
        private static ushort[][] stateShiftsOnTerminal_123 = {  };
        private static ushort[][] stateShiftsOnVariable_123 = {  };
        private static ushort[][] stateReducsOnTerminal_123 = { new ushort[2] { 0x41, 0x4D }, new ushort[2] { 0x44, 0x4D }, new ushort[2] { 0x4a, 0x4D }, new ushort[2] { 0x4b, 0x4D } };
        private static ushort[] stateExpectedIDs_124 = { 0x2D };
        private static string[] stateExpectedNames_124 = { "INTEGER" };
        private static string[] stateItems_124 = { "[terminal_def_cardinalilty → { INTEGER , • INTEGER }]" };
        private static ushort[][] stateShiftsOnTerminal_124 = { new ushort[2] { 0x2d, 0x130 } };
        private static ushort[][] stateShiftsOnVariable_124 = {  };
        private static ushort[][] stateReducsOnTerminal_124 = {  };
        private static ushort[] stateExpectedIDs_125 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_125 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_125 = { "[terminal_def_cardinalilty → { INTEGER } •]" };
        private static ushort[][] stateShiftsOnTerminal_125 = {  };
        private static ushort[][] stateShiftsOnVariable_125 = {  };
        private static ushort[][] stateReducsOnTerminal_125 = { new ushort[2] { 0xa, 0x27 }, new ushort[2] { 0xb, 0x27 }, new ushort[2] { 0x30, 0x27 }, new ushort[2] { 0x31, 0x27 }, new ushort[2] { 0x32, 0x27 }, new ushort[2] { 0x33, 0x27 }, new ushort[2] { 0x34, 0x27 }, new ushort[2] { 0x35, 0x27 }, new ushort[2] { 0x41, 0x27 }, new ushort[2] { 0x43, 0x27 }, new ushort[2] { 0x44, 0x27 }, new ushort[2] { 0x49, 0x27 }, new ushort[2] { 0x4a, 0x27 }, new ushort[2] { 0x4b, 0x27 } };
        private static ushort[] stateExpectedIDs_126 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_126 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_126 = { "[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_126 = {  };
        private static ushort[][] stateShiftsOnVariable_126 = {  };
        private static ushort[][] stateReducsOnTerminal_126 = { new ushort[2] { 0xa, 0xAA }, new ushort[2] { 0x12, 0xAA }, new ushort[2] { 0xda, 0xAA } };
        private static ushort[] stateExpectedIDs_127 = { 0x41 };
        private static string[] stateExpectedNames_127 = { "_T[;]" };
        private static string[] stateItems_127 = { "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_127 = { new ushort[2] { 0x41, 0x131 } };
        private static ushort[][] stateShiftsOnVariable_127 = {  };
        private static ushort[][] stateReducsOnTerminal_127 = {  };
        private static ushort[] stateExpectedIDs_128 = { 0xA, 0x11, 0x2E, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_128 = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_128 = { "[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> _m175 > •]" };
        private static ushort[][] stateShiftsOnTerminal_128 = {  };
        private static ushort[][] stateShiftsOnVariable_128 = {  };
        private static ushort[][] stateReducsOnTerminal_128 = { new ushort[2] { 0xa, 0x92 }, new ushort[2] { 0x11, 0x92 }, new ushort[2] { 0x2e, 0x92 }, new ushort[2] { 0x34, 0x92 }, new ushort[2] { 0x35, 0x92 }, new ushort[2] { 0x36, 0x92 }, new ushort[2] { 0x37, 0x92 }, new ushort[2] { 0x38, 0x92 }, new ushort[2] { 0x39, 0x92 }, new ushort[2] { 0x3a, 0x92 }, new ushort[2] { 0x3b, 0x92 }, new ushort[2] { 0x3c, 0x92 }, new ushort[2] { 0x3d, 0x92 }, new ushort[2] { 0x3e, 0x92 }, new ushort[2] { 0x3f, 0x92 }, new ushort[2] { 0x41, 0x92 }, new ushort[2] { 0x43, 0x92 }, new ushort[2] { 0x44, 0x92 }, new ushort[2] { 0x45, 0x92 }, new ushort[2] { 0x46, 0x92 }, new ushort[2] { 0x47, 0x92 }, new ushort[2] { 0x48, 0x92 }, new ushort[2] { 0x49, 0x92 }, new ushort[2] { 0x4a, 0x92 }, new ushort[2] { 0x4e, 0x92 }, new ushort[2] { 0x55, 0x92 }, new ushort[2] { 0x56, 0x92 }, new ushort[2] { 0xdb, 0x92 } };
        private static ushort[] stateExpectedIDs_129 = { 0x48, 0x4E };
        private static string[] stateExpectedNames_129 = { "_T[,]", "_T[>]" };
        private static string[] stateItems_129 = { "[_m175 → , rule_def_atom<grammar_bin_terminal> • _m175]", "[_m175 → • , rule_def_atom<grammar_bin_terminal> _m175]", "[_m175 → •]" };
        private static ushort[][] stateShiftsOnTerminal_129 = { new ushort[2] { 0x48, 0x116 } };
        private static ushort[][] stateShiftsOnVariable_129 = { new ushort[2] { 0x99, 0x132 } };
        private static ushort[][] stateReducsOnTerminal_129 = { new ushort[2] { 0x4e, 0x94 } };
        private static ushort[] stateExpectedIDs_12A = { 0xA, 0x12 };
        private static string[] stateExpectedNames_12A = { "NAME", "_T[}]" };
        private static string[] stateItems_12A = { "[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> rule_definition<grammar_text_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_12A = {  };
        private static ushort[][] stateShiftsOnVariable_12A = {  };
        private static ushort[][] stateReducsOnTerminal_12A = { new ushort[2] { 0xa, 0x78 }, new ushort[2] { 0x12, 0x78 } };
        private static ushort[] stateExpectedIDs_12B = { 0x4E };
        private static string[] stateExpectedNames_12B = { "_T[>]" };
        private static string[] stateItems_12B = { "[_m101 → , NAME _m101 •]" };
        private static ushort[][] stateShiftsOnTerminal_12B = {  };
        private static ushort[][] stateShiftsOnVariable_12B = {  };
        private static ushort[][] stateReducsOnTerminal_12B = { new ushort[2] { 0x4e, 0x51 } };
        private static ushort[] stateExpectedIDs_12C = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_12C = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_12C = { "[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_12C = {  };
        private static ushort[][] stateShiftsOnVariable_12C = {  };
        private static ushort[][] stateReducsOnTerminal_12C = { new ushort[2] { 0xa, 0xA2 }, new ushort[2] { 0x12, 0xA2 }, new ushort[2] { 0xda, 0xA2 } };
        private static ushort[] stateExpectedIDs_12D = { 0x41 };
        private static string[] stateExpectedNames_12D = { "_T[;]" };
        private static string[] stateItems_12D = { "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> • ;]" };
        private static ushort[][] stateShiftsOnTerminal_12D = { new ushort[2] { 0x41, 0x133 } };
        private static ushort[][] stateShiftsOnVariable_12D = {  };
        private static ushort[][] stateReducsOnTerminal_12D = {  };
        private static ushort[] stateExpectedIDs_12E = { 0xA, 0x11, 0x2E, 0x30, 0x41, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4E, 0x55, 0x56, 0xDB };
        private static string[] stateExpectedNames_12E = { "NAME", "_T[{]", "QUOTED_DATA", "SYMBOL_TERMINAL_TEXT", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[,]", "_T[-]", "_T[|]", "_T[>]", "_T[^]", "_T[!]", "_T[]]" };
        private static string[] stateItems_12E = { "[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> _m134 > •]" };
        private static ushort[][] stateShiftsOnTerminal_12E = {  };
        private static ushort[][] stateShiftsOnVariable_12E = {  };
        private static ushort[][] stateReducsOnTerminal_12E = { new ushort[2] { 0xa, 0x6F }, new ushort[2] { 0x11, 0x6F }, new ushort[2] { 0x2e, 0x6F }, new ushort[2] { 0x30, 0x6F }, new ushort[2] { 0x41, 0x6F }, new ushort[2] { 0x43, 0x6F }, new ushort[2] { 0x44, 0x6F }, new ushort[2] { 0x45, 0x6F }, new ushort[2] { 0x46, 0x6F }, new ushort[2] { 0x47, 0x6F }, new ushort[2] { 0x48, 0x6F }, new ushort[2] { 0x49, 0x6F }, new ushort[2] { 0x4a, 0x6F }, new ushort[2] { 0x4e, 0x6F }, new ushort[2] { 0x55, 0x6F }, new ushort[2] { 0x56, 0x6F }, new ushort[2] { 0xdb, 0x6F } };
        private static ushort[] stateExpectedIDs_12F = { 0x48, 0x4E };
        private static string[] stateExpectedNames_12F = { "_T[,]", "_T[>]" };
        private static string[] stateItems_12F = { "[_m134 → , rule_def_atom<grammar_text_terminal> • _m134]", "[_m134 → • , rule_def_atom<grammar_text_terminal> _m134]", "[_m134 → •]" };
        private static ushort[][] stateShiftsOnTerminal_12F = { new ushort[2] { 0x48, 0x121 } };
        private static ushort[][] stateShiftsOnVariable_12F = { new ushort[2] { 0x87, 0x134 } };
        private static ushort[][] stateReducsOnTerminal_12F = { new ushort[2] { 0x4e, 0x71 } };
        private static ushort[] stateExpectedIDs_130 = { 0x12 };
        private static string[] stateExpectedNames_130 = { "_T[}]" };
        private static string[] stateItems_130 = { "[terminal_def_cardinalilty → { INTEGER , INTEGER • }]" };
        private static ushort[][] stateShiftsOnTerminal_130 = { new ushort[2] { 0x12, 0x135 } };
        private static ushort[][] stateShiftsOnVariable_130 = {  };
        private static ushort[][] stateReducsOnTerminal_130 = {  };
        private static ushort[] stateExpectedIDs_131 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_131 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_131 = { "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_131 = {  };
        private static ushort[][] stateShiftsOnVariable_131 = {  };
        private static ushort[][] stateReducsOnTerminal_131 = { new ushort[2] { 0xa, 0xAD }, new ushort[2] { 0x12, 0xAD }, new ushort[2] { 0xda, 0xAD } };
        private static ushort[] stateExpectedIDs_132 = { 0x4E };
        private static string[] stateExpectedNames_132 = { "_T[>]" };
        private static string[] stateItems_132 = { "[_m175 → , rule_def_atom<grammar_bin_terminal> _m175 •]" };
        private static ushort[][] stateShiftsOnTerminal_132 = {  };
        private static ushort[][] stateShiftsOnVariable_132 = {  };
        private static ushort[][] stateReducsOnTerminal_132 = { new ushort[2] { 0x4e, 0x93 } };
        private static ushort[] stateExpectedIDs_133 = { 0xA, 0x12, 0xDA };
        private static string[] stateExpectedNames_133 = { "NAME", "_T[}]", "_T[[]" };
        private static string[] stateItems_133 = { "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ; •]" };
        private static ushort[][] stateShiftsOnTerminal_133 = {  };
        private static ushort[][] stateShiftsOnVariable_133 = {  };
        private static ushort[][] stateReducsOnTerminal_133 = { new ushort[2] { 0xa, 0xA5 }, new ushort[2] { 0x12, 0xA5 }, new ushort[2] { 0xda, 0xA5 } };
        private static ushort[] stateExpectedIDs_134 = { 0x4E };
        private static string[] stateExpectedNames_134 = { "_T[>]" };
        private static string[] stateItems_134 = { "[_m134 → , rule_def_atom<grammar_text_terminal> _m134 •]" };
        private static ushort[][] stateShiftsOnTerminal_134 = {  };
        private static ushort[][] stateShiftsOnVariable_134 = {  };
        private static ushort[][] stateReducsOnTerminal_134 = { new ushort[2] { 0x4e, 0x70 } };
        private static ushort[] stateExpectedIDs_135 = { 0xA, 0xB, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x41, 0x43, 0x44, 0x49, 0x4A, 0x4B };
        private static string[] stateExpectedNames_135 = { "NAME", "_T[.]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[(]", "_T[)]", "_T[-]", "_T[|]", "_T[=>]" };
        private static string[] stateItems_135 = { "[terminal_def_cardinalilty → { INTEGER , INTEGER } •]" };
        private static ushort[][] stateShiftsOnTerminal_135 = {  };
        private static ushort[][] stateShiftsOnVariable_135 = {  };
        private static ushort[][] stateReducsOnTerminal_135 = { new ushort[2] { 0xa, 0x26 }, new ushort[2] { 0xb, 0x26 }, new ushort[2] { 0x30, 0x26 }, new ushort[2] { 0x31, 0x26 }, new ushort[2] { 0x32, 0x26 }, new ushort[2] { 0x33, 0x26 }, new ushort[2] { 0x34, 0x26 }, new ushort[2] { 0x35, 0x26 }, new ushort[2] { 0x41, 0x26 }, new ushort[2] { 0x43, 0x26 }, new ushort[2] { 0x44, 0x26 }, new ushort[2] { 0x49, 0x26 }, new ushort[2] { 0x4a, 0x26 }, new ushort[2] { 0x4b, 0x26 } };
        private static ushort[][] staticStateExpectedIDs = { stateExpectedIDs_0, stateExpectedIDs_1, stateExpectedIDs_2, stateExpectedIDs_3, stateExpectedIDs_4, stateExpectedIDs_5, stateExpectedIDs_6, stateExpectedIDs_7, stateExpectedIDs_8, stateExpectedIDs_9, stateExpectedIDs_A, stateExpectedIDs_B, stateExpectedIDs_C, stateExpectedIDs_D, stateExpectedIDs_E, stateExpectedIDs_F, stateExpectedIDs_10, stateExpectedIDs_11, stateExpectedIDs_12, stateExpectedIDs_13, stateExpectedIDs_14, stateExpectedIDs_15, stateExpectedIDs_16, stateExpectedIDs_17, stateExpectedIDs_18, stateExpectedIDs_19, stateExpectedIDs_1A, stateExpectedIDs_1B, stateExpectedIDs_1C, stateExpectedIDs_1D, stateExpectedIDs_1E, stateExpectedIDs_1F, stateExpectedIDs_20, stateExpectedIDs_21, stateExpectedIDs_22, stateExpectedIDs_23, stateExpectedIDs_24, stateExpectedIDs_25, stateExpectedIDs_26, stateExpectedIDs_27, stateExpectedIDs_28, stateExpectedIDs_29, stateExpectedIDs_2A, stateExpectedIDs_2B, stateExpectedIDs_2C, stateExpectedIDs_2D, stateExpectedIDs_2E, stateExpectedIDs_2F, stateExpectedIDs_30, stateExpectedIDs_31, stateExpectedIDs_32, stateExpectedIDs_33, stateExpectedIDs_34, stateExpectedIDs_35, stateExpectedIDs_36, stateExpectedIDs_37, stateExpectedIDs_38, stateExpectedIDs_39, stateExpectedIDs_3A, stateExpectedIDs_3B, stateExpectedIDs_3C, stateExpectedIDs_3D, stateExpectedIDs_3E, stateExpectedIDs_3F, stateExpectedIDs_40, stateExpectedIDs_41, stateExpectedIDs_42, stateExpectedIDs_43, stateExpectedIDs_44, stateExpectedIDs_45, stateExpectedIDs_46, stateExpectedIDs_47, stateExpectedIDs_48, stateExpectedIDs_49, stateExpectedIDs_4A, stateExpectedIDs_4B, stateExpectedIDs_4C, stateExpectedIDs_4D, stateExpectedIDs_4E, stateExpectedIDs_4F, stateExpectedIDs_50, stateExpectedIDs_51, stateExpectedIDs_52, stateExpectedIDs_53, stateExpectedIDs_54, stateExpectedIDs_55, stateExpectedIDs_56, stateExpectedIDs_57, stateExpectedIDs_58, stateExpectedIDs_59, stateExpectedIDs_5A, stateExpectedIDs_5B, stateExpectedIDs_5C, stateExpectedIDs_5D, stateExpectedIDs_5E, stateExpectedIDs_5F, stateExpectedIDs_60, stateExpectedIDs_61, stateExpectedIDs_62, stateExpectedIDs_63, stateExpectedIDs_64, stateExpectedIDs_65, stateExpectedIDs_66, stateExpectedIDs_67, stateExpectedIDs_68, stateExpectedIDs_69, stateExpectedIDs_6A, stateExpectedIDs_6B, stateExpectedIDs_6C, stateExpectedIDs_6D, stateExpectedIDs_6E, stateExpectedIDs_6F, stateExpectedIDs_70, stateExpectedIDs_71, stateExpectedIDs_72, stateExpectedIDs_73, stateExpectedIDs_74, stateExpectedIDs_75, stateExpectedIDs_76, stateExpectedIDs_77, stateExpectedIDs_78, stateExpectedIDs_79, stateExpectedIDs_7A, stateExpectedIDs_7B, stateExpectedIDs_7C, stateExpectedIDs_7D, stateExpectedIDs_7E, stateExpectedIDs_7F, stateExpectedIDs_80, stateExpectedIDs_81, stateExpectedIDs_82, stateExpectedIDs_83, stateExpectedIDs_84, stateExpectedIDs_85, stateExpectedIDs_86, stateExpectedIDs_87, stateExpectedIDs_88, stateExpectedIDs_89, stateExpectedIDs_8A, stateExpectedIDs_8B, stateExpectedIDs_8C, stateExpectedIDs_8D, stateExpectedIDs_8E, stateExpectedIDs_8F, stateExpectedIDs_90, stateExpectedIDs_91, stateExpectedIDs_92, stateExpectedIDs_93, stateExpectedIDs_94, stateExpectedIDs_95, stateExpectedIDs_96, stateExpectedIDs_97, stateExpectedIDs_98, stateExpectedIDs_99, stateExpectedIDs_9A, stateExpectedIDs_9B, stateExpectedIDs_9C, stateExpectedIDs_9D, stateExpectedIDs_9E, stateExpectedIDs_9F, stateExpectedIDs_A0, stateExpectedIDs_A1, stateExpectedIDs_A2, stateExpectedIDs_A3, stateExpectedIDs_A4, stateExpectedIDs_A5, stateExpectedIDs_A6, stateExpectedIDs_A7, stateExpectedIDs_A8, stateExpectedIDs_A9, stateExpectedIDs_AA, stateExpectedIDs_AB, stateExpectedIDs_AC, stateExpectedIDs_AD, stateExpectedIDs_AE, stateExpectedIDs_AF, stateExpectedIDs_B0, stateExpectedIDs_B1, stateExpectedIDs_B2, stateExpectedIDs_B3, stateExpectedIDs_B4, stateExpectedIDs_B5, stateExpectedIDs_B6, stateExpectedIDs_B7, stateExpectedIDs_B8, stateExpectedIDs_B9, stateExpectedIDs_BA, stateExpectedIDs_BB, stateExpectedIDs_BC, stateExpectedIDs_BD, stateExpectedIDs_BE, stateExpectedIDs_BF, stateExpectedIDs_C0, stateExpectedIDs_C1, stateExpectedIDs_C2, stateExpectedIDs_C3, stateExpectedIDs_C4, stateExpectedIDs_C5, stateExpectedIDs_C6, stateExpectedIDs_C7, stateExpectedIDs_C8, stateExpectedIDs_C9, stateExpectedIDs_CA, stateExpectedIDs_CB, stateExpectedIDs_CC, stateExpectedIDs_CD, stateExpectedIDs_CE, stateExpectedIDs_CF, stateExpectedIDs_D0, stateExpectedIDs_D1, stateExpectedIDs_D2, stateExpectedIDs_D3, stateExpectedIDs_D4, stateExpectedIDs_D5, stateExpectedIDs_D6, stateExpectedIDs_D7, stateExpectedIDs_D8, stateExpectedIDs_D9, stateExpectedIDs_DA, stateExpectedIDs_DB, stateExpectedIDs_DC, stateExpectedIDs_DD, stateExpectedIDs_DE, stateExpectedIDs_DF, stateExpectedIDs_E0, stateExpectedIDs_E1, stateExpectedIDs_E2, stateExpectedIDs_E3, stateExpectedIDs_E4, stateExpectedIDs_E5, stateExpectedIDs_E6, stateExpectedIDs_E7, stateExpectedIDs_E8, stateExpectedIDs_E9, stateExpectedIDs_EA, stateExpectedIDs_EB, stateExpectedIDs_EC, stateExpectedIDs_ED, stateExpectedIDs_EE, stateExpectedIDs_EF, stateExpectedIDs_F0, stateExpectedIDs_F1, stateExpectedIDs_F2, stateExpectedIDs_F3, stateExpectedIDs_F4, stateExpectedIDs_F5, stateExpectedIDs_F6, stateExpectedIDs_F7, stateExpectedIDs_F8, stateExpectedIDs_F9, stateExpectedIDs_FA, stateExpectedIDs_FB, stateExpectedIDs_FC, stateExpectedIDs_FD, stateExpectedIDs_FE, stateExpectedIDs_FF, stateExpectedIDs_100, stateExpectedIDs_101, stateExpectedIDs_102, stateExpectedIDs_103, stateExpectedIDs_104, stateExpectedIDs_105, stateExpectedIDs_106, stateExpectedIDs_107, stateExpectedIDs_108, stateExpectedIDs_109, stateExpectedIDs_10A, stateExpectedIDs_10B, stateExpectedIDs_10C, stateExpectedIDs_10D, stateExpectedIDs_10E, stateExpectedIDs_10F, stateExpectedIDs_110, stateExpectedIDs_111, stateExpectedIDs_112, stateExpectedIDs_113, stateExpectedIDs_114, stateExpectedIDs_115, stateExpectedIDs_116, stateExpectedIDs_117, stateExpectedIDs_118, stateExpectedIDs_119, stateExpectedIDs_11A, stateExpectedIDs_11B, stateExpectedIDs_11C, stateExpectedIDs_11D, stateExpectedIDs_11E, stateExpectedIDs_11F, stateExpectedIDs_120, stateExpectedIDs_121, stateExpectedIDs_122, stateExpectedIDs_123, stateExpectedIDs_124, stateExpectedIDs_125, stateExpectedIDs_126, stateExpectedIDs_127, stateExpectedIDs_128, stateExpectedIDs_129, stateExpectedIDs_12A, stateExpectedIDs_12B, stateExpectedIDs_12C, stateExpectedIDs_12D, stateExpectedIDs_12E, stateExpectedIDs_12F, stateExpectedIDs_130, stateExpectedIDs_131, stateExpectedIDs_132, stateExpectedIDs_133, stateExpectedIDs_134, stateExpectedIDs_135 };
        private static string[][] staticStateExpectedNames = { stateExpectedNames_0, stateExpectedNames_1, stateExpectedNames_2, stateExpectedNames_3, stateExpectedNames_4, stateExpectedNames_5, stateExpectedNames_6, stateExpectedNames_7, stateExpectedNames_8, stateExpectedNames_9, stateExpectedNames_A, stateExpectedNames_B, stateExpectedNames_C, stateExpectedNames_D, stateExpectedNames_E, stateExpectedNames_F, stateExpectedNames_10, stateExpectedNames_11, stateExpectedNames_12, stateExpectedNames_13, stateExpectedNames_14, stateExpectedNames_15, stateExpectedNames_16, stateExpectedNames_17, stateExpectedNames_18, stateExpectedNames_19, stateExpectedNames_1A, stateExpectedNames_1B, stateExpectedNames_1C, stateExpectedNames_1D, stateExpectedNames_1E, stateExpectedNames_1F, stateExpectedNames_20, stateExpectedNames_21, stateExpectedNames_22, stateExpectedNames_23, stateExpectedNames_24, stateExpectedNames_25, stateExpectedNames_26, stateExpectedNames_27, stateExpectedNames_28, stateExpectedNames_29, stateExpectedNames_2A, stateExpectedNames_2B, stateExpectedNames_2C, stateExpectedNames_2D, stateExpectedNames_2E, stateExpectedNames_2F, stateExpectedNames_30, stateExpectedNames_31, stateExpectedNames_32, stateExpectedNames_33, stateExpectedNames_34, stateExpectedNames_35, stateExpectedNames_36, stateExpectedNames_37, stateExpectedNames_38, stateExpectedNames_39, stateExpectedNames_3A, stateExpectedNames_3B, stateExpectedNames_3C, stateExpectedNames_3D, stateExpectedNames_3E, stateExpectedNames_3F, stateExpectedNames_40, stateExpectedNames_41, stateExpectedNames_42, stateExpectedNames_43, stateExpectedNames_44, stateExpectedNames_45, stateExpectedNames_46, stateExpectedNames_47, stateExpectedNames_48, stateExpectedNames_49, stateExpectedNames_4A, stateExpectedNames_4B, stateExpectedNames_4C, stateExpectedNames_4D, stateExpectedNames_4E, stateExpectedNames_4F, stateExpectedNames_50, stateExpectedNames_51, stateExpectedNames_52, stateExpectedNames_53, stateExpectedNames_54, stateExpectedNames_55, stateExpectedNames_56, stateExpectedNames_57, stateExpectedNames_58, stateExpectedNames_59, stateExpectedNames_5A, stateExpectedNames_5B, stateExpectedNames_5C, stateExpectedNames_5D, stateExpectedNames_5E, stateExpectedNames_5F, stateExpectedNames_60, stateExpectedNames_61, stateExpectedNames_62, stateExpectedNames_63, stateExpectedNames_64, stateExpectedNames_65, stateExpectedNames_66, stateExpectedNames_67, stateExpectedNames_68, stateExpectedNames_69, stateExpectedNames_6A, stateExpectedNames_6B, stateExpectedNames_6C, stateExpectedNames_6D, stateExpectedNames_6E, stateExpectedNames_6F, stateExpectedNames_70, stateExpectedNames_71, stateExpectedNames_72, stateExpectedNames_73, stateExpectedNames_74, stateExpectedNames_75, stateExpectedNames_76, stateExpectedNames_77, stateExpectedNames_78, stateExpectedNames_79, stateExpectedNames_7A, stateExpectedNames_7B, stateExpectedNames_7C, stateExpectedNames_7D, stateExpectedNames_7E, stateExpectedNames_7F, stateExpectedNames_80, stateExpectedNames_81, stateExpectedNames_82, stateExpectedNames_83, stateExpectedNames_84, stateExpectedNames_85, stateExpectedNames_86, stateExpectedNames_87, stateExpectedNames_88, stateExpectedNames_89, stateExpectedNames_8A, stateExpectedNames_8B, stateExpectedNames_8C, stateExpectedNames_8D, stateExpectedNames_8E, stateExpectedNames_8F, stateExpectedNames_90, stateExpectedNames_91, stateExpectedNames_92, stateExpectedNames_93, stateExpectedNames_94, stateExpectedNames_95, stateExpectedNames_96, stateExpectedNames_97, stateExpectedNames_98, stateExpectedNames_99, stateExpectedNames_9A, stateExpectedNames_9B, stateExpectedNames_9C, stateExpectedNames_9D, stateExpectedNames_9E, stateExpectedNames_9F, stateExpectedNames_A0, stateExpectedNames_A1, stateExpectedNames_A2, stateExpectedNames_A3, stateExpectedNames_A4, stateExpectedNames_A5, stateExpectedNames_A6, stateExpectedNames_A7, stateExpectedNames_A8, stateExpectedNames_A9, stateExpectedNames_AA, stateExpectedNames_AB, stateExpectedNames_AC, stateExpectedNames_AD, stateExpectedNames_AE, stateExpectedNames_AF, stateExpectedNames_B0, stateExpectedNames_B1, stateExpectedNames_B2, stateExpectedNames_B3, stateExpectedNames_B4, stateExpectedNames_B5, stateExpectedNames_B6, stateExpectedNames_B7, stateExpectedNames_B8, stateExpectedNames_B9, stateExpectedNames_BA, stateExpectedNames_BB, stateExpectedNames_BC, stateExpectedNames_BD, stateExpectedNames_BE, stateExpectedNames_BF, stateExpectedNames_C0, stateExpectedNames_C1, stateExpectedNames_C2, stateExpectedNames_C3, stateExpectedNames_C4, stateExpectedNames_C5, stateExpectedNames_C6, stateExpectedNames_C7, stateExpectedNames_C8, stateExpectedNames_C9, stateExpectedNames_CA, stateExpectedNames_CB, stateExpectedNames_CC, stateExpectedNames_CD, stateExpectedNames_CE, stateExpectedNames_CF, stateExpectedNames_D0, stateExpectedNames_D1, stateExpectedNames_D2, stateExpectedNames_D3, stateExpectedNames_D4, stateExpectedNames_D5, stateExpectedNames_D6, stateExpectedNames_D7, stateExpectedNames_D8, stateExpectedNames_D9, stateExpectedNames_DA, stateExpectedNames_DB, stateExpectedNames_DC, stateExpectedNames_DD, stateExpectedNames_DE, stateExpectedNames_DF, stateExpectedNames_E0, stateExpectedNames_E1, stateExpectedNames_E2, stateExpectedNames_E3, stateExpectedNames_E4, stateExpectedNames_E5, stateExpectedNames_E6, stateExpectedNames_E7, stateExpectedNames_E8, stateExpectedNames_E9, stateExpectedNames_EA, stateExpectedNames_EB, stateExpectedNames_EC, stateExpectedNames_ED, stateExpectedNames_EE, stateExpectedNames_EF, stateExpectedNames_F0, stateExpectedNames_F1, stateExpectedNames_F2, stateExpectedNames_F3, stateExpectedNames_F4, stateExpectedNames_F5, stateExpectedNames_F6, stateExpectedNames_F7, stateExpectedNames_F8, stateExpectedNames_F9, stateExpectedNames_FA, stateExpectedNames_FB, stateExpectedNames_FC, stateExpectedNames_FD, stateExpectedNames_FE, stateExpectedNames_FF, stateExpectedNames_100, stateExpectedNames_101, stateExpectedNames_102, stateExpectedNames_103, stateExpectedNames_104, stateExpectedNames_105, stateExpectedNames_106, stateExpectedNames_107, stateExpectedNames_108, stateExpectedNames_109, stateExpectedNames_10A, stateExpectedNames_10B, stateExpectedNames_10C, stateExpectedNames_10D, stateExpectedNames_10E, stateExpectedNames_10F, stateExpectedNames_110, stateExpectedNames_111, stateExpectedNames_112, stateExpectedNames_113, stateExpectedNames_114, stateExpectedNames_115, stateExpectedNames_116, stateExpectedNames_117, stateExpectedNames_118, stateExpectedNames_119, stateExpectedNames_11A, stateExpectedNames_11B, stateExpectedNames_11C, stateExpectedNames_11D, stateExpectedNames_11E, stateExpectedNames_11F, stateExpectedNames_120, stateExpectedNames_121, stateExpectedNames_122, stateExpectedNames_123, stateExpectedNames_124, stateExpectedNames_125, stateExpectedNames_126, stateExpectedNames_127, stateExpectedNames_128, stateExpectedNames_129, stateExpectedNames_12A, stateExpectedNames_12B, stateExpectedNames_12C, stateExpectedNames_12D, stateExpectedNames_12E, stateExpectedNames_12F, stateExpectedNames_130, stateExpectedNames_131, stateExpectedNames_132, stateExpectedNames_133, stateExpectedNames_134, stateExpectedNames_135 };
        private static string[][] staticStateItems = { stateItems_0, stateItems_1, stateItems_2, stateItems_3, stateItems_4, stateItems_5, stateItems_6, stateItems_7, stateItems_8, stateItems_9, stateItems_A, stateItems_B, stateItems_C, stateItems_D, stateItems_E, stateItems_F, stateItems_10, stateItems_11, stateItems_12, stateItems_13, stateItems_14, stateItems_15, stateItems_16, stateItems_17, stateItems_18, stateItems_19, stateItems_1A, stateItems_1B, stateItems_1C, stateItems_1D, stateItems_1E, stateItems_1F, stateItems_20, stateItems_21, stateItems_22, stateItems_23, stateItems_24, stateItems_25, stateItems_26, stateItems_27, stateItems_28, stateItems_29, stateItems_2A, stateItems_2B, stateItems_2C, stateItems_2D, stateItems_2E, stateItems_2F, stateItems_30, stateItems_31, stateItems_32, stateItems_33, stateItems_34, stateItems_35, stateItems_36, stateItems_37, stateItems_38, stateItems_39, stateItems_3A, stateItems_3B, stateItems_3C, stateItems_3D, stateItems_3E, stateItems_3F, stateItems_40, stateItems_41, stateItems_42, stateItems_43, stateItems_44, stateItems_45, stateItems_46, stateItems_47, stateItems_48, stateItems_49, stateItems_4A, stateItems_4B, stateItems_4C, stateItems_4D, stateItems_4E, stateItems_4F, stateItems_50, stateItems_51, stateItems_52, stateItems_53, stateItems_54, stateItems_55, stateItems_56, stateItems_57, stateItems_58, stateItems_59, stateItems_5A, stateItems_5B, stateItems_5C, stateItems_5D, stateItems_5E, stateItems_5F, stateItems_60, stateItems_61, stateItems_62, stateItems_63, stateItems_64, stateItems_65, stateItems_66, stateItems_67, stateItems_68, stateItems_69, stateItems_6A, stateItems_6B, stateItems_6C, stateItems_6D, stateItems_6E, stateItems_6F, stateItems_70, stateItems_71, stateItems_72, stateItems_73, stateItems_74, stateItems_75, stateItems_76, stateItems_77, stateItems_78, stateItems_79, stateItems_7A, stateItems_7B, stateItems_7C, stateItems_7D, stateItems_7E, stateItems_7F, stateItems_80, stateItems_81, stateItems_82, stateItems_83, stateItems_84, stateItems_85, stateItems_86, stateItems_87, stateItems_88, stateItems_89, stateItems_8A, stateItems_8B, stateItems_8C, stateItems_8D, stateItems_8E, stateItems_8F, stateItems_90, stateItems_91, stateItems_92, stateItems_93, stateItems_94, stateItems_95, stateItems_96, stateItems_97, stateItems_98, stateItems_99, stateItems_9A, stateItems_9B, stateItems_9C, stateItems_9D, stateItems_9E, stateItems_9F, stateItems_A0, stateItems_A1, stateItems_A2, stateItems_A3, stateItems_A4, stateItems_A5, stateItems_A6, stateItems_A7, stateItems_A8, stateItems_A9, stateItems_AA, stateItems_AB, stateItems_AC, stateItems_AD, stateItems_AE, stateItems_AF, stateItems_B0, stateItems_B1, stateItems_B2, stateItems_B3, stateItems_B4, stateItems_B5, stateItems_B6, stateItems_B7, stateItems_B8, stateItems_B9, stateItems_BA, stateItems_BB, stateItems_BC, stateItems_BD, stateItems_BE, stateItems_BF, stateItems_C0, stateItems_C1, stateItems_C2, stateItems_C3, stateItems_C4, stateItems_C5, stateItems_C6, stateItems_C7, stateItems_C8, stateItems_C9, stateItems_CA, stateItems_CB, stateItems_CC, stateItems_CD, stateItems_CE, stateItems_CF, stateItems_D0, stateItems_D1, stateItems_D2, stateItems_D3, stateItems_D4, stateItems_D5, stateItems_D6, stateItems_D7, stateItems_D8, stateItems_D9, stateItems_DA, stateItems_DB, stateItems_DC, stateItems_DD, stateItems_DE, stateItems_DF, stateItems_E0, stateItems_E1, stateItems_E2, stateItems_E3, stateItems_E4, stateItems_E5, stateItems_E6, stateItems_E7, stateItems_E8, stateItems_E9, stateItems_EA, stateItems_EB, stateItems_EC, stateItems_ED, stateItems_EE, stateItems_EF, stateItems_F0, stateItems_F1, stateItems_F2, stateItems_F3, stateItems_F4, stateItems_F5, stateItems_F6, stateItems_F7, stateItems_F8, stateItems_F9, stateItems_FA, stateItems_FB, stateItems_FC, stateItems_FD, stateItems_FE, stateItems_FF, stateItems_100, stateItems_101, stateItems_102, stateItems_103, stateItems_104, stateItems_105, stateItems_106, stateItems_107, stateItems_108, stateItems_109, stateItems_10A, stateItems_10B, stateItems_10C, stateItems_10D, stateItems_10E, stateItems_10F, stateItems_110, stateItems_111, stateItems_112, stateItems_113, stateItems_114, stateItems_115, stateItems_116, stateItems_117, stateItems_118, stateItems_119, stateItems_11A, stateItems_11B, stateItems_11C, stateItems_11D, stateItems_11E, stateItems_11F, stateItems_120, stateItems_121, stateItems_122, stateItems_123, stateItems_124, stateItems_125, stateItems_126, stateItems_127, stateItems_128, stateItems_129, stateItems_12A, stateItems_12B, stateItems_12C, stateItems_12D, stateItems_12E, stateItems_12F, stateItems_130, stateItems_131, stateItems_132, stateItems_133, stateItems_134, stateItems_135 };
        private static ushort[][][] staticStateShiftsOnTerminal = { stateShiftsOnTerminal_0, stateShiftsOnTerminal_1, stateShiftsOnTerminal_2, stateShiftsOnTerminal_3, stateShiftsOnTerminal_4, stateShiftsOnTerminal_5, stateShiftsOnTerminal_6, stateShiftsOnTerminal_7, stateShiftsOnTerminal_8, stateShiftsOnTerminal_9, stateShiftsOnTerminal_A, stateShiftsOnTerminal_B, stateShiftsOnTerminal_C, stateShiftsOnTerminal_D, stateShiftsOnTerminal_E, stateShiftsOnTerminal_F, stateShiftsOnTerminal_10, stateShiftsOnTerminal_11, stateShiftsOnTerminal_12, stateShiftsOnTerminal_13, stateShiftsOnTerminal_14, stateShiftsOnTerminal_15, stateShiftsOnTerminal_16, stateShiftsOnTerminal_17, stateShiftsOnTerminal_18, stateShiftsOnTerminal_19, stateShiftsOnTerminal_1A, stateShiftsOnTerminal_1B, stateShiftsOnTerminal_1C, stateShiftsOnTerminal_1D, stateShiftsOnTerminal_1E, stateShiftsOnTerminal_1F, stateShiftsOnTerminal_20, stateShiftsOnTerminal_21, stateShiftsOnTerminal_22, stateShiftsOnTerminal_23, stateShiftsOnTerminal_24, stateShiftsOnTerminal_25, stateShiftsOnTerminal_26, stateShiftsOnTerminal_27, stateShiftsOnTerminal_28, stateShiftsOnTerminal_29, stateShiftsOnTerminal_2A, stateShiftsOnTerminal_2B, stateShiftsOnTerminal_2C, stateShiftsOnTerminal_2D, stateShiftsOnTerminal_2E, stateShiftsOnTerminal_2F, stateShiftsOnTerminal_30, stateShiftsOnTerminal_31, stateShiftsOnTerminal_32, stateShiftsOnTerminal_33, stateShiftsOnTerminal_34, stateShiftsOnTerminal_35, stateShiftsOnTerminal_36, stateShiftsOnTerminal_37, stateShiftsOnTerminal_38, stateShiftsOnTerminal_39, stateShiftsOnTerminal_3A, stateShiftsOnTerminal_3B, stateShiftsOnTerminal_3C, stateShiftsOnTerminal_3D, stateShiftsOnTerminal_3E, stateShiftsOnTerminal_3F, stateShiftsOnTerminal_40, stateShiftsOnTerminal_41, stateShiftsOnTerminal_42, stateShiftsOnTerminal_43, stateShiftsOnTerminal_44, stateShiftsOnTerminal_45, stateShiftsOnTerminal_46, stateShiftsOnTerminal_47, stateShiftsOnTerminal_48, stateShiftsOnTerminal_49, stateShiftsOnTerminal_4A, stateShiftsOnTerminal_4B, stateShiftsOnTerminal_4C, stateShiftsOnTerminal_4D, stateShiftsOnTerminal_4E, stateShiftsOnTerminal_4F, stateShiftsOnTerminal_50, stateShiftsOnTerminal_51, stateShiftsOnTerminal_52, stateShiftsOnTerminal_53, stateShiftsOnTerminal_54, stateShiftsOnTerminal_55, stateShiftsOnTerminal_56, stateShiftsOnTerminal_57, stateShiftsOnTerminal_58, stateShiftsOnTerminal_59, stateShiftsOnTerminal_5A, stateShiftsOnTerminal_5B, stateShiftsOnTerminal_5C, stateShiftsOnTerminal_5D, stateShiftsOnTerminal_5E, stateShiftsOnTerminal_5F, stateShiftsOnTerminal_60, stateShiftsOnTerminal_61, stateShiftsOnTerminal_62, stateShiftsOnTerminal_63, stateShiftsOnTerminal_64, stateShiftsOnTerminal_65, stateShiftsOnTerminal_66, stateShiftsOnTerminal_67, stateShiftsOnTerminal_68, stateShiftsOnTerminal_69, stateShiftsOnTerminal_6A, stateShiftsOnTerminal_6B, stateShiftsOnTerminal_6C, stateShiftsOnTerminal_6D, stateShiftsOnTerminal_6E, stateShiftsOnTerminal_6F, stateShiftsOnTerminal_70, stateShiftsOnTerminal_71, stateShiftsOnTerminal_72, stateShiftsOnTerminal_73, stateShiftsOnTerminal_74, stateShiftsOnTerminal_75, stateShiftsOnTerminal_76, stateShiftsOnTerminal_77, stateShiftsOnTerminal_78, stateShiftsOnTerminal_79, stateShiftsOnTerminal_7A, stateShiftsOnTerminal_7B, stateShiftsOnTerminal_7C, stateShiftsOnTerminal_7D, stateShiftsOnTerminal_7E, stateShiftsOnTerminal_7F, stateShiftsOnTerminal_80, stateShiftsOnTerminal_81, stateShiftsOnTerminal_82, stateShiftsOnTerminal_83, stateShiftsOnTerminal_84, stateShiftsOnTerminal_85, stateShiftsOnTerminal_86, stateShiftsOnTerminal_87, stateShiftsOnTerminal_88, stateShiftsOnTerminal_89, stateShiftsOnTerminal_8A, stateShiftsOnTerminal_8B, stateShiftsOnTerminal_8C, stateShiftsOnTerminal_8D, stateShiftsOnTerminal_8E, stateShiftsOnTerminal_8F, stateShiftsOnTerminal_90, stateShiftsOnTerminal_91, stateShiftsOnTerminal_92, stateShiftsOnTerminal_93, stateShiftsOnTerminal_94, stateShiftsOnTerminal_95, stateShiftsOnTerminal_96, stateShiftsOnTerminal_97, stateShiftsOnTerminal_98, stateShiftsOnTerminal_99, stateShiftsOnTerminal_9A, stateShiftsOnTerminal_9B, stateShiftsOnTerminal_9C, stateShiftsOnTerminal_9D, stateShiftsOnTerminal_9E, stateShiftsOnTerminal_9F, stateShiftsOnTerminal_A0, stateShiftsOnTerminal_A1, stateShiftsOnTerminal_A2, stateShiftsOnTerminal_A3, stateShiftsOnTerminal_A4, stateShiftsOnTerminal_A5, stateShiftsOnTerminal_A6, stateShiftsOnTerminal_A7, stateShiftsOnTerminal_A8, stateShiftsOnTerminal_A9, stateShiftsOnTerminal_AA, stateShiftsOnTerminal_AB, stateShiftsOnTerminal_AC, stateShiftsOnTerminal_AD, stateShiftsOnTerminal_AE, stateShiftsOnTerminal_AF, stateShiftsOnTerminal_B0, stateShiftsOnTerminal_B1, stateShiftsOnTerminal_B2, stateShiftsOnTerminal_B3, stateShiftsOnTerminal_B4, stateShiftsOnTerminal_B5, stateShiftsOnTerminal_B6, stateShiftsOnTerminal_B7, stateShiftsOnTerminal_B8, stateShiftsOnTerminal_B9, stateShiftsOnTerminal_BA, stateShiftsOnTerminal_BB, stateShiftsOnTerminal_BC, stateShiftsOnTerminal_BD, stateShiftsOnTerminal_BE, stateShiftsOnTerminal_BF, stateShiftsOnTerminal_C0, stateShiftsOnTerminal_C1, stateShiftsOnTerminal_C2, stateShiftsOnTerminal_C3, stateShiftsOnTerminal_C4, stateShiftsOnTerminal_C5, stateShiftsOnTerminal_C6, stateShiftsOnTerminal_C7, stateShiftsOnTerminal_C8, stateShiftsOnTerminal_C9, stateShiftsOnTerminal_CA, stateShiftsOnTerminal_CB, stateShiftsOnTerminal_CC, stateShiftsOnTerminal_CD, stateShiftsOnTerminal_CE, stateShiftsOnTerminal_CF, stateShiftsOnTerminal_D0, stateShiftsOnTerminal_D1, stateShiftsOnTerminal_D2, stateShiftsOnTerminal_D3, stateShiftsOnTerminal_D4, stateShiftsOnTerminal_D5, stateShiftsOnTerminal_D6, stateShiftsOnTerminal_D7, stateShiftsOnTerminal_D8, stateShiftsOnTerminal_D9, stateShiftsOnTerminal_DA, stateShiftsOnTerminal_DB, stateShiftsOnTerminal_DC, stateShiftsOnTerminal_DD, stateShiftsOnTerminal_DE, stateShiftsOnTerminal_DF, stateShiftsOnTerminal_E0, stateShiftsOnTerminal_E1, stateShiftsOnTerminal_E2, stateShiftsOnTerminal_E3, stateShiftsOnTerminal_E4, stateShiftsOnTerminal_E5, stateShiftsOnTerminal_E6, stateShiftsOnTerminal_E7, stateShiftsOnTerminal_E8, stateShiftsOnTerminal_E9, stateShiftsOnTerminal_EA, stateShiftsOnTerminal_EB, stateShiftsOnTerminal_EC, stateShiftsOnTerminal_ED, stateShiftsOnTerminal_EE, stateShiftsOnTerminal_EF, stateShiftsOnTerminal_F0, stateShiftsOnTerminal_F1, stateShiftsOnTerminal_F2, stateShiftsOnTerminal_F3, stateShiftsOnTerminal_F4, stateShiftsOnTerminal_F5, stateShiftsOnTerminal_F6, stateShiftsOnTerminal_F7, stateShiftsOnTerminal_F8, stateShiftsOnTerminal_F9, stateShiftsOnTerminal_FA, stateShiftsOnTerminal_FB, stateShiftsOnTerminal_FC, stateShiftsOnTerminal_FD, stateShiftsOnTerminal_FE, stateShiftsOnTerminal_FF, stateShiftsOnTerminal_100, stateShiftsOnTerminal_101, stateShiftsOnTerminal_102, stateShiftsOnTerminal_103, stateShiftsOnTerminal_104, stateShiftsOnTerminal_105, stateShiftsOnTerminal_106, stateShiftsOnTerminal_107, stateShiftsOnTerminal_108, stateShiftsOnTerminal_109, stateShiftsOnTerminal_10A, stateShiftsOnTerminal_10B, stateShiftsOnTerminal_10C, stateShiftsOnTerminal_10D, stateShiftsOnTerminal_10E, stateShiftsOnTerminal_10F, stateShiftsOnTerminal_110, stateShiftsOnTerminal_111, stateShiftsOnTerminal_112, stateShiftsOnTerminal_113, stateShiftsOnTerminal_114, stateShiftsOnTerminal_115, stateShiftsOnTerminal_116, stateShiftsOnTerminal_117, stateShiftsOnTerminal_118, stateShiftsOnTerminal_119, stateShiftsOnTerminal_11A, stateShiftsOnTerminal_11B, stateShiftsOnTerminal_11C, stateShiftsOnTerminal_11D, stateShiftsOnTerminal_11E, stateShiftsOnTerminal_11F, stateShiftsOnTerminal_120, stateShiftsOnTerminal_121, stateShiftsOnTerminal_122, stateShiftsOnTerminal_123, stateShiftsOnTerminal_124, stateShiftsOnTerminal_125, stateShiftsOnTerminal_126, stateShiftsOnTerminal_127, stateShiftsOnTerminal_128, stateShiftsOnTerminal_129, stateShiftsOnTerminal_12A, stateShiftsOnTerminal_12B, stateShiftsOnTerminal_12C, stateShiftsOnTerminal_12D, stateShiftsOnTerminal_12E, stateShiftsOnTerminal_12F, stateShiftsOnTerminal_130, stateShiftsOnTerminal_131, stateShiftsOnTerminal_132, stateShiftsOnTerminal_133, stateShiftsOnTerminal_134, stateShiftsOnTerminal_135 };
        private static ushort[][][] staticStateShiftsOnVariable = { stateShiftsOnVariable_0, stateShiftsOnVariable_1, stateShiftsOnVariable_2, stateShiftsOnVariable_3, stateShiftsOnVariable_4, stateShiftsOnVariable_5, stateShiftsOnVariable_6, stateShiftsOnVariable_7, stateShiftsOnVariable_8, stateShiftsOnVariable_9, stateShiftsOnVariable_A, stateShiftsOnVariable_B, stateShiftsOnVariable_C, stateShiftsOnVariable_D, stateShiftsOnVariable_E, stateShiftsOnVariable_F, stateShiftsOnVariable_10, stateShiftsOnVariable_11, stateShiftsOnVariable_12, stateShiftsOnVariable_13, stateShiftsOnVariable_14, stateShiftsOnVariable_15, stateShiftsOnVariable_16, stateShiftsOnVariable_17, stateShiftsOnVariable_18, stateShiftsOnVariable_19, stateShiftsOnVariable_1A, stateShiftsOnVariable_1B, stateShiftsOnVariable_1C, stateShiftsOnVariable_1D, stateShiftsOnVariable_1E, stateShiftsOnVariable_1F, stateShiftsOnVariable_20, stateShiftsOnVariable_21, stateShiftsOnVariable_22, stateShiftsOnVariable_23, stateShiftsOnVariable_24, stateShiftsOnVariable_25, stateShiftsOnVariable_26, stateShiftsOnVariable_27, stateShiftsOnVariable_28, stateShiftsOnVariable_29, stateShiftsOnVariable_2A, stateShiftsOnVariable_2B, stateShiftsOnVariable_2C, stateShiftsOnVariable_2D, stateShiftsOnVariable_2E, stateShiftsOnVariable_2F, stateShiftsOnVariable_30, stateShiftsOnVariable_31, stateShiftsOnVariable_32, stateShiftsOnVariable_33, stateShiftsOnVariable_34, stateShiftsOnVariable_35, stateShiftsOnVariable_36, stateShiftsOnVariable_37, stateShiftsOnVariable_38, stateShiftsOnVariable_39, stateShiftsOnVariable_3A, stateShiftsOnVariable_3B, stateShiftsOnVariable_3C, stateShiftsOnVariable_3D, stateShiftsOnVariable_3E, stateShiftsOnVariable_3F, stateShiftsOnVariable_40, stateShiftsOnVariable_41, stateShiftsOnVariable_42, stateShiftsOnVariable_43, stateShiftsOnVariable_44, stateShiftsOnVariable_45, stateShiftsOnVariable_46, stateShiftsOnVariable_47, stateShiftsOnVariable_48, stateShiftsOnVariable_49, stateShiftsOnVariable_4A, stateShiftsOnVariable_4B, stateShiftsOnVariable_4C, stateShiftsOnVariable_4D, stateShiftsOnVariable_4E, stateShiftsOnVariable_4F, stateShiftsOnVariable_50, stateShiftsOnVariable_51, stateShiftsOnVariable_52, stateShiftsOnVariable_53, stateShiftsOnVariable_54, stateShiftsOnVariable_55, stateShiftsOnVariable_56, stateShiftsOnVariable_57, stateShiftsOnVariable_58, stateShiftsOnVariable_59, stateShiftsOnVariable_5A, stateShiftsOnVariable_5B, stateShiftsOnVariable_5C, stateShiftsOnVariable_5D, stateShiftsOnVariable_5E, stateShiftsOnVariable_5F, stateShiftsOnVariable_60, stateShiftsOnVariable_61, stateShiftsOnVariable_62, stateShiftsOnVariable_63, stateShiftsOnVariable_64, stateShiftsOnVariable_65, stateShiftsOnVariable_66, stateShiftsOnVariable_67, stateShiftsOnVariable_68, stateShiftsOnVariable_69, stateShiftsOnVariable_6A, stateShiftsOnVariable_6B, stateShiftsOnVariable_6C, stateShiftsOnVariable_6D, stateShiftsOnVariable_6E, stateShiftsOnVariable_6F, stateShiftsOnVariable_70, stateShiftsOnVariable_71, stateShiftsOnVariable_72, stateShiftsOnVariable_73, stateShiftsOnVariable_74, stateShiftsOnVariable_75, stateShiftsOnVariable_76, stateShiftsOnVariable_77, stateShiftsOnVariable_78, stateShiftsOnVariable_79, stateShiftsOnVariable_7A, stateShiftsOnVariable_7B, stateShiftsOnVariable_7C, stateShiftsOnVariable_7D, stateShiftsOnVariable_7E, stateShiftsOnVariable_7F, stateShiftsOnVariable_80, stateShiftsOnVariable_81, stateShiftsOnVariable_82, stateShiftsOnVariable_83, stateShiftsOnVariable_84, stateShiftsOnVariable_85, stateShiftsOnVariable_86, stateShiftsOnVariable_87, stateShiftsOnVariable_88, stateShiftsOnVariable_89, stateShiftsOnVariable_8A, stateShiftsOnVariable_8B, stateShiftsOnVariable_8C, stateShiftsOnVariable_8D, stateShiftsOnVariable_8E, stateShiftsOnVariable_8F, stateShiftsOnVariable_90, stateShiftsOnVariable_91, stateShiftsOnVariable_92, stateShiftsOnVariable_93, stateShiftsOnVariable_94, stateShiftsOnVariable_95, stateShiftsOnVariable_96, stateShiftsOnVariable_97, stateShiftsOnVariable_98, stateShiftsOnVariable_99, stateShiftsOnVariable_9A, stateShiftsOnVariable_9B, stateShiftsOnVariable_9C, stateShiftsOnVariable_9D, stateShiftsOnVariable_9E, stateShiftsOnVariable_9F, stateShiftsOnVariable_A0, stateShiftsOnVariable_A1, stateShiftsOnVariable_A2, stateShiftsOnVariable_A3, stateShiftsOnVariable_A4, stateShiftsOnVariable_A5, stateShiftsOnVariable_A6, stateShiftsOnVariable_A7, stateShiftsOnVariable_A8, stateShiftsOnVariable_A9, stateShiftsOnVariable_AA, stateShiftsOnVariable_AB, stateShiftsOnVariable_AC, stateShiftsOnVariable_AD, stateShiftsOnVariable_AE, stateShiftsOnVariable_AF, stateShiftsOnVariable_B0, stateShiftsOnVariable_B1, stateShiftsOnVariable_B2, stateShiftsOnVariable_B3, stateShiftsOnVariable_B4, stateShiftsOnVariable_B5, stateShiftsOnVariable_B6, stateShiftsOnVariable_B7, stateShiftsOnVariable_B8, stateShiftsOnVariable_B9, stateShiftsOnVariable_BA, stateShiftsOnVariable_BB, stateShiftsOnVariable_BC, stateShiftsOnVariable_BD, stateShiftsOnVariable_BE, stateShiftsOnVariable_BF, stateShiftsOnVariable_C0, stateShiftsOnVariable_C1, stateShiftsOnVariable_C2, stateShiftsOnVariable_C3, stateShiftsOnVariable_C4, stateShiftsOnVariable_C5, stateShiftsOnVariable_C6, stateShiftsOnVariable_C7, stateShiftsOnVariable_C8, stateShiftsOnVariable_C9, stateShiftsOnVariable_CA, stateShiftsOnVariable_CB, stateShiftsOnVariable_CC, stateShiftsOnVariable_CD, stateShiftsOnVariable_CE, stateShiftsOnVariable_CF, stateShiftsOnVariable_D0, stateShiftsOnVariable_D1, stateShiftsOnVariable_D2, stateShiftsOnVariable_D3, stateShiftsOnVariable_D4, stateShiftsOnVariable_D5, stateShiftsOnVariable_D6, stateShiftsOnVariable_D7, stateShiftsOnVariable_D8, stateShiftsOnVariable_D9, stateShiftsOnVariable_DA, stateShiftsOnVariable_DB, stateShiftsOnVariable_DC, stateShiftsOnVariable_DD, stateShiftsOnVariable_DE, stateShiftsOnVariable_DF, stateShiftsOnVariable_E0, stateShiftsOnVariable_E1, stateShiftsOnVariable_E2, stateShiftsOnVariable_E3, stateShiftsOnVariable_E4, stateShiftsOnVariable_E5, stateShiftsOnVariable_E6, stateShiftsOnVariable_E7, stateShiftsOnVariable_E8, stateShiftsOnVariable_E9, stateShiftsOnVariable_EA, stateShiftsOnVariable_EB, stateShiftsOnVariable_EC, stateShiftsOnVariable_ED, stateShiftsOnVariable_EE, stateShiftsOnVariable_EF, stateShiftsOnVariable_F0, stateShiftsOnVariable_F1, stateShiftsOnVariable_F2, stateShiftsOnVariable_F3, stateShiftsOnVariable_F4, stateShiftsOnVariable_F5, stateShiftsOnVariable_F6, stateShiftsOnVariable_F7, stateShiftsOnVariable_F8, stateShiftsOnVariable_F9, stateShiftsOnVariable_FA, stateShiftsOnVariable_FB, stateShiftsOnVariable_FC, stateShiftsOnVariable_FD, stateShiftsOnVariable_FE, stateShiftsOnVariable_FF, stateShiftsOnVariable_100, stateShiftsOnVariable_101, stateShiftsOnVariable_102, stateShiftsOnVariable_103, stateShiftsOnVariable_104, stateShiftsOnVariable_105, stateShiftsOnVariable_106, stateShiftsOnVariable_107, stateShiftsOnVariable_108, stateShiftsOnVariable_109, stateShiftsOnVariable_10A, stateShiftsOnVariable_10B, stateShiftsOnVariable_10C, stateShiftsOnVariable_10D, stateShiftsOnVariable_10E, stateShiftsOnVariable_10F, stateShiftsOnVariable_110, stateShiftsOnVariable_111, stateShiftsOnVariable_112, stateShiftsOnVariable_113, stateShiftsOnVariable_114, stateShiftsOnVariable_115, stateShiftsOnVariable_116, stateShiftsOnVariable_117, stateShiftsOnVariable_118, stateShiftsOnVariable_119, stateShiftsOnVariable_11A, stateShiftsOnVariable_11B, stateShiftsOnVariable_11C, stateShiftsOnVariable_11D, stateShiftsOnVariable_11E, stateShiftsOnVariable_11F, stateShiftsOnVariable_120, stateShiftsOnVariable_121, stateShiftsOnVariable_122, stateShiftsOnVariable_123, stateShiftsOnVariable_124, stateShiftsOnVariable_125, stateShiftsOnVariable_126, stateShiftsOnVariable_127, stateShiftsOnVariable_128, stateShiftsOnVariable_129, stateShiftsOnVariable_12A, stateShiftsOnVariable_12B, stateShiftsOnVariable_12C, stateShiftsOnVariable_12D, stateShiftsOnVariable_12E, stateShiftsOnVariable_12F, stateShiftsOnVariable_130, stateShiftsOnVariable_131, stateShiftsOnVariable_132, stateShiftsOnVariable_133, stateShiftsOnVariable_134, stateShiftsOnVariable_135 };
        private static ushort[][][] staticStateReducsOnTerminal = { stateReducsOnTerminal_0, stateReducsOnTerminal_1, stateReducsOnTerminal_2, stateReducsOnTerminal_3, stateReducsOnTerminal_4, stateReducsOnTerminal_5, stateReducsOnTerminal_6, stateReducsOnTerminal_7, stateReducsOnTerminal_8, stateReducsOnTerminal_9, stateReducsOnTerminal_A, stateReducsOnTerminal_B, stateReducsOnTerminal_C, stateReducsOnTerminal_D, stateReducsOnTerminal_E, stateReducsOnTerminal_F, stateReducsOnTerminal_10, stateReducsOnTerminal_11, stateReducsOnTerminal_12, stateReducsOnTerminal_13, stateReducsOnTerminal_14, stateReducsOnTerminal_15, stateReducsOnTerminal_16, stateReducsOnTerminal_17, stateReducsOnTerminal_18, stateReducsOnTerminal_19, stateReducsOnTerminal_1A, stateReducsOnTerminal_1B, stateReducsOnTerminal_1C, stateReducsOnTerminal_1D, stateReducsOnTerminal_1E, stateReducsOnTerminal_1F, stateReducsOnTerminal_20, stateReducsOnTerminal_21, stateReducsOnTerminal_22, stateReducsOnTerminal_23, stateReducsOnTerminal_24, stateReducsOnTerminal_25, stateReducsOnTerminal_26, stateReducsOnTerminal_27, stateReducsOnTerminal_28, stateReducsOnTerminal_29, stateReducsOnTerminal_2A, stateReducsOnTerminal_2B, stateReducsOnTerminal_2C, stateReducsOnTerminal_2D, stateReducsOnTerminal_2E, stateReducsOnTerminal_2F, stateReducsOnTerminal_30, stateReducsOnTerminal_31, stateReducsOnTerminal_32, stateReducsOnTerminal_33, stateReducsOnTerminal_34, stateReducsOnTerminal_35, stateReducsOnTerminal_36, stateReducsOnTerminal_37, stateReducsOnTerminal_38, stateReducsOnTerminal_39, stateReducsOnTerminal_3A, stateReducsOnTerminal_3B, stateReducsOnTerminal_3C, stateReducsOnTerminal_3D, stateReducsOnTerminal_3E, stateReducsOnTerminal_3F, stateReducsOnTerminal_40, stateReducsOnTerminal_41, stateReducsOnTerminal_42, stateReducsOnTerminal_43, stateReducsOnTerminal_44, stateReducsOnTerminal_45, stateReducsOnTerminal_46, stateReducsOnTerminal_47, stateReducsOnTerminal_48, stateReducsOnTerminal_49, stateReducsOnTerminal_4A, stateReducsOnTerminal_4B, stateReducsOnTerminal_4C, stateReducsOnTerminal_4D, stateReducsOnTerminal_4E, stateReducsOnTerminal_4F, stateReducsOnTerminal_50, stateReducsOnTerminal_51, stateReducsOnTerminal_52, stateReducsOnTerminal_53, stateReducsOnTerminal_54, stateReducsOnTerminal_55, stateReducsOnTerminal_56, stateReducsOnTerminal_57, stateReducsOnTerminal_58, stateReducsOnTerminal_59, stateReducsOnTerminal_5A, stateReducsOnTerminal_5B, stateReducsOnTerminal_5C, stateReducsOnTerminal_5D, stateReducsOnTerminal_5E, stateReducsOnTerminal_5F, stateReducsOnTerminal_60, stateReducsOnTerminal_61, stateReducsOnTerminal_62, stateReducsOnTerminal_63, stateReducsOnTerminal_64, stateReducsOnTerminal_65, stateReducsOnTerminal_66, stateReducsOnTerminal_67, stateReducsOnTerminal_68, stateReducsOnTerminal_69, stateReducsOnTerminal_6A, stateReducsOnTerminal_6B, stateReducsOnTerminal_6C, stateReducsOnTerminal_6D, stateReducsOnTerminal_6E, stateReducsOnTerminal_6F, stateReducsOnTerminal_70, stateReducsOnTerminal_71, stateReducsOnTerminal_72, stateReducsOnTerminal_73, stateReducsOnTerminal_74, stateReducsOnTerminal_75, stateReducsOnTerminal_76, stateReducsOnTerminal_77, stateReducsOnTerminal_78, stateReducsOnTerminal_79, stateReducsOnTerminal_7A, stateReducsOnTerminal_7B, stateReducsOnTerminal_7C, stateReducsOnTerminal_7D, stateReducsOnTerminal_7E, stateReducsOnTerminal_7F, stateReducsOnTerminal_80, stateReducsOnTerminal_81, stateReducsOnTerminal_82, stateReducsOnTerminal_83, stateReducsOnTerminal_84, stateReducsOnTerminal_85, stateReducsOnTerminal_86, stateReducsOnTerminal_87, stateReducsOnTerminal_88, stateReducsOnTerminal_89, stateReducsOnTerminal_8A, stateReducsOnTerminal_8B, stateReducsOnTerminal_8C, stateReducsOnTerminal_8D, stateReducsOnTerminal_8E, stateReducsOnTerminal_8F, stateReducsOnTerminal_90, stateReducsOnTerminal_91, stateReducsOnTerminal_92, stateReducsOnTerminal_93, stateReducsOnTerminal_94, stateReducsOnTerminal_95, stateReducsOnTerminal_96, stateReducsOnTerminal_97, stateReducsOnTerminal_98, stateReducsOnTerminal_99, stateReducsOnTerminal_9A, stateReducsOnTerminal_9B, stateReducsOnTerminal_9C, stateReducsOnTerminal_9D, stateReducsOnTerminal_9E, stateReducsOnTerminal_9F, stateReducsOnTerminal_A0, stateReducsOnTerminal_A1, stateReducsOnTerminal_A2, stateReducsOnTerminal_A3, stateReducsOnTerminal_A4, stateReducsOnTerminal_A5, stateReducsOnTerminal_A6, stateReducsOnTerminal_A7, stateReducsOnTerminal_A8, stateReducsOnTerminal_A9, stateReducsOnTerminal_AA, stateReducsOnTerminal_AB, stateReducsOnTerminal_AC, stateReducsOnTerminal_AD, stateReducsOnTerminal_AE, stateReducsOnTerminal_AF, stateReducsOnTerminal_B0, stateReducsOnTerminal_B1, stateReducsOnTerminal_B2, stateReducsOnTerminal_B3, stateReducsOnTerminal_B4, stateReducsOnTerminal_B5, stateReducsOnTerminal_B6, stateReducsOnTerminal_B7, stateReducsOnTerminal_B8, stateReducsOnTerminal_B9, stateReducsOnTerminal_BA, stateReducsOnTerminal_BB, stateReducsOnTerminal_BC, stateReducsOnTerminal_BD, stateReducsOnTerminal_BE, stateReducsOnTerminal_BF, stateReducsOnTerminal_C0, stateReducsOnTerminal_C1, stateReducsOnTerminal_C2, stateReducsOnTerminal_C3, stateReducsOnTerminal_C4, stateReducsOnTerminal_C5, stateReducsOnTerminal_C6, stateReducsOnTerminal_C7, stateReducsOnTerminal_C8, stateReducsOnTerminal_C9, stateReducsOnTerminal_CA, stateReducsOnTerminal_CB, stateReducsOnTerminal_CC, stateReducsOnTerminal_CD, stateReducsOnTerminal_CE, stateReducsOnTerminal_CF, stateReducsOnTerminal_D0, stateReducsOnTerminal_D1, stateReducsOnTerminal_D2, stateReducsOnTerminal_D3, stateReducsOnTerminal_D4, stateReducsOnTerminal_D5, stateReducsOnTerminal_D6, stateReducsOnTerminal_D7, stateReducsOnTerminal_D8, stateReducsOnTerminal_D9, stateReducsOnTerminal_DA, stateReducsOnTerminal_DB, stateReducsOnTerminal_DC, stateReducsOnTerminal_DD, stateReducsOnTerminal_DE, stateReducsOnTerminal_DF, stateReducsOnTerminal_E0, stateReducsOnTerminal_E1, stateReducsOnTerminal_E2, stateReducsOnTerminal_E3, stateReducsOnTerminal_E4, stateReducsOnTerminal_E5, stateReducsOnTerminal_E6, stateReducsOnTerminal_E7, stateReducsOnTerminal_E8, stateReducsOnTerminal_E9, stateReducsOnTerminal_EA, stateReducsOnTerminal_EB, stateReducsOnTerminal_EC, stateReducsOnTerminal_ED, stateReducsOnTerminal_EE, stateReducsOnTerminal_EF, stateReducsOnTerminal_F0, stateReducsOnTerminal_F1, stateReducsOnTerminal_F2, stateReducsOnTerminal_F3, stateReducsOnTerminal_F4, stateReducsOnTerminal_F5, stateReducsOnTerminal_F6, stateReducsOnTerminal_F7, stateReducsOnTerminal_F8, stateReducsOnTerminal_F9, stateReducsOnTerminal_FA, stateReducsOnTerminal_FB, stateReducsOnTerminal_FC, stateReducsOnTerminal_FD, stateReducsOnTerminal_FE, stateReducsOnTerminal_FF, stateReducsOnTerminal_100, stateReducsOnTerminal_101, stateReducsOnTerminal_102, stateReducsOnTerminal_103, stateReducsOnTerminal_104, stateReducsOnTerminal_105, stateReducsOnTerminal_106, stateReducsOnTerminal_107, stateReducsOnTerminal_108, stateReducsOnTerminal_109, stateReducsOnTerminal_10A, stateReducsOnTerminal_10B, stateReducsOnTerminal_10C, stateReducsOnTerminal_10D, stateReducsOnTerminal_10E, stateReducsOnTerminal_10F, stateReducsOnTerminal_110, stateReducsOnTerminal_111, stateReducsOnTerminal_112, stateReducsOnTerminal_113, stateReducsOnTerminal_114, stateReducsOnTerminal_115, stateReducsOnTerminal_116, stateReducsOnTerminal_117, stateReducsOnTerminal_118, stateReducsOnTerminal_119, stateReducsOnTerminal_11A, stateReducsOnTerminal_11B, stateReducsOnTerminal_11C, stateReducsOnTerminal_11D, stateReducsOnTerminal_11E, stateReducsOnTerminal_11F, stateReducsOnTerminal_120, stateReducsOnTerminal_121, stateReducsOnTerminal_122, stateReducsOnTerminal_123, stateReducsOnTerminal_124, stateReducsOnTerminal_125, stateReducsOnTerminal_126, stateReducsOnTerminal_127, stateReducsOnTerminal_128, stateReducsOnTerminal_129, stateReducsOnTerminal_12A, stateReducsOnTerminal_12B, stateReducsOnTerminal_12C, stateReducsOnTerminal_12D, stateReducsOnTerminal_12E, stateReducsOnTerminal_12F, stateReducsOnTerminal_130, stateReducsOnTerminal_131, stateReducsOnTerminal_132, stateReducsOnTerminal_133, stateReducsOnTerminal_134, stateReducsOnTerminal_135 };
        protected override void setup()
        {
            rules = staticRules;
            rulesHeadID = staticRulesHeadID;
            rulesHeadName = staticRulesHeadName;
            rulesParserLength = staticRulesParserLength;
            stateExpectedIDs = staticStateExpectedIDs;
            stateExpectedNames = staticStateExpectedNames;
            stateItems = staticStateItems;
            stateShiftsOnTerminal = staticStateShiftsOnTerminal;
            stateShiftsOnVariable = staticStateShiftsOnVariable;
            stateReducsOnTerminal = staticStateReducsOnTerminal;
            errorSimulationLength = 3;
        }
        public FileCentralDogma_Parser(FileCentralDogma_Lexer lexer) : base (lexer) {}
    }
}
