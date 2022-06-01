"use strict";

const gallery = document.querySelector(".gallery-wrapper");
const selectList = document.querySelector("#categories-list");
const baseUrl = "https://localhost:7210/api/v1";
const filterBtn = document.querySelector("#filter-btn");
let courses;

async function getCourses() {
  let url = `${baseUrl}/courses/list`;

  let response = await fetch(url);

  if (!response.ok) {
    console.log("Could not fetch courses.");
  }

  courses = await response.json();

  gallery.innerHTML = "";
  courses.forEach((course) => {
    createGalleryCardHtml(course);
  });
}

async function getCategories() {
  let url = `${baseUrl}/categories/list`;
  let response = await fetch(url);
  if (!response.ok) {
    console.log("Could not fetch categories.");
  }

  let categories = await response.json();

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
  }

  courses.forEach((course) => {
    if (course.category.toLowerCase() === categoryName.toLowerCase()) {
      createGalleryCardHtml(course);
    }
  });
}

function createGalleryCardHtml(course) {
  gallery.insertAdjacentHTML(
    "beforeend",
    `<a class="gallery-card" href="details.html?courseId=${course.courseId}">
    <img src="${course.imageUrl}" alt="Course image">
    <div class="gallery-card-content">
    <h3>${course.name}</h3>
    <h4><span>Course code: ${course.courseCode}</span><span>${course.durationInHours} hours</span></h4>
    <p>${course.description}</p>
    </div>
    </a>`
  );
}

function createSelectListHtml(category) {
  selectList.insertAdjacentHTML(
    "beforeend",
    `<option value="${category.name}">${category.name}</option>`
  );
}

filterBtn.addEventListener("click", () => {
  let categoryName = selectList.value;
  filterByCategory(categoryName);
});

getCategories();
getCourses();
