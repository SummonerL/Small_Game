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
VAR mom_calling_first = 0
~ mom_calling_first = checkMemory("mom_calling_first")
~ currentDay = getDate()
~ currentTime = getTime()

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

$. #animation:BedCollapse

I'm way too tired.

~ removeMemory("mom_calling_first")
~ addMemory("mom_missed_first")

$. #dramaticpause:2

It was probably Mom anyway.

She's always manages to call as soon as I hit REM.

I wonder if I'll start waking up at 4AM when I'm that age.

$. #dramaticpause:2

$. #fade:out

$. #animationControl:reset_state

$. #animation:BedSleepingStill

$. #advancetime:1

$. #dramaticpause:2

$. #fade:in

$. #dramaticpause:2

I should probably get up.

At least my headache is gone.

$. #animation:BedSleepingGetUp

\*sigh\* Should I call Mom back?

$. #endsession:true
-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== generic_bed_event ===
$. #animation:BedCollapse

$. #dramaticpause:4

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

~ callback = -> end_story
$. #endsession:true
->DONE



=== end_story ===
I'm not tired at all.
-> END
