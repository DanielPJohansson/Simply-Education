"use strict";

const image = document.querySelector("#course-image");
const courseCode = document.querySelector("#course-code");
const name = document.querySelector("#course-name");
const duration = document.querySelector("#course-duration");
const category = document.querySelector("#course-category");
const description = document.querySelector("#course-description");
const details = document.querySelector("#course-details");

const baseUrl = "https://localhost:7210/api/v1/courses";

function onPageLoad() {
  const urlParams = new URLSearchParams(location.search);
  let courseId;

  urlParams.forEach((value, key) => {
    if (key === "courseId") {
      courseId = value;
    }
  });

  getCourse(courseId);
}

async function getCourse(courseId) {
  const url = `${baseUrl}/${courseId}`;

  const response = await fetch(url);
  console.log(response);

  if (response.ok) {
    const responseModel = await response.json();
    const course = JSON.parse(responseModel.data);
    createHtml(course);
  } else {
    console.log(`Error when getting course with id: ${courseId}`);
  }
}

function createHtml(course) {
  image.setAttribute("src", course.ImageUrl);
  name.innerHTML = course.Name;
  courseCode.innerHTML = course.CourseCode;
  duration.innerHTML = course.DurationInHours;
  category.innerHTML = course.Category;
  description.innerHTML = course.Description;
  details.innerHTML = course.Details;
}

onPageLoad();
