namespace Hime.Parsers
{
    [System.FlagsAttribute]
    public enum TerminalBinType : byte
    {
        OTHER = 0x00,
        SYMBOL_VALUE_UINT128 = 0x10,
        SYMBOL_JOKER_UINT128 = 0x22,
        SYMBOL_VALUE_UINT64 = 0x30,
        SYMBOL_JOKER_UINT64 = 0x42,
        SYMBOL_VALUE_UINT32 = 0x50,
        SYMBOL_JOKER_UINT32 = 0x62,
        SYMBOL_VALUE_UINT16 = 0x70,
        SYMBOL_JOKER_UINT16 = 0x82,
        SYMBOL_VALUE_UINT8 = 0x90,
        SYMBOL_JOKER_UINT8 = 0xA2,
        SYMBOL_VALUE_BINARY = 0xB1,
        SYMBOL_JOKER_BINARY = 0xC3,
        FLAG_BINARY = 0x01,
        FLAG_JOKER = 0x02
    }
}