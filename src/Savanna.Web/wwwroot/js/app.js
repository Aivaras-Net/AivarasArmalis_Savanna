window.bootstrapInterop = {
  initDropdowns: function () {
    // Check if bootstrap is available
    if (typeof bootstrap === "undefined") {
      console.error(
        "Bootstrap is not defined. Make sure bootstrap.bundle.min.js is loaded correctly."
      );
      return;
    }

    try {
      var dropdownElementList = [].slice.call(
        document.querySelectorAll(".dropdown-toggle")
      );
      dropdownElementList.forEach(function (dropdownToggleEl) {
        new bootstrap.Dropdown(dropdownToggleEl);
      });
    } catch (e) {
      console.error("Error initializing dropdowns:", e);
    }
  },

  toggleDropdown: function (id) {
    // Check if bootstrap is available
    if (typeof bootstrap === "undefined") {
      console.error(
        "Bootstrap is not defined. Make sure bootstrap.bundle.min.js is loaded correctly."
      );
      return;
    }

    try {
      var dropdown = document.getElementById(id);
      if (dropdown) {
        var instance = bootstrap.Dropdown.getInstance(dropdown);
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
