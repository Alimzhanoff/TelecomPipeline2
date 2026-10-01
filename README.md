Asignment #2 
Alimzhan Adilhan 
This project is a multi-part laboratory assignment focused on building a robust, high-performance, and thread-safe call processing pipeline in C# (.NET 8/12), featuring custom data structures, functional pricing logic, and manual multi-threading without data races.

---

## Project Structure & Architecture

The solution consists of two main projects:
1. **`TelecomPipeline2` (Core Library):** Contains the core business logic, immutable records, pricing algorithms, and parallel processing pipeline.
2. **`UnixTest` (Test Project):** An xUnit-based automated testing suite covering tariff correctness, boundary validations, and parallel-sequential consistency.

### Key Components:
* **`CallRecord` (`readonly record struct`):** Represents an immutable telecom call record containing unique ID, destination country, call duration (in minutes), and roaming status. Includes strict validation logic enforcing input integrity.
* **`CallPricing` (`static class`):** Implements pure-function tariff logic using advanced C# pattern matching and switch expressions to calculate call costs based on geographic and roaming rules.
* **`CallProcessor` (`static class`):** Handles both sequential baseline processing and parallel execution split exactly across two threads using raw `Thread` management and isolated output arrays.

---

## Task 3: Race Condition Explanation

### Why the global counter is unsafe under concurrent execution:
A global counter increment operation (such as `globalCallCounter++`) **is not atomic**[cite: 1]. At the CPU instruction level, it breaks down into three distinct steps:
1. **Read:** The thread reads the current value of the counter from shared memory into a CPU register.
2. **Modify:** The thread increments the value in its register by 1.
3. **Write:** The new value is written back to shared memory.

### Example of a Lost-Update Interleaving:
When two threads execute this concurrently, a race condition known as a **lost update** occurs:
* **Thread A** reads the counter (e.g., current value is `10`).
* **Thread B** reads the counter (value is still `10`).
* **Thread A** increments its register to `11` and writes it back.
* **Thread B** increments its register (which still holds `10`) to `11` and writes it back.

As a result, two calls were processed, but the counter only increased by 1 instead of 2.

### Resolution in our pipeline:
To guarantee thread safety in our parallel implementation (`ProcessCallsParallel`), **we completely avoided shared mutable state**. Instead of mutating a global counter, each worker thread writes its computation results into its own isolated, strictly partitioned output array (`results1` and `results2`), which are safely aggregated *only after* both threads have terminated via `Join()`[cite: 1].

---

## Running Automated Tests

The solution includes a comprehensive xUnit test suite divided into three validation groups:
* **Group A:** Tariff calculation correctness across various regions and roaming states[cite: 1].
* **Group B:** Edge cases, invalid data rejections, and boundary conditions[cite: 1].
* **Group C:** Sequential-parallel agreement, immutability guarantees, and thread synchronization verification[cite: 1].

To run the tests from the command line or Visual Studio:
1. Open the solution in **Visual Studio**.
2. Build the solution (`Ctrl + Shift + B`)[cite: 1].
3. Open **Test Explorer** (`Test` -> `Test Explorer`) and click **Run All Tests**[cite: 1].