using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ChangLab.Common
{
    public class XMLWebRequest
    {
        public XmlDocument Document { get; private set; }

        public bool Request(string Url)
        {
            HttpWebResponse res = null;
            string xml = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
                res = (HttpWebResponse)req.GetResponse();
                xml = string.Empty;

                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        xml += reader.ReadLine();

                        if (CancellationPending) { return false; }
                    }

                    try { res.Close(); res = null; }
                    finally { }
                }

                if (!string.IsNullOrWhiteSpace(xml))
                {
                    this.Document = new XmlDocument();
                    this.Document.LoadXml(xml);

                    return true;
                }
                else
                {
                    throw new ChangLabException("No data received for request.");
                }
            }
            catch (ChangLabException ex)
            {
                throw ex;
            }
            finally
            {
                if (res != null)
                {
                    try { res.Close(); }
                    finally { }
                }
            }
        }

        private bool CancellationPending { get; set; }
        public void CancelAsync() { this.CancellationPending = true; }

        protected virtual void OnRequestCompleted(RunWorkerCompletedEventArgs e) { if (RequestCompleted != null) { RequestCompleted(null, e); } }
        public event RunWorkerCompletedEventHandler RequestCompleted;

        #region Static/Sync
        public static XmlDocument RequestDocument(string Url)
        {
            HttpWebResponse res = null;
            string xml = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
                res = (HttpWebResponse)req.GetResponse();
                xml = string.Empty;

                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    xml = reader.ReadToEnd();

                    try { res.Close(); res = null; }
                    finally { }
                }

                if (!string.IsNullOrWhiteSpace(xml))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    return doc;
                }
                else
                {
                    throw new ChangLabException("No data received for request.");
                }
            }
            catch (ChangLabException ex)
            {
                throw ex;
            }
            finally
            {
                if (res != null)
                {
                    try { res.Close(); }
                    finally { }
                }
            }
        }
        #endregion
    }
}
