window.bootstrapInterop = {
  initDropdowns: function () {
    if (typeof bootstrap === "undefined") return;

    try {
      document.querySelectorAll(".dropdown-toggle").forEach(function (el) {
        new bootstrap.Dropdown(el);
      });
    } catch (e) {
      console.error("Error initializing dropdowns:", e);
    }
  },

  toggleDropdown: function (id) {
    if (typeof bootstrap === "undefined") return;

    try {
      const dropdown = document.getElementById(id);
      if (dropdown) {
        const instance = bootstrap.Dropdown.getInstance(dropdown);
        if (instance) {
          instance.toggle();
        } else {
          new bootstrap.Dropdown(dropdown).toggle();
        }
      }
    } catch (e) {
      console.error("Error toggling dropdown:", e);
    }
  },
};

window.gameState = {
  isGameRunning: false,
  pendingNavigation: null,
  modalInstance: null,
};
window.setupGame = function () {
  console.log("Game page initialized");

  if (typeof bootstrap !== "undefined") {
    const modalEl = document.getElementById("navigationWarningModal");
    if (modalEl) {
      try {
        window.gameState.modalInstance = new bootstrap.Modal(modalEl, {
          backdrop: "static",
          keyboard: false,
        });
        console.log("Modal initialized successfully");
      } catch (e) {
        console.error("Error initializing modal:", e);
      }
    }
  } else {
    console.warn("Bootstrap not available, modal functionality may be limited");
  }

  return true;
};

window.setBeforeUnloadWarning = function (enable) {
  window.gameState.isGameRunning = enable;

  if (enable) {
    window.addEventListener("beforeunload", window.handleBeforeUnload);
    document.addEventListener("click", window.handleLinkClick, true);
    console.log("Navigation warning enabled");
  } else {
    window.removeEventListener("beforeunload", window.handleBeforeUnload);
    document.removeEventListener("click", window.handleLinkClick, true);
    console.log("Navigation warning disabled");
  }
};

window.handleLinkClick = function (event) {
  if (!window.gameState.isGameRunning) return;

  let element = event.target;
  let href = null;

  while (element && element !== document) {
    if (element.tagName && element.tagName.toLowerCase() === "a") {
      href = element.getAttribute("href");
      break;
    }
    element = element.parentNode;
  }

  if (href && !href.startsWith("#") && !href.endsWith("/game")) {
    if (href.startsWith("/")) {
      window.gameState.pendingNavigation = window.location.origin + href;
    } else if (!href.startsWith("http")) {
      window.gameState.pendingNavigation = window.location.origin + "/" + href;
    } else {
      window.gameState.pendingNavigation = href;
    }

    console.log(
      "Navigation intercepted to:",
      window.gameState.pendingNavigation
    );

    event.preventDefault();
    event.stopPropagation();
    window.showNavigationWarningModal();
    return false;
  }
};

window.handleBeforeUnload = function (event) {
  if (window.gameState.isGameRunning) {
    const message =
      "You will lose your game progress if you leave this page. Are you sure you want to leave?";
    event.returnValue = message;
    return message;
  }
};
window.showNavigationWarningModal = function () {
  if (window.gameState.modalInstance) {
    window.gameState.modalInstance.show();
  } else if (typeof bootstrap !== "undefined") {
    const modalEl = document.getElementById("navigationWarningModal");
    if (modalEl) {
      try {
        const modal = new bootstrap.Modal(modalEl);
        modal.show();
      } catch (e) {
        console.error("Error showing modal:", e);
      }
    }
  }
};
window.hideNavigationWarningModal = function () {
  if (window.gameState.modalInstance) {
    window.gameState.modalInstance.hide();
  } else if (typeof bootstrap !== "undefined") {
    const modalEl = document.getElementById("navigationWarningModal");
    if (modalEl) {
      const modal = bootstrap.Modal.getInstance(modalEl);
      if (modal) modal.hide();
    }
  }
};
window.getAndClearPendingNavigation = function () {
  const url = window.gameState.pendingNavigation;
  window.gameState.pendingNavigation = null;
  return url || "/";
};
