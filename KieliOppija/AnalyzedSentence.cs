using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KieliOppija
{
    class AnalyzedSentence
    {
        string content;
        bool isMain = true;

        public AnalyzedSentence(string content, bool isMain)
        {
            this.Content = content;
            this.IsMain = isMain;
        }

        public string Content { get => content; set => content = value; }
        public bool IsMain { get => isMain; set => isMain = value; }

        public override string ToString()
        {
            return Content+" ("+(isMain?"pää":"sivu")+")";
        }
    }
}
