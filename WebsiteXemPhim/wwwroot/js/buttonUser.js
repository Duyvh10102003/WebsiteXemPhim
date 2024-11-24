$(document).ready(function () {
    // Xử lý sự kiện bấm nút dropdown
    $(".dropdown-toggle").click(function () {
        // Ẩn hoặc hiển thị dropdown menu tùy thuộc vào trạng thái hiện tại của nó
        $(".dropdown-menu").toggle();
    });

    // Đóng dropdown menu khi click bên ngoài
    $(document).on("click", function (event) {
        if (!$(event.target).closest(".dropdown").length) {
            $(".dropdown-menu").hide();
        }
    });
});