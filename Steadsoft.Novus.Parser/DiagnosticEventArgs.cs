namespace Steadsoft.Novus.Parser
{
    public class DiagnosticEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public DiagnosticEventArgs (Severity Severity, int Line, int Col, string Msg)
        {
            Message = $"{Severity} at {Line,2}, {Col,2} - {Msg}";
        }
    }
}
