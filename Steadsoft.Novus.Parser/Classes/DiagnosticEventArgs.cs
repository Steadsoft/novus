using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Classes
{
    public class DiagnosticEventArgs : EventArgs
    {
        public Severity Severity { get; private set; }
        public int Line { get; private set; }
        public int Col { get; private set; }
        public string Message { get; private set; }

        public DiagnosticEventArgs(Severity Severity, int Line, int Col, string Msg)
        {
            this.Severity = Severity;
            this.Line = Line;
            this.Col = Col;
            this.Message = $"{Severity} at {Line,2}, {Col,2} - {Msg}";
        }
    }
}
