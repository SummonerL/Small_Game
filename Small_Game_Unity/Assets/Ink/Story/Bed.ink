INCLUDE GlobalVariables.ink


VAR callback = -> bed_thinking_first_event

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

$. #dramaticpause:2

\*sigh\* Should I call Mom back?

~ addMemory("call_mom_back_first")

$. #endsession:true
-> callback
-> DONE



/**
*   The following knots makeup the main, non-conditional story, and progresses linearly.
*   Exclusive events will occur first, and then automatically transition to this.
*   Make sure to update the callback after each event.
**/
=== bed_thinking_first_event ===
$. #animation:BedCollapse

$. #dramaticpause:4

{checkMemory("talked_mom_first"):
    Mom says I have a lot to be proud of...
    Like what?
    I'm not trying to be dramatic, but I'm struggling to think of much.
    { checkMemory("talked_harper_first"):
        Harper is right, all I do is sit on my ass all day.
    }
    I haven't written any music in years.
    I Haven't done any work in a while.
    I'm struggling to pay rent.
    I'm afraid to leave the house.
    $. #dramaticpause:2
    I should just ask her next time.
    And listen to her struggle to come up with anything.
    $. #dramaticpause:2
    ...I'm being too harsh. 
    She's just trying to help.
    
    $. #dramaticpause:2
}

{ not checkMemory("talked_harper_first") || checkMemory("talked_nathan_first"):
    Maybe I should try and be more intentional about keeping in touch with my family.
    
    {not checkMemory("talked_harper_first"):
        I haven't talked to Harper in a while.
        
        {not checkMemory("talked_nathan_first"):
            ...or Nathan for that matter.
        }
    }
    
    $. #dramaticpause:2
}

{~->out_of_shape|->park_outside}

= out_of_shape
    I'm getting out of shape.
    
    Maybe I've been too sedentary lately.
    
    I smell like garbage as well.
    
    In my defense though, what apartments don't come equipped with a shower?
    
    ->fall_asleep

= park_outside
    I probably shouldn't have picked an apartment right next to a park.
    
    All of the park-sounds make me feel guilty for not getting out.
    
    So many families and kids laughing and playing.
    
    $. #dramaticpause:2
    
    I wonder if I'll ever have kids?
    
    $. #dramaticpause:2
    
    Probably not.
    
    ->fall_asleep

= fall_asleep

$. #fade:out

$. #animationControl:reset_state

$. #animation:BedSleepingStill

$. #advancetime:1

$. #fade:in

$. #dramaticpause:1

$. #animation:BedSleepingGetUp

~ callback = -> end_story
$. #endsession:true
->DONE



=== end_story ===
I'm not tired at all.
-> END
