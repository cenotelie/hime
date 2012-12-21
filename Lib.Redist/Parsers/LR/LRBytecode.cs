namespace Hime.Redist.Parsers
{
    public static class LRBytecode
    {
        public const ushort PopNoAction = 0;
        public const ushort PopDrop = 2;
        public const ushort PopPromote = 3;
        public const ushort Virtual = 4;
        public const ushort VirtualNoAction = Virtual + PopNoAction;
        public const ushort VirtualDrop = Virtual + PopDrop;
        public const ushort VirtualPromote = Virtual + PopPromote;
        public const ushort SemanticAction = 8;
        public const ushort NullVariable = 16;
        public const ushort NullVariableNoAction = NullVariable;
        public const ushort NullVariableDrop = NullVariable + PopDrop;
        public const ushort NullVariablePromote = NullVariable + PopPromote;

        public static bool IsPop(ushort code) { return (code <= PopPromote); }
        public static bool IsAddVirtual(ushort code) { return ((code & Virtual) == Virtual); }
        public static bool IsSemAction(ushort code) { return (code == SemanticAction); }
        public static bool IsAddNullVariable(ushort code) { return ((code & NullVariable) == NullVariable); }
        public static AST.CSTAction GetAction(ushort code) { return (AST.CSTAction)(code & 0x3); }
    }
}
