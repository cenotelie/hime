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
        private static Rule[] staticRules = {
           new Rule(Production_13_0, new Hime.Redist.Parsers.SymbolVariable(0x13, "qualified_name"), 2)
           , new Rule(Production_14_0, new Hime.Redist.Parsers.SymbolVariable(0x14, "symbol_access_public"), 1)
           , new Rule(Production_15_0, new Hime.Redist.Parsers.SymbolVariable(0x15, "symbol_access_private"), 1)
           , new Rule(Production_16_0, new Hime.Redist.Parsers.SymbolVariable(0x16, "symbol_access_protected"), 1)
           , new Rule(Production_17_0, new Hime.Redist.Parsers.SymbolVariable(0x17, "symbol_access_internal"), 1)
           , new Rule(Production_18_0, new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"), 1)
           , new Rule(Production_18_1, new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"), 1)
           , new Rule(Production_18_2, new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"), 1)
           , new Rule(Production_18_3, new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"), 1)
           , new Rule(Production_18_4, new Hime.Redist.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"), 1)
           , new Rule(Production_19_0, new Hime.Redist.Parsers.SymbolVariable(0x19, "Namespace_content"), 1)
           , new Rule(Production_1A_0, new Hime.Redist.Parsers.SymbolVariable(0x1A, "Namespace"), 5)
           , new Rule(Production_1B_0, new Hime.Redist.Parsers.SymbolVariable(0x1B, "_m20"), 3)
           , new Rule(Production_1B_1, new Hime.Redist.Parsers.SymbolVariable(0x1B, "_m20"), 0)
           , new Rule(Production_1C_0, new Hime.Redist.Parsers.SymbolVariable(0x1C, "_m25"), 2)
           , new Rule(Production_1C_1, new Hime.Redist.Parsers.SymbolVariable(0x1C, "_m25"), 0)
           , new Rule(Production_57_0, new Hime.Redist.Parsers.SymbolVariable(0x57, "option"), 4)
           , new Rule(Production_58_0, new Hime.Redist.Parsers.SymbolVariable(0x58, "terminal_def_atom_any"), 1)
           , new Rule(Production_59_0, new Hime.Redist.Parsers.SymbolVariable(0x59, "terminal_def_atom_unicode"), 1)
           , new Rule(Production_59_1, new Hime.Redist.Parsers.SymbolVariable(0x59, "terminal_def_atom_unicode"), 1)
           , new Rule(Production_5A_0, new Hime.Redist.Parsers.SymbolVariable(0x5A, "terminal_def_atom_text"), 1)
           , new Rule(Production_5B_0, new Hime.Redist.Parsers.SymbolVariable(0x5B, "terminal_def_atom_set"), 1)
           , new Rule(Production_5C_0, new Hime.Redist.Parsers.SymbolVariable(0x5C, "terminal_def_atom_ublock"), 1)
           , new Rule(Production_5D_0, new Hime.Redist.Parsers.SymbolVariable(0x5D, "terminal_def_atom_ucat"), 1)
           , new Rule(Production_5E_0, new Hime.Redist.Parsers.SymbolVariable(0x5E, "terminal_def_atom_span"), 3)
           , new Rule(Production_5F_0, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_1, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_2, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_3, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_4, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_5, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_6, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_5F_7, new Hime.Redist.Parsers.SymbolVariable(0x5F, "terminal_def_atom"), 1)
           , new Rule(Production_60_0, new Hime.Redist.Parsers.SymbolVariable(0x60, "terminal_def_element"), 1)
           , new Rule(Production_60_1, new Hime.Redist.Parsers.SymbolVariable(0x60, "terminal_def_element"), 3)
           , new Rule(Production_61_0, new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"), 1)
           , new Rule(Production_61_1, new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"), 1)
           , new Rule(Production_61_2, new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"), 1)
           , new Rule(Production_61_3, new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"), 5)
           , new Rule(Production_61_4, new Hime.Redist.Parsers.SymbolVariable(0x61, "terminal_def_cardinalilty"), 3)
           , new Rule(Production_62_0, new Hime.Redist.Parsers.SymbolVariable(0x62, "terminal_def_repetition"), 2)
           , new Rule(Production_62_1, new Hime.Redist.Parsers.SymbolVariable(0x62, "terminal_def_repetition"), 1)
           , new Rule(Production_63_0, new Hime.Redist.Parsers.SymbolVariable(0x63, "terminal_def_fragment"), 2)
           , new Rule(Production_64_0, new Hime.Redist.Parsers.SymbolVariable(0x64, "terminal_def_restrict"), 2)
           , new Rule(Production_65_0, new Hime.Redist.Parsers.SymbolVariable(0x65, "terminal_definition"), 2)
           , new Rule(Production_66_0, new Hime.Redist.Parsers.SymbolVariable(0x66, "terminal_subgrammar"), 2)
           , new Rule(Production_66_1, new Hime.Redist.Parsers.SymbolVariable(0x66, "terminal_subgrammar"), 0)
           , new Rule(Production_67_0, new Hime.Redist.Parsers.SymbolVariable(0x67, "terminal"), 5)
           , new Rule(Production_68_0, new Hime.Redist.Parsers.SymbolVariable(0x68, "rule_sym_action"), 3)
           , new Rule(Production_69_0, new Hime.Redist.Parsers.SymbolVariable(0x69, "rule_sym_virtual"), 1)
           , new Rule(Production_6A_0, new Hime.Redist.Parsers.SymbolVariable(0x6A, "rule_sym_ref_simple"), 1)
           , new Rule(Production_6B_0, new Hime.Redist.Parsers.SymbolVariable(0x6B, "rule_template_params"), 4)
           , new Rule(Production_6C_0, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_1, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_2, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_3, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_4, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_5, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_6, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_7, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_8, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_9, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_A, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6C_B, new Hime.Redist.Parsers.SymbolVariable(0x6C, "grammar_bin_terminal"), 1)
           , new Rule(Production_6D_0, new Hime.Redist.Parsers.SymbolVariable(0x6D, "grammar_text_terminal"), 1)
           , new Rule(Production_6E_0, new Hime.Redist.Parsers.SymbolVariable(0x6E, "grammar_options"), 4)
           , new Rule(Production_6F_0, new Hime.Redist.Parsers.SymbolVariable(0x6F, "grammar_terminals"), 4)
           , new Rule(Production_70_0, new Hime.Redist.Parsers.SymbolVariable(0x70, "grammar_parency"), 3)
           , new Rule(Production_70_1, new Hime.Redist.Parsers.SymbolVariable(0x70, "grammar_parency"), 0)
           , new Rule(Production_71_0, new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"), 1)
           , new Rule(Production_71_1, new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"), 1)
           , new Rule(Production_71_2, new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"), 1)
           , new Rule(Production_71_3, new Hime.Redist.Parsers.SymbolVariable(0x71, "grammar_access"), 1)
           , new Rule(Production_72_0, new Hime.Redist.Parsers.SymbolVariable(0x72, "cf_grammar_text"), 10)
           , new Rule(Production_73_0, new Hime.Redist.Parsers.SymbolVariable(0x73, "cf_grammar_bin"), 9)
           , new Rule(Production_74_0, new Hime.Redist.Parsers.SymbolVariable(0x74, "_m89"), 2)
           , new Rule(Production_74_1, new Hime.Redist.Parsers.SymbolVariable(0x74, "_m89"), 0)
           , new Rule(Production_75_0, new Hime.Redist.Parsers.SymbolVariable(0x75, "_m91"), 3)
           , new Rule(Production_75_1, new Hime.Redist.Parsers.SymbolVariable(0x75, "_m91"), 0)
           , new Rule(Production_76_0, new Hime.Redist.Parsers.SymbolVariable(0x76, "_m93"), 3)
           , new Rule(Production_76_1, new Hime.Redist.Parsers.SymbolVariable(0x76, "_m93"), 0)
           , new Rule(Production_77_0, new Hime.Redist.Parsers.SymbolVariable(0x77, "_m101"), 3)
           , new Rule(Production_77_1, new Hime.Redist.Parsers.SymbolVariable(0x77, "_m101"), 0)
           , new Rule(Production_78_0, new Hime.Redist.Parsers.SymbolVariable(0x78, "_m105"), 2)
           , new Rule(Production_78_1, new Hime.Redist.Parsers.SymbolVariable(0x78, "_m105"), 0)
           , new Rule(Production_79_0, new Hime.Redist.Parsers.SymbolVariable(0x79, "_m109"), 2)
           , new Rule(Production_79_1, new Hime.Redist.Parsers.SymbolVariable(0x79, "_m109"), 0)
           , new Rule(Production_7A_0, new Hime.Redist.Parsers.SymbolVariable(0x7A, "_m113"), 3)
           , new Rule(Production_7A_1, new Hime.Redist.Parsers.SymbolVariable(0x7A, "_m113"), 0)
           , new Rule(Production_7B_0, new Hime.Redist.Parsers.SymbolVariable(0x7B, "grammar_cf_rules<grammar_text_terminal>"), 4)
           , new Rule(Production_7C_0, new Hime.Redist.Parsers.SymbolVariable(0x7C, "cf_rule_simple<grammar_text_terminal>"), 4)
           , new Rule(Production_7D_0, new Hime.Redist.Parsers.SymbolVariable(0x7D, "rule_definition<grammar_text_terminal>"), 2)
           , new Rule(Production_7E_0, new Hime.Redist.Parsers.SymbolVariable(0x7E, "rule_def_choice<grammar_text_terminal>"), 1)
           , new Rule(Production_7E_1, new Hime.Redist.Parsers.SymbolVariable(0x7E, "rule_def_choice<grammar_text_terminal>"), 0)
           , new Rule(Production_7F_0, new Hime.Redist.Parsers.SymbolVariable(0x7F, "rule_def_restrict<grammar_text_terminal>"), 2)
           , new Rule(Production_80_0, new Hime.Redist.Parsers.SymbolVariable(0x80, "rule_def_fragment<grammar_text_terminal>"), 2)
           , new Rule(Production_81_0, new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"), 2)
           , new Rule(Production_81_1, new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"), 2)
           , new Rule(Production_81_2, new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"), 2)
           , new Rule(Production_81_3, new Hime.Redist.Parsers.SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"), 1)
           , new Rule(Production_82_0, new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"), 2)
           , new Rule(Production_82_1, new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"), 2)
           , new Rule(Production_82_2, new Hime.Redist.Parsers.SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"), 1)
           , new Rule(Production_83_0, new Hime.Redist.Parsers.SymbolVariable(0x83, "rule_def_element<grammar_text_terminal>"), 1)
           , new Rule(Production_83_1, new Hime.Redist.Parsers.SymbolVariable(0x83, "rule_def_element<grammar_text_terminal>"), 3)
           , new Rule(Production_84_0, new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 1)
           , new Rule(Production_84_1, new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 1)
           , new Rule(Production_84_2, new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 1)
           , new Rule(Production_84_3, new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 1)
           , new Rule(Production_84_4, new Hime.Redist.Parsers.SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 1)
           , new Rule(Production_85_0, new Hime.Redist.Parsers.SymbolVariable(0x85, "rule_sym_ref_template<grammar_text_terminal>"), 2)
           , new Rule(Production_86_0, new Hime.Redist.Parsers.SymbolVariable(0x86, "rule_sym_ref_params<grammar_text_terminal>"), 4)
           , new Rule(Production_87_0, new Hime.Redist.Parsers.SymbolVariable(0x87, "_m134"), 3)
           , new Rule(Production_87_1, new Hime.Redist.Parsers.SymbolVariable(0x87, "_m134"), 0)
           , new Rule(Production_88_0, new Hime.Redist.Parsers.SymbolVariable(0x88, "_m143"), 2)
           , new Rule(Production_88_1, new Hime.Redist.Parsers.SymbolVariable(0x88, "_m143"), 0)
           , new Rule(Production_89_0, new Hime.Redist.Parsers.SymbolVariable(0x89, "_m145"), 3)
           , new Rule(Production_89_1, new Hime.Redist.Parsers.SymbolVariable(0x89, "_m145"), 0)
           , new Rule(Production_8A_0, new Hime.Redist.Parsers.SymbolVariable(0x8A, "_m147"), 3)
           , new Rule(Production_8A_1, new Hime.Redist.Parsers.SymbolVariable(0x8A, "_m147"), 0)
           , new Rule(Production_8B_0, new Hime.Redist.Parsers.SymbolVariable(0x8B, "cf_rule_template<grammar_text_terminal>"), 5)
           , new Rule(Production_8C_0, new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), 2)
           , new Rule(Production_8C_1, new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), 2)
           , new Rule(Production_8C_2, new Hime.Redist.Parsers.SymbolVariable(0x8C, "_m152"), 0)
           , new Rule(Production_8D_0, new Hime.Redist.Parsers.SymbolVariable(0x8D, "grammar_cf_rules<grammar_bin_terminal>"), 4)
           , new Rule(Production_8E_0, new Hime.Redist.Parsers.SymbolVariable(0x8E, "cf_rule_simple<grammar_bin_terminal>"), 4)
           , new Rule(Production_8F_0, new Hime.Redist.Parsers.SymbolVariable(0x8F, "rule_definition<grammar_bin_terminal>"), 2)
           , new Rule(Production_90_0, new Hime.Redist.Parsers.SymbolVariable(0x90, "rule_def_choice<grammar_bin_terminal>"), 1)
           , new Rule(Production_90_1, new Hime.Redist.Parsers.SymbolVariable(0x90, "rule_def_choice<grammar_bin_terminal>"), 0)
           , new Rule(Production_91_0, new Hime.Redist.Parsers.SymbolVariable(0x91, "rule_def_restrict<grammar_bin_terminal>"), 2)
           , new Rule(Production_92_0, new Hime.Redist.Parsers.SymbolVariable(0x92, "rule_def_fragment<grammar_bin_terminal>"), 2)
           , new Rule(Production_93_0, new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"), 2)
           , new Rule(Production_93_1, new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"), 2)
           , new Rule(Production_93_2, new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"), 2)
           , new Rule(Production_93_3, new Hime.Redist.Parsers.SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"), 1)
           , new Rule(Production_94_0, new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"), 2)
           , new Rule(Production_94_1, new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"), 2)
           , new Rule(Production_94_2, new Hime.Redist.Parsers.SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"), 1)
           , new Rule(Production_95_0, new Hime.Redist.Parsers.SymbolVariable(0x95, "rule_def_element<grammar_bin_terminal>"), 1)
           , new Rule(Production_95_1, new Hime.Redist.Parsers.SymbolVariable(0x95, "rule_def_element<grammar_bin_terminal>"), 3)
           , new Rule(Production_96_0, new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 1)
           , new Rule(Production_96_1, new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 1)
           , new Rule(Production_96_2, new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 1)
           , new Rule(Production_96_3, new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 1)
           , new Rule(Production_96_4, new Hime.Redist.Parsers.SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 1)
           , new Rule(Production_97_0, new Hime.Redist.Parsers.SymbolVariable(0x97, "rule_sym_ref_template<grammar_bin_terminal>"), 2)
           , new Rule(Production_98_0, new Hime.Redist.Parsers.SymbolVariable(0x98, "rule_sym_ref_params<grammar_bin_terminal>"), 4)
           , new Rule(Production_99_0, new Hime.Redist.Parsers.SymbolVariable(0x99, "_m175"), 3)
           , new Rule(Production_99_1, new Hime.Redist.Parsers.SymbolVariable(0x99, "_m175"), 0)
           , new Rule(Production_9A_0, new Hime.Redist.Parsers.SymbolVariable(0x9A, "_m184"), 2)
           , new Rule(Production_9A_1, new Hime.Redist.Parsers.SymbolVariable(0x9A, "_m184"), 0)
           , new Rule(Production_9B_0, new Hime.Redist.Parsers.SymbolVariable(0x9B, "_m186"), 3)
           , new Rule(Production_9B_1, new Hime.Redist.Parsers.SymbolVariable(0x9B, "_m186"), 0)
           , new Rule(Production_9C_0, new Hime.Redist.Parsers.SymbolVariable(0x9C, "_m188"), 3)
           , new Rule(Production_9C_1, new Hime.Redist.Parsers.SymbolVariable(0x9C, "_m188"), 0)
           , new Rule(Production_9D_0, new Hime.Redist.Parsers.SymbolVariable(0x9D, "cf_rule_template<grammar_bin_terminal>"), 5)
           , new Rule(Production_9E_0, new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), 2)
           , new Rule(Production_9E_1, new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), 2)
           , new Rule(Production_9E_2, new Hime.Redist.Parsers.SymbolVariable(0x9E, "_m193"), 0)
           , new Rule(Production_DC_0, new Hime.Redist.Parsers.SymbolVariable(0xDC, "cs_grammar_text"), 9)
           , new Rule(Production_DD_0, new Hime.Redist.Parsers.SymbolVariable(0xDD, "cs_grammar_bin"), 8)
           , new Rule(Production_DE_0, new Hime.Redist.Parsers.SymbolVariable(0xDE, "grammar_cs_rules<grammar_text_terminal>"), 4)
           , new Rule(Production_DF_0, new Hime.Redist.Parsers.SymbolVariable(0xDF, "cs_rule_simple<grammar_text_terminal>"), 6)
           , new Rule(Production_E0_0, new Hime.Redist.Parsers.SymbolVariable(0xE0, "cs_rule_context<grammar_text_terminal>"), 3)
           , new Rule(Production_E0_1, new Hime.Redist.Parsers.SymbolVariable(0xE0, "cs_rule_context<grammar_text_terminal>"), 0)
           , new Rule(Production_E1_0, new Hime.Redist.Parsers.SymbolVariable(0xE1, "cs_rule_template<grammar_text_terminal>"), 7)
           , new Rule(Production_E2_0, new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), 2)
           , new Rule(Production_E2_1, new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), 2)
           , new Rule(Production_E2_2, new Hime.Redist.Parsers.SymbolVariable(0xE2, "_m160"), 0)
           , new Rule(Production_E3_0, new Hime.Redist.Parsers.SymbolVariable(0xE3, "grammar_cs_rules<grammar_bin_terminal>"), 4)
           , new Rule(Production_E4_0, new Hime.Redist.Parsers.SymbolVariable(0xE4, "cs_rule_simple<grammar_bin_terminal>"), 6)
           , new Rule(Production_E5_0, new Hime.Redist.Parsers.SymbolVariable(0xE5, "cs_rule_context<grammar_bin_terminal>"), 3)
           , new Rule(Production_E5_1, new Hime.Redist.Parsers.SymbolVariable(0xE5, "cs_rule_context<grammar_bin_terminal>"), 0)
           , new Rule(Production_E6_0, new Hime.Redist.Parsers.SymbolVariable(0xE6, "cs_rule_template<grammar_bin_terminal>"), 7)
           , new Rule(Production_E7_0, new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), 2)
           , new Rule(Production_E7_1, new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), 2)
           , new Rule(Production_E7_2, new Hime.Redist.Parsers.SymbolVariable(0xE7, "_m178"), 0)
           , new Rule(Production_E8_0, new Hime.Redist.Parsers.SymbolVariable(0xE8, "file_item"), 1)
           , new Rule(Production_E9_0, new Hime.Redist.Parsers.SymbolVariable(0xE9, "file"), 2)
           , new Rule(Production_EA_0, new Hime.Redist.Parsers.SymbolVariable(0xEA, "_m234"), 2)
           , new Rule(Production_EA_1, new Hime.Redist.Parsers.SymbolVariable(0xEA, "_m234"), 0)
           , new Rule(Production_EB_0, new Hime.Redist.Parsers.SymbolVariable(0xEB, "_Axiom_"), 2)
        };
        private static State[] staticStates = {
            new State(
               new string[21] {"[_Axiom_ → • file $, ε]", "[file → • file_item _m234, $]", "[file_item → • Namespace_child_symbol, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • Namespace, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace → • namespace qualified_name { Namespace_content }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[grammar_access → • symbol_access_public, grammar]", "[grammar_access → • symbol_access_private, grammar]", "[grammar_access → • symbol_access_protected, grammar]", "[grammar_access → • symbol_access_internal, grammar]", "[symbol_access_public → • public, grammar]", "[symbol_access_private → • private, grammar]", "[symbol_access_protected → • protected, grammar]", "[symbol_access_internal → • internal, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[6] {new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xe9, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_Axiom_ → file • $, ε]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2)},
               new ushort[1] {0x2},
               new ushort[1] {0x14},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[22] {"[file → file_item • _m234, $]", "[_m234 → • file_item _m234, $]", "[_m234 → •, $]", "[file_item → • Namespace_child_symbol, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • Namespace, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace → • namespace qualified_name { Namespace_content }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[grammar_access → • symbol_access_public, grammar]", "[grammar_access → • symbol_access_private, grammar]", "[grammar_access → • symbol_access_protected, grammar]", "[grammar_access → • symbol_access_internal, grammar]", "[symbol_access_public → • public, grammar]", "[symbol_access_private → • private, grammar]", "[symbol_access_protected → • protected, grammar]", "[symbol_access_internal → • internal, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xea, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x15, 0x16, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB4])})
            , new State(
               new string[1] {"[file_item → Namespace_child_symbol •, $/public/private/protected/internal/namespace/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[7] {new Reduction(0x2, staticRules[0xB1]), new Reduction(0xc, staticRules[0xB1]), new Reduction(0xd, staticRules[0xB1]), new Reduction(0xe, staticRules[0xB1]), new Reduction(0xf, staticRules[0xB1]), new Reduction(0x10, staticRules[0xB1]), new Reduction(0x52, staticRules[0xB1])})
            , new State(
               new string[1] {"[Namespace_child_symbol → Namespace •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x5]), new Reduction(0xc, staticRules[0x5]), new Reduction(0xd, staticRules[0x5]), new Reduction(0xe, staticRules[0x5]), new Reduction(0xf, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x12, staticRules[0x5]), new Reduction(0x52, staticRules[0x5])})
            , new State(
               new string[1] {"[Namespace_child_symbol → cf_grammar_text •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x6]), new Reduction(0xc, staticRules[0x6]), new Reduction(0xd, staticRules[0x6]), new Reduction(0xe, staticRules[0x6]), new Reduction(0xf, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x12, staticRules[0x6]), new Reduction(0x52, staticRules[0x6])})
            , new State(
               new string[1] {"[Namespace_child_symbol → cf_grammar_bin •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x7]), new Reduction(0xc, staticRules[0x7]), new Reduction(0xd, staticRules[0x7]), new Reduction(0xe, staticRules[0x7]), new Reduction(0xf, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x12, staticRules[0x7]), new Reduction(0x52, staticRules[0x7])})
            , new State(
               new string[1] {"[Namespace_child_symbol → cs_grammar_text •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x8]), new Reduction(0xc, staticRules[0x8]), new Reduction(0xd, staticRules[0x8]), new Reduction(0xe, staticRules[0x8]), new Reduction(0xf, staticRules[0x8]), new Reduction(0x10, staticRules[0x8]), new Reduction(0x12, staticRules[0x8]), new Reduction(0x52, staticRules[0x8])})
            , new State(
               new string[1] {"[Namespace_child_symbol → cs_grammar_bin •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x9]), new Reduction(0xc, staticRules[0x9]), new Reduction(0xd, staticRules[0x9]), new Reduction(0xe, staticRules[0x9]), new Reduction(0xf, staticRules[0x9]), new Reduction(0x10, staticRules[0x9]), new Reduction(0x12, staticRules[0x9]), new Reduction(0x52, staticRules[0x9])})
            , new State(
               new string[2] {"[Namespace → namespace • qualified_name { Namespace_content }, $/public/private/protected/internal/namespace/}/grammar]", "[qualified_name → • NAME _m20, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x17},
               new Reduction[0] {})
            , new State(
               new string[2] {"[cf_grammar_text → grammar_access • grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access • grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[1] {0x52},
               new ushort[1] {0x19},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[2] {"[cs_grammar_text → grammar • cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar • cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[cs]", 0xD9)},
               new ushort[1] {0xd9},
               new ushort[1] {0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_access → symbol_access_public •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x45])})
            , new State(
               new string[1] {"[grammar_access → symbol_access_private •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x46])})
            , new State(
               new string[1] {"[grammar_access → symbol_access_protected •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x47])})
            , new State(
               new string[1] {"[grammar_access → symbol_access_internal •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x48])})
            , new State(
               new string[1] {"[symbol_access_public → public •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x1])})
            , new State(
               new string[1] {"[symbol_access_private → private •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x2])})
            , new State(
               new string[1] {"[symbol_access_protected → protected •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x3])})
            , new State(
               new string[1] {"[symbol_access_internal → internal •, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x4])})
            , new State(
               new string[1] {"[_Axiom_ → file $ •, ε]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("ε", 0x1)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0xB5])})
            , new State(
               new string[1] {"[file → file_item _m234 •, $]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB2])})
            , new State(
               new string[22] {"[_m234 → file_item • _m234, $]", "[_m234 → • file_item _m234, $]", "[_m234 → •, $]", "[file_item → • Namespace_child_symbol, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • Namespace, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cf_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_text, $/public/private/protected/internal/namespace/grammar]", "[Namespace_child_symbol → • cs_grammar_bin, $/public/private/protected/internal/namespace/grammar]", "[Namespace → • namespace qualified_name { Namespace_content }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/grammar]", "[grammar_access → • symbol_access_public, grammar]", "[grammar_access → • symbol_access_private, grammar]", "[grammar_access → • symbol_access_protected, grammar]", "[grammar_access → • symbol_access_internal, grammar]", "[symbol_access_public → • public, grammar]", "[symbol_access_private → • private, grammar]", "[symbol_access_protected → • protected, grammar]", "[symbol_access_internal → • internal, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xea, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x1B, 0x16, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB4])})
            , new State(
               new string[1] {"[Namespace → namespace qualified_name • { Namespace_content }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x1C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[qualified_name → NAME • _m20, {/;/,]", "[_m20 → • . NAME _m20, {/;/,]", "[_m20 → •, {/;/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[1] {0xb},
               new ushort[1] {0x1E},
               new ushort[1] {0x1b},
               new ushort[1] {0x1D},
               new Reduction[3] {new Reduction(0x11, staticRules[0xD]), new Reduction(0x41, staticRules[0xD]), new Reduction(0x48, staticRules[0xD])})
            , new State(
               new string[2] {"[cf_grammar_text → grammar_access grammar • cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar • cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[cf]", 0x53)},
               new ushort[1] {0x53},
               new ushort[1] {0x1F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[2] {"[cs_grammar_text → grammar cs • NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar cs • NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x20},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_m234 → file_item _m234 •, $]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB3])})
            , new State(
               new string[22] {"[Namespace → namespace qualified_name { • Namespace_content }, $/public/private/protected/internal/namespace/}/grammar]", "[Namespace_content → • _m25, }]", "[_m25 → • Namespace_child_symbol _m25, }]", "[_m25 → •, }]", "[Namespace_child_symbol → • Namespace, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cf_grammar_text, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cf_grammar_bin, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cs_grammar_text, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cs_grammar_bin, public/private/protected/internal/namespace/}/grammar]", "[Namespace → • namespace qualified_name { Namespace_content }, public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[grammar_access → • symbol_access_public, grammar]", "[grammar_access → • symbol_access_private, grammar]", "[grammar_access → • symbol_access_protected, grammar]", "[grammar_access → • symbol_access_internal, grammar]", "[symbol_access_public → • public, grammar]", "[symbol_access_private → • private, grammar]", "[symbol_access_protected → • protected, grammar]", "[symbol_access_internal → • internal, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0x19, 0x1c, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x21, 0x22, 0x23, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x12, staticRules[0xF])})
            , new State(
               new string[1] {"[qualified_name → NAME _m20 •, {/;/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x11, staticRules[0x0]), new Reduction(0x41, staticRules[0x0]), new Reduction(0x48, staticRules[0x0])})
            , new State(
               new string[1] {"[_m20 → . • NAME _m20, {/;/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x24},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[2] {"[cf_grammar_text → grammar_access grammar cf • NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar cf • NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x25},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[4] {"[cs_grammar_text → grammar cs NAME • grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar cs NAME • grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_parency → • : qualified_name _m113, {]", "[grammar_parency → •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[:]", 0x51)},
               new ushort[1] {0x51},
               new ushort[1] {0x27},
               new ushort[1] {0x70},
               new ushort[1] {0x26},
               new Reduction[1] {new Reduction(0x11, staticRules[0x44])})
            , new State(
               new string[1] {"[Namespace → namespace qualified_name { Namespace_content • }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x28},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[Namespace_content → _m25 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA])})
            , new State(
               new string[21] {"[_m25 → Namespace_child_symbol • _m25, }]", "[_m25 → • Namespace_child_symbol _m25, }]", "[_m25 → •, }]", "[Namespace_child_symbol → • Namespace, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cf_grammar_text, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cf_grammar_bin, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cs_grammar_text, public/private/protected/internal/namespace/}/grammar]", "[Namespace_child_symbol → • cs_grammar_bin, public/private/protected/internal/namespace/}/grammar]", "[Namespace → • namespace qualified_name { Namespace_content }, public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_text → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → • grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_text → • grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → • grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }, public/private/protected/internal/namespace/}/grammar]", "[grammar_access → • symbol_access_public, grammar]", "[grammar_access → • symbol_access_private, grammar]", "[grammar_access → • symbol_access_protected, grammar]", "[grammar_access → • symbol_access_internal, grammar]", "[symbol_access_public → • public, grammar]", "[symbol_access_private → • private, grammar]", "[symbol_access_protected → • protected, grammar]", "[symbol_access_internal → • internal, grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[12] {0x1c, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[12] {0x29, 0x23, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x12, staticRules[0xF])})
            , new State(
               new string[3] {"[_m20 → . NAME • _m20, {/;/,]", "[_m20 → • . NAME _m20, {/;/,]", "[_m20 → •, {/;/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[1] {0xb},
               new ushort[1] {0x1E},
               new ushort[1] {0x1b},
               new ushort[1] {0x2A},
               new Reduction[3] {new Reduction(0x11, staticRules[0xD]), new Reduction(0x41, staticRules[0xD]), new Reduction(0x48, staticRules[0xD])})
            , new State(
               new string[4] {"[cf_grammar_text → grammar_access grammar cf NAME • grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar cf NAME • grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_parency → • : qualified_name _m113, {]", "[grammar_parency → •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[:]", 0x51)},
               new ushort[1] {0x51},
               new ushort[1] {0x27},
               new ushort[1] {0x70},
               new ushort[1] {0x2B},
               new Reduction[1] {new Reduction(0x11, staticRules[0x44])})
            , new State(
               new string[2] {"[cs_grammar_text → grammar cs NAME grammar_parency • { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar cs NAME grammar_parency • { grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x2C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[2] {"[grammar_parency → : • qualified_name _m113, {]", "[qualified_name → • NAME _m20, {/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x2D},
               new Reduction[0] {})
            , new State(
               new string[1] {"[Namespace → namespace qualified_name { Namespace_content } •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0xB]), new Reduction(0xc, staticRules[0xB]), new Reduction(0xd, staticRules[0xB]), new Reduction(0xe, staticRules[0xB]), new Reduction(0xf, staticRules[0xB]), new Reduction(0x10, staticRules[0xB]), new Reduction(0x12, staticRules[0xB]), new Reduction(0x52, staticRules[0xB])})
            , new State(
               new string[1] {"[_m25 → Namespace_child_symbol _m25 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xE])})
            , new State(
               new string[1] {"[_m20 → . NAME _m20 •, {/;/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x11, staticRules[0xC]), new Reduction(0x41, staticRules[0xC]), new Reduction(0x48, staticRules[0xC])})
            , new State(
               new string[2] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency • { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency • { grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x2E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[cs_grammar_text → grammar cs NAME grammar_parency { • grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar cs NAME grammar_parency { • grammar_options grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_options → • options { _m105 }, terminals/rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[options]", 0x4F)},
               new ushort[1] {0x4f},
               new ushort[1] {0x30},
               new ushort[1] {0x6e},
               new ushort[1] {0x2F},
               new Reduction[0] {})
            , new State(
               new string[3] {"[grammar_parency → : qualified_name • _m113, {]", "[_m113 → • , qualified_name _m113, {]", "[_m113 → •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[1] {0x48},
               new ushort[1] {0x32},
               new ushort[1] {0x7a},
               new ushort[1] {0x31},
               new Reduction[1] {new Reduction(0x11, staticRules[0x58])})
            , new State(
               new string[3] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { • grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { • grammar_options grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_options → • options { _m105 }, terminals/rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[options]", 0x4F)},
               new ushort[1] {0x4f},
               new ushort[1] {0x30},
               new ushort[1] {0x6e},
               new ushort[1] {0x33},
               new Reduction[0] {})
            , new State(
               new string[4] {"[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options • grammar_terminals grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options • grammar_cs_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_terminals → • terminals { _m109 }, rules]", "[grammar_cs_rules<grammar_bin_terminal> → • rules { _m178 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[terminals]", 0x50), new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[2] {0x50, 0x54},
               new ushort[2] {0x36, 0x37},
               new ushort[2] {0x6f, 0xe3},
               new ushort[2] {0x34, 0x35},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_options → options • { _m105 }, terminals/rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x38},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_parency → : qualified_name _m113 •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x11, staticRules[0x43])})
            , new State(
               new string[2] {"[_m113 → , • qualified_name _m113, {]", "[qualified_name → • NAME _m20, {/,]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x39},
               new Reduction[0] {})
            , new State(
               new string[4] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options • grammar_terminals grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options • grammar_cf_rules<grammar_bin_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_terminals → • terminals { _m109 }, rules]", "[grammar_cf_rules<grammar_bin_terminal> → • rules { _m193 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[terminals]", 0x50), new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[2] {0x50, 0x54},
               new ushort[2] {0x36, 0x3C},
               new ushort[2] {0x6f, 0x8d},
               new ushort[2] {0x3A, 0x3B},
               new Reduction[0] {})
            , new State(
               new string[2] {"[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals • grammar_cs_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_cs_rules<grammar_text_terminal> → • rules { _m160 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[1] {0x54},
               new ushort[1] {0x3E},
               new ushort[1] {0xde},
               new ushort[1] {0x3D},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> • }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x3F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_terminals → terminals • { _m109 }, rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x40},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_bin_terminal> → rules • { _m178 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x41},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[4] {"[grammar_options → options { • _m105 }, terminals/rules]", "[_m105 → • option _m105, }]", "[_m105 → •, }]", "[option → • NAME = QUOTED_DATA ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x44},
               new ushort[2] {0x78, 0x57},
               new ushort[2] {0x42, 0x43},
               new Reduction[1] {new Reduction(0x12, staticRules[0x54])})
            , new State(
               new string[3] {"[_m113 → , qualified_name • _m113, {]", "[_m113 → • , qualified_name _m113, {]", "[_m113 → •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[1] {0x48},
               new ushort[1] {0x32},
               new ushort[1] {0x7a},
               new ushort[1] {0x45},
               new Reduction[1] {new Reduction(0x11, staticRules[0x58])})
            , new State(
               new string[2] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals • grammar_cf_rules<grammar_text_terminal> }, $/public/private/protected/internal/namespace/}/grammar]", "[grammar_cf_rules<grammar_text_terminal> → • rules { _m152 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[1] {0x54},
               new ushort[1] {0x47},
               new ushort[1] {0x7b},
               new ushort[1] {0x46},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> • }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x48},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_bin_terminal> → rules • { _m193 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x49},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> • }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x4A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_text_terminal> → rules • { _m160 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x4B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_grammar_bin → grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> } •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0xA0]), new Reduction(0xc, staticRules[0xA0]), new Reduction(0xd, staticRules[0xA0]), new Reduction(0xe, staticRules[0xA0]), new Reduction(0xf, staticRules[0xA0]), new Reduction(0x10, staticRules[0xA0]), new Reduction(0x12, staticRules[0xA0]), new Reduction(0x52, staticRules[0xA0])})
            , new State(
               new string[4] {"[grammar_terminals → terminals { • _m109 }, rules]", "[_m109 → • terminal _m109, }]", "[_m109 → •, }]", "[terminal → • NAME -> terminal_definition terminal_subgrammar ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x4E},
               new ushort[2] {0x79, 0x67},
               new ushort[2] {0x4C, 0x4D},
               new Reduction[1] {new Reduction(0x12, staticRules[0x56])})
            , new State(
               new string[8] {"[grammar_cs_rules<grammar_bin_terminal> → rules { • _m178 }, }]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178, }]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178, }]", "[_m178 → •, }]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ], NAME]", "[cs_rule_context<grammar_bin_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x4F, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               new string[1] {"[grammar_options → options { _m105 • }, terminals/rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x54},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[4] {"[_m105 → option • _m105, }]", "[_m105 → • option _m105, }]", "[_m105 → •, }]", "[option → • NAME = QUOTED_DATA ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x44},
               new ushort[2] {0x78, 0x57},
               new ushort[2] {0x55, 0x43},
               new Reduction[1] {new Reduction(0x12, staticRules[0x54])})
            , new State(
               new string[1] {"[option → NAME • = QUOTED_DATA ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[=]", 0x40)},
               new ushort[1] {0x40},
               new ushort[1] {0x56},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_m113 → , qualified_name _m113 •, {]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x11, staticRules[0x57])})
            , new State(
               new string[1] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> • }, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x57},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_text_terminal> → rules • { _m152 }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11)},
               new ushort[1] {0x11},
               new ushort[1] {0x58},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_grammar_bin → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> } •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x4A]), new Reduction(0xc, staticRules[0x4A]), new Reduction(0xd, staticRules[0x4A]), new Reduction(0xe, staticRules[0x4A]), new Reduction(0xf, staticRules[0x4A]), new Reduction(0x10, staticRules[0x4A]), new Reduction(0x12, staticRules[0x4A]), new Reduction(0x52, staticRules[0x4A])})
            , new State(
               new string[6] {"[grammar_cf_rules<grammar_bin_terminal> → rules { • _m193 }, }]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193, }]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193, }]", "[_m193 → •, }]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;, NAME/}]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x59, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               new string[1] {"[cs_grammar_text → grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> } •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x9F]), new Reduction(0xc, staticRules[0x9F]), new Reduction(0xd, staticRules[0x9F]), new Reduction(0xe, staticRules[0x9F]), new Reduction(0xf, staticRules[0x9F]), new Reduction(0x10, staticRules[0x9F]), new Reduction(0x12, staticRules[0x9F]), new Reduction(0x52, staticRules[0x9F])})
            , new State(
               new string[8] {"[grammar_cs_rules<grammar_text_terminal> → rules { • _m160 }, }]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160, }]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160, }]", "[_m160 → •, }]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ], NAME]", "[cs_rule_context<grammar_text_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x5D, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               new string[1] {"[grammar_terminals → terminals { _m109 • }, rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x62},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[4] {"[_m109 → terminal • _m109, }]", "[_m109 → • terminal _m109, }]", "[_m109 → •, }]", "[terminal → • NAME -> terminal_definition terminal_subgrammar ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x4E},
               new ushort[2] {0x79, 0x67},
               new ushort[2] {0x63, 0x4D},
               new Reduction[1] {new Reduction(0x12, staticRules[0x56])})
            , new State(
               new string[1] {"[terminal → NAME • -> terminal_definition terminal_subgrammar ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[1] {0x4c},
               new ushort[1] {0x64},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_bin_terminal> → rules { _m178 • }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x65},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[8] {"[_m178 → cs_rule_simple<grammar_bin_terminal> • _m178, }]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178, }]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178, }]", "[_m178 → •, }]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ], NAME]", "[cs_rule_context<grammar_bin_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x66, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               new string[8] {"[_m178 → cs_rule_template<grammar_bin_terminal> • _m178, }]", "[_m178 → • cs_rule_simple<grammar_bin_terminal> _m178, }]", "[_m178 → • cs_rule_template<grammar_bin_terminal> _m178, }]", "[_m178 → •, }]", "[cs_rule_simple<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → • cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ], NAME]", "[cs_rule_context<grammar_bin_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x67, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               new string[2] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> • NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> • NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x68},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[36] {"[cs_rule_context<grammar_bin_terminal> → [ • rule_definition<grammar_bin_terminal> ], NAME/->/<]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, ]]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, |/]]", "[rule_def_choice<grammar_bin_terminal> → •, |/]]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, |/]]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, -/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/]]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x4a, staticRules[0x80]), new Reduction(0xdb, staticRules[0x80])})
            , new State(
               new string[1] {"[grammar_options → options { _m105 } •, terminals/rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[terminals]", 0x50), new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x50, staticRules[0x41]), new Reduction(0x54, staticRules[0x41])})
            , new State(
               new string[1] {"[_m105 → option _m105 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x53])})
            , new State(
               new string[1] {"[option → NAME = • QUOTED_DATA ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E)},
               new ushort[1] {0x2e},
               new ushort[1] {0x86},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_grammar_text → grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> } •, $/public/private/protected/internal/namespace/}/grammar]"},
               new Hime.Redist.Parsers.SymbolTerminal[8] {new Hime.Redist.Parsers.SymbolTerminal("$", 0x2), new Hime.Redist.Parsers.SymbolTerminal("_T[public]", 0xC), new Hime.Redist.Parsers.SymbolTerminal("_T[private]", 0xD), new Hime.Redist.Parsers.SymbolTerminal("_T[protected]", 0xE), new Hime.Redist.Parsers.SymbolTerminal("_T[internal]", 0xF), new Hime.Redist.Parsers.SymbolTerminal("_T[namespace]", 0x10), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[grammar]", 0x52)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x49]), new Reduction(0xc, staticRules[0x49]), new Reduction(0xd, staticRules[0x49]), new Reduction(0xe, staticRules[0x49]), new Reduction(0xf, staticRules[0x49]), new Reduction(0x10, staticRules[0x49]), new Reduction(0x12, staticRules[0x49]), new Reduction(0x52, staticRules[0x49])})
            , new State(
               new string[6] {"[grammar_cf_rules<grammar_text_terminal> → rules { • _m152 }, }]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152, }]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152, }]", "[_m152 → •, }]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;, NAME/}]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0x87, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_bin_terminal> → rules { _m193 • }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x8B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[6] {"[_m193 → cf_rule_simple<grammar_bin_terminal> • _m193, }]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193, }]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193, }]", "[_m193 → •, }]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;, NAME/}]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x8C, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               new string[6] {"[_m193 → cf_rule_template<grammar_bin_terminal> • _m193, }]", "[_m193 → • cf_rule_simple<grammar_bin_terminal> _m193, }]", "[_m193 → • cf_rule_template<grammar_bin_terminal> _m193, }]", "[_m193 → •, }]", "[cf_rule_simple<grammar_bin_terminal> → • NAME -> rule_definition<grammar_bin_terminal> ;, NAME/}]", "[cf_rule_template<grammar_bin_terminal> → • NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x8D, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               new string[3] {"[cf_rule_simple<grammar_bin_terminal> → NAME • -> rule_definition<grammar_bin_terminal> ;, NAME/}]", "[cf_rule_template<grammar_bin_terminal> → NAME • rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}]", "[rule_template_params → • < NAME _m101 >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x8E, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0x8F},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_text_terminal> → rules { _m160 • }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x91},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[8] {"[_m160 → cs_rule_simple<grammar_text_terminal> • _m160, }]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160, }]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160, }]", "[_m160 → •, }]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ], NAME]", "[cs_rule_context<grammar_text_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x92, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               new string[8] {"[_m160 → cs_rule_template<grammar_text_terminal> • _m160, }]", "[_m160 → • cs_rule_simple<grammar_text_terminal> _m160, }]", "[_m160 → • cs_rule_template<grammar_text_terminal> _m160, }]", "[_m160 → •, }]", "[cs_rule_simple<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → • cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ], NAME]", "[cs_rule_context<grammar_text_terminal> → •, NAME]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x93, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               new string[2] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> • NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> • NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x94},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[26] {"[cs_rule_context<grammar_text_terminal> → [ • rule_definition<grammar_text_terminal> ], NAME/->/<]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, ]]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, |/]]", "[rule_def_choice<grammar_text_terminal> → •, |/]]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, |/]]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, -/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/]]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x4a, staticRules[0x5D]), new Reduction(0xdb, staticRules[0x5D])})
            , new State(
               new string[1] {"[grammar_terminals → terminals { _m109 } •, rules]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[rules]", 0x54)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x54, staticRules[0x42])})
            , new State(
               new string[1] {"[_m109 → terminal _m109 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x55])})
            , new State(
               new string[24] {"[terminal → NAME -> • terminal_definition terminal_subgrammar ;, NAME/}]", "[terminal_definition → • terminal_def_restrict _m93, ;/=>]", "[terminal_def_restrict → • terminal_def_fragment _m91, ;/|/=>]", "[terminal_def_fragment → • terminal_def_repetition _m89, ;/-/|/=>]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/-/|/=>]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/-/|/=>]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/*/+/?/-/|/=>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[9] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[13] {0x65, 0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[13] {0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_bin_terminal> → rules { _m178 } •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA9])})
            , new State(
               new string[1] {"[_m178 → cs_rule_simple<grammar_bin_terminal> _m178 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xAE])})
            , new State(
               new string[1] {"[_m178 → cs_rule_template<grammar_bin_terminal> _m178 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xAF])})
            , new State(
               new string[4] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME • cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME • cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_bin_terminal> → • [ rule_definition<grammar_bin_terminal> ], ->/<]", "[cs_rule_context<grammar_bin_terminal> → •, ->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[1] {0xe5},
               new ushort[1] {0xBB},
               new Reduction[2] {new Reduction(0x4c, staticRules[0xAC]), new Reduction(0x4d, staticRules[0xAC])})
            , new State(
               new string[1] {"[cs_rule_context<grammar_bin_terminal> → [ rule_definition<grammar_bin_terminal> • ], NAME/->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0xdb},
               new ushort[1] {0xBC},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[rule_definition<grammar_bin_terminal> → rule_def_choice<grammar_bin_terminal> • _m188, ;/)/]]", "[_m188 → • | rule_def_choice<grammar_bin_terminal> _m188, ;/)/]]", "[_m188 → •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4a},
               new ushort[1] {0xBE},
               new ushort[1] {0x9c},
               new ushort[1] {0xBD},
               new Reduction[3] {new Reduction(0x41, staticRules[0x9A]), new Reduction(0x44, staticRules[0x9A]), new Reduction(0xdb, staticRules[0x9A])})
            , new State(
               new string[1] {"[rule_def_choice<grammar_bin_terminal> → rule_def_restrict<grammar_bin_terminal> •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x7F]), new Reduction(0x44, staticRules[0x7F]), new Reduction(0x4a, staticRules[0x7F]), new Reduction(0xdb, staticRules[0x7F])})
            , new State(
               new string[3] {"[rule_def_restrict<grammar_bin_terminal> → rule_def_fragment<grammar_bin_terminal> • _m186, ;/)/|/]]", "[_m186 → • - rule_def_fragment<grammar_bin_terminal> _m186, ;/)/|/]]", "[_m186 → •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x49},
               new ushort[1] {0xC0},
               new ushort[1] {0x9b},
               new ushort[1] {0xBF},
               new Reduction[4] {new Reduction(0x41, staticRules[0x98]), new Reduction(0x44, staticRules[0x98]), new Reduction(0x4a, staticRules[0x98]), new Reduction(0xdb, staticRules[0x98])})
            , new State(
               new string[33] {"[rule_def_fragment<grammar_bin_terminal> → rule_def_repetition<grammar_bin_terminal> • _m184, ;/)/-/|/]]", "[_m184 → • rule_def_repetition<grammar_bin_terminal> _m184, ;/)/-/|/]]", "[_m184 → •, ;/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[21] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x9a, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xC1, 0xC2, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[5] {new Reduction(0x41, staticRules[0x96]), new Reduction(0x44, staticRules[0x96]), new Reduction(0x49, staticRules[0x96]), new Reduction(0x4a, staticRules[0x96]), new Reduction(0xdb, staticRules[0x96])})
            , new State(
               new string[4] {"[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> • ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[24] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xC3, 0xC4, 0xC5},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x86]), new Reduction(0x11, staticRules[0x86]), new Reduction(0x2e, staticRules[0x86]), new Reduction(0x34, staticRules[0x86]), new Reduction(0x35, staticRules[0x86]), new Reduction(0x36, staticRules[0x86]), new Reduction(0x37, staticRules[0x86]), new Reduction(0x38, staticRules[0x86]), new Reduction(0x39, staticRules[0x86]), new Reduction(0x3a, staticRules[0x86]), new Reduction(0x3b, staticRules[0x86]), new Reduction(0x3c, staticRules[0x86]), new Reduction(0x3d, staticRules[0x86]), new Reduction(0x3e, staticRules[0x86]), new Reduction(0x3f, staticRules[0x86]), new Reduction(0x41, staticRules[0x86]), new Reduction(0x43, staticRules[0x86]), new Reduction(0x44, staticRules[0x86]), new Reduction(0x49, staticRules[0x86]), new Reduction(0x4a, staticRules[0x86]), new Reduction(0xdb, staticRules[0x86])})
            , new State(
               new string[3] {"[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> • ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> • !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[26] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xC6, 0xC7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x89]), new Reduction(0x11, staticRules[0x89]), new Reduction(0x2e, staticRules[0x89]), new Reduction(0x34, staticRules[0x89]), new Reduction(0x35, staticRules[0x89]), new Reduction(0x36, staticRules[0x89]), new Reduction(0x37, staticRules[0x89]), new Reduction(0x38, staticRules[0x89]), new Reduction(0x39, staticRules[0x89]), new Reduction(0x3a, staticRules[0x89]), new Reduction(0x3b, staticRules[0x89]), new Reduction(0x3c, staticRules[0x89]), new Reduction(0x3d, staticRules[0x89]), new Reduction(0x3e, staticRules[0x89]), new Reduction(0x3f, staticRules[0x89]), new Reduction(0x41, staticRules[0x89]), new Reduction(0x43, staticRules[0x89]), new Reduction(0x44, staticRules[0x89]), new Reduction(0x45, staticRules[0x89]), new Reduction(0x46, staticRules[0x89]), new Reduction(0x47, staticRules[0x89]), new Reduction(0x49, staticRules[0x89]), new Reduction(0x4a, staticRules[0x89]), new Reduction(0xdb, staticRules[0x89])})
            , new State(
               new string[1] {"[rule_def_element<grammar_bin_terminal> → rule_def_atom<grammar_bin_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[26] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[26] {new Reduction(0xa, staticRules[0x8A]), new Reduction(0x11, staticRules[0x8A]), new Reduction(0x2e, staticRules[0x8A]), new Reduction(0x34, staticRules[0x8A]), new Reduction(0x35, staticRules[0x8A]), new Reduction(0x36, staticRules[0x8A]), new Reduction(0x37, staticRules[0x8A]), new Reduction(0x38, staticRules[0x8A]), new Reduction(0x39, staticRules[0x8A]), new Reduction(0x3a, staticRules[0x8A]), new Reduction(0x3b, staticRules[0x8A]), new Reduction(0x3c, staticRules[0x8A]), new Reduction(0x3d, staticRules[0x8A]), new Reduction(0x3e, staticRules[0x8A]), new Reduction(0x3f, staticRules[0x8A]), new Reduction(0x41, staticRules[0x8A]), new Reduction(0x43, staticRules[0x8A]), new Reduction(0x44, staticRules[0x8A]), new Reduction(0x45, staticRules[0x8A]), new Reduction(0x46, staticRules[0x8A]), new Reduction(0x47, staticRules[0x8A]), new Reduction(0x49, staticRules[0x8A]), new Reduction(0x4a, staticRules[0x8A]), new Reduction(0x55, staticRules[0x8A]), new Reduction(0x56, staticRules[0x8A]), new Reduction(0xdb, staticRules[0x8A])})
            , new State(
               new string[36] {"[rule_def_element<grammar_bin_terminal> → ( • rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, )]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, )/|]", "[rule_def_choice<grammar_bin_terminal> → •, )/|]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, )/|]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, )/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/(/)/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xC8, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x44, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_bin_terminal> → rule_sym_action •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8C]), new Reduction(0x11, staticRules[0x8C]), new Reduction(0x2e, staticRules[0x8C]), new Reduction(0x34, staticRules[0x8C]), new Reduction(0x35, staticRules[0x8C]), new Reduction(0x36, staticRules[0x8C]), new Reduction(0x37, staticRules[0x8C]), new Reduction(0x38, staticRules[0x8C]), new Reduction(0x39, staticRules[0x8C]), new Reduction(0x3a, staticRules[0x8C]), new Reduction(0x3b, staticRules[0x8C]), new Reduction(0x3c, staticRules[0x8C]), new Reduction(0x3d, staticRules[0x8C]), new Reduction(0x3e, staticRules[0x8C]), new Reduction(0x3f, staticRules[0x8C]), new Reduction(0x41, staticRules[0x8C]), new Reduction(0x43, staticRules[0x8C]), new Reduction(0x44, staticRules[0x8C]), new Reduction(0x45, staticRules[0x8C]), new Reduction(0x46, staticRules[0x8C]), new Reduction(0x47, staticRules[0x8C]), new Reduction(0x48, staticRules[0x8C]), new Reduction(0x49, staticRules[0x8C]), new Reduction(0x4a, staticRules[0x8C]), new Reduction(0x4e, staticRules[0x8C]), new Reduction(0x55, staticRules[0x8C]), new Reduction(0x56, staticRules[0x8C]), new Reduction(0xdb, staticRules[0x8C])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_bin_terminal> → rule_sym_virtual •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8D]), new Reduction(0x11, staticRules[0x8D]), new Reduction(0x2e, staticRules[0x8D]), new Reduction(0x34, staticRules[0x8D]), new Reduction(0x35, staticRules[0x8D]), new Reduction(0x36, staticRules[0x8D]), new Reduction(0x37, staticRules[0x8D]), new Reduction(0x38, staticRules[0x8D]), new Reduction(0x39, staticRules[0x8D]), new Reduction(0x3a, staticRules[0x8D]), new Reduction(0x3b, staticRules[0x8D]), new Reduction(0x3c, staticRules[0x8D]), new Reduction(0x3d, staticRules[0x8D]), new Reduction(0x3e, staticRules[0x8D]), new Reduction(0x3f, staticRules[0x8D]), new Reduction(0x41, staticRules[0x8D]), new Reduction(0x43, staticRules[0x8D]), new Reduction(0x44, staticRules[0x8D]), new Reduction(0x45, staticRules[0x8D]), new Reduction(0x46, staticRules[0x8D]), new Reduction(0x47, staticRules[0x8D]), new Reduction(0x48, staticRules[0x8D]), new Reduction(0x49, staticRules[0x8D]), new Reduction(0x4a, staticRules[0x8D]), new Reduction(0x4e, staticRules[0x8D]), new Reduction(0x55, staticRules[0x8D]), new Reduction(0x56, staticRules[0x8D]), new Reduction(0xdb, staticRules[0x8D])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_bin_terminal> → rule_sym_ref_simple •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8E]), new Reduction(0x11, staticRules[0x8E]), new Reduction(0x2e, staticRules[0x8E]), new Reduction(0x34, staticRules[0x8E]), new Reduction(0x35, staticRules[0x8E]), new Reduction(0x36, staticRules[0x8E]), new Reduction(0x37, staticRules[0x8E]), new Reduction(0x38, staticRules[0x8E]), new Reduction(0x39, staticRules[0x8E]), new Reduction(0x3a, staticRules[0x8E]), new Reduction(0x3b, staticRules[0x8E]), new Reduction(0x3c, staticRules[0x8E]), new Reduction(0x3d, staticRules[0x8E]), new Reduction(0x3e, staticRules[0x8E]), new Reduction(0x3f, staticRules[0x8E]), new Reduction(0x41, staticRules[0x8E]), new Reduction(0x43, staticRules[0x8E]), new Reduction(0x44, staticRules[0x8E]), new Reduction(0x45, staticRules[0x8E]), new Reduction(0x46, staticRules[0x8E]), new Reduction(0x47, staticRules[0x8E]), new Reduction(0x48, staticRules[0x8E]), new Reduction(0x49, staticRules[0x8E]), new Reduction(0x4a, staticRules[0x8E]), new Reduction(0x4e, staticRules[0x8E]), new Reduction(0x55, staticRules[0x8E]), new Reduction(0x56, staticRules[0x8E]), new Reduction(0xdb, staticRules[0x8E])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_bin_terminal> → rule_sym_ref_template<grammar_bin_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8F]), new Reduction(0x11, staticRules[0x8F]), new Reduction(0x2e, staticRules[0x8F]), new Reduction(0x34, staticRules[0x8F]), new Reduction(0x35, staticRules[0x8F]), new Reduction(0x36, staticRules[0x8F]), new Reduction(0x37, staticRules[0x8F]), new Reduction(0x38, staticRules[0x8F]), new Reduction(0x39, staticRules[0x8F]), new Reduction(0x3a, staticRules[0x8F]), new Reduction(0x3b, staticRules[0x8F]), new Reduction(0x3c, staticRules[0x8F]), new Reduction(0x3d, staticRules[0x8F]), new Reduction(0x3e, staticRules[0x8F]), new Reduction(0x3f, staticRules[0x8F]), new Reduction(0x41, staticRules[0x8F]), new Reduction(0x43, staticRules[0x8F]), new Reduction(0x44, staticRules[0x8F]), new Reduction(0x45, staticRules[0x8F]), new Reduction(0x46, staticRules[0x8F]), new Reduction(0x47, staticRules[0x8F]), new Reduction(0x48, staticRules[0x8F]), new Reduction(0x49, staticRules[0x8F]), new Reduction(0x4a, staticRules[0x8F]), new Reduction(0x4e, staticRules[0x8F]), new Reduction(0x55, staticRules[0x8F]), new Reduction(0x56, staticRules[0x8F]), new Reduction(0xdb, staticRules[0x8F])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_bin_terminal> → grammar_bin_terminal •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x90]), new Reduction(0x11, staticRules[0x90]), new Reduction(0x2e, staticRules[0x90]), new Reduction(0x34, staticRules[0x90]), new Reduction(0x35, staticRules[0x90]), new Reduction(0x36, staticRules[0x90]), new Reduction(0x37, staticRules[0x90]), new Reduction(0x38, staticRules[0x90]), new Reduction(0x39, staticRules[0x90]), new Reduction(0x3a, staticRules[0x90]), new Reduction(0x3b, staticRules[0x90]), new Reduction(0x3c, staticRules[0x90]), new Reduction(0x3d, staticRules[0x90]), new Reduction(0x3e, staticRules[0x90]), new Reduction(0x3f, staticRules[0x90]), new Reduction(0x41, staticRules[0x90]), new Reduction(0x43, staticRules[0x90]), new Reduction(0x44, staticRules[0x90]), new Reduction(0x45, staticRules[0x90]), new Reduction(0x46, staticRules[0x90]), new Reduction(0x47, staticRules[0x90]), new Reduction(0x48, staticRules[0x90]), new Reduction(0x49, staticRules[0x90]), new Reduction(0x4a, staticRules[0x90]), new Reduction(0x4e, staticRules[0x90]), new Reduction(0x55, staticRules[0x90]), new Reduction(0x56, staticRules[0x90]), new Reduction(0xdb, staticRules[0x90])})
            , new State(
               new string[1] {"[rule_sym_action → { • NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0xC9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_virtual → QUOTED_DATA •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[29] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[29] {new Reduction(0xa, staticRules[0x31]), new Reduction(0x11, staticRules[0x31]), new Reduction(0x2e, staticRules[0x31]), new Reduction(0x30, staticRules[0x31]), new Reduction(0x34, staticRules[0x31]), new Reduction(0x35, staticRules[0x31]), new Reduction(0x36, staticRules[0x31]), new Reduction(0x37, staticRules[0x31]), new Reduction(0x38, staticRules[0x31]), new Reduction(0x39, staticRules[0x31]), new Reduction(0x3a, staticRules[0x31]), new Reduction(0x3b, staticRules[0x31]), new Reduction(0x3c, staticRules[0x31]), new Reduction(0x3d, staticRules[0x31]), new Reduction(0x3e, staticRules[0x31]), new Reduction(0x3f, staticRules[0x31]), new Reduction(0x41, staticRules[0x31]), new Reduction(0x43, staticRules[0x31]), new Reduction(0x44, staticRules[0x31]), new Reduction(0x45, staticRules[0x31]), new Reduction(0x46, staticRules[0x31]), new Reduction(0x47, staticRules[0x31]), new Reduction(0x48, staticRules[0x31]), new Reduction(0x49, staticRules[0x31]), new Reduction(0x4a, staticRules[0x31]), new Reduction(0x4e, staticRules[0x31]), new Reduction(0x55, staticRules[0x31]), new Reduction(0x56, staticRules[0x31]), new Reduction(0xdb, staticRules[0x31])})
            , new State(
               new string[3] {"[rule_sym_ref_simple → NAME •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → NAME • rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_sym_ref_params<grammar_bin_terminal> → • < rule_def_atom<grammar_bin_terminal> _m175 >, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[29] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4d},
               new ushort[1] {0xCB},
               new ushort[1] {0x98},
               new ushort[1] {0xCA},
               new Reduction[28] {new Reduction(0xa, staticRules[0x32]), new Reduction(0x11, staticRules[0x32]), new Reduction(0x2e, staticRules[0x32]), new Reduction(0x34, staticRules[0x32]), new Reduction(0x35, staticRules[0x32]), new Reduction(0x36, staticRules[0x32]), new Reduction(0x37, staticRules[0x32]), new Reduction(0x38, staticRules[0x32]), new Reduction(0x39, staticRules[0x32]), new Reduction(0x3a, staticRules[0x32]), new Reduction(0x3b, staticRules[0x32]), new Reduction(0x3c, staticRules[0x32]), new Reduction(0x3d, staticRules[0x32]), new Reduction(0x3e, staticRules[0x32]), new Reduction(0x3f, staticRules[0x32]), new Reduction(0x41, staticRules[0x32]), new Reduction(0x43, staticRules[0x32]), new Reduction(0x44, staticRules[0x32]), new Reduction(0x45, staticRules[0x32]), new Reduction(0x46, staticRules[0x32]), new Reduction(0x47, staticRules[0x32]), new Reduction(0x48, staticRules[0x32]), new Reduction(0x49, staticRules[0x32]), new Reduction(0x4a, staticRules[0x32]), new Reduction(0x4e, staticRules[0x32]), new Reduction(0x55, staticRules[0x32]), new Reduction(0x56, staticRules[0x32]), new Reduction(0xdb, staticRules[0x32])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_UINT8 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x34]), new Reduction(0x11, staticRules[0x34]), new Reduction(0x2e, staticRules[0x34]), new Reduction(0x34, staticRules[0x34]), new Reduction(0x35, staticRules[0x34]), new Reduction(0x36, staticRules[0x34]), new Reduction(0x37, staticRules[0x34]), new Reduction(0x38, staticRules[0x34]), new Reduction(0x39, staticRules[0x34]), new Reduction(0x3a, staticRules[0x34]), new Reduction(0x3b, staticRules[0x34]), new Reduction(0x3c, staticRules[0x34]), new Reduction(0x3d, staticRules[0x34]), new Reduction(0x3e, staticRules[0x34]), new Reduction(0x3f, staticRules[0x34]), new Reduction(0x41, staticRules[0x34]), new Reduction(0x43, staticRules[0x34]), new Reduction(0x44, staticRules[0x34]), new Reduction(0x45, staticRules[0x34]), new Reduction(0x46, staticRules[0x34]), new Reduction(0x47, staticRules[0x34]), new Reduction(0x48, staticRules[0x34]), new Reduction(0x49, staticRules[0x34]), new Reduction(0x4a, staticRules[0x34]), new Reduction(0x4e, staticRules[0x34]), new Reduction(0x55, staticRules[0x34]), new Reduction(0x56, staticRules[0x34]), new Reduction(0xdb, staticRules[0x34])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_UINT16 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x35]), new Reduction(0x11, staticRules[0x35]), new Reduction(0x2e, staticRules[0x35]), new Reduction(0x34, staticRules[0x35]), new Reduction(0x35, staticRules[0x35]), new Reduction(0x36, staticRules[0x35]), new Reduction(0x37, staticRules[0x35]), new Reduction(0x38, staticRules[0x35]), new Reduction(0x39, staticRules[0x35]), new Reduction(0x3a, staticRules[0x35]), new Reduction(0x3b, staticRules[0x35]), new Reduction(0x3c, staticRules[0x35]), new Reduction(0x3d, staticRules[0x35]), new Reduction(0x3e, staticRules[0x35]), new Reduction(0x3f, staticRules[0x35]), new Reduction(0x41, staticRules[0x35]), new Reduction(0x43, staticRules[0x35]), new Reduction(0x44, staticRules[0x35]), new Reduction(0x45, staticRules[0x35]), new Reduction(0x46, staticRules[0x35]), new Reduction(0x47, staticRules[0x35]), new Reduction(0x48, staticRules[0x35]), new Reduction(0x49, staticRules[0x35]), new Reduction(0x4a, staticRules[0x35]), new Reduction(0x4e, staticRules[0x35]), new Reduction(0x55, staticRules[0x35]), new Reduction(0x56, staticRules[0x35]), new Reduction(0xdb, staticRules[0x35])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_UINT32 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x36]), new Reduction(0x11, staticRules[0x36]), new Reduction(0x2e, staticRules[0x36]), new Reduction(0x34, staticRules[0x36]), new Reduction(0x35, staticRules[0x36]), new Reduction(0x36, staticRules[0x36]), new Reduction(0x37, staticRules[0x36]), new Reduction(0x38, staticRules[0x36]), new Reduction(0x39, staticRules[0x36]), new Reduction(0x3a, staticRules[0x36]), new Reduction(0x3b, staticRules[0x36]), new Reduction(0x3c, staticRules[0x36]), new Reduction(0x3d, staticRules[0x36]), new Reduction(0x3e, staticRules[0x36]), new Reduction(0x3f, staticRules[0x36]), new Reduction(0x41, staticRules[0x36]), new Reduction(0x43, staticRules[0x36]), new Reduction(0x44, staticRules[0x36]), new Reduction(0x45, staticRules[0x36]), new Reduction(0x46, staticRules[0x36]), new Reduction(0x47, staticRules[0x36]), new Reduction(0x48, staticRules[0x36]), new Reduction(0x49, staticRules[0x36]), new Reduction(0x4a, staticRules[0x36]), new Reduction(0x4e, staticRules[0x36]), new Reduction(0x55, staticRules[0x36]), new Reduction(0x56, staticRules[0x36]), new Reduction(0xdb, staticRules[0x36])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_UINT64 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x37]), new Reduction(0x11, staticRules[0x37]), new Reduction(0x2e, staticRules[0x37]), new Reduction(0x34, staticRules[0x37]), new Reduction(0x35, staticRules[0x37]), new Reduction(0x36, staticRules[0x37]), new Reduction(0x37, staticRules[0x37]), new Reduction(0x38, staticRules[0x37]), new Reduction(0x39, staticRules[0x37]), new Reduction(0x3a, staticRules[0x37]), new Reduction(0x3b, staticRules[0x37]), new Reduction(0x3c, staticRules[0x37]), new Reduction(0x3d, staticRules[0x37]), new Reduction(0x3e, staticRules[0x37]), new Reduction(0x3f, staticRules[0x37]), new Reduction(0x41, staticRules[0x37]), new Reduction(0x43, staticRules[0x37]), new Reduction(0x44, staticRules[0x37]), new Reduction(0x45, staticRules[0x37]), new Reduction(0x46, staticRules[0x37]), new Reduction(0x47, staticRules[0x37]), new Reduction(0x48, staticRules[0x37]), new Reduction(0x49, staticRules[0x37]), new Reduction(0x4a, staticRules[0x37]), new Reduction(0x4e, staticRules[0x37]), new Reduction(0x55, staticRules[0x37]), new Reduction(0x56, staticRules[0x37]), new Reduction(0xdb, staticRules[0x37])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_UINT128 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x38]), new Reduction(0x11, staticRules[0x38]), new Reduction(0x2e, staticRules[0x38]), new Reduction(0x34, staticRules[0x38]), new Reduction(0x35, staticRules[0x38]), new Reduction(0x36, staticRules[0x38]), new Reduction(0x37, staticRules[0x38]), new Reduction(0x38, staticRules[0x38]), new Reduction(0x39, staticRules[0x38]), new Reduction(0x3a, staticRules[0x38]), new Reduction(0x3b, staticRules[0x38]), new Reduction(0x3c, staticRules[0x38]), new Reduction(0x3d, staticRules[0x38]), new Reduction(0x3e, staticRules[0x38]), new Reduction(0x3f, staticRules[0x38]), new Reduction(0x41, staticRules[0x38]), new Reduction(0x43, staticRules[0x38]), new Reduction(0x44, staticRules[0x38]), new Reduction(0x45, staticRules[0x38]), new Reduction(0x46, staticRules[0x38]), new Reduction(0x47, staticRules[0x38]), new Reduction(0x48, staticRules[0x38]), new Reduction(0x49, staticRules[0x38]), new Reduction(0x4a, staticRules[0x38]), new Reduction(0x4e, staticRules[0x38]), new Reduction(0x55, staticRules[0x38]), new Reduction(0x56, staticRules[0x38]), new Reduction(0xdb, staticRules[0x38])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_VALUE_BINARY •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x39]), new Reduction(0x11, staticRules[0x39]), new Reduction(0x2e, staticRules[0x39]), new Reduction(0x34, staticRules[0x39]), new Reduction(0x35, staticRules[0x39]), new Reduction(0x36, staticRules[0x39]), new Reduction(0x37, staticRules[0x39]), new Reduction(0x38, staticRules[0x39]), new Reduction(0x39, staticRules[0x39]), new Reduction(0x3a, staticRules[0x39]), new Reduction(0x3b, staticRules[0x39]), new Reduction(0x3c, staticRules[0x39]), new Reduction(0x3d, staticRules[0x39]), new Reduction(0x3e, staticRules[0x39]), new Reduction(0x3f, staticRules[0x39]), new Reduction(0x41, staticRules[0x39]), new Reduction(0x43, staticRules[0x39]), new Reduction(0x44, staticRules[0x39]), new Reduction(0x45, staticRules[0x39]), new Reduction(0x46, staticRules[0x39]), new Reduction(0x47, staticRules[0x39]), new Reduction(0x48, staticRules[0x39]), new Reduction(0x49, staticRules[0x39]), new Reduction(0x4a, staticRules[0x39]), new Reduction(0x4e, staticRules[0x39]), new Reduction(0x55, staticRules[0x39]), new Reduction(0x56, staticRules[0x39]), new Reduction(0xdb, staticRules[0x39])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_UINT8 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3A]), new Reduction(0x11, staticRules[0x3A]), new Reduction(0x2e, staticRules[0x3A]), new Reduction(0x34, staticRules[0x3A]), new Reduction(0x35, staticRules[0x3A]), new Reduction(0x36, staticRules[0x3A]), new Reduction(0x37, staticRules[0x3A]), new Reduction(0x38, staticRules[0x3A]), new Reduction(0x39, staticRules[0x3A]), new Reduction(0x3a, staticRules[0x3A]), new Reduction(0x3b, staticRules[0x3A]), new Reduction(0x3c, staticRules[0x3A]), new Reduction(0x3d, staticRules[0x3A]), new Reduction(0x3e, staticRules[0x3A]), new Reduction(0x3f, staticRules[0x3A]), new Reduction(0x41, staticRules[0x3A]), new Reduction(0x43, staticRules[0x3A]), new Reduction(0x44, staticRules[0x3A]), new Reduction(0x45, staticRules[0x3A]), new Reduction(0x46, staticRules[0x3A]), new Reduction(0x47, staticRules[0x3A]), new Reduction(0x48, staticRules[0x3A]), new Reduction(0x49, staticRules[0x3A]), new Reduction(0x4a, staticRules[0x3A]), new Reduction(0x4e, staticRules[0x3A]), new Reduction(0x55, staticRules[0x3A]), new Reduction(0x56, staticRules[0x3A]), new Reduction(0xdb, staticRules[0x3A])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_UINT16 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3B]), new Reduction(0x11, staticRules[0x3B]), new Reduction(0x2e, staticRules[0x3B]), new Reduction(0x34, staticRules[0x3B]), new Reduction(0x35, staticRules[0x3B]), new Reduction(0x36, staticRules[0x3B]), new Reduction(0x37, staticRules[0x3B]), new Reduction(0x38, staticRules[0x3B]), new Reduction(0x39, staticRules[0x3B]), new Reduction(0x3a, staticRules[0x3B]), new Reduction(0x3b, staticRules[0x3B]), new Reduction(0x3c, staticRules[0x3B]), new Reduction(0x3d, staticRules[0x3B]), new Reduction(0x3e, staticRules[0x3B]), new Reduction(0x3f, staticRules[0x3B]), new Reduction(0x41, staticRules[0x3B]), new Reduction(0x43, staticRules[0x3B]), new Reduction(0x44, staticRules[0x3B]), new Reduction(0x45, staticRules[0x3B]), new Reduction(0x46, staticRules[0x3B]), new Reduction(0x47, staticRules[0x3B]), new Reduction(0x48, staticRules[0x3B]), new Reduction(0x49, staticRules[0x3B]), new Reduction(0x4a, staticRules[0x3B]), new Reduction(0x4e, staticRules[0x3B]), new Reduction(0x55, staticRules[0x3B]), new Reduction(0x56, staticRules[0x3B]), new Reduction(0xdb, staticRules[0x3B])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_UINT32 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3C]), new Reduction(0x11, staticRules[0x3C]), new Reduction(0x2e, staticRules[0x3C]), new Reduction(0x34, staticRules[0x3C]), new Reduction(0x35, staticRules[0x3C]), new Reduction(0x36, staticRules[0x3C]), new Reduction(0x37, staticRules[0x3C]), new Reduction(0x38, staticRules[0x3C]), new Reduction(0x39, staticRules[0x3C]), new Reduction(0x3a, staticRules[0x3C]), new Reduction(0x3b, staticRules[0x3C]), new Reduction(0x3c, staticRules[0x3C]), new Reduction(0x3d, staticRules[0x3C]), new Reduction(0x3e, staticRules[0x3C]), new Reduction(0x3f, staticRules[0x3C]), new Reduction(0x41, staticRules[0x3C]), new Reduction(0x43, staticRules[0x3C]), new Reduction(0x44, staticRules[0x3C]), new Reduction(0x45, staticRules[0x3C]), new Reduction(0x46, staticRules[0x3C]), new Reduction(0x47, staticRules[0x3C]), new Reduction(0x48, staticRules[0x3C]), new Reduction(0x49, staticRules[0x3C]), new Reduction(0x4a, staticRules[0x3C]), new Reduction(0x4e, staticRules[0x3C]), new Reduction(0x55, staticRules[0x3C]), new Reduction(0x56, staticRules[0x3C]), new Reduction(0xdb, staticRules[0x3C])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_UINT64 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3D]), new Reduction(0x11, staticRules[0x3D]), new Reduction(0x2e, staticRules[0x3D]), new Reduction(0x34, staticRules[0x3D]), new Reduction(0x35, staticRules[0x3D]), new Reduction(0x36, staticRules[0x3D]), new Reduction(0x37, staticRules[0x3D]), new Reduction(0x38, staticRules[0x3D]), new Reduction(0x39, staticRules[0x3D]), new Reduction(0x3a, staticRules[0x3D]), new Reduction(0x3b, staticRules[0x3D]), new Reduction(0x3c, staticRules[0x3D]), new Reduction(0x3d, staticRules[0x3D]), new Reduction(0x3e, staticRules[0x3D]), new Reduction(0x3f, staticRules[0x3D]), new Reduction(0x41, staticRules[0x3D]), new Reduction(0x43, staticRules[0x3D]), new Reduction(0x44, staticRules[0x3D]), new Reduction(0x45, staticRules[0x3D]), new Reduction(0x46, staticRules[0x3D]), new Reduction(0x47, staticRules[0x3D]), new Reduction(0x48, staticRules[0x3D]), new Reduction(0x49, staticRules[0x3D]), new Reduction(0x4a, staticRules[0x3D]), new Reduction(0x4e, staticRules[0x3D]), new Reduction(0x55, staticRules[0x3D]), new Reduction(0x56, staticRules[0x3D]), new Reduction(0xdb, staticRules[0x3D])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_UINT128 •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3E]), new Reduction(0x11, staticRules[0x3E]), new Reduction(0x2e, staticRules[0x3E]), new Reduction(0x34, staticRules[0x3E]), new Reduction(0x35, staticRules[0x3E]), new Reduction(0x36, staticRules[0x3E]), new Reduction(0x37, staticRules[0x3E]), new Reduction(0x38, staticRules[0x3E]), new Reduction(0x39, staticRules[0x3E]), new Reduction(0x3a, staticRules[0x3E]), new Reduction(0x3b, staticRules[0x3E]), new Reduction(0x3c, staticRules[0x3E]), new Reduction(0x3d, staticRules[0x3E]), new Reduction(0x3e, staticRules[0x3E]), new Reduction(0x3f, staticRules[0x3E]), new Reduction(0x41, staticRules[0x3E]), new Reduction(0x43, staticRules[0x3E]), new Reduction(0x44, staticRules[0x3E]), new Reduction(0x45, staticRules[0x3E]), new Reduction(0x46, staticRules[0x3E]), new Reduction(0x47, staticRules[0x3E]), new Reduction(0x48, staticRules[0x3E]), new Reduction(0x49, staticRules[0x3E]), new Reduction(0x4a, staticRules[0x3E]), new Reduction(0x4e, staticRules[0x3E]), new Reduction(0x55, staticRules[0x3E]), new Reduction(0x56, staticRules[0x3E]), new Reduction(0xdb, staticRules[0x3E])})
            , new State(
               new string[1] {"[grammar_bin_terminal → SYMBOL_JOKER_BINARY •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3F]), new Reduction(0x11, staticRules[0x3F]), new Reduction(0x2e, staticRules[0x3F]), new Reduction(0x34, staticRules[0x3F]), new Reduction(0x35, staticRules[0x3F]), new Reduction(0x36, staticRules[0x3F]), new Reduction(0x37, staticRules[0x3F]), new Reduction(0x38, staticRules[0x3F]), new Reduction(0x39, staticRules[0x3F]), new Reduction(0x3a, staticRules[0x3F]), new Reduction(0x3b, staticRules[0x3F]), new Reduction(0x3c, staticRules[0x3F]), new Reduction(0x3d, staticRules[0x3F]), new Reduction(0x3e, staticRules[0x3F]), new Reduction(0x3f, staticRules[0x3F]), new Reduction(0x41, staticRules[0x3F]), new Reduction(0x43, staticRules[0x3F]), new Reduction(0x44, staticRules[0x3F]), new Reduction(0x45, staticRules[0x3F]), new Reduction(0x46, staticRules[0x3F]), new Reduction(0x47, staticRules[0x3F]), new Reduction(0x48, staticRules[0x3F]), new Reduction(0x49, staticRules[0x3F]), new Reduction(0x4a, staticRules[0x3F]), new Reduction(0x4e, staticRules[0x3F]), new Reduction(0x55, staticRules[0x3F]), new Reduction(0x56, staticRules[0x3F]), new Reduction(0xdb, staticRules[0x3F])})
            , new State(
               new string[1] {"[option → NAME = QUOTED_DATA • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0xCC},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_text_terminal> → rules { _m152 • }, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0xCD},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[6] {"[_m152 → cf_rule_simple<grammar_text_terminal> • _m152, }]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152, }]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152, }]", "[_m152 → •, }]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;, NAME/}]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0xCE, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               new string[6] {"[_m152 → cf_rule_template<grammar_text_terminal> • _m152, }]", "[_m152 → • cf_rule_simple<grammar_text_terminal> _m152, }]", "[_m152 → • cf_rule_template<grammar_text_terminal> _m152, }]", "[_m152 → •, }]", "[cf_rule_simple<grammar_text_terminal> → • NAME -> rule_definition<grammar_text_terminal> ;, NAME/}]", "[cf_rule_template<grammar_text_terminal> → • NAME rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0xCF, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               new string[3] {"[cf_rule_simple<grammar_text_terminal> → NAME • -> rule_definition<grammar_text_terminal> ;, NAME/}]", "[cf_rule_template<grammar_text_terminal> → NAME • rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}]", "[rule_template_params → • < NAME _m101 >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0xD0, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0xD1},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_bin_terminal> → rules { _m193 } •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7C])})
            , new State(
               new string[1] {"[_m193 → cf_rule_simple<grammar_bin_terminal> _m193 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9C])})
            , new State(
               new string[1] {"[_m193 → cf_rule_template<grammar_bin_terminal> _m193 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9D])})
            , new State(
               new string[36] {"[cf_rule_simple<grammar_bin_terminal> → NAME -> • rule_definition<grammar_bin_terminal> ;, NAME/}]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, ;]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, ;/|]", "[rule_def_choice<grammar_bin_terminal> → •, ;/|]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, ;/|]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xD2, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               new string[1] {"[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params • -> rule_definition<grammar_bin_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[1] {0x4c},
               new ushort[1] {0xD3},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_template_params → < • NAME _m101 >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0xD4},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[grammar_cs_rules<grammar_text_terminal> → rules { _m160 } •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA1])})
            , new State(
               new string[1] {"[_m160 → cs_rule_simple<grammar_text_terminal> _m160 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA6])})
            , new State(
               new string[1] {"[_m160 → cs_rule_template<grammar_text_terminal> _m160 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA7])})
            , new State(
               new string[4] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME • cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME • cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_context<grammar_text_terminal> → • [ rule_definition<grammar_text_terminal> ], ->/<]", "[cs_rule_context<grammar_text_terminal> → •, ->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[1] {0xe0},
               new ushort[1] {0xD5},
               new Reduction[2] {new Reduction(0x4c, staticRules[0xA4]), new Reduction(0x4d, staticRules[0xA4])})
            , new State(
               new string[1] {"[cs_rule_context<grammar_text_terminal> → [ rule_definition<grammar_text_terminal> • ], NAME/->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0xdb},
               new ushort[1] {0xD6},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[rule_definition<grammar_text_terminal> → rule_def_choice<grammar_text_terminal> • _m147, ;/)/]]", "[_m147 → • | rule_def_choice<grammar_text_terminal> _m147, ;/)/]]", "[_m147 → •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4a},
               new ushort[1] {0xD8},
               new ushort[1] {0x8a},
               new ushort[1] {0xD7},
               new Reduction[3] {new Reduction(0x41, staticRules[0x77]), new Reduction(0x44, staticRules[0x77]), new Reduction(0xdb, staticRules[0x77])})
            , new State(
               new string[1] {"[rule_def_choice<grammar_text_terminal> → rule_def_restrict<grammar_text_terminal> •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5C]), new Reduction(0x44, staticRules[0x5C]), new Reduction(0x4a, staticRules[0x5C]), new Reduction(0xdb, staticRules[0x5C])})
            , new State(
               new string[3] {"[rule_def_restrict<grammar_text_terminal> → rule_def_fragment<grammar_text_terminal> • _m145, ;/)/|/]]", "[_m145 → • - rule_def_fragment<grammar_text_terminal> _m145, ;/)/|/]]", "[_m145 → •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x49},
               new ushort[1] {0xDA},
               new ushort[1] {0x89},
               new ushort[1] {0xD9},
               new Reduction[4] {new Reduction(0x41, staticRules[0x75]), new Reduction(0x44, staticRules[0x75]), new Reduction(0x4a, staticRules[0x75]), new Reduction(0xdb, staticRules[0x75])})
            , new State(
               new string[23] {"[rule_def_fragment<grammar_text_terminal> → rule_def_repetition<grammar_text_terminal> • _m143, ;/)/-/|/]]", "[_m143 → • rule_def_repetition<grammar_text_terminal> _m143, ;/)/-/|/]]", "[_m143 → •, ;/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[10] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x88, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0xDB, 0xDC, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[5] {new Reduction(0x41, staticRules[0x73]), new Reduction(0x44, staticRules[0x73]), new Reduction(0x49, staticRules[0x73]), new Reduction(0x4a, staticRules[0x73]), new Reduction(0xdb, staticRules[0x73])})
            , new State(
               new string[4] {"[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> • ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[13] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xDD, 0xDE, 0xDF},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x63]), new Reduction(0x11, staticRules[0x63]), new Reduction(0x2e, staticRules[0x63]), new Reduction(0x30, staticRules[0x63]), new Reduction(0x41, staticRules[0x63]), new Reduction(0x43, staticRules[0x63]), new Reduction(0x44, staticRules[0x63]), new Reduction(0x49, staticRules[0x63]), new Reduction(0x4a, staticRules[0x63]), new Reduction(0xdb, staticRules[0x63])})
            , new State(
               new string[3] {"[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> • ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> • !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[15] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xE0, 0xE1},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x66]), new Reduction(0x11, staticRules[0x66]), new Reduction(0x2e, staticRules[0x66]), new Reduction(0x30, staticRules[0x66]), new Reduction(0x41, staticRules[0x66]), new Reduction(0x43, staticRules[0x66]), new Reduction(0x44, staticRules[0x66]), new Reduction(0x45, staticRules[0x66]), new Reduction(0x46, staticRules[0x66]), new Reduction(0x47, staticRules[0x66]), new Reduction(0x49, staticRules[0x66]), new Reduction(0x4a, staticRules[0x66]), new Reduction(0xdb, staticRules[0x66])})
            , new State(
               new string[1] {"[rule_def_element<grammar_text_terminal> → rule_def_atom<grammar_text_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[15] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[15] {new Reduction(0xa, staticRules[0x67]), new Reduction(0x11, staticRules[0x67]), new Reduction(0x2e, staticRules[0x67]), new Reduction(0x30, staticRules[0x67]), new Reduction(0x41, staticRules[0x67]), new Reduction(0x43, staticRules[0x67]), new Reduction(0x44, staticRules[0x67]), new Reduction(0x45, staticRules[0x67]), new Reduction(0x46, staticRules[0x67]), new Reduction(0x47, staticRules[0x67]), new Reduction(0x49, staticRules[0x67]), new Reduction(0x4a, staticRules[0x67]), new Reduction(0x55, staticRules[0x67]), new Reduction(0x56, staticRules[0x67]), new Reduction(0xdb, staticRules[0x67])})
            , new State(
               new string[26] {"[rule_def_element<grammar_text_terminal> → ( • rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, )]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, )/|]", "[rule_def_choice<grammar_text_terminal> → •, )/|]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, )/|]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, )/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/(/)/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0xE2, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x44, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_text_terminal> → rule_sym_action •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x69]), new Reduction(0x11, staticRules[0x69]), new Reduction(0x2e, staticRules[0x69]), new Reduction(0x30, staticRules[0x69]), new Reduction(0x41, staticRules[0x69]), new Reduction(0x43, staticRules[0x69]), new Reduction(0x44, staticRules[0x69]), new Reduction(0x45, staticRules[0x69]), new Reduction(0x46, staticRules[0x69]), new Reduction(0x47, staticRules[0x69]), new Reduction(0x48, staticRules[0x69]), new Reduction(0x49, staticRules[0x69]), new Reduction(0x4a, staticRules[0x69]), new Reduction(0x4e, staticRules[0x69]), new Reduction(0x55, staticRules[0x69]), new Reduction(0x56, staticRules[0x69]), new Reduction(0xdb, staticRules[0x69])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_text_terminal> → rule_sym_virtual •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6A]), new Reduction(0x11, staticRules[0x6A]), new Reduction(0x2e, staticRules[0x6A]), new Reduction(0x30, staticRules[0x6A]), new Reduction(0x41, staticRules[0x6A]), new Reduction(0x43, staticRules[0x6A]), new Reduction(0x44, staticRules[0x6A]), new Reduction(0x45, staticRules[0x6A]), new Reduction(0x46, staticRules[0x6A]), new Reduction(0x47, staticRules[0x6A]), new Reduction(0x48, staticRules[0x6A]), new Reduction(0x49, staticRules[0x6A]), new Reduction(0x4a, staticRules[0x6A]), new Reduction(0x4e, staticRules[0x6A]), new Reduction(0x55, staticRules[0x6A]), new Reduction(0x56, staticRules[0x6A]), new Reduction(0xdb, staticRules[0x6A])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_text_terminal> → rule_sym_ref_simple •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6B]), new Reduction(0x11, staticRules[0x6B]), new Reduction(0x2e, staticRules[0x6B]), new Reduction(0x30, staticRules[0x6B]), new Reduction(0x41, staticRules[0x6B]), new Reduction(0x43, staticRules[0x6B]), new Reduction(0x44, staticRules[0x6B]), new Reduction(0x45, staticRules[0x6B]), new Reduction(0x46, staticRules[0x6B]), new Reduction(0x47, staticRules[0x6B]), new Reduction(0x48, staticRules[0x6B]), new Reduction(0x49, staticRules[0x6B]), new Reduction(0x4a, staticRules[0x6B]), new Reduction(0x4e, staticRules[0x6B]), new Reduction(0x55, staticRules[0x6B]), new Reduction(0x56, staticRules[0x6B]), new Reduction(0xdb, staticRules[0x6B])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_text_terminal> → rule_sym_ref_template<grammar_text_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6C]), new Reduction(0x11, staticRules[0x6C]), new Reduction(0x2e, staticRules[0x6C]), new Reduction(0x30, staticRules[0x6C]), new Reduction(0x41, staticRules[0x6C]), new Reduction(0x43, staticRules[0x6C]), new Reduction(0x44, staticRules[0x6C]), new Reduction(0x45, staticRules[0x6C]), new Reduction(0x46, staticRules[0x6C]), new Reduction(0x47, staticRules[0x6C]), new Reduction(0x48, staticRules[0x6C]), new Reduction(0x49, staticRules[0x6C]), new Reduction(0x4a, staticRules[0x6C]), new Reduction(0x4e, staticRules[0x6C]), new Reduction(0x55, staticRules[0x6C]), new Reduction(0x56, staticRules[0x6C]), new Reduction(0xdb, staticRules[0x6C])})
            , new State(
               new string[1] {"[rule_def_atom<grammar_text_terminal> → grammar_text_terminal •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6D]), new Reduction(0x11, staticRules[0x6D]), new Reduction(0x2e, staticRules[0x6D]), new Reduction(0x30, staticRules[0x6D]), new Reduction(0x41, staticRules[0x6D]), new Reduction(0x43, staticRules[0x6D]), new Reduction(0x44, staticRules[0x6D]), new Reduction(0x45, staticRules[0x6D]), new Reduction(0x46, staticRules[0x6D]), new Reduction(0x47, staticRules[0x6D]), new Reduction(0x48, staticRules[0x6D]), new Reduction(0x49, staticRules[0x6D]), new Reduction(0x4a, staticRules[0x6D]), new Reduction(0x4e, staticRules[0x6D]), new Reduction(0x55, staticRules[0x6D]), new Reduction(0x56, staticRules[0x6D]), new Reduction(0xdb, staticRules[0x6D])})
            , new State(
               new string[3] {"[rule_sym_ref_simple → NAME •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → NAME • rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_sym_ref_params<grammar_text_terminal> → • < rule_def_atom<grammar_text_terminal> _m134 >, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4d},
               new ushort[1] {0xE4},
               new ushort[1] {0x86},
               new ushort[1] {0xE3},
               new Reduction[17] {new Reduction(0xa, staticRules[0x32]), new Reduction(0x11, staticRules[0x32]), new Reduction(0x2e, staticRules[0x32]), new Reduction(0x30, staticRules[0x32]), new Reduction(0x41, staticRules[0x32]), new Reduction(0x43, staticRules[0x32]), new Reduction(0x44, staticRules[0x32]), new Reduction(0x45, staticRules[0x32]), new Reduction(0x46, staticRules[0x32]), new Reduction(0x47, staticRules[0x32]), new Reduction(0x48, staticRules[0x32]), new Reduction(0x49, staticRules[0x32]), new Reduction(0x4a, staticRules[0x32]), new Reduction(0x4e, staticRules[0x32]), new Reduction(0x55, staticRules[0x32]), new Reduction(0x56, staticRules[0x32]), new Reduction(0xdb, staticRules[0x32])})
            , new State(
               new string[1] {"[grammar_text_terminal → terminal_def_atom_text •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x40]), new Reduction(0x11, staticRules[0x40]), new Reduction(0x2e, staticRules[0x40]), new Reduction(0x30, staticRules[0x40]), new Reduction(0x41, staticRules[0x40]), new Reduction(0x43, staticRules[0x40]), new Reduction(0x44, staticRules[0x40]), new Reduction(0x45, staticRules[0x40]), new Reduction(0x46, staticRules[0x40]), new Reduction(0x47, staticRules[0x40]), new Reduction(0x48, staticRules[0x40]), new Reduction(0x49, staticRules[0x40]), new Reduction(0x4a, staticRules[0x40]), new Reduction(0x4e, staticRules[0x40]), new Reduction(0x55, staticRules[0x40]), new Reduction(0x56, staticRules[0x40]), new Reduction(0xdb, staticRules[0x40])})
            , new State(
               new string[1] {"[terminal_def_atom_text → SYMBOL_TERMINAL_TEXT •, NAME/./{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/,/-/|/=>/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[24] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x14]), new Reduction(0xb, staticRules[0x14]), new Reduction(0x11, staticRules[0x14]), new Reduction(0x2e, staticRules[0x14]), new Reduction(0x30, staticRules[0x14]), new Reduction(0x31, staticRules[0x14]), new Reduction(0x32, staticRules[0x14]), new Reduction(0x33, staticRules[0x14]), new Reduction(0x34, staticRules[0x14]), new Reduction(0x35, staticRules[0x14]), new Reduction(0x41, staticRules[0x14]), new Reduction(0x43, staticRules[0x14]), new Reduction(0x44, staticRules[0x14]), new Reduction(0x45, staticRules[0x14]), new Reduction(0x46, staticRules[0x14]), new Reduction(0x47, staticRules[0x14]), new Reduction(0x48, staticRules[0x14]), new Reduction(0x49, staticRules[0x14]), new Reduction(0x4a, staticRules[0x14]), new Reduction(0x4b, staticRules[0x14]), new Reduction(0x4e, staticRules[0x14]), new Reduction(0x55, staticRules[0x14]), new Reduction(0x56, staticRules[0x14]), new Reduction(0xdb, staticRules[0x14])})
            , new State(
               new string[3] {"[terminal → NAME -> terminal_definition • terminal_subgrammar ;, NAME/}]", "[terminal_subgrammar → • => qualified_name, ;]", "[terminal_subgrammar → •, ;]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x4b},
               new ushort[1] {0xE6},
               new ushort[1] {0x66},
               new ushort[1] {0xE5},
               new Reduction[1] {new Reduction(0x41, staticRules[0x2E])})
            , new State(
               new string[3] {"[terminal_definition → terminal_def_restrict • _m93, ;/)/=>]", "[_m93 → • | terminal_def_restrict _m93, ;/)/=>]", "[_m93 → •, ;/)/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x4a},
               new ushort[1] {0xE8},
               new ushort[1] {0x76},
               new ushort[1] {0xE7},
               new Reduction[3] {new Reduction(0x41, staticRules[0x50]), new Reduction(0x44, staticRules[0x50]), new Reduction(0x4b, staticRules[0x50])})
            , new State(
               new string[3] {"[terminal_def_restrict → terminal_def_fragment • _m91, ;/)/|/=>]", "[_m91 → • - terminal_def_fragment _m91, ;/)/|/=>]", "[_m91 → •, ;/)/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x49},
               new ushort[1] {0xEA},
               new ushort[1] {0x75},
               new ushort[1] {0xE9},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4E]), new Reduction(0x44, staticRules[0x4E]), new Reduction(0x4a, staticRules[0x4E]), new Reduction(0x4b, staticRules[0x4E])})
            , new State(
               new string[23] {"[terminal_def_fragment → terminal_def_repetition • _m89, ;/)/-/|/=>]", "[_m89 → • terminal_def_repetition _m89, ;/)/-/|/=>]", "[_m89 → •, ;/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x74, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0xEB, 0xEC, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4C]), new Reduction(0x44, staticRules[0x4C]), new Reduction(0x49, staticRules[0x4C]), new Reduction(0x4a, staticRules[0x4C]), new Reduction(0x4b, staticRules[0x4C])})
            , new State(
               new string[7] {"[terminal_def_repetition → terminal_def_element • terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_repetition → terminal_def_element •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → • *, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → • +, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → • ?, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → • { INTEGER , INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → • { INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[4] {0x45, 0x46, 0x47, 0x11},
               new ushort[4] {0xEE, 0xEF, 0xF0, 0xF1},
               new ushort[1] {0x61},
               new ushort[1] {0xED},
               new Reduction[14] {new Reduction(0xa, staticRules[0x29]), new Reduction(0xb, staticRules[0x29]), new Reduction(0x30, staticRules[0x29]), new Reduction(0x31, staticRules[0x29]), new Reduction(0x32, staticRules[0x29]), new Reduction(0x33, staticRules[0x29]), new Reduction(0x34, staticRules[0x29]), new Reduction(0x35, staticRules[0x29]), new Reduction(0x41, staticRules[0x29]), new Reduction(0x43, staticRules[0x29]), new Reduction(0x44, staticRules[0x29]), new Reduction(0x49, staticRules[0x29]), new Reduction(0x4a, staticRules[0x29]), new Reduction(0x4b, staticRules[0x29])})
            , new State(
               new string[1] {"[terminal_def_element → terminal_def_atom •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x21]), new Reduction(0xb, staticRules[0x21]), new Reduction(0x11, staticRules[0x21]), new Reduction(0x30, staticRules[0x21]), new Reduction(0x31, staticRules[0x21]), new Reduction(0x32, staticRules[0x21]), new Reduction(0x33, staticRules[0x21]), new Reduction(0x34, staticRules[0x21]), new Reduction(0x35, staticRules[0x21]), new Reduction(0x41, staticRules[0x21]), new Reduction(0x43, staticRules[0x21]), new Reduction(0x44, staticRules[0x21]), new Reduction(0x45, staticRules[0x21]), new Reduction(0x46, staticRules[0x21]), new Reduction(0x47, staticRules[0x21]), new Reduction(0x49, staticRules[0x21]), new Reduction(0x4a, staticRules[0x21]), new Reduction(0x4b, staticRules[0x21])})
            , new State(
               new string[24] {"[terminal_def_element → ( • terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_definition → • terminal_def_restrict _m93, )]", "[terminal_def_restrict → • terminal_def_fragment _m91, )/|]", "[terminal_def_fragment → • terminal_def_repetition _m89, )/-/|]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/-/|]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/-/|]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/../(/)/*/+/?/-/|]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/../(/)/*/+/?/-/|]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/(/)/*/+/?/-/|]"},
               new Hime.Redist.Parsers.SymbolTerminal[9] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[13] {0x65, 0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[13] {0xF2, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_any •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x19]), new Reduction(0xb, staticRules[0x19]), new Reduction(0x11, staticRules[0x19]), new Reduction(0x30, staticRules[0x19]), new Reduction(0x31, staticRules[0x19]), new Reduction(0x32, staticRules[0x19]), new Reduction(0x33, staticRules[0x19]), new Reduction(0x34, staticRules[0x19]), new Reduction(0x35, staticRules[0x19]), new Reduction(0x41, staticRules[0x19]), new Reduction(0x43, staticRules[0x19]), new Reduction(0x44, staticRules[0x19]), new Reduction(0x45, staticRules[0x19]), new Reduction(0x46, staticRules[0x19]), new Reduction(0x47, staticRules[0x19]), new Reduction(0x49, staticRules[0x19]), new Reduction(0x4a, staticRules[0x19]), new Reduction(0x4b, staticRules[0x19])})
            , new State(
               new string[2] {"[terminal_def_atom → terminal_def_atom_unicode •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_span → terminal_def_atom_unicode • .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[19] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[..]", 0x42), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x42},
               new ushort[1] {0xF3},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1A]), new Reduction(0xb, staticRules[0x1A]), new Reduction(0x11, staticRules[0x1A]), new Reduction(0x30, staticRules[0x1A]), new Reduction(0x31, staticRules[0x1A]), new Reduction(0x32, staticRules[0x1A]), new Reduction(0x33, staticRules[0x1A]), new Reduction(0x34, staticRules[0x1A]), new Reduction(0x35, staticRules[0x1A]), new Reduction(0x41, staticRules[0x1A]), new Reduction(0x43, staticRules[0x1A]), new Reduction(0x44, staticRules[0x1A]), new Reduction(0x45, staticRules[0x1A]), new Reduction(0x46, staticRules[0x1A]), new Reduction(0x47, staticRules[0x1A]), new Reduction(0x49, staticRules[0x1A]), new Reduction(0x4a, staticRules[0x1A]), new Reduction(0x4b, staticRules[0x1A])})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_text •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1B]), new Reduction(0xb, staticRules[0x1B]), new Reduction(0x11, staticRules[0x1B]), new Reduction(0x30, staticRules[0x1B]), new Reduction(0x31, staticRules[0x1B]), new Reduction(0x32, staticRules[0x1B]), new Reduction(0x33, staticRules[0x1B]), new Reduction(0x34, staticRules[0x1B]), new Reduction(0x35, staticRules[0x1B]), new Reduction(0x41, staticRules[0x1B]), new Reduction(0x43, staticRules[0x1B]), new Reduction(0x44, staticRules[0x1B]), new Reduction(0x45, staticRules[0x1B]), new Reduction(0x46, staticRules[0x1B]), new Reduction(0x47, staticRules[0x1B]), new Reduction(0x49, staticRules[0x1B]), new Reduction(0x4a, staticRules[0x1B]), new Reduction(0x4b, staticRules[0x1B])})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_set •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1C]), new Reduction(0xb, staticRules[0x1C]), new Reduction(0x11, staticRules[0x1C]), new Reduction(0x30, staticRules[0x1C]), new Reduction(0x31, staticRules[0x1C]), new Reduction(0x32, staticRules[0x1C]), new Reduction(0x33, staticRules[0x1C]), new Reduction(0x34, staticRules[0x1C]), new Reduction(0x35, staticRules[0x1C]), new Reduction(0x41, staticRules[0x1C]), new Reduction(0x43, staticRules[0x1C]), new Reduction(0x44, staticRules[0x1C]), new Reduction(0x45, staticRules[0x1C]), new Reduction(0x46, staticRules[0x1C]), new Reduction(0x47, staticRules[0x1C]), new Reduction(0x49, staticRules[0x1C]), new Reduction(0x4a, staticRules[0x1C]), new Reduction(0x4b, staticRules[0x1C])})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_span •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1D]), new Reduction(0xb, staticRules[0x1D]), new Reduction(0x11, staticRules[0x1D]), new Reduction(0x30, staticRules[0x1D]), new Reduction(0x31, staticRules[0x1D]), new Reduction(0x32, staticRules[0x1D]), new Reduction(0x33, staticRules[0x1D]), new Reduction(0x34, staticRules[0x1D]), new Reduction(0x35, staticRules[0x1D]), new Reduction(0x41, staticRules[0x1D]), new Reduction(0x43, staticRules[0x1D]), new Reduction(0x44, staticRules[0x1D]), new Reduction(0x45, staticRules[0x1D]), new Reduction(0x46, staticRules[0x1D]), new Reduction(0x47, staticRules[0x1D]), new Reduction(0x49, staticRules[0x1D]), new Reduction(0x4a, staticRules[0x1D]), new Reduction(0x4b, staticRules[0x1D])})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_ucat •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1E]), new Reduction(0xb, staticRules[0x1E]), new Reduction(0x11, staticRules[0x1E]), new Reduction(0x30, staticRules[0x1E]), new Reduction(0x31, staticRules[0x1E]), new Reduction(0x32, staticRules[0x1E]), new Reduction(0x33, staticRules[0x1E]), new Reduction(0x34, staticRules[0x1E]), new Reduction(0x35, staticRules[0x1E]), new Reduction(0x41, staticRules[0x1E]), new Reduction(0x43, staticRules[0x1E]), new Reduction(0x44, staticRules[0x1E]), new Reduction(0x45, staticRules[0x1E]), new Reduction(0x46, staticRules[0x1E]), new Reduction(0x47, staticRules[0x1E]), new Reduction(0x49, staticRules[0x1E]), new Reduction(0x4a, staticRules[0x1E]), new Reduction(0x4b, staticRules[0x1E])})
            , new State(
               new string[1] {"[terminal_def_atom → terminal_def_atom_ublock •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1F]), new Reduction(0xb, staticRules[0x1F]), new Reduction(0x11, staticRules[0x1F]), new Reduction(0x30, staticRules[0x1F]), new Reduction(0x31, staticRules[0x1F]), new Reduction(0x32, staticRules[0x1F]), new Reduction(0x33, staticRules[0x1F]), new Reduction(0x34, staticRules[0x1F]), new Reduction(0x35, staticRules[0x1F]), new Reduction(0x41, staticRules[0x1F]), new Reduction(0x43, staticRules[0x1F]), new Reduction(0x44, staticRules[0x1F]), new Reduction(0x45, staticRules[0x1F]), new Reduction(0x46, staticRules[0x1F]), new Reduction(0x47, staticRules[0x1F]), new Reduction(0x49, staticRules[0x1F]), new Reduction(0x4a, staticRules[0x1F]), new Reduction(0x4b, staticRules[0x1F])})
            , new State(
               new string[1] {"[terminal_def_atom → NAME •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x20]), new Reduction(0xb, staticRules[0x20]), new Reduction(0x11, staticRules[0x20]), new Reduction(0x30, staticRules[0x20]), new Reduction(0x31, staticRules[0x20]), new Reduction(0x32, staticRules[0x20]), new Reduction(0x33, staticRules[0x20]), new Reduction(0x34, staticRules[0x20]), new Reduction(0x35, staticRules[0x20]), new Reduction(0x41, staticRules[0x20]), new Reduction(0x43, staticRules[0x20]), new Reduction(0x44, staticRules[0x20]), new Reduction(0x45, staticRules[0x20]), new Reduction(0x46, staticRules[0x20]), new Reduction(0x47, staticRules[0x20]), new Reduction(0x49, staticRules[0x20]), new Reduction(0x4a, staticRules[0x20]), new Reduction(0x4b, staticRules[0x20])})
            , new State(
               new string[1] {"[terminal_def_atom_any → . •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x11]), new Reduction(0xb, staticRules[0x11]), new Reduction(0x11, staticRules[0x11]), new Reduction(0x30, staticRules[0x11]), new Reduction(0x31, staticRules[0x11]), new Reduction(0x32, staticRules[0x11]), new Reduction(0x33, staticRules[0x11]), new Reduction(0x34, staticRules[0x11]), new Reduction(0x35, staticRules[0x11]), new Reduction(0x41, staticRules[0x11]), new Reduction(0x43, staticRules[0x11]), new Reduction(0x44, staticRules[0x11]), new Reduction(0x45, staticRules[0x11]), new Reduction(0x46, staticRules[0x11]), new Reduction(0x47, staticRules[0x11]), new Reduction(0x49, staticRules[0x11]), new Reduction(0x4a, staticRules[0x11]), new Reduction(0x4b, staticRules[0x11])})
            , new State(
               new string[1] {"[terminal_def_atom_unicode → SYMBOL_VALUE_UINT8 •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[19] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[..]", 0x42), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[19] {new Reduction(0xa, staticRules[0x12]), new Reduction(0xb, staticRules[0x12]), new Reduction(0x11, staticRules[0x12]), new Reduction(0x30, staticRules[0x12]), new Reduction(0x31, staticRules[0x12]), new Reduction(0x32, staticRules[0x12]), new Reduction(0x33, staticRules[0x12]), new Reduction(0x34, staticRules[0x12]), new Reduction(0x35, staticRules[0x12]), new Reduction(0x41, staticRules[0x12]), new Reduction(0x42, staticRules[0x12]), new Reduction(0x43, staticRules[0x12]), new Reduction(0x44, staticRules[0x12]), new Reduction(0x45, staticRules[0x12]), new Reduction(0x46, staticRules[0x12]), new Reduction(0x47, staticRules[0x12]), new Reduction(0x49, staticRules[0x12]), new Reduction(0x4a, staticRules[0x12]), new Reduction(0x4b, staticRules[0x12])})
            , new State(
               new string[1] {"[terminal_def_atom_unicode → SYMBOL_VALUE_UINT16 •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[19] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[..]", 0x42), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[19] {new Reduction(0xa, staticRules[0x13]), new Reduction(0xb, staticRules[0x13]), new Reduction(0x11, staticRules[0x13]), new Reduction(0x30, staticRules[0x13]), new Reduction(0x31, staticRules[0x13]), new Reduction(0x32, staticRules[0x13]), new Reduction(0x33, staticRules[0x13]), new Reduction(0x34, staticRules[0x13]), new Reduction(0x35, staticRules[0x13]), new Reduction(0x41, staticRules[0x13]), new Reduction(0x42, staticRules[0x13]), new Reduction(0x43, staticRules[0x13]), new Reduction(0x44, staticRules[0x13]), new Reduction(0x45, staticRules[0x13]), new Reduction(0x46, staticRules[0x13]), new Reduction(0x47, staticRules[0x13]), new Reduction(0x49, staticRules[0x13]), new Reduction(0x4a, staticRules[0x13]), new Reduction(0x4b, staticRules[0x13])})
            , new State(
               new string[1] {"[terminal_def_atom_set → SYMBOL_TERMINAL_SET •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x15]), new Reduction(0xb, staticRules[0x15]), new Reduction(0x11, staticRules[0x15]), new Reduction(0x30, staticRules[0x15]), new Reduction(0x31, staticRules[0x15]), new Reduction(0x32, staticRules[0x15]), new Reduction(0x33, staticRules[0x15]), new Reduction(0x34, staticRules[0x15]), new Reduction(0x35, staticRules[0x15]), new Reduction(0x41, staticRules[0x15]), new Reduction(0x43, staticRules[0x15]), new Reduction(0x44, staticRules[0x15]), new Reduction(0x45, staticRules[0x15]), new Reduction(0x46, staticRules[0x15]), new Reduction(0x47, staticRules[0x15]), new Reduction(0x49, staticRules[0x15]), new Reduction(0x4a, staticRules[0x15]), new Reduction(0x4b, staticRules[0x15])})
            , new State(
               new string[1] {"[terminal_def_atom_ucat → SYMBOL_TERMINAL_UCAT •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x17]), new Reduction(0xb, staticRules[0x17]), new Reduction(0x11, staticRules[0x17]), new Reduction(0x30, staticRules[0x17]), new Reduction(0x31, staticRules[0x17]), new Reduction(0x32, staticRules[0x17]), new Reduction(0x33, staticRules[0x17]), new Reduction(0x34, staticRules[0x17]), new Reduction(0x35, staticRules[0x17]), new Reduction(0x41, staticRules[0x17]), new Reduction(0x43, staticRules[0x17]), new Reduction(0x44, staticRules[0x17]), new Reduction(0x45, staticRules[0x17]), new Reduction(0x46, staticRules[0x17]), new Reduction(0x47, staticRules[0x17]), new Reduction(0x49, staticRules[0x17]), new Reduction(0x4a, staticRules[0x17]), new Reduction(0x4b, staticRules[0x17])})
            , new State(
               new string[1] {"[terminal_def_atom_ublock → SYMBOL_TERMINAL_UBLOCK •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x16]), new Reduction(0xb, staticRules[0x16]), new Reduction(0x11, staticRules[0x16]), new Reduction(0x30, staticRules[0x16]), new Reduction(0x31, staticRules[0x16]), new Reduction(0x32, staticRules[0x16]), new Reduction(0x33, staticRules[0x16]), new Reduction(0x34, staticRules[0x16]), new Reduction(0x35, staticRules[0x16]), new Reduction(0x41, staticRules[0x16]), new Reduction(0x43, staticRules[0x16]), new Reduction(0x44, staticRules[0x16]), new Reduction(0x45, staticRules[0x16]), new Reduction(0x46, staticRules[0x16]), new Reduction(0x47, staticRules[0x16]), new Reduction(0x49, staticRules[0x16]), new Reduction(0x4a, staticRules[0x16]), new Reduction(0x4b, staticRules[0x16])})
            , new State(
               new string[3] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> • -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> • rule_template_params -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[rule_template_params → • < NAME _m101 >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0xF4, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0xF5},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_rule_context<grammar_bin_terminal> → [ rule_definition<grammar_bin_terminal> ] •, NAME/->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAB]), new Reduction(0x4c, staticRules[0xAB]), new Reduction(0x4d, staticRules[0xAB])})
            , new State(
               new string[1] {"[rule_definition<grammar_bin_terminal> → rule_def_choice<grammar_bin_terminal> _m188 •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x7E]), new Reduction(0x44, staticRules[0x7E]), new Reduction(0xdb, staticRules[0x7E])})
            , new State(
               new string[35] {"[_m188 → | • rule_def_choice<grammar_bin_terminal> _m188, ;/)/]]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, ;/)/|/]]", "[rule_def_choice<grammar_bin_terminal> → •, ;/)/|/]]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, ;/)/|/]]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[20] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[12] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[12] {0xF6, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[4] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x44, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80]), new Reduction(0xdb, staticRules[0x80])})
            , new State(
               new string[1] {"[rule_def_restrict<grammar_bin_terminal> → rule_def_fragment<grammar_bin_terminal> _m186 •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x81]), new Reduction(0x44, staticRules[0x81]), new Reduction(0x4a, staticRules[0x81]), new Reduction(0xdb, staticRules[0x81])})
            , new State(
               new string[32] {"[_m186 → - • rule_def_fragment<grammar_bin_terminal> _m186, ;/)/|/]]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[16] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xF7, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_def_fragment<grammar_bin_terminal> → rule_def_repetition<grammar_bin_terminal> _m184 •, ;/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x82]), new Reduction(0x44, staticRules[0x82]), new Reduction(0x49, staticRules[0x82]), new Reduction(0x4a, staticRules[0x82]), new Reduction(0xdb, staticRules[0x82])})
            , new State(
               new string[33] {"[_m184 → rule_def_repetition<grammar_bin_terminal> • _m184, ;/)/-/|/]]", "[_m184 → • rule_def_repetition<grammar_bin_terminal> _m184, ;/)/-/|/]]", "[_m184 → •, ;/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[21] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x9a, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xF8, 0xC2, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[5] {new Reduction(0x41, staticRules[0x96]), new Reduction(0x44, staticRules[0x96]), new Reduction(0x49, staticRules[0x96]), new Reduction(0x4a, staticRules[0x96]), new Reduction(0xdb, staticRules[0x96])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> * •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[21] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x83]), new Reduction(0x11, staticRules[0x83]), new Reduction(0x2e, staticRules[0x83]), new Reduction(0x34, staticRules[0x83]), new Reduction(0x35, staticRules[0x83]), new Reduction(0x36, staticRules[0x83]), new Reduction(0x37, staticRules[0x83]), new Reduction(0x38, staticRules[0x83]), new Reduction(0x39, staticRules[0x83]), new Reduction(0x3a, staticRules[0x83]), new Reduction(0x3b, staticRules[0x83]), new Reduction(0x3c, staticRules[0x83]), new Reduction(0x3d, staticRules[0x83]), new Reduction(0x3e, staticRules[0x83]), new Reduction(0x3f, staticRules[0x83]), new Reduction(0x41, staticRules[0x83]), new Reduction(0x43, staticRules[0x83]), new Reduction(0x44, staticRules[0x83]), new Reduction(0x49, staticRules[0x83]), new Reduction(0x4a, staticRules[0x83]), new Reduction(0xdb, staticRules[0x83])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> + •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[21] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x84]), new Reduction(0x11, staticRules[0x84]), new Reduction(0x2e, staticRules[0x84]), new Reduction(0x34, staticRules[0x84]), new Reduction(0x35, staticRules[0x84]), new Reduction(0x36, staticRules[0x84]), new Reduction(0x37, staticRules[0x84]), new Reduction(0x38, staticRules[0x84]), new Reduction(0x39, staticRules[0x84]), new Reduction(0x3a, staticRules[0x84]), new Reduction(0x3b, staticRules[0x84]), new Reduction(0x3c, staticRules[0x84]), new Reduction(0x3d, staticRules[0x84]), new Reduction(0x3e, staticRules[0x84]), new Reduction(0x3f, staticRules[0x84]), new Reduction(0x41, staticRules[0x84]), new Reduction(0x43, staticRules[0x84]), new Reduction(0x44, staticRules[0x84]), new Reduction(0x49, staticRules[0x84]), new Reduction(0x4a, staticRules[0x84]), new Reduction(0xdb, staticRules[0x84])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_bin_terminal> → rule_def_tree_action<grammar_bin_terminal> ? •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[21] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x85]), new Reduction(0x11, staticRules[0x85]), new Reduction(0x2e, staticRules[0x85]), new Reduction(0x34, staticRules[0x85]), new Reduction(0x35, staticRules[0x85]), new Reduction(0x36, staticRules[0x85]), new Reduction(0x37, staticRules[0x85]), new Reduction(0x38, staticRules[0x85]), new Reduction(0x39, staticRules[0x85]), new Reduction(0x3a, staticRules[0x85]), new Reduction(0x3b, staticRules[0x85]), new Reduction(0x3c, staticRules[0x85]), new Reduction(0x3d, staticRules[0x85]), new Reduction(0x3e, staticRules[0x85]), new Reduction(0x3f, staticRules[0x85]), new Reduction(0x41, staticRules[0x85]), new Reduction(0x43, staticRules[0x85]), new Reduction(0x44, staticRules[0x85]), new Reduction(0x49, staticRules[0x85]), new Reduction(0x4a, staticRules[0x85]), new Reduction(0xdb, staticRules[0x85])})
            , new State(
               new string[1] {"[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> ^ •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[24] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x87]), new Reduction(0x11, staticRules[0x87]), new Reduction(0x2e, staticRules[0x87]), new Reduction(0x34, staticRules[0x87]), new Reduction(0x35, staticRules[0x87]), new Reduction(0x36, staticRules[0x87]), new Reduction(0x37, staticRules[0x87]), new Reduction(0x38, staticRules[0x87]), new Reduction(0x39, staticRules[0x87]), new Reduction(0x3a, staticRules[0x87]), new Reduction(0x3b, staticRules[0x87]), new Reduction(0x3c, staticRules[0x87]), new Reduction(0x3d, staticRules[0x87]), new Reduction(0x3e, staticRules[0x87]), new Reduction(0x3f, staticRules[0x87]), new Reduction(0x41, staticRules[0x87]), new Reduction(0x43, staticRules[0x87]), new Reduction(0x44, staticRules[0x87]), new Reduction(0x45, staticRules[0x87]), new Reduction(0x46, staticRules[0x87]), new Reduction(0x47, staticRules[0x87]), new Reduction(0x49, staticRules[0x87]), new Reduction(0x4a, staticRules[0x87]), new Reduction(0xdb, staticRules[0x87])})
            , new State(
               new string[1] {"[rule_def_tree_action<grammar_bin_terminal> → rule_def_element<grammar_bin_terminal> ! •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[24] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x88]), new Reduction(0x11, staticRules[0x88]), new Reduction(0x2e, staticRules[0x88]), new Reduction(0x34, staticRules[0x88]), new Reduction(0x35, staticRules[0x88]), new Reduction(0x36, staticRules[0x88]), new Reduction(0x37, staticRules[0x88]), new Reduction(0x38, staticRules[0x88]), new Reduction(0x39, staticRules[0x88]), new Reduction(0x3a, staticRules[0x88]), new Reduction(0x3b, staticRules[0x88]), new Reduction(0x3c, staticRules[0x88]), new Reduction(0x3d, staticRules[0x88]), new Reduction(0x3e, staticRules[0x88]), new Reduction(0x3f, staticRules[0x88]), new Reduction(0x41, staticRules[0x88]), new Reduction(0x43, staticRules[0x88]), new Reduction(0x44, staticRules[0x88]), new Reduction(0x45, staticRules[0x88]), new Reduction(0x46, staticRules[0x88]), new Reduction(0x47, staticRules[0x88]), new Reduction(0x49, staticRules[0x88]), new Reduction(0x4a, staticRules[0x88]), new Reduction(0xdb, staticRules[0x88])})
            , new State(
               new string[1] {"[rule_def_element<grammar_bin_terminal> → ( rule_definition<grammar_bin_terminal> • ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44)},
               new ushort[1] {0x44},
               new ushort[1] {0xF9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_action → { NAME • }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0xFA},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_ref_template<grammar_bin_terminal> → NAME rule_sym_ref_params<grammar_bin_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x91]), new Reduction(0x11, staticRules[0x91]), new Reduction(0x2e, staticRules[0x91]), new Reduction(0x34, staticRules[0x91]), new Reduction(0x35, staticRules[0x91]), new Reduction(0x36, staticRules[0x91]), new Reduction(0x37, staticRules[0x91]), new Reduction(0x38, staticRules[0x91]), new Reduction(0x39, staticRules[0x91]), new Reduction(0x3a, staticRules[0x91]), new Reduction(0x3b, staticRules[0x91]), new Reduction(0x3c, staticRules[0x91]), new Reduction(0x3d, staticRules[0x91]), new Reduction(0x3e, staticRules[0x91]), new Reduction(0x3f, staticRules[0x91]), new Reduction(0x41, staticRules[0x91]), new Reduction(0x43, staticRules[0x91]), new Reduction(0x44, staticRules[0x91]), new Reduction(0x45, staticRules[0x91]), new Reduction(0x46, staticRules[0x91]), new Reduction(0x47, staticRules[0x91]), new Reduction(0x48, staticRules[0x91]), new Reduction(0x49, staticRules[0x91]), new Reduction(0x4a, staticRules[0x91]), new Reduction(0x4e, staticRules[0x91]), new Reduction(0x55, staticRules[0x91]), new Reduction(0x56, staticRules[0x91]), new Reduction(0xdb, staticRules[0x91])})
            , new State(
               new string[22] {"[rule_sym_ref_params<grammar_bin_terminal> → < • rule_def_atom<grammar_bin_terminal> _m175 >, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, ,/>]", "[rule_sym_action → • { NAME }, ,/>]", "[rule_sym_virtual → • QUOTED_DATA, ,/>]", "[rule_sym_ref_simple → • NAME, ,/>]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, ,/>]"},
               new Hime.Redist.Parsers.SymbolTerminal[15] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F)},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[6] {0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[6] {0xFB, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               new string[1] {"[option → NAME = QUOTED_DATA ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x10]), new Reduction(0x12, staticRules[0x10])})
            , new State(
               new string[1] {"[grammar_cf_rules<grammar_text_terminal> → rules { _m152 } •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x59])})
            , new State(
               new string[1] {"[_m152 → cf_rule_simple<grammar_text_terminal> _m152 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x79])})
            , new State(
               new string[1] {"[_m152 → cf_rule_template<grammar_text_terminal> _m152 •, }]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7A])})
            , new State(
               new string[26] {"[cf_rule_simple<grammar_text_terminal> → NAME -> • rule_definition<grammar_text_terminal> ;, NAME/}]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, ;]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, ;/|]", "[rule_def_choice<grammar_text_terminal> → •, ;/|]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, ;/|]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0xFC, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               new string[1] {"[cf_rule_template<grammar_text_terminal> → NAME rule_template_params • -> rule_definition<grammar_text_terminal> ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[1] {0x4c},
               new ushort[1] {0xFD},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_rule_simple<grammar_bin_terminal> → NAME -> rule_definition<grammar_bin_terminal> • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0xFE},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[36] {"[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> • rule_definition<grammar_bin_terminal> ;, NAME/}]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, ;]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, ;/|]", "[rule_def_choice<grammar_bin_terminal> → •, ;/|]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, ;/|]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xFF, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               new string[3] {"[rule_template_params → < NAME • _m101 >, ->]", "[_m101 → • , NAME _m101, >]", "[_m101 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x101},
               new ushort[1] {0x77},
               new ushort[1] {0x100},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x52])})
            , new State(
               new string[3] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> • -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> • rule_template_params -> rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[rule_template_params → • < NAME _m101 >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x102, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0x103},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_rule_context<grammar_text_terminal> → [ rule_definition<grammar_text_terminal> ] •, NAME/->/<]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C), new Hime.Redist.Parsers.SymbolTerminal("_T[<]", 0x4D)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA3]), new Reduction(0x4c, staticRules[0xA3]), new Reduction(0x4d, staticRules[0xA3])})
            , new State(
               new string[1] {"[rule_definition<grammar_text_terminal> → rule_def_choice<grammar_text_terminal> _m147 •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x5B]), new Reduction(0x44, staticRules[0x5B]), new Reduction(0xdb, staticRules[0x5B])})
            , new State(
               new string[25] {"[_m147 → | • rule_def_choice<grammar_text_terminal> _m147, ;/)/]]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, ;/)/|/]]", "[rule_def_choice<grammar_text_terminal> → •, ;/)/|/]]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, ;/)/|/]]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[9] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[13] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[13] {0x104, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x44, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D]), new Reduction(0xdb, staticRules[0x5D])})
            , new State(
               new string[1] {"[rule_def_restrict<grammar_text_terminal> → rule_def_fragment<grammar_text_terminal> _m145 •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5E]), new Reduction(0x44, staticRules[0x5E]), new Reduction(0x4a, staticRules[0x5E]), new Reduction(0xdb, staticRules[0x5E])})
            , new State(
               new string[22] {"[_m145 → - • rule_def_fragment<grammar_text_terminal> _m145, ;/)/|/]]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0x105, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_def_fragment<grammar_text_terminal> → rule_def_repetition<grammar_text_terminal> _m143 •, ;/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x5F]), new Reduction(0x44, staticRules[0x5F]), new Reduction(0x49, staticRules[0x5F]), new Reduction(0x4a, staticRules[0x5F]), new Reduction(0xdb, staticRules[0x5F])})
            , new State(
               new string[23] {"[_m143 → rule_def_repetition<grammar_text_terminal> • _m143, ;/)/-/|/]]", "[_m143 → • rule_def_repetition<grammar_text_terminal> _m143, ;/)/-/|/]]", "[_m143 → •, ;/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[10] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x88, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0x106, 0xDC, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[5] {new Reduction(0x41, staticRules[0x73]), new Reduction(0x44, staticRules[0x73]), new Reduction(0x49, staticRules[0x73]), new Reduction(0x4a, staticRules[0x73]), new Reduction(0xdb, staticRules[0x73])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> * •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[10] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x60]), new Reduction(0x11, staticRules[0x60]), new Reduction(0x2e, staticRules[0x60]), new Reduction(0x30, staticRules[0x60]), new Reduction(0x41, staticRules[0x60]), new Reduction(0x43, staticRules[0x60]), new Reduction(0x44, staticRules[0x60]), new Reduction(0x49, staticRules[0x60]), new Reduction(0x4a, staticRules[0x60]), new Reduction(0xdb, staticRules[0x60])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> + •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[10] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x61]), new Reduction(0x11, staticRules[0x61]), new Reduction(0x2e, staticRules[0x61]), new Reduction(0x30, staticRules[0x61]), new Reduction(0x41, staticRules[0x61]), new Reduction(0x43, staticRules[0x61]), new Reduction(0x44, staticRules[0x61]), new Reduction(0x49, staticRules[0x61]), new Reduction(0x4a, staticRules[0x61]), new Reduction(0xdb, staticRules[0x61])})
            , new State(
               new string[1] {"[rule_def_repetition<grammar_text_terminal> → rule_def_tree_action<grammar_text_terminal> ? •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[10] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x62]), new Reduction(0x11, staticRules[0x62]), new Reduction(0x2e, staticRules[0x62]), new Reduction(0x30, staticRules[0x62]), new Reduction(0x41, staticRules[0x62]), new Reduction(0x43, staticRules[0x62]), new Reduction(0x44, staticRules[0x62]), new Reduction(0x49, staticRules[0x62]), new Reduction(0x4a, staticRules[0x62]), new Reduction(0xdb, staticRules[0x62])})
            , new State(
               new string[1] {"[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> ^ •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[13] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x64]), new Reduction(0x11, staticRules[0x64]), new Reduction(0x2e, staticRules[0x64]), new Reduction(0x30, staticRules[0x64]), new Reduction(0x41, staticRules[0x64]), new Reduction(0x43, staticRules[0x64]), new Reduction(0x44, staticRules[0x64]), new Reduction(0x45, staticRules[0x64]), new Reduction(0x46, staticRules[0x64]), new Reduction(0x47, staticRules[0x64]), new Reduction(0x49, staticRules[0x64]), new Reduction(0x4a, staticRules[0x64]), new Reduction(0xdb, staticRules[0x64])})
            , new State(
               new string[1] {"[rule_def_tree_action<grammar_text_terminal> → rule_def_element<grammar_text_terminal> ! •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[13] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x65]), new Reduction(0x11, staticRules[0x65]), new Reduction(0x2e, staticRules[0x65]), new Reduction(0x30, staticRules[0x65]), new Reduction(0x41, staticRules[0x65]), new Reduction(0x43, staticRules[0x65]), new Reduction(0x44, staticRules[0x65]), new Reduction(0x45, staticRules[0x65]), new Reduction(0x46, staticRules[0x65]), new Reduction(0x47, staticRules[0x65]), new Reduction(0x49, staticRules[0x65]), new Reduction(0x4a, staticRules[0x65]), new Reduction(0xdb, staticRules[0x65])})
            , new State(
               new string[1] {"[rule_def_element<grammar_text_terminal> → ( rule_definition<grammar_text_terminal> • ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44)},
               new ushort[1] {0x44},
               new ushort[1] {0x107},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_ref_template<grammar_text_terminal> → NAME rule_sym_ref_params<grammar_text_terminal> •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6E]), new Reduction(0x11, staticRules[0x6E]), new Reduction(0x2e, staticRules[0x6E]), new Reduction(0x30, staticRules[0x6E]), new Reduction(0x41, staticRules[0x6E]), new Reduction(0x43, staticRules[0x6E]), new Reduction(0x44, staticRules[0x6E]), new Reduction(0x45, staticRules[0x6E]), new Reduction(0x46, staticRules[0x6E]), new Reduction(0x47, staticRules[0x6E]), new Reduction(0x48, staticRules[0x6E]), new Reduction(0x49, staticRules[0x6E]), new Reduction(0x4a, staticRules[0x6E]), new Reduction(0x4e, staticRules[0x6E]), new Reduction(0x55, staticRules[0x6E]), new Reduction(0x56, staticRules[0x6E]), new Reduction(0xdb, staticRules[0x6E])})
            , new State(
               new string[12] {"[rule_sym_ref_params<grammar_text_terminal> → < • rule_def_atom<grammar_text_terminal> _m134 >, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, ,/>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, ,/>]", "[rule_sym_action → • { NAME }, ,/>]", "[rule_sym_virtual → • QUOTED_DATA, ,/>]", "[rule_sym_ref_simple → • NAME, ,/>]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, ,/>]", "[grammar_text_terminal → • terminal_def_atom_text, ,/>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, ,/>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30)},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x77, 0x78, 0xA3, 0xA5},
               new ushort[7] {0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[7] {0x108, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal → NAME -> terminal_definition terminal_subgrammar • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x109},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[2] {"[terminal_subgrammar → => • qualified_name, ;]", "[qualified_name → • NAME _m20, ;]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x10A},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_definition → terminal_def_restrict _m93 •, ;/)/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x2C]), new Reduction(0x44, staticRules[0x2C]), new Reduction(0x4b, staticRules[0x2C])})
            , new State(
               new string[23] {"[_m93 → | • terminal_def_restrict _m93, ;/)/=>]", "[terminal_def_restrict → • terminal_def_fragment _m91, ;/)/|/=>]", "[terminal_def_fragment → • terminal_def_repetition _m89, ;/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[9] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[12] {0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[12] {0x10B, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_restrict → terminal_def_fragment _m91 •, ;/)/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x2B]), new Reduction(0x44, staticRules[0x2B]), new Reduction(0x4a, staticRules[0x2B]), new Reduction(0x4b, staticRules[0x2B])})
            , new State(
               new string[22] {"[_m91 → - • terminal_def_fragment _m91, ;/)/|/=>]", "[terminal_def_fragment → • terminal_def_repetition _m89, ;/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[9] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0x10C, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_fragment → terminal_def_repetition _m89 •, ;/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x2A]), new Reduction(0x44, staticRules[0x2A]), new Reduction(0x49, staticRules[0x2A]), new Reduction(0x4a, staticRules[0x2A]), new Reduction(0x4b, staticRules[0x2A])})
            , new State(
               new string[23] {"[_m89 → terminal_def_repetition • _m89, ;/)/-/|/=>]", "[_m89 → • terminal_def_repetition _m89, ;/)/-/|/=>]", "[_m89 → •, ;/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element terminal_def_cardinalilty, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_repetition → • terminal_def_element, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_element → • terminal_def_atom, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_element → • ( terminal_definition ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_any, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_text, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_set, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_span, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ucat, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • terminal_def_atom_ublock, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom → • NAME, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_any → • ., NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/../(/)/*/+/?/-/|/=>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_set → • SYMBOL_TERMINAL_SET, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_span → • terminal_def_atom_unicode .. terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ucat → • SYMBOL_TERMINAL_UCAT, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_ublock → • SYMBOL_TERMINAL_UBLOCK, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x74, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0x10D, 0xEC, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4C]), new Reduction(0x44, staticRules[0x4C]), new Reduction(0x49, staticRules[0x4C]), new Reduction(0x4a, staticRules[0x4C]), new Reduction(0x4b, staticRules[0x4C])})
            , new State(
               new string[1] {"[terminal_def_repetition → terminal_def_element terminal_def_cardinalilty •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x28]), new Reduction(0xb, staticRules[0x28]), new Reduction(0x30, staticRules[0x28]), new Reduction(0x31, staticRules[0x28]), new Reduction(0x32, staticRules[0x28]), new Reduction(0x33, staticRules[0x28]), new Reduction(0x34, staticRules[0x28]), new Reduction(0x35, staticRules[0x28]), new Reduction(0x41, staticRules[0x28]), new Reduction(0x43, staticRules[0x28]), new Reduction(0x44, staticRules[0x28]), new Reduction(0x49, staticRules[0x28]), new Reduction(0x4a, staticRules[0x28]), new Reduction(0x4b, staticRules[0x28])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → * •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x23]), new Reduction(0xb, staticRules[0x23]), new Reduction(0x30, staticRules[0x23]), new Reduction(0x31, staticRules[0x23]), new Reduction(0x32, staticRules[0x23]), new Reduction(0x33, staticRules[0x23]), new Reduction(0x34, staticRules[0x23]), new Reduction(0x35, staticRules[0x23]), new Reduction(0x41, staticRules[0x23]), new Reduction(0x43, staticRules[0x23]), new Reduction(0x44, staticRules[0x23]), new Reduction(0x49, staticRules[0x23]), new Reduction(0x4a, staticRules[0x23]), new Reduction(0x4b, staticRules[0x23])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → + •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x24]), new Reduction(0xb, staticRules[0x24]), new Reduction(0x30, staticRules[0x24]), new Reduction(0x31, staticRules[0x24]), new Reduction(0x32, staticRules[0x24]), new Reduction(0x33, staticRules[0x24]), new Reduction(0x34, staticRules[0x24]), new Reduction(0x35, staticRules[0x24]), new Reduction(0x41, staticRules[0x24]), new Reduction(0x43, staticRules[0x24]), new Reduction(0x44, staticRules[0x24]), new Reduction(0x49, staticRules[0x24]), new Reduction(0x4a, staticRules[0x24]), new Reduction(0x4b, staticRules[0x24])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → ? •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x25]), new Reduction(0xb, staticRules[0x25]), new Reduction(0x30, staticRules[0x25]), new Reduction(0x31, staticRules[0x25]), new Reduction(0x32, staticRules[0x25]), new Reduction(0x33, staticRules[0x25]), new Reduction(0x34, staticRules[0x25]), new Reduction(0x35, staticRules[0x25]), new Reduction(0x41, staticRules[0x25]), new Reduction(0x43, staticRules[0x25]), new Reduction(0x44, staticRules[0x25]), new Reduction(0x49, staticRules[0x25]), new Reduction(0x4a, staticRules[0x25]), new Reduction(0x4b, staticRules[0x25])})
            , new State(
               new string[2] {"[terminal_def_cardinalilty → { • INTEGER , INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → { • INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("INTEGER", 0x2D)},
               new ushort[1] {0x2d},
               new ushort[1] {0x10E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_element → ( terminal_definition • ), NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44)},
               new ushort[1] {0x44},
               new ushort[1] {0x10F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[terminal_def_atom_span → terminal_def_atom_unicode .. • terminal_def_atom_unicode, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT8, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]", "[terminal_def_atom_unicode → • SYMBOL_VALUE_UINT16, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35)},
               new ushort[2] {0x34, 0x35},
               new ushort[2] {0xB6, 0xB7},
               new ushort[1] {0x59},
               new ushort[1] {0x110},
               new Reduction[0] {})
            , new State(
               new string[36] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> • rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, ;]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, ;/|]", "[rule_def_choice<grammar_bin_terminal> → •, ;/|]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, ;/|]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x111, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               new string[1] {"[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params • -> rule_definition<grammar_bin_terminal> ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[1] {0x4c},
               new ushort[1] {0x112},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[_m188 → | rule_def_choice<grammar_bin_terminal> • _m188, ;/)/]]", "[_m188 → • | rule_def_choice<grammar_bin_terminal> _m188, ;/)/]]", "[_m188 → •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4a},
               new ushort[1] {0xBE},
               new ushort[1] {0x9c},
               new ushort[1] {0x113},
               new Reduction[3] {new Reduction(0x41, staticRules[0x9A]), new Reduction(0x44, staticRules[0x9A]), new Reduction(0xdb, staticRules[0x9A])})
            , new State(
               new string[3] {"[_m186 → - rule_def_fragment<grammar_bin_terminal> • _m186, ;/)/|/]]", "[_m186 → • - rule_def_fragment<grammar_bin_terminal> _m186, ;/)/|/]]", "[_m186 → •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x49},
               new ushort[1] {0xC0},
               new ushort[1] {0x9b},
               new ushort[1] {0x114},
               new Reduction[4] {new Reduction(0x41, staticRules[0x98]), new Reduction(0x44, staticRules[0x98]), new Reduction(0x4a, staticRules[0x98]), new Reduction(0xdb, staticRules[0x98])})
            , new State(
               new string[1] {"[_m184 → rule_def_repetition<grammar_bin_terminal> _m184 •, ;/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x95]), new Reduction(0x44, staticRules[0x95]), new Reduction(0x49, staticRules[0x95]), new Reduction(0x4a, staticRules[0x95]), new Reduction(0xdb, staticRules[0x95])})
            , new State(
               new string[1] {"[rule_def_element<grammar_bin_terminal> → ( rule_definition<grammar_bin_terminal> ) •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[26] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[26] {new Reduction(0xa, staticRules[0x8B]), new Reduction(0x11, staticRules[0x8B]), new Reduction(0x2e, staticRules[0x8B]), new Reduction(0x34, staticRules[0x8B]), new Reduction(0x35, staticRules[0x8B]), new Reduction(0x36, staticRules[0x8B]), new Reduction(0x37, staticRules[0x8B]), new Reduction(0x38, staticRules[0x8B]), new Reduction(0x39, staticRules[0x8B]), new Reduction(0x3a, staticRules[0x8B]), new Reduction(0x3b, staticRules[0x8B]), new Reduction(0x3c, staticRules[0x8B]), new Reduction(0x3d, staticRules[0x8B]), new Reduction(0x3e, staticRules[0x8B]), new Reduction(0x3f, staticRules[0x8B]), new Reduction(0x41, staticRules[0x8B]), new Reduction(0x43, staticRules[0x8B]), new Reduction(0x44, staticRules[0x8B]), new Reduction(0x45, staticRules[0x8B]), new Reduction(0x46, staticRules[0x8B]), new Reduction(0x47, staticRules[0x8B]), new Reduction(0x49, staticRules[0x8B]), new Reduction(0x4a, staticRules[0x8B]), new Reduction(0x55, staticRules[0x8B]), new Reduction(0x56, staticRules[0x8B]), new Reduction(0xdb, staticRules[0x8B])})
            , new State(
               new string[1] {"[rule_sym_action → { NAME } •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[29] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[29] {new Reduction(0xa, staticRules[0x30]), new Reduction(0x11, staticRules[0x30]), new Reduction(0x2e, staticRules[0x30]), new Reduction(0x30, staticRules[0x30]), new Reduction(0x34, staticRules[0x30]), new Reduction(0x35, staticRules[0x30]), new Reduction(0x36, staticRules[0x30]), new Reduction(0x37, staticRules[0x30]), new Reduction(0x38, staticRules[0x30]), new Reduction(0x39, staticRules[0x30]), new Reduction(0x3a, staticRules[0x30]), new Reduction(0x3b, staticRules[0x30]), new Reduction(0x3c, staticRules[0x30]), new Reduction(0x3d, staticRules[0x30]), new Reduction(0x3e, staticRules[0x30]), new Reduction(0x3f, staticRules[0x30]), new Reduction(0x41, staticRules[0x30]), new Reduction(0x43, staticRules[0x30]), new Reduction(0x44, staticRules[0x30]), new Reduction(0x45, staticRules[0x30]), new Reduction(0x46, staticRules[0x30]), new Reduction(0x47, staticRules[0x30]), new Reduction(0x48, staticRules[0x30]), new Reduction(0x49, staticRules[0x30]), new Reduction(0x4a, staticRules[0x30]), new Reduction(0x4e, staticRules[0x30]), new Reduction(0x55, staticRules[0x30]), new Reduction(0x56, staticRules[0x30]), new Reduction(0xdb, staticRules[0x30])})
            , new State(
               new string[3] {"[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> • _m175 >, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[_m175 → • , rule_def_atom<grammar_bin_terminal> _m175, >]", "[_m175 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x116},
               new ushort[1] {0x99},
               new ushort[1] {0x115},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x94])})
            , new State(
               new string[1] {"[cf_rule_simple<grammar_text_terminal> → NAME -> rule_definition<grammar_text_terminal> • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x117},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[26] {"[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> • rule_definition<grammar_text_terminal> ;, NAME/}]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, ;]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, ;/|]", "[rule_def_choice<grammar_text_terminal> → •, ;/|]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, ;/|]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x118, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               new string[1] {"[cf_rule_simple<grammar_bin_terminal> → NAME -> rule_definition<grammar_bin_terminal> ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x7D]), new Reduction(0x12, staticRules[0x7D])})
            , new State(
               new string[1] {"[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> rule_definition<grammar_bin_terminal> • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x119},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_template_params → < NAME _m101 • >, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x4e},
               new ushort[1] {0x11A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_m101 → , • NAME _m101, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA)},
               new ushort[1] {0xa},
               new ushort[1] {0x11B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[26] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> • rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, ;]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, ;/|]", "[rule_def_choice<grammar_text_terminal> → •, ;/|]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, ;/|]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x11C, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               new string[1] {"[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params • -> rule_definition<grammar_text_terminal> ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[1] {0x4c},
               new ushort[1] {0x11D},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[3] {"[_m147 → | rule_def_choice<grammar_text_terminal> • _m147, ;/)/]]", "[_m147 → • | rule_def_choice<grammar_text_terminal> _m147, ;/)/]]", "[_m147 → •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x4a},
               new ushort[1] {0xD8},
               new ushort[1] {0x8a},
               new ushort[1] {0x11E},
               new Reduction[3] {new Reduction(0x41, staticRules[0x77]), new Reduction(0x44, staticRules[0x77]), new Reduction(0xdb, staticRules[0x77])})
            , new State(
               new string[3] {"[_m145 → - rule_def_fragment<grammar_text_terminal> • _m145, ;/)/|/]]", "[_m145 → • - rule_def_fragment<grammar_text_terminal> _m145, ;/)/|/]]", "[_m145 → •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[1] {0x49},
               new ushort[1] {0xDA},
               new ushort[1] {0x89},
               new ushort[1] {0x11F},
               new Reduction[4] {new Reduction(0x41, staticRules[0x75]), new Reduction(0x44, staticRules[0x75]), new Reduction(0x4a, staticRules[0x75]), new Reduction(0xdb, staticRules[0x75])})
            , new State(
               new string[1] {"[_m143 → rule_def_repetition<grammar_text_terminal> _m143 •, ;/)/-/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x72]), new Reduction(0x44, staticRules[0x72]), new Reduction(0x49, staticRules[0x72]), new Reduction(0x4a, staticRules[0x72]), new Reduction(0xdb, staticRules[0x72])})
            , new State(
               new string[1] {"[rule_def_element<grammar_text_terminal> → ( rule_definition<grammar_text_terminal> ) •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/-/|/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[15] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[15] {new Reduction(0xa, staticRules[0x68]), new Reduction(0x11, staticRules[0x68]), new Reduction(0x2e, staticRules[0x68]), new Reduction(0x30, staticRules[0x68]), new Reduction(0x41, staticRules[0x68]), new Reduction(0x43, staticRules[0x68]), new Reduction(0x44, staticRules[0x68]), new Reduction(0x45, staticRules[0x68]), new Reduction(0x46, staticRules[0x68]), new Reduction(0x47, staticRules[0x68]), new Reduction(0x49, staticRules[0x68]), new Reduction(0x4a, staticRules[0x68]), new Reduction(0x55, staticRules[0x68]), new Reduction(0x56, staticRules[0x68]), new Reduction(0xdb, staticRules[0x68])})
            , new State(
               new string[3] {"[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> • _m134 >, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]", "[_m134 → • , rule_def_atom<grammar_text_terminal> _m134, >]", "[_m134 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x121},
               new ushort[1] {0x87},
               new ushort[1] {0x120},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x71])})
            , new State(
               new string[1] {"[terminal → NAME -> terminal_definition terminal_subgrammar ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x2F]), new Reduction(0x12, staticRules[0x2F])})
            , new State(
               new string[1] {"[terminal_subgrammar → => qualified_name •, ;]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x41, staticRules[0x2D])})
            , new State(
               new string[3] {"[_m93 → | terminal_def_restrict • _m93, ;/)/=>]", "[_m93 → • | terminal_def_restrict _m93, ;/)/=>]", "[_m93 → •, ;/)/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x4a},
               new ushort[1] {0xE8},
               new ushort[1] {0x76},
               new ushort[1] {0x122},
               new Reduction[3] {new Reduction(0x41, staticRules[0x50]), new Reduction(0x44, staticRules[0x50]), new Reduction(0x4b, staticRules[0x50])})
            , new State(
               new string[3] {"[_m91 → - terminal_def_fragment • _m91, ;/)/|/=>]", "[_m91 → • - terminal_def_fragment _m91, ;/)/|/=>]", "[_m91 → •, ;/)/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[1] {0x49},
               new ushort[1] {0xEA},
               new ushort[1] {0x75},
               new ushort[1] {0x123},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4E]), new Reduction(0x44, staticRules[0x4E]), new Reduction(0x4a, staticRules[0x4E]), new Reduction(0x4b, staticRules[0x4E])})
            , new State(
               new string[1] {"[_m89 → terminal_def_repetition _m89 •, ;/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[5] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4B]), new Reduction(0x44, staticRules[0x4B]), new Reduction(0x49, staticRules[0x4B]), new Reduction(0x4a, staticRules[0x4B]), new Reduction(0x4b, staticRules[0x4B])})
            , new State(
               new string[2] {"[terminal_def_cardinalilty → { INTEGER • , INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]", "[terminal_def_cardinalilty → { INTEGER • }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48)},
               new ushort[2] {0x48, 0x12},
               new ushort[2] {0x124, 0x125},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_element → ( terminal_definition ) •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x22]), new Reduction(0xb, staticRules[0x22]), new Reduction(0x11, staticRules[0x22]), new Reduction(0x30, staticRules[0x22]), new Reduction(0x31, staticRules[0x22]), new Reduction(0x32, staticRules[0x22]), new Reduction(0x33, staticRules[0x22]), new Reduction(0x34, staticRules[0x22]), new Reduction(0x35, staticRules[0x22]), new Reduction(0x41, staticRules[0x22]), new Reduction(0x43, staticRules[0x22]), new Reduction(0x44, staticRules[0x22]), new Reduction(0x45, staticRules[0x22]), new Reduction(0x46, staticRules[0x22]), new Reduction(0x47, staticRules[0x22]), new Reduction(0x49, staticRules[0x22]), new Reduction(0x4a, staticRules[0x22]), new Reduction(0x4b, staticRules[0x22])})
            , new State(
               new string[1] {"[terminal_def_atom_span → terminal_def_atom_unicode .. terminal_def_atom_unicode •, NAME/./{/SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/*/+/?/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x18]), new Reduction(0xb, staticRules[0x18]), new Reduction(0x11, staticRules[0x18]), new Reduction(0x30, staticRules[0x18]), new Reduction(0x31, staticRules[0x18]), new Reduction(0x32, staticRules[0x18]), new Reduction(0x33, staticRules[0x18]), new Reduction(0x34, staticRules[0x18]), new Reduction(0x35, staticRules[0x18]), new Reduction(0x41, staticRules[0x18]), new Reduction(0x43, staticRules[0x18]), new Reduction(0x44, staticRules[0x18]), new Reduction(0x45, staticRules[0x18]), new Reduction(0x46, staticRules[0x18]), new Reduction(0x47, staticRules[0x18]), new Reduction(0x49, staticRules[0x18]), new Reduction(0x4a, staticRules[0x18]), new Reduction(0x4b, staticRules[0x18])})
            , new State(
               new string[1] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> • ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x126},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[36] {"[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> • rule_definition<grammar_bin_terminal> ;, NAME/}/[]", "[rule_definition<grammar_bin_terminal> → • rule_def_choice<grammar_bin_terminal> _m188, ;]", "[rule_def_choice<grammar_bin_terminal> → • rule_def_restrict<grammar_bin_terminal>, ;/|]", "[rule_def_choice<grammar_bin_terminal> → •, ;/|]", "[rule_def_restrict<grammar_bin_terminal> → • rule_def_fragment<grammar_bin_terminal> _m186, ;/|]", "[rule_def_fragment<grammar_bin_terminal> → • rule_def_repetition<grammar_bin_terminal> _m184, ;/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_repetition<grammar_bin_terminal> → • rule_def_tree_action<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_bin_terminal> → • rule_def_element<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|]", "[rule_def_element<grammar_bin_terminal> → • rule_def_atom<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_bin_terminal> → • ( rule_definition<grammar_bin_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[18] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x127, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               new string[1] {"[_m188 → | rule_def_choice<grammar_bin_terminal> _m188 •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x99]), new Reduction(0x44, staticRules[0x99]), new Reduction(0xdb, staticRules[0x99])})
            , new State(
               new string[1] {"[_m186 → - rule_def_fragment<grammar_bin_terminal> _m186 •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x97]), new Reduction(0x44, staticRules[0x97]), new Reduction(0x4a, staticRules[0x97]), new Reduction(0xdb, staticRules[0x97])})
            , new State(
               new string[1] {"[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> _m175 • >, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x4e},
               new ushort[1] {0x128},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[22] {"[_m175 → , • rule_def_atom<grammar_bin_terminal> _m175, >]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_action, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_virtual, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_simple, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • rule_sym_ref_template<grammar_bin_terminal>, ,/>]", "[rule_def_atom<grammar_bin_terminal> → • grammar_bin_terminal, ,/>]", "[rule_sym_action → • { NAME }, ,/>]", "[rule_sym_virtual → • QUOTED_DATA, ,/>]", "[rule_sym_ref_simple → • NAME, ,/>]", "[rule_sym_ref_template<grammar_bin_terminal> → • NAME rule_sym_ref_params<grammar_bin_terminal>, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT8, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT16, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT32, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT64, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_UINT128, ,/>]", "[grammar_bin_terminal → • SYMBOL_VALUE_BINARY, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT8, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT16, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT32, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT64, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_UINT128, ,/>]", "[grammar_bin_terminal → • SYMBOL_JOKER_BINARY, ,/>]"},
               new Hime.Redist.Parsers.SymbolTerminal[15] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F)},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[6] {0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[6] {0x129, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_rule_simple<grammar_text_terminal> → NAME -> rule_definition<grammar_text_terminal> ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x5A]), new Reduction(0x12, staticRules[0x5A])})
            , new State(
               new string[1] {"[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> rule_definition<grammar_text_terminal> • ;, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x12A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cf_rule_template<grammar_bin_terminal> → NAME rule_template_params -> rule_definition<grammar_bin_terminal> ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x9B]), new Reduction(0x12, staticRules[0x9B])})
            , new State(
               new string[1] {"[rule_template_params → < NAME _m101 > •, ->]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[->]", 0x4C)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4c, staticRules[0x33])})
            , new State(
               new string[3] {"[_m101 → , NAME • _m101, >]", "[_m101 → • , NAME _m101, >]", "[_m101 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x101},
               new ushort[1] {0x77},
               new ushort[1] {0x12B},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x52])})
            , new State(
               new string[1] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> • ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x12C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[26] {"[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> • rule_definition<grammar_text_terminal> ;, NAME/}/[]", "[rule_definition<grammar_text_terminal> → • rule_def_choice<grammar_text_terminal> _m147, ;]", "[rule_def_choice<grammar_text_terminal> → • rule_def_restrict<grammar_text_terminal>, ;/|]", "[rule_def_choice<grammar_text_terminal> → •, ;/|]", "[rule_def_restrict<grammar_text_terminal> → • rule_def_fragment<grammar_text_terminal> _m145, ;/|]", "[rule_def_fragment<grammar_text_terminal> → • rule_def_repetition<grammar_text_terminal> _m143, ;/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> *, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> +, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal> ?, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_repetition<grammar_text_terminal> → • rule_def_tree_action<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> ^, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal> !, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_tree_action<grammar_text_terminal> → • rule_def_element<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|]", "[rule_def_element<grammar_text_terminal> → • rule_def_atom<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_element<grammar_text_terminal> → • ( rule_definition<grammar_text_terminal> ), NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_action → • { NAME }, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_virtual → • QUOTED_DATA, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_simple → • NAME, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[grammar_text_terminal → • terminal_def_atom_text, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/*/+/?/-/|/^/!]"},
               new Hime.Redist.Parsers.SymbolTerminal[7] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A)},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x12D, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               new string[1] {"[_m147 → | rule_def_choice<grammar_text_terminal> _m147 •, ;/)/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x76]), new Reduction(0x44, staticRules[0x76]), new Reduction(0xdb, staticRules[0x76])})
            , new State(
               new string[1] {"[_m145 → - rule_def_fragment<grammar_text_terminal> _m145 •, ;/)/|/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x74]), new Reduction(0x44, staticRules[0x74]), new Reduction(0x4a, staticRules[0x74]), new Reduction(0xdb, staticRules[0x74])})
            , new State(
               new string[1] {"[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> _m134 • >, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x4e},
               new ushort[1] {0x12E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[12] {"[_m134 → , • rule_def_atom<grammar_text_terminal> _m134, >]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_action, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_virtual, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_simple, ,/>]", "[rule_def_atom<grammar_text_terminal> → • rule_sym_ref_template<grammar_text_terminal>, ,/>]", "[rule_def_atom<grammar_text_terminal> → • grammar_text_terminal, ,/>]", "[rule_sym_action → • { NAME }, ,/>]", "[rule_sym_virtual → • QUOTED_DATA, ,/>]", "[rule_sym_ref_simple → • NAME, ,/>]", "[rule_sym_ref_template<grammar_text_terminal> → • NAME rule_sym_ref_params<grammar_text_terminal>, ,/>]", "[grammar_text_terminal → • terminal_def_atom_text, ,/>]", "[terminal_def_atom_text → • SYMBOL_TERMINAL_TEXT, ,/>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30)},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x77, 0x78, 0xA3, 0xA5},
               new ushort[7] {0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[7] {0x12F, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_m93 → | terminal_def_restrict _m93 •, ;/)/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x4F]), new Reduction(0x44, staticRules[0x4F]), new Reduction(0x4b, staticRules[0x4F])})
            , new State(
               new string[1] {"[_m91 → - terminal_def_fragment _m91 •, ;/)/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[4] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4D]), new Reduction(0x44, staticRules[0x4D]), new Reduction(0x4a, staticRules[0x4D]), new Reduction(0x4b, staticRules[0x4D])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → { INTEGER , • INTEGER }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("INTEGER", 0x2D)},
               new ushort[1] {0x2d},
               new ushort[1] {0x130},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → { INTEGER } •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x27]), new Reduction(0xb, staticRules[0x27]), new Reduction(0x30, staticRules[0x27]), new Reduction(0x31, staticRules[0x27]), new Reduction(0x32, staticRules[0x27]), new Reduction(0x33, staticRules[0x27]), new Reduction(0x34, staticRules[0x27]), new Reduction(0x35, staticRules[0x27]), new Reduction(0x41, staticRules[0x27]), new Reduction(0x43, staticRules[0x27]), new Reduction(0x44, staticRules[0x27]), new Reduction(0x49, staticRules[0x27]), new Reduction(0x4a, staticRules[0x27]), new Reduction(0x4b, staticRules[0x27])})
            , new State(
               new string[1] {"[cs_rule_simple<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ; •, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAA]), new Reduction(0x12, staticRules[0xAA]), new Reduction(0xda, staticRules[0xAA])})
            , new State(
               new string[1] {"[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> • ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x131},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_ref_params<grammar_bin_terminal> → < rule_def_atom<grammar_bin_terminal> _m175 > •, NAME/{/QUOTED_DATA/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/SYMBOL_VALUE_UINT32/SYMBOL_VALUE_UINT64/SYMBOL_VALUE_UINT128/SYMBOL_VALUE_BINARY/SYMBOL_JOKER_UINT8/SYMBOL_JOKER_UINT16/SYMBOL_JOKER_UINT32/SYMBOL_JOKER_UINT64/SYMBOL_JOKER_UINT128/SYMBOL_JOKER_BINARY/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[28] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x92]), new Reduction(0x11, staticRules[0x92]), new Reduction(0x2e, staticRules[0x92]), new Reduction(0x34, staticRules[0x92]), new Reduction(0x35, staticRules[0x92]), new Reduction(0x36, staticRules[0x92]), new Reduction(0x37, staticRules[0x92]), new Reduction(0x38, staticRules[0x92]), new Reduction(0x39, staticRules[0x92]), new Reduction(0x3a, staticRules[0x92]), new Reduction(0x3b, staticRules[0x92]), new Reduction(0x3c, staticRules[0x92]), new Reduction(0x3d, staticRules[0x92]), new Reduction(0x3e, staticRules[0x92]), new Reduction(0x3f, staticRules[0x92]), new Reduction(0x41, staticRules[0x92]), new Reduction(0x43, staticRules[0x92]), new Reduction(0x44, staticRules[0x92]), new Reduction(0x45, staticRules[0x92]), new Reduction(0x46, staticRules[0x92]), new Reduction(0x47, staticRules[0x92]), new Reduction(0x48, staticRules[0x92]), new Reduction(0x49, staticRules[0x92]), new Reduction(0x4a, staticRules[0x92]), new Reduction(0x4e, staticRules[0x92]), new Reduction(0x55, staticRules[0x92]), new Reduction(0x56, staticRules[0x92]), new Reduction(0xdb, staticRules[0x92])})
            , new State(
               new string[3] {"[_m175 → , rule_def_atom<grammar_bin_terminal> • _m175, >]", "[_m175 → • , rule_def_atom<grammar_bin_terminal> _m175, >]", "[_m175 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x116},
               new ushort[1] {0x99},
               new ushort[1] {0x132},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x94])})
            , new State(
               new string[1] {"[cf_rule_template<grammar_text_terminal> → NAME rule_template_params -> rule_definition<grammar_text_terminal> ; •, NAME/}]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x78]), new Reduction(0x12, staticRules[0x78])})
            , new State(
               new string[1] {"[_m101 → , NAME _m101 •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x51])})
            , new State(
               new string[1] {"[cs_rule_simple<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ; •, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA2]), new Reduction(0x12, staticRules[0xA2]), new Reduction(0xda, staticRules[0xA2])})
            , new State(
               new string[1] {"[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> • ;, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41)},
               new ushort[1] {0x41},
               new ushort[1] {0x133},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[rule_sym_ref_params<grammar_text_terminal> → < rule_def_atom<grammar_text_terminal> _m134 > •, NAME/{/QUOTED_DATA/SYMBOL_TERMINAL_TEXT/;/(/)/*/+/?/,/-/|/>/^/!/]]"},
               new Hime.Redist.Parsers.SymbolTerminal[17] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[{]", 0x11), new Hime.Redist.Parsers.SymbolTerminal("QUOTED_DATA", 0x2E), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0x45), new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x46), new Hime.Redist.Parsers.SymbolTerminal("_T[?]", 0x47), new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E), new Hime.Redist.Parsers.SymbolTerminal("_T[^]", 0x55), new Hime.Redist.Parsers.SymbolTerminal("_T[!]", 0x56), new Hime.Redist.Parsers.SymbolTerminal("_T[]]", 0xDB)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6F]), new Reduction(0x11, staticRules[0x6F]), new Reduction(0x2e, staticRules[0x6F]), new Reduction(0x30, staticRules[0x6F]), new Reduction(0x41, staticRules[0x6F]), new Reduction(0x43, staticRules[0x6F]), new Reduction(0x44, staticRules[0x6F]), new Reduction(0x45, staticRules[0x6F]), new Reduction(0x46, staticRules[0x6F]), new Reduction(0x47, staticRules[0x6F]), new Reduction(0x48, staticRules[0x6F]), new Reduction(0x49, staticRules[0x6F]), new Reduction(0x4a, staticRules[0x6F]), new Reduction(0x4e, staticRules[0x6F]), new Reduction(0x55, staticRules[0x6F]), new Reduction(0x56, staticRules[0x6F]), new Reduction(0xdb, staticRules[0x6F])})
            , new State(
               new string[3] {"[_m134 → , rule_def_atom<grammar_text_terminal> • _m134, >]", "[_m134 → • , rule_def_atom<grammar_text_terminal> _m134, >]", "[_m134 → •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[2] {new Hime.Redist.Parsers.SymbolTerminal("_T[,]", 0x48), new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[1] {0x48},
               new ushort[1] {0x121},
               new ushort[1] {0x87},
               new ushort[1] {0x134},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x71])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → { INTEGER , INTEGER • }, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12)},
               new ushort[1] {0x12},
               new ushort[1] {0x135},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[1] {"[cs_rule_template<grammar_bin_terminal> → cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ; •, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAD]), new Reduction(0x12, staticRules[0xAD]), new Reduction(0xda, staticRules[0xAD])})
            , new State(
               new string[1] {"[_m175 → , rule_def_atom<grammar_bin_terminal> _m175 •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x93])})
            , new State(
               new string[1] {"[cs_rule_template<grammar_text_terminal> → cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ; •, NAME/}/[]"},
               new Hime.Redist.Parsers.SymbolTerminal[3] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[}]", 0x12), new Hime.Redist.Parsers.SymbolTerminal("_T[[]", 0xDA)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA5]), new Reduction(0x12, staticRules[0xA5]), new Reduction(0xda, staticRules[0xA5])})
            , new State(
               new string[1] {"[_m134 → , rule_def_atom<grammar_text_terminal> _m134 •, >]"},
               new Hime.Redist.Parsers.SymbolTerminal[1] {new Hime.Redist.Parsers.SymbolTerminal("_T[>]", 0x4E)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x70])})
            , new State(
               new string[1] {"[terminal_def_cardinalilty → { INTEGER , INTEGER } •, NAME/./SYMBOL_TERMINAL_TEXT/SYMBOL_TERMINAL_SET/SYMBOL_TERMINAL_UBLOCK/SYMBOL_TERMINAL_UCAT/SYMBOL_VALUE_UINT8/SYMBOL_VALUE_UINT16/;/(/)/-/|/=>]"},
               new Hime.Redist.Parsers.SymbolTerminal[14] {new Hime.Redist.Parsers.SymbolTerminal("NAME", 0xA), new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0xB), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34), new Hime.Redist.Parsers.SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35), new Hime.Redist.Parsers.SymbolTerminal("_T[;]", 0x41), new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x43), new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0x44), new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x49), new Hime.Redist.Parsers.SymbolTerminal("_T[|]", 0x4A), new Hime.Redist.Parsers.SymbolTerminal("_T[=>]", 0x4B)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x26]), new Reduction(0xb, staticRules[0x26]), new Reduction(0x30, staticRules[0x26]), new Reduction(0x31, staticRules[0x26]), new Reduction(0x32, staticRules[0x26]), new Reduction(0x33, staticRules[0x26]), new Reduction(0x34, staticRules[0x26]), new Reduction(0x35, staticRules[0x26]), new Reduction(0x41, staticRules[0x26]), new Reduction(0x43, staticRules[0x26]), new Reduction(0x44, staticRules[0x26]), new Reduction(0x49, staticRules[0x26]), new Reduction(0x4a, staticRules[0x26]), new Reduction(0x4b, staticRules[0x26])})
        };
        protected override void setup()
        {
            rules = staticRules;
            states = staticStates;
            errorSimulationLength = 3;
        }
        public FileCentralDogma_Parser(FileCentralDogma_Lexer lexer) : base (lexer) {}
    }
}
