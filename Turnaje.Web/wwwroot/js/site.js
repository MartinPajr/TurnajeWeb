// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll('.nav-tabs .nav-link').forEach(tab => {
    tab.addEventListener('click', function (event) {
        event.preventDefault();

        // Deactivate all tabs
        document.querySelectorAll('.nav-tabs .nav-link').forEach(navLink => {
            navLink.classList.remove('active');
        });

        // Hide all tab contents
        document.querySelectorAll('.tab-content').forEach(content => {
            content.style.display = 'none';
        });

        // Activate this tab
        this.classList.add('active');

        // Show content for this tab
        const contentId = this.getAttribute('href').substring(1);
        document.getElementById(contentId).style.display = 'block';
    });
});