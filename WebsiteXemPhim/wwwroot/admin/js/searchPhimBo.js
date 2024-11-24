$(document).ready(function () {
    $("#search-input").keyup(function () {
        var query = $(this).val();
        if (query !== '') {
            $.ajax({
                url: "/Admin/PhimBoes/SearchSuggestions",
                type: "GET",
                data: { query: query },
                dataType: "json",
                success: function (data) {
                    $("#search-results").empty(); // Clear previous suggestions
                    $.each(data, function (index, suggestion) {
                        var suggestionParts = suggestion.split("|"); // Phân tách tên phim và đường dẫn ảnh
                        var tenPhim = suggestionParts[0];
                        var anh = suggestionParts[1];
                        // Thêm gợi ý mới với ảnh nhỏ và tên phim
                        $("#search-results").append("<div class='suggestion'>" +
                            "<img src='" + anh + "' alt='" + tenPhim + "' class='suggestion-img' />" +
                            "<span>" + tenPhim + "</span></div>");
                    });
                    $(".dropdown-content").css("display", "block"); // Show dropdown
                },
                error: function (xhr, status, error) {
                    console.log("Error:", error);
                }
            });
        } else {
            $("#search-results").empty(); // Clear suggestions if query is empty
            $(".dropdown-content").css("display", "none"); // Hide dropdown if input is empty
        }
    });
    // Handle click on suggestion
    $(document).on("click", ".suggestion", function () {
        var selectedSuggestion = $(this).text();
        $("#search-input").val(selectedSuggestion); // Set search input value to selected suggestion
        $("#search-results").empty(); // Clear suggestions
        $(".dropdown-content").css("display", "none"); // Hide dropdown
    });
    // Close dropdown if user clicks outside of it
    $(document).click(function (event) {
        if (!$(event.target).closest('.dropdown').length) {
            $(".dropdown-content").css("display", "none");
        }
    });
});
$(document).ready(function () {
    $("#search-form").submit(function (event) {
        // Prevent default form submission behavior
        event.preventDefault();
        // Get the search query from the input field
        var query = $("#search-input").val();
        // Call the SearchProducts action via AJAX
        $.ajax({
            url: "/Admin/PhimBoes/SearchPhims",
            type: "GET",
            data: { query: query },
            success: function (data) {
                // Handle the response, for example, update a div with the search results
                $("#phims").html(data);
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    });
});