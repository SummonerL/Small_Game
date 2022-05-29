INCLUDE GlobalVariables.ink


VAR callback = -> pyramid_scheme_email

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
VAR mom_calling_first = 0
~ mom_calling_first = checkMemory("mom_calling_first")

$. # ----------first Continue() consumes this line--

{
	- (not exclusive_missed_call_mom_first && mom_calling_first):
	    -> exclusive_missed_call_mom_first
}

-> callback
-> DONE

/**
*   Here are all of the events that are 'exclusive', or conditional
**/
=== exclusive_missed_call_mom_first ===
Whoever it is can call back later.

$. #audiostop:phone

Besides, this is the exact thing I should be doing with a splitting headache.

~ removeMemory("mom_calling_first")

-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== pyramid_scheme_email ===
$. #animation:ChairSit

An email from Chloe... Goodman?

I remember her from High School. We were never close though...

"Hey Lucas! I know this probably seems weird, but I remember you having super curly hair in high school."

"By any chance, have you been looking for a new moisturizing shampoo?"

$. #animation:ChairStand

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END