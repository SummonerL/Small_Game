INCLUDE GlobalVariables.ink


VAR callback = -> mom_first_call

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
=== mom_first_call ===
$. #animation:PhonePickUp

Oh, it's Mom.

$. #animation:PhoneToEar

Hello?

$. #actor:external

Lucas! I finally g... to... you!

$. #actor:lucas

I can't really hear you Mom.

$. #actor:external

W... ti... again?

\*Static\*

$. #actor:lucas

Still can't hear you.

$. #animation:PhoneEarToFace

$. #actor:external

Lucas?

$. #animation:PhoneToEar

$. #actor:lucas

Hey, I can hear you now.

$. #actor:external

Sorry! I was in an elevator. Just got to the office.

Can you remind me what your job title was again?

$. #actor:lucas

I'm a programmer. Why?

$. #actor:external

I thought you were an engineer? That's what I told Sunita.

$. #actor:lucas

I mean, I'm a software engineer I guess. But I haven't had any work in like a month.

$. #actor:external

Why not? What am I supposed to tell Sunita?

$. #actor:lucas

Why do you have to tell her anything?

$. #actor:external

Can't a mom brag about her kids on occasion?

$. #actor:lucas

Doesn't Sunita have like 3 doctor-kids? A freelance programmer isn't going to impress her.

$. #actor:external

Why are you being like this Lucas? You have a lot to be proud of.

$. #actor:lucas

Yeah...

I kind of have a headache Mom. Can we talk later?

$. #actor:external

Sure... Love you.

$. #actor:lucas

You too.

$. #animation:PhoneEarToFace

$. #dramaticpause:1

...fun stuff.

$. #animation:PhonePutDown

Well. I'm awake now.

~ callback = -> end_story
$. #endsession:true
-> DONE



=== end_story ===
-> END