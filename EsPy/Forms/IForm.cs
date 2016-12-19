using System.Windows.Forms;

namespace EsPy.Forms
{
    public interface IForm
    {
        ToolStrip ToolStrip
        { get; }

        ToolStrip MenuStrip
        { get; }

    }
}
