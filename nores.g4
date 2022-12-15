grammar nores;


prog: statement*  
    ;

statement
    :   assign_stmt 
    |   keyword_stmt 
    ;

assign_stmt
    :   reference EQUALS expression NEWLINE
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
    :   expression (TIMES | DIVIDE) expression
    |   expression (PLUS | MINUS) expression
    |   INT
    |   LPAR expression RPAR
    ;

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
    ;

keyword_stmt
    :   call_stmt 
    |   goto_stmt
    |   procedure_stmt
    ;

call_stmt
    :   CALL reference '(' ')' NEWLINE
    ;

goto_stmt
    :   GOTO IDENTIFIER NEWLINE
    ;

procedure_stmt
    :   (PROCEDURE | PROC) identifier ('(' ')')? '{' prog '}' NEWLINE
    ;

WS:         (' ')+ -> skip ;
NEWLINE:    [\r\n]+ ;
INT:        [0-9]+ ; 
CALL:       'call' ; 
GOTO:       'goto' ;
PROCEDURE:  'procedure' ;
PROC:       'proc' ;
IDENTIFIER: [a-zA-Z]+ ;
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

