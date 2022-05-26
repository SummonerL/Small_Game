INCLUDE GlobalVariables.ink


VAR callback = -> generic_bed_event

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
=== generic_bed_event ===
$. #animation:BedCollapse

$. #dramaticpause:3

I can't sleep...

Goddamn insomnia drives me crazy.

I swear to God, it's like I have no energy in the morning and yet I can't turn off my brain at night.

$. #fade:out

$. #animationControl:reset_state

$. #animation:BedSleepingStill

$. #advancetime:2

$. #fade:in

$. #dramaticpause:1

Wow... Did I actually manage to fall asleep?

What time is it...?

God, I have a splitting headache.

$. #animation:BedSleepingGetUp

~ callback = -> second_generic_bed_event
$. #endsession:true
->DONE



=== second_generic_bed_event ===

This is the second generic bed event

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END
