let selectedRating = 0;

// Hàm cập nhật giao diện sao khi chọn
function rateMovie(stars) {
    document.getElementById("star-rating").value = stars; // Lưu số sao đã chọn
    document.querySelectorAll('.ratingStar').forEach((star, index) => {
        star.classList.toggle('fa', index < stars); // Đánh dấu sao được chọn
        star.classList.toggle('far', index >= stars); // Đánh dấu sao chưa chọn
    });
    selectedRating = stars;
}

// Gửi đánh giá lên server
function submitRating(movieId, ratingType) {
    const starRating = selectedRating;
    const data = {
        PhimBoId: ratingType === 'phimbo' ? movieId : null,
        PhimLeId: ratingType === 'phimle' ? movieId : null,
        Star: starRating
    };

    fetch('/api/DanhGia/AddRating', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Kiểm tra nội dung thông báo từ server
                if (data.message === "Đã thêm đánh giá mới!") {
                    alert("Cảm ơn bạn đã thêm đánh giá!");
                } else if (data.message === "Đã cập nhật đánh giá!") {
                    alert("Đánh giá của bạn đã được cập nhật!");
                } else {
                    alert("Đánh giá của bạn đã được ghi nhận!");
                }
            } else {
                alert(data.message);
            }
        })
        .catch(error => alert('Có lỗi xảy ra!'));
}

$(document).ready(function () {
    // Sự kiện hover để làm nổi bật sao khi di chuột
    $(".ratingStar").hover(function () {
        const hoverValue = $(this).data("value");
        rateMovie(hoverValue);
    }, function () {
        rateMovie(selectedRating); // Khi rời chuột khỏi sao, trả về trạng thái đã chọn
    });

    // Sự kiện click để chọn số sao
    $(".ratingStar").click(function () {
        const stars = $(this).data("value");
        rateMovie(stars); // Cập nhật số sao khi click

        // Lấy movieId và ratingType từ phần tử cha
        const movieId = $(this).closest(".rating").data("movie-id");
        const ratingType = $(this).closest(".rating").data("rating-type");

        // Gọi hàm submitRating với movieId và ratingType thực tế
        submitRating(movieId, ratingType);
    });

});
