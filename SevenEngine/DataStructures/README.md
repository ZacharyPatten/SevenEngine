SevenEngine/DataStructures

IMPORTANT COMMENTS:-------------------------------------------------

1) See the documentation on SevenEngine.com for more help.

2) All data structures should be thread safe using the 
May-Readers-One-Writer threading pattern. (if you find a glitch in
the threading PLEASE LET ME KNOW).

3) All of my data structures follow the functional programming 
paradigm. This means that instead of using the traditional C#
"foreach" loops, they use traversal loops with a delegate function.
This delegate function determines what to do during the traversal.
Simply pass the traversal function a lambda expression that takes
the type of the structure (you declared the type during 
construction), and returns a bool value determining wheather or 
not to continue traversal (simulated break point by returning false).

RUNTIME INTERPRETATION:---------------------------------------------

These data structure files contain runtime values.
The runtime should be located in the "remarks" tag on all public members.
All runtimes are in O-Notation. Here is a brief explanation:
- "O(x)": the member has an upper bound of runtime equation "x"
- "Omega(x)": the member has a lower bound of runtime equation "x"
- "Theta(x)": the member has an upper and lower bound of runtime equation "x"
- "EstAvg(x)": the runtime equation "x" to typically expect
Note that if the letter "n" is used, it typically means the current 
number of items within the set.

CONTACT:------------------------------------------------------------

If you notice any glitches or missing documentation, post a notice on 
the official website (SevenEngine.com) or email seven@sevenengine.com.