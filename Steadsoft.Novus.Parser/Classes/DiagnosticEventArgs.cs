using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Classes
{
    public class DiagnosticEventArgs : System.EventArgs
    {
        public Severity Severity { get; private set; }
        public int Line { get; private set; }
        public int Col { get; private set; }
        public string Message { get; private set; }

        public DiagnosticEventArgs(Severity Severity, int Line, int Col, string Msg)
        {
            var location = $"(at {Line}, {Col})";

            this.Severity = Severity;
            this.Line = Line;
            this.Col = Col;
            this.Message = $"{Severity, -8} {location, -12} {Msg}";
        }
    }
}
