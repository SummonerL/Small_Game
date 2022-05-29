INCLUDE GlobalVariables.ink


VAR callback = -> call_nathan_first

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
	- (not mom_first_call && mom_calling_first):
	    -> mom_first_call
}

-> callback
-> DONE

/**
*   Here are all of the events that are 'exclusive', or conditional
**/
=== mom_first_call ===
$. #animation:PhonePickUp

Oh, it's Mom

$. #audiostop:phone

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

~ removeMemory("mom_calling_first")

~ addMemory("talked_mom_first")

$. #endsession:true
-> callback
-> DONE


/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== call_nathan_first ===
$. #animation:PhonePickUp

I wonder what Nathan's up to?

$. #animation:PhoneToEar

$. #dramaticpause:3

$. #actor:external

{currentTime == "08:00": Dude, you realize it's 8AM, right?|Hey man, what's up?}

$. #actor:lucas

Hey, Nathan. Just thought I'd give you a call and see what you're up to.

$. #actor:external

{currentTime == "08:00":
    I'm sleeping.
    ...well, I was sleeping.
    $. #actor:lucas
    Sorry bud. I just felt like catching up.
- else:
    Not much, trying to get some work done but went down this video rabbit hole.
    Have you ever seen those videos of {~stuff getting smushed by hydraulic presses?|that guy who reviews instant noodles?}
    $. #actor:lucas
    I probably have at some point.
}

Anyway, how is Sarah?

$. #actor:external

She's doing fine. She's actually going to look for tuxes {currentTime == "21:00":tomorrow.|later today.}

$. #actor:lucas

Tuxes? For the wedding? Shouldn't you be going with her?

$. #actor:external

Nah, I've got some stuff I have to do.

Anyway, I doubt my input would matter that much.

$. #dramaticpause:2

Actually, that reminds me.

I have to tell you what we were talking about the other day.

$. #fade:out

$. #advancetime:1

$. #dramaticpause:3

$. #fade:in

$. #dramaticpause:1

I know that was long-winded.

...but in other words, she's thinking I should look for a job that gets me out of the house.

$. #actor:lucas

Is that what you want?

$. #actor:external

I don't know, I kind of like the flexibility of working from home.

$. #dramaticpause:2

Oh shit, she's calling now actually. I gotta go.

\*hangs up\*

$. #actor:lucas

$. #animation:PhoneEarToFace

$. #animation:PhonePutDown

~ callback = -> call_harper_first
$. #endsession:true
->DONE

=== call_harper_first ===
$. #animation:PhonePickUp

Whoops, Harper sent me a text {currentTime == "08:00":last night|earlier}.

I'll just give her a call.

$. #animation:PhoneToEar

$. #dramaticpause:3

$. #actor:external

Hey Bro!

$. #actor:lucas

Hey Sis. How's it going?

$. #actor:external

Nothing much, just scrolling on my phone. Did you see {~Emilia got a new car|Alexis just got engaged to some rich dude}?

$. #actor:lucas

Nah, I don't really keep up with her. 



$. #actor:lucas

$. #animation:PhoneEarToFace

$. #animation:PhonePutDown

~ callback = -> end_story
$. #endsession:true
->DONE

=== end_story ===
-> END