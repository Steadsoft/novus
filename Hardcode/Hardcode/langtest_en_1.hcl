﻿
def message struct order(auto) aligned
    sequence bin(31)
    tag      byte(2)

    // Note all members that are padding/ignored must have same name 

    XXXX     byte (5) padding
    XXXX     byte (3) ignored // ignore the 3 byte checksum
end



#env lingua(fr) 

function test binary(31)

   declare a string

   while true

      a = a + 1
   
   end
   goto someplace

   go to another

   
   loop I = 1 to 100

      call reset_device(I)

   end

end
