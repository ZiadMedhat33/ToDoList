While working on this To-Do List backend project, I implemented core functionality such as:

✅ User Registration and Login

✅ Adding and Deleting Tasks

✅ Marking Tasks as Done

I also started exploring extended features like allowing users to become friends. However, during development, I encountered areas that revealed gaps in planning — and these became valuable lessons:

🛠️ I learned that using DTOs (Data Transfer Objects) is not just a bonus or optional — it's crucial for decoupling domain models from API contracts. Without DTOs, even simple updates (like modifying user info) became harder to manage and more error-prone.

🧱 I realized the need for a more convenient and flexible ORM abstraction, especially when it comes to supporting async operations, clean querying, and separation of database concerns but maybe keeping the ORM simple is good enough for just the CRUD operations.

🚨 I also gained insight into the importance of planning exception handling from the beginning — including building custom exceptions to make error responses consistent and meaningful.

Because of these insights, I've decided to end this project early and start fresh on a new one — one that applies everything I’ve learned here. Rather than patching this codebase, I’m choosing to move forward with a cleaner and more maintainable design.

This project was a valuable learning opportunity, and I’m grateful for the experience. I’m excited to continue building with a stronger foundation and better architectural practices in mind.