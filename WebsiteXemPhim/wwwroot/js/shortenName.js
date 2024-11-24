$(document).ready(function () {
    // Hàm cắt ngắn tên phim nếu quá 53 ký tự
    function truncateString(str, num) {
        if (str.length <= num) {
            return str;
        }
        return str.slice(0, num) + '...';
    }

    // Lặp qua tất cả phim trong phần gợi ý và cắt ngắn tên phim
    $(".anime__sidebar__item__text h5 a").each(function () {
        var tenPhim = $(this).contents().text().trim();
        // Cắt ngắn tên phim nếu cần
        var tenPhimDisplay = truncateString(tenPhim, 50);
        // Cập nhật lại tên phim hiển thị
        $(this).text(tenPhimDisplay);
    });

    // Lặp qua tất cả phim trong phần gợi ý và cắt ngắn tên phim
    $(".anime__details__title h3").each(function () {
        var tenPhim = $(this).contents().text().trim();
        // Cắt ngắn tên phim nếu cần
        var tenPhimDisplay = truncateString(tenPhim, 42);
        // Cập nhật lại tên phim hiển thị
        $(this).text(tenPhimDisplay);
    });
});
