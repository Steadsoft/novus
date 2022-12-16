counter = 100;

/* declare some variables named the same as keywords */

dcl dcl dec based(counter);  /* dcl is a keyword ! */

dcl counter binary;

declare goto binary; /* goto is a keyword ! */



goto call; /* call is a keyword ! */



call declare(); /* declare is a keyword */

call declare (1,2,3);

return (123);

if if = then then
   dcl realm binary;
   call reboot(0);
   go to end;
end;


