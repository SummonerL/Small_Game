INCLUDE GlobalVariables.ink

VAR currentDay = 0
VAR currentTime = ""

-> beginning

=== beginning ===
// move to a given cutscene
~ currentDay = getDate()
~ currentTime = getTime()

{
	- currentDay == 11 && currentTime == "08:00":
	    -> eleventh_morning
	- else:
	    -> end_session
}




=== eleventh_morning ===

$. #animation:BedSleepingStill

$. #fade:in

$. #dramaticpause:2

...Oh God.

My head is killing me.

Who is calling me? It's 8:00 in the morning.

$. #animation:BedSleepingGetUp

-> end_session




=== end_session ===
$. #endsession:true
-> beginning

-> END 