window.bootstrapInterop = {
  initDropdowns: function () {
    if (typeof bootstrap === "undefined") return;

    try {
      document.querySelectorAll(".dropdown-toggle").forEach(function (el) {
        new bootstrap.Dropdown(el);
      });
    } catch (e) {
      console.error(JS_CONSTANTS.ERROR_INITIALIZING_DROPDOWNS, e);
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
      console.error(JS_CONSTANTS.ERROR_TOGGLING_DROPDOWN, e);
    }
  },
};

window.gameState = {
  isGameRunning: false,
  pendingNavigation: null,
  modalInstance: null,
};
window.setupGame = function () {
  console.log(JS_CONSTANTS.GAME_PAGE_INITIALIZED);

  if (typeof bootstrap !== "undefined") {
    const modalEl = document.getElementById("navigationWarningModal");
    if (modalEl) {
      try {
        window.gameState.modalInstance = new bootstrap.Modal(modalEl, {
          backdrop: "static",
          keyboard: false,
        });
        console.log(JS_CONSTANTS.MODAL_INITIALIZED_SUCCESSFULLY);
      } catch (e) {
        console.error(JS_CONSTANTS.ERROR_INITIALIZING_MODAL, e);
      }
    }
  } else {
    console.warn(JS_CONSTANTS.BOOTSTRAP_NOT_AVAILABLE);
  }

  return true;
};

window.setBeforeUnloadWarning = function (enable) {
  window.gameState.isGameRunning = enable;

  if (enable) {
    window.addEventListener("beforeunload", window.handleBeforeUnload);
    document.addEventListener("click", window.handleLinkClick, true);
    console.log(JS_CONSTANTS.NAVIGATION_WARNING_ENABLED);
  } else {
    window.removeEventListener("beforeunload", window.handleBeforeUnload);
    document.removeEventListener("click", window.handleLinkClick, true);
    console.log(JS_CONSTANTS.NAVIGATION_WARNING_DISABLED);
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
      JS_CONSTANTS.NAVIGATION_INTERCEPTED,
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
    const message = JS_CONSTANTS.LEAVE_GAME_WARNING;
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
        console.error(JS_CONSTANTS.ERROR_SHOWING_MODAL, e);
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
