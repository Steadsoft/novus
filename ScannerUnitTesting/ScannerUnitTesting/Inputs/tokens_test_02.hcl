im_a_name;
im_another ;
so_am_i ; 


// Make "heap" a type

dcl packet_heap area(4096);

dcl p pointer;

allocate p in packet_heap;



/*
proc aaa
{

   FAc6 8AB7:H;
   FA06_8aB7 : H;
   Fa06W8AB7:H;

   fa56 77a3_33ba:H;

   C234__B556:H;

   1234.56;
   123 456 976.001;
   123.456:D;
   a = 123:D;

   var = I4_345_678;
}

procedure vbbb
{
   call initialize (100 123.45 123,456.88 Counter (5 12 MN) ); // ( spacelist )
   data = array (8) (4) (17) ; // possibility for subscripting arrays by distinct parenthesized subscripts, obligatory for arrays...
}

abc = 2 ;

LOOP
{
     a = 2 45 987.6 
}

loop until (a > 100)
{
   a = 123,567.8;
}

call operation("test");

loop I = 1 to 100 by 8 until (failure)
{
   r = get_it(I);
}

function get_it(X)
{
	arg X integer;
}

func get_it(X)
{
	arg X float(dec(32)) 
	dcl Y float(bin(256))
	dcl Z fixed(dec(16))
	dcl substring builtin;
	dcl counter fixed(bin(32 4)) unsigned;
	dcl tabs bit(16);

	counter = 11011.1101:b;
	tabs = "011010011";

	/*

	binary 16, 32, 64, 128 aka half, single, double, quad
	decimal 32, 64, 128

	*/

}

proc main
{

	// left_offset = 0;
	// left_frame = XXXX
	// right_offset = 0;
	// right_frame = YYYY
	// curr_frame = left_frame;
	// goto left_offset

	call left (X); // must create a collective stack frame that all coroutines will (in essence) share.

}

proc left(L) coroutine
{

	arg L;

	// dcl locals

	// do something

	yield to right(Z); // actually a goto right_offset into right and set left_offset to 1 and curr_frame to right_frame

	// do something else

	yield to right(Z); // actually a goto right_offset into right and set left_offset to 2 and curr_frame to right_frame

}

proc right(R) coroutine
{

	arg R;

	// dcl locals

	// do something

	yield to left(W); // actually a goto left_offset into left and set right_offset to 1 and curr_frame to left_frame

	// do something else

	yield to left(W); // actually a goto left_offset into left and set right_offset to 2 and curr_frame to left_frame

}

// Above, we can also consider the term "yield" or "yield to" 

func other(R) fixed(bin(16)) coroutine
{

	arg R;

	// dcl locals

	// do something

	a = yield to left(W); // actually a goto left_offset into left and set right_offset to 1 and curr_frame to left_frame

	// do something else

	yield (23) to left(W); // actually a goto left_offset into left and set right_offset to 2 and curr_frame to left_frame

}

procedure loop_worker coroutine
{
   while (true)
   {
      // do stuff
	  yield to loop_admin;
	  // do more stuff
   }
}

procedure loop_admin coroutine
{
   while (true)
   {
      // do stuff
	  yield to loop_worker;
	  // do more stuff
   }
}
*/