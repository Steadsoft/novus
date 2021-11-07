using System;

namespace Steadsoft.Novus.Parser
{
    public class DiagnosticEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public DiagnosticEventArgs (string Msg)
        {
            Message = Msg;
        }
    }
}
