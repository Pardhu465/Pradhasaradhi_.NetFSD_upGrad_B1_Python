const courseTableBody = document.getElementById("courseTableBody");

function renderCourses() {
  let tableRows = "";

  courses.forEach(function(course) {
    let lessonItems = "";

    course.lessons.forEach(function(lesson) {
      lessonItems += "<li>" + lesson + "</li>";
    });

    let statusText = "";

    if (course.completed) {
      statusText = "Completed";
    } else {
      statusText = "In Progress";
    }

    tableRows += `
      <tr>
        <td>${course.name}</td>
        <td><ol>${lessonItems}</ol></td>
        <td>${course.duration}</td>
        <td>${statusText}</td>
      </tr>
    `;
  });

  courseTableBody.innerHTML = tableRows;
}

renderCourses();