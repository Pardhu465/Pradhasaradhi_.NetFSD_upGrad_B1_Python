-- ═══════════════════════════════════════════════════════════════
-- COMPLETE SQL QUERY COLLECTION — E-Learning Platform
-- ═══════════════════════════════════════════════════════════════

USE ELearningDB;
GO

-- ─────────────────────────────────────────────────────────────
-- 1. BASIC QUERIES — SELECT, WHERE, ORDER BY
-- ─────────────────────────────────────────────────────────────

-- List all users ordered by name
SELECT UserId, FullName, Email, CreatedAt
FROM   Users
ORDER BY FullName ASC;

-- Find user by email
SELECT UserId, FullName, Email, PasswordHash
FROM   Users
WHERE  Email = 'student@email.com';

-- All courses ordered by creation date
SELECT CourseId, Title, Description, CreatedAt
FROM   Courses
ORDER BY CreatedAt DESC;

-- Lessons for a specific course ordered by index
SELECT LessonId, Title, Content, OrderIndex
FROM   Lessons
WHERE  CourseId = 1
ORDER BY OrderIndex ASC;

-- Quiz results for a user ordered by date
SELECT ResultId, QuizId, Score, AttemptDate
FROM   Results
WHERE  UserId = 1
ORDER BY AttemptDate DESC;

-- ─────────────────────────────────────────────────────────────
-- 2. JOINS — INNER JOIN, LEFT JOIN
-- ─────────────────────────────────────────────────────────────

-- INNER JOIN: Courses with their creator info
SELECT c.CourseId, c.Title, c.Description,
       u.FullName AS CreatedByName, u.Email AS CreatorEmail
FROM   Courses c
INNER JOIN Users u ON u.UserId = c.CreatedBy;

-- INNER JOIN: Lessons with course title
SELECT l.LessonId, l.Title AS LessonTitle,
       l.OrderIndex, c.Title AS CourseName
FROM   Lessons l
INNER JOIN Courses c ON c.CourseId = l.CourseId
ORDER BY c.Title, l.OrderIndex;

-- INNER JOIN: Quiz results with user and quiz details
SELECT r.ResultId, u.FullName, q.Title AS QuizTitle,
       r.Score, r.AttemptDate
FROM   Results r
INNER JOIN Users  u ON u.UserId = r.UserId
INNER JOIN Quizzes q ON q.QuizId = r.QuizId
ORDER BY r.AttemptDate DESC;

-- LEFT JOIN: All courses even those with no quizzes
SELECT c.CourseId, c.Title,
       COUNT(q.QuizId) AS QuizCount
FROM   Courses c
LEFT JOIN Quizzes q ON q.CourseId = c.CourseId
GROUP BY c.CourseId, c.Title;

-- LEFT JOIN: All users including those with no results
SELECT u.UserId, u.FullName, u.Email,
       COUNT(r.ResultId) AS TotalAttempts
FROM   Users u
LEFT JOIN Results r ON r.UserId = u.UserId
GROUP BY u.UserId, u.FullName, u.Email;

-- ─────────────────────────────────────────────────────────────
-- 3. AGGREGATION — GROUP BY, COUNT, AVG
-- ─────────────────────────────────────────────────────────────

-- Count lessons per course
SELECT c.Title, COUNT(l.LessonId) AS LessonCount
FROM   Courses c
LEFT JOIN Lessons l ON l.CourseId = c.CourseId
GROUP BY c.CourseId, c.Title
ORDER BY LessonCount DESC;

-- Average score per user
SELECT u.FullName,
       COUNT(r.ResultId)    AS TotalAttempts,
       AVG(r.Score)         AS AverageScore,
       MAX(r.Score)         AS BestScore
FROM   Users u
INNER JOIN Results r ON r.UserId = u.UserId
GROUP BY u.UserId, u.FullName
ORDER BY AverageScore DESC;

-- Count questions per quiz
SELECT q.Title AS QuizTitle, COUNT(qn.QuestionId) AS QuestionCount
FROM   Quizzes q
LEFT JOIN Questions qn ON qn.QuizId = q.QuizId
GROUP BY q.QuizId, q.Title;

-- Average score per quiz
SELECT q.Title AS QuizTitle,
       AVG(r.Score) AS AvgScore,
       COUNT(r.ResultId) AS TimesAttempted
FROM   Quizzes q
INNER JOIN Results r ON r.QuizId = q.QuizId
GROUP BY q.QuizId, q.Title
HAVING COUNT(r.ResultId) > 0;

-- ─────────────────────────────────────────────────────────────
-- 4. SUBQUERIES
-- ─────────────────────────────────────────────────────────────

-- Users scoring above average on any quiz
SELECT DISTINCT u.UserId, u.FullName, u.Email
FROM   Users u
WHERE  u.UserId IN (
    SELECT r.UserId
    FROM   Results r
    WHERE  r.Score > (SELECT AVG(Score) FROM Results)
);

-- Courses that have at least one quiz
SELECT CourseId, Title
FROM   Courses
WHERE  CourseId IN (SELECT DISTINCT CourseId FROM Quizzes);

-- Users who have never taken a quiz
SELECT UserId, FullName, Email
FROM   Users
WHERE  UserId NOT IN (SELECT DISTINCT UserId FROM Results);

-- Top quiz (highest average score) using subquery
SELECT QuizId, Title
FROM   Quizzes
WHERE  QuizId = (
    SELECT TOP 1 QuizId
    FROM   Results
    GROUP BY QuizId
    ORDER BY AVG(Score) DESC
);

-- ─────────────────────────────────────────────────────────────
-- 5. SET OPERATORS — UNION
-- ─────────────────────────────────────────────────────────────

-- UNION: All titles (courses and quizzes) in one list
SELECT 'Course' AS Type, Title FROM Courses
UNION
SELECT 'Quiz'   AS Type, Title FROM Quizzes
ORDER BY Type, Title;

-- UNION: Users who created courses OR took quizzes
SELECT UserId, FullName, 'Creator' AS Role
FROM   Users
WHERE  UserId IN (SELECT DISTINCT CreatedBy FROM Courses)
UNION
SELECT UserId, FullName, 'Learner' AS Role
FROM   Users
WHERE  UserId IN (SELECT DISTINCT UserId FROM Results);

-- ─────────────────────────────────────────────────────────────
-- 6. DML — INSERT, UPDATE, DELETE
-- ─────────────────────────────────────────────────────────────

-- INSERT User (PasswordHash stored — app hashes before calling)
INSERT INTO Users (FullName, Email, PasswordHash)
VALUES ('John Doe', 'john@example.com',
        'AQAAAAIAAYagAAAA...');  -- BCrypt hash from application

-- INSERT Course
INSERT INTO Courses (Title, Description, CreatedBy)
VALUES ('Introduction to C#', 'Learn C# from scratch', 1);

-- INSERT Lesson
INSERT INTO Lessons (CourseId, Title, Content, OrderIndex)
VALUES (1, 'Variables and Types',
        'In this lesson we cover all C# primitive types.', 1);

-- INSERT Quiz
INSERT INTO Quizzes (CourseId, Title)
VALUES (1, 'C# Basics Quiz');

-- INSERT Question
INSERT INTO Questions
    (QuizId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer)
VALUES (1, 'Which keyword declares a variable in C#?',
        'var', 'let', 'dim', 'def', 'A');

-- INSERT Result
INSERT INTO Results (UserId, QuizId, Score)
VALUES (2, 1, 85);

-- UPDATE Course
UPDATE Courses
SET    Title       = 'Advanced C#',
       Description = 'Deep dive into C# advanced features'
WHERE  CourseId = 1;

-- UPDATE Lesson OrderIndex
UPDATE Lessons
SET    OrderIndex = 2
WHERE  LessonId = 1;

-- UPDATE User profile
UPDATE Users
SET    FullName = 'Jane Smith'
WHERE  UserId = 2;

-- DELETE Lesson
DELETE FROM Lessons WHERE LessonId = 5;

-- DELETE Course (cascades to Lessons, Quizzes, Questions)
DELETE FROM Courses WHERE CourseId = 3;

-- DELETE Quiz Result
DELETE FROM Results WHERE ResultId = 10;
GO