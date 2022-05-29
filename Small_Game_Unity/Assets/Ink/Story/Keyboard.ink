INCLUDE GlobalVariables.ink


VAR callback = -> chloe_first_email

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

Besides, this is the exact thing I should be doing with a splitting headache.

~ removeMemory("mom_calling_first")
~ addMemory("mom_missed_first")

-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== chloe_first_email ===
$. #animation:ChairSit

$. #dramaticpause:3

An email from Chloe... Goodman?

I remember her from High School. We were never close though...

"Hey Lucas! I know this probably seems weird, but I remember you having super curly hair in high school."

"By any chance, have you been looking for a new moisturizing shampoo?"

"I recently started selling DivineShine products and let me tell you, they are the best!"

"If you're not really in need of shampoo, we've got all sorts of other products as well."

"Body wash, facial scrubs, you name it!"

"Also, please let me know if you know of any people who might be interested in buying or selling DivineShine."

"Give me a call at 614-555-0168. Thanks!"

"Heart sign. Chloe."

$. #dramaticpause:2

...poor girl. If she's resorted to sending me a sales pitch, things can't be going well.

Maybe I'll ask Sis if she's interested.

~ addMemory("chloe_email_pitch")

$. #fade:out

$. #dramaticpause:2

$. #advancetime:2

$. #fade:in

$. #dramaticpause:2

...bank account is getting low.

$400. Isn't rent due in like a week?

I should probably pick up some freelance again.

$. #animation:ChairStand

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END