grammar nores;


prog: statement*  
    ;

statement
    :   assign_stmt 
    |   keyword_stmt 
    |   SEMICOLON
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
    ;

keyword_stmt
    :   call_stmt 
    |   goto_stmt
    |   procedure_stmt
    |   end_stmt
    ;

call_stmt
    :   CALL reference '(' ')' SEMICOLON
    ;

goto_stmt
    :   GOTO IDENTIFIER SEMICOLON
    ;

end_stmt
    :   END SEMICOLON
    ;

procedure_stmt
    :   (PROCEDURE | PROC) identifier ('(' ')')?  prog end_stmt
    ;

WS:         (' ')+ -> skip ;
NEWLINE:    [\r\n]+ -> skip ;
TAB:        ('\t')+ -> skip ;
INT:        [0-9]+ ; 
CALL:       'call' ; 
GOTO:       'goto' ;
PROCEDURE:  'procedure' ;
PROC:       'proc' ;
END:        'end' ;

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
