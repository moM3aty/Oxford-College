
document.addEventListener("DOMContentLoaded", function () {

    // Back to Top Button Logic
    let backToTopButton = document.querySelector(".back-to-top");

    if (backToTopButton) {
        window.onscroll = function () {
            // Show the button when scrolling down 200px
            if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
                backToTopButton.style.display = "flex";
            } else {
                backToTopButton.style.display = "none";
            }
        };

        backToTopButton.addEventListener("click", function () {
            // Smooth scroll to top
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

});