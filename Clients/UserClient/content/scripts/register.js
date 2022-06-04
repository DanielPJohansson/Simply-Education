"use strict";

const emailInput = document.querySelector("#email");
const passwordInput = document.querySelector("#password");
const firstNameInput = document.querySelector("#first-name");
const lastNameInput = document.querySelector("#last-name");
const phoneNumberInput = document.querySelector("#phone-number");
const form = document.querySelector("#register-form");
const emailError = document.querySelector("#email-error");
const passwordError = document.querySelector("#password-error");

const baseUrl = "https://localhost:7210/api/v1";

async function register(event) {
  event.preventDefault();
  let url = `${baseUrl}/authentication/register`;

  let user = {
    email: emailInput.value,
    password: passwordInput.value,
  };

  const response = await fetch(url, {
    method: "Post",
    headers: { "Content-type": "application/json" },
    body: JSON.stringify(user),
  });

  const result = await response.json();

  if (response.ok) {
    localStorage.setItem("token", JSON.stringify(result.token));
    window.location = "index.html";
  } else {
    emailError.innerHTML = "";
    passwordError.innerHTML = "";

    let errors = result.errors;

    for (const error in errors) {
      if (error === "Email") {
        createErrorHtml(emailError, errors[error][0]);
      }

      if (error === "DuplicateUserName") {
        createErrorHtml(emailError, errors[error]);
      }

      if (error.includes("Password")) {
        createErrorHtml(passwordError, errors[error]);
      }
    }
  }
}

function createErrorHtml(input, error) {
  input.insertAdjacentHTML("beforeend", `<p>${error}</p>`);
}

form.addEventListener("submit", register);
