// Generated from java-escape by ANTLR 4.11.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class noresLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.11.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, WS=14, NEWLINE=15, TAB=16, INT=17, 
		CALL=18, GOTO=19, PROCEDURE=20, PROC=21, END=22, IDENTIFIER=23, ARROW=24, 
		DOT=25, COMMA=26, LPAR=27, RPAR=28, EQUALS=29, TIMES=30, DIVIDE=31, PLUS=32, 
		MINUS=33, SEMICOLON=34, POWER=35;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
			"T__9", "T__10", "T__11", "T__12", "WS", "NEWLINE", "TAB", "INT", "CALL", 
			"GOTO", "PROCEDURE", "PROC", "END", "IDENTIFIER", "ARROW", "DOT", "COMMA", 
			"LPAR", "RPAR", "EQUALS", "TIMES", "DIVIDE", "PLUS", "MINUS", "SEMICOLON", 
			"POWER"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'|:'", "'&:'", "'|'", "'~'", "'&'", "'||'", "'>'", "'>='", "'<'", 
			"'<='", "'~>'", "'~='", "'~<'", null, null, null, null, "'call'", "'goto'", 
			"'procedure'", "'proc'", "'end'", null, "'->'", "'.'", "','", "'('", 
			"')'", "'='", "'*'", "'/'", "'+'", "'-'", "';'", "'**'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, "WS", "NEWLINE", "TAB", "INT", "CALL", "GOTO", "PROCEDURE", 
			"PROC", "END", "IDENTIFIER", "ARROW", "DOT", "COMMA", "LPAR", "RPAR", 
			"EQUALS", "TIMES", "DIVIDE", "PLUS", "MINUS", "SEMICOLON", "POWER"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public noresLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "nores.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000#\u00bf\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002\u0001"+
		"\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004"+
		"\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007"+
		"\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b"+
		"\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002"+
		"\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002"+
		"\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002"+
		"\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002"+
		"\u0018\u0007\u0018\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002"+
		"\u001b\u0007\u001b\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002"+
		"\u001e\u0007\u001e\u0002\u001f\u0007\u001f\u0002 \u0007 \u0002!\u0007"+
		"!\u0002\"\u0007\"\u0001\u0000\u0001\u0000\u0001\u0000\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003\u0001"+
		"\u0004\u0001\u0004\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0006\u0001"+
		"\u0006\u0001\u0007\u0001\u0007\u0001\u0007\u0001\b\u0001\b\u0001\t\u0001"+
		"\t\u0001\t\u0001\n\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0001\u000b"+
		"\u0001\f\u0001\f\u0001\f\u0001\r\u0004\rk\b\r\u000b\r\f\rl\u0001\r\u0001"+
		"\r\u0001\u000e\u0004\u000er\b\u000e\u000b\u000e\f\u000es\u0001\u000e\u0001"+
		"\u000e\u0001\u000f\u0004\u000fy\b\u000f\u000b\u000f\f\u000fz\u0001\u000f"+
		"\u0001\u000f\u0001\u0010\u0004\u0010\u0080\b\u0010\u000b\u0010\f\u0010"+
		"\u0081\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0011\u0001"+
		"\u0012\u0001\u0012\u0001\u0012\u0001\u0012\u0001\u0012\u0001\u0013\u0001"+
		"\u0013\u0001\u0013\u0001\u0013\u0001\u0013\u0001\u0013\u0001\u0013\u0001"+
		"\u0013\u0001\u0013\u0001\u0013\u0001\u0014\u0001\u0014\u0001\u0014\u0001"+
		"\u0014\u0001\u0014\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0016\u0004\u0016\u00a2\b\u0016\u000b\u0016\f\u0016\u00a3\u0001\u0017"+
		"\u0001\u0017\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0019\u0001\u0019"+
		"\u0001\u001a\u0001\u001a\u0001\u001b\u0001\u001b\u0001\u001c\u0001\u001c"+
		"\u0001\u001d\u0001\u001d\u0001\u001e\u0001\u001e\u0001\u001f\u0001\u001f"+
		"\u0001 \u0001 \u0001!\u0001!\u0001\"\u0001\"\u0001\"\u0000\u0000#\u0001"+
		"\u0001\u0003\u0002\u0005\u0003\u0007\u0004\t\u0005\u000b\u0006\r\u0007"+
		"\u000f\b\u0011\t\u0013\n\u0015\u000b\u0017\f\u0019\r\u001b\u000e\u001d"+
		"\u000f\u001f\u0010!\u0011#\u0012%\u0013\'\u0014)\u0015+\u0016-\u0017/"+
		"\u00181\u00193\u001a5\u001b7\u001c9\u001d;\u001e=\u001f? A!C\"E#\u0001"+
		"\u0000\u0003\u0002\u0000\n\n\r\r\u0001\u000009\u0003\u0000AZ__az\u00c3"+
		"\u0000\u0001\u0001\u0000\u0000\u0000\u0000\u0003\u0001\u0000\u0000\u0000"+
		"\u0000\u0005\u0001\u0000\u0000\u0000\u0000\u0007\u0001\u0000\u0000\u0000"+
		"\u0000\t\u0001\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000\u0000\u0000"+
		"\r\u0001\u0000\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000\u0000\u0011"+
		"\u0001\u0000\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000\u0000\u0015"+
		"\u0001\u0000\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000\u0000\u0019"+
		"\u0001\u0000\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000\u0000\u001d"+
		"\u0001\u0000\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000\u0000!\u0001"+
		"\u0000\u0000\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%\u0001\u0000\u0000"+
		"\u0000\u0000\'\u0001\u0000\u0000\u0000\u0000)\u0001\u0000\u0000\u0000"+
		"\u0000+\u0001\u0000\u0000\u0000\u0000-\u0001\u0000\u0000\u0000\u0000/"+
		"\u0001\u0000\u0000\u0000\u00001\u0001\u0000\u0000\u0000\u00003\u0001\u0000"+
		"\u0000\u0000\u00005\u0001\u0000\u0000\u0000\u00007\u0001\u0000\u0000\u0000"+
		"\u00009\u0001\u0000\u0000\u0000\u0000;\u0001\u0000\u0000\u0000\u0000="+
		"\u0001\u0000\u0000\u0000\u0000?\u0001\u0000\u0000\u0000\u0000A\u0001\u0000"+
		"\u0000\u0000\u0000C\u0001\u0000\u0000\u0000\u0000E\u0001\u0000\u0000\u0000"+
		"\u0001G\u0001\u0000\u0000\u0000\u0003J\u0001\u0000\u0000\u0000\u0005M"+
		"\u0001\u0000\u0000\u0000\u0007O\u0001\u0000\u0000\u0000\tQ\u0001\u0000"+
		"\u0000\u0000\u000bS\u0001\u0000\u0000\u0000\rV\u0001\u0000\u0000\u0000"+
		"\u000fX\u0001\u0000\u0000\u0000\u0011[\u0001\u0000\u0000\u0000\u0013]"+
		"\u0001\u0000\u0000\u0000\u0015`\u0001\u0000\u0000\u0000\u0017c\u0001\u0000"+
		"\u0000\u0000\u0019f\u0001\u0000\u0000\u0000\u001bj\u0001\u0000\u0000\u0000"+
		"\u001dq\u0001\u0000\u0000\u0000\u001fx\u0001\u0000\u0000\u0000!\u007f"+
		"\u0001\u0000\u0000\u0000#\u0083\u0001\u0000\u0000\u0000%\u0088\u0001\u0000"+
		"\u0000\u0000\'\u008d\u0001\u0000\u0000\u0000)\u0097\u0001\u0000\u0000"+
		"\u0000+\u009c\u0001\u0000\u0000\u0000-\u00a1\u0001\u0000\u0000\u0000/"+
		"\u00a5\u0001\u0000\u0000\u00001\u00a8\u0001\u0000\u0000\u00003\u00aa\u0001"+
		"\u0000\u0000\u00005\u00ac\u0001\u0000\u0000\u00007\u00ae\u0001\u0000\u0000"+
		"\u00009\u00b0\u0001\u0000\u0000\u0000;\u00b2\u0001\u0000\u0000\u0000="+
		"\u00b4\u0001\u0000\u0000\u0000?\u00b6\u0001\u0000\u0000\u0000A\u00b8\u0001"+
		"\u0000\u0000\u0000C\u00ba\u0001\u0000\u0000\u0000E\u00bc\u0001\u0000\u0000"+
		"\u0000GH\u0005|\u0000\u0000HI\u0005:\u0000\u0000I\u0002\u0001\u0000\u0000"+
		"\u0000JK\u0005&\u0000\u0000KL\u0005:\u0000\u0000L\u0004\u0001\u0000\u0000"+
		"\u0000MN\u0005|\u0000\u0000N\u0006\u0001\u0000\u0000\u0000OP\u0005~\u0000"+
		"\u0000P\b\u0001\u0000\u0000\u0000QR\u0005&\u0000\u0000R\n\u0001\u0000"+
		"\u0000\u0000ST\u0005|\u0000\u0000TU\u0005|\u0000\u0000U\f\u0001\u0000"+
		"\u0000\u0000VW\u0005>\u0000\u0000W\u000e\u0001\u0000\u0000\u0000XY\u0005"+
		">\u0000\u0000YZ\u0005=\u0000\u0000Z\u0010\u0001\u0000\u0000\u0000[\\\u0005"+
		"<\u0000\u0000\\\u0012\u0001\u0000\u0000\u0000]^\u0005<\u0000\u0000^_\u0005"+
		"=\u0000\u0000_\u0014\u0001\u0000\u0000\u0000`a\u0005~\u0000\u0000ab\u0005"+
		">\u0000\u0000b\u0016\u0001\u0000\u0000\u0000cd\u0005~\u0000\u0000de\u0005"+
		"=\u0000\u0000e\u0018\u0001\u0000\u0000\u0000fg\u0005~\u0000\u0000gh\u0005"+
		"<\u0000\u0000h\u001a\u0001\u0000\u0000\u0000ik\u0005 \u0000\u0000ji\u0001"+
		"\u0000\u0000\u0000kl\u0001\u0000\u0000\u0000lj\u0001\u0000\u0000\u0000"+
		"lm\u0001\u0000\u0000\u0000mn\u0001\u0000\u0000\u0000no\u0006\r\u0000\u0000"+
		"o\u001c\u0001\u0000\u0000\u0000pr\u0007\u0000\u0000\u0000qp\u0001\u0000"+
		"\u0000\u0000rs\u0001\u0000\u0000\u0000sq\u0001\u0000\u0000\u0000st\u0001"+
		"\u0000\u0000\u0000tu\u0001\u0000\u0000\u0000uv\u0006\u000e\u0000\u0000"+
		"v\u001e\u0001\u0000\u0000\u0000wy\u0005\t\u0000\u0000xw\u0001\u0000\u0000"+
		"\u0000yz\u0001\u0000\u0000\u0000zx\u0001\u0000\u0000\u0000z{\u0001\u0000"+
		"\u0000\u0000{|\u0001\u0000\u0000\u0000|}\u0006\u000f\u0000\u0000} \u0001"+
		"\u0000\u0000\u0000~\u0080\u0007\u0001\u0000\u0000\u007f~\u0001\u0000\u0000"+
		"\u0000\u0080\u0081\u0001\u0000\u0000\u0000\u0081\u007f\u0001\u0000\u0000"+
		"\u0000\u0081\u0082\u0001\u0000\u0000\u0000\u0082\"\u0001\u0000\u0000\u0000"+
		"\u0083\u0084\u0005c\u0000\u0000\u0084\u0085\u0005a\u0000\u0000\u0085\u0086"+
		"\u0005l\u0000\u0000\u0086\u0087\u0005l\u0000\u0000\u0087$\u0001\u0000"+
		"\u0000\u0000\u0088\u0089\u0005g\u0000\u0000\u0089\u008a\u0005o\u0000\u0000"+
		"\u008a\u008b\u0005t\u0000\u0000\u008b\u008c\u0005o\u0000\u0000\u008c&"+
		"\u0001\u0000\u0000\u0000\u008d\u008e\u0005p\u0000\u0000\u008e\u008f\u0005"+
		"r\u0000\u0000\u008f\u0090\u0005o\u0000\u0000\u0090\u0091\u0005c\u0000"+
		"\u0000\u0091\u0092\u0005e\u0000\u0000\u0092\u0093\u0005d\u0000\u0000\u0093"+
		"\u0094\u0005u\u0000\u0000\u0094\u0095\u0005r\u0000\u0000\u0095\u0096\u0005"+
		"e\u0000\u0000\u0096(\u0001\u0000\u0000\u0000\u0097\u0098\u0005p\u0000"+
		"\u0000\u0098\u0099\u0005r\u0000\u0000\u0099\u009a\u0005o\u0000\u0000\u009a"+
		"\u009b\u0005c\u0000\u0000\u009b*\u0001\u0000\u0000\u0000\u009c\u009d\u0005"+
		"e\u0000\u0000\u009d\u009e\u0005n\u0000\u0000\u009e\u009f\u0005d\u0000"+
		"\u0000\u009f,\u0001\u0000\u0000\u0000\u00a0\u00a2\u0007\u0002\u0000\u0000"+
		"\u00a1\u00a0\u0001\u0000\u0000\u0000\u00a2\u00a3\u0001\u0000\u0000\u0000"+
		"\u00a3\u00a1\u0001\u0000\u0000\u0000\u00a3\u00a4\u0001\u0000\u0000\u0000"+
		"\u00a4.\u0001\u0000\u0000\u0000\u00a5\u00a6\u0005-\u0000\u0000\u00a6\u00a7"+
		"\u0005>\u0000\u0000\u00a70\u0001\u0000\u0000\u0000\u00a8\u00a9\u0005."+
		"\u0000\u0000\u00a92\u0001\u0000\u0000\u0000\u00aa\u00ab\u0005,\u0000\u0000"+
		"\u00ab4\u0001\u0000\u0000\u0000\u00ac\u00ad\u0005(\u0000\u0000\u00ad6"+
		"\u0001\u0000\u0000\u0000\u00ae\u00af\u0005)\u0000\u0000\u00af8\u0001\u0000"+
		"\u0000\u0000\u00b0\u00b1\u0005=\u0000\u0000\u00b1:\u0001\u0000\u0000\u0000"+
		"\u00b2\u00b3\u0005*\u0000\u0000\u00b3<\u0001\u0000\u0000\u0000\u00b4\u00b5"+
		"\u0005/\u0000\u0000\u00b5>\u0001\u0000\u0000\u0000\u00b6\u00b7\u0005+"+
		"\u0000\u0000\u00b7@\u0001\u0000\u0000\u0000\u00b8\u00b9\u0005-\u0000\u0000"+
		"\u00b9B\u0001\u0000\u0000\u0000\u00ba\u00bb\u0005;\u0000\u0000\u00bbD"+
		"\u0001\u0000\u0000\u0000\u00bc\u00bd\u0005*\u0000\u0000\u00bd\u00be\u0005"+
		"*\u0000\u0000\u00beF\u0001\u0000\u0000\u0000\u0006\u0000lsz\u0081\u00a3"+
		"\u0001\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}