
namespace Hime.Kernel.Resources.Parser
{


    public class Lexer_Hime_Kernel_FileCentralDogma : Hime.Kernel.Parsers.LexerText
    {
        private static ushort[] p_SymbolsSID = { 0x8, 0x9, 0xF, 0x10, 0xC6, 0x38, 0x39, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x41, 0x44, 0x45, 0x46, 0x49, 0x4D, 0x4E, 0xC7, 0x7, 0x4B, 0xC5, 0x3A, 0x29, 0x42, 0x43, 0x2A, 0x2B, 0x31, 0x37, 0x2C, 0x32, 0x4C, 0xA, 0x2D, 0x33, 0xB, 0x4A, 0x47, 0xD, 0xC, 0x48, 0xE, 0x2E, 0x34, 0x2F, 0x35, 0x30, 0x36 };
        private static string[] p_SymbolsName = { "NAME", "_T[.]", "_T[{]", "_T[}]", "_T[[]", "_T[=]", "_T[;]", "_T[(]", "_T[)]", "_T[*]", "_T[+]", "_T[?]", "_T[-]", "_T[|]", "_T[<]", "_T[,]", "_T[>]", "_T[:]", "_T[^]", "_T[!]", "_T[]]", "SEPARATOR", "_T[cf]", "_T[cs]", "_T[..]", "QUOTED_DATA", "_T[=>]", "_T[->]", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_BINARY", "SYMBOL_VALUE_UINT8", "SYMBOL_JOKER_UINT8", "_T[rules]", "_T[public]", "SYMBOL_VALUE_UINT16", "SYMBOL_JOKER_UINT16", "_T[private]", "_T[grammar]", "_T[options]", "_T[internal]", "_T[protected]", "_T[terminals]", "_T[namespace]", "SYMBOL_VALUE_UINT32", "SYMBOL_JOKER_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_JOKER_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_JOKER_UINT128" };

        private static ushort[][] p_Transitions0 = { new ushort[3] { 0x2F, 0x2F, 0x1 }, new ushort[3] { 0x70, 0x70, 0x54 }, new ushort[3] { 0x41, 0x5A, 0x55 }, new ushort[3] { 0x5F, 0x5F, 0x55 }, new ushort[3] { 0x61, 0x62, 0x55 }, new ushort[3] { 0x64, 0x66, 0x55 }, new ushort[3] { 0x68, 0x68, 0x55 }, new ushort[3] { 0x6A, 0x6D, 0x55 }, new ushort[3] { 0x71, 0x71, 0x55 }, new ushort[3] { 0x73, 0x73, 0x55 }, new ushort[3] { 0x75, 0x7A, 0x55 }, new ushort[3] { 0x69, 0x69, 0x56 }, new ushort[3] { 0x74, 0x74, 0x57 }, new ushort[3] { 0x67, 0x67, 0x58 }, new ushort[3] { 0x6E, 0x6E, 0x59 }, new ushort[3] { 0x72, 0x72, 0x5A }, new ushort[3] { 0x63, 0x63, 0x5B }, new ushort[3] { 0x6F, 0x6F, 0x5C }, new ushort[3] { 0x2E, 0x2E, 0x8E }, new ushort[3] { 0x7B, 0x7B, 0x8F }, new ushort[3] { 0x7D, 0x7D, 0x90 }, new ushort[3] { 0x22, 0x22, 0x3 }, new ushort[3] { 0x27, 0x27, 0x4 }, new ushort[3] { 0x5B, 0x5B, 0x91 }, new ushort[3] { 0x30, 0x30, 0x5 }, new ushort[3] { 0x3D, 0x3D, 0x92 }, new ushort[3] { 0x3B, 0x3B, 0x93 }, new ushort[3] { 0x28, 0x28, 0x94 }, new ushort[3] { 0x29, 0x29, 0x95 }, new ushort[3] { 0x2A, 0x2A, 0x96 }, new ushort[3] { 0x2B, 0x2B, 0x97 }, new ushort[3] { 0x3F, 0x3F, 0x98 }, new ushort[3] { 0x2D, 0x2D, 0x99 }, new ushort[3] { 0x7C, 0x7C, 0x9A }, new ushort[3] { 0x3C, 0x3C, 0x9B }, new ushort[3] { 0x2C, 0x2C, 0x9C }, new ushort[3] { 0x3E, 0x3E, 0x9D }, new ushort[3] { 0x3A, 0x3A, 0x9E }, new ushort[3] { 0x5E, 0x5E, 0x9F }, new ushort[3] { 0x21, 0x21, 0xA0 }, new ushort[3] { 0x5D, 0x5D, 0xA1 }, new ushort[3] { 0xA, 0xA, 0xA2 }, new ushort[3] { 0x2028, 0x2029, 0xA2 }, new ushort[3] { 0x9, 0x9, 0xA4 }, new ushort[3] { 0xB, 0xC, 0xA4 }, new ushort[3] { 0x20, 0x20, 0xA4 }, new ushort[3] { 0xD, 0xD, 0xA3 } };

        private static ushort[][] p_Transitions1 = { new ushort[3] { 0x2F, 0x2F, 0x6 }, new ushort[3] { 0x2A, 0x2A, 0x7 } };

        private static ushort[][] p_Transitions2 = { new ushort[3] { 0x2A, 0x2A, 0x8 }, new ushort[3] { 0x2F, 0x2F, 0x12 } };

        private static ushort[][] p_Transitions3 = { new ushort[3] { 0x0, 0x21, 0x3 }, new ushort[3] { 0x23, 0xFFFF, 0x3 }, new ushort[3] { 0x22, 0x22, 0xA8 } };

        private static ushort[][] p_Transitions4 = { new ushort[3] { 0x5C, 0x5C, 0x9 }, new ushort[3] { 0x0, 0x26, 0xA }, new ushort[3] { 0x28, 0x5B, 0xA }, new ushort[3] { 0x5D, 0xFFFF, 0xA } };

        private static ushort[][] p_Transitions5 = { new ushort[3] { 0x78, 0x78, 0xD }, new ushort[3] { 0x62, 0x62, 0xE } };

        private static ushort[][] p_Transitions6 = { new ushort[3] { 0xA, 0xA, 0xB1 }, new ushort[3] { 0x2028, 0x2029, 0xB1 }, new ushort[3] { 0x0, 0x9, 0x6 }, new ushort[3] { 0xB, 0xC, 0x6 }, new ushort[3] { 0xE, 0x2027, 0x6 }, new ushort[3] { 0x202A, 0xFFFF, 0x6 }, new ushort[3] { 0xD, 0xD, 0xB2 } };

        private static ushort[][] p_Transitions7 = { new ushort[3] { 0x0, 0x29, 0x7 }, new ushort[3] { 0x2B, 0xFFFF, 0x7 }, new ushort[3] { 0x2A, 0x2A, 0xF } };

        private static ushort[][] p_Transitions8 = { new ushort[3] { 0x0, 0x29, 0x8 }, new ushort[3] { 0x2B, 0xFFFF, 0x8 }, new ushort[3] { 0x2A, 0x2A, 0x15 } };

        private static ushort[][] p_Transitions9 = { new ushort[3] { 0x27, 0x27, 0xA }, new ushort[3] { 0x5C, 0x5C, 0xA } };

        private static ushort[][] p_Transitions10 = { new ushort[3] { 0x27, 0x27, 0xB3 }, new ushort[3] { 0x5C, 0x5C, 0x9 }, new ushort[3] { 0x0, 0x26, 0xA }, new ushort[3] { 0x28, 0x5B, 0xA }, new ushort[3] { 0x5D, 0xFFFF, 0xA } };

        private static ushort[][] p_Transitions11 = { new ushort[3] { 0x5B, 0x5D, 0xC } };

        private static ushort[][] p_Transitions12 = { new ushort[3] { 0x5D, 0x5D, 0xB4 }, new ushort[3] { 0x5C, 0x5C, 0xB }, new ushort[3] { 0x0, 0x5A, 0xC }, new ushort[3] { 0x5E, 0xFFFF, 0xC } };

        private static ushort[][] p_Transitions13 = { new ushort[3] { 0x30, 0x39, 0x10 }, new ushort[3] { 0x41, 0x46, 0x10 }, new ushort[3] { 0x61, 0x66, 0x10 }, new ushort[3] { 0x58, 0x58, 0x11 } };

        private static ushort[][] p_Transitions14 = { new ushort[3] { 0x30, 0x31, 0xB5 }, new ushort[3] { 0x42, 0x42, 0xB6 } };

        private static ushort[][] p_Transitions15 = { new ushort[3] { 0x2F, 0x2F, 0xB7 }, new ushort[3] { 0x0, 0x2E, 0x7 }, new ushort[3] { 0x30, 0xFFFF, 0x7 } };

        private static ushort[][] p_Transitions16 = { new ushort[3] { 0x30, 0x39, 0xB8 }, new ushort[3] { 0x41, 0x46, 0xB8 }, new ushort[3] { 0x61, 0x66, 0xB8 } };

        private static ushort[][] p_Transitions17 = { new ushort[3] { 0x58, 0x58, 0xB9 } };

        private static ushort[][] p_Transitions18 = { new ushort[3] { 0xA, 0xA, 0xAD }, new ushort[3] { 0xD, 0xD, 0xAD }, new ushort[3] { 0x2028, 0x2029, 0xAD }, new ushort[3] { 0x0, 0x9, 0x12 }, new ushort[3] { 0xB, 0xC, 0x12 }, new ushort[3] { 0xE, 0x2027, 0x12 }, new ushort[3] { 0x202A, 0xFFFF, 0x12 } };

        private static ushort[][] p_Transitions19 = { new ushort[3] { 0xA, 0xA, 0x16 }, new ushort[3] { 0xD, 0xD, 0x16 }, new ushort[3] { 0x2028, 0x2029, 0x16 }, new ushort[3] { 0x0, 0x9, 0x13 }, new ushort[3] { 0xB, 0xC, 0x13 }, new ushort[3] { 0xE, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 } };

        private static ushort[][] p_Transitions20 = { new ushort[3] { 0xA, 0xA, 0x16 }, new ushort[3] { 0xD, 0xD, 0x16 }, new ushort[3] { 0x2028, 0x2029, 0x16 }, new ushort[3] { 0x0, 0x9, 0x13 }, new ushort[3] { 0xB, 0xC, 0x13 }, new ushort[3] { 0xE, 0x29, 0x13 }, new ushort[3] { 0x2B, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x2A, 0x2A, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0x1C } };

        private static ushort[][] p_Transitions21 = { new ushort[3] { 0x2F, 0x2F, 0xAB }, new ushort[3] { 0x0, 0x2E, 0x19 }, new ushort[3] { 0x30, 0xFFFF, 0x19 } };

        private static ushort[][] p_Transitions22 = { new ushort[3] { 0xA, 0xA, 0x16 }, new ushort[3] { 0xD, 0xD, 0x16 }, new ushort[3] { 0x2028, 0x2029, 0x16 }, new ushort[3] { 0x0, 0x9, 0x13 }, new ushort[3] { 0xB, 0xC, 0x13 }, new ushort[3] { 0xE, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 } };

        private static ushort[][] p_Transitions23 = { new ushort[3] { 0x0, 0x29, 0x17 }, new ushort[3] { 0x2B, 0xFFFF, 0x17 }, new ushort[3] { 0x2A, 0x2A, 0x20 } };

        private static ushort[][] p_Transitions24 = { new ushort[3] { 0x0, 0x29, 0x18 }, new ushort[3] { 0x2B, 0xFFFF, 0x18 }, new ushort[3] { 0x2A, 0x2A, 0x21 } };

        private static ushort[][] p_Transitions25 = { new ushort[3] { 0x2A, 0x2A, 0x15 }, new ushort[3] { 0x0, 0x29, 0x8 }, new ushort[3] { 0x2B, 0xFFFF, 0x8 } };

        private static ushort[][] p_Transitions26 = { new ushort[3] { 0x0, 0xFFFF, 0x1A } };

        private static ushort[][] p_Transitions27 = { new ushort[3] { 0x0, 0x29, 0x1A }, new ushort[3] { 0x2B, 0x2E, 0x1A }, new ushort[3] { 0x30, 0xFFFF, 0x1A }, new ushort[3] { 0x2A, 0x2A, 0x18 }, new ushort[3] { 0x2F, 0x2F, 0x1D } };

        private static ushort[][] p_Transitions28 = { new ushort[3] { 0xA, 0xA, 0xAE }, new ushort[3] { 0x2028, 0x2029, 0xAE }, new ushort[3] { 0x0, 0x9, 0x1C }, new ushort[3] { 0xB, 0xC, 0x1C }, new ushort[3] { 0xE, 0x2027, 0x1C }, new ushort[3] { 0x202A, 0xFFFF, 0x1C }, new ushort[3] { 0xD, 0xD, 0xB0 } };

        private static ushort[][] p_Transitions29 = { new ushort[3] { 0xA, 0xA, 0xAF }, new ushort[3] { 0xD, 0xD, 0xAF }, new ushort[3] { 0x2028, 0x2029, 0xAF }, new ushort[3] { 0x0, 0x9, 0x1D }, new ushort[3] { 0xB, 0xC, 0x1D }, new ushort[3] { 0xE, 0x2027, 0x1D }, new ushort[3] { 0x202A, 0xFFFF, 0x1D } };

        private static ushort[][] p_Transitions30 = { new ushort[3] { 0x30, 0x39, 0xBC }, new ushort[3] { 0x41, 0x46, 0xBC }, new ushort[3] { 0x61, 0x66, 0xBC } };

        private static ushort[][] p_Transitions31 = { new ushort[3] { 0x58, 0x58, 0xBD } };

        private static ushort[][] p_Transitions32 = { new ushort[3] { 0x0, 0x2E, 0x17 }, new ushort[3] { 0x30, 0xFFFF, 0x17 }, new ushort[3] { 0x2F, 0x2F, 0xAE } };

        private static ushort[][] p_Transitions33 = { new ushort[3] { 0x0, 0x2E, 0x18 }, new ushort[3] { 0x30, 0xFFFF, 0x18 }, new ushort[3] { 0x2F, 0x2F, 0xAF } };

        private static ushort[][] p_Transitions34 = { new ushort[3] { 0x30, 0x39, 0x35 }, new ushort[3] { 0x41, 0x46, 0x35 }, new ushort[3] { 0x61, 0x66, 0x35 } };

        private static ushort[][] p_Transitions35 = { new ushort[3] { 0x30, 0x39, 0x2D }, new ushort[3] { 0x41, 0x46, 0x2D }, new ushort[3] { 0x61, 0x66, 0x2D } };

        private static ushort[][] p_Transitions36 = { new ushort[3] { 0x30, 0x39, 0x25 }, new ushort[3] { 0x41, 0x46, 0x25 }, new ushort[3] { 0x61, 0x66, 0x25 } };

        private static ushort[][] p_Transitions37 = { new ushort[3] { 0x30, 0x39, 0x26 }, new ushort[3] { 0x41, 0x46, 0x26 }, new ushort[3] { 0x61, 0x66, 0x26 } };

        private static ushort[][] p_Transitions38 = { new ushort[3] { 0x30, 0x39, 0x27 }, new ushort[3] { 0x41, 0x46, 0x27 }, new ushort[3] { 0x61, 0x66, 0x27 } };

        private static ushort[][] p_Transitions39 = { new ushort[3] { 0x30, 0x39, 0x28 }, new ushort[3] { 0x41, 0x46, 0x28 }, new ushort[3] { 0x61, 0x66, 0x28 } };

        private static ushort[][] p_Transitions40 = { new ushort[3] { 0x30, 0x39, 0x29 }, new ushort[3] { 0x41, 0x46, 0x29 }, new ushort[3] { 0x61, 0x66, 0x29 } };

        private static ushort[][] p_Transitions41 = { new ushort[3] { 0x30, 0x39, 0x2A }, new ushort[3] { 0x41, 0x46, 0x2A }, new ushort[3] { 0x61, 0x66, 0x2A } };

        private static ushort[][] p_Transitions42 = { new ushort[3] { 0x30, 0x39, 0x2B }, new ushort[3] { 0x41, 0x46, 0x2B }, new ushort[3] { 0x61, 0x66, 0x2B } };

        private static ushort[][] p_Transitions43 = { new ushort[3] { 0x30, 0x39, 0x2C }, new ushort[3] { 0x41, 0x46, 0x2C }, new ushort[3] { 0x61, 0x66, 0x2C } };

        private static ushort[][] p_Transitions44 = { new ushort[3] { 0x30, 0x39, 0x2E }, new ushort[3] { 0x41, 0x46, 0x2E }, new ushort[3] { 0x61, 0x66, 0x2E } };

        private static ushort[][] p_Transitions45 = { new ushort[3] { 0x30, 0x39, 0x2F }, new ushort[3] { 0x41, 0x46, 0x2F }, new ushort[3] { 0x61, 0x66, 0x2F } };

        private static ushort[][] p_Transitions46 = { new ushort[3] { 0x30, 0x39, 0x30 }, new ushort[3] { 0x41, 0x46, 0x30 }, new ushort[3] { 0x61, 0x66, 0x30 } };

        private static ushort[][] p_Transitions47 = { new ushort[3] { 0x30, 0x39, 0x31 }, new ushort[3] { 0x41, 0x46, 0x31 }, new ushort[3] { 0x61, 0x66, 0x31 } };

        private static ushort[][] p_Transitions48 = { new ushort[3] { 0x30, 0x39, 0x32 }, new ushort[3] { 0x41, 0x46, 0x32 }, new ushort[3] { 0x61, 0x66, 0x32 } };

        private static ushort[][] p_Transitions49 = { new ushort[3] { 0x30, 0x39, 0x33 }, new ushort[3] { 0x41, 0x46, 0x33 }, new ushort[3] { 0x61, 0x66, 0x33 } };

        private static ushort[][] p_Transitions50 = { new ushort[3] { 0x30, 0x39, 0x34 }, new ushort[3] { 0x41, 0x46, 0x34 }, new ushort[3] { 0x61, 0x66, 0x34 } };

        private static ushort[][] p_Transitions51 = { new ushort[3] { 0x30, 0x39, 0x36 }, new ushort[3] { 0x41, 0x46, 0x36 }, new ushort[3] { 0x61, 0x66, 0x36 } };

        private static ushort[][] p_Transitions52 = { new ushort[3] { 0x30, 0x39, 0x37 }, new ushort[3] { 0x41, 0x46, 0x37 }, new ushort[3] { 0x61, 0x66, 0x37 } };

        private static ushort[][] p_Transitions53 = { new ushort[3] { 0x30, 0x39, 0x4E }, new ushort[3] { 0x41, 0x46, 0x4E }, new ushort[3] { 0x61, 0x66, 0x4E } };

        private static ushort[][] p_Transitions54 = { new ushort[3] { 0x30, 0x39, 0x50 }, new ushort[3] { 0x41, 0x46, 0x50 }, new ushort[3] { 0x61, 0x66, 0x50 } };

        private static ushort[][] p_Transitions55 = { new ushort[3] { 0x30, 0x39, 0x52 }, new ushort[3] { 0x41, 0x46, 0x52 }, new ushort[3] { 0x61, 0x66, 0x52 } };

        private static ushort[][] p_Transitions56 = { new ushort[3] { 0x58, 0x58, 0x4B } };

        private static ushort[][] p_Transitions57 = { new ushort[3] { 0x58, 0x58, 0x43 } };

        private static ushort[][] p_Transitions58 = { new ushort[3] { 0x58, 0x58, 0x3B } };

        private static ushort[][] p_Transitions59 = { new ushort[3] { 0x58, 0x58, 0x3C } };

        private static ushort[][] p_Transitions60 = { new ushort[3] { 0x58, 0x58, 0x3D } };

        private static ushort[][] p_Transitions61 = { new ushort[3] { 0x58, 0x58, 0x3E } };

        private static ushort[][] p_Transitions62 = { new ushort[3] { 0x58, 0x58, 0x3F } };

        private static ushort[][] p_Transitions63 = { new ushort[3] { 0x58, 0x58, 0x40 } };

        private static ushort[][] p_Transitions64 = { new ushort[3] { 0x58, 0x58, 0x41 } };

        private static ushort[][] p_Transitions65 = { new ushort[3] { 0x58, 0x58, 0x42 } };

        private static ushort[][] p_Transitions66 = { new ushort[3] { 0x58, 0x58, 0x44 } };

        private static ushort[][] p_Transitions67 = { new ushort[3] { 0x58, 0x58, 0x45 } };

        private static ushort[][] p_Transitions68 = { new ushort[3] { 0x58, 0x58, 0x46 } };

        private static ushort[][] p_Transitions69 = { new ushort[3] { 0x58, 0x58, 0x47 } };

        private static ushort[][] p_Transitions70 = { new ushort[3] { 0x58, 0x58, 0x48 } };

        private static ushort[][] p_Transitions71 = { new ushort[3] { 0x58, 0x58, 0x49 } };

        private static ushort[][] p_Transitions72 = { new ushort[3] { 0x58, 0x58, 0x4A } };

        private static ushort[][] p_Transitions73 = { new ushort[3] { 0x58, 0x58, 0x4C } };

        private static ushort[][] p_Transitions74 = { new ushort[3] { 0x58, 0x58, 0x4D } };

        private static ushort[][] p_Transitions75 = { new ushort[3] { 0x58, 0x58, 0x4F } };

        private static ushort[][] p_Transitions76 = { new ushort[3] { 0x58, 0x58, 0x51 } };

        private static ushort[][] p_Transitions77 = { new ushort[3] { 0x58, 0x58, 0x53 } };

        private static ushort[][] p_Transitions78 = { new ushort[3] { 0x30, 0x39, 0xC5 }, new ushort[3] { 0x41, 0x46, 0xC5 }, new ushort[3] { 0x61, 0x66, 0xC5 } };

        private static ushort[][] p_Transitions79 = { new ushort[3] { 0x58, 0x58, 0xC6 } };

        private static ushort[][] p_Transitions80 = { new ushort[3] { 0x30, 0x39, 0xC7 }, new ushort[3] { 0x41, 0x46, 0xC7 }, new ushort[3] { 0x61, 0x66, 0xC7 } };

        private static ushort[][] p_Transitions81 = { new ushort[3] { 0x58, 0x58, 0xC8 } };

        private static ushort[][] p_Transitions82 = { new ushort[3] { 0x30, 0x39, 0xC9 }, new ushort[3] { 0x41, 0x46, 0xC9 }, new ushort[3] { 0x61, 0x66, 0xC9 } };

        private static ushort[][] p_Transitions83 = { new ushort[3] { 0x58, 0x58, 0xCA } };

        private static ushort[][] p_Transitions84 = { new ushort[3] { 0x75, 0x75, 0x5D }, new ushort[3] { 0x72, 0x72, 0x5E }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x71, 0x5F }, new ushort[3] { 0x73, 0x74, 0x5F }, new ushort[3] { 0x76, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions85 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions86 = { new ushort[3] { 0x6E, 0x6E, 0x60 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6D, 0x5F }, new ushort[3] { 0x6F, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions87 = { new ushort[3] { 0x65, 0x65, 0x61 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions88 = { new ushort[3] { 0x72, 0x72, 0x62 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x71, 0x5F }, new ushort[3] { 0x73, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions89 = { new ushort[3] { 0x61, 0x61, 0x63 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions90 = { new ushort[3] { 0x75, 0x75, 0x64 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x74, 0x5F }, new ushort[3] { 0x76, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions91 = { new ushort[3] { 0x66, 0x66, 0xA5 }, new ushort[3] { 0x73, 0x73, 0xA6 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x65, 0x5F }, new ushort[3] { 0x67, 0x72, 0x5F }, new ushort[3] { 0x74, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions92 = { new ushort[3] { 0x70, 0x70, 0x66 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6F, 0x5F }, new ushort[3] { 0x71, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions93 = { new ushort[3] { 0x62, 0x62, 0x65 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x61, 0x5F }, new ushort[3] { 0x63, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions94 = { new ushort[3] { 0x69, 0x69, 0x67 }, new ushort[3] { 0x6F, 0x6F, 0x68 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x68, 0x5F }, new ushort[3] { 0x6A, 0x6E, 0x5F }, new ushort[3] { 0x70, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions95 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions96 = { new ushort[3] { 0x74, 0x74, 0x69 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x73, 0x5F }, new ushort[3] { 0x75, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions97 = { new ushort[3] { 0x72, 0x72, 0x6B }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x71, 0x5F }, new ushort[3] { 0x73, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions98 = { new ushort[3] { 0x61, 0x61, 0x6C }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions99 = { new ushort[3] { 0x6D, 0x6D, 0x6A }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6C, 0x5F }, new ushort[3] { 0x6E, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions100 = { new ushort[3] { 0x6C, 0x6C, 0x6D }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6B, 0x5F }, new ushort[3] { 0x6D, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions101 = { new ushort[3] { 0x6C, 0x6C, 0x6F }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6B, 0x5F }, new ushort[3] { 0x6D, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions102 = { new ushort[3] { 0x74, 0x74, 0x6E }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x73, 0x5F }, new ushort[3] { 0x75, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions103 = { new ushort[3] { 0x76, 0x76, 0x70 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x75, 0x5F }, new ushort[3] { 0x77, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions104 = { new ushort[3] { 0x74, 0x74, 0x71 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x73, 0x5F }, new ushort[3] { 0x75, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions105 = { new ushort[3] { 0x65, 0x65, 0x72 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions106 = { new ushort[3] { 0x65, 0x65, 0x75 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions107 = { new ushort[3] { 0x6D, 0x6D, 0x73 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6C, 0x5F }, new ushort[3] { 0x6E, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions108 = { new ushort[3] { 0x6D, 0x6D, 0x74 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6C, 0x5F }, new ushort[3] { 0x6E, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions109 = { new ushort[3] { 0x65, 0x65, 0x77 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions110 = { new ushort[3] { 0x69, 0x69, 0x76 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x68, 0x5F }, new ushort[3] { 0x6A, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions111 = { new ushort[3] { 0x69, 0x69, 0x78 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x68, 0x5F }, new ushort[3] { 0x6A, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions112 = { new ushort[3] { 0x61, 0x61, 0x79 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions113 = { new ushort[3] { 0x65, 0x65, 0x7A }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions114 = { new ushort[3] { 0x72, 0x72, 0x7B }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x71, 0x5F }, new ushort[3] { 0x73, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions115 = { new ushort[3] { 0x69, 0x69, 0x7C }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x68, 0x5F }, new ushort[3] { 0x6A, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions116 = { new ushort[3] { 0x6D, 0x6D, 0x7D }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6C, 0x5F }, new ushort[3] { 0x6E, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions117 = { new ushort[3] { 0x73, 0x73, 0x7E }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x72, 0x5F }, new ushort[3] { 0x74, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions118 = { new ushort[3] { 0x6F, 0x6F, 0x7F }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6E, 0x5F }, new ushort[3] { 0x70, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions119 = { new ushort[3] { 0x73, 0x73, 0xBA }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x72, 0x5F }, new ushort[3] { 0x74, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions120 = { new ushort[3] { 0x63, 0x63, 0xBB }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x62, 0x5F }, new ushort[3] { 0x64, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions121 = { new ushort[3] { 0x74, 0x74, 0x87 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x73, 0x5F }, new ushort[3] { 0x75, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions122 = { new ushort[3] { 0x63, 0x63, 0x80 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x62, 0x5F }, new ushort[3] { 0x64, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions123 = { new ushort[3] { 0x6E, 0x6E, 0x81 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6D, 0x5F }, new ushort[3] { 0x6F, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions124 = { new ushort[3] { 0x6E, 0x6E, 0x82 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6D, 0x5F }, new ushort[3] { 0x6F, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions125 = { new ushort[3] { 0x61, 0x61, 0x88 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions126 = { new ushort[3] { 0x70, 0x70, 0x83 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6F, 0x5F }, new ushort[3] { 0x71, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions127 = { new ushort[3] { 0x6E, 0x6E, 0x89 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6D, 0x5F }, new ushort[3] { 0x6F, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions128 = { new ushort[3] { 0x74, 0x74, 0x84 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x73, 0x5F }, new ushort[3] { 0x75, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions129 = { new ushort[3] { 0x61, 0x61, 0x8A }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions130 = { new ushort[3] { 0x61, 0x61, 0x85 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions131 = { new ushort[3] { 0x61, 0x61, 0x86 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x62, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions132 = { new ushort[3] { 0x65, 0x65, 0x8B }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions133 = { new ushort[3] { 0x6C, 0x6C, 0x8C }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6B, 0x5F }, new ushort[3] { 0x6D, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions134 = { new ushort[3] { 0x63, 0x63, 0x8D }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x62, 0x5F }, new ushort[3] { 0x64, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions135 = { new ushort[3] { 0x65, 0x65, 0xBE }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions136 = { new ushort[3] { 0x72, 0x72, 0xBF }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x71, 0x5F }, new ushort[3] { 0x73, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions137 = { new ushort[3] { 0x73, 0x73, 0xC0 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x72, 0x5F }, new ushort[3] { 0x74, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions138 = { new ushort[3] { 0x6C, 0x6C, 0xC1 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x6B, 0x5F }, new ushort[3] { 0x6D, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions139 = { new ushort[3] { 0x64, 0x64, 0xC2 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x63, 0x5F }, new ushort[3] { 0x65, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions140 = { new ushort[3] { 0x73, 0x73, 0xC3 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x72, 0x5F }, new ushort[3] { 0x74, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions141 = { new ushort[3] { 0x65, 0x65, 0xC4 }, new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x64, 0x5F }, new ushort[3] { 0x66, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions142 = { new ushort[3] { 0x2E, 0x2E, 0xA7 } };

        private static ushort[][] p_Transitions143 = { };

        private static ushort[][] p_Transitions144 = { };

        private static ushort[][] p_Transitions145 = { new ushort[3] { 0x5C, 0x5C, 0xB }, new ushort[3] { 0x0, 0x5A, 0xC }, new ushort[3] { 0x5E, 0xFFFF, 0xC } };

        private static ushort[][] p_Transitions146 = { new ushort[3] { 0x3E, 0x3E, 0xA9 } };

        private static ushort[][] p_Transitions147 = { };

        private static ushort[][] p_Transitions148 = { };

        private static ushort[][] p_Transitions149 = { };

        private static ushort[][] p_Transitions150 = { };

        private static ushort[][] p_Transitions151 = { };

        private static ushort[][] p_Transitions152 = { };

        private static ushort[][] p_Transitions153 = { new ushort[3] { 0x3E, 0x3E, 0xAA } };

        private static ushort[][] p_Transitions154 = { };

        private static ushort[][] p_Transitions155 = { };

        private static ushort[][] p_Transitions156 = { };

        private static ushort[][] p_Transitions157 = { };

        private static ushort[][] p_Transitions158 = { };

        private static ushort[][] p_Transitions159 = { };

        private static ushort[][] p_Transitions160 = { };

        private static ushort[][] p_Transitions161 = { };

        private static ushort[][] p_Transitions162 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAB }, new ushort[3] { 0x20, 0x20, 0xAB }, new ushort[3] { 0x2028, 0x2029, 0xAB } };

        private static ushort[][] p_Transitions163 = { new ushort[3] { 0xA, 0xA, 0xA2 }, new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0x9, 0xAB }, new ushort[3] { 0xB, 0xD, 0xAB }, new ushort[3] { 0x20, 0x20, 0xAB }, new ushort[3] { 0x2028, 0x2029, 0xAB } };

        private static ushort[][] p_Transitions164 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAB }, new ushort[3] { 0x20, 0x20, 0xAB }, new ushort[3] { 0x2028, 0x2029, 0xAB } };

        private static ushort[][] p_Transitions165 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions166 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions167 = { };

        private static ushort[][] p_Transitions168 = { };

        private static ushort[][] p_Transitions169 = { };

        private static ushort[][] p_Transitions170 = { };

        private static ushort[][] p_Transitions171 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAB }, new ushort[3] { 0x20, 0x20, 0xAB }, new ushort[3] { 0x2028, 0x2029, 0xAB } };

        private static ushort[][] p_Transitions172 = { new ushort[3] { 0xA, 0xA, 0xAC }, new ushort[3] { 0xD, 0xD, 0xAC }, new ushort[3] { 0x2028, 0x2029, 0xAC }, new ushort[3] { 0x0, 0x8, 0x13 }, new ushort[3] { 0xE, 0x1F, 0x13 }, new ushort[3] { 0x21, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x2F, 0x2F, 0x14 }, new ushort[3] { 0x9, 0x9, 0xAE }, new ushort[3] { 0xB, 0xC, 0xAE }, new ushort[3] { 0x20, 0x20, 0xAE } };

        private static ushort[][] p_Transitions173 = { new ushort[3] { 0xA, 0xA, 0xAD }, new ushort[3] { 0xD, 0xD, 0xAD }, new ushort[3] { 0x2028, 0x2029, 0xAD }, new ushort[3] { 0x0, 0x8, 0x1A }, new ushort[3] { 0xE, 0x1F, 0x1A }, new ushort[3] { 0x21, 0x2E, 0x1A }, new ushort[3] { 0x30, 0x2027, 0x1A }, new ushort[3] { 0x202A, 0xFFFF, 0x1A }, new ushort[3] { 0x9, 0x9, 0xAF }, new ushort[3] { 0xB, 0xC, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2F, 0x2F, 0x1B } };

        private static ushort[][] p_Transitions174 = { new ushort[3] { 0xA, 0xA, 0xAC }, new ushort[3] { 0xD, 0xD, 0xAC }, new ushort[3] { 0x2028, 0x2029, 0xAC }, new ushort[3] { 0x0, 0x8, 0x13 }, new ushort[3] { 0xE, 0x1F, 0x13 }, new ushort[3] { 0x21, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x9, 0x9, 0xAE }, new ushort[3] { 0xB, 0xC, 0xAE }, new ushort[3] { 0x20, 0x20, 0xAE }, new ushort[3] { 0x2F, 0x2F, 0x14 } };

        private static ushort[][] p_Transitions175 = { new ushort[3] { 0xA, 0xA, 0xAD }, new ushort[3] { 0xD, 0xD, 0xAD }, new ushort[3] { 0x2028, 0x2029, 0xAD }, new ushort[3] { 0x0, 0x8, 0x1A }, new ushort[3] { 0xE, 0x1F, 0x1A }, new ushort[3] { 0x21, 0x2E, 0x1A }, new ushort[3] { 0x30, 0x2027, 0x1A }, new ushort[3] { 0x202A, 0xFFFF, 0x1A }, new ushort[3] { 0x9, 0x9, 0xAF }, new ushort[3] { 0xB, 0xC, 0xAF }, new ushort[3] { 0x20, 0x20, 0xAF }, new ushort[3] { 0x2F, 0x2F, 0x1B } };

        private static ushort[][] p_Transitions176 = { new ushort[3] { 0x9, 0xC, 0xAE }, new ushort[3] { 0x20, 0x20, 0xAE }, new ushort[3] { 0xD, 0xD, 0xAC }, new ushort[3] { 0x2028, 0x2029, 0xAC }, new ushort[3] { 0x0, 0x8, 0x13 }, new ushort[3] { 0xE, 0x1F, 0x13 }, new ushort[3] { 0x21, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x2F, 0x2F, 0x14 } };

        private static ushort[][] p_Transitions177 = { new ushort[3] { 0xA, 0xA, 0xAC }, new ushort[3] { 0xD, 0xD, 0xAC }, new ushort[3] { 0x2028, 0x2029, 0xAC }, new ushort[3] { 0x0, 0x8, 0x13 }, new ushort[3] { 0xE, 0x1F, 0x13 }, new ushort[3] { 0x21, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x2F, 0x2F, 0x14 }, new ushort[3] { 0x9, 0x9, 0xAE }, new ushort[3] { 0xB, 0xC, 0xAE }, new ushort[3] { 0x20, 0x20, 0xAE } };

        private static ushort[][] p_Transitions178 = { new ushort[3] { 0xA, 0xA, 0xB1 }, new ushort[3] { 0xD, 0xD, 0xAC }, new ushort[3] { 0x2028, 0x2029, 0xAC }, new ushort[3] { 0x0, 0x8, 0x13 }, new ushort[3] { 0xE, 0x1F, 0x13 }, new ushort[3] { 0x21, 0x2E, 0x13 }, new ushort[3] { 0x30, 0x2027, 0x13 }, new ushort[3] { 0x202A, 0xFFFF, 0x13 }, new ushort[3] { 0x2F, 0x2F, 0x14 }, new ushort[3] { 0x9, 0x9, 0xAE }, new ushort[3] { 0xB, 0xC, 0xAE }, new ushort[3] { 0x20, 0x20, 0xAE } };

        private static ushort[][] p_Transitions179 = { };

        private static ushort[][] p_Transitions180 = { };

        private static ushort[][] p_Transitions181 = { new ushort[3] { 0x30, 0x31, 0xB5 } };

        private static ushort[][] p_Transitions182 = { new ushort[3] { 0x42, 0x42, 0xB6 } };

        private static ushort[][] p_Transitions183 = { new ushort[3] { 0x2F, 0x2F, 0x2 }, new ushort[3] { 0x9, 0xD, 0xAB }, new ushort[3] { 0x20, 0x20, 0xAB }, new ushort[3] { 0x2028, 0x2029, 0xAB } };

        private static ushort[][] p_Transitions184 = { new ushort[3] { 0x30, 0x39, 0x1E }, new ushort[3] { 0x41, 0x46, 0x1E }, new ushort[3] { 0x61, 0x66, 0x1E } };

        private static ushort[][] p_Transitions185 = { new ushort[3] { 0x58, 0x58, 0x1F } };

        private static ushort[][] p_Transitions186 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions187 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions188 = { new ushort[3] { 0x30, 0x39, 0x22 }, new ushort[3] { 0x41, 0x46, 0x22 }, new ushort[3] { 0x61, 0x66, 0x22 } };

        private static ushort[][] p_Transitions189 = { new ushort[3] { 0x58, 0x58, 0x38 } };

        private static ushort[][] p_Transitions190 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions191 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions192 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions193 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions194 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions195 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions196 = { new ushort[3] { 0x30, 0x39, 0x5F }, new ushort[3] { 0x41, 0x5A, 0x5F }, new ushort[3] { 0x5F, 0x5F, 0x5F }, new ushort[3] { 0x61, 0x7A, 0x5F } };

        private static ushort[][] p_Transitions197 = { new ushort[3] { 0x30, 0x39, 0x23 }, new ushort[3] { 0x41, 0x46, 0x23 }, new ushort[3] { 0x61, 0x66, 0x23 } };

        private static ushort[][] p_Transitions198 = { new ushort[3] { 0x58, 0x58, 0x39 } };

        private static ushort[][] p_Transitions199 = { new ushort[3] { 0x30, 0x39, 0x24 }, new ushort[3] { 0x41, 0x46, 0x24 }, new ushort[3] { 0x61, 0x66, 0x24 } };

        private static ushort[][] p_Transitions200 = { new ushort[3] { 0x58, 0x58, 0x3A } };

        private static ushort[][] p_Transitions201 = { };

        private static ushort[][] p_Transitions202 = { };

        private static ushort[][][] p_Transitions = { p_Transitions0, p_Transitions1, p_Transitions2, p_Transitions3, p_Transitions4, p_Transitions5, p_Transitions6, p_Transitions7, p_Transitions8, p_Transitions9, p_Transitions10, p_Transitions11, p_Transitions12, p_Transitions13, p_Transitions14, p_Transitions15, p_Transitions16, p_Transitions17, p_Transitions18, p_Transitions19, p_Transitions20, p_Transitions21, p_Transitions22, p_Transitions23, p_Transitions24, p_Transitions25, p_Transitions26, p_Transitions27, p_Transitions28, p_Transitions29, p_Transitions30, p_Transitions31, p_Transitions32, p_Transitions33, p_Transitions34, p_Transitions35, p_Transitions36, p_Transitions37, p_Transitions38, p_Transitions39, p_Transitions40, p_Transitions41, p_Transitions42, p_Transitions43, p_Transitions44, p_Transitions45, p_Transitions46, p_Transitions47, p_Transitions48, p_Transitions49, p_Transitions50, p_Transitions51, p_Transitions52, p_Transitions53, p_Transitions54, p_Transitions55, p_Transitions56, p_Transitions57, p_Transitions58, p_Transitions59, p_Transitions60, p_Transitions61, p_Transitions62, p_Transitions63, p_Transitions64, p_Transitions65, p_Transitions66, p_Transitions67, p_Transitions68, p_Transitions69, p_Transitions70, p_Transitions71, p_Transitions72, p_Transitions73, p_Transitions74, p_Transitions75, p_Transitions76, p_Transitions77, p_Transitions78, p_Transitions79, p_Transitions80, p_Transitions81, p_Transitions82, p_Transitions83, p_Transitions84, p_Transitions85, p_Transitions86, p_Transitions87, p_Transitions88, p_Transitions89, p_Transitions90, p_Transitions91, p_Transitions92, p_Transitions93, p_Transitions94, p_Transitions95, p_Transitions96, p_Transitions97, p_Transitions98, p_Transitions99, p_Transitions100, p_Transitions101, p_Transitions102, p_Transitions103, p_Transitions104, p_Transitions105, p_Transitions106, p_Transitions107, p_Transitions108, p_Transitions109, p_Transitions110, p_Transitions111, p_Transitions112, p_Transitions113, p_Transitions114, p_Transitions115, p_Transitions116, p_Transitions117, p_Transitions118, p_Transitions119, p_Transitions120, p_Transitions121, p_Transitions122, p_Transitions123, p_Transitions124, p_Transitions125, p_Transitions126, p_Transitions127, p_Transitions128, p_Transitions129, p_Transitions130, p_Transitions131, p_Transitions132, p_Transitions133, p_Transitions134, p_Transitions135, p_Transitions136, p_Transitions137, p_Transitions138, p_Transitions139, p_Transitions140, p_Transitions141, p_Transitions142, p_Transitions143, p_Transitions144, p_Transitions145, p_Transitions146, p_Transitions147, p_Transitions148, p_Transitions149, p_Transitions150, p_Transitions151, p_Transitions152, p_Transitions153, p_Transitions154, p_Transitions155, p_Transitions156, p_Transitions157, p_Transitions158, p_Transitions159, p_Transitions160, p_Transitions161, p_Transitions162, p_Transitions163, p_Transitions164, p_Transitions165, p_Transitions166, p_Transitions167, p_Transitions168, p_Transitions169, p_Transitions170, p_Transitions171, p_Transitions172, p_Transitions173, p_Transitions174, p_Transitions175, p_Transitions176, p_Transitions177, p_Transitions178, p_Transitions179, p_Transitions180, p_Transitions181, p_Transitions182, p_Transitions183, p_Transitions184, p_Transitions185, p_Transitions186, p_Transitions187, p_Transitions188, p_Transitions189, p_Transitions190, p_Transitions191, p_Transitions192, p_Transitions193, p_Transitions194, p_Transitions195, p_Transitions196, p_Transitions197, p_Transitions198, p_Transitions199, p_Transitions200, p_Transitions201, p_Transitions202 };
        private static int[] p_Finals = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 21, 21, 22, 23, 24, 25, 26, 27, 21, 21, 21, 21, 21, 21, 21, 21, 28, 29, 30, 31, 21, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };

        public Lexer_Hime_Kernel_FileCentralDogma(string Input) : base(Input) { }

        public static string GetSymbolName(ushort SID)
        {
            for (int i = 0; i != p_SymbolsSID.Length; i++)
            {
                if (p_SymbolsSID[i] == SID)
                    return p_SymbolsName[i];
            }
            return null;
        }



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

        private delegate ushort Production(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack);

        private static ushort ReduceRule_11_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x11, "qualified_name"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x11;
        }

        private static ushort ReduceRule_12_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x12, "symbol_access_public"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_public"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x12;
        }

        private static ushort ReduceRule_13_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x13, "symbol_access_private"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_private"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x13;
        }

        private static ushort ReduceRule_14_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x14, "symbol_access_protected"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_protected"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x14;
        }

        private static ushort ReduceRule_15_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x15, "symbol_access_internal"));

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("access_internal"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x15;
        }

        private static ushort ReduceRule_16_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x16;
        }

        private static ushort ReduceRule_16_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x16;
        }

        private static ushort ReduceRule_16_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x16;
        }

        private static ushort ReduceRule_16_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x16;
        }

        private static ushort ReduceRule_16_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x16, "Namespace_child_symbol"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x16;
        }

        private static ushort ReduceRule_17_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x17, "Namespace_content"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x17;
        }

        private static ushort ReduceRule_18_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 5, 5);
            Nodes.RemoveRange(Nodes.Count - 5, 5);
            for (int i = 0; i != 5; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x18, "Namespace"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x18;
        }

        private static ushort ReduceRule_19_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x19, "_m18"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x19;
        }

        private static ushort ReduceRule_19_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x19, "_m18"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x19;
        }

        private static ushort ReduceRule_1A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1A, "_m23"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x1A;
        }

        private static ushort ReduceRule_1A_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x1A, "_m23"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x1A;
        }

        private static ushort ReduceRule_4F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x4F, "option"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x4F;
        }

        private static ushort ReduceRule_50_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x50, "terminal_def_atom_unicode"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x50;
        }

        private static ushort ReduceRule_50_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x50, "terminal_def_atom_unicode"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x50;
        }

        private static ushort ReduceRule_51_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x51, "terminal_def_atom_text"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x51;
        }

        private static ushort ReduceRule_52_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x52, "terminal_def_atom_set"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x52;
        }

        private static ushort ReduceRule_53_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x53, "terminal_def_atom_span"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x53;
        }

        private static ushort ReduceRule_54_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x54, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x54;
        }

        private static ushort ReduceRule_54_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x54, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x54;
        }

        private static ushort ReduceRule_54_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x54, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x54;
        }

        private static ushort ReduceRule_54_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x54, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x54;
        }

        private static ushort ReduceRule_54_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x54, "terminal_def_atom"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x54;
        }

        private static ushort ReduceRule_55_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x55, "terminal_def_element"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x55;
        }

        private static ushort ReduceRule_55_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x55, "terminal_def_element"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x55;
        }

        private static ushort ReduceRule_56_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x56;
        }

        private static ushort ReduceRule_56_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x56;
        }

        private static ushort ReduceRule_56_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x56;
        }

        private static ushort ReduceRule_56_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x56, "terminal_def_repetition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x56;
        }

        private static ushort ReduceRule_57_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x57, "terminal_def_fragment"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x57;
        }

        private static ushort ReduceRule_58_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x58, "terminal_def_restrict"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x58;
        }

        private static ushort ReduceRule_59_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x59, "terminal_definition"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x59;
        }

        private static ushort ReduceRule_5A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5A, "terminal_subgrammar"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x5A;
        }

        private static ushort ReduceRule_5A_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5A, "terminal_subgrammar"));

            Nodes.Add(SubRoot);
            return 0x5A;
        }

        private static ushort ReduceRule_5B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 5, 5);
            Nodes.RemoveRange(Nodes.Count - 5, 5);
            for (int i = 0; i != 5; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5B, "terminal"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x5B;
        }

        private static ushort ReduceRule_5C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5C, "rule_sym_action"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x5C;
        }

        private static ushort ReduceRule_5D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5D, "rule_sym_virtual"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x5D;
        }

        private static ushort ReduceRule_5E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5E, "rule_sym_ref_simple"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x5E;
        }

        private static ushort ReduceRule_5F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x5F, "rule_template_params"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x5F;
        }

        private static ushort ReduceRule_60_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_5(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_6(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_7(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_8(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_9(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_A(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_60_B(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x60, "grammar_bin_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x60;
        }

        private static ushort ReduceRule_61_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x61, "grammar_text_terminal"));

            SubRoot.AppendChild(Definition[0]);

            Nodes.Add(SubRoot);
            return 0x61;
        }

        private static ushort ReduceRule_62_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x62, "grammar_options"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x62;
        }

        private static ushort ReduceRule_63_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x63, "grammar_terminals"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x63;
        }

        private static ushort ReduceRule_64_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x64, "grammar_parency"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x64;
        }

        private static ushort ReduceRule_64_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x64, "grammar_parency"));

            Nodes.Add(SubRoot);
            return 0x64;
        }

        private static ushort ReduceRule_65_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x65, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x65;
        }

        private static ushort ReduceRule_65_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x65, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x65;
        }

        private static ushort ReduceRule_65_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x65, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x65;
        }

        private static ushort ReduceRule_65_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x65, "grammar_access"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x65;
        }

        private static ushort ReduceRule_66_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 10, 10);
            Nodes.RemoveRange(Nodes.Count - 10, 10);
            for (int i = 0; i != 10; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x66, "cf_grammar_text"));

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

            Nodes.Add(SubRoot);
            return 0x66;
        }

        private static ushort ReduceRule_67_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 9, 9);
            Nodes.RemoveRange(Nodes.Count - 9, 9);
            for (int i = 0; i != 9; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x67, "cf_grammar_bin"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7]);

            SubRoot.AppendChild(Definition[8], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x67;
        }

        private static ushort ReduceRule_68_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "_m75"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x68;
        }

        private static ushort ReduceRule_68_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x68, "_m75"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x68;
        }

        private static ushort ReduceRule_69_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x69, "_m77"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x69;
        }

        private static ushort ReduceRule_69_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x69, "_m77"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x69;
        }

        private static ushort ReduceRule_6A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6A, "_m79"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x6A;
        }

        private static ushort ReduceRule_6A_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6A, "_m79"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x6A;
        }

        private static ushort ReduceRule_6B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6B, "_m87"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x6B;
        }

        private static ushort ReduceRule_6B_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6B, "_m87"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x6B;
        }

        private static ushort ReduceRule_6C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6C, "_m91"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x6C;
        }

        private static ushort ReduceRule_6C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6C, "_m91"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x6C;
        }

        private static ushort ReduceRule_6D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "_m95"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x6D;
        }

        private static ushort ReduceRule_6D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6D, "_m95"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x6D;
        }

        private static ushort ReduceRule_6E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6E, "_m99"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x6E;
        }

        private static ushort ReduceRule_6E_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6E, "_m99"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x6E;
        }

        private static ushort ReduceRule_6F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x6F, "grammar_cf_rules<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x6F;
        }

        private static ushort ReduceRule_70_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x70, "cf_rule_simple<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x70;
        }

        private static ushort ReduceRule_71_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x71, "rule_definition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x71;
        }

        private static ushort ReduceRule_72_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x72, "rule_def_restrict<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x72;
        }

        private static ushort ReduceRule_73_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x73, "rule_def_fragment<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x73;
        }

        private static ushort ReduceRule_74_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x74;
        }

        private static ushort ReduceRule_74_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x74;
        }

        private static ushort ReduceRule_74_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x74;
        }

        private static ushort ReduceRule_74_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x74, "rule_def_repetition<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x74;
        }

        private static ushort ReduceRule_75_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x75, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x75;
        }

        private static ushort ReduceRule_75_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x75, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x75;
        }

        private static ushort ReduceRule_75_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x75, "rule_def_tree_action<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x75;
        }

        private static ushort ReduceRule_76_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x76, "rule_def_element<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x76;
        }

        private static ushort ReduceRule_76_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x76, "rule_def_element<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x76;
        }

        private static ushort ReduceRule_77_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x77;
        }

        private static ushort ReduceRule_77_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x77;
        }

        private static ushort ReduceRule_77_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x77;
        }

        private static ushort ReduceRule_77_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x77;
        }

        private static ushort ReduceRule_77_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x77, "rule_def_atom<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x77;
        }

        private static ushort ReduceRule_78_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x78, "rule_sym_ref_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x78;
        }

        private static ushort ReduceRule_79_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x79, "rule_sym_ref_params<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x79;
        }

        private static ushort ReduceRule_7A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7A, "_m119"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x7A;
        }

        private static ushort ReduceRule_7A_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7A, "_m119"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x7A;
        }

        private static ushort ReduceRule_7B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7B, "_m128"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x7B;
        }

        private static ushort ReduceRule_7B_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7B, "_m128"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x7B;
        }

        private static ushort ReduceRule_7C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "_m130"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x7C;
        }

        private static ushort ReduceRule_7C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7C, "_m130"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x7C;
        }

        private static ushort ReduceRule_7D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7D, "_m132"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x7D;
        }

        private static ushort ReduceRule_7D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7D, "_m132"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x7D;
        }

        private static ushort ReduceRule_7E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 5, 5);
            Nodes.RemoveRange(Nodes.Count - 5, 5);
            for (int i = 0; i != 5; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7E, "cf_rule_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x7E;
        }

        private static ushort ReduceRule_7F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "_m137"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x7F;
        }

        private static ushort ReduceRule_7F_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "_m137"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x7F;
        }

        private static ushort ReduceRule_7F_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x7F, "_m137"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x7F;
        }

        private static ushort ReduceRule_80_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x80, "grammar_cf_rules<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x80;
        }

        private static ushort ReduceRule_81_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x81, "cf_rule_simple<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x81;
        }

        private static ushort ReduceRule_82_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x82, "rule_definition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x82;
        }

        private static ushort ReduceRule_83_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x83, "rule_def_restrict<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x83;
        }

        private static ushort ReduceRule_84_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x84, "rule_def_fragment<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x84;
        }

        private static ushort ReduceRule_85_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x85;
        }

        private static ushort ReduceRule_85_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x85;
        }

        private static ushort ReduceRule_85_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x85;
        }

        private static ushort ReduceRule_85_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x85, "rule_def_repetition<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x85;
        }

        private static ushort ReduceRule_86_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x86, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x86;
        }

        private static ushort ReduceRule_86_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x86, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x86;
        }

        private static ushort ReduceRule_86_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x86, "rule_def_tree_action<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x86;
        }

        private static ushort ReduceRule_87_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x87, "rule_def_element<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x87;
        }

        private static ushort ReduceRule_87_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x87, "rule_def_element<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x87;
        }

        private static ushort ReduceRule_88_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x88;
        }

        private static ushort ReduceRule_88_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x88;
        }

        private static ushort ReduceRule_88_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x88;
        }

        private static ushort ReduceRule_88_3(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x88;
        }

        private static ushort ReduceRule_88_4(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x88, "rule_def_atom<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0x88;
        }

        private static ushort ReduceRule_89_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x89, "rule_sym_ref_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x89;
        }

        private static ushort ReduceRule_8A_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8A, "rule_sym_ref_params<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x8A;
        }

        private static ushort ReduceRule_8B_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8B, "_m159"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x8B;
        }

        private static ushort ReduceRule_8B_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8B, "_m159"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x8B;
        }

        private static ushort ReduceRule_8C_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8C, "_m168"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVirtual("concat"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x8C;
        }

        private static ushort ReduceRule_8C_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8C, "_m168"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x8C;
        }

        private static ushort ReduceRule_8D_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "_m170"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x8D;
        }

        private static ushort ReduceRule_8D_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8D, "_m170"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x8D;
        }

        private static ushort ReduceRule_8E_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8E, "_m172"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            Nodes.Add(SubRoot);
            return 0x8E;
        }

        private static ushort ReduceRule_8E_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8E, "_m172"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x8E;
        }

        private static ushort ReduceRule_8F_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 5, 5);
            Nodes.RemoveRange(Nodes.Count - 5, 5);
            for (int i = 0; i != 5; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x8F, "cf_rule_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0x8F;
        }

        private static ushort ReduceRule_90_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "_m177"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x90;
        }

        private static ushort ReduceRule_90_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "_m177"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0x90;
        }

        private static ushort ReduceRule_90_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0x90, "_m177"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0x90;
        }

        private static ushort ReduceRule_C8_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 9, 9);
            Nodes.RemoveRange(Nodes.Count - 9, 9);
            for (int i = 0; i != 9; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xC8, "cs_grammar_text"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7]);

            SubRoot.AppendChild(Definition[8], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xC8;
        }

        private static ushort ReduceRule_C9_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 8, 8);
            Nodes.RemoveRange(Nodes.Count - 8, 8);
            for (int i = 0; i != 8; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xC9, "cs_grammar_bin"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6]);

            SubRoot.AppendChild(Definition[7], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xC9;
        }

        private static ushort ReduceRule_CA_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCA, "grammar_cs_rules<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xCA;
        }

        private static ushort ReduceRule_CB_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 6, 6);
            Nodes.RemoveRange(Nodes.Count - 6, 6);
            for (int i = 0; i != 6; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCB, "cs_rule_simple<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xCB;
        }

        private static ushort ReduceRule_CC_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCC, "cs_rule_context<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xCC;
        }

        private static ushort ReduceRule_CC_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCC, "cs_rule_context<grammar_text_terminal>"));

            Nodes.Add(SubRoot);
            return 0xCC;
        }

        private static ushort ReduceRule_CD_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 7, 7);
            Nodes.RemoveRange(Nodes.Count - 7, 7);
            for (int i = 0; i != 7; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCD, "cs_rule_template<grammar_text_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xCD;
        }

        private static ushort ReduceRule_CE_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCE, "_m148"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xCE;
        }

        private static ushort ReduceRule_CE_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCE, "_m148"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xCE;
        }

        private static ushort ReduceRule_CE_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCE, "_m148"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0xCE;
        }

        private static ushort ReduceRule_CF_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 4, 4);
            Nodes.RemoveRange(Nodes.Count - 4, 4);
            for (int i = 0; i != 4; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xCF, "grammar_cs_rules<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xCF;
        }

        private static ushort ReduceRule_D0_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 6, 6);
            Nodes.RemoveRange(Nodes.Count - 6, 6);
            for (int i = 0; i != 6; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD0, "cs_rule_simple<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[4]);

            SubRoot.AppendChild(Definition[5], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xD0;
        }

        private static ushort ReduceRule_D1_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 3, 3);
            Nodes.RemoveRange(Nodes.Count - 3, 3);
            for (int i = 0; i != 3; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD1, "cs_rule_context<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xD1;
        }

        private static ushort ReduceRule_D1_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD1, "cs_rule_context<grammar_bin_terminal>"));

            Nodes.Add(SubRoot);
            return 0xD1;
        }

        private static ushort ReduceRule_D2_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 7, 7);
            Nodes.RemoveRange(Nodes.Count - 7, 7);
            for (int i = 0; i != 7; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD2, "cs_rule_template<grammar_bin_terminal>"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            SubRoot.AppendChild(Definition[2]);

            SubRoot.AppendChild(Definition[3]);

            SubRoot.AppendChild(Definition[4], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            SubRoot.AppendChild(Definition[5]);

            SubRoot.AppendChild(Definition[6], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xD2;
        }

        private static ushort ReduceRule_D3_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD3, "_m166"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xD3;
        }

        private static ushort ReduceRule_D3_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD3, "_m166"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xD3;
        }

        private static ushort ReduceRule_D3_2(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD3, "_m166"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0xD3;
        }

        private static ushort ReduceRule_D4_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 1, 1);
            Nodes.RemoveRange(Nodes.Count - 1, 1);
            for (int i = 0; i != 1; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD4, "file_item"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            Nodes.Add(SubRoot);
            return 0xD4;
        }

        private static ushort ReduceRule_D5_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD5, "file"));

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xD5;
        }

        private static ushort ReduceRule_D6_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD6, "_m214"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            SubRoot.AppendChild(Definition[0]);

            SubRoot.AppendChild(Definition[1]);

            Nodes.Add(SubRoot);
            return 0xD6;
        }

        private static ushort ReduceRule_D6_1(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD6, "_m214"), Hime.Kernel.Parsers.SyntaxTreeNodeAction.Replace);

            Nodes.Add(SubRoot);
            return 0xD6;
        }

        private static ushort ReduceRule_D7_0(Hime.Kernel.Parsers.SyntaxTreeNodeCollection Nodes, System.Collections.Generic.Stack<ushort> Stack)
        {

            System.Collections.Generic.List<Hime.Kernel.Parsers.SyntaxTreeNode> Definition = Nodes.GetRange(Nodes.Count - 2, 2);
            Nodes.RemoveRange(Nodes.Count - 2, 2);
            for (int i = 0; i != 2; i++)
                Stack.Pop();

            Hime.Kernel.Parsers.SyntaxTreeNode SubRoot = new Hime.Kernel.Parsers.SyntaxTreeNode(new Hime.Kernel.Parsers.SymbolVariable(0xD7, "_Axiom_"));

            SubRoot.AppendChild(Definition[0], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Promote);

            SubRoot.AppendChild(Definition[1], Hime.Kernel.Parsers.SyntaxTreeNodeAction.Drop);

            Nodes.Add(SubRoot);
            return 0xD7;
        }

        private static Production[] p_Rules = { ReduceRule_11_0, ReduceRule_12_0, ReduceRule_13_0, ReduceRule_14_0, ReduceRule_15_0, ReduceRule_16_0, ReduceRule_16_1, ReduceRule_16_2, ReduceRule_16_3, ReduceRule_16_4, ReduceRule_17_0, ReduceRule_18_0, ReduceRule_19_0, ReduceRule_19_1, ReduceRule_1A_0, ReduceRule_1A_1, ReduceRule_4F_0, ReduceRule_50_0, ReduceRule_50_1, ReduceRule_51_0, ReduceRule_52_0, ReduceRule_53_0, ReduceRule_54_0, ReduceRule_54_1, ReduceRule_54_2, ReduceRule_54_3, ReduceRule_54_4, ReduceRule_55_0, ReduceRule_55_1, ReduceRule_56_0, ReduceRule_56_1, ReduceRule_56_2, ReduceRule_56_3, ReduceRule_57_0, ReduceRule_58_0, ReduceRule_59_0, ReduceRule_5A_0, ReduceRule_5A_1, ReduceRule_5B_0, ReduceRule_5C_0, ReduceRule_5D_0, ReduceRule_5E_0, ReduceRule_5F_0, ReduceRule_60_0, ReduceRule_60_1, ReduceRule_60_2, ReduceRule_60_3, ReduceRule_60_4, ReduceRule_60_5, ReduceRule_60_6, ReduceRule_60_7, ReduceRule_60_8, ReduceRule_60_9, ReduceRule_60_A, ReduceRule_60_B, ReduceRule_61_0, ReduceRule_62_0, ReduceRule_63_0, ReduceRule_64_0, ReduceRule_64_1, ReduceRule_65_0, ReduceRule_65_1, ReduceRule_65_2, ReduceRule_65_3, ReduceRule_66_0, ReduceRule_67_0, ReduceRule_68_0, ReduceRule_68_1, ReduceRule_69_0, ReduceRule_69_1, ReduceRule_6A_0, ReduceRule_6A_1, ReduceRule_6B_0, ReduceRule_6B_1, ReduceRule_6C_0, ReduceRule_6C_1, ReduceRule_6D_0, ReduceRule_6D_1, ReduceRule_6E_0, ReduceRule_6E_1, ReduceRule_6F_0, ReduceRule_70_0, ReduceRule_71_0, ReduceRule_72_0, ReduceRule_73_0, ReduceRule_74_0, ReduceRule_74_1, ReduceRule_74_2, ReduceRule_74_3, ReduceRule_75_0, ReduceRule_75_1, ReduceRule_75_2, ReduceRule_76_0, ReduceRule_76_1, ReduceRule_77_0, ReduceRule_77_1, ReduceRule_77_2, ReduceRule_77_3, ReduceRule_77_4, ReduceRule_78_0, ReduceRule_79_0, ReduceRule_7A_0, ReduceRule_7A_1, ReduceRule_7B_0, ReduceRule_7B_1, ReduceRule_7C_0, ReduceRule_7C_1, ReduceRule_7D_0, ReduceRule_7D_1, ReduceRule_7E_0, ReduceRule_7F_0, ReduceRule_7F_1, ReduceRule_7F_2, ReduceRule_80_0, ReduceRule_81_0, ReduceRule_82_0, ReduceRule_83_0, ReduceRule_84_0, ReduceRule_85_0, ReduceRule_85_1, ReduceRule_85_2, ReduceRule_85_3, ReduceRule_86_0, ReduceRule_86_1, ReduceRule_86_2, ReduceRule_87_0, ReduceRule_87_1, ReduceRule_88_0, ReduceRule_88_1, ReduceRule_88_2, ReduceRule_88_3, ReduceRule_88_4, ReduceRule_89_0, ReduceRule_8A_0, ReduceRule_8B_0, ReduceRule_8B_1, ReduceRule_8C_0, ReduceRule_8C_1, ReduceRule_8D_0, ReduceRule_8D_1, ReduceRule_8E_0, ReduceRule_8E_1, ReduceRule_8F_0, ReduceRule_90_0, ReduceRule_90_1, ReduceRule_90_2, ReduceRule_C8_0, ReduceRule_C9_0, ReduceRule_CA_0, ReduceRule_CB_0, ReduceRule_CC_0, ReduceRule_CC_1, ReduceRule_CD_0, ReduceRule_CE_0, ReduceRule_CE_1, ReduceRule_CE_2, ReduceRule_CF_0, ReduceRule_D0_0, ReduceRule_D1_0, ReduceRule_D1_1, ReduceRule_D2_0, ReduceRule_D3_0, ReduceRule_D3_1, ReduceRule_D3_2, ReduceRule_D4_0, ReduceRule_D5_0, ReduceRule_D6_0, ReduceRule_D6_1, ReduceRule_D7_0 };

        private static ushort[] p_ExpectedIDs_0 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD };
        private static string[] p_ExpectedNames_0 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_Items_0 = { "[_Axiom_ -> . file $]", "[file -> . file_item _m214]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_ShiftsOnTerminal_0 = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0xB, 0x11 }, new ushort[2] { 0xC, 0x12 }, new ushort[2] { 0xD, 0x13 } };
        private static ushort[][] p_ShiftsOnVariable_0 = { new ushort[2] { 0xD5, 0x1 }, new ushort[2] { 0xD4, 0x2 }, new ushort[2] { 0x16, 0x3 }, new ushort[2] { 0x18, 0x4 }, new ushort[2] { 0x66, 0x5 }, new ushort[2] { 0x67, 0x6 }, new ushort[2] { 0xC8, 0x7 }, new ushort[2] { 0xC9, 0x8 }, new ushort[2] { 0x65, 0xA }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x13, 0xD }, new ushort[2] { 0x14, 0xE }, new ushort[2] { 0x15, 0xF } };
        private static ushort[][] p_ReducsOnTerminal_0 = { };

        private static ushort[] p_ExpectedIDs_1 = { 0x2 };
        private static string[] p_ExpectedNames_1 = { "$" };
        private static string[] p_Items_1 = { "[_Axiom_ -> file . $]" };
        private static ushort[][] p_ShiftsOnTerminal_1 = { new ushort[2] { 0x2, 0x14 } };
        private static ushort[][] p_ShiftsOnVariable_1 = { };
        private static ushort[][] p_ReducsOnTerminal_1 = { };

        private static ushort[] p_ExpectedIDs_2 = { 0x2, 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD };
        private static string[] p_ExpectedNames_2 = { "$", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_Items_2 = { "[file -> file_item . _m214]", "[_m214 -> . file_item _m214]", "[_m214 -> . ]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_ShiftsOnTerminal_2 = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0xB, 0x11 }, new ushort[2] { 0xC, 0x12 }, new ushort[2] { 0xD, 0x13 } };
        private static ushort[][] p_ShiftsOnVariable_2 = { new ushort[2] { 0xD6, 0x15 }, new ushort[2] { 0xD4, 0x16 }, new ushort[2] { 0x16, 0x3 }, new ushort[2] { 0x18, 0x4 }, new ushort[2] { 0x66, 0x5 }, new ushort[2] { 0x67, 0x6 }, new ushort[2] { 0xC8, 0x7 }, new ushort[2] { 0xC9, 0x8 }, new ushort[2] { 0x65, 0xA }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x13, 0xD }, new ushort[2] { 0x14, 0xE }, new ushort[2] { 0x15, 0xF } };
        private static ushort[][] p_ReducsOnTerminal_2 = { new ushort[2] { 0x2, 0xA7 } };

        private static ushort[] p_ExpectedIDs_3 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2 };
        private static string[] p_ExpectedNames_3 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$" };
        private static string[] p_Items_3 = { "[file_item -> Namespace_child_symbol . ]" };
        private static ushort[][] p_ShiftsOnTerminal_3 = { };
        private static ushort[][] p_ShiftsOnVariable_3 = { };
        private static ushort[][] p_ReducsOnTerminal_3 = { new ushort[2] { 0xE, 0xA4 }, new ushort[2] { 0x4A, 0xA4 }, new ushort[2] { 0xA, 0xA4 }, new ushort[2] { 0xB, 0xA4 }, new ushort[2] { 0xC, 0xA4 }, new ushort[2] { 0xD, 0xA4 }, new ushort[2] { 0x2, 0xA4 } };

        private static ushort[] p_ExpectedIDs_4 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_4 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_4 = { "[Namespace_child_symbol -> Namespace . ]" };
        private static ushort[][] p_ShiftsOnTerminal_4 = { };
        private static ushort[][] p_ShiftsOnVariable_4 = { };
        private static ushort[][] p_ReducsOnTerminal_4 = { new ushort[2] { 0xE, 0x5 }, new ushort[2] { 0x4A, 0x5 }, new ushort[2] { 0xA, 0x5 }, new ushort[2] { 0xB, 0x5 }, new ushort[2] { 0xC, 0x5 }, new ushort[2] { 0xD, 0x5 }, new ushort[2] { 0x2, 0x5 }, new ushort[2] { 0x10, 0x5 } };

        private static ushort[] p_ExpectedIDs_5 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_5 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_5 = { "[Namespace_child_symbol -> cf_grammar_text . ]" };
        private static ushort[][] p_ShiftsOnTerminal_5 = { };
        private static ushort[][] p_ShiftsOnVariable_5 = { };
        private static ushort[][] p_ReducsOnTerminal_5 = { new ushort[2] { 0xE, 0x6 }, new ushort[2] { 0x4A, 0x6 }, new ushort[2] { 0xA, 0x6 }, new ushort[2] { 0xB, 0x6 }, new ushort[2] { 0xC, 0x6 }, new ushort[2] { 0xD, 0x6 }, new ushort[2] { 0x2, 0x6 }, new ushort[2] { 0x10, 0x6 } };

        private static ushort[] p_ExpectedIDs_6 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_6 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_6 = { "[Namespace_child_symbol -> cf_grammar_bin . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6 = { };
        private static ushort[][] p_ShiftsOnVariable_6 = { };
        private static ushort[][] p_ReducsOnTerminal_6 = { new ushort[2] { 0xE, 0x7 }, new ushort[2] { 0x4A, 0x7 }, new ushort[2] { 0xA, 0x7 }, new ushort[2] { 0xB, 0x7 }, new ushort[2] { 0xC, 0x7 }, new ushort[2] { 0xD, 0x7 }, new ushort[2] { 0x2, 0x7 }, new ushort[2] { 0x10, 0x7 } };

        private static ushort[] p_ExpectedIDs_7 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_7 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_7 = { "[Namespace_child_symbol -> cs_grammar_text . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7 = { };
        private static ushort[][] p_ShiftsOnVariable_7 = { };
        private static ushort[][] p_ReducsOnTerminal_7 = { new ushort[2] { 0xE, 0x8 }, new ushort[2] { 0x4A, 0x8 }, new ushort[2] { 0xA, 0x8 }, new ushort[2] { 0xB, 0x8 }, new ushort[2] { 0xC, 0x8 }, new ushort[2] { 0xD, 0x8 }, new ushort[2] { 0x2, 0x8 }, new ushort[2] { 0x10, 0x8 } };

        private static ushort[] p_ExpectedIDs_8 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_8 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_8 = { "[Namespace_child_symbol -> cs_grammar_bin . ]" };
        private static ushort[][] p_ShiftsOnTerminal_8 = { };
        private static ushort[][] p_ShiftsOnVariable_8 = { };
        private static ushort[][] p_ReducsOnTerminal_8 = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0x9 }, new ushort[2] { 0xA, 0x9 }, new ushort[2] { 0xB, 0x9 }, new ushort[2] { 0xC, 0x9 }, new ushort[2] { 0xD, 0x9 }, new ushort[2] { 0x2, 0x9 }, new ushort[2] { 0x10, 0x9 } };

        private static ushort[] p_ExpectedIDs_9 = { 0x8 };
        private static string[] p_ExpectedNames_9 = { "NAME" };
        private static string[] p_Items_9 = { "[Namespace -> namespace . qualified_name { Namespace_content }]", "[qualified_name -> . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_9 = { new ushort[2] { 0x8, 0x18 } };
        private static ushort[][] p_ShiftsOnVariable_9 = { new ushort[2] { 0x11, 0x17 } };
        private static ushort[][] p_ReducsOnTerminal_9 = { };

        private static ushort[] p_ExpectedIDs_A = { 0x4A };
        private static string[] p_ExpectedNames_A = { "_T[grammar]" };
        private static string[] p_Items_A = { "[cf_grammar_text -> grammar_access . grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access . grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_A = { new ushort[2] { 0x4A, 0x19 } };
        private static ushort[][] p_ShiftsOnVariable_A = { };
        private static ushort[][] p_ReducsOnTerminal_A = { };

        private static ushort[] p_ExpectedIDs_B = { 0xC5 };
        private static string[] p_ExpectedNames_B = { "_T[cs]" };
        private static string[] p_Items_B = { "[cs_grammar_text -> grammar . cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar . cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_B = { new ushort[2] { 0xC5, 0x1A } };
        private static ushort[][] p_ShiftsOnVariable_B = { };
        private static ushort[][] p_ReducsOnTerminal_B = { };

        private static ushort[] p_ExpectedIDs_C = { 0x4A };
        private static string[] p_ExpectedNames_C = { "_T[grammar]" };
        private static string[] p_Items_C = { "[grammar_access -> symbol_access_public . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C = { };
        private static ushort[][] p_ShiftsOnVariable_C = { };
        private static ushort[][] p_ReducsOnTerminal_C = { new ushort[2] { 0x4A, 0x3C } };

        private static ushort[] p_ExpectedIDs_D = { 0x4A };
        private static string[] p_ExpectedNames_D = { "_T[grammar]" };
        private static string[] p_Items_D = { "[grammar_access -> symbol_access_private . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D = { };
        private static ushort[][] p_ShiftsOnVariable_D = { };
        private static ushort[][] p_ReducsOnTerminal_D = { new ushort[2] { 0x4A, 0x3D } };

        private static ushort[] p_ExpectedIDs_E = { 0x4A };
        private static string[] p_ExpectedNames_E = { "_T[grammar]" };
        private static string[] p_Items_E = { "[grammar_access -> symbol_access_protected . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E = { };
        private static ushort[][] p_ShiftsOnVariable_E = { };
        private static ushort[][] p_ReducsOnTerminal_E = { new ushort[2] { 0x4A, 0x3E } };

        private static ushort[] p_ExpectedIDs_F = { 0x4A };
        private static string[] p_ExpectedNames_F = { "_T[grammar]" };
        private static string[] p_Items_F = { "[grammar_access -> symbol_access_internal . ]" };
        private static ushort[][] p_ShiftsOnTerminal_F = { };
        private static ushort[][] p_ShiftsOnVariable_F = { };
        private static ushort[][] p_ReducsOnTerminal_F = { new ushort[2] { 0x4A, 0x3F } };

        private static ushort[] p_ExpectedIDs_10 = { 0x4A };
        private static string[] p_ExpectedNames_10 = { "_T[grammar]" };
        private static string[] p_Items_10 = { "[symbol_access_public -> public . ]" };
        private static ushort[][] p_ShiftsOnTerminal_10 = { };
        private static ushort[][] p_ShiftsOnVariable_10 = { };
        private static ushort[][] p_ReducsOnTerminal_10 = { new ushort[2] { 0x4A, 0x1 } };

        private static ushort[] p_ExpectedIDs_11 = { 0x4A };
        private static string[] p_ExpectedNames_11 = { "_T[grammar]" };
        private static string[] p_Items_11 = { "[symbol_access_private -> private . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11 = { };
        private static ushort[][] p_ShiftsOnVariable_11 = { };
        private static ushort[][] p_ReducsOnTerminal_11 = { new ushort[2] { 0x4A, 0x2 } };

        private static ushort[] p_ExpectedIDs_12 = { 0x4A };
        private static string[] p_ExpectedNames_12 = { "_T[grammar]" };
        private static string[] p_Items_12 = { "[symbol_access_protected -> protected . ]" };
        private static ushort[][] p_ShiftsOnTerminal_12 = { };
        private static ushort[][] p_ShiftsOnVariable_12 = { };
        private static ushort[][] p_ReducsOnTerminal_12 = { new ushort[2] { 0x4A, 0x3 } };

        private static ushort[] p_ExpectedIDs_13 = { 0x4A };
        private static string[] p_ExpectedNames_13 = { "_T[grammar]" };
        private static string[] p_Items_13 = { "[symbol_access_internal -> internal . ]" };
        private static ushort[][] p_ShiftsOnTerminal_13 = { };
        private static ushort[][] p_ShiftsOnVariable_13 = { };
        private static ushort[][] p_ReducsOnTerminal_13 = { new ushort[2] { 0x4A, 0x4 } };

        private static ushort[] p_ExpectedIDs_14 = { 0x1 };
        private static string[] p_ExpectedNames_14 = { "ε" };
        private static string[] p_Items_14 = { "[_Axiom_ -> file $ . ]" };
        private static ushort[][] p_ShiftsOnTerminal_14 = { };
        private static ushort[][] p_ShiftsOnVariable_14 = { };
        private static ushort[][] p_ReducsOnTerminal_14 = { new ushort[2] { 0x1, 0xA8 } };

        private static ushort[] p_ExpectedIDs_15 = { 0x2 };
        private static string[] p_ExpectedNames_15 = { "$" };
        private static string[] p_Items_15 = { "[file -> file_item _m214 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_15 = { };
        private static ushort[][] p_ShiftsOnVariable_15 = { };
        private static ushort[][] p_ReducsOnTerminal_15 = { new ushort[2] { 0x2, 0xA5 } };

        private static ushort[] p_ExpectedIDs_16 = { 0x2, 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD };
        private static string[] p_ExpectedNames_16 = { "$", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_Items_16 = { "[_m214 -> file_item . _m214]", "[_m214 -> . file_item _m214]", "[_m214 -> . ]", "[file_item -> . Namespace_child_symbol]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_ShiftsOnTerminal_16 = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0xB, 0x11 }, new ushort[2] { 0xC, 0x12 }, new ushort[2] { 0xD, 0x13 } };
        private static ushort[][] p_ShiftsOnVariable_16 = { new ushort[2] { 0xD6, 0x1B }, new ushort[2] { 0xD4, 0x16 }, new ushort[2] { 0x16, 0x3 }, new ushort[2] { 0x18, 0x4 }, new ushort[2] { 0x66, 0x5 }, new ushort[2] { 0x67, 0x6 }, new ushort[2] { 0xC8, 0x7 }, new ushort[2] { 0xC9, 0x8 }, new ushort[2] { 0x65, 0xA }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x13, 0xD }, new ushort[2] { 0x14, 0xE }, new ushort[2] { 0x15, 0xF } };
        private static ushort[][] p_ReducsOnTerminal_16 = { new ushort[2] { 0x2, 0xA7 } };

        private static ushort[] p_ExpectedIDs_17 = { 0xF };
        private static string[] p_ExpectedNames_17 = { "_T[{]" };
        private static string[] p_Items_17 = { "[Namespace -> namespace qualified_name . { Namespace_content }]" };
        private static ushort[][] p_ShiftsOnTerminal_17 = { new ushort[2] { 0xF, 0x1C } };
        private static ushort[][] p_ShiftsOnVariable_17 = { };
        private static ushort[][] p_ReducsOnTerminal_17 = { };

        private static ushort[] p_ExpectedIDs_18 = { 0xF, 0x45, 0x10, 0x39, 0x9 };
        private static string[] p_ExpectedNames_18 = { "_T[{]", "_T[,]", "_T[}]", "_T[;]", "_T[.]" };
        private static string[] p_Items_18 = { "[qualified_name -> NAME . _m18]", "[_m18 -> . . NAME _m18]", "[_m18 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_18 = { new ushort[2] { 0x9, 0x1E } };
        private static ushort[][] p_ShiftsOnVariable_18 = { new ushort[2] { 0x19, 0x1D } };
        private static ushort[][] p_ReducsOnTerminal_18 = { new ushort[2] { 0xF, 0xD }, new ushort[2] { 0x45, 0xD }, new ushort[2] { 0x10, 0xD }, new ushort[2] { 0x39, 0xD } };

        private static ushort[] p_ExpectedIDs_19 = { 0x4B };
        private static string[] p_ExpectedNames_19 = { "_T[cf]" };
        private static string[] p_Items_19 = { "[cf_grammar_text -> grammar_access grammar . cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar . cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_19 = { new ushort[2] { 0x4B, 0x1F } };
        private static ushort[][] p_ShiftsOnVariable_19 = { };
        private static ushort[][] p_ReducsOnTerminal_19 = { };

        private static ushort[] p_ExpectedIDs_1A = { 0x8 };
        private static string[] p_ExpectedNames_1A = { "NAME" };
        private static string[] p_Items_1A = { "[cs_grammar_text -> grammar cs . NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs . NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_1A = { new ushort[2] { 0x8, 0x20 } };
        private static ushort[][] p_ShiftsOnVariable_1A = { };
        private static ushort[][] p_ReducsOnTerminal_1A = { };

        private static ushort[] p_ExpectedIDs_1B = { 0x2 };
        private static string[] p_ExpectedNames_1B = { "$" };
        private static string[] p_Items_1B = { "[_m214 -> file_item _m214 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_1B = { };
        private static ushort[][] p_ShiftsOnVariable_1B = { };
        private static ushort[][] p_ReducsOnTerminal_1B = { new ushort[2] { 0x2, 0xA6 } };

        private static ushort[] p_ExpectedIDs_1C = { 0x10, 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD };
        private static string[] p_ExpectedNames_1C = { "_T[}]", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_Items_1C = { "[Namespace -> namespace qualified_name { . Namespace_content }]", "[Namespace_content -> . _m23]", "[_m23 -> . Namespace_child_symbol _m23]", "[_m23 -> . ]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_ShiftsOnTerminal_1C = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0xB, 0x11 }, new ushort[2] { 0xC, 0x12 }, new ushort[2] { 0xD, 0x13 } };
        private static ushort[][] p_ShiftsOnVariable_1C = { new ushort[2] { 0x17, 0x21 }, new ushort[2] { 0x1A, 0x22 }, new ushort[2] { 0x16, 0x23 }, new ushort[2] { 0x18, 0x4 }, new ushort[2] { 0x66, 0x5 }, new ushort[2] { 0x67, 0x6 }, new ushort[2] { 0xC8, 0x7 }, new ushort[2] { 0xC9, 0x8 }, new ushort[2] { 0x65, 0xA }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x13, 0xD }, new ushort[2] { 0x14, 0xE }, new ushort[2] { 0x15, 0xF } };
        private static ushort[][] p_ReducsOnTerminal_1C = { new ushort[2] { 0x10, 0xF } };

        private static ushort[] p_ExpectedIDs_1D = { 0xF, 0x45, 0x10, 0x39 };
        private static string[] p_ExpectedNames_1D = { "_T[{]", "_T[,]", "_T[}]", "_T[;]" };
        private static string[] p_Items_1D = { "[qualified_name -> NAME _m18 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_1D = { };
        private static ushort[][] p_ShiftsOnVariable_1D = { };
        private static ushort[][] p_ReducsOnTerminal_1D = { new ushort[2] { 0xF, 0x0 }, new ushort[2] { 0x45, 0x0 }, new ushort[2] { 0x10, 0x0 }, new ushort[2] { 0x39, 0x0 } };

        private static ushort[] p_ExpectedIDs_1E = { 0x8 };
        private static string[] p_ExpectedNames_1E = { "NAME" };
        private static string[] p_Items_1E = { "[_m18 -> . . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_1E = { new ushort[2] { 0x8, 0x24 } };
        private static ushort[][] p_ShiftsOnVariable_1E = { };
        private static ushort[][] p_ReducsOnTerminal_1E = { };

        private static ushort[] p_ExpectedIDs_1F = { 0x8 };
        private static string[] p_ExpectedNames_1F = { "NAME" };
        private static string[] p_Items_1F = { "[cf_grammar_text -> grammar_access grammar cf . NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf . NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_1F = { new ushort[2] { 0x8, 0x25 } };
        private static ushort[][] p_ShiftsOnVariable_1F = { };
        private static ushort[][] p_ReducsOnTerminal_1F = { };

        private static ushort[] p_ExpectedIDs_20 = { 0xF, 0x49 };
        private static string[] p_ExpectedNames_20 = { "_T[{]", "_T[:]" };
        private static string[] p_Items_20 = { "[cs_grammar_text -> grammar cs NAME . grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME . grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_parency -> . : qualified_name _m99]", "[grammar_parency -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_20 = { new ushort[2] { 0x49, 0x27 } };
        private static ushort[][] p_ShiftsOnVariable_20 = { new ushort[2] { 0x64, 0x26 } };
        private static ushort[][] p_ReducsOnTerminal_20 = { new ushort[2] { 0xF, 0x3B } };

        private static ushort[] p_ExpectedIDs_21 = { 0x10 };
        private static string[] p_ExpectedNames_21 = { "_T[}]" };
        private static string[] p_Items_21 = { "[Namespace -> namespace qualified_name { Namespace_content . }]" };
        private static ushort[][] p_ShiftsOnTerminal_21 = { new ushort[2] { 0x10, 0x28 } };
        private static ushort[][] p_ShiftsOnVariable_21 = { };
        private static ushort[][] p_ReducsOnTerminal_21 = { };

        private static ushort[] p_ExpectedIDs_22 = { 0x10 };
        private static string[] p_ExpectedNames_22 = { "_T[}]" };
        private static string[] p_Items_22 = { "[Namespace_content -> _m23 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_22 = { };
        private static ushort[][] p_ShiftsOnVariable_22 = { };
        private static ushort[][] p_ReducsOnTerminal_22 = { new ushort[2] { 0x10, 0xA } };

        private static ushort[] p_ExpectedIDs_23 = { 0x10, 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD };
        private static string[] p_ExpectedNames_23 = { "_T[}]", "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]" };
        private static string[] p_Items_23 = { "[_m23 -> Namespace_child_symbol . _m23]", "[_m23 -> . Namespace_child_symbol _m23]", "[_m23 -> . ]", "[Namespace_child_symbol -> . Namespace]", "[Namespace_child_symbol -> . cf_grammar_text]", "[Namespace_child_symbol -> . cf_grammar_bin]", "[Namespace_child_symbol -> . cs_grammar_text]", "[Namespace_child_symbol -> . cs_grammar_bin]", "[Namespace -> . namespace qualified_name { Namespace_content }]", "[cf_grammar_text -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> . grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[cs_grammar_text -> . grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> . grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_access -> . symbol_access_public]", "[grammar_access -> . symbol_access_private]", "[grammar_access -> . symbol_access_protected]", "[grammar_access -> . symbol_access_internal]", "[symbol_access_public -> . public]", "[symbol_access_private -> . private]", "[symbol_access_protected -> . protected]", "[symbol_access_internal -> . internal]" };
        private static ushort[][] p_ShiftsOnTerminal_23 = { new ushort[2] { 0xE, 0x9 }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0x10 }, new ushort[2] { 0xB, 0x11 }, new ushort[2] { 0xC, 0x12 }, new ushort[2] { 0xD, 0x13 } };
        private static ushort[][] p_ShiftsOnVariable_23 = { new ushort[2] { 0x1A, 0x29 }, new ushort[2] { 0x16, 0x23 }, new ushort[2] { 0x18, 0x4 }, new ushort[2] { 0x66, 0x5 }, new ushort[2] { 0x67, 0x6 }, new ushort[2] { 0xC8, 0x7 }, new ushort[2] { 0xC9, 0x8 }, new ushort[2] { 0x65, 0xA }, new ushort[2] { 0x12, 0xC }, new ushort[2] { 0x13, 0xD }, new ushort[2] { 0x14, 0xE }, new ushort[2] { 0x15, 0xF } };
        private static ushort[][] p_ReducsOnTerminal_23 = { new ushort[2] { 0x10, 0xF } };

        private static ushort[] p_ExpectedIDs_24 = { 0xF, 0x45, 0x10, 0x39, 0x9 };
        private static string[] p_ExpectedNames_24 = { "_T[{]", "_T[,]", "_T[}]", "_T[;]", "_T[.]" };
        private static string[] p_Items_24 = { "[_m18 -> . NAME . _m18]", "[_m18 -> . . NAME _m18]", "[_m18 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_24 = { new ushort[2] { 0x9, 0x1E } };
        private static ushort[][] p_ShiftsOnVariable_24 = { new ushort[2] { 0x19, 0x2A } };
        private static ushort[][] p_ReducsOnTerminal_24 = { new ushort[2] { 0xF, 0xD }, new ushort[2] { 0x45, 0xD }, new ushort[2] { 0x10, 0xD }, new ushort[2] { 0x39, 0xD } };

        private static ushort[] p_ExpectedIDs_25 = { 0xF, 0x49 };
        private static string[] p_ExpectedNames_25 = { "_T[{]", "_T[:]" };
        private static string[] p_Items_25 = { "[cf_grammar_text -> grammar_access grammar cf NAME . grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME . grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_parency -> . : qualified_name _m99]", "[grammar_parency -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_25 = { new ushort[2] { 0x49, 0x27 } };
        private static ushort[][] p_ShiftsOnVariable_25 = { new ushort[2] { 0x64, 0x2B } };
        private static ushort[][] p_ReducsOnTerminal_25 = { new ushort[2] { 0xF, 0x3B } };

        private static ushort[] p_ExpectedIDs_26 = { 0xF };
        private static string[] p_ExpectedNames_26 = { "_T[{]" };
        private static string[] p_Items_26 = { "[cs_grammar_text -> grammar cs NAME grammar_parency . { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency . { grammar_options grammar_cs_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_26 = { new ushort[2] { 0xF, 0x2C } };
        private static ushort[][] p_ShiftsOnVariable_26 = { };
        private static ushort[][] p_ReducsOnTerminal_26 = { };

        private static ushort[] p_ExpectedIDs_27 = { 0x8 };
        private static string[] p_ExpectedNames_27 = { "NAME" };
        private static string[] p_Items_27 = { "[grammar_parency -> : . qualified_name _m99]", "[qualified_name -> . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_27 = { new ushort[2] { 0x8, 0x18 } };
        private static ushort[][] p_ShiftsOnVariable_27 = { new ushort[2] { 0x11, 0x2D } };
        private static ushort[][] p_ReducsOnTerminal_27 = { };

        private static ushort[] p_ExpectedIDs_28 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_28 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_28 = { "[Namespace -> namespace qualified_name { Namespace_content } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_28 = { };
        private static ushort[][] p_ShiftsOnVariable_28 = { };
        private static ushort[][] p_ReducsOnTerminal_28 = { new ushort[2] { 0xE, 0xB }, new ushort[2] { 0x4A, 0xB }, new ushort[2] { 0xA, 0xB }, new ushort[2] { 0xB, 0xB }, new ushort[2] { 0xC, 0xB }, new ushort[2] { 0xD, 0xB }, new ushort[2] { 0x2, 0xB }, new ushort[2] { 0x10, 0xB } };

        private static ushort[] p_ExpectedIDs_29 = { 0x10 };
        private static string[] p_ExpectedNames_29 = { "_T[}]" };
        private static string[] p_Items_29 = { "[_m23 -> Namespace_child_symbol _m23 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_29 = { };
        private static ushort[][] p_ShiftsOnVariable_29 = { };
        private static ushort[][] p_ReducsOnTerminal_29 = { new ushort[2] { 0x10, 0xE } };

        private static ushort[] p_ExpectedIDs_2A = { 0xF, 0x45, 0x10, 0x39 };
        private static string[] p_ExpectedNames_2A = { "_T[{]", "_T[,]", "_T[}]", "_T[;]" };
        private static string[] p_Items_2A = { "[_m18 -> . NAME _m18 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_2A = { };
        private static ushort[][] p_ShiftsOnVariable_2A = { };
        private static ushort[][] p_ReducsOnTerminal_2A = { new ushort[2] { 0xF, 0xC }, new ushort[2] { 0x45, 0xC }, new ushort[2] { 0x10, 0xC }, new ushort[2] { 0x39, 0xC } };

        private static ushort[] p_ExpectedIDs_2B = { 0xF };
        private static string[] p_ExpectedNames_2B = { "_T[{]" };
        private static string[] p_Items_2B = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency . { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency . { grammar_options grammar_cf_rules<grammar_bin_terminal> }]" };
        private static ushort[][] p_ShiftsOnTerminal_2B = { new ushort[2] { 0xF, 0x2E } };
        private static ushort[][] p_ShiftsOnVariable_2B = { };
        private static ushort[][] p_ReducsOnTerminal_2B = { };

        private static ushort[] p_ExpectedIDs_2C = { 0x47 };
        private static string[] p_ExpectedNames_2C = { "_T[options]" };
        private static string[] p_Items_2C = { "[cs_grammar_text -> grammar cs NAME grammar_parency { . grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency { . grammar_options grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_options -> . options { _m91 }]" };
        private static ushort[][] p_ShiftsOnTerminal_2C = { new ushort[2] { 0x47, 0x30 } };
        private static ushort[][] p_ShiftsOnVariable_2C = { new ushort[2] { 0x62, 0x2F } };
        private static ushort[][] p_ReducsOnTerminal_2C = { };

        private static ushort[] p_ExpectedIDs_2D = { 0xF, 0x45 };
        private static string[] p_ExpectedNames_2D = { "_T[{]", "_T[,]" };
        private static string[] p_Items_2D = { "[grammar_parency -> : qualified_name . _m99]", "[_m99 -> . , qualified_name _m99]", "[_m99 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_2D = { new ushort[2] { 0x45, 0x32 } };
        private static ushort[][] p_ShiftsOnVariable_2D = { new ushort[2] { 0x6E, 0x31 } };
        private static ushort[][] p_ReducsOnTerminal_2D = { new ushort[2] { 0xF, 0x4F } };

        private static ushort[] p_ExpectedIDs_2E = { 0x47 };
        private static string[] p_ExpectedNames_2E = { "_T[options]" };
        private static string[] p_Items_2E = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { . grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { . grammar_options grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_options -> . options { _m91 }]" };
        private static ushort[][] p_ShiftsOnTerminal_2E = { new ushort[2] { 0x47, 0x30 } };
        private static ushort[][] p_ShiftsOnVariable_2E = { new ushort[2] { 0x62, 0x33 } };
        private static ushort[][] p_ReducsOnTerminal_2E = { };

        private static ushort[] p_ExpectedIDs_2F = { 0x48, 0x4C };
        private static string[] p_ExpectedNames_2F = { "_T[terminals]", "_T[rules]" };
        private static string[] p_Items_2F = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options . grammar_terminals grammar_cs_rules<grammar_text_terminal> }]", "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options . grammar_cs_rules<grammar_bin_terminal> }]", "[grammar_terminals -> . terminals { _m95 }]", "[grammar_cs_rules<grammar_bin_terminal> -> . rules { _m166 }]" };
        private static ushort[][] p_ShiftsOnTerminal_2F = { new ushort[2] { 0x48, 0x36 }, new ushort[2] { 0x4C, 0x37 } };
        private static ushort[][] p_ShiftsOnVariable_2F = { new ushort[2] { 0x63, 0x34 }, new ushort[2] { 0xCF, 0x35 } };
        private static ushort[][] p_ReducsOnTerminal_2F = { };

        private static ushort[] p_ExpectedIDs_30 = { 0xF };
        private static string[] p_ExpectedNames_30 = { "_T[{]" };
        private static string[] p_Items_30 = { "[grammar_options -> options . { _m91 }]" };
        private static ushort[][] p_ShiftsOnTerminal_30 = { new ushort[2] { 0xF, 0x38 } };
        private static ushort[][] p_ShiftsOnVariable_30 = { };
        private static ushort[][] p_ReducsOnTerminal_30 = { };

        private static ushort[] p_ExpectedIDs_31 = { 0xF };
        private static string[] p_ExpectedNames_31 = { "_T[{]" };
        private static string[] p_Items_31 = { "[grammar_parency -> : qualified_name _m99 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_31 = { };
        private static ushort[][] p_ShiftsOnVariable_31 = { };
        private static ushort[][] p_ReducsOnTerminal_31 = { new ushort[2] { 0xF, 0x3A } };

        private static ushort[] p_ExpectedIDs_32 = { 0x8 };
        private static string[] p_ExpectedNames_32 = { "NAME" };
        private static string[] p_Items_32 = { "[_m99 -> , . qualified_name _m99]", "[qualified_name -> . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_32 = { new ushort[2] { 0x8, 0x18 } };
        private static ushort[][] p_ShiftsOnVariable_32 = { new ushort[2] { 0x11, 0x39 } };
        private static ushort[][] p_ReducsOnTerminal_32 = { };

        private static ushort[] p_ExpectedIDs_33 = { 0x48, 0x4C };
        private static string[] p_ExpectedNames_33 = { "_T[terminals]", "_T[rules]" };
        private static string[] p_Items_33 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options . grammar_terminals grammar_cf_rules<grammar_text_terminal> }]", "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options . grammar_cf_rules<grammar_bin_terminal> }]", "[grammar_terminals -> . terminals { _m95 }]", "[grammar_cf_rules<grammar_bin_terminal> -> . rules { _m177 }]" };
        private static ushort[][] p_ShiftsOnTerminal_33 = { new ushort[2] { 0x48, 0x36 }, new ushort[2] { 0x4C, 0x3C } };
        private static ushort[][] p_ShiftsOnVariable_33 = { new ushort[2] { 0x63, 0x3A }, new ushort[2] { 0x80, 0x3B } };
        private static ushort[][] p_ReducsOnTerminal_33 = { };

        private static ushort[] p_ExpectedIDs_34 = { 0x4C };
        private static string[] p_ExpectedNames_34 = { "_T[rules]" };
        private static string[] p_Items_34 = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals . grammar_cs_rules<grammar_text_terminal> }]", "[grammar_cs_rules<grammar_text_terminal> -> . rules { _m148 }]" };
        private static ushort[][] p_ShiftsOnTerminal_34 = { new ushort[2] { 0x4C, 0x3E } };
        private static ushort[][] p_ShiftsOnVariable_34 = { new ushort[2] { 0xCA, 0x3D } };
        private static ushort[][] p_ReducsOnTerminal_34 = { };

        private static ushort[] p_ExpectedIDs_35 = { 0x10 };
        private static string[] p_ExpectedNames_35 = { "_T[}]" };
        private static string[] p_Items_35 = { "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> . }]" };
        private static ushort[][] p_ShiftsOnTerminal_35 = { new ushort[2] { 0x10, 0x3F } };
        private static ushort[][] p_ShiftsOnVariable_35 = { };
        private static ushort[][] p_ReducsOnTerminal_35 = { };

        private static ushort[] p_ExpectedIDs_36 = { 0xF };
        private static string[] p_ExpectedNames_36 = { "_T[{]" };
        private static string[] p_Items_36 = { "[grammar_terminals -> terminals . { _m95 }]" };
        private static ushort[][] p_ShiftsOnTerminal_36 = { new ushort[2] { 0xF, 0x40 } };
        private static ushort[][] p_ShiftsOnVariable_36 = { };
        private static ushort[][] p_ReducsOnTerminal_36 = { };

        private static ushort[] p_ExpectedIDs_37 = { 0xF };
        private static string[] p_ExpectedNames_37 = { "_T[{]" };
        private static string[] p_Items_37 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules . { _m166 }]" };
        private static ushort[][] p_ShiftsOnTerminal_37 = { new ushort[2] { 0xF, 0x41 } };
        private static ushort[][] p_ShiftsOnVariable_37 = { };
        private static ushort[][] p_ReducsOnTerminal_37 = { };

        private static ushort[] p_ExpectedIDs_38 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_38 = { "_T[}]", "NAME" };
        private static string[] p_Items_38 = { "[grammar_options -> options { . _m91 }]", "[_m91 -> . option _m91]", "[_m91 -> . ]", "[option -> . NAME = QUOTED_DATA ;]" };
        private static ushort[][] p_ShiftsOnTerminal_38 = { new ushort[2] { 0x8, 0x44 } };
        private static ushort[][] p_ShiftsOnVariable_38 = { new ushort[2] { 0x6C, 0x42 }, new ushort[2] { 0x4F, 0x43 } };
        private static ushort[][] p_ReducsOnTerminal_38 = { new ushort[2] { 0x10, 0x4B } };

        private static ushort[] p_ExpectedIDs_39 = { 0xF, 0x45 };
        private static string[] p_ExpectedNames_39 = { "_T[{]", "_T[,]" };
        private static string[] p_Items_39 = { "[_m99 -> , qualified_name . _m99]", "[_m99 -> . , qualified_name _m99]", "[_m99 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_39 = { new ushort[2] { 0x45, 0x32 } };
        private static ushort[][] p_ShiftsOnVariable_39 = { new ushort[2] { 0x6E, 0x45 } };
        private static ushort[][] p_ReducsOnTerminal_39 = { new ushort[2] { 0xF, 0x4F } };

        private static ushort[] p_ExpectedIDs_3A = { 0x4C };
        private static string[] p_ExpectedNames_3A = { "_T[rules]" };
        private static string[] p_Items_3A = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals . grammar_cf_rules<grammar_text_terminal> }]", "[grammar_cf_rules<grammar_text_terminal> -> . rules { _m137 }]" };
        private static ushort[][] p_ShiftsOnTerminal_3A = { new ushort[2] { 0x4C, 0x47 } };
        private static ushort[][] p_ShiftsOnVariable_3A = { new ushort[2] { 0x6F, 0x46 } };
        private static ushort[][] p_ReducsOnTerminal_3A = { };

        private static ushort[] p_ExpectedIDs_3B = { 0x10 };
        private static string[] p_ExpectedNames_3B = { "_T[}]" };
        private static string[] p_Items_3B = { "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> . }]" };
        private static ushort[][] p_ShiftsOnTerminal_3B = { new ushort[2] { 0x10, 0x48 } };
        private static ushort[][] p_ShiftsOnVariable_3B = { };
        private static ushort[][] p_ReducsOnTerminal_3B = { };

        private static ushort[] p_ExpectedIDs_3C = { 0xF };
        private static string[] p_ExpectedNames_3C = { "_T[{]" };
        private static string[] p_Items_3C = { "[grammar_cf_rules<grammar_bin_terminal> -> rules . { _m177 }]" };
        private static ushort[][] p_ShiftsOnTerminal_3C = { new ushort[2] { 0xF, 0x49 } };
        private static ushort[][] p_ShiftsOnVariable_3C = { };
        private static ushort[][] p_ReducsOnTerminal_3C = { };

        private static ushort[] p_ExpectedIDs_3D = { 0x10 };
        private static string[] p_ExpectedNames_3D = { "_T[}]" };
        private static string[] p_Items_3D = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> . }]" };
        private static ushort[][] p_ShiftsOnTerminal_3D = { new ushort[2] { 0x10, 0x4A } };
        private static ushort[][] p_ShiftsOnVariable_3D = { };
        private static ushort[][] p_ReducsOnTerminal_3D = { };

        private static ushort[] p_ExpectedIDs_3E = { 0xF };
        private static string[] p_ExpectedNames_3E = { "_T[{]" };
        private static string[] p_Items_3E = { "[grammar_cs_rules<grammar_text_terminal> -> rules . { _m148 }]" };
        private static ushort[][] p_ShiftsOnTerminal_3E = { new ushort[2] { 0xF, 0x4B } };
        private static ushort[][] p_ShiftsOnVariable_3E = { };
        private static ushort[][] p_ReducsOnTerminal_3E = { };

        private static ushort[] p_ExpectedIDs_3F = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_3F = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_3F = { "[cs_grammar_bin -> grammar cs NAME grammar_parency { grammar_options grammar_cs_rules<grammar_bin_terminal> } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_3F = { };
        private static ushort[][] p_ShiftsOnVariable_3F = { };
        private static ushort[][] p_ReducsOnTerminal_3F = { new ushort[2] { 0xE, 0x93 }, new ushort[2] { 0x4A, 0x93 }, new ushort[2] { 0xA, 0x93 }, new ushort[2] { 0xB, 0x93 }, new ushort[2] { 0xC, 0x93 }, new ushort[2] { 0xD, 0x93 }, new ushort[2] { 0x2, 0x93 }, new ushort[2] { 0x10, 0x93 } };

        private static ushort[] p_ExpectedIDs_40 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_40 = { "_T[}]", "NAME" };
        private static string[] p_Items_40 = { "[grammar_terminals -> terminals { . _m95 }]", "[_m95 -> . terminal _m95]", "[_m95 -> . ]", "[terminal -> . NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_ShiftsOnTerminal_40 = { new ushort[2] { 0x8, 0x4E } };
        private static ushort[][] p_ShiftsOnVariable_40 = { new ushort[2] { 0x6D, 0x4C }, new ushort[2] { 0x5B, 0x4D } };
        private static ushort[][] p_ReducsOnTerminal_40 = { new ushort[2] { 0x10, 0x4D } };

        private static ushort[] p_ExpectedIDs_41 = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_41 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_41 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { . _m166 }]", "[_m166 -> . cs_rule_simple<grammar_bin_terminal> _m166]", "[_m166 -> . cs_rule_template<grammar_bin_terminal> _m166]", "[_m166 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_41 = { new ushort[2] { 0xC6, 0x53 } };
        private static ushort[][] p_ShiftsOnVariable_41 = { new ushort[2] { 0xD3, 0x4F }, new ushort[2] { 0xD0, 0x50 }, new ushort[2] { 0xD2, 0x51 }, new ushort[2] { 0xD1, 0x52 } };
        private static ushort[][] p_ReducsOnTerminal_41 = { new ushort[2] { 0x10, 0xA3 }, new ushort[2] { 0x8, 0x9F } };

        private static ushort[] p_ExpectedIDs_42 = { 0x10 };
        private static string[] p_ExpectedNames_42 = { "_T[}]" };
        private static string[] p_Items_42 = { "[grammar_options -> options { _m91 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_42 = { new ushort[2] { 0x10, 0x54 } };
        private static ushort[][] p_ShiftsOnVariable_42 = { };
        private static ushort[][] p_ReducsOnTerminal_42 = { };

        private static ushort[] p_ExpectedIDs_43 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_43 = { "_T[}]", "NAME" };
        private static string[] p_Items_43 = { "[_m91 -> option . _m91]", "[_m91 -> . option _m91]", "[_m91 -> . ]", "[option -> . NAME = QUOTED_DATA ;]" };
        private static ushort[][] p_ShiftsOnTerminal_43 = { new ushort[2] { 0x8, 0x44 } };
        private static ushort[][] p_ShiftsOnVariable_43 = { new ushort[2] { 0x6C, 0x55 }, new ushort[2] { 0x4F, 0x43 } };
        private static ushort[][] p_ReducsOnTerminal_43 = { new ushort[2] { 0x10, 0x4B } };

        private static ushort[] p_ExpectedIDs_44 = { 0x38 };
        private static string[] p_ExpectedNames_44 = { "_T[=]" };
        private static string[] p_Items_44 = { "[option -> NAME . = QUOTED_DATA ;]" };
        private static ushort[][] p_ShiftsOnTerminal_44 = { new ushort[2] { 0x38, 0x56 } };
        private static ushort[][] p_ShiftsOnVariable_44 = { };
        private static ushort[][] p_ReducsOnTerminal_44 = { };

        private static ushort[] p_ExpectedIDs_45 = { 0xF };
        private static string[] p_ExpectedNames_45 = { "_T[{]" };
        private static string[] p_Items_45 = { "[_m99 -> , qualified_name _m99 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_45 = { };
        private static ushort[][] p_ShiftsOnVariable_45 = { };
        private static ushort[][] p_ReducsOnTerminal_45 = { new ushort[2] { 0xF, 0x4E } };

        private static ushort[] p_ExpectedIDs_46 = { 0x10 };
        private static string[] p_ExpectedNames_46 = { "_T[}]" };
        private static string[] p_Items_46 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> . }]" };
        private static ushort[][] p_ShiftsOnTerminal_46 = { new ushort[2] { 0x10, 0x57 } };
        private static ushort[][] p_ShiftsOnVariable_46 = { };
        private static ushort[][] p_ReducsOnTerminal_46 = { };

        private static ushort[] p_ExpectedIDs_47 = { 0xF };
        private static string[] p_ExpectedNames_47 = { "_T[{]" };
        private static string[] p_Items_47 = { "[grammar_cf_rules<grammar_text_terminal> -> rules . { _m137 }]" };
        private static ushort[][] p_ShiftsOnTerminal_47 = { new ushort[2] { 0xF, 0x58 } };
        private static ushort[][] p_ShiftsOnVariable_47 = { };
        private static ushort[][] p_ReducsOnTerminal_47 = { };

        private static ushort[] p_ExpectedIDs_48 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_48 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_48 = { "[cf_grammar_bin -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_cf_rules<grammar_bin_terminal> } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_48 = { };
        private static ushort[][] p_ShiftsOnVariable_48 = { };
        private static ushort[][] p_ReducsOnTerminal_48 = { new ushort[2] { 0xE, 0x41 }, new ushort[2] { 0x4A, 0x41 }, new ushort[2] { 0xA, 0x41 }, new ushort[2] { 0xB, 0x41 }, new ushort[2] { 0xC, 0x41 }, new ushort[2] { 0xD, 0x41 }, new ushort[2] { 0x2, 0x41 }, new ushort[2] { 0x10, 0x41 } };

        private static ushort[] p_ExpectedIDs_49 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_49 = { "_T[}]", "NAME" };
        private static string[] p_Items_49 = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { . _m177 }]", "[_m177 -> . cf_rule_simple<grammar_bin_terminal> _m177]", "[_m177 -> . cf_rule_template<grammar_bin_terminal> _m177]", "[_m177 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_49 = { new ushort[2] { 0x8, 0x5C } };
        private static ushort[][] p_ShiftsOnVariable_49 = { new ushort[2] { 0x90, 0x59 }, new ushort[2] { 0x81, 0x5A }, new ushort[2] { 0x8F, 0x5B } };
        private static ushort[][] p_ReducsOnTerminal_49 = { new ushort[2] { 0x10, 0x91 } };

        private static ushort[] p_ExpectedIDs_4A = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_4A = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_4A = { "[cs_grammar_text -> grammar cs NAME grammar_parency { grammar_options grammar_terminals grammar_cs_rules<grammar_text_terminal> } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_4A = { };
        private static ushort[][] p_ShiftsOnVariable_4A = { };
        private static ushort[][] p_ReducsOnTerminal_4A = { new ushort[2] { 0xE, 0x92 }, new ushort[2] { 0x4A, 0x92 }, new ushort[2] { 0xA, 0x92 }, new ushort[2] { 0xB, 0x92 }, new ushort[2] { 0xC, 0x92 }, new ushort[2] { 0xD, 0x92 }, new ushort[2] { 0x2, 0x92 }, new ushort[2] { 0x10, 0x92 } };

        private static ushort[] p_ExpectedIDs_4B = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_4B = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_4B = { "[grammar_cs_rules<grammar_text_terminal> -> rules { . _m148 }]", "[_m148 -> . cs_rule_simple<grammar_text_terminal> _m148]", "[_m148 -> . cs_rule_template<grammar_text_terminal> _m148]", "[_m148 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_4B = { new ushort[2] { 0xC6, 0x61 } };
        private static ushort[][] p_ShiftsOnVariable_4B = { new ushort[2] { 0xCE, 0x5D }, new ushort[2] { 0xCB, 0x5E }, new ushort[2] { 0xCD, 0x5F }, new ushort[2] { 0xCC, 0x60 } };
        private static ushort[][] p_ReducsOnTerminal_4B = { new ushort[2] { 0x10, 0x9B }, new ushort[2] { 0x8, 0x97 } };

        private static ushort[] p_ExpectedIDs_4C = { 0x10 };
        private static string[] p_ExpectedNames_4C = { "_T[}]" };
        private static string[] p_Items_4C = { "[grammar_terminals -> terminals { _m95 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_4C = { new ushort[2] { 0x10, 0x62 } };
        private static ushort[][] p_ShiftsOnVariable_4C = { };
        private static ushort[][] p_ReducsOnTerminal_4C = { };

        private static ushort[] p_ExpectedIDs_4D = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_4D = { "_T[}]", "NAME" };
        private static string[] p_Items_4D = { "[_m95 -> terminal . _m95]", "[_m95 -> . terminal _m95]", "[_m95 -> . ]", "[terminal -> . NAME -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_ShiftsOnTerminal_4D = { new ushort[2] { 0x8, 0x4E } };
        private static ushort[][] p_ShiftsOnVariable_4D = { new ushort[2] { 0x6D, 0x63 }, new ushort[2] { 0x5B, 0x4D } };
        private static ushort[][] p_ReducsOnTerminal_4D = { new ushort[2] { 0x10, 0x4D } };

        private static ushort[] p_ExpectedIDs_4E = { 0x43 };
        private static string[] p_ExpectedNames_4E = { "_T[->]" };
        private static string[] p_Items_4E = { "[terminal -> NAME . -> terminal_definition terminal_subgrammar ;]" };
        private static ushort[][] p_ShiftsOnTerminal_4E = { new ushort[2] { 0x43, 0x64 } };
        private static ushort[][] p_ShiftsOnVariable_4E = { };
        private static ushort[][] p_ReducsOnTerminal_4E = { };

        private static ushort[] p_ExpectedIDs_4F = { 0x10 };
        private static string[] p_ExpectedNames_4F = { "_T[}]" };
        private static string[] p_Items_4F = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { _m166 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_4F = { new ushort[2] { 0x10, 0x65 } };
        private static ushort[][] p_ShiftsOnVariable_4F = { };
        private static ushort[][] p_ReducsOnTerminal_4F = { };

        private static ushort[] p_ExpectedIDs_50 = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_50 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_50 = { "[_m166 -> cs_rule_simple<grammar_bin_terminal> . _m166]", "[_m166 -> . cs_rule_simple<grammar_bin_terminal> _m166]", "[_m166 -> . cs_rule_template<grammar_bin_terminal> _m166]", "[_m166 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_50 = { new ushort[2] { 0xC6, 0x53 } };
        private static ushort[][] p_ShiftsOnVariable_50 = { new ushort[2] { 0xD3, 0x66 }, new ushort[2] { 0xD0, 0x50 }, new ushort[2] { 0xD2, 0x51 }, new ushort[2] { 0xD1, 0x52 } };
        private static ushort[][] p_ReducsOnTerminal_50 = { new ushort[2] { 0x10, 0xA3 }, new ushort[2] { 0x8, 0x9F } };

        private static ushort[] p_ExpectedIDs_51 = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_51 = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_51 = { "[_m166 -> cs_rule_template<grammar_bin_terminal> . _m166]", "[_m166 -> . cs_rule_simple<grammar_bin_terminal> _m166]", "[_m166 -> . cs_rule_template<grammar_bin_terminal> _m166]", "[_m166 -> . ]", "[cs_rule_simple<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> . cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_51 = { new ushort[2] { 0xC6, 0x53 } };
        private static ushort[][] p_ShiftsOnVariable_51 = { new ushort[2] { 0xD3, 0x67 }, new ushort[2] { 0xD0, 0x50 }, new ushort[2] { 0xD2, 0x51 }, new ushort[2] { 0xD1, 0x52 } };
        private static ushort[][] p_ReducsOnTerminal_51 = { new ushort[2] { 0x10, 0xA3 }, new ushort[2] { 0x8, 0x9F } };

        private static ushort[] p_ExpectedIDs_52 = { 0x8 };
        private static string[] p_ExpectedNames_52 = { "NAME" };
        private static string[] p_Items_52 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> . NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> . NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_52 = { new ushort[2] { 0x8, 0x68 } };
        private static ushort[][] p_ShiftsOnVariable_52 = { };
        private static ushort[][] p_ReducsOnTerminal_52 = { };

        private static ushort[] p_ExpectedIDs_53 = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_53 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_53 = { "[cs_rule_context<grammar_bin_terminal> -> [ . rule_definition<grammar_bin_terminal> ]]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_53 = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_53 = { new ushort[2] { 0x82, 0x69 }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_53 = { };

        private static ushort[] p_ExpectedIDs_54 = { 0x48, 0x4C };
        private static string[] p_ExpectedNames_54 = { "_T[terminals]", "_T[rules]" };
        private static string[] p_Items_54 = { "[grammar_options -> options { _m91 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_54 = { };
        private static ushort[][] p_ShiftsOnVariable_54 = { };
        private static ushort[][] p_ReducsOnTerminal_54 = { new ushort[2] { 0x48, 0x38 }, new ushort[2] { 0x4C, 0x38 } };

        private static ushort[] p_ExpectedIDs_55 = { 0x10 };
        private static string[] p_ExpectedNames_55 = { "_T[}]" };
        private static string[] p_Items_55 = { "[_m91 -> option _m91 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_55 = { };
        private static ushort[][] p_ShiftsOnVariable_55 = { };
        private static ushort[][] p_ReducsOnTerminal_55 = { new ushort[2] { 0x10, 0x4A } };

        private static ushort[] p_ExpectedIDs_56 = { 0x29 };
        private static string[] p_ExpectedNames_56 = { "QUOTED_DATA" };
        private static string[] p_Items_56 = { "[option -> NAME = . QUOTED_DATA ;]" };
        private static ushort[][] p_ShiftsOnTerminal_56 = { new ushort[2] { 0x29, 0x85 } };
        private static ushort[][] p_ShiftsOnVariable_56 = { };
        private static ushort[][] p_ReducsOnTerminal_56 = { };

        private static ushort[] p_ExpectedIDs_57 = { 0xE, 0x4A, 0xA, 0xB, 0xC, 0xD, 0x2, 0x10 };
        private static string[] p_ExpectedNames_57 = { "_T[namespace]", "_T[grammar]", "_T[public]", "_T[private]", "_T[protected]", "_T[internal]", "$", "_T[}]" };
        private static string[] p_Items_57 = { "[cf_grammar_text -> grammar_access grammar cf NAME grammar_parency { grammar_options grammar_terminals grammar_cf_rules<grammar_text_terminal> } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_57 = { };
        private static ushort[][] p_ShiftsOnVariable_57 = { };
        private static ushort[][] p_ReducsOnTerminal_57 = { new ushort[2] { 0xE, 0x40 }, new ushort[2] { 0x4A, 0x40 }, new ushort[2] { 0xA, 0x40 }, new ushort[2] { 0xB, 0x40 }, new ushort[2] { 0xC, 0x40 }, new ushort[2] { 0xD, 0x40 }, new ushort[2] { 0x2, 0x40 }, new ushort[2] { 0x10, 0x40 } };

        private static ushort[] p_ExpectedIDs_58 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_58 = { "_T[}]", "NAME" };
        private static string[] p_Items_58 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { . _m137 }]", "[_m137 -> . cf_rule_simple<grammar_text_terminal> _m137]", "[_m137 -> . cf_rule_template<grammar_text_terminal> _m137]", "[_m137 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_58 = { new ushort[2] { 0x8, 0x89 } };
        private static ushort[][] p_ShiftsOnVariable_58 = { new ushort[2] { 0x7F, 0x86 }, new ushort[2] { 0x70, 0x87 }, new ushort[2] { 0x7E, 0x88 } };
        private static ushort[][] p_ReducsOnTerminal_58 = { new ushort[2] { 0x10, 0x70 } };

        private static ushort[] p_ExpectedIDs_59 = { 0x10 };
        private static string[] p_ExpectedNames_59 = { "_T[}]" };
        private static string[] p_Items_59 = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { _m177 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_59 = { new ushort[2] { 0x10, 0x8A } };
        private static ushort[][] p_ShiftsOnVariable_59 = { };
        private static ushort[][] p_ReducsOnTerminal_59 = { };

        private static ushort[] p_ExpectedIDs_5A = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_5A = { "_T[}]", "NAME" };
        private static string[] p_Items_5A = { "[_m177 -> cf_rule_simple<grammar_bin_terminal> . _m177]", "[_m177 -> . cf_rule_simple<grammar_bin_terminal> _m177]", "[_m177 -> . cf_rule_template<grammar_bin_terminal> _m177]", "[_m177 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_5A = { new ushort[2] { 0x8, 0x5C } };
        private static ushort[][] p_ShiftsOnVariable_5A = { new ushort[2] { 0x90, 0x8B }, new ushort[2] { 0x81, 0x5A }, new ushort[2] { 0x8F, 0x5B } };
        private static ushort[][] p_ReducsOnTerminal_5A = { new ushort[2] { 0x10, 0x91 } };

        private static ushort[] p_ExpectedIDs_5B = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_5B = { "_T[}]", "NAME" };
        private static string[] p_Items_5B = { "[_m177 -> cf_rule_template<grammar_bin_terminal> . _m177]", "[_m177 -> . cf_rule_simple<grammar_bin_terminal> _m177]", "[_m177 -> . cf_rule_template<grammar_bin_terminal> _m177]", "[_m177 -> . ]", "[cf_rule_simple<grammar_bin_terminal> -> . NAME -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> . NAME rule_template_params -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_5B = { new ushort[2] { 0x8, 0x5C } };
        private static ushort[][] p_ShiftsOnVariable_5B = { new ushort[2] { 0x90, 0x8C }, new ushort[2] { 0x81, 0x5A }, new ushort[2] { 0x8F, 0x5B } };
        private static ushort[][] p_ReducsOnTerminal_5B = { new ushort[2] { 0x10, 0x91 } };

        private static ushort[] p_ExpectedIDs_5C = { 0x43, 0x44 };
        private static string[] p_ExpectedNames_5C = { "_T[->]", "_T[<]" };
        private static string[] p_Items_5C = { "[cf_rule_simple<grammar_bin_terminal> -> NAME . -> rule_definition<grammar_bin_terminal> ;]", "[cf_rule_template<grammar_bin_terminal> -> NAME . rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params -> . < NAME _m87 >]" };
        private static ushort[][] p_ShiftsOnTerminal_5C = { new ushort[2] { 0x43, 0x8D }, new ushort[2] { 0x44, 0x8F } };
        private static ushort[][] p_ShiftsOnVariable_5C = { new ushort[2] { 0x5F, 0x8E } };
        private static ushort[][] p_ReducsOnTerminal_5C = { };

        private static ushort[] p_ExpectedIDs_5D = { 0x10 };
        private static string[] p_ExpectedNames_5D = { "_T[}]" };
        private static string[] p_Items_5D = { "[grammar_cs_rules<grammar_text_terminal> -> rules { _m148 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_5D = { new ushort[2] { 0x10, 0x90 } };
        private static ushort[][] p_ShiftsOnVariable_5D = { };
        private static ushort[][] p_ReducsOnTerminal_5D = { };

        private static ushort[] p_ExpectedIDs_5E = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_5E = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_5E = { "[_m148 -> cs_rule_simple<grammar_text_terminal> . _m148]", "[_m148 -> . cs_rule_simple<grammar_text_terminal> _m148]", "[_m148 -> . cs_rule_template<grammar_text_terminal> _m148]", "[_m148 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_5E = { new ushort[2] { 0xC6, 0x61 } };
        private static ushort[][] p_ShiftsOnVariable_5E = { new ushort[2] { 0xCE, 0x91 }, new ushort[2] { 0xCB, 0x5E }, new ushort[2] { 0xCD, 0x5F }, new ushort[2] { 0xCC, 0x60 } };
        private static ushort[][] p_ReducsOnTerminal_5E = { new ushort[2] { 0x10, 0x9B }, new ushort[2] { 0x8, 0x97 } };

        private static ushort[] p_ExpectedIDs_5F = { 0x10, 0x8, 0xC6 };
        private static string[] p_ExpectedNames_5F = { "_T[}]", "NAME", "_T[[]" };
        private static string[] p_Items_5F = { "[_m148 -> cs_rule_template<grammar_text_terminal> . _m148]", "[_m148 -> . cs_rule_simple<grammar_text_terminal> _m148]", "[_m148 -> . cs_rule_template<grammar_text_terminal> _m148]", "[_m148 -> . ]", "[cs_rule_simple<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> . cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_5F = { new ushort[2] { 0xC6, 0x61 } };
        private static ushort[][] p_ShiftsOnVariable_5F = { new ushort[2] { 0xCE, 0x92 }, new ushort[2] { 0xCB, 0x5E }, new ushort[2] { 0xCD, 0x5F }, new ushort[2] { 0xCC, 0x60 } };
        private static ushort[][] p_ReducsOnTerminal_5F = { new ushort[2] { 0x10, 0x9B }, new ushort[2] { 0x8, 0x97 } };

        private static ushort[] p_ExpectedIDs_60 = { 0x8 };
        private static string[] p_ExpectedNames_60 = { "NAME" };
        private static string[] p_Items_60 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> . NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> . NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_60 = { new ushort[2] { 0x8, 0x93 } };
        private static ushort[][] p_ShiftsOnVariable_60 = { };
        private static ushort[][] p_ReducsOnTerminal_60 = { };

        private static ushort[] p_ExpectedIDs_61 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_61 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_61 = { "[cs_rule_context<grammar_text_terminal> -> [ . rule_definition<grammar_text_terminal> ]]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_61 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_61 = { new ushort[2] { 0x71, 0x94 }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_61 = { };

        private static ushort[] p_ExpectedIDs_62 = { 0x4C };
        private static string[] p_ExpectedNames_62 = { "_T[rules]" };
        private static string[] p_Items_62 = { "[grammar_terminals -> terminals { _m95 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_62 = { };
        private static ushort[][] p_ShiftsOnVariable_62 = { };
        private static ushort[][] p_ReducsOnTerminal_62 = { new ushort[2] { 0x4C, 0x39 } };

        private static ushort[] p_ExpectedIDs_63 = { 0x10 };
        private static string[] p_ExpectedNames_63 = { "_T[}]" };
        private static string[] p_Items_63 = { "[_m95 -> terminal _m95 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_63 = { };
        private static ushort[][] p_ShiftsOnVariable_63 = { };
        private static ushort[][] p_ReducsOnTerminal_63 = { new ushort[2] { 0x10, 0x4C } };

        private static ushort[] p_ExpectedIDs_64 = { 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_64 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_64 = { "[terminal -> NAME -> . terminal_definition terminal_subgrammar ;]", "[terminal_definition -> . terminal_def_restrict _m79]", "[terminal_def_restrict -> . terminal_def_fragment _m77]", "[terminal_def_fragment -> . terminal_def_repetition _m75]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_64 = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_64 = { new ushort[2] { 0x59, 0xA4 }, new ushort[2] { 0x58, 0xA5 }, new ushort[2] { 0x57, 0xA6 }, new ushort[2] { 0x56, 0xA7 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_64 = { };

        private static ushort[] p_ExpectedIDs_65 = { 0x10 };
        private static string[] p_ExpectedNames_65 = { "_T[}]" };
        private static string[] p_Items_65 = { "[grammar_cs_rules<grammar_bin_terminal> -> rules { _m166 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_65 = { };
        private static ushort[][] p_ShiftsOnVariable_65 = { };
        private static ushort[][] p_ReducsOnTerminal_65 = { new ushort[2] { 0x10, 0x9C } };

        private static ushort[] p_ExpectedIDs_66 = { 0x10 };
        private static string[] p_ExpectedNames_66 = { "_T[}]" };
        private static string[] p_Items_66 = { "[_m166 -> cs_rule_simple<grammar_bin_terminal> _m166 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_66 = { };
        private static ushort[][] p_ShiftsOnVariable_66 = { };
        private static ushort[][] p_ReducsOnTerminal_66 = { new ushort[2] { 0x10, 0xA1 } };

        private static ushort[] p_ExpectedIDs_67 = { 0x10 };
        private static string[] p_ExpectedNames_67 = { "_T[}]" };
        private static string[] p_Items_67 = { "[_m166 -> cs_rule_template<grammar_bin_terminal> _m166 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_67 = { };
        private static ushort[][] p_ShiftsOnVariable_67 = { };
        private static ushort[][] p_ReducsOnTerminal_67 = { new ushort[2] { 0x10, 0xA2 } };

        private static ushort[] p_ExpectedIDs_68 = { 0x43, 0x44, 0xC6 };
        private static string[] p_ExpectedNames_68 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] p_Items_68 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME . cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME . cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_context<grammar_bin_terminal> -> . [ rule_definition<grammar_bin_terminal> ]]", "[cs_rule_context<grammar_bin_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_68 = { new ushort[2] { 0xC6, 0x53 } };
        private static ushort[][] p_ShiftsOnVariable_68 = { new ushort[2] { 0xD1, 0xB3 } };
        private static ushort[][] p_ReducsOnTerminal_68 = { new ushort[2] { 0x43, 0x9F }, new ushort[2] { 0x44, 0x9F } };

        private static ushort[] p_ExpectedIDs_69 = { 0xC7 };
        private static string[] p_ExpectedNames_69 = { "_T[]]" };
        private static string[] p_Items_69 = { "[cs_rule_context<grammar_bin_terminal> -> [ rule_definition<grammar_bin_terminal> . ]]" };
        private static ushort[][] p_ShiftsOnTerminal_69 = { new ushort[2] { 0xC7, 0xB4 } };
        private static ushort[][] p_ShiftsOnVariable_69 = { };
        private static ushort[][] p_ReducsOnTerminal_69 = { };

        private static ushort[] p_ExpectedIDs_6A = { 0xC7, 0x3C, 0x39, 0x41 };
        private static string[] p_ExpectedNames_6A = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_Items_6A = { "[rule_definition<grammar_bin_terminal> -> rule_def_restrict<grammar_bin_terminal> . _m172]", "[_m172 -> . | rule_def_restrict<grammar_bin_terminal> _m172]", "[_m172 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6A = { new ushort[2] { 0x41, 0xB6 } };
        private static ushort[][] p_ShiftsOnVariable_6A = { new ushort[2] { 0x8E, 0xB5 } };
        private static ushort[][] p_ReducsOnTerminal_6A = { new ushort[2] { 0xC7, 0x8D }, new ushort[2] { 0x3C, 0x8D }, new ushort[2] { 0x39, 0x8D } };

        private static ushort[] p_ExpectedIDs_6B = { 0x41, 0xC7, 0x3C, 0x39, 0x40 };
        private static string[] p_ExpectedNames_6B = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_Items_6B = { "[rule_def_restrict<grammar_bin_terminal> -> rule_def_fragment<grammar_bin_terminal> . _m170]", "[_m170 -> . - rule_def_fragment<grammar_bin_terminal> _m170]", "[_m170 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6B = { new ushort[2] { 0x40, 0xB8 } };
        private static ushort[][] p_ShiftsOnVariable_6B = { new ushort[2] { 0x8D, 0xB7 } };
        private static ushort[][] p_ReducsOnTerminal_6B = { new ushort[2] { 0x41, 0x8B }, new ushort[2] { 0xC7, 0x8B }, new ushort[2] { 0x3C, 0x8B }, new ushort[2] { 0x39, 0x8B } };

        private static ushort[] p_ExpectedIDs_6C = { 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_6C = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_6C = { "[rule_def_fragment<grammar_bin_terminal> -> rule_def_repetition<grammar_bin_terminal> . _m168]", "[_m168 -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[_m168 -> . ]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_6C = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_6C = { new ushort[2] { 0x8C, 0xB9 }, new ushort[2] { 0x85, 0xBA }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_6C = { new ushort[2] { 0x40, 0x89 }, new ushort[2] { 0x41, 0x89 }, new ushort[2] { 0xC7, 0x89 }, new ushort[2] { 0x3C, 0x89 }, new ushort[2] { 0x39, 0x89 } };

        private static ushort[] p_ExpectedIDs_6D = { 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3D, 0x3E, 0x3F };
        private static string[] p_ExpectedNames_6D = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_Items_6D = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . *]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . +]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . ?]", "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6D = { new ushort[2] { 0x3D, 0xBB }, new ushort[2] { 0x3E, 0xBC }, new ushort[2] { 0x3F, 0xBD } };
        private static ushort[][] p_ShiftsOnVariable_6D = { };
        private static ushort[][] p_ReducsOnTerminal_6D = { new ushort[2] { 0x3B, 0x79 }, new ushort[2] { 0x29, 0x79 }, new ushort[2] { 0x8, 0x79 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x79 }, new ushort[2] { 0xF, 0x79 }, new ushort[2] { 0x2E, 0x79 }, new ushort[2] { 0x2F, 0x79 }, new ushort[2] { 0x30, 0x79 }, new ushort[2] { 0x31, 0x79 }, new ushort[2] { 0x32, 0x79 }, new ushort[2] { 0x33, 0x79 }, new ushort[2] { 0x34, 0x79 }, new ushort[2] { 0x35, 0x79 }, new ushort[2] { 0x36, 0x79 }, new ushort[2] { 0x37, 0x79 }, new ushort[2] { 0x40, 0x79 }, new ushort[2] { 0x41, 0x79 }, new ushort[2] { 0xC7, 0x79 }, new ushort[2] { 0x3C, 0x79 }, new ushort[2] { 0x39, 0x79 } };

        private static ushort[] p_ExpectedIDs_6E = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x4D, 0x4E };
        private static string[] p_ExpectedNames_6E = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[^]", "_T[!]" };
        private static string[] p_Items_6E = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . ^]", "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . !]", "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6E = { new ushort[2] { 0x4D, 0xBE }, new ushort[2] { 0x4E, 0xBF } };
        private static ushort[][] p_ShiftsOnVariable_6E = { };
        private static ushort[][] p_ReducsOnTerminal_6E = { new ushort[2] { 0x3D, 0x7C }, new ushort[2] { 0x3E, 0x7C }, new ushort[2] { 0x3F, 0x7C }, new ushort[2] { 0x3B, 0x7C }, new ushort[2] { 0x29, 0x7C }, new ushort[2] { 0x8, 0x7C }, new ushort[2] { 0x2C, 0x7C }, new ushort[2] { 0x2D, 0x7C }, new ushort[2] { 0xF, 0x7C }, new ushort[2] { 0x2E, 0x7C }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7C }, new ushort[2] { 0x31, 0x7C }, new ushort[2] { 0x32, 0x7C }, new ushort[2] { 0x33, 0x7C }, new ushort[2] { 0x34, 0x7C }, new ushort[2] { 0x35, 0x7C }, new ushort[2] { 0x36, 0x7C }, new ushort[2] { 0x37, 0x7C }, new ushort[2] { 0x40, 0x7C }, new ushort[2] { 0x41, 0x7C }, new ushort[2] { 0xC7, 0x7C }, new ushort[2] { 0x3C, 0x7C }, new ushort[2] { 0x39, 0x7C } };

        private static ushort[] p_ExpectedIDs_6F = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_6F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_6F = { "[rule_def_element<grammar_bin_terminal> -> rule_def_atom<grammar_bin_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_6F = { };
        private static ushort[][] p_ShiftsOnVariable_6F = { };
        private static ushort[][] p_ReducsOnTerminal_6F = { new ushort[2] { 0x4D, 0x7D }, new ushort[2] { 0x4E, 0x7D }, new ushort[2] { 0x3D, 0x7D }, new ushort[2] { 0x3E, 0x7D }, new ushort[2] { 0x3F, 0x7D }, new ushort[2] { 0x3B, 0x7D }, new ushort[2] { 0x29, 0x7D }, new ushort[2] { 0x8, 0x7D }, new ushort[2] { 0x2C, 0x7D }, new ushort[2] { 0x2D, 0x7D }, new ushort[2] { 0xF, 0x7D }, new ushort[2] { 0x2E, 0x7D }, new ushort[2] { 0x2F, 0x7D }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7D }, new ushort[2] { 0x32, 0x7D }, new ushort[2] { 0x33, 0x7D }, new ushort[2] { 0x34, 0x7D }, new ushort[2] { 0x35, 0x7D }, new ushort[2] { 0x36, 0x7D }, new ushort[2] { 0x37, 0x7D }, new ushort[2] { 0x40, 0x7D }, new ushort[2] { 0x41, 0x7D }, new ushort[2] { 0xC7, 0x7D }, new ushort[2] { 0x3C, 0x7D }, new ushort[2] { 0x39, 0x7D } };

        private static ushort[] p_ExpectedIDs_70 = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_70 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_70 = { "[rule_def_element<grammar_bin_terminal> -> ( . rule_definition<grammar_bin_terminal> )]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_70 = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_70 = { new ushort[2] { 0x82, 0xC0 }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_70 = { };

        private static ushort[] p_ExpectedIDs_71 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_71 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_71 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_action . ]" };
        private static ushort[][] p_ShiftsOnTerminal_71 = { };
        private static ushort[][] p_ShiftsOnVariable_71 = { };
        private static ushort[][] p_ReducsOnTerminal_71 = { new ushort[2] { 0x4D, 0x7F }, new ushort[2] { 0x4E, 0x7F }, new ushort[2] { 0x3D, 0x7F }, new ushort[2] { 0x3E, 0x7F }, new ushort[2] { 0x3F, 0x7F }, new ushort[2] { 0x3B, 0x7F }, new ushort[2] { 0x29, 0x7F }, new ushort[2] { 0x8, 0x7F }, new ushort[2] { 0x2C, 0x7F }, new ushort[2] { 0x2D, 0x7F }, new ushort[2] { 0xF, 0x7F }, new ushort[2] { 0x2E, 0x7F }, new ushort[2] { 0x2F, 0x7F }, new ushort[2] { 0x30, 0x7F }, new ushort[2] { 0x31, 0x7F }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x7F }, new ushort[2] { 0x34, 0x7F }, new ushort[2] { 0x35, 0x7F }, new ushort[2] { 0x36, 0x7F }, new ushort[2] { 0x37, 0x7F }, new ushort[2] { 0x40, 0x7F }, new ushort[2] { 0x41, 0x7F }, new ushort[2] { 0xC7, 0x7F }, new ushort[2] { 0x3C, 0x7F }, new ushort[2] { 0x39, 0x7F }, new ushort[2] { 0x46, 0x7F }, new ushort[2] { 0x45, 0x7F } };

        private static ushort[] p_ExpectedIDs_72 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_72 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_72 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_virtual . ]" };
        private static ushort[][] p_ShiftsOnTerminal_72 = { };
        private static ushort[][] p_ShiftsOnVariable_72 = { };
        private static ushort[][] p_ReducsOnTerminal_72 = { new ushort[2] { 0x4D, 0x80 }, new ushort[2] { 0x4E, 0x80 }, new ushort[2] { 0x3D, 0x80 }, new ushort[2] { 0x3E, 0x80 }, new ushort[2] { 0x3F, 0x80 }, new ushort[2] { 0x3B, 0x80 }, new ushort[2] { 0x29, 0x80 }, new ushort[2] { 0x8, 0x80 }, new ushort[2] { 0x2C, 0x80 }, new ushort[2] { 0x2D, 0x80 }, new ushort[2] { 0xF, 0x80 }, new ushort[2] { 0x2E, 0x80 }, new ushort[2] { 0x2F, 0x80 }, new ushort[2] { 0x30, 0x80 }, new ushort[2] { 0x31, 0x80 }, new ushort[2] { 0x32, 0x80 }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x80 }, new ushort[2] { 0x35, 0x80 }, new ushort[2] { 0x36, 0x80 }, new ushort[2] { 0x37, 0x80 }, new ushort[2] { 0x40, 0x80 }, new ushort[2] { 0x41, 0x80 }, new ushort[2] { 0xC7, 0x80 }, new ushort[2] { 0x3C, 0x80 }, new ushort[2] { 0x39, 0x80 }, new ushort[2] { 0x46, 0x80 }, new ushort[2] { 0x45, 0x80 } };

        private static ushort[] p_ExpectedIDs_73 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_73 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_73 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_ref_simple . ]" };
        private static ushort[][] p_ShiftsOnTerminal_73 = { };
        private static ushort[][] p_ShiftsOnVariable_73 = { };
        private static ushort[][] p_ReducsOnTerminal_73 = { new ushort[2] { 0x4D, 0x81 }, new ushort[2] { 0x4E, 0x81 }, new ushort[2] { 0x3D, 0x81 }, new ushort[2] { 0x3E, 0x81 }, new ushort[2] { 0x3F, 0x81 }, new ushort[2] { 0x3B, 0x81 }, new ushort[2] { 0x29, 0x81 }, new ushort[2] { 0x8, 0x81 }, new ushort[2] { 0x2C, 0x81 }, new ushort[2] { 0x2D, 0x81 }, new ushort[2] { 0xF, 0x81 }, new ushort[2] { 0x2E, 0x81 }, new ushort[2] { 0x2F, 0x81 }, new ushort[2] { 0x30, 0x81 }, new ushort[2] { 0x31, 0x81 }, new ushort[2] { 0x32, 0x81 }, new ushort[2] { 0x33, 0x81 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x81 }, new ushort[2] { 0x36, 0x81 }, new ushort[2] { 0x37, 0x81 }, new ushort[2] { 0x40, 0x81 }, new ushort[2] { 0x41, 0x81 }, new ushort[2] { 0xC7, 0x81 }, new ushort[2] { 0x3C, 0x81 }, new ushort[2] { 0x39, 0x81 }, new ushort[2] { 0x46, 0x81 }, new ushort[2] { 0x45, 0x81 } };

        private static ushort[] p_ExpectedIDs_74 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_74 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_74 = { "[rule_def_atom<grammar_bin_terminal> -> rule_sym_ref_template<grammar_bin_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_74 = { };
        private static ushort[][] p_ShiftsOnVariable_74 = { };
        private static ushort[][] p_ReducsOnTerminal_74 = { new ushort[2] { 0x4D, 0x82 }, new ushort[2] { 0x4E, 0x82 }, new ushort[2] { 0x3D, 0x82 }, new ushort[2] { 0x3E, 0x82 }, new ushort[2] { 0x3F, 0x82 }, new ushort[2] { 0x3B, 0x82 }, new ushort[2] { 0x29, 0x82 }, new ushort[2] { 0x8, 0x82 }, new ushort[2] { 0x2C, 0x82 }, new ushort[2] { 0x2D, 0x82 }, new ushort[2] { 0xF, 0x82 }, new ushort[2] { 0x2E, 0x82 }, new ushort[2] { 0x2F, 0x82 }, new ushort[2] { 0x30, 0x82 }, new ushort[2] { 0x31, 0x82 }, new ushort[2] { 0x32, 0x82 }, new ushort[2] { 0x33, 0x82 }, new ushort[2] { 0x34, 0x82 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x82 }, new ushort[2] { 0x37, 0x82 }, new ushort[2] { 0x40, 0x82 }, new ushort[2] { 0x41, 0x82 }, new ushort[2] { 0xC7, 0x82 }, new ushort[2] { 0x3C, 0x82 }, new ushort[2] { 0x39, 0x82 }, new ushort[2] { 0x46, 0x82 }, new ushort[2] { 0x45, 0x82 } };

        private static ushort[] p_ExpectedIDs_75 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_75 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_75 = { "[rule_def_atom<grammar_bin_terminal> -> grammar_bin_terminal . ]" };
        private static ushort[][] p_ShiftsOnTerminal_75 = { };
        private static ushort[][] p_ShiftsOnVariable_75 = { };
        private static ushort[][] p_ReducsOnTerminal_75 = { new ushort[2] { 0x4D, 0x83 }, new ushort[2] { 0x4E, 0x83 }, new ushort[2] { 0x3D, 0x83 }, new ushort[2] { 0x3E, 0x83 }, new ushort[2] { 0x3F, 0x83 }, new ushort[2] { 0x3B, 0x83 }, new ushort[2] { 0x29, 0x83 }, new ushort[2] { 0x8, 0x83 }, new ushort[2] { 0x2C, 0x83 }, new ushort[2] { 0x2D, 0x83 }, new ushort[2] { 0xF, 0x83 }, new ushort[2] { 0x2E, 0x83 }, new ushort[2] { 0x2F, 0x83 }, new ushort[2] { 0x30, 0x83 }, new ushort[2] { 0x31, 0x83 }, new ushort[2] { 0x32, 0x83 }, new ushort[2] { 0x33, 0x83 }, new ushort[2] { 0x34, 0x83 }, new ushort[2] { 0x35, 0x83 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x83 }, new ushort[2] { 0x40, 0x83 }, new ushort[2] { 0x41, 0x83 }, new ushort[2] { 0xC7, 0x83 }, new ushort[2] { 0x3C, 0x83 }, new ushort[2] { 0x39, 0x83 }, new ushort[2] { 0x46, 0x83 }, new ushort[2] { 0x45, 0x83 } };

        private static ushort[] p_ExpectedIDs_76 = { 0x8 };
        private static string[] p_ExpectedNames_76 = { "NAME" };
        private static string[] p_Items_76 = { "[rule_sym_action -> { . qualified_name }]", "[qualified_name -> . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_76 = { new ushort[2] { 0x8, 0x18 } };
        private static ushort[][] p_ShiftsOnVariable_76 = { new ushort[2] { 0x11, 0xC1 } };
        private static ushort[][] p_ReducsOnTerminal_76 = { };

        private static ushort[] p_ExpectedIDs_77 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x2A, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_77 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_TERMINAL_TEXT", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_77 = { "[rule_sym_virtual -> QUOTED_DATA . ]" };
        private static ushort[][] p_ShiftsOnTerminal_77 = { };
        private static ushort[][] p_ShiftsOnVariable_77 = { };
        private static ushort[][] p_ReducsOnTerminal_77 = { new ushort[2] { 0x4D, 0x28 }, new ushort[2] { 0x4E, 0x28 }, new ushort[2] { 0x3D, 0x28 }, new ushort[2] { 0x3E, 0x28 }, new ushort[2] { 0x3F, 0x28 }, new ushort[2] { 0x3B, 0x28 }, new ushort[2] { 0x29, 0x28 }, new ushort[2] { 0x8, 0x28 }, new ushort[2] { 0x2C, 0x28 }, new ushort[2] { 0x2D, 0x28 }, new ushort[2] { 0xF, 0x28 }, new ushort[2] { 0x2E, 0x28 }, new ushort[2] { 0x2F, 0x28 }, new ushort[2] { 0x30, 0x28 }, new ushort[2] { 0x31, 0x28 }, new ushort[2] { 0x32, 0x28 }, new ushort[2] { 0x33, 0x28 }, new ushort[2] { 0x34, 0x28 }, new ushort[2] { 0x35, 0x28 }, new ushort[2] { 0x36, 0x28 }, new ushort[2] { 0x37, 0x28 }, new ushort[2] { 0x40, 0x28 }, new ushort[2] { 0x41, 0x28 }, new ushort[2] { 0xC7, 0x28 }, new ushort[2] { 0x2A, 0x28 }, new ushort[2] { 0x3C, 0x28 }, new ushort[2] { 0x39, 0x28 }, new ushort[2] { 0x46, 0x28 }, new ushort[2] { 0x45, 0x28 } };

        private static ushort[] p_ExpectedIDs_78 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45, 0x44 };
        private static string[] p_ExpectedNames_78 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]", "_T[<]" };
        private static string[] p_Items_78 = { "[rule_sym_ref_simple -> NAME . ]", "[rule_sym_ref_template<grammar_bin_terminal> -> NAME . rule_sym_ref_params<grammar_bin_terminal>]", "[rule_sym_ref_params<grammar_bin_terminal> -> . < rule_def_atom<grammar_bin_terminal> _m159 >]" };
        private static ushort[][] p_ShiftsOnTerminal_78 = { new ushort[2] { 0x44, 0xC3 } };
        private static ushort[][] p_ShiftsOnVariable_78 = { new ushort[2] { 0x8A, 0xC2 } };
        private static ushort[][] p_ReducsOnTerminal_78 = { new ushort[2] { 0x4D, 0x29 }, new ushort[2] { 0x4E, 0x29 }, new ushort[2] { 0x3D, 0x29 }, new ushort[2] { 0x3E, 0x29 }, new ushort[2] { 0x3F, 0x29 }, new ushort[2] { 0x3B, 0x29 }, new ushort[2] { 0x29, 0x29 }, new ushort[2] { 0x8, 0x29 }, new ushort[2] { 0x2C, 0x29 }, new ushort[2] { 0x2D, 0x29 }, new ushort[2] { 0xF, 0x29 }, new ushort[2] { 0x2E, 0x29 }, new ushort[2] { 0x2F, 0x29 }, new ushort[2] { 0x30, 0x29 }, new ushort[2] { 0x31, 0x29 }, new ushort[2] { 0x32, 0x29 }, new ushort[2] { 0x33, 0x29 }, new ushort[2] { 0x34, 0x29 }, new ushort[2] { 0x35, 0x29 }, new ushort[2] { 0x36, 0x29 }, new ushort[2] { 0x37, 0x29 }, new ushort[2] { 0x40, 0x29 }, new ushort[2] { 0x41, 0x29 }, new ushort[2] { 0xC7, 0x29 }, new ushort[2] { 0x3C, 0x29 }, new ushort[2] { 0x39, 0x29 }, new ushort[2] { 0x46, 0x29 }, new ushort[2] { 0x45, 0x29 } };

        private static ushort[] p_ExpectedIDs_79 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_79 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_79 = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT8 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_79 = { };
        private static ushort[][] p_ShiftsOnVariable_79 = { };
        private static ushort[][] p_ReducsOnTerminal_79 = { new ushort[2] { 0x4D, 0x2B }, new ushort[2] { 0x4E, 0x2B }, new ushort[2] { 0x3D, 0x2B }, new ushort[2] { 0x3E, 0x2B }, new ushort[2] { 0x3F, 0x2B }, new ushort[2] { 0x3B, 0x2B }, new ushort[2] { 0x29, 0x2B }, new ushort[2] { 0x8, 0x2B }, new ushort[2] { 0x2C, 0x2B }, new ushort[2] { 0x2D, 0x2B }, new ushort[2] { 0xF, 0x2B }, new ushort[2] { 0x2E, 0x2B }, new ushort[2] { 0x2F, 0x2B }, new ushort[2] { 0x30, 0x2B }, new ushort[2] { 0x31, 0x2B }, new ushort[2] { 0x32, 0x2B }, new ushort[2] { 0x33, 0x2B }, new ushort[2] { 0x34, 0x2B }, new ushort[2] { 0x35, 0x2B }, new ushort[2] { 0x36, 0x2B }, new ushort[2] { 0x37, 0x2B }, new ushort[2] { 0x40, 0x2B }, new ushort[2] { 0x41, 0x2B }, new ushort[2] { 0xC7, 0x2B }, new ushort[2] { 0x3C, 0x2B }, new ushort[2] { 0x39, 0x2B }, new ushort[2] { 0x46, 0x2B }, new ushort[2] { 0x45, 0x2B } };

        private static ushort[] p_ExpectedIDs_7A = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7A = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7A = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT16 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7A = { };
        private static ushort[][] p_ShiftsOnVariable_7A = { };
        private static ushort[][] p_ReducsOnTerminal_7A = { new ushort[2] { 0x4D, 0x2C }, new ushort[2] { 0x4E, 0x2C }, new ushort[2] { 0x3D, 0x2C }, new ushort[2] { 0x3E, 0x2C }, new ushort[2] { 0x3F, 0x2C }, new ushort[2] { 0x3B, 0x2C }, new ushort[2] { 0x29, 0x2C }, new ushort[2] { 0x8, 0x2C }, new ushort[2] { 0x2C, 0x2C }, new ushort[2] { 0x2D, 0x2C }, new ushort[2] { 0xF, 0x2C }, new ushort[2] { 0x2E, 0x2C }, new ushort[2] { 0x2F, 0x2C }, new ushort[2] { 0x30, 0x2C }, new ushort[2] { 0x31, 0x2C }, new ushort[2] { 0x32, 0x2C }, new ushort[2] { 0x33, 0x2C }, new ushort[2] { 0x34, 0x2C }, new ushort[2] { 0x35, 0x2C }, new ushort[2] { 0x36, 0x2C }, new ushort[2] { 0x37, 0x2C }, new ushort[2] { 0x40, 0x2C }, new ushort[2] { 0x41, 0x2C }, new ushort[2] { 0xC7, 0x2C }, new ushort[2] { 0x3C, 0x2C }, new ushort[2] { 0x39, 0x2C }, new ushort[2] { 0x46, 0x2C }, new ushort[2] { 0x45, 0x2C } };

        private static ushort[] p_ExpectedIDs_7B = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7B = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7B = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT32 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7B = { };
        private static ushort[][] p_ShiftsOnVariable_7B = { };
        private static ushort[][] p_ReducsOnTerminal_7B = { new ushort[2] { 0x4D, 0x2D }, new ushort[2] { 0x4E, 0x2D }, new ushort[2] { 0x3D, 0x2D }, new ushort[2] { 0x3E, 0x2D }, new ushort[2] { 0x3F, 0x2D }, new ushort[2] { 0x3B, 0x2D }, new ushort[2] { 0x29, 0x2D }, new ushort[2] { 0x8, 0x2D }, new ushort[2] { 0x2C, 0x2D }, new ushort[2] { 0x2D, 0x2D }, new ushort[2] { 0xF, 0x2D }, new ushort[2] { 0x2E, 0x2D }, new ushort[2] { 0x2F, 0x2D }, new ushort[2] { 0x30, 0x2D }, new ushort[2] { 0x31, 0x2D }, new ushort[2] { 0x32, 0x2D }, new ushort[2] { 0x33, 0x2D }, new ushort[2] { 0x34, 0x2D }, new ushort[2] { 0x35, 0x2D }, new ushort[2] { 0x36, 0x2D }, new ushort[2] { 0x37, 0x2D }, new ushort[2] { 0x40, 0x2D }, new ushort[2] { 0x41, 0x2D }, new ushort[2] { 0xC7, 0x2D }, new ushort[2] { 0x3C, 0x2D }, new ushort[2] { 0x39, 0x2D }, new ushort[2] { 0x46, 0x2D }, new ushort[2] { 0x45, 0x2D } };

        private static ushort[] p_ExpectedIDs_7C = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7C = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7C = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT64 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7C = { };
        private static ushort[][] p_ShiftsOnVariable_7C = { };
        private static ushort[][] p_ReducsOnTerminal_7C = { new ushort[2] { 0x4D, 0x2E }, new ushort[2] { 0x4E, 0x2E }, new ushort[2] { 0x3D, 0x2E }, new ushort[2] { 0x3E, 0x2E }, new ushort[2] { 0x3F, 0x2E }, new ushort[2] { 0x3B, 0x2E }, new ushort[2] { 0x29, 0x2E }, new ushort[2] { 0x8, 0x2E }, new ushort[2] { 0x2C, 0x2E }, new ushort[2] { 0x2D, 0x2E }, new ushort[2] { 0xF, 0x2E }, new ushort[2] { 0x2E, 0x2E }, new ushort[2] { 0x2F, 0x2E }, new ushort[2] { 0x30, 0x2E }, new ushort[2] { 0x31, 0x2E }, new ushort[2] { 0x32, 0x2E }, new ushort[2] { 0x33, 0x2E }, new ushort[2] { 0x34, 0x2E }, new ushort[2] { 0x35, 0x2E }, new ushort[2] { 0x36, 0x2E }, new ushort[2] { 0x37, 0x2E }, new ushort[2] { 0x40, 0x2E }, new ushort[2] { 0x41, 0x2E }, new ushort[2] { 0xC7, 0x2E }, new ushort[2] { 0x3C, 0x2E }, new ushort[2] { 0x39, 0x2E }, new ushort[2] { 0x46, 0x2E }, new ushort[2] { 0x45, 0x2E } };

        private static ushort[] p_ExpectedIDs_7D = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7D = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7D = { "[grammar_bin_terminal -> SYMBOL_VALUE_UINT128 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7D = { };
        private static ushort[][] p_ShiftsOnVariable_7D = { };
        private static ushort[][] p_ReducsOnTerminal_7D = { new ushort[2] { 0x4D, 0x2F }, new ushort[2] { 0x4E, 0x2F }, new ushort[2] { 0x3D, 0x2F }, new ushort[2] { 0x3E, 0x2F }, new ushort[2] { 0x3F, 0x2F }, new ushort[2] { 0x3B, 0x2F }, new ushort[2] { 0x29, 0x2F }, new ushort[2] { 0x8, 0x2F }, new ushort[2] { 0x2C, 0x2F }, new ushort[2] { 0x2D, 0x2F }, new ushort[2] { 0xF, 0x2F }, new ushort[2] { 0x2E, 0x2F }, new ushort[2] { 0x2F, 0x2F }, new ushort[2] { 0x30, 0x2F }, new ushort[2] { 0x31, 0x2F }, new ushort[2] { 0x32, 0x2F }, new ushort[2] { 0x33, 0x2F }, new ushort[2] { 0x34, 0x2F }, new ushort[2] { 0x35, 0x2F }, new ushort[2] { 0x36, 0x2F }, new ushort[2] { 0x37, 0x2F }, new ushort[2] { 0x40, 0x2F }, new ushort[2] { 0x41, 0x2F }, new ushort[2] { 0xC7, 0x2F }, new ushort[2] { 0x3C, 0x2F }, new ushort[2] { 0x39, 0x2F }, new ushort[2] { 0x46, 0x2F }, new ushort[2] { 0x45, 0x2F } };

        private static ushort[] p_ExpectedIDs_7E = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7E = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7E = { "[grammar_bin_terminal -> SYMBOL_VALUE_BINARY . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7E = { };
        private static ushort[][] p_ShiftsOnVariable_7E = { };
        private static ushort[][] p_ReducsOnTerminal_7E = { new ushort[2] { 0x4D, 0x30 }, new ushort[2] { 0x4E, 0x30 }, new ushort[2] { 0x3D, 0x30 }, new ushort[2] { 0x3E, 0x30 }, new ushort[2] { 0x3F, 0x30 }, new ushort[2] { 0x3B, 0x30 }, new ushort[2] { 0x29, 0x30 }, new ushort[2] { 0x8, 0x30 }, new ushort[2] { 0x2C, 0x30 }, new ushort[2] { 0x2D, 0x30 }, new ushort[2] { 0xF, 0x30 }, new ushort[2] { 0x2E, 0x30 }, new ushort[2] { 0x2F, 0x30 }, new ushort[2] { 0x30, 0x30 }, new ushort[2] { 0x31, 0x30 }, new ushort[2] { 0x32, 0x30 }, new ushort[2] { 0x33, 0x30 }, new ushort[2] { 0x34, 0x30 }, new ushort[2] { 0x35, 0x30 }, new ushort[2] { 0x36, 0x30 }, new ushort[2] { 0x37, 0x30 }, new ushort[2] { 0x40, 0x30 }, new ushort[2] { 0x41, 0x30 }, new ushort[2] { 0xC7, 0x30 }, new ushort[2] { 0x3C, 0x30 }, new ushort[2] { 0x39, 0x30 }, new ushort[2] { 0x46, 0x30 }, new ushort[2] { 0x45, 0x30 } };

        private static ushort[] p_ExpectedIDs_7F = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_7F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_7F = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT8 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_7F = { };
        private static ushort[][] p_ShiftsOnVariable_7F = { };
        private static ushort[][] p_ReducsOnTerminal_7F = { new ushort[2] { 0x4D, 0x31 }, new ushort[2] { 0x4E, 0x31 }, new ushort[2] { 0x3D, 0x31 }, new ushort[2] { 0x3E, 0x31 }, new ushort[2] { 0x3F, 0x31 }, new ushort[2] { 0x3B, 0x31 }, new ushort[2] { 0x29, 0x31 }, new ushort[2] { 0x8, 0x31 }, new ushort[2] { 0x2C, 0x31 }, new ushort[2] { 0x2D, 0x31 }, new ushort[2] { 0xF, 0x31 }, new ushort[2] { 0x2E, 0x31 }, new ushort[2] { 0x2F, 0x31 }, new ushort[2] { 0x30, 0x31 }, new ushort[2] { 0x31, 0x31 }, new ushort[2] { 0x32, 0x31 }, new ushort[2] { 0x33, 0x31 }, new ushort[2] { 0x34, 0x31 }, new ushort[2] { 0x35, 0x31 }, new ushort[2] { 0x36, 0x31 }, new ushort[2] { 0x37, 0x31 }, new ushort[2] { 0x40, 0x31 }, new ushort[2] { 0x41, 0x31 }, new ushort[2] { 0xC7, 0x31 }, new ushort[2] { 0x3C, 0x31 }, new ushort[2] { 0x39, 0x31 }, new ushort[2] { 0x46, 0x31 }, new ushort[2] { 0x45, 0x31 } };

        private static ushort[] p_ExpectedIDs_80 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_80 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_80 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT16 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_80 = { };
        private static ushort[][] p_ShiftsOnVariable_80 = { };
        private static ushort[][] p_ReducsOnTerminal_80 = { new ushort[2] { 0x4D, 0x32 }, new ushort[2] { 0x4E, 0x32 }, new ushort[2] { 0x3D, 0x32 }, new ushort[2] { 0x3E, 0x32 }, new ushort[2] { 0x3F, 0x32 }, new ushort[2] { 0x3B, 0x32 }, new ushort[2] { 0x29, 0x32 }, new ushort[2] { 0x8, 0x32 }, new ushort[2] { 0x2C, 0x32 }, new ushort[2] { 0x2D, 0x32 }, new ushort[2] { 0xF, 0x32 }, new ushort[2] { 0x2E, 0x32 }, new ushort[2] { 0x2F, 0x32 }, new ushort[2] { 0x30, 0x32 }, new ushort[2] { 0x31, 0x32 }, new ushort[2] { 0x32, 0x32 }, new ushort[2] { 0x33, 0x32 }, new ushort[2] { 0x34, 0x32 }, new ushort[2] { 0x35, 0x32 }, new ushort[2] { 0x36, 0x32 }, new ushort[2] { 0x37, 0x32 }, new ushort[2] { 0x40, 0x32 }, new ushort[2] { 0x41, 0x32 }, new ushort[2] { 0xC7, 0x32 }, new ushort[2] { 0x3C, 0x32 }, new ushort[2] { 0x39, 0x32 }, new ushort[2] { 0x46, 0x32 }, new ushort[2] { 0x45, 0x32 } };

        private static ushort[] p_ExpectedIDs_81 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_81 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_81 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT32 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_81 = { };
        private static ushort[][] p_ShiftsOnVariable_81 = { };
        private static ushort[][] p_ReducsOnTerminal_81 = { new ushort[2] { 0x4D, 0x33 }, new ushort[2] { 0x4E, 0x33 }, new ushort[2] { 0x3D, 0x33 }, new ushort[2] { 0x3E, 0x33 }, new ushort[2] { 0x3F, 0x33 }, new ushort[2] { 0x3B, 0x33 }, new ushort[2] { 0x29, 0x33 }, new ushort[2] { 0x8, 0x33 }, new ushort[2] { 0x2C, 0x33 }, new ushort[2] { 0x2D, 0x33 }, new ushort[2] { 0xF, 0x33 }, new ushort[2] { 0x2E, 0x33 }, new ushort[2] { 0x2F, 0x33 }, new ushort[2] { 0x30, 0x33 }, new ushort[2] { 0x31, 0x33 }, new ushort[2] { 0x32, 0x33 }, new ushort[2] { 0x33, 0x33 }, new ushort[2] { 0x34, 0x33 }, new ushort[2] { 0x35, 0x33 }, new ushort[2] { 0x36, 0x33 }, new ushort[2] { 0x37, 0x33 }, new ushort[2] { 0x40, 0x33 }, new ushort[2] { 0x41, 0x33 }, new ushort[2] { 0xC7, 0x33 }, new ushort[2] { 0x3C, 0x33 }, new ushort[2] { 0x39, 0x33 }, new ushort[2] { 0x46, 0x33 }, new ushort[2] { 0x45, 0x33 } };

        private static ushort[] p_ExpectedIDs_82 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_82 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_82 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT64 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_82 = { };
        private static ushort[][] p_ShiftsOnVariable_82 = { };
        private static ushort[][] p_ReducsOnTerminal_82 = { new ushort[2] { 0x4D, 0x34 }, new ushort[2] { 0x4E, 0x34 }, new ushort[2] { 0x3D, 0x34 }, new ushort[2] { 0x3E, 0x34 }, new ushort[2] { 0x3F, 0x34 }, new ushort[2] { 0x3B, 0x34 }, new ushort[2] { 0x29, 0x34 }, new ushort[2] { 0x8, 0x34 }, new ushort[2] { 0x2C, 0x34 }, new ushort[2] { 0x2D, 0x34 }, new ushort[2] { 0xF, 0x34 }, new ushort[2] { 0x2E, 0x34 }, new ushort[2] { 0x2F, 0x34 }, new ushort[2] { 0x30, 0x34 }, new ushort[2] { 0x31, 0x34 }, new ushort[2] { 0x32, 0x34 }, new ushort[2] { 0x33, 0x34 }, new ushort[2] { 0x34, 0x34 }, new ushort[2] { 0x35, 0x34 }, new ushort[2] { 0x36, 0x34 }, new ushort[2] { 0x37, 0x34 }, new ushort[2] { 0x40, 0x34 }, new ushort[2] { 0x41, 0x34 }, new ushort[2] { 0xC7, 0x34 }, new ushort[2] { 0x3C, 0x34 }, new ushort[2] { 0x39, 0x34 }, new ushort[2] { 0x46, 0x34 }, new ushort[2] { 0x45, 0x34 } };

        private static ushort[] p_ExpectedIDs_83 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_83 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_83 = { "[grammar_bin_terminal -> SYMBOL_JOKER_UINT128 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_83 = { };
        private static ushort[][] p_ShiftsOnVariable_83 = { };
        private static ushort[][] p_ReducsOnTerminal_83 = { new ushort[2] { 0x4D, 0x35 }, new ushort[2] { 0x4E, 0x35 }, new ushort[2] { 0x3D, 0x35 }, new ushort[2] { 0x3E, 0x35 }, new ushort[2] { 0x3F, 0x35 }, new ushort[2] { 0x3B, 0x35 }, new ushort[2] { 0x29, 0x35 }, new ushort[2] { 0x8, 0x35 }, new ushort[2] { 0x2C, 0x35 }, new ushort[2] { 0x2D, 0x35 }, new ushort[2] { 0xF, 0x35 }, new ushort[2] { 0x2E, 0x35 }, new ushort[2] { 0x2F, 0x35 }, new ushort[2] { 0x30, 0x35 }, new ushort[2] { 0x31, 0x35 }, new ushort[2] { 0x32, 0x35 }, new ushort[2] { 0x33, 0x35 }, new ushort[2] { 0x34, 0x35 }, new ushort[2] { 0x35, 0x35 }, new ushort[2] { 0x36, 0x35 }, new ushort[2] { 0x37, 0x35 }, new ushort[2] { 0x40, 0x35 }, new ushort[2] { 0x41, 0x35 }, new ushort[2] { 0xC7, 0x35 }, new ushort[2] { 0x3C, 0x35 }, new ushort[2] { 0x39, 0x35 }, new ushort[2] { 0x46, 0x35 }, new ushort[2] { 0x45, 0x35 } };

        private static ushort[] p_ExpectedIDs_84 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_84 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_84 = { "[grammar_bin_terminal -> SYMBOL_JOKER_BINARY . ]" };
        private static ushort[][] p_ShiftsOnTerminal_84 = { };
        private static ushort[][] p_ShiftsOnVariable_84 = { };
        private static ushort[][] p_ReducsOnTerminal_84 = { new ushort[2] { 0x4D, 0x36 }, new ushort[2] { 0x4E, 0x36 }, new ushort[2] { 0x3D, 0x36 }, new ushort[2] { 0x3E, 0x36 }, new ushort[2] { 0x3F, 0x36 }, new ushort[2] { 0x3B, 0x36 }, new ushort[2] { 0x29, 0x36 }, new ushort[2] { 0x8, 0x36 }, new ushort[2] { 0x2C, 0x36 }, new ushort[2] { 0x2D, 0x36 }, new ushort[2] { 0xF, 0x36 }, new ushort[2] { 0x2E, 0x36 }, new ushort[2] { 0x2F, 0x36 }, new ushort[2] { 0x30, 0x36 }, new ushort[2] { 0x31, 0x36 }, new ushort[2] { 0x32, 0x36 }, new ushort[2] { 0x33, 0x36 }, new ushort[2] { 0x34, 0x36 }, new ushort[2] { 0x35, 0x36 }, new ushort[2] { 0x36, 0x36 }, new ushort[2] { 0x37, 0x36 }, new ushort[2] { 0x40, 0x36 }, new ushort[2] { 0x41, 0x36 }, new ushort[2] { 0xC7, 0x36 }, new ushort[2] { 0x3C, 0x36 }, new ushort[2] { 0x39, 0x36 }, new ushort[2] { 0x46, 0x36 }, new ushort[2] { 0x45, 0x36 } };

        private static ushort[] p_ExpectedIDs_85 = { 0x39 };
        private static string[] p_ExpectedNames_85 = { "_T[;]" };
        private static string[] p_Items_85 = { "[option -> NAME = QUOTED_DATA . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_85 = { new ushort[2] { 0x39, 0xC4 } };
        private static ushort[][] p_ShiftsOnVariable_85 = { };
        private static ushort[][] p_ReducsOnTerminal_85 = { };

        private static ushort[] p_ExpectedIDs_86 = { 0x10 };
        private static string[] p_ExpectedNames_86 = { "_T[}]" };
        private static string[] p_Items_86 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { _m137 . }]" };
        private static ushort[][] p_ShiftsOnTerminal_86 = { new ushort[2] { 0x10, 0xC5 } };
        private static ushort[][] p_ShiftsOnVariable_86 = { };
        private static ushort[][] p_ReducsOnTerminal_86 = { };

        private static ushort[] p_ExpectedIDs_87 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_87 = { "_T[}]", "NAME" };
        private static string[] p_Items_87 = { "[_m137 -> cf_rule_simple<grammar_text_terminal> . _m137]", "[_m137 -> . cf_rule_simple<grammar_text_terminal> _m137]", "[_m137 -> . cf_rule_template<grammar_text_terminal> _m137]", "[_m137 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_87 = { new ushort[2] { 0x8, 0x89 } };
        private static ushort[][] p_ShiftsOnVariable_87 = { new ushort[2] { 0x7F, 0xC6 }, new ushort[2] { 0x70, 0x87 }, new ushort[2] { 0x7E, 0x88 } };
        private static ushort[][] p_ReducsOnTerminal_87 = { new ushort[2] { 0x10, 0x70 } };

        private static ushort[] p_ExpectedIDs_88 = { 0x10, 0x8 };
        private static string[] p_ExpectedNames_88 = { "_T[}]", "NAME" };
        private static string[] p_Items_88 = { "[_m137 -> cf_rule_template<grammar_text_terminal> . _m137]", "[_m137 -> . cf_rule_simple<grammar_text_terminal> _m137]", "[_m137 -> . cf_rule_template<grammar_text_terminal> _m137]", "[_m137 -> . ]", "[cf_rule_simple<grammar_text_terminal> -> . NAME -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> . NAME rule_template_params -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_88 = { new ushort[2] { 0x8, 0x89 } };
        private static ushort[][] p_ShiftsOnVariable_88 = { new ushort[2] { 0x7F, 0xC7 }, new ushort[2] { 0x70, 0x87 }, new ushort[2] { 0x7E, 0x88 } };
        private static ushort[][] p_ReducsOnTerminal_88 = { new ushort[2] { 0x10, 0x70 } };

        private static ushort[] p_ExpectedIDs_89 = { 0x43, 0x44 };
        private static string[] p_ExpectedNames_89 = { "_T[->]", "_T[<]" };
        private static string[] p_Items_89 = { "[cf_rule_simple<grammar_text_terminal> -> NAME . -> rule_definition<grammar_text_terminal> ;]", "[cf_rule_template<grammar_text_terminal> -> NAME . rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params -> . < NAME _m87 >]" };
        private static ushort[][] p_ShiftsOnTerminal_89 = { new ushort[2] { 0x43, 0xC8 }, new ushort[2] { 0x44, 0x8F } };
        private static ushort[][] p_ShiftsOnVariable_89 = { new ushort[2] { 0x5F, 0xC9 } };
        private static ushort[][] p_ReducsOnTerminal_89 = { };

        private static ushort[] p_ExpectedIDs_8A = { 0x10 };
        private static string[] p_ExpectedNames_8A = { "_T[}]" };
        private static string[] p_Items_8A = { "[grammar_cf_rules<grammar_bin_terminal> -> rules { _m177 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_8A = { };
        private static ushort[][] p_ShiftsOnVariable_8A = { };
        private static ushort[][] p_ReducsOnTerminal_8A = { new ushort[2] { 0x10, 0x71 } };

        private static ushort[] p_ExpectedIDs_8B = { 0x10 };
        private static string[] p_ExpectedNames_8B = { "_T[}]" };
        private static string[] p_Items_8B = { "[_m177 -> cf_rule_simple<grammar_bin_terminal> _m177 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_8B = { };
        private static ushort[][] p_ShiftsOnVariable_8B = { };
        private static ushort[][] p_ReducsOnTerminal_8B = { new ushort[2] { 0x10, 0x8F } };

        private static ushort[] p_ExpectedIDs_8C = { 0x10 };
        private static string[] p_ExpectedNames_8C = { "_T[}]" };
        private static string[] p_Items_8C = { "[_m177 -> cf_rule_template<grammar_bin_terminal> _m177 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_8C = { };
        private static ushort[][] p_ShiftsOnVariable_8C = { };
        private static ushort[][] p_ReducsOnTerminal_8C = { new ushort[2] { 0x10, 0x90 } };

        private static ushort[] p_ExpectedIDs_8D = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_8D = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_8D = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_8D = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_8D = { new ushort[2] { 0x82, 0xCA }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_8D = { };

        private static ushort[] p_ExpectedIDs_8E = { 0x43 };
        private static string[] p_ExpectedNames_8E = { "_T[->]" };
        private static string[] p_Items_8E = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params . -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_8E = { new ushort[2] { 0x43, 0xCB } };
        private static ushort[][] p_ShiftsOnVariable_8E = { };
        private static ushort[][] p_ReducsOnTerminal_8E = { };

        private static ushort[] p_ExpectedIDs_8F = { 0x8 };
        private static string[] p_ExpectedNames_8F = { "NAME" };
        private static string[] p_Items_8F = { "[rule_template_params -> < . NAME _m87 >]" };
        private static ushort[][] p_ShiftsOnTerminal_8F = { new ushort[2] { 0x8, 0xCC } };
        private static ushort[][] p_ShiftsOnVariable_8F = { };
        private static ushort[][] p_ReducsOnTerminal_8F = { };

        private static ushort[] p_ExpectedIDs_90 = { 0x10 };
        private static string[] p_ExpectedNames_90 = { "_T[}]" };
        private static string[] p_Items_90 = { "[grammar_cs_rules<grammar_text_terminal> -> rules { _m148 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_90 = { };
        private static ushort[][] p_ShiftsOnVariable_90 = { };
        private static ushort[][] p_ReducsOnTerminal_90 = { new ushort[2] { 0x10, 0x94 } };

        private static ushort[] p_ExpectedIDs_91 = { 0x10 };
        private static string[] p_ExpectedNames_91 = { "_T[}]" };
        private static string[] p_Items_91 = { "[_m148 -> cs_rule_simple<grammar_text_terminal> _m148 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_91 = { };
        private static ushort[][] p_ShiftsOnVariable_91 = { };
        private static ushort[][] p_ReducsOnTerminal_91 = { new ushort[2] { 0x10, 0x99 } };

        private static ushort[] p_ExpectedIDs_92 = { 0x10 };
        private static string[] p_ExpectedNames_92 = { "_T[}]" };
        private static string[] p_Items_92 = { "[_m148 -> cs_rule_template<grammar_text_terminal> _m148 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_92 = { };
        private static ushort[][] p_ShiftsOnVariable_92 = { };
        private static ushort[][] p_ReducsOnTerminal_92 = { new ushort[2] { 0x10, 0x9A } };

        private static ushort[] p_ExpectedIDs_93 = { 0x43, 0x44, 0xC6 };
        private static string[] p_ExpectedNames_93 = { "_T[->]", "_T[<]", "_T[[]" };
        private static string[] p_Items_93 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME . cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME . cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_context<grammar_text_terminal> -> . [ rule_definition<grammar_text_terminal> ]]", "[cs_rule_context<grammar_text_terminal> -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_93 = { new ushort[2] { 0xC6, 0x61 } };
        private static ushort[][] p_ShiftsOnVariable_93 = { new ushort[2] { 0xCC, 0xCD } };
        private static ushort[][] p_ReducsOnTerminal_93 = { new ushort[2] { 0x43, 0x97 }, new ushort[2] { 0x44, 0x97 } };

        private static ushort[] p_ExpectedIDs_94 = { 0xC7 };
        private static string[] p_ExpectedNames_94 = { "_T[]]" };
        private static string[] p_Items_94 = { "[cs_rule_context<grammar_text_terminal> -> [ rule_definition<grammar_text_terminal> . ]]" };
        private static ushort[][] p_ShiftsOnTerminal_94 = { new ushort[2] { 0xC7, 0xCE } };
        private static ushort[][] p_ShiftsOnVariable_94 = { };
        private static ushort[][] p_ReducsOnTerminal_94 = { };

        private static ushort[] p_ExpectedIDs_95 = { 0xC7, 0x3C, 0x39, 0x41 };
        private static string[] p_ExpectedNames_95 = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_Items_95 = { "[rule_definition<grammar_text_terminal> -> rule_def_restrict<grammar_text_terminal> . _m132]", "[_m132 -> . | rule_def_restrict<grammar_text_terminal> _m132]", "[_m132 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_95 = { new ushort[2] { 0x41, 0xD0 } };
        private static ushort[][] p_ShiftsOnVariable_95 = { new ushort[2] { 0x7D, 0xCF } };
        private static ushort[][] p_ReducsOnTerminal_95 = { new ushort[2] { 0xC7, 0x6C }, new ushort[2] { 0x3C, 0x6C }, new ushort[2] { 0x39, 0x6C } };

        private static ushort[] p_ExpectedIDs_96 = { 0x41, 0xC7, 0x3C, 0x39, 0x40 };
        private static string[] p_ExpectedNames_96 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_Items_96 = { "[rule_def_restrict<grammar_text_terminal> -> rule_def_fragment<grammar_text_terminal> . _m130]", "[_m130 -> . - rule_def_fragment<grammar_text_terminal> _m130]", "[_m130 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_96 = { new ushort[2] { 0x40, 0xD2 } };
        private static ushort[][] p_ShiftsOnVariable_96 = { new ushort[2] { 0x7C, 0xD1 } };
        private static ushort[][] p_ReducsOnTerminal_96 = { new ushort[2] { 0x41, 0x6A }, new ushort[2] { 0xC7, 0x6A }, new ushort[2] { 0x3C, 0x6A }, new ushort[2] { 0x39, 0x6A } };

        private static ushort[] p_ExpectedIDs_97 = { 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_97 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_97 = { "[rule_def_fragment<grammar_text_terminal> -> rule_def_repetition<grammar_text_terminal> . _m128]", "[_m128 -> . rule_def_repetition<grammar_text_terminal> _m128]", "[_m128 -> . ]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_97 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_97 = { new ushort[2] { 0x7B, 0xD3 }, new ushort[2] { 0x74, 0xD4 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_97 = { new ushort[2] { 0x40, 0x68 }, new ushort[2] { 0x41, 0x68 }, new ushort[2] { 0xC7, 0x68 }, new ushort[2] { 0x3C, 0x68 }, new ushort[2] { 0x39, 0x68 } };

        private static ushort[] p_ExpectedIDs_98 = { 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3D, 0x3E, 0x3F };
        private static string[] p_ExpectedNames_98 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_Items_98 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . *]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . +]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . ?]", "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_98 = { new ushort[2] { 0x3D, 0xD5 }, new ushort[2] { 0x3E, 0xD6 }, new ushort[2] { 0x3F, 0xD7 } };
        private static ushort[][] p_ShiftsOnVariable_98 = { };
        private static ushort[][] p_ReducsOnTerminal_98 = { new ushort[2] { 0x3B, 0x58 }, new ushort[2] { 0x29, 0x58 }, new ushort[2] { 0x8, 0x58 }, new ushort[2] { 0x2A, 0x58 }, new ushort[2] { 0xF, 0x58 }, new ushort[2] { 0x40, 0x58 }, new ushort[2] { 0x41, 0x58 }, new ushort[2] { 0xC7, 0x58 }, new ushort[2] { 0x3C, 0x58 }, new ushort[2] { 0x39, 0x58 } };

        private static ushort[] p_ExpectedIDs_99 = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x4D, 0x4E };
        private static string[] p_ExpectedNames_99 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[^]", "_T[!]" };
        private static string[] p_Items_99 = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . ^]", "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . !]", "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_99 = { new ushort[2] { 0x4D, 0xD8 }, new ushort[2] { 0x4E, 0xD9 } };
        private static ushort[][] p_ShiftsOnVariable_99 = { };
        private static ushort[][] p_ReducsOnTerminal_99 = { new ushort[2] { 0x3D, 0x5B }, new ushort[2] { 0x3E, 0x5B }, new ushort[2] { 0x3F, 0x5B }, new ushort[2] { 0x3B, 0x5B }, new ushort[2] { 0x29, 0x5B }, new ushort[2] { 0x8, 0x5B }, new ushort[2] { 0x2A, 0x5B }, new ushort[2] { 0xF, 0x5B }, new ushort[2] { 0x40, 0x5B }, new ushort[2] { 0x41, 0x5B }, new ushort[2] { 0xC7, 0x5B }, new ushort[2] { 0x3C, 0x5B }, new ushort[2] { 0x39, 0x5B } };

        private static ushort[] p_ExpectedIDs_9A = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_9A = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_9A = { "[rule_def_element<grammar_text_terminal> -> rule_def_atom<grammar_text_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_9A = { };
        private static ushort[][] p_ShiftsOnVariable_9A = { };
        private static ushort[][] p_ReducsOnTerminal_9A = { new ushort[2] { 0x4D, 0x5C }, new ushort[2] { 0x4E, 0x5C }, new ushort[2] { 0x3D, 0x5C }, new ushort[2] { 0x3E, 0x5C }, new ushort[2] { 0x3F, 0x5C }, new ushort[2] { 0x3B, 0x5C }, new ushort[2] { 0x29, 0x5C }, new ushort[2] { 0x8, 0x5C }, new ushort[2] { 0x2A, 0x5C }, new ushort[2] { 0xF, 0x5C }, new ushort[2] { 0x40, 0x5C }, new ushort[2] { 0x41, 0x5C }, new ushort[2] { 0xC7, 0x5C }, new ushort[2] { 0x3C, 0x5C }, new ushort[2] { 0x39, 0x5C } };

        private static ushort[] p_ExpectedIDs_9B = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_9B = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_9B = { "[rule_def_element<grammar_text_terminal> -> ( . rule_definition<grammar_text_terminal> )]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_9B = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_9B = { new ushort[2] { 0x71, 0xDA }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_9B = { };

        private static ushort[] p_ExpectedIDs_9C = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_9C = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_9C = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_action . ]" };
        private static ushort[][] p_ShiftsOnTerminal_9C = { };
        private static ushort[][] p_ShiftsOnVariable_9C = { };
        private static ushort[][] p_ReducsOnTerminal_9C = { new ushort[2] { 0x4D, 0x5E }, new ushort[2] { 0x4E, 0x5E }, new ushort[2] { 0x3D, 0x5E }, new ushort[2] { 0x3E, 0x5E }, new ushort[2] { 0x3F, 0x5E }, new ushort[2] { 0x3B, 0x5E }, new ushort[2] { 0x29, 0x5E }, new ushort[2] { 0x8, 0x5E }, new ushort[2] { 0x2A, 0x5E }, new ushort[2] { 0xF, 0x5E }, new ushort[2] { 0x40, 0x5E }, new ushort[2] { 0x41, 0x5E }, new ushort[2] { 0xC7, 0x5E }, new ushort[2] { 0x3C, 0x5E }, new ushort[2] { 0x39, 0x5E }, new ushort[2] { 0x46, 0x5E }, new ushort[2] { 0x45, 0x5E } };

        private static ushort[] p_ExpectedIDs_9D = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_9D = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_9D = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_virtual . ]" };
        private static ushort[][] p_ShiftsOnTerminal_9D = { };
        private static ushort[][] p_ShiftsOnVariable_9D = { };
        private static ushort[][] p_ReducsOnTerminal_9D = { new ushort[2] { 0x4D, 0x5F }, new ushort[2] { 0x4E, 0x5F }, new ushort[2] { 0x3D, 0x5F }, new ushort[2] { 0x3E, 0x5F }, new ushort[2] { 0x3F, 0x5F }, new ushort[2] { 0x3B, 0x5F }, new ushort[2] { 0x29, 0x5F }, new ushort[2] { 0x8, 0x5F }, new ushort[2] { 0x2A, 0x5F }, new ushort[2] { 0xF, 0x5F }, new ushort[2] { 0x40, 0x5F }, new ushort[2] { 0x41, 0x5F }, new ushort[2] { 0xC7, 0x5F }, new ushort[2] { 0x3C, 0x5F }, new ushort[2] { 0x39, 0x5F }, new ushort[2] { 0x46, 0x5F }, new ushort[2] { 0x45, 0x5F } };

        private static ushort[] p_ExpectedIDs_9E = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_9E = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_9E = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_ref_simple . ]" };
        private static ushort[][] p_ShiftsOnTerminal_9E = { };
        private static ushort[][] p_ShiftsOnVariable_9E = { };
        private static ushort[][] p_ReducsOnTerminal_9E = { new ushort[2] { 0x4D, 0x60 }, new ushort[2] { 0x4E, 0x60 }, new ushort[2] { 0x3D, 0x60 }, new ushort[2] { 0x3E, 0x60 }, new ushort[2] { 0x3F, 0x60 }, new ushort[2] { 0x3B, 0x60 }, new ushort[2] { 0x29, 0x60 }, new ushort[2] { 0x8, 0x60 }, new ushort[2] { 0x2A, 0x60 }, new ushort[2] { 0xF, 0x60 }, new ushort[2] { 0x40, 0x60 }, new ushort[2] { 0x41, 0x60 }, new ushort[2] { 0xC7, 0x60 }, new ushort[2] { 0x3C, 0x60 }, new ushort[2] { 0x39, 0x60 }, new ushort[2] { 0x46, 0x60 }, new ushort[2] { 0x45, 0x60 } };

        private static ushort[] p_ExpectedIDs_9F = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_9F = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_9F = { "[rule_def_atom<grammar_text_terminal> -> rule_sym_ref_template<grammar_text_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_9F = { };
        private static ushort[][] p_ShiftsOnVariable_9F = { };
        private static ushort[][] p_ReducsOnTerminal_9F = { new ushort[2] { 0x4D, 0x61 }, new ushort[2] { 0x4E, 0x61 }, new ushort[2] { 0x3D, 0x61 }, new ushort[2] { 0x3E, 0x61 }, new ushort[2] { 0x3F, 0x61 }, new ushort[2] { 0x3B, 0x61 }, new ushort[2] { 0x29, 0x61 }, new ushort[2] { 0x8, 0x61 }, new ushort[2] { 0x2A, 0x61 }, new ushort[2] { 0xF, 0x61 }, new ushort[2] { 0x40, 0x61 }, new ushort[2] { 0x41, 0x61 }, new ushort[2] { 0xC7, 0x61 }, new ushort[2] { 0x3C, 0x61 }, new ushort[2] { 0x39, 0x61 }, new ushort[2] { 0x46, 0x61 }, new ushort[2] { 0x45, 0x61 } };

        private static ushort[] p_ExpectedIDs_A0 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_A0 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_A0 = { "[rule_def_atom<grammar_text_terminal> -> grammar_text_terminal . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A0 = { };
        private static ushort[][] p_ShiftsOnVariable_A0 = { };
        private static ushort[][] p_ReducsOnTerminal_A0 = { new ushort[2] { 0x4D, 0x62 }, new ushort[2] { 0x4E, 0x62 }, new ushort[2] { 0x3D, 0x62 }, new ushort[2] { 0x3E, 0x62 }, new ushort[2] { 0x3F, 0x62 }, new ushort[2] { 0x3B, 0x62 }, new ushort[2] { 0x29, 0x62 }, new ushort[2] { 0x8, 0x62 }, new ushort[2] { 0x2A, 0x62 }, new ushort[2] { 0xF, 0x62 }, new ushort[2] { 0x40, 0x62 }, new ushort[2] { 0x41, 0x62 }, new ushort[2] { 0xC7, 0x62 }, new ushort[2] { 0x3C, 0x62 }, new ushort[2] { 0x39, 0x62 }, new ushort[2] { 0x46, 0x62 }, new ushort[2] { 0x45, 0x62 } };

        private static ushort[] p_ExpectedIDs_A1 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45, 0x44 };
        private static string[] p_ExpectedNames_A1 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]", "_T[<]" };
        private static string[] p_Items_A1 = { "[rule_sym_ref_simple -> NAME . ]", "[rule_sym_ref_template<grammar_text_terminal> -> NAME . rule_sym_ref_params<grammar_text_terminal>]", "[rule_sym_ref_params<grammar_text_terminal> -> . < rule_def_atom<grammar_text_terminal> _m119 >]" };
        private static ushort[][] p_ShiftsOnTerminal_A1 = { new ushort[2] { 0x44, 0xDC } };
        private static ushort[][] p_ShiftsOnVariable_A1 = { new ushort[2] { 0x79, 0xDB } };
        private static ushort[][] p_ReducsOnTerminal_A1 = { new ushort[2] { 0x4D, 0x29 }, new ushort[2] { 0x4E, 0x29 }, new ushort[2] { 0x3D, 0x29 }, new ushort[2] { 0x3E, 0x29 }, new ushort[2] { 0x3F, 0x29 }, new ushort[2] { 0x3B, 0x29 }, new ushort[2] { 0x29, 0x29 }, new ushort[2] { 0x8, 0x29 }, new ushort[2] { 0x2A, 0x29 }, new ushort[2] { 0xF, 0x29 }, new ushort[2] { 0x40, 0x29 }, new ushort[2] { 0x41, 0x29 }, new ushort[2] { 0xC7, 0x29 }, new ushort[2] { 0x3C, 0x29 }, new ushort[2] { 0x39, 0x29 }, new ushort[2] { 0x46, 0x29 }, new ushort[2] { 0x45, 0x29 } };

        private static ushort[] p_ExpectedIDs_A2 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_A2 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_A2 = { "[grammar_text_terminal -> terminal_def_atom_text . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A2 = { };
        private static ushort[][] p_ShiftsOnVariable_A2 = { };
        private static ushort[][] p_ReducsOnTerminal_A2 = { new ushort[2] { 0x4D, 0x37 }, new ushort[2] { 0x4E, 0x37 }, new ushort[2] { 0x3D, 0x37 }, new ushort[2] { 0x3E, 0x37 }, new ushort[2] { 0x3F, 0x37 }, new ushort[2] { 0x3B, 0x37 }, new ushort[2] { 0x29, 0x37 }, new ushort[2] { 0x8, 0x37 }, new ushort[2] { 0x2A, 0x37 }, new ushort[2] { 0xF, 0x37 }, new ushort[2] { 0x40, 0x37 }, new ushort[2] { 0x41, 0x37 }, new ushort[2] { 0xC7, 0x37 }, new ushort[2] { 0x3C, 0x37 }, new ushort[2] { 0x39, 0x37 }, new ushort[2] { 0x46, 0x37 }, new ushort[2] { 0x45, 0x37 } };

        private static ushort[] p_ExpectedIDs_A3 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x2C, 0x2B, 0x2D, 0x39, 0x42, 0x3C, 0x46, 0x45 };
        private static string[] p_ExpectedNames_A3 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[;]", "_T[=>]", "_T[)]", "_T[>]", "_T[,]" };
        private static string[] p_Items_A3 = { "[terminal_def_atom_text -> SYMBOL_TERMINAL_TEXT . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A3 = { };
        private static ushort[][] p_ShiftsOnVariable_A3 = { };
        private static ushort[][] p_ReducsOnTerminal_A3 = { new ushort[2] { 0x4D, 0x13 }, new ushort[2] { 0x4E, 0x13 }, new ushort[2] { 0x3D, 0x13 }, new ushort[2] { 0x3E, 0x13 }, new ushort[2] { 0x3F, 0x13 }, new ushort[2] { 0x3B, 0x13 }, new ushort[2] { 0x29, 0x13 }, new ushort[2] { 0x8, 0x13 }, new ushort[2] { 0x2A, 0x13 }, new ushort[2] { 0xF, 0x13 }, new ushort[2] { 0x40, 0x13 }, new ushort[2] { 0x41, 0x13 }, new ushort[2] { 0xC7, 0x13 }, new ushort[2] { 0x2C, 0x13 }, new ushort[2] { 0x2B, 0x13 }, new ushort[2] { 0x2D, 0x13 }, new ushort[2] { 0x39, 0x13 }, new ushort[2] { 0x42, 0x13 }, new ushort[2] { 0x3C, 0x13 }, new ushort[2] { 0x46, 0x13 }, new ushort[2] { 0x45, 0x13 } };

        private static ushort[] p_ExpectedIDs_A4 = { 0x39, 0x42 };
        private static string[] p_ExpectedNames_A4 = { "_T[;]", "_T[=>]" };
        private static string[] p_Items_A4 = { "[terminal -> NAME -> terminal_definition . terminal_subgrammar ;]", "[terminal_subgrammar -> . => qualified_name]", "[terminal_subgrammar -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A4 = { new ushort[2] { 0x42, 0xDE } };
        private static ushort[][] p_ShiftsOnVariable_A4 = { new ushort[2] { 0x5A, 0xDD } };
        private static ushort[][] p_ReducsOnTerminal_A4 = { new ushort[2] { 0x39, 0x25 } };

        private static ushort[] p_ExpectedIDs_A5 = { 0x39, 0x42, 0x3C, 0x41 };
        private static string[] p_ExpectedNames_A5 = { "_T[;]", "_T[=>]", "_T[)]", "_T[|]" };
        private static string[] p_Items_A5 = { "[terminal_definition -> terminal_def_restrict . _m79]", "[_m79 -> . | terminal_def_restrict _m79]", "[_m79 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A5 = { new ushort[2] { 0x41, 0xE0 } };
        private static ushort[][] p_ShiftsOnVariable_A5 = { new ushort[2] { 0x6A, 0xDF } };
        private static ushort[][] p_ReducsOnTerminal_A5 = { new ushort[2] { 0x39, 0x47 }, new ushort[2] { 0x42, 0x47 }, new ushort[2] { 0x3C, 0x47 } };

        private static ushort[] p_ExpectedIDs_A6 = { 0x41, 0x39, 0x42, 0x3C, 0x40 };
        private static string[] p_ExpectedNames_A6 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[-]" };
        private static string[] p_Items_A6 = { "[terminal_def_restrict -> terminal_def_fragment . _m77]", "[_m77 -> . - terminal_def_fragment _m77]", "[_m77 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A6 = { new ushort[2] { 0x40, 0xE2 } };
        private static ushort[][] p_ShiftsOnVariable_A6 = { new ushort[2] { 0x69, 0xE1 } };
        private static ushort[][] p_ReducsOnTerminal_A6 = { new ushort[2] { 0x41, 0x45 }, new ushort[2] { 0x39, 0x45 }, new ushort[2] { 0x42, 0x45 }, new ushort[2] { 0x3C, 0x45 } };

        private static ushort[] p_ExpectedIDs_A7 = { 0x40, 0x41, 0x39, 0x42, 0x3C, 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_A7 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_A7 = { "[terminal_def_fragment -> terminal_def_repetition . _m75]", "[_m75 -> . terminal_def_repetition _m75]", "[_m75 -> . ]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_A7 = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_A7 = { new ushort[2] { 0x68, 0xE3 }, new ushort[2] { 0x56, 0xE4 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_A7 = { new ushort[2] { 0x40, 0x43 }, new ushort[2] { 0x41, 0x43 }, new ushort[2] { 0x39, 0x43 }, new ushort[2] { 0x42, 0x43 }, new ushort[2] { 0x3C, 0x43 } };

        private static ushort[] p_ExpectedIDs_A8 = { 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C, 0x3D, 0x3E, 0x3F };
        private static string[] p_ExpectedNames_A8 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[*]", "_T[+]", "_T[?]" };
        private static string[] p_Items_A8 = { "[terminal_def_repetition -> terminal_def_element . *]", "[terminal_def_repetition -> terminal_def_element . +]", "[terminal_def_repetition -> terminal_def_element . ?]", "[terminal_def_repetition -> terminal_def_element . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A8 = { new ushort[2] { 0x3D, 0xE5 }, new ushort[2] { 0x3E, 0xE6 }, new ushort[2] { 0x3F, 0xE7 } };
        private static ushort[][] p_ShiftsOnVariable_A8 = { };
        private static ushort[][] p_ReducsOnTerminal_A8 = { new ushort[2] { 0x8, 0x20 }, new ushort[2] { 0x3B, 0x20 }, new ushort[2] { 0x2C, 0x20 }, new ushort[2] { 0x2A, 0x20 }, new ushort[2] { 0x2B, 0x20 }, new ushort[2] { 0x2D, 0x20 }, new ushort[2] { 0x40, 0x20 }, new ushort[2] { 0x41, 0x20 }, new ushort[2] { 0x39, 0x20 }, new ushort[2] { 0x42, 0x20 }, new ushort[2] { 0x3C, 0x20 } };

        private static ushort[] p_ExpectedIDs_A9 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_A9 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_A9 = { "[terminal_def_element -> terminal_def_atom . ]" };
        private static ushort[][] p_ShiftsOnTerminal_A9 = { };
        private static ushort[][] p_ShiftsOnVariable_A9 = { };
        private static ushort[][] p_ReducsOnTerminal_A9 = { new ushort[2] { 0x3D, 0x1B }, new ushort[2] { 0x3E, 0x1B }, new ushort[2] { 0x3F, 0x1B }, new ushort[2] { 0x8, 0x1B }, new ushort[2] { 0x3B, 0x1B }, new ushort[2] { 0x2C, 0x1B }, new ushort[2] { 0x2A, 0x1B }, new ushort[2] { 0x2B, 0x1B }, new ushort[2] { 0x2D, 0x1B }, new ushort[2] { 0x40, 0x1B }, new ushort[2] { 0x41, 0x1B }, new ushort[2] { 0x39, 0x1B }, new ushort[2] { 0x42, 0x1B }, new ushort[2] { 0x3C, 0x1B } };

        private static ushort[] p_ExpectedIDs_AA = { 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_AA = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_AA = { "[terminal_def_element -> ( . terminal_definition )]", "[terminal_definition -> . terminal_def_restrict _m79]", "[terminal_def_restrict -> . terminal_def_fragment _m77]", "[terminal_def_fragment -> . terminal_def_repetition _m75]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_AA = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_AA = { new ushort[2] { 0x59, 0xE8 }, new ushort[2] { 0x58, 0xA5 }, new ushort[2] { 0x57, 0xA6 }, new ushort[2] { 0x56, 0xA7 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_AA = { };

        private static ushort[] p_ExpectedIDs_AB = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C, 0x3A };
        private static string[] p_ExpectedNames_AB = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[..]" };
        private static string[] p_Items_AB = { "[terminal_def_atom -> terminal_def_atom_unicode . ]", "[terminal_def_atom_span -> terminal_def_atom_unicode . .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_AB = { new ushort[2] { 0x3A, 0xE9 } };
        private static ushort[][] p_ShiftsOnVariable_AB = { };
        private static ushort[][] p_ReducsOnTerminal_AB = { new ushort[2] { 0x3D, 0x16 }, new ushort[2] { 0x3E, 0x16 }, new ushort[2] { 0x3F, 0x16 }, new ushort[2] { 0x8, 0x16 }, new ushort[2] { 0x3B, 0x16 }, new ushort[2] { 0x2C, 0x16 }, new ushort[2] { 0x2A, 0x16 }, new ushort[2] { 0x2B, 0x16 }, new ushort[2] { 0x2D, 0x16 }, new ushort[2] { 0x40, 0x16 }, new ushort[2] { 0x41, 0x16 }, new ushort[2] { 0x39, 0x16 }, new ushort[2] { 0x42, 0x16 }, new ushort[2] { 0x3C, 0x16 } };

        private static ushort[] p_ExpectedIDs_AC = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_AC = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_AC = { "[terminal_def_atom -> terminal_def_atom_text . ]" };
        private static ushort[][] p_ShiftsOnTerminal_AC = { };
        private static ushort[][] p_ShiftsOnVariable_AC = { };
        private static ushort[][] p_ReducsOnTerminal_AC = { new ushort[2] { 0x3D, 0x17 }, new ushort[2] { 0x3E, 0x17 }, new ushort[2] { 0x3F, 0x17 }, new ushort[2] { 0x8, 0x17 }, new ushort[2] { 0x3B, 0x17 }, new ushort[2] { 0x2C, 0x17 }, new ushort[2] { 0x2A, 0x17 }, new ushort[2] { 0x2B, 0x17 }, new ushort[2] { 0x2D, 0x17 }, new ushort[2] { 0x40, 0x17 }, new ushort[2] { 0x41, 0x17 }, new ushort[2] { 0x39, 0x17 }, new ushort[2] { 0x42, 0x17 }, new ushort[2] { 0x3C, 0x17 } };

        private static ushort[] p_ExpectedIDs_AD = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_AD = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_AD = { "[terminal_def_atom -> terminal_def_atom_set . ]" };
        private static ushort[][] p_ShiftsOnTerminal_AD = { };
        private static ushort[][] p_ShiftsOnVariable_AD = { };
        private static ushort[][] p_ReducsOnTerminal_AD = { new ushort[2] { 0x3D, 0x18 }, new ushort[2] { 0x3E, 0x18 }, new ushort[2] { 0x3F, 0x18 }, new ushort[2] { 0x8, 0x18 }, new ushort[2] { 0x3B, 0x18 }, new ushort[2] { 0x2C, 0x18 }, new ushort[2] { 0x2A, 0x18 }, new ushort[2] { 0x2B, 0x18 }, new ushort[2] { 0x2D, 0x18 }, new ushort[2] { 0x40, 0x18 }, new ushort[2] { 0x41, 0x18 }, new ushort[2] { 0x39, 0x18 }, new ushort[2] { 0x42, 0x18 }, new ushort[2] { 0x3C, 0x18 } };

        private static ushort[] p_ExpectedIDs_AE = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_AE = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_AE = { "[terminal_def_atom -> terminal_def_atom_span . ]" };
        private static ushort[][] p_ShiftsOnTerminal_AE = { };
        private static ushort[][] p_ShiftsOnVariable_AE = { };
        private static ushort[][] p_ReducsOnTerminal_AE = { new ushort[2] { 0x3D, 0x19 }, new ushort[2] { 0x3E, 0x19 }, new ushort[2] { 0x3F, 0x19 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x3B, 0x19 }, new ushort[2] { 0x2C, 0x19 }, new ushort[2] { 0x2A, 0x19 }, new ushort[2] { 0x2B, 0x19 }, new ushort[2] { 0x2D, 0x19 }, new ushort[2] { 0x40, 0x19 }, new ushort[2] { 0x41, 0x19 }, new ushort[2] { 0x39, 0x19 }, new ushort[2] { 0x42, 0x19 }, new ushort[2] { 0x3C, 0x19 } };

        private static ushort[] p_ExpectedIDs_AF = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_AF = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_AF = { "[terminal_def_atom -> NAME . ]" };
        private static ushort[][] p_ShiftsOnTerminal_AF = { };
        private static ushort[][] p_ShiftsOnVariable_AF = { };
        private static ushort[][] p_ReducsOnTerminal_AF = { new ushort[2] { 0x3D, 0x1A }, new ushort[2] { 0x3E, 0x1A }, new ushort[2] { 0x3F, 0x1A }, new ushort[2] { 0x8, 0x1A }, new ushort[2] { 0x3B, 0x1A }, new ushort[2] { 0x2C, 0x1A }, new ushort[2] { 0x2A, 0x1A }, new ushort[2] { 0x2B, 0x1A }, new ushort[2] { 0x2D, 0x1A }, new ushort[2] { 0x40, 0x1A }, new ushort[2] { 0x41, 0x1A }, new ushort[2] { 0x39, 0x1A }, new ushort[2] { 0x42, 0x1A }, new ushort[2] { 0x3C, 0x1A } };

        private static ushort[] p_ExpectedIDs_B0 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3A, 0x3C };
        private static string[] p_ExpectedNames_B0 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[..]", "_T[)]" };
        private static string[] p_Items_B0 = { "[terminal_def_atom_unicode -> SYMBOL_VALUE_UINT8 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B0 = { };
        private static ushort[][] p_ShiftsOnVariable_B0 = { };
        private static ushort[][] p_ReducsOnTerminal_B0 = { new ushort[2] { 0x3D, 0x11 }, new ushort[2] { 0x3E, 0x11 }, new ushort[2] { 0x3F, 0x11 }, new ushort[2] { 0x8, 0x11 }, new ushort[2] { 0x3B, 0x11 }, new ushort[2] { 0x2C, 0x11 }, new ushort[2] { 0x2A, 0x11 }, new ushort[2] { 0x2B, 0x11 }, new ushort[2] { 0x2D, 0x11 }, new ushort[2] { 0x40, 0x11 }, new ushort[2] { 0x41, 0x11 }, new ushort[2] { 0x39, 0x11 }, new ushort[2] { 0x42, 0x11 }, new ushort[2] { 0x3A, 0x11 }, new ushort[2] { 0x3C, 0x11 } };

        private static ushort[] p_ExpectedIDs_B1 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3A, 0x3C };
        private static string[] p_ExpectedNames_B1 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[..]", "_T[)]" };
        private static string[] p_Items_B1 = { "[terminal_def_atom_unicode -> SYMBOL_VALUE_UINT16 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B1 = { };
        private static ushort[][] p_ShiftsOnVariable_B1 = { };
        private static ushort[][] p_ReducsOnTerminal_B1 = { new ushort[2] { 0x3D, 0x12 }, new ushort[2] { 0x3E, 0x12 }, new ushort[2] { 0x3F, 0x12 }, new ushort[2] { 0x8, 0x12 }, new ushort[2] { 0x3B, 0x12 }, new ushort[2] { 0x2C, 0x12 }, new ushort[2] { 0x2A, 0x12 }, new ushort[2] { 0x2B, 0x12 }, new ushort[2] { 0x2D, 0x12 }, new ushort[2] { 0x40, 0x12 }, new ushort[2] { 0x41, 0x12 }, new ushort[2] { 0x39, 0x12 }, new ushort[2] { 0x42, 0x12 }, new ushort[2] { 0x3A, 0x12 }, new ushort[2] { 0x3C, 0x12 } };

        private static ushort[] p_ExpectedIDs_B2 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_B2 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_B2 = { "[terminal_def_atom_set -> SYMBOL_TERMINAL_SET . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B2 = { };
        private static ushort[][] p_ShiftsOnVariable_B2 = { };
        private static ushort[][] p_ReducsOnTerminal_B2 = { new ushort[2] { 0x3D, 0x14 }, new ushort[2] { 0x3E, 0x14 }, new ushort[2] { 0x3F, 0x14 }, new ushort[2] { 0x8, 0x14 }, new ushort[2] { 0x3B, 0x14 }, new ushort[2] { 0x2C, 0x14 }, new ushort[2] { 0x2A, 0x14 }, new ushort[2] { 0x2B, 0x14 }, new ushort[2] { 0x2D, 0x14 }, new ushort[2] { 0x40, 0x14 }, new ushort[2] { 0x41, 0x14 }, new ushort[2] { 0x39, 0x14 }, new ushort[2] { 0x42, 0x14 }, new ushort[2] { 0x3C, 0x14 } };

        private static ushort[] p_ExpectedIDs_B3 = { 0x43, 0x44 };
        private static string[] p_ExpectedNames_B3 = { "_T[->]", "_T[<]" };
        private static string[] p_Items_B3 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> . -> rule_definition<grammar_bin_terminal> ;]", "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> . rule_template_params -> rule_definition<grammar_bin_terminal> ;]", "[rule_template_params -> . < NAME _m87 >]" };
        private static ushort[][] p_ShiftsOnTerminal_B3 = { new ushort[2] { 0x43, 0xEA }, new ushort[2] { 0x44, 0x8F } };
        private static ushort[][] p_ShiftsOnVariable_B3 = { new ushort[2] { 0x5F, 0xEB } };
        private static ushort[][] p_ReducsOnTerminal_B3 = { };

        private static ushort[] p_ExpectedIDs_B4 = { 0x8, 0x43, 0x44 };
        private static string[] p_ExpectedNames_B4 = { "NAME", "_T[->]", "_T[<]" };
        private static string[] p_Items_B4 = { "[cs_rule_context<grammar_bin_terminal> -> [ rule_definition<grammar_bin_terminal> ] . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B4 = { };
        private static ushort[][] p_ShiftsOnVariable_B4 = { };
        private static ushort[][] p_ReducsOnTerminal_B4 = { new ushort[2] { 0x8, 0x9E }, new ushort[2] { 0x43, 0x9E }, new ushort[2] { 0x44, 0x9E } };

        private static ushort[] p_ExpectedIDs_B5 = { 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_B5 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_B5 = { "[rule_definition<grammar_bin_terminal> -> rule_def_restrict<grammar_bin_terminal> _m172 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B5 = { };
        private static ushort[][] p_ShiftsOnVariable_B5 = { };
        private static ushort[][] p_ReducsOnTerminal_B5 = { new ushort[2] { 0xC7, 0x73 }, new ushort[2] { 0x3C, 0x73 }, new ushort[2] { 0x39, 0x73 } };

        private static ushort[] p_ExpectedIDs_B6 = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_B6 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_B6 = { "[_m172 -> | . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_B6 = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_B6 = { new ushort[2] { 0x83, 0xEC }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_B6 = { };

        private static ushort[] p_ExpectedIDs_B7 = { 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_B7 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_B7 = { "[rule_def_restrict<grammar_bin_terminal> -> rule_def_fragment<grammar_bin_terminal> _m170 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B7 = { };
        private static ushort[][] p_ShiftsOnVariable_B7 = { };
        private static ushort[][] p_ReducsOnTerminal_B7 = { new ushort[2] { 0x41, 0x74 }, new ushort[2] { 0xC7, 0x74 }, new ushort[2] { 0x3C, 0x74 }, new ushort[2] { 0x39, 0x74 } };

        private static ushort[] p_ExpectedIDs_B8 = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_B8 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_B8 = { "[_m170 -> - . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_B8 = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_B8 = { new ushort[2] { 0x84, 0xED }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_B8 = { };

        private static ushort[] p_ExpectedIDs_B9 = { 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_B9 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_B9 = { "[rule_def_fragment<grammar_bin_terminal> -> rule_def_repetition<grammar_bin_terminal> _m168 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_B9 = { };
        private static ushort[][] p_ShiftsOnVariable_B9 = { };
        private static ushort[][] p_ReducsOnTerminal_B9 = { new ushort[2] { 0x40, 0x75 }, new ushort[2] { 0x41, 0x75 }, new ushort[2] { 0xC7, 0x75 }, new ushort[2] { 0x3C, 0x75 }, new ushort[2] { 0x39, 0x75 } };

        private static ushort[] p_ExpectedIDs_BA = { 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_BA = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_BA = { "[_m168 -> rule_def_repetition<grammar_bin_terminal> . _m168]", "[_m168 -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[_m168 -> . ]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_BA = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_BA = { new ushort[2] { 0x8C, 0xEE }, new ushort[2] { 0x85, 0xBA }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_BA = { new ushort[2] { 0x40, 0x89 }, new ushort[2] { 0x41, 0x89 }, new ushort[2] { 0xC7, 0x89 }, new ushort[2] { 0x3C, 0x89 }, new ushort[2] { 0x39, 0x89 } };

        private static ushort[] p_ExpectedIDs_BB = { 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_BB = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_BB = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> * . ]" };
        private static ushort[][] p_ShiftsOnTerminal_BB = { };
        private static ushort[][] p_ShiftsOnVariable_BB = { };
        private static ushort[][] p_ReducsOnTerminal_BB = { new ushort[2] { 0x3B, 0x76 }, new ushort[2] { 0x29, 0x76 }, new ushort[2] { 0x8, 0x76 }, new ushort[2] { 0x2C, 0x76 }, new ushort[2] { 0x2D, 0x76 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x2E, 0x76 }, new ushort[2] { 0x2F, 0x76 }, new ushort[2] { 0x30, 0x76 }, new ushort[2] { 0x31, 0x76 }, new ushort[2] { 0x32, 0x76 }, new ushort[2] { 0x33, 0x76 }, new ushort[2] { 0x34, 0x76 }, new ushort[2] { 0x35, 0x76 }, new ushort[2] { 0x36, 0x76 }, new ushort[2] { 0x37, 0x76 }, new ushort[2] { 0x40, 0x76 }, new ushort[2] { 0x41, 0x76 }, new ushort[2] { 0xC7, 0x76 }, new ushort[2] { 0x3C, 0x76 }, new ushort[2] { 0x39, 0x76 } };

        private static ushort[] p_ExpectedIDs_BC = { 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_BC = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_BC = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> + . ]" };
        private static ushort[][] p_ShiftsOnTerminal_BC = { };
        private static ushort[][] p_ShiftsOnVariable_BC = { };
        private static ushort[][] p_ReducsOnTerminal_BC = { new ushort[2] { 0x3B, 0x77 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x77 }, new ushort[2] { 0x2C, 0x77 }, new ushort[2] { 0x2D, 0x77 }, new ushort[2] { 0xF, 0x77 }, new ushort[2] { 0x2E, 0x77 }, new ushort[2] { 0x2F, 0x77 }, new ushort[2] { 0x30, 0x77 }, new ushort[2] { 0x31, 0x77 }, new ushort[2] { 0x32, 0x77 }, new ushort[2] { 0x33, 0x77 }, new ushort[2] { 0x34, 0x77 }, new ushort[2] { 0x35, 0x77 }, new ushort[2] { 0x36, 0x77 }, new ushort[2] { 0x37, 0x77 }, new ushort[2] { 0x40, 0x77 }, new ushort[2] { 0x41, 0x77 }, new ushort[2] { 0xC7, 0x77 }, new ushort[2] { 0x3C, 0x77 }, new ushort[2] { 0x39, 0x77 } };

        private static ushort[] p_ExpectedIDs_BD = { 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_BD = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_BD = { "[rule_def_repetition<grammar_bin_terminal> -> rule_def_tree_action<grammar_bin_terminal> ? . ]" };
        private static ushort[][] p_ShiftsOnTerminal_BD = { };
        private static ushort[][] p_ShiftsOnVariable_BD = { };
        private static ushort[][] p_ReducsOnTerminal_BD = { new ushort[2] { 0x3B, 0x78 }, new ushort[2] { 0x29, 0x78 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x78 }, new ushort[2] { 0x2D, 0x78 }, new ushort[2] { 0xF, 0x78 }, new ushort[2] { 0x2E, 0x78 }, new ushort[2] { 0x2F, 0x78 }, new ushort[2] { 0x30, 0x78 }, new ushort[2] { 0x31, 0x78 }, new ushort[2] { 0x32, 0x78 }, new ushort[2] { 0x33, 0x78 }, new ushort[2] { 0x34, 0x78 }, new ushort[2] { 0x35, 0x78 }, new ushort[2] { 0x36, 0x78 }, new ushort[2] { 0x37, 0x78 }, new ushort[2] { 0x40, 0x78 }, new ushort[2] { 0x41, 0x78 }, new ushort[2] { 0xC7, 0x78 }, new ushort[2] { 0x3C, 0x78 }, new ushort[2] { 0x39, 0x78 } };

        private static ushort[] p_ExpectedIDs_BE = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_BE = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_BE = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> ^ . ]" };
        private static ushort[][] p_ShiftsOnTerminal_BE = { };
        private static ushort[][] p_ShiftsOnVariable_BE = { };
        private static ushort[][] p_ReducsOnTerminal_BE = { new ushort[2] { 0x3D, 0x7A }, new ushort[2] { 0x3E, 0x7A }, new ushort[2] { 0x3F, 0x7A }, new ushort[2] { 0x3B, 0x7A }, new ushort[2] { 0x29, 0x7A }, new ushort[2] { 0x8, 0x7A }, new ushort[2] { 0x2C, 0x7A }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0xF, 0x7A }, new ushort[2] { 0x2E, 0x7A }, new ushort[2] { 0x2F, 0x7A }, new ushort[2] { 0x30, 0x7A }, new ushort[2] { 0x31, 0x7A }, new ushort[2] { 0x32, 0x7A }, new ushort[2] { 0x33, 0x7A }, new ushort[2] { 0x34, 0x7A }, new ushort[2] { 0x35, 0x7A }, new ushort[2] { 0x36, 0x7A }, new ushort[2] { 0x37, 0x7A }, new ushort[2] { 0x40, 0x7A }, new ushort[2] { 0x41, 0x7A }, new ushort[2] { 0xC7, 0x7A }, new ushort[2] { 0x3C, 0x7A }, new ushort[2] { 0x39, 0x7A } };

        private static ushort[] p_ExpectedIDs_BF = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_BF = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_BF = { "[rule_def_tree_action<grammar_bin_terminal> -> rule_def_element<grammar_bin_terminal> ! . ]" };
        private static ushort[][] p_ShiftsOnTerminal_BF = { };
        private static ushort[][] p_ShiftsOnVariable_BF = { };
        private static ushort[][] p_ReducsOnTerminal_BF = { new ushort[2] { 0x3D, 0x7B }, new ushort[2] { 0x3E, 0x7B }, new ushort[2] { 0x3F, 0x7B }, new ushort[2] { 0x3B, 0x7B }, new ushort[2] { 0x29, 0x7B }, new ushort[2] { 0x8, 0x7B }, new ushort[2] { 0x2C, 0x7B }, new ushort[2] { 0x2D, 0x7B }, new ushort[2] { 0xF, 0x7B }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7B }, new ushort[2] { 0x30, 0x7B }, new ushort[2] { 0x31, 0x7B }, new ushort[2] { 0x32, 0x7B }, new ushort[2] { 0x33, 0x7B }, new ushort[2] { 0x34, 0x7B }, new ushort[2] { 0x35, 0x7B }, new ushort[2] { 0x36, 0x7B }, new ushort[2] { 0x37, 0x7B }, new ushort[2] { 0x40, 0x7B }, new ushort[2] { 0x41, 0x7B }, new ushort[2] { 0xC7, 0x7B }, new ushort[2] { 0x3C, 0x7B }, new ushort[2] { 0x39, 0x7B } };

        private static ushort[] p_ExpectedIDs_C0 = { 0x3C };
        private static string[] p_ExpectedNames_C0 = { "_T[)]" };
        private static string[] p_Items_C0 = { "[rule_def_element<grammar_bin_terminal> -> ( rule_definition<grammar_bin_terminal> . )]" };
        private static ushort[][] p_ShiftsOnTerminal_C0 = { new ushort[2] { 0x3C, 0xEF } };
        private static ushort[][] p_ShiftsOnVariable_C0 = { };
        private static ushort[][] p_ReducsOnTerminal_C0 = { };

        private static ushort[] p_ExpectedIDs_C1 = { 0x10 };
        private static string[] p_ExpectedNames_C1 = { "_T[}]" };
        private static string[] p_Items_C1 = { "[rule_sym_action -> { qualified_name . }]" };
        private static ushort[][] p_ShiftsOnTerminal_C1 = { new ushort[2] { 0x10, 0xF0 } };
        private static ushort[][] p_ShiftsOnVariable_C1 = { };
        private static ushort[][] p_ReducsOnTerminal_C1 = { };

        private static ushort[] p_ExpectedIDs_C2 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_C2 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_C2 = { "[rule_sym_ref_template<grammar_bin_terminal> -> NAME rule_sym_ref_params<grammar_bin_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C2 = { };
        private static ushort[][] p_ShiftsOnVariable_C2 = { };
        private static ushort[][] p_ReducsOnTerminal_C2 = { new ushort[2] { 0x4D, 0x84 }, new ushort[2] { 0x4E, 0x84 }, new ushort[2] { 0x3D, 0x84 }, new ushort[2] { 0x3E, 0x84 }, new ushort[2] { 0x3F, 0x84 }, new ushort[2] { 0x3B, 0x84 }, new ushort[2] { 0x29, 0x84 }, new ushort[2] { 0x8, 0x84 }, new ushort[2] { 0x2C, 0x84 }, new ushort[2] { 0x2D, 0x84 }, new ushort[2] { 0xF, 0x84 }, new ushort[2] { 0x2E, 0x84 }, new ushort[2] { 0x2F, 0x84 }, new ushort[2] { 0x30, 0x84 }, new ushort[2] { 0x31, 0x84 }, new ushort[2] { 0x32, 0x84 }, new ushort[2] { 0x33, 0x84 }, new ushort[2] { 0x34, 0x84 }, new ushort[2] { 0x35, 0x84 }, new ushort[2] { 0x36, 0x84 }, new ushort[2] { 0x37, 0x84 }, new ushort[2] { 0x40, 0x84 }, new ushort[2] { 0x41, 0x84 }, new ushort[2] { 0xC7, 0x84 }, new ushort[2] { 0x3C, 0x84 }, new ushort[2] { 0x39, 0x84 }, new ushort[2] { 0x46, 0x84 }, new ushort[2] { 0x45, 0x84 } };

        private static ushort[] p_ExpectedIDs_C3 = { 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_C3 = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_C3 = { "[rule_sym_ref_params<grammar_bin_terminal> -> < . rule_def_atom<grammar_bin_terminal> _m159 >]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_C3 = { new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_C3 = { new ushort[2] { 0x88, 0xF1 }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_C3 = { };

        private static ushort[] p_ExpectedIDs_C4 = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_C4 = { "NAME", "_T[}]" };
        private static string[] p_Items_C4 = { "[option -> NAME = QUOTED_DATA ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C4 = { };
        private static ushort[][] p_ShiftsOnVariable_C4 = { };
        private static ushort[][] p_ReducsOnTerminal_C4 = { new ushort[2] { 0x8, 0x10 }, new ushort[2] { 0x10, 0x10 } };

        private static ushort[] p_ExpectedIDs_C5 = { 0x10 };
        private static string[] p_ExpectedNames_C5 = { "_T[}]" };
        private static string[] p_Items_C5 = { "[grammar_cf_rules<grammar_text_terminal> -> rules { _m137 } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C5 = { };
        private static ushort[][] p_ShiftsOnVariable_C5 = { };
        private static ushort[][] p_ReducsOnTerminal_C5 = { new ushort[2] { 0x10, 0x50 } };

        private static ushort[] p_ExpectedIDs_C6 = { 0x10 };
        private static string[] p_ExpectedNames_C6 = { "_T[}]" };
        private static string[] p_Items_C6 = { "[_m137 -> cf_rule_simple<grammar_text_terminal> _m137 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C6 = { };
        private static ushort[][] p_ShiftsOnVariable_C6 = { };
        private static ushort[][] p_ReducsOnTerminal_C6 = { new ushort[2] { 0x10, 0x6E } };

        private static ushort[] p_ExpectedIDs_C7 = { 0x10 };
        private static string[] p_ExpectedNames_C7 = { "_T[}]" };
        private static string[] p_Items_C7 = { "[_m137 -> cf_rule_template<grammar_text_terminal> _m137 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_C7 = { };
        private static ushort[][] p_ShiftsOnVariable_C7 = { };
        private static ushort[][] p_ReducsOnTerminal_C7 = { new ushort[2] { 0x10, 0x6F } };

        private static ushort[] p_ExpectedIDs_C8 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_C8 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_C8 = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_C8 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_C8 = { new ushort[2] { 0x71, 0xF2 }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_C8 = { };

        private static ushort[] p_ExpectedIDs_C9 = { 0x43 };
        private static string[] p_ExpectedNames_C9 = { "_T[->]" };
        private static string[] p_Items_C9 = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params . -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_C9 = { new ushort[2] { 0x43, 0xF3 } };
        private static ushort[][] p_ShiftsOnVariable_C9 = { };
        private static ushort[][] p_ReducsOnTerminal_C9 = { };

        private static ushort[] p_ExpectedIDs_CA = { 0x39 };
        private static string[] p_ExpectedNames_CA = { "_T[;]" };
        private static string[] p_Items_CA = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_CA = { new ushort[2] { 0x39, 0xF4 } };
        private static ushort[][] p_ShiftsOnVariable_CA = { };
        private static ushort[][] p_ReducsOnTerminal_CA = { };

        private static ushort[] p_ExpectedIDs_CB = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_CB = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_CB = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_CB = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_CB = { new ushort[2] { 0x82, 0xF5 }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_CB = { };

        private static ushort[] p_ExpectedIDs_CC = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_CC = { "_T[>]", "_T[,]" };
        private static string[] p_Items_CC = { "[rule_template_params -> < NAME . _m87 >]", "[_m87 -> . , NAME _m87]", "[_m87 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_CC = { new ushort[2] { 0x45, 0xF7 } };
        private static ushort[][] p_ShiftsOnVariable_CC = { new ushort[2] { 0x6B, 0xF6 } };
        private static ushort[][] p_ReducsOnTerminal_CC = { new ushort[2] { 0x46, 0x49 } };

        private static ushort[] p_ExpectedIDs_CD = { 0x43, 0x44 };
        private static string[] p_ExpectedNames_CD = { "_T[->]", "_T[<]" };
        private static string[] p_Items_CD = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> . -> rule_definition<grammar_text_terminal> ;]", "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> . rule_template_params -> rule_definition<grammar_text_terminal> ;]", "[rule_template_params -> . < NAME _m87 >]" };
        private static ushort[][] p_ShiftsOnTerminal_CD = { new ushort[2] { 0x43, 0xF8 }, new ushort[2] { 0x44, 0x8F } };
        private static ushort[][] p_ShiftsOnVariable_CD = { new ushort[2] { 0x5F, 0xF9 } };
        private static ushort[][] p_ReducsOnTerminal_CD = { };

        private static ushort[] p_ExpectedIDs_CE = { 0x8, 0x43, 0x44 };
        private static string[] p_ExpectedNames_CE = { "NAME", "_T[->]", "_T[<]" };
        private static string[] p_Items_CE = { "[cs_rule_context<grammar_text_terminal> -> [ rule_definition<grammar_text_terminal> ] . ]" };
        private static ushort[][] p_ShiftsOnTerminal_CE = { };
        private static ushort[][] p_ShiftsOnVariable_CE = { };
        private static ushort[][] p_ReducsOnTerminal_CE = { new ushort[2] { 0x8, 0x96 }, new ushort[2] { 0x43, 0x96 }, new ushort[2] { 0x44, 0x96 } };

        private static ushort[] p_ExpectedIDs_CF = { 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_CF = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_CF = { "[rule_definition<grammar_text_terminal> -> rule_def_restrict<grammar_text_terminal> _m132 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_CF = { };
        private static ushort[][] p_ShiftsOnVariable_CF = { };
        private static ushort[][] p_ReducsOnTerminal_CF = { new ushort[2] { 0xC7, 0x52 }, new ushort[2] { 0x3C, 0x52 }, new ushort[2] { 0x39, 0x52 } };

        private static ushort[] p_ExpectedIDs_D0 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_D0 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_D0 = { "[_m132 -> | . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_D0 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_D0 = { new ushort[2] { 0x72, 0xFA }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_D0 = { };

        private static ushort[] p_ExpectedIDs_D1 = { 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D1 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D1 = { "[rule_def_restrict<grammar_text_terminal> -> rule_def_fragment<grammar_text_terminal> _m130 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D1 = { };
        private static ushort[][] p_ShiftsOnVariable_D1 = { };
        private static ushort[][] p_ReducsOnTerminal_D1 = { new ushort[2] { 0x41, 0x53 }, new ushort[2] { 0xC7, 0x53 }, new ushort[2] { 0x3C, 0x53 }, new ushort[2] { 0x39, 0x53 } };

        private static ushort[] p_ExpectedIDs_D2 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_D2 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_D2 = { "[_m130 -> - . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_D2 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_D2 = { new ushort[2] { 0x73, 0xFB }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_D2 = { };

        private static ushort[] p_ExpectedIDs_D3 = { 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D3 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D3 = { "[rule_def_fragment<grammar_text_terminal> -> rule_def_repetition<grammar_text_terminal> _m128 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D3 = { };
        private static ushort[][] p_ShiftsOnVariable_D3 = { };
        private static ushort[][] p_ReducsOnTerminal_D3 = { new ushort[2] { 0x40, 0x54 }, new ushort[2] { 0x41, 0x54 }, new ushort[2] { 0xC7, 0x54 }, new ushort[2] { 0x3C, 0x54 }, new ushort[2] { 0x39, 0x54 } };

        private static ushort[] p_ExpectedIDs_D4 = { 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_D4 = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_D4 = { "[_m128 -> rule_def_repetition<grammar_text_terminal> . _m128]", "[_m128 -> . rule_def_repetition<grammar_text_terminal> _m128]", "[_m128 -> . ]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_D4 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_D4 = { new ushort[2] { 0x7B, 0xFC }, new ushort[2] { 0x74, 0xD4 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_D4 = { new ushort[2] { 0x40, 0x68 }, new ushort[2] { 0x41, 0x68 }, new ushort[2] { 0xC7, 0x68 }, new ushort[2] { 0x3C, 0x68 }, new ushort[2] { 0x39, 0x68 } };

        private static ushort[] p_ExpectedIDs_D5 = { 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D5 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D5 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> * . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D5 = { };
        private static ushort[][] p_ShiftsOnVariable_D5 = { };
        private static ushort[][] p_ReducsOnTerminal_D5 = { new ushort[2] { 0x3B, 0x55 }, new ushort[2] { 0x29, 0x55 }, new ushort[2] { 0x8, 0x55 }, new ushort[2] { 0x2A, 0x55 }, new ushort[2] { 0xF, 0x55 }, new ushort[2] { 0x40, 0x55 }, new ushort[2] { 0x41, 0x55 }, new ushort[2] { 0xC7, 0x55 }, new ushort[2] { 0x3C, 0x55 }, new ushort[2] { 0x39, 0x55 } };

        private static ushort[] p_ExpectedIDs_D6 = { 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D6 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D6 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> + . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D6 = { };
        private static ushort[][] p_ShiftsOnVariable_D6 = { };
        private static ushort[][] p_ReducsOnTerminal_D6 = { new ushort[2] { 0x3B, 0x56 }, new ushort[2] { 0x29, 0x56 }, new ushort[2] { 0x8, 0x56 }, new ushort[2] { 0x2A, 0x56 }, new ushort[2] { 0xF, 0x56 }, new ushort[2] { 0x40, 0x56 }, new ushort[2] { 0x41, 0x56 }, new ushort[2] { 0xC7, 0x56 }, new ushort[2] { 0x3C, 0x56 }, new ushort[2] { 0x39, 0x56 } };

        private static ushort[] p_ExpectedIDs_D7 = { 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D7 = { "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D7 = { "[rule_def_repetition<grammar_text_terminal> -> rule_def_tree_action<grammar_text_terminal> ? . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D7 = { };
        private static ushort[][] p_ShiftsOnVariable_D7 = { };
        private static ushort[][] p_ReducsOnTerminal_D7 = { new ushort[2] { 0x3B, 0x57 }, new ushort[2] { 0x29, 0x57 }, new ushort[2] { 0x8, 0x57 }, new ushort[2] { 0x2A, 0x57 }, new ushort[2] { 0xF, 0x57 }, new ushort[2] { 0x40, 0x57 }, new ushort[2] { 0x41, 0x57 }, new ushort[2] { 0xC7, 0x57 }, new ushort[2] { 0x3C, 0x57 }, new ushort[2] { 0x39, 0x57 } };

        private static ushort[] p_ExpectedIDs_D8 = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D8 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D8 = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> ^ . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D8 = { };
        private static ushort[][] p_ShiftsOnVariable_D8 = { };
        private static ushort[][] p_ReducsOnTerminal_D8 = { new ushort[2] { 0x3D, 0x59 }, new ushort[2] { 0x3E, 0x59 }, new ushort[2] { 0x3F, 0x59 }, new ushort[2] { 0x3B, 0x59 }, new ushort[2] { 0x29, 0x59 }, new ushort[2] { 0x8, 0x59 }, new ushort[2] { 0x2A, 0x59 }, new ushort[2] { 0xF, 0x59 }, new ushort[2] { 0x40, 0x59 }, new ushort[2] { 0x41, 0x59 }, new ushort[2] { 0xC7, 0x59 }, new ushort[2] { 0x3C, 0x59 }, new ushort[2] { 0x39, 0x59 } };

        private static ushort[] p_ExpectedIDs_D9 = { 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_D9 = { "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_D9 = { "[rule_def_tree_action<grammar_text_terminal> -> rule_def_element<grammar_text_terminal> ! . ]" };
        private static ushort[][] p_ShiftsOnTerminal_D9 = { };
        private static ushort[][] p_ShiftsOnVariable_D9 = { };
        private static ushort[][] p_ReducsOnTerminal_D9 = { new ushort[2] { 0x3D, 0x5A }, new ushort[2] { 0x3E, 0x5A }, new ushort[2] { 0x3F, 0x5A }, new ushort[2] { 0x3B, 0x5A }, new ushort[2] { 0x29, 0x5A }, new ushort[2] { 0x8, 0x5A }, new ushort[2] { 0x2A, 0x5A }, new ushort[2] { 0xF, 0x5A }, new ushort[2] { 0x40, 0x5A }, new ushort[2] { 0x41, 0x5A }, new ushort[2] { 0xC7, 0x5A }, new ushort[2] { 0x3C, 0x5A }, new ushort[2] { 0x39, 0x5A } };

        private static ushort[] p_ExpectedIDs_DA = { 0x3C };
        private static string[] p_ExpectedNames_DA = { "_T[)]" };
        private static string[] p_Items_DA = { "[rule_def_element<grammar_text_terminal> -> ( rule_definition<grammar_text_terminal> . )]" };
        private static ushort[][] p_ShiftsOnTerminal_DA = { new ushort[2] { 0x3C, 0xFD } };
        private static ushort[][] p_ShiftsOnVariable_DA = { };
        private static ushort[][] p_ReducsOnTerminal_DA = { };

        private static ushort[] p_ExpectedIDs_DB = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_DB = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_DB = { "[rule_sym_ref_template<grammar_text_terminal> -> NAME rule_sym_ref_params<grammar_text_terminal> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_DB = { };
        private static ushort[][] p_ShiftsOnVariable_DB = { };
        private static ushort[][] p_ReducsOnTerminal_DB = { new ushort[2] { 0x4D, 0x63 }, new ushort[2] { 0x4E, 0x63 }, new ushort[2] { 0x3D, 0x63 }, new ushort[2] { 0x3E, 0x63 }, new ushort[2] { 0x3F, 0x63 }, new ushort[2] { 0x3B, 0x63 }, new ushort[2] { 0x29, 0x63 }, new ushort[2] { 0x8, 0x63 }, new ushort[2] { 0x2A, 0x63 }, new ushort[2] { 0xF, 0x63 }, new ushort[2] { 0x40, 0x63 }, new ushort[2] { 0x41, 0x63 }, new ushort[2] { 0xC7, 0x63 }, new ushort[2] { 0x3C, 0x63 }, new ushort[2] { 0x39, 0x63 }, new ushort[2] { 0x46, 0x63 }, new ushort[2] { 0x45, 0x63 } };

        private static ushort[] p_ExpectedIDs_DC = { 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_DC = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_DC = { "[rule_sym_ref_params<grammar_text_terminal> -> < . rule_def_atom<grammar_text_terminal> _m119 >]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_DC = { new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_DC = { new ushort[2] { 0x77, 0xFE }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_DC = { };

        private static ushort[] p_ExpectedIDs_DD = { 0x39 };
        private static string[] p_ExpectedNames_DD = { "_T[;]" };
        private static string[] p_Items_DD = { "[terminal -> NAME -> terminal_definition terminal_subgrammar . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_DD = { new ushort[2] { 0x39, 0xFF } };
        private static ushort[][] p_ShiftsOnVariable_DD = { };
        private static ushort[][] p_ReducsOnTerminal_DD = { };

        private static ushort[] p_ExpectedIDs_DE = { 0x8 };
        private static string[] p_ExpectedNames_DE = { "NAME" };
        private static string[] p_Items_DE = { "[terminal_subgrammar -> => . qualified_name]", "[qualified_name -> . NAME _m18]" };
        private static ushort[][] p_ShiftsOnTerminal_DE = { new ushort[2] { 0x8, 0x18 } };
        private static ushort[][] p_ShiftsOnVariable_DE = { new ushort[2] { 0x11, 0x100 } };
        private static ushort[][] p_ReducsOnTerminal_DE = { };

        private static ushort[] p_ExpectedIDs_DF = { 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_DF = { "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_DF = { "[terminal_definition -> terminal_def_restrict _m79 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_DF = { };
        private static ushort[][] p_ShiftsOnVariable_DF = { };
        private static ushort[][] p_ReducsOnTerminal_DF = { new ushort[2] { 0x39, 0x23 }, new ushort[2] { 0x42, 0x23 }, new ushort[2] { 0x3C, 0x23 } };

        private static ushort[] p_ExpectedIDs_E0 = { 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_E0 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_E0 = { "[_m79 -> | . terminal_def_restrict _m79]", "[terminal_def_restrict -> . terminal_def_fragment _m77]", "[terminal_def_fragment -> . terminal_def_repetition _m75]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_E0 = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_E0 = { new ushort[2] { 0x58, 0x101 }, new ushort[2] { 0x57, 0xA6 }, new ushort[2] { 0x56, 0xA7 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_E0 = { };

        private static ushort[] p_ExpectedIDs_E1 = { 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_E1 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_E1 = { "[terminal_def_restrict -> terminal_def_fragment _m77 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E1 = { };
        private static ushort[][] p_ShiftsOnVariable_E1 = { };
        private static ushort[][] p_ReducsOnTerminal_E1 = { new ushort[2] { 0x41, 0x22 }, new ushort[2] { 0x39, 0x22 }, new ushort[2] { 0x42, 0x22 }, new ushort[2] { 0x3C, 0x22 } };

        private static ushort[] p_ExpectedIDs_E2 = { 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_E2 = { "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_E2 = { "[_m77 -> - . terminal_def_fragment _m77]", "[terminal_def_fragment -> . terminal_def_repetition _m75]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_E2 = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_E2 = { new ushort[2] { 0x57, 0x102 }, new ushort[2] { 0x56, 0xA7 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_E2 = { };

        private static ushort[] p_ExpectedIDs_E3 = { 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_E3 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_E3 = { "[terminal_def_fragment -> terminal_def_repetition _m75 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E3 = { };
        private static ushort[][] p_ShiftsOnVariable_E3 = { };
        private static ushort[][] p_ReducsOnTerminal_E3 = { new ushort[2] { 0x40, 0x21 }, new ushort[2] { 0x41, 0x21 }, new ushort[2] { 0x39, 0x21 }, new ushort[2] { 0x42, 0x21 }, new ushort[2] { 0x3C, 0x21 } };

        private static ushort[] p_ExpectedIDs_E4 = { 0x40, 0x41, 0x39, 0x42, 0x3C, 0x3B, 0x8, 0x2C, 0x2D, 0x2A, 0x2B };
        private static string[] p_ExpectedNames_E4 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[(]", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET" };
        private static string[] p_Items_E4 = { "[_m75 -> terminal_def_repetition . _m75]", "[_m75 -> . terminal_def_repetition _m75]", "[_m75 -> . ]", "[terminal_def_repetition -> . terminal_def_element *]", "[terminal_def_repetition -> . terminal_def_element +]", "[terminal_def_repetition -> . terminal_def_element ?]", "[terminal_def_repetition -> . terminal_def_element]", "[terminal_def_element -> . terminal_def_atom]", "[terminal_def_element -> . ( terminal_definition )]", "[terminal_def_atom -> . terminal_def_atom_unicode]", "[terminal_def_atom -> . terminal_def_atom_text]", "[terminal_def_atom -> . terminal_def_atom_set]", "[terminal_def_atom -> . terminal_def_atom_span]", "[terminal_def_atom -> . NAME]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]", "[terminal_def_atom_set -> . SYMBOL_TERMINAL_SET]", "[terminal_def_atom_span -> . terminal_def_atom_unicode .. terminal_def_atom_unicode]" };
        private static ushort[][] p_ShiftsOnTerminal_E4 = { new ushort[2] { 0x3B, 0xAA }, new ushort[2] { 0x8, 0xAF }, new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 }, new ushort[2] { 0x2A, 0xA3 }, new ushort[2] { 0x2B, 0xB2 } };
        private static ushort[][] p_ShiftsOnVariable_E4 = { new ushort[2] { 0x68, 0x103 }, new ushort[2] { 0x56, 0xE4 }, new ushort[2] { 0x55, 0xA8 }, new ushort[2] { 0x54, 0xA9 }, new ushort[2] { 0x50, 0xAB }, new ushort[2] { 0x51, 0xAC }, new ushort[2] { 0x52, 0xAD }, new ushort[2] { 0x53, 0xAE } };
        private static ushort[][] p_ReducsOnTerminal_E4 = { new ushort[2] { 0x40, 0x43 }, new ushort[2] { 0x41, 0x43 }, new ushort[2] { 0x39, 0x43 }, new ushort[2] { 0x42, 0x43 }, new ushort[2] { 0x3C, 0x43 } };

        private static ushort[] p_ExpectedIDs_E5 = { 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_E5 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_E5 = { "[terminal_def_repetition -> terminal_def_element * . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E5 = { };
        private static ushort[][] p_ShiftsOnVariable_E5 = { };
        private static ushort[][] p_ReducsOnTerminal_E5 = { new ushort[2] { 0x8, 0x1D }, new ushort[2] { 0x3B, 0x1D }, new ushort[2] { 0x2C, 0x1D }, new ushort[2] { 0x2A, 0x1D }, new ushort[2] { 0x2B, 0x1D }, new ushort[2] { 0x2D, 0x1D }, new ushort[2] { 0x40, 0x1D }, new ushort[2] { 0x41, 0x1D }, new ushort[2] { 0x39, 0x1D }, new ushort[2] { 0x42, 0x1D }, new ushort[2] { 0x3C, 0x1D } };

        private static ushort[] p_ExpectedIDs_E6 = { 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_E6 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_E6 = { "[terminal_def_repetition -> terminal_def_element + . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E6 = { };
        private static ushort[][] p_ShiftsOnVariable_E6 = { };
        private static ushort[][] p_ReducsOnTerminal_E6 = { new ushort[2] { 0x8, 0x1E }, new ushort[2] { 0x3B, 0x1E }, new ushort[2] { 0x2C, 0x1E }, new ushort[2] { 0x2A, 0x1E }, new ushort[2] { 0x2B, 0x1E }, new ushort[2] { 0x2D, 0x1E }, new ushort[2] { 0x40, 0x1E }, new ushort[2] { 0x41, 0x1E }, new ushort[2] { 0x39, 0x1E }, new ushort[2] { 0x42, 0x1E }, new ushort[2] { 0x3C, 0x1E } };

        private static ushort[] p_ExpectedIDs_E7 = { 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_E7 = { "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_E7 = { "[terminal_def_repetition -> terminal_def_element ? . ]" };
        private static ushort[][] p_ShiftsOnTerminal_E7 = { };
        private static ushort[][] p_ShiftsOnVariable_E7 = { };
        private static ushort[][] p_ReducsOnTerminal_E7 = { new ushort[2] { 0x8, 0x1F }, new ushort[2] { 0x3B, 0x1F }, new ushort[2] { 0x2C, 0x1F }, new ushort[2] { 0x2A, 0x1F }, new ushort[2] { 0x2B, 0x1F }, new ushort[2] { 0x2D, 0x1F }, new ushort[2] { 0x40, 0x1F }, new ushort[2] { 0x41, 0x1F }, new ushort[2] { 0x39, 0x1F }, new ushort[2] { 0x42, 0x1F }, new ushort[2] { 0x3C, 0x1F } };

        private static ushort[] p_ExpectedIDs_E8 = { 0x3C };
        private static string[] p_ExpectedNames_E8 = { "_T[)]" };
        private static string[] p_Items_E8 = { "[terminal_def_element -> ( terminal_definition . )]" };
        private static ushort[][] p_ShiftsOnTerminal_E8 = { new ushort[2] { 0x3C, 0x104 } };
        private static ushort[][] p_ShiftsOnVariable_E8 = { };
        private static ushort[][] p_ReducsOnTerminal_E8 = { };

        private static ushort[] p_ExpectedIDs_E9 = { 0x2C, 0x2D };
        private static string[] p_ExpectedNames_E9 = { "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16" };
        private static string[] p_Items_E9 = { "[terminal_def_atom_span -> terminal_def_atom_unicode .. . terminal_def_atom_unicode]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT8]", "[terminal_def_atom_unicode -> . SYMBOL_VALUE_UINT16]" };
        private static ushort[][] p_ShiftsOnTerminal_E9 = { new ushort[2] { 0x2C, 0xB0 }, new ushort[2] { 0x2D, 0xB1 } };
        private static ushort[][] p_ShiftsOnVariable_E9 = { new ushort[2] { 0x50, 0x105 } };
        private static ushort[][] p_ReducsOnTerminal_E9 = { };

        private static ushort[] p_ExpectedIDs_EA = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_EA = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_EA = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_EA = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_EA = { new ushort[2] { 0x82, 0x106 }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_EA = { };

        private static ushort[] p_ExpectedIDs_EB = { 0x43 };
        private static string[] p_ExpectedNames_EB = { "_T[->]" };
        private static string[] p_Items_EB = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params . -> rule_definition<grammar_bin_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_EB = { new ushort[2] { 0x43, 0x107 } };
        private static ushort[][] p_ShiftsOnVariable_EB = { };
        private static ushort[][] p_ReducsOnTerminal_EB = { };

        private static ushort[] p_ExpectedIDs_EC = { 0xC7, 0x3C, 0x39, 0x41 };
        private static string[] p_ExpectedNames_EC = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_Items_EC = { "[_m172 -> | rule_def_restrict<grammar_bin_terminal> . _m172]", "[_m172 -> . | rule_def_restrict<grammar_bin_terminal> _m172]", "[_m172 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_EC = { new ushort[2] { 0x41, 0xB6 } };
        private static ushort[][] p_ShiftsOnVariable_EC = { new ushort[2] { 0x8E, 0x108 } };
        private static ushort[][] p_ReducsOnTerminal_EC = { new ushort[2] { 0xC7, 0x8D }, new ushort[2] { 0x3C, 0x8D }, new ushort[2] { 0x39, 0x8D } };

        private static ushort[] p_ExpectedIDs_ED = { 0x41, 0xC7, 0x3C, 0x39, 0x40 };
        private static string[] p_ExpectedNames_ED = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_Items_ED = { "[_m170 -> - rule_def_fragment<grammar_bin_terminal> . _m170]", "[_m170 -> . - rule_def_fragment<grammar_bin_terminal> _m170]", "[_m170 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_ED = { new ushort[2] { 0x40, 0xB8 } };
        private static ushort[][] p_ShiftsOnVariable_ED = { new ushort[2] { 0x8D, 0x109 } };
        private static ushort[][] p_ReducsOnTerminal_ED = { new ushort[2] { 0x41, 0x8B }, new ushort[2] { 0xC7, 0x8B }, new ushort[2] { 0x3C, 0x8B }, new ushort[2] { 0x39, 0x8B } };

        private static ushort[] p_ExpectedIDs_EE = { 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_EE = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_EE = { "[_m168 -> rule_def_repetition<grammar_bin_terminal> _m168 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_EE = { };
        private static ushort[][] p_ShiftsOnVariable_EE = { };
        private static ushort[][] p_ReducsOnTerminal_EE = { new ushort[2] { 0x40, 0x88 }, new ushort[2] { 0x41, 0x88 }, new ushort[2] { 0xC7, 0x88 }, new ushort[2] { 0x3C, 0x88 }, new ushort[2] { 0x39, 0x88 } };

        private static ushort[] p_ExpectedIDs_EF = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_EF = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_EF = { "[rule_def_element<grammar_bin_terminal> -> ( rule_definition<grammar_bin_terminal> ) . ]" };
        private static ushort[][] p_ShiftsOnTerminal_EF = { };
        private static ushort[][] p_ShiftsOnVariable_EF = { };
        private static ushort[][] p_ReducsOnTerminal_EF = { new ushort[2] { 0x4D, 0x7E }, new ushort[2] { 0x4E, 0x7E }, new ushort[2] { 0x3D, 0x7E }, new ushort[2] { 0x3E, 0x7E }, new ushort[2] { 0x3F, 0x7E }, new ushort[2] { 0x3B, 0x7E }, new ushort[2] { 0x29, 0x7E }, new ushort[2] { 0x8, 0x7E }, new ushort[2] { 0x2C, 0x7E }, new ushort[2] { 0x2D, 0x7E }, new ushort[2] { 0xF, 0x7E }, new ushort[2] { 0x2E, 0x7E }, new ushort[2] { 0x2F, 0x7E }, new ushort[2] { 0x30, 0x7E }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7E }, new ushort[2] { 0x33, 0x7E }, new ushort[2] { 0x34, 0x7E }, new ushort[2] { 0x35, 0x7E }, new ushort[2] { 0x36, 0x7E }, new ushort[2] { 0x37, 0x7E }, new ushort[2] { 0x40, 0x7E }, new ushort[2] { 0x41, 0x7E }, new ushort[2] { 0xC7, 0x7E }, new ushort[2] { 0x3C, 0x7E }, new ushort[2] { 0x39, 0x7E } };

        private static ushort[] p_ExpectedIDs_F0 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x2A, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_F0 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "SYMBOL_TERMINAL_TEXT", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_F0 = { "[rule_sym_action -> { qualified_name } . ]" };
        private static ushort[][] p_ShiftsOnTerminal_F0 = { };
        private static ushort[][] p_ShiftsOnVariable_F0 = { };
        private static ushort[][] p_ReducsOnTerminal_F0 = { new ushort[2] { 0x4D, 0x27 }, new ushort[2] { 0x4E, 0x27 }, new ushort[2] { 0x3D, 0x27 }, new ushort[2] { 0x3E, 0x27 }, new ushort[2] { 0x3F, 0x27 }, new ushort[2] { 0x3B, 0x27 }, new ushort[2] { 0x29, 0x27 }, new ushort[2] { 0x8, 0x27 }, new ushort[2] { 0x2C, 0x27 }, new ushort[2] { 0x2D, 0x27 }, new ushort[2] { 0xF, 0x27 }, new ushort[2] { 0x2E, 0x27 }, new ushort[2] { 0x2F, 0x27 }, new ushort[2] { 0x30, 0x27 }, new ushort[2] { 0x31, 0x27 }, new ushort[2] { 0x32, 0x27 }, new ushort[2] { 0x33, 0x27 }, new ushort[2] { 0x34, 0x27 }, new ushort[2] { 0x35, 0x27 }, new ushort[2] { 0x36, 0x27 }, new ushort[2] { 0x37, 0x27 }, new ushort[2] { 0x40, 0x27 }, new ushort[2] { 0x41, 0x27 }, new ushort[2] { 0xC7, 0x27 }, new ushort[2] { 0x2A, 0x27 }, new ushort[2] { 0x3C, 0x27 }, new ushort[2] { 0x39, 0x27 }, new ushort[2] { 0x46, 0x27 }, new ushort[2] { 0x45, 0x27 } };

        private static ushort[] p_ExpectedIDs_F1 = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_F1 = { "_T[>]", "_T[,]" };
        private static string[] p_Items_F1 = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> . _m159 >]", "[_m159 -> . , rule_def_atom<grammar_bin_terminal> _m159]", "[_m159 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_F1 = { new ushort[2] { 0x45, 0x10B } };
        private static ushort[][] p_ShiftsOnVariable_F1 = { new ushort[2] { 0x8B, 0x10A } };
        private static ushort[][] p_ReducsOnTerminal_F1 = { new ushort[2] { 0x46, 0x87 } };

        private static ushort[] p_ExpectedIDs_F2 = { 0x39 };
        private static string[] p_ExpectedNames_F2 = { "_T[;]" };
        private static string[] p_Items_F2 = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_F2 = { new ushort[2] { 0x39, 0x10C } };
        private static ushort[][] p_ShiftsOnVariable_F2 = { };
        private static ushort[][] p_ReducsOnTerminal_F2 = { };

        private static ushort[] p_ExpectedIDs_F3 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_F3 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_F3 = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_F3 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_F3 = { new ushort[2] { 0x71, 0x10D }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_F3 = { };

        private static ushort[] p_ExpectedIDs_F4 = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_F4 = { "NAME", "_T[}]" };
        private static string[] p_Items_F4 = { "[cf_rule_simple<grammar_bin_terminal> -> NAME -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_F4 = { };
        private static ushort[][] p_ShiftsOnVariable_F4 = { };
        private static ushort[][] p_ReducsOnTerminal_F4 = { new ushort[2] { 0x8, 0x72 }, new ushort[2] { 0x10, 0x72 } };

        private static ushort[] p_ExpectedIDs_F5 = { 0x39 };
        private static string[] p_ExpectedNames_F5 = { "_T[;]" };
        private static string[] p_Items_F5 = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_F5 = { new ushort[2] { 0x39, 0x10E } };
        private static ushort[][] p_ShiftsOnVariable_F5 = { };
        private static ushort[][] p_ReducsOnTerminal_F5 = { };

        private static ushort[] p_ExpectedIDs_F6 = { 0x46 };
        private static string[] p_ExpectedNames_F6 = { "_T[>]" };
        private static string[] p_Items_F6 = { "[rule_template_params -> < NAME _m87 . >]" };
        private static ushort[][] p_ShiftsOnTerminal_F6 = { new ushort[2] { 0x46, 0x10F } };
        private static ushort[][] p_ShiftsOnVariable_F6 = { };
        private static ushort[][] p_ReducsOnTerminal_F6 = { };

        private static ushort[] p_ExpectedIDs_F7 = { 0x8 };
        private static string[] p_ExpectedNames_F7 = { "NAME" };
        private static string[] p_Items_F7 = { "[_m87 -> , . NAME _m87]" };
        private static ushort[][] p_ShiftsOnTerminal_F7 = { new ushort[2] { 0x8, 0x110 } };
        private static ushort[][] p_ShiftsOnVariable_F7 = { };
        private static ushort[][] p_ReducsOnTerminal_F7 = { };

        private static ushort[] p_ExpectedIDs_F8 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_F8 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_F8 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_F8 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_F8 = { new ushort[2] { 0x71, 0x111 }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_F8 = { };

        private static ushort[] p_ExpectedIDs_F9 = { 0x43 };
        private static string[] p_ExpectedNames_F9 = { "_T[->]" };
        private static string[] p_Items_F9 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params . -> rule_definition<grammar_text_terminal> ;]" };
        private static ushort[][] p_ShiftsOnTerminal_F9 = { new ushort[2] { 0x43, 0x112 } };
        private static ushort[][] p_ShiftsOnVariable_F9 = { };
        private static ushort[][] p_ReducsOnTerminal_F9 = { };

        private static ushort[] p_ExpectedIDs_FA = { 0xC7, 0x3C, 0x39, 0x41 };
        private static string[] p_ExpectedNames_FA = { "_T[]]", "_T[)]", "_T[;]", "_T[|]" };
        private static string[] p_Items_FA = { "[_m132 -> | rule_def_restrict<grammar_text_terminal> . _m132]", "[_m132 -> . | rule_def_restrict<grammar_text_terminal> _m132]", "[_m132 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FA = { new ushort[2] { 0x41, 0xD0 } };
        private static ushort[][] p_ShiftsOnVariable_FA = { new ushort[2] { 0x7D, 0x113 } };
        private static ushort[][] p_ReducsOnTerminal_FA = { new ushort[2] { 0xC7, 0x6C }, new ushort[2] { 0x3C, 0x6C }, new ushort[2] { 0x39, 0x6C } };

        private static ushort[] p_ExpectedIDs_FB = { 0x41, 0xC7, 0x3C, 0x39, 0x40 };
        private static string[] p_ExpectedNames_FB = { "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[-]" };
        private static string[] p_Items_FB = { "[_m130 -> - rule_def_fragment<grammar_text_terminal> . _m130]", "[_m130 -> . - rule_def_fragment<grammar_text_terminal> _m130]", "[_m130 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FB = { new ushort[2] { 0x40, 0xD2 } };
        private static ushort[][] p_ShiftsOnVariable_FB = { new ushort[2] { 0x7C, 0x114 } };
        private static ushort[][] p_ReducsOnTerminal_FB = { new ushort[2] { 0x41, 0x6A }, new ushort[2] { 0xC7, 0x6A }, new ushort[2] { 0x3C, 0x6A }, new ushort[2] { 0x39, 0x6A } };

        private static ushort[] p_ExpectedIDs_FC = { 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_FC = { "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_FC = { "[_m128 -> rule_def_repetition<grammar_text_terminal> _m128 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FC = { };
        private static ushort[][] p_ShiftsOnVariable_FC = { };
        private static ushort[][] p_ReducsOnTerminal_FC = { new ushort[2] { 0x40, 0x67 }, new ushort[2] { 0x41, 0x67 }, new ushort[2] { 0xC7, 0x67 }, new ushort[2] { 0x3C, 0x67 }, new ushort[2] { 0x39, 0x67 } };

        private static ushort[] p_ExpectedIDs_FD = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_FD = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_FD = { "[rule_def_element<grammar_text_terminal> -> ( rule_definition<grammar_text_terminal> ) . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FD = { };
        private static ushort[][] p_ShiftsOnVariable_FD = { };
        private static ushort[][] p_ReducsOnTerminal_FD = { new ushort[2] { 0x4D, 0x5D }, new ushort[2] { 0x4E, 0x5D }, new ushort[2] { 0x3D, 0x5D }, new ushort[2] { 0x3E, 0x5D }, new ushort[2] { 0x3F, 0x5D }, new ushort[2] { 0x3B, 0x5D }, new ushort[2] { 0x29, 0x5D }, new ushort[2] { 0x8, 0x5D }, new ushort[2] { 0x2A, 0x5D }, new ushort[2] { 0xF, 0x5D }, new ushort[2] { 0x40, 0x5D }, new ushort[2] { 0x41, 0x5D }, new ushort[2] { 0xC7, 0x5D }, new ushort[2] { 0x3C, 0x5D }, new ushort[2] { 0x39, 0x5D } };

        private static ushort[] p_ExpectedIDs_FE = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_FE = { "_T[>]", "_T[,]" };
        private static string[] p_Items_FE = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> . _m119 >]", "[_m119 -> . , rule_def_atom<grammar_text_terminal> _m119]", "[_m119 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FE = { new ushort[2] { 0x45, 0x116 } };
        private static ushort[][] p_ShiftsOnVariable_FE = { new ushort[2] { 0x7A, 0x115 } };
        private static ushort[][] p_ReducsOnTerminal_FE = { new ushort[2] { 0x46, 0x66 } };

        private static ushort[] p_ExpectedIDs_FF = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_FF = { "NAME", "_T[}]" };
        private static string[] p_Items_FF = { "[terminal -> NAME -> terminal_definition terminal_subgrammar ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_FF = { };
        private static ushort[][] p_ShiftsOnVariable_FF = { };
        private static ushort[][] p_ReducsOnTerminal_FF = { new ushort[2] { 0x8, 0x26 }, new ushort[2] { 0x10, 0x26 } };

        private static ushort[] p_ExpectedIDs_100 = { 0x39 };
        private static string[] p_ExpectedNames_100 = { "_T[;]" };
        private static string[] p_Items_100 = { "[terminal_subgrammar -> => qualified_name . ]" };
        private static ushort[][] p_ShiftsOnTerminal_100 = { };
        private static ushort[][] p_ShiftsOnVariable_100 = { };
        private static ushort[][] p_ReducsOnTerminal_100 = { new ushort[2] { 0x39, 0x24 } };

        private static ushort[] p_ExpectedIDs_101 = { 0x39, 0x42, 0x3C, 0x41 };
        private static string[] p_ExpectedNames_101 = { "_T[;]", "_T[=>]", "_T[)]", "_T[|]" };
        private static string[] p_Items_101 = { "[_m79 -> | terminal_def_restrict . _m79]", "[_m79 -> . | terminal_def_restrict _m79]", "[_m79 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_101 = { new ushort[2] { 0x41, 0xE0 } };
        private static ushort[][] p_ShiftsOnVariable_101 = { new ushort[2] { 0x6A, 0x117 } };
        private static ushort[][] p_ReducsOnTerminal_101 = { new ushort[2] { 0x39, 0x47 }, new ushort[2] { 0x42, 0x47 }, new ushort[2] { 0x3C, 0x47 } };

        private static ushort[] p_ExpectedIDs_102 = { 0x41, 0x39, 0x42, 0x3C, 0x40 };
        private static string[] p_ExpectedNames_102 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]", "_T[-]" };
        private static string[] p_Items_102 = { "[_m77 -> - terminal_def_fragment . _m77]", "[_m77 -> . - terminal_def_fragment _m77]", "[_m77 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_102 = { new ushort[2] { 0x40, 0xE2 } };
        private static ushort[][] p_ShiftsOnVariable_102 = { new ushort[2] { 0x69, 0x118 } };
        private static ushort[][] p_ReducsOnTerminal_102 = { new ushort[2] { 0x41, 0x45 }, new ushort[2] { 0x39, 0x45 }, new ushort[2] { 0x42, 0x45 }, new ushort[2] { 0x3C, 0x45 } };

        private static ushort[] p_ExpectedIDs_103 = { 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_103 = { "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_103 = { "[_m75 -> terminal_def_repetition _m75 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_103 = { };
        private static ushort[][] p_ShiftsOnVariable_103 = { };
        private static ushort[][] p_ReducsOnTerminal_103 = { new ushort[2] { 0x40, 0x42 }, new ushort[2] { 0x41, 0x42 }, new ushort[2] { 0x39, 0x42 }, new ushort[2] { 0x42, 0x42 }, new ushort[2] { 0x3C, 0x42 } };

        private static ushort[] p_ExpectedIDs_104 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_104 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_104 = { "[terminal_def_element -> ( terminal_definition ) . ]" };
        private static ushort[][] p_ShiftsOnTerminal_104 = { };
        private static ushort[][] p_ShiftsOnVariable_104 = { };
        private static ushort[][] p_ReducsOnTerminal_104 = { new ushort[2] { 0x3D, 0x1C }, new ushort[2] { 0x3E, 0x1C }, new ushort[2] { 0x3F, 0x1C }, new ushort[2] { 0x8, 0x1C }, new ushort[2] { 0x3B, 0x1C }, new ushort[2] { 0x2C, 0x1C }, new ushort[2] { 0x2A, 0x1C }, new ushort[2] { 0x2B, 0x1C }, new ushort[2] { 0x2D, 0x1C }, new ushort[2] { 0x40, 0x1C }, new ushort[2] { 0x41, 0x1C }, new ushort[2] { 0x39, 0x1C }, new ushort[2] { 0x42, 0x1C }, new ushort[2] { 0x3C, 0x1C } };

        private static ushort[] p_ExpectedIDs_105 = { 0x3D, 0x3E, 0x3F, 0x8, 0x3B, 0x2C, 0x2A, 0x2B, 0x2D, 0x40, 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_105 = { "_T[*]", "_T[+]", "_T[?]", "NAME", "_T[(]", "SYMBOL_VALUE_UINT8", "SYMBOL_TERMINAL_TEXT", "SYMBOL_TERMINAL_SET", "SYMBOL_VALUE_UINT16", "_T[-]", "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_105 = { "[terminal_def_atom_span -> terminal_def_atom_unicode .. terminal_def_atom_unicode . ]" };
        private static ushort[][] p_ShiftsOnTerminal_105 = { };
        private static ushort[][] p_ShiftsOnVariable_105 = { };
        private static ushort[][] p_ReducsOnTerminal_105 = { new ushort[2] { 0x3D, 0x15 }, new ushort[2] { 0x3E, 0x15 }, new ushort[2] { 0x3F, 0x15 }, new ushort[2] { 0x8, 0x15 }, new ushort[2] { 0x3B, 0x15 }, new ushort[2] { 0x2C, 0x15 }, new ushort[2] { 0x2A, 0x15 }, new ushort[2] { 0x2B, 0x15 }, new ushort[2] { 0x2D, 0x15 }, new ushort[2] { 0x40, 0x15 }, new ushort[2] { 0x41, 0x15 }, new ushort[2] { 0x39, 0x15 }, new ushort[2] { 0x42, 0x15 }, new ushort[2] { 0x3C, 0x15 } };

        private static ushort[] p_ExpectedIDs_106 = { 0x39 };
        private static string[] p_ExpectedNames_106 = { "_T[;]" };
        private static string[] p_Items_106 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_106 = { new ushort[2] { 0x39, 0x119 } };
        private static ushort[][] p_ShiftsOnVariable_106 = { };
        private static ushort[][] p_ReducsOnTerminal_106 = { };

        private static ushort[] p_ExpectedIDs_107 = { 0x3B, 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_107 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_107 = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> . rule_definition<grammar_bin_terminal> ;]", "[rule_definition<grammar_bin_terminal> -> . rule_def_restrict<grammar_bin_terminal> _m172]", "[rule_def_restrict<grammar_bin_terminal> -> . rule_def_fragment<grammar_bin_terminal> _m170]", "[rule_def_fragment<grammar_bin_terminal> -> . rule_def_repetition<grammar_bin_terminal> _m168]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> *]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> +]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal> ?]", "[rule_def_repetition<grammar_bin_terminal> -> . rule_def_tree_action<grammar_bin_terminal>]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> ^]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal> !]", "[rule_def_tree_action<grammar_bin_terminal> -> . rule_def_element<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . rule_def_atom<grammar_bin_terminal>]", "[rule_def_element<grammar_bin_terminal> -> . ( rule_definition<grammar_bin_terminal> )]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_107 = { new ushort[2] { 0x3B, 0x70 }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_107 = { new ushort[2] { 0x82, 0x11A }, new ushort[2] { 0x83, 0x6A }, new ushort[2] { 0x84, 0x6B }, new ushort[2] { 0x85, 0x6C }, new ushort[2] { 0x86, 0x6D }, new ushort[2] { 0x87, 0x6E }, new ushort[2] { 0x88, 0x6F }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_107 = { };

        private static ushort[] p_ExpectedIDs_108 = { 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_108 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_108 = { "[_m172 -> | rule_def_restrict<grammar_bin_terminal> _m172 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_108 = { };
        private static ushort[][] p_ShiftsOnVariable_108 = { };
        private static ushort[][] p_ReducsOnTerminal_108 = { new ushort[2] { 0xC7, 0x8C }, new ushort[2] { 0x3C, 0x8C }, new ushort[2] { 0x39, 0x8C } };

        private static ushort[] p_ExpectedIDs_109 = { 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_109 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_109 = { "[_m170 -> - rule_def_fragment<grammar_bin_terminal> _m170 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_109 = { };
        private static ushort[][] p_ShiftsOnVariable_109 = { };
        private static ushort[][] p_ReducsOnTerminal_109 = { new ushort[2] { 0x41, 0x8A }, new ushort[2] { 0xC7, 0x8A }, new ushort[2] { 0x3C, 0x8A }, new ushort[2] { 0x39, 0x8A } };

        private static ushort[] p_ExpectedIDs_10A = { 0x46 };
        private static string[] p_ExpectedNames_10A = { "_T[>]" };
        private static string[] p_Items_10A = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> _m159 . >]" };
        private static ushort[][] p_ShiftsOnTerminal_10A = { new ushort[2] { 0x46, 0x11B } };
        private static ushort[][] p_ShiftsOnVariable_10A = { };
        private static ushort[][] p_ReducsOnTerminal_10A = { };

        private static ushort[] p_ExpectedIDs_10B = { 0xF, 0x29, 0x8, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        private static string[] p_ExpectedNames_10B = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY" };
        private static string[] p_Items_10B = { "[_m159 -> , . rule_def_atom<grammar_bin_terminal> _m159]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_bin_terminal> -> . rule_sym_ref_template<grammar_bin_terminal>]", "[rule_def_atom<grammar_bin_terminal> -> . grammar_bin_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_bin_terminal> -> . NAME rule_sym_ref_params<grammar_bin_terminal>]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT8]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT16]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT32]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT64]", "[grammar_bin_terminal -> . SYMBOL_VALUE_UINT128]", "[grammar_bin_terminal -> . SYMBOL_VALUE_BINARY]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT8]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT16]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT32]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT64]", "[grammar_bin_terminal -> . SYMBOL_JOKER_UINT128]", "[grammar_bin_terminal -> . SYMBOL_JOKER_BINARY]" };
        private static ushort[][] p_ShiftsOnTerminal_10B = { new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0x78 }, new ushort[2] { 0x2C, 0x79 }, new ushort[2] { 0x2D, 0x7A }, new ushort[2] { 0x2E, 0x7B }, new ushort[2] { 0x2F, 0x7C }, new ushort[2] { 0x30, 0x7D }, new ushort[2] { 0x31, 0x7E }, new ushort[2] { 0x32, 0x7F }, new ushort[2] { 0x33, 0x80 }, new ushort[2] { 0x34, 0x81 }, new ushort[2] { 0x35, 0x82 }, new ushort[2] { 0x36, 0x83 }, new ushort[2] { 0x37, 0x84 } };
        private static ushort[][] p_ShiftsOnVariable_10B = { new ushort[2] { 0x88, 0x11C }, new ushort[2] { 0x5C, 0x71 }, new ushort[2] { 0x5D, 0x72 }, new ushort[2] { 0x5E, 0x73 }, new ushort[2] { 0x89, 0x74 }, new ushort[2] { 0x60, 0x75 } };
        private static ushort[][] p_ReducsOnTerminal_10B = { };

        private static ushort[] p_ExpectedIDs_10C = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_10C = { "NAME", "_T[}]" };
        private static string[] p_Items_10C = { "[cf_rule_simple<grammar_text_terminal> -> NAME -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_10C = { };
        private static ushort[][] p_ShiftsOnVariable_10C = { };
        private static ushort[][] p_ReducsOnTerminal_10C = { new ushort[2] { 0x8, 0x51 }, new ushort[2] { 0x10, 0x51 } };

        private static ushort[] p_ExpectedIDs_10D = { 0x39 };
        private static string[] p_ExpectedNames_10D = { "_T[;]" };
        private static string[] p_Items_10D = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_10D = { new ushort[2] { 0x39, 0x11D } };
        private static ushort[][] p_ShiftsOnVariable_10D = { };
        private static ushort[][] p_ReducsOnTerminal_10D = { };

        private static ushort[] p_ExpectedIDs_10E = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_10E = { "NAME", "_T[}]" };
        private static string[] p_Items_10E = { "[cf_rule_template<grammar_bin_terminal> -> NAME rule_template_params -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_10E = { };
        private static ushort[][] p_ShiftsOnVariable_10E = { };
        private static ushort[][] p_ReducsOnTerminal_10E = { new ushort[2] { 0x8, 0x8E }, new ushort[2] { 0x10, 0x8E } };

        private static ushort[] p_ExpectedIDs_10F = { 0x43 };
        private static string[] p_ExpectedNames_10F = { "_T[->]" };
        private static string[] p_Items_10F = { "[rule_template_params -> < NAME _m87 > . ]" };
        private static ushort[][] p_ShiftsOnTerminal_10F = { };
        private static ushort[][] p_ShiftsOnVariable_10F = { };
        private static ushort[][] p_ReducsOnTerminal_10F = { new ushort[2] { 0x43, 0x2A } };

        private static ushort[] p_ExpectedIDs_110 = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_110 = { "_T[>]", "_T[,]" };
        private static string[] p_Items_110 = { "[_m87 -> , NAME . _m87]", "[_m87 -> . , NAME _m87]", "[_m87 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_110 = { new ushort[2] { 0x45, 0xF7 } };
        private static ushort[][] p_ShiftsOnVariable_110 = { new ushort[2] { 0x6B, 0x11E } };
        private static ushort[][] p_ReducsOnTerminal_110 = { new ushort[2] { 0x46, 0x49 } };

        private static ushort[] p_ExpectedIDs_111 = { 0x39 };
        private static string[] p_ExpectedNames_111 = { "_T[;]" };
        private static string[] p_Items_111 = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_111 = { new ushort[2] { 0x39, 0x11F } };
        private static ushort[][] p_ShiftsOnVariable_111 = { };
        private static ushort[][] p_ReducsOnTerminal_111 = { };

        private static ushort[] p_ExpectedIDs_112 = { 0x3B, 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_112 = { "_T[(]", "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_112 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> . rule_definition<grammar_text_terminal> ;]", "[rule_definition<grammar_text_terminal> -> . rule_def_restrict<grammar_text_terminal> _m132]", "[rule_def_restrict<grammar_text_terminal> -> . rule_def_fragment<grammar_text_terminal> _m130]", "[rule_def_fragment<grammar_text_terminal> -> . rule_def_repetition<grammar_text_terminal> _m128]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> *]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> +]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal> ?]", "[rule_def_repetition<grammar_text_terminal> -> . rule_def_tree_action<grammar_text_terminal>]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> ^]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal> !]", "[rule_def_tree_action<grammar_text_terminal> -> . rule_def_element<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . rule_def_atom<grammar_text_terminal>]", "[rule_def_element<grammar_text_terminal> -> . ( rule_definition<grammar_text_terminal> )]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_112 = { new ushort[2] { 0x3B, 0x9B }, new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_112 = { new ushort[2] { 0x71, 0x120 }, new ushort[2] { 0x72, 0x95 }, new ushort[2] { 0x73, 0x96 }, new ushort[2] { 0x74, 0x97 }, new ushort[2] { 0x75, 0x98 }, new ushort[2] { 0x76, 0x99 }, new ushort[2] { 0x77, 0x9A }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_112 = { };

        private static ushort[] p_ExpectedIDs_113 = { 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_113 = { "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_113 = { "[_m132 -> | rule_def_restrict<grammar_text_terminal> _m132 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_113 = { };
        private static ushort[][] p_ShiftsOnVariable_113 = { };
        private static ushort[][] p_ReducsOnTerminal_113 = { new ushort[2] { 0xC7, 0x6B }, new ushort[2] { 0x3C, 0x6B }, new ushort[2] { 0x39, 0x6B } };

        private static ushort[] p_ExpectedIDs_114 = { 0x41, 0xC7, 0x3C, 0x39 };
        private static string[] p_ExpectedNames_114 = { "_T[|]", "_T[]]", "_T[)]", "_T[;]" };
        private static string[] p_Items_114 = { "[_m130 -> - rule_def_fragment<grammar_text_terminal> _m130 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_114 = { };
        private static ushort[][] p_ShiftsOnVariable_114 = { };
        private static ushort[][] p_ReducsOnTerminal_114 = { new ushort[2] { 0x41, 0x69 }, new ushort[2] { 0xC7, 0x69 }, new ushort[2] { 0x3C, 0x69 }, new ushort[2] { 0x39, 0x69 } };

        private static ushort[] p_ExpectedIDs_115 = { 0x46 };
        private static string[] p_ExpectedNames_115 = { "_T[>]" };
        private static string[] p_Items_115 = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> _m119 . >]" };
        private static ushort[][] p_ShiftsOnTerminal_115 = { new ushort[2] { 0x46, 0x121 } };
        private static ushort[][] p_ShiftsOnVariable_115 = { };
        private static ushort[][] p_ReducsOnTerminal_115 = { };

        private static ushort[] p_ExpectedIDs_116 = { 0xF, 0x29, 0x8, 0x2A };
        private static string[] p_ExpectedNames_116 = { "_T[{]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT" };
        private static string[] p_Items_116 = { "[_m119 -> , . rule_def_atom<grammar_text_terminal> _m119]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_action]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_virtual]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_simple]", "[rule_def_atom<grammar_text_terminal> -> . rule_sym_ref_template<grammar_text_terminal>]", "[rule_def_atom<grammar_text_terminal> -> . grammar_text_terminal]", "[rule_sym_action -> . { qualified_name }]", "[rule_sym_virtual -> . QUOTED_DATA]", "[rule_sym_ref_simple -> . NAME]", "[rule_sym_ref_template<grammar_text_terminal> -> . NAME rule_sym_ref_params<grammar_text_terminal>]", "[grammar_text_terminal -> . terminal_def_atom_text]", "[terminal_def_atom_text -> . SYMBOL_TERMINAL_TEXT]" };
        private static ushort[][] p_ShiftsOnTerminal_116 = { new ushort[2] { 0xF, 0x76 }, new ushort[2] { 0x29, 0x77 }, new ushort[2] { 0x8, 0xA1 }, new ushort[2] { 0x2A, 0xA3 } };
        private static ushort[][] p_ShiftsOnVariable_116 = { new ushort[2] { 0x77, 0x122 }, new ushort[2] { 0x5C, 0x9C }, new ushort[2] { 0x5D, 0x9D }, new ushort[2] { 0x5E, 0x9E }, new ushort[2] { 0x78, 0x9F }, new ushort[2] { 0x61, 0xA0 }, new ushort[2] { 0x51, 0xA2 } };
        private static ushort[][] p_ReducsOnTerminal_116 = { };

        private static ushort[] p_ExpectedIDs_117 = { 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_117 = { "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_117 = { "[_m79 -> | terminal_def_restrict _m79 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_117 = { };
        private static ushort[][] p_ShiftsOnVariable_117 = { };
        private static ushort[][] p_ReducsOnTerminal_117 = { new ushort[2] { 0x39, 0x46 }, new ushort[2] { 0x42, 0x46 }, new ushort[2] { 0x3C, 0x46 } };

        private static ushort[] p_ExpectedIDs_118 = { 0x41, 0x39, 0x42, 0x3C };
        private static string[] p_ExpectedNames_118 = { "_T[|]", "_T[;]", "_T[=>]", "_T[)]" };
        private static string[] p_Items_118 = { "[_m77 -> - terminal_def_fragment _m77 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_118 = { };
        private static ushort[][] p_ShiftsOnVariable_118 = { };
        private static ushort[][] p_ReducsOnTerminal_118 = { new ushort[2] { 0x41, 0x44 }, new ushort[2] { 0x39, 0x44 }, new ushort[2] { 0x42, 0x44 }, new ushort[2] { 0x3C, 0x44 } };

        private static ushort[] p_ExpectedIDs_119 = { 0x8, 0xC6, 0x10 };
        private static string[] p_ExpectedNames_119 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_Items_119 = { "[cs_rule_simple<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_119 = { };
        private static ushort[][] p_ShiftsOnVariable_119 = { };
        private static ushort[][] p_ReducsOnTerminal_119 = { new ushort[2] { 0x8, 0x9D }, new ushort[2] { 0xC6, 0x9D }, new ushort[2] { 0x10, 0x9D } };

        private static ushort[] p_ExpectedIDs_11A = { 0x39 };
        private static string[] p_ExpectedNames_11A = { "_T[;]" };
        private static string[] p_Items_11A = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_11A = { new ushort[2] { 0x39, 0x123 } };
        private static ushort[][] p_ShiftsOnVariable_11A = { };
        private static ushort[][] p_ReducsOnTerminal_11A = { };

        private static ushort[] p_ExpectedIDs_11B = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2C, 0x2D, 0xF, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_11B = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_VALUE_UINT8", "SYMBOL_VALUE_UINT16", "_T[{]", "SYMBOL_VALUE_UINT32", "SYMBOL_VALUE_UINT64", "SYMBOL_VALUE_UINT128", "SYMBOL_VALUE_BINARY", "SYMBOL_JOKER_UINT8", "SYMBOL_JOKER_UINT16", "SYMBOL_JOKER_UINT32", "SYMBOL_JOKER_UINT64", "SYMBOL_JOKER_UINT128", "SYMBOL_JOKER_BINARY", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_11B = { "[rule_sym_ref_params<grammar_bin_terminal> -> < rule_def_atom<grammar_bin_terminal> _m159 > . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11B = { };
        private static ushort[][] p_ShiftsOnVariable_11B = { };
        private static ushort[][] p_ReducsOnTerminal_11B = { new ushort[2] { 0x4D, 0x85 }, new ushort[2] { 0x4E, 0x85 }, new ushort[2] { 0x3D, 0x85 }, new ushort[2] { 0x3E, 0x85 }, new ushort[2] { 0x3F, 0x85 }, new ushort[2] { 0x3B, 0x85 }, new ushort[2] { 0x29, 0x85 }, new ushort[2] { 0x8, 0x85 }, new ushort[2] { 0x2C, 0x85 }, new ushort[2] { 0x2D, 0x85 }, new ushort[2] { 0xF, 0x85 }, new ushort[2] { 0x2E, 0x85 }, new ushort[2] { 0x2F, 0x85 }, new ushort[2] { 0x30, 0x85 }, new ushort[2] { 0x31, 0x85 }, new ushort[2] { 0x32, 0x85 }, new ushort[2] { 0x33, 0x85 }, new ushort[2] { 0x34, 0x85 }, new ushort[2] { 0x35, 0x85 }, new ushort[2] { 0x36, 0x85 }, new ushort[2] { 0x37, 0x85 }, new ushort[2] { 0x40, 0x85 }, new ushort[2] { 0x41, 0x85 }, new ushort[2] { 0xC7, 0x85 }, new ushort[2] { 0x3C, 0x85 }, new ushort[2] { 0x39, 0x85 }, new ushort[2] { 0x46, 0x85 }, new ushort[2] { 0x45, 0x85 } };

        private static ushort[] p_ExpectedIDs_11C = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_11C = { "_T[>]", "_T[,]" };
        private static string[] p_Items_11C = { "[_m159 -> , rule_def_atom<grammar_bin_terminal> . _m159]", "[_m159 -> . , rule_def_atom<grammar_bin_terminal> _m159]", "[_m159 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11C = { new ushort[2] { 0x45, 0x10B } };
        private static ushort[][] p_ShiftsOnVariable_11C = { new ushort[2] { 0x8B, 0x124 } };
        private static ushort[][] p_ReducsOnTerminal_11C = { new ushort[2] { 0x46, 0x87 } };

        private static ushort[] p_ExpectedIDs_11D = { 0x8, 0x10 };
        private static string[] p_ExpectedNames_11D = { "NAME", "_T[}]" };
        private static string[] p_Items_11D = { "[cf_rule_template<grammar_text_terminal> -> NAME rule_template_params -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11D = { };
        private static ushort[][] p_ShiftsOnVariable_11D = { };
        private static ushort[][] p_ReducsOnTerminal_11D = { new ushort[2] { 0x8, 0x6D }, new ushort[2] { 0x10, 0x6D } };

        private static ushort[] p_ExpectedIDs_11E = { 0x46 };
        private static string[] p_ExpectedNames_11E = { "_T[>]" };
        private static string[] p_Items_11E = { "[_m87 -> , NAME _m87 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11E = { };
        private static ushort[][] p_ShiftsOnVariable_11E = { };
        private static ushort[][] p_ReducsOnTerminal_11E = { new ushort[2] { 0x46, 0x48 } };

        private static ushort[] p_ExpectedIDs_11F = { 0x8, 0xC6, 0x10 };
        private static string[] p_ExpectedNames_11F = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_Items_11F = { "[cs_rule_simple<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_11F = { };
        private static ushort[][] p_ShiftsOnVariable_11F = { };
        private static ushort[][] p_ReducsOnTerminal_11F = { new ushort[2] { 0x8, 0x95 }, new ushort[2] { 0xC6, 0x95 }, new ushort[2] { 0x10, 0x95 } };

        private static ushort[] p_ExpectedIDs_120 = { 0x39 };
        private static string[] p_ExpectedNames_120 = { "_T[;]" };
        private static string[] p_Items_120 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> . ;]" };
        private static ushort[][] p_ShiftsOnTerminal_120 = { new ushort[2] { 0x39, 0x125 } };
        private static ushort[][] p_ShiftsOnVariable_120 = { };
        private static ushort[][] p_ReducsOnTerminal_120 = { };

        private static ushort[] p_ExpectedIDs_121 = { 0x4D, 0x4E, 0x3D, 0x3E, 0x3F, 0x3B, 0x29, 0x8, 0x2A, 0xF, 0x40, 0x41, 0xC7, 0x3C, 0x39, 0x46, 0x45 };
        private static string[] p_ExpectedNames_121 = { "_T[^]", "_T[!]", "_T[*]", "_T[+]", "_T[?]", "_T[(]", "QUOTED_DATA", "NAME", "SYMBOL_TERMINAL_TEXT", "_T[{]", "_T[-]", "_T[|]", "_T[]]", "_T[)]", "_T[;]", "_T[>]", "_T[,]" };
        private static string[] p_Items_121 = { "[rule_sym_ref_params<grammar_text_terminal> -> < rule_def_atom<grammar_text_terminal> _m119 > . ]" };
        private static ushort[][] p_ShiftsOnTerminal_121 = { };
        private static ushort[][] p_ShiftsOnVariable_121 = { };
        private static ushort[][] p_ReducsOnTerminal_121 = { new ushort[2] { 0x4D, 0x64 }, new ushort[2] { 0x4E, 0x64 }, new ushort[2] { 0x3D, 0x64 }, new ushort[2] { 0x3E, 0x64 }, new ushort[2] { 0x3F, 0x64 }, new ushort[2] { 0x3B, 0x64 }, new ushort[2] { 0x29, 0x64 }, new ushort[2] { 0x8, 0x64 }, new ushort[2] { 0x2A, 0x64 }, new ushort[2] { 0xF, 0x64 }, new ushort[2] { 0x40, 0x64 }, new ushort[2] { 0x41, 0x64 }, new ushort[2] { 0xC7, 0x64 }, new ushort[2] { 0x3C, 0x64 }, new ushort[2] { 0x39, 0x64 }, new ushort[2] { 0x46, 0x64 }, new ushort[2] { 0x45, 0x64 } };

        private static ushort[] p_ExpectedIDs_122 = { 0x46, 0x45 };
        private static string[] p_ExpectedNames_122 = { "_T[>]", "_T[,]" };
        private static string[] p_Items_122 = { "[_m119 -> , rule_def_atom<grammar_text_terminal> . _m119]", "[_m119 -> . , rule_def_atom<grammar_text_terminal> _m119]", "[_m119 -> . ]" };
        private static ushort[][] p_ShiftsOnTerminal_122 = { new ushort[2] { 0x45, 0x116 } };
        private static ushort[][] p_ShiftsOnVariable_122 = { new ushort[2] { 0x7A, 0x126 } };
        private static ushort[][] p_ReducsOnTerminal_122 = { new ushort[2] { 0x46, 0x66 } };

        private static ushort[] p_ExpectedIDs_123 = { 0x8, 0xC6, 0x10 };
        private static string[] p_ExpectedNames_123 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_Items_123 = { "[cs_rule_template<grammar_bin_terminal> -> cs_rule_context<grammar_bin_terminal> NAME cs_rule_context<grammar_bin_terminal> rule_template_params -> rule_definition<grammar_bin_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_123 = { };
        private static ushort[][] p_ShiftsOnVariable_123 = { };
        private static ushort[][] p_ReducsOnTerminal_123 = { new ushort[2] { 0x8, 0xA0 }, new ushort[2] { 0xC6, 0xA0 }, new ushort[2] { 0x10, 0xA0 } };

        private static ushort[] p_ExpectedIDs_124 = { 0x46 };
        private static string[] p_ExpectedNames_124 = { "_T[>]" };
        private static string[] p_Items_124 = { "[_m159 -> , rule_def_atom<grammar_bin_terminal> _m159 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_124 = { };
        private static ushort[][] p_ShiftsOnVariable_124 = { };
        private static ushort[][] p_ReducsOnTerminal_124 = { new ushort[2] { 0x46, 0x86 } };

        private static ushort[] p_ExpectedIDs_125 = { 0x8, 0xC6, 0x10 };
        private static string[] p_ExpectedNames_125 = { "NAME", "_T[[]", "_T[}]" };
        private static string[] p_Items_125 = { "[cs_rule_template<grammar_text_terminal> -> cs_rule_context<grammar_text_terminal> NAME cs_rule_context<grammar_text_terminal> rule_template_params -> rule_definition<grammar_text_terminal> ; . ]" };
        private static ushort[][] p_ShiftsOnTerminal_125 = { };
        private static ushort[][] p_ShiftsOnVariable_125 = { };
        private static ushort[][] p_ReducsOnTerminal_125 = { new ushort[2] { 0x8, 0x98 }, new ushort[2] { 0xC6, 0x98 }, new ushort[2] { 0x10, 0x98 } };

        private static ushort[] p_ExpectedIDs_126 = { 0x46 };
        private static string[] p_ExpectedNames_126 = { "_T[>]" };
        private static string[] p_Items_126 = { "[_m119 -> , rule_def_atom<grammar_text_terminal> _m119 . ]" };
        private static ushort[][] p_ShiftsOnTerminal_126 = { };
        private static ushort[][] p_ShiftsOnVariable_126 = { };
        private static ushort[][] p_ReducsOnTerminal_126 = { new ushort[2] { 0x46, 0x65 } };

        private static ushort[][] p_ExpectedIDs = { p_ExpectedIDs_0, p_ExpectedIDs_1, p_ExpectedIDs_2, p_ExpectedIDs_3, p_ExpectedIDs_4, p_ExpectedIDs_5, p_ExpectedIDs_6, p_ExpectedIDs_7, p_ExpectedIDs_8, p_ExpectedIDs_9, p_ExpectedIDs_A, p_ExpectedIDs_B, p_ExpectedIDs_C, p_ExpectedIDs_D, p_ExpectedIDs_E, p_ExpectedIDs_F, p_ExpectedIDs_10, p_ExpectedIDs_11, p_ExpectedIDs_12, p_ExpectedIDs_13, p_ExpectedIDs_14, p_ExpectedIDs_15, p_ExpectedIDs_16, p_ExpectedIDs_17, p_ExpectedIDs_18, p_ExpectedIDs_19, p_ExpectedIDs_1A, p_ExpectedIDs_1B, p_ExpectedIDs_1C, p_ExpectedIDs_1D, p_ExpectedIDs_1E, p_ExpectedIDs_1F, p_ExpectedIDs_20, p_ExpectedIDs_21, p_ExpectedIDs_22, p_ExpectedIDs_23, p_ExpectedIDs_24, p_ExpectedIDs_25, p_ExpectedIDs_26, p_ExpectedIDs_27, p_ExpectedIDs_28, p_ExpectedIDs_29, p_ExpectedIDs_2A, p_ExpectedIDs_2B, p_ExpectedIDs_2C, p_ExpectedIDs_2D, p_ExpectedIDs_2E, p_ExpectedIDs_2F, p_ExpectedIDs_30, p_ExpectedIDs_31, p_ExpectedIDs_32, p_ExpectedIDs_33, p_ExpectedIDs_34, p_ExpectedIDs_35, p_ExpectedIDs_36, p_ExpectedIDs_37, p_ExpectedIDs_38, p_ExpectedIDs_39, p_ExpectedIDs_3A, p_ExpectedIDs_3B, p_ExpectedIDs_3C, p_ExpectedIDs_3D, p_ExpectedIDs_3E, p_ExpectedIDs_3F, p_ExpectedIDs_40, p_ExpectedIDs_41, p_ExpectedIDs_42, p_ExpectedIDs_43, p_ExpectedIDs_44, p_ExpectedIDs_45, p_ExpectedIDs_46, p_ExpectedIDs_47, p_ExpectedIDs_48, p_ExpectedIDs_49, p_ExpectedIDs_4A, p_ExpectedIDs_4B, p_ExpectedIDs_4C, p_ExpectedIDs_4D, p_ExpectedIDs_4E, p_ExpectedIDs_4F, p_ExpectedIDs_50, p_ExpectedIDs_51, p_ExpectedIDs_52, p_ExpectedIDs_53, p_ExpectedIDs_54, p_ExpectedIDs_55, p_ExpectedIDs_56, p_ExpectedIDs_57, p_ExpectedIDs_58, p_ExpectedIDs_59, p_ExpectedIDs_5A, p_ExpectedIDs_5B, p_ExpectedIDs_5C, p_ExpectedIDs_5D, p_ExpectedIDs_5E, p_ExpectedIDs_5F, p_ExpectedIDs_60, p_ExpectedIDs_61, p_ExpectedIDs_62, p_ExpectedIDs_63, p_ExpectedIDs_64, p_ExpectedIDs_65, p_ExpectedIDs_66, p_ExpectedIDs_67, p_ExpectedIDs_68, p_ExpectedIDs_69, p_ExpectedIDs_6A, p_ExpectedIDs_6B, p_ExpectedIDs_6C, p_ExpectedIDs_6D, p_ExpectedIDs_6E, p_ExpectedIDs_6F, p_ExpectedIDs_70, p_ExpectedIDs_71, p_ExpectedIDs_72, p_ExpectedIDs_73, p_ExpectedIDs_74, p_ExpectedIDs_75, p_ExpectedIDs_76, p_ExpectedIDs_77, p_ExpectedIDs_78, p_ExpectedIDs_79, p_ExpectedIDs_7A, p_ExpectedIDs_7B, p_ExpectedIDs_7C, p_ExpectedIDs_7D, p_ExpectedIDs_7E, p_ExpectedIDs_7F, p_ExpectedIDs_80, p_ExpectedIDs_81, p_ExpectedIDs_82, p_ExpectedIDs_83, p_ExpectedIDs_84, p_ExpectedIDs_85, p_ExpectedIDs_86, p_ExpectedIDs_87, p_ExpectedIDs_88, p_ExpectedIDs_89, p_ExpectedIDs_8A, p_ExpectedIDs_8B, p_ExpectedIDs_8C, p_ExpectedIDs_8D, p_ExpectedIDs_8E, p_ExpectedIDs_8F, p_ExpectedIDs_90, p_ExpectedIDs_91, p_ExpectedIDs_92, p_ExpectedIDs_93, p_ExpectedIDs_94, p_ExpectedIDs_95, p_ExpectedIDs_96, p_ExpectedIDs_97, p_ExpectedIDs_98, p_ExpectedIDs_99, p_ExpectedIDs_9A, p_ExpectedIDs_9B, p_ExpectedIDs_9C, p_ExpectedIDs_9D, p_ExpectedIDs_9E, p_ExpectedIDs_9F, p_ExpectedIDs_A0, p_ExpectedIDs_A1, p_ExpectedIDs_A2, p_ExpectedIDs_A3, p_ExpectedIDs_A4, p_ExpectedIDs_A5, p_ExpectedIDs_A6, p_ExpectedIDs_A7, p_ExpectedIDs_A8, p_ExpectedIDs_A9, p_ExpectedIDs_AA, p_ExpectedIDs_AB, p_ExpectedIDs_AC, p_ExpectedIDs_AD, p_ExpectedIDs_AE, p_ExpectedIDs_AF, p_ExpectedIDs_B0, p_ExpectedIDs_B1, p_ExpectedIDs_B2, p_ExpectedIDs_B3, p_ExpectedIDs_B4, p_ExpectedIDs_B5, p_ExpectedIDs_B6, p_ExpectedIDs_B7, p_ExpectedIDs_B8, p_ExpectedIDs_B9, p_ExpectedIDs_BA, p_ExpectedIDs_BB, p_ExpectedIDs_BC, p_ExpectedIDs_BD, p_ExpectedIDs_BE, p_ExpectedIDs_BF, p_ExpectedIDs_C0, p_ExpectedIDs_C1, p_ExpectedIDs_C2, p_ExpectedIDs_C3, p_ExpectedIDs_C4, p_ExpectedIDs_C5, p_ExpectedIDs_C6, p_ExpectedIDs_C7, p_ExpectedIDs_C8, p_ExpectedIDs_C9, p_ExpectedIDs_CA, p_ExpectedIDs_CB, p_ExpectedIDs_CC, p_ExpectedIDs_CD, p_ExpectedIDs_CE, p_ExpectedIDs_CF, p_ExpectedIDs_D0, p_ExpectedIDs_D1, p_ExpectedIDs_D2, p_ExpectedIDs_D3, p_ExpectedIDs_D4, p_ExpectedIDs_D5, p_ExpectedIDs_D6, p_ExpectedIDs_D7, p_ExpectedIDs_D8, p_ExpectedIDs_D9, p_ExpectedIDs_DA, p_ExpectedIDs_DB, p_ExpectedIDs_DC, p_ExpectedIDs_DD, p_ExpectedIDs_DE, p_ExpectedIDs_DF, p_ExpectedIDs_E0, p_ExpectedIDs_E1, p_ExpectedIDs_E2, p_ExpectedIDs_E3, p_ExpectedIDs_E4, p_ExpectedIDs_E5, p_ExpectedIDs_E6, p_ExpectedIDs_E7, p_ExpectedIDs_E8, p_ExpectedIDs_E9, p_ExpectedIDs_EA, p_ExpectedIDs_EB, p_ExpectedIDs_EC, p_ExpectedIDs_ED, p_ExpectedIDs_EE, p_ExpectedIDs_EF, p_ExpectedIDs_F0, p_ExpectedIDs_F1, p_ExpectedIDs_F2, p_ExpectedIDs_F3, p_ExpectedIDs_F4, p_ExpectedIDs_F5, p_ExpectedIDs_F6, p_ExpectedIDs_F7, p_ExpectedIDs_F8, p_ExpectedIDs_F9, p_ExpectedIDs_FA, p_ExpectedIDs_FB, p_ExpectedIDs_FC, p_ExpectedIDs_FD, p_ExpectedIDs_FE, p_ExpectedIDs_FF, p_ExpectedIDs_100, p_ExpectedIDs_101, p_ExpectedIDs_102, p_ExpectedIDs_103, p_ExpectedIDs_104, p_ExpectedIDs_105, p_ExpectedIDs_106, p_ExpectedIDs_107, p_ExpectedIDs_108, p_ExpectedIDs_109, p_ExpectedIDs_10A, p_ExpectedIDs_10B, p_ExpectedIDs_10C, p_ExpectedIDs_10D, p_ExpectedIDs_10E, p_ExpectedIDs_10F, p_ExpectedIDs_110, p_ExpectedIDs_111, p_ExpectedIDs_112, p_ExpectedIDs_113, p_ExpectedIDs_114, p_ExpectedIDs_115, p_ExpectedIDs_116, p_ExpectedIDs_117, p_ExpectedIDs_118, p_ExpectedIDs_119, p_ExpectedIDs_11A, p_ExpectedIDs_11B, p_ExpectedIDs_11C, p_ExpectedIDs_11D, p_ExpectedIDs_11E, p_ExpectedIDs_11F, p_ExpectedIDs_120, p_ExpectedIDs_121, p_ExpectedIDs_122, p_ExpectedIDs_123, p_ExpectedIDs_124, p_ExpectedIDs_125, p_ExpectedIDs_126 };
        private static string[][] p_ExpectedNames = { p_ExpectedNames_0, p_ExpectedNames_1, p_ExpectedNames_2, p_ExpectedNames_3, p_ExpectedNames_4, p_ExpectedNames_5, p_ExpectedNames_6, p_ExpectedNames_7, p_ExpectedNames_8, p_ExpectedNames_9, p_ExpectedNames_A, p_ExpectedNames_B, p_ExpectedNames_C, p_ExpectedNames_D, p_ExpectedNames_E, p_ExpectedNames_F, p_ExpectedNames_10, p_ExpectedNames_11, p_ExpectedNames_12, p_ExpectedNames_13, p_ExpectedNames_14, p_ExpectedNames_15, p_ExpectedNames_16, p_ExpectedNames_17, p_ExpectedNames_18, p_ExpectedNames_19, p_ExpectedNames_1A, p_ExpectedNames_1B, p_ExpectedNames_1C, p_ExpectedNames_1D, p_ExpectedNames_1E, p_ExpectedNames_1F, p_ExpectedNames_20, p_ExpectedNames_21, p_ExpectedNames_22, p_ExpectedNames_23, p_ExpectedNames_24, p_ExpectedNames_25, p_ExpectedNames_26, p_ExpectedNames_27, p_ExpectedNames_28, p_ExpectedNames_29, p_ExpectedNames_2A, p_ExpectedNames_2B, p_ExpectedNames_2C, p_ExpectedNames_2D, p_ExpectedNames_2E, p_ExpectedNames_2F, p_ExpectedNames_30, p_ExpectedNames_31, p_ExpectedNames_32, p_ExpectedNames_33, p_ExpectedNames_34, p_ExpectedNames_35, p_ExpectedNames_36, p_ExpectedNames_37, p_ExpectedNames_38, p_ExpectedNames_39, p_ExpectedNames_3A, p_ExpectedNames_3B, p_ExpectedNames_3C, p_ExpectedNames_3D, p_ExpectedNames_3E, p_ExpectedNames_3F, p_ExpectedNames_40, p_ExpectedNames_41, p_ExpectedNames_42, p_ExpectedNames_43, p_ExpectedNames_44, p_ExpectedNames_45, p_ExpectedNames_46, p_ExpectedNames_47, p_ExpectedNames_48, p_ExpectedNames_49, p_ExpectedNames_4A, p_ExpectedNames_4B, p_ExpectedNames_4C, p_ExpectedNames_4D, p_ExpectedNames_4E, p_ExpectedNames_4F, p_ExpectedNames_50, p_ExpectedNames_51, p_ExpectedNames_52, p_ExpectedNames_53, p_ExpectedNames_54, p_ExpectedNames_55, p_ExpectedNames_56, p_ExpectedNames_57, p_ExpectedNames_58, p_ExpectedNames_59, p_ExpectedNames_5A, p_ExpectedNames_5B, p_ExpectedNames_5C, p_ExpectedNames_5D, p_ExpectedNames_5E, p_ExpectedNames_5F, p_ExpectedNames_60, p_ExpectedNames_61, p_ExpectedNames_62, p_ExpectedNames_63, p_ExpectedNames_64, p_ExpectedNames_65, p_ExpectedNames_66, p_ExpectedNames_67, p_ExpectedNames_68, p_ExpectedNames_69, p_ExpectedNames_6A, p_ExpectedNames_6B, p_ExpectedNames_6C, p_ExpectedNames_6D, p_ExpectedNames_6E, p_ExpectedNames_6F, p_ExpectedNames_70, p_ExpectedNames_71, p_ExpectedNames_72, p_ExpectedNames_73, p_ExpectedNames_74, p_ExpectedNames_75, p_ExpectedNames_76, p_ExpectedNames_77, p_ExpectedNames_78, p_ExpectedNames_79, p_ExpectedNames_7A, p_ExpectedNames_7B, p_ExpectedNames_7C, p_ExpectedNames_7D, p_ExpectedNames_7E, p_ExpectedNames_7F, p_ExpectedNames_80, p_ExpectedNames_81, p_ExpectedNames_82, p_ExpectedNames_83, p_ExpectedNames_84, p_ExpectedNames_85, p_ExpectedNames_86, p_ExpectedNames_87, p_ExpectedNames_88, p_ExpectedNames_89, p_ExpectedNames_8A, p_ExpectedNames_8B, p_ExpectedNames_8C, p_ExpectedNames_8D, p_ExpectedNames_8E, p_ExpectedNames_8F, p_ExpectedNames_90, p_ExpectedNames_91, p_ExpectedNames_92, p_ExpectedNames_93, p_ExpectedNames_94, p_ExpectedNames_95, p_ExpectedNames_96, p_ExpectedNames_97, p_ExpectedNames_98, p_ExpectedNames_99, p_ExpectedNames_9A, p_ExpectedNames_9B, p_ExpectedNames_9C, p_ExpectedNames_9D, p_ExpectedNames_9E, p_ExpectedNames_9F, p_ExpectedNames_A0, p_ExpectedNames_A1, p_ExpectedNames_A2, p_ExpectedNames_A3, p_ExpectedNames_A4, p_ExpectedNames_A5, p_ExpectedNames_A6, p_ExpectedNames_A7, p_ExpectedNames_A8, p_ExpectedNames_A9, p_ExpectedNames_AA, p_ExpectedNames_AB, p_ExpectedNames_AC, p_ExpectedNames_AD, p_ExpectedNames_AE, p_ExpectedNames_AF, p_ExpectedNames_B0, p_ExpectedNames_B1, p_ExpectedNames_B2, p_ExpectedNames_B3, p_ExpectedNames_B4, p_ExpectedNames_B5, p_ExpectedNames_B6, p_ExpectedNames_B7, p_ExpectedNames_B8, p_ExpectedNames_B9, p_ExpectedNames_BA, p_ExpectedNames_BB, p_ExpectedNames_BC, p_ExpectedNames_BD, p_ExpectedNames_BE, p_ExpectedNames_BF, p_ExpectedNames_C0, p_ExpectedNames_C1, p_ExpectedNames_C2, p_ExpectedNames_C3, p_ExpectedNames_C4, p_ExpectedNames_C5, p_ExpectedNames_C6, p_ExpectedNames_C7, p_ExpectedNames_C8, p_ExpectedNames_C9, p_ExpectedNames_CA, p_ExpectedNames_CB, p_ExpectedNames_CC, p_ExpectedNames_CD, p_ExpectedNames_CE, p_ExpectedNames_CF, p_ExpectedNames_D0, p_ExpectedNames_D1, p_ExpectedNames_D2, p_ExpectedNames_D3, p_ExpectedNames_D4, p_ExpectedNames_D5, p_ExpectedNames_D6, p_ExpectedNames_D7, p_ExpectedNames_D8, p_ExpectedNames_D9, p_ExpectedNames_DA, p_ExpectedNames_DB, p_ExpectedNames_DC, p_ExpectedNames_DD, p_ExpectedNames_DE, p_ExpectedNames_DF, p_ExpectedNames_E0, p_ExpectedNames_E1, p_ExpectedNames_E2, p_ExpectedNames_E3, p_ExpectedNames_E4, p_ExpectedNames_E5, p_ExpectedNames_E6, p_ExpectedNames_E7, p_ExpectedNames_E8, p_ExpectedNames_E9, p_ExpectedNames_EA, p_ExpectedNames_EB, p_ExpectedNames_EC, p_ExpectedNames_ED, p_ExpectedNames_EE, p_ExpectedNames_EF, p_ExpectedNames_F0, p_ExpectedNames_F1, p_ExpectedNames_F2, p_ExpectedNames_F3, p_ExpectedNames_F4, p_ExpectedNames_F5, p_ExpectedNames_F6, p_ExpectedNames_F7, p_ExpectedNames_F8, p_ExpectedNames_F9, p_ExpectedNames_FA, p_ExpectedNames_FB, p_ExpectedNames_FC, p_ExpectedNames_FD, p_ExpectedNames_FE, p_ExpectedNames_FF, p_ExpectedNames_100, p_ExpectedNames_101, p_ExpectedNames_102, p_ExpectedNames_103, p_ExpectedNames_104, p_ExpectedNames_105, p_ExpectedNames_106, p_ExpectedNames_107, p_ExpectedNames_108, p_ExpectedNames_109, p_ExpectedNames_10A, p_ExpectedNames_10B, p_ExpectedNames_10C, p_ExpectedNames_10D, p_ExpectedNames_10E, p_ExpectedNames_10F, p_ExpectedNames_110, p_ExpectedNames_111, p_ExpectedNames_112, p_ExpectedNames_113, p_ExpectedNames_114, p_ExpectedNames_115, p_ExpectedNames_116, p_ExpectedNames_117, p_ExpectedNames_118, p_ExpectedNames_119, p_ExpectedNames_11A, p_ExpectedNames_11B, p_ExpectedNames_11C, p_ExpectedNames_11D, p_ExpectedNames_11E, p_ExpectedNames_11F, p_ExpectedNames_120, p_ExpectedNames_121, p_ExpectedNames_122, p_ExpectedNames_123, p_ExpectedNames_124, p_ExpectedNames_125, p_ExpectedNames_126 };
        private static string[][] p_Items = { p_Items_0, p_Items_1, p_Items_2, p_Items_3, p_Items_4, p_Items_5, p_Items_6, p_Items_7, p_Items_8, p_Items_9, p_Items_A, p_Items_B, p_Items_C, p_Items_D, p_Items_E, p_Items_F, p_Items_10, p_Items_11, p_Items_12, p_Items_13, p_Items_14, p_Items_15, p_Items_16, p_Items_17, p_Items_18, p_Items_19, p_Items_1A, p_Items_1B, p_Items_1C, p_Items_1D, p_Items_1E, p_Items_1F, p_Items_20, p_Items_21, p_Items_22, p_Items_23, p_Items_24, p_Items_25, p_Items_26, p_Items_27, p_Items_28, p_Items_29, p_Items_2A, p_Items_2B, p_Items_2C, p_Items_2D, p_Items_2E, p_Items_2F, p_Items_30, p_Items_31, p_Items_32, p_Items_33, p_Items_34, p_Items_35, p_Items_36, p_Items_37, p_Items_38, p_Items_39, p_Items_3A, p_Items_3B, p_Items_3C, p_Items_3D, p_Items_3E, p_Items_3F, p_Items_40, p_Items_41, p_Items_42, p_Items_43, p_Items_44, p_Items_45, p_Items_46, p_Items_47, p_Items_48, p_Items_49, p_Items_4A, p_Items_4B, p_Items_4C, p_Items_4D, p_Items_4E, p_Items_4F, p_Items_50, p_Items_51, p_Items_52, p_Items_53, p_Items_54, p_Items_55, p_Items_56, p_Items_57, p_Items_58, p_Items_59, p_Items_5A, p_Items_5B, p_Items_5C, p_Items_5D, p_Items_5E, p_Items_5F, p_Items_60, p_Items_61, p_Items_62, p_Items_63, p_Items_64, p_Items_65, p_Items_66, p_Items_67, p_Items_68, p_Items_69, p_Items_6A, p_Items_6B, p_Items_6C, p_Items_6D, p_Items_6E, p_Items_6F, p_Items_70, p_Items_71, p_Items_72, p_Items_73, p_Items_74, p_Items_75, p_Items_76, p_Items_77, p_Items_78, p_Items_79, p_Items_7A, p_Items_7B, p_Items_7C, p_Items_7D, p_Items_7E, p_Items_7F, p_Items_80, p_Items_81, p_Items_82, p_Items_83, p_Items_84, p_Items_85, p_Items_86, p_Items_87, p_Items_88, p_Items_89, p_Items_8A, p_Items_8B, p_Items_8C, p_Items_8D, p_Items_8E, p_Items_8F, p_Items_90, p_Items_91, p_Items_92, p_Items_93, p_Items_94, p_Items_95, p_Items_96, p_Items_97, p_Items_98, p_Items_99, p_Items_9A, p_Items_9B, p_Items_9C, p_Items_9D, p_Items_9E, p_Items_9F, p_Items_A0, p_Items_A1, p_Items_A2, p_Items_A3, p_Items_A4, p_Items_A5, p_Items_A6, p_Items_A7, p_Items_A8, p_Items_A9, p_Items_AA, p_Items_AB, p_Items_AC, p_Items_AD, p_Items_AE, p_Items_AF, p_Items_B0, p_Items_B1, p_Items_B2, p_Items_B3, p_Items_B4, p_Items_B5, p_Items_B6, p_Items_B7, p_Items_B8, p_Items_B9, p_Items_BA, p_Items_BB, p_Items_BC, p_Items_BD, p_Items_BE, p_Items_BF, p_Items_C0, p_Items_C1, p_Items_C2, p_Items_C3, p_Items_C4, p_Items_C5, p_Items_C6, p_Items_C7, p_Items_C8, p_Items_C9, p_Items_CA, p_Items_CB, p_Items_CC, p_Items_CD, p_Items_CE, p_Items_CF, p_Items_D0, p_Items_D1, p_Items_D2, p_Items_D3, p_Items_D4, p_Items_D5, p_Items_D6, p_Items_D7, p_Items_D8, p_Items_D9, p_Items_DA, p_Items_DB, p_Items_DC, p_Items_DD, p_Items_DE, p_Items_DF, p_Items_E0, p_Items_E1, p_Items_E2, p_Items_E3, p_Items_E4, p_Items_E5, p_Items_E6, p_Items_E7, p_Items_E8, p_Items_E9, p_Items_EA, p_Items_EB, p_Items_EC, p_Items_ED, p_Items_EE, p_Items_EF, p_Items_F0, p_Items_F1, p_Items_F2, p_Items_F3, p_Items_F4, p_Items_F5, p_Items_F6, p_Items_F7, p_Items_F8, p_Items_F9, p_Items_FA, p_Items_FB, p_Items_FC, p_Items_FD, p_Items_FE, p_Items_FF, p_Items_100, p_Items_101, p_Items_102, p_Items_103, p_Items_104, p_Items_105, p_Items_106, p_Items_107, p_Items_108, p_Items_109, p_Items_10A, p_Items_10B, p_Items_10C, p_Items_10D, p_Items_10E, p_Items_10F, p_Items_110, p_Items_111, p_Items_112, p_Items_113, p_Items_114, p_Items_115, p_Items_116, p_Items_117, p_Items_118, p_Items_119, p_Items_11A, p_Items_11B, p_Items_11C, p_Items_11D, p_Items_11E, p_Items_11F, p_Items_120, p_Items_121, p_Items_122, p_Items_123, p_Items_124, p_Items_125, p_Items_126 };
        private static ushort[][][] p_ShiftsOnTerminal = { p_ShiftsOnTerminal_0, p_ShiftsOnTerminal_1, p_ShiftsOnTerminal_2, p_ShiftsOnTerminal_3, p_ShiftsOnTerminal_4, p_ShiftsOnTerminal_5, p_ShiftsOnTerminal_6, p_ShiftsOnTerminal_7, p_ShiftsOnTerminal_8, p_ShiftsOnTerminal_9, p_ShiftsOnTerminal_A, p_ShiftsOnTerminal_B, p_ShiftsOnTerminal_C, p_ShiftsOnTerminal_D, p_ShiftsOnTerminal_E, p_ShiftsOnTerminal_F, p_ShiftsOnTerminal_10, p_ShiftsOnTerminal_11, p_ShiftsOnTerminal_12, p_ShiftsOnTerminal_13, p_ShiftsOnTerminal_14, p_ShiftsOnTerminal_15, p_ShiftsOnTerminal_16, p_ShiftsOnTerminal_17, p_ShiftsOnTerminal_18, p_ShiftsOnTerminal_19, p_ShiftsOnTerminal_1A, p_ShiftsOnTerminal_1B, p_ShiftsOnTerminal_1C, p_ShiftsOnTerminal_1D, p_ShiftsOnTerminal_1E, p_ShiftsOnTerminal_1F, p_ShiftsOnTerminal_20, p_ShiftsOnTerminal_21, p_ShiftsOnTerminal_22, p_ShiftsOnTerminal_23, p_ShiftsOnTerminal_24, p_ShiftsOnTerminal_25, p_ShiftsOnTerminal_26, p_ShiftsOnTerminal_27, p_ShiftsOnTerminal_28, p_ShiftsOnTerminal_29, p_ShiftsOnTerminal_2A, p_ShiftsOnTerminal_2B, p_ShiftsOnTerminal_2C, p_ShiftsOnTerminal_2D, p_ShiftsOnTerminal_2E, p_ShiftsOnTerminal_2F, p_ShiftsOnTerminal_30, p_ShiftsOnTerminal_31, p_ShiftsOnTerminal_32, p_ShiftsOnTerminal_33, p_ShiftsOnTerminal_34, p_ShiftsOnTerminal_35, p_ShiftsOnTerminal_36, p_ShiftsOnTerminal_37, p_ShiftsOnTerminal_38, p_ShiftsOnTerminal_39, p_ShiftsOnTerminal_3A, p_ShiftsOnTerminal_3B, p_ShiftsOnTerminal_3C, p_ShiftsOnTerminal_3D, p_ShiftsOnTerminal_3E, p_ShiftsOnTerminal_3F, p_ShiftsOnTerminal_40, p_ShiftsOnTerminal_41, p_ShiftsOnTerminal_42, p_ShiftsOnTerminal_43, p_ShiftsOnTerminal_44, p_ShiftsOnTerminal_45, p_ShiftsOnTerminal_46, p_ShiftsOnTerminal_47, p_ShiftsOnTerminal_48, p_ShiftsOnTerminal_49, p_ShiftsOnTerminal_4A, p_ShiftsOnTerminal_4B, p_ShiftsOnTerminal_4C, p_ShiftsOnTerminal_4D, p_ShiftsOnTerminal_4E, p_ShiftsOnTerminal_4F, p_ShiftsOnTerminal_50, p_ShiftsOnTerminal_51, p_ShiftsOnTerminal_52, p_ShiftsOnTerminal_53, p_ShiftsOnTerminal_54, p_ShiftsOnTerminal_55, p_ShiftsOnTerminal_56, p_ShiftsOnTerminal_57, p_ShiftsOnTerminal_58, p_ShiftsOnTerminal_59, p_ShiftsOnTerminal_5A, p_ShiftsOnTerminal_5B, p_ShiftsOnTerminal_5C, p_ShiftsOnTerminal_5D, p_ShiftsOnTerminal_5E, p_ShiftsOnTerminal_5F, p_ShiftsOnTerminal_60, p_ShiftsOnTerminal_61, p_ShiftsOnTerminal_62, p_ShiftsOnTerminal_63, p_ShiftsOnTerminal_64, p_ShiftsOnTerminal_65, p_ShiftsOnTerminal_66, p_ShiftsOnTerminal_67, p_ShiftsOnTerminal_68, p_ShiftsOnTerminal_69, p_ShiftsOnTerminal_6A, p_ShiftsOnTerminal_6B, p_ShiftsOnTerminal_6C, p_ShiftsOnTerminal_6D, p_ShiftsOnTerminal_6E, p_ShiftsOnTerminal_6F, p_ShiftsOnTerminal_70, p_ShiftsOnTerminal_71, p_ShiftsOnTerminal_72, p_ShiftsOnTerminal_73, p_ShiftsOnTerminal_74, p_ShiftsOnTerminal_75, p_ShiftsOnTerminal_76, p_ShiftsOnTerminal_77, p_ShiftsOnTerminal_78, p_ShiftsOnTerminal_79, p_ShiftsOnTerminal_7A, p_ShiftsOnTerminal_7B, p_ShiftsOnTerminal_7C, p_ShiftsOnTerminal_7D, p_ShiftsOnTerminal_7E, p_ShiftsOnTerminal_7F, p_ShiftsOnTerminal_80, p_ShiftsOnTerminal_81, p_ShiftsOnTerminal_82, p_ShiftsOnTerminal_83, p_ShiftsOnTerminal_84, p_ShiftsOnTerminal_85, p_ShiftsOnTerminal_86, p_ShiftsOnTerminal_87, p_ShiftsOnTerminal_88, p_ShiftsOnTerminal_89, p_ShiftsOnTerminal_8A, p_ShiftsOnTerminal_8B, p_ShiftsOnTerminal_8C, p_ShiftsOnTerminal_8D, p_ShiftsOnTerminal_8E, p_ShiftsOnTerminal_8F, p_ShiftsOnTerminal_90, p_ShiftsOnTerminal_91, p_ShiftsOnTerminal_92, p_ShiftsOnTerminal_93, p_ShiftsOnTerminal_94, p_ShiftsOnTerminal_95, p_ShiftsOnTerminal_96, p_ShiftsOnTerminal_97, p_ShiftsOnTerminal_98, p_ShiftsOnTerminal_99, p_ShiftsOnTerminal_9A, p_ShiftsOnTerminal_9B, p_ShiftsOnTerminal_9C, p_ShiftsOnTerminal_9D, p_ShiftsOnTerminal_9E, p_ShiftsOnTerminal_9F, p_ShiftsOnTerminal_A0, p_ShiftsOnTerminal_A1, p_ShiftsOnTerminal_A2, p_ShiftsOnTerminal_A3, p_ShiftsOnTerminal_A4, p_ShiftsOnTerminal_A5, p_ShiftsOnTerminal_A6, p_ShiftsOnTerminal_A7, p_ShiftsOnTerminal_A8, p_ShiftsOnTerminal_A9, p_ShiftsOnTerminal_AA, p_ShiftsOnTerminal_AB, p_ShiftsOnTerminal_AC, p_ShiftsOnTerminal_AD, p_ShiftsOnTerminal_AE, p_ShiftsOnTerminal_AF, p_ShiftsOnTerminal_B0, p_ShiftsOnTerminal_B1, p_ShiftsOnTerminal_B2, p_ShiftsOnTerminal_B3, p_ShiftsOnTerminal_B4, p_ShiftsOnTerminal_B5, p_ShiftsOnTerminal_B6, p_ShiftsOnTerminal_B7, p_ShiftsOnTerminal_B8, p_ShiftsOnTerminal_B9, p_ShiftsOnTerminal_BA, p_ShiftsOnTerminal_BB, p_ShiftsOnTerminal_BC, p_ShiftsOnTerminal_BD, p_ShiftsOnTerminal_BE, p_ShiftsOnTerminal_BF, p_ShiftsOnTerminal_C0, p_ShiftsOnTerminal_C1, p_ShiftsOnTerminal_C2, p_ShiftsOnTerminal_C3, p_ShiftsOnTerminal_C4, p_ShiftsOnTerminal_C5, p_ShiftsOnTerminal_C6, p_ShiftsOnTerminal_C7, p_ShiftsOnTerminal_C8, p_ShiftsOnTerminal_C9, p_ShiftsOnTerminal_CA, p_ShiftsOnTerminal_CB, p_ShiftsOnTerminal_CC, p_ShiftsOnTerminal_CD, p_ShiftsOnTerminal_CE, p_ShiftsOnTerminal_CF, p_ShiftsOnTerminal_D0, p_ShiftsOnTerminal_D1, p_ShiftsOnTerminal_D2, p_ShiftsOnTerminal_D3, p_ShiftsOnTerminal_D4, p_ShiftsOnTerminal_D5, p_ShiftsOnTerminal_D6, p_ShiftsOnTerminal_D7, p_ShiftsOnTerminal_D8, p_ShiftsOnTerminal_D9, p_ShiftsOnTerminal_DA, p_ShiftsOnTerminal_DB, p_ShiftsOnTerminal_DC, p_ShiftsOnTerminal_DD, p_ShiftsOnTerminal_DE, p_ShiftsOnTerminal_DF, p_ShiftsOnTerminal_E0, p_ShiftsOnTerminal_E1, p_ShiftsOnTerminal_E2, p_ShiftsOnTerminal_E3, p_ShiftsOnTerminal_E4, p_ShiftsOnTerminal_E5, p_ShiftsOnTerminal_E6, p_ShiftsOnTerminal_E7, p_ShiftsOnTerminal_E8, p_ShiftsOnTerminal_E9, p_ShiftsOnTerminal_EA, p_ShiftsOnTerminal_EB, p_ShiftsOnTerminal_EC, p_ShiftsOnTerminal_ED, p_ShiftsOnTerminal_EE, p_ShiftsOnTerminal_EF, p_ShiftsOnTerminal_F0, p_ShiftsOnTerminal_F1, p_ShiftsOnTerminal_F2, p_ShiftsOnTerminal_F3, p_ShiftsOnTerminal_F4, p_ShiftsOnTerminal_F5, p_ShiftsOnTerminal_F6, p_ShiftsOnTerminal_F7, p_ShiftsOnTerminal_F8, p_ShiftsOnTerminal_F9, p_ShiftsOnTerminal_FA, p_ShiftsOnTerminal_FB, p_ShiftsOnTerminal_FC, p_ShiftsOnTerminal_FD, p_ShiftsOnTerminal_FE, p_ShiftsOnTerminal_FF, p_ShiftsOnTerminal_100, p_ShiftsOnTerminal_101, p_ShiftsOnTerminal_102, p_ShiftsOnTerminal_103, p_ShiftsOnTerminal_104, p_ShiftsOnTerminal_105, p_ShiftsOnTerminal_106, p_ShiftsOnTerminal_107, p_ShiftsOnTerminal_108, p_ShiftsOnTerminal_109, p_ShiftsOnTerminal_10A, p_ShiftsOnTerminal_10B, p_ShiftsOnTerminal_10C, p_ShiftsOnTerminal_10D, p_ShiftsOnTerminal_10E, p_ShiftsOnTerminal_10F, p_ShiftsOnTerminal_110, p_ShiftsOnTerminal_111, p_ShiftsOnTerminal_112, p_ShiftsOnTerminal_113, p_ShiftsOnTerminal_114, p_ShiftsOnTerminal_115, p_ShiftsOnTerminal_116, p_ShiftsOnTerminal_117, p_ShiftsOnTerminal_118, p_ShiftsOnTerminal_119, p_ShiftsOnTerminal_11A, p_ShiftsOnTerminal_11B, p_ShiftsOnTerminal_11C, p_ShiftsOnTerminal_11D, p_ShiftsOnTerminal_11E, p_ShiftsOnTerminal_11F, p_ShiftsOnTerminal_120, p_ShiftsOnTerminal_121, p_ShiftsOnTerminal_122, p_ShiftsOnTerminal_123, p_ShiftsOnTerminal_124, p_ShiftsOnTerminal_125, p_ShiftsOnTerminal_126 };
        private static ushort[][][] p_ShiftsOnVariable = { p_ShiftsOnVariable_0, p_ShiftsOnVariable_1, p_ShiftsOnVariable_2, p_ShiftsOnVariable_3, p_ShiftsOnVariable_4, p_ShiftsOnVariable_5, p_ShiftsOnVariable_6, p_ShiftsOnVariable_7, p_ShiftsOnVariable_8, p_ShiftsOnVariable_9, p_ShiftsOnVariable_A, p_ShiftsOnVariable_B, p_ShiftsOnVariable_C, p_ShiftsOnVariable_D, p_ShiftsOnVariable_E, p_ShiftsOnVariable_F, p_ShiftsOnVariable_10, p_ShiftsOnVariable_11, p_ShiftsOnVariable_12, p_ShiftsOnVariable_13, p_ShiftsOnVariable_14, p_ShiftsOnVariable_15, p_ShiftsOnVariable_16, p_ShiftsOnVariable_17, p_ShiftsOnVariable_18, p_ShiftsOnVariable_19, p_ShiftsOnVariable_1A, p_ShiftsOnVariable_1B, p_ShiftsOnVariable_1C, p_ShiftsOnVariable_1D, p_ShiftsOnVariable_1E, p_ShiftsOnVariable_1F, p_ShiftsOnVariable_20, p_ShiftsOnVariable_21, p_ShiftsOnVariable_22, p_ShiftsOnVariable_23, p_ShiftsOnVariable_24, p_ShiftsOnVariable_25, p_ShiftsOnVariable_26, p_ShiftsOnVariable_27, p_ShiftsOnVariable_28, p_ShiftsOnVariable_29, p_ShiftsOnVariable_2A, p_ShiftsOnVariable_2B, p_ShiftsOnVariable_2C, p_ShiftsOnVariable_2D, p_ShiftsOnVariable_2E, p_ShiftsOnVariable_2F, p_ShiftsOnVariable_30, p_ShiftsOnVariable_31, p_ShiftsOnVariable_32, p_ShiftsOnVariable_33, p_ShiftsOnVariable_34, p_ShiftsOnVariable_35, p_ShiftsOnVariable_36, p_ShiftsOnVariable_37, p_ShiftsOnVariable_38, p_ShiftsOnVariable_39, p_ShiftsOnVariable_3A, p_ShiftsOnVariable_3B, p_ShiftsOnVariable_3C, p_ShiftsOnVariable_3D, p_ShiftsOnVariable_3E, p_ShiftsOnVariable_3F, p_ShiftsOnVariable_40, p_ShiftsOnVariable_41, p_ShiftsOnVariable_42, p_ShiftsOnVariable_43, p_ShiftsOnVariable_44, p_ShiftsOnVariable_45, p_ShiftsOnVariable_46, p_ShiftsOnVariable_47, p_ShiftsOnVariable_48, p_ShiftsOnVariable_49, p_ShiftsOnVariable_4A, p_ShiftsOnVariable_4B, p_ShiftsOnVariable_4C, p_ShiftsOnVariable_4D, p_ShiftsOnVariable_4E, p_ShiftsOnVariable_4F, p_ShiftsOnVariable_50, p_ShiftsOnVariable_51, p_ShiftsOnVariable_52, p_ShiftsOnVariable_53, p_ShiftsOnVariable_54, p_ShiftsOnVariable_55, p_ShiftsOnVariable_56, p_ShiftsOnVariable_57, p_ShiftsOnVariable_58, p_ShiftsOnVariable_59, p_ShiftsOnVariable_5A, p_ShiftsOnVariable_5B, p_ShiftsOnVariable_5C, p_ShiftsOnVariable_5D, p_ShiftsOnVariable_5E, p_ShiftsOnVariable_5F, p_ShiftsOnVariable_60, p_ShiftsOnVariable_61, p_ShiftsOnVariable_62, p_ShiftsOnVariable_63, p_ShiftsOnVariable_64, p_ShiftsOnVariable_65, p_ShiftsOnVariable_66, p_ShiftsOnVariable_67, p_ShiftsOnVariable_68, p_ShiftsOnVariable_69, p_ShiftsOnVariable_6A, p_ShiftsOnVariable_6B, p_ShiftsOnVariable_6C, p_ShiftsOnVariable_6D, p_ShiftsOnVariable_6E, p_ShiftsOnVariable_6F, p_ShiftsOnVariable_70, p_ShiftsOnVariable_71, p_ShiftsOnVariable_72, p_ShiftsOnVariable_73, p_ShiftsOnVariable_74, p_ShiftsOnVariable_75, p_ShiftsOnVariable_76, p_ShiftsOnVariable_77, p_ShiftsOnVariable_78, p_ShiftsOnVariable_79, p_ShiftsOnVariable_7A, p_ShiftsOnVariable_7B, p_ShiftsOnVariable_7C, p_ShiftsOnVariable_7D, p_ShiftsOnVariable_7E, p_ShiftsOnVariable_7F, p_ShiftsOnVariable_80, p_ShiftsOnVariable_81, p_ShiftsOnVariable_82, p_ShiftsOnVariable_83, p_ShiftsOnVariable_84, p_ShiftsOnVariable_85, p_ShiftsOnVariable_86, p_ShiftsOnVariable_87, p_ShiftsOnVariable_88, p_ShiftsOnVariable_89, p_ShiftsOnVariable_8A, p_ShiftsOnVariable_8B, p_ShiftsOnVariable_8C, p_ShiftsOnVariable_8D, p_ShiftsOnVariable_8E, p_ShiftsOnVariable_8F, p_ShiftsOnVariable_90, p_ShiftsOnVariable_91, p_ShiftsOnVariable_92, p_ShiftsOnVariable_93, p_ShiftsOnVariable_94, p_ShiftsOnVariable_95, p_ShiftsOnVariable_96, p_ShiftsOnVariable_97, p_ShiftsOnVariable_98, p_ShiftsOnVariable_99, p_ShiftsOnVariable_9A, p_ShiftsOnVariable_9B, p_ShiftsOnVariable_9C, p_ShiftsOnVariable_9D, p_ShiftsOnVariable_9E, p_ShiftsOnVariable_9F, p_ShiftsOnVariable_A0, p_ShiftsOnVariable_A1, p_ShiftsOnVariable_A2, p_ShiftsOnVariable_A3, p_ShiftsOnVariable_A4, p_ShiftsOnVariable_A5, p_ShiftsOnVariable_A6, p_ShiftsOnVariable_A7, p_ShiftsOnVariable_A8, p_ShiftsOnVariable_A9, p_ShiftsOnVariable_AA, p_ShiftsOnVariable_AB, p_ShiftsOnVariable_AC, p_ShiftsOnVariable_AD, p_ShiftsOnVariable_AE, p_ShiftsOnVariable_AF, p_ShiftsOnVariable_B0, p_ShiftsOnVariable_B1, p_ShiftsOnVariable_B2, p_ShiftsOnVariable_B3, p_ShiftsOnVariable_B4, p_ShiftsOnVariable_B5, p_ShiftsOnVariable_B6, p_ShiftsOnVariable_B7, p_ShiftsOnVariable_B8, p_ShiftsOnVariable_B9, p_ShiftsOnVariable_BA, p_ShiftsOnVariable_BB, p_ShiftsOnVariable_BC, p_ShiftsOnVariable_BD, p_ShiftsOnVariable_BE, p_ShiftsOnVariable_BF, p_ShiftsOnVariable_C0, p_ShiftsOnVariable_C1, p_ShiftsOnVariable_C2, p_ShiftsOnVariable_C3, p_ShiftsOnVariable_C4, p_ShiftsOnVariable_C5, p_ShiftsOnVariable_C6, p_ShiftsOnVariable_C7, p_ShiftsOnVariable_C8, p_ShiftsOnVariable_C9, p_ShiftsOnVariable_CA, p_ShiftsOnVariable_CB, p_ShiftsOnVariable_CC, p_ShiftsOnVariable_CD, p_ShiftsOnVariable_CE, p_ShiftsOnVariable_CF, p_ShiftsOnVariable_D0, p_ShiftsOnVariable_D1, p_ShiftsOnVariable_D2, p_ShiftsOnVariable_D3, p_ShiftsOnVariable_D4, p_ShiftsOnVariable_D5, p_ShiftsOnVariable_D6, p_ShiftsOnVariable_D7, p_ShiftsOnVariable_D8, p_ShiftsOnVariable_D9, p_ShiftsOnVariable_DA, p_ShiftsOnVariable_DB, p_ShiftsOnVariable_DC, p_ShiftsOnVariable_DD, p_ShiftsOnVariable_DE, p_ShiftsOnVariable_DF, p_ShiftsOnVariable_E0, p_ShiftsOnVariable_E1, p_ShiftsOnVariable_E2, p_ShiftsOnVariable_E3, p_ShiftsOnVariable_E4, p_ShiftsOnVariable_E5, p_ShiftsOnVariable_E6, p_ShiftsOnVariable_E7, p_ShiftsOnVariable_E8, p_ShiftsOnVariable_E9, p_ShiftsOnVariable_EA, p_ShiftsOnVariable_EB, p_ShiftsOnVariable_EC, p_ShiftsOnVariable_ED, p_ShiftsOnVariable_EE, p_ShiftsOnVariable_EF, p_ShiftsOnVariable_F0, p_ShiftsOnVariable_F1, p_ShiftsOnVariable_F2, p_ShiftsOnVariable_F3, p_ShiftsOnVariable_F4, p_ShiftsOnVariable_F5, p_ShiftsOnVariable_F6, p_ShiftsOnVariable_F7, p_ShiftsOnVariable_F8, p_ShiftsOnVariable_F9, p_ShiftsOnVariable_FA, p_ShiftsOnVariable_FB, p_ShiftsOnVariable_FC, p_ShiftsOnVariable_FD, p_ShiftsOnVariable_FE, p_ShiftsOnVariable_FF, p_ShiftsOnVariable_100, p_ShiftsOnVariable_101, p_ShiftsOnVariable_102, p_ShiftsOnVariable_103, p_ShiftsOnVariable_104, p_ShiftsOnVariable_105, p_ShiftsOnVariable_106, p_ShiftsOnVariable_107, p_ShiftsOnVariable_108, p_ShiftsOnVariable_109, p_ShiftsOnVariable_10A, p_ShiftsOnVariable_10B, p_ShiftsOnVariable_10C, p_ShiftsOnVariable_10D, p_ShiftsOnVariable_10E, p_ShiftsOnVariable_10F, p_ShiftsOnVariable_110, p_ShiftsOnVariable_111, p_ShiftsOnVariable_112, p_ShiftsOnVariable_113, p_ShiftsOnVariable_114, p_ShiftsOnVariable_115, p_ShiftsOnVariable_116, p_ShiftsOnVariable_117, p_ShiftsOnVariable_118, p_ShiftsOnVariable_119, p_ShiftsOnVariable_11A, p_ShiftsOnVariable_11B, p_ShiftsOnVariable_11C, p_ShiftsOnVariable_11D, p_ShiftsOnVariable_11E, p_ShiftsOnVariable_11F, p_ShiftsOnVariable_120, p_ShiftsOnVariable_121, p_ShiftsOnVariable_122, p_ShiftsOnVariable_123, p_ShiftsOnVariable_124, p_ShiftsOnVariable_125, p_ShiftsOnVariable_126 };
        private static ushort[][][] p_ReducsOnTerminal = { p_ReducsOnTerminal_0, p_ReducsOnTerminal_1, p_ReducsOnTerminal_2, p_ReducsOnTerminal_3, p_ReducsOnTerminal_4, p_ReducsOnTerminal_5, p_ReducsOnTerminal_6, p_ReducsOnTerminal_7, p_ReducsOnTerminal_8, p_ReducsOnTerminal_9, p_ReducsOnTerminal_A, p_ReducsOnTerminal_B, p_ReducsOnTerminal_C, p_ReducsOnTerminal_D, p_ReducsOnTerminal_E, p_ReducsOnTerminal_F, p_ReducsOnTerminal_10, p_ReducsOnTerminal_11, p_ReducsOnTerminal_12, p_ReducsOnTerminal_13, p_ReducsOnTerminal_14, p_ReducsOnTerminal_15, p_ReducsOnTerminal_16, p_ReducsOnTerminal_17, p_ReducsOnTerminal_18, p_ReducsOnTerminal_19, p_ReducsOnTerminal_1A, p_ReducsOnTerminal_1B, p_ReducsOnTerminal_1C, p_ReducsOnTerminal_1D, p_ReducsOnTerminal_1E, p_ReducsOnTerminal_1F, p_ReducsOnTerminal_20, p_ReducsOnTerminal_21, p_ReducsOnTerminal_22, p_ReducsOnTerminal_23, p_ReducsOnTerminal_24, p_ReducsOnTerminal_25, p_ReducsOnTerminal_26, p_ReducsOnTerminal_27, p_ReducsOnTerminal_28, p_ReducsOnTerminal_29, p_ReducsOnTerminal_2A, p_ReducsOnTerminal_2B, p_ReducsOnTerminal_2C, p_ReducsOnTerminal_2D, p_ReducsOnTerminal_2E, p_ReducsOnTerminal_2F, p_ReducsOnTerminal_30, p_ReducsOnTerminal_31, p_ReducsOnTerminal_32, p_ReducsOnTerminal_33, p_ReducsOnTerminal_34, p_ReducsOnTerminal_35, p_ReducsOnTerminal_36, p_ReducsOnTerminal_37, p_ReducsOnTerminal_38, p_ReducsOnTerminal_39, p_ReducsOnTerminal_3A, p_ReducsOnTerminal_3B, p_ReducsOnTerminal_3C, p_ReducsOnTerminal_3D, p_ReducsOnTerminal_3E, p_ReducsOnTerminal_3F, p_ReducsOnTerminal_40, p_ReducsOnTerminal_41, p_ReducsOnTerminal_42, p_ReducsOnTerminal_43, p_ReducsOnTerminal_44, p_ReducsOnTerminal_45, p_ReducsOnTerminal_46, p_ReducsOnTerminal_47, p_ReducsOnTerminal_48, p_ReducsOnTerminal_49, p_ReducsOnTerminal_4A, p_ReducsOnTerminal_4B, p_ReducsOnTerminal_4C, p_ReducsOnTerminal_4D, p_ReducsOnTerminal_4E, p_ReducsOnTerminal_4F, p_ReducsOnTerminal_50, p_ReducsOnTerminal_51, p_ReducsOnTerminal_52, p_ReducsOnTerminal_53, p_ReducsOnTerminal_54, p_ReducsOnTerminal_55, p_ReducsOnTerminal_56, p_ReducsOnTerminal_57, p_ReducsOnTerminal_58, p_ReducsOnTerminal_59, p_ReducsOnTerminal_5A, p_ReducsOnTerminal_5B, p_ReducsOnTerminal_5C, p_ReducsOnTerminal_5D, p_ReducsOnTerminal_5E, p_ReducsOnTerminal_5F, p_ReducsOnTerminal_60, p_ReducsOnTerminal_61, p_ReducsOnTerminal_62, p_ReducsOnTerminal_63, p_ReducsOnTerminal_64, p_ReducsOnTerminal_65, p_ReducsOnTerminal_66, p_ReducsOnTerminal_67, p_ReducsOnTerminal_68, p_ReducsOnTerminal_69, p_ReducsOnTerminal_6A, p_ReducsOnTerminal_6B, p_ReducsOnTerminal_6C, p_ReducsOnTerminal_6D, p_ReducsOnTerminal_6E, p_ReducsOnTerminal_6F, p_ReducsOnTerminal_70, p_ReducsOnTerminal_71, p_ReducsOnTerminal_72, p_ReducsOnTerminal_73, p_ReducsOnTerminal_74, p_ReducsOnTerminal_75, p_ReducsOnTerminal_76, p_ReducsOnTerminal_77, p_ReducsOnTerminal_78, p_ReducsOnTerminal_79, p_ReducsOnTerminal_7A, p_ReducsOnTerminal_7B, p_ReducsOnTerminal_7C, p_ReducsOnTerminal_7D, p_ReducsOnTerminal_7E, p_ReducsOnTerminal_7F, p_ReducsOnTerminal_80, p_ReducsOnTerminal_81, p_ReducsOnTerminal_82, p_ReducsOnTerminal_83, p_ReducsOnTerminal_84, p_ReducsOnTerminal_85, p_ReducsOnTerminal_86, p_ReducsOnTerminal_87, p_ReducsOnTerminal_88, p_ReducsOnTerminal_89, p_ReducsOnTerminal_8A, p_ReducsOnTerminal_8B, p_ReducsOnTerminal_8C, p_ReducsOnTerminal_8D, p_ReducsOnTerminal_8E, p_ReducsOnTerminal_8F, p_ReducsOnTerminal_90, p_ReducsOnTerminal_91, p_ReducsOnTerminal_92, p_ReducsOnTerminal_93, p_ReducsOnTerminal_94, p_ReducsOnTerminal_95, p_ReducsOnTerminal_96, p_ReducsOnTerminal_97, p_ReducsOnTerminal_98, p_ReducsOnTerminal_99, p_ReducsOnTerminal_9A, p_ReducsOnTerminal_9B, p_ReducsOnTerminal_9C, p_ReducsOnTerminal_9D, p_ReducsOnTerminal_9E, p_ReducsOnTerminal_9F, p_ReducsOnTerminal_A0, p_ReducsOnTerminal_A1, p_ReducsOnTerminal_A2, p_ReducsOnTerminal_A3, p_ReducsOnTerminal_A4, p_ReducsOnTerminal_A5, p_ReducsOnTerminal_A6, p_ReducsOnTerminal_A7, p_ReducsOnTerminal_A8, p_ReducsOnTerminal_A9, p_ReducsOnTerminal_AA, p_ReducsOnTerminal_AB, p_ReducsOnTerminal_AC, p_ReducsOnTerminal_AD, p_ReducsOnTerminal_AE, p_ReducsOnTerminal_AF, p_ReducsOnTerminal_B0, p_ReducsOnTerminal_B1, p_ReducsOnTerminal_B2, p_ReducsOnTerminal_B3, p_ReducsOnTerminal_B4, p_ReducsOnTerminal_B5, p_ReducsOnTerminal_B6, p_ReducsOnTerminal_B7, p_ReducsOnTerminal_B8, p_ReducsOnTerminal_B9, p_ReducsOnTerminal_BA, p_ReducsOnTerminal_BB, p_ReducsOnTerminal_BC, p_ReducsOnTerminal_BD, p_ReducsOnTerminal_BE, p_ReducsOnTerminal_BF, p_ReducsOnTerminal_C0, p_ReducsOnTerminal_C1, p_ReducsOnTerminal_C2, p_ReducsOnTerminal_C3, p_ReducsOnTerminal_C4, p_ReducsOnTerminal_C5, p_ReducsOnTerminal_C6, p_ReducsOnTerminal_C7, p_ReducsOnTerminal_C8, p_ReducsOnTerminal_C9, p_ReducsOnTerminal_CA, p_ReducsOnTerminal_CB, p_ReducsOnTerminal_CC, p_ReducsOnTerminal_CD, p_ReducsOnTerminal_CE, p_ReducsOnTerminal_CF, p_ReducsOnTerminal_D0, p_ReducsOnTerminal_D1, p_ReducsOnTerminal_D2, p_ReducsOnTerminal_D3, p_ReducsOnTerminal_D4, p_ReducsOnTerminal_D5, p_ReducsOnTerminal_D6, p_ReducsOnTerminal_D7, p_ReducsOnTerminal_D8, p_ReducsOnTerminal_D9, p_ReducsOnTerminal_DA, p_ReducsOnTerminal_DB, p_ReducsOnTerminal_DC, p_ReducsOnTerminal_DD, p_ReducsOnTerminal_DE, p_ReducsOnTerminal_DF, p_ReducsOnTerminal_E0, p_ReducsOnTerminal_E1, p_ReducsOnTerminal_E2, p_ReducsOnTerminal_E3, p_ReducsOnTerminal_E4, p_ReducsOnTerminal_E5, p_ReducsOnTerminal_E6, p_ReducsOnTerminal_E7, p_ReducsOnTerminal_E8, p_ReducsOnTerminal_E9, p_ReducsOnTerminal_EA, p_ReducsOnTerminal_EB, p_ReducsOnTerminal_EC, p_ReducsOnTerminal_ED, p_ReducsOnTerminal_EE, p_ReducsOnTerminal_EF, p_ReducsOnTerminal_F0, p_ReducsOnTerminal_F1, p_ReducsOnTerminal_F2, p_ReducsOnTerminal_F3, p_ReducsOnTerminal_F4, p_ReducsOnTerminal_F5, p_ReducsOnTerminal_F6, p_ReducsOnTerminal_F7, p_ReducsOnTerminal_F8, p_ReducsOnTerminal_F9, p_ReducsOnTerminal_FA, p_ReducsOnTerminal_FB, p_ReducsOnTerminal_FC, p_ReducsOnTerminal_FD, p_ReducsOnTerminal_FE, p_ReducsOnTerminal_FF, p_ReducsOnTerminal_100, p_ReducsOnTerminal_101, p_ReducsOnTerminal_102, p_ReducsOnTerminal_103, p_ReducsOnTerminal_104, p_ReducsOnTerminal_105, p_ReducsOnTerminal_106, p_ReducsOnTerminal_107, p_ReducsOnTerminal_108, p_ReducsOnTerminal_109, p_ReducsOnTerminal_10A, p_ReducsOnTerminal_10B, p_ReducsOnTerminal_10C, p_ReducsOnTerminal_10D, p_ReducsOnTerminal_10E, p_ReducsOnTerminal_10F, p_ReducsOnTerminal_110, p_ReducsOnTerminal_111, p_ReducsOnTerminal_112, p_ReducsOnTerminal_113, p_ReducsOnTerminal_114, p_ReducsOnTerminal_115, p_ReducsOnTerminal_116, p_ReducsOnTerminal_117, p_ReducsOnTerminal_118, p_ReducsOnTerminal_119, p_ReducsOnTerminal_11A, p_ReducsOnTerminal_11B, p_ReducsOnTerminal_11C, p_ReducsOnTerminal_11D, p_ReducsOnTerminal_11E, p_ReducsOnTerminal_11F, p_ReducsOnTerminal_120, p_ReducsOnTerminal_121, p_ReducsOnTerminal_122, p_ReducsOnTerminal_123, p_ReducsOnTerminal_124, p_ReducsOnTerminal_125, p_ReducsOnTerminal_126 };

        private System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError> p_Errors;
        private Lexer_Hime_Kernel_FileCentralDogma p_Lexer;
        private Hime.Kernel.Parsers.SyntaxTreeNodeCollection p_Nodes;
        private System.Collections.Generic.Stack<ushort> p_Stack;
        private Hime.Kernel.Parsers.SymbolToken p_NextToken;
        private ushort p_CurrentState;

        public System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError> Errors { get { return p_Errors; } }

        private static ushort Analyse_GetNextByShiftOnTerminal(ushort State, ushort SID)
        {
            for (int i = 0; i != p_ShiftsOnTerminal[State].Length; i++)
            {
                if (p_ShiftsOnTerminal[State][i][0] == SID)
                    return p_ShiftsOnTerminal[State][i][1];
            }
            return 0xFFFF;
        }
        private static ushort Analyse_GetNextByShiftOnVariable(ushort State, ushort SID)
        {
            for (int i = 0; i != p_ShiftsOnVariable[State].Length; i++)
            {
                if (p_ShiftsOnVariable[State][i][0] == SID)
                    return p_ShiftsOnVariable[State][i][1];
            }
            return 0xFFFF;
        }
        private static Production Analyse_GetProductionOnTerminal(ushort State, ushort SID)
        {
            for (int i = 0; i != p_ReducsOnTerminal[State].Length; i++)
            {
                if (p_ReducsOnTerminal[State][i][0] == SID)
                    return p_Rules[p_ReducsOnTerminal[State][i][1]];
            }
            return null;
        }

        private void DisplayState()
        {
            System.Console.Clear();
            System.Console.WriteLine("Expected :");
            System.Console.Write("{ ");
            for (int i = 0; i != p_ExpectedNames[p_CurrentState].Length; i++)
            {
                if (i != 0) System.Console.Write(", ");
                System.Console.Write(p_ExpectedNames[p_CurrentState][i]);
            }
            System.Console.WriteLine(" }");
            System.Console.WriteLine();
            System.Console.WriteLine("Items :");
            for (int i = 0; i != p_Items[p_CurrentState].Length; i++)
                System.Console.WriteLine(p_Items[p_CurrentState][i]);
        }


        public Parser_Hime_Kernel_FileCentralDogma(Lexer_Hime_Kernel_FileCentralDogma Input)
        {
            p_Errors = new System.Collections.Generic.List<Hime.Kernel.Parsers.IParserError>();
            p_Lexer = Input;
            p_Nodes = new Hime.Kernel.Parsers.SyntaxTreeNodeCollection();
            p_Stack = new System.Collections.Generic.Stack<ushort>();
            p_CurrentState = 0x0;
            p_NextToken = null;
        }


        private void Analyse_HandleUnexpectedToken()
        {
            p_Errors.Add(new Hime.Kernel.Parsers.ParserErrorUnexpectedToken(p_NextToken, p_ExpectedNames[p_CurrentState]));

            if (p_Errors.Count >= 100)
                throw new Hime.Kernel.Parsers.ParserException("Too much errors, parsing stopped.");

            for (int i = p_Nodes.Count - 1; i != -1; i--)
            {
                if (p_Nodes[i].Symbol is Hime.Kernel.Parsers.SymbolVariable)
                    break;
                p_Nodes.RemoveAt(i);
                p_Stack.Pop();
            }
            if (p_Nodes.Count == 0)
                throw new Hime.Kernel.Parsers.ParserException("Unrecoverable error encountered");

            p_CurrentState = p_Stack.Peek();
            p_NextToken = p_Lexer.GetNextToken();
            if (p_NextToken == null)
                throw new Hime.Kernel.Parsers.ParserException("Unrecoverable error encountered");
        }

        public Hime.Kernel.Parsers.SyntaxTreeNode Analyse()
        {
            p_Stack.Push(p_CurrentState);
            p_NextToken = p_Lexer.GetNextToken();

            while (true)
            {
                ushort NextState = Analyse_GetNextByShiftOnTerminal(p_CurrentState, p_NextToken.SymbolID);
                if (NextState != 0xFFFF)
                {
                    p_Nodes.Add(new Hime.Kernel.Parsers.SyntaxTreeNode(p_NextToken));
                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    p_NextToken = p_Lexer.GetNextToken();
                    continue;
                }
                Production Reduce = Analyse_GetProductionOnTerminal(p_CurrentState, p_NextToken.SymbolID);
                if (Reduce != null)
                {
                    ushort VarSID = Reduce(p_Nodes, p_Stack);
                    if (p_NextToken.SymbolID == 0x1)
                        return p_Nodes[0];
                    NextState = Analyse_GetNextByShiftOnVariable(p_Stack.Peek(), VarSID);

                    // Handle error here : no transition for symbol VarSID
                    if (NextState == 0xFFFF)
                        throw new Hime.Kernel.Parsers.ParserException("No shift for variable");

                    p_CurrentState = NextState;
                    p_Stack.Push(p_CurrentState);
                    continue;
                }
                // Handle error here : no action for symbol p_NextToken.SymbolID
                Analyse_HandleUnexpectedToken();
            }
        }

    }


}
