
namespace Hime.Kernel.Resources.Parser
{


    public class Lexer_Hime_Kernel_FileCentralDogma : Hime.Kernel.Parsers.LexerText
    {
        private static ushort[] p_SymbolsSID = { 0xB, 0xA, 0x11, 0x12, 0xD2, 0x9, 0x3E, 0x3F, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x4A, 0x4B, 0x4C, 0x4F, 0x53, 0x54, 0xD3, 0x7, 0x40, 0x2D, 0x48, 0x49, 0x51, 0xD1, 0x2E, 0x2F, 0x37, 0x3D, 0x32, 0x38, 0x52, 0xC, 0x30, 0x31, 0x33, 0x39, 0xD, 0x4D, 0x50, 0xF, 0xE, 0x10, 0x4E, 0x34, 0x3A, 0x35, 0x3B, 0x36, 0x3C };
        private static string[] p_SymbolsName = { "_T[.]", "NAME", "_T[{]", "_T[}]", "_T[[]", "NAME_CHARACTER", "_T[=]", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[<]", "_T[,]", "_T[>]", "_T[:]", "_T[^]", "_T[!]", "_T[]]", "SEPARATOR", "_T[..]", "QUOTED_DATA", "_T[=>]", "_T[->]", "_T[cf]", "_T[cs]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_BINARY", "SYMBOL_VALUE_UINT8", "SYMBOL_JOKER_UINT8", "_T[rules]", "_T[public]", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_TERMINAL_UCAT", "SYMBOL_VALUE_UINT16", "SYMBOL_JOKER_UINT16", "_T[private]", "_T[options]", "_T[grammar]", "_T[internal]", "_T[protected]", "_T[namespace]", "_T[terminals]", "SYMBOL_VALUE_UINT32", "SYMBOL_JOKER_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_JOKER_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_JOKER_UINT128" };

        private static ushort[][] p_Transitions0 = { new ushort[3] { 0x2F, 0x2F, 0x1 }, new ushort[3] { 0x2E, 0x2E, 0x5E }, new ushort[3] { 0x70, 0x70, 0x5F }, new ushort[3] { 0x69, 0x69, 0x60 }, new ushort[3] { 0x6E, 0x6E, 0x61 }, new ushort[3] { 0x7B, 0x7B, 0x68 }, new ushort[3] { 0x7D, 0x7D, 0x69 }, new ushort[3] { 0x22, 0x22, 0x3 }, new ushort[3] { 0x27, 0x27, 0x4 }, new ushort[3] { 0x5B, 0x5B, 0x6A }, new ushort[3] { 0x5C, 0x5C, 0x5 }, new ushort[3] { 0x30, 0x30, 0x6B }, new ushort[3] { 0x3D, 0x3D, 0x6D }, new ushort[3] { 0x3B, 0x3B, 0x6E }, new ushort[3] { 0x28, 0x28, 0x6F }, new ushort[3] { 0x29, 0x29, 0x70 }, new ushort[3] { 0x2A, 0x2A, 0x71 }, new ushort[3] { 0x2B, 0x2B, 0x72 }, new ushort[3] { 0x3F, 0x3F, 0x73 }, new ushort[3] { 0x2D, 0x2D, 0x74 }, new ushort[3] { 0x7C, 0x7C, 0x75 }, new ushort[3] { 0x3C, 0x3C, 0x76 }, new ushort[3] { 0x2C, 0x2C, 0x77 }, new ushort[3] { 0x3E, 0x3E, 0x78 }, new ushort[3] { 0x6F, 0x6F, 0x62 }, new ushort[3] { 0x74, 0x74, 0x63 }, new ushort[3] { 0x3A, 0x3A, 0x79 }, new ushort[3] { 0x67, 0x67, 0x64 }, new ushort[3] { 0x63, 0x63, 0x65 }, new ushort[3] { 0x72, 0x72, 0x66 }, new ushort[3] { 0x5E, 0x5E, 0x7A }, new ushort[3] { 0x21, 0x21, 0x7B }, new ushort[3] { 0x5D, 0x5D, 0x7C }, new ushort[3] { 0xA, 0xA, 0x7D }, new ushort[3] { 0x2028, 0x2029, 0x7D }, new ushort[3] { 0x9, 0x9, 0x7F }, new ushort[3] { 0xB, 0xC, 0x7F }, new ushort[3] { 0x20, 0x20, 0x7F }, new ushort[3] { 0x41, 0x5A, 0x67 }, new ushort[3] { 0x5F, 0x5F, 0x67 }, new ushort[3] { 0x61, 0x62, 0x67 }, new ushort[3] { 0x64, 0x66, 0x67 }, new ushort[3] { 0x68, 0x68, 0x67 }, new ushort[3] { 0x6A, 0x6D, 0x67 }, new ushort[3] { 0x71, 0x71, 0x67 }, new ushort[3] { 0x73, 0x73, 0x67 }, new ushort[3] { 0x75, 0x7A, 0x67 }, new ushort[3] { 0x370, 0x3FF, 0x67 }, new ushort[3] { 0x31, 0x39, 0x6C }, new ushort[3] { 0x40, 0x40, 0x6 }, new ushort[3] { 0xD, 0xD, 0x7E } };

        private static ushort[][] p_Transitions1 = { new ushort[3] { 0x2F, 0x2F, 0x7 }, new ushort[3] { 0x2A, 0x2A, 0x8 } };

        private static ushort[][] p_Transitions2 = { new ushort[3] { 0x2A, 0x2A, 0x9 }, new ushort[3] { 0x2F, 0x2F, 0x16 } };

        private static ushort[][] p_Transitions3 = { new ushort[3] { 0x0, 0x21, 0x3 }, new ushort[3] { 0x23, 0xFFFF, 0x3 }, new ushort[3] { 0x22, 0x22, 0xB3 } };

        private static ushort[][] p_Transitions4 = { new ushort[3] { 0x5C, 0x5C, 0xA }, new ushort[3] { 0x0, 0x26, 0xB }, new ushort[3] { 0x28, 0x5B, 0xB }, new ushort[3] { 0x5D, 0xFFFF, 0xB } };

        private static ushort[][] p_Transitions5 = { new ushort[3] { 0x75, 0x75, 0xE } };

        private static ushort[][] p_Transitions6 = { new ushort[3] { 0x41, 0x5A, 0x8B }, new ushort[3] { 0x5F, 0x5F, 0x8B }, new ushort[3] { 0x61, 0x7A, 0x8B }, new ushort[3] { 0x370, 0x3FF, 0x8B } };

        private static ushort[][] p_Transitions7 = { new ushort[3] { 0xA, 0xA, 0xBE }, new ushort[3] { 0x2028, 0x2029, 0xBE }, new ushort[3] { 0x0, 0x9, 0x7 }, new ushort[3] { 0xB, 0xC, 0x7 }, new ushort[3] { 0xE, 0x2027, 0x7 }, new ushort[3] { 0x202A, 0xFFFF, 0x7 }, new ushort[3] { 0xD, 0xD, 0xBF } };

        private static ushort[][] p_Transitions8 = { new ushort[3] { 0x0, 0x29, 0x8 }, new ushort[3] { 0x2B, 0xFFFF, 0x8 }, new ushort[3] { 0x2A, 0x2A, 0x11 } };

        private static ushort[][] p_Transitions9 = { new ushort[3] { 0x0, 0x29, 0x9 }, new ushort[3] { 0x2B, 0xFFFF, 0x9 }, new ushort[3] { 0x2A, 0x2A, 0x1B } };

        private static ushort[][] p_Transitions10 = { new ushort[3] { 0x27, 0x27, 0xB }, new ushort[3] { 0x5C, 0x5C, 0xB } };

        private static ushort[][] p_Transitions11 = { new ushort[3] { 0x27, 0x27, 0xC0 }, new ushort[3] { 0x5C, 0x5C, 0xA }, new ushort[3] { 0x0, 0x26, 0xB }, new ushort[3] { 0x28, 0x5B, 0xB }, new ushort[3] { 0x5D, 0xFFFF, 0xB } };

        private static ushort[][] p_Transitions12 = { new ushort[3] { 0x5B, 0x5D, 0xD } };

        private static ushort[][] p_Transitions13 = { new ushort[3] { 0x5D, 0x5D, 0xC1 }, new ushort[3] { 0x5C, 0x5C, 0xC }, new ushort[3] { 0x0, 0x5A, 0xD }, new ushort[3] { 0x5E, 0xFFFF, 0xD } };

        private static ushort[][] p_Transitions14 = { new ushort[3] { 0x62, 0x62, 0x12 }, new ushort[3] { 0x63, 0x63, 0x13 } };

        private static ushort[][] p_Transitions15 = { new ushort[3] { 0x30, 0x39, 0x14 }, new ushort[3] { 0x41, 0x46, 0x14 }, new ushort[3] { 0x61, 0x66, 0x14 }, new ushort[3] { 0x58, 0x58, 0x15 } };

        private static ushort[][] p_Transitions16 = { new ushort[3] { 0x30, 0x31, 0xC2 }, new ushort[3] { 0x42, 0x42, 0xC3 } };

        private static ushort[][] p_Transitions17 = { new ushort[3] { 0x2F, 0x2F, 0xC4 }, new ushort[3] { 0x0, 0x2E, 0x8 }, new ushort[3] { 0x30, 0xFFFF, 0x8 } };

        private static ushort[][] p_Transitions18 = { new ushort[3] { 0x7B, 0x7B, 0x19 } };

        private static ushort[][] p_Transitions19 = { new ushort[3] { 0x7B, 0x7B, 0x1A } };

        private static ushort[][] p_Transitions20 = { new ushort[3] { 0x30, 0x39, 0xC5 }, new ushort[3] { 0x41, 0x46, 0xC5 }, new ushort[3] { 0x61, 0x66, 0xC5 } };

        private static ushort[][] p_Transitions21 = { new ushort[3] { 0x58, 0x58, 0xC6 } };

        private static ushort[][] p_Transitions22 = { new ushort[3] { 0xA, 0xA, 0xBA }, new ushort[3] { 0xD, 0xD, 0xBA }, new ushort[3] { 0x2028, 0x2029, 0xBA }, new ushort[3] { 0x0, 0x9, 0x16 }, new ushort[3] { 0xB, 0xC, 0x16 }, new ushort[3] { 0xE, 0x2027, 0x16 }, new ushort[3] { 0x202A, 0xFFFF, 0x16 } };

        private static ushort[][] p_Transitions23 = { new ushort[3] { 0xA, 0xA, 0x1C }, new ushort[3] { 0xD, 0xD, 0x1C }, new ushort[3] { 0x2028, 0x2029, 0x1C }, new ushort[3] { 0x0, 0x9, 0x17 }, new ushort[3] { 0xB, 0xC, 0x17 }, new ushort[3] { 0xE, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 } };

        private static ushort[][] p_Transitions24 = { new ushort[3] { 0xA, 0xA, 0x1C }, new ushort[3] { 0xD, 0xD, 0x1C }, new ushort[3] { 0x2028, 0x2029, 0x1C }, new ushort[3] { 0x0, 0x9, 0x17 }, new ushort[3] { 0xB, 0xC, 0x17 }, new ushort[3] { 0xE, 0x29, 0x17 }, new ushort[3] { 0x2B, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x2A, 0x2A, 0x1D }, new ushort[3] { 0x2F, 0x2F, 0x22 } };

        private static ushort[][] p_Transitions25 = { new ushort[3] { 0x40, 0x40, 0x24 }, new ushort[3] { 0x41, 0x5A, 0x26 }, new ushort[3] { 0x5F, 0x5F, 0x26 }, new ushort[3] { 0x61, 0x7A, 0x26 }, new ushort[3] { 0x370, 0x3FF, 0x26 } };

        private static ushort[][] p_Transitions26 = { new ushort[3] { 0x40, 0x40, 0x25 }, new ushort[3] { 0x41, 0x5A, 0x27 }, new ushort[3] { 0x5F, 0x5F, 0x27 }, new ushort[3] { 0x61, 0x7A, 0x27 }, new ushort[3] { 0x370, 0x3FF, 0x27 } };

        private static ushort[][] p_Transitions27 = { new ushort[3] { 0x2F, 0x2F, 0xB8 }, new ushort[3] { 0x0, 0x2E, 0x1F }, new ushort[3] { 0x30, 0xFFFF, 0x1F } };

        private static ushort[][] p_Transitions28 = { new ushort[3] { 0xA, 0xA, 0x1C }, new ushort[3] { 0xD, 0xD, 0x1C }, new ushort[3] { 0x2028, 0x2029, 0x1C }, new ushort[3] { 0x0, 0x9, 0x17 }, new ushort[3] { 0xB, 0xC, 0x17 }, new ushort[3] { 0xE, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 } };

        private static ushort[][] p_Transitions29 = { new ushort[3] { 0x0, 0x29, 0x1D }, new ushort[3] { 0x2B, 0xFFFF, 0x1D }, new ushort[3] { 0x2A, 0x2A, 0x2A } };

        private static ushort[][] p_Transitions30 = { new ushort[3] { 0x0, 0x29, 0x1E }, new ushort[3] { 0x2B, 0xFFFF, 0x1E }, new ushort[3] { 0x2A, 0x2A, 0x2B } };

        private static ushort[][] p_Transitions31 = { new ushort[3] { 0x2A, 0x2A, 0x1B }, new ushort[3] { 0x0, 0x29, 0x9 }, new ushort[3] { 0x2B, 0xFFFF, 0x9 } };

        private static ushort[][] p_Transitions32 = { new ushort[3] { 0x0, 0xFFFF, 0x20 } };

        private static ushort[][] p_Transitions33 = { new ushort[3] { 0x0, 0x29, 0x20 }, new ushort[3] { 0x2B, 0x2E, 0x20 }, new ushort[3] { 0x30, 0xFFFF, 0x20 }, new ushort[3] { 0x2A, 0x2A, 0x1E }, new ushort[3] { 0x2F, 0x2F, 0x23 } };

        private static ushort[][] p_Transitions34 = { new ushort[3] { 0xA, 0xA, 0xBB }, new ushort[3] { 0x2028, 0x2029, 0xBB }, new ushort[3] { 0x0, 0x9, 0x22 }, new ushort[3] { 0xB, 0xC, 0x22 }, new ushort[3] { 0xE, 0x2027, 0x22 }, new ushort[3] { 0x202A, 0xFFFF, 0x22 }, new ushort[3] { 0xD, 0xD, 0xBD } };

        private static ushort[][] p_Transitions35 = { new ushort[3] { 0xA, 0xA, 0xBC }, new ushort[3] { 0xD, 0xD, 0xBC }, new ushort[3] { 0x2028, 0x2029, 0xBC }, new ushort[3] { 0x0, 0x9, 0x23 }, new ushort[3] { 0xB, 0xC, 0x23 }, new ushort[3] { 0xE, 0x2027, 0x23 }, new ushort[3] { 0x202A, 0xFFFF, 0x23 } };

        private static ushort[][] p_Transitions36 = { new ushort[3] { 0x41, 0x5A, 0x26 }, new ushort[3] { 0x5F, 0x5F, 0x26 }, new ushort[3] { 0x61, 0x7A, 0x26 }, new ushort[3] { 0x370, 0x3FF, 0x26 } };

        private static ushort[][] p_Transitions37 = { new ushort[3] { 0x41, 0x5A, 0x27 }, new ushort[3] { 0x5F, 0x5F, 0x27 }, new ushort[3] { 0x61, 0x7A, 0x27 }, new ushort[3] { 0x370, 0x3FF, 0x27 } };

        private static ushort[][] p_Transitions38 = { new ushort[3] { 0x30, 0x39, 0x26 }, new ushort[3] { 0x41, 0x5A, 0x26 }, new ushort[3] { 0x5F, 0x5F, 0x26 }, new ushort[3] { 0x61, 0x7A, 0x26 }, new ushort[3] { 0x370, 0x3FF, 0x26 }, new ushort[3] { 0x7D, 0x7D, 0xC9 } };

        private static ushort[][] p_Transitions39 = { new ushort[3] { 0x30, 0x39, 0x27 }, new ushort[3] { 0x41, 0x5A, 0x27 }, new ushort[3] { 0x5F, 0x5F, 0x27 }, new ushort[3] { 0x61, 0x7A, 0x27 }, new ushort[3] { 0x370, 0x3FF, 0x27 }, new ushort[3] { 0x7D, 0x7D, 0xCA } };

        private static ushort[][] p_Transitions40 = { new ushort[3] { 0x30, 0x39, 0xCB }, new ushort[3] { 0x41, 0x46, 0xCB }, new ushort[3] { 0x61, 0x66, 0xCB } };

        private static ushort[][] p_Transitions41 = { new ushort[3] { 0x58, 0x58, 0xCC } };

        private static ushort[][] p_Transitions42 = { new ushort[3] { 0x0, 0x2E, 0x1D }, new ushort[3] { 0x30, 0xFFFF, 0x1D }, new ushort[3] { 0x2F, 0x2F, 0xBB } };

        private static ushort[][] p_Transitions43 = { new ushort[3] { 0x0, 0x2E, 0x1E }, new ushort[3] { 0x30, 0xFFFF, 0x1E }, new ushort[3] { 0x2F, 0x2F, 0xBC } };

        private static ushort[][] p_Transitions44 = { new ushort[3] { 0x30, 0x39, 0x3F }, new ushort[3] { 0x41, 0x46, 0x3F }, new ushort[3] { 0x61, 0x66, 0x3F } };

        private static ushort[][] p_Transitions45 = { new ushort[3] { 0x30, 0x39, 0x37 }, new ushort[3] { 0x41, 0x46, 0x37 }, new ushort[3] { 0x61, 0x66, 0x37 } };

        private static ushort[][] p_Transitions46 = { new ushort[3] { 0x30, 0x39, 0x2F }, new ushort[3] { 0x41, 0x46, 0x2F }, new ushort[3] { 0x61, 0x66, 0x2F } };

        private static ushort[][] p_Transitions47 = { new ushort[3] { 0x30, 0x39, 0x30 }, new ushort[3] { 0x41, 0x46, 0x30 }, new ushort[3] { 0x61, 0x66, 0x30 } };

        private static ushort[][] p_Transitions48 = { new ushort[3] { 0x30, 0x39, 0x31 }, new ushort[3] { 0x41, 0x46, 0x31 }, new ushort[3] { 0x61, 0x66, 0x31 } };

        private static ushort[][] p_Transitions49 = { new ushort[3] { 0x30, 0x39, 0x32 }, new ushort[3] { 0x41, 0x46, 0x32 }, new ushort[3] { 0x61, 0x66, 0x32 } };

        private static ushort[][] p_Transitions50 = { new ushort[3] { 0x30, 0x39, 0x33 }, new ushort[3] { 0x41, 0x46, 0x33 }, new ushort[3] { 0x61, 0x66, 0x33 } };

        private static ushort[][] p_Transitions51 = { new ushort[3] { 0x30, 0x39, 0x34 }, new ushort[3] { 0x41, 0x46, 0x34 }, new ushort[3] { 0x61, 0x66, 0x34 } };

        private static ushort[][] p_Transitions52 = { new ushort[3] { 0x30, 0x39, 0x35 }, new ushort[3] { 0x41, 0x46, 0x35 }, new ushort[3] { 0x61, 0x66, 0x35 } };

        private static ushort[][] p_Transitions53 = { new ushort[3] { 0x30, 0x39, 0x36 }, new ushort[3] { 0x41, 0x46, 0x36 }, new ushort[3] { 0x61, 0x66, 0x36 } };

        private static ushort[][] p_Transitions54 = { new ushort[3] { 0x30, 0x39, 0x38 }, new ushort[3] { 0x41, 0x46, 0x38 }, new ushort[3] { 0x61, 0x66, 0x38 } };

        private static ushort[][] p_Transitions55 = { new ushort[3] { 0x30, 0x39, 0x39 }, new ushort[3] { 0x41, 0x46, 0x39 }, new ushort[3] { 0x61, 0x66, 0x39 } };

        private static ushort[][] p_Transitions56 = { new ushort[3] { 0x30, 0x39, 0x3A }, new ushort[3] { 0x41, 0x46, 0x3A }, new ushort[3] { 0x61, 0x66, 0x3A } };

        private static ushort[][] p_Transitions57 = { new ushort[3] { 0x30, 0x39, 0x3B }, new ushort[3] { 0x41, 0x46, 0x3B }, new ushort[3] { 0x61, 0x66, 0x3B } };

        private static ushort[][] p_Transitions58 = { new ushort[3] { 0x30, 0x39, 0x3C }, new ushort[3] { 0x41, 0x46, 0x3C }, new ushort[3] { 0x61, 0x66, 0x3C } };

        private static ushort[][] p_Transitions59 = { new ushort[3] { 0x30, 0x39, 0x3D }, new ushort[3] { 0x41, 0x46, 0x3D }, new ushort[3] { 0x61, 0x66, 0x3D } };

        private static ushort[][] p_Transitions60 = { new ushort[3] { 0x30, 0x39, 0x3E }, new ushort[3] { 0x41, 0x46, 0x3E }, new ushort[3] { 0x61, 0x66, 0x3E } };

        private static ushort[][] p_Transitions61 = { new ushort[3] { 0x30, 0x39, 0x40 }, new ushort[3] { 0x41, 0x46, 0x40 }, new ushort[3] { 0x61, 0x66, 0x40 } };

        private static ushort[][] p_Transitions62 = { new ushort[3] { 0x30, 0x39, 0x41 }, new ushort[3] { 0x41, 0x46, 0x41 }, new ushort[3] { 0x61, 0x66, 0x41 } };

        private static ushort[][] p_Transitions63 = { new ushort[3] { 0x30, 0x39, 0x58 }, new ushort[3] { 0x41, 0x46, 0x58 }, new ushort[3] { 0x61, 0x66, 0x58 } };

        private static ushort[][] p_Transitions64 = { new ushort[3] { 0x30, 0x39, 0x5A }, new ushort[3] { 0x41, 0x46, 0x5A }, new ushort[3] { 0x61, 0x66, 0x5A } };

        private static ushort[][] p_Transitions65 = { new ushort[3] { 0x30, 0x39, 0x5C }, new ushort[3] { 0x41, 0x46, 0x5C }, new ushort[3] { 0x61, 0x66, 0x5C } };

        private static ushort[][] p_Transitions66 = { new ushort[3] { 0x58, 0x58, 0x55 } };

        private static ushort[][] p_Transitions67 = { new ushort[3] { 0x58, 0x58, 0x4D } };

        private static ushort[][] p_Transitions68 = { new ushort[3] { 0x58, 0x58, 0x45 } };

        private static ushort[][] p_Transitions69 = { new ushort[3] { 0x58, 0x58, 0x46 } };

        private static ushort[][] p_Transitions70 = { new ushort[3] { 0x58, 0x58, 0x47 } };

        private static ushort[][] p_Transitions71 = { new ushort[3] { 0x58, 0x58, 0x48 } };

        private static ushort[][] p_Transitions72 = { new ushort[3] { 0x58, 0x58, 0x49 } };

        private static ushort[][] p_Transitions73 = { new ushort[3] { 0x58, 0x58, 0x4A } };

        private static ushort[][] p_Transitions74 = { new ushort[3] { 0x58, 0x58, 0x4B } };

        private static ushort[][] p_Transitions75 = { new ushort[3] { 0x58, 0x58, 0x4C } };

        private static ushort[][] p_Transitions76 = { new ushort[3] { 0x58, 0x58, 0x4E } };

        private static ushort[][] p_Transitions77 = { new ushort[3] { 0x58, 0x58, 0x4F } };

        private static ushort[][] p_Transitions78 = { new ushort[3] { 0x58, 0x58, 0x50 } };

        private static ushort[][] p_Transitions79 = { new ushort[3] { 0x58, 0x58, 0x51 } };

        private static ushort[][] p_Transitions80 = { new ushort[3] { 0x58, 0x58, 0x52 } };

        private static ushort[][] p_Transitions81 = { new ushort[3] { 0x58, 0x58, 0x53 } };

        private static ushort[][] p_Transitions82 = { new ushort[3] { 0x58, 0x58, 0x54 } };

        private static ushort[][] p_Transitions83 = { new ushort[3] { 0x58, 0x58, 0x56 } };

        private static ushort[][] p_Transitions84 = { new ushort[3] { 0x58, 0x58, 0x57 } };

        private static ushort[][] p_Transitions85 = { new ushort[3] { 0x58, 0x58, 0x59 } };

        private static ushort[][] p_Transitions86 = { new ushort[3] { 0x58, 0x58, 0x5B } };

        private static ushort[][] p_Transitions87 = { new ushort[3] { 0x58, 0x58, 0x5D } };

        private static ushort[][] p_Transitions88 = { new ushort[3] { 0x30, 0x39, 0xD4 }, new ushort[3] { 0x41, 0x46, 0xD4 }, new ushort[3] { 0x61, 0x66, 0xD4 } };

        private static ushort[][] p_Transitions89 = { new ushort[3] { 0x58, 0x58, 0xD5 } };

        private static ushort[][] p_Transitions90 = { new ushort[3] { 0x30, 0x39, 0xD6 }, new ushort[3] { 0x41, 0x46, 0xD6 }, new ushort[3] { 0x61, 0x66, 0xD6 } };

        private static ushort[][] p_Transitions91 = { new ushort[3] { 0x58, 0x58, 0xD7 } };

        private static ushort[][] p_Transitions92 = { new ushort[3] { 0x30, 0x39, 0xD8 }, new ushort[3] { 0x41, 0x46, 0xD8 }, new ushort[3] { 0x61, 0x66, 0xD8 } };

        private static ushort[][] p_Transitions93 = { new ushort[3] { 0x58, 0x58, 0xD9 } };

        private static ushort[][] p_Transitions94 = { new ushort[3] { 0x2E, 0x2E, 0x80 } };

        private static ushort[][] p_Transitions95 = { new ushort[3] { 0x75, 0x75, 0x81 }, new ushort[3] { 0x72, 0x72, 0x82 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x71, 0x83 }, new ushort[3] { 0x73, 0x74, 0x83 }, new ushort[3] { 0x76, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions96 = { new ushort[3] { 0x6E, 0x6E, 0x84 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6D, 0x83 }, new ushort[3] { 0x6F, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions97 = { new ushort[3] { 0x61, 0x61, 0x85 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions98 = { new ushort[3] { 0x70, 0x70, 0x86 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6F, 0x83 }, new ushort[3] { 0x71, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions99 = { new ushort[3] { 0x65, 0x65, 0x87 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions100 = { new ushort[3] { 0x72, 0x72, 0x88 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x71, 0x83 }, new ushort[3] { 0x73, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions101 = { new ushort[3] { 0x66, 0x66, 0xB6 }, new ushort[3] { 0x73, 0x73, 0xB7 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x65, 0x83 }, new ushort[3] { 0x67, 0x72, 0x83 }, new ushort[3] { 0x74, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions102 = { new ushort[3] { 0x75, 0x75, 0x89 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x74, 0x83 }, new ushort[3] { 0x76, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions103 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions104 = { };

        private static ushort[][] p_Transitions105 = { };

        private static ushort[][] p_Transitions106 = { new ushort[3] { 0x5C, 0x5C, 0xC }, new ushort[3] { 0x0, 0x5A, 0xD }, new ushort[3] { 0x5E, 0xFFFF, 0xD } };

        private static ushort[][] p_Transitions107 = { new ushort[3] { 0x78, 0x78, 0xF }, new ushort[3] { 0x62, 0x62, 0x10 } };

        private static ushort[][] p_Transitions108 = { };

        private static ushort[][] p_Transitions109 = { new ushort[3] { 0x3E, 0x3E, 0xB4 } };

        private static ushort[][] p_Transitions110 = { };

        private static ushort[][] p_Transitions111 = { };

        private static ushort[][] p_Transitions112 = { };

        private static ushort[][] p_Transitions113 = { };

        private static ushort[][] p_Transitions114 = { };

        private static ushort[][] p_Transitions115 = { };

        private static ushort[][] p_Transitions116 = { new ushort[3] { 0x3E, 0x3E, 0xB5 } };

        private static ushort[][] p_Transitions117 = { };

        private static ushort[][] p_Transitions118 = { };

        private static ushort[][] p_Transitions119 = { };

        private static ushort[][] p_Transitions120 = { };

        private static ushort[][] p_Transitions121 = { };

        private static ushort[][] p_Transitions122 = { };

        private static ushort[][] p_Transitions123 = { };

        private static ushort[][] p_Transitions124 = { };

        private static ushort[][] p_Transitions125 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xB8 }, new ushort[3] { 0x20, 0x20, 0xB8 }, new ushort[3] { 0x2028, 0x2029, 0xB8 } };

        private static ushort[][] p_Transitions126 = { new ushort[3] { 0xA, 0xA, 0x7D }, new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0x9, 0xB8 }, new ushort[3] { 0xB, 0xD, 0xB8 }, new ushort[3] { 0x20, 0x20, 0xB8 }, new ushort[3] { 0x2028, 0x2029, 0xB8 } };

        private static ushort[][] p_Transitions127 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xB8 }, new ushort[3] { 0x20, 0x20, 0xB8 }, new ushort[3] { 0x2028, 0x2029, 0xB8 } };

        private static ushort[][] p_Transitions128 = { };

        private static ushort[][] p_Transitions129 = { new ushort[3] { 0x62, 0x62, 0x8A }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x61, 0x83 }, new ushort[3] { 0x63, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions130 = { new ushort[3] { 0x69, 0x69, 0x8C }, new ushort[3] { 0x6F, 0x6F, 0x8D }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x68, 0x83 }, new ushort[3] { 0x6A, 0x6E, 0x83 }, new ushort[3] { 0x70, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions131 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions132 = { new ushort[3] { 0x74, 0x74, 0x8E }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x73, 0x83 }, new ushort[3] { 0x75, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions133 = { new ushort[3] { 0x6D, 0x6D, 0x8F }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6C, 0x83 }, new ushort[3] { 0x6E, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions134 = { new ushort[3] { 0x74, 0x74, 0x90 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x73, 0x83 }, new ushort[3] { 0x75, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions135 = { new ushort[3] { 0x72, 0x72, 0x91 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x71, 0x83 }, new ushort[3] { 0x73, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions136 = { new ushort[3] { 0x61, 0x61, 0x92 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions137 = { new ushort[3] { 0x6C, 0x6C, 0x93 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6B, 0x83 }, new ushort[3] { 0x6D, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions138 = { new ushort[3] { 0x6C, 0x6C, 0x94 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6B, 0x83 }, new ushort[3] { 0x6D, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions139 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions140 = { new ushort[3] { 0x76, 0x76, 0x95 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x75, 0x83 }, new ushort[3] { 0x77, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions141 = { new ushort[3] { 0x74, 0x74, 0x96 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x73, 0x83 }, new ushort[3] { 0x75, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions142 = { new ushort[3] { 0x65, 0x65, 0x97 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions143 = { new ushort[3] { 0x65, 0x65, 0x98 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions144 = { new ushort[3] { 0x69, 0x69, 0x99 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x68, 0x83 }, new ushort[3] { 0x6A, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions145 = { new ushort[3] { 0x6D, 0x6D, 0x9A }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6C, 0x83 }, new ushort[3] { 0x6E, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions146 = { new ushort[3] { 0x6D, 0x6D, 0x9B }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6C, 0x83 }, new ushort[3] { 0x6E, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions147 = { new ushort[3] { 0x65, 0x65, 0x9C }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions148 = { new ushort[3] { 0x69, 0x69, 0x9D }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x68, 0x83 }, new ushort[3] { 0x6A, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions149 = { new ushort[3] { 0x61, 0x61, 0x9E }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions150 = { new ushort[3] { 0x65, 0x65, 0x9F }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions151 = { new ushort[3] { 0x72, 0x72, 0xA0 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x71, 0x83 }, new ushort[3] { 0x73, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions152 = { new ushort[3] { 0x73, 0x73, 0xA2 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x72, 0x83 }, new ushort[3] { 0x74, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions153 = { new ushort[3] { 0x6F, 0x6F, 0xA3 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6E, 0x83 }, new ushort[3] { 0x70, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions154 = { new ushort[3] { 0x69, 0x69, 0xA1 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x68, 0x83 }, new ushort[3] { 0x6A, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions155 = { new ushort[3] { 0x6D, 0x6D, 0xA4 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6C, 0x83 }, new ushort[3] { 0x6E, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions156 = { new ushort[3] { 0x73, 0x73, 0xC7 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x72, 0x83 }, new ushort[3] { 0x74, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions157 = { new ushort[3] { 0x63, 0x63, 0xC8 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x62, 0x83 }, new ushort[3] { 0x64, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions158 = { new ushort[3] { 0x74, 0x74, 0xAC }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x73, 0x83 }, new ushort[3] { 0x75, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions159 = { new ushort[3] { 0x63, 0x63, 0xA5 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x62, 0x83 }, new ushort[3] { 0x64, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions160 = { new ushort[3] { 0x6E, 0x6E, 0xA6 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6D, 0x83 }, new ushort[3] { 0x6F, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions161 = { new ushort[3] { 0x6E, 0x6E, 0xA8 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6D, 0x83 }, new ushort[3] { 0x6F, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions162 = { new ushort[3] { 0x70, 0x70, 0xA7 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6F, 0x83 }, new ushort[3] { 0x71, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions163 = { new ushort[3] { 0x6E, 0x6E, 0xAD }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6D, 0x83 }, new ushort[3] { 0x6F, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions164 = { new ushort[3] { 0x61, 0x61, 0xAE }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions165 = { new ushort[3] { 0x74, 0x74, 0xA9 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x73, 0x83 }, new ushort[3] { 0x75, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions166 = { new ushort[3] { 0x61, 0x61, 0xAF }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions167 = { new ushort[3] { 0x61, 0x61, 0xAA }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions168 = { new ushort[3] { 0x61, 0x61, 0xAB }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x62, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions169 = { new ushort[3] { 0x65, 0x65, 0xB0 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions170 = { new ushort[3] { 0x63, 0x63, 0xB1 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x62, 0x83 }, new ushort[3] { 0x64, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions171 = { new ushort[3] { 0x6C, 0x6C, 0xB2 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6B, 0x83 }, new ushort[3] { 0x6D, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions172 = { new ushort[3] { 0x65, 0x65, 0xCD }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions173 = { new ushort[3] { 0x73, 0x73, 0xCE }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x72, 0x83 }, new ushort[3] { 0x74, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions174 = { new ushort[3] { 0x72, 0x72, 0xCF }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x71, 0x83 }, new ushort[3] { 0x73, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions175 = { new ushort[3] { 0x6C, 0x6C, 0xD0 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x6B, 0x83 }, new ushort[3] { 0x6D, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions176 = { new ushort[3] { 0x64, 0x64, 0xD1 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x63, 0x83 }, new ushort[3] { 0x65, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions177 = { new ushort[3] { 0x65, 0x65, 0xD2 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x64, 0x83 }, new ushort[3] { 0x66, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions178 = { new ushort[3] { 0x73, 0x73, 0xD3 }, new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x72, 0x83 }, new ushort[3] { 0x74, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions179 = { };

        private static ushort[][] p_Transitions180 = { };

        private static ushort[][] p_Transitions181 = { };

        private static ushort[][] p_Transitions182 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions183 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions184 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xB8 }, new ushort[3] { 0x20, 0x20, 0xB8 }, new ushort[3] { 0x2028, 0x2029, 0xB8 } };

        private static ushort[][] p_Transitions185 = { new ushort[3] { 0xA, 0xA, 0xB9 }, new ushort[3] { 0xD, 0xD, 0xB9 }, new ushort[3] { 0x2028, 0x2029, 0xB9 }, new ushort[3] { 0x0, 0x8, 0x17 }, new ushort[3] { 0xE, 0x1F, 0x17 }, new ushort[3] { 0x21, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0x18 }, new ushort[3] { 0x9, 0x9, 0xBB }, new ushort[3] { 0xB, 0xC, 0xBB }, new ushort[3] { 0x20, 0x20, 0xBB } };

        private static ushort[][] p_Transitions186 = { new ushort[3] { 0xA, 0xA, 0xBA }, new ushort[3] { 0xD, 0xD, 0xBA }, new ushort[3] { 0x2028, 0x2029, 0xBA }, new ushort[3] { 0x0, 0x8, 0x20 }, new ushort[3] { 0xE, 0x1F, 0x20 }, new ushort[3] { 0x21, 0x2E, 0x20 }, new ushort[3] { 0x30, 0x2027, 0x20 }, new ushort[3] { 0x202A, 0xFFFF, 0x20 }, new ushort[3] { 0x9, 0x9, 0xBC }, new ushort[3] { 0xB, 0xC, 0xBC }, new ushort[3] { 0x20, 0x20, 0xBC }, new ushort[3] { 0x2F, 0x2F, 0x21 } };

        private static ushort[][] p_Transitions187 = { new ushort[3] { 0xA, 0xA, 0xB9 }, new ushort[3] { 0xD, 0xD, 0xB9 }, new ushort[3] { 0x2028, 0x2029, 0xB9 }, new ushort[3] { 0x0, 0x8, 0x17 }, new ushort[3] { 0xE, 0x1F, 0x17 }, new ushort[3] { 0x21, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x9, 0x9, 0xBB }, new ushort[3] { 0xB, 0xC, 0xBB }, new ushort[3] { 0x20, 0x20, 0xBB }, new ushort[3] { 0x2F, 0x2F, 0x18 } };

        private static ushort[][] p_Transitions188 = { new ushort[3] { 0xA, 0xA, 0xBA }, new ushort[3] { 0xD, 0xD, 0xBA }, new ushort[3] { 0x2028, 0x2029, 0xBA }, new ushort[3] { 0x0, 0x8, 0x20 }, new ushort[3] { 0xE, 0x1F, 0x20 }, new ushort[3] { 0x21, 0x2E, 0x20 }, new ushort[3] { 0x30, 0x2027, 0x20 }, new ushort[3] { 0x202A, 0xFFFF, 0x20 }, new ushort[3] { 0x9, 0x9, 0xBC }, new ushort[3] { 0xB, 0xC, 0xBC }, new ushort[3] { 0x20, 0x20, 0xBC }, new ushort[3] { 0x2F, 0x2F, 0x21 } };

        private static ushort[][] p_Transitions189 = { new ushort[3] { 0x9, 0xC, 0xBB }, new ushort[3] { 0x20, 0x20, 0xBB }, new ushort[3] { 0xD, 0xD, 0xB9 }, new ushort[3] { 0x2028, 0x2029, 0xB9 }, new ushort[3] { 0x0, 0x8, 0x17 }, new ushort[3] { 0xE, 0x1F, 0x17 }, new ushort[3] { 0x21, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0x18 } };

        private static ushort[][] p_Transitions190 = { new ushort[3] { 0xA, 0xA, 0xB9 }, new ushort[3] { 0xD, 0xD, 0xB9 }, new ushort[3] { 0x2028, 0x2029, 0xB9 }, new ushort[3] { 0x0, 0x8, 0x17 }, new ushort[3] { 0xE, 0x1F, 0x17 }, new ushort[3] { 0x21, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0x18 }, new ushort[3] { 0x9, 0x9, 0xBB }, new ushort[3] { 0xB, 0xC, 0xBB }, new ushort[3] { 0x20, 0x20, 0xBB } };

        private static ushort[][] p_Transitions191 = { new ushort[3] { 0xA, 0xA, 0xBE }, new ushort[3] { 0xD, 0xD, 0xB9 }, new ushort[3] { 0x2028, 0x2029, 0xB9 }, new ushort[3] { 0x0, 0x8, 0x17 }, new ushort[3] { 0xE, 0x1F, 0x17 }, new ushort[3] { 0x21, 0x2E, 0x17 }, new ushort[3] { 0x30, 0x2027, 0x17 }, new ushort[3] { 0x202A, 0xFFFF, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0x18 }, new ushort[3] { 0x9, 0x9, 0xBB }, new ushort[3] { 0xB, 0xC, 0xBB }, new ushort[3] { 0x20, 0x20, 0xBB } };

        private static ushort[][] p_Transitions192 = { };

        private static ushort[][] p_Transitions193 = { };

        private static ushort[][] p_Transitions194 = { new ushort[3] { 0x30, 0x31, 0xC2 } };

        private static ushort[][] p_Transitions195 = { new ushort[3] { 0x42, 0x42, 0xC3 } };

        private static ushort[][] p_Transitions196 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xB8 }, new ushort[3] { 0x20, 0x20, 0xB8 }, new ushort[3] { 0x2028, 0x2029, 0xB8 } };

        private static ushort[][] p_Transitions197 = { new ushort[3] { 0x30, 0x39, 0x28 }, new ushort[3] { 0x41, 0x46, 0x28 }, new ushort[3] { 0x61, 0x66, 0x28 } };

        private static ushort[][] p_Transitions198 = { new ushort[3] { 0x58, 0x58, 0x29 } };

        private static ushort[][] p_Transitions199 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions200 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions201 = { };

        private static ushort[][] p_Transitions202 = { };

        private static ushort[][] p_Transitions203 = { new ushort[3] { 0x30, 0x39, 0x2C }, new ushort[3] { 0x41, 0x46, 0x2C }, new ushort[3] { 0x61, 0x66, 0x2C } };

        private static ushort[][] p_Transitions204 = { new ushort[3] { 0x58, 0x58, 0x42 } };

        private static ushort[][] p_Transitions205 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions206 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions207 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions208 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions209 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions210 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions211 = { new ushort[3] { 0x30, 0x39, 0x83 }, new ushort[3] { 0x41, 0x5A, 0x83 }, new ushort[3] { 0x5F, 0x5F, 0x83 }, new ushort[3] { 0x61, 0x7A, 0x83 }, new ushort[3] { 0x370, 0x3FF, 0x83 } };

        private static ushort[][] p_Transitions212 = { new ushort[3] { 0x30, 0x39, 0x2D }, new ushort[3] { 0x41, 0x46, 0x2D }, new ushort[3] { 0x61, 0x66, 0x2D } };

        private static ushort[][] p_Transitions213 = { new ushort[3] { 0x58, 0x58, 0x43 } };

        private static ushort[][] p_Transitions214 = { new ushort[3] { 0x30, 0x39, 0x2E }, new ushort[3] { 0x41, 0x46, 0x2E }, new ushort[3] { 0x61, 0x66, 0x2E } };

        private static ushort[][] p_Transitions215 = { new ushort[3] { 0x58, 0x58, 0x44 } };

        private static ushort[][] p_Transitions216 = { };

        private static ushort[][] p_Transitions217 = { };

        private static ushort[][][] p_Transitions = { p_Transitions0, p_Transitions1, p_Transitions2, p_Transitions3, p_Transitions4, p_Transitions5, p_Transitions6, p_Transitions7, p_Transitions8, p_Transitions9, p_Transitions10, p_Transitions11, p_Transitions12, p_Transitions13, p_Transitions14, p_Transitions15, p_Transitions16, p_Transitions17, p_Transitions18, p_Transitions19, p_Transitions20, p_Transitions21, p_Transitions22, p_Transitions23, p_Transitions24, p_Transitions25, p_Transitions26, p_Transitions27, p_Transitions28, p_Transitions29, p_Transitions30, p_Transitions31, p_Transitions32, p_Transitions33, p_Transitions34, p_Transitions35, p_Transitions36, p_Transitions37, p_Transitions38, p_Transitions39, p_Transitions40, p_Transitions41, p_Transitions42, p_Transitions43, p_Transitions44, p_Transitions45, p_Transitions46, p_Transitions47, p_Transitions48, p_Transitions49, p_Transitions50, p_Transitions51, p_Transitions52, p_Transitions53, p_Transitions54, p_Transitions55, p_Transitions56, p_Transitions57, p_Transitions58, p_Transitions59, p_Transitions60, p_Transitions61, p_Transitions62, p_Transitions63, p_Transitions64, p_Transitions65, p_Transitions66, p_Transitions67, p_Transitions68, p_Transitions69, p_Transitions70, p_Transitions71, p_Transitions72, p_Transitions73, p_Transitions74, p_Transitions75, p_Transitions76, p_Transitions77, p_Transitions78, p_Transitions79, p_Transitions80, p_Transitions81, p_Transitions82, p_Transitions83, p_Transitions84, p_Transitions85, p_Transitions86, p_Transitions87, p_Transitions88, p_Transitions89, p_Transitions90, p_Transitions91, p_Transitions92, p_Transitions93, p_Transitions94, p_Transitions95, p_Transitions96, p_Transitions97, p_Transitions98, p_Transitions99, p_Transitions100, p_Transitions101, p_Transitions102, p_Transitions103, p_Transitions104, p_Transitions105, p_Transitions106, p_Transitions107, p_Transitions108, p_Transitions109, p_Transitions110, p_Transitions111, p_Transitions112, p_Transitions113, p_Transitions114, p_Transitions115, p_Transitions116, p_Transitions117, p_Transitions118, p_Transitions119, p_Transitions120, p_Transitions121, p_Transitions122, p_Transitions123, p_Transitions124, p_Transitions125, p_Transitions126, p_Transitions127, p_Transitions128, p_Transitions129, p_Transitions130, p_Transitions131, p_Transitions132, p_Transitions133, p_Transitions134, p_Transitions135, p_Transitions136, p_Transitions137, p_Transitions138, p_Transitions139, p_Transitions140, p_Transitions141, p_Transitions142, p_Transitions143, p_Transitions144, p_Transitions145, p_Transitions146, p_Transitions147, p_Transitions148, p_Transitions149, p_Transitions150, p_Transitions151, p_Transitions152, p_Transitions153, p_Transitions154, p_Transitions155, p_Transitions156, p_Transitions157, p_Transitions158, p_Transitions159, p_Transitions160, p_Transitions161, p_Transitions162, p_Transitions163, p_Transitions164, p_Transitions165, p_Transitions166, p_Transitions167, p_Transitions168, p_Transitions169, p_Transitions170, p_Transitions171, p_Transitions172, p_Transitions173, p_Transitions174, p_Transitions175, p_Transitions176, p_Transitions177, p_Transitions178, p_Transitions179, p_Transitions180, p_Transitions181, p_Transitions182, p_Transitions183, p_Transitions184, p_Transitions185, p_Transitions186, p_Transitions187, p_Transitions188, p_Transitions189, p_Transitions190, p_Transitions191, p_Transitions192, p_Transitions193, p_Transitions194, p_Transitions195, p_Transitions196, p_Transitions197, p_Transitions198, p_Transitions199, p_Transitions200, p_Transitions201, p_Transitions202, p_Transitions203, p_Transitions204, p_Transitions205, p_Transitions206, p_Transitions207, p_Transitions208, p_Transitions209, p_Transitions210, p_Transitions211, p_Transitions212, p_Transitions213, p_Transitions214, p_Transitions215, p_Transitions216, p_Transitions217 };
        private static int[] p_Finals = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 22, 22, 23, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 24, 25, 26, 27, 28, 22, 22, 22, 22, 22, 22, 22, 22, 29, 30, 31, 32, 22, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 };

        public Lexer_Hime_Kernel_FileCentralDogma(string input) : base(input) { }
        protected Lexer_Hime_Kernel_FileCentralDogma(string input, int position, int line, System.Collections.Generic.List<Hime.Kernel.Parsers.LexerTextError> errors) : base(input, position, line, errors) { }

        public static string GetSymbolName(ushort SID)
        {
            for (int i = 0; i != p_SymbolsSID.Length; i++)
            {
                if (p_SymbolsSID[i] == SID)
                    return p_SymbolsName[i];
            }
            return null;
        }



        public Lexer_Hime_Kernel_FileCentralDogma Clone_Hime_Kernel_FileCentralDogma() { return new Lexer_Hime_Kernel_FileCentralDogma(p_Input, p_CurrentPosition, p_Line, p_Errors); }
        public override Hime.Kernel.Parsers.SymbolToken GetNextToken(ushort[] IDs) { throw new Hime.Kernel.Parsers.LexerException("Text lexer does not support this method."); }
        public override Hime.Kernel.Parsers.SymbolToken GetNextToken()
        {
            if (p_CurrentPosition == p_Length)
            {
                p_CurrentPosition++;
                return new Hime.Kernel.Parsers.SymbolTokenDollar();
            }
            if (p_CurrentPosition > p_Length)
                return new Hime.Kernel.Parsers.SymbolTokenEpsilon();

            while (true)
            {
                if (p_CurrentPosition == p_Length)
                {
                    p_CurrentPosition++;
                    return new Hime.Kernel.Parsers.SymbolTokenDollar();
                }
                Hime.Kernel.Parsers.SymbolTokenText Token = GetNextToken_DFA();
                if (Token == null)
                {
                    p_Errors.Add(new Hime.Kernel.Parsers.LexerTextErrorDiscardedChar(p_Input[p_CurrentPosition], p_Line, CurrentColumn));
                    p_CurrentPosition++;
                }

                else if (Token.SymbolID == 7)
                {
                    p_CurrentPosition += Token.ValueText.Length;
                    foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
                }

                else
                {

                    p_CurrentPosition += Token.ValueText.Length;
                    foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
                    return Token;
                }
            }
        }

        private Hime.Kernel.Parsers.SymbolTokenText GetNextToken_DFA()
        {
            System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolTokenText> MatchedTokens = new System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolTokenText>();
            int End = p_CurrentPosition;
            ushort State = 0;

            while (true)
            {
                if (p_Finals[State] != -1)
                {
                    string Value = p_Input.Substring(p_CurrentPosition, End - p_CurrentPosition);
                    MatchedTokens.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_SymbolsName[p_Finals[State]], p_SymbolsSID[p_Finals[State]], Value, p_Line));
                }
                if (End == p_Length)
                    break;
                ushort Char = System.Convert.ToUInt16(p_Input[End]);
                ushort NextState = 0xFFFF;
                End++;
                for (int i = 0; i != p_Transitions[State].Length; i++)
                {
                    if (Char >= p_Transitions[State][i][0] && Char <= p_Transitions[State][i][1])
                        NextState = p_Transitions[State][i][2];
                }
                if (NextState == 0xFFFF)
                    break;
                State = NextState;
            }
            if (MatchedTokens.Count == 0)
                return null;
            return MatchedTokens[MatchedTokens.Count - 1];
        }
    }

    public class Parser_Hime_Kernel_FileCentralDogma : Hime.Kernel.Parsers.IParser
    {

        private delegate void Production(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes);

        private static void ReduceRule_13_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x13, "qualified_name"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_14_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x14, "symbol_access_public"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_public"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_15_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x15, "symbol_access_private"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_private"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_16_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "symbol_access_protected"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_protected"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_17_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x17, "symbol_access_internal"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_internal"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_18_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_18_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_18_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_18_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_18_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_19_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x19, "Namespace_content"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_1A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1A, "Namespace"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_1B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1B, "_m20"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_1B_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1B, "_m20"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_1C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1C, "_m25"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_1C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1C, "_m25"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_55_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x55, "option"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_56_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_atom_unicode"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_56_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_atom_unicode"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_57_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x57, "terminal_def_atom_text"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_58_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x58, "terminal_def_atom_set"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_59_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x59, "terminal_def_atom_ublock"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5A, "terminal_def_atom_ucat"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5B, "terminal_def_atom_span"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_5(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5C_6(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5D, "terminal_def_element"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5D, "terminal_def_element"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5E, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5E_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5E, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5E_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5E, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5E_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5E, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_5F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5F, "terminal_def_fragment"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_60_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "terminal_def_restrict"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_61_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x61, "terminal_definition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_62_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x62, "terminal_subgrammar"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_62_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x62, "terminal_subgrammar"));

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_63_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x63, "terminal"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_64_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x64, "rule_sym_action"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_65_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x65, "rule_sym_virtual"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_66_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x66, "rule_sym_ref_simple"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_67_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x67, "rule_template_params"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_5(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_6(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_7(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_8(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_9(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_A(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_68_B(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_69_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x69, "grammar_text_terminal"));

            SubRoot.AppendChild(Definition[0]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6A, "grammar_options"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6B, "grammar_terminals"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6C, "grammar_parency"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6C, "grammar_parency"));

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6D_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6D_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 10, 10);
            nodes.RemoveRange(nodes.Count - 10, 10);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6E, "cf_grammar_text"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7]);

            SubRoot.AppendChild(Definition[8]);

            SubRoot.AppendChild(Definition[9], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_6F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 9, 9);
            nodes.RemoveRange(nodes.Count - 9, 9);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6F, "cf_grammar_bin"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7]);

            SubRoot.AppendChild(Definition[8], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_70_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x70, "_m81"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_70_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x70, "_m81"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_71_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x71, "_m83"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_71_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x71, "_m83"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_72_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x72, "_m85"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_72_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x72, "_m85"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_73_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x73, "_m93"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_73_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x73, "_m93"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_74_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "_m97"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_74_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "_m97"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_75_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x75, "_m101"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_75_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x75, "_m101"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_76_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x76, "_m105"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_76_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x76, "_m105"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_77_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "grammar_cf_rules<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_78_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x78, "cf_rule_simple<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_79_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x79, "rule_definition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7A, "rule_def_restrict<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7B, "rule_def_fragment<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7C_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7C_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7D, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7D, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7D_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7D, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7E, "rule_def_element<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7E_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7E, "rule_def_element<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7F_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7F_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7F_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_7F_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_80_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x80, "rule_sym_ref_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_81_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x81, "rule_sym_ref_params<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_82_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x82, "_m125"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_82_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x82, "_m125"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_83_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x83, "_m134"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_83_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x83, "_m134"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_84_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x84, "_m136"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_84_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x84, "_m136"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_85_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "_m138"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_85_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "_m138"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_86_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x86, "cf_rule_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_87_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x87, "_m143"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_87_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x87, "_m143"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_87_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x87, "_m143"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_88_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "grammar_cf_rules<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_89_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x89, "cf_rule_simple<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8A, "rule_definition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8B, "rule_def_restrict<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8C, "rule_def_fragment<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8D_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8D_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8E, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8E_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8E, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8E_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8E, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8F, "rule_def_element<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_8F_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8F, "rule_def_element<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_90_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_90_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_90_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_90_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_90_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_91_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x91, "rule_sym_ref_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_92_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x92, "rule_sym_ref_params<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_93_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x93, "_m165"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_93_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x93, "_m165"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_94_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x94, "_m174"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_94_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x94, "_m174"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_95_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x95, "_m176"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_95_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x95, "_m176"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_96_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x96, "_m178"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_96_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x96, "_m178"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_97_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 5, 5);
            nodes.RemoveRange(nodes.Count - 5, 5);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x97, "cf_rule_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_98_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x98, "_m183"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_98_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x98, "_m183"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_98_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x98, "_m183"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D4_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 9, 9);
            nodes.RemoveRange(nodes.Count - 9, 9);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD4, "cs_grammar_text"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7]);

            SubRoot.AppendChild(Definition[8], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D5_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 8, 8);
            nodes.RemoveRange(nodes.Count - 8, 8);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD5, "cs_grammar_bin"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D6_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD6, "grammar_cs_rules<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D7_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 6, 6);
            nodes.RemoveRange(nodes.Count - 6, 6);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD7, "cs_rule_simple<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D8_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD8, "cs_rule_context<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D8_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD8, "cs_rule_context<grammar_text_terminal>"));

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_D9_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 7, 7);
            nodes.RemoveRange(nodes.Count - 7, 7);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD9, "cs_rule_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DA_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDA, "_m154"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DA_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDA, "_m154"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DA_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDA, "_m154"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DB_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDB, "grammar_cs_rules<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DC_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 6, 6);
            nodes.RemoveRange(nodes.Count - 6, 6);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDC, "cs_rule_simple<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DD_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDD, "cs_rule_context<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DD_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDD, "cs_rule_context<grammar_bin_terminal>"));

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DE_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 7, 7);
            nodes.RemoveRange(nodes.Count - 7, 7);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDE, "cs_rule_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DF_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDF, "_m172"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DF_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDF, "_m172"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_DF_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xDF, "_m172"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_E0_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xE0, "file_item"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_E1_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xE1, "file"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_E2_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xE2, "_m226"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_E2_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xE2, "_m226"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            nodes.Add(SubRoot);
        }

        private static void ReduceRule_E3_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection nodes)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xE3, "_Axiom_"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            nodes.Add(SubRoot);
        }

        private static Production[] p_Rules = { ReduceRule_13_0, ReduceRule_14_0, ReduceRule_15_0, ReduceRule_16_0, ReduceRule_17_0, ReduceRule_18_0, ReduceRule_18_1, ReduceRule_18_2, ReduceRule_18_3, ReduceRule_18_4, ReduceRule_19_0, ReduceRule_1A_0, ReduceRule_1B_0, ReduceRule_1B_1, ReduceRule_1C_0, ReduceRule_1C_1, ReduceRule_55_0, ReduceRule_56_0, ReduceRule_56_1, ReduceRule_57_0, ReduceRule_58_0, ReduceRule_59_0, ReduceRule_5A_0, ReduceRule_5B_0, ReduceRule_5C_0, ReduceRule_5C_1, ReduceRule_5C_2, ReduceRule_5C_3, ReduceRule_5C_4, ReduceRule_5C_5, ReduceRule_5C_6, ReduceRule_5D_0, ReduceRule_5D_1, ReduceRule_5E_0, ReduceRule_5E_1, ReduceRule_5E_2, ReduceRule_5E_3, ReduceRule_5F_0, ReduceRule_60_0, ReduceRule_61_0, ReduceRule_62_0, ReduceRule_62_1, ReduceRule_63_0, ReduceRule_64_0, ReduceRule_65_0, ReduceRule_66_0, ReduceRule_67_0, ReduceRule_68_0, ReduceRule_68_1, ReduceRule_68_2, ReduceRule_68_3, ReduceRule_68_4, ReduceRule_68_5, ReduceRule_68_6, ReduceRule_68_7, ReduceRule_68_8, ReduceRule_68_9, ReduceRule_68_A, ReduceRule_68_B, ReduceRule_69_0, ReduceRule_6A_0, ReduceRule_6B_0, ReduceRule_6C_0, ReduceRule_6C_1, ReduceRule_6D_0, ReduceRule_6D_1, ReduceRule_6D_2, ReduceRule_6D_3, ReduceRule_6E_0, ReduceRule_6F_0, ReduceRule_70_0, ReduceRule_70_1, ReduceRule_71_0, ReduceRule_71_1, ReduceRule_72_0, ReduceRule_72_1, ReduceRule_73_0, ReduceRule_73_1, ReduceRule_74_0, ReduceRule_74_1, ReduceRule_75_0, ReduceRule_75_1, ReduceRule_76_0, ReduceRule_76_1, ReduceRule_77_0, ReduceRule_78_0, ReduceRule_79_0, ReduceRule_7A_0, ReduceRule_7B_0, ReduceRule_7C_0, ReduceRule_7C_1, ReduceRule_7C_2, ReduceRule_7C_3, ReduceRule_7D_0, ReduceRule_7D_1, ReduceRule_7D_2, ReduceRule_7E_0, ReduceRule_7E_1, ReduceRule_7F_0, ReduceRule_7F_1, ReduceRule_7F_2, ReduceRule_7F_3, ReduceRule_7F_4, ReduceRule_80_0, ReduceRule_81_0, ReduceRule_82_0, ReduceRule_82_1, ReduceRule_83_0, ReduceRule_83_1, ReduceRule_84_0, ReduceRule_84_1, ReduceRule_85_0, ReduceRule_85_1, ReduceRule_86_0, ReduceRule_87_0, ReduceRule_87_1, ReduceRule_87_2, ReduceRule_88_0, ReduceRule_89_0, ReduceRule_8A_0, ReduceRule_8B_0, ReduceRule_8C_0, ReduceRule_8D_0, ReduceRule_8D_1, ReduceRule_8D_2, ReduceRule_8D_3, ReduceRule_8E_0, ReduceRule_8E_1, ReduceRule_8E_2, ReduceRule_8F_0, ReduceRule_8F_1, ReduceRule_90_0, ReduceRule_90_1, ReduceRule_90_2, ReduceRule_90_3, ReduceRule_90_4, ReduceRule_91_0, ReduceRule_92_0, ReduceRule_93_0, ReduceRule_93_1, ReduceRule_94_0, ReduceRule_94_1, ReduceRule_95_0, ReduceRule_95_1, ReduceRule_96_0, ReduceRule_96_1, ReduceRule_97_0, ReduceRule_98_0, ReduceRule_98_1, ReduceRule_98_2, ReduceRule_D4_0, ReduceRule_D5_0, ReduceRule_D6_0, ReduceRule_D7_0, ReduceRule_D8_0, ReduceRule_D8_1, ReduceRule_D9_0, ReduceRule_DA_0, ReduceRule_DA_1, ReduceRule_DA_2, ReduceRule_DB_0, ReduceRule_DC_0, ReduceRule_DD_0, ReduceRule_DD_1, ReduceRule_DE_0, ReduceRule_DF_0, ReduceRule_DF_1, ReduceRule_DF_2, ReduceRule_E0_0, ReduceRule_E1_0, ReduceRule_E2_0, ReduceRule_E2_1, ReduceRule_E3_0 };
        private static ushort[] p_RulesHeadID = { 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x18, 0x18, 0x18, 0x18, 0x19, 0x1A, 0x1B, 0x1B, 0x1C, 0x1C, 0x55, 0x56, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5D, 0x5D, 0x5E, 0x5E, 0x5E, 0x5E, 0x5F, 0x60, 0x61, 0x62, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6C, 0x6D, 0x6D, 0x6D, 0x6D, 0x6E, 0x6F, 0x70, 0x70, 0x71, 0x71, 0x72, 0x72, 0x73, 0x73, 0x74, 0x74, 0x75, 0x75, 0x76, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7C, 0x7C, 0x7C, 0x7D, 0x7D, 0x7D, 0x7E, 0x7E, 0x7F, 0x7F, 0x7F, 0x7F, 0x7F, 0x80, 0x81, 0x82, 0x82, 0x83, 0x83, 0x84, 0x84, 0x85, 0x85, 0x86, 0x87, 0x87, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8D, 0x8D, 0x8D, 0x8E, 0x8E, 0x8E, 0x8F, 0x8F, 0x90, 0x90, 0x90, 0x90, 0x90, 0x91, 0x92, 0x93, 0x93, 0x94, 0x94, 0x95, 0x95, 0x96, 0x96, 0x97, 0x98, 0x98, 0x98, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD8, 0xD9, 0xDA, 0xDA, 0xDA, 0xDB, 0xDC, 0xDD, 0xDD, 0xDE, 0xDF, 0xDF, 0xDF, 0xE0, 0xE1, 0xE2, 0xE2, 0xE3 };
        private static string[] p_RulesHeadName = { "qualified_name", "symbol_access_public", "symbol_access_private", "symbol_access_protected", "symbol_access_internal", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_child_symbol", "Namespace_content", "Namespace", "_m20", "_m20", "_m25", "_m25", "option", "terminal_def_atom_unicode", "terminal_def_atom_unicode", "terminal_def_atom_text", "terminal_def_atom_set", "terminal_def_atom_ublock", "terminal_def_atom_ucat", "terminal_def_atom_span", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_atom", "terminal_def_element", "terminal_def_element", "terminal_def_repetition", "terminal_def_repetition", "terminal_def_repetition", "terminal_def_repetition", "terminal_def_fragment", "terminal_def_restrict", "terminal_definition", "terminal_subgrammar", "terminal_subgrammar", "terminal", "rule_sym_action", "rule_sym_virtual", "rule_sym_ref_simple", "rule_template_params", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_bin_terminal", "grammar_text_terminal", "grammar_options", "grammar_terminals", "grammar_parency", "grammar_parency", "grammar_access", "grammar_access", "grammar_access", "grammar_access", "cf_grammar_text", "cf_grammar_bin", "_m81", "_m81", "_m83", "_m83", "_m85", "_m85", "_m93", "_m93", "_m97", "_m97", "_m101", "_m101", "_m105", "_m105", "grammar_cf_rules<grammar_text_terminal>", "cf_rule_simple<grammar_text_terminal>", "rule_definition<grammar_text_terminal>", "rule_def_restrict<grammar_text_terminal>", "rule_def_fragment<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_repetition<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_tree_action<grammar_text_terminal>", "rule_def_element<grammar_text_terminal>", "rule_def_element<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_def_atom<grammar_text_terminal>", "rule_sym_ref_template<grammar_text_terminal>", "rule_sym_ref_params<grammar_text_terminal>", "_m125", "_m125", "_m134", "_m134", "_m136", "_m136", "_m138", "_m138", "cf_rule_template<grammar_text_terminal>", "_m143", "_m143", "_m143", "grammar_cf_rules<grammar_bin_terminal>", "cf_rule_simple<grammar_bin_terminal>", "rule_definition<grammar_bin_terminal>", "rule_def_restrict<grammar_bin_terminal>", "rule_def_fragment<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_repetition<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_tree_action<grammar_bin_terminal>", "rule_def_element<grammar_bin_terminal>", "rule_def_element<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_def_atom<grammar_bin_terminal>", "rule_sym_ref_template<grammar_bin_terminal>", "rule_sym_ref_params<grammar_bin_terminal>", "_m165", "_m165", "_m174", "_m174", "_m176", "_m176", "_m178", "_m178", "cf_rule_template<grammar_bin_terminal>", "_m183", "_m183", "_m183", "cs_grammar_text", "cs_grammar_bin", "grammar_cs_rules<grammar_text_terminal>", "cs_rule_simple<grammar_text_terminal>", "cs_rule_context<grammar_text_terminal>", "cs_rule_context<grammar_text_terminal>", "cs_rule_template<grammar_text_terminal>", "_m154", "_m154", "_m154", "grammar_cs_rules<grammar_bin_terminal>", "cs_rule_simple<grammar_bin_terminal>", "cs_rule_context<grammar_bin_terminal>", "cs_rule_context<grammar_bin_terminal>", "cs_rule_template<grammar_bin_terminal>", "_m172", "_m172", "_m172", "file_item", "file", "_m226", "_m226", "_Axiom_" };
        private static ushort[] p_RulesParserLength = { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 3, 0, 2, 0, 4, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2, 2, 2, 1, 2, 2, 2, 2, 0, 5, 3, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 3, 0, 1, 1, 1, 1, 10, 9, 2, 0, 3, 0, 3, 0, 3, 0, 2, 0, 2, 0, 3, 0, 4, 4, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 1, 3, 1, 1, 1, 1, 1, 2, 4, 3, 0, 2, 0, 3, 0, 3, 0, 5, 2, 2, 0, 4, 4, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 1, 3, 1, 1, 1, 1, 1, 2, 4, 3, 0, 2, 0, 3, 0, 3, 0, 5, 2, 2, 0, 9, 8, 4, 6, 3, 0, 7, 2, 2, 0, 4, 6, 3, 0, 7, 2, 2, 0, 1, 2, 2, 0, 2 };

        private static ushort[] p_StateExpectedIDs_0 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_0 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_StateItems_0 = { "[_Axiom_ -> . file $]", "[file -> . file_item _m226]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_StateShiftsOnTerminal_0 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0x10 }, new ushort[2] { 0xD, 0x11 }, new ushort[2] { 0xE, 0x12 }, new ushort[2] { 0xF, 0x13 } };
        private static ushort[][] p_StateShiftsOnVariable_0 = { new ushort[2] { 0xE1, 0x1 }, new ushort[2] { 0xE0, 0x2 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1A, 0x4 }, new ushort[2] { 0x6E, 0x5 }, new ushort[2] { 0x6F, 0x6 }, new ushort[2] { 0xD4, 0x7 }, new ushort[2] { 0xD5, 0x8 }, new ushort[2] { 0x6D, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_0 = { };

        private static ushort[] p_StateExpectedIDs_1 = { 0x2 };
        private static string[] p_StateExpectedNames_1 = { "$" };
        private static string[] p_StateItems_1 = { "[_Axiom_ -> file . $]" };
        private static ushort[][] p_StateShiftsOnTerminal_1 = { new ushort[2] { 0x2, 0x14 } };
        private static ushort[][] p_StateShiftsOnVariable_1 = { };
        private static ushort[][] p_StateReducsOnTerminal_1 = { };

        private static ushort[] p_StateExpectedIDs_2 = { 0x2, 0x10, 0x50, 0xC, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_2 = { "$", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_StateItems_2 = { "[file -> file_item . _m226]", "[_m226 -> . file_item _m226]", "[_m226 -> . ]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_StateShiftsOnTerminal_2 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0x10 }, new ushort[2] { 0xD, 0x11 }, new ushort[2] { 0xE, 0x12 }, new ushort[2] { 0xF, 0x13 } };
        private static ushort[][] p_StateShiftsOnVariable_2 = { new ushort[2] { 0xE2, 0x15 }, new ushort[2] { 0xE0, 0x16 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1A, 0x4 }, new ushort[2] { 0x6E, 0x5 }, new ushort[2] { 0x6F, 0x6 }, new ushort[2] { 0xD4, 0x7 }, new ushort[2] { 0xD5, 0x8 }, new ushort[2] { 0x6D, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_2 = { new ushort[2] { 0x2, 0xAB } };

        private static ushort[] p_StateExpectedIDs_3 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2 };
        private static string[] p_StateExpectedNames_3 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$" };
        private static string[] p_StateItems_3 = { "[file_item -> Namespace_child_symbol . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3 = { };
        private static ushort[][] p_StateShiftsOnVariable_3 = { };
        private static ushort[][] p_StateReducsOnTerminal_3 = { new ushort[2] { 0x10, 0xA8 }, new ushort[2] { 0x50, 0xA8 }, new ushort[2] { 0xC, 0xA8 }, new ushort[2] { 0xD, 0xA8 }, new ushort[2] { 0xE, 0xA8 }, new ushort[2] { 0xF, 0xA8 }, new ushort[2] { 0x2, 0xA8 } };

        private static ushort[] p_StateExpectedIDs_4 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_4 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_4 = { "[Namespace_child_symbol -> Namespace . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_4 = { };
        private static ushort[][] p_StateShiftsOnVariable_4 = { };
        private static ushort[][] p_StateReducsOnTerminal_4 = { new ushort[2] { 0x10, 0x5 }, new ushort[2] { 0x50, 0x5 }, new ushort[2] { 0xC, 0x5 }, new ushort[2] { 0xD, 0x5 }, new ushort[2] { 0xE, 0x5 }, new ushort[2] { 0xF, 0x5 }, new ushort[2] { 0x2, 0x5 }, new ushort[2] { 0x12, 0x5 } };

        private static ushort[] p_StateExpectedIDs_5 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_5 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_5 = { "[Namespace_child_symbol -> cf_grammar_text . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_5 = { };
        private static ushort[][] p_StateShiftsOnVariable_5 = { };
        private static ushort[][] p_StateReducsOnTerminal_5 = { new ushort[2] { 0x10, 0x6 }, new ushort[2] { 0x50, 0x6 }, new ushort[2] { 0xC, 0x6 }, new ushort[2] { 0xD, 0x6 }, new ushort[2] { 0xE, 0x6 }, new ushort[2] { 0xF, 0x6 }, new ushort[2] { 0x2, 0x6 }, new ushort[2] { 0x12, 0x6 } };

        private static ushort[] p_StateExpectedIDs_6 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_6 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_6 = { "[Namespace_child_symbol -> cf_grammar_bin . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6 = { };
        private static ushort[][] p_StateShiftsOnVariable_6 = { };
        private static ushort[][] p_StateReducsOnTerminal_6 = { new ushort[2] { 0x10, 0x7 }, new ushort[2] { 0x50, 0x7 }, new ushort[2] { 0xC, 0x7 }, new ushort[2] { 0xD, 0x7 }, new ushort[2] { 0xE, 0x7 }, new ushort[2] { 0xF, 0x7 }, new ushort[2] { 0x2, 0x7 }, new ushort[2] { 0x12, 0x7 } };

        private static ushort[] p_StateExpectedIDs_7 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_7 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_7 = { "[Namespace_child_symbol -> cs_grammar_text . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7 = { };
        private static ushort[][] p_StateShiftsOnVariable_7 = { };
        private static ushort[][] p_StateReducsOnTerminal_7 = { new ushort[2] { 0x10, 0x8 }, new ushort[2] { 0x50, 0x8 }, new ushort[2] { 0xC, 0x8 }, new ushort[2] { 0xD, 0x8 }, new ushort[2] { 0xE, 0x8 }, new ushort[2] { 0xF, 0x8 }, new ushort[2] { 0x2, 0x8 }, new ushort[2] { 0x12, 0x8 } };

        private static ushort[] p_StateExpectedIDs_8 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_8 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_8 = { "[Namespace_child_symbol -> cs_grammar_bin . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_8 = { };
        private static ushort[][] p_StateShiftsOnVariable_8 = { };
        private static ushort[][] p_StateReducsOnTerminal_8 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0x9 }, new ushort[2] { 0xC, 0x9 }, new ushort[2] { 0xD, 0x9 }, new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0xF, 0x9 }, new ushort[2] { 0x2, 0x9 }, new ushort[2] { 0x12, 0x9 } };

        private static ushort[] p_StateExpectedIDs_9 = { 0xA };
        private static string[] p_StateExpectedNames_9 = { "NAME" };
        private static string[] p_StateItems_9 = { "[Namespace -> namespace . qualified_name { Namespace_content }]", "[qualified_name -> . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_9 = { new ushort[2] { 0xA, 0x18 } };
        private static ushort[][] p_StateShiftsOnVariable_9 = { new ushort[2] { 0x13, 0x17 } };
        private static ushort[][] p_StateReducsOnTerminal_9 = { };

        private static ushort[] p_StateExpectedIDs_A = { 0x50 };
        private static string[] p_StateExpectedNames_A = { "_T[grammar]" };
        private static string[] p_StateItems_A = { "[cf_grammar_text -> grammar_access . grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access . grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_A = { new ushort[2] { 0x50, 0x19 } };
        private static ushort[][] p_StateShiftsOnVariable_A = { };
        private static ushort[][] p_StateReducsOnTerminal_A = { };

        private static ushort[] p_StateExpectedIDs_B = { 0xD1 };
        private static string[] p_StateExpectedNames_B = { "_T[cs]" };
        private static string[] p_StateItems_B = { "[cs_grammar_text -> grammar . cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar . cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_B = { new ushort[2] { 0xD1, 0x1A } };
        private static ushort[][] p_StateShiftsOnVariable_B = { };
        private static ushort[][] p_StateReducsOnTerminal_B = { };

        private static ushort[] p_StateExpectedIDs_C = { 0x50 };
        private static string[] p_StateExpectedNames_C = { "_T[grammar]" };
        private static string[] p_StateItems_C = { "[grammar_access -> symbol_access_public . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C = { };
        private static ushort[][] p_StateShiftsOnVariable_C = { };
        private static ushort[][] p_StateReducsOnTerminal_C = { new ushort[2] { 0x50, 0x40 } };

        private static ushort[] p_StateExpectedIDs_D = { 0x50 };
        private static string[] p_StateExpectedNames_D = { "_T[grammar]" };
        private static string[] p_StateItems_D = { "[grammar_access -> symbol_access_private . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D = { };
        private static ushort[][] p_StateShiftsOnVariable_D = { };
        private static ushort[][] p_StateReducsOnTerminal_D = { new ushort[2] { 0x50, 0x41 } };

        private static ushort[] p_StateExpectedIDs_E = { 0x50 };
        private static string[] p_StateExpectedNames_E = { "_T[grammar]" };
        private static string[] p_StateItems_E = { "[grammar_access -> symbol_access_protected . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_E = { };
        private static ushort[][] p_StateShiftsOnVariable_E = { };
        private static ushort[][] p_StateReducsOnTerminal_E = { new ushort[2] { 0x50, 0x42 } };

        private static ushort[] p_StateExpectedIDs_F = { 0x50 };
        private static string[] p_StateExpectedNames_F = { "_T[grammar]" };
        private static string[] p_StateItems_F = { "[grammar_access -> symbol_access_internal . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F = { };
        private static ushort[][] p_StateShiftsOnVariable_F = { };
        private static ushort[][] p_StateReducsOnTerminal_F = { new ushort[2] { 0x50, 0x43 } };

        private static ushort[] p_StateExpectedIDs_10 = { 0x50 };
        private static string[] p_StateExpectedNames_10 = { "_T[grammar]" };
        private static string[] p_StateItems_10 = { "[symbol_access_public -> public . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_10 = { };
        private static ushort[][] p_StateShiftsOnVariable_10 = { };
        private static ushort[][] p_StateReducsOnTerminal_10 = { new ushort[2] { 0x50, 0x1 } };

        private static ushort[] p_StateExpectedIDs_11 = { 0x50 };
        private static string[] p_StateExpectedNames_11 = { "_T[grammar]" };
        private static string[] p_StateItems_11 = { "[symbol_access_private -> private . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11 = { };
        private static ushort[][] p_StateShiftsOnVariable_11 = { };
        private static ushort[][] p_StateReducsOnTerminal_11 = { new ushort[2] { 0x50, 0x2 } };

        private static ushort[] p_StateExpectedIDs_12 = { 0x50 };
        private static string[] p_StateExpectedNames_12 = { "_T[grammar]" };
        private static string[] p_StateItems_12 = { "[symbol_access_protected -> protected . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_12 = { };
        private static ushort[][] p_StateShiftsOnVariable_12 = { };
        private static ushort[][] p_StateReducsOnTerminal_12 = { new ushort[2] { 0x50, 0x3 } };

        private static ushort[] p_StateExpectedIDs_13 = { 0x50 };
        private static string[] p_StateExpectedNames_13 = { "_T[grammar]" };
        private static string[] p_StateItems_13 = { "[symbol_access_internal -> internal . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_13 = { };
        private static ushort[][] p_StateShiftsOnVariable_13 = { };
        private static ushort[][] p_StateReducsOnTerminal_13 = { new ushort[2] { 0x50, 0x4 } };

        private static ushort[] p_StateExpectedIDs_14 = { 0x1 };
        private static string[] p_StateExpectedNames_14 = { "ε" };
        private static string[] p_StateItems_14 = { "[_Axiom_ -> file $ . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_14 = { };
        private static ushort[][] p_StateShiftsOnVariable_14 = { };
        private static ushort[][] p_StateReducsOnTerminal_14 = { new ushort[2] { 0x1, 0xAC } };

        private static ushort[] p_StateExpectedIDs_15 = { 0x2 };
        private static string[] p_StateExpectedNames_15 = { "$" };
        private static string[] p_StateItems_15 = { "[file -> file_item _m226 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_15 = { };
        private static ushort[][] p_StateShiftsOnVariable_15 = { };
        private static ushort[][] p_StateReducsOnTerminal_15 = { new ushort[2] { 0x2, 0xA9 } };

        private static ushort[] p_StateExpectedIDs_16 = { 0x2, 0x10, 0x50, 0xC, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_16 = { "$", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_StateItems_16 = { "[_m226 -> file_item . _m226]", "[_m226 -> . file_item _m226]", "[_m226 -> . ]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_StateShiftsOnTerminal_16 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0x10 }, new ushort[2] { 0xD, 0x11 }, new ushort[2] { 0xE, 0x12 }, new ushort[2] { 0xF, 0x13 } };
        private static ushort[][] p_StateShiftsOnVariable_16 = { new ushort[2] { 0xE2, 0x1B }, new ushort[2] { 0xE0, 0x16 }, new ushort[2] { 0x18, 0x3 }, new ushort[2] { 0x1A, 0x4 }, new ushort[2] { 0x6E, 0x5 }, new ushort[2] { 0x6F, 0x6 }, new ushort[2] { 0xD4, 0x7 }, new ushort[2] { 0xD5, 0x8 }, new ushort[2] { 0x6D, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_16 = { new ushort[2] { 0x2, 0xAB } };

        private static ushort[] p_StateExpectedIDs_17 = { 0x11 };
        private static string[] p_StateExpectedNames_17 = { "_T[{]" };
        private static string[] p_StateItems_17 = { "[Namespace -> namespace qualified_name . { Namespace_content }]" };
        private static ushort[][] p_StateShiftsOnTerminal_17 = { new ushort[2] { 0x11, 0x1C } };
        private static ushort[][] p_StateShiftsOnVariable_17 = { };
        private static ushort[][] p_StateReducsOnTerminal_17 = { };

        private static ushort[] p_StateExpectedIDs_18 = { 0x11, 0x4B, 0x12, 0x3F, 0xB };
        private static string[] p_StateExpectedNames_18 = { "_T[{]", "_T[,]", "_T[}]", "_T[;]", "_T[.]" };
        private static string[] p_StateItems_18 = { "[qualified_name -> NAME . _m20]", "[_m20 -> . . NAME _m20]", "[_m20 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_18 = { new ushort[2] { 0xB, 0x1E } };
        private static ushort[][] p_StateShiftsOnVariable_18 = { new ushort[2] { 0x1B, 0x1D } };
        private static ushort[][] p_StateReducsOnTerminal_18 = { new ushort[2] { 0x11, 0xD }, new ushort[2] { 0x4B, 0xD }, new ushort[2] { 0x12, 0xD }, new ushort[2] { 0x3F, 0xD } };

        private static ushort[] p_StateExpectedIDs_19 = { 0x51 };
        private static string[] p_StateExpectedNames_19 = { "_T[cf]" };
        private static string[] p_StateItems_19 = { "[cf_grammar_text -> grammar_access grammar . cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar . cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_19 = { new ushort[2] { 0x51, 0x1F } };
        private static ushort[][] p_StateShiftsOnVariable_19 = { };
        private static ushort[][] p_StateReducsOnTerminal_19 = { };

        private static ushort[] p_StateExpectedIDs_1A = { 0xA };
        private static string[] p_StateExpectedNames_1A = { "NAME" };
        private static string[] p_StateItems_1A = { "[cs_grammar_text -> grammar cs . NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs . NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_1A = { new ushort[2] { 0xA, 0x20 } };
        private static ushort[][] p_StateShiftsOnVariable_1A = { };
        private static ushort[][] p_StateReducsOnTerminal_1A = { };

        private static ushort[] p_StateExpectedIDs_1B = { 0x2 };
        private static string[] p_StateExpectedNames_1B = { "$" };
        private static string[] p_StateItems_1B = { "[_m226 -> file_item _m226 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1B = { };
        private static ushort[][] p_StateShiftsOnVariable_1B = { };
        private static ushort[][] p_StateReducsOnTerminal_1B = { new ushort[2] { 0x2, 0xAA } };

        private static ushort[] p_StateExpectedIDs_1C = { 0x12, 0x10, 0x50, 0xC, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_1C = { "_T[}]", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_StateItems_1C = { "[Namespace -> namespace qualified_name { . Namespace_content }]", "[Namespace_content -> . _m25]", "[_m25 -> . Namespace_child_symbol _m25]", "[_m25 -> . ]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_StateShiftsOnTerminal_1C = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0x10 }, new ushort[2] { 0xD, 0x11 }, new ushort[2] { 0xE, 0x12 }, new ushort[2] { 0xF, 0x13 } };
        private static ushort[][] p_StateShiftsOnVariable_1C = { new ushort[2] { 0x19, 0x21 }, new ushort[2] { 0x1C, 0x22 }, new ushort[2] { 0x18, 0x23 }, new ushort[2] { 0x1A, 0x4 }, new ushort[2] { 0x6E, 0x5 }, new ushort[2] { 0x6F, 0x6 }, new ushort[2] { 0xD4, 0x7 }, new ushort[2] { 0xD5, 0x8 }, new ushort[2] { 0x6D, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_1C = { new ushort[2] { 0x12, 0xF } };

        private static ushort[] p_StateExpectedIDs_1D = { 0x11, 0x4B, 0x12, 0x3F };
        private static string[] p_StateExpectedNames_1D = { "_T[{]", "_T[,]", "_T[}]", "_T[;]" };
        private static string[] p_StateItems_1D = { "[qualified_name -> NAME _m20 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1D = { };
        private static ushort[][] p_StateShiftsOnVariable_1D = { };
        private static ushort[][] p_StateReducsOnTerminal_1D = { new ushort[2] { 0x11, 0x0 }, new ushort[2] { 0x4B, 0x0 }, new ushort[2] { 0x12, 0x0 }, new ushort[2] { 0x3F, 0x0 } };

        private static ushort[] p_StateExpectedIDs_1E = { 0xA };
        private static string[] p_StateExpectedNames_1E = { "NAME" };
        private static string[] p_StateItems_1E = { "[_m20 -> . . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_1E = { new ushort[2] { 0xA, 0x24 } };
        private static ushort[][] p_StateShiftsOnVariable_1E = { };
        private static ushort[][] p_StateReducsOnTerminal_1E = { };

        private static ushort[] p_StateExpectedIDs_1F = { 0xA };
        private static string[] p_StateExpectedNames_1F = { "NAME" };
        private static string[] p_StateItems_1F = { "[cf_grammar_text -> grammar_access grammar cf . NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf . NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_1F = { new ushort[2] { 0xA, 0x25 } };
        private static ushort[][] p_StateShiftsOnVariable_1F = { };
        private static ushort[][] p_StateReducsOnTerminal_1F = { };

        private static ushort[] p_StateExpectedIDs_20 = { 0x11, 0x4F };
        private static string[] p_StateExpectedNames_20 = { "_T[{]", "_T[:]" };
        private static string[] p_StateItems_20 = { "[cs_grammar_text -> grammar cs NAME . grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME . grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_parency -> . : qualified_name _m105]", "[grammar_parency -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_20 = { new ushort[2] { 0x4F, 0x27 } };
        private static ushort[][] p_StateShiftsOnVariable_20 = { new ushort[2] { 0x6C, 0x26 } };
        private static ushort[][] p_StateReducsOnTerminal_20 = { new ushort[2] { 0x11, 0x3F } };

        private static ushort[] p_StateExpectedIDs_21 = { 0x12 };
        private static string[] p_StateExpectedNames_21 = { "_T[}]" };
        private static string[] p_StateItems_21 = { "[Namespace -> namespace qualified_name { Namespace_content . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_21 = { new ushort[2] { 0x12, 0x28 } };
        private static ushort[][] p_StateShiftsOnVariable_21 = { };
        private static ushort[][] p_StateReducsOnTerminal_21 = { };

        private static ushort[] p_StateExpectedIDs_22 = { 0x12 };
        private static string[] p_StateExpectedNames_22 = { "_T[}]" };
        private static string[] p_StateItems_22 = { "[Namespace_content -> _m25 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_22 = { };
        private static ushort[][] p_StateShiftsOnVariable_22 = { };
        private static ushort[][] p_StateReducsOnTerminal_22 = { new ushort[2] { 0x12, 0xA } };

        private static ushort[] p_StateExpectedIDs_23 = { 0x12, 0x10, 0x50, 0xC, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_23 = { "_T[}]", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_StateItems_23 = { "[_m25 -> Namespace_child_symbol . _m25]", "[_m25 -> . Namespace_child_symbol _m25]", "[_m25 -> . ]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_StateShiftsOnTerminal_23 = { new ushort[2] { 0x10, 0x9 }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0x10 }, new ushort[2] { 0xD, 0x11 }, new ushort[2] { 0xE, 0x12 }, new ushort[2] { 0xF, 0x13 } };
        private static ushort[][] p_StateShiftsOnVariable_23 = { new ushort[2] { 0x1C, 0x29 }, new ushort[2] { 0x18, 0x23 }, new ushort[2] { 0x1A, 0x4 }, new ushort[2] { 0x6E, 0x5 }, new ushort[2] { 0x6F, 0x6 }, new ushort[2] { 0xD4, 0x7 }, new ushort[2] { 0xD5, 0x8 }, new ushort[2] { 0x6D, 0xA }, new ushort[2] { 0x14, 0xC }, new ushort[2] { 0x15, 0xD }, new ushort[2] { 0x16, 0xE }, new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_23 = { new ushort[2] { 0x12, 0xF } };

        private static ushort[] p_StateExpectedIDs_24 = { 0x11, 0x4B, 0x12, 0x3F, 0xB };
        private static string[] p_StateExpectedNames_24 = { "_T[{]", "_T[,]", "_T[}]", "_T[;]", "_T[.]" };
        private static string[] p_StateItems_24 = { "[_m20 -> . NAME . _m20]", "[_m20 -> . . NAME _m20]", "[_m20 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_24 = { new ushort[2] { 0xB, 0x1E } };
        private static ushort[][] p_StateShiftsOnVariable_24 = { new ushort[2] { 0x1B, 0x2A } };
        private static ushort[][] p_StateReducsOnTerminal_24 = { new ushort[2] { 0x11, 0xD }, new ushort[2] { 0x4B, 0xD }, new ushort[2] { 0x12, 0xD }, new ushort[2] { 0x3F, 0xD } };

        private static ushort[] p_StateExpectedIDs_25 = { 0x11, 0x4F };
        private static string[] p_StateExpectedNames_25 = { "_T[{]", "_T[:]" };
        private static string[] p_StateItems_25 = { "[cf_grammar_text -> grammar_access grammar cf NAME . grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME . grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_parency -> . : qualified_name _m105]", "[grammar_parency -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_25 = { new ushort[2] { 0x4F, 0x27 } };
        private static ushort[][] p_StateShiftsOnVariable_25 = { new ushort[2] { 0x6C, 0x2B } };
        private static ushort[][] p_StateReducsOnTerminal_25 = { new ushort[2] { 0x11, 0x3F } };

        private static ushort[] p_StateExpectedIDs_26 = { 0x11 };
        private static string[] p_StateExpectedNames_26 = { "_T[{]" };
        private static string[] p_StateItems_26 = { "[cs_grammar_text -> grammar cs NAME grammar_parency . { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency . { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_26 = { new ushort[2] { 0x11, 0x2C } };
        private static ushort[][] p_StateShiftsOnVariable_26 = { };
        private static ushort[][] p_StateReducsOnTerminal_26 = { };

        private static ushort[] p_StateExpectedIDs_27 = { 0xA };
        private static string[] p_StateExpectedNames_27 = { "NAME" };
        private static string[] p_StateItems_27 = { "[grammar_parency -> : . qualified_name _m105]", "[qualified_name -> . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_27 = { new ushort[2] { 0xA, 0x18 } };
        private static ushort[][] p_StateShiftsOnVariable_27 = { new ushort[2] { 0x13, 0x2D } };
        private static ushort[][] p_StateReducsOnTerminal_27 = { };

        private static ushort[] p_StateExpectedIDs_28 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_28 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_28 = { "[Namespace -> namespace qualified_name { Namespace_content } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_28 = { };
        private static ushort[][] p_StateShiftsOnVariable_28 = { };
        private static ushort[][] p_StateReducsOnTerminal_28 = { new ushort[2] { 0x10, 0xB }, new ushort[2] { 0x50, 0xB }, new ushort[2] { 0xC, 0xB }, new ushort[2] { 0xD, 0xB }, new ushort[2] { 0xE, 0xB }, new ushort[2] { 0xF, 0xB }, new ushort[2] { 0x2, 0xB }, new ushort[2] { 0x12, 0xB } };

        private static ushort[] p_StateExpectedIDs_29 = { 0x12 };
        private static string[] p_StateExpectedNames_29 = { "_T[}]" };
        private static string[] p_StateItems_29 = { "[_m25 -> Namespace_child_symbol _m25 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_29 = { };
        private static ushort[][] p_StateShiftsOnVariable_29 = { };
        private static ushort[][] p_StateReducsOnTerminal_29 = { new ushort[2] { 0x12, 0xE } };

        private static ushort[] p_StateExpectedIDs_2A = { 0x11, 0x4B, 0x12, 0x3F };
        private static string[] p_StateExpectedNames_2A = { "_T[{]", "_T[,]", "_T[}]", "_T[;]" };
        private static string[] p_StateItems_2A = { "[_m20 -> . NAME _m20 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_2A = { };
        private static ushort[][] p_StateShiftsOnVariable_2A = { };
        private static ushort[][] p_StateReducsOnTerminal_2A = { new ushort[2] { 0x11, 0xC }, new ushort[2] { 0x4B, 0xC }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x3F, 0xC } };

        private static ushort[] p_StateExpectedIDs_2B = { 0x11 };
        private static string[] p_StateExpectedNames_2B = { "_T[{]" };
        private static string[] p_StateItems_2B = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency . { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency . { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_StateShiftsOnTerminal_2B = { new ushort[2] { 0x11, 0x2E } };
        private static ushort[][] p_StateShiftsOnVariable_2B = { };
        private static ushort[][] p_StateReducsOnTerminal_2B = { };

        private static ushort[] p_StateExpectedIDs_2C = { 0x4D };
        private static string[] p_StateExpectedNames_2C = { "_T[options]" };
        private static string[] p_StateItems_2C = { "[cs_grammar_text -> grammar cs NAME grammar_parency { . grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency { . grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_options -> . options { _m97 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_2C = { new ushort[2] { 0x4D, 0x30 } };
        private static ushort[][] p_StateShiftsOnVariable_2C = { new ushort[2] { 0x6A, 0x2F } };
        private static ushort[][] p_StateReducsOnTerminal_2C = { };

        private static ushort[] p_StateExpectedIDs_2D = { 0x11, 0x4B };
        private static string[] p_StateExpectedNames_2D = { "_T[{]", "_T[,]" };
        private static string[] p_StateItems_2D = { "[grammar_parency -> : qualified_name . _m105]", "[_m105 -> . , qualified_name _m105]", "[_m105 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_2D = { new ushort[2] { 0x4B, 0x32 } };
        private static ushort[][] p_StateShiftsOnVariable_2D = { new ushort[2] { 0x76, 0x31 } };
        private static ushort[][] p_StateReducsOnTerminal_2D = { new ushort[2] { 0x11, 0x53 } };

        private static ushort[] p_StateExpectedIDs_2E = { 0x4D };
        private static string[] p_StateExpectedNames_2E = { "_T[options]" };
        private static string[] p_StateItems_2E = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { . grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { . grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_options -> . options { _m97 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_2E = { new ushort[2] { 0x4D, 0x30 } };
        private static ushort[][] p_StateShiftsOnVariable_2E = { new ushort[2] { 0x6A, 0x33 } };
        private static ushort[][] p_StateReducsOnTerminal_2E = { };

        private static ushort[] p_StateExpectedIDs_2F = { 0x4E, 0x52 };
        private static string[] p_StateExpectedNames_2F = { "_T[terminals]", "_T[rules]" };
        private static string[] p_StateItems_2F = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options . grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options . grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_terminals -> . terminals { _m101 }]", "[grammar_cs_rules<grammar_bin_terminal> -> . rules { _m172 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_2F = { new ushort[2] { 0x4E, 0x36 }, new ushort[2] { 0x52, 0x37 } };
        private static ushort[][] p_StateShiftsOnVariable_2F = { new ushort[2] { 0x6B, 0x34 }, new ushort[2] { 0xDB, 0x35 } };
        private static ushort[][] p_StateReducsOnTerminal_2F = { };

        private static ushort[] p_StateExpectedIDs_30 = { 0x11 };
        private static string[] p_StateExpectedNames_30 = { "_T[{]" };
        private static string[] p_StateItems_30 = { "[grammar_options -> options . { _m97 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_30 = { new ushort[2] { 0x11, 0x38 } };
        private static ushort[][] p_StateShiftsOnVariable_30 = { };
        private static ushort[][] p_StateReducsOnTerminal_30 = { };

        private static ushort[] p_StateExpectedIDs_31 = { 0x11 };
        private static string[] p_StateExpectedNames_31 = { "_T[{]" };
        private static string[] p_StateItems_31 = { "[grammar_parency -> : qualified_name _m105 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_31 = { };
        private static ushort[][] p_StateShiftsOnVariable_31 = { };
        private static ushort[][] p_StateReducsOnTerminal_31 = { new ushort[2] { 0x11, 0x3E } };

        private static ushort[] p_StateExpectedIDs_32 = { 0xA };
        private static string[] p_StateExpectedNames_32 = { "NAME" };
        private static string[] p_StateItems_32 = { "[_m105 -> , . qualified_name _m105]", "[qualified_name -> . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_32 = { new ushort[2] { 0xA, 0x18 } };
        private static ushort[][] p_StateShiftsOnVariable_32 = { new ushort[2] { 0x13, 0x39 } };
        private static ushort[][] p_StateReducsOnTerminal_32 = { };

        private static ushort[] p_StateExpectedIDs_33 = { 0x4E, 0x52 };
        private static string[] p_StateExpectedNames_33 = { "_T[terminals]", "_T[rules]" };
        private static string[] p_StateItems_33 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options . grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options . grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_terminals -> . terminals { _m101 }]", "[grammar_cf_rules<grammar_bin_terminal> -> . rules { _m183 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_33 = { new ushort[2] { 0x4E, 0x36 }, new ushort[2] { 0x52, 0x3C } };
        private static ushort[][] p_StateShiftsOnVariable_33 = { new ushort[2] { 0x6B, 0x3A }, new ushort[2] { 0x88, 0x3B } };
        private static ushort[][] p_StateReducsOnTerminal_33 = { };

        private static ushort[] p_StateExpectedIDs_34 = { 0x52 };
        private static string[] p_StateExpectedNames_34 = { "_T[rules]" };
        private static string[] p_StateItems_34 = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals . grammar_cs_rules<grammar_text_terminal> }]", "[grammar_cs_rules<grammar_text_terminal> -> . rules { _m154 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_34 = { new ushort[2] { 0x52, 0x3E } };
        private static ushort[][] p_StateShiftsOnVariable_34 = { new ushort[2] { 0xD6, 0x3D } };
        private static ushort[][] p_StateReducsOnTerminal_34 = { };

        private static ushort[] p_StateExpectedIDs_35 = { 0x12 };
        private static string[] p_StateExpectedNames_35 = { "_T[}]" };
        private static string[] p_StateItems_35 = { "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_35 = { new ushort[2] { 0x12, 0x3F } };
        private static ushort[][] p_StateShiftsOnVariable_35 = { };
        private static ushort[][] p_StateReducsOnTerminal_35 = { };

        private static ushort[] p_StateExpectedIDs_36 = { 0x11 };
        private static string[] p_StateExpectedNames_36 = { "_T[{]" };
        private static string[] p_StateItems_36 = { "[grammar_terminals -> terminals . { _m101 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_36 = { new ushort[2] { 0x11, 0x40 } };
        private static ushort[][] p_StateShiftsOnVariable_36 = { };
        private static ushort[][] p_StateReducsOnTerminal_36 = { };

        private static ushort[] p_StateExpectedIDs_37 = { 0x11 };
        private static string[] p_StateExpectedNames_37 = { "_T[{]" };
        private static string[] p_StateItems_37 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules . { _m172 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_37 = { new ushort[2] { 0x11, 0x41 } };
        private static ushort[][] p_StateShiftsOnVariable_37 = { };
        private static ushort[][] p_StateReducsOnTerminal_37 = { };

        private static ushort[] p_StateExpectedIDs_38 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_38 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_38 = { "[grammar_options -> options { . _m97 }]", "[_m97 -> . option _m97]", "[_m97 -> . ]", "[option -> . NAME = QUOTED_DATA ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_38 = { new ushort[2] { 0xA, 0x44 } };
        private static ushort[][] p_StateShiftsOnVariable_38 = { new ushort[2] { 0x74, 0x42 }, new ushort[2] { 0x55, 0x43 } };
        private static ushort[][] p_StateReducsOnTerminal_38 = { new ushort[2] { 0x12, 0x4F } };

        private static ushort[] p_StateExpectedIDs_39 = { 0x11, 0x4B };
        private static string[] p_StateExpectedNames_39 = { "_T[{]", "_T[,]" };
        private static string[] p_StateItems_39 = { "[_m105 -> , qualified_name . _m105]", "[_m105 -> . , qualified_name _m105]", "[_m105 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_39 = { new ushort[2] { 0x4B, 0x32 } };
        private static ushort[][] p_StateShiftsOnVariable_39 = { new ushort[2] { 0x76, 0x45 } };
        private static ushort[][] p_StateReducsOnTerminal_39 = { new ushort[2] { 0x11, 0x53 } };

        private static ushort[] p_StateExpectedIDs_3A = { 0x52 };
        private static string[] p_StateExpectedNames_3A = { "_T[rules]" };
        private static string[] p_StateItems_3A = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals . grammar_cf_rules<grammar_text_terminal> }]", "[grammar_cf_rules<grammar_text_terminal> -> . rules { _m143 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_3A = { new ushort[2] { 0x52, 0x47 } };
        private static ushort[][] p_StateShiftsOnVariable_3A = { new ushort[2] { 0x77, 0x46 } };
        private static ushort[][] p_StateReducsOnTerminal_3A = { };

        private static ushort[] p_StateExpectedIDs_3B = { 0x12 };
        private static string[] p_StateExpectedNames_3B = { "_T[}]" };
        private static string[] p_StateItems_3B = { "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_3B = { new ushort[2] { 0x12, 0x48 } };
        private static ushort[][] p_StateShiftsOnVariable_3B = { };
        private static ushort[][] p_StateReducsOnTerminal_3B = { };

        private static ushort[] p_StateExpectedIDs_3C = { 0x11 };
        private static string[] p_StateExpectedNames_3C = { "_T[{]" };
        private static string[] p_StateItems_3C = { "[grammar_cf_rules<grammar_bin_terminal> -> rules . { _m183 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_3C = { new ushort[2] { 0x11, 0x49 } };
        private static ushort[][] p_StateShiftsOnVariable_3C = { };
        private static ushort[][] p_StateReducsOnTerminal_3C = { };

        private static ushort[] p_StateExpectedIDs_3D = { 0x12 };
        private static string[] p_StateExpectedNames_3D = { "_T[}]" };
        private static string[] p_StateItems_3D = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_3D = { new ushort[2] { 0x12, 0x4A } };
        private static ushort[][] p_StateShiftsOnVariable_3D = { };
        private static ushort[][] p_StateReducsOnTerminal_3D = { };

        private static ushort[] p_StateExpectedIDs_3E = { 0x11 };
        private static string[] p_StateExpectedNames_3E = { "_T[{]" };
        private static string[] p_StateItems_3E = { "[grammar_cs_rules<grammar_text_terminal> -> rules . { _m154 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_3E = { new ushort[2] { 0x11, 0x4B } };
        private static ushort[][] p_StateShiftsOnVariable_3E = { };
        private static ushort[][] p_StateReducsOnTerminal_3E = { };

        private static ushort[] p_StateExpectedIDs_3F = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_3F = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_3F = { "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3F = { };
        private static ushort[][] p_StateShiftsOnVariable_3F = { };
        private static ushort[][] p_StateReducsOnTerminal_3F = { new ushort[2] { 0x10, 0x97 }, new ushort[2] { 0x50, 0x97 }, new ushort[2] { 0xC, 0x97 }, new ushort[2] { 0xD, 0x97 }, new ushort[2] { 0xE, 0x97 }, new ushort[2] { 0xF, 0x97 }, new ushort[2] { 0x2, 0x97 }, new ushort[2] { 0x12, 0x97 } };

        private static ushort[] p_StateExpectedIDs_40 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_40 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_40 = { "[grammar_terminals -> terminals { . _m101 }]", "[_m101 -> . terminal _m101]", "[_m101 -> . ]", "[terminal -> . NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_40 = { new ushort[2] { 0xA, 0x4E } };
        private static ushort[][] p_StateShiftsOnVariable_40 = { new ushort[2] { 0x75, 0x4C }, new ushort[2] { 0x63, 0x4D } };
        private static ushort[][] p_StateReducsOnTerminal_40 = { new ushort[2] { 0x12, 0x51 } };

        private static ushort[] p_StateExpectedIDs_41 = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_41 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_41 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { . _m172 }]", "[_m172 -> . cs_rule_simple<grammar_bin_terminal> _m172]", "[_m172 -> . cs_rule_template<grammar_bin_terminal> _m172]", "[_m172 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_41 = { new ushort[2] { 0xD2, 0x53 } };
        private static ushort[][] p_StateShiftsOnVariable_41 = { new ushort[2] { 0xDF, 0x4F }, new ushort[2] { 0xDC, 0x50 }, new ushort[2] { 0xDE, 0x51 }, new ushort[2] { 0xDD, 0x52 } };
        private static ushort[][] p_StateReducsOnTerminal_41 = { new ushort[2] { 0x12, 0xA7 }, new ushort[2] { 0xA, 0xA3 } };

        private static ushort[] p_StateExpectedIDs_42 = { 0x12 };
        private static string[] p_StateExpectedNames_42 = { "_T[}]" };
        private static string[] p_StateItems_42 = { "[grammar_options -> options { _m97 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_42 = { new ushort[2] { 0x12, 0x54 } };
        private static ushort[][] p_StateShiftsOnVariable_42 = { };
        private static ushort[][] p_StateReducsOnTerminal_42 = { };

        private static ushort[] p_StateExpectedIDs_43 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_43 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_43 = { "[_m97 -> option . _m97]", "[_m97 -> . option _m97]", "[_m97 -> . ]", "[option -> . NAME = QUOTED_DATA ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_43 = { new ushort[2] { 0xA, 0x44 } };
        private static ushort[][] p_StateShiftsOnVariable_43 = { new ushort[2] { 0x74, 0x55 }, new ushort[2] { 0x55, 0x43 } };
        private static ushort[][] p_StateReducsOnTerminal_43 = { new ushort[2] { 0x12, 0x4F } };

        private static ushort[] p_StateExpectedIDs_44 = { 0x3E };
        private static string[] p_StateExpectedNames_44 = { "_T[=]" };
        private static string[] p_StateItems_44 = { "[option -> NAME . = QUOTED_DATA ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_44 = { new ushort[2] { 0x3E, 0x56 } };
        private static ushort[][] p_StateShiftsOnVariable_44 = { };
        private static ushort[][] p_StateReducsOnTerminal_44 = { };

        private static ushort[] p_StateExpectedIDs_45 = { 0x11 };
        private static string[] p_StateExpectedNames_45 = { "_T[{]" };
        private static string[] p_StateItems_45 = { "[_m105 -> , qualified_name _m105 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_45 = { };
        private static ushort[][] p_StateShiftsOnVariable_45 = { };
        private static ushort[][] p_StateReducsOnTerminal_45 = { new ushort[2] { 0x11, 0x52 } };

        private static ushort[] p_StateExpectedIDs_46 = { 0x12 };
        private static string[] p_StateExpectedNames_46 = { "_T[}]" };
        private static string[] p_StateItems_46 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_46 = { new ushort[2] { 0x12, 0x57 } };
        private static ushort[][] p_StateShiftsOnVariable_46 = { };
        private static ushort[][] p_StateReducsOnTerminal_46 = { };

        private static ushort[] p_StateExpectedIDs_47 = { 0x11 };
        private static string[] p_StateExpectedNames_47 = { "_T[{]" };
        private static string[] p_StateItems_47 = { "[grammar_cf_rules<grammar_text_terminal> -> rules . { _m143 }]" };
        private static ushort[][] p_StateShiftsOnTerminal_47 = { new ushort[2] { 0x11, 0x58 } };
        private static ushort[][] p_StateShiftsOnVariable_47 = { };
        private static ushort[][] p_StateReducsOnTerminal_47 = { };

        private static ushort[] p_StateExpectedIDs_48 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_48 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_48 = { "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_48 = { };
        private static ushort[][] p_StateShiftsOnVariable_48 = { };
        private static ushort[][] p_StateReducsOnTerminal_48 = { new ushort[2] { 0x10, 0x45 }, new ushort[2] { 0x50, 0x45 }, new ushort[2] { 0xC, 0x45 }, new ushort[2] { 0xD, 0x45 }, new ushort[2] { 0xE, 0x45 }, new ushort[2] { 0xF, 0x45 }, new ushort[2] { 0x2, 0x45 }, new ushort[2] { 0x12, 0x45 } };

        private static ushort[] p_StateExpectedIDs_49 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_49 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_49 = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { . _m183 }]", "[_m183 -> . cf_rule_simple<grammar_bin_terminal> _m183]", "[_m183 -> . cf_rule_template<grammar_bin_terminal> _m183]", "[_m183 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_49 = { new ushort[2] { 0xA, 0x5C } };
        private static ushort[][] p_StateShiftsOnVariable_49 = { new ushort[2] { 0x98, 0x59 }, new ushort[2] { 0x89, 0x5A }, new ushort[2] { 0x97, 0x5B } };
        private static ushort[][] p_StateReducsOnTerminal_49 = { new ushort[2] { 0x12, 0x95 } };

        private static ushort[] p_StateExpectedIDs_4A = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_4A = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_4A = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_4A = { };
        private static ushort[][] p_StateShiftsOnVariable_4A = { };
        private static ushort[][] p_StateReducsOnTerminal_4A = { new ushort[2] { 0x10, 0x96 }, new ushort[2] { 0x50, 0x96 }, new ushort[2] { 0xC, 0x96 }, new ushort[2] { 0xD, 0x96 }, new ushort[2] { 0xE, 0x96 }, new ushort[2] { 0xF, 0x96 }, new ushort[2] { 0x2, 0x96 }, new ushort[2] { 0x12, 0x96 } };

        private static ushort[] p_StateExpectedIDs_4B = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_4B = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_4B = { "[grammar_cs_rules<grammar_text_terminal> -> rules { . _m154 }]", "[_m154 -> . cs_rule_simple<grammar_text_terminal> _m154]", "[_m154 -> . cs_rule_template<grammar_text_terminal> _m154]", "[_m154 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_4B = { new ushort[2] { 0xD2, 0x61 } };
        private static ushort[][] p_StateShiftsOnVariable_4B = { new ushort[2] { 0xDA, 0x5D }, new ushort[2] { 0xD7, 0x5E }, new ushort[2] { 0xD9, 0x5F }, new ushort[2] { 0xD8, 0x60 } };
        private static ushort[][] p_StateReducsOnTerminal_4B = { new ushort[2] { 0x12, 0x9F }, new ushort[2] { 0xA, 0x9B } };

        private static ushort[] p_StateExpectedIDs_4C = { 0x12 };
        private static string[] p_StateExpectedNames_4C = { "_T[}]" };
        private static string[] p_StateItems_4C = { "[grammar_terminals -> terminals { _m101 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_4C = { new ushort[2] { 0x12, 0x62 } };
        private static ushort[][] p_StateShiftsOnVariable_4C = { };
        private static ushort[][] p_StateReducsOnTerminal_4C = { };

        private static ushort[] p_StateExpectedIDs_4D = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_4D = { "_T[}]", "NAME" };
        private static string[] p_StateItems_4D = { "[_m101 -> terminal . _m101]", "[_m101 -> . terminal _m101]", "[_m101 -> . ]", "[terminal -> . NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_4D = { new ushort[2] { 0xA, 0x4E } };
        private static ushort[][] p_StateShiftsOnVariable_4D = { new ushort[2] { 0x75, 0x63 }, new ushort[2] { 0x63, 0x4D } };
        private static ushort[][] p_StateReducsOnTerminal_4D = { new ushort[2] { 0x12, 0x51 } };

        private static ushort[] p_StateExpectedIDs_4E = { 0x49 };
        private static string[] p_StateExpectedNames_4E = { "_T[->]" };
        private static string[] p_StateItems_4E = { "[terminal -> NAME . -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_4E = { new ushort[2] { 0x49, 0x64 } };
        private static ushort[][] p_StateShiftsOnVariable_4E = { };
        private static ushort[][] p_StateReducsOnTerminal_4E = { };

        private static ushort[] p_StateExpectedIDs_4F = { 0x12 };
        private static string[] p_StateExpectedNames_4F = { "_T[}]" };
        private static string[] p_StateItems_4F = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { _m172 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_4F = { new ushort[2] { 0x12, 0x65 } };
        private static ushort[][] p_StateShiftsOnVariable_4F = { };
        private static ushort[][] p_StateReducsOnTerminal_4F = { };

        private static ushort[] p_StateExpectedIDs_50 = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_50 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_50 = { "[_m172 -> cs_rule_simple<grammar_bin_terminal> . _m172]", "[_m172 -> . cs_rule_simple<grammar_bin_terminal> _m172]", "[_m172 -> . cs_rule_template<grammar_bin_terminal> _m172]", "[_m172 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_50 = { new ushort[2] { 0xD2, 0x53 } };
        private static ushort[][] p_StateShiftsOnVariable_50 = { new ushort[2] { 0xDF, 0x66 }, new ushort[2] { 0xDC, 0x50 }, new ushort[2] { 0xDE, 0x51 }, new ushort[2] { 0xDD, 0x52 } };
        private static ushort[][] p_StateReducsOnTerminal_50 = { new ushort[2] { 0x12, 0xA7 }, new ushort[2] { 0xA, 0xA3 } };

        private static ushort[] p_StateExpectedIDs_51 = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_51 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_51 = { "[_m172 -> cs_rule_template<grammar_bin_terminal> . _m172]", "[_m172 -> . cs_rule_simple<grammar_bin_terminal> _m172]", "[_m172 -> . cs_rule_template<grammar_bin_terminal> _m172]", "[_m172 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_51 = { new ushort[2] { 0xD2, 0x53 } };
        private static ushort[][] p_StateShiftsOnVariable_51 = { new ushort[2] { 0xDF, 0x67 }, new ushort[2] { 0xDC, 0x50 }, new ushort[2] { 0xDE, 0x51 }, new ushort[2] { 0xDD, 0x52 } };
        private static ushort[][] p_StateReducsOnTerminal_51 = { new ushort[2] { 0x12, 0xA7 }, new ushort[2] { 0xA, 0xA3 } };

        private static ushort[] p_StateExpectedIDs_52 = { 0xA };
        private static string[] p_StateExpectedNames_52 = { "NAME" };
        private static string[] p_StateItems_52 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> . NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> . NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_52 = { new ushort[2] { 0xA, 0x68 } };
        private static ushort[][] p_StateShiftsOnVariable_52 = { };
        private static ushort[][] p_StateReducsOnTerminal_52 = { };

        private static ushort[] p_StateExpectedIDs_53 = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_53 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_53 = { "[cs_rule_context<grammar_bin_terminal> -> [ . rule_definition<grammar_bin_terminal> ]]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_53 = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_53 = { new ushort[2] { 0x8A, 0x69 }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_53 = { };

        private static ushort[] p_StateExpectedIDs_54 = { 0x4E, 0x52 };
        private static string[] p_StateExpectedNames_54 = { "_T[terminals]", "_T[rules]" };
        private static string[] p_StateItems_54 = { "[grammar_options -> options { _m97 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_54 = { };
        private static ushort[][] p_StateShiftsOnVariable_54 = { };
        private static ushort[][] p_StateReducsOnTerminal_54 = { new ushort[2] { 0x4E, 0x3C }, new ushort[2] { 0x52, 0x3C } };

        private static ushort[] p_StateExpectedIDs_55 = { 0x12 };
        private static string[] p_StateExpectedNames_55 = { "_T[}]" };
        private static string[] p_StateItems_55 = { "[_m97 -> option _m97 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_55 = { };
        private static ushort[][] p_StateShiftsOnVariable_55 = { };
        private static ushort[][] p_StateReducsOnTerminal_55 = { new ushort[2] { 0x12, 0x4E } };

        private static ushort[] p_StateExpectedIDs_56 = { 0x2D };
        private static string[] p_StateExpectedNames_56 = { "QUOTED_DATA" };
        private static string[] p_StateItems_56 = { "[option -> NAME = . QUOTED_DATA ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_56 = { new ushort[2] { 0x2D, 0x85 } };
        private static ushort[][] p_StateShiftsOnVariable_56 = { };
        private static ushort[][] p_StateReducsOnTerminal_56 = { };

        private static ushort[] p_StateExpectedIDs_57 = { 0x10, 0x50, 0xC, 0xD, 0xE, 0xF, 0x2, 0x12 };
        private static string[] p_StateExpectedNames_57 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_StateItems_57 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_57 = { };
        private static ushort[][] p_StateShiftsOnVariable_57 = { };
        private static ushort[][] p_StateReducsOnTerminal_57 = { new ushort[2] { 0x10, 0x44 }, new ushort[2] { 0x50, 0x44 }, new ushort[2] { 0xC, 0x44 }, new ushort[2] { 0xD, 0x44 }, new ushort[2] { 0xE, 0x44 }, new ushort[2] { 0xF, 0x44 }, new ushort[2] { 0x2, 0x44 }, new ushort[2] { 0x12, 0x44 } };

        private static ushort[] p_StateExpectedIDs_58 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_58 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_58 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { . _m143 }]", "[_m143 -> . cf_rule_simple<grammar_text_terminal> _m143]", "[_m143 -> . cf_rule_template<grammar_text_terminal> _m143]", "[_m143 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_58 = { new ushort[2] { 0xA, 0x89 } };
        private static ushort[][] p_StateShiftsOnVariable_58 = { new ushort[2] { 0x87, 0x86 }, new ushort[2] { 0x78, 0x87 }, new ushort[2] { 0x86, 0x88 } };
        private static ushort[][] p_StateReducsOnTerminal_58 = { new ushort[2] { 0x12, 0x74 } };

        private static ushort[] p_StateExpectedIDs_59 = { 0x12 };
        private static string[] p_StateExpectedNames_59 = { "_T[}]" };
        private static string[] p_StateItems_59 = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { _m183 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_59 = { new ushort[2] { 0x12, 0x8A } };
        private static ushort[][] p_StateShiftsOnVariable_59 = { };
        private static ushort[][] p_StateReducsOnTerminal_59 = { };

        private static ushort[] p_StateExpectedIDs_5A = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_5A = { "_T[}]", "NAME" };
        private static string[] p_StateItems_5A = { "[_m183 -> cf_rule_simple<grammar_bin_terminal> . _m183]", "[_m183 -> . cf_rule_simple<grammar_bin_terminal> _m183]", "[_m183 -> . cf_rule_template<grammar_bin_terminal> _m183]", "[_m183 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_5A = { new ushort[2] { 0xA, 0x5C } };
        private static ushort[][] p_StateShiftsOnVariable_5A = { new ushort[2] { 0x98, 0x8B }, new ushort[2] { 0x89, 0x5A }, new ushort[2] { 0x97, 0x5B } };
        private static ushort[][] p_StateReducsOnTerminal_5A = { new ushort[2] { 0x12, 0x95 } };

        private static ushort[] p_StateExpectedIDs_5B = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_5B = { "_T[}]", "NAME" };
        private static string[] p_StateItems_5B = { "[_m183 -> cf_rule_template<grammar_bin_terminal> . _m183]", "[_m183 -> . cf_rule_simple<grammar_bin_terminal> _m183]", "[_m183 -> . cf_rule_template<grammar_bin_terminal> _m183]", "[_m183 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_5B = { new ushort[2] { 0xA, 0x5C } };
        private static ushort[][] p_StateShiftsOnVariable_5B = { new ushort[2] { 0x98, 0x8C }, new ushort[2] { 0x89, 0x5A }, new ushort[2] { 0x97, 0x5B } };
        private static ushort[][] p_StateReducsOnTerminal_5B = { new ushort[2] { 0x12, 0x95 } };

        private static ushort[] p_StateExpectedIDs_5C = { 0x49, 0x4A };
        private static string[] p_StateExpectedNames_5C = { "_T[->]", "_T[<]" };
        private static string[] p_StateItems_5C = { "[cf_rule_simple<grammar_bin_terminal> -> NAME . -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> NAME . rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params -> . < NAME _m93 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_5C = { new ushort[2] { 0x49, 0x8D }, new ushort[2] { 0x4A, 0x8F } };
        private static ushort[][] p_StateShiftsOnVariable_5C = { new ushort[2] { 0x67, 0x8E } };
        private static ushort[][] p_StateReducsOnTerminal_5C = { };

        private static ushort[] p_StateExpectedIDs_5D = { 0x12 };
        private static string[] p_StateExpectedNames_5D = { "_T[}]" };
        private static string[] p_StateItems_5D = { "[grammar_cs_rules<grammar_text_terminal> -> rules { _m154 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_5D = { new ushort[2] { 0x12, 0x90 } };
        private static ushort[][] p_StateShiftsOnVariable_5D = { };
        private static ushort[][] p_StateReducsOnTerminal_5D = { };

        private static ushort[] p_StateExpectedIDs_5E = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_5E = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_5E = { "[_m154 -> cs_rule_simple<grammar_text_terminal> . _m154]", "[_m154 -> . cs_rule_simple<grammar_text_terminal> _m154]", "[_m154 -> . cs_rule_template<grammar_text_terminal> _m154]", "[_m154 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_5E = { new ushort[2] { 0xD2, 0x61 } };
        private static ushort[][] p_StateShiftsOnVariable_5E = { new ushort[2] { 0xDA, 0x91 }, new ushort[2] { 0xD7, 0x5E }, new ushort[2] { 0xD9, 0x5F }, new ushort[2] { 0xD8, 0x60 } };
        private static ushort[][] p_StateReducsOnTerminal_5E = { new ushort[2] { 0x12, 0x9F }, new ushort[2] { 0xA, 0x9B } };

        private static ushort[] p_StateExpectedIDs_5F = { 0x12, 0xA, 0xD2 };
        private static string[] p_StateExpectedNames_5F = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_StateItems_5F = { "[_m154 -> cs_rule_template<grammar_text_terminal> . _m154]", "[_m154 -> . cs_rule_simple<grammar_text_terminal> _m154]", "[_m154 -> . cs_rule_template<grammar_text_terminal> _m154]", "[_m154 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_5F = { new ushort[2] { 0xD2, 0x61 } };
        private static ushort[][] p_StateShiftsOnVariable_5F = { new ushort[2] { 0xDA, 0x92 }, new ushort[2] { 0xD7, 0x5E }, new ushort[2] { 0xD9, 0x5F }, new ushort[2] { 0xD8, 0x60 } };
        private static ushort[][] p_StateReducsOnTerminal_5F = { new ushort[2] { 0x12, 0x9F }, new ushort[2] { 0xA, 0x9B } };

        private static ushort[] p_StateExpectedIDs_60 = { 0xA };
        private static string[] p_StateExpectedNames_60 = { "NAME" };
        private static string[] p_StateItems_60 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> . NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> . NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_60 = { new ushort[2] { 0xA, 0x93 } };
        private static ushort[][] p_StateShiftsOnVariable_60 = { };
        private static ushort[][] p_StateReducsOnTerminal_60 = { };

        private static ushort[] p_StateExpectedIDs_61 = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_61 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_61 = { "[cs_rule_context<grammar_text_terminal> -> [ . rule_definition<grammar_text_terminal> ]]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_61 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_61 = { new ushort[2] { 0x79, 0x94 }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_61 = { };

        private static ushort[] p_StateExpectedIDs_62 = { 0x52 };
        private static string[] p_StateExpectedNames_62 = { "_T[rules]" };
        private static string[] p_StateItems_62 = { "[grammar_terminals -> terminals { _m101 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_62 = { };
        private static ushort[][] p_StateShiftsOnVariable_62 = { };
        private static ushort[][] p_StateReducsOnTerminal_62 = { new ushort[2] { 0x52, 0x3D } };

        private static ushort[] p_StateExpectedIDs_63 = { 0x12 };
        private static string[] p_StateExpectedNames_63 = { "_T[}]" };
        private static string[] p_StateItems_63 = { "[_m101 -> terminal _m101 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_63 = { };
        private static ushort[][] p_StateShiftsOnVariable_63 = { };
        private static ushort[][] p_StateReducsOnTerminal_63 = { new ushort[2] { 0x12, 0x50 } };

        private static ushort[] p_StateExpectedIDs_64 = { 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_64 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_64 = { "[terminal -> NAME -> . terminal_definition terminal_subgrammar ;]", "[terminal_definition -> . terminal_def_restrict _m85]", "[terminal_def_restrict -> . terminal_def_fragment _m83]", "[terminal_def_fragment -> . terminal_def_repetition _m81]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_64 = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_64 = { new ushort[2] { 0x61, 0xA4 }, new ushort[2] { 0x60, 0xA5 }, new ushort[2] { 0x5F, 0xA6 }, new ushort[2] { 0x5E, 0xA7 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_64 = { };

        private static ushort[] p_StateExpectedIDs_65 = { 0x12 };
        private static string[] p_StateExpectedNames_65 = { "_T[}]" };
        private static string[] p_StateItems_65 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { _m172 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_65 = { };
        private static ushort[][] p_StateShiftsOnVariable_65 = { };
        private static ushort[][] p_StateReducsOnTerminal_65 = { new ushort[2] { 0x12, 0xA0 } };

        private static ushort[] p_StateExpectedIDs_66 = { 0x12 };
        private static string[] p_StateExpectedNames_66 = { "_T[}]" };
        private static string[] p_StateItems_66 = { "[_m172 -> cs_rule_simple<grammar_bin_terminal> _m172 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_66 = { };
        private static ushort[][] p_StateShiftsOnVariable_66 = { };
        private static ushort[][] p_StateReducsOnTerminal_66 = { new ushort[2] { 0x12, 0xA5 } };

        private static ushort[] p_StateExpectedIDs_67 = { 0x12 };
        private static string[] p_StateExpectedNames_67 = { "_T[}]" };
        private static string[] p_StateItems_67 = { "[_m172 -> cs_rule_template<grammar_bin_terminal> _m172 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_67 = { };
        private static ushort[][] p_StateShiftsOnVariable_67 = { };
        private static ushort[][] p_StateReducsOnTerminal_67 = { new ushort[2] { 0x12, 0xA6 } };

        private static ushort[] p_StateExpectedIDs_68 = { 0x49, 0x4A, 0xD2 };
        private static string[] p_StateExpectedNames_68 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] p_StateItems_68 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME . cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME . cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_68 = { new ushort[2] { 0xD2, 0x53 } };
        private static ushort[][] p_StateShiftsOnVariable_68 = { new ushort[2] { 0xDD, 0xB7 } };
        private static ushort[][] p_StateReducsOnTerminal_68 = { new ushort[2] { 0x49, 0xA3 }, new ushort[2] { 0x4A, 0xA3 } };

        private static ushort[] p_StateExpectedIDs_69 = { 0xD3 };
        private static string[] p_StateExpectedNames_69 = { "_T[]]" };
        private static string[] p_StateItems_69 = { "[cs_rule_context<grammar_bin_terminal> -> [ rule_definition<grammar_bin_terminal> . ]]" };
        private static ushort[][] p_StateShiftsOnTerminal_69 = { new ushort[2] { 0xD3, 0xB8 } };
        private static ushort[][] p_StateShiftsOnVariable_69 = { };
        private static ushort[][] p_StateReducsOnTerminal_69 = { };

        private static ushort[] p_StateExpectedIDs_6A = { 0xD3, 0x42, 0x3F, 0x47 };
        private static string[] p_StateExpectedNames_6A = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_StateItems_6A = { "[rule_definition<grammar_bin_terminal> -> rule_def_restrict<grammar_bin_terminal> . _m178]", "[_m178 -> . | rule_def_restrict<grammar_bin_terminal> _m178]", "[_m178 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6A = { new ushort[2] { 0x47, 0xBA } };
        private static ushort[][] p_StateShiftsOnVariable_6A = { new ushort[2] { 0x96, 0xB9 } };
        private static ushort[][] p_StateReducsOnTerminal_6A = { new ushort[2] { 0xD3, 0x91 }, new ushort[2] { 0x42, 0x91 }, new ushort[2] { 0x3F, 0x91 } };

        private static ushort[] p_StateExpectedIDs_6B = { 0x47, 0xD3, 0x42, 0x3F, 0x46 };
        private static string[] p_StateExpectedNames_6B = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_StateItems_6B = { "[rule_def_restrict<grammar_bin_terminal> -> rule_def_fragment<grammar_bin_terminal> . _m176]", "[_m176 -> . - rule_def_fragment<grammar_bin_terminal> _m176]", "[_m176 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6B = { new ushort[2] { 0x46, 0xBC } };
        private static ushort[][] p_StateShiftsOnVariable_6B = { new ushort[2] { 0x95, 0xBB } };
        private static ushort[][] p_StateReducsOnTerminal_6B = { new ushort[2] { 0x47, 0x8F }, new ushort[2] { 0xD3, 0x8F }, new ushort[2] { 0x42, 0x8F }, new ushort[2] { 0x3F, 0x8F } };

        private static ushort[] p_StateExpectedIDs_6C = { 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_6C = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_6C = { "[rule_def_fragment<grammar_bin_terminal> -> rule_def_repetition<grammar_bin_terminal> . _m174]", "[_m174 -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[_m174 -> . ]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_6C = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_6C = { new ushort[2] { 0x94, 0xBD }, new ushort[2] { 0x8D, 0xBE }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_6C = { new ushort[2] { 0x46, 0x8D }, new ushort[2] { 0x47, 0x8D }, new ushort[2] { 0xD3, 0x8D }, new ushort[2] { 0x42, 0x8D }, new ushort[2] { 0x3F, 0x8D } };

        private static ushort[] p_StateExpectedIDs_6D = { 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x43, 0x44, 0x45 };
        private static string[] p_StateExpectedNames_6D = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_StateItems_6D = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . *]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . +]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . ?]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6D = { new ushort[2] { 0x43, 0xBF }, new ushort[2] { 0x44, 0xC0 }, new ushort[2] { 0x45, 0xC1 } };
        private static ushort[][] p_StateShiftsOnVariable_6D = { };
        private static ushort[][] p_StateReducsOnTerminal_6D = { new ushort[2] { 0x41, 0x7D }, new ushort[2] { 0x2D, 0x7D }, new ushort[2] { 0xA, 0x7D }, new ushort[2] { 0x32, 0x7D }, new ushort[2] { 0x33, 0x7D }, new ushort[2] { 0x11, 0x7D }, new ushort[2] { 0x34, 0x7D }, new ushort[2] { 0x35, 0x7D }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x38, 0x7D }, new ushort[2] { 0x39, 0x7D }, new ushort[2] { 0x3A, 0x7D }, new ushort[2] { 0x3B, 0x7D }, new ushort[2] { 0x3C, 0x7D }, new ushort[2] { 0x3D, 0x7D }, new ushort[2] { 0x46, 0x7D }, new ushort[2] { 0x47, 0x7D }, new ushort[2] { 0xD3, 0x7D }, new ushort[2] { 0x42, 0x7D }, new ushort[2] { 0x3F, 0x7D } };

        private static ushort[] p_StateExpectedIDs_6E = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x53, 0x54 };
        private static string[] p_StateExpectedNames_6E = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[^]", "_T[!]" };
        private static string[] p_StateItems_6E = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . ^]", "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . !]", "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6E = { new ushort[2] { 0x53, 0xC2 }, new ushort[2] { 0x54, 0xC3 } };
        private static ushort[][] p_StateShiftsOnVariable_6E = { };
        private static ushort[][] p_StateReducsOnTerminal_6E = { new ushort[2] { 0x43, 0x80 }, new ushort[2] { 0x44, 0x80 }, new ushort[2] { 0x45, 0x80 }, new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0x2D, 0x80 }, new ushort[2] { 0xA, 0x80 }, new ushort[2] { 0x32, 0x80 }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x11, 0x80 }, new ushort[2] { 0x34, 0x80 }, new ushort[2] { 0x35, 0x80 }, new ushort[2] { 0x36, 0x80 }, new ushort[2] { 0x37, 0x80 }, new ushort[2] { 0x38, 0x80 }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x80 }, new ushort[2] { 0x3B, 0x80 }, new ushort[2] { 0x3C, 0x80 }, new ushort[2] { 0x3D, 0x80 }, new ushort[2] { 0x46, 0x80 }, new ushort[2] { 0x47, 0x80 }, new ushort[2] { 0xD3, 0x80 }, new ushort[2] { 0x42, 0x80 }, new ushort[2] { 0x3F, 0x80 } };

        private static ushort[] p_StateExpectedIDs_6F = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_6F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_6F = { "[rule_def_element<grammar_bin_terminal> -> rule_def_atom<grammar_bin_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6F = { };
        private static ushort[][] p_StateShiftsOnVariable_6F = { };
        private static ushort[][] p_StateReducsOnTerminal_6F = { new ushort[2] { 0x53, 0x81 }, new ushort[2] { 0x54, 0x81 }, new ushort[2] { 0x43, 0x81 }, new ushort[2] { 0x44, 0x81 }, new ushort[2] { 0x45, 0x81 }, new ushort[2] { 0x41, 0x81 }, new ushort[2] { 0x2D, 0x81 }, new ushort[2] { 0xA, 0x81 }, new ushort[2] { 0x32, 0x81 }, new ushort[2] { 0x33, 0x81 }, new ushort[2] { 0x11, 0x81 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x81 }, new ushort[2] { 0x36, 0x81 }, new ushort[2] { 0x37, 0x81 }, new ushort[2] { 0x38, 0x81 }, new ushort[2] { 0x39, 0x81 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x81 }, new ushort[2] { 0x3C, 0x81 }, new ushort[2] { 0x3D, 0x81 }, new ushort[2] { 0x46, 0x81 }, new ushort[2] { 0x47, 0x81 }, new ushort[2] { 0xD3, 0x81 }, new ushort[2] { 0x42, 0x81 }, new ushort[2] { 0x3F, 0x81 } };

        private static ushort[] p_StateExpectedIDs_70 = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_70 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_70 = { "[rule_def_element<grammar_bin_terminal> -> ( . rule_definition<grammar_bin_terminal> )]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_70 = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_70 = { new ushort[2] { 0x8A, 0xC4 }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_70 = { };

        private static ushort[] p_StateExpectedIDs_71 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_71 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_71 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_action . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_71 = { };
        private static ushort[][] p_StateShiftsOnVariable_71 = { };
        private static ushort[][] p_StateReducsOnTerminal_71 = { new ushort[2] { 0x53, 0x83 }, new ushort[2] { 0x54, 0x83 }, new ushort[2] { 0x43, 0x83 }, new ushort[2] { 0x44, 0x83 }, new ushort[2] { 0x45, 0x83 }, new ushort[2] { 0x41, 0x83 }, new ushort[2] { 0x2D, 0x83 }, new ushort[2] { 0xA, 0x83 }, new ushort[2] { 0x32, 0x83 }, new ushort[2] { 0x33, 0x83 }, new ushort[2] { 0x11, 0x83 }, new ushort[2] { 0x34, 0x83 }, new ushort[2] { 0x35, 0x83 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x83 }, new ushort[2] { 0x38, 0x83 }, new ushort[2] { 0x39, 0x83 }, new ushort[2] { 0x3A, 0x83 }, new ushort[2] { 0x3B, 0x83 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x83 }, new ushort[2] { 0x46, 0x83 }, new ushort[2] { 0x47, 0x83 }, new ushort[2] { 0xD3, 0x83 }, new ushort[2] { 0x42, 0x83 }, new ushort[2] { 0x3F, 0x83 }, new ushort[2] { 0x4C, 0x83 }, new ushort[2] { 0x4B, 0x83 } };

        private static ushort[] p_StateExpectedIDs_72 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_72 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_72 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_virtual . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_72 = { };
        private static ushort[][] p_StateShiftsOnVariable_72 = { };
        private static ushort[][] p_StateReducsOnTerminal_72 = { new ushort[2] { 0x53, 0x84 }, new ushort[2] { 0x54, 0x84 }, new ushort[2] { 0x43, 0x84 }, new ushort[2] { 0x44, 0x84 }, new ushort[2] { 0x45, 0x84 }, new ushort[2] { 0x41, 0x84 }, new ushort[2] { 0x2D, 0x84 }, new ushort[2] { 0xA, 0x84 }, new ushort[2] { 0x32, 0x84 }, new ushort[2] { 0x33, 0x84 }, new ushort[2] { 0x11, 0x84 }, new ushort[2] { 0x34, 0x84 }, new ushort[2] { 0x35, 0x84 }, new ushort[2] { 0x36, 0x84 }, new ushort[2] { 0x37, 0x84 }, new ushort[2] { 0x38, 0x84 }, new ushort[2] { 0x39, 0x84 }, new ushort[2] { 0x3A, 0x84 }, new ushort[2] { 0x3B, 0x84 }, new ushort[2] { 0x3C, 0x84 }, new ushort[2] { 0x3D, 0x84 }, new ushort[2] { 0x46, 0x84 }, new ushort[2] { 0x47, 0x84 }, new ushort[2] { 0xD3, 0x84 }, new ushort[2] { 0x42, 0x84 }, new ushort[2] { 0x3F, 0x84 }, new ushort[2] { 0x4C, 0x84 }, new ushort[2] { 0x4B, 0x84 } };

        private static ushort[] p_StateExpectedIDs_73 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_73 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_73 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_ref_simple . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_73 = { };
        private static ushort[][] p_StateShiftsOnVariable_73 = { };
        private static ushort[][] p_StateReducsOnTerminal_73 = { new ushort[2] { 0x53, 0x85 }, new ushort[2] { 0x54, 0x85 }, new ushort[2] { 0x43, 0x85 }, new ushort[2] { 0x44, 0x85 }, new ushort[2] { 0x45, 0x85 }, new ushort[2] { 0x41, 0x85 }, new ushort[2] { 0x2D, 0x85 }, new ushort[2] { 0xA, 0x85 }, new ushort[2] { 0x32, 0x85 }, new ushort[2] { 0x33, 0x85 }, new ushort[2] { 0x11, 0x85 }, new ushort[2] { 0x34, 0x85 }, new ushort[2] { 0x35, 0x85 }, new ushort[2] { 0x36, 0x85 }, new ushort[2] { 0x37, 0x85 }, new ushort[2] { 0x38, 0x85 }, new ushort[2] { 0x39, 0x85 }, new ushort[2] { 0x3A, 0x85 }, new ushort[2] { 0x3B, 0x85 }, new ushort[2] { 0x3C, 0x85 }, new ushort[2] { 0x3D, 0x85 }, new ushort[2] { 0x46, 0x85 }, new ushort[2] { 0x47, 0x85 }, new ushort[2] { 0xD3, 0x85 }, new ushort[2] { 0x42, 0x85 }, new ushort[2] { 0x3F, 0x85 }, new ushort[2] { 0x4C, 0x85 }, new ushort[2] { 0x4B, 0x85 } };

        private static ushort[] p_StateExpectedIDs_74 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_74 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_74 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_ref_template<grammar_bin_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_74 = { };
        private static ushort[][] p_StateShiftsOnVariable_74 = { };
        private static ushort[][] p_StateReducsOnTerminal_74 = { new ushort[2] { 0x53, 0x86 }, new ushort[2] { 0x54, 0x86 }, new ushort[2] { 0x43, 0x86 }, new ushort[2] { 0x44, 0x86 }, new ushort[2] { 0x45, 0x86 }, new ushort[2] { 0x41, 0x86 }, new ushort[2] { 0x2D, 0x86 }, new ushort[2] { 0xA, 0x86 }, new ushort[2] { 0x32, 0x86 }, new ushort[2] { 0x33, 0x86 }, new ushort[2] { 0x11, 0x86 }, new ushort[2] { 0x34, 0x86 }, new ushort[2] { 0x35, 0x86 }, new ushort[2] { 0x36, 0x86 }, new ushort[2] { 0x37, 0x86 }, new ushort[2] { 0x38, 0x86 }, new ushort[2] { 0x39, 0x86 }, new ushort[2] { 0x3A, 0x86 }, new ushort[2] { 0x3B, 0x86 }, new ushort[2] { 0x3C, 0x86 }, new ushort[2] { 0x3D, 0x86 }, new ushort[2] { 0x46, 0x86 }, new ushort[2] { 0x47, 0x86 }, new ushort[2] { 0xD3, 0x86 }, new ushort[2] { 0x42, 0x86 }, new ushort[2] { 0x3F, 0x86 }, new ushort[2] { 0x4C, 0x86 }, new ushort[2] { 0x4B, 0x86 } };

        private static ushort[] p_StateExpectedIDs_75 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_75 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_75 = { "[rule_def_atom<grammar_bin_terminal> -> grammar_bin_terminal . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_75 = { };
        private static ushort[][] p_StateShiftsOnVariable_75 = { };
        private static ushort[][] p_StateReducsOnTerminal_75 = { new ushort[2] { 0x53, 0x87 }, new ushort[2] { 0x54, 0x87 }, new ushort[2] { 0x43, 0x87 }, new ushort[2] { 0x44, 0x87 }, new ushort[2] { 0x45, 0x87 }, new ushort[2] { 0x41, 0x87 }, new ushort[2] { 0x2D, 0x87 }, new ushort[2] { 0xA, 0x87 }, new ushort[2] { 0x32, 0x87 }, new ushort[2] { 0x33, 0x87 }, new ushort[2] { 0x11, 0x87 }, new ushort[2] { 0x34, 0x87 }, new ushort[2] { 0x35, 0x87 }, new ushort[2] { 0x36, 0x87 }, new ushort[2] { 0x37, 0x87 }, new ushort[2] { 0x38, 0x87 }, new ushort[2] { 0x39, 0x87 }, new ushort[2] { 0x3A, 0x87 }, new ushort[2] { 0x3B, 0x87 }, new ushort[2] { 0x3C, 0x87 }, new ushort[2] { 0x3D, 0x87 }, new ushort[2] { 0x46, 0x87 }, new ushort[2] { 0x47, 0x87 }, new ushort[2] { 0xD3, 0x87 }, new ushort[2] { 0x42, 0x87 }, new ushort[2] { 0x3F, 0x87 }, new ushort[2] { 0x4C, 0x87 }, new ushort[2] { 0x4B, 0x87 } };

        private static ushort[] p_StateExpectedIDs_76 = { 0xA };
        private static string[] p_StateExpectedNames_76 = { "NAME" };
        private static string[] p_StateItems_76 = { "[rule_sym_action -> { . qualified_name }]", "[qualified_name -> . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_76 = { new ushort[2] { 0xA, 0x18 } };
        private static ushort[][] p_StateShiftsOnVariable_76 = { new ushort[2] { 0x13, 0xC5 } };
        private static ushort[][] p_StateReducsOnTerminal_76 = { };

        private static ushort[] p_StateExpectedIDs_77 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x2E, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_77 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_TERMINAL_TEXT", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_77 = { "[rule_sym_virtual -> QUOTED_DATA . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_77 = { };
        private static ushort[][] p_StateShiftsOnVariable_77 = { };
        private static ushort[][] p_StateReducsOnTerminal_77 = { new ushort[2] { 0x53, 0x2C }, new ushort[2] { 0x54, 0x2C }, new ushort[2] { 0x43, 0x2C }, new ushort[2] { 0x44, 0x2C }, new ushort[2] { 0x45, 0x2C }, new ushort[2] { 0x41, 0x2C }, new ushort[2] { 0x2D, 0x2C }, new ushort[2] { 0xA, 0x2C }, new ushort[2] { 0x32, 0x2C }, new ushort[2] { 0x33, 0x2C }, new ushort[2] { 0x11, 0x2C }, new ushort[2] { 0x34, 0x2C }, new ushort[2] { 0x35, 0x2C }, new ushort[2] { 0x36, 0x2C }, new ushort[2] { 0x37, 0x2C }, new ushort[2] { 0x38, 0x2C }, new ushort[2] { 0x39, 0x2C }, new ushort[2] { 0x3A, 0x2C }, new ushort[2] { 0x3B, 0x2C }, new ushort[2] { 0x3C, 0x2C }, new ushort[2] { 0x3D, 0x2C }, new ushort[2] { 0x46, 0x2C }, new ushort[2] { 0x47, 0x2C }, new ushort[2] { 0xD3, 0x2C }, new ushort[2] { 0x2E, 0x2C }, new ushort[2] { 0x42, 0x2C }, new ushort[2] { 0x3F, 0x2C }, new ushort[2] { 0x4C, 0x2C }, new ushort[2] { 0x4B, 0x2C } };

        private static ushort[] p_StateExpectedIDs_78 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B, 0x4A };
        private static string[] p_StateExpectedNames_78 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]", "_T[<]" };
        private static string[] p_StateItems_78 = { "[rule_sym_ref_simple -> NAME . ]", "[rule_sym_ref_template<grammar_bin_terminal> -> NAME . rule_sym_ref_params<grammar_bin_terminal>]", "[rule_sym_ref_params<grammar_bin_terminal> -> . < rule_def_atom<grammar_bin_terminal> _m165 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_78 = { new ushort[2] { 0x4A, 0xC7 } };
        private static ushort[][] p_StateShiftsOnVariable_78 = { new ushort[2] { 0x92, 0xC6 } };
        private static ushort[][] p_StateReducsOnTerminal_78 = { new ushort[2] { 0x53, 0x2D }, new ushort[2] { 0x54, 0x2D }, new ushort[2] { 0x43, 0x2D }, new ushort[2] { 0x44, 0x2D }, new ushort[2] { 0x45, 0x2D }, new ushort[2] { 0x41, 0x2D }, new ushort[2] { 0x2D, 0x2D }, new ushort[2] { 0xA, 0x2D }, new ushort[2] { 0x32, 0x2D }, new ushort[2] { 0x33, 0x2D }, new ushort[2] { 0x11, 0x2D }, new ushort[2] { 0x34, 0x2D }, new ushort[2] { 0x35, 0x2D }, new ushort[2] { 0x36, 0x2D }, new ushort[2] { 0x37, 0x2D }, new ushort[2] { 0x38, 0x2D }, new ushort[2] { 0x39, 0x2D }, new ushort[2] { 0x3A, 0x2D }, new ushort[2] { 0x3B, 0x2D }, new ushort[2] { 0x3C, 0x2D }, new ushort[2] { 0x3D, 0x2D }, new ushort[2] { 0x46, 0x2D }, new ushort[2] { 0x47, 0x2D }, new ushort[2] { 0xD3, 0x2D }, new ushort[2] { 0x42, 0x2D }, new ushort[2] { 0x3F, 0x2D }, new ushort[2] { 0x4C, 0x2D }, new ushort[2] { 0x4B, 0x2D } };

        private static ushort[] p_StateExpectedIDs_79 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_79 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_79 = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT8 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_79 = { };
        private static ushort[][] p_StateShiftsOnVariable_79 = { };
        private static ushort[][] p_StateReducsOnTerminal_79 = { new ushort[2] { 0x53, 0x2F }, new ushort[2] { 0x54, 0x2F }, new ushort[2] { 0x43, 0x2F }, new ushort[2] { 0x44, 0x2F }, new ushort[2] { 0x45, 0x2F }, new ushort[2] { 0x41, 0x2F }, new ushort[2] { 0x2D, 0x2F }, new ushort[2] { 0xA, 0x2F }, new ushort[2] { 0x32, 0x2F }, new ushort[2] { 0x33, 0x2F }, new ushort[2] { 0x11, 0x2F }, new ushort[2] { 0x34, 0x2F }, new ushort[2] { 0x35, 0x2F }, new ushort[2] { 0x36, 0x2F }, new ushort[2] { 0x37, 0x2F }, new ushort[2] { 0x38, 0x2F }, new ushort[2] { 0x39, 0x2F }, new ushort[2] { 0x3A, 0x2F }, new ushort[2] { 0x3B, 0x2F }, new ushort[2] { 0x3C, 0x2F }, new ushort[2] { 0x3D, 0x2F }, new ushort[2] { 0x46, 0x2F }, new ushort[2] { 0x47, 0x2F }, new ushort[2] { 0xD3, 0x2F }, new ushort[2] { 0x42, 0x2F }, new ushort[2] { 0x3F, 0x2F }, new ushort[2] { 0x4C, 0x2F }, new ushort[2] { 0x4B, 0x2F } };

        private static ushort[] p_StateExpectedIDs_7A = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7A = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7A = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT16 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7A = { };
        private static ushort[][] p_StateShiftsOnVariable_7A = { };
        private static ushort[][] p_StateReducsOnTerminal_7A = { new ushort[2] { 0x53, 0x30 }, new ushort[2] { 0x54, 0x30 }, new ushort[2] { 0x43, 0x30 }, new ushort[2] { 0x44, 0x30 }, new ushort[2] { 0x45, 0x30 }, new ushort[2] { 0x41, 0x30 }, new ushort[2] { 0x2D, 0x30 }, new ushort[2] { 0xA, 0x30 }, new ushort[2] { 0x32, 0x30 }, new ushort[2] { 0x33, 0x30 }, new ushort[2] { 0x11, 0x30 }, new ushort[2] { 0x34, 0x30 }, new ushort[2] { 0x35, 0x30 }, new ushort[2] { 0x36, 0x30 }, new ushort[2] { 0x37, 0x30 }, new ushort[2] { 0x38, 0x30 }, new ushort[2] { 0x39, 0x30 }, new ushort[2] { 0x3A, 0x30 }, new ushort[2] { 0x3B, 0x30 }, new ushort[2] { 0x3C, 0x30 }, new ushort[2] { 0x3D, 0x30 }, new ushort[2] { 0x46, 0x30 }, new ushort[2] { 0x47, 0x30 }, new ushort[2] { 0xD3, 0x30 }, new ushort[2] { 0x42, 0x30 }, new ushort[2] { 0x3F, 0x30 }, new ushort[2] { 0x4C, 0x30 }, new ushort[2] { 0x4B, 0x30 } };

        private static ushort[] p_StateExpectedIDs_7B = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7B = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7B = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT32 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7B = { };
        private static ushort[][] p_StateShiftsOnVariable_7B = { };
        private static ushort[][] p_StateReducsOnTerminal_7B = { new ushort[2] { 0x53, 0x31 }, new ushort[2] { 0x54, 0x31 }, new ushort[2] { 0x43, 0x31 }, new ushort[2] { 0x44, 0x31 }, new ushort[2] { 0x45, 0x31 }, new ushort[2] { 0x41, 0x31 }, new ushort[2] { 0x2D, 0x31 }, new ushort[2] { 0xA, 0x31 }, new ushort[2] { 0x32, 0x31 }, new ushort[2] { 0x33, 0x31 }, new ushort[2] { 0x11, 0x31 }, new ushort[2] { 0x34, 0x31 }, new ushort[2] { 0x35, 0x31 }, new ushort[2] { 0x36, 0x31 }, new ushort[2] { 0x37, 0x31 }, new ushort[2] { 0x38, 0x31 }, new ushort[2] { 0x39, 0x31 }, new ushort[2] { 0x3A, 0x31 }, new ushort[2] { 0x3B, 0x31 }, new ushort[2] { 0x3C, 0x31 }, new ushort[2] { 0x3D, 0x31 }, new ushort[2] { 0x46, 0x31 }, new ushort[2] { 0x47, 0x31 }, new ushort[2] { 0xD3, 0x31 }, new ushort[2] { 0x42, 0x31 }, new ushort[2] { 0x3F, 0x31 }, new ushort[2] { 0x4C, 0x31 }, new ushort[2] { 0x4B, 0x31 } };

        private static ushort[] p_StateExpectedIDs_7C = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7C = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7C = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT64 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7C = { };
        private static ushort[][] p_StateShiftsOnVariable_7C = { };
        private static ushort[][] p_StateReducsOnTerminal_7C = { new ushort[2] { 0x53, 0x32 }, new ushort[2] { 0x54, 0x32 }, new ushort[2] { 0x43, 0x32 }, new ushort[2] { 0x44, 0x32 }, new ushort[2] { 0x45, 0x32 }, new ushort[2] { 0x41, 0x32 }, new ushort[2] { 0x2D, 0x32 }, new ushort[2] { 0xA, 0x32 }, new ushort[2] { 0x32, 0x32 }, new ushort[2] { 0x33, 0x32 }, new ushort[2] { 0x11, 0x32 }, new ushort[2] { 0x34, 0x32 }, new ushort[2] { 0x35, 0x32 }, new ushort[2] { 0x36, 0x32 }, new ushort[2] { 0x37, 0x32 }, new ushort[2] { 0x38, 0x32 }, new ushort[2] { 0x39, 0x32 }, new ushort[2] { 0x3A, 0x32 }, new ushort[2] { 0x3B, 0x32 }, new ushort[2] { 0x3C, 0x32 }, new ushort[2] { 0x3D, 0x32 }, new ushort[2] { 0x46, 0x32 }, new ushort[2] { 0x47, 0x32 }, new ushort[2] { 0xD3, 0x32 }, new ushort[2] { 0x42, 0x32 }, new ushort[2] { 0x3F, 0x32 }, new ushort[2] { 0x4C, 0x32 }, new ushort[2] { 0x4B, 0x32 } };

        private static ushort[] p_StateExpectedIDs_7D = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7D = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7D = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT128 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7D = { };
        private static ushort[][] p_StateShiftsOnVariable_7D = { };
        private static ushort[][] p_StateReducsOnTerminal_7D = { new ushort[2] { 0x53, 0x33 }, new ushort[2] { 0x54, 0x33 }, new ushort[2] { 0x43, 0x33 }, new ushort[2] { 0x44, 0x33 }, new ushort[2] { 0x45, 0x33 }, new ushort[2] { 0x41, 0x33 }, new ushort[2] { 0x2D, 0x33 }, new ushort[2] { 0xA, 0x33 }, new ushort[2] { 0x32, 0x33 }, new ushort[2] { 0x33, 0x33 }, new ushort[2] { 0x11, 0x33 }, new ushort[2] { 0x34, 0x33 }, new ushort[2] { 0x35, 0x33 }, new ushort[2] { 0x36, 0x33 }, new ushort[2] { 0x37, 0x33 }, new ushort[2] { 0x38, 0x33 }, new ushort[2] { 0x39, 0x33 }, new ushort[2] { 0x3A, 0x33 }, new ushort[2] { 0x3B, 0x33 }, new ushort[2] { 0x3C, 0x33 }, new ushort[2] { 0x3D, 0x33 }, new ushort[2] { 0x46, 0x33 }, new ushort[2] { 0x47, 0x33 }, new ushort[2] { 0xD3, 0x33 }, new ushort[2] { 0x42, 0x33 }, new ushort[2] { 0x3F, 0x33 }, new ushort[2] { 0x4C, 0x33 }, new ushort[2] { 0x4B, 0x33 } };

        private static ushort[] p_StateExpectedIDs_7E = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7E = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7E = { "[grammar_bin_terminal -> SYMBOL_VALUE_BINARY . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7E = { };
        private static ushort[][] p_StateShiftsOnVariable_7E = { };
        private static ushort[][] p_StateReducsOnTerminal_7E = { new ushort[2] { 0x53, 0x34 }, new ushort[2] { 0x54, 0x34 }, new ushort[2] { 0x43, 0x34 }, new ushort[2] { 0x44, 0x34 }, new ushort[2] { 0x45, 0x34 }, new ushort[2] { 0x41, 0x34 }, new ushort[2] { 0x2D, 0x34 }, new ushort[2] { 0xA, 0x34 }, new ushort[2] { 0x32, 0x34 }, new ushort[2] { 0x33, 0x34 }, new ushort[2] { 0x11, 0x34 }, new ushort[2] { 0x34, 0x34 }, new ushort[2] { 0x35, 0x34 }, new ushort[2] { 0x36, 0x34 }, new ushort[2] { 0x37, 0x34 }, new ushort[2] { 0x38, 0x34 }, new ushort[2] { 0x39, 0x34 }, new ushort[2] { 0x3A, 0x34 }, new ushort[2] { 0x3B, 0x34 }, new ushort[2] { 0x3C, 0x34 }, new ushort[2] { 0x3D, 0x34 }, new ushort[2] { 0x46, 0x34 }, new ushort[2] { 0x47, 0x34 }, new ushort[2] { 0xD3, 0x34 }, new ushort[2] { 0x42, 0x34 }, new ushort[2] { 0x3F, 0x34 }, new ushort[2] { 0x4C, 0x34 }, new ushort[2] { 0x4B, 0x34 } };

        private static ushort[] p_StateExpectedIDs_7F = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_7F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_7F = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT8 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7F = { };
        private static ushort[][] p_StateShiftsOnVariable_7F = { };
        private static ushort[][] p_StateReducsOnTerminal_7F = { new ushort[2] { 0x53, 0x35 }, new ushort[2] { 0x54, 0x35 }, new ushort[2] { 0x43, 0x35 }, new ushort[2] { 0x44, 0x35 }, new ushort[2] { 0x45, 0x35 }, new ushort[2] { 0x41, 0x35 }, new ushort[2] { 0x2D, 0x35 }, new ushort[2] { 0xA, 0x35 }, new ushort[2] { 0x32, 0x35 }, new ushort[2] { 0x33, 0x35 }, new ushort[2] { 0x11, 0x35 }, new ushort[2] { 0x34, 0x35 }, new ushort[2] { 0x35, 0x35 }, new ushort[2] { 0x36, 0x35 }, new ushort[2] { 0x37, 0x35 }, new ushort[2] { 0x38, 0x35 }, new ushort[2] { 0x39, 0x35 }, new ushort[2] { 0x3A, 0x35 }, new ushort[2] { 0x3B, 0x35 }, new ushort[2] { 0x3C, 0x35 }, new ushort[2] { 0x3D, 0x35 }, new ushort[2] { 0x46, 0x35 }, new ushort[2] { 0x47, 0x35 }, new ushort[2] { 0xD3, 0x35 }, new ushort[2] { 0x42, 0x35 }, new ushort[2] { 0x3F, 0x35 }, new ushort[2] { 0x4C, 0x35 }, new ushort[2] { 0x4B, 0x35 } };

        private static ushort[] p_StateExpectedIDs_80 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_80 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_80 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT16 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_80 = { };
        private static ushort[][] p_StateShiftsOnVariable_80 = { };
        private static ushort[][] p_StateReducsOnTerminal_80 = { new ushort[2] { 0x53, 0x36 }, new ushort[2] { 0x54, 0x36 }, new ushort[2] { 0x43, 0x36 }, new ushort[2] { 0x44, 0x36 }, new ushort[2] { 0x45, 0x36 }, new ushort[2] { 0x41, 0x36 }, new ushort[2] { 0x2D, 0x36 }, new ushort[2] { 0xA, 0x36 }, new ushort[2] { 0x32, 0x36 }, new ushort[2] { 0x33, 0x36 }, new ushort[2] { 0x11, 0x36 }, new ushort[2] { 0x34, 0x36 }, new ushort[2] { 0x35, 0x36 }, new ushort[2] { 0x36, 0x36 }, new ushort[2] { 0x37, 0x36 }, new ushort[2] { 0x38, 0x36 }, new ushort[2] { 0x39, 0x36 }, new ushort[2] { 0x3A, 0x36 }, new ushort[2] { 0x3B, 0x36 }, new ushort[2] { 0x3C, 0x36 }, new ushort[2] { 0x3D, 0x36 }, new ushort[2] { 0x46, 0x36 }, new ushort[2] { 0x47, 0x36 }, new ushort[2] { 0xD3, 0x36 }, new ushort[2] { 0x42, 0x36 }, new ushort[2] { 0x3F, 0x36 }, new ushort[2] { 0x4C, 0x36 }, new ushort[2] { 0x4B, 0x36 } };

        private static ushort[] p_StateExpectedIDs_81 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_81 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_81 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT32 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_81 = { };
        private static ushort[][] p_StateShiftsOnVariable_81 = { };
        private static ushort[][] p_StateReducsOnTerminal_81 = { new ushort[2] { 0x53, 0x37 }, new ushort[2] { 0x54, 0x37 }, new ushort[2] { 0x43, 0x37 }, new ushort[2] { 0x44, 0x37 }, new ushort[2] { 0x45, 0x37 }, new ushort[2] { 0x41, 0x37 }, new ushort[2] { 0x2D, 0x37 }, new ushort[2] { 0xA, 0x37 }, new ushort[2] { 0x32, 0x37 }, new ushort[2] { 0x33, 0x37 }, new ushort[2] { 0x11, 0x37 }, new ushort[2] { 0x34, 0x37 }, new ushort[2] { 0x35, 0x37 }, new ushort[2] { 0x36, 0x37 }, new ushort[2] { 0x37, 0x37 }, new ushort[2] { 0x38, 0x37 }, new ushort[2] { 0x39, 0x37 }, new ushort[2] { 0x3A, 0x37 }, new ushort[2] { 0x3B, 0x37 }, new ushort[2] { 0x3C, 0x37 }, new ushort[2] { 0x3D, 0x37 }, new ushort[2] { 0x46, 0x37 }, new ushort[2] { 0x47, 0x37 }, new ushort[2] { 0xD3, 0x37 }, new ushort[2] { 0x42, 0x37 }, new ushort[2] { 0x3F, 0x37 }, new ushort[2] { 0x4C, 0x37 }, new ushort[2] { 0x4B, 0x37 } };

        private static ushort[] p_StateExpectedIDs_82 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_82 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_82 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT64 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_82 = { };
        private static ushort[][] p_StateShiftsOnVariable_82 = { };
        private static ushort[][] p_StateReducsOnTerminal_82 = { new ushort[2] { 0x53, 0x38 }, new ushort[2] { 0x54, 0x38 }, new ushort[2] { 0x43, 0x38 }, new ushort[2] { 0x44, 0x38 }, new ushort[2] { 0x45, 0x38 }, new ushort[2] { 0x41, 0x38 }, new ushort[2] { 0x2D, 0x38 }, new ushort[2] { 0xA, 0x38 }, new ushort[2] { 0x32, 0x38 }, new ushort[2] { 0x33, 0x38 }, new ushort[2] { 0x11, 0x38 }, new ushort[2] { 0x34, 0x38 }, new ushort[2] { 0x35, 0x38 }, new ushort[2] { 0x36, 0x38 }, new ushort[2] { 0x37, 0x38 }, new ushort[2] { 0x38, 0x38 }, new ushort[2] { 0x39, 0x38 }, new ushort[2] { 0x3A, 0x38 }, new ushort[2] { 0x3B, 0x38 }, new ushort[2] { 0x3C, 0x38 }, new ushort[2] { 0x3D, 0x38 }, new ushort[2] { 0x46, 0x38 }, new ushort[2] { 0x47, 0x38 }, new ushort[2] { 0xD3, 0x38 }, new ushort[2] { 0x42, 0x38 }, new ushort[2] { 0x3F, 0x38 }, new ushort[2] { 0x4C, 0x38 }, new ushort[2] { 0x4B, 0x38 } };

        private static ushort[] p_StateExpectedIDs_83 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_83 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_83 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT128 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_83 = { };
        private static ushort[][] p_StateShiftsOnVariable_83 = { };
        private static ushort[][] p_StateReducsOnTerminal_83 = { new ushort[2] { 0x53, 0x39 }, new ushort[2] { 0x54, 0x39 }, new ushort[2] { 0x43, 0x39 }, new ushort[2] { 0x44, 0x39 }, new ushort[2] { 0x45, 0x39 }, new ushort[2] { 0x41, 0x39 }, new ushort[2] { 0x2D, 0x39 }, new ushort[2] { 0xA, 0x39 }, new ushort[2] { 0x32, 0x39 }, new ushort[2] { 0x33, 0x39 }, new ushort[2] { 0x11, 0x39 }, new ushort[2] { 0x34, 0x39 }, new ushort[2] { 0x35, 0x39 }, new ushort[2] { 0x36, 0x39 }, new ushort[2] { 0x37, 0x39 }, new ushort[2] { 0x38, 0x39 }, new ushort[2] { 0x39, 0x39 }, new ushort[2] { 0x3A, 0x39 }, new ushort[2] { 0x3B, 0x39 }, new ushort[2] { 0x3C, 0x39 }, new ushort[2] { 0x3D, 0x39 }, new ushort[2] { 0x46, 0x39 }, new ushort[2] { 0x47, 0x39 }, new ushort[2] { 0xD3, 0x39 }, new ushort[2] { 0x42, 0x39 }, new ushort[2] { 0x3F, 0x39 }, new ushort[2] { 0x4C, 0x39 }, new ushort[2] { 0x4B, 0x39 } };

        private static ushort[] p_StateExpectedIDs_84 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_84 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_84 = { "[grammar_bin_terminal -> SYMBOL_JOKER_BINARY . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_84 = { };
        private static ushort[][] p_StateShiftsOnVariable_84 = { };
        private static ushort[][] p_StateReducsOnTerminal_84 = { new ushort[2] { 0x53, 0x3A }, new ushort[2] { 0x54, 0x3A }, new ushort[2] { 0x43, 0x3A }, new ushort[2] { 0x44, 0x3A }, new ushort[2] { 0x45, 0x3A }, new ushort[2] { 0x41, 0x3A }, new ushort[2] { 0x2D, 0x3A }, new ushort[2] { 0xA, 0x3A }, new ushort[2] { 0x32, 0x3A }, new ushort[2] { 0x33, 0x3A }, new ushort[2] { 0x11, 0x3A }, new ushort[2] { 0x34, 0x3A }, new ushort[2] { 0x35, 0x3A }, new ushort[2] { 0x36, 0x3A }, new ushort[2] { 0x37, 0x3A }, new ushort[2] { 0x38, 0x3A }, new ushort[2] { 0x39, 0x3A }, new ushort[2] { 0x3A, 0x3A }, new ushort[2] { 0x3B, 0x3A }, new ushort[2] { 0x3C, 0x3A }, new ushort[2] { 0x3D, 0x3A }, new ushort[2] { 0x46, 0x3A }, new ushort[2] { 0x47, 0x3A }, new ushort[2] { 0xD3, 0x3A }, new ushort[2] { 0x42, 0x3A }, new ushort[2] { 0x3F, 0x3A }, new ushort[2] { 0x4C, 0x3A }, new ushort[2] { 0x4B, 0x3A } };

        private static ushort[] p_StateExpectedIDs_85 = { 0x3F };
        private static string[] p_StateExpectedNames_85 = { "_T[;]" };
        private static string[] p_StateItems_85 = { "[option -> NAME = QUOTED_DATA . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_85 = { new ushort[2] { 0x3F, 0xC8 } };
        private static ushort[][] p_StateShiftsOnVariable_85 = { };
        private static ushort[][] p_StateReducsOnTerminal_85 = { };

        private static ushort[] p_StateExpectedIDs_86 = { 0x12 };
        private static string[] p_StateExpectedNames_86 = { "_T[}]" };
        private static string[] p_StateItems_86 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { _m143 . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_86 = { new ushort[2] { 0x12, 0xC9 } };
        private static ushort[][] p_StateShiftsOnVariable_86 = { };
        private static ushort[][] p_StateReducsOnTerminal_86 = { };

        private static ushort[] p_StateExpectedIDs_87 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_87 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_87 = { "[_m143 -> cf_rule_simple<grammar_text_terminal> . _m143]", "[_m143 -> . cf_rule_simple<grammar_text_terminal> _m143]", "[_m143 -> . cf_rule_template<grammar_text_terminal> _m143]", "[_m143 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_87 = { new ushort[2] { 0xA, 0x89 } };
        private static ushort[][] p_StateShiftsOnVariable_87 = { new ushort[2] { 0x87, 0xCA }, new ushort[2] { 0x78, 0x87 }, new ushort[2] { 0x86, 0x88 } };
        private static ushort[][] p_StateReducsOnTerminal_87 = { new ushort[2] { 0x12, 0x74 } };

        private static ushort[] p_StateExpectedIDs_88 = { 0x12, 0xA };
        private static string[] p_StateExpectedNames_88 = { "_T[}]", "NAME" };
        private static string[] p_StateItems_88 = { "[_m143 -> cf_rule_template<grammar_text_terminal> . _m143]", "[_m143 -> . cf_rule_simple<grammar_text_terminal> _m143]", "[_m143 -> . cf_rule_template<grammar_text_terminal> _m143]", "[_m143 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_88 = { new ushort[2] { 0xA, 0x89 } };
        private static ushort[][] p_StateShiftsOnVariable_88 = { new ushort[2] { 0x87, 0xCB }, new ushort[2] { 0x78, 0x87 }, new ushort[2] { 0x86, 0x88 } };
        private static ushort[][] p_StateReducsOnTerminal_88 = { new ushort[2] { 0x12, 0x74 } };

        private static ushort[] p_StateExpectedIDs_89 = { 0x49, 0x4A };
        private static string[] p_StateExpectedNames_89 = { "_T[->]", "_T[<]" };
        private static string[] p_StateItems_89 = { "[cf_rule_simple<grammar_text_terminal> -> NAME . -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> NAME . rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params -> . < NAME _m93 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_89 = { new ushort[2] { 0x49, 0xCC }, new ushort[2] { 0x4A, 0x8F } };
        private static ushort[][] p_StateShiftsOnVariable_89 = { new ushort[2] { 0x67, 0xCD } };
        private static ushort[][] p_StateReducsOnTerminal_89 = { };

        private static ushort[] p_StateExpectedIDs_8A = { 0x12 };
        private static string[] p_StateExpectedNames_8A = { "_T[}]" };
        private static string[] p_StateItems_8A = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { _m183 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_8A = { };
        private static ushort[][] p_StateShiftsOnVariable_8A = { };
        private static ushort[][] p_StateReducsOnTerminal_8A = { new ushort[2] { 0x12, 0x75 } };

        private static ushort[] p_StateExpectedIDs_8B = { 0x12 };
        private static string[] p_StateExpectedNames_8B = { "_T[}]" };
        private static string[] p_StateItems_8B = { "[_m183 -> cf_rule_simple<grammar_bin_terminal> _m183 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_8B = { };
        private static ushort[][] p_StateShiftsOnVariable_8B = { };
        private static ushort[][] p_StateReducsOnTerminal_8B = { new ushort[2] { 0x12, 0x93 } };

        private static ushort[] p_StateExpectedIDs_8C = { 0x12 };
        private static string[] p_StateExpectedNames_8C = { "_T[}]" };
        private static string[] p_StateItems_8C = { "[_m183 -> cf_rule_template<grammar_bin_terminal> _m183 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_8C = { };
        private static ushort[][] p_StateShiftsOnVariable_8C = { };
        private static ushort[][] p_StateReducsOnTerminal_8C = { new ushort[2] { 0x12, 0x94 } };

        private static ushort[] p_StateExpectedIDs_8D = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_8D = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_8D = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_8D = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_8D = { new ushort[2] { 0x8A, 0xCE }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_8D = { };

        private static ushort[] p_StateExpectedIDs_8E = { 0x49 };
        private static string[] p_StateExpectedNames_8E = { "_T[->]" };
        private static string[] p_StateItems_8E = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params . -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_8E = { new ushort[2] { 0x49, 0xCF } };
        private static ushort[][] p_StateShiftsOnVariable_8E = { };
        private static ushort[][] p_StateReducsOnTerminal_8E = { };

        private static ushort[] p_StateExpectedIDs_8F = { 0xA };
        private static string[] p_StateExpectedNames_8F = { "NAME" };
        private static string[] p_StateItems_8F = { "[rule_template_params -> < . NAME _m93 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_8F = { new ushort[2] { 0xA, 0xD0 } };
        private static ushort[][] p_StateShiftsOnVariable_8F = { };
        private static ushort[][] p_StateReducsOnTerminal_8F = { };

        private static ushort[] p_StateExpectedIDs_90 = { 0x12 };
        private static string[] p_StateExpectedNames_90 = { "_T[}]" };
        private static string[] p_StateItems_90 = { "[grammar_cs_rules<grammar_text_terminal> -> rules { _m154 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_90 = { };
        private static ushort[][] p_StateShiftsOnVariable_90 = { };
        private static ushort[][] p_StateReducsOnTerminal_90 = { new ushort[2] { 0x12, 0x98 } };

        private static ushort[] p_StateExpectedIDs_91 = { 0x12 };
        private static string[] p_StateExpectedNames_91 = { "_T[}]" };
        private static string[] p_StateItems_91 = { "[_m154 -> cs_rule_simple<grammar_text_terminal> _m154 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_91 = { };
        private static ushort[][] p_StateShiftsOnVariable_91 = { };
        private static ushort[][] p_StateReducsOnTerminal_91 = { new ushort[2] { 0x12, 0x9D } };

        private static ushort[] p_StateExpectedIDs_92 = { 0x12 };
        private static string[] p_StateExpectedNames_92 = { "_T[}]" };
        private static string[] p_StateItems_92 = { "[_m154 -> cs_rule_template<grammar_text_terminal> _m154 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_92 = { };
        private static ushort[][] p_StateShiftsOnVariable_92 = { };
        private static ushort[][] p_StateReducsOnTerminal_92 = { new ushort[2] { 0x12, 0x9E } };

        private static ushort[] p_StateExpectedIDs_93 = { 0x49, 0x4A, 0xD2 };
        private static string[] p_StateExpectedNames_93 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] p_StateItems_93 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME . cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME . cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_93 = { new ushort[2] { 0xD2, 0x61 } };
        private static ushort[][] p_StateShiftsOnVariable_93 = { new ushort[2] { 0xD8, 0xD1 } };
        private static ushort[][] p_StateReducsOnTerminal_93 = { new ushort[2] { 0x49, 0x9B }, new ushort[2] { 0x4A, 0x9B } };

        private static ushort[] p_StateExpectedIDs_94 = { 0xD3 };
        private static string[] p_StateExpectedNames_94 = { "_T[]]" };
        private static string[] p_StateItems_94 = { "[cs_rule_context<grammar_text_terminal> -> [ rule_definition<grammar_text_terminal> . ]]" };
        private static ushort[][] p_StateShiftsOnTerminal_94 = { new ushort[2] { 0xD3, 0xD2 } };
        private static ushort[][] p_StateShiftsOnVariable_94 = { };
        private static ushort[][] p_StateReducsOnTerminal_94 = { };

        private static ushort[] p_StateExpectedIDs_95 = { 0xD3, 0x42, 0x3F, 0x47 };
        private static string[] p_StateExpectedNames_95 = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_StateItems_95 = { "[rule_definition<grammar_text_terminal> -> rule_def_restrict<grammar_text_terminal> . _m138]", "[_m138 -> . | rule_def_restrict<grammar_text_terminal> _m138]", "[_m138 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_95 = { new ushort[2] { 0x47, 0xD4 } };
        private static ushort[][] p_StateShiftsOnVariable_95 = { new ushort[2] { 0x85, 0xD3 } };
        private static ushort[][] p_StateReducsOnTerminal_95 = { new ushort[2] { 0xD3, 0x70 }, new ushort[2] { 0x42, 0x70 }, new ushort[2] { 0x3F, 0x70 } };

        private static ushort[] p_StateExpectedIDs_96 = { 0x47, 0xD3, 0x42, 0x3F, 0x46 };
        private static string[] p_StateExpectedNames_96 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_StateItems_96 = { "[rule_def_restrict<grammar_text_terminal> -> rule_def_fragment<grammar_text_terminal> . _m136]", "[_m136 -> . - rule_def_fragment<grammar_text_terminal> _m136]", "[_m136 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_96 = { new ushort[2] { 0x46, 0xD6 } };
        private static ushort[][] p_StateShiftsOnVariable_96 = { new ushort[2] { 0x84, 0xD5 } };
        private static ushort[][] p_StateReducsOnTerminal_96 = { new ushort[2] { 0x47, 0x6E }, new ushort[2] { 0xD3, 0x6E }, new ushort[2] { 0x42, 0x6E }, new ushort[2] { 0x3F, 0x6E } };

        private static ushort[] p_StateExpectedIDs_97 = { 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_97 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_97 = { "[rule_def_fragment<grammar_text_terminal> -> rule_def_repetition<grammar_text_terminal> . _m134]", "[_m134 -> . rule_def_repetition<grammar_text_terminal> _m134]", "[_m134 -> . ]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_97 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_97 = { new ushort[2] { 0x83, 0xD7 }, new ushort[2] { 0x7C, 0xD8 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_97 = { new ushort[2] { 0x46, 0x6C }, new ushort[2] { 0x47, 0x6C }, new ushort[2] { 0xD3, 0x6C }, new ushort[2] { 0x42, 0x6C }, new ushort[2] { 0x3F, 0x6C } };

        private static ushort[] p_StateExpectedIDs_98 = { 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x43, 0x44, 0x45 };
        private static string[] p_StateExpectedNames_98 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_StateItems_98 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . *]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . +]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . ?]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_98 = { new ushort[2] { 0x43, 0xD9 }, new ushort[2] { 0x44, 0xDA }, new ushort[2] { 0x45, 0xDB } };
        private static ushort[][] p_StateShiftsOnVariable_98 = { };
        private static ushort[][] p_StateReducsOnTerminal_98 = { new ushort[2] { 0x41, 0x5C }, new ushort[2] { 0x2D, 0x5C }, new ushort[2] { 0xA, 0x5C }, new ushort[2] { 0x2E, 0x5C }, new ushort[2] { 0x11, 0x5C }, new ushort[2] { 0x46, 0x5C }, new ushort[2] { 0x47, 0x5C }, new ushort[2] { 0xD3, 0x5C }, new ushort[2] { 0x42, 0x5C }, new ushort[2] { 0x3F, 0x5C } };

        private static ushort[] p_StateExpectedIDs_99 = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x53, 0x54 };
        private static string[] p_StateExpectedNames_99 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[^]", "_T[!]" };
        private static string[] p_StateItems_99 = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . ^]", "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . !]", "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_99 = { new ushort[2] { 0x53, 0xDC }, new ushort[2] { 0x54, 0xDD } };
        private static ushort[][] p_StateShiftsOnVariable_99 = { };
        private static ushort[][] p_StateReducsOnTerminal_99 = { new ushort[2] { 0x43, 0x5F }, new ushort[2] { 0x44, 0x5F }, new ushort[2] { 0x45, 0x5F }, new ushort[2] { 0x41, 0x5F }, new ushort[2] { 0x2D, 0x5F }, new ushort[2] { 0xA, 0x5F }, new ushort[2] { 0x2E, 0x5F }, new ushort[2] { 0x11, 0x5F }, new ushort[2] { 0x46, 0x5F }, new ushort[2] { 0x47, 0x5F }, new ushort[2] { 0xD3, 0x5F }, new ushort[2] { 0x42, 0x5F }, new ushort[2] { 0x3F, 0x5F } };

        private static ushort[] p_StateExpectedIDs_9A = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_9A = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_9A = { "[rule_def_element<grammar_text_terminal> -> rule_def_atom<grammar_text_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9A = { };
        private static ushort[][] p_StateShiftsOnVariable_9A = { };
        private static ushort[][] p_StateReducsOnTerminal_9A = { new ushort[2] { 0x53, 0x60 }, new ushort[2] { 0x54, 0x60 }, new ushort[2] { 0x43, 0x60 }, new ushort[2] { 0x44, 0x60 }, new ushort[2] { 0x45, 0x60 }, new ushort[2] { 0x41, 0x60 }, new ushort[2] { 0x2D, 0x60 }, new ushort[2] { 0xA, 0x60 }, new ushort[2] { 0x2E, 0x60 }, new ushort[2] { 0x11, 0x60 }, new ushort[2] { 0x46, 0x60 }, new ushort[2] { 0x47, 0x60 }, new ushort[2] { 0xD3, 0x60 }, new ushort[2] { 0x42, 0x60 }, new ushort[2] { 0x3F, 0x60 } };

        private static ushort[] p_StateExpectedIDs_9B = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_9B = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_9B = { "[rule_def_element<grammar_text_terminal> -> ( . rule_definition<grammar_text_terminal> )]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_9B = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_9B = { new ushort[2] { 0x79, 0xDE }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_9B = { };

        private static ushort[] p_StateExpectedIDs_9C = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_9C = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_9C = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_action . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9C = { };
        private static ushort[][] p_StateShiftsOnVariable_9C = { };
        private static ushort[][] p_StateReducsOnTerminal_9C = { new ushort[2] { 0x53, 0x62 }, new ushort[2] { 0x54, 0x62 }, new ushort[2] { 0x43, 0x62 }, new ushort[2] { 0x44, 0x62 }, new ushort[2] { 0x45, 0x62 }, new ushort[2] { 0x41, 0x62 }, new ushort[2] { 0x2D, 0x62 }, new ushort[2] { 0xA, 0x62 }, new ushort[2] { 0x2E, 0x62 }, new ushort[2] { 0x11, 0x62 }, new ushort[2] { 0x46, 0x62 }, new ushort[2] { 0x47, 0x62 }, new ushort[2] { 0xD3, 0x62 }, new ushort[2] { 0x42, 0x62 }, new ushort[2] { 0x3F, 0x62 }, new ushort[2] { 0x4C, 0x62 }, new ushort[2] { 0x4B, 0x62 } };

        private static ushort[] p_StateExpectedIDs_9D = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_9D = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_9D = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_virtual . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9D = { };
        private static ushort[][] p_StateShiftsOnVariable_9D = { };
        private static ushort[][] p_StateReducsOnTerminal_9D = { new ushort[2] { 0x53, 0x63 }, new ushort[2] { 0x54, 0x63 }, new ushort[2] { 0x43, 0x63 }, new ushort[2] { 0x44, 0x63 }, new ushort[2] { 0x45, 0x63 }, new ushort[2] { 0x41, 0x63 }, new ushort[2] { 0x2D, 0x63 }, new ushort[2] { 0xA, 0x63 }, new ushort[2] { 0x2E, 0x63 }, new ushort[2] { 0x11, 0x63 }, new ushort[2] { 0x46, 0x63 }, new ushort[2] { 0x47, 0x63 }, new ushort[2] { 0xD3, 0x63 }, new ushort[2] { 0x42, 0x63 }, new ushort[2] { 0x3F, 0x63 }, new ushort[2] { 0x4C, 0x63 }, new ushort[2] { 0x4B, 0x63 } };

        private static ushort[] p_StateExpectedIDs_9E = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_9E = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_9E = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_ref_simple . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9E = { };
        private static ushort[][] p_StateShiftsOnVariable_9E = { };
        private static ushort[][] p_StateReducsOnTerminal_9E = { new ushort[2] { 0x53, 0x64 }, new ushort[2] { 0x54, 0x64 }, new ushort[2] { 0x43, 0x64 }, new ushort[2] { 0x44, 0x64 }, new ushort[2] { 0x45, 0x64 }, new ushort[2] { 0x41, 0x64 }, new ushort[2] { 0x2D, 0x64 }, new ushort[2] { 0xA, 0x64 }, new ushort[2] { 0x2E, 0x64 }, new ushort[2] { 0x11, 0x64 }, new ushort[2] { 0x46, 0x64 }, new ushort[2] { 0x47, 0x64 }, new ushort[2] { 0xD3, 0x64 }, new ushort[2] { 0x42, 0x64 }, new ushort[2] { 0x3F, 0x64 }, new ushort[2] { 0x4C, 0x64 }, new ushort[2] { 0x4B, 0x64 } };

        private static ushort[] p_StateExpectedIDs_9F = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_9F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_9F = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_ref_template<grammar_text_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9F = { };
        private static ushort[][] p_StateShiftsOnVariable_9F = { };
        private static ushort[][] p_StateReducsOnTerminal_9F = { new ushort[2] { 0x53, 0x65 }, new ushort[2] { 0x54, 0x65 }, new ushort[2] { 0x43, 0x65 }, new ushort[2] { 0x44, 0x65 }, new ushort[2] { 0x45, 0x65 }, new ushort[2] { 0x41, 0x65 }, new ushort[2] { 0x2D, 0x65 }, new ushort[2] { 0xA, 0x65 }, new ushort[2] { 0x2E, 0x65 }, new ushort[2] { 0x11, 0x65 }, new ushort[2] { 0x46, 0x65 }, new ushort[2] { 0x47, 0x65 }, new ushort[2] { 0xD3, 0x65 }, new ushort[2] { 0x42, 0x65 }, new ushort[2] { 0x3F, 0x65 }, new ushort[2] { 0x4C, 0x65 }, new ushort[2] { 0x4B, 0x65 } };

        private static ushort[] p_StateExpectedIDs_A0 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_A0 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_A0 = { "[rule_def_atom<grammar_text_terminal> -> grammar_text_terminal . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A0 = { };
        private static ushort[][] p_StateShiftsOnVariable_A0 = { };
        private static ushort[][] p_StateReducsOnTerminal_A0 = { new ushort[2] { 0x53, 0x66 }, new ushort[2] { 0x54, 0x66 }, new ushort[2] { 0x43, 0x66 }, new ushort[2] { 0x44, 0x66 }, new ushort[2] { 0x45, 0x66 }, new ushort[2] { 0x41, 0x66 }, new ushort[2] { 0x2D, 0x66 }, new ushort[2] { 0xA, 0x66 }, new ushort[2] { 0x2E, 0x66 }, new ushort[2] { 0x11, 0x66 }, new ushort[2] { 0x46, 0x66 }, new ushort[2] { 0x47, 0x66 }, new ushort[2] { 0xD3, 0x66 }, new ushort[2] { 0x42, 0x66 }, new ushort[2] { 0x3F, 0x66 }, new ushort[2] { 0x4C, 0x66 }, new ushort[2] { 0x4B, 0x66 } };

        private static ushort[] p_StateExpectedIDs_A1 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B, 0x4A };
        private static string[] p_StateExpectedNames_A1 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]", "_T[<]" };
        private static string[] p_StateItems_A1 = { "[rule_sym_ref_simple -> NAME . ]", "[rule_sym_ref_template<grammar_text_terminal> -> NAME . rule_sym_ref_params<grammar_text_terminal>]", "[rule_sym_ref_params<grammar_text_terminal> -> . < rule_def_atom<grammar_text_terminal> _m125 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_A1 = { new ushort[2] { 0x4A, 0xE0 } };
        private static ushort[][] p_StateShiftsOnVariable_A1 = { new ushort[2] { 0x81, 0xDF } };
        private static ushort[][] p_StateReducsOnTerminal_A1 = { new ushort[2] { 0x53, 0x2D }, new ushort[2] { 0x54, 0x2D }, new ushort[2] { 0x43, 0x2D }, new ushort[2] { 0x44, 0x2D }, new ushort[2] { 0x45, 0x2D }, new ushort[2] { 0x41, 0x2D }, new ushort[2] { 0x2D, 0x2D }, new ushort[2] { 0xA, 0x2D }, new ushort[2] { 0x2E, 0x2D }, new ushort[2] { 0x11, 0x2D }, new ushort[2] { 0x46, 0x2D }, new ushort[2] { 0x47, 0x2D }, new ushort[2] { 0xD3, 0x2D }, new ushort[2] { 0x42, 0x2D }, new ushort[2] { 0x3F, 0x2D }, new ushort[2] { 0x4C, 0x2D }, new ushort[2] { 0x4B, 0x2D } };

        private static ushort[] p_StateExpectedIDs_A2 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_A2 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_A2 = { "[grammar_text_terminal -> terminal_def_atom_text . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A2 = { };
        private static ushort[][] p_StateShiftsOnVariable_A2 = { };
        private static ushort[][] p_StateReducsOnTerminal_A2 = { new ushort[2] { 0x53, 0x3B }, new ushort[2] { 0x54, 0x3B }, new ushort[2] { 0x43, 0x3B }, new ushort[2] { 0x44, 0x3B }, new ushort[2] { 0x45, 0x3B }, new ushort[2] { 0x41, 0x3B }, new ushort[2] { 0x2D, 0x3B }, new ushort[2] { 0xA, 0x3B }, new ushort[2] { 0x2E, 0x3B }, new ushort[2] { 0x11, 0x3B }, new ushort[2] { 0x46, 0x3B }, new ushort[2] { 0x47, 0x3B }, new ushort[2] { 0xD3, 0x3B }, new ushort[2] { 0x42, 0x3B }, new ushort[2] { 0x3F, 0x3B }, new ushort[2] { 0x4C, 0x3B }, new ushort[2] { 0x4B, 0x3B } };

        private static ushort[] p_StateExpectedIDs_A3 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x32, 0x2F, 0x31, 0x30, 0x33, 0x3F, 0x48, 0x42, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_A3 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[=>]", "_T[)]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_A3 = { "[terminal_def_atom_text -> SYMBOL_TERMINAL_TEXT . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A3 = { };
        private static ushort[][] p_StateShiftsOnVariable_A3 = { };
        private static ushort[][] p_StateReducsOnTerminal_A3 = { new ushort[2] { 0x53, 0x13 }, new ushort[2] { 0x54, 0x13 }, new ushort[2] { 0x43, 0x13 }, new ushort[2] { 0x44, 0x13 }, new ushort[2] { 0x45, 0x13 }, new ushort[2] { 0x41, 0x13 }, new ushort[2] { 0x2D, 0x13 }, new ushort[2] { 0xA, 0x13 }, new ushort[2] { 0x2E, 0x13 }, new ushort[2] { 0x11, 0x13 }, new ushort[2] { 0x46, 0x13 }, new ushort[2] { 0x47, 0x13 }, new ushort[2] { 0xD3, 0x13 }, new ushort[2] { 0x32, 0x13 }, new ushort[2] { 0x2F, 0x13 }, new ushort[2] { 0x31, 0x13 }, new ushort[2] { 0x30, 0x13 }, new ushort[2] { 0x33, 0x13 }, new ushort[2] { 0x3F, 0x13 }, new ushort[2] { 0x48, 0x13 }, new ushort[2] { 0x42, 0x13 }, new ushort[2] { 0x4C, 0x13 }, new ushort[2] { 0x4B, 0x13 } };

        private static ushort[] p_StateExpectedIDs_A4 = { 0x3F, 0x48 };
        private static string[] p_StateExpectedNames_A4 = { "_T[;]", "_T[=>]" };
        private static string[] p_StateItems_A4 = { "[terminal -> NAME -> terminal_definition . terminal_subgrammar ;]", "[terminal_subgrammar -> . => qualified_name]", "[terminal_subgrammar -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A4 = { new ushort[2] { 0x48, 0xE2 } };
        private static ushort[][] p_StateShiftsOnVariable_A4 = { new ushort[2] { 0x62, 0xE1 } };
        private static ushort[][] p_StateReducsOnTerminal_A4 = { new ushort[2] { 0x3F, 0x29 } };

        private static ushort[] p_StateExpectedIDs_A5 = { 0x3F, 0x48, 0x42, 0x47 };
        private static string[] p_StateExpectedNames_A5 = { "_T[;]", "_T[=>]", "_T[)]", "_T[|]" };
        private static string[] p_StateItems_A5 = { "[terminal_definition -> terminal_def_restrict . _m85]", "[_m85 -> . | terminal_def_restrict _m85]", "[_m85 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A5 = { new ushort[2] { 0x47, 0xE4 } };
        private static ushort[][] p_StateShiftsOnVariable_A5 = { new ushort[2] { 0x72, 0xE3 } };
        private static ushort[][] p_StateReducsOnTerminal_A5 = { new ushort[2] { 0x3F, 0x4B }, new ushort[2] { 0x48, 0x4B }, new ushort[2] { 0x42, 0x4B } };

        private static ushort[] p_StateExpectedIDs_A6 = { 0x47, 0x3F, 0x48, 0x42, 0x46 };
        private static string[] p_StateExpectedNames_A6 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[-]" };
        private static string[] p_StateItems_A6 = { "[terminal_def_restrict -> terminal_def_fragment . _m83]", "[_m83 -> . - terminal_def_fragment _m83]", "[_m83 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A6 = { new ushort[2] { 0x46, 0xE6 } };
        private static ushort[][] p_StateShiftsOnVariable_A6 = { new ushort[2] { 0x71, 0xE5 } };
        private static ushort[][] p_StateReducsOnTerminal_A6 = { new ushort[2] { 0x47, 0x49 }, new ushort[2] { 0x3F, 0x49 }, new ushort[2] { 0x48, 0x49 }, new ushort[2] { 0x42, 0x49 } };

        private static ushort[] p_StateExpectedIDs_A7 = { 0x46, 0x47, 0x3F, 0x48, 0x42, 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_A7 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_A7 = { "[terminal_def_fragment -> terminal_def_repetition . _m81]", "[_m81 -> . terminal_def_repetition _m81]", "[_m81 -> . ]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_A7 = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_A7 = { new ushort[2] { 0x70, 0xE7 }, new ushort[2] { 0x5E, 0xE8 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_A7 = { new ushort[2] { 0x46, 0x47 }, new ushort[2] { 0x47, 0x47 }, new ushort[2] { 0x3F, 0x47 }, new ushort[2] { 0x48, 0x47 }, new ushort[2] { 0x42, 0x47 } };

        private static ushort[] p_StateExpectedIDs_A8 = { 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42, 0x43, 0x44, 0x45 };
        private static string[] p_StateExpectedNames_A8 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_StateItems_A8 = { "[terminal_def_repetition -> terminal_def_element . *]", "[terminal_def_repetition -> terminal_def_element . +]", "[terminal_def_repetition -> terminal_def_element . ?]", "[terminal_def_repetition -> terminal_def_element . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A8 = { new ushort[2] { 0x43, 0xE9 }, new ushort[2] { 0x44, 0xEA }, new ushort[2] { 0x45, 0xEB } };
        private static ushort[][] p_StateShiftsOnVariable_A8 = { };
        private static ushort[][] p_StateReducsOnTerminal_A8 = { new ushort[2] { 0xA, 0x24 }, new ushort[2] { 0x41, 0x24 }, new ushort[2] { 0x32, 0x24 }, new ushort[2] { 0x2E, 0x24 }, new ushort[2] { 0x2F, 0x24 }, new ushort[2] { 0x31, 0x24 }, new ushort[2] { 0x30, 0x24 }, new ushort[2] { 0x33, 0x24 }, new ushort[2] { 0x46, 0x24 }, new ushort[2] { 0x47, 0x24 }, new ushort[2] { 0x3F, 0x24 }, new ushort[2] { 0x48, 0x24 }, new ushort[2] { 0x42, 0x24 } };

        private static ushort[] p_StateExpectedIDs_A9 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_A9 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_A9 = { "[terminal_def_element -> terminal_def_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A9 = { };
        private static ushort[][] p_StateShiftsOnVariable_A9 = { };
        private static ushort[][] p_StateReducsOnTerminal_A9 = { new ushort[2] { 0x43, 0x1F }, new ushort[2] { 0x44, 0x1F }, new ushort[2] { 0x45, 0x1F }, new ushort[2] { 0xA, 0x1F }, new ushort[2] { 0x41, 0x1F }, new ushort[2] { 0x32, 0x1F }, new ushort[2] { 0x2E, 0x1F }, new ushort[2] { 0x2F, 0x1F }, new ushort[2] { 0x31, 0x1F }, new ushort[2] { 0x30, 0x1F }, new ushort[2] { 0x33, 0x1F }, new ushort[2] { 0x46, 0x1F }, new ushort[2] { 0x47, 0x1F }, new ushort[2] { 0x3F, 0x1F }, new ushort[2] { 0x48, 0x1F }, new ushort[2] { 0x42, 0x1F } };

        private static ushort[] p_StateExpectedIDs_AA = { 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_AA = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_AA = { "[terminal_def_element -> ( . terminal_definition )]", "[terminal_definition -> . terminal_def_restrict _m85]", "[terminal_def_restrict -> . terminal_def_fragment _m83]", "[terminal_def_fragment -> . terminal_def_repetition _m81]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_AA = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_AA = { new ushort[2] { 0x61, 0xEC }, new ushort[2] { 0x60, 0xA5 }, new ushort[2] { 0x5F, 0xA6 }, new ushort[2] { 0x5E, 0xA7 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_AA = { };

        private static ushort[] p_StateExpectedIDs_AB = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42, 0x40 };
        private static string[] p_StateExpectedNames_AB = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[..]" };
        private static string[] p_StateItems_AB = { "[terminal_def_atom -> terminal_def_atom_unicode . ]", "[terminal_def_atom_span -> terminal_def_atom_unicode . .. terminal_def_atom_unicode]" };
        private static ushort[][] p_StateShiftsOnTerminal_AB = { new ushort[2] { 0x40, 0xED } };
        private static ushort[][] p_StateShiftsOnVariable_AB = { };
        private static ushort[][] p_StateReducsOnTerminal_AB = { new ushort[2] { 0x43, 0x18 }, new ushort[2] { 0x44, 0x18 }, new ushort[2] { 0x45, 0x18 }, new ushort[2] { 0xA, 0x18 }, new ushort[2] { 0x41, 0x18 }, new ushort[2] { 0x32, 0x18 }, new ushort[2] { 0x2E, 0x18 }, new ushort[2] { 0x2F, 0x18 }, new ushort[2] { 0x31, 0x18 }, new ushort[2] { 0x30, 0x18 }, new ushort[2] { 0x33, 0x18 }, new ushort[2] { 0x46, 0x18 }, new ushort[2] { 0x47, 0x18 }, new ushort[2] { 0x3F, 0x18 }, new ushort[2] { 0x48, 0x18 }, new ushort[2] { 0x42, 0x18 } };

        private static ushort[] p_StateExpectedIDs_AC = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_AC = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_AC = { "[terminal_def_atom -> terminal_def_atom_text . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_AC = { };
        private static ushort[][] p_StateShiftsOnVariable_AC = { };
        private static ushort[][] p_StateReducsOnTerminal_AC = { new ushort[2] { 0x43, 0x19 }, new ushort[2] { 0x44, 0x19 }, new ushort[2] { 0x45, 0x19 }, new ushort[2] { 0xA, 0x19 }, new ushort[2] { 0x41, 0x19 }, new ushort[2] { 0x32, 0x19 }, new ushort[2] { 0x2E, 0x19 }, new ushort[2] { 0x2F, 0x19 }, new ushort[2] { 0x31, 0x19 }, new ushort[2] { 0x30, 0x19 }, new ushort[2] { 0x33, 0x19 }, new ushort[2] { 0x46, 0x19 }, new ushort[2] { 0x47, 0x19 }, new ushort[2] { 0x3F, 0x19 }, new ushort[2] { 0x48, 0x19 }, new ushort[2] { 0x42, 0x19 } };

        private static ushort[] p_StateExpectedIDs_AD = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_AD = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_AD = { "[terminal_def_atom -> terminal_def_atom_set . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_AD = { };
        private static ushort[][] p_StateShiftsOnVariable_AD = { };
        private static ushort[][] p_StateReducsOnTerminal_AD = { new ushort[2] { 0x43, 0x1A }, new ushort[2] { 0x44, 0x1A }, new ushort[2] { 0x45, 0x1A }, new ushort[2] { 0xA, 0x1A }, new ushort[2] { 0x41, 0x1A }, new ushort[2] { 0x32, 0x1A }, new ushort[2] { 0x2E, 0x1A }, new ushort[2] { 0x2F, 0x1A }, new ushort[2] { 0x31, 0x1A }, new ushort[2] { 0x30, 0x1A }, new ushort[2] { 0x33, 0x1A }, new ushort[2] { 0x46, 0x1A }, new ushort[2] { 0x47, 0x1A }, new ushort[2] { 0x3F, 0x1A }, new ushort[2] { 0x48, 0x1A }, new ushort[2] { 0x42, 0x1A } };

        private static ushort[] p_StateExpectedIDs_AE = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_AE = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_AE = { "[terminal_def_atom -> terminal_def_atom_span . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_AE = { };
        private static ushort[][] p_StateShiftsOnVariable_AE = { };
        private static ushort[][] p_StateReducsOnTerminal_AE = { new ushort[2] { 0x43, 0x1B }, new ushort[2] { 0x44, 0x1B }, new ushort[2] { 0x45, 0x1B }, new ushort[2] { 0xA, 0x1B }, new ushort[2] { 0x41, 0x1B }, new ushort[2] { 0x32, 0x1B }, new ushort[2] { 0x2E, 0x1B }, new ushort[2] { 0x2F, 0x1B }, new ushort[2] { 0x31, 0x1B }, new ushort[2] { 0x30, 0x1B }, new ushort[2] { 0x33, 0x1B }, new ushort[2] { 0x46, 0x1B }, new ushort[2] { 0x47, 0x1B }, new ushort[2] { 0x3F, 0x1B }, new ushort[2] { 0x48, 0x1B }, new ushort[2] { 0x42, 0x1B } };

        private static ushort[] p_StateExpectedIDs_AF = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_AF = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_AF = { "[terminal_def_atom -> terminal_def_atom_ucat . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_AF = { };
        private static ushort[][] p_StateShiftsOnVariable_AF = { };
        private static ushort[][] p_StateReducsOnTerminal_AF = { new ushort[2] { 0x43, 0x1C }, new ushort[2] { 0x44, 0x1C }, new ushort[2] { 0x45, 0x1C }, new ushort[2] { 0xA, 0x1C }, new ushort[2] { 0x41, 0x1C }, new ushort[2] { 0x32, 0x1C }, new ushort[2] { 0x2E, 0x1C }, new ushort[2] { 0x2F, 0x1C }, new ushort[2] { 0x31, 0x1C }, new ushort[2] { 0x30, 0x1C }, new ushort[2] { 0x33, 0x1C }, new ushort[2] { 0x46, 0x1C }, new ushort[2] { 0x47, 0x1C }, new ushort[2] { 0x3F, 0x1C }, new ushort[2] { 0x48, 0x1C }, new ushort[2] { 0x42, 0x1C } };

        private static ushort[] p_StateExpectedIDs_B0 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_B0 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_B0 = { "[terminal_def_atom -> terminal_def_atom_ublock . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B0 = { };
        private static ushort[][] p_StateShiftsOnVariable_B0 = { };
        private static ushort[][] p_StateReducsOnTerminal_B0 = { new ushort[2] { 0x43, 0x1D }, new ushort[2] { 0x44, 0x1D }, new ushort[2] { 0x45, 0x1D }, new ushort[2] { 0xA, 0x1D }, new ushort[2] { 0x41, 0x1D }, new ushort[2] { 0x32, 0x1D }, new ushort[2] { 0x2E, 0x1D }, new ushort[2] { 0x2F, 0x1D }, new ushort[2] { 0x31, 0x1D }, new ushort[2] { 0x30, 0x1D }, new ushort[2] { 0x33, 0x1D }, new ushort[2] { 0x46, 0x1D }, new ushort[2] { 0x47, 0x1D }, new ushort[2] { 0x3F, 0x1D }, new ushort[2] { 0x48, 0x1D }, new ushort[2] { 0x42, 0x1D } };

        private static ushort[] p_StateExpectedIDs_B1 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_B1 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_B1 = { "[terminal_def_atom -> NAME . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B1 = { };
        private static ushort[][] p_StateShiftsOnVariable_B1 = { };
        private static ushort[][] p_StateReducsOnTerminal_B1 = { new ushort[2] { 0x43, 0x1E }, new ushort[2] { 0x44, 0x1E }, new ushort[2] { 0x45, 0x1E }, new ushort[2] { 0xA, 0x1E }, new ushort[2] { 0x41, 0x1E }, new ushort[2] { 0x32, 0x1E }, new ushort[2] { 0x2E, 0x1E }, new ushort[2] { 0x2F, 0x1E }, new ushort[2] { 0x31, 0x1E }, new ushort[2] { 0x30, 0x1E }, new ushort[2] { 0x33, 0x1E }, new ushort[2] { 0x46, 0x1E }, new ushort[2] { 0x47, 0x1E }, new ushort[2] { 0x3F, 0x1E }, new ushort[2] { 0x48, 0x1E }, new ushort[2] { 0x42, 0x1E } };

        private static ushort[] p_StateExpectedIDs_B2 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x40, 0x42 };
        private static string[] p_StateExpectedNames_B2 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[..]", "_T[)]" };
        private static string[] p_StateItems_B2 = { "[terminal_def_atom_unicode -> SYMBOL_VALUE_UINT8 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B2 = { };
        private static ushort[][] p_StateShiftsOnVariable_B2 = { };
        private static ushort[][] p_StateReducsOnTerminal_B2 = { new ushort[2] { 0x43, 0x11 }, new ushort[2] { 0x44, 0x11 }, new ushort[2] { 0x45, 0x11 }, new ushort[2] { 0xA, 0x11 }, new ushort[2] { 0x41, 0x11 }, new ushort[2] { 0x32, 0x11 }, new ushort[2] { 0x2E, 0x11 }, new ushort[2] { 0x2F, 0x11 }, new ushort[2] { 0x31, 0x11 }, new ushort[2] { 0x30, 0x11 }, new ushort[2] { 0x33, 0x11 }, new ushort[2] { 0x46, 0x11 }, new ushort[2] { 0x47, 0x11 }, new ushort[2] { 0x3F, 0x11 }, new ushort[2] { 0x48, 0x11 }, new ushort[2] { 0x40, 0x11 }, new ushort[2] { 0x42, 0x11 } };

        private static ushort[] p_StateExpectedIDs_B3 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x40, 0x42 };
        private static string[] p_StateExpectedNames_B3 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[..]", "_T[)]" };
        private static string[] p_StateItems_B3 = { "[terminal_def_atom_unicode -> SYMBOL_VALUE_UINT16 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B3 = { };
        private static ushort[][] p_StateShiftsOnVariable_B3 = { };
        private static ushort[][] p_StateReducsOnTerminal_B3 = { new ushort[2] { 0x43, 0x12 }, new ushort[2] { 0x44, 0x12 }, new ushort[2] { 0x45, 0x12 }, new ushort[2] { 0xA, 0x12 }, new ushort[2] { 0x41, 0x12 }, new ushort[2] { 0x32, 0x12 }, new ushort[2] { 0x2E, 0x12 }, new ushort[2] { 0x2F, 0x12 }, new ushort[2] { 0x31, 0x12 }, new ushort[2] { 0x30, 0x12 }, new ushort[2] { 0x33, 0x12 }, new ushort[2] { 0x46, 0x12 }, new ushort[2] { 0x47, 0x12 }, new ushort[2] { 0x3F, 0x12 }, new ushort[2] { 0x48, 0x12 }, new ushort[2] { 0x40, 0x12 }, new ushort[2] { 0x42, 0x12 } };

        private static ushort[] p_StateExpectedIDs_B4 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_B4 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_B4 = { "[terminal_def_atom_set -> SYMBOL_TERMINAL_SET . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B4 = { };
        private static ushort[][] p_StateShiftsOnVariable_B4 = { };
        private static ushort[][] p_StateReducsOnTerminal_B4 = { new ushort[2] { 0x43, 0x14 }, new ushort[2] { 0x44, 0x14 }, new ushort[2] { 0x45, 0x14 }, new ushort[2] { 0xA, 0x14 }, new ushort[2] { 0x41, 0x14 }, new ushort[2] { 0x32, 0x14 }, new ushort[2] { 0x2E, 0x14 }, new ushort[2] { 0x2F, 0x14 }, new ushort[2] { 0x31, 0x14 }, new ushort[2] { 0x30, 0x14 }, new ushort[2] { 0x33, 0x14 }, new ushort[2] { 0x46, 0x14 }, new ushort[2] { 0x47, 0x14 }, new ushort[2] { 0x3F, 0x14 }, new ushort[2] { 0x48, 0x14 }, new ushort[2] { 0x42, 0x14 } };

        private static ushort[] p_StateExpectedIDs_B5 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_B5 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_B5 = { "[terminal_def_atom_ucat -> SYMBOL_TERMINAL_UCAT . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B5 = { };
        private static ushort[][] p_StateShiftsOnVariable_B5 = { };
        private static ushort[][] p_StateReducsOnTerminal_B5 = { new ushort[2] { 0x43, 0x16 }, new ushort[2] { 0x44, 0x16 }, new ushort[2] { 0x45, 0x16 }, new ushort[2] { 0xA, 0x16 }, new ushort[2] { 0x41, 0x16 }, new ushort[2] { 0x32, 0x16 }, new ushort[2] { 0x2E, 0x16 }, new ushort[2] { 0x2F, 0x16 }, new ushort[2] { 0x31, 0x16 }, new ushort[2] { 0x30, 0x16 }, new ushort[2] { 0x33, 0x16 }, new ushort[2] { 0x46, 0x16 }, new ushort[2] { 0x47, 0x16 }, new ushort[2] { 0x3F, 0x16 }, new ushort[2] { 0x48, 0x16 }, new ushort[2] { 0x42, 0x16 } };

        private static ushort[] p_StateExpectedIDs_B6 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_B6 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_B6 = { "[terminal_def_atom_ublock -> SYMBOL_TERMINAL_UBLOCK . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B6 = { };
        private static ushort[][] p_StateShiftsOnVariable_B6 = { };
        private static ushort[][] p_StateReducsOnTerminal_B6 = { new ushort[2] { 0x43, 0x15 }, new ushort[2] { 0x44, 0x15 }, new ushort[2] { 0x45, 0x15 }, new ushort[2] { 0xA, 0x15 }, new ushort[2] { 0x41, 0x15 }, new ushort[2] { 0x32, 0x15 }, new ushort[2] { 0x2E, 0x15 }, new ushort[2] { 0x2F, 0x15 }, new ushort[2] { 0x31, 0x15 }, new ushort[2] { 0x30, 0x15 }, new ushort[2] { 0x33, 0x15 }, new ushort[2] { 0x46, 0x15 }, new ushort[2] { 0x47, 0x15 }, new ushort[2] { 0x3F, 0x15 }, new ushort[2] { 0x48, 0x15 }, new ushort[2] { 0x42, 0x15 } };

        private static ushort[] p_StateExpectedIDs_B7 = { 0x49, 0x4A };
        private static string[] p_StateExpectedNames_B7 = { "_T[->]", "_T[<]" };
        private static string[] p_StateItems_B7 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> . -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> . rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params -> . < NAME _m93 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_B7 = { new ushort[2] { 0x49, 0xEE }, new ushort[2] { 0x4A, 0x8F } };
        private static ushort[][] p_StateShiftsOnVariable_B7 = { new ushort[2] { 0x67, 0xEF } };
        private static ushort[][] p_StateReducsOnTerminal_B7 = { };

        private static ushort[] p_StateExpectedIDs_B8 = { 0xA, 0x49, 0x4A };
        private static string[] p_StateExpectedNames_B8 = { "NAME", "_T[->]", "_T[<]" };
        private static string[] p_StateItems_B8 = { "[cs_rule_context<grammar_bin_terminal> -> [ rule_definition<grammar_bin_terminal> ] . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B8 = { };
        private static ushort[][] p_StateShiftsOnVariable_B8 = { };
        private static ushort[][] p_StateReducsOnTerminal_B8 = { new ushort[2] { 0xA, 0xA2 }, new ushort[2] { 0x49, 0xA2 }, new ushort[2] { 0x4A, 0xA2 } };

        private static ushort[] p_StateExpectedIDs_B9 = { 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_B9 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_B9 = { "[rule_definition<grammar_bin_terminal> -> rule_def_restrict<grammar_bin_terminal> _m178 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_B9 = { };
        private static ushort[][] p_StateShiftsOnVariable_B9 = { };
        private static ushort[][] p_StateReducsOnTerminal_B9 = { new ushort[2] { 0xD3, 0x77 }, new ushort[2] { 0x42, 0x77 }, new ushort[2] { 0x3F, 0x77 } };

        private static ushort[] p_StateExpectedIDs_BA = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_BA = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_BA = { "[_m178 -> | . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_BA = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_BA = { new ushort[2] { 0x8B, 0xF0 }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_BA = { };

        private static ushort[] p_StateExpectedIDs_BB = { 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_BB = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_BB = { "[rule_def_restrict<grammar_bin_terminal> -> rule_def_fragment<grammar_bin_terminal> _m176 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_BB = { };
        private static ushort[][] p_StateShiftsOnVariable_BB = { };
        private static ushort[][] p_StateReducsOnTerminal_BB = { new ushort[2] { 0x47, 0x78 }, new ushort[2] { 0xD3, 0x78 }, new ushort[2] { 0x42, 0x78 }, new ushort[2] { 0x3F, 0x78 } };

        private static ushort[] p_StateExpectedIDs_BC = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_BC = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_BC = { "[_m176 -> - . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_BC = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_BC = { new ushort[2] { 0x8C, 0xF1 }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_BC = { };

        private static ushort[] p_StateExpectedIDs_BD = { 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_BD = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_BD = { "[rule_def_fragment<grammar_bin_terminal> -> rule_def_repetition<grammar_bin_terminal> _m174 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_BD = { };
        private static ushort[][] p_StateShiftsOnVariable_BD = { };
        private static ushort[][] p_StateReducsOnTerminal_BD = { new ushort[2] { 0x46, 0x79 }, new ushort[2] { 0x47, 0x79 }, new ushort[2] { 0xD3, 0x79 }, new ushort[2] { 0x42, 0x79 }, new ushort[2] { 0x3F, 0x79 } };

        private static ushort[] p_StateExpectedIDs_BE = { 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_BE = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_BE = { "[_m174 -> rule_def_repetition<grammar_bin_terminal> . _m174]", "[_m174 -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[_m174 -> . ]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_BE = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_BE = { new ushort[2] { 0x94, 0xF2 }, new ushort[2] { 0x8D, 0xBE }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_BE = { new ushort[2] { 0x46, 0x8D }, new ushort[2] { 0x47, 0x8D }, new ushort[2] { 0xD3, 0x8D }, new ushort[2] { 0x42, 0x8D }, new ushort[2] { 0x3F, 0x8D } };

        private static ushort[] p_StateExpectedIDs_BF = { 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_BF = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_BF = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> * . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_BF = { };
        private static ushort[][] p_StateShiftsOnVariable_BF = { };
        private static ushort[][] p_StateReducsOnTerminal_BF = { new ushort[2] { 0x41, 0x7A }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0xA, 0x7A }, new ushort[2] { 0x32, 0x7A }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x11, 0x7A }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7A }, new ushort[2] { 0x36, 0x7A }, new ushort[2] { 0x37, 0x7A }, new ushort[2] { 0x38, 0x7A }, new ushort[2] { 0x39, 0x7A }, new ushort[2] { 0x3A, 0x7A }, new ushort[2] { 0x3B, 0x7A }, new ushort[2] { 0x3C, 0x7A }, new ushort[2] { 0x3D, 0x7A }, new ushort[2] { 0x46, 0x7A }, new ushort[2] { 0x47, 0x7A }, new ushort[2] { 0xD3, 0x7A }, new ushort[2] { 0x42, 0x7A }, new ushort[2] { 0x3F, 0x7A } };

        private static ushort[] p_StateExpectedIDs_C0 = { 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_C0 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_C0 = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> + . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C0 = { };
        private static ushort[][] p_StateShiftsOnVariable_C0 = { };
        private static ushort[][] p_StateReducsOnTerminal_C0 = { new ushort[2] { 0x41, 0x7B }, new ushort[2] { 0x2D, 0x7B }, new ushort[2] { 0xA, 0x7B }, new ushort[2] { 0x32, 0x7B }, new ushort[2] { 0x33, 0x7B }, new ushort[2] { 0x11, 0x7B }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7B }, new ushort[2] { 0x37, 0x7B }, new ushort[2] { 0x38, 0x7B }, new ushort[2] { 0x39, 0x7B }, new ushort[2] { 0x3A, 0x7B }, new ushort[2] { 0x3B, 0x7B }, new ushort[2] { 0x3C, 0x7B }, new ushort[2] { 0x3D, 0x7B }, new ushort[2] { 0x46, 0x7B }, new ushort[2] { 0x47, 0x7B }, new ushort[2] { 0xD3, 0x7B }, new ushort[2] { 0x42, 0x7B }, new ushort[2] { 0x3F, 0x7B } };

        private static ushort[] p_StateExpectedIDs_C1 = { 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_C1 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_C1 = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> ? . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C1 = { };
        private static ushort[][] p_StateShiftsOnVariable_C1 = { };
        private static ushort[][] p_StateReducsOnTerminal_C1 = { new ushort[2] { 0x41, 0x7C }, new ushort[2] { 0x2D, 0x7C }, new ushort[2] { 0xA, 0x7C }, new ushort[2] { 0x32, 0x7C }, new ushort[2] { 0x33, 0x7C }, new ushort[2] { 0x11, 0x7C }, new ushort[2] { 0x34, 0x7C }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7C }, new ushort[2] { 0x38, 0x7C }, new ushort[2] { 0x39, 0x7C }, new ushort[2] { 0x3A, 0x7C }, new ushort[2] { 0x3B, 0x7C }, new ushort[2] { 0x3C, 0x7C }, new ushort[2] { 0x3D, 0x7C }, new ushort[2] { 0x46, 0x7C }, new ushort[2] { 0x47, 0x7C }, new ushort[2] { 0xD3, 0x7C }, new ushort[2] { 0x42, 0x7C }, new ushort[2] { 0x3F, 0x7C } };

        private static ushort[] p_StateExpectedIDs_C2 = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_C2 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_C2 = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> ^ . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C2 = { };
        private static ushort[][] p_StateShiftsOnVariable_C2 = { };
        private static ushort[][] p_StateReducsOnTerminal_C2 = { new ushort[2] { 0x43, 0x7E }, new ushort[2] { 0x44, 0x7E }, new ushort[2] { 0x45, 0x7E }, new ushort[2] { 0x41, 0x7E }, new ushort[2] { 0x2D, 0x7E }, new ushort[2] { 0xA, 0x7E }, new ushort[2] { 0x32, 0x7E }, new ushort[2] { 0x33, 0x7E }, new ushort[2] { 0x11, 0x7E }, new ushort[2] { 0x34, 0x7E }, new ushort[2] { 0x35, 0x7E }, new ushort[2] { 0x36, 0x7E }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7E }, new ushort[2] { 0x39, 0x7E }, new ushort[2] { 0x3A, 0x7E }, new ushort[2] { 0x3B, 0x7E }, new ushort[2] { 0x3C, 0x7E }, new ushort[2] { 0x3D, 0x7E }, new ushort[2] { 0x46, 0x7E }, new ushort[2] { 0x47, 0x7E }, new ushort[2] { 0xD3, 0x7E }, new ushort[2] { 0x42, 0x7E }, new ushort[2] { 0x3F, 0x7E } };

        private static ushort[] p_StateExpectedIDs_C3 = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_C3 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_C3 = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> ! . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C3 = { };
        private static ushort[][] p_StateShiftsOnVariable_C3 = { };
        private static ushort[][] p_StateReducsOnTerminal_C3 = { new ushort[2] { 0x43, 0x7F }, new ushort[2] { 0x44, 0x7F }, new ushort[2] { 0x45, 0x7F }, new ushort[2] { 0x41, 0x7F }, new ushort[2] { 0x2D, 0x7F }, new ushort[2] { 0xA, 0x7F }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x7F }, new ushort[2] { 0x11, 0x7F }, new ushort[2] { 0x34, 0x7F }, new ushort[2] { 0x35, 0x7F }, new ushort[2] { 0x36, 0x7F }, new ushort[2] { 0x37, 0x7F }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x3A, 0x7F }, new ushort[2] { 0x3B, 0x7F }, new ushort[2] { 0x3C, 0x7F }, new ushort[2] { 0x3D, 0x7F }, new ushort[2] { 0x46, 0x7F }, new ushort[2] { 0x47, 0x7F }, new ushort[2] { 0xD3, 0x7F }, new ushort[2] { 0x42, 0x7F }, new ushort[2] { 0x3F, 0x7F } };

        private static ushort[] p_StateExpectedIDs_C4 = { 0x42 };
        private static string[] p_StateExpectedNames_C4 = { "_T[)]" };
        private static string[] p_StateItems_C4 = { "[rule_def_element<grammar_bin_terminal> -> ( rule_definition<grammar_bin_terminal> . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_C4 = { new ushort[2] { 0x42, 0xF3 } };
        private static ushort[][] p_StateShiftsOnVariable_C4 = { };
        private static ushort[][] p_StateReducsOnTerminal_C4 = { };

        private static ushort[] p_StateExpectedIDs_C5 = { 0x12 };
        private static string[] p_StateExpectedNames_C5 = { "_T[}]" };
        private static string[] p_StateItems_C5 = { "[rule_sym_action -> { qualified_name . }]" };
        private static ushort[][] p_StateShiftsOnTerminal_C5 = { new ushort[2] { 0x12, 0xF4 } };
        private static ushort[][] p_StateShiftsOnVariable_C5 = { };
        private static ushort[][] p_StateReducsOnTerminal_C5 = { };

        private static ushort[] p_StateExpectedIDs_C6 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_C6 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_C6 = { "[rule_sym_ref_template<grammar_bin_terminal> -> NAME rule_sym_ref_params<grammar_bin_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C6 = { };
        private static ushort[][] p_StateShiftsOnVariable_C6 = { };
        private static ushort[][] p_StateReducsOnTerminal_C6 = { new ushort[2] { 0x53, 0x88 }, new ushort[2] { 0x54, 0x88 }, new ushort[2] { 0x43, 0x88 }, new ushort[2] { 0x44, 0x88 }, new ushort[2] { 0x45, 0x88 }, new ushort[2] { 0x41, 0x88 }, new ushort[2] { 0x2D, 0x88 }, new ushort[2] { 0xA, 0x88 }, new ushort[2] { 0x32, 0x88 }, new ushort[2] { 0x33, 0x88 }, new ushort[2] { 0x11, 0x88 }, new ushort[2] { 0x34, 0x88 }, new ushort[2] { 0x35, 0x88 }, new ushort[2] { 0x36, 0x88 }, new ushort[2] { 0x37, 0x88 }, new ushort[2] { 0x38, 0x88 }, new ushort[2] { 0x39, 0x88 }, new ushort[2] { 0x3A, 0x88 }, new ushort[2] { 0x3B, 0x88 }, new ushort[2] { 0x3C, 0x88 }, new ushort[2] { 0x3D, 0x88 }, new ushort[2] { 0x46, 0x88 }, new ushort[2] { 0x47, 0x88 }, new ushort[2] { 0xD3, 0x88 }, new ushort[2] { 0x42, 0x88 }, new ushort[2] { 0x3F, 0x88 }, new ushort[2] { 0x4C, 0x88 }, new ushort[2] { 0x4B, 0x88 } };

        private static ushort[] p_StateExpectedIDs_C7 = { 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_C7 = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_C7 = { "[rule_sym_ref_params<grammar_bin_terminal> -> < . rule_def_atom<grammar_bin_terminal> _m165 >]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_C7 = { new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_C7 = { new ushort[2] { 0x90, 0xF5 }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_C7 = { };

        private static ushort[] p_StateExpectedIDs_C8 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_C8 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_C8 = { "[option -> NAME = QUOTED_DATA ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C8 = { };
        private static ushort[][] p_StateShiftsOnVariable_C8 = { };
        private static ushort[][] p_StateReducsOnTerminal_C8 = { new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0x12, 0x10 } };

        private static ushort[] p_StateExpectedIDs_C9 = { 0x12 };
        private static string[] p_StateExpectedNames_C9 = { "_T[}]" };
        private static string[] p_StateItems_C9 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { _m143 } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C9 = { };
        private static ushort[][] p_StateShiftsOnVariable_C9 = { };
        private static ushort[][] p_StateReducsOnTerminal_C9 = { new ushort[2] { 0x12, 0x54 } };

        private static ushort[] p_StateExpectedIDs_CA = { 0x12 };
        private static string[] p_StateExpectedNames_CA = { "_T[}]" };
        private static string[] p_StateItems_CA = { "[_m143 -> cf_rule_simple<grammar_text_terminal> _m143 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_CA = { };
        private static ushort[][] p_StateShiftsOnVariable_CA = { };
        private static ushort[][] p_StateReducsOnTerminal_CA = { new ushort[2] { 0x12, 0x72 } };

        private static ushort[] p_StateExpectedIDs_CB = { 0x12 };
        private static string[] p_StateExpectedNames_CB = { "_T[}]" };
        private static string[] p_StateItems_CB = { "[_m143 -> cf_rule_template<grammar_text_terminal> _m143 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_CB = { };
        private static ushort[][] p_StateShiftsOnVariable_CB = { };
        private static ushort[][] p_StateReducsOnTerminal_CB = { new ushort[2] { 0x12, 0x73 } };

        private static ushort[] p_StateExpectedIDs_CC = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_CC = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_CC = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_CC = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_CC = { new ushort[2] { 0x79, 0xF6 }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_CC = { };

        private static ushort[] p_StateExpectedIDs_CD = { 0x49 };
        private static string[] p_StateExpectedNames_CD = { "_T[->]" };
        private static string[] p_StateItems_CD = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params . -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_CD = { new ushort[2] { 0x49, 0xF7 } };
        private static ushort[][] p_StateShiftsOnVariable_CD = { };
        private static ushort[][] p_StateReducsOnTerminal_CD = { };

        private static ushort[] p_StateExpectedIDs_CE = { 0x3F };
        private static string[] p_StateExpectedNames_CE = { "_T[;]" };
        private static string[] p_StateItems_CE = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_CE = { new ushort[2] { 0x3F, 0xF8 } };
        private static ushort[][] p_StateShiftsOnVariable_CE = { };
        private static ushort[][] p_StateReducsOnTerminal_CE = { };

        private static ushort[] p_StateExpectedIDs_CF = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_CF = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_CF = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_CF = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_CF = { new ushort[2] { 0x8A, 0xF9 }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_CF = { };

        private static ushort[] p_StateExpectedIDs_D0 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_D0 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_D0 = { "[rule_template_params -> < NAME . _m93 >]", "[_m93 -> . , NAME _m93]", "[_m93 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D0 = { new ushort[2] { 0x4B, 0xFB } };
        private static ushort[][] p_StateShiftsOnVariable_D0 = { new ushort[2] { 0x73, 0xFA } };
        private static ushort[][] p_StateReducsOnTerminal_D0 = { new ushort[2] { 0x4C, 0x4D } };

        private static ushort[] p_StateExpectedIDs_D1 = { 0x49, 0x4A };
        private static string[] p_StateExpectedNames_D1 = { "_T[->]", "_T[<]" };
        private static string[] p_StateItems_D1 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> . -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> . rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params -> . < NAME _m93 >]" };
        private static ushort[][] p_StateShiftsOnTerminal_D1 = { new ushort[2] { 0x49, 0xFC }, new ushort[2] { 0x4A, 0x8F } };
        private static ushort[][] p_StateShiftsOnVariable_D1 = { new ushort[2] { 0x67, 0xFD } };
        private static ushort[][] p_StateReducsOnTerminal_D1 = { };

        private static ushort[] p_StateExpectedIDs_D2 = { 0xA, 0x49, 0x4A };
        private static string[] p_StateExpectedNames_D2 = { "NAME", "_T[->]", "_T[<]" };
        private static string[] p_StateItems_D2 = { "[cs_rule_context<grammar_text_terminal> -> [ rule_definition<grammar_text_terminal> ] . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D2 = { };
        private static ushort[][] p_StateShiftsOnVariable_D2 = { };
        private static ushort[][] p_StateReducsOnTerminal_D2 = { new ushort[2] { 0xA, 0x9A }, new ushort[2] { 0x49, 0x9A }, new ushort[2] { 0x4A, 0x9A } };

        private static ushort[] p_StateExpectedIDs_D3 = { 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_D3 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_D3 = { "[rule_definition<grammar_text_terminal> -> rule_def_restrict<grammar_text_terminal> _m138 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D3 = { };
        private static ushort[][] p_StateShiftsOnVariable_D3 = { };
        private static ushort[][] p_StateReducsOnTerminal_D3 = { new ushort[2] { 0xD3, 0x56 }, new ushort[2] { 0x42, 0x56 }, new ushort[2] { 0x3F, 0x56 } };

        private static ushort[] p_StateExpectedIDs_D4 = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_D4 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_D4 = { "[_m138 -> | . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_D4 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_D4 = { new ushort[2] { 0x7A, 0xFE }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_D4 = { };

        private static ushort[] p_StateExpectedIDs_D5 = { 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_D5 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_D5 = { "[rule_def_restrict<grammar_text_terminal> -> rule_def_fragment<grammar_text_terminal> _m136 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D5 = { };
        private static ushort[][] p_StateShiftsOnVariable_D5 = { };
        private static ushort[][] p_StateReducsOnTerminal_D5 = { new ushort[2] { 0x47, 0x57 }, new ushort[2] { 0xD3, 0x57 }, new ushort[2] { 0x42, 0x57 }, new ushort[2] { 0x3F, 0x57 } };

        private static ushort[] p_StateExpectedIDs_D6 = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_D6 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_D6 = { "[_m136 -> - . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_D6 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_D6 = { new ushort[2] { 0x7B, 0xFF }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_D6 = { };

        private static ushort[] p_StateExpectedIDs_D7 = { 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_D7 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_D7 = { "[rule_def_fragment<grammar_text_terminal> -> rule_def_repetition<grammar_text_terminal> _m134 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D7 = { };
        private static ushort[][] p_StateShiftsOnVariable_D7 = { };
        private static ushort[][] p_StateReducsOnTerminal_D7 = { new ushort[2] { 0x46, 0x58 }, new ushort[2] { 0x47, 0x58 }, new ushort[2] { 0xD3, 0x58 }, new ushort[2] { 0x42, 0x58 }, new ushort[2] { 0x3F, 0x58 } };

        private static ushort[] p_StateExpectedIDs_D8 = { 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_D8 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_D8 = { "[_m134 -> rule_def_repetition<grammar_text_terminal> . _m134]", "[_m134 -> . rule_def_repetition<grammar_text_terminal> _m134]", "[_m134 -> . ]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_D8 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_D8 = { new ushort[2] { 0x83, 0x100 }, new ushort[2] { 0x7C, 0xD8 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_D8 = { new ushort[2] { 0x46, 0x6C }, new ushort[2] { 0x47, 0x6C }, new ushort[2] { 0xD3, 0x6C }, new ushort[2] { 0x42, 0x6C }, new ushort[2] { 0x3F, 0x6C } };

        private static ushort[] p_StateExpectedIDs_D9 = { 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_D9 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_D9 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> * . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D9 = { };
        private static ushort[][] p_StateShiftsOnVariable_D9 = { };
        private static ushort[][] p_StateReducsOnTerminal_D9 = { new ushort[2] { 0x41, 0x59 }, new ushort[2] { 0x2D, 0x59 }, new ushort[2] { 0xA, 0x59 }, new ushort[2] { 0x2E, 0x59 }, new ushort[2] { 0x11, 0x59 }, new ushort[2] { 0x46, 0x59 }, new ushort[2] { 0x47, 0x59 }, new ushort[2] { 0xD3, 0x59 }, new ushort[2] { 0x42, 0x59 }, new ushort[2] { 0x3F, 0x59 } };

        private static ushort[] p_StateExpectedIDs_DA = { 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_DA = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_DA = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> + . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_DA = { };
        private static ushort[][] p_StateShiftsOnVariable_DA = { };
        private static ushort[][] p_StateReducsOnTerminal_DA = { new ushort[2] { 0x41, 0x5A }, new ushort[2] { 0x2D, 0x5A }, new ushort[2] { 0xA, 0x5A }, new ushort[2] { 0x2E, 0x5A }, new ushort[2] { 0x11, 0x5A }, new ushort[2] { 0x46, 0x5A }, new ushort[2] { 0x47, 0x5A }, new ushort[2] { 0xD3, 0x5A }, new ushort[2] { 0x42, 0x5A }, new ushort[2] { 0x3F, 0x5A } };

        private static ushort[] p_StateExpectedIDs_DB = { 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_DB = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_DB = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> ? . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_DB = { };
        private static ushort[][] p_StateShiftsOnVariable_DB = { };
        private static ushort[][] p_StateReducsOnTerminal_DB = { new ushort[2] { 0x41, 0x5B }, new ushort[2] { 0x2D, 0x5B }, new ushort[2] { 0xA, 0x5B }, new ushort[2] { 0x2E, 0x5B }, new ushort[2] { 0x11, 0x5B }, new ushort[2] { 0x46, 0x5B }, new ushort[2] { 0x47, 0x5B }, new ushort[2] { 0xD3, 0x5B }, new ushort[2] { 0x42, 0x5B }, new ushort[2] { 0x3F, 0x5B } };

        private static ushort[] p_StateExpectedIDs_DC = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_DC = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_DC = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> ^ . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_DC = { };
        private static ushort[][] p_StateShiftsOnVariable_DC = { };
        private static ushort[][] p_StateReducsOnTerminal_DC = { new ushort[2] { 0x43, 0x5D }, new ushort[2] { 0x44, 0x5D }, new ushort[2] { 0x45, 0x5D }, new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0x2D, 0x5D }, new ushort[2] { 0xA, 0x5D }, new ushort[2] { 0x2E, 0x5D }, new ushort[2] { 0x11, 0x5D }, new ushort[2] { 0x46, 0x5D }, new ushort[2] { 0x47, 0x5D }, new ushort[2] { 0xD3, 0x5D }, new ushort[2] { 0x42, 0x5D }, new ushort[2] { 0x3F, 0x5D } };

        private static ushort[] p_StateExpectedIDs_DD = { 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_DD = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_DD = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> ! . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_DD = { };
        private static ushort[][] p_StateShiftsOnVariable_DD = { };
        private static ushort[][] p_StateReducsOnTerminal_DD = { new ushort[2] { 0x43, 0x5E }, new ushort[2] { 0x44, 0x5E }, new ushort[2] { 0x45, 0x5E }, new ushort[2] { 0x41, 0x5E }, new ushort[2] { 0x2D, 0x5E }, new ushort[2] { 0xA, 0x5E }, new ushort[2] { 0x2E, 0x5E }, new ushort[2] { 0x11, 0x5E }, new ushort[2] { 0x46, 0x5E }, new ushort[2] { 0x47, 0x5E }, new ushort[2] { 0xD3, 0x5E }, new ushort[2] { 0x42, 0x5E }, new ushort[2] { 0x3F, 0x5E } };

        private static ushort[] p_StateExpectedIDs_DE = { 0x42 };
        private static string[] p_StateExpectedNames_DE = { "_T[)]" };
        private static string[] p_StateItems_DE = { "[rule_def_element<grammar_text_terminal> -> ( rule_definition<grammar_text_terminal> . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_DE = { new ushort[2] { 0x42, 0x101 } };
        private static ushort[][] p_StateShiftsOnVariable_DE = { };
        private static ushort[][] p_StateReducsOnTerminal_DE = { };

        private static ushort[] p_StateExpectedIDs_DF = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_DF = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_DF = { "[rule_sym_ref_template<grammar_text_terminal> -> NAME rule_sym_ref_params<grammar_text_terminal> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_DF = { };
        private static ushort[][] p_StateShiftsOnVariable_DF = { };
        private static ushort[][] p_StateReducsOnTerminal_DF = { new ushort[2] { 0x53, 0x67 }, new ushort[2] { 0x54, 0x67 }, new ushort[2] { 0x43, 0x67 }, new ushort[2] { 0x44, 0x67 }, new ushort[2] { 0x45, 0x67 }, new ushort[2] { 0x41, 0x67 }, new ushort[2] { 0x2D, 0x67 }, new ushort[2] { 0xA, 0x67 }, new ushort[2] { 0x2E, 0x67 }, new ushort[2] { 0x11, 0x67 }, new ushort[2] { 0x46, 0x67 }, new ushort[2] { 0x47, 0x67 }, new ushort[2] { 0xD3, 0x67 }, new ushort[2] { 0x42, 0x67 }, new ushort[2] { 0x3F, 0x67 }, new ushort[2] { 0x4C, 0x67 }, new ushort[2] { 0x4B, 0x67 } };

        private static ushort[] p_StateExpectedIDs_E0 = { 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_E0 = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_E0 = { "[rule_sym_ref_params<grammar_text_terminal> -> < . rule_def_atom<grammar_text_terminal> _m125 >]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_E0 = { new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_E0 = { new ushort[2] { 0x7F, 0x102 }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_E0 = { };

        private static ushort[] p_StateExpectedIDs_E1 = { 0x3F };
        private static string[] p_StateExpectedNames_E1 = { "_T[;]" };
        private static string[] p_StateItems_E1 = { "[terminal -> NAME -> terminal_definition terminal_subgrammar . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_E1 = { new ushort[2] { 0x3F, 0x103 } };
        private static ushort[][] p_StateShiftsOnVariable_E1 = { };
        private static ushort[][] p_StateReducsOnTerminal_E1 = { };

        private static ushort[] p_StateExpectedIDs_E2 = { 0xA };
        private static string[] p_StateExpectedNames_E2 = { "NAME" };
        private static string[] p_StateItems_E2 = { "[terminal_subgrammar -> => . qualified_name]", "[qualified_name -> . NAME _m20]" };
        private static ushort[][] p_StateShiftsOnTerminal_E2 = { new ushort[2] { 0xA, 0x18 } };
        private static ushort[][] p_StateShiftsOnVariable_E2 = { new ushort[2] { 0x13, 0x104 } };
        private static ushort[][] p_StateReducsOnTerminal_E2 = { };

        private static ushort[] p_StateExpectedIDs_E3 = { 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_E3 = { "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_E3 = { "[terminal_definition -> terminal_def_restrict _m85 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_E3 = { };
        private static ushort[][] p_StateShiftsOnVariable_E3 = { };
        private static ushort[][] p_StateReducsOnTerminal_E3 = { new ushort[2] { 0x3F, 0x27 }, new ushort[2] { 0x48, 0x27 }, new ushort[2] { 0x42, 0x27 } };

        private static ushort[] p_StateExpectedIDs_E4 = { 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_E4 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_E4 = { "[_m85 -> | . terminal_def_restrict _m85]", "[terminal_def_restrict -> . terminal_def_fragment _m83]", "[terminal_def_fragment -> . terminal_def_repetition _m81]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_E4 = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_E4 = { new ushort[2] { 0x60, 0x105 }, new ushort[2] { 0x5F, 0xA6 }, new ushort[2] { 0x5E, 0xA7 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_E4 = { };

        private static ushort[] p_StateExpectedIDs_E5 = { 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_E5 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_E5 = { "[terminal_def_restrict -> terminal_def_fragment _m83 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_E5 = { };
        private static ushort[][] p_StateShiftsOnVariable_E5 = { };
        private static ushort[][] p_StateReducsOnTerminal_E5 = { new ushort[2] { 0x47, 0x26 }, new ushort[2] { 0x3F, 0x26 }, new ushort[2] { 0x48, 0x26 }, new ushort[2] { 0x42, 0x26 } };

        private static ushort[] p_StateExpectedIDs_E6 = { 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_E6 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_E6 = { "[_m83 -> - . terminal_def_fragment _m83]", "[terminal_def_fragment -> . terminal_def_repetition _m81]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_E6 = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_E6 = { new ushort[2] { 0x5F, 0x106 }, new ushort[2] { 0x5E, 0xA7 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_E6 = { };

        private static ushort[] p_StateExpectedIDs_E7 = { 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_E7 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_E7 = { "[terminal_def_fragment -> terminal_def_repetition _m81 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_E7 = { };
        private static ushort[][] p_StateShiftsOnVariable_E7 = { };
        private static ushort[][] p_StateReducsOnTerminal_E7 = { new ushort[2] { 0x46, 0x25 }, new ushort[2] { 0x47, 0x25 }, new ushort[2] { 0x3F, 0x25 }, new ushort[2] { 0x48, 0x25 }, new ushort[2] { 0x42, 0x25 } };

        private static ushort[] p_StateExpectedIDs_E8 = { 0x46, 0x47, 0x3F, 0x48, 0x42, 0x41, 0xA, 0x32, 0x33, 0x2E, 0x2F, 0x31, 0x30 };
        private static string[] p_StateExpectedNames_E8 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK" };
        private static string[] p_StateItems_E8 = { "[_m81 -> terminal_def_repetition . _m81]", "[_m81 -> . terminal_def_repetition _m81]", "[_m81 -> . ]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . terminal_def_atom_ucat]", "[terminal_def_atom -> . terminal_def_atom_ublock]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]", "[terminal_def_atom_ucat -> . SYMBOL_TERMINAL_UCAT]", "[terminal_def_atom_ublock -> . SYMBOL_TERMINAL_UBLOCK]" };
        private static ushort[][] p_StateShiftsOnTerminal_E8 = { new ushort[2] { 0x41, 0xAA }, new ushort[2] { 0xA, 0xB1 }, new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 }, new ushort[2] { 0x2E, 0xA3 }, new ushort[2] { 0x2F, 0xB4 }, new ushort[2] { 0x31, 0xB5 }, new ushort[2] { 0x30, 0xB6 } };
        private static ushort[][] p_StateShiftsOnVariable_E8 = { new ushort[2] { 0x70, 0x107 }, new ushort[2] { 0x5E, 0xE8 }, new ushort[2] { 0x5D, 0xA8 }, new ushort[2] { 0x5C, 0xA9 }, new ushort[2] { 0x56, 0xAB }, new ushort[2] { 0x57, 0xAC }, new ushort[2] { 0x58, 0xAD }, new ushort[2] { 0x5B, 0xAE }, new ushort[2] { 0x5A, 0xAF }, new ushort[2] { 0x59, 0xB0 } };
        private static ushort[][] p_StateReducsOnTerminal_E8 = { new ushort[2] { 0x46, 0x47 }, new ushort[2] { 0x47, 0x47 }, new ushort[2] { 0x3F, 0x47 }, new ushort[2] { 0x48, 0x47 }, new ushort[2] { 0x42, 0x47 } };

        private static ushort[] p_StateExpectedIDs_E9 = { 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_E9 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_E9 = { "[terminal_def_repetition -> terminal_def_element * . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_E9 = { };
        private static ushort[][] p_StateShiftsOnVariable_E9 = { };
        private static ushort[][] p_StateReducsOnTerminal_E9 = { new ushort[2] { 0xA, 0x21 }, new ushort[2] { 0x41, 0x21 }, new ushort[2] { 0x32, 0x21 }, new ushort[2] { 0x2E, 0x21 }, new ushort[2] { 0x2F, 0x21 }, new ushort[2] { 0x31, 0x21 }, new ushort[2] { 0x30, 0x21 }, new ushort[2] { 0x33, 0x21 }, new ushort[2] { 0x46, 0x21 }, new ushort[2] { 0x47, 0x21 }, new ushort[2] { 0x3F, 0x21 }, new ushort[2] { 0x48, 0x21 }, new ushort[2] { 0x42, 0x21 } };

        private static ushort[] p_StateExpectedIDs_EA = { 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_EA = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_EA = { "[terminal_def_repetition -> terminal_def_element + . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_EA = { };
        private static ushort[][] p_StateShiftsOnVariable_EA = { };
        private static ushort[][] p_StateReducsOnTerminal_EA = { new ushort[2] { 0xA, 0x22 }, new ushort[2] { 0x41, 0x22 }, new ushort[2] { 0x32, 0x22 }, new ushort[2] { 0x2E, 0x22 }, new ushort[2] { 0x2F, 0x22 }, new ushort[2] { 0x31, 0x22 }, new ushort[2] { 0x30, 0x22 }, new ushort[2] { 0x33, 0x22 }, new ushort[2] { 0x46, 0x22 }, new ushort[2] { 0x47, 0x22 }, new ushort[2] { 0x3F, 0x22 }, new ushort[2] { 0x48, 0x22 }, new ushort[2] { 0x42, 0x22 } };

        private static ushort[] p_StateExpectedIDs_EB = { 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_EB = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_EB = { "[terminal_def_repetition -> terminal_def_element ? . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_EB = { };
        private static ushort[][] p_StateShiftsOnVariable_EB = { };
        private static ushort[][] p_StateReducsOnTerminal_EB = { new ushort[2] { 0xA, 0x23 }, new ushort[2] { 0x41, 0x23 }, new ushort[2] { 0x32, 0x23 }, new ushort[2] { 0x2E, 0x23 }, new ushort[2] { 0x2F, 0x23 }, new ushort[2] { 0x31, 0x23 }, new ushort[2] { 0x30, 0x23 }, new ushort[2] { 0x33, 0x23 }, new ushort[2] { 0x46, 0x23 }, new ushort[2] { 0x47, 0x23 }, new ushort[2] { 0x3F, 0x23 }, new ushort[2] { 0x48, 0x23 }, new ushort[2] { 0x42, 0x23 } };

        private static ushort[] p_StateExpectedIDs_EC = { 0x42 };
        private static string[] p_StateExpectedNames_EC = { "_T[)]" };
        private static string[] p_StateItems_EC = { "[terminal_def_element -> ( terminal_definition . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_EC = { new ushort[2] { 0x42, 0x108 } };
        private static ushort[][] p_StateShiftsOnVariable_EC = { };
        private static ushort[][] p_StateReducsOnTerminal_EC = { };

        private static ushort[] p_StateExpectedIDs_ED = { 0x32, 0x33 };
        private static string[] p_StateExpectedNames_ED = { "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16" };
        private static string[] p_StateItems_ED = { "[terminal_def_atom_span -> terminal_def_atom_unicode .. . terminal_def_atom_unicode]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]" };
        private static ushort[][] p_StateShiftsOnTerminal_ED = { new ushort[2] { 0x32, 0xB2 }, new ushort[2] { 0x33, 0xB3 } };
        private static ushort[][] p_StateShiftsOnVariable_ED = { new ushort[2] { 0x56, 0x109 } };
        private static ushort[][] p_StateReducsOnTerminal_ED = { };

        private static ushort[] p_StateExpectedIDs_EE = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_EE = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_EE = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_EE = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_EE = { new ushort[2] { 0x8A, 0x10A }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_EE = { };

        private static ushort[] p_StateExpectedIDs_EF = { 0x49 };
        private static string[] p_StateExpectedNames_EF = { "_T[->]" };
        private static string[] p_StateItems_EF = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params . -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_EF = { new ushort[2] { 0x49, 0x10B } };
        private static ushort[][] p_StateShiftsOnVariable_EF = { };
        private static ushort[][] p_StateReducsOnTerminal_EF = { };

        private static ushort[] p_StateExpectedIDs_F0 = { 0xD3, 0x42, 0x3F, 0x47 };
        private static string[] p_StateExpectedNames_F0 = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_StateItems_F0 = { "[_m178 -> | rule_def_restrict<grammar_bin_terminal> . _m178]", "[_m178 -> . | rule_def_restrict<grammar_bin_terminal> _m178]", "[_m178 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F0 = { new ushort[2] { 0x47, 0xBA } };
        private static ushort[][] p_StateShiftsOnVariable_F0 = { new ushort[2] { 0x96, 0x10C } };
        private static ushort[][] p_StateReducsOnTerminal_F0 = { new ushort[2] { 0xD3, 0x91 }, new ushort[2] { 0x42, 0x91 }, new ushort[2] { 0x3F, 0x91 } };

        private static ushort[] p_StateExpectedIDs_F1 = { 0x47, 0xD3, 0x42, 0x3F, 0x46 };
        private static string[] p_StateExpectedNames_F1 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_StateItems_F1 = { "[_m176 -> - rule_def_fragment<grammar_bin_terminal> . _m176]", "[_m176 -> . - rule_def_fragment<grammar_bin_terminal> _m176]", "[_m176 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F1 = { new ushort[2] { 0x46, 0xBC } };
        private static ushort[][] p_StateShiftsOnVariable_F1 = { new ushort[2] { 0x95, 0x10D } };
        private static ushort[][] p_StateReducsOnTerminal_F1 = { new ushort[2] { 0x47, 0x8F }, new ushort[2] { 0xD3, 0x8F }, new ushort[2] { 0x42, 0x8F }, new ushort[2] { 0x3F, 0x8F } };

        private static ushort[] p_StateExpectedIDs_F2 = { 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_F2 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_F2 = { "[_m174 -> rule_def_repetition<grammar_bin_terminal> _m174 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F2 = { };
        private static ushort[][] p_StateShiftsOnVariable_F2 = { };
        private static ushort[][] p_StateReducsOnTerminal_F2 = { new ushort[2] { 0x46, 0x8C }, new ushort[2] { 0x47, 0x8C }, new ushort[2] { 0xD3, 0x8C }, new ushort[2] { 0x42, 0x8C }, new ushort[2] { 0x3F, 0x8C } };

        private static ushort[] p_StateExpectedIDs_F3 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_F3 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_F3 = { "[rule_def_element<grammar_bin_terminal> -> ( rule_definition<grammar_bin_terminal> ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F3 = { };
        private static ushort[][] p_StateShiftsOnVariable_F3 = { };
        private static ushort[][] p_StateReducsOnTerminal_F3 = { new ushort[2] { 0x53, 0x82 }, new ushort[2] { 0x54, 0x82 }, new ushort[2] { 0x43, 0x82 }, new ushort[2] { 0x44, 0x82 }, new ushort[2] { 0x45, 0x82 }, new ushort[2] { 0x41, 0x82 }, new ushort[2] { 0x2D, 0x82 }, new ushort[2] { 0xA, 0x82 }, new ushort[2] { 0x32, 0x82 }, new ushort[2] { 0x33, 0x82 }, new ushort[2] { 0x11, 0x82 }, new ushort[2] { 0x34, 0x82 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x82 }, new ushort[2] { 0x37, 0x82 }, new ushort[2] { 0x38, 0x82 }, new ushort[2] { 0x39, 0x82 }, new ushort[2] { 0x3A, 0x82 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x82 }, new ushort[2] { 0x3D, 0x82 }, new ushort[2] { 0x46, 0x82 }, new ushort[2] { 0x47, 0x82 }, new ushort[2] { 0xD3, 0x82 }, new ushort[2] { 0x42, 0x82 }, new ushort[2] { 0x3F, 0x82 } };

        private static ushort[] p_StateExpectedIDs_F4 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x2E, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_F4 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_TERMINAL_TEXT", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_F4 = { "[rule_sym_action -> { qualified_name } . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F4 = { };
        private static ushort[][] p_StateShiftsOnVariable_F4 = { };
        private static ushort[][] p_StateReducsOnTerminal_F4 = { new ushort[2] { 0x53, 0x2B }, new ushort[2] { 0x54, 0x2B }, new ushort[2] { 0x43, 0x2B }, new ushort[2] { 0x44, 0x2B }, new ushort[2] { 0x45, 0x2B }, new ushort[2] { 0x41, 0x2B }, new ushort[2] { 0x2D, 0x2B }, new ushort[2] { 0xA, 0x2B }, new ushort[2] { 0x32, 0x2B }, new ushort[2] { 0x33, 0x2B }, new ushort[2] { 0x11, 0x2B }, new ushort[2] { 0x34, 0x2B }, new ushort[2] { 0x35, 0x2B }, new ushort[2] { 0x36, 0x2B }, new ushort[2] { 0x37, 0x2B }, new ushort[2] { 0x38, 0x2B }, new ushort[2] { 0x39, 0x2B }, new ushort[2] { 0x3A, 0x2B }, new ushort[2] { 0x3B, 0x2B }, new ushort[2] { 0x3C, 0x2B }, new ushort[2] { 0x3D, 0x2B }, new ushort[2] { 0x46, 0x2B }, new ushort[2] { 0x47, 0x2B }, new ushort[2] { 0xD3, 0x2B }, new ushort[2] { 0x2E, 0x2B }, new ushort[2] { 0x42, 0x2B }, new ushort[2] { 0x3F, 0x2B }, new ushort[2] { 0x4C, 0x2B }, new ushort[2] { 0x4B, 0x2B } };

        private static ushort[] p_StateExpectedIDs_F5 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_F5 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_F5 = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> . _m165 >]", "[_m165 -> . , rule_def_atom<grammar_bin_terminal> _m165]", "[_m165 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F5 = { new ushort[2] { 0x4B, 0x10F } };
        private static ushort[][] p_StateShiftsOnVariable_F5 = { new ushort[2] { 0x93, 0x10E } };
        private static ushort[][] p_StateReducsOnTerminal_F5 = { new ushort[2] { 0x4C, 0x8B } };

        private static ushort[] p_StateExpectedIDs_F6 = { 0x3F };
        private static string[] p_StateExpectedNames_F6 = { "_T[;]" };
        private static string[] p_StateItems_F6 = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_F6 = { new ushort[2] { 0x3F, 0x110 } };
        private static ushort[][] p_StateShiftsOnVariable_F6 = { };
        private static ushort[][] p_StateReducsOnTerminal_F6 = { };

        private static ushort[] p_StateExpectedIDs_F7 = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_F7 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_F7 = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_F7 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_F7 = { new ushort[2] { 0x79, 0x111 }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_F7 = { };

        private static ushort[] p_StateExpectedIDs_F8 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_F8 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_F8 = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F8 = { };
        private static ushort[][] p_StateShiftsOnVariable_F8 = { };
        private static ushort[][] p_StateReducsOnTerminal_F8 = { new ushort[2] { 0xA, 0x76 }, new ushort[2] { 0x12, 0x76 } };

        private static ushort[] p_StateExpectedIDs_F9 = { 0x3F };
        private static string[] p_StateExpectedNames_F9 = { "_T[;]" };
        private static string[] p_StateItems_F9 = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_F9 = { new ushort[2] { 0x3F, 0x112 } };
        private static ushort[][] p_StateShiftsOnVariable_F9 = { };
        private static ushort[][] p_StateReducsOnTerminal_F9 = { };

        private static ushort[] p_StateExpectedIDs_FA = { 0x4C };
        private static string[] p_StateExpectedNames_FA = { "_T[>]" };
        private static string[] p_StateItems_FA = { "[rule_template_params -> < NAME _m93 . >]" };
        private static ushort[][] p_StateShiftsOnTerminal_FA = { new ushort[2] { 0x4C, 0x113 } };
        private static ushort[][] p_StateShiftsOnVariable_FA = { };
        private static ushort[][] p_StateReducsOnTerminal_FA = { };

        private static ushort[] p_StateExpectedIDs_FB = { 0xA };
        private static string[] p_StateExpectedNames_FB = { "NAME" };
        private static string[] p_StateItems_FB = { "[_m93 -> , . NAME _m93]" };
        private static ushort[][] p_StateShiftsOnTerminal_FB = { new ushort[2] { 0xA, 0x114 } };
        private static ushort[][] p_StateShiftsOnVariable_FB = { };
        private static ushort[][] p_StateReducsOnTerminal_FB = { };

        private static ushort[] p_StateExpectedIDs_FC = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_FC = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_FC = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_FC = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_FC = { new ushort[2] { 0x79, 0x115 }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_FC = { };

        private static ushort[] p_StateExpectedIDs_FD = { 0x49 };
        private static string[] p_StateExpectedNames_FD = { "_T[->]" };
        private static string[] p_StateItems_FD = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params . -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_FD = { new ushort[2] { 0x49, 0x116 } };
        private static ushort[][] p_StateShiftsOnVariable_FD = { };
        private static ushort[][] p_StateReducsOnTerminal_FD = { };

        private static ushort[] p_StateExpectedIDs_FE = { 0xD3, 0x42, 0x3F, 0x47 };
        private static string[] p_StateExpectedNames_FE = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_StateItems_FE = { "[_m138 -> | rule_def_restrict<grammar_text_terminal> . _m138]", "[_m138 -> . | rule_def_restrict<grammar_text_terminal> _m138]", "[_m138 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_FE = { new ushort[2] { 0x47, 0xD4 } };
        private static ushort[][] p_StateShiftsOnVariable_FE = { new ushort[2] { 0x85, 0x117 } };
        private static ushort[][] p_StateReducsOnTerminal_FE = { new ushort[2] { 0xD3, 0x70 }, new ushort[2] { 0x42, 0x70 }, new ushort[2] { 0x3F, 0x70 } };

        private static ushort[] p_StateExpectedIDs_FF = { 0x47, 0xD3, 0x42, 0x3F, 0x46 };
        private static string[] p_StateExpectedNames_FF = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_StateItems_FF = { "[_m136 -> - rule_def_fragment<grammar_text_terminal> . _m136]", "[_m136 -> . - rule_def_fragment<grammar_text_terminal> _m136]", "[_m136 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_FF = { new ushort[2] { 0x46, 0xD6 } };
        private static ushort[][] p_StateShiftsOnVariable_FF = { new ushort[2] { 0x84, 0x118 } };
        private static ushort[][] p_StateReducsOnTerminal_FF = { new ushort[2] { 0x47, 0x6E }, new ushort[2] { 0xD3, 0x6E }, new ushort[2] { 0x42, 0x6E }, new ushort[2] { 0x3F, 0x6E } };

        private static ushort[] p_StateExpectedIDs_100 = { 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_100 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_100 = { "[_m134 -> rule_def_repetition<grammar_text_terminal> _m134 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_100 = { };
        private static ushort[][] p_StateShiftsOnVariable_100 = { };
        private static ushort[][] p_StateReducsOnTerminal_100 = { new ushort[2] { 0x46, 0x6B }, new ushort[2] { 0x47, 0x6B }, new ushort[2] { 0xD3, 0x6B }, new ushort[2] { 0x42, 0x6B }, new ushort[2] { 0x3F, 0x6B } };

        private static ushort[] p_StateExpectedIDs_101 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_101 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_101 = { "[rule_def_element<grammar_text_terminal> -> ( rule_definition<grammar_text_terminal> ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_101 = { };
        private static ushort[][] p_StateShiftsOnVariable_101 = { };
        private static ushort[][] p_StateReducsOnTerminal_101 = { new ushort[2] { 0x53, 0x61 }, new ushort[2] { 0x54, 0x61 }, new ushort[2] { 0x43, 0x61 }, new ushort[2] { 0x44, 0x61 }, new ushort[2] { 0x45, 0x61 }, new ushort[2] { 0x41, 0x61 }, new ushort[2] { 0x2D, 0x61 }, new ushort[2] { 0xA, 0x61 }, new ushort[2] { 0x2E, 0x61 }, new ushort[2] { 0x11, 0x61 }, new ushort[2] { 0x46, 0x61 }, new ushort[2] { 0x47, 0x61 }, new ushort[2] { 0xD3, 0x61 }, new ushort[2] { 0x42, 0x61 }, new ushort[2] { 0x3F, 0x61 } };

        private static ushort[] p_StateExpectedIDs_102 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_102 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_102 = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> . _m125 >]", "[_m125 -> . , rule_def_atom<grammar_text_terminal> _m125]", "[_m125 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_102 = { new ushort[2] { 0x4B, 0x11A } };
        private static ushort[][] p_StateShiftsOnVariable_102 = { new ushort[2] { 0x82, 0x119 } };
        private static ushort[][] p_StateReducsOnTerminal_102 = { new ushort[2] { 0x4C, 0x6A } };

        private static ushort[] p_StateExpectedIDs_103 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_103 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_103 = { "[terminal -> NAME -> terminal_definition terminal_subgrammar ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_103 = { };
        private static ushort[][] p_StateShiftsOnVariable_103 = { };
        private static ushort[][] p_StateReducsOnTerminal_103 = { new ushort[2] { 0xA, 0x2A }, new ushort[2] { 0x12, 0x2A } };

        private static ushort[] p_StateExpectedIDs_104 = { 0x3F };
        private static string[] p_StateExpectedNames_104 = { "_T[;]" };
        private static string[] p_StateItems_104 = { "[terminal_subgrammar -> => qualified_name . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_104 = { };
        private static ushort[][] p_StateShiftsOnVariable_104 = { };
        private static ushort[][] p_StateReducsOnTerminal_104 = { new ushort[2] { 0x3F, 0x28 } };

        private static ushort[] p_StateExpectedIDs_105 = { 0x3F, 0x48, 0x42, 0x47 };
        private static string[] p_StateExpectedNames_105 = { "_T[;]", "_T[=>]", "_T[)]", "_T[|]" };
        private static string[] p_StateItems_105 = { "[_m85 -> | terminal_def_restrict . _m85]", "[_m85 -> . | terminal_def_restrict _m85]", "[_m85 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_105 = { new ushort[2] { 0x47, 0xE4 } };
        private static ushort[][] p_StateShiftsOnVariable_105 = { new ushort[2] { 0x72, 0x11B } };
        private static ushort[][] p_StateReducsOnTerminal_105 = { new ushort[2] { 0x3F, 0x4B }, new ushort[2] { 0x48, 0x4B }, new ushort[2] { 0x42, 0x4B } };

        private static ushort[] p_StateExpectedIDs_106 = { 0x47, 0x3F, 0x48, 0x42, 0x46 };
        private static string[] p_StateExpectedNames_106 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[-]" };
        private static string[] p_StateItems_106 = { "[_m83 -> - terminal_def_fragment . _m83]", "[_m83 -> . - terminal_def_fragment _m83]", "[_m83 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_106 = { new ushort[2] { 0x46, 0xE6 } };
        private static ushort[][] p_StateShiftsOnVariable_106 = { new ushort[2] { 0x71, 0x11C } };
        private static ushort[][] p_StateReducsOnTerminal_106 = { new ushort[2] { 0x47, 0x49 }, new ushort[2] { 0x3F, 0x49 }, new ushort[2] { 0x48, 0x49 }, new ushort[2] { 0x42, 0x49 } };

        private static ushort[] p_StateExpectedIDs_107 = { 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_107 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_107 = { "[_m81 -> terminal_def_repetition _m81 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_107 = { };
        private static ushort[][] p_StateShiftsOnVariable_107 = { };
        private static ushort[][] p_StateReducsOnTerminal_107 = { new ushort[2] { 0x46, 0x46 }, new ushort[2] { 0x47, 0x46 }, new ushort[2] { 0x3F, 0x46 }, new ushort[2] { 0x48, 0x46 }, new ushort[2] { 0x42, 0x46 } };

        private static ushort[] p_StateExpectedIDs_108 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_108 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_108 = { "[terminal_def_element -> ( terminal_definition ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_108 = { };
        private static ushort[][] p_StateShiftsOnVariable_108 = { };
        private static ushort[][] p_StateReducsOnTerminal_108 = { new ushort[2] { 0x43, 0x20 }, new ushort[2] { 0x44, 0x20 }, new ushort[2] { 0x45, 0x20 }, new ushort[2] { 0xA, 0x20 }, new ushort[2] { 0x41, 0x20 }, new ushort[2] { 0x32, 0x20 }, new ushort[2] { 0x2E, 0x20 }, new ushort[2] { 0x2F, 0x20 }, new ushort[2] { 0x31, 0x20 }, new ushort[2] { 0x30, 0x20 }, new ushort[2] { 0x33, 0x20 }, new ushort[2] { 0x46, 0x20 }, new ushort[2] { 0x47, 0x20 }, new ushort[2] { 0x3F, 0x20 }, new ushort[2] { 0x48, 0x20 }, new ushort[2] { 0x42, 0x20 } };

        private static ushort[] p_StateExpectedIDs_109 = { 0x43, 0x44, 0x45, 0xA, 0x41, 0x32, 0x2E, 0x2F, 0x31, 0x30, 0x33, 0x46, 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_109 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_TERMINAL_UCAT", "SYMBOL_TERMINAL_UBLOCK", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_109 = { "[terminal_def_atom_span -> terminal_def_atom_unicode .. terminal_def_atom_unicode . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_109 = { };
        private static ushort[][] p_StateShiftsOnVariable_109 = { };
        private static ushort[][] p_StateReducsOnTerminal_109 = { new ushort[2] { 0x43, 0x17 }, new ushort[2] { 0x44, 0x17 }, new ushort[2] { 0x45, 0x17 }, new ushort[2] { 0xA, 0x17 }, new ushort[2] { 0x41, 0x17 }, new ushort[2] { 0x32, 0x17 }, new ushort[2] { 0x2E, 0x17 }, new ushort[2] { 0x2F, 0x17 }, new ushort[2] { 0x31, 0x17 }, new ushort[2] { 0x30, 0x17 }, new ushort[2] { 0x33, 0x17 }, new ushort[2] { 0x46, 0x17 }, new ushort[2] { 0x47, 0x17 }, new ushort[2] { 0x3F, 0x17 }, new ushort[2] { 0x48, 0x17 }, new ushort[2] { 0x42, 0x17 } };

        private static ushort[] p_StateExpectedIDs_10A = { 0x3F };
        private static string[] p_StateExpectedNames_10A = { "_T[;]" };
        private static string[] p_StateItems_10A = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_10A = { new ushort[2] { 0x3F, 0x11D } };
        private static ushort[][] p_StateShiftsOnVariable_10A = { };
        private static ushort[][] p_StateReducsOnTerminal_10A = { };

        private static ushort[] p_StateExpectedIDs_10B = { 0x41, 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_10B = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_10B = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m178]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m176]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m174]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_10B = { new ushort[2] { 0x41, 0x70 }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_10B = { new ushort[2] { 0x8A, 0x11E }, new ushort[2] { 0x8B, 0x6A }, new ushort[2] { 0x8C, 0x6B }, new ushort[2] { 0x8D, 0x6C }, new ushort[2] { 0x8E, 0x6D }, new ushort[2] { 0x8F, 0x6E }, new ushort[2] { 0x90, 0x6F }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_10B = { };

        private static ushort[] p_StateExpectedIDs_10C = { 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_10C = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_10C = { "[_m178 -> | rule_def_restrict<grammar_bin_terminal> _m178 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_10C = { };
        private static ushort[][] p_StateShiftsOnVariable_10C = { };
        private static ushort[][] p_StateReducsOnTerminal_10C = { new ushort[2] { 0xD3, 0x90 }, new ushort[2] { 0x42, 0x90 }, new ushort[2] { 0x3F, 0x90 } };

        private static ushort[] p_StateExpectedIDs_10D = { 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_10D = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_10D = { "[_m176 -> - rule_def_fragment<grammar_bin_terminal> _m176 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_10D = { };
        private static ushort[][] p_StateShiftsOnVariable_10D = { };
        private static ushort[][] p_StateReducsOnTerminal_10D = { new ushort[2] { 0x47, 0x8E }, new ushort[2] { 0xD3, 0x8E }, new ushort[2] { 0x42, 0x8E }, new ushort[2] { 0x3F, 0x8E } };

        private static ushort[] p_StateExpectedIDs_10E = { 0x4C };
        private static string[] p_StateExpectedNames_10E = { "_T[>]" };
        private static string[] p_StateItems_10E = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> _m165 . >]" };
        private static ushort[][] p_StateShiftsOnTerminal_10E = { new ushort[2] { 0x4C, 0x11F } };
        private static ushort[][] p_StateShiftsOnVariable_10E = { };
        private static ushort[][] p_StateReducsOnTerminal_10E = { };

        private static ushort[] p_StateExpectedIDs_10F = { 0x11, 0x2D, 0xA, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D };
        private static string[] p_StateExpectedNames_10F = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_StateItems_10F = { "[_m165 -> , . rule_def_atom<grammar_bin_terminal> _m165]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_StateShiftsOnTerminal_10F = { new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0x78 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x38, 0x7F }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x3A, 0x81 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x3D, 0x84 } };
        private static ushort[][] p_StateShiftsOnVariable_10F = { new ushort[2] { 0x90, 0x120 }, new ushort[2] { 0x64, 0x71 }, new ushort[2] { 0x65, 0x72 }, new ushort[2] { 0x66, 0x73 }, new ushort[2] { 0x91, 0x74 }, new ushort[2] { 0x68, 0x75 } };
        private static ushort[][] p_StateReducsOnTerminal_10F = { };

        private static ushort[] p_StateExpectedIDs_110 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_110 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_110 = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_110 = { };
        private static ushort[][] p_StateShiftsOnVariable_110 = { };
        private static ushort[][] p_StateReducsOnTerminal_110 = { new ushort[2] { 0xA, 0x55 }, new ushort[2] { 0x12, 0x55 } };

        private static ushort[] p_StateExpectedIDs_111 = { 0x3F };
        private static string[] p_StateExpectedNames_111 = { "_T[;]" };
        private static string[] p_StateItems_111 = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_111 = { new ushort[2] { 0x3F, 0x121 } };
        private static ushort[][] p_StateShiftsOnVariable_111 = { };
        private static ushort[][] p_StateReducsOnTerminal_111 = { };

        private static ushort[] p_StateExpectedIDs_112 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_112 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_112 = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_112 = { };
        private static ushort[][] p_StateShiftsOnVariable_112 = { };
        private static ushort[][] p_StateReducsOnTerminal_112 = { new ushort[2] { 0xA, 0x92 }, new ushort[2] { 0x12, 0x92 } };

        private static ushort[] p_StateExpectedIDs_113 = { 0x49 };
        private static string[] p_StateExpectedNames_113 = { "_T[->]" };
        private static string[] p_StateItems_113 = { "[rule_template_params -> < NAME _m93 > . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_113 = { };
        private static ushort[][] p_StateShiftsOnVariable_113 = { };
        private static ushort[][] p_StateReducsOnTerminal_113 = { new ushort[2] { 0x49, 0x2E } };

        private static ushort[] p_StateExpectedIDs_114 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_114 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_114 = { "[_m93 -> , NAME . _m93]", "[_m93 -> . , NAME _m93]", "[_m93 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_114 = { new ushort[2] { 0x4B, 0xFB } };
        private static ushort[][] p_StateShiftsOnVariable_114 = { new ushort[2] { 0x73, 0x122 } };
        private static ushort[][] p_StateReducsOnTerminal_114 = { new ushort[2] { 0x4C, 0x4D } };

        private static ushort[] p_StateExpectedIDs_115 = { 0x3F };
        private static string[] p_StateExpectedNames_115 = { "_T[;]" };
        private static string[] p_StateItems_115 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_115 = { new ushort[2] { 0x3F, 0x123 } };
        private static ushort[][] p_StateShiftsOnVariable_115 = { };
        private static ushort[][] p_StateReducsOnTerminal_115 = { };

        private static ushort[] p_StateExpectedIDs_116 = { 0x41, 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_116 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_116 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m138]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m136]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m134]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_116 = { new ushort[2] { 0x41, 0x9B }, new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_116 = { new ushort[2] { 0x79, 0x124 }, new ushort[2] { 0x7A, 0x95 }, new ushort[2] { 0x7B, 0x96 }, new ushort[2] { 0x7C, 0x97 }, new ushort[2] { 0x7D, 0x98 }, new ushort[2] { 0x7E, 0x99 }, new ushort[2] { 0x7F, 0x9A }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_116 = { };

        private static ushort[] p_StateExpectedIDs_117 = { 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_117 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_117 = { "[_m138 -> | rule_def_restrict<grammar_text_terminal> _m138 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_117 = { };
        private static ushort[][] p_StateShiftsOnVariable_117 = { };
        private static ushort[][] p_StateReducsOnTerminal_117 = { new ushort[2] { 0xD3, 0x6F }, new ushort[2] { 0x42, 0x6F }, new ushort[2] { 0x3F, 0x6F } };

        private static ushort[] p_StateExpectedIDs_118 = { 0x47, 0xD3, 0x42, 0x3F };
        private static string[] p_StateExpectedNames_118 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_StateItems_118 = { "[_m136 -> - rule_def_fragment<grammar_text_terminal> _m136 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_118 = { };
        private static ushort[][] p_StateShiftsOnVariable_118 = { };
        private static ushort[][] p_StateReducsOnTerminal_118 = { new ushort[2] { 0x47, 0x6D }, new ushort[2] { 0xD3, 0x6D }, new ushort[2] { 0x42, 0x6D }, new ushort[2] { 0x3F, 0x6D } };

        private static ushort[] p_StateExpectedIDs_119 = { 0x4C };
        private static string[] p_StateExpectedNames_119 = { "_T[>]" };
        private static string[] p_StateItems_119 = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> _m125 . >]" };
        private static ushort[][] p_StateShiftsOnTerminal_119 = { new ushort[2] { 0x4C, 0x125 } };
        private static ushort[][] p_StateShiftsOnVariable_119 = { };
        private static ushort[][] p_StateReducsOnTerminal_119 = { };

        private static ushort[] p_StateExpectedIDs_11A = { 0x11, 0x2D, 0xA, 0x2E };
        private static string[] p_StateExpectedNames_11A = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_StateItems_11A = { "[_m125 -> , . rule_def_atom<grammar_text_terminal> _m125]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_StateShiftsOnTerminal_11A = { new ushort[2] { 0x11, 0x76 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0x2E, 0xA3 } };
        private static ushort[][] p_StateShiftsOnVariable_11A = { new ushort[2] { 0x7F, 0x126 }, new ushort[2] { 0x64, 0x9C }, new ushort[2] { 0x65, 0x9D }, new ushort[2] { 0x66, 0x9E }, new ushort[2] { 0x80, 0x9F }, new ushort[2] { 0x69, 0xA0 }, new ushort[2] { 0x57, 0xA2 } };
        private static ushort[][] p_StateReducsOnTerminal_11A = { };

        private static ushort[] p_StateExpectedIDs_11B = { 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_11B = { "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_11B = { "[_m85 -> | terminal_def_restrict _m85 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11B = { };
        private static ushort[][] p_StateShiftsOnVariable_11B = { };
        private static ushort[][] p_StateReducsOnTerminal_11B = { new ushort[2] { 0x3F, 0x4A }, new ushort[2] { 0x48, 0x4A }, new ushort[2] { 0x42, 0x4A } };

        private static ushort[] p_StateExpectedIDs_11C = { 0x47, 0x3F, 0x48, 0x42 };
        private static string[] p_StateExpectedNames_11C = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_StateItems_11C = { "[_m83 -> - terminal_def_fragment _m83 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11C = { };
        private static ushort[][] p_StateShiftsOnVariable_11C = { };
        private static ushort[][] p_StateReducsOnTerminal_11C = { new ushort[2] { 0x47, 0x48 }, new ushort[2] { 0x3F, 0x48 }, new ushort[2] { 0x48, 0x48 }, new ushort[2] { 0x42, 0x48 } };

        private static ushort[] p_StateExpectedIDs_11D = { 0xA, 0xD2, 0x12 };
        private static string[] p_StateExpectedNames_11D = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_StateItems_11D = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11D = { };
        private static ushort[][] p_StateShiftsOnVariable_11D = { };
        private static ushort[][] p_StateReducsOnTerminal_11D = { new ushort[2] { 0xA, 0xA1 }, new ushort[2] { 0xD2, 0xA1 }, new ushort[2] { 0x12, 0xA1 } };

        private static ushort[] p_StateExpectedIDs_11E = { 0x3F };
        private static string[] p_StateExpectedNames_11E = { "_T[;]" };
        private static string[] p_StateItems_11E = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_11E = { new ushort[2] { 0x3F, 0x127 } };
        private static ushort[][] p_StateShiftsOnVariable_11E = { };
        private static ushort[][] p_StateReducsOnTerminal_11E = { };

        private static ushort[] p_StateExpectedIDs_11F = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x32, 0x33, 0x11, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_11F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_11F = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> _m165 > . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11F = { };
        private static ushort[][] p_StateShiftsOnVariable_11F = { };
        private static ushort[][] p_StateReducsOnTerminal_11F = { new ushort[2] { 0x53, 0x89 }, new ushort[2] { 0x54, 0x89 }, new ushort[2] { 0x43, 0x89 }, new ushort[2] { 0x44, 0x89 }, new ushort[2] { 0x45, 0x89 }, new ushort[2] { 0x41, 0x89 }, new ushort[2] { 0x2D, 0x89 }, new ushort[2] { 0xA, 0x89 }, new ushort[2] { 0x32, 0x89 }, new ushort[2] { 0x33, 0x89 }, new ushort[2] { 0x11, 0x89 }, new ushort[2] { 0x34, 0x89 }, new ushort[2] { 0x35, 0x89 }, new ushort[2] { 0x36, 0x89 }, new ushort[2] { 0x37, 0x89 }, new ushort[2] { 0x38, 0x89 }, new ushort[2] { 0x39, 0x89 }, new ushort[2] { 0x3A, 0x89 }, new ushort[2] { 0x3B, 0x89 }, new ushort[2] { 0x3C, 0x89 }, new ushort[2] { 0x3D, 0x89 }, new ushort[2] { 0x46, 0x89 }, new ushort[2] { 0x47, 0x89 }, new ushort[2] { 0xD3, 0x89 }, new ushort[2] { 0x42, 0x89 }, new ushort[2] { 0x3F, 0x89 }, new ushort[2] { 0x4C, 0x89 }, new ushort[2] { 0x4B, 0x89 } };

        private static ushort[] p_StateExpectedIDs_120 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_120 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_120 = { "[_m165 -> , rule_def_atom<grammar_bin_terminal> . _m165]", "[_m165 -> . , rule_def_atom<grammar_bin_terminal> _m165]", "[_m165 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_120 = { new ushort[2] { 0x4B, 0x10F } };
        private static ushort[][] p_StateShiftsOnVariable_120 = { new ushort[2] { 0x93, 0x128 } };
        private static ushort[][] p_StateReducsOnTerminal_120 = { new ushort[2] { 0x4C, 0x8B } };

        private static ushort[] p_StateExpectedIDs_121 = { 0xA, 0x12 };
        private static string[] p_StateExpectedNames_121 = { "NAME", "_T[}]" };
        private static string[] p_StateItems_121 = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_121 = { };
        private static ushort[][] p_StateShiftsOnVariable_121 = { };
        private static ushort[][] p_StateReducsOnTerminal_121 = { new ushort[2] { 0xA, 0x71 }, new ushort[2] { 0x12, 0x71 } };

        private static ushort[] p_StateExpectedIDs_122 = { 0x4C };
        private static string[] p_StateExpectedNames_122 = { "_T[>]" };
        private static string[] p_StateItems_122 = { "[_m93 -> , NAME _m93 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_122 = { };
        private static ushort[][] p_StateShiftsOnVariable_122 = { };
        private static ushort[][] p_StateReducsOnTerminal_122 = { new ushort[2] { 0x4C, 0x4C } };

        private static ushort[] p_StateExpectedIDs_123 = { 0xA, 0xD2, 0x12 };
        private static string[] p_StateExpectedNames_123 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_StateItems_123 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_123 = { };
        private static ushort[][] p_StateShiftsOnVariable_123 = { };
        private static ushort[][] p_StateReducsOnTerminal_123 = { new ushort[2] { 0xA, 0x99 }, new ushort[2] { 0xD2, 0x99 }, new ushort[2] { 0x12, 0x99 } };

        private static ushort[] p_StateExpectedIDs_124 = { 0x3F };
        private static string[] p_StateExpectedNames_124 = { "_T[;]" };
        private static string[] p_StateItems_124 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_StateShiftsOnTerminal_124 = { new ushort[2] { 0x3F, 0x129 } };
        private static ushort[][] p_StateShiftsOnVariable_124 = { };
        private static ushort[][] p_StateReducsOnTerminal_124 = { };

        private static ushort[] p_StateExpectedIDs_125 = { 0x53, 0x54, 0x43, 0x44, 0x45, 0x41, 0x2D, 0xA, 0x2E, 0x11, 0x46, 0x47, 0xD3, 0x42, 0x3F, 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_125 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_StateItems_125 = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> _m125 > . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_125 = { };
        private static ushort[][] p_StateShiftsOnVariable_125 = { };
        private static ushort[][] p_StateReducsOnTerminal_125 = { new ushort[2] { 0x53, 0x68 }, new ushort[2] { 0x54, 0x68 }, new ushort[2] { 0x43, 0x68 }, new ushort[2] { 0x44, 0x68 }, new ushort[2] { 0x45, 0x68 }, new ushort[2] { 0x41, 0x68 }, new ushort[2] { 0x2D, 0x68 }, new ushort[2] { 0xA, 0x68 }, new ushort[2] { 0x2E, 0x68 }, new ushort[2] { 0x11, 0x68 }, new ushort[2] { 0x46, 0x68 }, new ushort[2] { 0x47, 0x68 }, new ushort[2] { 0xD3, 0x68 }, new ushort[2] { 0x42, 0x68 }, new ushort[2] { 0x3F, 0x68 }, new ushort[2] { 0x4C, 0x68 }, new ushort[2] { 0x4B, 0x68 } };

        private static ushort[] p_StateExpectedIDs_126 = { 0x4C, 0x4B };
        private static string[] p_StateExpectedNames_126 = { "_T[>]", "_T[,]" };
        private static string[] p_StateItems_126 = { "[_m125 -> , rule_def_atom<grammar_text_terminal> . _m125]", "[_m125 -> . , rule_def_atom<grammar_text_terminal> _m125]", "[_m125 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_126 = { new ushort[2] { 0x4B, 0x11A } };
        private static ushort[][] p_StateShiftsOnVariable_126 = { new ushort[2] { 0x82, 0x12A } };
        private static ushort[][] p_StateReducsOnTerminal_126 = { new ushort[2] { 0x4C, 0x6A } };

        private static ushort[] p_StateExpectedIDs_127 = { 0xA, 0xD2, 0x12 };
        private static string[] p_StateExpectedNames_127 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_StateItems_127 = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_127 = { };
        private static ushort[][] p_StateShiftsOnVariable_127 = { };
        private static ushort[][] p_StateReducsOnTerminal_127 = { new ushort[2] { 0xA, 0xA4 }, new ushort[2] { 0xD2, 0xA4 }, new ushort[2] { 0x12, 0xA4 } };

        private static ushort[] p_StateExpectedIDs_128 = { 0x4C };
        private static string[] p_StateExpectedNames_128 = { "_T[>]" };
        private static string[] p_StateItems_128 = { "[_m165 -> , rule_def_atom<grammar_bin_terminal> _m165 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_128 = { };
        private static ushort[][] p_StateShiftsOnVariable_128 = { };
        private static ushort[][] p_StateReducsOnTerminal_128 = { new ushort[2] { 0x4C, 0x8A } };

        private static ushort[] p_StateExpectedIDs_129 = { 0xA, 0xD2, 0x12 };
        private static string[] p_StateExpectedNames_129 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_StateItems_129 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_129 = { };
        private static ushort[][] p_StateShiftsOnVariable_129 = { };
        private static ushort[][] p_StateReducsOnTerminal_129 = { new ushort[2] { 0xA, 0x9C }, new ushort[2] { 0xD2, 0x9C }, new ushort[2] { 0x12, 0x9C } };

        private static ushort[] p_StateExpectedIDs_12A = { 0x4C };
        private static string[] p_StateExpectedNames_12A = { "_T[>]" };
        private static string[] p_StateItems_12A = { "[_m125 -> , rule_def_atom<grammar_text_terminal> _m125 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_12A = { };
        private static ushort[][] p_StateShiftsOnVariable_12A = { };
        private static ushort[][] p_StateReducsOnTerminal_12A = { new ushort[2] { 0x4C, 0x69 } };

        private static ushort[][] p_StateExpectedIDs = { p_StateExpectedIDs_0, p_StateExpectedIDs_1, p_StateExpectedIDs_2, p_StateExpectedIDs_3, p_StateExpectedIDs_4, p_StateExpectedIDs_5, p_StateExpectedIDs_6, p_StateExpectedIDs_7, p_StateExpectedIDs_8, p_StateExpectedIDs_9, p_StateExpectedIDs_A, p_StateExpectedIDs_B, p_StateExpectedIDs_C, p_StateExpectedIDs_D, p_StateExpectedIDs_E, p_StateExpectedIDs_F, p_StateExpectedIDs_10, p_StateExpectedIDs_11, p_StateExpectedIDs_12, p_StateExpectedIDs_13, p_StateExpectedIDs_14, p_StateExpectedIDs_15, p_StateExpectedIDs_16, p_StateExpectedIDs_17, p_StateExpectedIDs_18, p_StateExpectedIDs_19, p_StateExpectedIDs_1A, p_StateExpectedIDs_1B, p_StateExpectedIDs_1C, p_StateExpectedIDs_1D, p_StateExpectedIDs_1E, p_StateExpectedIDs_1F, p_StateExpectedIDs_20, p_StateExpectedIDs_21, p_StateExpectedIDs_22, p_StateExpectedIDs_23, p_StateExpectedIDs_24, p_StateExpectedIDs_25, p_StateExpectedIDs_26, p_StateExpectedIDs_27, p_StateExpectedIDs_28, p_StateExpectedIDs_29, p_StateExpectedIDs_2A, p_StateExpectedIDs_2B, p_StateExpectedIDs_2C, p_StateExpectedIDs_2D, p_StateExpectedIDs_2E, p_StateExpectedIDs_2F, p_StateExpectedIDs_30, p_StateExpectedIDs_31, p_StateExpectedIDs_32, p_StateExpectedIDs_33, p_StateExpectedIDs_34, p_StateExpectedIDs_35, p_StateExpectedIDs_36, p_StateExpectedIDs_37, p_StateExpectedIDs_38, p_StateExpectedIDs_39, p_StateExpectedIDs_3A, p_StateExpectedIDs_3B, p_StateExpectedIDs_3C, p_StateExpectedIDs_3D, p_StateExpectedIDs_3E, p_StateExpectedIDs_3F, p_StateExpectedIDs_40, p_StateExpectedIDs_41, p_StateExpectedIDs_42, p_StateExpectedIDs_43, p_StateExpectedIDs_44, p_StateExpectedIDs_45, p_StateExpectedIDs_46, p_StateExpectedIDs_47, p_StateExpectedIDs_48, p_StateExpectedIDs_49, p_StateExpectedIDs_4A, p_StateExpectedIDs_4B, p_StateExpectedIDs_4C, p_StateExpectedIDs_4D, p_StateExpectedIDs_4E, p_StateExpectedIDs_4F, p_StateExpectedIDs_50, p_StateExpectedIDs_51, p_StateExpectedIDs_52, p_StateExpectedIDs_53, p_StateExpectedIDs_54, p_StateExpectedIDs_55, p_StateExpectedIDs_56, p_StateExpectedIDs_57, p_StateExpectedIDs_58, p_StateExpectedIDs_59, p_StateExpectedIDs_5A, p_StateExpectedIDs_5B, p_StateExpectedIDs_5C, p_StateExpectedIDs_5D, p_StateExpectedIDs_5E, p_StateExpectedIDs_5F, p_StateExpectedIDs_60, p_StateExpectedIDs_61, p_StateExpectedIDs_62, p_StateExpectedIDs_63, p_StateExpectedIDs_64, p_StateExpectedIDs_65, p_StateExpectedIDs_66, p_StateExpectedIDs_67, p_StateExpectedIDs_68, p_StateExpectedIDs_69, p_StateExpectedIDs_6A, p_StateExpectedIDs_6B, p_StateExpectedIDs_6C, p_StateExpectedIDs_6D, p_StateExpectedIDs_6E, p_StateExpectedIDs_6F, p_StateExpectedIDs_70, p_StateExpectedIDs_71, p_StateExpectedIDs_72, p_StateExpectedIDs_73, p_StateExpectedIDs_74, p_StateExpectedIDs_75, p_StateExpectedIDs_76, p_StateExpectedIDs_77, p_StateExpectedIDs_78, p_StateExpectedIDs_79, p_StateExpectedIDs_7A, p_StateExpectedIDs_7B, p_StateExpectedIDs_7C, p_StateExpectedIDs_7D, p_StateExpectedIDs_7E, p_StateExpectedIDs_7F, p_StateExpectedIDs_80, p_StateExpectedIDs_81, p_StateExpectedIDs_82, p_StateExpectedIDs_83, p_StateExpectedIDs_84, p_StateExpectedIDs_85, p_StateExpectedIDs_86, p_StateExpectedIDs_87, p_StateExpectedIDs_88, p_StateExpectedIDs_89, p_StateExpectedIDs_8A, p_StateExpectedIDs_8B, p_StateExpectedIDs_8C, p_StateExpectedIDs_8D, p_StateExpectedIDs_8E, p_StateExpectedIDs_8F, p_StateExpectedIDs_90, p_StateExpectedIDs_91, p_StateExpectedIDs_92, p_StateExpectedIDs_93, p_StateExpectedIDs_94, p_StateExpectedIDs_95, p_StateExpectedIDs_96, p_StateExpectedIDs_97, p_StateExpectedIDs_98, p_StateExpectedIDs_99, p_StateExpectedIDs_9A, p_StateExpectedIDs_9B, p_StateExpectedIDs_9C, p_StateExpectedIDs_9D, p_StateExpectedIDs_9E, p_StateExpectedIDs_9F, p_StateExpectedIDs_A0, p_StateExpectedIDs_A1, p_StateExpectedIDs_A2, p_StateExpectedIDs_A3, p_StateExpectedIDs_A4, p_StateExpectedIDs_A5, p_StateExpectedIDs_A6, p_StateExpectedIDs_A7, p_StateExpectedIDs_A8, p_StateExpectedIDs_A9, p_StateExpectedIDs_AA, p_StateExpectedIDs_AB, p_StateExpectedIDs_AC, p_StateExpectedIDs_AD, p_StateExpectedIDs_AE, p_StateExpectedIDs_AF, p_StateExpectedIDs_B0, p_StateExpectedIDs_B1, p_StateExpectedIDs_B2, p_StateExpectedIDs_B3, p_StateExpectedIDs_B4, p_StateExpectedIDs_B5, p_StateExpectedIDs_B6, p_StateExpectedIDs_B7, p_StateExpectedIDs_B8, p_StateExpectedIDs_B9, p_StateExpectedIDs_BA, p_StateExpectedIDs_BB, p_StateExpectedIDs_BC, p_StateExpectedIDs_BD, p_StateExpectedIDs_BE, p_StateExpectedIDs_BF, p_StateExpectedIDs_C0, p_StateExpectedIDs_C1, p_StateExpectedIDs_C2, p_StateExpectedIDs_C3, p_StateExpectedIDs_C4, p_StateExpectedIDs_C5, p_StateExpectedIDs_C6, p_StateExpectedIDs_C7, p_StateExpectedIDs_C8, p_StateExpectedIDs_C9, p_StateExpectedIDs_CA, p_StateExpectedIDs_CB, p_StateExpectedIDs_CC, p_StateExpectedIDs_CD, p_StateExpectedIDs_CE, p_StateExpectedIDs_CF, p_StateExpectedIDs_D0, p_StateExpectedIDs_D1, p_StateExpectedIDs_D2, p_StateExpectedIDs_D3, p_StateExpectedIDs_D4, p_StateExpectedIDs_D5, p_StateExpectedIDs_D6, p_StateExpectedIDs_D7, p_StateExpectedIDs_D8, p_StateExpectedIDs_D9, p_StateExpectedIDs_DA, p_StateExpectedIDs_DB, p_StateExpectedIDs_DC, p_StateExpectedIDs_DD, p_StateExpectedIDs_DE, p_StateExpectedIDs_DF, p_StateExpectedIDs_E0, p_StateExpectedIDs_E1, p_StateExpectedIDs_E2, p_StateExpectedIDs_E3, p_StateExpectedIDs_E4, p_StateExpectedIDs_E5, p_StateExpectedIDs_E6, p_StateExpectedIDs_E7, p_StateExpectedIDs_E8, p_StateExpectedIDs_E9, p_StateExpectedIDs_EA, p_StateExpectedIDs_EB, p_StateExpectedIDs_EC, p_StateExpectedIDs_ED, p_StateExpectedIDs_EE, p_StateExpectedIDs_EF, p_StateExpectedIDs_F0, p_StateExpectedIDs_F1, p_StateExpectedIDs_F2, p_StateExpectedIDs_F3, p_StateExpectedIDs_F4, p_StateExpectedIDs_F5, p_StateExpectedIDs_F6, p_StateExpectedIDs_F7, p_StateExpectedIDs_F8, p_StateExpectedIDs_F9, p_StateExpectedIDs_FA, p_StateExpectedIDs_FB, p_StateExpectedIDs_FC, p_StateExpectedIDs_FD, p_StateExpectedIDs_FE, p_StateExpectedIDs_FF, p_StateExpectedIDs_100, p_StateExpectedIDs_101, p_StateExpectedIDs_102, p_StateExpectedIDs_103, p_StateExpectedIDs_104, p_StateExpectedIDs_105, p_StateExpectedIDs_106, p_StateExpectedIDs_107, p_StateExpectedIDs_108, p_StateExpectedIDs_109, p_StateExpectedIDs_10A, p_StateExpectedIDs_10B, p_StateExpectedIDs_10C, p_StateExpectedIDs_10D, p_StateExpectedIDs_10E, p_StateExpectedIDs_10F, p_StateExpectedIDs_110, p_StateExpectedIDs_111, p_StateExpectedIDs_112, p_StateExpectedIDs_113, p_StateExpectedIDs_114, p_StateExpectedIDs_115, p_StateExpectedIDs_116, p_StateExpectedIDs_117, p_StateExpectedIDs_118, p_StateExpectedIDs_119, p_StateExpectedIDs_11A, p_StateExpectedIDs_11B, p_StateExpectedIDs_11C, p_StateExpectedIDs_11D, p_StateExpectedIDs_11E, p_StateExpectedIDs_11F, p_StateExpectedIDs_120, p_StateExpectedIDs_121, p_StateExpectedIDs_122, p_StateExpectedIDs_123, p_StateExpectedIDs_124, p_StateExpectedIDs_125, p_StateExpectedIDs_126, p_StateExpectedIDs_127, p_StateExpectedIDs_128, p_StateExpectedIDs_129, p_StateExpectedIDs_12A };
        private static string[][] p_StateExpectedNames = { p_StateExpectedNames_0, p_StateExpectedNames_1, p_StateExpectedNames_2, p_StateExpectedNames_3, p_StateExpectedNames_4, p_StateExpectedNames_5, p_StateExpectedNames_6, p_StateExpectedNames_7, p_StateExpectedNames_8, p_StateExpectedNames_9, p_StateExpectedNames_A, p_StateExpectedNames_B, p_StateExpectedNames_C, p_StateExpectedNames_D, p_StateExpectedNames_E, p_StateExpectedNames_F, p_StateExpectedNames_10, p_StateExpectedNames_11, p_StateExpectedNames_12, p_StateExpectedNames_13, p_StateExpectedNames_14, p_StateExpectedNames_15, p_StateExpectedNames_16, p_StateExpectedNames_17, p_StateExpectedNames_18, p_StateExpectedNames_19, p_StateExpectedNames_1A, p_StateExpectedNames_1B, p_StateExpectedNames_1C, p_StateExpectedNames_1D, p_StateExpectedNames_1E, p_StateExpectedNames_1F, p_StateExpectedNames_20, p_StateExpectedNames_21, p_StateExpectedNames_22, p_StateExpectedNames_23, p_StateExpectedNames_24, p_StateExpectedNames_25, p_StateExpectedNames_26, p_StateExpectedNames_27, p_StateExpectedNames_28, p_StateExpectedNames_29, p_StateExpectedNames_2A, p_StateExpectedNames_2B, p_StateExpectedNames_2C, p_StateExpectedNames_2D, p_StateExpectedNames_2E, p_StateExpectedNames_2F, p_StateExpectedNames_30, p_StateExpectedNames_31, p_StateExpectedNames_32, p_StateExpectedNames_33, p_StateExpectedNames_34, p_StateExpectedNames_35, p_StateExpectedNames_36, p_StateExpectedNames_37, p_StateExpectedNames_38, p_StateExpectedNames_39, p_StateExpectedNames_3A, p_StateExpectedNames_3B, p_StateExpectedNames_3C, p_StateExpectedNames_3D, p_StateExpectedNames_3E, p_StateExpectedNames_3F, p_StateExpectedNames_40, p_StateExpectedNames_41, p_StateExpectedNames_42, p_StateExpectedNames_43, p_StateExpectedNames_44, p_StateExpectedNames_45, p_StateExpectedNames_46, p_StateExpectedNames_47, p_StateExpectedNames_48, p_StateExpectedNames_49, p_StateExpectedNames_4A, p_StateExpectedNames_4B, p_StateExpectedNames_4C, p_StateExpectedNames_4D, p_StateExpectedNames_4E, p_StateExpectedNames_4F, p_StateExpectedNames_50, p_StateExpectedNames_51, p_StateExpectedNames_52, p_StateExpectedNames_53, p_StateExpectedNames_54, p_StateExpectedNames_55, p_StateExpectedNames_56, p_StateExpectedNames_57, p_StateExpectedNames_58, p_StateExpectedNames_59, p_StateExpectedNames_5A, p_StateExpectedNames_5B, p_StateExpectedNames_5C, p_StateExpectedNames_5D, p_StateExpectedNames_5E, p_StateExpectedNames_5F, p_StateExpectedNames_60, p_StateExpectedNames_61, p_StateExpectedNames_62, p_StateExpectedNames_63, p_StateExpectedNames_64, p_StateExpectedNames_65, p_StateExpectedNames_66, p_StateExpectedNames_67, p_StateExpectedNames_68, p_StateExpectedNames_69, p_StateExpectedNames_6A, p_StateExpectedNames_6B, p_StateExpectedNames_6C, p_StateExpectedNames_6D, p_StateExpectedNames_6E, p_StateExpectedNames_6F, p_StateExpectedNames_70, p_StateExpectedNames_71, p_StateExpectedNames_72, p_StateExpectedNames_73, p_StateExpectedNames_74, p_StateExpectedNames_75, p_StateExpectedNames_76, p_StateExpectedNames_77, p_StateExpectedNames_78, p_StateExpectedNames_79, p_StateExpectedNames_7A, p_StateExpectedNames_7B, p_StateExpectedNames_7C, p_StateExpectedNames_7D, p_StateExpectedNames_7E, p_StateExpectedNames_7F, p_StateExpectedNames_80, p_StateExpectedNames_81, p_StateExpectedNames_82, p_StateExpectedNames_83, p_StateExpectedNames_84, p_StateExpectedNames_85, p_StateExpectedNames_86, p_StateExpectedNames_87, p_StateExpectedNames_88, p_StateExpectedNames_89, p_StateExpectedNames_8A, p_StateExpectedNames_8B, p_StateExpectedNames_8C, p_StateExpectedNames_8D, p_StateExpectedNames_8E, p_StateExpectedNames_8F, p_StateExpectedNames_90, p_StateExpectedNames_91, p_StateExpectedNames_92, p_StateExpectedNames_93, p_StateExpectedNames_94, p_StateExpectedNames_95, p_StateExpectedNames_96, p_StateExpectedNames_97, p_StateExpectedNames_98, p_StateExpectedNames_99, p_StateExpectedNames_9A, p_StateExpectedNames_9B, p_StateExpectedNames_9C, p_StateExpectedNames_9D, p_StateExpectedNames_9E, p_StateExpectedNames_9F, p_StateExpectedNames_A0, p_StateExpectedNames_A1, p_StateExpectedNames_A2, p_StateExpectedNames_A3, p_StateExpectedNames_A4, p_StateExpectedNames_A5, p_StateExpectedNames_A6, p_StateExpectedNames_A7, p_StateExpectedNames_A8, p_StateExpectedNames_A9, p_StateExpectedNames_AA, p_StateExpectedNames_AB, p_StateExpectedNames_AC, p_StateExpectedNames_AD, p_StateExpectedNames_AE, p_StateExpectedNames_AF, p_StateExpectedNames_B0, p_StateExpectedNames_B1, p_StateExpectedNames_B2, p_StateExpectedNames_B3, p_StateExpectedNames_B4, p_StateExpectedNames_B5, p_StateExpectedNames_B6, p_StateExpectedNames_B7, p_StateExpectedNames_B8, p_StateExpectedNames_B9, p_StateExpectedNames_BA, p_StateExpectedNames_BB, p_StateExpectedNames_BC, p_StateExpectedNames_BD, p_StateExpectedNames_BE, p_StateExpectedNames_BF, p_StateExpectedNames_C0, p_StateExpectedNames_C1, p_StateExpectedNames_C2, p_StateExpectedNames_C3, p_StateExpectedNames_C4, p_StateExpectedNames_C5, p_StateExpectedNames_C6, p_StateExpectedNames_C7, p_StateExpectedNames_C8, p_StateExpectedNames_C9, p_StateExpectedNames_CA, p_StateExpectedNames_CB, p_StateExpectedNames_CC, p_StateExpectedNames_CD, p_StateExpectedNames_CE, p_StateExpectedNames_CF, p_StateExpectedNames_D0, p_StateExpectedNames_D1, p_StateExpectedNames_D2, p_StateExpectedNames_D3, p_StateExpectedNames_D4, p_StateExpectedNames_D5, p_StateExpectedNames_D6, p_StateExpectedNames_D7, p_StateExpectedNames_D8, p_StateExpectedNames_D9, p_StateExpectedNames_DA, p_StateExpectedNames_DB, p_StateExpectedNames_DC, p_StateExpectedNames_DD, p_StateExpectedNames_DE, p_StateExpectedNames_DF, p_StateExpectedNames_E0, p_StateExpectedNames_E1, p_StateExpectedNames_E2, p_StateExpectedNames_E3, p_StateExpectedNames_E4, p_StateExpectedNames_E5, p_StateExpectedNames_E6, p_StateExpectedNames_E7, p_StateExpectedNames_E8, p_StateExpectedNames_E9, p_StateExpectedNames_EA, p_StateExpectedNames_EB, p_StateExpectedNames_EC, p_StateExpectedNames_ED, p_StateExpectedNames_EE, p_StateExpectedNames_EF, p_StateExpectedNames_F0, p_StateExpectedNames_F1, p_StateExpectedNames_F2, p_StateExpectedNames_F3, p_StateExpectedNames_F4, p_StateExpectedNames_F5, p_StateExpectedNames_F6, p_StateExpectedNames_F7, p_StateExpectedNames_F8, p_StateExpectedNames_F9, p_StateExpectedNames_FA, p_StateExpectedNames_FB, p_StateExpectedNames_FC, p_StateExpectedNames_FD, p_StateExpectedNames_FE, p_StateExpectedNames_FF, p_StateExpectedNames_100, p_StateExpectedNames_101, p_StateExpectedNames_102, p_StateExpectedNames_103, p_StateExpectedNames_104, p_StateExpectedNames_105, p_StateExpectedNames_106, p_StateExpectedNames_107, p_StateExpectedNames_108, p_StateExpectedNames_109, p_StateExpectedNames_10A, p_StateExpectedNames_10B, p_StateExpectedNames_10C, p_StateExpectedNames_10D, p_StateExpectedNames_10E, p_StateExpectedNames_10F, p_StateExpectedNames_110, p_StateExpectedNames_111, p_StateExpectedNames_112, p_StateExpectedNames_113, p_StateExpectedNames_114, p_StateExpectedNames_115, p_StateExpectedNames_116, p_StateExpectedNames_117, p_StateExpectedNames_118, p_StateExpectedNames_119, p_StateExpectedNames_11A, p_StateExpectedNames_11B, p_StateExpectedNames_11C, p_StateExpectedNames_11D, p_StateExpectedNames_11E, p_StateExpectedNames_11F, p_StateExpectedNames_120, p_StateExpectedNames_121, p_StateExpectedNames_122, p_StateExpectedNames_123, p_StateExpectedNames_124, p_StateExpectedNames_125, p_StateExpectedNames_126, p_StateExpectedNames_127, p_StateExpectedNames_128, p_StateExpectedNames_129, p_StateExpectedNames_12A };
        private static string[][] p_StateItems = { p_StateItems_0, p_StateItems_1, p_StateItems_2, p_StateItems_3, p_StateItems_4, p_StateItems_5, p_StateItems_6, p_StateItems_7, p_StateItems_8, p_StateItems_9, p_StateItems_A, p_StateItems_B, p_StateItems_C, p_StateItems_D, p_StateItems_E, p_StateItems_F, p_StateItems_10, p_StateItems_11, p_StateItems_12, p_StateItems_13, p_StateItems_14, p_StateItems_15, p_StateItems_16, p_StateItems_17, p_StateItems_18, p_StateItems_19, p_StateItems_1A, p_StateItems_1B, p_StateItems_1C, p_StateItems_1D, p_StateItems_1E, p_StateItems_1F, p_StateItems_20, p_StateItems_21, p_StateItems_22, p_StateItems_23, p_StateItems_24, p_StateItems_25, p_StateItems_26, p_StateItems_27, p_StateItems_28, p_StateItems_29, p_StateItems_2A, p_StateItems_2B, p_StateItems_2C, p_StateItems_2D, p_StateItems_2E, p_StateItems_2F, p_StateItems_30, p_StateItems_31, p_StateItems_32, p_StateItems_33, p_StateItems_34, p_StateItems_35, p_StateItems_36, p_StateItems_37, p_StateItems_38, p_StateItems_39, p_StateItems_3A, p_StateItems_3B, p_StateItems_3C, p_StateItems_3D, p_StateItems_3E, p_StateItems_3F, p_StateItems_40, p_StateItems_41, p_StateItems_42, p_StateItems_43, p_StateItems_44, p_StateItems_45, p_StateItems_46, p_StateItems_47, p_StateItems_48, p_StateItems_49, p_StateItems_4A, p_StateItems_4B, p_StateItems_4C, p_StateItems_4D, p_StateItems_4E, p_StateItems_4F, p_StateItems_50, p_StateItems_51, p_StateItems_52, p_StateItems_53, p_StateItems_54, p_StateItems_55, p_StateItems_56, p_StateItems_57, p_StateItems_58, p_StateItems_59, p_StateItems_5A, p_StateItems_5B, p_StateItems_5C, p_StateItems_5D, p_StateItems_5E, p_StateItems_5F, p_StateItems_60, p_StateItems_61, p_StateItems_62, p_StateItems_63, p_StateItems_64, p_StateItems_65, p_StateItems_66, p_StateItems_67, p_StateItems_68, p_StateItems_69, p_StateItems_6A, p_StateItems_6B, p_StateItems_6C, p_StateItems_6D, p_StateItems_6E, p_StateItems_6F, p_StateItems_70, p_StateItems_71, p_StateItems_72, p_StateItems_73, p_StateItems_74, p_StateItems_75, p_StateItems_76, p_StateItems_77, p_StateItems_78, p_StateItems_79, p_StateItems_7A, p_StateItems_7B, p_StateItems_7C, p_StateItems_7D, p_StateItems_7E, p_StateItems_7F, p_StateItems_80, p_StateItems_81, p_StateItems_82, p_StateItems_83, p_StateItems_84, p_StateItems_85, p_StateItems_86, p_StateItems_87, p_StateItems_88, p_StateItems_89, p_StateItems_8A, p_StateItems_8B, p_StateItems_8C, p_StateItems_8D, p_StateItems_8E, p_StateItems_8F, p_StateItems_90, p_StateItems_91, p_StateItems_92, p_StateItems_93, p_StateItems_94, p_StateItems_95, p_StateItems_96, p_StateItems_97, p_StateItems_98, p_StateItems_99, p_StateItems_9A, p_StateItems_9B, p_StateItems_9C, p_StateItems_9D, p_StateItems_9E, p_StateItems_9F, p_StateItems_A0, p_StateItems_A1, p_StateItems_A2, p_StateItems_A3, p_StateItems_A4, p_StateItems_A5, p_StateItems_A6, p_StateItems_A7, p_StateItems_A8, p_StateItems_A9, p_StateItems_AA, p_StateItems_AB, p_StateItems_AC, p_StateItems_AD, p_StateItems_AE, p_StateItems_AF, p_StateItems_B0, p_StateItems_B1, p_StateItems_B2, p_StateItems_B3, p_StateItems_B4, p_StateItems_B5, p_StateItems_B6, p_StateItems_B7, p_StateItems_B8, p_StateItems_B9, p_StateItems_BA, p_StateItems_BB, p_StateItems_BC, p_StateItems_BD, p_StateItems_BE, p_StateItems_BF, p_StateItems_C0, p_StateItems_C1, p_StateItems_C2, p_StateItems_C3, p_StateItems_C4, p_StateItems_C5, p_StateItems_C6, p_StateItems_C7, p_StateItems_C8, p_StateItems_C9, p_StateItems_CA, p_StateItems_CB, p_StateItems_CC, p_StateItems_CD, p_StateItems_CE, p_StateItems_CF, p_StateItems_D0, p_StateItems_D1, p_StateItems_D2, p_StateItems_D3, p_StateItems_D4, p_StateItems_D5, p_StateItems_D6, p_StateItems_D7, p_StateItems_D8, p_StateItems_D9, p_StateItems_DA, p_StateItems_DB, p_StateItems_DC, p_StateItems_DD, p_StateItems_DE, p_StateItems_DF, p_StateItems_E0, p_StateItems_E1, p_StateItems_E2, p_StateItems_E3, p_StateItems_E4, p_StateItems_E5, p_StateItems_E6, p_StateItems_E7, p_StateItems_E8, p_StateItems_E9, p_StateItems_EA, p_StateItems_EB, p_StateItems_EC, p_StateItems_ED, p_StateItems_EE, p_StateItems_EF, p_StateItems_F0, p_StateItems_F1, p_StateItems_F2, p_StateItems_F3, p_StateItems_F4, p_StateItems_F5, p_StateItems_F6, p_StateItems_F7, p_StateItems_F8, p_StateItems_F9, p_StateItems_FA, p_StateItems_FB, p_StateItems_FC, p_StateItems_FD, p_StateItems_FE, p_StateItems_FF, p_StateItems_100, p_StateItems_101, p_StateItems_102, p_StateItems_103, p_StateItems_104, p_StateItems_105, p_StateItems_106, p_StateItems_107, p_StateItems_108, p_StateItems_109, p_StateItems_10A, p_StateItems_10B, p_StateItems_10C, p_StateItems_10D, p_StateItems_10E, p_StateItems_10F, p_StateItems_110, p_StateItems_111, p_StateItems_112, p_StateItems_113, p_StateItems_114, p_StateItems_115, p_StateItems_116, p_StateItems_117, p_StateItems_118, p_StateItems_119, p_StateItems_11A, p_StateItems_11B, p_StateItems_11C, p_StateItems_11D, p_StateItems_11E, p_StateItems_11F, p_StateItems_120, p_StateItems_121, p_StateItems_122, p_StateItems_123, p_StateItems_124, p_StateItems_125, p_StateItems_126, p_StateItems_127, p_StateItems_128, p_StateItems_129, p_StateItems_12A };
        private static ushort[][][] p_StateShiftsOnTerminal = { p_StateShiftsOnTerminal_0, p_StateShiftsOnTerminal_1, p_StateShiftsOnTerminal_2, p_StateShiftsOnTerminal_3, p_StateShiftsOnTerminal_4, p_StateShiftsOnTerminal_5, p_StateShiftsOnTerminal_6, p_StateShiftsOnTerminal_7, p_StateShiftsOnTerminal_8, p_StateShiftsOnTerminal_9, p_StateShiftsOnTerminal_A, p_StateShiftsOnTerminal_B, p_StateShiftsOnTerminal_C, p_StateShiftsOnTerminal_D, p_StateShiftsOnTerminal_E, p_StateShiftsOnTerminal_F, p_StateShiftsOnTerminal_10, p_StateShiftsOnTerminal_11, p_StateShiftsOnTerminal_12, p_StateShiftsOnTerminal_13, p_StateShiftsOnTerminal_14, p_StateShiftsOnTerminal_15, p_StateShiftsOnTerminal_16, p_StateShiftsOnTerminal_17, p_StateShiftsOnTerminal_18, p_StateShiftsOnTerminal_19, p_StateShiftsOnTerminal_1A, p_StateShiftsOnTerminal_1B, p_StateShiftsOnTerminal_1C, p_StateShiftsOnTerminal_1D, p_StateShiftsOnTerminal_1E, p_StateShiftsOnTerminal_1F, p_StateShiftsOnTerminal_20, p_StateShiftsOnTerminal_21, p_StateShiftsOnTerminal_22, p_StateShiftsOnTerminal_23, p_StateShiftsOnTerminal_24, p_StateShiftsOnTerminal_25, p_StateShiftsOnTerminal_26, p_StateShiftsOnTerminal_27, p_StateShiftsOnTerminal_28, p_StateShiftsOnTerminal_29, p_StateShiftsOnTerminal_2A, p_StateShiftsOnTerminal_2B, p_StateShiftsOnTerminal_2C, p_StateShiftsOnTerminal_2D, p_StateShiftsOnTerminal_2E, p_StateShiftsOnTerminal_2F, p_StateShiftsOnTerminal_30, p_StateShiftsOnTerminal_31, p_StateShiftsOnTerminal_32, p_StateShiftsOnTerminal_33, p_StateShiftsOnTerminal_34, p_StateShiftsOnTerminal_35, p_StateShiftsOnTerminal_36, p_StateShiftsOnTerminal_37, p_StateShiftsOnTerminal_38, p_StateShiftsOnTerminal_39, p_StateShiftsOnTerminal_3A, p_StateShiftsOnTerminal_3B, p_StateShiftsOnTerminal_3C, p_StateShiftsOnTerminal_3D, p_StateShiftsOnTerminal_3E, p_StateShiftsOnTerminal_3F, p_StateShiftsOnTerminal_40, p_StateShiftsOnTerminal_41, p_StateShiftsOnTerminal_42, p_StateShiftsOnTerminal_43, p_StateShiftsOnTerminal_44, p_StateShiftsOnTerminal_45, p_StateShiftsOnTerminal_46, p_StateShiftsOnTerminal_47, p_StateShiftsOnTerminal_48, p_StateShiftsOnTerminal_49, p_StateShiftsOnTerminal_4A, p_StateShiftsOnTerminal_4B, p_StateShiftsOnTerminal_4C, p_StateShiftsOnTerminal_4D, p_StateShiftsOnTerminal_4E, p_StateShiftsOnTerminal_4F, p_StateShiftsOnTerminal_50, p_StateShiftsOnTerminal_51, p_StateShiftsOnTerminal_52, p_StateShiftsOnTerminal_53, p_StateShiftsOnTerminal_54, p_StateShiftsOnTerminal_55, p_StateShiftsOnTerminal_56, p_StateShiftsOnTerminal_57, p_StateShiftsOnTerminal_58, p_StateShiftsOnTerminal_59, p_StateShiftsOnTerminal_5A, p_StateShiftsOnTerminal_5B, p_StateShiftsOnTerminal_5C, p_StateShiftsOnTerminal_5D, p_StateShiftsOnTerminal_5E, p_StateShiftsOnTerminal_5F, p_StateShiftsOnTerminal_60, p_StateShiftsOnTerminal_61, p_StateShiftsOnTerminal_62, p_StateShiftsOnTerminal_63, p_StateShiftsOnTerminal_64, p_StateShiftsOnTerminal_65, p_StateShiftsOnTerminal_66, p_StateShiftsOnTerminal_67, p_StateShiftsOnTerminal_68, p_StateShiftsOnTerminal_69, p_StateShiftsOnTerminal_6A, p_StateShiftsOnTerminal_6B, p_StateShiftsOnTerminal_6C, p_StateShiftsOnTerminal_6D, p_StateShiftsOnTerminal_6E, p_StateShiftsOnTerminal_6F, p_StateShiftsOnTerminal_70, p_StateShiftsOnTerminal_71, p_StateShiftsOnTerminal_72, p_StateShiftsOnTerminal_73, p_StateShiftsOnTerminal_74, p_StateShiftsOnTerminal_75, p_StateShiftsOnTerminal_76, p_StateShiftsOnTerminal_77, p_StateShiftsOnTerminal_78, p_StateShiftsOnTerminal_79, p_StateShiftsOnTerminal_7A, p_StateShiftsOnTerminal_7B, p_StateShiftsOnTerminal_7C, p_StateShiftsOnTerminal_7D, p_StateShiftsOnTerminal_7E, p_StateShiftsOnTerminal_7F, p_StateShiftsOnTerminal_80, p_StateShiftsOnTerminal_81, p_StateShiftsOnTerminal_82, p_StateShiftsOnTerminal_83, p_StateShiftsOnTerminal_84, p_StateShiftsOnTerminal_85, p_StateShiftsOnTerminal_86, p_StateShiftsOnTerminal_87, p_StateShiftsOnTerminal_88, p_StateShiftsOnTerminal_89, p_StateShiftsOnTerminal_8A, p_StateShiftsOnTerminal_8B, p_StateShiftsOnTerminal_8C, p_StateShiftsOnTerminal_8D, p_StateShiftsOnTerminal_8E, p_StateShiftsOnTerminal_8F, p_StateShiftsOnTerminal_90, p_StateShiftsOnTerminal_91, p_StateShiftsOnTerminal_92, p_StateShiftsOnTerminal_93, p_StateShiftsOnTerminal_94, p_StateShiftsOnTerminal_95, p_StateShiftsOnTerminal_96, p_StateShiftsOnTerminal_97, p_StateShiftsOnTerminal_98, p_StateShiftsOnTerminal_99, p_StateShiftsOnTerminal_9A, p_StateShiftsOnTerminal_9B, p_StateShiftsOnTerminal_9C, p_StateShiftsOnTerminal_9D, p_StateShiftsOnTerminal_9E, p_StateShiftsOnTerminal_9F, p_StateShiftsOnTerminal_A0, p_StateShiftsOnTerminal_A1, p_StateShiftsOnTerminal_A2, p_StateShiftsOnTerminal_A3, p_StateShiftsOnTerminal_A4, p_StateShiftsOnTerminal_A5, p_StateShiftsOnTerminal_A6, p_StateShiftsOnTerminal_A7, p_StateShiftsOnTerminal_A8, p_StateShiftsOnTerminal_A9, p_StateShiftsOnTerminal_AA, p_StateShiftsOnTerminal_AB, p_StateShiftsOnTerminal_AC, p_StateShiftsOnTerminal_AD, p_StateShiftsOnTerminal_AE, p_StateShiftsOnTerminal_AF, p_StateShiftsOnTerminal_B0, p_StateShiftsOnTerminal_B1, p_StateShiftsOnTerminal_B2, p_StateShiftsOnTerminal_B3, p_StateShiftsOnTerminal_B4, p_StateShiftsOnTerminal_B5, p_StateShiftsOnTerminal_B6, p_StateShiftsOnTerminal_B7, p_StateShiftsOnTerminal_B8, p_StateShiftsOnTerminal_B9, p_StateShiftsOnTerminal_BA, p_StateShiftsOnTerminal_BB, p_StateShiftsOnTerminal_BC, p_StateShiftsOnTerminal_BD, p_StateShiftsOnTerminal_BE, p_StateShiftsOnTerminal_BF, p_StateShiftsOnTerminal_C0, p_StateShiftsOnTerminal_C1, p_StateShiftsOnTerminal_C2, p_StateShiftsOnTerminal_C3, p_StateShiftsOnTerminal_C4, p_StateShiftsOnTerminal_C5, p_StateShiftsOnTerminal_C6, p_StateShiftsOnTerminal_C7, p_StateShiftsOnTerminal_C8, p_StateShiftsOnTerminal_C9, p_StateShiftsOnTerminal_CA, p_StateShiftsOnTerminal_CB, p_StateShiftsOnTerminal_CC, p_StateShiftsOnTerminal_CD, p_StateShiftsOnTerminal_CE, p_StateShiftsOnTerminal_CF, p_StateShiftsOnTerminal_D0, p_StateShiftsOnTerminal_D1, p_StateShiftsOnTerminal_D2, p_StateShiftsOnTerminal_D3, p_StateShiftsOnTerminal_D4, p_StateShiftsOnTerminal_D5, p_StateShiftsOnTerminal_D6, p_StateShiftsOnTerminal_D7, p_StateShiftsOnTerminal_D8, p_StateShiftsOnTerminal_D9, p_StateShiftsOnTerminal_DA, p_StateShiftsOnTerminal_DB, p_StateShiftsOnTerminal_DC, p_StateShiftsOnTerminal_DD, p_StateShiftsOnTerminal_DE, p_StateShiftsOnTerminal_DF, p_StateShiftsOnTerminal_E0, p_StateShiftsOnTerminal_E1, p_StateShiftsOnTerminal_E2, p_StateShiftsOnTerminal_E3, p_StateShiftsOnTerminal_E4, p_StateShiftsOnTerminal_E5, p_StateShiftsOnTerminal_E6, p_StateShiftsOnTerminal_E7, p_StateShiftsOnTerminal_E8, p_StateShiftsOnTerminal_E9, p_StateShiftsOnTerminal_EA, p_StateShiftsOnTerminal_EB, p_StateShiftsOnTerminal_EC, p_StateShiftsOnTerminal_ED, p_StateShiftsOnTerminal_EE, p_StateShiftsOnTerminal_EF, p_StateShiftsOnTerminal_F0, p_StateShiftsOnTerminal_F1, p_StateShiftsOnTerminal_F2, p_StateShiftsOnTerminal_F3, p_StateShiftsOnTerminal_F4, p_StateShiftsOnTerminal_F5, p_StateShiftsOnTerminal_F6, p_StateShiftsOnTerminal_F7, p_StateShiftsOnTerminal_F8, p_StateShiftsOnTerminal_F9, p_StateShiftsOnTerminal_FA, p_StateShiftsOnTerminal_FB, p_StateShiftsOnTerminal_FC, p_StateShiftsOnTerminal_FD, p_StateShiftsOnTerminal_FE, p_StateShiftsOnTerminal_FF, p_StateShiftsOnTerminal_100, p_StateShiftsOnTerminal_101, p_StateShiftsOnTerminal_102, p_StateShiftsOnTerminal_103, p_StateShiftsOnTerminal_104, p_StateShiftsOnTerminal_105, p_StateShiftsOnTerminal_106, p_StateShiftsOnTerminal_107, p_StateShiftsOnTerminal_108, p_StateShiftsOnTerminal_109, p_StateShiftsOnTerminal_10A, p_StateShiftsOnTerminal_10B, p_StateShiftsOnTerminal_10C, p_StateShiftsOnTerminal_10D, p_StateShiftsOnTerminal_10E, p_StateShiftsOnTerminal_10F, p_StateShiftsOnTerminal_110, p_StateShiftsOnTerminal_111, p_StateShiftsOnTerminal_112, p_StateShiftsOnTerminal_113, p_StateShiftsOnTerminal_114, p_StateShiftsOnTerminal_115, p_StateShiftsOnTerminal_116, p_StateShiftsOnTerminal_117, p_StateShiftsOnTerminal_118, p_StateShiftsOnTerminal_119, p_StateShiftsOnTerminal_11A, p_StateShiftsOnTerminal_11B, p_StateShiftsOnTerminal_11C, p_StateShiftsOnTerminal_11D, p_StateShiftsOnTerminal_11E, p_StateShiftsOnTerminal_11F, p_StateShiftsOnTerminal_120, p_StateShiftsOnTerminal_121, p_StateShiftsOnTerminal_122, p_StateShiftsOnTerminal_123, p_StateShiftsOnTerminal_124, p_StateShiftsOnTerminal_125, p_StateShiftsOnTerminal_126, p_StateShiftsOnTerminal_127, p_StateShiftsOnTerminal_128, p_StateShiftsOnTerminal_129, p_StateShiftsOnTerminal_12A };
        private static ushort[][][] p_StateShiftsOnVariable = { p_StateShiftsOnVariable_0, p_StateShiftsOnVariable_1, p_StateShiftsOnVariable_2, p_StateShiftsOnVariable_3, p_StateShiftsOnVariable_4, p_StateShiftsOnVariable_5, p_StateShiftsOnVariable_6, p_StateShiftsOnVariable_7, p_StateShiftsOnVariable_8, p_StateShiftsOnVariable_9, p_StateShiftsOnVariable_A, p_StateShiftsOnVariable_B, p_StateShiftsOnVariable_C, p_StateShiftsOnVariable_D, p_StateShiftsOnVariable_E, p_StateShiftsOnVariable_F, p_StateShiftsOnVariable_10, p_StateShiftsOnVariable_11, p_StateShiftsOnVariable_12, p_StateShiftsOnVariable_13, p_StateShiftsOnVariable_14, p_StateShiftsOnVariable_15, p_StateShiftsOnVariable_16, p_StateShiftsOnVariable_17, p_StateShiftsOnVariable_18, p_StateShiftsOnVariable_19, p_StateShiftsOnVariable_1A, p_StateShiftsOnVariable_1B, p_StateShiftsOnVariable_1C, p_StateShiftsOnVariable_1D, p_StateShiftsOnVariable_1E, p_StateShiftsOnVariable_1F, p_StateShiftsOnVariable_20, p_StateShiftsOnVariable_21, p_StateShiftsOnVariable_22, p_StateShiftsOnVariable_23, p_StateShiftsOnVariable_24, p_StateShiftsOnVariable_25, p_StateShiftsOnVariable_26, p_StateShiftsOnVariable_27, p_StateShiftsOnVariable_28, p_StateShiftsOnVariable_29, p_StateShiftsOnVariable_2A, p_StateShiftsOnVariable_2B, p_StateShiftsOnVariable_2C, p_StateShiftsOnVariable_2D, p_StateShiftsOnVariable_2E, p_StateShiftsOnVariable_2F, p_StateShiftsOnVariable_30, p_StateShiftsOnVariable_31, p_StateShiftsOnVariable_32, p_StateShiftsOnVariable_33, p_StateShiftsOnVariable_34, p_StateShiftsOnVariable_35, p_StateShiftsOnVariable_36, p_StateShiftsOnVariable_37, p_StateShiftsOnVariable_38, p_StateShiftsOnVariable_39, p_StateShiftsOnVariable_3A, p_StateShiftsOnVariable_3B, p_StateShiftsOnVariable_3C, p_StateShiftsOnVariable_3D, p_StateShiftsOnVariable_3E, p_StateShiftsOnVariable_3F, p_StateShiftsOnVariable_40, p_StateShiftsOnVariable_41, p_StateShiftsOnVariable_42, p_StateShiftsOnVariable_43, p_StateShiftsOnVariable_44, p_StateShiftsOnVariable_45, p_StateShiftsOnVariable_46, p_StateShiftsOnVariable_47, p_StateShiftsOnVariable_48, p_StateShiftsOnVariable_49, p_StateShiftsOnVariable_4A, p_StateShiftsOnVariable_4B, p_StateShiftsOnVariable_4C, p_StateShiftsOnVariable_4D, p_StateShiftsOnVariable_4E, p_StateShiftsOnVariable_4F, p_StateShiftsOnVariable_50, p_StateShiftsOnVariable_51, p_StateShiftsOnVariable_52, p_StateShiftsOnVariable_53, p_StateShiftsOnVariable_54, p_StateShiftsOnVariable_55, p_StateShiftsOnVariable_56, p_StateShiftsOnVariable_57, p_StateShiftsOnVariable_58, p_StateShiftsOnVariable_59, p_StateShiftsOnVariable_5A, p_StateShiftsOnVariable_5B, p_StateShiftsOnVariable_5C, p_StateShiftsOnVariable_5D, p_StateShiftsOnVariable_5E, p_StateShiftsOnVariable_5F, p_StateShiftsOnVariable_60, p_StateShiftsOnVariable_61, p_StateShiftsOnVariable_62, p_StateShiftsOnVariable_63, p_StateShiftsOnVariable_64, p_StateShiftsOnVariable_65, p_StateShiftsOnVariable_66, p_StateShiftsOnVariable_67, p_StateShiftsOnVariable_68, p_StateShiftsOnVariable_69, p_StateShiftsOnVariable_6A, p_StateShiftsOnVariable_6B, p_StateShiftsOnVariable_6C, p_StateShiftsOnVariable_6D, p_StateShiftsOnVariable_6E, p_StateShiftsOnVariable_6F, p_StateShiftsOnVariable_70, p_StateShiftsOnVariable_71, p_StateShiftsOnVariable_72, p_StateShiftsOnVariable_73, p_StateShiftsOnVariable_74, p_StateShiftsOnVariable_75, p_StateShiftsOnVariable_76, p_StateShiftsOnVariable_77, p_StateShiftsOnVariable_78, p_StateShiftsOnVariable_79, p_StateShiftsOnVariable_7A, p_StateShiftsOnVariable_7B, p_StateShiftsOnVariable_7C, p_StateShiftsOnVariable_7D, p_StateShiftsOnVariable_7E, p_StateShiftsOnVariable_7F, p_StateShiftsOnVariable_80, p_StateShiftsOnVariable_81, p_StateShiftsOnVariable_82, p_StateShiftsOnVariable_83, p_StateShiftsOnVariable_84, p_StateShiftsOnVariable_85, p_StateShiftsOnVariable_86, p_StateShiftsOnVariable_87, p_StateShiftsOnVariable_88, p_StateShiftsOnVariable_89, p_StateShiftsOnVariable_8A, p_StateShiftsOnVariable_8B, p_StateShiftsOnVariable_8C, p_StateShiftsOnVariable_8D, p_StateShiftsOnVariable_8E, p_StateShiftsOnVariable_8F, p_StateShiftsOnVariable_90, p_StateShiftsOnVariable_91, p_StateShiftsOnVariable_92, p_StateShiftsOnVariable_93, p_StateShiftsOnVariable_94, p_StateShiftsOnVariable_95, p_StateShiftsOnVariable_96, p_StateShiftsOnVariable_97, p_StateShiftsOnVariable_98, p_StateShiftsOnVariable_99, p_StateShiftsOnVariable_9A, p_StateShiftsOnVariable_9B, p_StateShiftsOnVariable_9C, p_StateShiftsOnVariable_9D, p_StateShiftsOnVariable_9E, p_StateShiftsOnVariable_9F, p_StateShiftsOnVariable_A0, p_StateShiftsOnVariable_A1, p_StateShiftsOnVariable_A2, p_StateShiftsOnVariable_A3, p_StateShiftsOnVariable_A4, p_StateShiftsOnVariable_A5, p_StateShiftsOnVariable_A6, p_StateShiftsOnVariable_A7, p_StateShiftsOnVariable_A8, p_StateShiftsOnVariable_A9, p_StateShiftsOnVariable_AA, p_StateShiftsOnVariable_AB, p_StateShiftsOnVariable_AC, p_StateShiftsOnVariable_AD, p_StateShiftsOnVariable_AE, p_StateShiftsOnVariable_AF, p_StateShiftsOnVariable_B0, p_StateShiftsOnVariable_B1, p_StateShiftsOnVariable_B2, p_StateShiftsOnVariable_B3, p_StateShiftsOnVariable_B4, p_StateShiftsOnVariable_B5, p_StateShiftsOnVariable_B6, p_StateShiftsOnVariable_B7, p_StateShiftsOnVariable_B8, p_StateShiftsOnVariable_B9, p_StateShiftsOnVariable_BA, p_StateShiftsOnVariable_BB, p_StateShiftsOnVariable_BC, p_StateShiftsOnVariable_BD, p_StateShiftsOnVariable_BE, p_StateShiftsOnVariable_BF, p_StateShiftsOnVariable_C0, p_StateShiftsOnVariable_C1, p_StateShiftsOnVariable_C2, p_StateShiftsOnVariable_C3, p_StateShiftsOnVariable_C4, p_StateShiftsOnVariable_C5, p_StateShiftsOnVariable_C6, p_StateShiftsOnVariable_C7, p_StateShiftsOnVariable_C8, p_StateShiftsOnVariable_C9, p_StateShiftsOnVariable_CA, p_StateShiftsOnVariable_CB, p_StateShiftsOnVariable_CC, p_StateShiftsOnVariable_CD, p_StateShiftsOnVariable_CE, p_StateShiftsOnVariable_CF, p_StateShiftsOnVariable_D0, p_StateShiftsOnVariable_D1, p_StateShiftsOnVariable_D2, p_StateShiftsOnVariable_D3, p_StateShiftsOnVariable_D4, p_StateShiftsOnVariable_D5, p_StateShiftsOnVariable_D6, p_StateShiftsOnVariable_D7, p_StateShiftsOnVariable_D8, p_StateShiftsOnVariable_D9, p_StateShiftsOnVariable_DA, p_StateShiftsOnVariable_DB, p_StateShiftsOnVariable_DC, p_StateShiftsOnVariable_DD, p_StateShiftsOnVariable_DE, p_StateShiftsOnVariable_DF, p_StateShiftsOnVariable_E0, p_StateShiftsOnVariable_E1, p_StateShiftsOnVariable_E2, p_StateShiftsOnVariable_E3, p_StateShiftsOnVariable_E4, p_StateShiftsOnVariable_E5, p_StateShiftsOnVariable_E6, p_StateShiftsOnVariable_E7, p_StateShiftsOnVariable_E8, p_StateShiftsOnVariable_E9, p_StateShiftsOnVariable_EA, p_StateShiftsOnVariable_EB, p_StateShiftsOnVariable_EC, p_StateShiftsOnVariable_ED, p_StateShiftsOnVariable_EE, p_StateShiftsOnVariable_EF, p_StateShiftsOnVariable_F0, p_StateShiftsOnVariable_F1, p_StateShiftsOnVariable_F2, p_StateShiftsOnVariable_F3, p_StateShiftsOnVariable_F4, p_StateShiftsOnVariable_F5, p_StateShiftsOnVariable_F6, p_StateShiftsOnVariable_F7, p_StateShiftsOnVariable_F8, p_StateShiftsOnVariable_F9, p_StateShiftsOnVariable_FA, p_StateShiftsOnVariable_FB, p_StateShiftsOnVariable_FC, p_StateShiftsOnVariable_FD, p_StateShiftsOnVariable_FE, p_StateShiftsOnVariable_FF, p_StateShiftsOnVariable_100, p_StateShiftsOnVariable_101, p_StateShiftsOnVariable_102, p_StateShiftsOnVariable_103, p_StateShiftsOnVariable_104, p_StateShiftsOnVariable_105, p_StateShiftsOnVariable_106, p_StateShiftsOnVariable_107, p_StateShiftsOnVariable_108, p_StateShiftsOnVariable_109, p_StateShiftsOnVariable_10A, p_StateShiftsOnVariable_10B, p_StateShiftsOnVariable_10C, p_StateShiftsOnVariable_10D, p_StateShiftsOnVariable_10E, p_StateShiftsOnVariable_10F, p_StateShiftsOnVariable_110, p_StateShiftsOnVariable_111, p_StateShiftsOnVariable_112, p_StateShiftsOnVariable_113, p_StateShiftsOnVariable_114, p_StateShiftsOnVariable_115, p_StateShiftsOnVariable_116, p_StateShiftsOnVariable_117, p_StateShiftsOnVariable_118, p_StateShiftsOnVariable_119, p_StateShiftsOnVariable_11A, p_StateShiftsOnVariable_11B, p_StateShiftsOnVariable_11C, p_StateShiftsOnVariable_11D, p_StateShiftsOnVariable_11E, p_StateShiftsOnVariable_11F, p_StateShiftsOnVariable_120, p_StateShiftsOnVariable_121, p_StateShiftsOnVariable_122, p_StateShiftsOnVariable_123, p_StateShiftsOnVariable_124, p_StateShiftsOnVariable_125, p_StateShiftsOnVariable_126, p_StateShiftsOnVariable_127, p_StateShiftsOnVariable_128, p_StateShiftsOnVariable_129, p_StateShiftsOnVariable_12A };
        private static ushort[][][] p_StateReducsOnTerminal = { p_StateReducsOnTerminal_0, p_StateReducsOnTerminal_1, p_StateReducsOnTerminal_2, p_StateReducsOnTerminal_3, p_StateReducsOnTerminal_4, p_StateReducsOnTerminal_5, p_StateReducsOnTerminal_6, p_StateReducsOnTerminal_7, p_StateReducsOnTerminal_8, p_StateReducsOnTerminal_9, p_StateReducsOnTerminal_A, p_StateReducsOnTerminal_B, p_StateReducsOnTerminal_C, p_StateReducsOnTerminal_D, p_StateReducsOnTerminal_E, p_StateReducsOnTerminal_F, p_StateReducsOnTerminal_10, p_StateReducsOnTerminal_11, p_StateReducsOnTerminal_12, p_StateReducsOnTerminal_13, p_StateReducsOnTerminal_14, p_StateReducsOnTerminal_15, p_StateReducsOnTerminal_16, p_StateReducsOnTerminal_17, p_StateReducsOnTerminal_18, p_StateReducsOnTerminal_19, p_StateReducsOnTerminal_1A, p_StateReducsOnTerminal_1B, p_StateReducsOnTerminal_1C, p_StateReducsOnTerminal_1D, p_StateReducsOnTerminal_1E, p_StateReducsOnTerminal_1F, p_StateReducsOnTerminal_20, p_StateReducsOnTerminal_21, p_StateReducsOnTerminal_22, p_StateReducsOnTerminal_23, p_StateReducsOnTerminal_24, p_StateReducsOnTerminal_25, p_StateReducsOnTerminal_26, p_StateReducsOnTerminal_27, p_StateReducsOnTerminal_28, p_StateReducsOnTerminal_29, p_StateReducsOnTerminal_2A, p_StateReducsOnTerminal_2B, p_StateReducsOnTerminal_2C, p_StateReducsOnTerminal_2D, p_StateReducsOnTerminal_2E, p_StateReducsOnTerminal_2F, p_StateReducsOnTerminal_30, p_StateReducsOnTerminal_31, p_StateReducsOnTerminal_32, p_StateReducsOnTerminal_33, p_StateReducsOnTerminal_34, p_StateReducsOnTerminal_35, p_StateReducsOnTerminal_36, p_StateReducsOnTerminal_37, p_StateReducsOnTerminal_38, p_StateReducsOnTerminal_39, p_StateReducsOnTerminal_3A, p_StateReducsOnTerminal_3B, p_StateReducsOnTerminal_3C, p_StateReducsOnTerminal_3D, p_StateReducsOnTerminal_3E, p_StateReducsOnTerminal_3F, p_StateReducsOnTerminal_40, p_StateReducsOnTerminal_41, p_StateReducsOnTerminal_42, p_StateReducsOnTerminal_43, p_StateReducsOnTerminal_44, p_StateReducsOnTerminal_45, p_StateReducsOnTerminal_46, p_StateReducsOnTerminal_47, p_StateReducsOnTerminal_48, p_StateReducsOnTerminal_49, p_StateReducsOnTerminal_4A, p_StateReducsOnTerminal_4B, p_StateReducsOnTerminal_4C, p_StateReducsOnTerminal_4D, p_StateReducsOnTerminal_4E, p_StateReducsOnTerminal_4F, p_StateReducsOnTerminal_50, p_StateReducsOnTerminal_51, p_StateReducsOnTerminal_52, p_StateReducsOnTerminal_53, p_StateReducsOnTerminal_54, p_StateReducsOnTerminal_55, p_StateReducsOnTerminal_56, p_StateReducsOnTerminal_57, p_StateReducsOnTerminal_58, p_StateReducsOnTerminal_59, p_StateReducsOnTerminal_5A, p_StateReducsOnTerminal_5B, p_StateReducsOnTerminal_5C, p_StateReducsOnTerminal_5D, p_StateReducsOnTerminal_5E, p_StateReducsOnTerminal_5F, p_StateReducsOnTerminal_60, p_StateReducsOnTerminal_61, p_StateReducsOnTerminal_62, p_StateReducsOnTerminal_63, p_StateReducsOnTerminal_64, p_StateReducsOnTerminal_65, p_StateReducsOnTerminal_66, p_StateReducsOnTerminal_67, p_StateReducsOnTerminal_68, p_StateReducsOnTerminal_69, p_StateReducsOnTerminal_6A, p_StateReducsOnTerminal_6B, p_StateReducsOnTerminal_6C, p_StateReducsOnTerminal_6D, p_StateReducsOnTerminal_6E, p_StateReducsOnTerminal_6F, p_StateReducsOnTerminal_70, p_StateReducsOnTerminal_71, p_StateReducsOnTerminal_72, p_StateReducsOnTerminal_73, p_StateReducsOnTerminal_74, p_StateReducsOnTerminal_75, p_StateReducsOnTerminal_76, p_StateReducsOnTerminal_77, p_StateReducsOnTerminal_78, p_StateReducsOnTerminal_79, p_StateReducsOnTerminal_7A, p_StateReducsOnTerminal_7B, p_StateReducsOnTerminal_7C, p_StateReducsOnTerminal_7D, p_StateReducsOnTerminal_7E, p_StateReducsOnTerminal_7F, p_StateReducsOnTerminal_80, p_StateReducsOnTerminal_81, p_StateReducsOnTerminal_82, p_StateReducsOnTerminal_83, p_StateReducsOnTerminal_84, p_StateReducsOnTerminal_85, p_StateReducsOnTerminal_86, p_StateReducsOnTerminal_87, p_StateReducsOnTerminal_88, p_StateReducsOnTerminal_89, p_StateReducsOnTerminal_8A, p_StateReducsOnTerminal_8B, p_StateReducsOnTerminal_8C, p_StateReducsOnTerminal_8D, p_StateReducsOnTerminal_8E, p_StateReducsOnTerminal_8F, p_StateReducsOnTerminal_90, p_StateReducsOnTerminal_91, p_StateReducsOnTerminal_92, p_StateReducsOnTerminal_93, p_StateReducsOnTerminal_94, p_StateReducsOnTerminal_95, p_StateReducsOnTerminal_96, p_StateReducsOnTerminal_97, p_StateReducsOnTerminal_98, p_StateReducsOnTerminal_99, p_StateReducsOnTerminal_9A, p_StateReducsOnTerminal_9B, p_StateReducsOnTerminal_9C, p_StateReducsOnTerminal_9D, p_StateReducsOnTerminal_9E, p_StateReducsOnTerminal_9F, p_StateReducsOnTerminal_A0, p_StateReducsOnTerminal_A1, p_StateReducsOnTerminal_A2, p_StateReducsOnTerminal_A3, p_StateReducsOnTerminal_A4, p_StateReducsOnTerminal_A5, p_StateReducsOnTerminal_A6, p_StateReducsOnTerminal_A7, p_StateReducsOnTerminal_A8, p_StateReducsOnTerminal_A9, p_StateReducsOnTerminal_AA, p_StateReducsOnTerminal_AB, p_StateReducsOnTerminal_AC, p_StateReducsOnTerminal_AD, p_StateReducsOnTerminal_AE, p_StateReducsOnTerminal_AF, p_StateReducsOnTerminal_B0, p_StateReducsOnTerminal_B1, p_StateReducsOnTerminal_B2, p_StateReducsOnTerminal_B3, p_StateReducsOnTerminal_B4, p_StateReducsOnTerminal_B5, p_StateReducsOnTerminal_B6, p_StateReducsOnTerminal_B7, p_StateReducsOnTerminal_B8, p_StateReducsOnTerminal_B9, p_StateReducsOnTerminal_BA, p_StateReducsOnTerminal_BB, p_StateReducsOnTerminal_BC, p_StateReducsOnTerminal_BD, p_StateReducsOnTerminal_BE, p_StateReducsOnTerminal_BF, p_StateReducsOnTerminal_C0, p_StateReducsOnTerminal_C1, p_StateReducsOnTerminal_C2, p_StateReducsOnTerminal_C3, p_StateReducsOnTerminal_C4, p_StateReducsOnTerminal_C5, p_StateReducsOnTerminal_C6, p_StateReducsOnTerminal_C7, p_StateReducsOnTerminal_C8, p_StateReducsOnTerminal_C9, p_StateReducsOnTerminal_CA, p_StateReducsOnTerminal_CB, p_StateReducsOnTerminal_CC, p_StateReducsOnTerminal_CD, p_StateReducsOnTerminal_CE, p_StateReducsOnTerminal_CF, p_StateReducsOnTerminal_D0, p_StateReducsOnTerminal_D1, p_StateReducsOnTerminal_D2, p_StateReducsOnTerminal_D3, p_StateReducsOnTerminal_D4, p_StateReducsOnTerminal_D5, p_StateReducsOnTerminal_D6, p_StateReducsOnTerminal_D7, p_StateReducsOnTerminal_D8, p_StateReducsOnTerminal_D9, p_StateReducsOnTerminal_DA, p_StateReducsOnTerminal_DB, p_StateReducsOnTerminal_DC, p_StateReducsOnTerminal_DD, p_StateReducsOnTerminal_DE, p_StateReducsOnTerminal_DF, p_StateReducsOnTerminal_E0, p_StateReducsOnTerminal_E1, p_StateReducsOnTerminal_E2, p_StateReducsOnTerminal_E3, p_StateReducsOnTerminal_E4, p_StateReducsOnTerminal_E5, p_StateReducsOnTerminal_E6, p_StateReducsOnTerminal_E7, p_StateReducsOnTerminal_E8, p_StateReducsOnTerminal_E9, p_StateReducsOnTerminal_EA, p_StateReducsOnTerminal_EB, p_StateReducsOnTerminal_EC, p_StateReducsOnTerminal_ED, p_StateReducsOnTerminal_EE, p_StateReducsOnTerminal_EF, p_StateReducsOnTerminal_F0, p_StateReducsOnTerminal_F1, p_StateReducsOnTerminal_F2, p_StateReducsOnTerminal_F3, p_StateReducsOnTerminal_F4, p_StateReducsOnTerminal_F5, p_StateReducsOnTerminal_F6, p_StateReducsOnTerminal_F7, p_StateReducsOnTerminal_F8, p_StateReducsOnTerminal_F9, p_StateReducsOnTerminal_FA, p_StateReducsOnTerminal_FB, p_StateReducsOnTerminal_FC, p_StateReducsOnTerminal_FD, p_StateReducsOnTerminal_FE, p_StateReducsOnTerminal_FF, p_StateReducsOnTerminal_100, p_StateReducsOnTerminal_101, p_StateReducsOnTerminal_102, p_StateReducsOnTerminal_103, p_StateReducsOnTerminal_104, p_StateReducsOnTerminal_105, p_StateReducsOnTerminal_106, p_StateReducsOnTerminal_107, p_StateReducsOnTerminal_108, p_StateReducsOnTerminal_109, p_StateReducsOnTerminal_10A, p_StateReducsOnTerminal_10B, p_StateReducsOnTerminal_10C, p_StateReducsOnTerminal_10D, p_StateReducsOnTerminal_10E, p_StateReducsOnTerminal_10F, p_StateReducsOnTerminal_110, p_StateReducsOnTerminal_111, p_StateReducsOnTerminal_112, p_StateReducsOnTerminal_113, p_StateReducsOnTerminal_114, p_StateReducsOnTerminal_115, p_StateReducsOnTerminal_116, p_StateReducsOnTerminal_117, p_StateReducsOnTerminal_118, p_StateReducsOnTerminal_119, p_StateReducsOnTerminal_11A, p_StateReducsOnTerminal_11B, p_StateReducsOnTerminal_11C, p_StateReducsOnTerminal_11D, p_StateReducsOnTerminal_11E, p_StateReducsOnTerminal_11F, p_StateReducsOnTerminal_120, p_StateReducsOnTerminal_121, p_StateReducsOnTerminal_122, p_StateReducsOnTerminal_123, p_StateReducsOnTerminal_124, p_StateReducsOnTerminal_125, p_StateReducsOnTerminal_126, p_StateReducsOnTerminal_127, p_StateReducsOnTerminal_128, p_StateReducsOnTerminal_129, p_StateReducsOnTerminal_12A };
        private static int p_ErrorSimulationLength = 3;

        private System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError> p_Errors;
        private Lexer_Hime_Kernel_FileCentralDogma p_Lexer;
        private Hime.Kernel.Parsers.SyntaxTreeNodeCollection p_Nodes;
        private System.Collections.Generic.Stack<ushort> p_Stack;
        private Hime.Kernel.Parsers.SymbolToken p_NextToken;
        private ushort p_CurrentState;

        public System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError> Errors { get { return p_Errors; } }

        private static ushort Analyse_GetNextByShiftOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnTerminal[state].Length; i++)
            {
                if (p_StateShiftsOnTerminal[state][i][0] == sid)
                    return p_StateShiftsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }
        private static ushort Analyse_GetNextByShiftOnVariable(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateShiftsOnVariable[state].Length; i++)
            {
                if (p_StateShiftsOnVariable[state][i][0] == sid)
                    return p_StateShiftsOnVariable[state][i][1];
            }
            return 0xFFFF;
        }
        private static ushort Analyse_GetProductionOnTerminal(ushort state, ushort sid)
        {
            for (int i = 0; i != p_StateReducsOnTerminal[state].Length; i++)
            {
                if (p_StateReducsOnTerminal[state][i][0] == sid)
                    return p_StateReducsOnTerminal[state][i][1];
            }
            return 0xFFFF;
        }


        public Parser_Hime_Kernel_FileCentralDogma(Lexer_Hime_Kernel_FileCentralDogma input)
        {
            p_Errors = new System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError>();
            p_Lexer = input;
            p_Nodes = new Hime.Kernel.Parsers.SyntaxTreeNodeCollection();
            p_Stack = new System.Collections.Generic.Stack<ushort>();
            p_CurrentState = 0x0;
            p_NextToken = null;
        }


        private void Analyse_HandleUnexpectedToken()
        {
            p_Errors.Add(new Hime.Kernel.Parsers.ParserErrorUnexpectedToken(p_NextToken, p_StateExpectedNames[p_CurrentState]));

            if (p_Errors.Count >= 100)
                throw new Hime.Kernel.Parsers.ParserException("Too much errors, parsing stopped.");

            if (Analyse_HandleUnexpectedToken_SimpleRecovery()) return;
            throw new Hime.Kernel.Parsers.ParserException("Unrecoverable error encountered");
        }
        private bool Analyse_HandleUnexpectedToken_SimpleRecovery()
        {
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()) return true;
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()) return true;
            if (Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()) return true;
            return false;
        }
        private bool Analyse_HandleUnexpectedToken_SimpleRecovery_RemoveUnexpected()
        {
            Lexer_Hime_Kernel_FileCentralDogma TestLexer = p_Lexer.Clone_Hime_Kernel_FileCentralDogma();
            System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
            TempStack.Reverse();
            System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
            if (Analyse_Simulate(TestStack, TestLexer))
            {
                p_NextToken = p_Lexer.GetNextToken();
                return true;
            }
            return false;
        }
        private bool Analyse_HandleUnexpectedToken_SimpleRecovery_InsertExpected()
        {
            for (int i = 0; i != p_StateExpectedIDs[p_CurrentState].Length; i++)
            {
                Lexer_Hime_Kernel_FileCentralDogma TestLexer = p_Lexer.Clone_Hime_Kernel_FileCentralDogma();
                System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
                TempStack.Reverse();
                System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
                System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken> Inserted = new System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken>();
                Inserted.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
                Inserted.Add(p_NextToken);
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    Analyse_RunForToken(Inserted[1]);
                    p_NextToken = p_Lexer.GetNextToken();
                    return true;
                }
            }
            return false;
        }
        private bool Analyse_HandleUnexpectedToken_SimpleRecovery_ReplaceUnexpectedByExpected()
        {
            for (int i = 0; i != p_StateExpectedIDs[p_CurrentState].Length; i++)
            {
                Lexer_Hime_Kernel_FileCentralDogma TestLexer = p_Lexer.Clone_Hime_Kernel_FileCentralDogma();
                System.Collections.Generic.List<ushort> TempStack = new System.Collections.Generic.List<ushort>(p_Stack);
                TempStack.Reverse();
                System.Collections.Generic.Stack<ushort> TestStack = new System.Collections.Generic.Stack<ushort>(TempStack);
                System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken> Inserted = new System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken>();
                Inserted.Add(new Hime.Kernel.Parsers.SymbolTokenText(p_StateExpectedNames[p_CurrentState][i], p_StateExpectedIDs[p_CurrentState][i], string.Empty, p_Lexer.CurrentLine));
                if (Analyse_Simulate(TestStack, TestLexer, Inserted))
                {
                    Analyse_RunForToken(Inserted[0]);
                    p_NextToken = p_Lexer.GetNextToken();
                    return true;
                }
            }
            return false;
        }

        private bool Analyse_Simulate(System.Collections.Generic.Stack<ushort> stack, Lexer_Hime_Kernel_FileCentralDogma lexer, System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken> inserted)
        {
            int InsertedIndex = 0;
            ushort CurrentState = stack.Peek();
            Hime.Kernel.Parsers.SymbolToken NextToken = null;
            if (inserted.Count != 0)
            {
                NextToken = inserted[0];
                InsertedIndex++;
            }
            else
                NextToken = lexer.GetNextToken();

            for (int i = 0; i != p_ErrorSimulationLength + inserted.Count; i++)
            {
                ushort NextState = Analyse_GetNextByShiftOnTerminal(CurrentState, NextToken.SymbolID);
                if (NextState != 0xFFFF)
                {
                    CurrentState = NextState;
                    stack.Push(CurrentState);
                    if (InsertedIndex != inserted.Count)
                    {
                        NextToken = inserted[InsertedIndex];
                        InsertedIndex++;
                    }
                    else
                        NextToken = lexer.GetNextToken();
                    continue;
                }
                ushort ReductionIndex = Analyse_GetProductionOnTerminal(CurrentState, NextToken.SymbolID);
                if (ReductionIndex != 0xFFFF)
                {
                    Production Reduce = p_Rules[ReductionIndex];
                    ushort HeadID = p_RulesHeadID[ReductionIndex];
                    for (ushort j = 0; j != p_RulesParserLength[ReductionIndex]; j++)
                        stack.Pop();
                    // If next symbol is ε (after $) : return
                    if (NextToken.SymbolID == 0x1)
                        return true;
                    // Shift to next state on the reduce variable
                    NextState = Analyse_GetNextByShiftOnVariable(stack.Peek(), HeadID);
                    // Handle error here : no transition for symbol HeadID
                    if (NextState == 0xFFFF)
                        return false;
                    CurrentState = NextState;
                    stack.Push(CurrentState);
                    continue;
                }
                // Handle error here : no action for symbol NextToken.SymbolID
                return false;
            }
            return true;
        }
        private bool Analyse_Simulate(System.Collections.Generic.Stack<ushort> stack, Lexer_Hime_Kernel_FileCentralDogma lexer)
        {
            return Analyse_Simulate(stack, lexer, new System.Collections.Generic.List<Hime.Kernel.Parsers.SymbolToken>());
        }

        private bool Analyse_RunForToken(Hime.Kernel.Parsers.SymbolToken token)
        {
            while (true)
            {
                ushort NextState = Analyse_GetNextByShiftOnTerminal(p_CurrentState, token.SymbolID);
                if (NextState != 0xFFFF)
                {
                    p_Nodes.Add(new Hime.Kernel.Parsers.SyntaxTreeNode(token));
                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    return true;
                }
                ushort ReductionIndex = Analyse_GetProductionOnTerminal(p_CurrentState, token.SymbolID);
                if (ReductionIndex != 0xFFFF)
                {
                    Production Reduce = p_Rules[ReductionIndex];
                    ushort HeadID = p_RulesHeadID[ReductionIndex];
                    Reduce(p_Nodes);
                    for (ushort j = 0; j != p_RulesParserLength[ReductionIndex]; j++)
                        p_Stack.Pop();
                    // Shift to next state on the reduce variable
                    NextState = Analyse_GetNextByShiftOnVariable(p_Stack.Peek(), HeadID);
                    if (NextState == 0xFFFF)
                        return false;
                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    continue;
                }
                return false;
            }
        }
        public Hime.Kernel.Parsers.SyntaxTreeNode Analyse()
        {
            p_Stack.Push(p_CurrentState);
            p_NextToken = p_Lexer.GetNextToken();

            while (true)
            {
                if (Analyse_RunForToken(p_NextToken))
                {
                    p_NextToken = p_Lexer.GetNextToken();
                    continue;
                }
                else if (p_NextToken.SymbolID == 0x0001)
                    return p_Nodes[0];
                else
                    Analyse_HandleUnexpectedToken();
            }
        }

    }


}
