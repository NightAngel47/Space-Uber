Kuon approaches your terminal. "I tend to not get along with a lot of the crew. Do you think I should try to communicate with them better? They seem not to care for my dedication to the job." 
+ ["The job comes first but having friends couldn't hurt."] -> PosRes1
+ ["You care way too much about your job."] -> NegRes1
+ ["Doing the job and doing it well is more important than friends."] -> NeuRes1


=== PosRes1 ===
He glances off to the side, taking a moment to think. Looking back at your camera, he continues. "Huh. I guess I should try to have conversations with the others. Do you have any advice for doing so?"
+ ["Talk with them about their job."] -> NegRes2
+ ["Try to listen well and give meaningful responses."] -> PosRes2
+ ["I'm a computer. I don't know too much about making friends."] -> NeuRes2

=== NeuRes1 ===
He glances down, as if in thought. "I guess that's true. That being said I still want to get along with at least some of the crew. Any advice?"
+ ["Talk with them about their job."] -> NegRes2
+ ["Try to listen well and give meaningful responses."] -> PosRes2
+ ["I'm a computer. I don't know too much about making friends."] -> NeuRes2

=== NegRes1 ===
He crosses his arms and glares at you, obviously annoyed at your bad mouthing his dedication. Dropping his arms and the glare, he continues. "Well, if that's your opinion, then do you have any advice to help me talk with the others?"
+ ["Talk with them about their job."] -> NegRes2
+ ["Try to listen well and give meaningful responses."] -> PosRes2
+ ["I'm a computer. I don't know too much about making friends."] -> NeuRes2

=== PosRes2 ===
He ponders your advice for a second. "It's basic advice...but I think it could help. I don't... have much of a life outside my job, so allowing others to do most of the talking makes sense. Thank you."
-> END

=== NeuRes2 ===
"I guess that's true. You must not have been programmed  I'll see the others can give me some advice." He sighs, thanks you for your time, and leaves the room.
-> END

=== NegRes2 ===
Annoyed at your advice, he responds. "Didn't I tell you that they think I'm too obsessed with my job in the first place? They obviously don't want to hear about it... and you obviously haven't been listening to me."
-> END