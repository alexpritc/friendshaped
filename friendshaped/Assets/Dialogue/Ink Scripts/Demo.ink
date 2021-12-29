A passenger on this train, John Doe, was murdered last night.
-> Interrogate

===Interrogate
There are 3 possible suspects. Who should I interrogate?

 * [Inspector Sands]
 -> Inspector_Sands
 * [Lady Mortimer]
 -> Lady_Mortimer
 * [The Train Conductor]
 -> The_Train_Conductor
   * [Accuse someone]
 ->Accuse

===Inspector_Sands
Inspector Sands is a middle aged gentlemen, a P.I (and a hack one at that). 
He wears a brown trench coat and carries a magnifying glass everywhere he goes, like he belongs in a cartoon.
-> Inspector_Sands_Interrogate

===Inspector_Sands_Interrogate
 * [What's your alibi?]
 -> Inspector_Sands_Alibi
 * [Who do you think is to blame?]
 -> Inspector_Sands_Suspects
 * [How is the P.I work?]
 -> Inspector_Sands_Info
 * [Interrogate someone else]
 ->Interrogate
  * [Accuse someone]
 ->Accuse
 
 ===Inspector_Sands_Alibi
"Where were you at the time of the murder, Inspector?" I ask.
He looks nervous.
"First of all, I don't know when the murder happened. But the last time I saw John Doe was in the dining car. Everbody was there. I retired to my cabin early in the evening, though. John Doe was alive when I left."
->Inspector_Sands_Interrogate

 ===Inspector_Sands_Suspects
 "Does anyone on the train have motive to kill John Doe?"
 He thinks for a moment.
 "Hmm.. Lady Mortimer was having an arguement with John Doe in the dining car last night. It was practically a domestic, maybe you should speak to her."
 ->Inspector_Sands_Interrogate
 
  ===Inspector_Sands_Info
"How goes being a P.I these days?"

"It's a tough gig, usually the husband is cheating on his wife. I lost my license recently because the husband had a much better laywer than me. As a P.I you don't often get a juicy murder like this. Speaking of, I do not believe you are qualified to investigate this and I do not understand why The Train Conductor is allowing you to play detective when I, a professional, am on board."

 * [That sounds like a motive to me]
 -> Inspector_Sands_Suspicious
 * [What's your expert opinion?]
 -> Inspector_Sands_Opinion

===Inspector_Sands_Suspicious
"Sounds to me like you benefit from this murder. 'DISGRACED P.I. SOLVES LOCOMOTIVE LOCO-MURDER' - what do you reckon? A killer headline or what?"

He scoffs.

"What are you insinuating? That I would commit a murder for the sole purpose of solving it? I should sue you for slander, I've gone after people for less before and I have much better lawyers now."
->Inspector_Sands_Interrogate

===Inspector_Sands_Opinion
"You've got lots of experience, then. What do you make of this incident?"

His eyes light up and he takes out a notebook from his coat pocket.

"Finally! A smart question. Well, clearly we know that the spouse couldn't have done it, as Mrs Doe is not onboard. That leaves one possibility: a suicide! John Doe killed himself so he wouldn't have to pay his bar tab."

I look at him and wait for the punchline, but he is completely serious...
-> Inspector_Sands_Interrogate

===Lady_Mortimer
Lady Mortimer is an elderly lady and quite the character. She wears a gold bird cage as a head ornement (which she insists isn't a hat) so she can be close to her dearest pet conure at all times.
-> Lady_Mortimer_Interrogate

===Lady_Mortimer_Interrogate
 * [What's your alibi?]
 -> Lady_Mortimer_Alibi
 * [Who do you think is to blame?]
 -> Lady_Mortimer_Suspects
 * [What is your deal with the bird?]
 -> Lady_Mortimer_Info
 * [Interrogate someone else]
 ->Interrogate
   * [Accuse someone]
 ->Accuse
 
 ===Lady_Mortimer_Alibi
 "Ma'am, did anything strange happen last night?"
 
 She takes a few seconds before responding.
 
 "Not particularly, besides the total disregard for social etiquette. John Doe was incredibly rude to me last night and neither Inspector Sands nor The Train Conductor condemned his behaviour. We were all gathered in the dining car for some drinks. John Doe must've had a few too many Chardonnays because he was making all sorts of rude comments about my appearance and my dear sweet companion here." 
 
 She guestures towards the bird nesting in her hair.
 ->Lady_Mortimer_Interrogate
 
 ===Lady_Mortimer_Suspects
 "Did anyone have any reason to kill John Doe?"
 
 "Hm, Well, this kind of thing takes lots of planning. I've seen the movies. You have to have thought about killing them for a long time and you need someone on the inside - like The Train Conductor - they were in the dining car last night, maybe they were involved."
 ->Lady_Mortimer_Interrogate
 
 ===Lady_Mortimer_Info
 "So... The elephant in the room. Bird, I mean. What's with the bird?"
 
 "Excuse you." She looks horridly offended "Mr. Lionel is my dear beloved friend and companion, he's not just 'the bird'. He is a passenger on this train like you or I and you shall show him the up most respect."
 
 * [Apologise to the bird]
 -> Lady_Mortimer_Apologise
 * [What did John Doe say to Mr. Lionel?]
 -> Lady_Mortimer_Argument

===Lady_Mortimer_Apologise
"My sincere apologies, Lady Mortimer, Mr. Lionel. How long have you two been travelling companions?"

"Oh, at least 20 odd years... He has seen so much in that time."
->Lady_Mortimer_Interrogate

===Lady_Mortimer_Argument
"What did John Doe say to you and Mr.Lionel last night in the dining car?"

"All sorts of nasty comments. He was very rude about my head ornament, and called Mr.Lionel... Oh I hate to even repeat it. A "bird brain"."

She's wailing hysterically.
-> Lady_Mortimer_Interrogate

===The_Train_Conductor
The Train Conductor is not present as they are driving the locomotive. They speak over an intercom.
->Conductor_Interrogate

===Conductor_Interrogate
 * [What's your alibi?]
 -> Conductor_Alibi
 * [Who do you think is to blame?]
 -> Conductor_Suspects
 * [What can you tell me about the train?]
 -> Conductor_Info
 * [Interrogate someone else]
 ->Interrogate
   * [Accuse someone]
 ->Accuse

===Conductor_Alibi
"Could you please recount the events of last night."

"Of course. Anyth-- to help --th the investigation." The intercom keeps cutting out.

"I was off-duty in the dining car, enjoying a lat-- dinner. However, there was a disturbance, two passengers --ing. I do not like to intervene with these things but I kep-- a close eye in case it became necessary. Inspector Sands must have had quite enough of the racket too beca-- he left first, quite early on in fact. I went back on-duty shortly after that."
-> Conductor_Interrogate

===Conductor_Suspects
"Who do you think murdered John Doe?"

"It is not my place to point fingers, but Inspector Sands did mention he has been out of work for a while. A murder investigation is exactly what he needed."
->Conductor_Interrogate

===Conductor_Info
"A murder on a train isn't exactly original, but it could be relevant to finding out why this happened. What can you tell me about this train?"

"She's a beaut, isn't she? Built fi-- years ago. The FastTrainX Model 4000 has stat-- of the art engine and boosters. Top speed is 150kmph, if the conditions are right. Inside she is beautifu-- lined with linen and gold. A real first-class ride. Nobody else appreciates her like I do, I wish the world could see her in action."

* [Would you kill for the world to see?]
 -> The_Train_Conductor_Suspicious
 * [I love trains!]
 -> The_Train_Conductor_Trains

===The_Train_Conductor_Suspicious
"Would you perhaps kill for the world to see your train? Excuse the phrase..."

"Absolutely not! That is not the sort of publicity that a condu-- wants for their train."
->Conductor_Interrogate

===The_Train_Conductor_Trains
"I love going on train journeys!"

"Ah, someone with good taste. If we weren't dealing with an incident such as this I would invite y-- in here to have a go at driving her yourself."
->Conductor_Interrogate

===Loop
What should I do?
 * [Interrogate the suspects]
 -> Interrogate
 * [Accuse someone]
 -> Accuse

===Accuse
"Thank you everyone for gathering here, and co-operating during our respective interviews. I have now come to the conclusion that the murderer is you..."
 
 * [Inspector Sands!]
 -> Inspector_Sands_Accused
 * [Lady Mortimer!]
 -> Lady_Mortimer_Accused
 * [The Train Conductor!]
 -> The_Train_Conductor_Accused

===Inspector_Sands_Accused
"Inspector Sands! You killed John Doe so that you could solve a murder and get your P.I credibility back! How do you plead?"

"I knew you weren't the right person for this job. You've gone and accussed an innocent man. I did not kill John Doe, you'll see!"
-> Murderer_Escaped

===Lady_Mortimer_Accused
"Lady Mortimer! You killed John Doe after he drunkenly insulted Mr.Lionel last night. Your alibi does not match that of the other passengers. You claimed that you left the dining car first last night, but two other accounts suggest you stayed longer, alone with John Doe."

"Poppycock! I would never... Oh what's the point in pretending anymore. You caught me. I killed him and I would do it again in a heartbeat. Nobody who offends Mr.Lionel goes unpunished."
-> Murderer_Caught

===The_Train_Conductor_Accused
"The Train Conductor! You killed John Doe in an attempt to get your darling train on the news so that everyone could see her."

"This is ridiculous! I am innocent! The train logs will show that I was driving her and couldn't possibly have done this."
-> Murderer_Escaped

===Murderer_Caught
I successfully deduced who the murderer was.
->END
===Murderer_Escaped
I let the murderer escape.
->END