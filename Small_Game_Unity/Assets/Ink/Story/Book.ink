INCLUDE GlobalVariables.ink


VAR callback = -> book_heard_about_1

/***
*   The exclusive_events knot is switched to at the beginning of every
*   story session. The first line is a buffer for the necessary .Continue()
*   statement. The callback will be called after any exclusive events.
*/
=== exclusive_events ===
#---------------------------------------------------
# keep external checks in the below section. Unforuntately,
# 'canContinue' can't actually evaluate external methods, and will
# always return true. As a result, we can't safely check canContinue until after
# these have been evaluated.

$. # ----------first Continue() consumes this line--

-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== book_heard_about_1 ===
Oh yeah... I keep hearing so many great things about this book online. 

If I could just bring myself to read it.

~ addMemory("book_heard_about_1")

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END