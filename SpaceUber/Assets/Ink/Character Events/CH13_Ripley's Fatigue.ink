Your camera's notice the ship's doctor is hard at work in the Medbay, hard at work on the autopsy of a recently deceased crew member. All of a sudden she stops, puts down her tools and removes her gloves. Sinking down into a nearby chair, she puts her head in her hands and starts sobbing.
You wonder if you should try to comfort her. You need to say something, as you need to make sure she can function in case of an emergency.
+ ["Are you going to be able continue working?"] -> NeuRes1
+ ["Are you okay? Is something wrong?"] -> PosRes1
+ ["What's there to be upset about. I think you'd be used to it by now."] -> NegRes1


=== PosRes1 ===
At your words, her head jerks up, her face covered in tears. She snaps. "Does it look like I'm okay?!" Wiping her face, her voice wavers a little as she talks. "I'm sorry. Thank you for asking. It's just that... I'm just so tired of this. It feels that for every three patients I heal, I have one that is beyond help.
"I used to hope that, whenever we set off, we'd get through the job without losing anyone. Eventually, it changed to hoping that the fatalities would be limited to just one unfortunate soul per job. Now, I can't even bother hoping. It seems pointless."
+ ["How many more would be dead without you, though?"] -> PosRes2
+ ["One day we will be able to do this job without a death. I promise it."] -> NeuRes2

=== NeuRes1 ===
At your words, her head jerks up, her face covered in tears. Wiping off, she speaks, her voice wavering. "Yes, I'll be...I'll be fine. It's just that... I'm just so tired of this. It feels that for every three patients I heal, I have one that is beyond help.
"I used to hope that, whenever we set off, we'd get through the job without losing anyone. Eventually, it changed to hoping that the fatalities would be limited to just one unfortunate soul per job. Now, I can't even bother hoping. It seems pointless."
+ ["How many more would be dead without you, though?"] -> PosRes2
+ ["One day we will be able to do this job without a death. I promise it."] -> NeuRes2

=== NegRes1 ===
At your cold, emotionless voice, Ripley's head snaps up. "Used to it? You think I'm used to having people I work with brought into the Medbay, bereft of life. I've grown numb to it, but sometimes it, it just break through, and when it does, dammit, it hurts. Why can't we just get through one damn job without a death?"
+ ["How many more would be dead without you, though?"] -> PosRes2
+ ["One day we will be able to do this job without a death. I promise it."] -> NeuRes2

=== PosRes2 ===
Your response makes her stop and think. Eventually, she utters out. "I never think about that. So, so often all I think of is those I've lost. Maybe I just need to keep thinking about the people who I've helped keep alive.
"I can't help those who are already gone when they are brought here. But I've saved so many who are at death's door." Composing herself, you can see the resolve on her face. "And there will be many more who need that help in the future. Thank you so much for your words, but I can't talk any longer. I've got work to do."
-> END

=== NeuRes2 ===
"How can you promise that. Not only is it out of your control, but I know your concern isn't the crew but the credits." She pauses. "Maybe you're telling the truth though. I have to believe that because I can't keep this up otherwise. Thank you for checking on me."
With that, she composes herself and gets back to work.
-> END