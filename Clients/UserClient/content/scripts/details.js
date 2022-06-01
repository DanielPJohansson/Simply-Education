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

  console.log(courseId);

  const course = getCourse(courseId);
}

async function getCourse(courseId) {
  const url = `${baseUrl}/${courseId}`;

  const response = await fetch(url);

  if (!response.ok) {
    console.log(`Error when getting course with id: ${courseId}`);
  }

  const course = await response.json();
  createHtml(course);
}

function createHtml(course) {
  image.setAttribute("src", course.imageUrl);
  name.innerHTML = course.name;
  courseCode.innerHTML = course.courseCode;
  duration.innerHTML = course.durationInHours;
  category.innerHTML = course.category;
  description.innerHTML = course.description;
  details.innerHTML = course.details;
}

onPageLoad();
