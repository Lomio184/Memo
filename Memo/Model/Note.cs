using System;

namespace Memo.Model
{
    public class Note
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string FileName { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(FileName) ? "New Note" : FileName;
        }
    }
}
