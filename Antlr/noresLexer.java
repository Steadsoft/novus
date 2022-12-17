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
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, COMMENT=16, 
		WS=17, NEWLINE=18, TAB=19, INT=20, GOTO=21, GO=22, TO=23, CALL=24, PROCEDURE=25, 
		PROC=26, END=27, DECLARE=28, BINARY=29, DECIMAL=30, AUTOMATIC=31, BUILTIN=32, 
		STATIC=33, VARIABLE=34, BASED=35, DEFINED=36, INTERNAL=37, EXTERNAL=38, 
		RETURN=39, IF=40, THEN=41, ELSE=42, IDENTIFIER=43, ARROW=44, DOT=45, COMMA=46, 
		LPAR=47, RPAR=48, EQUALS=49, TIMES=50, DIVIDE=51, PLUS=52, MINUS=53, SEMICOLON=54, 
		POWER=55;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
			"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "COMMENT", "WS", 
			"NEWLINE", "TAB", "INT", "GOTO", "GO", "TO", "CALL", "PROCEDURE", "PROC", 
			"END", "DECLARE", "BINARY", "DECIMAL", "AUTOMATIC", "BUILTIN", "STATIC", 
			"VARIABLE", "BASED", "DEFINED", "INTERNAL", "EXTERNAL", "RETURN", "IF", 
			"THEN", "ELSE", "IDENTIFIER", "ARROW", "DOT", "COMMA", "LPAR", "RPAR", 
			"EQUALS", "TIMES", "DIVIDE", "PLUS", "MINUS", "SEMICOLON", "POWER"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'%'", "'include'", "'|:'", "'&:'", "'|'", "'~'", "'&'", "'||'", 
			"'>'", "'>='", "'<'", "'<='", "'~>'", "'~='", "'~<'", null, null, null, 
			null, null, null, null, null, null, null, "'proc'", "'end'", null, null, 
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, "'->'", "'.'", "','", "'('", "')'", "'='", "'*'", "'/'", 
			"'+'", "'-'", "';'", "'**'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, "COMMENT", "WS", "NEWLINE", "TAB", "INT", "GOTO", 
			"GO", "TO", "CALL", "PROCEDURE", "PROC", "END", "DECLARE", "BINARY", 
			"DECIMAL", "AUTOMATIC", "BUILTIN", "STATIC", "VARIABLE", "BASED", "DEFINED", 
			"INTERNAL", "EXTERNAL", "RETURN", "IF", "THEN", "ELSE", "IDENTIFIER", 
			"ARROW", "DOT", "COMMA", "LPAR", "RPAR", "EQUALS", "TIMES", "DIVIDE", 
			"PLUS", "MINUS", "SEMICOLON", "POWER"
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

	public String langcode = "en";

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

	@Override
	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 20:
			return GOTO_sempred((RuleContext)_localctx, predIndex);
		case 21:
			return GO_sempred((RuleContext)_localctx, predIndex);
		case 22:
			return TO_sempred((RuleContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean GOTO_sempred(RuleContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return langcode=="en";
		case 1:
			return langcode=="fr";
		}
		return true;
	}
	private boolean GO_sempred(RuleContext _localctx, int predIndex) {
		switch (predIndex) {
		case 2:
			return langcode=="en";
		case 3:
			return langcode=="fr";
		}
		return true;
	}
	private boolean TO_sempred(RuleContext _localctx, int predIndex) {
		switch (predIndex) {
		case 4:
			return langcode=="en";
		case 5:
			return langcode=="fr";
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u00007\u019c\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002\u0001"+
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
		"!\u0002\"\u0007\"\u0002#\u0007#\u0002$\u0007$\u0002%\u0007%\u0002&\u0007"+
		"&\u0002\'\u0007\'\u0002(\u0007(\u0002)\u0007)\u0002*\u0007*\u0002+\u0007"+
		"+\u0002,\u0007,\u0002-\u0007-\u0002.\u0007.\u0002/\u0007/\u00020\u0007"+
		"0\u00021\u00071\u00022\u00072\u00023\u00073\u00024\u00074\u00025\u0007"+
		"5\u00026\u00076\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0002\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001"+
		"\u0004\u0001\u0004\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006\u0001"+
		"\u0007\u0001\u0007\u0001\u0007\u0001\b\u0001\b\u0001\t\u0001\t\u0001\t"+
		"\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001"+
		"\f\u0001\r\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000e\u0001\u000f"+
		"\u0001\u000f\u0001\u000f\u0001\u000f\u0005\u000f\u00a0\b\u000f\n\u000f"+
		"\f\u000f\u00a3\t\u000f\u0001\u000f\u0001\u000f\u0001\u000f\u0001\u000f"+
		"\u0001\u000f\u0001\u0010\u0004\u0010\u00ab\b\u0010\u000b\u0010\f\u0010"+
		"\u00ac\u0001\u0010\u0001\u0010\u0001\u0011\u0004\u0011\u00b2\b\u0011\u000b"+
		"\u0011\f\u0011\u00b3\u0001\u0011\u0001\u0011\u0001\u0012\u0004\u0012\u00b9"+
		"\b\u0012\u000b\u0012\f\u0012\u00ba\u0001\u0012\u0001\u0012\u0001\u0013"+
		"\u0004\u0013\u00c0\b\u0013\u000b\u0013\f\u0013\u00c1\u0001\u0014\u0001"+
		"\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001"+
		"\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0003\u0014\u00cf\b\u0014\u0001"+
		"\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0001\u0015\u0001\u0015\u0003\u0015\u00da\b\u0015\u0001\u0016\u0001"+
		"\u0016\u0001\u0016\u0001\u0016\u0001\u0016\u0003\u0016\u00e1\b\u0016\u0001"+
		"\u0017\u0001\u0017\u0001\u0017\u0001\u0017\u0001\u0017\u0001\u0018\u0001"+
		"\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0001"+
		"\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0001\u0018\u0003"+
		"\u0018\u00f5\b\u0018\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019\u0001"+
		"\u0019\u0001\u001a\u0001\u001a\u0001\u001a\u0001\u001a\u0001\u001b\u0001"+
		"\u001b\u0001\u001b\u0001\u001b\u0001\u001b\u0001\u001b\u0001\u001b\u0001"+
		"\u001b\u0001\u001b\u0001\u001b\u0003\u001b\u010a\b\u001b\u0001\u001c\u0001"+
		"\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001\u001c\u0001"+
		"\u001c\u0001\u001c\u0003\u001c\u0115\b\u001c\u0001\u001d\u0001\u001d\u0001"+
		"\u001d\u0001\u001d\u0001\u001d\u0001\u001d\u0001\u001d\u0001\u001d\u0001"+
		"\u001d\u0001\u001d\u0003\u001d\u0121\b\u001d\u0001\u001e\u0001\u001e\u0001"+
		"\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0001"+
		"\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0003\u001e\u0130"+
		"\b\u001e\u0001\u001f\u0001\u001f\u0001\u001f\u0001\u001f\u0001\u001f\u0001"+
		"\u001f\u0001\u001f\u0001\u001f\u0001 \u0001 \u0001 \u0001 \u0001 \u0001"+
		" \u0001 \u0001!\u0001!\u0001!\u0001!\u0001!\u0001!\u0001!\u0001!\u0001"+
		"!\u0001\"\u0001\"\u0001\"\u0001\"\u0001\"\u0001\"\u0001#\u0001#\u0001"+
		"#\u0001#\u0001#\u0001#\u0001#\u0001#\u0001$\u0001$\u0001$\u0001$\u0001"+
		"$\u0001$\u0001$\u0001$\u0001$\u0001%\u0001%\u0001%\u0001%\u0001%\u0001"+
		"%\u0001%\u0001%\u0001%\u0001&\u0001&\u0001&\u0001&\u0001&\u0001&\u0001"+
		"&\u0001\'\u0001\'\u0001\'\u0001(\u0001(\u0001(\u0001(\u0001(\u0001)\u0001"+
		")\u0001)\u0001)\u0001)\u0001*\u0004*\u017f\b*\u000b*\f*\u0180\u0001+\u0001"+
		"+\u0001+\u0001,\u0001,\u0001-\u0001-\u0001.\u0001.\u0001/\u0001/\u0001"+
		"0\u00010\u00011\u00011\u00012\u00012\u00013\u00013\u00014\u00014\u0001"+
		"5\u00015\u00016\u00016\u00016\u0001\u00a1\u00007\u0001\u0001\u0003\u0002"+
		"\u0005\u0003\u0007\u0004\t\u0005\u000b\u0006\r\u0007\u000f\b\u0011\t\u0013"+
		"\n\u0015\u000b\u0017\f\u0019\r\u001b\u000e\u001d\u000f\u001f\u0010!\u0011"+
		"#\u0012%\u0013\'\u0014)\u0015+\u0016-\u0017/\u00181\u00193\u001a5\u001b"+
		"7\u001c9\u001d;\u001e=\u001f? A!C\"E#G$I%K&M\'O(Q)S*U+W,Y-[.]/_0a1c2e"+
		"3g4i5k6m7\u0001\u0000\u0003\u0002\u0000\n\n\r\r\u0001\u000009\u0003\u0000"+
		"AZ__az\u01a9\u0000\u0001\u0001\u0000\u0000\u0000\u0000\u0003\u0001\u0000"+
		"\u0000\u0000\u0000\u0005\u0001\u0000\u0000\u0000\u0000\u0007\u0001\u0000"+
		"\u0000\u0000\u0000\t\u0001\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000"+
		"\u0000\u0000\r\u0001\u0000\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000"+
		"\u0000\u0011\u0001\u0000\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000"+
		"\u0000\u0015\u0001\u0000\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000"+
		"\u0000\u0019\u0001\u0000\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000"+
		"\u0000\u001d\u0001\u0000\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000"+
		"\u0000!\u0001\u0000\u0000\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%"+
		"\u0001\u0000\u0000\u0000\u0000\'\u0001\u0000\u0000\u0000\u0000)\u0001"+
		"\u0000\u0000\u0000\u0000+\u0001\u0000\u0000\u0000\u0000-\u0001\u0000\u0000"+
		"\u0000\u0000/\u0001\u0000\u0000\u0000\u00001\u0001\u0000\u0000\u0000\u0000"+
		"3\u0001\u0000\u0000\u0000\u00005\u0001\u0000\u0000\u0000\u00007\u0001"+
		"\u0000\u0000\u0000\u00009\u0001\u0000\u0000\u0000\u0000;\u0001\u0000\u0000"+
		"\u0000\u0000=\u0001\u0000\u0000\u0000\u0000?\u0001\u0000\u0000\u0000\u0000"+
		"A\u0001\u0000\u0000\u0000\u0000C\u0001\u0000\u0000\u0000\u0000E\u0001"+
		"\u0000\u0000\u0000\u0000G\u0001\u0000\u0000\u0000\u0000I\u0001\u0000\u0000"+
		"\u0000\u0000K\u0001\u0000\u0000\u0000\u0000M\u0001\u0000\u0000\u0000\u0000"+
		"O\u0001\u0000\u0000\u0000\u0000Q\u0001\u0000\u0000\u0000\u0000S\u0001"+
		"\u0000\u0000\u0000\u0000U\u0001\u0000\u0000\u0000\u0000W\u0001\u0000\u0000"+
		"\u0000\u0000Y\u0001\u0000\u0000\u0000\u0000[\u0001\u0000\u0000\u0000\u0000"+
		"]\u0001\u0000\u0000\u0000\u0000_\u0001\u0000\u0000\u0000\u0000a\u0001"+
		"\u0000\u0000\u0000\u0000c\u0001\u0000\u0000\u0000\u0000e\u0001\u0000\u0000"+
		"\u0000\u0000g\u0001\u0000\u0000\u0000\u0000i\u0001\u0000\u0000\u0000\u0000"+
		"k\u0001\u0000\u0000\u0000\u0000m\u0001\u0000\u0000\u0000\u0001o\u0001"+
		"\u0000\u0000\u0000\u0003q\u0001\u0000\u0000\u0000\u0005y\u0001\u0000\u0000"+
		"\u0000\u0007|\u0001\u0000\u0000\u0000\t\u007f\u0001\u0000\u0000\u0000"+
		"\u000b\u0081\u0001\u0000\u0000\u0000\r\u0083\u0001\u0000\u0000\u0000\u000f"+
		"\u0085\u0001\u0000\u0000\u0000\u0011\u0088\u0001\u0000\u0000\u0000\u0013"+
		"\u008a\u0001\u0000\u0000\u0000\u0015\u008d\u0001\u0000\u0000\u0000\u0017"+
		"\u008f\u0001\u0000\u0000\u0000\u0019\u0092\u0001\u0000\u0000\u0000\u001b"+
		"\u0095\u0001\u0000\u0000\u0000\u001d\u0098\u0001\u0000\u0000\u0000\u001f"+
		"\u009b\u0001\u0000\u0000\u0000!\u00aa\u0001\u0000\u0000\u0000#\u00b1\u0001"+
		"\u0000\u0000\u0000%\u00b8\u0001\u0000\u0000\u0000\'\u00bf\u0001\u0000"+
		"\u0000\u0000)\u00ce\u0001\u0000\u0000\u0000+\u00d9\u0001\u0000\u0000\u0000"+
		"-\u00e0\u0001\u0000\u0000\u0000/\u00e2\u0001\u0000\u0000\u00001\u00f4"+
		"\u0001\u0000\u0000\u00003\u00f6\u0001\u0000\u0000\u00005\u00fb\u0001\u0000"+
		"\u0000\u00007\u0109\u0001\u0000\u0000\u00009\u0114\u0001\u0000\u0000\u0000"+
		";\u0120\u0001\u0000\u0000\u0000=\u012f\u0001\u0000\u0000\u0000?\u0131"+
		"\u0001\u0000\u0000\u0000A\u0139\u0001\u0000\u0000\u0000C\u0140\u0001\u0000"+
		"\u0000\u0000E\u0149\u0001\u0000\u0000\u0000G\u014f\u0001\u0000\u0000\u0000"+
		"I\u0157\u0001\u0000\u0000\u0000K\u0160\u0001\u0000\u0000\u0000M\u0169"+
		"\u0001\u0000\u0000\u0000O\u0170\u0001\u0000\u0000\u0000Q\u0173\u0001\u0000"+
		"\u0000\u0000S\u0178\u0001\u0000\u0000\u0000U\u017e\u0001\u0000\u0000\u0000"+
		"W\u0182\u0001\u0000\u0000\u0000Y\u0185\u0001\u0000\u0000\u0000[\u0187"+
		"\u0001\u0000\u0000\u0000]\u0189\u0001\u0000\u0000\u0000_\u018b\u0001\u0000"+
		"\u0000\u0000a\u018d\u0001\u0000\u0000\u0000c\u018f\u0001\u0000\u0000\u0000"+
		"e\u0191\u0001\u0000\u0000\u0000g\u0193\u0001\u0000\u0000\u0000i\u0195"+
		"\u0001\u0000\u0000\u0000k\u0197\u0001\u0000\u0000\u0000m\u0199\u0001\u0000"+
		"\u0000\u0000op\u0005%\u0000\u0000p\u0002\u0001\u0000\u0000\u0000qr\u0005"+
		"i\u0000\u0000rs\u0005n\u0000\u0000st\u0005c\u0000\u0000tu\u0005l\u0000"+
		"\u0000uv\u0005u\u0000\u0000vw\u0005d\u0000\u0000wx\u0005e\u0000\u0000"+
		"x\u0004\u0001\u0000\u0000\u0000yz\u0005|\u0000\u0000z{\u0005:\u0000\u0000"+
		"{\u0006\u0001\u0000\u0000\u0000|}\u0005&\u0000\u0000}~\u0005:\u0000\u0000"+
		"~\b\u0001\u0000\u0000\u0000\u007f\u0080\u0005|\u0000\u0000\u0080\n\u0001"+
		"\u0000\u0000\u0000\u0081\u0082\u0005~\u0000\u0000\u0082\f\u0001\u0000"+
		"\u0000\u0000\u0083\u0084\u0005&\u0000\u0000\u0084\u000e\u0001\u0000\u0000"+
		"\u0000\u0085\u0086\u0005|\u0000\u0000\u0086\u0087\u0005|\u0000\u0000\u0087"+
		"\u0010\u0001\u0000\u0000\u0000\u0088\u0089\u0005>\u0000\u0000\u0089\u0012"+
		"\u0001\u0000\u0000\u0000\u008a\u008b\u0005>\u0000\u0000\u008b\u008c\u0005"+
		"=\u0000\u0000\u008c\u0014\u0001\u0000\u0000\u0000\u008d\u008e\u0005<\u0000"+
		"\u0000\u008e\u0016\u0001\u0000\u0000\u0000\u008f\u0090\u0005<\u0000\u0000"+
		"\u0090\u0091\u0005=\u0000\u0000\u0091\u0018\u0001\u0000\u0000\u0000\u0092"+
		"\u0093\u0005~\u0000\u0000\u0093\u0094\u0005>\u0000\u0000\u0094\u001a\u0001"+
		"\u0000\u0000\u0000\u0095\u0096\u0005~\u0000\u0000\u0096\u0097\u0005=\u0000"+
		"\u0000\u0097\u001c\u0001\u0000\u0000\u0000\u0098\u0099\u0005~\u0000\u0000"+
		"\u0099\u009a\u0005<\u0000\u0000\u009a\u001e\u0001\u0000\u0000\u0000\u009b"+
		"\u009c\u0005/\u0000\u0000\u009c\u009d\u0005*\u0000\u0000\u009d\u00a1\u0001"+
		"\u0000\u0000\u0000\u009e\u00a0\t\u0000\u0000\u0000\u009f\u009e\u0001\u0000"+
		"\u0000\u0000\u00a0\u00a3\u0001\u0000\u0000\u0000\u00a1\u00a2\u0001\u0000"+
		"\u0000\u0000\u00a1\u009f\u0001\u0000\u0000\u0000\u00a2\u00a4\u0001\u0000"+
		"\u0000\u0000\u00a3\u00a1\u0001\u0000\u0000\u0000\u00a4\u00a5\u0005*\u0000"+
		"\u0000\u00a5\u00a6\u0005/\u0000\u0000\u00a6\u00a7\u0001\u0000\u0000\u0000"+
		"\u00a7\u00a8\u0006\u000f\u0000\u0000\u00a8 \u0001\u0000\u0000\u0000\u00a9"+
		"\u00ab\u0005 \u0000\u0000\u00aa\u00a9\u0001\u0000\u0000\u0000\u00ab\u00ac"+
		"\u0001\u0000\u0000\u0000\u00ac\u00aa\u0001\u0000\u0000\u0000\u00ac\u00ad"+
		"\u0001\u0000\u0000\u0000\u00ad\u00ae\u0001\u0000\u0000\u0000\u00ae\u00af"+
		"\u0006\u0010\u0001\u0000\u00af\"\u0001\u0000\u0000\u0000\u00b0\u00b2\u0007"+
		"\u0000\u0000\u0000\u00b1\u00b0\u0001\u0000\u0000\u0000\u00b2\u00b3\u0001"+
		"\u0000\u0000\u0000\u00b3\u00b1\u0001\u0000\u0000\u0000\u00b3\u00b4\u0001"+
		"\u0000\u0000\u0000\u00b4\u00b5\u0001\u0000\u0000\u0000\u00b5\u00b6\u0006"+
		"\u0011\u0001\u0000\u00b6$\u0001\u0000\u0000\u0000\u00b7\u00b9\u0005\t"+
		"\u0000\u0000\u00b8\u00b7\u0001\u0000\u0000\u0000\u00b9\u00ba\u0001\u0000"+
		"\u0000\u0000\u00ba\u00b8\u0001\u0000\u0000\u0000\u00ba\u00bb\u0001\u0000"+
		"\u0000\u0000\u00bb\u00bc\u0001\u0000\u0000\u0000\u00bc\u00bd\u0006\u0012"+
		"\u0001\u0000\u00bd&\u0001\u0000\u0000\u0000\u00be\u00c0\u0007\u0001\u0000"+
		"\u0000\u00bf\u00be\u0001\u0000\u0000\u0000\u00c0\u00c1\u0001\u0000\u0000"+
		"\u0000\u00c1\u00bf\u0001\u0000\u0000\u0000\u00c1\u00c2\u0001\u0000\u0000"+
		"\u0000\u00c2(\u0001\u0000\u0000\u0000\u00c3\u00c4\u0004\u0014\u0000\u0000"+
		"\u00c4\u00c5\u0005g\u0000\u0000\u00c5\u00c6\u0005o\u0000\u0000\u00c6\u00c7"+
		"\u0005t\u0000\u0000\u00c7\u00cf\u0005o\u0000\u0000\u00c8\u00c9\u0004\u0014"+
		"\u0001\u0000\u00c9\u00ca\u0005a\u0000\u0000\u00ca\u00cb\u0005l\u0000\u0000"+
		"\u00cb\u00cc\u0005l\u0000\u0000\u00cc\u00cd\u0005e\u0000\u0000\u00cd\u00cf"+
		"\u0005r\u0000\u0000\u00ce\u00c3\u0001\u0000\u0000\u0000\u00ce\u00c8\u0001"+
		"\u0000\u0000\u0000\u00cf*\u0001\u0000\u0000\u0000\u00d0\u00d1\u0004\u0015"+
		"\u0002\u0000\u00d1\u00d2\u0005g\u0000\u0000\u00d2\u00da\u0005o\u0000\u0000"+
		"\u00d3\u00d4\u0004\u0015\u0003\u0000\u00d4\u00d5\u0005a\u0000\u0000\u00d5"+
		"\u00d6\u0005l\u0000\u0000\u00d6\u00d7\u0005l\u0000\u0000\u00d7\u00d8\u0005"+
		"e\u0000\u0000\u00d8\u00da\u0005r\u0000\u0000\u00d9\u00d0\u0001\u0000\u0000"+
		"\u0000\u00d9\u00d3\u0001\u0000\u0000\u0000\u00da,\u0001\u0000\u0000\u0000"+
		"\u00db\u00dc\u0004\u0016\u0004\u0000\u00dc\u00dd\u0005t\u0000\u0000\u00dd"+
		"\u00e1\u0005o\u0000\u0000\u00de\u00df\u0004\u0016\u0005\u0000\u00df\u00e1"+
		"\u0005\u8000\ufffd\u0000\u0000\u00e0\u00db\u0001\u0000\u0000\u0000\u00e0"+
		"\u00de\u0001\u0000\u0000\u0000\u00e1.\u0001\u0000\u0000\u0000\u00e2\u00e3"+
		"\u0005c\u0000\u0000\u00e3\u00e4\u0005a\u0000\u0000\u00e4\u00e5\u0005l"+
		"\u0000\u0000\u00e5\u00e6\u0005l\u0000\u0000\u00e60\u0001\u0000\u0000\u0000"+
		"\u00e7\u00e8\u0005p\u0000\u0000\u00e8\u00e9\u0005r\u0000\u0000\u00e9\u00ea"+
		"\u0005o\u0000\u0000\u00ea\u00eb\u0005c\u0000\u0000\u00eb\u00ec\u0005e"+
		"\u0000\u0000\u00ec\u00ed\u0005d\u0000\u0000\u00ed\u00ee\u0005u\u0000\u0000"+
		"\u00ee\u00ef\u0005r\u0000\u0000\u00ef\u00f5\u0005e\u0000\u0000\u00f0\u00f1"+
		"\u0005p\u0000\u0000\u00f1\u00f2\u0005r\u0000\u0000\u00f2\u00f3\u0005o"+
		"\u0000\u0000\u00f3\u00f5\u0005c\u0000\u0000\u00f4\u00e7\u0001\u0000\u0000"+
		"\u0000\u00f4\u00f0\u0001\u0000\u0000\u0000\u00f52\u0001\u0000\u0000\u0000"+
		"\u00f6\u00f7\u0005p\u0000\u0000\u00f7\u00f8\u0005r\u0000\u0000\u00f8\u00f9"+
		"\u0005o\u0000\u0000\u00f9\u00fa\u0005c\u0000\u0000\u00fa4\u0001\u0000"+
		"\u0000\u0000\u00fb\u00fc\u0005e\u0000\u0000\u00fc\u00fd\u0005n\u0000\u0000"+
		"\u00fd\u00fe\u0005d\u0000\u0000\u00fe6\u0001\u0000\u0000\u0000\u00ff\u0100"+
		"\u0005d\u0000\u0000\u0100\u0101\u0005e\u0000\u0000\u0101\u0102\u0005c"+
		"\u0000\u0000\u0102\u0103\u0005l\u0000\u0000\u0103\u0104\u0005a\u0000\u0000"+
		"\u0104\u0105\u0005r\u0000\u0000\u0105\u010a\u0005e\u0000\u0000\u0106\u0107"+
		"\u0005d\u0000\u0000\u0107\u0108\u0005c\u0000\u0000\u0108\u010a\u0005l"+
		"\u0000\u0000\u0109\u00ff\u0001\u0000\u0000\u0000\u0109\u0106\u0001\u0000"+
		"\u0000\u0000\u010a8\u0001\u0000\u0000\u0000\u010b\u010c\u0005b\u0000\u0000"+
		"\u010c\u010d\u0005i\u0000\u0000\u010d\u010e\u0005n\u0000\u0000\u010e\u010f"+
		"\u0005a\u0000\u0000\u010f\u0110\u0005r\u0000\u0000\u0110\u0115\u0005y"+
		"\u0000\u0000\u0111\u0112\u0005b\u0000\u0000\u0112\u0113\u0005i\u0000\u0000"+
		"\u0113\u0115\u0005n\u0000\u0000\u0114\u010b\u0001\u0000\u0000\u0000\u0114"+
		"\u0111\u0001\u0000\u0000\u0000\u0115:\u0001\u0000\u0000\u0000\u0116\u0117"+
		"\u0005d\u0000\u0000\u0117\u0118\u0005e\u0000\u0000\u0118\u0119\u0005c"+
		"\u0000\u0000\u0119\u011a\u0005i\u0000\u0000\u011a\u011b\u0005m\u0000\u0000"+
		"\u011b\u011c\u0005a\u0000\u0000\u011c\u0121\u0005l\u0000\u0000\u011d\u011e"+
		"\u0005d\u0000\u0000\u011e\u011f\u0005e\u0000\u0000\u011f\u0121\u0005c"+
		"\u0000\u0000\u0120\u0116\u0001\u0000\u0000\u0000\u0120\u011d\u0001\u0000"+
		"\u0000\u0000\u0121<\u0001\u0000\u0000\u0000\u0122\u0123\u0005a\u0000\u0000"+
		"\u0123\u0124\u0005u\u0000\u0000\u0124\u0125\u0005t\u0000\u0000\u0125\u0126"+
		"\u0005o\u0000\u0000\u0126\u0127\u0005m\u0000\u0000\u0127\u0128\u0005a"+
		"\u0000\u0000\u0128\u0129\u0005t\u0000\u0000\u0129\u012a\u0005i\u0000\u0000"+
		"\u012a\u0130\u0005c\u0000\u0000\u012b\u012c\u0005a\u0000\u0000\u012c\u012d"+
		"\u0005u\u0000\u0000\u012d\u012e\u0005t\u0000\u0000\u012e\u0130\u0005o"+
		"\u0000\u0000\u012f\u0122\u0001\u0000\u0000\u0000\u012f\u012b\u0001\u0000"+
		"\u0000\u0000\u0130>\u0001\u0000\u0000\u0000\u0131\u0132\u0005b\u0000\u0000"+
		"\u0132\u0133\u0005u\u0000\u0000\u0133\u0134\u0005i\u0000\u0000\u0134\u0135"+
		"\u0005l\u0000\u0000\u0135\u0136\u0005t\u0000\u0000\u0136\u0137\u0005i"+
		"\u0000\u0000\u0137\u0138\u0005n\u0000\u0000\u0138@\u0001\u0000\u0000\u0000"+
		"\u0139\u013a\u0005s\u0000\u0000\u013a\u013b\u0005t\u0000\u0000\u013b\u013c"+
		"\u0005a\u0000\u0000\u013c\u013d\u0005t\u0000\u0000\u013d\u013e\u0005i"+
		"\u0000\u0000\u013e\u013f\u0005c\u0000\u0000\u013fB\u0001\u0000\u0000\u0000"+
		"\u0140\u0141\u0005v\u0000\u0000\u0141\u0142\u0005a\u0000\u0000\u0142\u0143"+
		"\u0005r\u0000\u0000\u0143\u0144\u0005i\u0000\u0000\u0144\u0145\u0005a"+
		"\u0000\u0000\u0145\u0146\u0005b\u0000\u0000\u0146\u0147\u0005l\u0000\u0000"+
		"\u0147\u0148\u0005e\u0000\u0000\u0148D\u0001\u0000\u0000\u0000\u0149\u014a"+
		"\u0005b\u0000\u0000\u014a\u014b\u0005a\u0000\u0000\u014b\u014c\u0005s"+
		"\u0000\u0000\u014c\u014d\u0005e\u0000\u0000\u014d\u014e\u0005d\u0000\u0000"+
		"\u014eF\u0001\u0000\u0000\u0000\u014f\u0150\u0005d\u0000\u0000\u0150\u0151"+
		"\u0005e\u0000\u0000\u0151\u0152\u0005f\u0000\u0000\u0152\u0153\u0005i"+
		"\u0000\u0000\u0153\u0154\u0005n\u0000\u0000\u0154\u0155\u0005e\u0000\u0000"+
		"\u0155\u0156\u0005d\u0000\u0000\u0156H\u0001\u0000\u0000\u0000\u0157\u0158"+
		"\u0005i\u0000\u0000\u0158\u0159\u0005n\u0000\u0000\u0159\u015a\u0005t"+
		"\u0000\u0000\u015a\u015b\u0005e\u0000\u0000\u015b\u015c\u0005r\u0000\u0000"+
		"\u015c\u015d\u0005n\u0000\u0000\u015d\u015e\u0005a\u0000\u0000\u015e\u015f"+
		"\u0005l\u0000\u0000\u015fJ\u0001\u0000\u0000\u0000\u0160\u0161\u0005e"+
		"\u0000\u0000\u0161\u0162\u0005x\u0000\u0000\u0162\u0163\u0005t\u0000\u0000"+
		"\u0163\u0164\u0005e\u0000\u0000\u0164\u0165\u0005r\u0000\u0000\u0165\u0166"+
		"\u0005n\u0000\u0000\u0166\u0167\u0005a\u0000\u0000\u0167\u0168\u0005l"+
		"\u0000\u0000\u0168L\u0001\u0000\u0000\u0000\u0169\u016a\u0005r\u0000\u0000"+
		"\u016a\u016b\u0005e\u0000\u0000\u016b\u016c\u0005t\u0000\u0000\u016c\u016d"+
		"\u0005u\u0000\u0000\u016d\u016e\u0005r\u0000\u0000\u016e\u016f\u0005n"+
		"\u0000\u0000\u016fN\u0001\u0000\u0000\u0000\u0170\u0171\u0005i\u0000\u0000"+
		"\u0171\u0172\u0005f\u0000\u0000\u0172P\u0001\u0000\u0000\u0000\u0173\u0174"+
		"\u0005t\u0000\u0000\u0174\u0175\u0005h\u0000\u0000\u0175\u0176\u0005e"+
		"\u0000\u0000\u0176\u0177\u0005n\u0000\u0000\u0177R\u0001\u0000\u0000\u0000"+
		"\u0178\u0179\u0005e\u0000\u0000\u0179\u017a\u0005l\u0000\u0000\u017a\u017b"+
		"\u0005s\u0000\u0000\u017b\u017c\u0005e\u0000\u0000\u017cT\u0001\u0000"+
		"\u0000\u0000\u017d\u017f\u0007\u0002\u0000\u0000\u017e\u017d\u0001\u0000"+
		"\u0000\u0000\u017f\u0180\u0001\u0000\u0000\u0000\u0180\u017e\u0001\u0000"+
		"\u0000\u0000\u0180\u0181\u0001\u0000\u0000\u0000\u0181V\u0001\u0000\u0000"+
		"\u0000\u0182\u0183\u0005-\u0000\u0000\u0183\u0184\u0005>\u0000\u0000\u0184"+
		"X\u0001\u0000\u0000\u0000\u0185\u0186\u0005.\u0000\u0000\u0186Z\u0001"+
		"\u0000\u0000\u0000\u0187\u0188\u0005,\u0000\u0000\u0188\\\u0001\u0000"+
		"\u0000\u0000\u0189\u018a\u0005(\u0000\u0000\u018a^\u0001\u0000\u0000\u0000"+
		"\u018b\u018c\u0005)\u0000\u0000\u018c`\u0001\u0000\u0000\u0000\u018d\u018e"+
		"\u0005=\u0000\u0000\u018eb\u0001\u0000\u0000\u0000\u018f\u0190\u0005*"+
		"\u0000\u0000\u0190d\u0001\u0000\u0000\u0000\u0191\u0192\u0005/\u0000\u0000"+
		"\u0192f\u0001\u0000\u0000\u0000\u0193\u0194\u0005+\u0000\u0000\u0194h"+
		"\u0001\u0000\u0000\u0000\u0195\u0196\u0005-\u0000\u0000\u0196j\u0001\u0000"+
		"\u0000\u0000\u0197\u0198\u0005;\u0000\u0000\u0198l\u0001\u0000\u0000\u0000"+
		"\u0199\u019a\u0005*\u0000\u0000\u019a\u019b\u0005*\u0000\u0000\u019bn"+
		"\u0001\u0000\u0000\u0000\u000f\u0000\u00a1\u00ac\u00b3\u00ba\u00c1\u00ce"+
		"\u00d9\u00e0\u00f4\u0109\u0114\u0120\u012f\u0180\u0002\u0000\u0002\u0000"+
		"\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}