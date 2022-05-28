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

$. # ----------first Continue() consumes this line--

-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== first_event ===

$. #animation:GameConsolePickUp

Wait a second... wasn't the remastered edition of Detective Hirata coming out today? 

I should download it to see if Yume no Tabi can successfully ruin my childhood.

$. #fade:out

$. #animation:GameConsoleSit

$. #advancetime:1

$. #fade:in

What the hell? It's just finished downloading? 

Since when is a visual novel 20GB?

Regardless, my internet is absolute garbage. I'm going to have to upgrade soon.

I'm not even sure if I want to play anymore. Maybe later.

$. #fade:out

$. #animation:GameConsoleStand

$. #fade:in

$. #animation:GameConsolePutDown

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END