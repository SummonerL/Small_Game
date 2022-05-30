INCLUDE GlobalVariables.ink

# unique methods bound to this story
EXTERNAL setCutscene(cutscene)

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

	- (not twelfth_evening) && currentDay == 12 && currentTime == "21:00":
	    ~ setCutscene("twelfth_evening")
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

$. #audiostart:phone

$. #dramaticpause:3

Who is calling me? It's 8:00 in the morning.

~ addMemory("mom_calling_first")

$. #animation:BedSleepingGetUp

-> end_session
->DONE



=== twelfth_evening ===
$. #actor:lucas

$. #dramaticpause:2


Well guys... that's the end of the demo!

I hope you're interested in seeing more.

Elliot and Fatima put a lot of hard work into this, trust me.

If you don't mind, please take a second to fill out the short survey.

If you didn't get the link, tell Elliot that Lucas told him to do a better job.

Anyway, it's getting late. I'm heading to bed.

See you all soon!

$. #fade:out

$. #dramaticpause:600

-> end_session
->DONE



=== end_session ===
$. #endsession:true

# we'll never get to this END
-> END 