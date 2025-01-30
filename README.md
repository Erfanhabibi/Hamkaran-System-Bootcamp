# C# Programming Tasks

---

## Task 1: Implement Max Heap and Min Heap

**Objective**:  
Implement Max Heap and Min Heap in C# following the Open-Closed principle.

**Requirements**:
- Create a base `Heap<T>` class with common functionality
- Derive `MaxHeap<T>` and `MinHeap<T>` classes
- Encapsulate comparison logic in derived classes
- Implement core heap operations:
  - Insert
  - RemoveTop
  - Peek
  - HeapifyUp
  - HeapifyDown

---

## Task 2: Task Prioritization Manager

**Objective**:  
Create a console application to manage and prioritize tasks based on due dates and priority levels.

**Requirements**:
- Task model with:
  - Title
  - Description
  - Creation date
  - Due date
  - Priority (Low/Medium/High)
- Program logic to:
  - Find task closest to current date
  - Resolve ties using priority levels
  - Output task details
- Use hardcoded sample data
