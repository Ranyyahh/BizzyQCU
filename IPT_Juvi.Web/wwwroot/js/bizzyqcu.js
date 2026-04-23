(() => {
  const menuToggle = document.querySelector("[data-bizzy-menu-toggle]");
  const menu = document.querySelector("[data-bizzy-menu]");
  if (menuToggle && menu) {
    menuToggle.addEventListener("click", () => {
      const open = menu.getAttribute("data-bizzy-menu-open") === "true";
      menu.setAttribute("data-bizzy-menu-open", open ? "false" : "true");
    });
  }

  document.querySelectorAll("[data-bizzy-back]").forEach((btn) => {
    btn.addEventListener("click", () => history.back());
  });

  const toggleEditBtn = document.querySelector("[data-bizzy-toggle-edit]");
  if (toggleEditBtn) {
    toggleEditBtn.addEventListener("click", () => {
      const fields = document.querySelectorAll("[data-bizzy-editable]");
      const anyReadonly = Array.from(fields).some((f) => f.hasAttribute("readonly"));
      fields.forEach((f) => {
        if (anyReadonly) f.removeAttribute("readonly");
        else f.setAttribute("readonly", "readonly");
      });
    });
  }

  const changePhotoBtn = document.querySelector("[data-bizzy-change-photo]");
  const photoInput = document.querySelector("[data-bizzy-photo-input]");
  const uploadForm = document.querySelector("[data-bizzy-photo-form]");
  const submitMode = document.querySelector("[data-bizzy-submit-mode]");
  if (changePhotoBtn && photoInput) {
    changePhotoBtn.addEventListener("click", () => photoInput.click());
    photoInput.addEventListener("change", () => {
      if (submitMode) submitMode.value = "photo";
      if (uploadForm) uploadForm.submit();
    });
  }
})();
