INCLUDE Lady Intro
INCLUDE Steward Intro
INCLUDE Inspector Intro
INCLUDE Conductor Intro


// who to talk to
->Start
===Start 
Who do you want to talk to?
*{not SandsDescription} [Inspector Sands]
->SandsDescription
+{SandsDescription} [Inspector Sands]
->SandsIntro
*{not LadyDescription} [Lady Mortimer]
->LadyDescription
+{LadyDescription} [Lady Mortimer]
->LadyIntro
*{not StewardDescription} [The Steward]
->StewardDescription
+{StewardDescription} [The Steward]
->StewardIntro
*{not ConductorDescription} [The Conductor]
-> ConductorDescription
+{ConductorDescription} [The Conductor]
-> ConductorIntro
*->
-> END








