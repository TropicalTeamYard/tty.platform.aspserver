using System.Xml.Linq;

namespace XCore
{
    /// <summary>
    /// 支持<see cref="object"/>的快速序列化和反序列化。
    /// </summary>
    public interface IXSerializable
    {
        void DeSerialize(XElement xElement);
        XElement Serialize();
    }
}
