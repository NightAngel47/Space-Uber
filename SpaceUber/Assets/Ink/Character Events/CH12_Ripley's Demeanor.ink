Ripley comes into your terminal room. "I don't see why we couldn't have held this conversation in the Medbay. You're taking me away from my work." You reply that you thought she'd want some privacy for this conversation. Her response is simple. "I don't really care. Hurry up and ask."
+ ["You really need to improve your attitude towards your patients"] -> NegRes1
+ ["You lack compassion. Better bedside manner would reassure the crew."] -> NeuRes1

=== NeuRes1 ===
She just looks into your camera, not showing any emotion. "What's the point? Being compassionate to a hurt crew member wouldn't change the fact that the next time they're in Medbay they're likely to be decomposing on my autopsy table instead of recovering in a bed."
+ ["I understand. Thank you for your time."] -> NegRes2
+ ["I know but try to make what could be patients final hours a more pleasant one. Please."] -> PosRes1


=== NegRes1 ===
Knitting her brow, she replies. "I don't see how putting on a pathetic little act and giving meaningless reassurances would help anything. Why lie and tell them everything is going to be fine when they're likely to be brought back zipped up in a plastic bag."
+ ["I understand. Thank you for your time."] -> NegRes2
+ ["I know but try to make what could be patients final hours a more pleasant one. Please."] -> PosRes1
=== PosRes1 ===
She pauses for a long moment, her eyes eventually drifting down. "I've numbed myself to loss so much that, while their deaths don't hurt me, I'm hurting them in life. I don't know if I can promise much, but I will try to make the time the crew spends in the Medbay a more pleasant one."
Still in contemplation, she leaves, heading back to Medbay to await patients, both the living and the dead.
-> END

=== NegRes2 ===
"Next time you want to waste my time, just do it in the Medbay." With that, she leaves.
-> END