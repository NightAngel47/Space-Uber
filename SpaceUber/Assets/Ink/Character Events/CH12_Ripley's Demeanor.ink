Ripley comes into your terminal room. "I don't see why we couldn't have held this conversation in the Medbay. You're taking me away from my work." You reply that you thought she'd want some privacy for this conversation. Her response is simple. "I don't really care. Hurry up and ask."
+ ["You need to improve your bedside manner."] -> NegRes1
+ ["You lack compassion."] -> NeuRes1

=== NeuRes1 ===
She just looks into your camera, not showing any emotion. "What's the point? Being compassionate to a hurt crew member wouldn't change the fact that the next time they're in Medbay they're likely to be decomposing on my autopsy table instead of recovering in a bed."
+ ["Thank you for your input."] -> NegRes2
+ ["Final moments should be peaceful."] -> PosRes1


=== NegRes1 ===
Knitting her brow, she replies. "I don't see how putting on a pathetic little act and giving meaningless reassurances would help anything. Why lie and tell them everything is going to be fine when they're likely to be brought back zipped up in a plastic bag."
+ ["Thank you for your input."] -> NegRes2
+ ["Final moments should be peaceful."] -> PosRes1
=== PosRes1 ===
She pauses for a long moment, her eyes eventually drifting down. "I've numbed myself to loss so much that, while their deaths don't hurt me, my callousness is hurting them in life. I don't know if I can promise much, but I will try to make the time the crew spends in the Medbay a more pleasant one."
Still in contemplation, she leaves, heading back to Medbay to await patients, both the living and the dead.
-> END

=== NegRes2 ===
"If you want to waste my time, just do it in the Medbay." With that, she leaves.
-> END