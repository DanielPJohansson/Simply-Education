"use strict";

const gallery = document.querySelector(".gallery-wrapper");
const selectList = document.querySelector("#categories-list");
const baseUrl = "https://localhost:7210/api/v1/courses";
const filterBtn = document.querySelector("#filter-btn");
let courses;

async function getCourses() {
  let url = `${baseUrl}/list`;

  let response = await fetch(url);

  if (!response.ok) {
    console.log("Could not fetch courses.");
  }

  const responseModel = await response.json();
  courses = JSON.parse(responseModel.data);

  gallery.innerHTML = "";
  courses.forEach((course) => {
    createGalleryCardHtml(course);
  });
}

async function getCategories() {
  let url = `${baseUrl}/categories`;
  let response = await fetch(url);
  if (!response.ok) {
    console.log("Could not fetch categories.");
  }

  const responseModel = await response.json();
  const categories = JSON.parse(responseModel.data);

  categories.forEach((category) => {
    createSelectListHtml(category);
  });
}

function filterByCategory(categoryName) {
  gallery.innerHTML = "";
  if (categoryName === "all") {
    courses.forEach((course) => {
      createGalleryCardHtml(course);
    });
  } else {
    courses.forEach((course) => {
      if (course.Category.toLowerCase() === categoryName.toLowerCase()) {
        createGalleryCardHtml(course);
      }
    });
  }
}

function createGalleryCardHtml(course) {
  gallery.insertAdjacentHTML(
    "beforeend",
    `<a class="gallery-card" href="details.html?courseId=${course.CourseId}">
    <img src="${course.ImageUrl}" alt="Course image">
    <div class="gallery-card-content">
    <h3>${course.Name}</h3>
    <h4><span>Course code: ${course.CourseCode}</span><span>${course.DurationInHours} hours</span></h4>
    <p>${course.Description}</p>
    </div>
    </a>`
  );
}

function createSelectListHtml(category) {
  selectList.insertAdjacentHTML(
    "beforeend",
    `<option value="${category.Name}">${category.Name}</option>`
  );
}

filterBtn.addEventListener("click", () => {
  let categoryName = selectList.value;
  filterByCategory(categoryName);
});

getCategories();
getCourses();
