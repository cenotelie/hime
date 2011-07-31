using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Hime.Kernel.Resources.Parser
{
    public class FileCentralDogma_Lexer : LexerText
    {
        public static readonly SymbolTerminal[] terminals = {
            new SymbolTerminal(0x1, "ε"),
            new SymbolTerminal(0x2, "$"),
            new SymbolTerminal(0xB, "."),
            new SymbolTerminal(0xA, "NAME"),
            new SymbolTerminal(0x11, "{"),
            new SymbolTerminal(0x12, "}"),
            new SymbolTerminal(0xDC, "["),
            new SymbolTerminal(0x2D, "INTEGER"),
            new SymbolTerminal(0x40, "="),
            new SymbolTerminal(0x41, ";"),
            new SymbolTerminal(0x43, "("),
            new SymbolTerminal(0x44, ")"),
            new SymbolTerminal(0x45, "*"),
            new SymbolTerminal(0x46, "+"),
            new SymbolTerminal(0x47, "?"),
            new SymbolTerminal(0x48, ","),
            new SymbolTerminal(0x49, "-"),
            new SymbolTerminal(0x4A, "|"),
            new SymbolTerminal(0x4D, "<"),
            new SymbolTerminal(0x4E, ">"),
            new SymbolTerminal(0x51, ":"),
            new SymbolTerminal(0x55, "^"),
            new SymbolTerminal(0x56, "!"),
            new SymbolTerminal(0xDD, "]"),
            new SymbolTerminal(0x7, "SEPARATOR"),
            new SymbolTerminal(0x42, ".."),
            new SymbolTerminal(0x2E, "QUOTED_DATA"),
            new SymbolTerminal(0x2F, "ESCAPEES"),
            new SymbolTerminal(0x4B, "=>"),
            new SymbolTerminal(0x4C, "->"),
            new SymbolTerminal(0x53, "cf"),
            new SymbolTerminal(0xDB, "cs"),
            new SymbolTerminal(0x30, "SYMBOL_TERMINAL_TEXT"),
            new SymbolTerminal(0x31, "SYMBOL_TERMINAL_SET"),
            new SymbolTerminal(0x39, "SYMBOL_VALUE_BINARY"),
            new SymbolTerminal(0x3F, "SYMBOL_JOKER_BINARY"),
            new SymbolTerminal(0x57, "bin"),
            new SymbolTerminal(0x34, "SYMBOL_VALUE_UINT8"),
            new SymbolTerminal(0x3A, "SYMBOL_JOKER_UINT8"),
            new SymbolTerminal(0x54, "rules"),
            new SymbolTerminal(0xC, "public"),
            new SymbolTerminal(0x32, "SYMBOL_TERMINAL_UBLOCK"),
            new SymbolTerminal(0x33, "SYMBOL_TERMINAL_UCAT"),
            new SymbolTerminal(0x35, "SYMBOL_VALUE_UINT16"),
            new SymbolTerminal(0x3B, "SYMBOL_JOKER_UINT16"),
            new SymbolTerminal(0xD, "private"),
            new SymbolTerminal(0x4F, "options"),
            new SymbolTerminal(0x52, "grammar"),
            new SymbolTerminal(0xF, "internal"),
            new SymbolTerminal(0xE, "protected"),
            new SymbolTerminal(0x10, "namespace"),
            new SymbolTerminal(0x50, "terminals"),
            new SymbolTerminal(0x36, "SYMBOL_VALUE_UINT32"),
            new SymbolTerminal(0x3C, "SYMBOL_JOKER_UINT32"),
            new SymbolTerminal(0x37, "SYMBOL_VALUE_UINT64"),
            new SymbolTerminal(0x3D, "SYMBOL_JOKER_UINT64"),
            new SymbolTerminal(0x38, "SYMBOL_VALUE_UINT128"),
            new SymbolTerminal(0x3E, "SYMBOL_JOKER_UINT128") };
        private static LexerDFAState[] staticStates = { 
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x1 },
                new ushort[3] { 0x2E, 0x2E, 0x54 },
                new ushort[3] { 0x70, 0x70, 0x55 },
                new ushort[3] { 0x69, 0x69, 0x56 },
                new ushort[3] { 0x6E, 0x6E, 0x57 },
                new ushort[3] { 0x7B, 0x7B, 0x5F },
                new ushort[3] { 0x7D, 0x7D, 0x60 },
                new ushort[3] { 0x22, 0x22, 0x3 },
                new ushort[3] { 0x27, 0x27, 0x4 },
                new ushort[3] { 0x5B, 0x5B, 0x61 },
                new ushort[3] { 0x5C, 0x5C, 0x5 },
                new ushort[3] { 0x30, 0x30, 0x62 },
                new ushort[3] { 0x3D, 0x3D, 0x64 },
                new ushort[3] { 0x3B, 0x3B, 0x65 },
                new ushort[3] { 0x28, 0x28, 0x66 },
                new ushort[3] { 0x29, 0x29, 0x67 },
                new ushort[3] { 0x2A, 0x2A, 0x68 },
                new ushort[3] { 0x2B, 0x2B, 0x69 },
                new ushort[3] { 0x3F, 0x3F, 0x6A },
                new ushort[3] { 0x2C, 0x2C, 0x6B },
                new ushort[3] { 0x2D, 0x2D, 0x6C },
                new ushort[3] { 0x7C, 0x7C, 0x6D },
                new ushort[3] { 0x3C, 0x3C, 0x6E },
                new ushort[3] { 0x3E, 0x3E, 0x6F },
                new ushort[3] { 0x6F, 0x6F, 0x58 },
                new ushort[3] { 0x74, 0x74, 0x59 },
                new ushort[3] { 0x3A, 0x3A, 0x70 },
                new ushort[3] { 0x67, 0x67, 0x5A },
                new ushort[3] { 0x63, 0x63, 0x5B },
                new ushort[3] { 0x72, 0x72, 0x5C },
                new ushort[3] { 0x5E, 0x5E, 0x71 },
                new ushort[3] { 0x21, 0x21, 0x72 },
                new ushort[3] { 0x62, 0x62, 0x5D },
                new ushort[3] { 0x5D, 0x5D, 0x73 },
                new ushort[3] { 0xA, 0xA, 0x74 },
                new ushort[3] { 0x2028, 0x2029, 0x74 },
                new ushort[3] { 0x9, 0x9, 0x76 },
                new ushort[3] { 0xB, 0xC, 0x76 },
                new ushort[3] { 0x20, 0x20, 0x76 },
                new ushort[3] { 0x41, 0x5A, 0x5E },
                new ushort[3] { 0x5F, 0x5F, 0x5E },
                new ushort[3] { 0x61, 0x61, 0x5E },
                new ushort[3] { 0x64, 0x66, 0x5E },
                new ushort[3] { 0x68, 0x68, 0x5E },
                new ushort[3] { 0x6A, 0x6D, 0x5E },
                new ushort[3] { 0x71, 0x71, 0x5E },
                new ushort[3] { 0x73, 0x73, 0x5E },
                new ushort[3] { 0x75, 0x7A, 0x5E },
                new ushort[3] { 0x370, 0x3FF, 0x5E },
                new ushort[3] { 0x31, 0x39, 0x63 },
                new ushort[3] { 0x40, 0x40, 0x6 },
                new ushort[3] { 0xD, 0xD, 0x75 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x7 },
                new ushort[3] { 0x2A, 0x2A, 0x8 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x9 },
                new ushort[3] { 0x2F, 0x2F, 0x18 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x0, 0x21, 0x3 },
                new ushort[3] { 0x23, 0xFFFF, 0x3 },
                new ushort[3] { 0x22, 0x22, 0xAB }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x5C, 0x5C, 0xA },
                new ushort[3] { 0x0, 0x26, 0xB },
                new ushort[3] { 0x28, 0x5B, 0xB },
                new ushort[3] { 0x5D, 0xFFFF, 0xB }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0xE },
                new ushort[3] { 0x30, 0x30, 0xAC },
                new ushort[3] { 0x5C, 0x5C, 0xAC },
                new ushort[3] { 0x61, 0x62, 0xAC },
                new ushort[3] { 0x66, 0x66, 0xAC },
                new ushort[3] { 0x6E, 0x6E, 0xAC },
                new ushort[3] { 0x72, 0x72, 0xAC },
                new ushort[3] { 0x74, 0x74, 0xAC },
                new ushort[3] { 0x76, 0x76, 0xAC }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x82 },
                new ushort[3] { 0x5F, 0x5F, 0x82 },
                new ushort[3] { 0x61, 0x7A, 0x82 },
                new ushort[3] { 0x370, 0x3FF, 0x82 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x0, 0x9, 0x7 },
                new ushort[3] { 0xB, 0xC, 0x7 },
                new ushort[3] { 0xE, 0x2027, 0x7 },
                new ushort[3] { 0x202A, 0xFFFF, 0x7 },
                new ushort[3] { 0xA, 0xA, 0xB3 },
                new ushort[3] { 0x2028, 0x2029, 0xB3 },
                new ushort[3] { 0xD, 0xD, 0xB4 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0xFFFF, 0x12 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0xFFFF, 0x13 }},
                null),
            new LexerDFAState(new ushort[][] {
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
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x27, 0x27, 0xB5 },
                new ushort[3] { 0x5C, 0x5C, 0xA },
                new ushort[3] { 0x0, 0x26, 0xB },
                new ushort[3] { 0x28, 0x5B, 0xB },
                new ushort[3] { 0x5D, 0xFFFF, 0xB }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x30, 0xD },
                new ushort[3] { 0x5B, 0x5D, 0xD },
                new ushort[3] { 0x61, 0x62, 0xD },
                new ushort[3] { 0x66, 0x66, 0xD },
                new ushort[3] { 0x6E, 0x6E, 0xD },
                new ushort[3] { 0x72, 0x72, 0xD },
                new ushort[3] { 0x74, 0x74, 0xD },
                new ushort[3] { 0x76, 0x76, 0xD }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x5D, 0x5D, 0xB6 },
                new ushort[3] { 0x5C, 0x5C, 0xC },
                new ushort[3] { 0x0, 0x5A, 0xD },
                new ushort[3] { 0x5E, 0xFFFF, 0xD }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x62, 0x62, 0x14 },
                new ushort[3] { 0x63, 0x63, 0x15 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x16 },
                new ushort[3] { 0x41, 0x46, 0x16 },
                new ushort[3] { 0x61, 0x66, 0x16 },
                new ushort[3] { 0x58, 0x58, 0x17 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x31, 0xB7 },
                new ushort[3] { 0x42, 0x42, 0xB8 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0x2E, 0x12 },
                new ushort[3] { 0x30, 0xFFFF, 0x12 },
                new ushort[3] { 0x2F, 0x2F, 0xBA }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x11 },
                new ushort[3] { 0x0, 0x29, 0x12 },
                new ushort[3] { 0x2B, 0xFFFF, 0x12 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0xFFFF, 0x13 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x7B, 0x7B, 0x19 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x7B, 0x7B, 0x1A }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xBB },
                new ushort[3] { 0x41, 0x46, 0xBB },
                new ushort[3] { 0x61, 0x66, 0xBB }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xBC }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x0, 0x9, 0x18 },
                new ushort[3] { 0xB, 0xC, 0x18 },
                new ushort[3] { 0xE, 0x2027, 0x18 },
                new ushort[3] { 0x202A, 0xFFFF, 0x18 },
                new ushort[3] { 0xA, 0xA, 0xB1 },
                new ushort[3] { 0xD, 0xD, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x40, 0x40, 0x1C },
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x40, 0x40, 0x1D },
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2A, 0x2A, 0x1B },
                new ushort[3] { 0x0, 0x29, 0x13 },
                new ushort[3] { 0x2B, 0x2E, 0x13 },
                new ushort[3] { 0x30, 0xFFFF, 0x13 },
                new ushort[3] { 0x2F, 0x2F, 0xB1 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x1E },
                new ushort[3] { 0x41, 0x5A, 0x1E },
                new ushort[3] { 0x5F, 0x5F, 0x1E },
                new ushort[3] { 0x61, 0x7A, 0x1E },
                new ushort[3] { 0x370, 0x3FF, 0x1E },
                new ushort[3] { 0x7D, 0x7D, 0xBF }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x1F },
                new ushort[3] { 0x41, 0x5A, 0x1F },
                new ushort[3] { 0x5F, 0x5F, 0x1F },
                new ushort[3] { 0x61, 0x7A, 0x1F },
                new ushort[3] { 0x370, 0x3FF, 0x1F },
                new ushort[3] { 0x7D, 0x7D, 0xC0 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xC1 },
                new ushort[3] { 0x41, 0x46, 0xC1 },
                new ushort[3] { 0x61, 0x66, 0xC1 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xC2 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x35 },
                new ushort[3] { 0x41, 0x46, 0x35 },
                new ushort[3] { 0x61, 0x66, 0x35 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2D },
                new ushort[3] { 0x41, 0x46, 0x2D },
                new ushort[3] { 0x61, 0x66, 0x2D }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x25 },
                new ushort[3] { 0x41, 0x46, 0x25 },
                new ushort[3] { 0x61, 0x66, 0x25 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x26 },
                new ushort[3] { 0x41, 0x46, 0x26 },
                new ushort[3] { 0x61, 0x66, 0x26 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x27 },
                new ushort[3] { 0x41, 0x46, 0x27 },
                new ushort[3] { 0x61, 0x66, 0x27 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x28 },
                new ushort[3] { 0x41, 0x46, 0x28 },
                new ushort[3] { 0x61, 0x66, 0x28 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x29 },
                new ushort[3] { 0x41, 0x46, 0x29 },
                new ushort[3] { 0x61, 0x66, 0x29 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2A },
                new ushort[3] { 0x41, 0x46, 0x2A },
                new ushort[3] { 0x61, 0x66, 0x2A }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2B },
                new ushort[3] { 0x41, 0x46, 0x2B },
                new ushort[3] { 0x61, 0x66, 0x2B }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2C },
                new ushort[3] { 0x41, 0x46, 0x2C },
                new ushort[3] { 0x61, 0x66, 0x2C }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2E },
                new ushort[3] { 0x41, 0x46, 0x2E },
                new ushort[3] { 0x61, 0x66, 0x2E }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x2F },
                new ushort[3] { 0x41, 0x46, 0x2F },
                new ushort[3] { 0x61, 0x66, 0x2F }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x30 },
                new ushort[3] { 0x41, 0x46, 0x30 },
                new ushort[3] { 0x61, 0x66, 0x30 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x31 },
                new ushort[3] { 0x41, 0x46, 0x31 },
                new ushort[3] { 0x61, 0x66, 0x31 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x32 },
                new ushort[3] { 0x41, 0x46, 0x32 },
                new ushort[3] { 0x61, 0x66, 0x32 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x33 },
                new ushort[3] { 0x41, 0x46, 0x33 },
                new ushort[3] { 0x61, 0x66, 0x33 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x34 },
                new ushort[3] { 0x41, 0x46, 0x34 },
                new ushort[3] { 0x61, 0x66, 0x34 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x36 },
                new ushort[3] { 0x41, 0x46, 0x36 },
                new ushort[3] { 0x61, 0x66, 0x36 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x37 },
                new ushort[3] { 0x41, 0x46, 0x37 },
                new ushort[3] { 0x61, 0x66, 0x37 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x4E },
                new ushort[3] { 0x41, 0x46, 0x4E },
                new ushort[3] { 0x61, 0x66, 0x4E }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x50 },
                new ushort[3] { 0x41, 0x46, 0x50 },
                new ushort[3] { 0x61, 0x66, 0x50 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x52 },
                new ushort[3] { 0x41, 0x46, 0x52 },
                new ushort[3] { 0x61, 0x66, 0x52 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4B }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x43 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3B }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3C }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3D }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3E }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3F }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x40 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x41 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x42 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x44 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x45 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x46 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x47 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x48 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x49 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4A }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4C }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4D }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x4F }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x51 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x53 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xCA },
                new ushort[3] { 0x41, 0x46, 0xCA },
                new ushort[3] { 0x61, 0x66, 0xCA }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xCB }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xCC },
                new ushort[3] { 0x41, 0x46, 0xCC },
                new ushort[3] { 0x61, 0x66, 0xCC }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xCD }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xCE },
                new ushort[3] { 0x41, 0x46, 0xCE },
                new ushort[3] { 0x61, 0x66, 0xCE }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0xCF }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2E, 0x2E, 0x77 }},
                terminals[0x2]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0x78 },
                new ushort[3] { 0x72, 0x72, 0x79 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x71, 0x7A },
                new ushort[3] { 0x73, 0x74, 0x7A },
                new ushort[3] { 0x76, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0x7B },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6D, 0x7A },
                new ushort[3] { 0x6F, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x7C },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x7D },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6F, 0x7A },
                new ushort[3] { 0x71, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x7E },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x7F },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x71, 0x7A },
                new ushort[3] { 0x73, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x66, 0x66, 0xAF },
                new ushort[3] { 0x73, 0x73, 0xB0 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x65, 0x7A },
                new ushort[3] { 0x67, 0x72, 0x7A },
                new ushort[3] { 0x74, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x75, 0x75, 0x80 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x74, 0x7A },
                new ushort[3] { 0x76, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x83 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x68, 0x7A },
                new ushort[3] { 0x6A, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {}, terminals[0x4]),
            new LexerDFAState(new ushort[][] {}, terminals[0x5]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x5C, 0x5C, 0xC },
                new ushort[3] { 0x0, 0x5A, 0xD },
                new ushort[3] { 0x5E, 0xFFFF, 0xD }},
                terminals[0x6]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x78, 0x78, 0xF },
                new ushort[3] { 0x62, 0x62, 0x10 }},
                terminals[0x7]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xB2 }},
                terminals[0x7]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x3E, 0x3E, 0xAD }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {}, terminals[0x9]),
            new LexerDFAState(new ushort[][] {}, terminals[0xA]),
            new LexerDFAState(new ushort[][] {}, terminals[0xB]),
            new LexerDFAState(new ushort[][] {}, terminals[0xC]),
            new LexerDFAState(new ushort[][] {}, terminals[0xD]),
            new LexerDFAState(new ushort[][] {}, terminals[0xE]),
            new LexerDFAState(new ushort[][] {}, terminals[0xF]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x3E, 0x3E, 0xAE }},
                terminals[0x10]),
            new LexerDFAState(new ushort[][] {}, terminals[0x11]),
            new LexerDFAState(new ushort[][] {}, terminals[0x12]),
            new LexerDFAState(new ushort[][] {}, terminals[0x13]),
            new LexerDFAState(new ushort[][] {}, terminals[0x14]),
            new LexerDFAState(new ushort[][] {}, terminals[0x15]),
            new LexerDFAState(new ushort[][] {}, terminals[0x16]),
            new LexerDFAState(new ushort[][] {}, terminals[0x17]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0xA, 0xA, 0x74 },
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0x9, 0xB1 },
                new ushort[3] { 0xB, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {}, terminals[0x19]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x62, 0x62, 0x81 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x61, 0x7A },
                new ushort[3] { 0x63, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x84 },
                new ushort[3] { 0x6F, 0x6F, 0x85 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x68, 0x7A },
                new ushort[3] { 0x6A, 0x6E, 0x7A },
                new ushort[3] { 0x70, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x86 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x73, 0x7A },
                new ushort[3] { 0x75, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x87 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6C, 0x7A },
                new ushort[3] { 0x6E, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x88 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x73, 0x7A },
                new ushort[3] { 0x75, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x89 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x71, 0x7A },
                new ushort[3] { 0x73, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x8A },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0x8B },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6B, 0x7A },
                new ushort[3] { 0x6D, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0x8C },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6B, 0x7A },
                new ushort[3] { 0x6D, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0xB9 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6D, 0x7A },
                new ushort[3] { 0x6F, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x76, 0x76, 0x8D },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x75, 0x7A },
                new ushort[3] { 0x77, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0x8E },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x73, 0x7A },
                new ushort[3] { 0x75, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x8F },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x90 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x91 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x68, 0x7A },
                new ushort[3] { 0x6A, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x92 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6C, 0x7A },
                new ushort[3] { 0x6E, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x93 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6C, 0x7A },
                new ushort[3] { 0x6E, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x94 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x95 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x68, 0x7A },
                new ushort[3] { 0x6A, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0x96 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x97 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0x98 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x71, 0x7A },
                new ushort[3] { 0x73, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0x9A },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x72, 0x7A },
                new ushort[3] { 0x74, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6F, 0x6F, 0x9B },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6E, 0x7A },
                new ushort[3] { 0x70, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x69, 0x69, 0x99 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x68, 0x7A },
                new ushort[3] { 0x6A, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6D, 0x6D, 0x9C },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6C, 0x7A },
                new ushort[3] { 0x6E, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xBD },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x72, 0x7A },
                new ushort[3] { 0x74, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0xBE },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x62, 0x7A },
                new ushort[3] { 0x64, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0xA4 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x73, 0x7A },
                new ushort[3] { 0x75, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0x9D },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x62, 0x7A },
                new ushort[3] { 0x64, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0x9E },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6D, 0x7A },
                new ushort[3] { 0x6F, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0xA0 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6D, 0x7A },
                new ushort[3] { 0x6F, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x9F },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6F, 0x7A },
                new ushort[3] { 0x71, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6E, 0x6E, 0xA5 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6D, 0x7A },
                new ushort[3] { 0x6F, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA6 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x74, 0x74, 0xA1 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x73, 0x7A },
                new ushort[3] { 0x75, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA7 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA2 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x61, 0x61, 0xA3 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x62, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xA8 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x63, 0x63, 0xA9 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x62, 0x7A },
                new ushort[3] { 0x64, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0xAA },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6B, 0x7A },
                new ushort[3] { 0x6D, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xC3 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xC4 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x72, 0x7A },
                new ushort[3] { 0x74, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x72, 0x72, 0xC5 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x71, 0x7A },
                new ushort[3] { 0x73, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6C, 0x6C, 0xC6 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x6B, 0x7A },
                new ushort[3] { 0x6D, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x64, 0x64, 0xC7 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x63, 0x7A },
                new ushort[3] { 0x65, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0xC8 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x64, 0x7A },
                new ushort[3] { 0x66, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x73, 0x73, 0xC9 },
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x72, 0x7A },
                new ushort[3] { 0x74, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x3]),
            new LexerDFAState(new ushort[][] {}, terminals[0x1A]),
            new LexerDFAState(new ushort[][] {}, terminals[0x1B]),
            new LexerDFAState(new ushort[][] {}, terminals[0x1C]),
            new LexerDFAState(new ushort[][] {}, terminals[0x1D]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x1E]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x1F]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xB2 }},
                terminals[0x7]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0xA, 0xA, 0xB3 },
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0x9, 0xB1 },
                new ushort[3] { 0xB, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {}, terminals[0x20]),
            new LexerDFAState(new ushort[][] {}, terminals[0x21]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x31, 0xB7 }},
                terminals[0x22]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x42, 0x42, 0xB8 }},
                terminals[0x23]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x24]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2F, 0x2F, 0x2 },
                new ushort[3] { 0x9, 0xD, 0xB1 },
                new ushort[3] { 0x20, 0x20, 0xB1 },
                new ushort[3] { 0x2028, 0x2029, 0xB1 }},
                terminals[0x18]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x20 },
                new ushort[3] { 0x41, 0x46, 0x20 },
                new ushort[3] { 0x61, 0x66, 0x20 }},
                terminals[0x25]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x21 }},
                terminals[0x26]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x27]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x28]),
            new LexerDFAState(new ushort[][] {}, terminals[0x29]),
            new LexerDFAState(new ushort[][] {}, terminals[0x2A]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x22 },
                new ushort[3] { 0x41, 0x46, 0x22 },
                new ushort[3] { 0x61, 0x66, 0x22 }},
                terminals[0x2B]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x38 }},
                terminals[0x2C]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x2D]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x2E]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x2F]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x30]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x31]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x32]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x7A },
                new ushort[3] { 0x41, 0x5A, 0x7A },
                new ushort[3] { 0x5F, 0x5F, 0x7A },
                new ushort[3] { 0x61, 0x7A, 0x7A },
                new ushort[3] { 0x370, 0x3FF, 0x7A }},
                terminals[0x33]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x23 },
                new ushort[3] { 0x41, 0x46, 0x23 },
                new ushort[3] { 0x61, 0x66, 0x23 }},
                terminals[0x34]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x39 }},
                terminals[0x35]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x24 },
                new ushort[3] { 0x41, 0x46, 0x24 },
                new ushort[3] { 0x61, 0x66, 0x24 }},
                terminals[0x36]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x58, 0x58, 0x3A }},
                terminals[0x37]),
            new LexerDFAState(new ushort[][] {}, terminals[0x38]),
            new LexerDFAState(new ushort[][] {}, terminals[0x39]) };
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
    public class FileCentralDogma_Parser : LR1TextParser
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
            new SymbolVariable(0x58, "option"), 
            new SymbolVariable(0x59, "terminal_def_atom_any"), 
            new SymbolVariable(0x5A, "terminal_def_atom_unicode"), 
            new SymbolVariable(0x5B, "terminal_def_atom_text"), 
            new SymbolVariable(0x5C, "terminal_def_atom_set"), 
            new SymbolVariable(0x5D, "terminal_def_atom_ublock"), 
            new SymbolVariable(0x5E, "terminal_def_atom_ucat"), 
            new SymbolVariable(0x5F, "terminal_def_atom_span"), 
            new SymbolVariable(0x60, "terminal_def_atom"), 
            new SymbolVariable(0x61, "terminal_def_element"), 
            new SymbolVariable(0x62, "terminal_def_cardinalilty"), 
            new SymbolVariable(0x63, "terminal_def_repetition"), 
            new SymbolVariable(0x64, "terminal_def_fragment"), 
            new SymbolVariable(0x65, "terminal_def_restrict"), 
            new SymbolVariable(0x66, "terminal_definition"), 
            new SymbolVariable(0x67, "terminal_subgrammar"), 
            new SymbolVariable(0x68, "terminal"), 
            new SymbolVariable(0x69, "rule_sym_action"), 
            new SymbolVariable(0x6A, "rule_sym_virtual"), 
            new SymbolVariable(0x6B, "rule_sym_ref_simple"), 
            new SymbolVariable(0x6C, "rule_template_params"), 
            new SymbolVariable(0x6D, "grammar_bin_terminal"), 
            new SymbolVariable(0x6E, "grammar_text_terminal"), 
            new SymbolVariable(0x6F, "grammar_options"), 
            new SymbolVariable(0x70, "grammar_terminals"), 
            new SymbolVariable(0x71, "grammar_parency"), 
            new SymbolVariable(0x72, "grammar_access"), 
            new SymbolVariable(0x73, "cf_grammar_text"), 
            new SymbolVariable(0x74, "cf_grammar_bin"), 
            new SymbolVariable(0x75, "_m86"), 
            new SymbolVariable(0x76, "_m88"), 
            new SymbolVariable(0x77, "_m90"), 
            new SymbolVariable(0x78, "_m94"), 
            new SymbolVariable(0x79, "_m97"), 
            new SymbolVariable(0x7A, "_m99"), 
            new SymbolVariable(0x7B, "_m101"), 
            new SymbolVariable(0x7C, "grammar_cf_rules<grammar_text_terminal>"), 
            new SymbolVariable(0x7D, "cf_rule_simple<grammar_text_terminal>"), 
            new SymbolVariable(0x7E, "rule_definition<grammar_text_terminal>"), 
            new SymbolVariable(0x7F, "rule_def_choice<grammar_text_terminal>"), 
            new SymbolVariable(0x80, "rule_def_restrict<grammar_text_terminal>"), 
            new SymbolVariable(0x81, "rule_def_fragment<grammar_text_terminal>"), 
            new SymbolVariable(0x82, "rule_def_repetition<grammar_text_terminal>"), 
            new SymbolVariable(0x83, "rule_def_tree_action<grammar_text_terminal>"), 
            new SymbolVariable(0x84, "rule_def_element<grammar_text_terminal>"), 
            new SymbolVariable(0x85, "rule_def_atom<grammar_text_terminal>"), 
            new SymbolVariable(0x86, "rule_sym_ref_template<grammar_text_terminal>"), 
            new SymbolVariable(0x87, "rule_sym_ref_params<grammar_text_terminal>"), 
            new SymbolVariable(0x88, "_m117"), 
            new SymbolVariable(0x89, "_m120"), 
            new SymbolVariable(0x8A, "_m121"), 
            new SymbolVariable(0x8B, "_m122"), 
            new SymbolVariable(0x8C, "cf_rule_template<grammar_text_terminal>"), 
            new SymbolVariable(0x8D, "_m124"), 
            new SymbolVariable(0x8E, "grammar_cf_rules<grammar_bin_terminal>"), 
            new SymbolVariable(0x8F, "cf_rule_simple<grammar_bin_terminal>"), 
            new SymbolVariable(0x90, "rule_definition<grammar_bin_terminal>"), 
            new SymbolVariable(0x91, "rule_def_choice<grammar_bin_terminal>"), 
            new SymbolVariable(0x92, "rule_def_restrict<grammar_bin_terminal>"), 
            new SymbolVariable(0x93, "rule_def_fragment<grammar_bin_terminal>"), 
            new SymbolVariable(0x94, "rule_def_repetition<grammar_bin_terminal>"), 
            new SymbolVariable(0x95, "rule_def_tree_action<grammar_bin_terminal>"), 
            new SymbolVariable(0x96, "rule_def_element<grammar_bin_terminal>"), 
            new SymbolVariable(0x97, "rule_def_atom<grammar_bin_terminal>"), 
            new SymbolVariable(0x98, "rule_sym_ref_template<grammar_bin_terminal>"), 
            new SymbolVariable(0x99, "rule_sym_ref_params<grammar_bin_terminal>"), 
            new SymbolVariable(0x9A, "_m138"), 
            new SymbolVariable(0x9B, "_m139"), 
            new SymbolVariable(0x9C, "_m140"), 
            new SymbolVariable(0x9D, "_m141"), 
            new SymbolVariable(0x9E, "cf_rule_template<grammar_bin_terminal>"), 
            new SymbolVariable(0x9F, "_m143"), 
            new SymbolVariable(0xDE, "cs_grammar_text"), 
            new SymbolVariable(0xDF, "cs_grammar_bin"), 
            new SymbolVariable(0xE0, "grammar_cs_rules<grammar_text_terminal>"), 
            new SymbolVariable(0xE1, "cs_rule_simple<grammar_text_terminal>"), 
            new SymbolVariable(0xE2, "cs_rule_context<grammar_text_terminal>"), 
            new SymbolVariable(0xE3, "cs_rule_template<grammar_text_terminal>"), 
            new SymbolVariable(0xE4, "_m153"), 
            new SymbolVariable(0xE5, "grammar_cs_rules<grammar_bin_terminal>"), 
            new SymbolVariable(0xE6, "cs_rule_simple<grammar_bin_terminal>"), 
            new SymbolVariable(0xE7, "cs_rule_context<grammar_bin_terminal>"), 
            new SymbolVariable(0xE8, "cs_rule_template<grammar_bin_terminal>"), 
            new SymbolVariable(0xE9, "_m158"), 
            new SymbolVariable(0xEA, "file_item"), 
            new SymbolVariable(0xEB, "file"), 
            new SymbolVariable(0xEC, "_m236"), 
            new SymbolVariable(0xED, "_Axiom_") };
        private static SyntaxTreeNode Production_13_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_13_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_14_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_15_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_16_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_17_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_18_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_18_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_18_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_18_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_18_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_19_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[6]);
            return root;
        }
        private static SyntaxTreeNode Production_19_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_1A_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_1B_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[8], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1B_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[8], SyntaxTreeNodeAction.Replace);
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
            return root;
        }
        private static SyntaxTreeNode Production_1C_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[9], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_1C_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_58_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_59_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5A_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5A_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5B_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5C_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5D_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5E_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_5F_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_5 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_6 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_60_7 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_61_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_61_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_62_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_62_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_62_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_62_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_62_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_63_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_63_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_64_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[22]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_64_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_65_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[23]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_65_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_66_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[24]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_66_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_67_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[25]);
            return root;
        }
        private static SyntaxTreeNode Production_67_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_68_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_69_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6A_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6B_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6C_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6C_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_5 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_6 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_7 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_8 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_9 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_A (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6D_B (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6E_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_6F_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[33]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6F_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_70_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_70_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_71_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[35]);
            return root;
        }
        private static SyntaxTreeNode Production_71_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[35]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_71_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_72_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_72_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_72_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_72_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_73_0 (LRParser baseParser)
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_73_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_74_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_75_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[39], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_75_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[39], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_76_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_76_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[40], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_77_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_77_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[41], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_78_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[42], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_78_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[42], SyntaxTreeNodeAction.Replace);
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
            return root;
        }
        private static SyntaxTreeNode Production_79_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[43], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_79_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7A_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[44], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7A_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7B_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[45], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7B_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[45], SyntaxTreeNodeAction.Replace);
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
            return root;
        }
        private static SyntaxTreeNode Production_7C_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7C_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7D_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7E_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[48]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_7E_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7F_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_7F_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[49]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("emptypart"), SyntaxTreeNodeAction.Promote));
            return root;
        }
        private static SyntaxTreeNode Production_80_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[50]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_80_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_81_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[51]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_81_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_82_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_82_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_82_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_82_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_83_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_83_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_83_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_84_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_84_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_85_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_85_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_85_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_85_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_85_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_86_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_87_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_87_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_88_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[58], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_88_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[58], SyntaxTreeNodeAction.Replace);
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
            return root;
        }
        private static SyntaxTreeNode Production_89_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[59], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_89_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[59], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8A_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_8A_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[60], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_8B_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_8B_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[61], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_8C_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_8D_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[63], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8D_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[63], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8D_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_8D_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_8E_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_8E_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_8F_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_90_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[66]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_90_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_91_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_91_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[67]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("emptypart"), SyntaxTreeNodeAction.Promote));
            return root;
        }
        private static SyntaxTreeNode Production_92_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[68]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_92_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_93_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[69]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_93_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_94_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_94_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_94_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_94_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_95_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_95_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_95_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_96_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_96_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_97_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_97_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_97_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_97_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_97_4 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_98_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_99_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_99_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_9A_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[76], SyntaxTreeNodeAction.Replace);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9A_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[76], SyntaxTreeNodeAction.Replace);
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
            return root;
        }
        private static SyntaxTreeNode Production_9B_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[77], SyntaxTreeNodeAction.Replace);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9B_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[77], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("concat"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9C_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_9C_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[78], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_9D_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            return root;
        }
        private static SyntaxTreeNode Production_9D_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[79], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_9E_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_9F_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[81], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9F_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[81], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9F_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_9F_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_DE_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_DF_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E0_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E0_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E1_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E2_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[86]);
            return root;
        }
        private static SyntaxTreeNode Production_E2_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E3_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E4_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[88], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E4_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[88], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E4_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E4_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E5_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
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
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E5_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E6_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E7_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[91]);
            return root;
        }
        private static SyntaxTreeNode Production_E7_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E8_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E9_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[93], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E9_1 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[93], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_E9_2 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_E9_3 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_EA_0 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_EB_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[95]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_EC_0 (LRParser baseParser)
        {
            FileCentralDogma_Parser parser = baseParser as FileCentralDogma_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[96], SyntaxTreeNodeAction.Replace);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_EC_1 (LRParser baseParser)
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
        private static SyntaxTreeNode Production_ED_0 (LRParser baseParser)
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
        private static LRRule[] staticRules = {
           new LRRule(Production_13_0, variables[0], 1)
           , new LRRule(Production_13_1, variables[0], 2)
           , new LRRule(Production_14_0, variables[1], 1)
           , new LRRule(Production_15_0, variables[2], 1)
           , new LRRule(Production_16_0, variables[3], 1)
           , new LRRule(Production_17_0, variables[4], 1)
           , new LRRule(Production_18_0, variables[5], 1)
           , new LRRule(Production_18_1, variables[5], 1)
           , new LRRule(Production_18_2, variables[5], 1)
           , new LRRule(Production_18_3, variables[5], 1)
           , new LRRule(Production_18_4, variables[5], 1)
           , new LRRule(Production_19_0, variables[6], 0)
           , new LRRule(Production_19_1, variables[6], 1)
           , new LRRule(Production_1A_0, variables[7], 5)
           , new LRRule(Production_1B_0, variables[8], 2)
           , new LRRule(Production_1B_1, variables[8], 3)
           , new LRRule(Production_1C_0, variables[9], 1)
           , new LRRule(Production_1C_1, variables[9], 2)
           , new LRRule(Production_58_0, variables[10], 4)
           , new LRRule(Production_59_0, variables[11], 1)
           , new LRRule(Production_5A_0, variables[12], 1)
           , new LRRule(Production_5A_1, variables[12], 1)
           , new LRRule(Production_5B_0, variables[13], 1)
           , new LRRule(Production_5C_0, variables[14], 1)
           , new LRRule(Production_5D_0, variables[15], 1)
           , new LRRule(Production_5E_0, variables[16], 1)
           , new LRRule(Production_5F_0, variables[17], 3)
           , new LRRule(Production_60_0, variables[18], 1)
           , new LRRule(Production_60_1, variables[18], 1)
           , new LRRule(Production_60_2, variables[18], 1)
           , new LRRule(Production_60_3, variables[18], 1)
           , new LRRule(Production_60_4, variables[18], 1)
           , new LRRule(Production_60_5, variables[18], 1)
           , new LRRule(Production_60_6, variables[18], 1)
           , new LRRule(Production_60_7, variables[18], 1)
           , new LRRule(Production_61_0, variables[19], 1)
           , new LRRule(Production_61_1, variables[19], 3)
           , new LRRule(Production_62_0, variables[20], 1)
           , new LRRule(Production_62_1, variables[20], 1)
           , new LRRule(Production_62_2, variables[20], 1)
           , new LRRule(Production_62_3, variables[20], 3)
           , new LRRule(Production_62_4, variables[20], 5)
           , new LRRule(Production_63_0, variables[21], 1)
           , new LRRule(Production_63_1, variables[21], 2)
           , new LRRule(Production_64_0, variables[22], 1)
           , new LRRule(Production_64_1, variables[22], 2)
           , new LRRule(Production_65_0, variables[23], 1)
           , new LRRule(Production_65_1, variables[23], 2)
           , new LRRule(Production_66_0, variables[24], 1)
           , new LRRule(Production_66_1, variables[24], 2)
           , new LRRule(Production_67_0, variables[25], 0)
           , new LRRule(Production_67_1, variables[25], 2)
           , new LRRule(Production_68_0, variables[26], 5)
           , new LRRule(Production_69_0, variables[27], 3)
           , new LRRule(Production_6A_0, variables[28], 1)
           , new LRRule(Production_6B_0, variables[29], 1)
           , new LRRule(Production_6C_0, variables[30], 3)
           , new LRRule(Production_6C_1, variables[30], 4)
           , new LRRule(Production_6D_0, variables[31], 1)
           , new LRRule(Production_6D_1, variables[31], 1)
           , new LRRule(Production_6D_2, variables[31], 1)
           , new LRRule(Production_6D_3, variables[31], 1)
           , new LRRule(Production_6D_4, variables[31], 1)
           , new LRRule(Production_6D_5, variables[31], 1)
           , new LRRule(Production_6D_6, variables[31], 1)
           , new LRRule(Production_6D_7, variables[31], 1)
           , new LRRule(Production_6D_8, variables[31], 1)
           , new LRRule(Production_6D_9, variables[31], 1)
           , new LRRule(Production_6D_A, variables[31], 1)
           , new LRRule(Production_6D_B, variables[31], 1)
           , new LRRule(Production_6E_0, variables[32], 1)
           , new LRRule(Production_6F_0, variables[33], 3)
           , new LRRule(Production_6F_1, variables[33], 4)
           , new LRRule(Production_70_0, variables[34], 3)
           , new LRRule(Production_70_1, variables[34], 4)
           , new LRRule(Production_71_0, variables[35], 0)
           , new LRRule(Production_71_1, variables[35], 2)
           , new LRRule(Production_71_2, variables[35], 3)
           , new LRRule(Production_72_0, variables[36], 1)
           , new LRRule(Production_72_1, variables[36], 1)
           , new LRRule(Production_72_2, variables[36], 1)
           , new LRRule(Production_72_3, variables[36], 1)
           , new LRRule(Production_73_0, variables[37], 9)
           , new LRRule(Production_73_1, variables[37], 10)
           , new LRRule(Production_74_0, variables[38], 10)
           , new LRRule(Production_75_0, variables[39], 1)
           , new LRRule(Production_75_1, variables[39], 2)
           , new LRRule(Production_76_0, variables[40], 2)
           , new LRRule(Production_76_1, variables[40], 3)
           , new LRRule(Production_77_0, variables[41], 2)
           , new LRRule(Production_77_1, variables[41], 3)
           , new LRRule(Production_78_0, variables[42], 2)
           , new LRRule(Production_78_1, variables[42], 3)
           , new LRRule(Production_79_0, variables[43], 1)
           , new LRRule(Production_79_1, variables[43], 2)
           , new LRRule(Production_7A_0, variables[44], 1)
           , new LRRule(Production_7A_1, variables[44], 2)
           , new LRRule(Production_7B_0, variables[45], 2)
           , new LRRule(Production_7B_1, variables[45], 3)
           , new LRRule(Production_7C_0, variables[46], 3)
           , new LRRule(Production_7C_1, variables[46], 4)
           , new LRRule(Production_7D_0, variables[47], 4)
           , new LRRule(Production_7E_0, variables[48], 1)
           , new LRRule(Production_7E_1, variables[48], 2)
           , new LRRule(Production_7F_0, variables[49], 1)
           , new LRRule(Production_7F_1, variables[49], 0)
           , new LRRule(Production_80_0, variables[50], 1)
           , new LRRule(Production_80_1, variables[50], 2)
           , new LRRule(Production_81_0, variables[51], 1)
           , new LRRule(Production_81_1, variables[51], 2)
           , new LRRule(Production_82_0, variables[52], 1)
           , new LRRule(Production_82_1, variables[52], 2)
           , new LRRule(Production_82_2, variables[52], 2)
           , new LRRule(Production_82_3, variables[52], 2)
           , new LRRule(Production_83_0, variables[53], 1)
           , new LRRule(Production_83_1, variables[53], 2)
           , new LRRule(Production_83_2, variables[53], 2)
           , new LRRule(Production_84_0, variables[54], 1)
           , new LRRule(Production_84_1, variables[54], 3)
           , new LRRule(Production_85_0, variables[55], 1)
           , new LRRule(Production_85_1, variables[55], 1)
           , new LRRule(Production_85_2, variables[55], 1)
           , new LRRule(Production_85_3, variables[55], 1)
           , new LRRule(Production_85_4, variables[55], 1)
           , new LRRule(Production_86_0, variables[56], 2)
           , new LRRule(Production_87_0, variables[57], 3)
           , new LRRule(Production_87_1, variables[57], 4)
           , new LRRule(Production_88_0, variables[58], 2)
           , new LRRule(Production_88_1, variables[58], 3)
           , new LRRule(Production_89_0, variables[59], 1)
           , new LRRule(Production_89_1, variables[59], 2)
           , new LRRule(Production_8A_0, variables[60], 2)
           , new LRRule(Production_8A_1, variables[60], 3)
           , new LRRule(Production_8B_0, variables[61], 2)
           , new LRRule(Production_8B_1, variables[61], 3)
           , new LRRule(Production_8C_0, variables[62], 5)
           , new LRRule(Production_8D_0, variables[63], 1)
           , new LRRule(Production_8D_1, variables[63], 1)
           , new LRRule(Production_8D_2, variables[63], 2)
           , new LRRule(Production_8D_3, variables[63], 2)
           , new LRRule(Production_8E_0, variables[64], 3)
           , new LRRule(Production_8E_1, variables[64], 4)
           , new LRRule(Production_8F_0, variables[65], 4)
           , new LRRule(Production_90_0, variables[66], 1)
           , new LRRule(Production_90_1, variables[66], 2)
           , new LRRule(Production_91_0, variables[67], 1)
           , new LRRule(Production_91_1, variables[67], 0)
           , new LRRule(Production_92_0, variables[68], 1)
           , new LRRule(Production_92_1, variables[68], 2)
           , new LRRule(Production_93_0, variables[69], 1)
           , new LRRule(Production_93_1, variables[69], 2)
           , new LRRule(Production_94_0, variables[70], 1)
           , new LRRule(Production_94_1, variables[70], 2)
           , new LRRule(Production_94_2, variables[70], 2)
           , new LRRule(Production_94_3, variables[70], 2)
           , new LRRule(Production_95_0, variables[71], 1)
           , new LRRule(Production_95_1, variables[71], 2)
           , new LRRule(Production_95_2, variables[71], 2)
           , new LRRule(Production_96_0, variables[72], 1)
           , new LRRule(Production_96_1, variables[72], 3)
           , new LRRule(Production_97_0, variables[73], 1)
           , new LRRule(Production_97_1, variables[73], 1)
           , new LRRule(Production_97_2, variables[73], 1)
           , new LRRule(Production_97_3, variables[73], 1)
           , new LRRule(Production_97_4, variables[73], 1)
           , new LRRule(Production_98_0, variables[74], 2)
           , new LRRule(Production_99_0, variables[75], 3)
           , new LRRule(Production_99_1, variables[75], 4)
           , new LRRule(Production_9A_0, variables[76], 2)
           , new LRRule(Production_9A_1, variables[76], 3)
           , new LRRule(Production_9B_0, variables[77], 1)
           , new LRRule(Production_9B_1, variables[77], 2)
           , new LRRule(Production_9C_0, variables[78], 2)
           , new LRRule(Production_9C_1, variables[78], 3)
           , new LRRule(Production_9D_0, variables[79], 2)
           , new LRRule(Production_9D_1, variables[79], 3)
           , new LRRule(Production_9E_0, variables[80], 5)
           , new LRRule(Production_9F_0, variables[81], 1)
           , new LRRule(Production_9F_1, variables[81], 1)
           , new LRRule(Production_9F_2, variables[81], 2)
           , new LRRule(Production_9F_3, variables[81], 2)
           , new LRRule(Production_DE_0, variables[82], 9)
           , new LRRule(Production_DF_0, variables[83], 8)
           , new LRRule(Production_E0_0, variables[84], 3)
           , new LRRule(Production_E0_1, variables[84], 4)
           , new LRRule(Production_E1_0, variables[85], 6)
           , new LRRule(Production_E2_0, variables[86], 0)
           , new LRRule(Production_E2_1, variables[86], 3)
           , new LRRule(Production_E3_0, variables[87], 7)
           , new LRRule(Production_E4_0, variables[88], 1)
           , new LRRule(Production_E4_1, variables[88], 1)
           , new LRRule(Production_E4_2, variables[88], 2)
           , new LRRule(Production_E4_3, variables[88], 2)
           , new LRRule(Production_E5_0, variables[89], 3)
           , new LRRule(Production_E5_1, variables[89], 4)
           , new LRRule(Production_E6_0, variables[90], 6)
           , new LRRule(Production_E7_0, variables[91], 0)
           , new LRRule(Production_E7_1, variables[91], 3)
           , new LRRule(Production_E8_0, variables[92], 7)
           , new LRRule(Production_E9_0, variables[93], 1)
           , new LRRule(Production_E9_1, variables[93], 1)
           , new LRRule(Production_E9_2, variables[93], 2)
           , new LRRule(Production_E9_3, variables[93], 2)
           , new LRRule(Production_EA_0, variables[94], 1)
           , new LRRule(Production_EB_0, variables[95], 1)
           , new LRRule(Production_EC_0, variables[96], 1)
           , new LRRule(Production_EC_1, variables[96], 2)
           , new LRRule(Production_ED_0, variables[97], 2)
        };
        private static LR1State[] staticStates = {
            new LR1State(
               null,
               new SymbolTerminal[6] {FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[47]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0xA, 0xC, 0x11, 0x12, 0x13, 0x14},
               new ushort[14] {0xeb, 0xec, 0xea, 0x18, 0x1a, 0x73, 0x74, 0xde, 0xdf, 0x72, 0x14, 0x15, 0x16, 0x17},
               new ushort[14] {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xB, 0xD, 0xE, 0xF, 0x10},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[1]},
               new ushort[1] {0x2},
               new ushort[1] {0x15},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[47]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0xA, 0xC, 0x11, 0x12, 0x13, 0x14},
               new ushort[12] {0xea, 0x18, 0x1a, 0x73, 0x74, 0xde, 0xdf, 0x72, 0x14, 0x15, 0x16, 0x17},
               new ushort[12] {0x16, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xB, 0xD, 0xE, 0xF, 0x10},
               new LRReduction[1] {new LRReduction(0x2, staticRules[0xCC])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[7] {new LRReduction(0x2, staticRules[0xCD]), new LRReduction(0xc, staticRules[0xCD]), new LRReduction(0xd, staticRules[0xCD]), new LRReduction(0xe, staticRules[0xCD]), new LRReduction(0xf, staticRules[0xCD]), new LRReduction(0x10, staticRules[0xCD]), new LRReduction(0x52, staticRules[0xCD])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[7] {new LRReduction(0x2, staticRules[0xCB]), new LRReduction(0xc, staticRules[0xCB]), new LRReduction(0xd, staticRules[0xCB]), new LRReduction(0xe, staticRules[0xCB]), new LRReduction(0xf, staticRules[0xCB]), new LRReduction(0x10, staticRules[0xCB]), new LRReduction(0x52, staticRules[0xCB])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x6]), new LRReduction(0xc, staticRules[0x6]), new LRReduction(0xd, staticRules[0x6]), new LRReduction(0xe, staticRules[0x6]), new LRReduction(0xf, staticRules[0x6]), new LRReduction(0x10, staticRules[0x6]), new LRReduction(0x12, staticRules[0x6]), new LRReduction(0x52, staticRules[0x6])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x7]), new LRReduction(0xc, staticRules[0x7]), new LRReduction(0xd, staticRules[0x7]), new LRReduction(0xe, staticRules[0x7]), new LRReduction(0xf, staticRules[0x7]), new LRReduction(0x10, staticRules[0x7]), new LRReduction(0x12, staticRules[0x7]), new LRReduction(0x52, staticRules[0x7])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x8]), new LRReduction(0xc, staticRules[0x8]), new LRReduction(0xd, staticRules[0x8]), new LRReduction(0xe, staticRules[0x8]), new LRReduction(0xf, staticRules[0x8]), new LRReduction(0x10, staticRules[0x8]), new LRReduction(0x12, staticRules[0x8]), new LRReduction(0x52, staticRules[0x8])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x9]), new LRReduction(0xc, staticRules[0x9]), new LRReduction(0xd, staticRules[0x9]), new LRReduction(0xe, staticRules[0x9]), new LRReduction(0xf, staticRules[0x9]), new LRReduction(0x10, staticRules[0x9]), new LRReduction(0x12, staticRules[0x9]), new LRReduction(0x52, staticRules[0x9])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0xA]), new LRReduction(0xc, staticRules[0xA]), new LRReduction(0xd, staticRules[0xA]), new LRReduction(0xe, staticRules[0xA]), new LRReduction(0xf, staticRules[0xA]), new LRReduction(0x10, staticRules[0xA]), new LRReduction(0x12, staticRules[0xA]), new LRReduction(0x52, staticRules[0xA])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x17},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[1] {0x52},
               new ushort[1] {0x19},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[31]},
               new ushort[1] {0xdb},
               new ushort[1] {0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x4E])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x4F])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x50])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x51])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x2])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x3])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x4])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x52, staticRules[0x5])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[0]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x1, staticRules[0xCF])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[7] {new LRReduction(0x2, staticRules[0xCE]), new LRReduction(0xc, staticRules[0xCE]), new LRReduction(0xd, staticRules[0xCE]), new LRReduction(0xe, staticRules[0xCE]), new LRReduction(0xf, staticRules[0xCE]), new LRReduction(0x10, staticRules[0xCE]), new LRReduction(0x52, staticRules[0xCE])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x1B},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0xb},
               new ushort[1] {0x1D},
               new ushort[1] {0x1b},
               new ushort[1] {0x1C},
               new LRReduction[3] {new LRReduction(0x11, staticRules[0x0]), new LRReduction(0x41, staticRules[0x0]), new LRReduction(0x48, staticRules[0x0])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[30]},
               new ushort[1] {0x53},
               new ushort[1] {0x1E},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x1F},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0xA, 0xC, 0x11, 0x12, 0x13, 0x14},
               new ushort[13] {0x19, 0x1c, 0x18, 0x1a, 0x73, 0x74, 0xde, 0xdf, 0x72, 0x14, 0x15, 0x16, 0x17},
               new ushort[13] {0x20, 0x21, 0x22, 0x5, 0x6, 0x7, 0x8, 0x9, 0xB, 0xD, 0xE, 0xF, 0x10},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xB])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0xb},
               new ushort[1] {0x23},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0x11, staticRules[0x1]), new LRReduction(0x41, staticRules[0x1]), new LRReduction(0x48, staticRules[0x1])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x24},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[36]},
               new ushort[2] {0xa, 0x57},
               new ushort[2] {0x25, 0x26},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[20]},
               new ushort[1] {0x51},
               new ushort[1] {0x28},
               new ushort[1] {0x71},
               new ushort[1] {0x27},
               new LRReduction[1] {new LRReduction(0x11, staticRules[0x4B])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x29},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[6] {0x10, 0x52, 0xc, 0xd, 0xe, 0xf},
               new ushort[6] {0xA, 0xC, 0x11, 0x12, 0x13, 0x14},
               new ushort[11] {0x18, 0x1a, 0x73, 0x74, 0xde, 0xdf, 0x72, 0x14, 0x15, 0x16, 0x17},
               new ushort[11] {0x2A, 0x5, 0x6, 0x7, 0x8, 0x9, 0xB, 0xD, 0xE, 0xF, 0x10},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xC])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[7] {new LRReduction(0xc, staticRules[0x10]), new LRReduction(0xd, staticRules[0x10]), new LRReduction(0xe, staticRules[0x10]), new LRReduction(0xf, staticRules[0x10]), new LRReduction(0x10, staticRules[0x10]), new LRReduction(0x12, staticRules[0x10]), new LRReduction(0x52, staticRules[0x10])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x2B},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0xb, staticRules[0xE]), new LRReduction(0x11, staticRules[0xE]), new LRReduction(0x41, staticRules[0xE]), new LRReduction(0x48, staticRules[0xE])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[20]},
               new ushort[1] {0x51},
               new ushort[1] {0x28},
               new ushort[1] {0x71},
               new ushort[1] {0x2C},
               new LRReduction[1] {new LRReduction(0x11, staticRules[0x4B])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x2D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x2E},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x2F},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0xD]), new LRReduction(0xc, staticRules[0xD]), new LRReduction(0xd, staticRules[0xD]), new LRReduction(0xe, staticRules[0xD]), new LRReduction(0xf, staticRules[0xD]), new LRReduction(0x10, staticRules[0xD]), new LRReduction(0x12, staticRules[0xD]), new LRReduction(0x52, staticRules[0xD])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[7] {new LRReduction(0xc, staticRules[0x11]), new LRReduction(0xd, staticRules[0x11]), new LRReduction(0xe, staticRules[0x11]), new LRReduction(0xf, staticRules[0x11]), new LRReduction(0x10, staticRules[0x11]), new LRReduction(0x12, staticRules[0x11]), new LRReduction(0x52, staticRules[0x11])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0xb, staticRules[0xF]), new LRReduction(0x11, staticRules[0xF]), new LRReduction(0x41, staticRules[0xF]), new LRReduction(0x48, staticRules[0xF])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x30},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[20]},
               new ushort[1] {0x51},
               new ushort[1] {0x28},
               new ushort[1] {0x71},
               new ushort[1] {0x31},
               new LRReduction[1] {new LRReduction(0x11, staticRules[0x4B])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[1] {0x4f},
               new ushort[1] {0x33},
               new ushort[1] {0x6f},
               new ushort[1] {0x32},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0x48},
               new ushort[1] {0x35},
               new ushort[1] {0x7b},
               new ushort[1] {0x34},
               new LRReduction[1] {new LRReduction(0x11, staticRules[0x4C])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[1] {0x4f},
               new ushort[1] {0x33},
               new ushort[1] {0x6f},
               new ushort[1] {0x36},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x37},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[39]},
               new ushort[2] {0x50, 0x54},
               new ushort[2] {0x3A, 0x3B},
               new ushort[2] {0x70, 0xe5},
               new ushort[2] {0x38, 0x39},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x3C},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[1] {0x48},
               new ushort[1] {0x3D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x11, staticRules[0x4D])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x3E},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[39]},
               new ushort[2] {0x54, 0x50},
               new ushort[2] {0x41, 0x3A},
               new ushort[2] {0x7c, 0x70},
               new ushort[2] {0x3F, 0x40},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[46]},
               new ushort[1] {0x4f},
               new ushort[1] {0x33},
               new ushort[1] {0x6f},
               new ushort[1] {0x42},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[39]},
               new ushort[1] {0x54},
               new ushort[1] {0x44},
               new ushort[1] {0xe0},
               new ushort[1] {0x43},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x45},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x46},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x47},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x48, 0x4B},
               new ushort[2] {0x79, 0x58},
               new ushort[2] {0x49, 0x4A},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x4C},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x11, staticRules[0x61]), new LRReduction(0x48, staticRules[0x61])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x4D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[39]},
               new ushort[1] {0x54},
               new ushort[1] {0x41},
               new ushort[1] {0x7c},
               new ushort[1] {0x4E},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x4F},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[39]},
               new ushort[1] {0x54},
               new ushort[1] {0x51},
               new ushort[1] {0x8e},
               new ushort[1] {0x50},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x52},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x53},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0xB6]), new LRReduction(0xc, staticRules[0xB6]), new LRReduction(0xd, staticRules[0xB6]), new LRReduction(0xe, staticRules[0xB6]), new LRReduction(0xf, staticRules[0xB6]), new LRReduction(0x10, staticRules[0xB6]), new LRReduction(0x12, staticRules[0xB6]), new LRReduction(0x52, staticRules[0xB6])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x54, 0x57},
               new ushort[2] {0x7a, 0x68},
               new ushort[2] {0x55, 0x56},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[2] {0x12, 0xdc},
               new ushort[2] {0x58, 0x5D},
               new ushort[4] {0xe9, 0xe6, 0xe8, 0xe7},
               new ushort[4] {0x59, 0x5A, 0x5B, 0x5C},
               new LRReduction[1] {new LRReduction(0xa, staticRules[0xC4])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[39]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x50, staticRules[0x47]), new LRReduction(0x54, staticRules[0x47])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x5E, 0x4B},
               new ushort[1] {0x58},
               new ushort[1] {0x5F},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x5D]), new LRReduction(0x12, staticRules[0x5D])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[8]},
               new ushort[1] {0x40},
               new ushort[1] {0x60},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[15]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x11, staticRules[0x62]), new LRReduction(0x48, staticRules[0x62])})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x52]), new LRReduction(0xc, staticRules[0x52]), new LRReduction(0xd, staticRules[0x52]), new LRReduction(0xe, staticRules[0x52]), new LRReduction(0xf, staticRules[0x52]), new LRReduction(0x10, staticRules[0x52]), new LRReduction(0x12, staticRules[0x52]), new LRReduction(0x52, staticRules[0x52])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x61},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x62, 0x66},
               new ushort[3] {0x8d, 0x7d, 0x8c},
               new ushort[3] {0x63, 0x64, 0x65},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x67},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[4]},
               new ushort[1] {0x11},
               new ushort[1] {0x68},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0xB5]), new LRReduction(0xc, staticRules[0xB5]), new LRReduction(0xd, staticRules[0xB5]), new LRReduction(0xe, staticRules[0xB5]), new LRReduction(0xf, staticRules[0xB5]), new LRReduction(0x10, staticRules[0xB5]), new LRReduction(0x12, staticRules[0xB5]), new LRReduction(0x52, staticRules[0xB5])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[2] {0x12, 0xdc},
               new ushort[2] {0x69, 0x6E},
               new ushort[4] {0xe4, 0xe1, 0xe3, 0xe2},
               new ushort[4] {0x6A, 0x6B, 0x6C, 0x6D},
               new LRReduction[1] {new LRReduction(0xa, staticRules[0xBA])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[39]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x54, staticRules[0x49])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x6F, 0x57},
               new ushort[1] {0x68},
               new ushort[1] {0x70},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x5F]), new LRReduction(0x12, staticRules[0x5F])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x71},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xC1])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[2] {0x12, 0xdc},
               new ushort[2] {0x72, 0x5D},
               new ushort[3] {0xe6, 0xe8, 0xe7},
               new ushort[3] {0x73, 0x74, 0x5C},
               new LRReduction[1] {new LRReduction(0xa, staticRules[0xC4])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC7]), new LRReduction(0x12, staticRules[0xC7]), new LRReduction(0xdc, staticRules[0xC7])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC8]), new LRReduction(0x12, staticRules[0xC8]), new LRReduction(0xdc, staticRules[0xC8])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x75},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x4a, staticRules[0x92]), new LRReduction(0xdd, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[51], FileCentralDogma_Lexer.terminals[39]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x50, staticRules[0x48]), new LRReduction(0x54, staticRules[0x48])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x5E]), new LRReduction(0x12, staticRules[0x5E])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[26]},
               new ushort[1] {0x2e},
               new ushort[1] {0x93},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x53]), new LRReduction(0xc, staticRules[0x53]), new LRReduction(0xd, staticRules[0x53]), new LRReduction(0xe, staticRules[0x53]), new LRReduction(0xf, staticRules[0x53]), new LRReduction(0x10, staticRules[0x53]), new LRReduction(0x12, staticRules[0x53]), new LRReduction(0x52, staticRules[0x53])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0x63])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x94, 0x66},
               new ushort[2] {0x7d, 0x8c},
               new ushort[2] {0x95, 0x96},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x88]), new LRReduction(0x12, staticRules[0x88])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x89]), new LRReduction(0x12, staticRules[0x89])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x97, 0x99},
               new ushort[1] {0x6c},
               new ushort[1] {0x98},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[8] {FileCentralDogma_Lexer.terminals[1], FileCentralDogma_Lexer.terminals[40], FileCentralDogma_Lexer.terminals[45], FileCentralDogma_Lexer.terminals[49], FileCentralDogma_Lexer.terminals[48], FileCentralDogma_Lexer.terminals[50], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[47]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[8] {new LRReduction(0x2, staticRules[0x54]), new LRReduction(0xc, staticRules[0x54]), new LRReduction(0xd, staticRules[0x54]), new LRReduction(0xe, staticRules[0x54]), new LRReduction(0xf, staticRules[0x54]), new LRReduction(0x10, staticRules[0x54]), new LRReduction(0x12, staticRules[0x54]), new LRReduction(0x52, staticRules[0x54])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0x9A, 0x9E},
               new ushort[3] {0x9f, 0x8f, 0x9e},
               new ushort[3] {0x9B, 0x9C, 0x9D},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xB7])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[2] {0x12, 0xdc},
               new ushort[2] {0x9F, 0x6E},
               new ushort[3] {0xe1, 0xe3, 0xe2},
               new ushort[3] {0xA0, 0xA1, 0x6D},
               new LRReduction[1] {new LRReduction(0xa, staticRules[0xBA])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xBD]), new LRReduction(0x12, staticRules[0xBD]), new LRReduction(0xdc, staticRules[0xBD])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xBE]), new LRReduction(0x12, staticRules[0xBE]), new LRReduction(0xdc, staticRules[0xBE])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0xA2},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x4a, staticRules[0x69]), new LRReduction(0xdd, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[39]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x54, staticRules[0x4A])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x60]), new LRReduction(0x12, staticRules[0x60])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[13] {0x66, 0x65, 0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[13] {0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xC2])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC9]), new LRReduction(0x12, staticRules[0xC9]), new LRReduction(0xdc, staticRules[0xC9])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xCA]), new LRReduction(0x12, staticRules[0xCA]), new LRReduction(0xdc, staticRules[0xCA])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xdc},
               new ushort[1] {0x5D},
               new ushort[1] {0xe7},
               new ushort[1] {0xC9},
               new LRReduction[2] {new LRReduction(0x4c, staticRules[0xC4]), new LRReduction(0x4d, staticRules[0xC4])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0xdd},
               new ushort[1] {0xCA},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xCC},
               new ushort[1] {0x9d},
               new ushort[1] {0xCB},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x8F]), new LRReduction(0x44, staticRules[0x8F]), new LRReduction(0xdd, staticRules[0x8F])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x91]), new LRReduction(0x44, staticRules[0x91]), new LRReduction(0x4a, staticRules[0x91]), new LRReduction(0xdd, staticRules[0x91])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xCE},
               new ushort[1] {0x9c},
               new ushort[1] {0xCD},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x93]), new LRReduction(0x44, staticRules[0x93]), new LRReduction(0x4a, staticRules[0x93]), new LRReduction(0xdd, staticRules[0x93])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[10] {0x9b, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[10] {0xCF, 0xD0, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x95]), new LRReduction(0x44, staticRules[0x95]), new LRReduction(0x49, staticRules[0x95]), new LRReduction(0x4a, staticRules[0x95]), new LRReduction(0xdd, staticRules[0x95])})
            , new LR1State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xD1, 0xD2, 0xD3},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0x97]), new LRReduction(0x11, staticRules[0x97]), new LRReduction(0x2e, staticRules[0x97]), new LRReduction(0x34, staticRules[0x97]), new LRReduction(0x35, staticRules[0x97]), new LRReduction(0x36, staticRules[0x97]), new LRReduction(0x37, staticRules[0x97]), new LRReduction(0x38, staticRules[0x97]), new LRReduction(0x39, staticRules[0x97]), new LRReduction(0x3a, staticRules[0x97]), new LRReduction(0x3b, staticRules[0x97]), new LRReduction(0x3c, staticRules[0x97]), new LRReduction(0x3d, staticRules[0x97]), new LRReduction(0x3e, staticRules[0x97]), new LRReduction(0x3f, staticRules[0x97]), new LRReduction(0x41, staticRules[0x97]), new LRReduction(0x43, staticRules[0x97]), new LRReduction(0x44, staticRules[0x97]), new LRReduction(0x49, staticRules[0x97]), new LRReduction(0x4a, staticRules[0x97]), new LRReduction(0xdd, staticRules[0x97])})
            , new LR1State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xD4, 0xD5},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[24] {new LRReduction(0xa, staticRules[0x9B]), new LRReduction(0x11, staticRules[0x9B]), new LRReduction(0x2e, staticRules[0x9B]), new LRReduction(0x34, staticRules[0x9B]), new LRReduction(0x35, staticRules[0x9B]), new LRReduction(0x36, staticRules[0x9B]), new LRReduction(0x37, staticRules[0x9B]), new LRReduction(0x38, staticRules[0x9B]), new LRReduction(0x39, staticRules[0x9B]), new LRReduction(0x3a, staticRules[0x9B]), new LRReduction(0x3b, staticRules[0x9B]), new LRReduction(0x3c, staticRules[0x9B]), new LRReduction(0x3d, staticRules[0x9B]), new LRReduction(0x3e, staticRules[0x9B]), new LRReduction(0x3f, staticRules[0x9B]), new LRReduction(0x41, staticRules[0x9B]), new LRReduction(0x43, staticRules[0x9B]), new LRReduction(0x44, staticRules[0x9B]), new LRReduction(0x45, staticRules[0x9B]), new LRReduction(0x46, staticRules[0x9B]), new LRReduction(0x47, staticRules[0x9B]), new LRReduction(0x49, staticRules[0x9B]), new LRReduction(0x4a, staticRules[0x9B]), new LRReduction(0xdd, staticRules[0x9B])})
            , new LR1State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[26] {new LRReduction(0xa, staticRules[0x9E]), new LRReduction(0x11, staticRules[0x9E]), new LRReduction(0x2e, staticRules[0x9E]), new LRReduction(0x34, staticRules[0x9E]), new LRReduction(0x35, staticRules[0x9E]), new LRReduction(0x36, staticRules[0x9E]), new LRReduction(0x37, staticRules[0x9E]), new LRReduction(0x38, staticRules[0x9E]), new LRReduction(0x39, staticRules[0x9E]), new LRReduction(0x3a, staticRules[0x9E]), new LRReduction(0x3b, staticRules[0x9E]), new LRReduction(0x3c, staticRules[0x9E]), new LRReduction(0x3d, staticRules[0x9E]), new LRReduction(0x3e, staticRules[0x9E]), new LRReduction(0x3f, staticRules[0x9E]), new LRReduction(0x41, staticRules[0x9E]), new LRReduction(0x43, staticRules[0x9E]), new LRReduction(0x44, staticRules[0x9E]), new LRReduction(0x45, staticRules[0x9E]), new LRReduction(0x46, staticRules[0x9E]), new LRReduction(0x47, staticRules[0x9E]), new LRReduction(0x49, staticRules[0x9E]), new LRReduction(0x4a, staticRules[0x9E]), new LRReduction(0x55, staticRules[0x9E]), new LRReduction(0x56, staticRules[0x9E]), new LRReduction(0xdd, staticRules[0x9E])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0xD6, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x44, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA0]), new LRReduction(0x11, staticRules[0xA0]), new LRReduction(0x2e, staticRules[0xA0]), new LRReduction(0x34, staticRules[0xA0]), new LRReduction(0x35, staticRules[0xA0]), new LRReduction(0x36, staticRules[0xA0]), new LRReduction(0x37, staticRules[0xA0]), new LRReduction(0x38, staticRules[0xA0]), new LRReduction(0x39, staticRules[0xA0]), new LRReduction(0x3a, staticRules[0xA0]), new LRReduction(0x3b, staticRules[0xA0]), new LRReduction(0x3c, staticRules[0xA0]), new LRReduction(0x3d, staticRules[0xA0]), new LRReduction(0x3e, staticRules[0xA0]), new LRReduction(0x3f, staticRules[0xA0]), new LRReduction(0x41, staticRules[0xA0]), new LRReduction(0x43, staticRules[0xA0]), new LRReduction(0x44, staticRules[0xA0]), new LRReduction(0x45, staticRules[0xA0]), new LRReduction(0x46, staticRules[0xA0]), new LRReduction(0x47, staticRules[0xA0]), new LRReduction(0x48, staticRules[0xA0]), new LRReduction(0x49, staticRules[0xA0]), new LRReduction(0x4a, staticRules[0xA0]), new LRReduction(0x4e, staticRules[0xA0]), new LRReduction(0x55, staticRules[0xA0]), new LRReduction(0x56, staticRules[0xA0]), new LRReduction(0xdd, staticRules[0xA0])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA1]), new LRReduction(0x11, staticRules[0xA1]), new LRReduction(0x2e, staticRules[0xA1]), new LRReduction(0x34, staticRules[0xA1]), new LRReduction(0x35, staticRules[0xA1]), new LRReduction(0x36, staticRules[0xA1]), new LRReduction(0x37, staticRules[0xA1]), new LRReduction(0x38, staticRules[0xA1]), new LRReduction(0x39, staticRules[0xA1]), new LRReduction(0x3a, staticRules[0xA1]), new LRReduction(0x3b, staticRules[0xA1]), new LRReduction(0x3c, staticRules[0xA1]), new LRReduction(0x3d, staticRules[0xA1]), new LRReduction(0x3e, staticRules[0xA1]), new LRReduction(0x3f, staticRules[0xA1]), new LRReduction(0x41, staticRules[0xA1]), new LRReduction(0x43, staticRules[0xA1]), new LRReduction(0x44, staticRules[0xA1]), new LRReduction(0x45, staticRules[0xA1]), new LRReduction(0x46, staticRules[0xA1]), new LRReduction(0x47, staticRules[0xA1]), new LRReduction(0x48, staticRules[0xA1]), new LRReduction(0x49, staticRules[0xA1]), new LRReduction(0x4a, staticRules[0xA1]), new LRReduction(0x4e, staticRules[0xA1]), new LRReduction(0x55, staticRules[0xA1]), new LRReduction(0x56, staticRules[0xA1]), new LRReduction(0xdd, staticRules[0xA1])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA2]), new LRReduction(0x11, staticRules[0xA2]), new LRReduction(0x2e, staticRules[0xA2]), new LRReduction(0x34, staticRules[0xA2]), new LRReduction(0x35, staticRules[0xA2]), new LRReduction(0x36, staticRules[0xA2]), new LRReduction(0x37, staticRules[0xA2]), new LRReduction(0x38, staticRules[0xA2]), new LRReduction(0x39, staticRules[0xA2]), new LRReduction(0x3a, staticRules[0xA2]), new LRReduction(0x3b, staticRules[0xA2]), new LRReduction(0x3c, staticRules[0xA2]), new LRReduction(0x3d, staticRules[0xA2]), new LRReduction(0x3e, staticRules[0xA2]), new LRReduction(0x3f, staticRules[0xA2]), new LRReduction(0x41, staticRules[0xA2]), new LRReduction(0x43, staticRules[0xA2]), new LRReduction(0x44, staticRules[0xA2]), new LRReduction(0x45, staticRules[0xA2]), new LRReduction(0x46, staticRules[0xA2]), new LRReduction(0x47, staticRules[0xA2]), new LRReduction(0x48, staticRules[0xA2]), new LRReduction(0x49, staticRules[0xA2]), new LRReduction(0x4a, staticRules[0xA2]), new LRReduction(0x4e, staticRules[0xA2]), new LRReduction(0x55, staticRules[0xA2]), new LRReduction(0x56, staticRules[0xA2]), new LRReduction(0xdd, staticRules[0xA2])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA3]), new LRReduction(0x11, staticRules[0xA3]), new LRReduction(0x2e, staticRules[0xA3]), new LRReduction(0x34, staticRules[0xA3]), new LRReduction(0x35, staticRules[0xA3]), new LRReduction(0x36, staticRules[0xA3]), new LRReduction(0x37, staticRules[0xA3]), new LRReduction(0x38, staticRules[0xA3]), new LRReduction(0x39, staticRules[0xA3]), new LRReduction(0x3a, staticRules[0xA3]), new LRReduction(0x3b, staticRules[0xA3]), new LRReduction(0x3c, staticRules[0xA3]), new LRReduction(0x3d, staticRules[0xA3]), new LRReduction(0x3e, staticRules[0xA3]), new LRReduction(0x3f, staticRules[0xA3]), new LRReduction(0x41, staticRules[0xA3]), new LRReduction(0x43, staticRules[0xA3]), new LRReduction(0x44, staticRules[0xA3]), new LRReduction(0x45, staticRules[0xA3]), new LRReduction(0x46, staticRules[0xA3]), new LRReduction(0x47, staticRules[0xA3]), new LRReduction(0x48, staticRules[0xA3]), new LRReduction(0x49, staticRules[0xA3]), new LRReduction(0x4a, staticRules[0xA3]), new LRReduction(0x4e, staticRules[0xA3]), new LRReduction(0x55, staticRules[0xA3]), new LRReduction(0x56, staticRules[0xA3]), new LRReduction(0xdd, staticRules[0xA3])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA4]), new LRReduction(0x11, staticRules[0xA4]), new LRReduction(0x2e, staticRules[0xA4]), new LRReduction(0x34, staticRules[0xA4]), new LRReduction(0x35, staticRules[0xA4]), new LRReduction(0x36, staticRules[0xA4]), new LRReduction(0x37, staticRules[0xA4]), new LRReduction(0x38, staticRules[0xA4]), new LRReduction(0x39, staticRules[0xA4]), new LRReduction(0x3a, staticRules[0xA4]), new LRReduction(0x3b, staticRules[0xA4]), new LRReduction(0x3c, staticRules[0xA4]), new LRReduction(0x3d, staticRules[0xA4]), new LRReduction(0x3e, staticRules[0xA4]), new LRReduction(0x3f, staticRules[0xA4]), new LRReduction(0x41, staticRules[0xA4]), new LRReduction(0x43, staticRules[0xA4]), new LRReduction(0x44, staticRules[0xA4]), new LRReduction(0x45, staticRules[0xA4]), new LRReduction(0x46, staticRules[0xA4]), new LRReduction(0x47, staticRules[0xA4]), new LRReduction(0x48, staticRules[0xA4]), new LRReduction(0x49, staticRules[0xA4]), new LRReduction(0x4a, staticRules[0xA4]), new LRReduction(0x4e, staticRules[0xA4]), new LRReduction(0x55, staticRules[0xA4]), new LRReduction(0x56, staticRules[0xA4]), new LRReduction(0xdd, staticRules[0xA4])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0xD7},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[29] {new LRReduction(0xa, staticRules[0x36]), new LRReduction(0x11, staticRules[0x36]), new LRReduction(0x2e, staticRules[0x36]), new LRReduction(0x30, staticRules[0x36]), new LRReduction(0x34, staticRules[0x36]), new LRReduction(0x35, staticRules[0x36]), new LRReduction(0x36, staticRules[0x36]), new LRReduction(0x37, staticRules[0x36]), new LRReduction(0x38, staticRules[0x36]), new LRReduction(0x39, staticRules[0x36]), new LRReduction(0x3a, staticRules[0x36]), new LRReduction(0x3b, staticRules[0x36]), new LRReduction(0x3c, staticRules[0x36]), new LRReduction(0x3d, staticRules[0x36]), new LRReduction(0x3e, staticRules[0x36]), new LRReduction(0x3f, staticRules[0x36]), new LRReduction(0x41, staticRules[0x36]), new LRReduction(0x43, staticRules[0x36]), new LRReduction(0x44, staticRules[0x36]), new LRReduction(0x45, staticRules[0x36]), new LRReduction(0x46, staticRules[0x36]), new LRReduction(0x47, staticRules[0x36]), new LRReduction(0x48, staticRules[0x36]), new LRReduction(0x49, staticRules[0x36]), new LRReduction(0x4a, staticRules[0x36]), new LRReduction(0x4e, staticRules[0x36]), new LRReduction(0x55, staticRules[0x36]), new LRReduction(0x56, staticRules[0x36]), new LRReduction(0xdd, staticRules[0x36])})
            , new LR1State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4d},
               new ushort[1] {0xD9},
               new ushort[1] {0x99},
               new ushort[1] {0xD8},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x37]), new LRReduction(0x11, staticRules[0x37]), new LRReduction(0x2e, staticRules[0x37]), new LRReduction(0x34, staticRules[0x37]), new LRReduction(0x35, staticRules[0x37]), new LRReduction(0x36, staticRules[0x37]), new LRReduction(0x37, staticRules[0x37]), new LRReduction(0x38, staticRules[0x37]), new LRReduction(0x39, staticRules[0x37]), new LRReduction(0x3a, staticRules[0x37]), new LRReduction(0x3b, staticRules[0x37]), new LRReduction(0x3c, staticRules[0x37]), new LRReduction(0x3d, staticRules[0x37]), new LRReduction(0x3e, staticRules[0x37]), new LRReduction(0x3f, staticRules[0x37]), new LRReduction(0x41, staticRules[0x37]), new LRReduction(0x43, staticRules[0x37]), new LRReduction(0x44, staticRules[0x37]), new LRReduction(0x45, staticRules[0x37]), new LRReduction(0x46, staticRules[0x37]), new LRReduction(0x47, staticRules[0x37]), new LRReduction(0x48, staticRules[0x37]), new LRReduction(0x49, staticRules[0x37]), new LRReduction(0x4a, staticRules[0x37]), new LRReduction(0x4e, staticRules[0x37]), new LRReduction(0x55, staticRules[0x37]), new LRReduction(0x56, staticRules[0x37]), new LRReduction(0xdd, staticRules[0x37])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3A]), new LRReduction(0x11, staticRules[0x3A]), new LRReduction(0x2e, staticRules[0x3A]), new LRReduction(0x34, staticRules[0x3A]), new LRReduction(0x35, staticRules[0x3A]), new LRReduction(0x36, staticRules[0x3A]), new LRReduction(0x37, staticRules[0x3A]), new LRReduction(0x38, staticRules[0x3A]), new LRReduction(0x39, staticRules[0x3A]), new LRReduction(0x3a, staticRules[0x3A]), new LRReduction(0x3b, staticRules[0x3A]), new LRReduction(0x3c, staticRules[0x3A]), new LRReduction(0x3d, staticRules[0x3A]), new LRReduction(0x3e, staticRules[0x3A]), new LRReduction(0x3f, staticRules[0x3A]), new LRReduction(0x41, staticRules[0x3A]), new LRReduction(0x43, staticRules[0x3A]), new LRReduction(0x44, staticRules[0x3A]), new LRReduction(0x45, staticRules[0x3A]), new LRReduction(0x46, staticRules[0x3A]), new LRReduction(0x47, staticRules[0x3A]), new LRReduction(0x48, staticRules[0x3A]), new LRReduction(0x49, staticRules[0x3A]), new LRReduction(0x4a, staticRules[0x3A]), new LRReduction(0x4e, staticRules[0x3A]), new LRReduction(0x55, staticRules[0x3A]), new LRReduction(0x56, staticRules[0x3A]), new LRReduction(0xdd, staticRules[0x3A])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3B]), new LRReduction(0x11, staticRules[0x3B]), new LRReduction(0x2e, staticRules[0x3B]), new LRReduction(0x34, staticRules[0x3B]), new LRReduction(0x35, staticRules[0x3B]), new LRReduction(0x36, staticRules[0x3B]), new LRReduction(0x37, staticRules[0x3B]), new LRReduction(0x38, staticRules[0x3B]), new LRReduction(0x39, staticRules[0x3B]), new LRReduction(0x3a, staticRules[0x3B]), new LRReduction(0x3b, staticRules[0x3B]), new LRReduction(0x3c, staticRules[0x3B]), new LRReduction(0x3d, staticRules[0x3B]), new LRReduction(0x3e, staticRules[0x3B]), new LRReduction(0x3f, staticRules[0x3B]), new LRReduction(0x41, staticRules[0x3B]), new LRReduction(0x43, staticRules[0x3B]), new LRReduction(0x44, staticRules[0x3B]), new LRReduction(0x45, staticRules[0x3B]), new LRReduction(0x46, staticRules[0x3B]), new LRReduction(0x47, staticRules[0x3B]), new LRReduction(0x48, staticRules[0x3B]), new LRReduction(0x49, staticRules[0x3B]), new LRReduction(0x4a, staticRules[0x3B]), new LRReduction(0x4e, staticRules[0x3B]), new LRReduction(0x55, staticRules[0x3B]), new LRReduction(0x56, staticRules[0x3B]), new LRReduction(0xdd, staticRules[0x3B])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3C]), new LRReduction(0x11, staticRules[0x3C]), new LRReduction(0x2e, staticRules[0x3C]), new LRReduction(0x34, staticRules[0x3C]), new LRReduction(0x35, staticRules[0x3C]), new LRReduction(0x36, staticRules[0x3C]), new LRReduction(0x37, staticRules[0x3C]), new LRReduction(0x38, staticRules[0x3C]), new LRReduction(0x39, staticRules[0x3C]), new LRReduction(0x3a, staticRules[0x3C]), new LRReduction(0x3b, staticRules[0x3C]), new LRReduction(0x3c, staticRules[0x3C]), new LRReduction(0x3d, staticRules[0x3C]), new LRReduction(0x3e, staticRules[0x3C]), new LRReduction(0x3f, staticRules[0x3C]), new LRReduction(0x41, staticRules[0x3C]), new LRReduction(0x43, staticRules[0x3C]), new LRReduction(0x44, staticRules[0x3C]), new LRReduction(0x45, staticRules[0x3C]), new LRReduction(0x46, staticRules[0x3C]), new LRReduction(0x47, staticRules[0x3C]), new LRReduction(0x48, staticRules[0x3C]), new LRReduction(0x49, staticRules[0x3C]), new LRReduction(0x4a, staticRules[0x3C]), new LRReduction(0x4e, staticRules[0x3C]), new LRReduction(0x55, staticRules[0x3C]), new LRReduction(0x56, staticRules[0x3C]), new LRReduction(0xdd, staticRules[0x3C])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3D]), new LRReduction(0x11, staticRules[0x3D]), new LRReduction(0x2e, staticRules[0x3D]), new LRReduction(0x34, staticRules[0x3D]), new LRReduction(0x35, staticRules[0x3D]), new LRReduction(0x36, staticRules[0x3D]), new LRReduction(0x37, staticRules[0x3D]), new LRReduction(0x38, staticRules[0x3D]), new LRReduction(0x39, staticRules[0x3D]), new LRReduction(0x3a, staticRules[0x3D]), new LRReduction(0x3b, staticRules[0x3D]), new LRReduction(0x3c, staticRules[0x3D]), new LRReduction(0x3d, staticRules[0x3D]), new LRReduction(0x3e, staticRules[0x3D]), new LRReduction(0x3f, staticRules[0x3D]), new LRReduction(0x41, staticRules[0x3D]), new LRReduction(0x43, staticRules[0x3D]), new LRReduction(0x44, staticRules[0x3D]), new LRReduction(0x45, staticRules[0x3D]), new LRReduction(0x46, staticRules[0x3D]), new LRReduction(0x47, staticRules[0x3D]), new LRReduction(0x48, staticRules[0x3D]), new LRReduction(0x49, staticRules[0x3D]), new LRReduction(0x4a, staticRules[0x3D]), new LRReduction(0x4e, staticRules[0x3D]), new LRReduction(0x55, staticRules[0x3D]), new LRReduction(0x56, staticRules[0x3D]), new LRReduction(0xdd, staticRules[0x3D])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3E]), new LRReduction(0x11, staticRules[0x3E]), new LRReduction(0x2e, staticRules[0x3E]), new LRReduction(0x34, staticRules[0x3E]), new LRReduction(0x35, staticRules[0x3E]), new LRReduction(0x36, staticRules[0x3E]), new LRReduction(0x37, staticRules[0x3E]), new LRReduction(0x38, staticRules[0x3E]), new LRReduction(0x39, staticRules[0x3E]), new LRReduction(0x3a, staticRules[0x3E]), new LRReduction(0x3b, staticRules[0x3E]), new LRReduction(0x3c, staticRules[0x3E]), new LRReduction(0x3d, staticRules[0x3E]), new LRReduction(0x3e, staticRules[0x3E]), new LRReduction(0x3f, staticRules[0x3E]), new LRReduction(0x41, staticRules[0x3E]), new LRReduction(0x43, staticRules[0x3E]), new LRReduction(0x44, staticRules[0x3E]), new LRReduction(0x45, staticRules[0x3E]), new LRReduction(0x46, staticRules[0x3E]), new LRReduction(0x47, staticRules[0x3E]), new LRReduction(0x48, staticRules[0x3E]), new LRReduction(0x49, staticRules[0x3E]), new LRReduction(0x4a, staticRules[0x3E]), new LRReduction(0x4e, staticRules[0x3E]), new LRReduction(0x55, staticRules[0x3E]), new LRReduction(0x56, staticRules[0x3E]), new LRReduction(0xdd, staticRules[0x3E])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x3F]), new LRReduction(0x11, staticRules[0x3F]), new LRReduction(0x2e, staticRules[0x3F]), new LRReduction(0x34, staticRules[0x3F]), new LRReduction(0x35, staticRules[0x3F]), new LRReduction(0x36, staticRules[0x3F]), new LRReduction(0x37, staticRules[0x3F]), new LRReduction(0x38, staticRules[0x3F]), new LRReduction(0x39, staticRules[0x3F]), new LRReduction(0x3a, staticRules[0x3F]), new LRReduction(0x3b, staticRules[0x3F]), new LRReduction(0x3c, staticRules[0x3F]), new LRReduction(0x3d, staticRules[0x3F]), new LRReduction(0x3e, staticRules[0x3F]), new LRReduction(0x3f, staticRules[0x3F]), new LRReduction(0x41, staticRules[0x3F]), new LRReduction(0x43, staticRules[0x3F]), new LRReduction(0x44, staticRules[0x3F]), new LRReduction(0x45, staticRules[0x3F]), new LRReduction(0x46, staticRules[0x3F]), new LRReduction(0x47, staticRules[0x3F]), new LRReduction(0x48, staticRules[0x3F]), new LRReduction(0x49, staticRules[0x3F]), new LRReduction(0x4a, staticRules[0x3F]), new LRReduction(0x4e, staticRules[0x3F]), new LRReduction(0x55, staticRules[0x3F]), new LRReduction(0x56, staticRules[0x3F]), new LRReduction(0xdd, staticRules[0x3F])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x40]), new LRReduction(0x11, staticRules[0x40]), new LRReduction(0x2e, staticRules[0x40]), new LRReduction(0x34, staticRules[0x40]), new LRReduction(0x35, staticRules[0x40]), new LRReduction(0x36, staticRules[0x40]), new LRReduction(0x37, staticRules[0x40]), new LRReduction(0x38, staticRules[0x40]), new LRReduction(0x39, staticRules[0x40]), new LRReduction(0x3a, staticRules[0x40]), new LRReduction(0x3b, staticRules[0x40]), new LRReduction(0x3c, staticRules[0x40]), new LRReduction(0x3d, staticRules[0x40]), new LRReduction(0x3e, staticRules[0x40]), new LRReduction(0x3f, staticRules[0x40]), new LRReduction(0x41, staticRules[0x40]), new LRReduction(0x43, staticRules[0x40]), new LRReduction(0x44, staticRules[0x40]), new LRReduction(0x45, staticRules[0x40]), new LRReduction(0x46, staticRules[0x40]), new LRReduction(0x47, staticRules[0x40]), new LRReduction(0x48, staticRules[0x40]), new LRReduction(0x49, staticRules[0x40]), new LRReduction(0x4a, staticRules[0x40]), new LRReduction(0x4e, staticRules[0x40]), new LRReduction(0x55, staticRules[0x40]), new LRReduction(0x56, staticRules[0x40]), new LRReduction(0xdd, staticRules[0x40])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x41]), new LRReduction(0x11, staticRules[0x41]), new LRReduction(0x2e, staticRules[0x41]), new LRReduction(0x34, staticRules[0x41]), new LRReduction(0x35, staticRules[0x41]), new LRReduction(0x36, staticRules[0x41]), new LRReduction(0x37, staticRules[0x41]), new LRReduction(0x38, staticRules[0x41]), new LRReduction(0x39, staticRules[0x41]), new LRReduction(0x3a, staticRules[0x41]), new LRReduction(0x3b, staticRules[0x41]), new LRReduction(0x3c, staticRules[0x41]), new LRReduction(0x3d, staticRules[0x41]), new LRReduction(0x3e, staticRules[0x41]), new LRReduction(0x3f, staticRules[0x41]), new LRReduction(0x41, staticRules[0x41]), new LRReduction(0x43, staticRules[0x41]), new LRReduction(0x44, staticRules[0x41]), new LRReduction(0x45, staticRules[0x41]), new LRReduction(0x46, staticRules[0x41]), new LRReduction(0x47, staticRules[0x41]), new LRReduction(0x48, staticRules[0x41]), new LRReduction(0x49, staticRules[0x41]), new LRReduction(0x4a, staticRules[0x41]), new LRReduction(0x4e, staticRules[0x41]), new LRReduction(0x55, staticRules[0x41]), new LRReduction(0x56, staticRules[0x41]), new LRReduction(0xdd, staticRules[0x41])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x42]), new LRReduction(0x11, staticRules[0x42]), new LRReduction(0x2e, staticRules[0x42]), new LRReduction(0x34, staticRules[0x42]), new LRReduction(0x35, staticRules[0x42]), new LRReduction(0x36, staticRules[0x42]), new LRReduction(0x37, staticRules[0x42]), new LRReduction(0x38, staticRules[0x42]), new LRReduction(0x39, staticRules[0x42]), new LRReduction(0x3a, staticRules[0x42]), new LRReduction(0x3b, staticRules[0x42]), new LRReduction(0x3c, staticRules[0x42]), new LRReduction(0x3d, staticRules[0x42]), new LRReduction(0x3e, staticRules[0x42]), new LRReduction(0x3f, staticRules[0x42]), new LRReduction(0x41, staticRules[0x42]), new LRReduction(0x43, staticRules[0x42]), new LRReduction(0x44, staticRules[0x42]), new LRReduction(0x45, staticRules[0x42]), new LRReduction(0x46, staticRules[0x42]), new LRReduction(0x47, staticRules[0x42]), new LRReduction(0x48, staticRules[0x42]), new LRReduction(0x49, staticRules[0x42]), new LRReduction(0x4a, staticRules[0x42]), new LRReduction(0x4e, staticRules[0x42]), new LRReduction(0x55, staticRules[0x42]), new LRReduction(0x56, staticRules[0x42]), new LRReduction(0xdd, staticRules[0x42])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x43]), new LRReduction(0x11, staticRules[0x43]), new LRReduction(0x2e, staticRules[0x43]), new LRReduction(0x34, staticRules[0x43]), new LRReduction(0x35, staticRules[0x43]), new LRReduction(0x36, staticRules[0x43]), new LRReduction(0x37, staticRules[0x43]), new LRReduction(0x38, staticRules[0x43]), new LRReduction(0x39, staticRules[0x43]), new LRReduction(0x3a, staticRules[0x43]), new LRReduction(0x3b, staticRules[0x43]), new LRReduction(0x3c, staticRules[0x43]), new LRReduction(0x3d, staticRules[0x43]), new LRReduction(0x3e, staticRules[0x43]), new LRReduction(0x3f, staticRules[0x43]), new LRReduction(0x41, staticRules[0x43]), new LRReduction(0x43, staticRules[0x43]), new LRReduction(0x44, staticRules[0x43]), new LRReduction(0x45, staticRules[0x43]), new LRReduction(0x46, staticRules[0x43]), new LRReduction(0x47, staticRules[0x43]), new LRReduction(0x48, staticRules[0x43]), new LRReduction(0x49, staticRules[0x43]), new LRReduction(0x4a, staticRules[0x43]), new LRReduction(0x4e, staticRules[0x43]), new LRReduction(0x55, staticRules[0x43]), new LRReduction(0x56, staticRules[0x43]), new LRReduction(0xdd, staticRules[0x43])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x44]), new LRReduction(0x11, staticRules[0x44]), new LRReduction(0x2e, staticRules[0x44]), new LRReduction(0x34, staticRules[0x44]), new LRReduction(0x35, staticRules[0x44]), new LRReduction(0x36, staticRules[0x44]), new LRReduction(0x37, staticRules[0x44]), new LRReduction(0x38, staticRules[0x44]), new LRReduction(0x39, staticRules[0x44]), new LRReduction(0x3a, staticRules[0x44]), new LRReduction(0x3b, staticRules[0x44]), new LRReduction(0x3c, staticRules[0x44]), new LRReduction(0x3d, staticRules[0x44]), new LRReduction(0x3e, staticRules[0x44]), new LRReduction(0x3f, staticRules[0x44]), new LRReduction(0x41, staticRules[0x44]), new LRReduction(0x43, staticRules[0x44]), new LRReduction(0x44, staticRules[0x44]), new LRReduction(0x45, staticRules[0x44]), new LRReduction(0x46, staticRules[0x44]), new LRReduction(0x47, staticRules[0x44]), new LRReduction(0x48, staticRules[0x44]), new LRReduction(0x49, staticRules[0x44]), new LRReduction(0x4a, staticRules[0x44]), new LRReduction(0x4e, staticRules[0x44]), new LRReduction(0x55, staticRules[0x44]), new LRReduction(0x56, staticRules[0x44]), new LRReduction(0xdd, staticRules[0x44])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0x45]), new LRReduction(0x11, staticRules[0x45]), new LRReduction(0x2e, staticRules[0x45]), new LRReduction(0x34, staticRules[0x45]), new LRReduction(0x35, staticRules[0x45]), new LRReduction(0x36, staticRules[0x45]), new LRReduction(0x37, staticRules[0x45]), new LRReduction(0x38, staticRules[0x45]), new LRReduction(0x39, staticRules[0x45]), new LRReduction(0x3a, staticRules[0x45]), new LRReduction(0x3b, staticRules[0x45]), new LRReduction(0x3c, staticRules[0x45]), new LRReduction(0x3d, staticRules[0x45]), new LRReduction(0x3e, staticRules[0x45]), new LRReduction(0x3f, staticRules[0x45]), new LRReduction(0x41, staticRules[0x45]), new LRReduction(0x43, staticRules[0x45]), new LRReduction(0x44, staticRules[0x45]), new LRReduction(0x45, staticRules[0x45]), new LRReduction(0x46, staticRules[0x45]), new LRReduction(0x47, staticRules[0x45]), new LRReduction(0x48, staticRules[0x45]), new LRReduction(0x49, staticRules[0x45]), new LRReduction(0x4a, staticRules[0x45]), new LRReduction(0x4e, staticRules[0x45]), new LRReduction(0x55, staticRules[0x45]), new LRReduction(0x56, staticRules[0x45]), new LRReduction(0xdd, staticRules[0x45])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0xDA},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0x64])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x8A]), new LRReduction(0x12, staticRules[0x8A])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x8B]), new LRReduction(0x12, staticRules[0x8B])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0xDB, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0xDC},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0xDD},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0x8C])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[2] {0x12, 0xa},
               new ushort[2] {0xDE, 0x9E},
               new ushort[2] {0x8f, 0x9e},
               new ushort[2] {0xDF, 0xE0},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0xB1]), new LRReduction(0x12, staticRules[0xB1])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0xB2]), new LRReduction(0x12, staticRules[0xB2])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0xE1, 0x99},
               new ushort[1] {0x6c},
               new ushort[1] {0xE2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0xB8])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xBF]), new LRReduction(0x12, staticRules[0xBF]), new LRReduction(0xdc, staticRules[0xBF])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC0]), new LRReduction(0x12, staticRules[0xC0]), new LRReduction(0xdc, staticRules[0xC0])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[6]},
               new ushort[1] {0xdc},
               new ushort[1] {0x6E},
               new ushort[1] {0xe2},
               new ushort[1] {0xE3},
               new LRReduction[2] {new LRReduction(0x4c, staticRules[0xBA]), new LRReduction(0x4d, staticRules[0xBA])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0xdd},
               new ushort[1] {0xE4},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0xE6},
               new ushort[1] {0x8b},
               new ushort[1] {0xE5},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x66]), new LRReduction(0x44, staticRules[0x66]), new LRReduction(0xdd, staticRules[0x66])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x68]), new LRReduction(0x44, staticRules[0x68]), new LRReduction(0x4a, staticRules[0x68]), new LRReduction(0xdd, staticRules[0x68])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0xE8},
               new ushort[1] {0x8a},
               new ushort[1] {0xE7},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x6A]), new LRReduction(0x44, staticRules[0x6A]), new LRReduction(0x4a, staticRules[0x6A]), new LRReduction(0xdd, staticRules[0x6A])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[11] {0x89, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[11] {0xE9, 0xEA, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x6C]), new LRReduction(0x44, staticRules[0x6C]), new LRReduction(0x49, staticRules[0x6C]), new LRReduction(0x4a, staticRules[0x6C]), new LRReduction(0xdd, staticRules[0x6C])})
            , new LR1State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[3] {0x45, 0x46, 0x47},
               new ushort[3] {0xEB, 0xEC, 0xED},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x6E]), new LRReduction(0x11, staticRules[0x6E]), new LRReduction(0x2e, staticRules[0x6E]), new LRReduction(0x30, staticRules[0x6E]), new LRReduction(0x41, staticRules[0x6E]), new LRReduction(0x43, staticRules[0x6E]), new LRReduction(0x44, staticRules[0x6E]), new LRReduction(0x49, staticRules[0x6E]), new LRReduction(0x4a, staticRules[0x6E]), new LRReduction(0xdd, staticRules[0x6E])})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[2] {0x55, 0x56},
               new ushort[2] {0xEE, 0xEF},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[13] {new LRReduction(0xa, staticRules[0x72]), new LRReduction(0x11, staticRules[0x72]), new LRReduction(0x2e, staticRules[0x72]), new LRReduction(0x30, staticRules[0x72]), new LRReduction(0x41, staticRules[0x72]), new LRReduction(0x43, staticRules[0x72]), new LRReduction(0x44, staticRules[0x72]), new LRReduction(0x45, staticRules[0x72]), new LRReduction(0x46, staticRules[0x72]), new LRReduction(0x47, staticRules[0x72]), new LRReduction(0x49, staticRules[0x72]), new LRReduction(0x4a, staticRules[0x72]), new LRReduction(0xdd, staticRules[0x72])})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[15] {new LRReduction(0xa, staticRules[0x75]), new LRReduction(0x11, staticRules[0x75]), new LRReduction(0x2e, staticRules[0x75]), new LRReduction(0x30, staticRules[0x75]), new LRReduction(0x41, staticRules[0x75]), new LRReduction(0x43, staticRules[0x75]), new LRReduction(0x44, staticRules[0x75]), new LRReduction(0x45, staticRules[0x75]), new LRReduction(0x46, staticRules[0x75]), new LRReduction(0x47, staticRules[0x75]), new LRReduction(0x49, staticRules[0x75]), new LRReduction(0x4a, staticRules[0x75]), new LRReduction(0x55, staticRules[0x75]), new LRReduction(0x56, staticRules[0x75]), new LRReduction(0xdd, staticRules[0x75])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0xF0, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x44, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x77]), new LRReduction(0x11, staticRules[0x77]), new LRReduction(0x2e, staticRules[0x77]), new LRReduction(0x30, staticRules[0x77]), new LRReduction(0x41, staticRules[0x77]), new LRReduction(0x43, staticRules[0x77]), new LRReduction(0x44, staticRules[0x77]), new LRReduction(0x45, staticRules[0x77]), new LRReduction(0x46, staticRules[0x77]), new LRReduction(0x47, staticRules[0x77]), new LRReduction(0x48, staticRules[0x77]), new LRReduction(0x49, staticRules[0x77]), new LRReduction(0x4a, staticRules[0x77]), new LRReduction(0x4e, staticRules[0x77]), new LRReduction(0x55, staticRules[0x77]), new LRReduction(0x56, staticRules[0x77]), new LRReduction(0xdd, staticRules[0x77])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x78]), new LRReduction(0x11, staticRules[0x78]), new LRReduction(0x2e, staticRules[0x78]), new LRReduction(0x30, staticRules[0x78]), new LRReduction(0x41, staticRules[0x78]), new LRReduction(0x43, staticRules[0x78]), new LRReduction(0x44, staticRules[0x78]), new LRReduction(0x45, staticRules[0x78]), new LRReduction(0x46, staticRules[0x78]), new LRReduction(0x47, staticRules[0x78]), new LRReduction(0x48, staticRules[0x78]), new LRReduction(0x49, staticRules[0x78]), new LRReduction(0x4a, staticRules[0x78]), new LRReduction(0x4e, staticRules[0x78]), new LRReduction(0x55, staticRules[0x78]), new LRReduction(0x56, staticRules[0x78]), new LRReduction(0xdd, staticRules[0x78])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x79]), new LRReduction(0x11, staticRules[0x79]), new LRReduction(0x2e, staticRules[0x79]), new LRReduction(0x30, staticRules[0x79]), new LRReduction(0x41, staticRules[0x79]), new LRReduction(0x43, staticRules[0x79]), new LRReduction(0x44, staticRules[0x79]), new LRReduction(0x45, staticRules[0x79]), new LRReduction(0x46, staticRules[0x79]), new LRReduction(0x47, staticRules[0x79]), new LRReduction(0x48, staticRules[0x79]), new LRReduction(0x49, staticRules[0x79]), new LRReduction(0x4a, staticRules[0x79]), new LRReduction(0x4e, staticRules[0x79]), new LRReduction(0x55, staticRules[0x79]), new LRReduction(0x56, staticRules[0x79]), new LRReduction(0xdd, staticRules[0x79])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x7A]), new LRReduction(0x11, staticRules[0x7A]), new LRReduction(0x2e, staticRules[0x7A]), new LRReduction(0x30, staticRules[0x7A]), new LRReduction(0x41, staticRules[0x7A]), new LRReduction(0x43, staticRules[0x7A]), new LRReduction(0x44, staticRules[0x7A]), new LRReduction(0x45, staticRules[0x7A]), new LRReduction(0x46, staticRules[0x7A]), new LRReduction(0x47, staticRules[0x7A]), new LRReduction(0x48, staticRules[0x7A]), new LRReduction(0x49, staticRules[0x7A]), new LRReduction(0x4a, staticRules[0x7A]), new LRReduction(0x4e, staticRules[0x7A]), new LRReduction(0x55, staticRules[0x7A]), new LRReduction(0x56, staticRules[0x7A]), new LRReduction(0xdd, staticRules[0x7A])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x7B]), new LRReduction(0x11, staticRules[0x7B]), new LRReduction(0x2e, staticRules[0x7B]), new LRReduction(0x30, staticRules[0x7B]), new LRReduction(0x41, staticRules[0x7B]), new LRReduction(0x43, staticRules[0x7B]), new LRReduction(0x44, staticRules[0x7B]), new LRReduction(0x45, staticRules[0x7B]), new LRReduction(0x46, staticRules[0x7B]), new LRReduction(0x47, staticRules[0x7B]), new LRReduction(0x48, staticRules[0x7B]), new LRReduction(0x49, staticRules[0x7B]), new LRReduction(0x4a, staticRules[0x7B]), new LRReduction(0x4e, staticRules[0x7B]), new LRReduction(0x55, staticRules[0x7B]), new LRReduction(0x56, staticRules[0x7B]), new LRReduction(0xdd, staticRules[0x7B])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[18], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4d},
               new ushort[1] {0xF2},
               new ushort[1] {0x87},
               new ushort[1] {0xF1},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x37]), new LRReduction(0x11, staticRules[0x37]), new LRReduction(0x2e, staticRules[0x37]), new LRReduction(0x30, staticRules[0x37]), new LRReduction(0x41, staticRules[0x37]), new LRReduction(0x43, staticRules[0x37]), new LRReduction(0x44, staticRules[0x37]), new LRReduction(0x45, staticRules[0x37]), new LRReduction(0x46, staticRules[0x37]), new LRReduction(0x47, staticRules[0x37]), new LRReduction(0x48, staticRules[0x37]), new LRReduction(0x49, staticRules[0x37]), new LRReduction(0x4a, staticRules[0x37]), new LRReduction(0x4e, staticRules[0x37]), new LRReduction(0x55, staticRules[0x37]), new LRReduction(0x56, staticRules[0x37]), new LRReduction(0xdd, staticRules[0x37])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x46]), new LRReduction(0x11, staticRules[0x46]), new LRReduction(0x2e, staticRules[0x46]), new LRReduction(0x30, staticRules[0x46]), new LRReduction(0x41, staticRules[0x46]), new LRReduction(0x43, staticRules[0x46]), new LRReduction(0x44, staticRules[0x46]), new LRReduction(0x45, staticRules[0x46]), new LRReduction(0x46, staticRules[0x46]), new LRReduction(0x47, staticRules[0x46]), new LRReduction(0x48, staticRules[0x46]), new LRReduction(0x49, staticRules[0x46]), new LRReduction(0x4a, staticRules[0x46]), new LRReduction(0x4e, staticRules[0x46]), new LRReduction(0x55, staticRules[0x46]), new LRReduction(0x56, staticRules[0x46]), new LRReduction(0xdd, staticRules[0x46])})
            , new LR1State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[24] {new LRReduction(0xa, staticRules[0x16]), new LRReduction(0xb, staticRules[0x16]), new LRReduction(0x11, staticRules[0x16]), new LRReduction(0x2e, staticRules[0x16]), new LRReduction(0x30, staticRules[0x16]), new LRReduction(0x31, staticRules[0x16]), new LRReduction(0x32, staticRules[0x16]), new LRReduction(0x33, staticRules[0x16]), new LRReduction(0x34, staticRules[0x16]), new LRReduction(0x35, staticRules[0x16]), new LRReduction(0x41, staticRules[0x16]), new LRReduction(0x43, staticRules[0x16]), new LRReduction(0x44, staticRules[0x16]), new LRReduction(0x45, staticRules[0x16]), new LRReduction(0x46, staticRules[0x16]), new LRReduction(0x47, staticRules[0x16]), new LRReduction(0x48, staticRules[0x16]), new LRReduction(0x49, staticRules[0x16]), new LRReduction(0x4a, staticRules[0x16]), new LRReduction(0x4b, staticRules[0x16]), new LRReduction(0x4e, staticRules[0x16]), new LRReduction(0x55, staticRules[0x16]), new LRReduction(0x56, staticRules[0x16]), new LRReduction(0xdd, staticRules[0x16])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4b},
               new ushort[1] {0xF4},
               new ushort[1] {0x67},
               new ushort[1] {0xF3},
               new LRReduction[1] {new LRReduction(0x41, staticRules[0x32])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4a},
               new ushort[1] {0xF6},
               new ushort[1] {0x77},
               new ushort[1] {0xF5},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x30]), new LRReduction(0x44, staticRules[0x30]), new LRReduction(0x4b, staticRules[0x30])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x49},
               new ushort[1] {0xF8},
               new ushort[1] {0x76},
               new ushort[1] {0xF7},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x2E]), new LRReduction(0x44, staticRules[0x2E]), new LRReduction(0x4a, staticRules[0x2E]), new LRReduction(0x4b, staticRules[0x2E])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[11] {0x75, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[11] {0xF9, 0xFA, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x2C]), new LRReduction(0x44, staticRules[0x2C]), new LRReduction(0x49, staticRules[0x2C]), new LRReduction(0x4a, staticRules[0x2C]), new LRReduction(0x4b, staticRules[0x2C])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[4] {0x45, 0x46, 0x47, 0x11},
               new ushort[4] {0xFC, 0xFD, 0xFE, 0xFF},
               new ushort[1] {0x62},
               new ushort[1] {0xFB},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x2A]), new LRReduction(0xb, staticRules[0x2A]), new LRReduction(0x30, staticRules[0x2A]), new LRReduction(0x31, staticRules[0x2A]), new LRReduction(0x32, staticRules[0x2A]), new LRReduction(0x33, staticRules[0x2A]), new LRReduction(0x34, staticRules[0x2A]), new LRReduction(0x35, staticRules[0x2A]), new LRReduction(0x41, staticRules[0x2A]), new LRReduction(0x43, staticRules[0x2A]), new LRReduction(0x44, staticRules[0x2A]), new LRReduction(0x49, staticRules[0x2A]), new LRReduction(0x4a, staticRules[0x2A]), new LRReduction(0x4b, staticRules[0x2A])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x23]), new LRReduction(0xb, staticRules[0x23]), new LRReduction(0x11, staticRules[0x23]), new LRReduction(0x30, staticRules[0x23]), new LRReduction(0x31, staticRules[0x23]), new LRReduction(0x32, staticRules[0x23]), new LRReduction(0x33, staticRules[0x23]), new LRReduction(0x34, staticRules[0x23]), new LRReduction(0x35, staticRules[0x23]), new LRReduction(0x41, staticRules[0x23]), new LRReduction(0x43, staticRules[0x23]), new LRReduction(0x44, staticRules[0x23]), new LRReduction(0x45, staticRules[0x23]), new LRReduction(0x46, staticRules[0x23]), new LRReduction(0x47, staticRules[0x23]), new LRReduction(0x49, staticRules[0x23]), new LRReduction(0x4a, staticRules[0x23]), new LRReduction(0x4b, staticRules[0x23])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[13] {0x66, 0x65, 0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[13] {0x100, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1B]), new LRReduction(0xb, staticRules[0x1B]), new LRReduction(0x11, staticRules[0x1B]), new LRReduction(0x30, staticRules[0x1B]), new LRReduction(0x31, staticRules[0x1B]), new LRReduction(0x32, staticRules[0x1B]), new LRReduction(0x33, staticRules[0x1B]), new LRReduction(0x34, staticRules[0x1B]), new LRReduction(0x35, staticRules[0x1B]), new LRReduction(0x41, staticRules[0x1B]), new LRReduction(0x43, staticRules[0x1B]), new LRReduction(0x44, staticRules[0x1B]), new LRReduction(0x45, staticRules[0x1B]), new LRReduction(0x46, staticRules[0x1B]), new LRReduction(0x47, staticRules[0x1B]), new LRReduction(0x49, staticRules[0x1B]), new LRReduction(0x4a, staticRules[0x1B]), new LRReduction(0x4b, staticRules[0x1B])})
            , new LR1State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x42},
               new ushort[1] {0x101},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1C]), new LRReduction(0xb, staticRules[0x1C]), new LRReduction(0x11, staticRules[0x1C]), new LRReduction(0x30, staticRules[0x1C]), new LRReduction(0x31, staticRules[0x1C]), new LRReduction(0x32, staticRules[0x1C]), new LRReduction(0x33, staticRules[0x1C]), new LRReduction(0x34, staticRules[0x1C]), new LRReduction(0x35, staticRules[0x1C]), new LRReduction(0x41, staticRules[0x1C]), new LRReduction(0x43, staticRules[0x1C]), new LRReduction(0x44, staticRules[0x1C]), new LRReduction(0x45, staticRules[0x1C]), new LRReduction(0x46, staticRules[0x1C]), new LRReduction(0x47, staticRules[0x1C]), new LRReduction(0x49, staticRules[0x1C]), new LRReduction(0x4a, staticRules[0x1C]), new LRReduction(0x4b, staticRules[0x1C])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1D]), new LRReduction(0xb, staticRules[0x1D]), new LRReduction(0x11, staticRules[0x1D]), new LRReduction(0x30, staticRules[0x1D]), new LRReduction(0x31, staticRules[0x1D]), new LRReduction(0x32, staticRules[0x1D]), new LRReduction(0x33, staticRules[0x1D]), new LRReduction(0x34, staticRules[0x1D]), new LRReduction(0x35, staticRules[0x1D]), new LRReduction(0x41, staticRules[0x1D]), new LRReduction(0x43, staticRules[0x1D]), new LRReduction(0x44, staticRules[0x1D]), new LRReduction(0x45, staticRules[0x1D]), new LRReduction(0x46, staticRules[0x1D]), new LRReduction(0x47, staticRules[0x1D]), new LRReduction(0x49, staticRules[0x1D]), new LRReduction(0x4a, staticRules[0x1D]), new LRReduction(0x4b, staticRules[0x1D])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1E]), new LRReduction(0xb, staticRules[0x1E]), new LRReduction(0x11, staticRules[0x1E]), new LRReduction(0x30, staticRules[0x1E]), new LRReduction(0x31, staticRules[0x1E]), new LRReduction(0x32, staticRules[0x1E]), new LRReduction(0x33, staticRules[0x1E]), new LRReduction(0x34, staticRules[0x1E]), new LRReduction(0x35, staticRules[0x1E]), new LRReduction(0x41, staticRules[0x1E]), new LRReduction(0x43, staticRules[0x1E]), new LRReduction(0x44, staticRules[0x1E]), new LRReduction(0x45, staticRules[0x1E]), new LRReduction(0x46, staticRules[0x1E]), new LRReduction(0x47, staticRules[0x1E]), new LRReduction(0x49, staticRules[0x1E]), new LRReduction(0x4a, staticRules[0x1E]), new LRReduction(0x4b, staticRules[0x1E])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1F]), new LRReduction(0xb, staticRules[0x1F]), new LRReduction(0x11, staticRules[0x1F]), new LRReduction(0x30, staticRules[0x1F]), new LRReduction(0x31, staticRules[0x1F]), new LRReduction(0x32, staticRules[0x1F]), new LRReduction(0x33, staticRules[0x1F]), new LRReduction(0x34, staticRules[0x1F]), new LRReduction(0x35, staticRules[0x1F]), new LRReduction(0x41, staticRules[0x1F]), new LRReduction(0x43, staticRules[0x1F]), new LRReduction(0x44, staticRules[0x1F]), new LRReduction(0x45, staticRules[0x1F]), new LRReduction(0x46, staticRules[0x1F]), new LRReduction(0x47, staticRules[0x1F]), new LRReduction(0x49, staticRules[0x1F]), new LRReduction(0x4a, staticRules[0x1F]), new LRReduction(0x4b, staticRules[0x1F])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x20]), new LRReduction(0xb, staticRules[0x20]), new LRReduction(0x11, staticRules[0x20]), new LRReduction(0x30, staticRules[0x20]), new LRReduction(0x31, staticRules[0x20]), new LRReduction(0x32, staticRules[0x20]), new LRReduction(0x33, staticRules[0x20]), new LRReduction(0x34, staticRules[0x20]), new LRReduction(0x35, staticRules[0x20]), new LRReduction(0x41, staticRules[0x20]), new LRReduction(0x43, staticRules[0x20]), new LRReduction(0x44, staticRules[0x20]), new LRReduction(0x45, staticRules[0x20]), new LRReduction(0x46, staticRules[0x20]), new LRReduction(0x47, staticRules[0x20]), new LRReduction(0x49, staticRules[0x20]), new LRReduction(0x4a, staticRules[0x20]), new LRReduction(0x4b, staticRules[0x20])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x21]), new LRReduction(0xb, staticRules[0x21]), new LRReduction(0x11, staticRules[0x21]), new LRReduction(0x30, staticRules[0x21]), new LRReduction(0x31, staticRules[0x21]), new LRReduction(0x32, staticRules[0x21]), new LRReduction(0x33, staticRules[0x21]), new LRReduction(0x34, staticRules[0x21]), new LRReduction(0x35, staticRules[0x21]), new LRReduction(0x41, staticRules[0x21]), new LRReduction(0x43, staticRules[0x21]), new LRReduction(0x44, staticRules[0x21]), new LRReduction(0x45, staticRules[0x21]), new LRReduction(0x46, staticRules[0x21]), new LRReduction(0x47, staticRules[0x21]), new LRReduction(0x49, staticRules[0x21]), new LRReduction(0x4a, staticRules[0x21]), new LRReduction(0x4b, staticRules[0x21])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x22]), new LRReduction(0xb, staticRules[0x22]), new LRReduction(0x11, staticRules[0x22]), new LRReduction(0x30, staticRules[0x22]), new LRReduction(0x31, staticRules[0x22]), new LRReduction(0x32, staticRules[0x22]), new LRReduction(0x33, staticRules[0x22]), new LRReduction(0x34, staticRules[0x22]), new LRReduction(0x35, staticRules[0x22]), new LRReduction(0x41, staticRules[0x22]), new LRReduction(0x43, staticRules[0x22]), new LRReduction(0x44, staticRules[0x22]), new LRReduction(0x45, staticRules[0x22]), new LRReduction(0x46, staticRules[0x22]), new LRReduction(0x47, staticRules[0x22]), new LRReduction(0x49, staticRules[0x22]), new LRReduction(0x4a, staticRules[0x22]), new LRReduction(0x4b, staticRules[0x22])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x13]), new LRReduction(0xb, staticRules[0x13]), new LRReduction(0x11, staticRules[0x13]), new LRReduction(0x30, staticRules[0x13]), new LRReduction(0x31, staticRules[0x13]), new LRReduction(0x32, staticRules[0x13]), new LRReduction(0x33, staticRules[0x13]), new LRReduction(0x34, staticRules[0x13]), new LRReduction(0x35, staticRules[0x13]), new LRReduction(0x41, staticRules[0x13]), new LRReduction(0x43, staticRules[0x13]), new LRReduction(0x44, staticRules[0x13]), new LRReduction(0x45, staticRules[0x13]), new LRReduction(0x46, staticRules[0x13]), new LRReduction(0x47, staticRules[0x13]), new LRReduction(0x49, staticRules[0x13]), new LRReduction(0x4a, staticRules[0x13]), new LRReduction(0x4b, staticRules[0x13])})
            , new LR1State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[19] {new LRReduction(0xa, staticRules[0x14]), new LRReduction(0xb, staticRules[0x14]), new LRReduction(0x11, staticRules[0x14]), new LRReduction(0x30, staticRules[0x14]), new LRReduction(0x31, staticRules[0x14]), new LRReduction(0x32, staticRules[0x14]), new LRReduction(0x33, staticRules[0x14]), new LRReduction(0x34, staticRules[0x14]), new LRReduction(0x35, staticRules[0x14]), new LRReduction(0x41, staticRules[0x14]), new LRReduction(0x42, staticRules[0x14]), new LRReduction(0x43, staticRules[0x14]), new LRReduction(0x44, staticRules[0x14]), new LRReduction(0x45, staticRules[0x14]), new LRReduction(0x46, staticRules[0x14]), new LRReduction(0x47, staticRules[0x14]), new LRReduction(0x49, staticRules[0x14]), new LRReduction(0x4a, staticRules[0x14]), new LRReduction(0x4b, staticRules[0x14])})
            , new LR1State(
               null,
               new SymbolTerminal[19] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[25], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[19] {new LRReduction(0xa, staticRules[0x15]), new LRReduction(0xb, staticRules[0x15]), new LRReduction(0x11, staticRules[0x15]), new LRReduction(0x30, staticRules[0x15]), new LRReduction(0x31, staticRules[0x15]), new LRReduction(0x32, staticRules[0x15]), new LRReduction(0x33, staticRules[0x15]), new LRReduction(0x34, staticRules[0x15]), new LRReduction(0x35, staticRules[0x15]), new LRReduction(0x41, staticRules[0x15]), new LRReduction(0x42, staticRules[0x15]), new LRReduction(0x43, staticRules[0x15]), new LRReduction(0x44, staticRules[0x15]), new LRReduction(0x45, staticRules[0x15]), new LRReduction(0x46, staticRules[0x15]), new LRReduction(0x47, staticRules[0x15]), new LRReduction(0x49, staticRules[0x15]), new LRReduction(0x4a, staticRules[0x15]), new LRReduction(0x4b, staticRules[0x15])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x17]), new LRReduction(0xb, staticRules[0x17]), new LRReduction(0x11, staticRules[0x17]), new LRReduction(0x30, staticRules[0x17]), new LRReduction(0x31, staticRules[0x17]), new LRReduction(0x32, staticRules[0x17]), new LRReduction(0x33, staticRules[0x17]), new LRReduction(0x34, staticRules[0x17]), new LRReduction(0x35, staticRules[0x17]), new LRReduction(0x41, staticRules[0x17]), new LRReduction(0x43, staticRules[0x17]), new LRReduction(0x44, staticRules[0x17]), new LRReduction(0x45, staticRules[0x17]), new LRReduction(0x46, staticRules[0x17]), new LRReduction(0x47, staticRules[0x17]), new LRReduction(0x49, staticRules[0x17]), new LRReduction(0x4a, staticRules[0x17]), new LRReduction(0x4b, staticRules[0x17])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x19]), new LRReduction(0xb, staticRules[0x19]), new LRReduction(0x11, staticRules[0x19]), new LRReduction(0x30, staticRules[0x19]), new LRReduction(0x31, staticRules[0x19]), new LRReduction(0x32, staticRules[0x19]), new LRReduction(0x33, staticRules[0x19]), new LRReduction(0x34, staticRules[0x19]), new LRReduction(0x35, staticRules[0x19]), new LRReduction(0x41, staticRules[0x19]), new LRReduction(0x43, staticRules[0x19]), new LRReduction(0x44, staticRules[0x19]), new LRReduction(0x45, staticRules[0x19]), new LRReduction(0x46, staticRules[0x19]), new LRReduction(0x47, staticRules[0x19]), new LRReduction(0x49, staticRules[0x19]), new LRReduction(0x4a, staticRules[0x19]), new LRReduction(0x4b, staticRules[0x19])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x18]), new LRReduction(0xb, staticRules[0x18]), new LRReduction(0x11, staticRules[0x18]), new LRReduction(0x30, staticRules[0x18]), new LRReduction(0x31, staticRules[0x18]), new LRReduction(0x32, staticRules[0x18]), new LRReduction(0x33, staticRules[0x18]), new LRReduction(0x34, staticRules[0x18]), new LRReduction(0x35, staticRules[0x18]), new LRReduction(0x41, staticRules[0x18]), new LRReduction(0x43, staticRules[0x18]), new LRReduction(0x44, staticRules[0x18]), new LRReduction(0x45, staticRules[0x18]), new LRReduction(0x46, staticRules[0x18]), new LRReduction(0x47, staticRules[0x18]), new LRReduction(0x49, staticRules[0x18]), new LRReduction(0x4a, staticRules[0x18]), new LRReduction(0x4b, staticRules[0x18])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x102, 0x99},
               new ushort[1] {0x6c},
               new ushort[1] {0x103},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC5]), new LRReduction(0x4c, staticRules[0xC5]), new LRReduction(0x4d, staticRules[0xC5])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0x104},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x90]), new LRReduction(0x44, staticRules[0x90]), new LRReduction(0xdd, staticRules[0x90])})
            , new LR1State(
               null,
               new SymbolTerminal[20] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[12] {0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[12] {0x105, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x44, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92]), new LRReduction(0xdd, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0x106},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x94]), new LRReduction(0x44, staticRules[0x94]), new LRReduction(0x4a, staticRules[0x94]), new LRReduction(0xdd, staticRules[0x94])})
            , new LR1State(
               null,
               new SymbolTerminal[16] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[10] {0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[10] {0x107, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[9] {0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[9] {0x108, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x96]), new LRReduction(0x44, staticRules[0x96]), new LRReduction(0x49, staticRules[0x96]), new LRReduction(0x4a, staticRules[0x96]), new LRReduction(0xdd, staticRules[0x96])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0xAA]), new LRReduction(0x11, staticRules[0xAA]), new LRReduction(0x2e, staticRules[0xAA]), new LRReduction(0x34, staticRules[0xAA]), new LRReduction(0x35, staticRules[0xAA]), new LRReduction(0x36, staticRules[0xAA]), new LRReduction(0x37, staticRules[0xAA]), new LRReduction(0x38, staticRules[0xAA]), new LRReduction(0x39, staticRules[0xAA]), new LRReduction(0x3a, staticRules[0xAA]), new LRReduction(0x3b, staticRules[0xAA]), new LRReduction(0x3c, staticRules[0xAA]), new LRReduction(0x3d, staticRules[0xAA]), new LRReduction(0x3e, staticRules[0xAA]), new LRReduction(0x3f, staticRules[0xAA]), new LRReduction(0x41, staticRules[0xAA]), new LRReduction(0x43, staticRules[0xAA]), new LRReduction(0x44, staticRules[0xAA]), new LRReduction(0x49, staticRules[0xAA]), new LRReduction(0x4a, staticRules[0xAA]), new LRReduction(0xdd, staticRules[0xAA])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0x98]), new LRReduction(0x11, staticRules[0x98]), new LRReduction(0x2e, staticRules[0x98]), new LRReduction(0x34, staticRules[0x98]), new LRReduction(0x35, staticRules[0x98]), new LRReduction(0x36, staticRules[0x98]), new LRReduction(0x37, staticRules[0x98]), new LRReduction(0x38, staticRules[0x98]), new LRReduction(0x39, staticRules[0x98]), new LRReduction(0x3a, staticRules[0x98]), new LRReduction(0x3b, staticRules[0x98]), new LRReduction(0x3c, staticRules[0x98]), new LRReduction(0x3d, staticRules[0x98]), new LRReduction(0x3e, staticRules[0x98]), new LRReduction(0x3f, staticRules[0x98]), new LRReduction(0x41, staticRules[0x98]), new LRReduction(0x43, staticRules[0x98]), new LRReduction(0x44, staticRules[0x98]), new LRReduction(0x49, staticRules[0x98]), new LRReduction(0x4a, staticRules[0x98]), new LRReduction(0xdd, staticRules[0x98])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0x99]), new LRReduction(0x11, staticRules[0x99]), new LRReduction(0x2e, staticRules[0x99]), new LRReduction(0x34, staticRules[0x99]), new LRReduction(0x35, staticRules[0x99]), new LRReduction(0x36, staticRules[0x99]), new LRReduction(0x37, staticRules[0x99]), new LRReduction(0x38, staticRules[0x99]), new LRReduction(0x39, staticRules[0x99]), new LRReduction(0x3a, staticRules[0x99]), new LRReduction(0x3b, staticRules[0x99]), new LRReduction(0x3c, staticRules[0x99]), new LRReduction(0x3d, staticRules[0x99]), new LRReduction(0x3e, staticRules[0x99]), new LRReduction(0x3f, staticRules[0x99]), new LRReduction(0x41, staticRules[0x99]), new LRReduction(0x43, staticRules[0x99]), new LRReduction(0x44, staticRules[0x99]), new LRReduction(0x49, staticRules[0x99]), new LRReduction(0x4a, staticRules[0x99]), new LRReduction(0xdd, staticRules[0x99])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0x9A]), new LRReduction(0x11, staticRules[0x9A]), new LRReduction(0x2e, staticRules[0x9A]), new LRReduction(0x34, staticRules[0x9A]), new LRReduction(0x35, staticRules[0x9A]), new LRReduction(0x36, staticRules[0x9A]), new LRReduction(0x37, staticRules[0x9A]), new LRReduction(0x38, staticRules[0x9A]), new LRReduction(0x39, staticRules[0x9A]), new LRReduction(0x3a, staticRules[0x9A]), new LRReduction(0x3b, staticRules[0x9A]), new LRReduction(0x3c, staticRules[0x9A]), new LRReduction(0x3d, staticRules[0x9A]), new LRReduction(0x3e, staticRules[0x9A]), new LRReduction(0x3f, staticRules[0x9A]), new LRReduction(0x41, staticRules[0x9A]), new LRReduction(0x43, staticRules[0x9A]), new LRReduction(0x44, staticRules[0x9A]), new LRReduction(0x49, staticRules[0x9A]), new LRReduction(0x4a, staticRules[0x9A]), new LRReduction(0xdd, staticRules[0x9A])})
            , new LR1State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[24] {new LRReduction(0xa, staticRules[0x9C]), new LRReduction(0x11, staticRules[0x9C]), new LRReduction(0x2e, staticRules[0x9C]), new LRReduction(0x34, staticRules[0x9C]), new LRReduction(0x35, staticRules[0x9C]), new LRReduction(0x36, staticRules[0x9C]), new LRReduction(0x37, staticRules[0x9C]), new LRReduction(0x38, staticRules[0x9C]), new LRReduction(0x39, staticRules[0x9C]), new LRReduction(0x3a, staticRules[0x9C]), new LRReduction(0x3b, staticRules[0x9C]), new LRReduction(0x3c, staticRules[0x9C]), new LRReduction(0x3d, staticRules[0x9C]), new LRReduction(0x3e, staticRules[0x9C]), new LRReduction(0x3f, staticRules[0x9C]), new LRReduction(0x41, staticRules[0x9C]), new LRReduction(0x43, staticRules[0x9C]), new LRReduction(0x44, staticRules[0x9C]), new LRReduction(0x45, staticRules[0x9C]), new LRReduction(0x46, staticRules[0x9C]), new LRReduction(0x47, staticRules[0x9C]), new LRReduction(0x49, staticRules[0x9C]), new LRReduction(0x4a, staticRules[0x9C]), new LRReduction(0xdd, staticRules[0x9C])})
            , new LR1State(
               null,
               new SymbolTerminal[24] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[24] {new LRReduction(0xa, staticRules[0x9D]), new LRReduction(0x11, staticRules[0x9D]), new LRReduction(0x2e, staticRules[0x9D]), new LRReduction(0x34, staticRules[0x9D]), new LRReduction(0x35, staticRules[0x9D]), new LRReduction(0x36, staticRules[0x9D]), new LRReduction(0x37, staticRules[0x9D]), new LRReduction(0x38, staticRules[0x9D]), new LRReduction(0x39, staticRules[0x9D]), new LRReduction(0x3a, staticRules[0x9D]), new LRReduction(0x3b, staticRules[0x9D]), new LRReduction(0x3c, staticRules[0x9D]), new LRReduction(0x3d, staticRules[0x9D]), new LRReduction(0x3e, staticRules[0x9D]), new LRReduction(0x3f, staticRules[0x9D]), new LRReduction(0x41, staticRules[0x9D]), new LRReduction(0x43, staticRules[0x9D]), new LRReduction(0x44, staticRules[0x9D]), new LRReduction(0x45, staticRules[0x9D]), new LRReduction(0x46, staticRules[0x9D]), new LRReduction(0x47, staticRules[0x9D]), new LRReduction(0x49, staticRules[0x9D]), new LRReduction(0x4a, staticRules[0x9D]), new LRReduction(0xdd, staticRules[0x9D])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0x109},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x10A},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA5]), new LRReduction(0x11, staticRules[0xA5]), new LRReduction(0x2e, staticRules[0xA5]), new LRReduction(0x34, staticRules[0xA5]), new LRReduction(0x35, staticRules[0xA5]), new LRReduction(0x36, staticRules[0xA5]), new LRReduction(0x37, staticRules[0xA5]), new LRReduction(0x38, staticRules[0xA5]), new LRReduction(0x39, staticRules[0xA5]), new LRReduction(0x3a, staticRules[0xA5]), new LRReduction(0x3b, staticRules[0xA5]), new LRReduction(0x3c, staticRules[0xA5]), new LRReduction(0x3d, staticRules[0xA5]), new LRReduction(0x3e, staticRules[0xA5]), new LRReduction(0x3f, staticRules[0xA5]), new LRReduction(0x41, staticRules[0xA5]), new LRReduction(0x43, staticRules[0xA5]), new LRReduction(0x44, staticRules[0xA5]), new LRReduction(0x45, staticRules[0xA5]), new LRReduction(0x46, staticRules[0xA5]), new LRReduction(0x47, staticRules[0xA5]), new LRReduction(0x48, staticRules[0xA5]), new LRReduction(0x49, staticRules[0xA5]), new LRReduction(0x4a, staticRules[0xA5]), new LRReduction(0x4e, staticRules[0xA5]), new LRReduction(0x55, staticRules[0xA5]), new LRReduction(0x56, staticRules[0xA5]), new LRReduction(0xdd, staticRules[0xA5])})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35]},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[6] {0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[6] {0x10B, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x12]), new LRReduction(0x12, staticRules[0x12])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x10C},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0x10D, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x10E, 0x110},
               new ushort[1] {0x78},
               new ushort[1] {0x10F},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x12, staticRules[0x8D])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0xB3]), new LRReduction(0x12, staticRules[0xB3])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0xB4]), new LRReduction(0x12, staticRules[0xB4])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0x111, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x112},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[2] {0x4c, 0x4d},
               new ushort[2] {0x113, 0x99},
               new ushort[1] {0x6c},
               new ushort[1] {0x114},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[29], FileCentralDogma_Lexer.terminals[18]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xBB]), new LRReduction(0x4c, staticRules[0xBB]), new LRReduction(0x4d, staticRules[0xBB])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x4a},
               new ushort[1] {0x115},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x67]), new LRReduction(0x44, staticRules[0x67]), new LRReduction(0xdd, staticRules[0x67])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[13] {0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[13] {0x116, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x44, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69]), new LRReduction(0xdd, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[1] {0x49},
               new ushort[1] {0x117},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x6B]), new LRReduction(0x44, staticRules[0x6B]), new LRReduction(0x4a, staticRules[0x6B]), new LRReduction(0xdd, staticRules[0x6B])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[11] {0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[11] {0x118, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[10] {0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[10] {0x119, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x6D]), new LRReduction(0x44, staticRules[0x6D]), new LRReduction(0x49, staticRules[0x6D]), new LRReduction(0x4a, staticRules[0x6D]), new LRReduction(0xdd, staticRules[0x6D])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x81]), new LRReduction(0x11, staticRules[0x81]), new LRReduction(0x2e, staticRules[0x81]), new LRReduction(0x30, staticRules[0x81]), new LRReduction(0x41, staticRules[0x81]), new LRReduction(0x43, staticRules[0x81]), new LRReduction(0x44, staticRules[0x81]), new LRReduction(0x49, staticRules[0x81]), new LRReduction(0x4a, staticRules[0x81]), new LRReduction(0xdd, staticRules[0x81])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x6F]), new LRReduction(0x11, staticRules[0x6F]), new LRReduction(0x2e, staticRules[0x6F]), new LRReduction(0x30, staticRules[0x6F]), new LRReduction(0x41, staticRules[0x6F]), new LRReduction(0x43, staticRules[0x6F]), new LRReduction(0x44, staticRules[0x6F]), new LRReduction(0x49, staticRules[0x6F]), new LRReduction(0x4a, staticRules[0x6F]), new LRReduction(0xdd, staticRules[0x6F])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x70]), new LRReduction(0x11, staticRules[0x70]), new LRReduction(0x2e, staticRules[0x70]), new LRReduction(0x30, staticRules[0x70]), new LRReduction(0x41, staticRules[0x70]), new LRReduction(0x43, staticRules[0x70]), new LRReduction(0x44, staticRules[0x70]), new LRReduction(0x49, staticRules[0x70]), new LRReduction(0x4a, staticRules[0x70]), new LRReduction(0xdd, staticRules[0x70])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x71]), new LRReduction(0x11, staticRules[0x71]), new LRReduction(0x2e, staticRules[0x71]), new LRReduction(0x30, staticRules[0x71]), new LRReduction(0x41, staticRules[0x71]), new LRReduction(0x43, staticRules[0x71]), new LRReduction(0x44, staticRules[0x71]), new LRReduction(0x49, staticRules[0x71]), new LRReduction(0x4a, staticRules[0x71]), new LRReduction(0xdd, staticRules[0x71])})
            , new LR1State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[13] {new LRReduction(0xa, staticRules[0x73]), new LRReduction(0x11, staticRules[0x73]), new LRReduction(0x2e, staticRules[0x73]), new LRReduction(0x30, staticRules[0x73]), new LRReduction(0x41, staticRules[0x73]), new LRReduction(0x43, staticRules[0x73]), new LRReduction(0x44, staticRules[0x73]), new LRReduction(0x45, staticRules[0x73]), new LRReduction(0x46, staticRules[0x73]), new LRReduction(0x47, staticRules[0x73]), new LRReduction(0x49, staticRules[0x73]), new LRReduction(0x4a, staticRules[0x73]), new LRReduction(0xdd, staticRules[0x73])})
            , new LR1State(
               null,
               new SymbolTerminal[13] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[13] {new LRReduction(0xa, staticRules[0x74]), new LRReduction(0x11, staticRules[0x74]), new LRReduction(0x2e, staticRules[0x74]), new LRReduction(0x30, staticRules[0x74]), new LRReduction(0x41, staticRules[0x74]), new LRReduction(0x43, staticRules[0x74]), new LRReduction(0x44, staticRules[0x74]), new LRReduction(0x45, staticRules[0x74]), new LRReduction(0x46, staticRules[0x74]), new LRReduction(0x47, staticRules[0x74]), new LRReduction(0x49, staticRules[0x74]), new LRReduction(0x4a, staticRules[0x74]), new LRReduction(0xdd, staticRules[0x74])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0x11A},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x7C]), new LRReduction(0x11, staticRules[0x7C]), new LRReduction(0x2e, staticRules[0x7C]), new LRReduction(0x30, staticRules[0x7C]), new LRReduction(0x41, staticRules[0x7C]), new LRReduction(0x43, staticRules[0x7C]), new LRReduction(0x44, staticRules[0x7C]), new LRReduction(0x45, staticRules[0x7C]), new LRReduction(0x46, staticRules[0x7C]), new LRReduction(0x47, staticRules[0x7C]), new LRReduction(0x48, staticRules[0x7C]), new LRReduction(0x49, staticRules[0x7C]), new LRReduction(0x4a, staticRules[0x7C]), new LRReduction(0x4e, staticRules[0x7C]), new LRReduction(0x55, staticRules[0x7C]), new LRReduction(0x56, staticRules[0x7C]), new LRReduction(0xdd, staticRules[0x7C])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32]},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x84, 0x85, 0xB1, 0xB3},
               new ushort[7] {0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[7] {0x11B, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x11C},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x18},
               new ushort[1] {0x13},
               new ushort[1] {0x11D},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x4a},
               new ushort[1] {0x11E},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0x41, staticRules[0x31]), new LRReduction(0x44, staticRules[0x31]), new LRReduction(0x4b, staticRules[0x31])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[12] {0x65, 0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[12] {0x11F, 0xB6, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[1] {0x49},
               new ushort[1] {0x120},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x2F]), new LRReduction(0x44, staticRules[0x2F]), new LRReduction(0x4a, staticRules[0x2F]), new LRReduction(0x4b, staticRules[0x2F])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[11] {0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[11] {0x121, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[10] {0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[10] {0x122, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x2D]), new LRReduction(0x44, staticRules[0x2D]), new LRReduction(0x49, staticRules[0x2D]), new LRReduction(0x4a, staticRules[0x2D]), new LRReduction(0x4b, staticRules[0x2D])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x55]), new LRReduction(0xb, staticRules[0x55]), new LRReduction(0x30, staticRules[0x55]), new LRReduction(0x31, staticRules[0x55]), new LRReduction(0x32, staticRules[0x55]), new LRReduction(0x33, staticRules[0x55]), new LRReduction(0x34, staticRules[0x55]), new LRReduction(0x35, staticRules[0x55]), new LRReduction(0x41, staticRules[0x55]), new LRReduction(0x43, staticRules[0x55]), new LRReduction(0x44, staticRules[0x55]), new LRReduction(0x49, staticRules[0x55]), new LRReduction(0x4a, staticRules[0x55]), new LRReduction(0x4b, staticRules[0x55])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x2B]), new LRReduction(0xb, staticRules[0x2B]), new LRReduction(0x30, staticRules[0x2B]), new LRReduction(0x31, staticRules[0x2B]), new LRReduction(0x32, staticRules[0x2B]), new LRReduction(0x33, staticRules[0x2B]), new LRReduction(0x34, staticRules[0x2B]), new LRReduction(0x35, staticRules[0x2B]), new LRReduction(0x41, staticRules[0x2B]), new LRReduction(0x43, staticRules[0x2B]), new LRReduction(0x44, staticRules[0x2B]), new LRReduction(0x49, staticRules[0x2B]), new LRReduction(0x4a, staticRules[0x2B]), new LRReduction(0x4b, staticRules[0x2B])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x25]), new LRReduction(0xb, staticRules[0x25]), new LRReduction(0x30, staticRules[0x25]), new LRReduction(0x31, staticRules[0x25]), new LRReduction(0x32, staticRules[0x25]), new LRReduction(0x33, staticRules[0x25]), new LRReduction(0x34, staticRules[0x25]), new LRReduction(0x35, staticRules[0x25]), new LRReduction(0x41, staticRules[0x25]), new LRReduction(0x43, staticRules[0x25]), new LRReduction(0x44, staticRules[0x25]), new LRReduction(0x49, staticRules[0x25]), new LRReduction(0x4a, staticRules[0x25]), new LRReduction(0x4b, staticRules[0x25])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x26]), new LRReduction(0xb, staticRules[0x26]), new LRReduction(0x30, staticRules[0x26]), new LRReduction(0x31, staticRules[0x26]), new LRReduction(0x32, staticRules[0x26]), new LRReduction(0x33, staticRules[0x26]), new LRReduction(0x34, staticRules[0x26]), new LRReduction(0x35, staticRules[0x26]), new LRReduction(0x41, staticRules[0x26]), new LRReduction(0x43, staticRules[0x26]), new LRReduction(0x44, staticRules[0x26]), new LRReduction(0x49, staticRules[0x26]), new LRReduction(0x4a, staticRules[0x26]), new LRReduction(0x4b, staticRules[0x26])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x27]), new LRReduction(0xb, staticRules[0x27]), new LRReduction(0x30, staticRules[0x27]), new LRReduction(0x31, staticRules[0x27]), new LRReduction(0x32, staticRules[0x27]), new LRReduction(0x33, staticRules[0x27]), new LRReduction(0x34, staticRules[0x27]), new LRReduction(0x35, staticRules[0x27]), new LRReduction(0x41, staticRules[0x27]), new LRReduction(0x43, staticRules[0x27]), new LRReduction(0x44, staticRules[0x27]), new LRReduction(0x49, staticRules[0x27]), new LRReduction(0x4a, staticRules[0x27]), new LRReduction(0x4b, staticRules[0x27])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[7]},
               new ushort[1] {0x2d},
               new ushort[1] {0x123},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[11]},
               new ushort[1] {0x44},
               new ushort[1] {0x124},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43]},
               new ushort[2] {0x34, 0x35},
               new ushort[2] {0xC4, 0xC5},
               new ushort[1] {0x5a},
               new ushort[1] {0x125},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0x126, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x127},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[20] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[12] {0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[12] {0x128, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x44, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92]), new LRReduction(0xdd, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0xAE]), new LRReduction(0x44, staticRules[0xAE]), new LRReduction(0x4a, staticRules[0xAE]), new LRReduction(0xdd, staticRules[0xAE])})
            , new LR1State(
               null,
               new SymbolTerminal[16] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[10]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[10] {0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[10] {0x129, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0xAC]), new LRReduction(0x44, staticRules[0xAC]), new LRReduction(0x49, staticRules[0xAC]), new LRReduction(0x4a, staticRules[0xAC]), new LRReduction(0xdd, staticRules[0xAC])})
            , new LR1State(
               null,
               new SymbolTerminal[21] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[21] {new LRReduction(0xa, staticRules[0xAB]), new LRReduction(0x11, staticRules[0xAB]), new LRReduction(0x2e, staticRules[0xAB]), new LRReduction(0x34, staticRules[0xAB]), new LRReduction(0x35, staticRules[0xAB]), new LRReduction(0x36, staticRules[0xAB]), new LRReduction(0x37, staticRules[0xAB]), new LRReduction(0x38, staticRules[0xAB]), new LRReduction(0x39, staticRules[0xAB]), new LRReduction(0x3a, staticRules[0xAB]), new LRReduction(0x3b, staticRules[0xAB]), new LRReduction(0x3c, staticRules[0xAB]), new LRReduction(0x3d, staticRules[0xAB]), new LRReduction(0x3e, staticRules[0xAB]), new LRReduction(0x3f, staticRules[0xAB]), new LRReduction(0x41, staticRules[0xAB]), new LRReduction(0x43, staticRules[0xAB]), new LRReduction(0x44, staticRules[0xAB]), new LRReduction(0x49, staticRules[0xAB]), new LRReduction(0x4a, staticRules[0xAB]), new LRReduction(0xdd, staticRules[0xAB])})
            , new LR1State(
               null,
               new SymbolTerminal[26] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[26] {new LRReduction(0xa, staticRules[0x9F]), new LRReduction(0x11, staticRules[0x9F]), new LRReduction(0x2e, staticRules[0x9F]), new LRReduction(0x34, staticRules[0x9F]), new LRReduction(0x35, staticRules[0x9F]), new LRReduction(0x36, staticRules[0x9F]), new LRReduction(0x37, staticRules[0x9F]), new LRReduction(0x38, staticRules[0x9F]), new LRReduction(0x39, staticRules[0x9F]), new LRReduction(0x3a, staticRules[0x9F]), new LRReduction(0x3b, staticRules[0x9F]), new LRReduction(0x3c, staticRules[0x9F]), new LRReduction(0x3d, staticRules[0x9F]), new LRReduction(0x3e, staticRules[0x9F]), new LRReduction(0x3f, staticRules[0x9F]), new LRReduction(0x41, staticRules[0x9F]), new LRReduction(0x43, staticRules[0x9F]), new LRReduction(0x44, staticRules[0x9F]), new LRReduction(0x45, staticRules[0x9F]), new LRReduction(0x46, staticRules[0x9F]), new LRReduction(0x47, staticRules[0x9F]), new LRReduction(0x49, staticRules[0x9F]), new LRReduction(0x4a, staticRules[0x9F]), new LRReduction(0x55, staticRules[0x9F]), new LRReduction(0x56, staticRules[0x9F]), new LRReduction(0xdd, staticRules[0x9F])})
            , new LR1State(
               null,
               new SymbolTerminal[29] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[29] {new LRReduction(0xa, staticRules[0x35]), new LRReduction(0x11, staticRules[0x35]), new LRReduction(0x2e, staticRules[0x35]), new LRReduction(0x30, staticRules[0x35]), new LRReduction(0x34, staticRules[0x35]), new LRReduction(0x35, staticRules[0x35]), new LRReduction(0x36, staticRules[0x35]), new LRReduction(0x37, staticRules[0x35]), new LRReduction(0x38, staticRules[0x35]), new LRReduction(0x39, staticRules[0x35]), new LRReduction(0x3a, staticRules[0x35]), new LRReduction(0x3b, staticRules[0x35]), new LRReduction(0x3c, staticRules[0x35]), new LRReduction(0x3d, staticRules[0x35]), new LRReduction(0x3e, staticRules[0x35]), new LRReduction(0x3f, staticRules[0x35]), new LRReduction(0x41, staticRules[0x35]), new LRReduction(0x43, staticRules[0x35]), new LRReduction(0x44, staticRules[0x35]), new LRReduction(0x45, staticRules[0x35]), new LRReduction(0x46, staticRules[0x35]), new LRReduction(0x47, staticRules[0x35]), new LRReduction(0x48, staticRules[0x35]), new LRReduction(0x49, staticRules[0x35]), new LRReduction(0x4a, staticRules[0x35]), new LRReduction(0x4e, staticRules[0x35]), new LRReduction(0x55, staticRules[0x35]), new LRReduction(0x56, staticRules[0x35]), new LRReduction(0xdd, staticRules[0x35])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x12A, 0x12C},
               new ushort[1] {0x9a},
               new ushort[1] {0x12B},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x65]), new LRReduction(0x12, staticRules[0x65])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x12D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x4c, staticRules[0x38])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x12E, 0x12F},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x130},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x131},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0x132, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0x133, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[1] {0x4c},
               new ushort[1] {0x134},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[13] {0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[13] {0x135, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x44, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69]), new LRReduction(0xdd, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x85]), new LRReduction(0x44, staticRules[0x85]), new LRReduction(0x4a, staticRules[0x85]), new LRReduction(0xdd, staticRules[0x85])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[10]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[11] {0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[11] {0x136, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x83]), new LRReduction(0x44, staticRules[0x83]), new LRReduction(0x49, staticRules[0x83]), new LRReduction(0x4a, staticRules[0x83]), new LRReduction(0xdd, staticRules[0x83])})
            , new LR1State(
               null,
               new SymbolTerminal[10] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[10] {new LRReduction(0xa, staticRules[0x82]), new LRReduction(0x11, staticRules[0x82]), new LRReduction(0x2e, staticRules[0x82]), new LRReduction(0x30, staticRules[0x82]), new LRReduction(0x41, staticRules[0x82]), new LRReduction(0x43, staticRules[0x82]), new LRReduction(0x44, staticRules[0x82]), new LRReduction(0x49, staticRules[0x82]), new LRReduction(0x4a, staticRules[0x82]), new LRReduction(0xdd, staticRules[0x82])})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[15] {new LRReduction(0xa, staticRules[0x76]), new LRReduction(0x11, staticRules[0x76]), new LRReduction(0x2e, staticRules[0x76]), new LRReduction(0x30, staticRules[0x76]), new LRReduction(0x41, staticRules[0x76]), new LRReduction(0x43, staticRules[0x76]), new LRReduction(0x44, staticRules[0x76]), new LRReduction(0x45, staticRules[0x76]), new LRReduction(0x46, staticRules[0x76]), new LRReduction(0x47, staticRules[0x76]), new LRReduction(0x49, staticRules[0x76]), new LRReduction(0x4a, staticRules[0x76]), new LRReduction(0x55, staticRules[0x76]), new LRReduction(0x56, staticRules[0x76]), new LRReduction(0xdd, staticRules[0x76])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x137, 0x139},
               new ushort[1] {0x88},
               new ushort[1] {0x138},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x34]), new LRReduction(0x12, staticRules[0x34])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x41, staticRules[0x33])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[12] {0x65, 0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[12] {0x13A, 0xB6, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x59]), new LRReduction(0x44, staticRules[0x59]), new LRReduction(0x4a, staticRules[0x59]), new LRReduction(0x4b, staticRules[0x59])})
            , new LR1State(
               null,
               new SymbolTerminal[9] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[10]},
               new ushort[9] {0x43, 0xa, 0xb, 0x34, 0x35, 0x30, 0x31, 0x33, 0x32},
               new ushort[9] {0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xB3, 0xC6, 0xC7, 0xC8},
               new ushort[11] {0x64, 0x63, 0x61, 0x60, 0x59, 0x5a, 0x5b, 0x5c, 0x5f, 0x5e, 0x5d},
               new ushort[11] {0x13B, 0xB7, 0xB8, 0xB9, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x57]), new LRReduction(0x44, staticRules[0x57]), new LRReduction(0x49, staticRules[0x57]), new LRReduction(0x4a, staticRules[0x57]), new LRReduction(0x4b, staticRules[0x57])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x56]), new LRReduction(0xb, staticRules[0x56]), new LRReduction(0x30, staticRules[0x56]), new LRReduction(0x31, staticRules[0x56]), new LRReduction(0x32, staticRules[0x56]), new LRReduction(0x33, staticRules[0x56]), new LRReduction(0x34, staticRules[0x56]), new LRReduction(0x35, staticRules[0x56]), new LRReduction(0x41, staticRules[0x56]), new LRReduction(0x43, staticRules[0x56]), new LRReduction(0x44, staticRules[0x56]), new LRReduction(0x49, staticRules[0x56]), new LRReduction(0x4a, staticRules[0x56]), new LRReduction(0x4b, staticRules[0x56])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[15]},
               new ushort[2] {0x12, 0x48},
               new ushort[2] {0x13C, 0x13D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x24]), new LRReduction(0xb, staticRules[0x24]), new LRReduction(0x11, staticRules[0x24]), new LRReduction(0x30, staticRules[0x24]), new LRReduction(0x31, staticRules[0x24]), new LRReduction(0x32, staticRules[0x24]), new LRReduction(0x33, staticRules[0x24]), new LRReduction(0x34, staticRules[0x24]), new LRReduction(0x35, staticRules[0x24]), new LRReduction(0x41, staticRules[0x24]), new LRReduction(0x43, staticRules[0x24]), new LRReduction(0x44, staticRules[0x24]), new LRReduction(0x45, staticRules[0x24]), new LRReduction(0x46, staticRules[0x24]), new LRReduction(0x47, staticRules[0x24]), new LRReduction(0x49, staticRules[0x24]), new LRReduction(0x4a, staticRules[0x24]), new LRReduction(0x4b, staticRules[0x24])})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[18] {new LRReduction(0xa, staticRules[0x1A]), new LRReduction(0xb, staticRules[0x1A]), new LRReduction(0x11, staticRules[0x1A]), new LRReduction(0x30, staticRules[0x1A]), new LRReduction(0x31, staticRules[0x1A]), new LRReduction(0x32, staticRules[0x1A]), new LRReduction(0x33, staticRules[0x1A]), new LRReduction(0x34, staticRules[0x1A]), new LRReduction(0x35, staticRules[0x1A]), new LRReduction(0x41, staticRules[0x1A]), new LRReduction(0x43, staticRules[0x1A]), new LRReduction(0x44, staticRules[0x1A]), new LRReduction(0x45, staticRules[0x1A]), new LRReduction(0x46, staticRules[0x1A]), new LRReduction(0x47, staticRules[0x1A]), new LRReduction(0x49, staticRules[0x1A]), new LRReduction(0x4a, staticRules[0x1A]), new LRReduction(0x4b, staticRules[0x1A])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x13E},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[18] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[16] {0x43, 0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[16] {0x7E, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[13] {0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[13] {0x13F, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x92]), new LRReduction(0x4a, staticRules[0x92])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0xAF]), new LRReduction(0x44, staticRules[0xAF]), new LRReduction(0x4a, staticRules[0xAF]), new LRReduction(0xdd, staticRules[0xAF])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0xAD]), new LRReduction(0x44, staticRules[0xAD]), new LRReduction(0x49, staticRules[0xAD]), new LRReduction(0x4a, staticRules[0xAD]), new LRReduction(0xdd, staticRules[0xAD])})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA6]), new LRReduction(0x11, staticRules[0xA6]), new LRReduction(0x2e, staticRules[0xA6]), new LRReduction(0x34, staticRules[0xA6]), new LRReduction(0x35, staticRules[0xA6]), new LRReduction(0x36, staticRules[0xA6]), new LRReduction(0x37, staticRules[0xA6]), new LRReduction(0x38, staticRules[0xA6]), new LRReduction(0x39, staticRules[0xA6]), new LRReduction(0x3a, staticRules[0xA6]), new LRReduction(0x3b, staticRules[0xA6]), new LRReduction(0x3c, staticRules[0xA6]), new LRReduction(0x3d, staticRules[0xA6]), new LRReduction(0x3e, staticRules[0xA6]), new LRReduction(0x3f, staticRules[0xA6]), new LRReduction(0x41, staticRules[0xA6]), new LRReduction(0x43, staticRules[0xA6]), new LRReduction(0x44, staticRules[0xA6]), new LRReduction(0x45, staticRules[0xA6]), new LRReduction(0x46, staticRules[0xA6]), new LRReduction(0x47, staticRules[0xA6]), new LRReduction(0x48, staticRules[0xA6]), new LRReduction(0x49, staticRules[0xA6]), new LRReduction(0x4a, staticRules[0xA6]), new LRReduction(0x4e, staticRules[0xA6]), new LRReduction(0x55, staticRules[0xA6]), new LRReduction(0x56, staticRules[0xA6]), new LRReduction(0xdd, staticRules[0xA6])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x140, 0x141},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35]},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[6] {0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[6] {0x142, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x87]), new LRReduction(0x12, staticRules[0x87])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[29]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[1] {new LRReduction(0x4c, staticRules[0x39])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[3]},
               new ushort[1] {0xa},
               new ushort[1] {0x143},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0x5B]), new LRReduction(0x4e, staticRules[0x5B])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0x8E]), new LRReduction(0x12, staticRules[0x8E])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x144},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x145},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[7] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[17]},
               new ushort[5] {0x43, 0x11, 0x2e, 0xa, 0x30},
               new ushort[5] {0xAB, 0x84, 0x85, 0xB1, 0xB3},
               new ushort[14] {0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[14] {0x146, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[2] {new LRReduction(0x41, staticRules[0x69]), new LRReduction(0x4a, staticRules[0x69])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x86]), new LRReduction(0x44, staticRules[0x86]), new LRReduction(0x4a, staticRules[0x86]), new LRReduction(0xdd, staticRules[0x86])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x84]), new LRReduction(0x44, staticRules[0x84]), new LRReduction(0x49, staticRules[0x84]), new LRReduction(0x4a, staticRules[0x84]), new LRReduction(0xdd, staticRules[0x84])})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x7D]), new LRReduction(0x11, staticRules[0x7D]), new LRReduction(0x2e, staticRules[0x7D]), new LRReduction(0x30, staticRules[0x7D]), new LRReduction(0x41, staticRules[0x7D]), new LRReduction(0x43, staticRules[0x7D]), new LRReduction(0x44, staticRules[0x7D]), new LRReduction(0x45, staticRules[0x7D]), new LRReduction(0x46, staticRules[0x7D]), new LRReduction(0x47, staticRules[0x7D]), new LRReduction(0x48, staticRules[0x7D]), new LRReduction(0x49, staticRules[0x7D]), new LRReduction(0x4a, staticRules[0x7D]), new LRReduction(0x4e, staticRules[0x7D]), new LRReduction(0x55, staticRules[0x7D]), new LRReduction(0x56, staticRules[0x7D]), new LRReduction(0xdd, staticRules[0x7D])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[2] {0x4e, 0x48},
               new ushort[2] {0x147, 0x148},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32]},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x84, 0x85, 0xB1, 0xB3},
               new ushort[7] {0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[7] {0x149, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[4] {new LRReduction(0x41, staticRules[0x5A]), new LRReduction(0x44, staticRules[0x5A]), new LRReduction(0x4a, staticRules[0x5A]), new LRReduction(0x4b, staticRules[0x5A])})
            , new LR1State(
               null,
               new SymbolTerminal[5] {FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[5] {new LRReduction(0x41, staticRules[0x58]), new LRReduction(0x44, staticRules[0x58]), new LRReduction(0x49, staticRules[0x58]), new LRReduction(0x4a, staticRules[0x58]), new LRReduction(0x4b, staticRules[0x58])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x28]), new LRReduction(0xb, staticRules[0x28]), new LRReduction(0x30, staticRules[0x28]), new LRReduction(0x31, staticRules[0x28]), new LRReduction(0x32, staticRules[0x28]), new LRReduction(0x33, staticRules[0x28]), new LRReduction(0x34, staticRules[0x28]), new LRReduction(0x35, staticRules[0x28]), new LRReduction(0x41, staticRules[0x28]), new LRReduction(0x43, staticRules[0x28]), new LRReduction(0x44, staticRules[0x28]), new LRReduction(0x49, staticRules[0x28]), new LRReduction(0x4a, staticRules[0x28]), new LRReduction(0x4b, staticRules[0x28])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[7]},
               new ushort[1] {0x2d},
               new ushort[1] {0x14A},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC3]), new LRReduction(0x12, staticRules[0xC3]), new LRReduction(0xdc, staticRules[0xC3])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x14B},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[28] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[28] {new LRReduction(0xa, staticRules[0xA7]), new LRReduction(0x11, staticRules[0xA7]), new LRReduction(0x2e, staticRules[0xA7]), new LRReduction(0x34, staticRules[0xA7]), new LRReduction(0x35, staticRules[0xA7]), new LRReduction(0x36, staticRules[0xA7]), new LRReduction(0x37, staticRules[0xA7]), new LRReduction(0x38, staticRules[0xA7]), new LRReduction(0x39, staticRules[0xA7]), new LRReduction(0x3a, staticRules[0xA7]), new LRReduction(0x3b, staticRules[0xA7]), new LRReduction(0x3c, staticRules[0xA7]), new LRReduction(0x3d, staticRules[0xA7]), new LRReduction(0x3e, staticRules[0xA7]), new LRReduction(0x3f, staticRules[0xA7]), new LRReduction(0x41, staticRules[0xA7]), new LRReduction(0x43, staticRules[0xA7]), new LRReduction(0x44, staticRules[0xA7]), new LRReduction(0x45, staticRules[0xA7]), new LRReduction(0x46, staticRules[0xA7]), new LRReduction(0x47, staticRules[0xA7]), new LRReduction(0x48, staticRules[0xA7]), new LRReduction(0x49, staticRules[0xA7]), new LRReduction(0x4a, staticRules[0xA7]), new LRReduction(0x4e, staticRules[0xA7]), new LRReduction(0x55, staticRules[0xA7]), new LRReduction(0x56, staticRules[0xA7]), new LRReduction(0xdd, staticRules[0xA7])})
            , new LR1State(
               null,
               new SymbolTerminal[15] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[52], FileCentralDogma_Lexer.terminals[54], FileCentralDogma_Lexer.terminals[56], FileCentralDogma_Lexer.terminals[34], FileCentralDogma_Lexer.terminals[38], FileCentralDogma_Lexer.terminals[44], FileCentralDogma_Lexer.terminals[53], FileCentralDogma_Lexer.terminals[55], FileCentralDogma_Lexer.terminals[57], FileCentralDogma_Lexer.terminals[35]},
               new ushort[15] {0x11, 0x2e, 0xa, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f},
               new ushort[15] {0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x90, 0x91, 0x92},
               new ushort[6] {0x97, 0x69, 0x6a, 0x6b, 0x98, 0x6d},
               new ushort[6] {0x14C, 0x7F, 0x80, 0x81, 0x82, 0x83},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0xA8]), new LRReduction(0x4e, staticRules[0xA8])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0x5C]), new LRReduction(0x4e, staticRules[0x5C])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0xa, staticRules[0xB0]), new LRReduction(0x12, staticRules[0xB0])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xB9]), new LRReduction(0x12, staticRules[0xB9]), new LRReduction(0xdc, staticRules[0xB9])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[9]},
               new ushort[1] {0x41},
               new ushort[1] {0x14D},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[17] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[12], FileCentralDogma_Lexer.terminals[13], FileCentralDogma_Lexer.terminals[14], FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[19], FileCentralDogma_Lexer.terminals[21], FileCentralDogma_Lexer.terminals[22], FileCentralDogma_Lexer.terminals[23]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[17] {new LRReduction(0xa, staticRules[0x7E]), new LRReduction(0x11, staticRules[0x7E]), new LRReduction(0x2e, staticRules[0x7E]), new LRReduction(0x30, staticRules[0x7E]), new LRReduction(0x41, staticRules[0x7E]), new LRReduction(0x43, staticRules[0x7E]), new LRReduction(0x44, staticRules[0x7E]), new LRReduction(0x45, staticRules[0x7E]), new LRReduction(0x46, staticRules[0x7E]), new LRReduction(0x47, staticRules[0x7E]), new LRReduction(0x48, staticRules[0x7E]), new LRReduction(0x49, staticRules[0x7E]), new LRReduction(0x4a, staticRules[0x7E]), new LRReduction(0x4e, staticRules[0x7E]), new LRReduction(0x55, staticRules[0x7E]), new LRReduction(0x56, staticRules[0x7E]), new LRReduction(0xdd, staticRules[0x7E])})
            , new LR1State(
               null,
               new SymbolTerminal[4] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[4], FileCentralDogma_Lexer.terminals[26], FileCentralDogma_Lexer.terminals[32]},
               new ushort[4] {0x11, 0x2e, 0xa, 0x30},
               new ushort[4] {0x84, 0x85, 0xB1, 0xB3},
               new ushort[7] {0x85, 0x69, 0x6a, 0x6b, 0x86, 0x6e, 0x5b},
               new ushort[7] {0x14E, 0xAC, 0xAD, 0xAE, 0xAF, 0xB0, 0xB2},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0x7F]), new LRReduction(0x4e, staticRules[0x7F])})
            , new LR1State(
               null,
               new SymbolTerminal[1] {FileCentralDogma_Lexer.terminals[5]},
               new ushort[1] {0x12},
               new ushort[1] {0x14F},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[0] {})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xC6]), new LRReduction(0x12, staticRules[0xC6]), new LRReduction(0xdc, staticRules[0xC6])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0xA9]), new LRReduction(0x4e, staticRules[0xA9])})
            , new LR1State(
               null,
               new SymbolTerminal[3] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[5], FileCentralDogma_Lexer.terminals[6]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[3] {new LRReduction(0xa, staticRules[0xBC]), new LRReduction(0x12, staticRules[0xBC]), new LRReduction(0xdc, staticRules[0xBC])})
            , new LR1State(
               null,
               new SymbolTerminal[2] {FileCentralDogma_Lexer.terminals[15], FileCentralDogma_Lexer.terminals[19]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[2] {new LRReduction(0x48, staticRules[0x80]), new LRReduction(0x4e, staticRules[0x80])})
            , new LR1State(
               null,
               new SymbolTerminal[14] {FileCentralDogma_Lexer.terminals[3], FileCentralDogma_Lexer.terminals[2], FileCentralDogma_Lexer.terminals[32], FileCentralDogma_Lexer.terminals[33], FileCentralDogma_Lexer.terminals[41], FileCentralDogma_Lexer.terminals[42], FileCentralDogma_Lexer.terminals[37], FileCentralDogma_Lexer.terminals[43], FileCentralDogma_Lexer.terminals[9], FileCentralDogma_Lexer.terminals[10], FileCentralDogma_Lexer.terminals[11], FileCentralDogma_Lexer.terminals[16], FileCentralDogma_Lexer.terminals[17], FileCentralDogma_Lexer.terminals[28]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new LRReduction[14] {new LRReduction(0xa, staticRules[0x29]), new LRReduction(0xb, staticRules[0x29]), new LRReduction(0x30, staticRules[0x29]), new LRReduction(0x31, staticRules[0x29]), new LRReduction(0x32, staticRules[0x29]), new LRReduction(0x33, staticRules[0x29]), new LRReduction(0x34, staticRules[0x29]), new LRReduction(0x35, staticRules[0x29]), new LRReduction(0x41, staticRules[0x29]), new LRReduction(0x43, staticRules[0x29]), new LRReduction(0x44, staticRules[0x29]), new LRReduction(0x49, staticRules[0x29]), new LRReduction(0x4a, staticRules[0x29]), new LRReduction(0x4b, staticRules[0x29])})
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
