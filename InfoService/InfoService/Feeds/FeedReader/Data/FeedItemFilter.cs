using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedReader.Data
{
    public class FeedItemFilter
    {
        public FeedItemFilter(bool IsRegEx, string ReplaceThis, string ReplaceWith, bool UseInTitle, bool UseInBody, bool CleanBefore)
        {
            this.IsRegEx = IsRegEx;
            this.ReplaceThis = ReplaceThis;
            this.ReplaceWith = ReplaceWith;
            this.UseInTitle = UseInTitle;
            this.UseInBody = UseInBody;
            this.CleanBefore = CleanBefore;

        }
        public bool IsRegEx { get; set; }
        public string ReplaceThis { get; set; }
        public string ReplaceWith { get; set; }
        public bool UseInTitle { get; set; }
        public bool UseInBody { get; set; }
        public bool CleanBefore { get; set; }
    }
}
