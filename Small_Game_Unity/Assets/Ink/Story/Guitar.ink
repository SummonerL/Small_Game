INCLUDE GlobalVariables.ink


VAR callback = -> generic_guitar_event

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
VAR book_1_memory = 0
~ book_1_memory = checkMemory("book_heard_about_1")

$. # ----------first Continue() consumes this line--


{
	- (not exclusive_saw_book && book_1_memory):
	    -> exclusive_saw_book
}
-> callback
-> DONE


=== exclusive_saw_book ====
The book said to play guitar.
-> callback
-> DONE


/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== generic_guitar_event ===
I haven't played in so long...

I wonder if I can even remember any of my songs?

~ callback = -> generic_guitar_event2
$. #endsession:true
-> DONE


=== generic_guitar_event2 ===
This is a test. Can I see this?

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END