using System;

namespace OOPAssignment05
{
    #region PART 01: THEORETICAL QUESTIONS ANSWERS

    /*
     * Q1: What is an interface in C#? Benefits & direct dependence vs interfaces.
     * - An interface is a full abstract contract (`interface`) that defines signatures for methods, 
     *   properties, events, or indexers without providing default storage/state.
     * - We use interfaces to decouple consumers from concrete implementations (Dependency Inversion),
     *   allowing code to interact with abstract capabilities rather than fixed class types.
     * - Benefits:
     *   1. Multiple Inheritance of Types: Enables a class to implement multiple contracts.
     *   2. Decoupling & Testability: Code depends on contracts, making mock objects easy to insert.
     *   3. Polymorphism across unrelated hierarchies: Allows completely different class hierarchies 
     *      to be processed uniformly if they implement the same interface (e.g., Ticket and Receipt implementing IPrintable).
     * 
     * Q2: Interface Member Name Collision & Explicit Interface Implementation.
     * a) Problem: Both `IEnglishSpeaker` and `IArabicSpeaker` contain `void Greet()`. Currently, `Translator` 
     *    implements a single implicit `Greet()` method that handles both interfaces simultaneously with a merged output.
     * b) Solution: Use Explicit Interface Implementation:
     *    void IEnglishSpeaker.Greet() => Console.WriteLine("Hello");
     *    void IArabicSpeaker.Greet() => Console.WriteLine("Ahlan");
     *    Technique Name: Explicit Interface Implementation.
     * c) No, you cannot call `translator.Greet()` directly on the object instance because explicit methods 
     *    are not public members of the class instance. You must cast the instance to the specific interface:
     *    ((IEnglishSpeaker)translator).Greet();
     *    ((IArabicSpeaker)translator).Greet();
     * 
     * Q3: Shallow Copy vs Deep Copy.
     * - Shallow Copy: Duplicates the top-level object structure. Primitive types are copied by value, 
     *   but reference-type fields copy only the memory address (reference).
     * - Deep Copy: Duplicates the top-level object AND recursively creates distinct copies of all 
     *   referenced objects, ensuring zero shared state.
     * - Usage: Shallow copy is suitable for simple flat value-based objects. Deep copy is mandatory 
     *   when objects contain nested reference types that must mutate independently.
     * - Risk of Shallow Copy: Mutating a reference field in the copied object silently modifies the original 
     *   object's data as both variables point to the exact same memory location on the heap.
     * 
     * Q4: Code Output & Explanation.
     * - Output:
     *   Dev - Testing
     *   QA - Testing
     * - Explanation: `MemberwiseClone()` created a shallow copy (`e2`). Modifying primitive `Title` on `e2` 
     *   ("QA") did not alter `e1.Title` ("Dev"). However, `Dept` is a reference-type field; both `e1.Dept` 
     *   and `e2.Dept` point to the same `Department` instance in memory. Setting `e2.Dept.Name = "Testing"` 
     *   mutated the underlying shared object, reflecting "Testing" for both employees.
     */

    #endregion
}