namespace Hime.Kernel.Reporting
{
    public interface IEntry
    {
        ELevel Level { get; }
        string Component { get; }
        string Message { get; }

        System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc);
    }
}