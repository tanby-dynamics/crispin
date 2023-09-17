﻿using System.IO;
using System.Text;
using System.Xml;

namespace crispin
{
    // http://www.hanselman.com/blog/XmlFragmentWriterOmitingTheXmlDeclarationAndTheXSDAndXSINamespaces.aspx
    public class XmlFragmentWriter : XmlTextWriter
    {
        private bool _skip = false;

        public XmlFragmentWriter(TextWriter w) : base(w)
        {
        }

        public XmlFragmentWriter(Stream w, Encoding encoding) : base(w, encoding)
        {
        }

        public XmlFragmentWriter(string filename, Encoding encoding) :
            base(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None), encoding)
        {
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            // STEP 1 - Omits XSD and XSI declarations.
            // From Kzu - http://weblogs.asp.net/cazzu/archive/2004/01/23/62141.aspx
            if (prefix == "xmlns" && (localName == "xsd" || localName == "xsi"))
            {
                _skip = true;
                return;
            }
            base.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            if (_skip) return;
            base.WriteString(text);
        }

        public override void WriteEndAttribute()
        {
            if (_skip)
            {
                // Reset the flag, so we keep writing.
                _skip = false;
                return;
            }
            base.WriteEndAttribute();
        }

        public override void WriteStartDocument()
        {
            // STEP 2: Do nothing so we omit the xml declaration.
        }
    }
}