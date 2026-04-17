let score = localStorage.getItem("quizScore");
let grade = localStorage.getItem("quizGrade");
let status = localStorage.getItem("quizStatus");
let completedCourses = JSON.parse(localStorage.getItem("completedCourses")) || [];

if (score) {
    document.getElementById("score").textContent = score + " / 5";
} else {
    document.getElementById("score").textContent = "Quiz not attempted";
}

if (document.getElementById("grade")) {
    document.getElementById("grade").textContent = grade ? grade : "Not available";
}

if (document.getElementById("status")) {
    document.getElementById("status").textContent = status ? status : "Not available";
}

let completedList = document.getElementById("completedCoursesList");

if (completedList) {
    if (completedCourses.length > 0) {
        completedList.innerHTML = "";

        completedCourses.forEach(function(course) {
            let li = document.createElement("li");
            li.textContent = course;
            completedList.appendChild(li);
        });
    } else {
        completedList.innerHTML = "<li>No courses completed yet</li>";
    }
}

let progressSection = document.getElementById("progressSection");

if (progressSection) {
    let totalCourses = 3;
    let completedCount = completedCourses.length;
    let overallProgress = (completedCount / totalCourses) * 100;

    progressSection.innerHTML =
        "<p><strong>Overall Course Completion</strong></p>" +
        "<progress value='" + overallProgress + "' max='100'></progress>" +
        "<p>" + overallProgress.toFixed(0) + "% Completed</p>";
}