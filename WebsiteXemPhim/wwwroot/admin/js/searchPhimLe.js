$(document).ready(function () {
    $("#search-input").keyup(function () {
        var query = $(this).val();
        if (query !== '') {
            $.ajax({
                url: "/Admin/PhimLes/SearchSuggestions",
                type: "GET",
                data: { query: query },
                dataType: "json",
                success: function (data) {
                    $("#search-results").empty(); // Xóa gợi ý trước đó
                    $.each(data, function (index, suggestion) {
                        var suggestionParts = suggestion.split("|"); // Phân tách tên phim và đường dẫn ảnh
                        var tenPhim = suggestionParts[0];
                        var anh = suggestionParts[1];
                        // Thêm gợi ý mới với ảnh nhỏ và tên phim
                        $("#search-results").append("<div class='suggestion'>" +
                            "<img src='" + anh + "' alt='" + tenPhim + "' class='suggestion-img' />" +
                            "<span>" + tenPhim + "</span></div>");
                    });
                    $(".dropdown-content").css("display", "block"); // Hiển thị danh sách gợi ý
                },
                error: function (xhr, status, error) {
                    console.log("Lỗi:", error);
                }
            });
        } else {
            $("#search-results").empty(); // Xóa gợi ý nếu ô tìm kiếm trống
            $(".dropdown-content").css("display", "none"); // Ẩn danh sách gợi ý nếu ô trống
        }
    });
    // Xử lý khi người dùng click vào một gợi ý
    $(document).on("click", ".suggestion", function () {
        var selectedSuggestion = $(this).text();
        $("#search-input").val(selectedSuggestion); // Đặt giá trị ô tìm kiếm là gợi ý được chọn
        $("#search-results").empty(); // Xóa các gợi ý
        $(".dropdown-content").css("display", "none"); // Ẩn danh sách gợi ý
    });
    // Đóng danh sách gợi ý nếu người dùng click bên ngoài
    $(document).click(function (event) {
        if (!$(event.target).closest('.dropdown').length) {
            $(".dropdown-content").css("display", "none");
        }
    });
});

$(document).ready(function () {
    $("#search-form").submit(function (event) {
        // Ngăn chặn hành động mặc định của form
        event.preventDefault();
        // Lấy giá trị tìm kiếm từ ô nhập
        var query = $("#search-input").val();
        // Gọi hàm SearchPhims thông qua AJAX
        $.ajax({
            url: "/Admin/PhimLes/SearchPhims",
            type: "GET",
            data: { query: query },
            success: function (data) {
                // Xử lý phản hồi, ví dụ, cập nhật một div với kết quả tìm kiếm
                $("#phims").html(data);
            },
            error: function (xhr, status, error) {
                console.error("Lỗi:", error);
            }
        });
    });
});
