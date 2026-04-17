-- ═══════════════════════════════════════════════════════════════
-- E-Learning Platform — SQL Server Schema
-- ═══════════════════════════════════════════════════════════════

CREATE DATABASE ELearningDB;
GO
USE ELearningDB;
GO

-- ── 1. Users ─────────────────────────────────────────────────
CREATE TABLE Users (
    UserId        INT IDENTITY(1,1) PRIMARY KEY,
    FullName      VARCHAR(150)  NOT NULL,
    Email         VARCHAR(200)  NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    PasswordHash  VARCHAR(256)  NOT NULL,
    CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE()
);

-- ── 2. Courses ────────────────────────────────────────────────
CREATE TABLE Courses (
    CourseId    INT IDENTITY(1,1) PRIMARY KEY,
    Title       VARCHAR(200)    NOT NULL,
    Description NVARCHAR(1000)  NULL,
    CreatedBy   INT             NOT NULL REFERENCES Users(UserId),
    CreatedAt   DATETIME        NOT NULL DEFAULT GETDATE()
);

-- ── 3. Lessons ────────────────────────────────────────────────
CREATE TABLE Lessons (
    LessonId    INT IDENTITY(1,1) PRIMARY KEY,
    CourseId    INT           NOT NULL
                REFERENCES Courses(CourseId) ON DELETE CASCADE,
    Title       VARCHAR(200)  NOT NULL,
    Content     NVARCHAR(MAX) NULL,
    OrderIndex  INT           NOT NULL DEFAULT 1
);

-- ── 4. Quizzes ────────────────────────────────────────────────
CREATE TABLE Quizzes (
    QuizId    INT IDENTITY(1,1) PRIMARY KEY,
    CourseId  INT          NOT NULL
              REFERENCES Courses(CourseId) ON DELETE CASCADE,
    Title     VARCHAR(200) NOT NULL
);

-- ── 5. Questions ──────────────────────────────────────────────
CREATE TABLE Questions (
    QuestionId   INT IDENTITY(1,1) PRIMARY KEY,
    QuizId       INT           NOT NULL
                 REFERENCES Quizzes(QuizId) ON DELETE CASCADE,
    QuestionText NVARCHAR(500) NOT NULL,
    OptionA      VARCHAR(300)  NOT NULL,
    OptionB      VARCHAR(300)  NOT NULL,
    OptionC      VARCHAR(300)  NOT NULL,
    OptionD      VARCHAR(300)  NOT NULL,
    CorrectAnswer CHAR(1)      NOT NULL
                  CHECK (CorrectAnswer IN ('A','B','C','D'))
);

-- ── 6. Results ────────────────────────────────────────────────
CREATE TABLE Results (
    ResultId    INT IDENTITY(1,1) PRIMARY KEY,
    UserId      INT      NOT NULL REFERENCES Users(UserId),
    QuizId      INT      NOT NULL REFERENCES Quizzes(QuizId),
    Score       INT      NOT NULL,
    AttemptDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- ── Indexes ───────────────────────────────────────────────────
CREATE INDEX IX_Courses_CreatedBy   ON Courses(CreatedBy);
CREATE INDEX IX_Lessons_CourseId    ON Lessons(CourseId);
CREATE INDEX IX_Quizzes_CourseId    ON Quizzes(CourseId);
CREATE INDEX IX_Questions_QuizId    ON Questions(QuizId);
CREATE INDEX IX_Results_UserId      ON Results(UserId);
CREATE INDEX IX_Results_QuizId      ON Results(QuizId);
GO