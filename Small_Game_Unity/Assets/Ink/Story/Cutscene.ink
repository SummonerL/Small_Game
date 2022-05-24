INCLUDE GlobalVariables.ink

# unique methods bound to this story
EXTERNAL setCutscene(cutscene)

VAR currentDay = 0
VAR currentTime = ""

/***
*   The cutscene check is accessed through a separate 'flow'.
*   this will set the active cutscene knot (in the main flow), 
*   as well as indicate that the Cutscene state should be transitioned to.
*/
=== cutscene_check ===
// move to a given cutscene
~ currentDay = getDate()
~ currentTime = getTime()

{
	- (not eleventh_morning) && currentDay == 11 && currentTime == "08:00":
	    ~ setCutscene("eleventh_morning")
	    ->DONE
	- else:
	    ->DONE
}
->DONE


=== eleventh_morning ===

$. #animation:BedSleepingStill

$. #fade:in

$. #dramaticpause:3

...Oh God.

My head is killing me.

Who is calling me? It's 8:00 in the morning.

$. #animation:BedSleepingGetUp

-> end_session
->DONE




=== end_session ===
$. #endsession:true

# we'll never get to this END
-> END 