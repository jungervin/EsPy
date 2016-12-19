using System.Windows.Forms;

namespace EsPy.Forms
{
    public interface IDocument : IForm
    {
        string FileName
        { get; }

        bool CanSave
        { get; }

        bool Modified
        { get; }

        bool CanPaste
        { get; set; }

        string Source
        { get; }

        void LoadFromFile(string fanme);
        DialogResult Save();
        DialogResult SaveAs();

        void UpdateUI();       
    }
}
