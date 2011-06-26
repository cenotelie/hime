using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Hime.Kernel.Resources.Parser
{
    class FileCentralDogma_Lexer : LexerText
    {
        public static readonly SymbolTerminal[] terminals = {
            new SymbolTerminal("ε", 0x1),
            new SymbolTerminal("$", 0x2),
            new SymbolTerminal("_T[.]", 0xB),
            new SymbolTerminal("NAME", 0xA),
            new SymbolTerminal("_T[{]", 0x11),
            new SymbolTerminal("_T[}]", 0x12),
            new SymbolTerminal("_T[[]", 0xDA),
            new SymbolTerminal("INTEGER", 0x2D),
            new SymbolTerminal("_T[=]", 0x40),
            new SymbolTerminal("_T[;]", 0x41),
            new SymbolTerminal("_T[(]", 0x43),
            new SymbolTerminal("_T[)]", 0x44),
            new SymbolTerminal("_T[*]", 0x45),
            new SymbolTerminal("_T[+]", 0x46),
            new SymbolTerminal("_T[?]", 0x47),
            new SymbolTerminal("_T[,]", 0x48),
            new SymbolTerminal("_T[-]", 0x49),
            new SymbolTerminal("_T[|]", 0x4A),
            new SymbolTerminal("_T[<]", 0x4D),
            new SymbolTerminal("_T[>]", 0x4E),
            new SymbolTerminal("_T[:]", 0x51),
            new SymbolTerminal("_T[^]", 0x55),
            new SymbolTerminal("_T[!]", 0x56),
            new SymbolTerminal("_T[]]", 0xDB),
            new SymbolTerminal("SEPARATOR", 0x7),
            new SymbolTerminal("_T[..]", 0x42),
            new SymbolTerminal("QUOTED_DATA", 0x2E),
            new SymbolTerminal("ESCAPEES", 0x2F),
            new SymbolTerminal("_T[=>]", 0x4B),
            new SymbolTerminal("_T[->]", 0x4C),
            new SymbolTerminal("_T[cf]", 0x53),
            new SymbolTerminal("_T[cs]", 0xD9),
            new SymbolTerminal("SYMBOL_TERMINAL_TEXT", 0x30),
            new SymbolTerminal("SYMBOL_TERMINAL_SET", 0x31),
            new SymbolTerminal("SYMBOL_VALUE_BINARY", 0x39),
            new SymbolTerminal("SYMBOL_JOKER_BINARY", 0x3F),
            new SymbolTerminal("SYMBOL_VALUE_UINT8", 0x34),
            new SymbolTerminal("SYMBOL_JOKER_UINT8", 0x3A),
            new SymbolTerminal("_T[rules]", 0x54),
            new SymbolTerminal("_T[public]", 0xC),
            new SymbolTerminal("SYMBOL_TERMINAL_UBLOCK", 0x32),
            new SymbolTerminal("SYMBOL_TERMINAL_UCAT", 0x33),
            new SymbolTerminal("SYMBOL_VALUE_UINT16", 0x35),
            new SymbolTerminal("SYMBOL_JOKER_UINT16", 0x3B),
            new SymbolTerminal("_T[private]", 0xD),
            new SymbolTerminal("_T[options]", 0x4F),
            new SymbolTerminal("_T[grammar]", 0x52),
            new SymbolTerminal("_T[internal]", 0xF),
            new SymbolTerminal("_T[protected]", 0xE),
            new SymbolTerminal("_T[namespace]", 0x10),
            new SymbolTerminal("_T[terminals]", 0x50),
            new SymbolTerminal("SYMBOL_VALUE_UINT32", 0x36),
            new SymbolTerminal("SYMBOL_JOKER_UINT32", 0x3C),
            new SymbolTerminal("SYMBOL_VALUE_UINT64", 0x37),
            new SymbolTerminal("SYMBOL_JOKER_UINT64", 0x3D),
            new SymbolTerminal("SYMBOL_VALUE_UINT128", 0x38),
            new SymbolTerminal("SYMBOL_JOKER_UINT128", 0x3E) };
        private static State[] staticStates = { 
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x1 },
                new ushort[3] { 0x2E, 0x2E, 0x54 },
                new ushort[3] { 0x70, 0x70, 0x55 },
                new ushort[3] { 0x69, 0x69, 0x56 },
                new ushort[3] { 0x6E, 0x6E, 0x57 },
                new ushort[3] { 0x7B, 0x7B, 0x5E },
                new ushort[3] { 0x7D, 0x7D, 0x5F },
                new ushort[3] { 0x22, 0x22, 0x3 },
                new ushort[3] { 0x27, 0x27, 0x4 },
                new ushort[3] { 0x5B, 0x5B, 0x60 },
                new ushort[3] { 0x5C, 0x5C, 0x5 },
                new ushort[3] { 0x30, 0x30, 0x61 },
                new ushort[3] { 0x3D, 0x3D, 0x63 },
                new ushort[3] { 0x3B, 0x3B, 0x64 },
                new ushort[3] { 0x28, 0x28, 0x65 },
                new ushort[3] { 0x29, 0x29, 0x66 },
                new ushort[3] { 0x2A, 0x2A, 0x67 },
                new ushort[3] { 0x2B, 0x2B, 0x68 },
                new ushort[3] { 0x3F, 0x3F, 0x69 },
                new ushort[3] { 0x2C, 0x2C, 0x6A },
                new ushort[3] { 0x2D, 0x2D, 0x6B },
                new ushort[3] { 0x7C, 0x7C, 0x6C },
                new ushort[3] { 0x3C, 0x3C, 0x6D },
                new ushort[3] { 0x3E, 0x3E, 0x6E },
                new ushort[3] { 0x6F, 0x6F, 0x58 },
                new ushort[3] { 0x74, 0x74, 0x59 },
                new ushort[3] { 0x3A, 0x3A, 0x6F },
                new ushort[3] { 0x67, 0x67, 0x5A },
                new ushort[3] { 0x63, 0x63, 0x5B },
                new ushort[3] { 0x72, 0x72, 0x5C },
                new ushort[3] { 0x5E, 0x5E, 0x70 },
                new ushort[3] { 0x21, 0x21, 0x71 },
                new ushort[3] { 0x5D, 0x5D, 0x72 },
                new ushort[3] { 0xA, 0xA, 0x73 },
                new ushort[3] { 0x2028, 0x2029, 0x73 },
                new ushort[3] { 0x9, 0x9, 0x75 },
                new ushort[3] { 0xB, 0xC, 0x75 },
                new ushort[3] { 0x20, 0x20, 0x75 },
                new ushort[3] { 0x41, 0x5A, 0x5D },
                new ushort[3] { 0x5F, 0x5F, 0x5D },
                new ushort[3] { 0x61, 0x62, 0x5D },
                new ushort[3] { 0x64, 0x66, 0x5D },
                new ushort[3] { 0x68, 0x68, 0x5D },
                new ushort[3] { 0x6A, 0x6D, 0x5D },
                new ushort[3] { 0x71, 0x71, 0x5D },
                new ushort[3] { 0x73, 0x73, 0x5D },
                new ushort[3] { 0x75, 0x7A, 0x5D },
                new ushort[3] { 0x370, 0x3FF, 0x5D },
                new ushort[3] { 0x31, 0x39, 0x62 },
                new ushort[3] { 0x40, 0x40, 0x6 },
                new ushort[3] { 0xD, 0xD, 0x74 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x7 },
                new ushort[3] { 0x2A, 0x2A, 0x8 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x9 },
                new ushort[3] { 0x2F, 0x2F, 0x18 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x0, 0x21, 0x3 },
                new ushort[3] { 0x23, 0xFFFF, 0x3 },
                new ushort[3] { 0x22, 0x22, 0xA9 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x5C, 0x5C, 0xA },
                new ushort[3] { 0x0, 0x26, 0xB },
                new ushort[3] { 0x28, 0x5B, 0xB },
                new ushort[3] { 0x5D, 0xFFFF, 0xB }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0xE },
                new ushort[3] { 0x30, 0x30, 0xAA },
                new ushort[3] { 0x5C, 0x5C, 0xAA },
                new ushort[3] { 0x61, 0x62, 0xAA },
                new ushort[3] { 0x66, 0x66, 0xAA },
                new ushort[3] { 0x6E, 0x6E, 0xAA },
                new ushort[3] { 0x72, 0x72, 0xAA },
                new ushort[3] { 0x74, 0x74, 0xAA },
                new ushort[3] { 0x76, 0x76, 0xAA }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x81 },
                new ushort[3] { 0x5F, 0x5F, 0x81 },
                new ushort[3] { 0x61, 0x7A, 0x81 },
                new ushort[3] { 0x370, 0x3FF, 0x81 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x0, 0x9, 0x7 },
                new ushort[3] { 0xB, 0xC, 0x7 },
                new ushort[3] { 0xE, 0x2027, 0x7 },
                new ushort[3] { 0x202A, 0xFFFF, 0x7 },
                new ushort[3] { 0xA, 0xA, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 },
                new ushort[3] { 0xD, 0xD, 0xB2 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0xFFFF, 0x12 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0xFFFF, 0x13 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x27, 0x27, 0xB },
                new ushort[3] { 0x30, 0x30, 0xB },
                new ushort[3] { 0x5C, 0x5C, 0xB },
                new ushort[3] { 0x61, 0x62, 0xB },
                new ushort[3] { 0x66, 0x66, 0xB },
                new ushort[3] { 0x6E, 0x6E, 0xB },
                new ushort[3] { 0x72, 0x72, 0xB },
                new ushort[3] { 0x74, 0x74, 0xB },
                new ushort[3] { 0x76, 0x76, 0xB }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x27, 0x27, 0xB3 },
                new ushort[3] { 0x5C, 0x5C, 0xA },
                new ushort[3] { 0x0, 0x26, 0xB },
                new ushort[3] { 0x28, 0x5B, 0xB },
                new ushort[3] { 0x5D, 0xFFFF, 0xB }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x30, 0xD },
                new ushort[3] { 0x5B, 0x5D, 0xD },
                new ushort[3] { 0x61, 0x62, 0xD },
                new ushort[3] { 0x66, 0x66, 0xD },
                new ushort[3] { 0x6E, 0x6E, 0xD },
                new ushort[3] { 0x72, 0x72, 0xD },
                new ushort[3] { 0x74, 0x74, 0xD },
                new ushort[3] { 0x76, 0x76, 0xD }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x5D, 0x5D, 0xB4 },
                new ushort[3] { 0x5C, 0x5C, 0xC },
                new ushort[3] { 0x0, 0x5A, 0xD },
                new ushort[3] { 0x5E, 0xFFFF, 0xD }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x62, 0x62, 0x14 },
                new ushort[3] { 0x63, 0x63, 0x15 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x16 },
                new ushort[3] { 0x41, 0x46, 0x16 },
                new ushort[3] { 0x61, 0x66, 0x16 },
                new ushort[3] { 0x58, 0x58, 0x17 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x31, 0xB5 },
                new ushort[3] { 0x42, 0x42, 0xB6 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0x2E, 0x12 },
                new ushort[3] { 0x30, 0xFFFF, 0x12 },
                new ushort[3] { 0x2F, 0x2F, 0xB7 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0xFFFF, 0x12 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0xFFFF, 0x13 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x7B, 0x7B, 0x19 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x7B, 0x7B, 0x1A }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xB8 },
                new ushort[3] { 0x41, 0x46, 0xB8 },
                new ushort[3] { 0x61, 0x66, 0xB8 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xB9 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x0, 0x9, 0x18 },
                new ushort[3] { 0xB, 0xC, 0x18 },
                new ushort[3] { 0xE, 0x2027, 0x18 },
                new ushort[3] { 0x202A, 0xFFFF, 0x18 },
                new ushort[3] { 0xA, 0xA, 0xAF },
                new ushort[3] { 0xD, 0xD, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x40, 0x40, 0x1C },
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x40, 0x40, 0x1D },
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0x2E, 0x13 },
                new ushort[3] { 0x30, 0xFFFF, 0x13 },
                new ushort[3] { 0x2F, 0x2F, 0xAF }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x1E },
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E },
                new ushort[3] { 0x7D, 0x7D, 0xBC }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x1F },
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F },
                new ushort[3] { 0x7D, 0x7D, 0xBD }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xBE },
                new ushort[3] { 0x41, 0x46, 0xBE },
                new ushort[3] { 0x61, 0x66, 0xBE }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xBF }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x35 },
                new ushort[3] { 0x41, 0x46, 0x35 },
                new ushort[3] { 0x61, 0x66, 0x35 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2D },
                new ushort[3] { 0x41, 0x46, 0x2D },
                new ushort[3] { 0x61, 0x66, 0x2D }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x25 },
                new ushort[3] { 0x41, 0x46, 0x25 },
                new ushort[3] { 0x61, 0x66, 0x25 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x26 },
                new ushort[3] { 0x41, 0x46, 0x26 },
                new ushort[3] { 0x61, 0x66, 0x26 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x27 },
                new ushort[3] { 0x41, 0x46, 0x27 },
                new ushort[3] { 0x61, 0x66, 0x27 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x28 },
                new ushort[3] { 0x41, 0x46, 0x28 },
                new ushort[3] { 0x61, 0x66, 0x28 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x29 },
                new ushort[3] { 0x41, 0x46, 0x29 },
                new ushort[3] { 0x61, 0x66, 0x29 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2A },
                new ushort[3] { 0x41, 0x46, 0x2A },
                new ushort[3] { 0x61, 0x66, 0x2A }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2B },
                new ushort[3] { 0x41, 0x46, 0x2B },
                new ushort[3] { 0x61, 0x66, 0x2B }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2C },
                new ushort[3] { 0x41, 0x46, 0x2C },
                new ushort[3] { 0x61, 0x66, 0x2C }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2E },
                new ushort[3] { 0x41, 0x46, 0x2E },
                new ushort[3] { 0x61, 0x66, 0x2E }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2F },
                new ushort[3] { 0x41, 0x46, 0x2F },
                new ushort[3] { 0x61, 0x66, 0x2F }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x30 },
                new ushort[3] { 0x41, 0x46, 0x30 },
                new ushort[3] { 0x61, 0x66, 0x30 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x31 },
                new ushort[3] { 0x41, 0x46, 0x31 },
                new ushort[3] { 0x61, 0x66, 0x31 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x32 },
                new ushort[3] { 0x41, 0x46, 0x32 },
                new ushort[3] { 0x61, 0x66, 0x32 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x33 },
                new ushort[3] { 0x41, 0x46, 0x33 },
                new ushort[3] { 0x61, 0x66, 0x33 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x34 },
                new ushort[3] { 0x41, 0x46, 0x34 },
                new ushort[3] { 0x61, 0x66, 0x34 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x36 },
                new ushort[3] { 0x41, 0x46, 0x36 },
                new ushort[3] { 0x61, 0x66, 0x36 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x37 },
                new ushort[3] { 0x41, 0x46, 0x37 },
                new ushort[3] { 0x61, 0x66, 0x37 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x4E },
                new ushort[3] { 0x41, 0x46, 0x4E },
                new ushort[3] { 0x61, 0x66, 0x4E }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x50 },
                new ushort[3] { 0x41, 0x46, 0x50 },
                new ushort[3] { 0x61, 0x66, 0x50 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x52 },
                new ushort[3] { 0x41, 0x46, 0x52 },
                new ushort[3] { 0x61, 0x66, 0x52 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4B }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x43 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3B }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3C }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3D }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3E }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3F }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x40 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x41 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x42 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x44 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x45 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x46 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x47 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x48 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x49 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4A }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4C }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4D }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4F }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x51 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x53 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xC7 },
                new ushort[3] { 0x41, 0x46, 0xC7 },
                new ushort[3] { 0x61, 0x66, 0xC7 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xC8 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xC9 },
                new ushort[3] { 0x41, 0x46, 0xC9 },
                new ushort[3] { 0x61, 0x66, 0xC9 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xCA }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xCB },
                new ushort[3] { 0x41, 0x46, 0xCB },
                new ushort[3] { 0x61, 0x66, 0xCB }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xCC }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2E, 0x2E, 0x76 }},
                terminals[0x2]),
            new State(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0x77 },
                new ushort[3] { 0x72, 0x72, 0x78 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x71, 0x79 },
                new ushort[3] { 0x73, 0x74, 0x79 },
                new ushort[3] { 0x76, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0x7A },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6D, 0x79 },
                new ushort[3] { 0x6F, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x7B },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x7C },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6F, 0x79 },
                new ushort[3] { 0x71, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x7D },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x7E },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x71, 0x79 },
                new ushort[3] { 0x73, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x66, 0x66, 0xAD },
                new ushort[3] { 0x73, 0x73, 0xAE },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x65, 0x79 },
                new ushort[3] { 0x67, 0x72, 0x79 },
                new ushort[3] { 0x74, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0x7F },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x74, 0x79 },
                new ushort[3] { 0x76, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {}, terminals[0x4]),
            new State(new ushort[][] {}, terminals[0x5]),
            new State(new ushort[][] {
                new ushort[3] { 0x5C, 0x5C, 0xC },
                new ushort[3] { 0x0, 0x5A, 0xD },
                new ushort[3] { 0x5E, 0xFFFF, 0xD }},
                terminals[0x6]),
            new State(new ushort[][] {
                new ushort[3] { 0x78, 0x78, 0xF },
                new ushort[3] { 0x62, 0x62, 0x10 }},
                terminals[0x7]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xB0 }},
                terminals[0x7]),
            new State(new ushort[][] {
                new ushort[3] { 0x3E, 0x3E, 0xAB }},
                terminals[0x8]),
            new State(new ushort[][] {}, terminals[0x9]),
            new State(new ushort[][] {}, terminals[0xA]),
            new State(new ushort[][] {}, terminals[0xB]),
            new State(new ushort[][] {}, terminals[0xC]),
            new State(new ushort[][] {}, terminals[0xD]),
            new State(new ushort[][] {}, terminals[0xE]),
            new State(new ushort[][] {}, terminals[0xF]),
            new State(new ushort[][] {
                new ushort[3] { 0x3E, 0x3E, 0xAC }},
                terminals[0x10]),
            new State(new ushort[][] {}, terminals[0x11]),
            new State(new ushort[][] {}, terminals[0x12]),
            new State(new ushort[][] {}, terminals[0x13]),
            new State(new ushort[][] {}, terminals[0x14]),
            new State(new ushort[][] {}, terminals[0x15]),
            new State(new ushort[][] {}, terminals[0x16]),
            new State(new ushort[][] {}, terminals[0x17]),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {
                new ushort[3] { 0xA, 0xA, 0x73 },
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0x9, 0xAF },
                new ushort[3] { 0xB, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {}, terminals[0x19]),
            new State(new ushort[][] {
                new ushort[3] { 0x62, 0x62, 0x80 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x61, 0x79 },
                new ushort[3] { 0x63, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x82 },
                new ushort[3] { 0x6F, 0x6F, 0x83 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x68, 0x79 },
                new ushort[3] { 0x6A, 0x6E, 0x79 },
                new ushort[3] { 0x70, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x84 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x73, 0x79 },
                new ushort[3] { 0x75, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x85 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6C, 0x79 },
                new ushort[3] { 0x6E, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x86 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x73, 0x79 },
                new ushort[3] { 0x75, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x87 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x71, 0x79 },
                new ushort[3] { 0x73, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x88 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0x89 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6B, 0x79 },
                new ushort[3] { 0x6D, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0x8A },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6B, 0x79 },
                new ushort[3] { 0x6D, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x76, 0x76, 0x8B },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x75, 0x79 },
                new ushort[3] { 0x77, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x8C },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x73, 0x79 },
                new ushort[3] { 0x75, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x8D },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x8E },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x8F },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x68, 0x79 },
                new ushort[3] { 0x6A, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x90 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6C, 0x79 },
                new ushort[3] { 0x6E, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x91 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6C, 0x79 },
                new ushort[3] { 0x6E, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x92 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x93 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x68, 0x79 },
                new ushort[3] { 0x6A, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x94 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x95 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x96 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x71, 0x79 },
                new ushort[3] { 0x73, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0x98 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x72, 0x79 },
                new ushort[3] { 0x74, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6F, 0x6F, 0x99 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6E, 0x79 },
                new ushort[3] { 0x70, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x97 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x68, 0x79 },
                new ushort[3] { 0x6A, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x9A },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6C, 0x79 },
                new ushort[3] { 0x6E, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xBA },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x72, 0x79 },
                new ushort[3] { 0x74, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0xBB },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x62, 0x79 },
                new ushort[3] { 0x64, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0xA2 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x73, 0x79 },
                new ushort[3] { 0x75, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0x9B },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x62, 0x79 },
                new ushort[3] { 0x64, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0x9C },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6D, 0x79 },
                new ushort[3] { 0x6F, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0x9E },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6D, 0x79 },
                new ushort[3] { 0x6F, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x9D },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6F, 0x79 },
                new ushort[3] { 0x71, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0xA3 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6D, 0x79 },
                new ushort[3] { 0x6F, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA4 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x9F },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x73, 0x79 },
                new ushort[3] { 0x75, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA5 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA0 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA1 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x62, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xA6 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0xA7 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x62, 0x79 },
                new ushort[3] { 0x64, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0xA8 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6B, 0x79 },
                new ushort[3] { 0x6D, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xC0 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xC1 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x72, 0x79 },
                new ushort[3] { 0x74, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0xC2 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x71, 0x79 },
                new ushort[3] { 0x73, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0xC3 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x6B, 0x79 },
                new ushort[3] { 0x6D, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x64, 0x64, 0xC4 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x63, 0x79 },
                new ushort[3] { 0x65, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xC5 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x64, 0x79 },
                new ushort[3] { 0x66, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xC6 },
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x72, 0x79 },
                new ushort[3] { 0x74, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x3]),
            new State(new ushort[][] {}, terminals[0x1A]),
            new State(new ushort[][] {}, terminals[0x1B]),
            new State(new ushort[][] {}, terminals[0x1C]),
            new State(new ushort[][] {}, terminals[0x1D]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x1E]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x1F]),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xB0 }},
                terminals[0x7]),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {
                new ushort[3] { 0xA, 0xA, 0xB1 },
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0x9, 0xAF },
                new ushort[3] { 0xB, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {}, terminals[0x20]),
            new State(new ushort[][] {}, terminals[0x21]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x31, 0xB5 }},
                terminals[0x22]),
            new State(new ushort[][] {
                new ushort[3] { 0x42, 0x42, 0xB6 }},
                terminals[0x23]),
            new State(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xAF },
                new ushort[3] { 0x20, 0x20, 0xAF },
                new ushort[3] { 0x2028, 0x2029, 0xAF }},
                terminals[0x18]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x20 },
                new ushort[3] { 0x41, 0x46, 0x20 },
                new ushort[3] { 0x61, 0x66, 0x20 }},
                terminals[0x24]),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x21 }},
                terminals[0x25]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x26]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x27]),
            new State(new ushort[][] {}, terminals[0x28]),
            new State(new ushort[][] {}, terminals[0x29]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x22 },
                new ushort[3] { 0x41, 0x46, 0x22 },
                new ushort[3] { 0x61, 0x66, 0x22 }},
                terminals[0x2A]),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x38 }},
                terminals[0x2B]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x2C]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x2D]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x2E]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x2F]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x30]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x31]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x79 },
                new ushort[3] { 0x41, 0x5A, 0x79 },
                new ushort[3] { 0x5F, 0x5F, 0x79 },
                new ushort[3] { 0x61, 0x7A, 0x79 },
                new ushort[3] { 0x370, 0x3FF, 0x79 }},
                terminals[0x32]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x23 },
                new ushort[3] { 0x41, 0x46, 0x23 },
                new ushort[3] { 0x61, 0x66, 0x23 }},
                terminals[0x33]),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x39 }},
                terminals[0x34]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x24 },
                new ushort[3] { 0x41, 0x46, 0x24 },
                new ushort[3] { 0x61, 0x66, 0x24 }},
                terminals[0x35]),
            new State(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3A }},
                terminals[0x36]),
            new State(new ushort[][] {}, terminals[0x37]),
            new State(new ushort[][] {}, terminals[0x38]) };
        protected override void setup() {
            states = staticStates;
            subGrammars = new Dictionary<ushort, MatchSubGrammar>();
            separatorID = 0x7;
        }
        public override ILexer Clone() {
            return new FileCentralDogma_Lexer(this);
        }
        public FileCentralDogma_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public FileCentralDogma_Lexer(System.IO.TextReader input) : base(input) {}
        public FileCentralDogma_Lexer(FileCentralDogma_Lexer original) : base(original) {}
    }
    class FileCentralDogma_Parser : LR1TextParser
    {
        public static readonly SymbolVariable[] variables = {
            new SymbolVariable(0x13, "qualified_name"), 
            new SymbolVariable(0x14, "symbol_access_public"), 
            new SymbolVariable(0x15, "symbol_access_private"), 
            new SymbolVariable(0x16, "symbol_access_protected"), 
            new SymbolVariable(0x17, "symbol_access_internal"), 
            new SymbolVariable(0x18, "Namespace_child_symbol"), 
            new SymbolVariable(0x19, "Namespace_content"), 
            new SymbolVariable(0x1A, "Namespace"), 
            new SymbolVariable(0x1B, "_m20"), 
            new SymbolVariable(0x1C, "_m25"), 
            new SymbolVariable(0x57, "option"), 
            new SymbolVariable(0x58, "terminal_def_atom_any"), 
            new SymbolVariable(0x59, "terminal_def_atom_unicode"), 
            new SymbolVariable(0x5A, "terminal_def_atom_text"), 
            new SymbolVariable(0x5B, "terminal_def_atom_set"), 
            new SymbolVariable(0x5C, "terminal_def_atom_ublock"), 
            new SymbolVariable(0x5D, "terminal_def_atom_ucat"), 
            new SymbolVariable(0x5E, "terminal_def_atom_span"), 
            new SymbolVariable(0x5F, "terminal_def_atom"), 
            new SymbolVariable(0x60, "terminal_def_element"), 
            new SymbolVariable(0x61, "terminal_def_cardinalilty"), 
            new SymbolVariable(0x62, "terminal_def_repetition"), 
            new SymbolVariable(0x63, "terminal_def_fragment"), 
            new SymbolVariable(0x64, "terminal_def_restrict"), 
            new SymbolVariable(0x65, "terminal_definition"), 
            new SymbolVariable(0x66, "terminal_subgrammar"), 
            new SymbolVariable(0x67, "terminal"), 
            new SymbolVariable(0x68, "rule_sym_action"), 
            new SymbolVariable(0x69, "rule_sym_virtual"), 
            new SymbolVariable(0x6A, "rule_sym_ref_simple"), 
            new SymbolVariable(0x6B, "rule_template_params"), 
            new SymbolVariable(0x6C, "grammar_bin_terminal"), 
            new SymbolVariable(0x6D, "grammar_text_terminal"), 
            new SymbolVariable(0x6E, "grammar_options"), 
            new SymbolVariable(0x6F, "grammar_terminals"), 
            new SymbolVariable(0x70, "grammar_parency"), 
            new SymbolVariable(0x71, "grammar_access"), 
            new SymbolVariable(0x72, "cf_grammar_text"), 
            new SymbolVariable(0x73, "cf_grammar_bin"), 
            new SymbolVariable(0x74, "_m89"), 
            new SymbolVariable(0x75, "_m91"), 
            new SymbolVariable(0x76, "_m93"), 
            new SymbolVariable(0x77, "_m101"), 
            new SymbolVariable(0x78, "_m105"), 
            new SymbolVariable(0x79, "_m109"), 
            new SymbolVariable(0x7A, "_m113"), 
            new SymbolVariable(0x7B, "grammar_cf_rules<grammar_text_terminal>"), 
            new SymbolVariable(0x7C, "cf_rule_simple<grammar_text_terminal>"), 
            new SymbolVariable(0x7D, "rule_definition<grammar_text_terminal>"), 
            new SymbolVariable(0x7E, "rule_def_choice<grammar_text_terminal>"), 
            new SymbolVariable(0x7F, "rule_def_restrict<grammar_text_terminal>"), 
            new SymbolVariable(0x80, "rule_def_fragment<grammar_text_terminal>"), 
            new SymbolVariable(0x81, "rule_def_repetition<grammar_text_terminal>"), 
            new SymbolVariable(0x82, "rule_def_tree_action<grammar_text_terminal>"), 
            new SymbolVariable(0x83, "rule_def_element<grammar_text_terminal>"), 
            new SymbolVariable(0x84, "rule_def_atom<grammar_text_terminal>"), 
            new SymbolVariable(0x85, "rule_sym_ref_template<grammar_text_terminal>"), 
            new SymbolVariable(0x86, "rule_sym_ref_params<grammar_text_terminal>"), 
            new SymbolVariable(0x87, "_m134"), 
            new SymbolVariable(0x88, "_m143"), 
            new SymbolVariable(0x89, "_m145"), 
            new SymbolVariable(0x8A, "_m147"), 
            new SymbolVariable(0x8B, "cf_rule_template<grammar_text_terminal>"), 
            new SymbolVariable(0x8C, "_m152"), 
            new SymbolVariable(0x8D, "grammar_cf_rules<grammar_bin_terminal>"), 
            new SymbolVariable(0x8E, "cf_rule_simple<grammar_bin_terminal>"), 
            new SymbolVariable(0x8F, "rule_definition<grammar_bin_terminal>"), 
            new SymbolVariable(0x90, "rule_def_choice<grammar_bin_terminal>"), 
            new SymbolVariable(0x91, "rule_def_restrict<grammar_bin_terminal>"), 
            new SymbolVariable(0x92, "rule_def_fragment<grammar_bin_terminal>"), 
            new SymbolVariable(0x93, "rule_def_repetition<grammar_bin_terminal>"), 
            new SymbolVariable(0x94, "rule_def_tree_action<grammar_bin_terminal>"), 
            new SymbolVariable(0x95, "rule_def_element<grammar_bin_terminal>"), 
            new SymbolVariable(0x96, "rule_def_atom<grammar_bin_terminal>"), 
            new SymbolVariable(0x97, "rule_sym_ref_template<grammar_bin_terminal>"), 
            new SymbolVariable(0x98, "rule_sym_ref_params<grammar_bin_terminal>"), 
            new SymbolVariable(0x99, "_m175"), 
            new SymbolVariable(0x9A, "_m184"), 
            new SymbolVariable(0x9B, "_m186"), 
            new SymbolVariable(0x9C, "_m188"), 
            new SymbolVariable(0x9D, "cf_rule_template<grammar_bin_terminal>"), 
            new SymbolVariable(0x9E, "_m193"), 
            new SymbolVariable(0xDC, "cs_grammar_text"), 
            new SymbolVariable(0xDD, "cs_grammar_bin"), 
            new SymbolVariable(0xDE, "grammar_cs_rules<grammar_text_terminal>"), 
            new SymbolVariable(0xDF, "cs_rule_simple<grammar_text_terminal>"), 
            new SymbolVariable(0xE0, "cs_rule_context<grammar_text_terminal>"), 
            new SymbolVariable(0xE1, "cs_rule_template<grammar_text_terminal>"), 
            new SymbolVariable(0xE2, "_m160"), 
            new SymbolVariable(0xE3, "grammar_cs_rules<grammar_bin_terminal>"), 
            new SymbolVariable(0xE4, "cs_rule_simple<grammar_bin_terminal>"), 
            new SymbolVariable(0xE5, "cs_rule_context<grammar_bin_terminal>"), 
            new SymbolVariable(0xE6, "cs_rule_template<grammar_bin_terminal>"), 
            new SymbolVariable(0xE7, "_m178"), 
            new SymbolVariable(0xE8, "file_item"), 
            new SymbolVariable(0xE9, "file"), 
            new SymbolVariable(0xEA, "_m234"), 
            new SymbolVariable(0xEB, "_Axiom_") };
        private static SyntaxTreeNode Production_13_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_14_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("access_public"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_15_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("access_private"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_16_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[3]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("access_protected"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_17_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[4]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("access_internal"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_18_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_18_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_18_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_18_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_18_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_19_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[6]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[7]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[8], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1B_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[8], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_1C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[9], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1C_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[9], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_57_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[10]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_58_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[11]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_59_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[12]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_59_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[12]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[13]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[14]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[15]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5D_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[16]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5E_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[17]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_5 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_6 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5F_7 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[18]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_60_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[19]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_60_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[19]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_61_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[20]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_61_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[20]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_61_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[20]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_61_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[20]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("range"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_61_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[20]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("range"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_62_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[21]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_62_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[21]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_63_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[22]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_64_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[23]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_65_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[24]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_66_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[25]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_66_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[25]);
            return root;
        }
        private static SyntaxTreeNode Production_67_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[26]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_68_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[27]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_69_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[28]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[29]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[30]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_5 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_6 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_7 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_8 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_9 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_A (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_B (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[31]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6D_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[32]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6E_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[33]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6F_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[34]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_70_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[35]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_70_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[35]);
            return root;
        }
        private static SyntaxTreeNode Production_71_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[36]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_71_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[36]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_71_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[36]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_71_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[36]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_72_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[37]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_73_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[38]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_74_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[39], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_74_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[39], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_75_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[40], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_75_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[40], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_76_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[41], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_76_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[41], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_77_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[42], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_77_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[42], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_78_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[43], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_78_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[43], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_79_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[44], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_79_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[44], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_7A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[45], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7A_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[45], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_7B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[46]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[47]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7D_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[48]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7E_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[49]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7E_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[49]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("emptypart"), SyntaxTreeNodeAction.Promote));
            return root;
        }
        private static SyntaxTreeNode Production_7F_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[50]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_80_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[51]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_81_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[52]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_81_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[52]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_81_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[52]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_81_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[52]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_82_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[53]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_82_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[53]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_82_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[53]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_83_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[54]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_83_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[54]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_84_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[55]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_84_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[55]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_84_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[55]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_84_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[55]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_84_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[55]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_85_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[56]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_86_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[57]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_87_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[58], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_87_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[58], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_88_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[59], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_88_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[59], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_89_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[60], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_89_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[60], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_8A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[61], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8A_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[61], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_8B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[62]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[63], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8C_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[63], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8C_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[63], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_8D_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[64]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8E_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[65]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8F_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[66]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_90_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[67]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_90_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[67]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("emptypart"), SyntaxTreeNodeAction.Promote));
            return root;
        }
        private static SyntaxTreeNode Production_91_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[68]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_92_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[69]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_93_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[70]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_93_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[70]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_93_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[70]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_93_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[70]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_94_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[71]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_94_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[71]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_94_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[71]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_95_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[72]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_95_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[72]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_96_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[73]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_96_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[73]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_96_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[73]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_96_3 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[73]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_96_4 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[73]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_97_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[74]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_98_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[75]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_99_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[76], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_99_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[76], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_9A_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[77], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9A_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[77], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_9B_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[78], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9B_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[78], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_9C_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[79], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9C_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[79], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_9D_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[80]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9E_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[81], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9E_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[81], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9E_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[81], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_DC_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[82]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_DD_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[83]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_DE_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[84]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_DF_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[85]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E0_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[86]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E0_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[86]);
            return root;
        }
        private static SyntaxTreeNode Production_E1_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[87]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E2_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[88], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E2_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[88], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E2_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[88], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_E3_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[89]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E4_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[90]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E5_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[91]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E5_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[91]);
            return root;
        }
        private static SyntaxTreeNode Production_E6_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[92]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E7_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[93], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E7_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[93], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E7_2 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[93], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_E8_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[94]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E9_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[95]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_EA_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[96], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_EA_1 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[96], SyntaxTreeNodeAction.Replace);
            return root;
        }
        private static SyntaxTreeNode Production_EB_0 (BaseLR1Parser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[97]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static Rule[] staticRules = {
           new Rule(Production_13_0, variables[0], 2)
           , new Rule(Production_14_0, variables[1], 1)
           , new Rule(Production_15_0, variables[2], 1)
           , new Rule(Production_16_0, variables[3], 1)
           , new Rule(Production_17_0, variables[4], 1)
           , new Rule(Production_18_0, variables[5], 1)
           , new Rule(Production_18_1, variables[5], 1)
           , new Rule(Production_18_2, variables[5], 1)
           , new Rule(Production_18_3, variables[5], 1)
           , new Rule(Production_18_4, variables[5], 1)
           , new Rule(Production_19_0, variables[6], 1)
           , new Rule(Production_1A_0, variables[7], 5)
           , new Rule(Production_1B_0, variables[8], 3)
           , new Rule(Production_1B_1, variables[8], 0)
           , new Rule(Production_1C_0, variables[9], 2)
           , new Rule(Production_1C_1, variables[9], 0)
           , new Rule(Production_57_0, variables[10], 4)
           , new Rule(Production_58_0, variables[11], 1)
           , new Rule(Production_59_0, variables[12], 1)
           , new Rule(Production_59_1, variables[12], 1)
           , new Rule(Production_5A_0, variables[13], 1)
           , new Rule(Production_5B_0, variables[14], 1)
           , new Rule(Production_5C_0, variables[15], 1)
           , new Rule(Production_5D_0, variables[16], 1)
           , new Rule(Production_5E_0, variables[17], 3)
           , new Rule(Production_5F_0, variables[18], 1)
           , new Rule(Production_5F_1, variables[18], 1)
           , new Rule(Production_5F_2, variables[18], 1)
           , new Rule(Production_5F_3, variables[18], 1)
           , new Rule(Production_5F_4, variables[18], 1)
           , new Rule(Production_5F_5, variables[18], 1)
           , new Rule(Production_5F_6, variables[18], 1)
           , new Rule(Production_5F_7, variables[18], 1)
           , new Rule(Production_60_0, variables[19], 1)
           , new Rule(Production_60_1, variables[19], 3)
           , new Rule(Production_61_0, variables[20], 1)
           , new Rule(Production_61_1, variables[20], 1)
           , new Rule(Production_61_2, variables[20], 1)
           , new Rule(Production_61_3, variables[20], 5)
           , new Rule(Production_61_4, variables[20], 3)
           , new Rule(Production_62_0, variables[21], 2)
           , new Rule(Production_62_1, variables[21], 1)
           , new Rule(Production_63_0, variables[22], 2)
           , new Rule(Production_64_0, variables[23], 2)
           , new Rule(Production_65_0, variables[24], 2)
           , new Rule(Production_66_0, variables[25], 2)
           , new Rule(Production_66_1, variables[25], 0)
           , new Rule(Production_67_0, variables[26], 5)
           , new Rule(Production_68_0, variables[27], 3)
           , new Rule(Production_69_0, variables[28], 1)
           , new Rule(Production_6A_0, variables[29], 1)
           , new Rule(Production_6B_0, variables[30], 4)
           , new Rule(Production_6C_0, variables[31], 1)
           , new Rule(Production_6C_1, variables[31], 1)
           , new Rule(Production_6C_2, variables[31], 1)
           , new Rule(Production_6C_3, variables[31], 1)
           , new Rule(Production_6C_4, variables[31], 1)
           , new Rule(Production_6C_5, variables[31], 1)
           , new Rule(Production_6C_6, variables[31], 1)
           , new Rule(Production_6C_7, variables[31], 1)
           , new Rule(Production_6C_8, variables[31], 1)
           , new Rule(Production_6C_9, variables[31], 1)
           , new Rule(Production_6C_A, variables[31], 1)
           , new Rule(Production_6C_B, variables[31], 1)
           , new Rule(Production_6D_0, variables[32], 1)
           , new Rule(Production_6E_0, variables[33], 4)
           , new Rule(Production_6F_0, variables[34], 4)
           , new Rule(Production_70_0, variables[35], 3)
           , new Rule(Production_70_1, variables[35], 0)
           , new Rule(Production_71_0, variables[36], 1)
           , new Rule(Production_71_1, variables[36], 1)
           , new Rule(Production_71_2, variables[36], 1)
           , new Rule(Production_71_3, variables[36], 1)
           , new Rule(Production_72_0, variables[37], 10)
           , new Rule(Production_73_0, variables[38], 9)
           , new Rule(Production_74_0, variables[39], 2)
           , new Rule(Production_74_1, variables[39], 0)
           , new Rule(Production_75_0, variables[40], 3)
           , new Rule(Production_75_1, variables[40], 0)
           , new Rule(Production_76_0, variables[41], 3)
           , new Rule(Production_76_1, variables[41], 0)
           , new Rule(Production_77_0, variables[42], 3)
           , new Rule(Production_77_1, variables[42], 0)
           , new Rule(Production_78_0, variables[43], 2)
           , new Rule(Production_78_1, variables[43], 0)
           , new Rule(Production_79_0, variables[44], 2)
           , new Rule(Production_79_1, variables[44], 0)
           , new Rule(Production_7A_0, variables[45], 3)
           , new Rule(Production_7A_1, variables[45], 0)
           , new Rule(Production_7B_0, variables[46], 4)
           , new Rule(Production_7C_0, variables[47], 4)
           , new Rule(Production_7D_0, variables[48], 2)
           , new Rule(Production_7E_0, variables[49], 1)
           , new Rule(Production_7E_1, variables[49], 0)
           , new Rule(Production_7F_0, variables[50], 2)
           , new Rule(Production_80_0, variables[51], 2)
           , new Rule(Production_81_0, variables[52], 2)
           , new Rule(Production_81_1, variables[52], 2)
           , new Rule(Production_81_2, variables[52], 2)
           , new Rule(Production_81_3, variables[52], 1)
           , new Rule(Production_82_0, variables[53], 2)
           , new Rule(Production_82_1, variables[53], 2)
           , new Rule(Production_82_2, variables[53], 1)
           , new Rule(Production_83_0, variables[54], 1)
           , new Rule(Production_83_1, variables[54], 3)
           , new Rule(Production_84_0, variables[55], 1)
           , new Rule(Production_84_1, variables[55], 1)
           , new Rule(Production_84_2, variables[55], 1)
           , new Rule(Production_84_3, variables[55], 1)
           , new Rule(Production_84_4, variables[55], 1)
           , new Rule(Production_85_0, variables[56], 2)
           , new Rule(Production_86_0, variables[57], 4)
           , new Rule(Production_87_0, variables[58], 3)
           , new Rule(Production_87_1, variables[58], 0)
           , new Rule(Production_88_0, variables[59], 2)
           , new Rule(Production_88_1, variables[59], 0)
           , new Rule(Production_89_0, variables[60], 3)
           , new Rule(Production_89_1, variables[60], 0)
           , new Rule(Production_8A_0, variables[61], 3)
           , new Rule(Production_8A_1, variables[61], 0)
           , new Rule(Production_8B_0, variables[62], 5)
           , new Rule(Production_8C_0, variables[63], 2)
           , new Rule(Production_8C_1, variables[63], 2)
           , new Rule(Production_8C_2, variables[63], 0)
           , new Rule(Production_8D_0, variables[64], 4)
           , new Rule(Production_8E_0, variables[65], 4)
           , new Rule(Production_8F_0, variables[66], 2)
           , new Rule(Production_90_0, variables[67], 1)
           , new Rule(Production_90_1, variables[67], 0)
           , new Rule(Production_91_0, variables[68], 2)
           , new Rule(Production_92_0, variables[69], 2)
           , new Rule(Production_93_0, variables[70], 2)
           , new Rule(Production_93_1, variables[70], 2)
           , new Rule(Production_93_2, variables[70], 2)
           , new Rule(Production_93_3, variables[70], 1)
           , new Rule(Production_94_0, variables[71], 2)
           , new Rule(Production_94_1, variables[71], 2)
           , new Rule(Production_94_2, variables[71], 1)
           , new Rule(Production_95_0, variables[72], 1)
           , new Rule(Production_95_1, variables[72], 3)
           , new Rule(Production_96_0, variables[73], 1)
           , new Rule(Production_96_1, variables[73], 1)
           , new Rule(Production_96_2, variables[73], 1)
           , new Rule(Production_96_3, variables[73], 1)
           , new Rule(Production_96_4, variables[73], 1)
           , new Rule(Production_97_0, variables[74], 2)
           , new Rule(Production_98_0, variables[75], 4)
           , new Rule(Production_99_0, variables[76], 3)
           , new Rule(Production_99_1, variables[76], 0)
           , new Rule(Production_9A_0, variables[77], 2)
           , new Rule(Production_9A_1, variables[77], 0)
           , new Rule(Production_9B_0, variables[78], 3)
           , new Rule(Production_9B_1, variables[78], 0)
           , new Rule(Production_9C_0, variables[79], 3)
           , new Rule(Production_9C_1, variables[79], 0)
           , new Rule(Production_9D_0, variables[80], 5)
           , new Rule(Production_9E_0, variables[81], 2)
           , new Rule(Production_9E_1, variables[81], 2)
           , new Rule(Production_9E_2, variables[81], 0)
           , new Rule(Production_DC_0, variables[82], 9)
           , new Rule(Production_DD_0, variables[83], 8)
           , new Rule(Production_DE_0, variables[84], 4)
           , new Rule(Production_DF_0, variables[85], 6)
           , new Rule(Production_E0_0, variables[86], 3)
           , new Rule(Production_E0_1, variables[86], 0)
           , new Rule(Production_E1_0, variables[87], 7)
           , new Rule(Production_E2_0, variables[88], 2)
           , new Rule(Production_E2_1, variables[88], 2)
           , new Rule(Production_E2_2, variables[88], 0)
           , new Rule(Production_E3_0, variables[89], 4)
           , new Rule(Production_E4_0, variables[90], 6)
           , new Rule(Production_E5_0, variables[91], 3)
           , new Rule(Production_E5_1, variables[91], 0)
           , new Rule(Production_E6_0, variables[92], 7)
           , new Rule(Production_E7_0, variables[93], 2)
           , new Rule(Production_E7_1, variables[93], 2)
           , new Rule(Production_E7_2, variables[93], 0)
           , new Rule(Production_E8_0, variables[94], 1)
           , new Rule(Production_E9_0, variables[95], 2)
           , new Rule(Production_EA_0, variables[96], 2)
           , new Rule(Production_EA_1, variables[96], 0)
           , new Rule(Production_EB_0, variables[97], 2)
        };
        private static State[] staticStates = {
            new State(
               null,
               new SymbolTerminal[6] {FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[46]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xe9, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[1]},
               new ushort[1] {0x2},
               new ushort[1] {0x14},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[46]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xea, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x15, 0x16, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB4])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[7] {new Reduction(0x2, staticRules[0xB1]), new Reduction(0xc, staticRules[0xB1]), new Reduction(0xd, staticRules[0xB1]), new Reduction(0xe, staticRules[0xB1]), new Reduction(0xf, staticRules[0xB1]), new Reduction(0x10, staticRules[0xB1]), new Reduction(0x52, staticRules[0xB1])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x5]), new Reduction(0xc, staticRules[0x5]), new Reduction(0xd, staticRules[0x5]), new Reduction(0xe, staticRules[0x5]), new Reduction(0xf, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x12, staticRules[0x5]), new Reduction(0x52, staticRules[0x5])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x6]), new Reduction(0xc, staticRules[0x6]), new Reduction(0xd, staticRules[0x6]), new Reduction(0xe, staticRules[0x6]), new Reduction(0xf, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x12, staticRules[0x6]), new Reduction(0x52, staticRules[0x6])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x7]), new Reduction(0xc, staticRules[0x7]), new Reduction(0xd, staticRules[0x7]), new Reduction(0xe, staticRules[0x7]), new Reduction(0xf, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x12, staticRules[0x7]), new Reduction(0x52, staticRules[0x7])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x8]), new Reduction(0xc, staticRules[0x8]), new Reduction(0xd, staticRules[0x8]), new Reduction(0xe, staticRules[0x8]), new Reduction(0xf, staticRules[0x8]), new Reduction(0x10, staticRules[0x8]), new Reduction(0x12, staticRules[0x8]), new Reduction(0x52, staticRules[0x8])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x9]), new Reduction(0xc, staticRules[0x9]), new Reduction(0xd, staticRules[0x9]), new Reduction(0xe, staticRules[0x9]), new Reduction(0xf, staticRules[0x9]), new Reduction(0x10, staticRules[0x9]), new Reduction(0x12, staticRules[0x9]), new Reduction(0x52, staticRules[0x9])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x17},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[1] {0x52},
               new ushort[1] {0x19},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[31]},
               new ushort[1] {0xd9},
               new ushort[1] {0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x45])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x46])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x47])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x48])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x1])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x2])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x3])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x52, staticRules[0x4])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[0]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0xB5])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[1]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB2])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[46]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0xea, 0xe8, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x1B, 0x16, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB4])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x1C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0xb},
               new ushort[1] {0x1E},
               new ushort[1] {0x1b},
               new ushort[1] {0x1D},
               new Reduction[3] {new Reduction(0x11, staticRules[0xD]), new Reduction(0x41, staticRules[0xD]), new Reduction(0x48, staticRules[0xD])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[30]},
               new ushort[1] {0x53},
               new ushort[1] {0x1F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x20},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[1]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x2, staticRules[0xB3])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[13] {0x19, 0x1c, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x21, 0x22, 0x23, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x12, staticRules[0xF])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x11, staticRules[0x0]), new Reduction(0x41, staticRules[0x0]), new Reduction(0x48, staticRules[0x0])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x24},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x25},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[20]},
               new ushort[1] {0x51},
               new ushort[1] {0x27},
               new ushort[1] {0x70},
               new ushort[1] {0x26},
               new Reduction[1] {new Reduction(0x11, staticRules[0x44])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x28},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0x9, 0xB, 0x10, 0x11, 0x12, 0x13},
               new ushort[12] {0x1c, 0x18, 0x1a, 0x72, 0x73, 0xdc, 0xdd, 0x71, 0x14, 0x15, 0x16, 0x17},
               new ushort[12] {0x29, 0x23, 0x4, 0x5, 0x6, 0x7, 0x8, 0xA, 0xC, 0xD, 0xE, 0xF},
               new Reduction[1] {new Reduction(0x12, staticRules[0xF])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0xb},
               new ushort[1] {0x1E},
               new ushort[1] {0x1b},
               new ushort[1] {0x2A},
               new Reduction[3] {new Reduction(0x11, staticRules[0xD]), new Reduction(0x41, staticRules[0xD]), new Reduction(0x48, staticRules[0xD])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[20]},
               new ushort[1] {0x51},
               new ushort[1] {0x27},
               new ushort[1] {0x70},
               new ushort[1] {0x2B},
               new Reduction[1] {new Reduction(0x11, staticRules[0x44])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x2C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x2D},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0xB]), new Reduction(0xc, staticRules[0xB]), new Reduction(0xd, staticRules[0xB]), new Reduction(0xe, staticRules[0xB]), new Reduction(0xf, staticRules[0xB]), new Reduction(0x10, staticRules[0xB]), new Reduction(0x12, staticRules[0xB]), new Reduction(0x52, staticRules[0xB])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xE])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x11, staticRules[0xC]), new Reduction(0x41, staticRules[0xC]), new Reduction(0x48, staticRules[0xC])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x2E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[45]},
               new ushort[1] {0x4f},
               new ushort[1] {0x30},
               new ushort[1] {0x6e},
               new ushort[1] {0x2F},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0x48},
               new ushort[1] {0x32},
               new ushort[1] {0x7a},
               new ushort[1] {0x31},
               new Reduction[1] {new Reduction(0x11, staticRules[0x58])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[45]},
               new ushort[1] {0x4f},
               new ushort[1] {0x30},
               new ushort[1] {0x6e},
               new ushort[1] {0x33},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[38]},
               new ushort[2] {0x50, 0x54},
               new ushort[2] {0x36, 0x37},
               new ushort[2] {0x6f, 0xe3},
               new ushort[2] {0x34, 0x35},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x38},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x11, staticRules[0x43])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x39},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[38]},
               new ushort[2] {0x50, 0x54},
               new ushort[2] {0x36, 0x3C},
               new ushort[2] {0x6f, 0x8d},
               new ushort[2] {0x3A, 0x3B},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[38]},
               new ushort[1] {0x54},
               new ushort[1] {0x3E},
               new ushort[1] {0xde},
               new ushort[1] {0x3D},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x3F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x40},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x41},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x44},
               new ushort[2] {0x78, 0x57},
               new ushort[2] {0x42, 0x43},
               new Reduction[1] {new Reduction(0x12, staticRules[0x54])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0x48},
               new ushort[1] {0x32},
               new ushort[1] {0x7a},
               new ushort[1] {0x45},
               new Reduction[1] {new Reduction(0x11, staticRules[0x58])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[38]},
               new ushort[1] {0x54},
               new ushort[1] {0x47},
               new ushort[1] {0x7b},
               new ushort[1] {0x46},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x48},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x49},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x4A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x4B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0xA0]), new Reduction(0xc, staticRules[0xA0]), new Reduction(0xd, staticRules[0xA0]), new Reduction(0xe, staticRules[0xA0]), new Reduction(0xf, staticRules[0xA0]), new Reduction(0x10, staticRules[0xA0]), new Reduction(0x12, staticRules[0xA0]), new Reduction(0x52, staticRules[0xA0])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x4E},
               new ushort[2] {0x79, 0x67},
               new ushort[2] {0x4C, 0x4D},
               new Reduction[1] {new Reduction(0x12, staticRules[0x56])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x4F, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x54},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x44},
               new ushort[2] {0x78, 0x57},
               new ushort[2] {0x55, 0x43},
               new Reduction[1] {new Reduction(0x12, staticRules[0x54])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[8]},
               new ushort[1] {0x40},
               new ushort[1] {0x56},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x11, staticRules[0x57])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x57},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x58},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x4A]), new Reduction(0xc, staticRules[0x4A]), new Reduction(0xd, staticRules[0x4A]), new Reduction(0xe, staticRules[0x4A]), new Reduction(0xf, staticRules[0x4A]), new Reduction(0x10, staticRules[0x4A]), new Reduction(0x12, staticRules[0x4A]), new Reduction(0x52, staticRules[0x4A])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x59, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x9F]), new Reduction(0xc, staticRules[0x9F]), new Reduction(0xd, staticRules[0x9F]), new Reduction(0xe, staticRules[0x9F]), new Reduction(0xf, staticRules[0x9F]), new Reduction(0x10, staticRules[0x9F]), new Reduction(0x12, staticRules[0x9F]), new Reduction(0x52, staticRules[0x9F])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x5D, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x62},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x4E},
               new ushort[2] {0x79, 0x67},
               new ushort[2] {0x63, 0x4D},
               new Reduction[1] {new Reduction(0x12, staticRules[0x56])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x64},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x65},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x66, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[4] {0xe7, 0xe4, 0xe6, 0xe5},
               new ushort[4] {0x67, 0x50, 0x51, 0x52},
               new Reduction[2] {new Reduction(0x12, staticRules[0xB0]), new Reduction(0xa, staticRules[0xAC])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x68},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x4a, staticRules[0x80]), new Reduction(0xdb, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[38]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x50, staticRules[0x41]), new Reduction(0x54, staticRules[0x41])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x53])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[26]},
               new ushort[1] {0x2e},
               new ushort[1] {0x86},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[39], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[47], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[46]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[8] {new Reduction(0x2, staticRules[0x49]), new Reduction(0xc, staticRules[0x49]), new Reduction(0xd, staticRules[0x49]), new Reduction(0xe, staticRules[0x49]), new Reduction(0xf, staticRules[0x49]), new Reduction(0x10, staticRules[0x49]), new Reduction(0x12, staticRules[0x49]), new Reduction(0x52, staticRules[0x49])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0x87, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x8B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x8C, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x5C},
               new ushort[3] {0x9e, 0x8e, 0x9d},
               new ushort[3] {0x8D, 0x5A, 0x5B},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9E])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x8E, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0x8F},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x91},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x92, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[4] {0xe2, 0xdf, 0xe1, 0xe0},
               new ushort[4] {0x93, 0x5E, 0x5F, 0x60},
               new Reduction[2] {new Reduction(0x12, staticRules[0xA8]), new Reduction(0xa, staticRules[0xA4])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x94},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x4a, staticRules[0x5D]), new Reduction(0xdb, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[38]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x54, staticRules[0x42])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x55])})
            , new State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[13] {0x65, 0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[13] {0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA9])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xAE])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xAF])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x53},
               new ushort[1] {0xe5},
               new ushort[1] {0xBB},
               new Reduction[2] {new Reduction(0x4c, staticRules[0xAC]), new Reduction(0x4d, staticRules[0xAC])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0xdb},
               new ushort[1] {0xBC},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xBE},
               new ushort[1] {0x9c},
               new ushort[1] {0xBD},
               new Reduction[3] {new Reduction(0x41, staticRules[0x9A]), new Reduction(0x44, staticRules[0x9A]), new Reduction(0xdb, staticRules[0x9A])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x7F]), new Reduction(0x44, staticRules[0x7F]), new Reduction(0x4a, staticRules[0x7F]), new Reduction(0xdb, staticRules[0x7F])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xC0},
               new ushort[1] {0x9b},
               new ushort[1] {0xBF},
               new Reduction[4] {new Reduction(0x41, staticRules[0x98]), new Reduction(0x44, staticRules[0x98]), new Reduction(0x4a, staticRules[0x98]), new Reduction(0xdb, staticRules[0x98])})
            , new State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x9a, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xC1, 0xC2, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[5] {new Reduction(0x41, staticRules[0x96]), new Reduction(0x44, staticRules[0x96]), new Reduction(0x49, staticRules[0x96]), new Reduction(0x4a, staticRules[0x96]), new Reduction(0xdb, staticRules[0x96])})
            , new State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xC3, 0xC4, 0xC5},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x86]), new Reduction(0x11, staticRules[0x86]), new Reduction(0x2e, staticRules[0x86]), new Reduction(0x34, staticRules[0x86]), new Reduction(0x35, staticRules[0x86]), new Reduction(0x36, staticRules[0x86]), new Reduction(0x37, staticRules[0x86]), new Reduction(0x38, staticRules[0x86]), new Reduction(0x39, staticRules[0x86]), new Reduction(0x3a, staticRules[0x86]), new Reduction(0x3b, staticRules[0x86]), new Reduction(0x3c, staticRules[0x86]), new Reduction(0x3d, staticRules[0x86]), new Reduction(0x3e, staticRules[0x86]), new Reduction(0x3f, staticRules[0x86]), new Reduction(0x41, staticRules[0x86]), new Reduction(0x43, staticRules[0x86]), new Reduction(0x44, staticRules[0x86]), new Reduction(0x49, staticRules[0x86]), new Reduction(0x4a, staticRules[0x86]), new Reduction(0xdb, staticRules[0x86])})
            , new State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xC6, 0xC7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x89]), new Reduction(0x11, staticRules[0x89]), new Reduction(0x2e, staticRules[0x89]), new Reduction(0x34, staticRules[0x89]), new Reduction(0x35, staticRules[0x89]), new Reduction(0x36, staticRules[0x89]), new Reduction(0x37, staticRules[0x89]), new Reduction(0x38, staticRules[0x89]), new Reduction(0x39, staticRules[0x89]), new Reduction(0x3a, staticRules[0x89]), new Reduction(0x3b, staticRules[0x89]), new Reduction(0x3c, staticRules[0x89]), new Reduction(0x3d, staticRules[0x89]), new Reduction(0x3e, staticRules[0x89]), new Reduction(0x3f, staticRules[0x89]), new Reduction(0x41, staticRules[0x89]), new Reduction(0x43, staticRules[0x89]), new Reduction(0x44, staticRules[0x89]), new Reduction(0x45, staticRules[0x89]), new Reduction(0x46, staticRules[0x89]), new Reduction(0x47, staticRules[0x89]), new Reduction(0x49, staticRules[0x89]), new Reduction(0x4a, staticRules[0x89]), new Reduction(0xdb, staticRules[0x89])})
            , new State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[26] {new Reduction(0xa, staticRules[0x8A]), new Reduction(0x11, staticRules[0x8A]), new Reduction(0x2e, staticRules[0x8A]), new Reduction(0x34, staticRules[0x8A]), new Reduction(0x35, staticRules[0x8A]), new Reduction(0x36, staticRules[0x8A]), new Reduction(0x37, staticRules[0x8A]), new Reduction(0x38, staticRules[0x8A]), new Reduction(0x39, staticRules[0x8A]), new Reduction(0x3a, staticRules[0x8A]), new Reduction(0x3b, staticRules[0x8A]), new Reduction(0x3c, staticRules[0x8A]), new Reduction(0x3d, staticRules[0x8A]), new Reduction(0x3e, staticRules[0x8A]), new Reduction(0x3f, staticRules[0x8A]), new Reduction(0x41, staticRules[0x8A]), new Reduction(0x43, staticRules[0x8A]), new Reduction(0x44, staticRules[0x8A]), new Reduction(0x45, staticRules[0x8A]), new Reduction(0x46, staticRules[0x8A]), new Reduction(0x47, staticRules[0x8A]), new Reduction(0x49, staticRules[0x8A]), new Reduction(0x4a, staticRules[0x8A]), new Reduction(0x55, staticRules[0x8A]), new Reduction(0x56, staticRules[0x8A]), new Reduction(0xdb, staticRules[0x8A])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xC8, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x44, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8C]), new Reduction(0x11, staticRules[0x8C]), new Reduction(0x2e, staticRules[0x8C]), new Reduction(0x34, staticRules[0x8C]), new Reduction(0x35, staticRules[0x8C]), new Reduction(0x36, staticRules[0x8C]), new Reduction(0x37, staticRules[0x8C]), new Reduction(0x38, staticRules[0x8C]), new Reduction(0x39, staticRules[0x8C]), new Reduction(0x3a, staticRules[0x8C]), new Reduction(0x3b, staticRules[0x8C]), new Reduction(0x3c, staticRules[0x8C]), new Reduction(0x3d, staticRules[0x8C]), new Reduction(0x3e, staticRules[0x8C]), new Reduction(0x3f, staticRules[0x8C]), new Reduction(0x41, staticRules[0x8C]), new Reduction(0x43, staticRules[0x8C]), new Reduction(0x44, staticRules[0x8C]), new Reduction(0x45, staticRules[0x8C]), new Reduction(0x46, staticRules[0x8C]), new Reduction(0x47, staticRules[0x8C]), new Reduction(0x48, staticRules[0x8C]), new Reduction(0x49, staticRules[0x8C]), new Reduction(0x4a, staticRules[0x8C]), new Reduction(0x4e, staticRules[0x8C]), new Reduction(0x55, staticRules[0x8C]), new Reduction(0x56, staticRules[0x8C]), new Reduction(0xdb, staticRules[0x8C])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8D]), new Reduction(0x11, staticRules[0x8D]), new Reduction(0x2e, staticRules[0x8D]), new Reduction(0x34, staticRules[0x8D]), new Reduction(0x35, staticRules[0x8D]), new Reduction(0x36, staticRules[0x8D]), new Reduction(0x37, staticRules[0x8D]), new Reduction(0x38, staticRules[0x8D]), new Reduction(0x39, staticRules[0x8D]), new Reduction(0x3a, staticRules[0x8D]), new Reduction(0x3b, staticRules[0x8D]), new Reduction(0x3c, staticRules[0x8D]), new Reduction(0x3d, staticRules[0x8D]), new Reduction(0x3e, staticRules[0x8D]), new Reduction(0x3f, staticRules[0x8D]), new Reduction(0x41, staticRules[0x8D]), new Reduction(0x43, staticRules[0x8D]), new Reduction(0x44, staticRules[0x8D]), new Reduction(0x45, staticRules[0x8D]), new Reduction(0x46, staticRules[0x8D]), new Reduction(0x47, staticRules[0x8D]), new Reduction(0x48, staticRules[0x8D]), new Reduction(0x49, staticRules[0x8D]), new Reduction(0x4a, staticRules[0x8D]), new Reduction(0x4e, staticRules[0x8D]), new Reduction(0x55, staticRules[0x8D]), new Reduction(0x56, staticRules[0x8D]), new Reduction(0xdb, staticRules[0x8D])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8E]), new Reduction(0x11, staticRules[0x8E]), new Reduction(0x2e, staticRules[0x8E]), new Reduction(0x34, staticRules[0x8E]), new Reduction(0x35, staticRules[0x8E]), new Reduction(0x36, staticRules[0x8E]), new Reduction(0x37, staticRules[0x8E]), new Reduction(0x38, staticRules[0x8E]), new Reduction(0x39, staticRules[0x8E]), new Reduction(0x3a, staticRules[0x8E]), new Reduction(0x3b, staticRules[0x8E]), new Reduction(0x3c, staticRules[0x8E]), new Reduction(0x3d, staticRules[0x8E]), new Reduction(0x3e, staticRules[0x8E]), new Reduction(0x3f, staticRules[0x8E]), new Reduction(0x41, staticRules[0x8E]), new Reduction(0x43, staticRules[0x8E]), new Reduction(0x44, staticRules[0x8E]), new Reduction(0x45, staticRules[0x8E]), new Reduction(0x46, staticRules[0x8E]), new Reduction(0x47, staticRules[0x8E]), new Reduction(0x48, staticRules[0x8E]), new Reduction(0x49, staticRules[0x8E]), new Reduction(0x4a, staticRules[0x8E]), new Reduction(0x4e, staticRules[0x8E]), new Reduction(0x55, staticRules[0x8E]), new Reduction(0x56, staticRules[0x8E]), new Reduction(0xdb, staticRules[0x8E])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x8F]), new Reduction(0x11, staticRules[0x8F]), new Reduction(0x2e, staticRules[0x8F]), new Reduction(0x34, staticRules[0x8F]), new Reduction(0x35, staticRules[0x8F]), new Reduction(0x36, staticRules[0x8F]), new Reduction(0x37, staticRules[0x8F]), new Reduction(0x38, staticRules[0x8F]), new Reduction(0x39, staticRules[0x8F]), new Reduction(0x3a, staticRules[0x8F]), new Reduction(0x3b, staticRules[0x8F]), new Reduction(0x3c, staticRules[0x8F]), new Reduction(0x3d, staticRules[0x8F]), new Reduction(0x3e, staticRules[0x8F]), new Reduction(0x3f, staticRules[0x8F]), new Reduction(0x41, staticRules[0x8F]), new Reduction(0x43, staticRules[0x8F]), new Reduction(0x44, staticRules[0x8F]), new Reduction(0x45, staticRules[0x8F]), new Reduction(0x46, staticRules[0x8F]), new Reduction(0x47, staticRules[0x8F]), new Reduction(0x48, staticRules[0x8F]), new Reduction(0x49, staticRules[0x8F]), new Reduction(0x4a, staticRules[0x8F]), new Reduction(0x4e, staticRules[0x8F]), new Reduction(0x55, staticRules[0x8F]), new Reduction(0x56, staticRules[0x8F]), new Reduction(0xdb, staticRules[0x8F])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x90]), new Reduction(0x11, staticRules[0x90]), new Reduction(0x2e, staticRules[0x90]), new Reduction(0x34, staticRules[0x90]), new Reduction(0x35, staticRules[0x90]), new Reduction(0x36, staticRules[0x90]), new Reduction(0x37, staticRules[0x90]), new Reduction(0x38, staticRules[0x90]), new Reduction(0x39, staticRules[0x90]), new Reduction(0x3a, staticRules[0x90]), new Reduction(0x3b, staticRules[0x90]), new Reduction(0x3c, staticRules[0x90]), new Reduction(0x3d, staticRules[0x90]), new Reduction(0x3e, staticRules[0x90]), new Reduction(0x3f, staticRules[0x90]), new Reduction(0x41, staticRules[0x90]), new Reduction(0x43, staticRules[0x90]), new Reduction(0x44, staticRules[0x90]), new Reduction(0x45, staticRules[0x90]), new Reduction(0x46, staticRules[0x90]), new Reduction(0x47, staticRules[0x90]), new Reduction(0x48, staticRules[0x90]), new Reduction(0x49, staticRules[0x90]), new Reduction(0x4a, staticRules[0x90]), new Reduction(0x4e, staticRules[0x90]), new Reduction(0x55, staticRules[0x90]), new Reduction(0x56, staticRules[0x90]), new Reduction(0xdb, staticRules[0x90])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0xC9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[29] {new Reduction(0xa, staticRules[0x31]), new Reduction(0x11, staticRules[0x31]), new Reduction(0x2e, staticRules[0x31]), new Reduction(0x30, staticRules[0x31]), new Reduction(0x34, staticRules[0x31]), new Reduction(0x35, staticRules[0x31]), new Reduction(0x36, staticRules[0x31]), new Reduction(0x37, staticRules[0x31]), new Reduction(0x38, staticRules[0x31]), new Reduction(0x39, staticRules[0x31]), new Reduction(0x3a, staticRules[0x31]), new Reduction(0x3b, staticRules[0x31]), new Reduction(0x3c, staticRules[0x31]), new Reduction(0x3d, staticRules[0x31]), new Reduction(0x3e, staticRules[0x31]), new Reduction(0x3f, staticRules[0x31]), new Reduction(0x41, staticRules[0x31]), new Reduction(0x43, staticRules[0x31]), new Reduction(0x44, staticRules[0x31]), new Reduction(0x45, staticRules[0x31]), new Reduction(0x46, staticRules[0x31]), new Reduction(0x47, staticRules[0x31]), new Reduction(0x48, staticRules[0x31]), new Reduction(0x49, staticRules[0x31]), new Reduction(0x4a, staticRules[0x31]), new Reduction(0x4e, staticRules[0x31]), new Reduction(0x55, staticRules[0x31]), new Reduction(0x56, staticRules[0x31]), new Reduction(0xdb, staticRules[0x31])})
            , new State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4d},
               new ushort[1] {0xCB},
               new ushort[1] {0x98},
               new ushort[1] {0xCA},
               new Reduction[28] {new Reduction(0xa, staticRules[0x32]), new Reduction(0x11, staticRules[0x32]), new Reduction(0x2e, staticRules[0x32]), new Reduction(0x34, staticRules[0x32]), new Reduction(0x35, staticRules[0x32]), new Reduction(0x36, staticRules[0x32]), new Reduction(0x37, staticRules[0x32]), new Reduction(0x38, staticRules[0x32]), new Reduction(0x39, staticRules[0x32]), new Reduction(0x3a, staticRules[0x32]), new Reduction(0x3b, staticRules[0x32]), new Reduction(0x3c, staticRules[0x32]), new Reduction(0x3d, staticRules[0x32]), new Reduction(0x3e, staticRules[0x32]), new Reduction(0x3f, staticRules[0x32]), new Reduction(0x41, staticRules[0x32]), new Reduction(0x43, staticRules[0x32]), new Reduction(0x44, staticRules[0x32]), new Reduction(0x45, staticRules[0x32]), new Reduction(0x46, staticRules[0x32]), new Reduction(0x47, staticRules[0x32]), new Reduction(0x48, staticRules[0x32]), new Reduction(0x49, staticRules[0x32]), new Reduction(0x4a, staticRules[0x32]), new Reduction(0x4e, staticRules[0x32]), new Reduction(0x55, staticRules[0x32]), new Reduction(0x56, staticRules[0x32]), new Reduction(0xdb, staticRules[0x32])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x34]), new Reduction(0x11, staticRules[0x34]), new Reduction(0x2e, staticRules[0x34]), new Reduction(0x34, staticRules[0x34]), new Reduction(0x35, staticRules[0x34]), new Reduction(0x36, staticRules[0x34]), new Reduction(0x37, staticRules[0x34]), new Reduction(0x38, staticRules[0x34]), new Reduction(0x39, staticRules[0x34]), new Reduction(0x3a, staticRules[0x34]), new Reduction(0x3b, staticRules[0x34]), new Reduction(0x3c, staticRules[0x34]), new Reduction(0x3d, staticRules[0x34]), new Reduction(0x3e, staticRules[0x34]), new Reduction(0x3f, staticRules[0x34]), new Reduction(0x41, staticRules[0x34]), new Reduction(0x43, staticRules[0x34]), new Reduction(0x44, staticRules[0x34]), new Reduction(0x45, staticRules[0x34]), new Reduction(0x46, staticRules[0x34]), new Reduction(0x47, staticRules[0x34]), new Reduction(0x48, staticRules[0x34]), new Reduction(0x49, staticRules[0x34]), new Reduction(0x4a, staticRules[0x34]), new Reduction(0x4e, staticRules[0x34]), new Reduction(0x55, staticRules[0x34]), new Reduction(0x56, staticRules[0x34]), new Reduction(0xdb, staticRules[0x34])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x35]), new Reduction(0x11, staticRules[0x35]), new Reduction(0x2e, staticRules[0x35]), new Reduction(0x34, staticRules[0x35]), new Reduction(0x35, staticRules[0x35]), new Reduction(0x36, staticRules[0x35]), new Reduction(0x37, staticRules[0x35]), new Reduction(0x38, staticRules[0x35]), new Reduction(0x39, staticRules[0x35]), new Reduction(0x3a, staticRules[0x35]), new Reduction(0x3b, staticRules[0x35]), new Reduction(0x3c, staticRules[0x35]), new Reduction(0x3d, staticRules[0x35]), new Reduction(0x3e, staticRules[0x35]), new Reduction(0x3f, staticRules[0x35]), new Reduction(0x41, staticRules[0x35]), new Reduction(0x43, staticRules[0x35]), new Reduction(0x44, staticRules[0x35]), new Reduction(0x45, staticRules[0x35]), new Reduction(0x46, staticRules[0x35]), new Reduction(0x47, staticRules[0x35]), new Reduction(0x48, staticRules[0x35]), new Reduction(0x49, staticRules[0x35]), new Reduction(0x4a, staticRules[0x35]), new Reduction(0x4e, staticRules[0x35]), new Reduction(0x55, staticRules[0x35]), new Reduction(0x56, staticRules[0x35]), new Reduction(0xdb, staticRules[0x35])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x36]), new Reduction(0x11, staticRules[0x36]), new Reduction(0x2e, staticRules[0x36]), new Reduction(0x34, staticRules[0x36]), new Reduction(0x35, staticRules[0x36]), new Reduction(0x36, staticRules[0x36]), new Reduction(0x37, staticRules[0x36]), new Reduction(0x38, staticRules[0x36]), new Reduction(0x39, staticRules[0x36]), new Reduction(0x3a, staticRules[0x36]), new Reduction(0x3b, staticRules[0x36]), new Reduction(0x3c, staticRules[0x36]), new Reduction(0x3d, staticRules[0x36]), new Reduction(0x3e, staticRules[0x36]), new Reduction(0x3f, staticRules[0x36]), new Reduction(0x41, staticRules[0x36]), new Reduction(0x43, staticRules[0x36]), new Reduction(0x44, staticRules[0x36]), new Reduction(0x45, staticRules[0x36]), new Reduction(0x46, staticRules[0x36]), new Reduction(0x47, staticRules[0x36]), new Reduction(0x48, staticRules[0x36]), new Reduction(0x49, staticRules[0x36]), new Reduction(0x4a, staticRules[0x36]), new Reduction(0x4e, staticRules[0x36]), new Reduction(0x55, staticRules[0x36]), new Reduction(0x56, staticRules[0x36]), new Reduction(0xdb, staticRules[0x36])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x37]), new Reduction(0x11, staticRules[0x37]), new Reduction(0x2e, staticRules[0x37]), new Reduction(0x34, staticRules[0x37]), new Reduction(0x35, staticRules[0x37]), new Reduction(0x36, staticRules[0x37]), new Reduction(0x37, staticRules[0x37]), new Reduction(0x38, staticRules[0x37]), new Reduction(0x39, staticRules[0x37]), new Reduction(0x3a, staticRules[0x37]), new Reduction(0x3b, staticRules[0x37]), new Reduction(0x3c, staticRules[0x37]), new Reduction(0x3d, staticRules[0x37]), new Reduction(0x3e, staticRules[0x37]), new Reduction(0x3f, staticRules[0x37]), new Reduction(0x41, staticRules[0x37]), new Reduction(0x43, staticRules[0x37]), new Reduction(0x44, staticRules[0x37]), new Reduction(0x45, staticRules[0x37]), new Reduction(0x46, staticRules[0x37]), new Reduction(0x47, staticRules[0x37]), new Reduction(0x48, staticRules[0x37]), new Reduction(0x49, staticRules[0x37]), new Reduction(0x4a, staticRules[0x37]), new Reduction(0x4e, staticRules[0x37]), new Reduction(0x55, staticRules[0x37]), new Reduction(0x56, staticRules[0x37]), new Reduction(0xdb, staticRules[0x37])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x38]), new Reduction(0x11, staticRules[0x38]), new Reduction(0x2e, staticRules[0x38]), new Reduction(0x34, staticRules[0x38]), new Reduction(0x35, staticRules[0x38]), new Reduction(0x36, staticRules[0x38]), new Reduction(0x37, staticRules[0x38]), new Reduction(0x38, staticRules[0x38]), new Reduction(0x39, staticRules[0x38]), new Reduction(0x3a, staticRules[0x38]), new Reduction(0x3b, staticRules[0x38]), new Reduction(0x3c, staticRules[0x38]), new Reduction(0x3d, staticRules[0x38]), new Reduction(0x3e, staticRules[0x38]), new Reduction(0x3f, staticRules[0x38]), new Reduction(0x41, staticRules[0x38]), new Reduction(0x43, staticRules[0x38]), new Reduction(0x44, staticRules[0x38]), new Reduction(0x45, staticRules[0x38]), new Reduction(0x46, staticRules[0x38]), new Reduction(0x47, staticRules[0x38]), new Reduction(0x48, staticRules[0x38]), new Reduction(0x49, staticRules[0x38]), new Reduction(0x4a, staticRules[0x38]), new Reduction(0x4e, staticRules[0x38]), new Reduction(0x55, staticRules[0x38]), new Reduction(0x56, staticRules[0x38]), new Reduction(0xdb, staticRules[0x38])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x39]), new Reduction(0x11, staticRules[0x39]), new Reduction(0x2e, staticRules[0x39]), new Reduction(0x34, staticRules[0x39]), new Reduction(0x35, staticRules[0x39]), new Reduction(0x36, staticRules[0x39]), new Reduction(0x37, staticRules[0x39]), new Reduction(0x38, staticRules[0x39]), new Reduction(0x39, staticRules[0x39]), new Reduction(0x3a, staticRules[0x39]), new Reduction(0x3b, staticRules[0x39]), new Reduction(0x3c, staticRules[0x39]), new Reduction(0x3d, staticRules[0x39]), new Reduction(0x3e, staticRules[0x39]), new Reduction(0x3f, staticRules[0x39]), new Reduction(0x41, staticRules[0x39]), new Reduction(0x43, staticRules[0x39]), new Reduction(0x44, staticRules[0x39]), new Reduction(0x45, staticRules[0x39]), new Reduction(0x46, staticRules[0x39]), new Reduction(0x47, staticRules[0x39]), new Reduction(0x48, staticRules[0x39]), new Reduction(0x49, staticRules[0x39]), new Reduction(0x4a, staticRules[0x39]), new Reduction(0x4e, staticRules[0x39]), new Reduction(0x55, staticRules[0x39]), new Reduction(0x56, staticRules[0x39]), new Reduction(0xdb, staticRules[0x39])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3A]), new Reduction(0x11, staticRules[0x3A]), new Reduction(0x2e, staticRules[0x3A]), new Reduction(0x34, staticRules[0x3A]), new Reduction(0x35, staticRules[0x3A]), new Reduction(0x36, staticRules[0x3A]), new Reduction(0x37, staticRules[0x3A]), new Reduction(0x38, staticRules[0x3A]), new Reduction(0x39, staticRules[0x3A]), new Reduction(0x3a, staticRules[0x3A]), new Reduction(0x3b, staticRules[0x3A]), new Reduction(0x3c, staticRules[0x3A]), new Reduction(0x3d, staticRules[0x3A]), new Reduction(0x3e, staticRules[0x3A]), new Reduction(0x3f, staticRules[0x3A]), new Reduction(0x41, staticRules[0x3A]), new Reduction(0x43, staticRules[0x3A]), new Reduction(0x44, staticRules[0x3A]), new Reduction(0x45, staticRules[0x3A]), new Reduction(0x46, staticRules[0x3A]), new Reduction(0x47, staticRules[0x3A]), new Reduction(0x48, staticRules[0x3A]), new Reduction(0x49, staticRules[0x3A]), new Reduction(0x4a, staticRules[0x3A]), new Reduction(0x4e, staticRules[0x3A]), new Reduction(0x55, staticRules[0x3A]), new Reduction(0x56, staticRules[0x3A]), new Reduction(0xdb, staticRules[0x3A])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3B]), new Reduction(0x11, staticRules[0x3B]), new Reduction(0x2e, staticRules[0x3B]), new Reduction(0x34, staticRules[0x3B]), new Reduction(0x35, staticRules[0x3B]), new Reduction(0x36, staticRules[0x3B]), new Reduction(0x37, staticRules[0x3B]), new Reduction(0x38, staticRules[0x3B]), new Reduction(0x39, staticRules[0x3B]), new Reduction(0x3a, staticRules[0x3B]), new Reduction(0x3b, staticRules[0x3B]), new Reduction(0x3c, staticRules[0x3B]), new Reduction(0x3d, staticRules[0x3B]), new Reduction(0x3e, staticRules[0x3B]), new Reduction(0x3f, staticRules[0x3B]), new Reduction(0x41, staticRules[0x3B]), new Reduction(0x43, staticRules[0x3B]), new Reduction(0x44, staticRules[0x3B]), new Reduction(0x45, staticRules[0x3B]), new Reduction(0x46, staticRules[0x3B]), new Reduction(0x47, staticRules[0x3B]), new Reduction(0x48, staticRules[0x3B]), new Reduction(0x49, staticRules[0x3B]), new Reduction(0x4a, staticRules[0x3B]), new Reduction(0x4e, staticRules[0x3B]), new Reduction(0x55, staticRules[0x3B]), new Reduction(0x56, staticRules[0x3B]), new Reduction(0xdb, staticRules[0x3B])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3C]), new Reduction(0x11, staticRules[0x3C]), new Reduction(0x2e, staticRules[0x3C]), new Reduction(0x34, staticRules[0x3C]), new Reduction(0x35, staticRules[0x3C]), new Reduction(0x36, staticRules[0x3C]), new Reduction(0x37, staticRules[0x3C]), new Reduction(0x38, staticRules[0x3C]), new Reduction(0x39, staticRules[0x3C]), new Reduction(0x3a, staticRules[0x3C]), new Reduction(0x3b, staticRules[0x3C]), new Reduction(0x3c, staticRules[0x3C]), new Reduction(0x3d, staticRules[0x3C]), new Reduction(0x3e, staticRules[0x3C]), new Reduction(0x3f, staticRules[0x3C]), new Reduction(0x41, staticRules[0x3C]), new Reduction(0x43, staticRules[0x3C]), new Reduction(0x44, staticRules[0x3C]), new Reduction(0x45, staticRules[0x3C]), new Reduction(0x46, staticRules[0x3C]), new Reduction(0x47, staticRules[0x3C]), new Reduction(0x48, staticRules[0x3C]), new Reduction(0x49, staticRules[0x3C]), new Reduction(0x4a, staticRules[0x3C]), new Reduction(0x4e, staticRules[0x3C]), new Reduction(0x55, staticRules[0x3C]), new Reduction(0x56, staticRules[0x3C]), new Reduction(0xdb, staticRules[0x3C])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3D]), new Reduction(0x11, staticRules[0x3D]), new Reduction(0x2e, staticRules[0x3D]), new Reduction(0x34, staticRules[0x3D]), new Reduction(0x35, staticRules[0x3D]), new Reduction(0x36, staticRules[0x3D]), new Reduction(0x37, staticRules[0x3D]), new Reduction(0x38, staticRules[0x3D]), new Reduction(0x39, staticRules[0x3D]), new Reduction(0x3a, staticRules[0x3D]), new Reduction(0x3b, staticRules[0x3D]), new Reduction(0x3c, staticRules[0x3D]), new Reduction(0x3d, staticRules[0x3D]), new Reduction(0x3e, staticRules[0x3D]), new Reduction(0x3f, staticRules[0x3D]), new Reduction(0x41, staticRules[0x3D]), new Reduction(0x43, staticRules[0x3D]), new Reduction(0x44, staticRules[0x3D]), new Reduction(0x45, staticRules[0x3D]), new Reduction(0x46, staticRules[0x3D]), new Reduction(0x47, staticRules[0x3D]), new Reduction(0x48, staticRules[0x3D]), new Reduction(0x49, staticRules[0x3D]), new Reduction(0x4a, staticRules[0x3D]), new Reduction(0x4e, staticRules[0x3D]), new Reduction(0x55, staticRules[0x3D]), new Reduction(0x56, staticRules[0x3D]), new Reduction(0xdb, staticRules[0x3D])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3E]), new Reduction(0x11, staticRules[0x3E]), new Reduction(0x2e, staticRules[0x3E]), new Reduction(0x34, staticRules[0x3E]), new Reduction(0x35, staticRules[0x3E]), new Reduction(0x36, staticRules[0x3E]), new Reduction(0x37, staticRules[0x3E]), new Reduction(0x38, staticRules[0x3E]), new Reduction(0x39, staticRules[0x3E]), new Reduction(0x3a, staticRules[0x3E]), new Reduction(0x3b, staticRules[0x3E]), new Reduction(0x3c, staticRules[0x3E]), new Reduction(0x3d, staticRules[0x3E]), new Reduction(0x3e, staticRules[0x3E]), new Reduction(0x3f, staticRules[0x3E]), new Reduction(0x41, staticRules[0x3E]), new Reduction(0x43, staticRules[0x3E]), new Reduction(0x44, staticRules[0x3E]), new Reduction(0x45, staticRules[0x3E]), new Reduction(0x46, staticRules[0x3E]), new Reduction(0x47, staticRules[0x3E]), new Reduction(0x48, staticRules[0x3E]), new Reduction(0x49, staticRules[0x3E]), new Reduction(0x4a, staticRules[0x3E]), new Reduction(0x4e, staticRules[0x3E]), new Reduction(0x55, staticRules[0x3E]), new Reduction(0x56, staticRules[0x3E]), new Reduction(0xdb, staticRules[0x3E])})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x3F]), new Reduction(0x11, staticRules[0x3F]), new Reduction(0x2e, staticRules[0x3F]), new Reduction(0x34, staticRules[0x3F]), new Reduction(0x35, staticRules[0x3F]), new Reduction(0x36, staticRules[0x3F]), new Reduction(0x37, staticRules[0x3F]), new Reduction(0x38, staticRules[0x3F]), new Reduction(0x39, staticRules[0x3F]), new Reduction(0x3a, staticRules[0x3F]), new Reduction(0x3b, staticRules[0x3F]), new Reduction(0x3c, staticRules[0x3F]), new Reduction(0x3d, staticRules[0x3F]), new Reduction(0x3e, staticRules[0x3F]), new Reduction(0x3f, staticRules[0x3F]), new Reduction(0x41, staticRules[0x3F]), new Reduction(0x43, staticRules[0x3F]), new Reduction(0x44, staticRules[0x3F]), new Reduction(0x45, staticRules[0x3F]), new Reduction(0x46, staticRules[0x3F]), new Reduction(0x47, staticRules[0x3F]), new Reduction(0x48, staticRules[0x3F]), new Reduction(0x49, staticRules[0x3F]), new Reduction(0x4a, staticRules[0x3F]), new Reduction(0x4e, staticRules[0x3F]), new Reduction(0x55, staticRules[0x3F]), new Reduction(0x56, staticRules[0x3F]), new Reduction(0xdb, staticRules[0x3F])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0xCC},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0xCD},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0xCE, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0xa},
               new ushort[1] {0x8A},
               new ushort[3] {0x8c, 0x7c, 0x8b},
               new ushort[3] {0xCF, 0x88, 0x89},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7B])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0xD0, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0xD1},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7C])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9C])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x9D])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xD2, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0xD3},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0xD4},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA1])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA6])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0xA7])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xda},
               new ushort[1] {0x61},
               new ushort[1] {0xe0},
               new ushort[1] {0xD5},
               new Reduction[2] {new Reduction(0x4c, staticRules[0xA4]), new Reduction(0x4d, staticRules[0xA4])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0xdb},
               new ushort[1] {0xD6},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xD8},
               new ushort[1] {0x8a},
               new ushort[1] {0xD7},
               new Reduction[3] {new Reduction(0x41, staticRules[0x77]), new Reduction(0x44, staticRules[0x77]), new Reduction(0xdb, staticRules[0x77])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5C]), new Reduction(0x44, staticRules[0x5C]), new Reduction(0x4a, staticRules[0x5C]), new Reduction(0xdb, staticRules[0x5C])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xDA},
               new ushort[1] {0x89},
               new ushort[1] {0xD9},
               new Reduction[4] {new Reduction(0x41, staticRules[0x75]), new Reduction(0x44, staticRules[0x75]), new Reduction(0x4a, staticRules[0x75]), new Reduction(0xdb, staticRules[0x75])})
            , new State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x88, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0xDB, 0xDC, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[5] {new Reduction(0x41, staticRules[0x73]), new Reduction(0x44, staticRules[0x73]), new Reduction(0x49, staticRules[0x73]), new Reduction(0x4a, staticRules[0x73]), new Reduction(0xdb, staticRules[0x73])})
            , new State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xDD, 0xDE, 0xDF},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x63]), new Reduction(0x11, staticRules[0x63]), new Reduction(0x2e, staticRules[0x63]), new Reduction(0x30, staticRules[0x63]), new Reduction(0x41, staticRules[0x63]), new Reduction(0x43, staticRules[0x63]), new Reduction(0x44, staticRules[0x63]), new Reduction(0x49, staticRules[0x63]), new Reduction(0x4a, staticRules[0x63]), new Reduction(0xdb, staticRules[0x63])})
            , new State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xE0, 0xE1},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x66]), new Reduction(0x11, staticRules[0x66]), new Reduction(0x2e, staticRules[0x66]), new Reduction(0x30, staticRules[0x66]), new Reduction(0x41, staticRules[0x66]), new Reduction(0x43, staticRules[0x66]), new Reduction(0x44, staticRules[0x66]), new Reduction(0x45, staticRules[0x66]), new Reduction(0x46, staticRules[0x66]), new Reduction(0x47, staticRules[0x66]), new Reduction(0x49, staticRules[0x66]), new Reduction(0x4a, staticRules[0x66]), new Reduction(0xdb, staticRules[0x66])})
            , new State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[15] {new Reduction(0xa, staticRules[0x67]), new Reduction(0x11, staticRules[0x67]), new Reduction(0x2e, staticRules[0x67]), new Reduction(0x30, staticRules[0x67]), new Reduction(0x41, staticRules[0x67]), new Reduction(0x43, staticRules[0x67]), new Reduction(0x44, staticRules[0x67]), new Reduction(0x45, staticRules[0x67]), new Reduction(0x46, staticRules[0x67]), new Reduction(0x47, staticRules[0x67]), new Reduction(0x49, staticRules[0x67]), new Reduction(0x4a, staticRules[0x67]), new Reduction(0x55, staticRules[0x67]), new Reduction(0x56, staticRules[0x67]), new Reduction(0xdb, staticRules[0x67])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0xE2, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x44, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x69]), new Reduction(0x11, staticRules[0x69]), new Reduction(0x2e, staticRules[0x69]), new Reduction(0x30, staticRules[0x69]), new Reduction(0x41, staticRules[0x69]), new Reduction(0x43, staticRules[0x69]), new Reduction(0x44, staticRules[0x69]), new Reduction(0x45, staticRules[0x69]), new Reduction(0x46, staticRules[0x69]), new Reduction(0x47, staticRules[0x69]), new Reduction(0x48, staticRules[0x69]), new Reduction(0x49, staticRules[0x69]), new Reduction(0x4a, staticRules[0x69]), new Reduction(0x4e, staticRules[0x69]), new Reduction(0x55, staticRules[0x69]), new Reduction(0x56, staticRules[0x69]), new Reduction(0xdb, staticRules[0x69])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6A]), new Reduction(0x11, staticRules[0x6A]), new Reduction(0x2e, staticRules[0x6A]), new Reduction(0x30, staticRules[0x6A]), new Reduction(0x41, staticRules[0x6A]), new Reduction(0x43, staticRules[0x6A]), new Reduction(0x44, staticRules[0x6A]), new Reduction(0x45, staticRules[0x6A]), new Reduction(0x46, staticRules[0x6A]), new Reduction(0x47, staticRules[0x6A]), new Reduction(0x48, staticRules[0x6A]), new Reduction(0x49, staticRules[0x6A]), new Reduction(0x4a, staticRules[0x6A]), new Reduction(0x4e, staticRules[0x6A]), new Reduction(0x55, staticRules[0x6A]), new Reduction(0x56, staticRules[0x6A]), new Reduction(0xdb, staticRules[0x6A])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6B]), new Reduction(0x11, staticRules[0x6B]), new Reduction(0x2e, staticRules[0x6B]), new Reduction(0x30, staticRules[0x6B]), new Reduction(0x41, staticRules[0x6B]), new Reduction(0x43, staticRules[0x6B]), new Reduction(0x44, staticRules[0x6B]), new Reduction(0x45, staticRules[0x6B]), new Reduction(0x46, staticRules[0x6B]), new Reduction(0x47, staticRules[0x6B]), new Reduction(0x48, staticRules[0x6B]), new Reduction(0x49, staticRules[0x6B]), new Reduction(0x4a, staticRules[0x6B]), new Reduction(0x4e, staticRules[0x6B]), new Reduction(0x55, staticRules[0x6B]), new Reduction(0x56, staticRules[0x6B]), new Reduction(0xdb, staticRules[0x6B])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6C]), new Reduction(0x11, staticRules[0x6C]), new Reduction(0x2e, staticRules[0x6C]), new Reduction(0x30, staticRules[0x6C]), new Reduction(0x41, staticRules[0x6C]), new Reduction(0x43, staticRules[0x6C]), new Reduction(0x44, staticRules[0x6C]), new Reduction(0x45, staticRules[0x6C]), new Reduction(0x46, staticRules[0x6C]), new Reduction(0x47, staticRules[0x6C]), new Reduction(0x48, staticRules[0x6C]), new Reduction(0x49, staticRules[0x6C]), new Reduction(0x4a, staticRules[0x6C]), new Reduction(0x4e, staticRules[0x6C]), new Reduction(0x55, staticRules[0x6C]), new Reduction(0x56, staticRules[0x6C]), new Reduction(0xdb, staticRules[0x6C])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6D]), new Reduction(0x11, staticRules[0x6D]), new Reduction(0x2e, staticRules[0x6D]), new Reduction(0x30, staticRules[0x6D]), new Reduction(0x41, staticRules[0x6D]), new Reduction(0x43, staticRules[0x6D]), new Reduction(0x44, staticRules[0x6D]), new Reduction(0x45, staticRules[0x6D]), new Reduction(0x46, staticRules[0x6D]), new Reduction(0x47, staticRules[0x6D]), new Reduction(0x48, staticRules[0x6D]), new Reduction(0x49, staticRules[0x6D]), new Reduction(0x4a, staticRules[0x6D]), new Reduction(0x4e, staticRules[0x6D]), new Reduction(0x55, staticRules[0x6D]), new Reduction(0x56, staticRules[0x6D]), new Reduction(0xdb, staticRules[0x6D])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4d},
               new ushort[1] {0xE4},
               new ushort[1] {0x86},
               new ushort[1] {0xE3},
               new Reduction[17] {new Reduction(0xa, staticRules[0x32]), new Reduction(0x11, staticRules[0x32]), new Reduction(0x2e, staticRules[0x32]), new Reduction(0x30, staticRules[0x32]), new Reduction(0x41, staticRules[0x32]), new Reduction(0x43, staticRules[0x32]), new Reduction(0x44, staticRules[0x32]), new Reduction(0x45, staticRules[0x32]), new Reduction(0x46, staticRules[0x32]), new Reduction(0x47, staticRules[0x32]), new Reduction(0x48, staticRules[0x32]), new Reduction(0x49, staticRules[0x32]), new Reduction(0x4a, staticRules[0x32]), new Reduction(0x4e, staticRules[0x32]), new Reduction(0x55, staticRules[0x32]), new Reduction(0x56, staticRules[0x32]), new Reduction(0xdb, staticRules[0x32])})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x40]), new Reduction(0x11, staticRules[0x40]), new Reduction(0x2e, staticRules[0x40]), new Reduction(0x30, staticRules[0x40]), new Reduction(0x41, staticRules[0x40]), new Reduction(0x43, staticRules[0x40]), new Reduction(0x44, staticRules[0x40]), new Reduction(0x45, staticRules[0x40]), new Reduction(0x46, staticRules[0x40]), new Reduction(0x47, staticRules[0x40]), new Reduction(0x48, staticRules[0x40]), new Reduction(0x49, staticRules[0x40]), new Reduction(0x4a, staticRules[0x40]), new Reduction(0x4e, staticRules[0x40]), new Reduction(0x55, staticRules[0x40]), new Reduction(0x56, staticRules[0x40]), new Reduction(0xdb, staticRules[0x40])})
            , new State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x14]), new Reduction(0xb, staticRules[0x14]), new Reduction(0x11, staticRules[0x14]), new Reduction(0x2e, staticRules[0x14]), new Reduction(0x30, staticRules[0x14]), new Reduction(0x31, staticRules[0x14]), new Reduction(0x32, staticRules[0x14]), new Reduction(0x33, staticRules[0x14]), new Reduction(0x34, staticRules[0x14]), new Reduction(0x35, staticRules[0x14]), new Reduction(0x41, staticRules[0x14]), new Reduction(0x43, staticRules[0x14]), new Reduction(0x44, staticRules[0x14]), new Reduction(0x45, staticRules[0x14]), new Reduction(0x46, staticRules[0x14]), new Reduction(0x47, staticRules[0x14]), new Reduction(0x48, staticRules[0x14]), new Reduction(0x49, staticRules[0x14]), new Reduction(0x4a, staticRules[0x14]), new Reduction(0x4b, staticRules[0x14]), new Reduction(0x4e, staticRules[0x14]), new Reduction(0x55, staticRules[0x14]), new Reduction(0x56, staticRules[0x14]), new Reduction(0xdb, staticRules[0x14])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4b},
               new ushort[1] {0xE6},
               new ushort[1] {0x66},
               new ushort[1] {0xE5},
               new Reduction[1] {new Reduction(0x41, staticRules[0x2E])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4a},
               new ushort[1] {0xE8},
               new ushort[1] {0x76},
               new ushort[1] {0xE7},
               new Reduction[3] {new Reduction(0x41, staticRules[0x50]), new Reduction(0x44, staticRules[0x50]), new Reduction(0x4b, staticRules[0x50])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x49},
               new ushort[1] {0xEA},
               new ushort[1] {0x75},
               new ushort[1] {0xE9},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4E]), new Reduction(0x44, staticRules[0x4E]), new Reduction(0x4a, staticRules[0x4E]), new Reduction(0x4b, staticRules[0x4E])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x74, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0xEB, 0xEC, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4C]), new Reduction(0x44, staticRules[0x4C]), new Reduction(0x49, staticRules[0x4C]), new Reduction(0x4a, staticRules[0x4C]), new Reduction(0x4b, staticRules[0x4C])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[4] {0x45, 0x46, 0x47, 0x11},
               new ushort[4] {0xEE, 0xEF, 0xF0, 0xF1},
               new ushort[1] {0x61},
               new ushort[1] {0xED},
               new Reduction[14] {new Reduction(0xa, staticRules[0x29]), new Reduction(0xb, staticRules[0x29]), new Reduction(0x30, staticRules[0x29]), new Reduction(0x31, staticRules[0x29]), new Reduction(0x32, staticRules[0x29]), new Reduction(0x33, staticRules[0x29]), new Reduction(0x34, staticRules[0x29]), new Reduction(0x35, staticRules[0x29]), new Reduction(0x41, staticRules[0x29]), new Reduction(0x43, staticRules[0x29]), new Reduction(0x44, staticRules[0x29]), new Reduction(0x49, staticRules[0x29]), new Reduction(0x4a, staticRules[0x29]), new Reduction(0x4b, staticRules[0x29])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x21]), new Reduction(0xb, staticRules[0x21]), new Reduction(0x11, staticRules[0x21]), new Reduction(0x30, staticRules[0x21]), new Reduction(0x31, staticRules[0x21]), new Reduction(0x32, staticRules[0x21]), new Reduction(0x33, staticRules[0x21]), new Reduction(0x34, staticRules[0x21]), new Reduction(0x35, staticRules[0x21]), new Reduction(0x41, staticRules[0x21]), new Reduction(0x43, staticRules[0x21]), new Reduction(0x44, staticRules[0x21]), new Reduction(0x45, staticRules[0x21]), new Reduction(0x46, staticRules[0x21]), new Reduction(0x47, staticRules[0x21]), new Reduction(0x49, staticRules[0x21]), new Reduction(0x4a, staticRules[0x21]), new Reduction(0x4b, staticRules[0x21])})
            , new State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[13] {0x65, 0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[13] {0xF2, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x19]), new Reduction(0xb, staticRules[0x19]), new Reduction(0x11, staticRules[0x19]), new Reduction(0x30, staticRules[0x19]), new Reduction(0x31, staticRules[0x19]), new Reduction(0x32, staticRules[0x19]), new Reduction(0x33, staticRules[0x19]), new Reduction(0x34, staticRules[0x19]), new Reduction(0x35, staticRules[0x19]), new Reduction(0x41, staticRules[0x19]), new Reduction(0x43, staticRules[0x19]), new Reduction(0x44, staticRules[0x19]), new Reduction(0x45, staticRules[0x19]), new Reduction(0x46, staticRules[0x19]), new Reduction(0x47, staticRules[0x19]), new Reduction(0x49, staticRules[0x19]), new Reduction(0x4a, staticRules[0x19]), new Reduction(0x4b, staticRules[0x19])})
            , new State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x42},
               new ushort[1] {0xF3},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1A]), new Reduction(0xb, staticRules[0x1A]), new Reduction(0x11, staticRules[0x1A]), new Reduction(0x30, staticRules[0x1A]), new Reduction(0x31, staticRules[0x1A]), new Reduction(0x32, staticRules[0x1A]), new Reduction(0x33, staticRules[0x1A]), new Reduction(0x34, staticRules[0x1A]), new Reduction(0x35, staticRules[0x1A]), new Reduction(0x41, staticRules[0x1A]), new Reduction(0x43, staticRules[0x1A]), new Reduction(0x44, staticRules[0x1A]), new Reduction(0x45, staticRules[0x1A]), new Reduction(0x46, staticRules[0x1A]), new Reduction(0x47, staticRules[0x1A]), new Reduction(0x49, staticRules[0x1A]), new Reduction(0x4a, staticRules[0x1A]), new Reduction(0x4b, staticRules[0x1A])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1B]), new Reduction(0xb, staticRules[0x1B]), new Reduction(0x11, staticRules[0x1B]), new Reduction(0x30, staticRules[0x1B]), new Reduction(0x31, staticRules[0x1B]), new Reduction(0x32, staticRules[0x1B]), new Reduction(0x33, staticRules[0x1B]), new Reduction(0x34, staticRules[0x1B]), new Reduction(0x35, staticRules[0x1B]), new Reduction(0x41, staticRules[0x1B]), new Reduction(0x43, staticRules[0x1B]), new Reduction(0x44, staticRules[0x1B]), new Reduction(0x45, staticRules[0x1B]), new Reduction(0x46, staticRules[0x1B]), new Reduction(0x47, staticRules[0x1B]), new Reduction(0x49, staticRules[0x1B]), new Reduction(0x4a, staticRules[0x1B]), new Reduction(0x4b, staticRules[0x1B])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1C]), new Reduction(0xb, staticRules[0x1C]), new Reduction(0x11, staticRules[0x1C]), new Reduction(0x30, staticRules[0x1C]), new Reduction(0x31, staticRules[0x1C]), new Reduction(0x32, staticRules[0x1C]), new Reduction(0x33, staticRules[0x1C]), new Reduction(0x34, staticRules[0x1C]), new Reduction(0x35, staticRules[0x1C]), new Reduction(0x41, staticRules[0x1C]), new Reduction(0x43, staticRules[0x1C]), new Reduction(0x44, staticRules[0x1C]), new Reduction(0x45, staticRules[0x1C]), new Reduction(0x46, staticRules[0x1C]), new Reduction(0x47, staticRules[0x1C]), new Reduction(0x49, staticRules[0x1C]), new Reduction(0x4a, staticRules[0x1C]), new Reduction(0x4b, staticRules[0x1C])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1D]), new Reduction(0xb, staticRules[0x1D]), new Reduction(0x11, staticRules[0x1D]), new Reduction(0x30, staticRules[0x1D]), new Reduction(0x31, staticRules[0x1D]), new Reduction(0x32, staticRules[0x1D]), new Reduction(0x33, staticRules[0x1D]), new Reduction(0x34, staticRules[0x1D]), new Reduction(0x35, staticRules[0x1D]), new Reduction(0x41, staticRules[0x1D]), new Reduction(0x43, staticRules[0x1D]), new Reduction(0x44, staticRules[0x1D]), new Reduction(0x45, staticRules[0x1D]), new Reduction(0x46, staticRules[0x1D]), new Reduction(0x47, staticRules[0x1D]), new Reduction(0x49, staticRules[0x1D]), new Reduction(0x4a, staticRules[0x1D]), new Reduction(0x4b, staticRules[0x1D])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1E]), new Reduction(0xb, staticRules[0x1E]), new Reduction(0x11, staticRules[0x1E]), new Reduction(0x30, staticRules[0x1E]), new Reduction(0x31, staticRules[0x1E]), new Reduction(0x32, staticRules[0x1E]), new Reduction(0x33, staticRules[0x1E]), new Reduction(0x34, staticRules[0x1E]), new Reduction(0x35, staticRules[0x1E]), new Reduction(0x41, staticRules[0x1E]), new Reduction(0x43, staticRules[0x1E]), new Reduction(0x44, staticRules[0x1E]), new Reduction(0x45, staticRules[0x1E]), new Reduction(0x46, staticRules[0x1E]), new Reduction(0x47, staticRules[0x1E]), new Reduction(0x49, staticRules[0x1E]), new Reduction(0x4a, staticRules[0x1E]), new Reduction(0x4b, staticRules[0x1E])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x1F]), new Reduction(0xb, staticRules[0x1F]), new Reduction(0x11, staticRules[0x1F]), new Reduction(0x30, staticRules[0x1F]), new Reduction(0x31, staticRules[0x1F]), new Reduction(0x32, staticRules[0x1F]), new Reduction(0x33, staticRules[0x1F]), new Reduction(0x34, staticRules[0x1F]), new Reduction(0x35, staticRules[0x1F]), new Reduction(0x41, staticRules[0x1F]), new Reduction(0x43, staticRules[0x1F]), new Reduction(0x44, staticRules[0x1F]), new Reduction(0x45, staticRules[0x1F]), new Reduction(0x46, staticRules[0x1F]), new Reduction(0x47, staticRules[0x1F]), new Reduction(0x49, staticRules[0x1F]), new Reduction(0x4a, staticRules[0x1F]), new Reduction(0x4b, staticRules[0x1F])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x20]), new Reduction(0xb, staticRules[0x20]), new Reduction(0x11, staticRules[0x20]), new Reduction(0x30, staticRules[0x20]), new Reduction(0x31, staticRules[0x20]), new Reduction(0x32, staticRules[0x20]), new Reduction(0x33, staticRules[0x20]), new Reduction(0x34, staticRules[0x20]), new Reduction(0x35, staticRules[0x20]), new Reduction(0x41, staticRules[0x20]), new Reduction(0x43, staticRules[0x20]), new Reduction(0x44, staticRules[0x20]), new Reduction(0x45, staticRules[0x20]), new Reduction(0x46, staticRules[0x20]), new Reduction(0x47, staticRules[0x20]), new Reduction(0x49, staticRules[0x20]), new Reduction(0x4a, staticRules[0x20]), new Reduction(0x4b, staticRules[0x20])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x11]), new Reduction(0xb, staticRules[0x11]), new Reduction(0x11, staticRules[0x11]), new Reduction(0x30, staticRules[0x11]), new Reduction(0x31, staticRules[0x11]), new Reduction(0x32, staticRules[0x11]), new Reduction(0x33, staticRules[0x11]), new Reduction(0x34, staticRules[0x11]), new Reduction(0x35, staticRules[0x11]), new Reduction(0x41, staticRules[0x11]), new Reduction(0x43, staticRules[0x11]), new Reduction(0x44, staticRules[0x11]), new Reduction(0x45, staticRules[0x11]), new Reduction(0x46, staticRules[0x11]), new Reduction(0x47, staticRules[0x11]), new Reduction(0x49, staticRules[0x11]), new Reduction(0x4a, staticRules[0x11]), new Reduction(0x4b, staticRules[0x11])})
            , new State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[19] {new Reduction(0xa, staticRules[0x12]), new Reduction(0xb, staticRules[0x12]), new Reduction(0x11, staticRules[0x12]), new Reduction(0x30, staticRules[0x12]), new Reduction(0x31, staticRules[0x12]), new Reduction(0x32, staticRules[0x12]), new Reduction(0x33, staticRules[0x12]), new Reduction(0x34, staticRules[0x12]), new Reduction(0x35, staticRules[0x12]), new Reduction(0x41, staticRules[0x12]), new Reduction(0x42, staticRules[0x12]), new Reduction(0x43, staticRules[0x12]), new Reduction(0x44, staticRules[0x12]), new Reduction(0x45, staticRules[0x12]), new Reduction(0x46, staticRules[0x12]), new Reduction(0x47, staticRules[0x12]), new Reduction(0x49, staticRules[0x12]), new Reduction(0x4a, staticRules[0x12]), new Reduction(0x4b, staticRules[0x12])})
            , new State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[19] {new Reduction(0xa, staticRules[0x13]), new Reduction(0xb, staticRules[0x13]), new Reduction(0x11, staticRules[0x13]), new Reduction(0x30, staticRules[0x13]), new Reduction(0x31, staticRules[0x13]), new Reduction(0x32, staticRules[0x13]), new Reduction(0x33, staticRules[0x13]), new Reduction(0x34, staticRules[0x13]), new Reduction(0x35, staticRules[0x13]), new Reduction(0x41, staticRules[0x13]), new Reduction(0x42, staticRules[0x13]), new Reduction(0x43, staticRules[0x13]), new Reduction(0x44, staticRules[0x13]), new Reduction(0x45, staticRules[0x13]), new Reduction(0x46, staticRules[0x13]), new Reduction(0x47, staticRules[0x13]), new Reduction(0x49, staticRules[0x13]), new Reduction(0x4a, staticRules[0x13]), new Reduction(0x4b, staticRules[0x13])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x15]), new Reduction(0xb, staticRules[0x15]), new Reduction(0x11, staticRules[0x15]), new Reduction(0x30, staticRules[0x15]), new Reduction(0x31, staticRules[0x15]), new Reduction(0x32, staticRules[0x15]), new Reduction(0x33, staticRules[0x15]), new Reduction(0x34, staticRules[0x15]), new Reduction(0x35, staticRules[0x15]), new Reduction(0x41, staticRules[0x15]), new Reduction(0x43, staticRules[0x15]), new Reduction(0x44, staticRules[0x15]), new Reduction(0x45, staticRules[0x15]), new Reduction(0x46, staticRules[0x15]), new Reduction(0x47, staticRules[0x15]), new Reduction(0x49, staticRules[0x15]), new Reduction(0x4a, staticRules[0x15]), new Reduction(0x4b, staticRules[0x15])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x17]), new Reduction(0xb, staticRules[0x17]), new Reduction(0x11, staticRules[0x17]), new Reduction(0x30, staticRules[0x17]), new Reduction(0x31, staticRules[0x17]), new Reduction(0x32, staticRules[0x17]), new Reduction(0x33, staticRules[0x17]), new Reduction(0x34, staticRules[0x17]), new Reduction(0x35, staticRules[0x17]), new Reduction(0x41, staticRules[0x17]), new Reduction(0x43, staticRules[0x17]), new Reduction(0x44, staticRules[0x17]), new Reduction(0x45, staticRules[0x17]), new Reduction(0x46, staticRules[0x17]), new Reduction(0x47, staticRules[0x17]), new Reduction(0x49, staticRules[0x17]), new Reduction(0x4a, staticRules[0x17]), new Reduction(0x4b, staticRules[0x17])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x16]), new Reduction(0xb, staticRules[0x16]), new Reduction(0x11, staticRules[0x16]), new Reduction(0x30, staticRules[0x16]), new Reduction(0x31, staticRules[0x16]), new Reduction(0x32, staticRules[0x16]), new Reduction(0x33, staticRules[0x16]), new Reduction(0x34, staticRules[0x16]), new Reduction(0x35, staticRules[0x16]), new Reduction(0x41, staticRules[0x16]), new Reduction(0x43, staticRules[0x16]), new Reduction(0x44, staticRules[0x16]), new Reduction(0x45, staticRules[0x16]), new Reduction(0x46, staticRules[0x16]), new Reduction(0x47, staticRules[0x16]), new Reduction(0x49, staticRules[0x16]), new Reduction(0x4a, staticRules[0x16]), new Reduction(0x4b, staticRules[0x16])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0xF4, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0xF5},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAB]), new Reduction(0x4c, staticRules[0xAB]), new Reduction(0x4d, staticRules[0xAB])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x7E]), new Reduction(0x44, staticRules[0x7E]), new Reduction(0xdb, staticRules[0x7E])})
            , new State(
               null,
               new SymbolTerminal[20] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[12] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[12] {0xF6, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[4] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x44, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80]), new Reduction(0xdb, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x81]), new Reduction(0x44, staticRules[0x81]), new Reduction(0x4a, staticRules[0x81]), new Reduction(0xdb, staticRules[0x81])})
            , new State(
               null,
               new SymbolTerminal[16] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xF7, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x82]), new Reduction(0x44, staticRules[0x82]), new Reduction(0x49, staticRules[0x82]), new Reduction(0x4a, staticRules[0x82]), new Reduction(0xdb, staticRules[0x82])})
            , new State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[10] {0x9a, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[10] {0xF8, 0xC2, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[5] {new Reduction(0x41, staticRules[0x96]), new Reduction(0x44, staticRules[0x96]), new Reduction(0x49, staticRules[0x96]), new Reduction(0x4a, staticRules[0x96]), new Reduction(0xdb, staticRules[0x96])})
            , new State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x83]), new Reduction(0x11, staticRules[0x83]), new Reduction(0x2e, staticRules[0x83]), new Reduction(0x34, staticRules[0x83]), new Reduction(0x35, staticRules[0x83]), new Reduction(0x36, staticRules[0x83]), new Reduction(0x37, staticRules[0x83]), new Reduction(0x38, staticRules[0x83]), new Reduction(0x39, staticRules[0x83]), new Reduction(0x3a, staticRules[0x83]), new Reduction(0x3b, staticRules[0x83]), new Reduction(0x3c, staticRules[0x83]), new Reduction(0x3d, staticRules[0x83]), new Reduction(0x3e, staticRules[0x83]), new Reduction(0x3f, staticRules[0x83]), new Reduction(0x41, staticRules[0x83]), new Reduction(0x43, staticRules[0x83]), new Reduction(0x44, staticRules[0x83]), new Reduction(0x49, staticRules[0x83]), new Reduction(0x4a, staticRules[0x83]), new Reduction(0xdb, staticRules[0x83])})
            , new State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x84]), new Reduction(0x11, staticRules[0x84]), new Reduction(0x2e, staticRules[0x84]), new Reduction(0x34, staticRules[0x84]), new Reduction(0x35, staticRules[0x84]), new Reduction(0x36, staticRules[0x84]), new Reduction(0x37, staticRules[0x84]), new Reduction(0x38, staticRules[0x84]), new Reduction(0x39, staticRules[0x84]), new Reduction(0x3a, staticRules[0x84]), new Reduction(0x3b, staticRules[0x84]), new Reduction(0x3c, staticRules[0x84]), new Reduction(0x3d, staticRules[0x84]), new Reduction(0x3e, staticRules[0x84]), new Reduction(0x3f, staticRules[0x84]), new Reduction(0x41, staticRules[0x84]), new Reduction(0x43, staticRules[0x84]), new Reduction(0x44, staticRules[0x84]), new Reduction(0x49, staticRules[0x84]), new Reduction(0x4a, staticRules[0x84]), new Reduction(0xdb, staticRules[0x84])})
            , new State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[21] {new Reduction(0xa, staticRules[0x85]), new Reduction(0x11, staticRules[0x85]), new Reduction(0x2e, staticRules[0x85]), new Reduction(0x34, staticRules[0x85]), new Reduction(0x35, staticRules[0x85]), new Reduction(0x36, staticRules[0x85]), new Reduction(0x37, staticRules[0x85]), new Reduction(0x38, staticRules[0x85]), new Reduction(0x39, staticRules[0x85]), new Reduction(0x3a, staticRules[0x85]), new Reduction(0x3b, staticRules[0x85]), new Reduction(0x3c, staticRules[0x85]), new Reduction(0x3d, staticRules[0x85]), new Reduction(0x3e, staticRules[0x85]), new Reduction(0x3f, staticRules[0x85]), new Reduction(0x41, staticRules[0x85]), new Reduction(0x43, staticRules[0x85]), new Reduction(0x44, staticRules[0x85]), new Reduction(0x49, staticRules[0x85]), new Reduction(0x4a, staticRules[0x85]), new Reduction(0xdb, staticRules[0x85])})
            , new State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x87]), new Reduction(0x11, staticRules[0x87]), new Reduction(0x2e, staticRules[0x87]), new Reduction(0x34, staticRules[0x87]), new Reduction(0x35, staticRules[0x87]), new Reduction(0x36, staticRules[0x87]), new Reduction(0x37, staticRules[0x87]), new Reduction(0x38, staticRules[0x87]), new Reduction(0x39, staticRules[0x87]), new Reduction(0x3a, staticRules[0x87]), new Reduction(0x3b, staticRules[0x87]), new Reduction(0x3c, staticRules[0x87]), new Reduction(0x3d, staticRules[0x87]), new Reduction(0x3e, staticRules[0x87]), new Reduction(0x3f, staticRules[0x87]), new Reduction(0x41, staticRules[0x87]), new Reduction(0x43, staticRules[0x87]), new Reduction(0x44, staticRules[0x87]), new Reduction(0x45, staticRules[0x87]), new Reduction(0x46, staticRules[0x87]), new Reduction(0x47, staticRules[0x87]), new Reduction(0x49, staticRules[0x87]), new Reduction(0x4a, staticRules[0x87]), new Reduction(0xdb, staticRules[0x87])})
            , new State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[24] {new Reduction(0xa, staticRules[0x88]), new Reduction(0x11, staticRules[0x88]), new Reduction(0x2e, staticRules[0x88]), new Reduction(0x34, staticRules[0x88]), new Reduction(0x35, staticRules[0x88]), new Reduction(0x36, staticRules[0x88]), new Reduction(0x37, staticRules[0x88]), new Reduction(0x38, staticRules[0x88]), new Reduction(0x39, staticRules[0x88]), new Reduction(0x3a, staticRules[0x88]), new Reduction(0x3b, staticRules[0x88]), new Reduction(0x3c, staticRules[0x88]), new Reduction(0x3d, staticRules[0x88]), new Reduction(0x3e, staticRules[0x88]), new Reduction(0x3f, staticRules[0x88]), new Reduction(0x41, staticRules[0x88]), new Reduction(0x43, staticRules[0x88]), new Reduction(0x44, staticRules[0x88]), new Reduction(0x45, staticRules[0x88]), new Reduction(0x46, staticRules[0x88]), new Reduction(0x47, staticRules[0x88]), new Reduction(0x49, staticRules[0x88]), new Reduction(0x4a, staticRules[0x88]), new Reduction(0xdb, staticRules[0x88])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0xF9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0xFA},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x91]), new Reduction(0x11, staticRules[0x91]), new Reduction(0x2e, staticRules[0x91]), new Reduction(0x34, staticRules[0x91]), new Reduction(0x35, staticRules[0x91]), new Reduction(0x36, staticRules[0x91]), new Reduction(0x37, staticRules[0x91]), new Reduction(0x38, staticRules[0x91]), new Reduction(0x39, staticRules[0x91]), new Reduction(0x3a, staticRules[0x91]), new Reduction(0x3b, staticRules[0x91]), new Reduction(0x3c, staticRules[0x91]), new Reduction(0x3d, staticRules[0x91]), new Reduction(0x3e, staticRules[0x91]), new Reduction(0x3f, staticRules[0x91]), new Reduction(0x41, staticRules[0x91]), new Reduction(0x43, staticRules[0x91]), new Reduction(0x44, staticRules[0x91]), new Reduction(0x45, staticRules[0x91]), new Reduction(0x46, staticRules[0x91]), new Reduction(0x47, staticRules[0x91]), new Reduction(0x48, staticRules[0x91]), new Reduction(0x49, staticRules[0x91]), new Reduction(0x4a, staticRules[0x91]), new Reduction(0x4e, staticRules[0x91]), new Reduction(0x55, staticRules[0x91]), new Reduction(0x56, staticRules[0x91]), new Reduction(0xdb, staticRules[0x91])})
            , new State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35]},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[6] {0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[6] {0xFB, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x10]), new Reduction(0x12, staticRules[0x10])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x59])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x79])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x12, staticRules[0x7A])})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0xFC, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0xFD},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0xFE},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0xFF, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x101},
               new ushort[1] {0x77},
               new ushort[1] {0x100},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x52])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x102, 0x90},
               new ushort[1] {0x6b},
               new ushort[1] {0x103},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA3]), new Reduction(0x4c, staticRules[0xA3]), new Reduction(0x4d, staticRules[0xA3])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x5B]), new Reduction(0x44, staticRules[0x5B]), new Reduction(0xdb, staticRules[0x5B])})
            , new State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[13] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[13] {0x104, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x44, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D]), new Reduction(0xdb, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x5E]), new Reduction(0x44, staticRules[0x5E]), new Reduction(0x4a, staticRules[0x5E]), new Reduction(0xdb, staticRules[0x5E])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0x105, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x5F]), new Reduction(0x44, staticRules[0x5F]), new Reduction(0x49, staticRules[0x5F]), new Reduction(0x4a, staticRules[0x5F]), new Reduction(0xdb, staticRules[0x5F])})
            , new State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[11] {0x88, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[11] {0x106, 0xDC, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[5] {new Reduction(0x41, staticRules[0x73]), new Reduction(0x44, staticRules[0x73]), new Reduction(0x49, staticRules[0x73]), new Reduction(0x4a, staticRules[0x73]), new Reduction(0xdb, staticRules[0x73])})
            , new State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x60]), new Reduction(0x11, staticRules[0x60]), new Reduction(0x2e, staticRules[0x60]), new Reduction(0x30, staticRules[0x60]), new Reduction(0x41, staticRules[0x60]), new Reduction(0x43, staticRules[0x60]), new Reduction(0x44, staticRules[0x60]), new Reduction(0x49, staticRules[0x60]), new Reduction(0x4a, staticRules[0x60]), new Reduction(0xdb, staticRules[0x60])})
            , new State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x61]), new Reduction(0x11, staticRules[0x61]), new Reduction(0x2e, staticRules[0x61]), new Reduction(0x30, staticRules[0x61]), new Reduction(0x41, staticRules[0x61]), new Reduction(0x43, staticRules[0x61]), new Reduction(0x44, staticRules[0x61]), new Reduction(0x49, staticRules[0x61]), new Reduction(0x4a, staticRules[0x61]), new Reduction(0xdb, staticRules[0x61])})
            , new State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[10] {new Reduction(0xa, staticRules[0x62]), new Reduction(0x11, staticRules[0x62]), new Reduction(0x2e, staticRules[0x62]), new Reduction(0x30, staticRules[0x62]), new Reduction(0x41, staticRules[0x62]), new Reduction(0x43, staticRules[0x62]), new Reduction(0x44, staticRules[0x62]), new Reduction(0x49, staticRules[0x62]), new Reduction(0x4a, staticRules[0x62]), new Reduction(0xdb, staticRules[0x62])})
            , new State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x64]), new Reduction(0x11, staticRules[0x64]), new Reduction(0x2e, staticRules[0x64]), new Reduction(0x30, staticRules[0x64]), new Reduction(0x41, staticRules[0x64]), new Reduction(0x43, staticRules[0x64]), new Reduction(0x44, staticRules[0x64]), new Reduction(0x45, staticRules[0x64]), new Reduction(0x46, staticRules[0x64]), new Reduction(0x47, staticRules[0x64]), new Reduction(0x49, staticRules[0x64]), new Reduction(0x4a, staticRules[0x64]), new Reduction(0xdb, staticRules[0x64])})
            , new State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[13] {new Reduction(0xa, staticRules[0x65]), new Reduction(0x11, staticRules[0x65]), new Reduction(0x2e, staticRules[0x65]), new Reduction(0x30, staticRules[0x65]), new Reduction(0x41, staticRules[0x65]), new Reduction(0x43, staticRules[0x65]), new Reduction(0x44, staticRules[0x65]), new Reduction(0x45, staticRules[0x65]), new Reduction(0x46, staticRules[0x65]), new Reduction(0x47, staticRules[0x65]), new Reduction(0x49, staticRules[0x65]), new Reduction(0x4a, staticRules[0x65]), new Reduction(0xdb, staticRules[0x65])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0x107},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6E]), new Reduction(0x11, staticRules[0x6E]), new Reduction(0x2e, staticRules[0x6E]), new Reduction(0x30, staticRules[0x6E]), new Reduction(0x41, staticRules[0x6E]), new Reduction(0x43, staticRules[0x6E]), new Reduction(0x44, staticRules[0x6E]), new Reduction(0x45, staticRules[0x6E]), new Reduction(0x46, staticRules[0x6E]), new Reduction(0x47, staticRules[0x6E]), new Reduction(0x48, staticRules[0x6E]), new Reduction(0x49, staticRules[0x6E]), new Reduction(0x4a, staticRules[0x6E]), new Reduction(0x4e, staticRules[0x6E]), new Reduction(0x55, staticRules[0x6E]), new Reduction(0x56, staticRules[0x6E]), new Reduction(0xdb, staticRules[0x6E])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32]},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x77, 0x78, 0xA3, 0xA5},
               new ushort[7] {0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[7] {0x108, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x109},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x10A},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x2C]), new Reduction(0x44, staticRules[0x2C]), new Reduction(0x4b, staticRules[0x2C])})
            , new State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[12] {0x64, 0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[12] {0x10B, 0xA8, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x2B]), new Reduction(0x44, staticRules[0x2B]), new Reduction(0x4a, staticRules[0x2B]), new Reduction(0x4b, staticRules[0x2B])})
            , new State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x63, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0x10C, 0xA9, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x2A]), new Reduction(0x44, staticRules[0x2A]), new Reduction(0x49, staticRules[0x2A]), new Reduction(0x4a, staticRules[0x2A]), new Reduction(0x4b, staticRules[0x2A])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xAC, 0xB4, 0xB5, 0xB6, 0xB7, 0xA5, 0xB8, 0xB9, 0xBA},
               new ushort[11] {0x74, 0x62, 0x60, 0x5f, 0x58, 0x59, 0x5a, 0x5b, 0x5e, 0x5d, 0x5c},
               new ushort[11] {0x10D, 0xEC, 0xAA, 0xAB, 0xAD, 0xAE, 0xAF, 0xB0, 0xB1, 0xB2, 0xB3},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4C]), new Reduction(0x44, staticRules[0x4C]), new Reduction(0x49, staticRules[0x4C]), new Reduction(0x4a, staticRules[0x4C]), new Reduction(0x4b, staticRules[0x4C])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x28]), new Reduction(0xb, staticRules[0x28]), new Reduction(0x30, staticRules[0x28]), new Reduction(0x31, staticRules[0x28]), new Reduction(0x32, staticRules[0x28]), new Reduction(0x33, staticRules[0x28]), new Reduction(0x34, staticRules[0x28]), new Reduction(0x35, staticRules[0x28]), new Reduction(0x41, staticRules[0x28]), new Reduction(0x43, staticRules[0x28]), new Reduction(0x44, staticRules[0x28]), new Reduction(0x49, staticRules[0x28]), new Reduction(0x4a, staticRules[0x28]), new Reduction(0x4b, staticRules[0x28])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x23]), new Reduction(0xb, staticRules[0x23]), new Reduction(0x30, staticRules[0x23]), new Reduction(0x31, staticRules[0x23]), new Reduction(0x32, staticRules[0x23]), new Reduction(0x33, staticRules[0x23]), new Reduction(0x34, staticRules[0x23]), new Reduction(0x35, staticRules[0x23]), new Reduction(0x41, staticRules[0x23]), new Reduction(0x43, staticRules[0x23]), new Reduction(0x44, staticRules[0x23]), new Reduction(0x49, staticRules[0x23]), new Reduction(0x4a, staticRules[0x23]), new Reduction(0x4b, staticRules[0x23])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x24]), new Reduction(0xb, staticRules[0x24]), new Reduction(0x30, staticRules[0x24]), new Reduction(0x31, staticRules[0x24]), new Reduction(0x32, staticRules[0x24]), new Reduction(0x33, staticRules[0x24]), new Reduction(0x34, staticRules[0x24]), new Reduction(0x35, staticRules[0x24]), new Reduction(0x41, staticRules[0x24]), new Reduction(0x43, staticRules[0x24]), new Reduction(0x44, staticRules[0x24]), new Reduction(0x49, staticRules[0x24]), new Reduction(0x4a, staticRules[0x24]), new Reduction(0x4b, staticRules[0x24])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x25]), new Reduction(0xb, staticRules[0x25]), new Reduction(0x30, staticRules[0x25]), new Reduction(0x31, staticRules[0x25]), new Reduction(0x32, staticRules[0x25]), new Reduction(0x33, staticRules[0x25]), new Reduction(0x34, staticRules[0x25]), new Reduction(0x35, staticRules[0x25]), new Reduction(0x41, staticRules[0x25]), new Reduction(0x43, staticRules[0x25]), new Reduction(0x44, staticRules[0x25]), new Reduction(0x49, staticRules[0x25]), new Reduction(0x4a, staticRules[0x25]), new Reduction(0x4b, staticRules[0x25])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[7]},
               new ushort[1] {0x2d},
               new ushort[1] {0x10E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0x10F},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42]},
               new ushort[2] {0x34, 0x35},
               new ushort[2] {0xB6, 0xB7},
               new ushort[1] {0x59},
               new ushort[1] {0x110},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x111, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x112},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xBE},
               new ushort[1] {0x9c},
               new ushort[1] {0x113},
               new Reduction[3] {new Reduction(0x41, staticRules[0x9A]), new Reduction(0x44, staticRules[0x9A]), new Reduction(0xdb, staticRules[0x9A])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xC0},
               new ushort[1] {0x9b},
               new ushort[1] {0x114},
               new Reduction[4] {new Reduction(0x41, staticRules[0x98]), new Reduction(0x44, staticRules[0x98]), new Reduction(0x4a, staticRules[0x98]), new Reduction(0xdb, staticRules[0x98])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x95]), new Reduction(0x44, staticRules[0x95]), new Reduction(0x49, staticRules[0x95]), new Reduction(0x4a, staticRules[0x95]), new Reduction(0xdb, staticRules[0x95])})
            , new State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[26] {new Reduction(0xa, staticRules[0x8B]), new Reduction(0x11, staticRules[0x8B]), new Reduction(0x2e, staticRules[0x8B]), new Reduction(0x34, staticRules[0x8B]), new Reduction(0x35, staticRules[0x8B]), new Reduction(0x36, staticRules[0x8B]), new Reduction(0x37, staticRules[0x8B]), new Reduction(0x38, staticRules[0x8B]), new Reduction(0x39, staticRules[0x8B]), new Reduction(0x3a, staticRules[0x8B]), new Reduction(0x3b, staticRules[0x8B]), new Reduction(0x3c, staticRules[0x8B]), new Reduction(0x3d, staticRules[0x8B]), new Reduction(0x3e, staticRules[0x8B]), new Reduction(0x3f, staticRules[0x8B]), new Reduction(0x41, staticRules[0x8B]), new Reduction(0x43, staticRules[0x8B]), new Reduction(0x44, staticRules[0x8B]), new Reduction(0x45, staticRules[0x8B]), new Reduction(0x46, staticRules[0x8B]), new Reduction(0x47, staticRules[0x8B]), new Reduction(0x49, staticRules[0x8B]), new Reduction(0x4a, staticRules[0x8B]), new Reduction(0x55, staticRules[0x8B]), new Reduction(0x56, staticRules[0x8B]), new Reduction(0xdb, staticRules[0x8B])})
            , new State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[29] {new Reduction(0xa, staticRules[0x30]), new Reduction(0x11, staticRules[0x30]), new Reduction(0x2e, staticRules[0x30]), new Reduction(0x30, staticRules[0x30]), new Reduction(0x34, staticRules[0x30]), new Reduction(0x35, staticRules[0x30]), new Reduction(0x36, staticRules[0x30]), new Reduction(0x37, staticRules[0x30]), new Reduction(0x38, staticRules[0x30]), new Reduction(0x39, staticRules[0x30]), new Reduction(0x3a, staticRules[0x30]), new Reduction(0x3b, staticRules[0x30]), new Reduction(0x3c, staticRules[0x30]), new Reduction(0x3d, staticRules[0x30]), new Reduction(0x3e, staticRules[0x30]), new Reduction(0x3f, staticRules[0x30]), new Reduction(0x41, staticRules[0x30]), new Reduction(0x43, staticRules[0x30]), new Reduction(0x44, staticRules[0x30]), new Reduction(0x45, staticRules[0x30]), new Reduction(0x46, staticRules[0x30]), new Reduction(0x47, staticRules[0x30]), new Reduction(0x48, staticRules[0x30]), new Reduction(0x49, staticRules[0x30]), new Reduction(0x4a, staticRules[0x30]), new Reduction(0x4e, staticRules[0x30]), new Reduction(0x55, staticRules[0x30]), new Reduction(0x56, staticRules[0x30]), new Reduction(0xdb, staticRules[0x30])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x116},
               new ushort[1] {0x99},
               new ushort[1] {0x115},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x94])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x117},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x118, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x7D]), new Reduction(0x12, staticRules[0x7D])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x119},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x4e},
               new ushort[1] {0x11A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x11B},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x11C, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x11D},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xD8},
               new ushort[1] {0x8a},
               new ushort[1] {0x11E},
               new Reduction[3] {new Reduction(0x41, staticRules[0x77]), new Reduction(0x44, staticRules[0x77]), new Reduction(0xdb, staticRules[0x77])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xDA},
               new ushort[1] {0x89},
               new ushort[1] {0x11F},
               new Reduction[4] {new Reduction(0x41, staticRules[0x75]), new Reduction(0x44, staticRules[0x75]), new Reduction(0x4a, staticRules[0x75]), new Reduction(0xdb, staticRules[0x75])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x72]), new Reduction(0x44, staticRules[0x72]), new Reduction(0x49, staticRules[0x72]), new Reduction(0x4a, staticRules[0x72]), new Reduction(0xdb, staticRules[0x72])})
            , new State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[15] {new Reduction(0xa, staticRules[0x68]), new Reduction(0x11, staticRules[0x68]), new Reduction(0x2e, staticRules[0x68]), new Reduction(0x30, staticRules[0x68]), new Reduction(0x41, staticRules[0x68]), new Reduction(0x43, staticRules[0x68]), new Reduction(0x44, staticRules[0x68]), new Reduction(0x45, staticRules[0x68]), new Reduction(0x46, staticRules[0x68]), new Reduction(0x47, staticRules[0x68]), new Reduction(0x49, staticRules[0x68]), new Reduction(0x4a, staticRules[0x68]), new Reduction(0x55, staticRules[0x68]), new Reduction(0x56, staticRules[0x68]), new Reduction(0xdb, staticRules[0x68])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x121},
               new ushort[1] {0x87},
               new ushort[1] {0x120},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x71])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x2F]), new Reduction(0x12, staticRules[0x2F])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x41, staticRules[0x2D])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4a},
               new ushort[1] {0xE8},
               new ushort[1] {0x76},
               new ushort[1] {0x122},
               new Reduction[3] {new Reduction(0x41, staticRules[0x50]), new Reduction(0x44, staticRules[0x50]), new Reduction(0x4b, staticRules[0x50])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x49},
               new ushort[1] {0xEA},
               new ushort[1] {0x75},
               new ushort[1] {0x123},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4E]), new Reduction(0x44, staticRules[0x4E]), new Reduction(0x4a, staticRules[0x4E]), new Reduction(0x4b, staticRules[0x4E])})
            , new State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x41, staticRules[0x4B]), new Reduction(0x44, staticRules[0x4B]), new Reduction(0x49, staticRules[0x4B]), new Reduction(0x4a, staticRules[0x4B]), new Reduction(0x4b, staticRules[0x4B])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[15]},
               new ushort[2] {0x48, 0x12},
               new ushort[2] {0x124, 0x125},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x22]), new Reduction(0xb, staticRules[0x22]), new Reduction(0x11, staticRules[0x22]), new Reduction(0x30, staticRules[0x22]), new Reduction(0x31, staticRules[0x22]), new Reduction(0x32, staticRules[0x22]), new Reduction(0x33, staticRules[0x22]), new Reduction(0x34, staticRules[0x22]), new Reduction(0x35, staticRules[0x22]), new Reduction(0x41, staticRules[0x22]), new Reduction(0x43, staticRules[0x22]), new Reduction(0x44, staticRules[0x22]), new Reduction(0x45, staticRules[0x22]), new Reduction(0x46, staticRules[0x22]), new Reduction(0x47, staticRules[0x22]), new Reduction(0x49, staticRules[0x22]), new Reduction(0x4a, staticRules[0x22]), new Reduction(0x4b, staticRules[0x22])})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[18] {new Reduction(0xa, staticRules[0x18]), new Reduction(0xb, staticRules[0x18]), new Reduction(0x11, staticRules[0x18]), new Reduction(0x30, staticRules[0x18]), new Reduction(0x31, staticRules[0x18]), new Reduction(0x32, staticRules[0x18]), new Reduction(0x33, staticRules[0x18]), new Reduction(0x34, staticRules[0x18]), new Reduction(0x35, staticRules[0x18]), new Reduction(0x41, staticRules[0x18]), new Reduction(0x43, staticRules[0x18]), new Reduction(0x44, staticRules[0x18]), new Reduction(0x45, staticRules[0x18]), new Reduction(0x46, staticRules[0x18]), new Reduction(0x47, staticRules[0x18]), new Reduction(0x49, staticRules[0x18]), new Reduction(0x4a, staticRules[0x18]), new Reduction(0x4b, staticRules[0x18])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x126},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x71, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[13] {0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[13] {0x127, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[2] {new Reduction(0x41, staticRules[0x80]), new Reduction(0x4a, staticRules[0x80])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x99]), new Reduction(0x44, staticRules[0x99]), new Reduction(0xdb, staticRules[0x99])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x97]), new Reduction(0x44, staticRules[0x97]), new Reduction(0x4a, staticRules[0x97]), new Reduction(0xdb, staticRules[0x97])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x4e},
               new ushort[1] {0x128},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35]},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85},
               new ushort[6] {0x96, 0x68, 0x69, 0x6a, 0x97, 0x6c},
               new ushort[6] {0x129, 0x72, 0x73, 0x74, 0x75, 0x76},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x5A]), new Reduction(0x12, staticRules[0x5A])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x12A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x9B]), new Reduction(0x12, staticRules[0x9B])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4c, staticRules[0x33])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x101},
               new ushort[1] {0x77},
               new ushort[1] {0x12B},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x52])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x12C},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0x9D, 0x77, 0x78, 0xA3, 0xA5},
               new ushort[14] {0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[14] {0x12D, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[2] {new Reduction(0x41, staticRules[0x5D]), new Reduction(0x4a, staticRules[0x5D])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x76]), new Reduction(0x44, staticRules[0x76]), new Reduction(0xdb, staticRules[0x76])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x74]), new Reduction(0x44, staticRules[0x74]), new Reduction(0x4a, staticRules[0x74]), new Reduction(0xdb, staticRules[0x74])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x4e},
               new ushort[1] {0x12E},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32]},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x77, 0x78, 0xA3, 0xA5},
               new ushort[7] {0x84, 0x68, 0x69, 0x6a, 0x85, 0x6d, 0x5a},
               new ushort[7] {0x12F, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x41, staticRules[0x4F]), new Reduction(0x44, staticRules[0x4F]), new Reduction(0x4b, staticRules[0x4F])})
            , new State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x41, staticRules[0x4D]), new Reduction(0x44, staticRules[0x4D]), new Reduction(0x4a, staticRules[0x4D]), new Reduction(0x4b, staticRules[0x4D])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[7]},
               new ushort[1] {0x2d},
               new ushort[1] {0x130},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[14] {new Reduction(0xa, staticRules[0x27]), new Reduction(0xb, staticRules[0x27]), new Reduction(0x30, staticRules[0x27]), new Reduction(0x31, staticRules[0x27]), new Reduction(0x32, staticRules[0x27]), new Reduction(0x33, staticRules[0x27]), new Reduction(0x34, staticRules[0x27]), new Reduction(0x35, staticRules[0x27]), new Reduction(0x41, staticRules[0x27]), new Reduction(0x43, staticRules[0x27]), new Reduction(0x44, staticRules[0x27]), new Reduction(0x49, staticRules[0x27]), new Reduction(0x4a, staticRules[0x27]), new Reduction(0x4b, staticRules[0x27])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAA]), new Reduction(0x12, staticRules[0xAA]), new Reduction(0xda, staticRules[0xAA])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x131},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[28] {new Reduction(0xa, staticRules[0x92]), new Reduction(0x11, staticRules[0x92]), new Reduction(0x2e, staticRules[0x92]), new Reduction(0x34, staticRules[0x92]), new Reduction(0x35, staticRules[0x92]), new Reduction(0x36, staticRules[0x92]), new Reduction(0x37, staticRules[0x92]), new Reduction(0x38, staticRules[0x92]), new Reduction(0x39, staticRules[0x92]), new Reduction(0x3a, staticRules[0x92]), new Reduction(0x3b, staticRules[0x92]), new Reduction(0x3c, staticRules[0x92]), new Reduction(0x3d, staticRules[0x92]), new Reduction(0x3e, staticRules[0x92]), new Reduction(0x3f, staticRules[0x92]), new Reduction(0x41, staticRules[0x92]), new Reduction(0x43, staticRules[0x92]), new Reduction(0x44, staticRules[0x92]), new Reduction(0x45, staticRules[0x92]), new Reduction(0x46, staticRules[0x92]), new Reduction(0x47, staticRules[0x92]), new Reduction(0x48, staticRules[0x92]), new Reduction(0x49, staticRules[0x92]), new Reduction(0x4a, staticRules[0x92]), new Reduction(0x4e, staticRules[0x92]), new Reduction(0x55, staticRules[0x92]), new Reduction(0x56, staticRules[0x92]), new Reduction(0xdb, staticRules[0x92])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x116},
               new ushort[1] {0x99},
               new ushort[1] {0x132},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x94])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0xa, staticRules[0x78]), new Reduction(0x12, staticRules[0x78])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x51])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA2]), new Reduction(0x12, staticRules[0xA2]), new Reduction(0xda, staticRules[0xA2])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x133},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[17] {new Reduction(0xa, staticRules[0x6F]), new Reduction(0x11, staticRules[0x6F]), new Reduction(0x2e, staticRules[0x6F]), new Reduction(0x30, staticRules[0x6F]), new Reduction(0x41, staticRules[0x6F]), new Reduction(0x43, staticRules[0x6F]), new Reduction(0x44, staticRules[0x6F]), new Reduction(0x45, staticRules[0x6F]), new Reduction(0x46, staticRules[0x6F]), new Reduction(0x47, staticRules[0x6F]), new Reduction(0x48, staticRules[0x6F]), new Reduction(0x49, staticRules[0x6F]), new Reduction(0x4a, staticRules[0x6F]), new Reduction(0x4e, staticRules[0x6F]), new Reduction(0x55, staticRules[0x6F]), new Reduction(0x56, staticRules[0x6F]), new Reduction(0xdb, staticRules[0x6F])})
            , new State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[1] {0x48},
               new ushort[1] {0x121},
               new ushort[1] {0x87},
               new ushort[1] {0x134},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x71])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x135},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xAD]), new Reduction(0x12, staticRules[0xAD]), new Reduction(0xda, staticRules[0xAD])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x93])})
            , new State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xa, staticRules[0xA5]), new Reduction(0x12, staticRules[0xA5]), new Reduction(0xda, staticRules[0xA5])})
            , new State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x4e, staticRules[0x70])})
            , new State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[36], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
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
