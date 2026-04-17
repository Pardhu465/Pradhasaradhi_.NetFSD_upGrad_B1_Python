const quizContainer = document.getElementById("quizContainer");
const resultDiv = document.getElementById("result");
const messageDiv = document.getElementById("quizMessage");


// Simulate loading quiz using Promise + setTimeout
function loadQuizData() {
    return new Promise((resolve) => {
        setTimeout(function () {
            resolve(quizQuestions);
        }, 2000);
    });
}


// Async function to display quiz
async function displayQuiz() {
    let questions = await loadQuizData();

    messageDiv.style.display = "none";
    quizContainer.innerHTML = "";

    questions.forEach(function (q, index) {
        let questionBlock = document.createElement("div");
        questionBlock.classList.add("question-block");

        let title = document.createElement("h4");
        title.textContent = (index + 1) + ". " + q.question;
        questionBlock.appendChild(title);

        q.options.forEach(function (option, optIndex) {
            let label = document.createElement("label");
            label.classList.add("option-label");

            let radio = document.createElement("input");
            radio.type = "radio";
            radio.name = "question" + index;
            radio.value = optIndex;

            // onchange event
            radio.onchange = function () {
                console.log("Answer selected for question " + (index + 1));
            };

            label.appendChild(radio);
            label.append(" " + option);

            questionBlock.appendChild(label);
            questionBlock.appendChild(document.createElement("br"));
        });

        quizContainer.appendChild(questionBlock);
    });
}


// Load quiz when page opens
displayQuiz();


// Calculate percentage
function calculatePercentage(score, total) {
    return (score / total) * 100;
}


// Determine pass or fail
function checkPass(score, total) {
    let percent = calculatePercentage(score, total);

    if (percent >= 50) {
        return true;
    } else {
        return false;
    }
}


// Grade calculation using if-else
function calculateGrade(percent) {
    if (percent >= 80) {
        return "A";
    } else if (percent >= 60) {
        return "B";
    } else if (percent >= 50) {
        return "C";
    } else {
        return "F";
    }
}


// Feedback using switch statement
function performanceMessage(grade) {
    let message = "";

    switch (grade) {
        case "A":
            message = "Excellent performance!";
            break;

        case "B":
            message = "Very good work!";
            break;

        case "C":
            message = "You passed, but keep practicing.";
            break;

        case "F":
            message = "You need more practice.";
            break;

        default:
            message = "Result unavailable";
    }

    return message;
}


// Quiz submission event
function submitQuiz() {
    let score = 0;
    let skippedQuestions = 0;

    quizQuestions.forEach(function (q, index) {
        let selected = document.querySelector('input[name="question' + index + '"]:checked');

        if (!selected) {
            skippedQuestions++;
            return;
        }

        if (Number(selected.value) === q.answer) {
            score++;
        }
    });

    // validation for skipped questions
    if (skippedQuestions > 0) {
        resultDiv.innerHTML =
            "<div class='result-card error-card'>" +
            "<h3>Incomplete Submission</h3>" +
            "<p>Please answer all questions before submitting.</p>" +
            "<p>You skipped " + skippedQuestions + " question(s).</p>" +
            "</div>";

        resultDiv.style.display = "block";
        return;
    }

    let percent = calculatePercentage(score, quizQuestions.length);
    let grade = calculateGrade(percent);
    let pass = checkPass(score, quizQuestions.length);
    let feedback = performanceMessage(grade);

    // store result in localStorage
    localStorage.setItem("quizScore", score);
    localStorage.setItem("quizPercentage", percent.toFixed(2));
    localStorage.setItem("quizGrade", grade);
    localStorage.setItem("quizFeedback", feedback);
    localStorage.setItem("quizStatus", pass ? "Pass" : "Fail");

    // store completed courses
    let completedCourses = JSON.parse(localStorage.getItem("completedCourses")) || [];

    if (pass) {
        if (!completedCourses.includes("JavaScript Essentials")) {
            completedCourses.push("JavaScript Essentials");
            localStorage.setItem("completedCourses", JSON.stringify(completedCourses));
        }
    }

    // show result
    resultDiv.innerHTML =
        "<div class='result-card'>" +
        "<h3>Quiz Result</h3>" +
        "<p><strong>Your Score:</strong> " + score + " / " + quizQuestions.length + "</p>" +
        "<p><strong>Percentage:</strong> " + percent.toFixed(2) + "%</p>" +
        "<p><strong>Grade:</strong> " + grade + "</p>" +
        "<p><strong>Status:</strong> " + (pass ? "Pass" : "Fail") + "</p>" +
        "<p><strong>Feedback:</strong> " + feedback + "</p>" +
        "</div>";

    resultDiv.style.display = "block";
}