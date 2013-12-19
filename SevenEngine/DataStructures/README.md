SevenEngine/DataStructures README.md

IMPORTANT COMMENTS:-------------------------------------------------

1) All of the data structures follow the "DataStructure" interface.
This interface enforces the data structures to have "ToArray()"
converters, as well as anonymous method traversals, which is a more
efficient version of iteration.

2) All data structures should be thread safe using the 
"May-Readers-One-Writer" threading pattern.

3) I have an interface for each type of structure. The actual classes
have more specific names relative to the method of implementation.
For example, a "Linked" data structure has pointers to the remaining
items in the structure while "Array" structures are implemented as
an expanding and contracting array. Other names, such as "Static", are
specific to the data structure, so check the class summaries for details.

DATA STRUCTURES REFERENCE:------------------------------------------

Here is a tree representing a class/interface diagram for all the 
currently implemented data structures in SevenEngine:

"-" = interface
"+" = class

+ Link2
+ Link3
+ Link4
+ Link5
+ Link6
+ Link7
- DataStructure<Type>
|--- Array<Type>
|  |--+ ArrayBase<Type>
|  |--+ ArrayCyclic<Type>
|--- List<Type>
|  |--+ ListLinked<ValueType, KeyType>
|  |--+ ListArray<Type>
|--- Stack<Type>
|  |--+ StackLinked<Type>
|  |--+ StackArray<Type>
|--- Queue<Type>
|  |--+ QueueLinked<Type>
|  |--+ QueueArray<Type>
|--- Heap<Type>
|  |--+ HeapArrayStatic<Type>
|  |--+ HeapArrayDynamic<Type>
|--- HashTable<ValueType, KeyType>
|  |--+ HashTableLinked<ValueType, KeyType>
|--- AvlTree<ValueType, Keytype>
|  |--+ AvlTreeLinked<ValueType, Keytype>
|  |--- AvlTree<ValueType, FirstKeytype, SecondKeyType>
|     |--+ AvlTreeLinked<ValueType, FirstKeytype, SecondKeyType>
|--- ReadBlackTree<ValueType, Keytype>
|  |--+ RedBlackTreeLinked<ValueType, Keytype>
|  |--- RedBlackTree<ValueType, FirstKeytype, SecondKeyType>
|     |--+ RedBlackTreeLinked<ValueType, FirstKeytype, SecondKeyType>
|--- Octree<ValueType, Keytype>
   |--+ OctreeLinked<ValueType, Keytype>

NOTE: if more than two key types are required for multi-key possible 
structures, just follow the pattern I have from single to double keys.

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

SORTING ALGORITHMS:------------------------------------------------

Many data structures have methods of sorting the data within them.
However, if you have an array that you wish to sort without throwing
the data into a structure, you can use the static "Sort" class to
sort your array. Each algorithm includes "IComparable" implementations
and anonymous method comparisons, so you can sort them any way you
need to. :)

SUPPORT:-----------------------------------------------------------

If you notice any glitches or missing documentation, post a notice on 
the official website (SevenEngine.com) or email seven@sevenengine.com.