INCLUDE GlobalVariables.ink


VAR callback = -> first_event

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
Whoever it is can call back later. It's time for games.

$. #audiostop:phone

~ removeMemory("mom_calling_first")

$. #dramaticpause:1
-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== first_event ===

Wait a second... wasn't the remastered edition of Detective Hirata coming out today? 

$. #animation:GameConsolePickUp

I should download it to see if Yumetenko can successfully ruin my childhood.

$. #dramaticpause:1

God, I hope they didn't give Hirata a voice.

$. #fade:out

$. #animation:GameConsoleSit

$. #advancetime:1

$. #fade:in

...it just finished downloading? 

Since when is a visual novel 20GB? I hope the $60 was worth it.

Either way, my internet is horseshit. I'm going to have to upgrade soon.

$. #dramaticpause:2

I'm not even sure if I want to play anymore... Maybe later.

$. #fade:out

$. #animation:GameConsoleStand

$. #fade:in

$. #animation:GameConsolePutDown

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END