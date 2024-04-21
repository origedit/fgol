60 constant width
24 constant height
width height * constant size

1000 20 / constant delay

require random.fs

bl constant dead
char # constant alive

create board1   size chars allot
create board2   size chars allot

board1 value oldb
board2 value newb

: swapb   oldb newb to oldb to newb ;

: tileoffset   swap width * + chars ;
: tile@ ( y x -- c )
 width mod swap height mod swap
 tileoffset oldb + c@ ;
: tile! ( y x c -- )
 >r tileoffset newb + r> swap c! ;

create xdirs
-1 , 0 , 1 , -1 , 1 , -1 , 0 , 1 ,
create ydirs
-1 , -1 , -1 , 0 , 0 , 1 , 1 , 1 ,

: xdir   cells xdirs + @ ;
: ydir   cells ydirs + @ ; 

: neighbors ( y x -- n )
 0 -rot 8 0 do
  over i ydir + over i xdir + tile@ alive = if >r >r 1+ r> r> then
 loop 2drop ;

: newstate ( neighbors state -- state )
 alive = if
  dup 2 = swap 3 = or
 else
  3 = 
 then
 alive dead rot select ;

: updtile ( y x -- )
 2dup tile@ >r 2dup neighbors r> newstate tile! ;

: update
 height 0 do width 0 do
  j i updtile
 loop loop ;

: draw
 page
 newb height 0 do
  dup width type cr
  width chars +
 loop drop ;

utime drop seed !

: randomise
 oldb dup size chars + swap do
  alive dead rnd 1 and select i c!
 loop ;

: key ( -- quit? )
 key? if
  key dup [char] q = swap
  [char] r = if randomise then
 else false then ;

: gol
 randomise
 begin
  update draw swapb
  key if exit then
  delay ms
 again ;

gol bye
