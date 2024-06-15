if (document.getElementById('searchForm')) {
    document.getElementById('searchForm').addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission
        document.getElementById('searchSection').style.display = 'none'; // Hide the search section
        document.getElementById('loadingSpinner').style.display = 'block'; // Show the loading spinner

        event.target.submit();
    });
}