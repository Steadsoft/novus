/*
    This grammar is based on the PL/I grammar (in particular the grammar defined in ANSI X3.74-1987)
    It has no reserved words, any keyword may be used as an identifier without any ambiguit arising.
    It suppors multiple kewyord languages.
    We can add more keywords over time and never break backward compatibility with existing code
    which migt contain identifiers that are the same as these new keywords.
*/

grammar nores;

@lexer::members {public String langcode = "en";}

prog: statement*  
    ;

statement
    :   preprocessor_stmt
    |   assign_stmt 
    |   keyword_stmt 
    |   SEMICOLON
    ;

preprocessor_stmt
    :   '%' 'include' identifier SEMICOLON
    ;

assign_stmt
    :   reference EQUALS expression SEMICOLON
    ;

reference
    :   reference ARROW basic_reference arguments_list?
    |   basic_reference arguments_list?
    ;

arguments
    :   LPAR subscript_commalist? RPAR
    ;

arguments_list
    :   arguments+
    ;

basic_reference
    :   structure_qualification_list? identifier 
    ;

structure_qualification
    :   identifier arguments? DOT
    ;

structure_qualification_list
    :   structure_qualification+
    ;

subscript
    :   expression 
    ;

subscript_commalist
    :   subscript (COMMA subscript)*
    ;

expression
    :   expression_9 | expression '|:' expression_9
    ;

expression_9
    :   expression_8 | expression_9 '&:' expression_8
    ;

expression_8
    :   expression_7 | expression_8 ('|' | '~') expression_7
    ;

expression_7
    :   expression_6 | expression_7 '&' expression_6
    ;

expression_6
    :   expression_5 | expression_6 comparison_operator expression_5
    ;

expression_5
    :   expression_4 | expression_5 '||' expression_4
    ;

expression_4
    :   expression_3 | expression_4 (PLUS | MINUS) expression_3
    ;

expression_3
    :   expression_2 | expression_3 (TIMES | DIVIDE) expression_2
    ;

expression_2
    :   primitive_expression | prefix_expression | parenthesized_expression | expression_1
    ;

expression_1
    :   (primitive_expression | parenthesized_expression) POWER expression_2
    ;

prefix_expression
    :   prefix_operator expression_2
    ;

parenthesized_expression
    :   '(' expression ')'
    ;

primitive_expression
    :   reference
    |   constant
    // | enquiry
    ;

constant
    :   INT
    ;

prefix_operator
    :   '+'
    |   '-'
    |   '~'
    ;

comparison_operator
    :   '>'
    |   '>='
    |   '='
    |   '<'
    |   '<='
    |   '~>'
    |   '~='
    |   '~<'
    ;


/*
expression
    :   expression (TIMES | DIVIDE) expression
    |   expression (PLUS | MINUS) expression
    |   INT
    |   LPAR expression RPAR
    ;
*/

identifier
    :   keyword
    |   IDENTIFIER
    ;

/***********************************/
/* Add new keywords here as needed */
/***********************************/

keyword
    : CALL
    | GOTO
    | PROCEDURE
    | PROC
    | END
    | DECLARE
    | RETURN
    | IF
    | THEN
    | ELSE
    | GO
    | TO
    ;

keyword_stmt
    :   call_stmt 
    |   goto_stmt
    |   procedure_stmt
    // |   end_stmt
    |   declare_stmt
    |   return_stmt
    |   if_stmt
    ;

call_stmt
    :   CALL reference SEMICOLON
    ;

goto_stmt
    :   (GOTO | GO TO) reference SEMICOLON
    ;

end_stmt
    :   END SEMICOLON
    ;

declare_stmt
    :   DECLARE identifier attribute* SEMICOLON 
    ;

attribute
    :   (data_attribute | AUTOMATIC | BUILTIN | STATIC | VARIABLE | based | defined) 
    ;

data_attribute
    :   (BINARY | DECIMAL)
    ;

based
    :   BASED ('(' reference ')')?
    ;

defined
    :   DEFINED ('(' reference ')')?
    ;

procedure_stmt
    :   PROCEDURE identifier ('(' ')')?  prog end_stmt 
    ;

return_stmt
    :   RETURN ('(' expression ')')? SEMICOLON
    ;

if_stmt
    :   then_clause (assign_stmt | keyword_stmt)+ else_clause? end_stmt 
    ;

then_clause
    :   IF expression THEN
    ;

else_clause
    :   ELSE (assign_stmt | keyword_stmt)+
    ;

COMMENT:    '/*' .*? '*/' -> channel(2) ;
WS:         (' ')+ -> skip ;
NEWLINE:    [\r\n]+ -> skip ;
TAB:        ('\t')+ -> skip ;
INT:        [0-9]+ ; 


GOTO:       
    {langcode=="en"}? 'goto' |
    {langcode=="fr"}? 'aller' 
    ;
GO:             
    {langcode=="en"}? 'go' |
    {langcode=="fr"}? 'aller' 
    ;
TO:             
    {langcode=="en"}? 'to' |
    {langcode=="fr"}? 'à' 
    ;



CALL:           ('call') ; 
//GOTO:           ('goto') ;
//GO:             ('go');
//TO:             ('to');
PROCEDURE:      ('procedure' | 'proc') ;
PROC:           'proc' ;
END:            'end' ;
DECLARE:        ('declare' | 'dcl') ;
BINARY:         ('binary' | 'bin') ;
DECIMAL:        ('decimal' | 'dec') ;
AUTOMATIC:      ('automatic' | 'auto') ;
BUILTIN:        ('builtin');
STATIC:         ('static');
VARIABLE:       ('variable');
BASED:          ('based');
DEFINED:        ('defined');
INTERNAL:       ('internal');
EXTERNAL:       ('external');
RETURN:         ('return');
IF:             ('if');
THEN:           ('then');
ELSE:           ('else');

IDENTIFIER: [a-zA-Z_]+ ;
ARROW:      '->' ;
DOT:        '.' ;
COMMA:      ',' ;
LPAR:       '(' ;
RPAR:       ')' ;
EQUALS:     '=' ;
TIMES:      '*' ;
DIVIDE:     '/' ;
PLUS:       '+' ;
MINUS:      '-' ;
SEMICOLON:  ';' ;
POWER:      '**' ;
