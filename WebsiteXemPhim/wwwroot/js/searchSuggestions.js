$(document).ready(function () {
    // Hàm cắt ngắn tên phim nếu quá 40 ký tự
    function truncateString(str, num) {
        if (str.length <= num) {
            return str;
        }
        return str.slice(0, num) + '...';
    }

    $("#search-input").keyup(function () {
        var query = $(this).val();
        if (query !== '') {
            $.ajax({
                url: "/Home/SearchSuggestions",
                type: "GET",
                data: { query: query },
                dataType: "json",
                success: function (data) {
                    $("#search-results").empty(); // Xóa gợi ý trước đó
                    $.each(data, function (index, suggestion) {
                        var suggestionParts = suggestion.split("|"); // Phân tách tên phim và đường dẫn ảnh
                        var tenPhim = suggestionParts[0];
                        var anh = suggestionParts[1];
                        // Sử dụng hàm truncateString để cắt ngắn tên phim nếu quá dài
                        var tenPhimDisplay = truncateString(tenPhim, 40);
                        // Thêm gợi ý mới với ảnh nhỏ và tên phim
                        $("#search-results").append(
                            "<div class='suggestion' data-fullname='" + tenPhim + "'>" +
                            "<div class='suggestion-img-wrapper'>" +
                            "<img src='" + anh + "' alt='" + tenPhim + "' class='suggestion-img' />" +
                            "</div>" +
                            "<div class='suggestion-text-wrapper'>" +
                            "<span class='suggestion-text'>" + tenPhimDisplay + "</span>" +
                            "</div>" +
                            "</div>"
                        );
                    });

                    $(".dropdown-content").css("display", "block"); // Hiển thị dropdown
                },
                error: function (xhr, status, error) {
                    console.log("Error:", error);
                }
            });
        } else {
            $("#search-results").empty(); // Xóa gợi ý nếu truy vấn trống
            $(".dropdown-content").css("display", "none"); // Ẩn dropdown nếu truy vấn trống
        }
    });

    // Xử lý khi click vào gợi ý
    $(document).on("click", ".suggestion", function () {
        var selectedSuggestion = $(this).data("fullname"); // Lấy tên đầy đủ từ data-fullname
        $("#search-input").val(selectedSuggestion); // Set giá trị của ô tìm kiếm thành tên đầy đủ của phim
        $("#search-results").empty(); // Xóa gợi ý
        $(".dropdown-content").css("display", "none"); // Ẩn dropdown
    });

    // Đóng dropdown khi click bên ngoài
    $(document).click(function (event) {
        if (!$(event.target).closest('.dropdown').length && !$(event.target).is("#search-input")) {
            $(".dropdown-content").css("display", "none");
        }
    });
});
