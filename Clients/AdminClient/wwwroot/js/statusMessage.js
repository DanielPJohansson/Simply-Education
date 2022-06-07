"use strict";

const closeBtn = document.querySelector("#close");
const message = document.querySelector("#message");

closeBtn.addEventListener("click", () => {
  message.classList.add("hide");
});
