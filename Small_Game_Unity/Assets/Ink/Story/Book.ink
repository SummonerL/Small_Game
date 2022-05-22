INCLUDE GlobalVariables.ink

-> book_heard_about_1

=== book_heard_about_1 ===
Oh yeah... I keep hearing so many great things about this book online. 

If I could just bring myself to read it.

~ addMemory("book_heard_about_1")

$. #endsession:true

-> book_really_heard_about_2

=== book_really_heard_about_2 ===
No, I mean I REALLY keep hearing about this

Variable check.

First is {checkMemory("book_heard_about_1")}

Second is {checkMemory("book_really_heard_about_2")}

~ addMemory("book_really_heard_about_2")

Third is {checkMemory("book_really_heard_about_2")}

-> END